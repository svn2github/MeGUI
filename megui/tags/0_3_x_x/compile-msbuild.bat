"%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe" MeGUI_2008.csproj /t:Rebuild /p:Configuration=Release /p:Platform="x86"
copy Bin\x86\Release\*.exe .

rem make dist with everything
md Dist\BigDist
copy *.exe .\Dist\BigDist
copy .\lib\*.dll .\Dist\BigDist
copy *.txt .\Dist\BigDist
rem make minimum distribution for install
md Dist\MinDist
copy megui.exe .\Dist\MinDist
copy gpl.txt .\Dist\MinDist
copy changelog.txt .\Dist\MinDist
copy .\lib\MessageBoxExLib.dll .\Dist\MinDist
copy .\lib\ICSharpCode.SharpZipLib.dll .\Dist\MinDist
rem also separate distribution into packages
md Dist\core
copy megui.exe .\Dist\core
copy gpl.txt .\Dist\core
copy changelog.txt .\Dist\core
md Dist\libs
copy .\lib\MessageBoxExLib.dll .\Dist\libs
copy .\lib\ICSharpCode.SharpZipLib.dll .\Dist\libs
copy .\lib\MediaInfo.dll .\Dist\libs
copy .\lib\MediaInfoWrapper.dll .\Dist\libs
md Dist\avswrapper
copy .\lib\AviSynthWrapper.dll .\Dist\avswrapper
md Dist\Data
copy .\Data\*.xml .\Dist\Data
EXIT