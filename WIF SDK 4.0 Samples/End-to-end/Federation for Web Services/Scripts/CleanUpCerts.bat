@echo off
setlocal

echo Deleting Certificates

call "%~dp0..\..\..\Utilities\Scripts\CleanUpCert.bat" WifBookStoreService.com trustedpeople
call "%~dp0..\..\..\Utilities\Scripts\CleanUpCert.bat" WifBookStoreSTS.com trustedpeople
call "%~dp0..\..\..\Utilities\Scripts\CleanUpCert.bat" WifHomeRealmSTS.com trustedpeople
