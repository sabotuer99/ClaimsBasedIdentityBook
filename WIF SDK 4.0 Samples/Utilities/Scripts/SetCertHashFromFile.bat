@echo off

@if "%1"=="" (
   @echo usage: %~n0 {certificate subject name}
   @goto :end
)

if NOT EXIST "%~dp0%1.cer" (
  goto :end
)

set CERTHASH=

set WIFCERTUTIL="%~dp0WifCertUtil.exe"

pushd %~dp0
for /f %%a in ( '%WIFCERTUTIL% GetCertThumbprint my %1.cer' ) do ( set CERTHASH=%%a )
popd

set %1CERTHASH=%CERTHASH%

:end
