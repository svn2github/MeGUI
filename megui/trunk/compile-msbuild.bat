@ECHO OFF

REM Compile x86 build
RD /S /Q bin\x86\Release
IF EXIST "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" (
  "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" MeGUI.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86" /v:minimal
) ELSE (
  "%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86" /v:minimal
)

REM Compile x64 build
RD /S /Q bin\x64\Release
IF EXIST "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" (
  "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" MeGUI.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x64" /v:minimal
) ELSE (
  "%WINDIR%\Microsoft.NET\Framework64\v3.5\MSBuild.exe" MeGUI_2008.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x64" /v:minimal
)

EXIT