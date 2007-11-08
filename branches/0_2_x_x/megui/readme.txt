THIS VERSION IS NOWHERE NEAR STABLE


This refactor is just a beginning. It compiles at the moment, but it isn't guaranteed to work. The goals of this refactor are to become package-oriented and to reorganise the folders to make light of that. It also aims to reduce the amount of code in Form1.cs, which has already been partly achieved by creating a JobControl, and audio- and video- encoding components.

The package system is made up as follows:
IPackage: contains everything a package may conatin. A package should contain everything that is interdependent, but for one-way dependencies or tools which could generally be useful, then it should just list which ones are required. Dependencies are still to be worked out, and the initial version will statically register all the packages anyway, so dependencies aren't a problem. The components of packages are listed below:
-ITool: this is something which is in the Tools menu at the moment. It may do whatever it wants, as it is just a void run(MainForm) command
-IMediaFileFactor: this is for sourcing video files. At the moment, there are three of these: AvsFileFactory, D2VFileFactory, MediaInfoFileFactory
-JobPre/PostProcessor: These are for any custom steps required, to stop them being built in so uglily. Before and after every job, JobControl calls every pre/postprocessor registered, and it is their job to determine whether they must do anything.
-IVideoSettingsProvider: for setting up video codecs. Has been around for ages
-IAudioSettingsProvider: for setting up audio codecs. Also has been around for ages
-IMuxing: for setting up muxers. Has been around for some time

All aspects of the IPackage system are already partly set up, but a lot more reorganising, testing is required first. 