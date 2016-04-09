@echo off
setlocal
echo Deleting virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BookHostingService"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BookPublisherSTS"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" BookHostingService
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" BookPublisherSTS 
)

echo Done.

pause
