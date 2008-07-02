cd .\megui\trunk
start /wait compile-msbuild.bat
cd ..\..
cd .\testsuite\trunk
start /wait compile.bat
cd ..\..
md .\megui\trunk\Dist\updatecopier
cd .\UpdateCopier\trunk
start /wait compile-updatecopier.bat
cd ..\..
copy .\UpdateCopier\trunk\updatecopier.exe .\megui\trunk\Dist\updatecopier
