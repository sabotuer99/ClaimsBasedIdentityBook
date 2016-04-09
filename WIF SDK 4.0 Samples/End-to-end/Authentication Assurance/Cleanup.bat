@echo off
setlocal
echo Deleting virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/AuthAssuranceSTS"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/AuthAssuranceRP"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  AuthAssuranceSTS
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  AuthAssuranceRP
)

echo Deleting Certificate...

call "%~dp0..\..\Utilities\Scripts\CleanUpCert.bat" bobclient trustedpeople CurrentUser

echo Done.

pause
