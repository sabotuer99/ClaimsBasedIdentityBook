rem echo Usage: updateSetupFolder.cmd (Source-folder-path) (Destination-folder-path)

@echo off
%~d0

IF "%1%"=="" goto EXIT
IF "%2%"=="" goto EXIT
SET srcFolderPath=%1%
SET destFolderPath=%2%

cd "%destFolderPath%"
del DependencyChecker.exe
del Microsoft.Web.Deployment.dll
del Microsoft.Web.PlatformInstaller.dll
rem del Dependencies.xml

cd "%~dp0"

cd "%srcFolderPath%"
xcopy DependencyChecker.exe "%destFolderPath%"
xcopy Microsoft.Web.Deployment.dll "%destFolderPath%"
xcopy Microsoft.Web.PlatformInstaller.dll "%destFolderPath%"
rem xcopy Dependencies.xml "%destFolderPath%"

echo Done
GOTO EXIT

:EXIT
cd "%~dp0"