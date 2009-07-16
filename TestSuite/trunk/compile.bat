CD ..
COPY .\megui\trunk\megui.exe .\TestSuite\trunk
COPY .\megui\trunk\icsharpcode.sharpziplib.dll .\TestSuite\trunk
COPY .\megui\trunk\mediainfowrapper.dll .\TestSuite\trunk
COPY .\megui\trunk\MessageBoxExLib.dll .\TestSuite\trunk
COPY .\megui\trunk\Avisynthwrapper.dll .\TestSuite\trunk

CD TestSuite\trunk
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /r:megui.exe /r:nunit.framework.dll /target:library /out:testsuite.dll /recurse:*.cs /warn:0
EXIT