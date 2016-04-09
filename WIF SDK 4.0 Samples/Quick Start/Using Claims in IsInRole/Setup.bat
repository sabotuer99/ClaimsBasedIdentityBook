@echo off
setlocal
set IsVistaOrHigher=
set IsWin7OrHigher=

for /f "skip=1" %%i in ( 'wmic os get version' ) do ( 
    set OS_VERSION=%%i 
    goto:__ver_done
)
:__ver_done

for /f "delims=. tokens=1,2,3" %%i in ("%OS_VERSION%") do ( 
    set OS_MAJOR=%%i&set OS_MINOR=%%j&set OS_BUILD=%%k  
    goto :__ver_split_done
)
:__ver_split_done

if "%OS_MAJOR%" GEQ "6" (
    set IsVistaOrHigher=true
    if "%OS_MINOR%" == "1" (
        set IsWin7OrHigher=true
        goto :__ver_set_done
    )
    if "%OS_MAJOR%" GTR "6" (
        set IsWin7OrHigher=true
        goto :__ver_set_done
    )
)

if "%OS_MAJOR%" == "5" (
    if "%OS_MINOR%" == "2" (
        set IsWin2K3=true
    )
    goto :__ver_set_done
)

:__ver_set_done

echo Creating virtual directories...

(reg query HKLM\Software\Microsoft\InetStp /v MajorVersion | findstr /C:"0x7") && set IsIIS7=true

if "%IsIIS7%" == "true" (
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/BookHostingService /physicalPath:"%~dp0BookHostingService" /applicationPool:WifSamples
   "%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/BookPublisherSTS /physicalPath:"%~dp0BookPublisherSTS" /applicationPool:WifSamples
   icacls BookHostingService\FrameworkSamples.txt /grant "IIS_IUSRS":WRX
) else (
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  BookHostingService "%~dp0.\BookHostingService"
   cscript //nologo "%~dp0..\..\Utilities\Scripts\CreateSampleVdir.vbs"  BookPublisherSTS "%~dp0.\BookPublisherSTS"
)

icacls "%~dp0.\BookHostingService\FrameworkSamples.txt" /grant *S-1-5-20:F

@if "%IsWin7OrHigher%" == "true" (
    icacls "%~dp0.\BookHostingService\FrameworkSamples.txt" /grant "IIS_IUSRS":F
)

echo Done.

pause
