@ECHO OFF
TITLE Compiling MeGUI...
CD megui\trunk
START /B /WAIT compile-msbuild.bat

CD ..\..
REM Detect if we are running on 64bit WIN and use Wow6432Node, set the path
REM of NSIS accordingly and compile installer
IF "%PROGRAMFILES(x86)%zzz"=="zzz" (SET "U_=HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
) ELSE (
SET "U_=HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
)

SET "K_=NSIS"
SET "M_=NSIS is NOT installed! Installer won't be built!"
FOR /f "delims=" %%a IN (
	'REG QUERY "%U_%\%K_%" /v "InstallLocation"2^>Nul^|FIND "REG_"') DO (
	SET "NSISPath=%%a"&Call :Sub %%NSISPath:*Z=%%)

FOR /f "delims=" %%a IN (
	'REG QUERY "%U_%\%K_%" /v "DisplayVersion"2^>Nul^|FIND "REG_"') DO (
	SET "NSISVer=%%a"&Call :Sub2 %%NSISVer:*Z=%%)

ECHO.
IF DEFINED NSISPath (ECHO:Compiling installer...&&(
	"%NSISPath%\makensis.exe" /V2 "Installer\trunk\megui.nsi")&&(
	ECHO.&&ECHO:Installer compiled with NSIS v%NSISVer% successfully!)
) ELSE (ECHO:%M_%)

ECHO.
ECHO.
PAUSE

:Sub
SET NSISPath=%*

:Sub2
SET NSISVer=%*
