"%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI.csproj /t:Rebuild /p:Configuration=Release /p:Platform="x86" /v:minimal
COPY Bin\x86\Release\*.exe .

REM make dist with everything
MD Dist\BigDist >NUL 2>&1
COPY *.exe .\Dist\BigDist
COPY .\lib\*.dll .\Dist\BigDist
COPY *.txt .\Dist\BigDist
REM make minimum distribution for install
MD Dist\MinDist >NUL 2>&1
COPY megui.exe .\Dist\MinDist
COPY gpl.txt .\Dist\MinDist
COPY changelog.txt .\Dist\MinDist
COPY .\lib\MessageBoxExLib.dll .\Dist\MinDist
COPY .\lib\ICSharpCode.SharpZipLib.dll .\Dist\MinDist
REM also separate distribution into packages
MD Dist\core >NUL 2>&1
COPY megui.exe .\Dist\core
COPY gpl.txt .\Dist\core
COPY changelog.txt .\Dist\core
MD Dist\libs >NUL 2>&1
COPY .\lib\MessageBoxExLib.dll .\Dist\libs
COPY .\lib\ICSharpCode.SharpZipLib.dll .\Dist\libs
COPY .\lib\MediaInfo.dll .\Dist\libs
COPY .\lib\MediaInfoWrapper.dll .\Dist\libs
COPY .\lib\LinqBridge.dll .\Dist\libs
MD Dist\avswrapper >NUL 2>&1
COPY .\lib\AviSynthWrapper.dll .\Dist\avswrapper
MD Dist\Data >NUL 2>&1
COPY .\Data\*.xml .\Dist\Data

EXIT