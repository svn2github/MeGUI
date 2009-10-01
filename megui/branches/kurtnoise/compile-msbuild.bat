@ECHO OFF
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI.csproj^
 /t:Rebuild /p:Configuration=Release /p:Platform="x86" /v:minimal
COPY Bin\x86\Release\*.exe . /Y

REM make dist with everything
MD Dist\BigDist >NUL 2>&1
COPY *.exe Dist\BigDist /Y
COPY lib\*.dll Dist\BigDist /Y
COPY *.dll Dist\BigDist /Y
COPY *.txt Dist\BigDist /Y

REM make minimum distribution for install
MD Dist\MinDist >NUL 2>&1
COPY megui.exe Dist\MinDist /Y
COPY gpl.txt Dist\MinDist /Y
COPY changelog.txt Dist\MinDist /Y
COPY lib\MessageBoxExLib.dll Dist\MinDist /Y
COPY lib\ICSharpCode.SharpZipLib.dll Dist\MinDist /Y

REM also separate distribution into packages
MD Dist\core >NUL 2>&1
COPY megui.exe Dist\core /Y
COPY gpl.txt Dist\core /Y
COPY changelog.txt Dist\core /Y
MD Dist\libs >NUL 2>&1
COPY lib\MessageBoxExLib.dll Dist\libs /Y
COPY lib\ICSharpCode.SharpZipLib.dll Dist\libs /Y
COPY MediaInfo.dll Dist\libs /Y
COPY lib\MediaInfoWrapper.dll Dist\libs /Y
MD Dist\avswrapper >NUL 2>&1
COPY AviSynthWrapper.dll Dist\avswrapper /Y
MD Dist\Data >NUL 2>&1
COPY Data\*.xml Dist\Data /Y

EXIT