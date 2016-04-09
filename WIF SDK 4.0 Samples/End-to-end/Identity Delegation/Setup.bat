@echo off
echo Creating virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
  "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveSTS /physicalPath:"%~dp0PassiveSTS" /applicationPool:WifSamples
  "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/WFE /physicalPath:"%~dp0WFE" /applicationPool:WifSamples
  "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/Service1 /physicalPath:"%~dp0Service1" /applicationPool:WifSamples
  "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTS" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
  "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTS" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost
) else (
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveSTS "%~dp0.\PassiveSTS" windowsAuth
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  WFE "%~dp0.\WFE"
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  Service1 "%~dp0.\Service1"
)

echo Done.

pause
