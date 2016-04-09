@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/CustomUserNameCardStsHostFactory /physicalPath:"%~dp0CustomUserNameCardStsHostFactory" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/CustomUserNameCardStsHostFactoryWebSite /physicalPath:"%~dp0CustomUserNameCardStsHostFactoryWebSite" /applicationPool:WifSamples
) else (
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  CustomUserNameCardStsHostFactory "%~dp0.\CustomUserNameCardStsHostFactory"
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  CustomUserNameCardStsHostFactoryWebSite "%~dp0.\CustomUserNameCardStsHostFactoryWebSite"
)
 
echo Done.

pause
