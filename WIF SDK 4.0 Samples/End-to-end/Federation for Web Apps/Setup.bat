@echo off
setlocal
echo Creating virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/CorpWebSite /physicalPath:"%~dp0CorpWebSite" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveRP /physicalPath:"%~dp0FederationPassiveRP" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveFPSTS /physicalPath:"%~dp0FPSTS" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveIPSTS1 /physicalPath:"%~dp0IPSTS1" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveIPSTS2 /physicalPath:"%~dp0IPSTS2" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveIPSTS1" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveIPSTS1" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost	
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveIPSTS2" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
   "%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveIPSTS2" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost	   
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  CorpWebSite "%~dp0.\CorpWebSite"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveRP "%~dp0.\FederationPassiveRP"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveFPSTS "%~dp0.\FPSTS"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveIPSTS1 "%~dp0.\IPSTS1" windowsAuth
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveIPSTS2 "%~dp0.\IPSTS2" windowsAuth
)

echo Done.

pause
