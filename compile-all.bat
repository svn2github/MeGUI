cd trunk
start /wait compile.bat
cd ..
md trunk\Dist\updatecopier
cd UpdateCopier
start /wait compile-updatecopier.bat
cd ..\trunk
copy ..\UpdateCopier\updatecopier.exe .\Dist\updatecopier
