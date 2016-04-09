@echo off

@if "%1"=="" (
   @echo usage: %~n0 {certificate's subject name} {extra store name} [store location]
   @echo Example: %~n0 localhost root
   @echo Example: %~n0 localhost trustedpeople
   @echo Example: %~n0 bobclient trustedpeople CurrentUser
   @goto :end
)

@set CERTFILE="%~dp0%1.cer"

if NOT EXIST %CERTFILE% (
  goto :end
)

call "%~dp0SetCertHashFromFile.bat" %1

set CERTUTIL_OPTION=
if "%3" == "CurrentUser" (
  set CERTUTIL_OPTION=-user
)

set KEYCONTAINER=
set WIFCERTUTIL="%~dp0WifCertUtil.exe"
for /f %%a in ( '%WIFCERTUTIL% GetCertKeyContainer my %CERTHASH% %CERTUTIL_OPTION%' ) do ( set KEYCONTAINER=%%a )

if "%3" == "CurrentUser" (
  set CSP="Microsoft Enhanced RSA and AES Cryptographic Provider"
) else (
  set CSP="Microsoft RSA SChannel Cryptographic Provider"
)

REM delete the certificate that was made by WifMakeCert
certutil %CERTUTIL_OPTION% -delstore my %CERTHASH%

REM remove the certificate from the trusted root store
certutil %CERTUTIL_OPTION% -delstore %2 %CERTHASH%

REM remove the private key file from the system
certutil %CERTUTIL_OPTION% -delkey -csp %CSP% %KEYCONTAINER%

del %CERTFILE%

:end
