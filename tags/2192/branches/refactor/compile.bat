@ECHO OFF
IF "%1"=="full" GOTO Full
IF "%1"=="" GOTO FULL
GOTO Syntax

:Full
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /res:App.ico /res:Changelog.txt,MeGUI.Changelog.txt /res:pause.ico /res:play.ico /r:MessageBoxExLib.dll /r:Microsoft.VisualBasic.dll /r:ICSharpCode.SharpZiplib.dll /r:MediaInfoWrapper.dll /target:winexe /out:megui.exe /win32icon:app.ico /unsafe+ *.cs /d:CSC /o
rem %windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /target:exe /out:neroraw.exe /unsafe+ neroraw\*.cs /d:CSC /o
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /target:exe /out:updatecopier.exe updatecopier\*.cs /d:CSC /o
IF "%1"=="" GOTO Pausing
GOTO End

:Syntax
ECHO.
ECHO MeGUI compilation script 1.2
ECHO.
ECHO Usage: compile.bat [mode]
ECHO.
ECHO WHERE: [mode]   full     : full mode
ECHO.
GOTO End

:Pausing
PAUSE
:End
rem make dist with everything
md Dist\BigDist
copy *.exe .\Dist\BigDist
copy *.dll .\Dist\BigDist
copy *.txt .\Dist\BigDist
rem make minimum distribution for install
md Dist\MinDist
copy megui.exe .\Dist\MinDist
copy gpl.txt .\Dist\MinDist
copy changelog.txt .\Dist\MinDist
copy MessageBoxExLib.dll .\Dist\MinDist
copy ICSharpCode.SharpZipLib.dll .\Dist\MinDist
rem also separate distribution into packages
md Dist\core
copy megui.exe .\Dist\core
copy gpl.txt .\Dist\core
copy changelog.txt .\Dist\core
md Dist\libs
copy MessageBoxExLib.dll .\Dist\libs
copy ICSharpCode.SharpZipLib.dll .\Dist\libs
copy MediaInfo.dll .\Dist\libs
copy MediaInfoWrapper.dll .\Dist\libs
md Dist\avswrapper
copy AviSynthWrapper.dll .\Dist\avswrapper
md Dist\updatecopier
copy updatecopier.exe .\Dist\updatecopier
md Dist\Data
copy .\Data\*.xml .\Dist\Data