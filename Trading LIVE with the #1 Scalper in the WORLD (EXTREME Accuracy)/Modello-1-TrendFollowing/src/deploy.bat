@echo off
echo ================================================
echo  FabioTrendFollowing — Deploy to ATAS
echo  Model 1: Trend Following (New York Session)
echo ================================================
echo.

echo [1/2] Building Release...
dotnet build -c Release
if %ERRORLEVEL% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo [2/2] Copying DLL to ATAS Indicators...
set "ATAS_IND=%APPDATA%\ATAS\Indicators"
if not exist "%ATAS_IND%" mkdir "%ATAS_IND%"
copy /Y "bin\Release\net10.0-windows\FabioTrendFollowing.dll" "%ATAS_IND%\"

echo.
echo Done! DLL: %ATAS_IND%\FabioTrendFollowing.dll
echo.
pause
