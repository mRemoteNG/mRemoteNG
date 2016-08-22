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

%SORTRESX% %TEMP_FOLDER%\ar\Language.ar.resx "%~dp0\mRemoteV1\Language\Language.ar.resx"
%SORTRESX% %TEMP_FOLDER%\bg\Language.bg.resx "%~dp0\mRemoteV1\Language\Language.bg.resx"
%SORTRESX% %TEMP_FOLDER%\de\Language.de.resx "%~dp0\mRemoteV1\Language\Language.de.resx"
%SORTRESX% %TEMP_FOLDER%\el\Language.el.resx "%~dp0\mRemoteV1\Language\Language.el.resx"
%SORTRESX% %TEMP_FOLDER%\en-US\Language.en.resx "%~dp0\mRemoteV1\Language\Language.en-US.resx"
%SORTRESX% %TEMP_FOLDER%\es-AR\Language.es.resx "%~dp0\mRemoteV1\Language\Language.es-AR.resx"
%SORTRESX% %TEMP_FOLDER%\es-ES\Language.es.resx "%~dp0\mRemoteV1\Language\Language.es.resx"
%SORTRESX% %TEMP_FOLDER%\fr\Language.fr.resx "%~dp0\mRemoteV1\Language\Language.fr.resx"
%SORTRESX% %TEMP_FOLDER%\he\Language.he.resx "%~dp0\mRemoteV1\Language\Language.he.resx"
%SORTRESX% %TEMP_FOLDER%\hu\Language.hu.resx "%~dp0\mRemoteV1\Language\Language.hu.resx"
%SORTRESX% %TEMP_FOLDER%\it\Language.it.resx "%~dp0\mRemoteV1\Language\Language.it.resx"
%SORTRESX% %TEMP_FOLDER%\nb\Language.nb.resx "%~dp0\mRemoteV1\Language\Language.nb-NO.resx"
%SORTRESX% %TEMP_FOLDER%\nl\Language.nl.resx "%~dp0\mRemoteV1\Language\Language.nl.resx"
%SORTRESX% %TEMP_FOLDER%\pl\Language.pl.resx "%~dp0\mRemoteV1\Language\Language.pl.resx"
%SORTRESX% %TEMP_FOLDER%\pt-BR\Language.pt.resx "%~dp0\mRemoteV1\Language\Language.pt-BR.resx"
%SORTRESX% %TEMP_FOLDER%\pt-PT\Language.pt.resx "%~dp0\mRemoteV1\Language\Language.pt.resx"
%SORTRESX% %TEMP_FOLDER%\ru\Language.ru.resx "%~dp0\mRemoteV1\Language\Language.ru.resx"
%SORTRESX% %TEMP_FOLDER%\uk\Language.uk.resx "%~dp0\mRemoteV1\Language\Language.uk.resx"
%SORTRESX% %TEMP_FOLDER%\zh-CN\Language.zh.resx "%~dp0\mRemoteV1\Language\Language.zh-CN.resx"
%SORTRESX% %TEMP_FOLDER%\zh-TW\Language.zh.resx "%~dp0\mRemoteV1\Language\Language.zh-TW.resx"

rmdir /s /q %TEMP_FOLDER%

echo Done.
echo.
pause
