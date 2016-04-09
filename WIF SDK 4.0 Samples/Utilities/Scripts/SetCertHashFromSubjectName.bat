@echo off

@if "%1"=="" (
   @echo usage: %~n0 {certificate subject name}
   @goto :hashend
)

set CERTHASH=

set WIFCERTUTIL="%~dp0WifCertUtil.exe"

for /f %%a in ( '%WIFCERTUTIL% GetCertThumbprint my %1' ) do ( set CERTHASH=%%a )

set %1CERTHASH=%CERTHASH%

:hashend
	
