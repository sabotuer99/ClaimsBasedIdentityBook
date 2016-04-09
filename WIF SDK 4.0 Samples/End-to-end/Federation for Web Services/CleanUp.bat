@echo off
setlocal

echo Deleting sample virtual directories from IIS
echo. 

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

@if "%IsIIS7%" == "true" (

   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/FederationSample/BookStoreService"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/FederationSample/BookStoreSTS"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/FederationSample/HomeRealmSTS"
   "%systemroot%\system32\inetsrv\AppCmd.exe" delete vdir "Default Web Site/FederationSample"
   
)  else (
   
      cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" FederationSample/BookStoreService
      cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" FederationSample/BookStoreSTS
      cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" FederationSample/HomeRealmSTS
      cscript //nologo "%~dp0..\..\Utilities\Scripts\DeleteSampleVdir.vbs" FederationSample
) 


REM Remove certs from store
echo.
call "%~dp0Scripts/CleanUpCerts.bat"

iisreset

echo.
pause
 
