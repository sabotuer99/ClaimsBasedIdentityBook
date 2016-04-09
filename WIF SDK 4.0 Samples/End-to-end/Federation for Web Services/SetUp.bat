@echo off
setlocal

echo Creating virtual directories...

echo.
echo FederationSample

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

@if "%IsIIS7%" == "true" (

   "%systemroot%\system32\inetsrv\AppCmd.exe" add vdir /app.name:"Default Web Site/" /path:/FederationSample /physicalPath:"%~dp0
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/FederationSample/BookStoreService /physicalPath:"%~dp0BookStoreService" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/FederationSample/BookStoreSTS /physicalPath:"%~dp0BookStoreSTS" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/FederationSample/HomeRealmSTS /physicalPath:"%~dp0HomeRealmSTS" /applicationPool:WifSamples

) else (
 
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs" FederationSample "%~dp0"
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs" FederationSample/BookStoreService "%~dp0BookStoreService"    
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs" FederationSample/BookStoreSTS "%~dp0BookStoreSTS"    
  cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs" FederationSample/HomeRealmSTS "%~dp0HomeRealmSTS"    

)  

call "%~dp0Scripts\SetUpCerts.bat"

iisreset

echo Done.
pause


