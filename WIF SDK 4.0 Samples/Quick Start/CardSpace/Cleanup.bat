@echo off

echo Deleting virtual directories ...
(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/CustomUserNameCardStsHostFactory"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/CustomUserNameCardStsHostFactoryWebSite"
) else (
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" CustomUserNameCardStsHostFactory
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" CustomUserNameCardStsHostFactoryWebSite
)
 
echo Done.

pause 
