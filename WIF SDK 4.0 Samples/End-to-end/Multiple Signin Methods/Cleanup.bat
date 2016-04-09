@echo off

echo Deleting virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/MultiAuthRP"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/MultiAuthSTS_Forms"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/MultiAuthSTS_Windows"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" MultiAuthRP
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" MultiAuthSTS_Forms
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" MultiAuthSTS_Windows
)

echo Done.

pause
 
