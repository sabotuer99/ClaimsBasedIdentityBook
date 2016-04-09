@echo off
setlocal
echo Creating virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/AuthAssuranceSTS /physicalPath:"%~dp0AuthAssuranceSTS" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/AuthAssuranceRP /physicalPath:"%~dp0AuthAssuranceRP" /applicationPool:WifSamples
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  AuthAssuranceSTS "%~dp0.\AuthAssuranceSTS"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  AuthAssuranceRP "%~dp0.\AuthAssuranceRP"
)

echo Creating Certificate...

call "%~dp0..\..\Utilities\Scripts\SetUpCert.bat" bobclient trustedpeople CurrentUser

echo Done.

pause
