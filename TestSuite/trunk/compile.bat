@ECHO OFF
CD ..
COPY megui\trunk\megui.exe TestSuite\trunk /Y >NUL
COPY megui\trunk\icsharpcode.sharpziplib.dll TestSuite\trunk /Y >NUL
COPY megui\trunk\mediainfowrapper.dll TestSuite\trunk /Y >NUL
COPY megui\trunk\MessageBoxExLib.dll TestSuite\trunk /Y >NUL
COPY megui\trunk\Avisynthwrapper.dll TestSuite\trunk /Y >NUL

CD TestSuite\trunk
%WINDIR%\Microsoft.NET\Framework\v2.0.50727\csc /platform:x86^
 /r:megui.exe /r:nunit.framework.dll /target:library /out:testsuite.dll^
 /recurse:*.cs /warn:0

EXIT