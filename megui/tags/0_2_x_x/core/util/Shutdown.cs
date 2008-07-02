using System;
using System.Runtime.InteropServices;

namespace MeGUI
{
	/// <summary>
	/// Summary description for Shutdown.
	/// </summary>
	public class Shutdown
	{
		[StructLayout(LayoutKind.Sequential, Pack=1)]
			internal struct TokPriv1Luid
		{
			public int  Count;
			public long  Luid;
			public int  Attr;
		}

		[DllImport("kernel32.dll", ExactSpelling=true) ]
		internal static extern IntPtr GetCurrentProcess();

		[DllImport("advapi32.dll", ExactSpelling=true, SetLastError=true) ]
		internal static extern bool OpenProcessToken( IntPtr h, int acc, ref IntPtr phtok );

		[DllImport("advapi32.dll", SetLastError=true) ]
		internal static extern bool LookupPrivilegeValue( string host, string name, ref long pluid );

		[DllImport("advapi32.dll", ExactSpelling=true, SetLastError=true) ]
		internal static extern bool AdjustTokenPrivileges( IntPtr htok, bool disall,
			ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen );

		[DllImport("user32.dll", ExactSpelling=true, SetLastError=true) ]
		internal static extern bool ExitWindowsEx( int flg, uint rea );

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int TOKEN_QUERY = 0x00000008;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        private const int EWX_SHUTDOWN = 0x00000001;
        private const int EWX_REBOOT = 0x00000002;
        private const int EWX_FORCE = 0x00000004;
        private const int EWX_POWEROFF = 0x00000008;
        private const int EWX_FORCEIFHUNG = 0x00000010;

        private const uint SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;
        private const uint SHTDN_REASON_MAJOR_APPLICATION = 0x00040000;
        private const uint SHTDN_REASON_MAJOR_SYSTEM = 0x00050000;

        private const uint SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001;
        private const uint SHTDN_REASON_MINOR_NONE = 0x000000ff;
        private const uint SHTDN_REASON_MINOR_UPGRADE = 0x00000003;

        private const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;


		public Shutdown()
		{
		}

		public static bool shutdown()
		{
			bool success;
			TokPriv1Luid tp;
			IntPtr hproc = GetCurrentProcess();
			IntPtr htok = IntPtr.Zero;
			success = OpenProcessToken( hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok );
			tp.Count = 1;
			tp.Luid = 0;
			tp.Attr = SE_PRIVILEGE_ENABLED;
			success = LookupPrivilegeValue( null, SE_SHUTDOWN_NAME, ref tp.Luid );
			success = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero );
            return ExitWindowsEx(EWX_SHUTDOWN + EWX_FORCE, SHTDN_REASON_MAJOR_APPLICATION | SHTDN_REASON_MINOR_NONE | SHTDN_REASON_FLAG_PLANNED);
		}
	}
}