@ECHO OFF
IF "%1"=="test" GOTO Test

:Full
ECHO Compiling bautodeint...
ECHO.
dmd main.d avireader.d clip.d commandlineparse.d ScriptServer.d type.d unittestscriptcreator.d avifil32.lib
GOTO End

:Test
ECHO Compiling bautodeint unittest version...
ECHO.
dmd main.d avireader.d clip.d commandlineparse.d ScriptServer.d type.d unittestscriptcreator.d avifil32.lib -unittest
ECHO.
ECHO Running bautodeint unittest version...
ECHO.
main.exe
GOTO End

:End
ECHO.
ECHO Deleting temp files
del bautodeint.exe
rename main.exe bautodeint.exe
del *.obj main.map
PAUSE