@echo off
setlocal

echo Creating Certificates

call "%~dp0..\..\..\Utilities\Scripts\SetUpCert.bat" WifBookStoreService.com trustedpeople
call "%~dp0..\..\..\Utilities\Scripts\SetUpCert.bat" WifBookStoreSTS.com trustedpeople
call "%~dp0..\..\..\Utilities\Scripts\SetUpCert.bat" WifHomeRealmSTS.com trustedpeople

