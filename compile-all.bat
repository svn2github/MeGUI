cd megui
start /wait compile.bat
cd ..
cd testsuite
start /wait compile.bat
cd ..
md megui\Dist\updatecopier
cd UpdateCopier
start /wait compile-updatecopier.bat
cd ..\megui
copy ..\UpdateCopier\updatecopier.exe .\Dist\updatecopier
