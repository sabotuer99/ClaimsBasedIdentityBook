@echo off

echo Deleting virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveSTSForClaimsAwareWebAppInWebFarm"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveRedirectBasedClaimsAwareWebAppInWebFarm"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/TokenReplayCacheService"   
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" PassiveSTSForClaimsAwareWebAppInWebFarm 
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" PassiveRedirectBasedClaimsAwareWebAppInWebFarm
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" TokenReplayCacheService
)
 
echo Done.

pause
