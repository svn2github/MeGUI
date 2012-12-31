// ****************************************************************************
// 
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class x264Device
    {
        private string strName;
        private int iID, iProfile, iLevel, iVBVBufsize, iVBVMaxrate, iBframes, iReframes, iMaxWidth, iMaxHeight, iMaxGop, iBPyramid;
        private bool bBluRay;

        public static List<x264Device> CreateDeviceList()
        {
            List<x264Device> x264DeviceList = new List<x264Device>();
            x264DeviceList.Add(new x264Device(0, "Default", -1, 15, -1, -1, -1, -1, -1, -1));
            x264DeviceList.Add(new x264Device(1, "Android G1", 0, 7, 2500, 2500, 0, -1, 480, 368));
            x264DeviceList.Add(new x264Device(2, "AVCHD", 2, 11, 14000, 14000, 3, 6, 1920, 1080));
            x264DeviceList.Add(new x264Device(3, "Blu-ray", 2, 11, 30000, 40000, 3, 6, 1920, 1080));
            x264DeviceList.Add(new x264Device(4, "DivX Plus HD", 2, 10, 25000, 20000, 3, -1, 1920, 1080));
            x264DeviceList.Add(new x264Device(5, "DXVA", 2, 11, -1, -1, -1, -1, -1, -1));
            x264DeviceList.Add(new x264Device(6, "iPad", 2, 8, -1, -1, -1, -1, 1024, 768));
            x264DeviceList.Add(new x264Device(7, "iPhone", 0, 7, 10000, 10000, 0, -1, 480, 320));
            x264DeviceList.Add(new x264Device(8, "iPhone 4", 1, 8, -1, -1, -1, -1, 960, 640));
            x264DeviceList.Add(new x264Device(9, "iPod", 0, 7, 10000, 10000, 0, 5, 320, 240));
            x264DeviceList.Add(new x264Device(10, "Nokia N8", 2, 8, -1, -1, -1, -1, 640, 360));
            x264DeviceList.Add(new x264Device(11, "Nokia N900", 0, 7, -1, -1, -1, -1, 800, 480));
            x264DeviceList.Add(new x264Device(12, "PS3", 2, 11, 31250, 31250, -1, -1, 1920, 1080));
            x264DeviceList.Add(new x264Device(13, "PSP", 1, 7, 10000, 10000, -1, 3, 480, 272));
            x264DeviceList.Add(new x264Device(14, "Xbox 360", 2, 11, 24000, 24000, 3, 3, 1920, 1080));
            x264DeviceList.Add(new x264Device(15, "WDTV", 2, 11, -1, -1, -1, -1, 1920, 1080));
            x264DeviceList[2].MaxGOP = 1;
            x264DeviceList[2].BluRay = true;
            x264DeviceList[3].MaxGOP = 1;
            x264DeviceList[3].BluRay = true;
            x264DeviceList[4].MaxGOP = 4;
            x264DeviceList[13].BPyramid = 0;
            return x264DeviceList;
        }

        public x264Device(int iID, string strName, int iProfile, int iLevel, int iVBVBufsize, int iVBVMaxrate, int iBframes, int iReframes, int iMaxWidth, int iMaxHeight)
        {
            this.iID = iID;
            this.strName = strName;
            this.iProfile = iProfile;
            this.iLevel = iLevel;
            this.iVBVBufsize = iVBVBufsize;
            this.iVBVMaxrate = iVBVMaxrate;
            this.iBframes = iBframes;
            this.iReframes = iReframes;
            this.iMaxWidth = iMaxWidth;
            this.iMaxHeight = iMaxHeight;
            this.bBluRay = false;
            this.iBPyramid = -1;
            this.iMaxGop = -1;
        }

        public int ID
        {
            get { return iID; }
        }

        public string Name
        {
            get { return strName; }
        }

        public int Profile
        {
            get { return iProfile; }
        }

        public int Level
        {
            get { return iLevel; }
        }

        public int VBVBufsize
        {
            get { return iVBVBufsize; }
        }

        public int VBVMaxrate
        {
            get { return iVBVMaxrate; }
        }

        public int BFrames
        {
            get { return iBframes; }
        }

        public int ReferenceFrames
        {
            get { return iReframes; }
        }

        public int Height
        {
            get { return iMaxHeight; }
        }

        public int Width
        {
            get { return iMaxWidth; }
        }

        public int MaxGOP
        {
            get { return iMaxGop; }
            set { iMaxGop = value; }
        }

        public bool BluRay
        {
            get { return bBluRay; }
            set { bBluRay = value; }
        }

        public int BPyramid
        {
            get { return iBPyramid; }
            set { iBPyramid = value;}
        }

        public override string ToString()
        {
            return strName;
        }
    }
}