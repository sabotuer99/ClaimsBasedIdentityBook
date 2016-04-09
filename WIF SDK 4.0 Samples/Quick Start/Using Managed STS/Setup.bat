@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
	"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/ClaimsAwareWebAppWithManagedSTS /physicalPath:"%~dp0ClaimsAwareWebAppWithManagedSTS" /applicationPool:WifSamples
) else (
	cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  ClaimsAwareWebAppWithManagedSTS "%~dp0.\ClaimsAwareWebAppWithManagedSTS"
) 

echo Done.

pause
