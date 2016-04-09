@echo off

echo Creating virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/AjaxSample_RP /physicalPath:"%~dp0AjaxRP" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/AjaxSample_STS /physicalPath:"%~dp0AjaxSTS" /applicationPool:WifSamples
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  AjaxSample_RP "%~dp0.\AjaxRP"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  AjaxSample_STS "%~dp0.\AjaxSTS"
)

echo Done.

pause 
