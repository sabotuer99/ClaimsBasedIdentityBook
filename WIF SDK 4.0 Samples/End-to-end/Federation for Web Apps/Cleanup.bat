@echo off
setlocal
echo Deleting virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/CorpWebSite"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveRP"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveFPSTS"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveIPSTS1"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/PassiveIPSTS2"
) else (
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  CorpWebSite
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  PassiveRP
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  PassiveFPSTS
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  PassiveIPSTS1
  cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs"  PassiveIPSTS2
)

echo Done.

pause
