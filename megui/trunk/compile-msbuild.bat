@ECHO OFF
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.csproj^
 /t:Rebuild /p:Configuration="Release" /p:Platform="x86" /v:minimal

ECHO.
REM make dist with everything
MD Dist\BigDist >NUL 2>&1
ECHO:Copying files...

COPY Bin\x86\Release\AvisynthWrapper.dll Dist\BigDist /V /Y >NUL
COPY Changelog.txt Dist\BigDist /V /Y >NUL
COPY gpl.txt Dist\BigDist /V /Y >NUL
COPY lib\ICSharpCode.SharpZipLib.dll Dist\BigDist /V /Y >NUL
COPY lib\LinqBridge.dll Dist\BigDist /V /Y >NUL
COPY Bin\x86\Release\MediaInfo.dll Dist\BigDist /V /Y >NUL
COPY lib\MediaInfoWrapper.dll Dist\BigDist /V /Y >NUL
COPY Bin\x86\Release\MeGUI.exe Dist\BigDist /V /Y >NUL
COPY lib\MessageBoxExLib.dll Dist\BigDist /V /Y >NUL

REM make minimum distribution for install
MD Dist\MinDist >NUL 2>&1
COPY Changelog.txt Dist\MinDist /V /Y >NUL
COPY gpl.txt Dist\MinDist /V /Y >NUL
COPY lib\ICSharpCode.SharpZipLib.dll Dist\MinDist /V /Y >NUL
COPY Bin\x86\Release\MeGUI.exe Dist\MinDist /V /Y >NUL
COPY lib\MessageBoxExLib.dll Dist\MinDist /V /Y >NUL

REM also separate distribution into packages
MD Dist\core >NUL 2>&1
COPY Changelog.txt Dist\core /V /Y >NUL
COPY gpl.txt Dist\core /V /Y >NUL
COPY Bin\x86\Release\MeGUI.exe Dist\core /V /Y >NUL

MD Dist\libs >NUL 2>&1
COPY lib\ICSharpCode.SharpZipLib.dll Dist\libs /V /Y >NUL
COPY lib\LinqBridge.dll Dist\libs /V /Y >NUL
COPY Bin\x86\Release\MediaInfo.dll Dist\libs /V /Y >NUL
COPY lib\MediaInfoWrapper.dll Dist\libs /V /Y >NUL
COPY lib\MessageBoxExLib.dll Dist\libs /V /Y >NUL

MD Dist\avswrapper >NUL 2>&1
COPY Bin\x86\Release\AvisynthWrapper.dll Dist\avswrapper /V /Y >NUL

MD Dist\Data >NUL 2>&1
COPY Data\ContextHelp.xml Dist\Data /V /Y >NUL

EXIT