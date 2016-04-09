@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/MultiAuthRP /physicalPath:"%~dp0RelyingParty" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/MultiAuthSTS_Forms /physicalPath:"%~dp0STSForms" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/MultiAuthSTS_Windows /physicalPath:"%~dp0STSWindows" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/MultiAuthSTS_Windows" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/MultiAuthSTS_Windows" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  MultiAuthRP "%~dp0.\RelyingParty"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  MultiAuthSTS_Forms "%~dp0.\STSForms"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  MultiAuthSTS_Windows "%~dp0.\STSWindows" windowsAuth
)
 
echo Done.

pause
