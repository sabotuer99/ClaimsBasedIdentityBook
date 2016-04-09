@echo off
setlocal
echo Creating virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/ClaimsAwareService /physicalPath:"%~dp0ClaimsAwareService" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/ClientWebSite /physicalPath:"%~dp0ClientWebSite" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/STSService /physicalPath:"%~dp0STSService" /applicationPool:WifSamples
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  ClaimsAwareService "%~dp0.\ClaimsAwareService"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  ClientWebSite "%~dp0.\ClientWebSite"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  STSService "%~dp0.\STSService"
)

echo Done.

pause
