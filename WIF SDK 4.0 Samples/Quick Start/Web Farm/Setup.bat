@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveRedirectBasedClaimsAwareWebAppInWebFarm /physicalPath:"%~dp0PassiveRedirectBasedClaimsAwareWebAppInWebFarm" /applicationPool:WifSamples
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveSTSForClaimsAwareWebAppInWebFarm /physicalPath:"%~dp0PassiveSTS" /applicationPool:WifSamples
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/TokenReplayCacheService /physicalPath:"%~dp0TokenReplayCacheService" /applicationPool:WifSamples
   	"%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTSForClaimsAwareWebAppInWebFarm" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
	"%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTSForClaimsAwareWebAppInWebFarm" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost	
) else (
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveRedirectBasedClaimsAwareWebAppInWebFarm "%~dp0.\PassiveRedirectBasedClaimsAwareWebAppInWebFarm"
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveSTSForClaimsAwareWebAppInWebFarm "%~dp0.\PassiveSTS" windowsAuth
    cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  TokenReplayCacheService "%~dp0.\TokenReplayCacheService"	
) 

echo Done.

pause
