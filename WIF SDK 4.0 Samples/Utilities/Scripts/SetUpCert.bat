@echo off
set OS_VERSION=
set OS_MAJOR=
set OS_MINOR=
set OS_BUILD=
set IsVistaOrHigher=
set IsWin7OrHigher=
set IsWin2K3=

for /f "skip=1" %%i in ( 'wmic os get version' ) do ( 
    set OS_VERSION=%%i 
    goto:__ver_done
)
:__ver_done

for /f "delims=. tokens=1,2,3" %%i in ("%OS_VERSION%") do ( 
    set OS_MAJOR=%%i&set OS_MINOR=%%j&set OS_BUILD=%%k  
    goto :__ver_split_done
)
:__ver_split_done

if "%OS_MAJOR%" GEQ "6" (
    set IsVistaOrHigher=true
    if "%OS_MINOR%" == "1" (
        set IsWin7OrHigher=true
        goto :__ver_set_done
    )
    if "%OS_MAJOR%" GTR "6" (
        set IsWin7OrHigher=true
        goto :__ver_set_done
    )
)

if "%OS_MAJOR%" == "5" (
    if "%OS_MINOR%" == "2" (
        set IsWin2K3=true
    )
    goto :__ver_set_done
)

:__ver_set_done

@if "%1"=="" (
   @echo usage: %~n0 {certificate's subject name} {extra store name} [store location]
   @echo Example: %~n0 localhost root
   @echo Example: %~n0 localhost trustedpeople
   @echo Example: %~n0 bobclient trustedpeople CurrentUser
   @goto :end
)

set SUBJECT=
for /f "tokens=* usebackq" %%i in (`certutil -v -store my ^| findstr /e /c:%1`) do (
    set SUBJECT=%%i
    goto :foundsubject
)

:foundsubject

set CERTHASH=

call "%~dp0SetCertHashFromSubjectName.bat" %1

for %%j in (%SUBJECT%) do (
    if %%j==%1 (
        echo.
        echo WARNING: A certificate with the subject %SUBJECT% is already present. 
        echo The sample might fail if there are two certificates with the same subject.
        echo Do you want to install the certificate anyway?
        choice
        echo.
        if ERRORLEVEL 2 goto :end
    )
)

@if NOT "%CERTHASH%" =="" (
  goto :doneHash
)

set WIFMAKECERT="%~dp0WifMakeCert.exe"

set CERTSTORE_LOCATION=Machine
if "%3" NEQ "" (
  set CERTSTORE_LOCATION=User
)

%WIFMAKECERT% %CERTSTORE_LOCATION% "%1" "%~dp0%1.cer" 2>NUL

if "%2" == "root" (
 echo.
 echo WARNING: A TEST CERTIFICATE WITH SUBJECT NAME "%1" HAS BEEN ADDED TO THE
 echo TRUSTED ROOT CERTIFICATION AUTHORITY STORE. FOR SECURITY REASONS, 
 echo PLEASE REMEMBER TO REMOVE THAT CERTIFICATE AFTER YOU FINISH RUNNING THE
 echo SAMPLES BY RUNNING THE CLEANUP SCRIPT.
 echo. 
) 

if "%2" == "trustedpeople" (
 echo.
 echo WARNING: A TEST CERTIFICATE WITH SUBJECT NAME "%1" HAS BEEN ADDED TO THE TRUSTED
 echo PEOPLE STORE. FOR SECURITY REASONS, PLEASE REMEMBER TO REMOVE THAT CERTIFICATE 
 echo AFTER YOU FINISH RUNNING THE SAMPLES BY RUNNING THE CLEANUP SCRIPT.
 echo. 
)

set CERTUTIL_OPTION=
if "%3" == "CurrentUser" (
  set CERTUTIL_OPTION="-user"
)

certutil -f %CERTUTIL_OPTION% -addstore %2 "%~dp0%1.cer"

call "%~dp0SetCertHashFromFile.bat" %1

@if "%CERTHASH%" =="" (
   @echo usage: failed to generate cert hash
   @goto :end
)

:doneHash

if "%3" == "CurrentUser" (
   @goto :end
)

set KEYCONTAINER=

set WIFCERTUTIL="%~dp0WifCertUtil.exe"

for /f %%a in ( '%WIFCERTUTIL% GetCertKeyContainer my %CERTHASH%' ) do ( set KEYCONTAINER=%%a )

set COMMON_APPDATA_PATH=
set WIN2k3_COMMON_APPDATAJS="%~dp0Win2k3.js"

if "%IsWin2k3%" == "true" (
   for /f "tokens=*" %%a in ( 'cscript //nologo //e:JScript %WIN2k3_COMMON_APPDATAJS%' ) do  set COMMON_APPDATA_PATH=%%a   
) else (
   set COMMON_APPDATA_PATH=%ALLUSERSPROFILE%
)

pushd "%COMMON_APPDATA_PATH%\Microsoft\Crypto\RSA\MachineKeys"
icacls %KEYCONTAINER% /grant *S-1-5-20:R

@if "%IsWin7OrHigher%" == "true" (
    icacls %KEYCONTAINER% /grant "IIS_IUSRS":R
)

popd
:end
