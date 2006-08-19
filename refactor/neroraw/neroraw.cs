using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("neroraw")]
[assembly: AssemblyDescription("encodes RAW PCM data from stdin using Nero AAC")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("www.doom9.net")]
[assembly: AssemblyCopyright("Licensed under the GPL")]
[assembly: AssemblyProduct("neroraw")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("0.0.2.1")]
[assembly: AssemblyFileVersion("0.0.2.1")]

namespace MeGUI
{
    sealed class NeroRawAacEncoder
    {
        [DllImport("bsn.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = false)]
        private static extern int BSN_Init(ref int nSampleRate, int nChannelsCount, string sOutFileName, int nBitsPerSample, int nShowDialog, string path, string[] argv, int argc);

        [DllImport("bsn.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        private static extern void BSN_DeInit();

        [DllImport("bsn.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        private static extern int BSN_EncodeBlock(IntPtr pBuf, uint blockSizeRead);

        private static int showUsage()
        {
            Console.WriteLine("usage: neroraw [-i input] -o output -rr samplerate -rb bitsPerSample -rc channelsCount <encoder options>");
            Console.WriteLine("if -i input is omited stdin is used");
            Console.WriteLine("for <encoder options> look @ bsn switch documentation in besweet");
            Console.WriteLine("bsn.dll from besweet must be @ executable folder");
            return -1;
        }

        [STAThread]
        public static int Main(string[] args)
        {
            killRegistryKey();

            try
            {

                string inputFileName = null;
                string outputFileName = null;
                int nSampleRate = 44100;
                int nBitsPerSample = 16;
                int nChannelsCount = 2;
                int nShowDialog = 0;
                ArrayList arr = new ArrayList();
                arr.Add("-bsn(");
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-d":
                            nShowDialog = int.Parse(args[++i]);
                            continue;
                        case "-i":
                            inputFileName = args[++i];
                            continue;
                        case "-o":
                            outputFileName = args[++i];
                            continue;
                        case "-rr":
                            nSampleRate = int.Parse(args[++i]);
                            continue;
                        case "-rb":
                            nBitsPerSample = int.Parse(args[++i]);
                            continue;
                        case "-rc":
                            nChannelsCount = int.Parse(args[++i]);
                            continue;
                        default:
                            arr.Add(args[i]);
                            break;
                    }
                }
                arr.Add(")");

                if (outputFileName == null || 0 == outputFileName.Length)
                    return showUsage();

                string[] array = (string[])arr.ToArray(typeof(string));
                int nRes;
                try
                {
                    if (0 != (nRes = BSN_Init(ref nSampleRate, nChannelsCount, outputFileName, nBitsPerSample, nShowDialog, null, array, array.Length)))
                        throw new ApplicationException(string.Format("Error configuring bsn {0}", nRes));

                    const int MAX_BUFFER_PER_ONCE = 0x10000;
                    int nBufSize = MAX_BUFFER_PER_ONCE - MAX_BUFFER_PER_ONCE % (nBitsPerSample * nChannelsCount / 8);
                    byte[] buf = new byte[nBufSize];
                    uint bytesRead = 0;
                    GCHandle h = GCHandle.Alloc(buf, GCHandleType.Pinned);
                    try
                    {
                        Stream s = inputFileName == null ? Console.OpenStandardInput() : new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                        try
                        {

                            while (0 != (bytesRead = (uint)s.Read(buf, 0, nBufSize)))
                            {
                                BSN_EncodeBlock(h.AddrOfPinnedObject(), bytesRead);
                            }
                        }
                        finally
                        {
                            if (inputFileName != null)
                                (s as IDisposable).Dispose();
                        }
                    }
                    finally
                    {
                        h.Free();
                    }
                }
                finally
                {
                    try
                    {
                        BSN_DeInit();
                    }
                    catch { };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }

            killRegistryKey();

            return 0;
        }

        private static void killRegistryKey()
        {
            try
            {
                Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(".zhdb");
            }
            catch { };
        }

    }
}
