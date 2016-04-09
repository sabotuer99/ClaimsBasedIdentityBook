@echo off

echo Deleting virtual directories ...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/AjaxSample_STS"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/AjaxSample_RP"
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" AjaxSample_STS
   cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" AjaxSample_RP
)

echo Done.

pause 
 
