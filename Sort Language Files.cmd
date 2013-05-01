@echo off

set TEMP_FOLDER="%TEMP%\Translations.tmp"
set SORTRESX="%~dp0\Tools\SortRESX.exe"

echo.
echo This script sorts the language files
echo.

choice /m "Would you like to continue?"

echo.

rmdir /s /q %TEMP_FOLDER%
mkdir %TEMP_FOLDER%
copy "%~dp0\mRemoteV1\Language\*.resx" %TEMP_FOLDER%

pushd %TEMP_FOLDER%
for %%f in (*) do %SORTRESX% %TEMP_FOLDER%\%%f "%~dp0\mRemoteV1\Language\%%f"
popd

rmdir /s /q %TEMP_FOLDER%

echo Done.
echo.
pause
