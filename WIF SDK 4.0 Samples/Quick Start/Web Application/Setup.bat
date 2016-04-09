@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveRedirectBasedClaimsAwareWebApp /physicalPath:"%~dp0PassiveRedirectBasedClaimsAwareWebApp" /applicationPool:WifSamples
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/WebControlBasedClaimsAwareWebApp /physicalPath:"%~dp0WebControlBasedClaimsAwareWebApp" /applicationPool:WifSamples
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/PassiveSTSForClaimsAwareWebApp /physicalPath:"%~dp0PassiveSTS" /applicationPool:WifSamples
   	"%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTSForClaimsAwareWebApp" /section:system.webServer/security/authentication/windowsAuthentication /enabled:true /commit:apphost
	"%systemroot%\system32\inetsrv\AppCmd.exe" SET CONFIG "Default Web Site/PassiveSTSForClaimsAwareWebApp" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:false /commit:apphost	
) else (
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveRedirectBasedClaimsAwareWebApp "%~dp0.\PassiveRedirectBasedClaimsAwareWebApp"
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  WebControlBasedClaimsAwareWebApp "%~dp0.\WebControlBasedClaimsAwareWebApp"
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  PassiveSTSForClaimsAwareWebApp "%~dp0.\PassiveSTS" windowsAuth
) 

echo Done.

pause
