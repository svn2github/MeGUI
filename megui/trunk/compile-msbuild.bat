@ECHO OFF
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.sln^
 /t:Rebuild /p:Configuration="Release" /p:Platform="x86" /v:minimal

"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.sln^
 /t:Rebuild /p:Configuration="Release" /p:Platform="x64" /v:minimal

EXIT