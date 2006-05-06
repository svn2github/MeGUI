@ECHO OFF
IF "%1"=="full" GOTO Full
IF "%1"=="" GOTO FULL
IF "%1"=="full-svn" GOTO Full-svn
GOTO Syntax


:Full
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /res:App.ico /res:Changelog.txt,MeGUI.Changelog.txt /res:pause.ico /res:play.ico /r:MessageBoxExLib.dll /r:Microsoft.VisualBasic.dll /r:ICSharpCode.SharpZiplib.dll /target:winexe /out:megui.exe /win32icon:app.ico /unsafe+ *.cs /d:CSC /o
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /target:exe /out:neroraw.exe /unsafe+ neroraw\*.cs /d:CSC /o
IF "%1"=="" GOTO Pausing
GOTO End

:Full-svn
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /res:App.ico /res:Changelog.txt,MeGUI.Changelog.txt /res:pause.ico /res:play.ico /r:MessageBoxExLib.dll /r:Microsoft.VisualBasic.dll /target:winexe /out:megui-svn.exe /win32icon:app.ico /unsafe+ *.cs /d:CSC;SVN /o
GOTO End



:Syntax
ECHO.
ECHO MeGUI compilation script 1.2
ECHO.
ECHO Usage: compile.bat [mode]
ECHO.
ECHO WHERE: [mode]   full     : full mode
ECHO.
ECHO                 full-svn : full mode, x264 features limited to SVN
ECHO.
GOTO End

:Pausing
PAUSE
:End
md Dist
copy *.exe .\Dist
copy *.dll .\Dist
copy *.txt .\Dist
md Dist\Data
copy .\Data\*.xml .\Dist\Data