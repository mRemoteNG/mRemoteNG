call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe" "D:\Source\mRemoteNG\mRemoteV1.sln" /Rebuild "Release Portable"

rmdir /S /Q D:\Source\mRemoteNG\mRemoteV1\bin\package
mkdir D:\Source\mRemoteNG\mRemoteV1\bin\package
copy D:\Source\mRemoteNG\*.txt D:\Source\mRemoteNG\mRemoteV1\bin\package
copy D:\Source\PuTTYNG\WINDOWS\PuTTYNG.exe D:\Source\mRemoteNG\mRemoteV1\bin\package

rem These del's can error out, that's OK. We don't want these files in the release.
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\confCons*"
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\mRemote3G.log"
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\pnlLayout.xml"
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\extApps.xml"
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\*.pdb"
del "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable\*vshost*"

xcopy /S /Y "D:\Source\mRemoteNG\mRemoteV1\bin\Release Portable" D:\Source\mRemoteNG\mRemoteV1\bin\package

for /f "delims=" %%i in ('findstr /R /C:"^<Assembly: AssemblyVersion" "mRemoteV1\My Project\AssemblyInfo.vb"') do set output="%%i"

"C:\Program Files\7-Zip\7z.exe" a -r -tzip -y D:\Source\mRemoteNG\mRemoteV1\bin\mRemote3G-%output:~-12,-6%.zip D:\Source\mRemoteNG\mRemoteV1\bin\package\*.*
