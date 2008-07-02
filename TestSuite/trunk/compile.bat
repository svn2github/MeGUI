cd ..
copy .\megui\trunk\megui.exe .\TestSuite\trunk
copy .\megui\trunk\icsharpcode.sharpziplib.dll .\TestSuite\trunk
copy .\megui\trunk\mediainfowrapper.dll .\TestSuite\trunk
copy .\megui\trunk\MessageBoxExLib.dll .\TestSuite\trunk
copy .\megui\trunk\Avisynthwrapper.dll .\TestSuite\trunk

cd TestSuite\trunk
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /r:megui.exe /r:nunit.framework.dll /target:library /out:testsuite.dll /recurse:*.cs /warn:0
EXIT