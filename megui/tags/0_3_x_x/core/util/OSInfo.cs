// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MeGUI
{
    /// <summary>
    /// OSInfo Class based from http://www.codeproject.com/csharp/osversion_producttype.asp
    /// </summary>
    public class OSInfo
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        #region Private Constants
        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the product type of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system product type.</returns>
        public static string GetOSProductType()
        {
            PlatformID id = Environment.OSVersion.Platform;
            if (id == PlatformID.WinCE || id == PlatformID.Win32Windows || id == PlatformID.Win32S || id == PlatformID.Win32NT)
                return GetWindowsProductType();
            else
                return GetGenericProductType();
        }

        private static string GetGenericProductType()
        {
            return Environment.OSVersion.VersionString;
        }

        private static string GetWindowsProductType()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
            OperatingSystem osInfo = Environment.OSVersion;

            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            if (!GetVersionEx(ref osVersionInfo))
            {
                return "";
            }
            else
            {
                if (osInfo.Version.Major == 4)
                {
                    if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                    {
                        // Windows NT 4.0 Workstation
                        return " Workstation";
                    }
                    else if (osVersionInfo.wProductType == VER_NT_SERVER)
                    {
                        // Windows NT 4.0 Server
                        return " Server";
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (osInfo.Version.Major == 5)
                {
                    if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                    {
                        if ((osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) == VER_SUITE_PERSONAL)
                        {
                            // Windows XP Home Edition
                            return " Home Edition";
                        }
                        else
                        {
                            // Windows XP / Windows 2000 Professional
                            return " Professional";
                        }
                    }
                    else if (osVersionInfo.wProductType == VER_NT_SERVER)
                    {
                        if (osInfo.Version.Minor == 0)
                        {
                            if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                            {
                                // Windows 2000 Datacenter Server
                                return " Datacenter Server";
                            }
                            else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                            {
                                // Windows 2000 Advanced Server
                                return " Advanced Server";
                            }
                            else
                            {
                                // Windows 2000 Server
                                return " Server";
                            }
                        }
                        else
                        {
                            if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                            {
                                // Windows Server 2003 Datacenter Edition
                                return " Datacenter Edition";
                            }
                            else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                            {
                                // Windows Server 2003 Enterprise Edition
                                return " Enterprise Edition";
                            }
                            else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) == VER_SUITE_BLADE)
                            {
                                // Windows Server 2003 Web Edition
                                return " Web Edition";
                            }
                            else
                            {
                                // Windows Server 2003 Standard Edition
                                return " Standard Edition";
                            }
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Returns the service pack information of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the operating system service pack information.</returns>
        public static string GetOSServicePack()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            if (!GetVersionEx(ref osVersionInfo))
            {
                return "";
            }
            else
            {
                if (osVersionInfo.szCSDVersion != "")
                {
                    return " SP" + osVersionInfo.szCSDVersion.Substring(13, 1);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Returns the name of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system name.</returns>
        public static string GetOSName()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            string osName = "UNKNOWN";

            switch (osInfo.Platform)
            {
                case PlatformID.Win32Windows:
                    {
                        switch (osInfo.Version.Minor)
                        {
                            case 0:
                                {
                                    osName = "Windows 95";
                                    break;
                                }

                            case 10:
                                {
                                    if (osInfo.Version.Revision.ToString() == "2222A")
                                    {
                                        osName = "Windows 98 Second Edition";
                                    }
                                    else
                                    {
                                        osName = "Windows 98";
                                    }
                                    break;
                                }

                            case 90:
                                {
                                    osName = "Windows Me";
                                    break;
                                }
                        }
                        break;
                    }

                case PlatformID.Win32NT:
                    {
                        switch (osInfo.Version.Major)
                        {
                            case 3:
                                {
                                    osName = "Windows NT 3.51";
                                    break;
                                }

                            case 4:
                                {
                                    osName = "Windows NT 4.0";
                                    break;
                                }

                            case 5:
                                {
                                    if (osInfo.Version.Minor == 0)
                                    {
                                        osName = "Windows 2000";
                                    }
                                    else if (osInfo.Version.Minor == 1)
                                    {
                                        osName = "Windows XP";
                                    }
                                    else if (osInfo.Version.Minor == 2)
                                    {
                                        osName = "Windows Server 2003";
                                    }
                                    break;
                                }

                            case 6:
                                {
                                    osName = "Windows Vista";
                                    break;
                                }
                        }
                        break;
                    }
            }

            return osName;
        }

        /// <summary>
        /// Returns the name of the dotNet Framework running on this computer.
        /// </summary>
        /// <returns>A string containing the Name of the Framework Version.</returns>
        /// 
        public static string FormatDotNetVersion()
        {
            string fv = "unknown";
            Version clr = Environment.Version;

            switch (clr.Major)
            {
                case 1:
                    {
                        switch (clr.Minor)
                        {
                            case 0:
                                {
                                    if (clr.Revision.ToString()  == "209")
                                    {
                                        fv = "1.0 SP1";
                                        break;
                                    }
                                    else if (clr.Revision.ToString() == "288")
                                    {
                                        fv = "1.0 SP2";
                                        break;
                                    }
                                    else if (clr.Revision.ToString() == "6018")
                                    {
                                        fv = "1.0 SP3";
                                        break;
                                    }
                                    else
                                    {
                                        fv = "1.0";
                                        break;
                                    }
                                }
                            case 1:
                                {
                                    if (clr.Revision.ToString() == "2032" || clr.Revision.ToString() == "2300")
                                    {
                                        fv = "1.1 SP1";
                                        break;
                                    }
                                    else
                                    {
                                        fv = "1.1";
                                        break;
                                    }
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        if (clr.Revision.ToString() == "42" ||
                            clr.Revision.ToString() == "312" || // Vista
                            clr.Revision.ToString() == "832")   // KB928365
                        {
                            fv = "2.0";
                            break;
                        }
                        else
                        {
                            fv = "2.0 SP1";
                            break;
                        }
                    }
                case 3:
                    {
                        switch (clr.Minor)
                        {
                            case 0:
                                {
                                    if (clr.Revision.ToString() == "26" || // Vista
                                        clr.Revision.ToString() == "30")
                                    {
                                        fv = "3.0";
                                        break;
                                    }
                                    else
                                    {
                                        fv = "3.0 SP1";
                                        break;
                                    }
                                }
                            case 5:
                                {
                                    if (clr.Revision.ToString() == "08")
                                    {
                                        fv = "3.5";
                                        break;
                                    }
                                }
                                break;
                        }
                        break;
                    }
            }

            return fv;
        }

        /// <summary>
        /// Get some stuff from the Management Object Queries
        /// </summary>
        /// <returns>A string containing the result of the MO query.</returns>
        /// 
        public static string GetMOStuff(string queryObject)
        {
            ManagementObjectSearcher searcher = null;
            string res = "";
            try
            {
               searcher = new ManagementObjectSearcher("SELECT * FROM " + queryObject);
               foreach (ManagementObject mo in searcher.Get())
                {
                    if (queryObject == "Win32_OperatingSystem")
                    {
                        res = mo["Caption"].ToString();
                    }
                    else if (queryObject == "Win32_Processor")
                    {
                        res = mo["Name"].ToString();
                    }
                    else if (queryObject == "Win32_LogicalDisk")
                    {
                        if (mo["DriveType"].ToString() == "3") // HDD
                        {
                            long freespace = long.Parse(mo["FreeSpace"].ToString()) / 1073741824;
                            long totalsize = long.Parse(mo["Size"].ToString()) / 1073741824;

                            if (mo["VolumeName"].ToString() == "")
                                mo["VolumeName"] = "Local Disk";

                            res += mo["VolumeName"].ToString() + " (" + mo["Name"].ToString() + ")  -  " + Convert.ToString(freespace) + " Go free of " + Convert.ToString(totalsize) + " Go\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return res;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the full version of the operating system running on this computer.
        /// </summary>
        public static string OSVersion
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }

        /// <summary>
        /// Gets the major version of the operating system running on this computer.
        /// </summary>
        public static int OSMajorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Major;
            }
        }

        /// <summary>
        /// Gets the minor version of the operating system running on this computer.
        /// </summary>
        public static int OSMinorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Minor;
            }
        }

        /// <summary>
        /// Gets the build version of the operating system running on this computer.
        /// </summary>
        public static int OSBuildVersion
        {
            get
            {
                return Environment.OSVersion.Version.Build;
            }
        }

        /// <summary>
        /// Gets the revision version of the operating system running on this computer.
        /// </summary>
        public static int OSRevisionVersion
        {
            get
            {
                return Environment.OSVersion.Version.Revision;
            }
        }
        #endregion
    }
}
