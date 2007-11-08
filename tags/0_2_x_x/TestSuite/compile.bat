cd ..
copy trunk\megui.exe TestSuite
copy trunk\icsharpcode.sharpziplib.dll TestSuite
copy trunk\mediainfowrapper.dll TestSuite
copy trunk\MessageBoxExLib.dll TestSuite
copy trunk\Avisynthwrapper.dll TestSuite

cd TestSuite
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /r:megui.exe /r:nunit.framework.dll /target:library /out:testsuite.dll /recurse:*.cs /warn:0
EXIT