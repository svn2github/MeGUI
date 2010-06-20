@ECHO OFF
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" neroraw_2008.sln^
 /t:Rebuild /p:Configuration="Release" /p:Platform="Any CPU" /v:minimal
EXIT /B