@echo off
REM FabioOrderFlow - Trade Report Tool
REM Extracts and formats MR trades from ATAS logs

setlocal enabledelayedexpansion

set LOG_FILE=%APPDATA%\ATAS\Logs\FabioOrderFlow.log
set OUTPUT_FILE=trade-report.txt
set DATE_FILTER=%1

if "%DATE_FILTER%"=="" (
    REM Default: today's date in format yyyy-MM-dd
    for /f "tokens=1-3 delims=/ " %%a in ('date /t') do (
        set TODAY=%%c-%%b-%%a
    )
    set DATE_FILTER=!TODAY!
)

echo ===============================================================================
echo FabioOrderFlow - Trade Report
echo ===============================================================================
echo Log file: %LOG_FILE%
echo Date filter: %DATE_FILTER%
echo Output: %OUTPUT_FILE%
echo.

if not exist "%LOG_FILE%" (
    echo ERROR: Log file not found: %LOG_FILE%
    pause
    exit /b 1
)

echo Extracting trades for date: %DATE_FILTER%
echo.

REM Create output file with header
echo =============================================================================== > %OUTPUT_FILE%
echo FabioOrderFlow - Trade Report >> %OUTPUT_FILE%
echo Date: %DATE_FILTER% >> %OUTPUT_FILE%
echo Generated: %DATE% %TIME% >> %OUTPUT_FILE%
echo =============================================================================== >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%

REM Extract triggers
echo [TRIGGERS] >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%
findstr /C:"[MR_TRIGGER]" /C:"[MR_EARLY_TRIGGER]" "%LOG_FILE%" | findstr /C:"Italy=%DATE_FILTER%" >> %OUTPUT_FILE% 2>nul
echo. >> %OUTPUT_FILE%

REM Extract aggression confirmations
echo [AGGRESSION CONFIRMATIONS] >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%
findstr /C:"[MR_AGGRESSION_CONFIRM]" "%LOG_FILE%" | findstr /C:"Italy=%DATE_FILTER%" >> %OUTPUT_FILE% 2>nul
echo. >> %OUTPUT_FILE%

REM Extract MFE updates
echo [MFE UPDATES] >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%
findstr /C:"[MR_MFE_UPDATE]" "%LOG_FILE%" | findstr /C:"Italy=%DATE_FILTER%" >> %OUTPUT_FILE% 2>nul
echo. >> %OUTPUT_FILE%

REM Extract target hits
echo [TARGET HITS] >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%
findstr /C:"[MR_TARGET_HIT]" "%LOG_FILE%" | findstr /C:"Italy=%DATE_FILTER%" >> %OUTPUT_FILE% 2>nul
echo. >> %OUTPUT_FILE%

REM Extract position closed
echo [POSITION CLOSED] >> %OUTPUT_FILE%
echo. >> %OUTPUT_FILE%
findstr /C:"[MR_POSITION_CLOSED]" "%LOG_FILE%" | findstr /C:"Italy=%DATE_FILTER%" >> %OUTPUT_FILE% 2>nul
echo. >> %OUTPUT_FILE%

echo =============================================================================== >> %OUTPUT_FILE%
echo End of Report >> %OUTPUT_FILE%
echo =============================================================================== >> %OUTPUT_FILE%

echo.
echo Report generated: %OUTPUT_FILE%
echo.
echo Opening report...
type %OUTPUT_FILE%

echo.
echo Press any key to exit...
pause >nul
