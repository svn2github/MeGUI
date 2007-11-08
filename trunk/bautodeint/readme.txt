BAutoDeint:

A standalone program for interlace detection of AviSynth files. Usage is documented within the program.

Compilation:

bautodeint compiles under DMD 2.003, which you can get from http://www.digitalmars.com/d/
More recent versions of DMD may work, but are not guaranteed to. To compile, DMD must be located somewhere in your PATH. Then, you simply run compile.bat, which will generate bautodeint.exe. The run the unittests, run

    compile.bat test

If there are no lines with AssertError, they have run successfully.



 -- berrinam