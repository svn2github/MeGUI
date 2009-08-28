@ECHO OFF
CD megui\trunk
START /B /WAIT compile-msbuild.bat

CD ..\..
CD TestSuite\trunk
START /B /WAIT compile.bat

CD ..\..
MD megui\trunk\Dist\updatecopier >NUL 2>&1
CD UpdateCopier\trunk
START /B /WAIT compile-updatecopier.bat

CD ..\..
COPY UpdateCopier\trunk\updatecopier.exe^
 megui\trunk\Dist\updatecopier

CD Installer\trunk
REM Detect if we are running on 64bit WIN and use Wow6432Node, set the path
REM of NSIS accordingly and compile installer
IF "%PROGRAMFILES(x86)%zzz"=="zzz" (SET "U_=HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
) ELSE (
SET "U_=HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
)

SET "K_=NSIS"
SET "M_=NSIS IS NOT INSTALLED!!!"
FOR /f "delims=" %%a IN (
	'REG QUERY "%U_%\%K_%" /v "InstallLocation"2^>Nul^|FIND "REG_"') DO (
	SET "NSISPath=%%a"&Call :Sub %%NSISPath:*Z=%%)

FOR /f "delims=" %%a IN (
	'REG QUERY "%U_%\%K_%" /v "DisplayVersion"2^>Nul^|FIND "REG_"') DO (
	SET "NSISVer=%%a"&Call :Sub2 %%NSISVer:*Z=%%)

ECHO.
IF DEFINED NSISPath ("%NSISPath%\makensis.exe" /V2 "megui.nsi"&&(
	ECHO.&&ECHO:Installer compiled with NSIS v%NSISVer% successfully!)) ELSE (ECHO:%M_%)

ECHO.
PAUSE

:Sub
SET NSISPath=%*

:Sub2
SET NSISVer=%*
