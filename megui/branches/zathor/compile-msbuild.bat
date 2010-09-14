@ECHO OFF

REM Compile x86 build
RD /S /Q bin\x86\Release >NUL 2>&1
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.sln /t:Rebuild^
 /p:Configuration="Release" /p:Platform="x86" /v:minimal

REM Compile x64 build
RD /S /Q bin\x64\Release >NUL 2>&1
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.sln /t:Rebuild^
 /p:Configuration="Release" /p:Platform="x64" /v:minimal

EXIT