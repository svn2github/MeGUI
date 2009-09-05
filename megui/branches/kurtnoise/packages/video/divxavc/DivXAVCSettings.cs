// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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

namespace MeGUI
{
    /// <summary>
    /// Summary description for divxAVC Settings.
    /// </summary>
    [Serializable]
    public class DivXAVCSettings : VideoCodecSettings
    {
        public static string ID = "DivX264";

        private int aqo, gopLength, interlaceMode, maxRefFrames, maxBFrames;
        private bool pyramid, basref;

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override bool UsesSAR
        {
            get { return true; }
        }
        /// <summary>
        /// default constructor
        /// initializes all the variables at the codec's default (based on the xvid VfW defaults
        /// </summary>
        public DivXAVCSettings()
            : base(ID, VideoEncoderType.DIVXAVC)
        {
            EncodingMode = 2;
            aqo = 1;
            gopLength = 4;
            interlaceMode = 0;
            maxRefFrames = 4;
            maxBFrames = 2;
            pyramid = false;
            basref = false;
            Turbo = true;
            base.MaxNumberOfPasses = 2;
        }

       /// <summary>
        ///  Handles assessment of whether the encoding options vary between two xvidSettings instances
        /// The following are excluded from the comparison:
        /// BitrateQuantizer
        /// CreditsQuantizer
        /// Logfile
        /// PAR
        /// PARs
        /// SARX
        /// SARY
        /// Zones
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public bool IsAltered(VideoCodecSettings settings)
        {
            if (!(settings is DivXAVCSettings))
                return true;
            DivXAVCSettings otherSettings = (DivXAVCSettings)settings;
            if (
                    this.EncodingMode != otherSettings.EncodingMode ||
                    this.AQO != otherSettings.AQO ||
                    this.GOPLength != otherSettings.GOPLength ||
                    this.MaxBFrames != otherSettings.MaxBFrames ||
                    this.InterlaceMode != otherSettings.InterlaceMode ||
                    this.MaxRefFrames != otherSettings.MaxRefFrames ||
                    this.Pyramid != otherSettings.Pyramid ||
                    this.BasRef != otherSettings.BasRef ||
                    this.Turbo != otherSettings.Turbo
               )
                return true;
            else
                return false;
        }

        public int AQO
        {
            get { return aqo; }
            set { aqo = value; }
        }

        public int GOPLength
        {
            get { return gopLength; }
            set { gopLength = value; }
        }

        public int InterlaceMode
        {
            get { return interlaceMode; }
            set { interlaceMode = value; }
        }

        public int MaxBFrames
        {
            get { return maxBFrames; }
            set { maxBFrames = value; }
        }

        public int MaxRefFrames
        {
            get { return maxRefFrames; }
            set { maxRefFrames = value; }
        }

        public bool Pyramid
        {
            get { return pyramid; }
            set { pyramid = value; }
        }

        public bool BasRef
        {
            get { return basref; }
            set { basref = value; }
        }

        public void doTriStateAdjustment()
        {
            if (EncodingMode != 0)
                Turbo = false;
            if (Turbo)
            {
                Pyramid = false;
                BasRef = false;
                AQO = 0;
                MaxRefFrames = 1;
                MaxBFrames = 0;
            }
        }
    }
}
