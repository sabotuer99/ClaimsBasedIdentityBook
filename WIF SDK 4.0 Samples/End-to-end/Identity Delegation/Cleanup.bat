@echo off
echo Deleting virtual directories...
(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

echo.

if "%IsIIS7%" == "true" (
  "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveSTS"
  "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/WFE"
  "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/Service1"
) else (
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  PassiveSTS
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  WFE
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  Service1
)

echo Done.

pause
