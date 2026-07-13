@echo off
setlocal

echo ================================================
echo  FabioOrderFlow - Build and deploy neutral shell
echo ================================================
echo.

echo [1/2] Building Release...
dotnet build -c Release
if %ERRORLEVEL% neq 0 (
    echo BUILD FAILED
    exit /b 1
)

echo.
echo [2/2] Copying DLL to ATAS Indicators...
set "ATAS_IND=%APPDATA%\ATAS\Indicators"
if not exist "%ATAS_IND%" mkdir "%ATAS_IND%"
copy /Y "bin\Release\net10.0-windows\FabioOrderFlow.dll" "%ATAS_IND%\"
if %ERRORLEVEL% neq 0 (
    echo DEPLOY FAILED
    exit /b 1
)

echo.
echo DLL deployed to %ATAS_IND%\FabioOrderFlow.dll
echo Restart ATAS or remove and re-add the indicator to load it.
