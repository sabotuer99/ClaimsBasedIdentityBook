@echo off
setlocal
echo Deleting virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/ClaimsAwareService"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/ClientWebSite"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/STSService"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" ClaimsAwareService
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" ClientWebSite 
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" STSService 
)

echo Done.

pause
