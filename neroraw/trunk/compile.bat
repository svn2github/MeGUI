@ECHO OFF
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" neroraw.sln^
 /t:Rebuild /p:Configuration="Release" /p:Platform="Any CPU" /v:minimal
EXIT /B