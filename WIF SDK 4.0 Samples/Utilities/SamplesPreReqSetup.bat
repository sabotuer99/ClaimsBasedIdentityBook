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


echo.
echo This script will perform the following steps:
echo.
echo STEP 1: Run SamplesPreReqCleanup.bat before the set up. (NOTE: SamplesPreReqCleanup.bat
echo relies on httpcfg/netsh)
echo. 
echo STEP 2: It will try to see if there are certificates with subject name localhost and
echo STS already installed in the Personal store in LocalMachine. If so, you will have a
echo choice to use the existing certificate or create a new one. Otherwise, it will use 
echo wifmakecert.exe to generate these certificates and install it in the Personal store.
echo The localhost certificate will be installed in Trusted Root Certification Authority store
echo in LocalMachine and the STS certificate will be installed in the Trusted People
echo store in LocalMachine. Regardless of whether the script creates the certificate or finds a 
echo pre-existing one, it will give NETWORK SERVICE read permission to the private 
echo key file of the certificate. 
echo.
echo STEP 3: It will use netsh or httpcfg to set up SSL. (NOTE: This will clear the 
echo 0.0.0.0:443 http.sys configuration. ). It also sets up SSL at port 8082 using
echo the STS Certificate.
echo.
echo STEP 4: Restart the Internet Information Service (IIS).
echo.

pause

echo -------------------------------
echo --- STEP 1: Run SamplesPreReqCleanup.bat ---
echo -------------------------------

echo ***********Start calling SamplesPreReqCleanup.bat**************
call "%~dp0SamplesPreReqCleanup.bat"
echo ***********End calling SamplesPreReqCleanup.bat**************

echo --------------------------------------------------------------------------
echo --- STEP 2: Create localhost and STS certificates if they do not exist ---
echo ---- and give permission to the private key file of the certificates -----
echo --------------------------------------------------------------------------

call "%~dp0Scripts\SetUpCert.bat" localhost root
call "%~dp0Scripts\SetUpCert.bat" STS trustedpeople

echo ------------------------------------------------------------------
echo --- STEP 3: Set up SSL ---
echo --------------------------

@if "%IsVistaOrHigher%" == "true" (
   echo Running on vista and higher
   REM on Vista and higher, we use netsh instead of httpcfg.exe
   REM Setup SSL at port 443 using localhost certificate.
   netsh http add sslcert ipport=0.0.0.0:443 appid={00000000-0000-0000-0000-000000000000} certhash=%localhostCERTHASH% clientcertnegotiation=enable

   REM Setup SSL at port 8082 using STS certificate.  
   netsh http add urlacl url=https://+:8082/STS/mex user="IIS_IUSRS"
   netsh http add sslcert ipport=0.0.0.0:8082 appid={00000000-0000-0000-0000-000000000000} certhash=%STSCERTHASH% clientcertnegotiation=enable

   %windir%\system32\inetsrv\appcmd.exe add apppool /name:WifSamples /managedRuntimeVersion:v4.0 /processModel.loadUserProfile:true

) else ( 

   REM Reserve appropriate HTTP.SYS namespace for NetworkService.
   httpcfg.exe set urlacl -url https://+:8082/STS/mex -acl "D:(A;;GA;;;NS)"

   REM Import server certificate and point HTTP.SYS at it.
   httpcfg.exe set ssl -i 0.0.0.0:443 -f 2 -h %localhostCERTHASH%
   httpcfg.exe set ssl -i 0.0.0.0:8082 -f 2 -h %STSCERTHASH%      
)

REM Modify the IIS metabase so it sets up the SSL certificate 
set IsIIS7=

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%~dp0Scripts\IIS7Util.exe" SetSslCert %localhostCERTHASH%

) else (

   cscript //nologo "%~dp0Scripts\SetSSLCertificate.js" /W3SVC/1 %localhostCERTHASH%
)

echo -------------------------
echo --- STEP 4: Reset IIS ---
echo -------------------------

iisreset

echo Done.

:end
pause
