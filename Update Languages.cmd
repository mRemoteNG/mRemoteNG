@echo off

set ZIP_FILE="%~dp0\..\mremoteng.zip"
set TEMP_FOLDER="%TEMP%\Translations.tmp"
set SORTRESX="%~dp0\Tools\SortRESX.exe"
set RAR="%ProgramFiles%\WinRAR\WinRAR.exe"

call :expand %ZIP_FILE%
goto :skip
:expand
set ZIP_FILE="%~f1"
exit /b
:skip

echo.
echo This script updates the language files with new translations from Crowdin
echo.
echo Download the latest translation file from Crowdin and place it here:
echo.
echo %ZIP_FILE%
echo.

choice /m "Would you like to continue?"

echo.

mkdir %TEMP_FOLDER%
%RAR% x -o+ %ZIP_FILE% *.* %TEMP_FOLDER%

%SORTRESX% %TEMP_FOLDER%\de\Language.de-DE.resx "%~dp0\mRemoteV1\Language\Language.de.resx"
%SORTRESX% %TEMP_FOLDER%\en-US\Language..resx "%~dp0\mRemoteV1\Language\Language.en-US.resx"
%SORTRESX% %TEMP_FOLDER%\es-ES\Language.es-ES.resx "%~dp0\mRemoteV1\Language\Language.es.resx"
%SORTRESX% %TEMP_FOLDER%\fr\Language.fr-FR.resx "%~dp0\mRemoteV1\Language\Language.fr.resx"
%SORTRESX% %TEMP_FOLDER%\it\Language.it-IT.resx "%~dp0\mRemoteV1\Language\Language.it.resx"
%SORTRESX% %TEMP_FOLDER%\nl\Language.nl-NL.resx "%~dp0\mRemoteV1\Language\Language.nl.resx"
%SORTRESX% %TEMP_FOLDER%\ru\Language.ru-RU.resx "%~dp0\mRemoteV1\Language\Language.ru.resx"
%SORTRESX% %TEMP_FOLDER%\uk\Language.uk-UA.resx "%~dp0\mRemoteV1\Language\Language.uk.resx"

rmdir /s /q %TEMP_FOLDER%

echo Done.
echo.
pause
