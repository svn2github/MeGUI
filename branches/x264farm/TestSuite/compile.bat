cd ..
copy megui\megui.exe TestSuite
copy megui\icsharpcode.sharpziplib.dll TestSuite
copy megui\mediainfowrapper.dll TestSuite
copy megui\MessageBoxExLib.dll TestSuite
copy megui\Avisynthwrapper.dll TestSuite

cd TestSuite
%windir%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86 /r:megui.exe /r:nunit.framework.dll /target:library /out:testsuite.dll /recurse:*.cs /warn:0
EXIT