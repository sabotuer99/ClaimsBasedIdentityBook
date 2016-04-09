@echo off

echo Deleting virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveSTSForClaimsAwareWebApp"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveRedirectBasedClaimsAwareWebApp"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/WebControlBasedClaimsAwareWebApp"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" PassiveSTSForClaimsAwareWebApp 
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" PassiveRedirectBasedClaimsAwareWebApp
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" WebControlBasedClaimsAwareWebApp
)
 
echo Done.

pause
