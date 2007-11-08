// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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
	/// Summary description for x264farmSettings.
	/// </summary>
	[Serializable]
	public class x264farmSettings: VideoCodecSettings
	{
        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
            //if (QuantizerMatrixType == 2) // CQM
            //{
            //    if (substitutionTable.ContainsKey(QuantizerMatrix))
            //        QuantizerMatrix = substitutionTable[QuantizerMatrix];
            //}
        }
        public override string[] RequiredFiles
        {
            get
            {
                List<string> list = new List<string>(base.RequiredFiles);
                //if (QuantizerMatrixType == 2) // Custom profile
                //    list.Add(QuantizerMatrix);
                return list.ToArray();
            }
        }
        
        //string --firstavs, --fastavs, --config "config.xml", //iMPL in core: --zones ""
        string firstPassSettings, secondPassSettings, configPath, firstPassAVS, firstPassAVSFast;
        
        //int --preseek 0, --batch 5000, --split 250, --3gops 1073741823(???), --rerc 0, --seek 0, --frames 0, --qpmin 10, --qpmax 51
        //    --qpstep 4
        int preseekFrames, batchFPSize, splitFPFrames, gopsReEnc, reRateControlGopsReEnc, 
            x264Seek, x264Frames, x264MinQuantizer, x264MaxQuantizer, x264MaxQuantDelta;
                
        //decimal --thresh 20.0, --3thresh 0.8, --3ratio 0.05, --sizeprec 1.0,
        //        --qcomp 0.6, --qcomp 0.6, --cplxblur 20.0, --qblur 0.5, --ipratio 1.4, --pbratio 1.3

        decimal batchFPMulti, threshFPSplit, threshFPReSplit, threshTPAccuracy, ratioTPGopsReEnc, sizePrecision, x264QuantCompression, x264TempComplexityBlur, x264TempQuanBlurCC, x264IPFactor, x264PBFactor;

        //bool --force, --restart, --savedisk, --nocomp
        bool forceSPReEnc, restartTotal, savediskOnMerge, noCompression, configExt, gopsReEncDont, threshFPReSplitDont;


		#region constructor
        public override VideoCodec Codec
        {
            get { return VideoCodec.AVC; }
        }
        public override VideoEncoderType EncoderType
        {
            get { return VideoEncoderType.X264FARM; }
        }
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public x264farmSettings():base()
		{            
            firstPassSettings = "";
            secondPassSettings = "";            
            BitrateQuantizer = 1000;
            EncodingMode = 0;//0 - kbps, 1 - %
            //Mode.twopassAutomated = default and Only setting. to show in Jobs Properly
            //Mode.quality = 9
            //Mode.CBR = 0
            
            preseekFrames = 0;
            sizePrecision = new decimal(1.0);
            forceSPReEnc = false;
            restartTotal = false;
            savediskOnMerge = false;
            noCompression = false;
            configExt = false;
            configPath = "config.xml";
            firstPassAVS = "";
            firstPassAVSFast = "";
         
            batchFPSize = 5000;
            batchFPMulti = new decimal(0.5);
            splitFPFrames = 250;
            threshFPSplit = new decimal(20.0);
            threshFPReSplitDont = true;
            threshFPReSplit = new decimal(10.5);

            threshTPAccuracy = new decimal(0.8);
            gopsReEnc = 0;
            gopsReEncDont = true;
            ratioTPGopsReEnc = new decimal(0.05);
            reRateControlGopsReEnc = 0;
                        
            x264Seek = 0;
            x264Frames = 0;
            x264MinQuantizer = 10;
            x264MaxQuantizer = 51;
            x264MaxQuantDelta = 4;
            x264QuantCompression = new decimal(0.6);
            x264TempComplexityBlur = 20;
            x264TempQuanBlurCC = new decimal(0.5);
            x264IPFactor = new decimal(1.4);
            x264PBFactor = new decimal(1.3);           
            
		}
		#endregion
		#region properties                
        public string FirstPassSettings
        {
            get { return firstPassSettings; }
            set { firstPassSettings = value; }
        }
        public string SecondPassSettings
        {
            get { return secondPassSettings; }
            set { secondPassSettings = value; }
        }
        public string FirstPassAVS
        {
            get { return firstPassAVS; }
            set { firstPassAVS = value; }
        }
        public string FirstPassAVSFast
        {
            get { return firstPassAVSFast; }
            set { firstPassAVSFast = value; }
        }
        public int PreseekFrames
        {
            get { return preseekFrames; }
            set { preseekFrames = value; }
        }
        
        public bool ForceSPReEnc
        {
            get { return forceSPReEnc; }
            set { forceSPReEnc = value; }
        }
        
        public bool RestartTotal
        {
            get { return restartTotal; }
            set { restartTotal = value; }
        }
        
        public bool SavediskOnMerge
        {
            get { return savediskOnMerge; }
            set { savediskOnMerge = value; }
        }
        
        public bool NoCompression
        {
            get { return noCompression; }
            set { noCompression = value; }
        }
        
        public string ConfigPath
        {
            get { return configPath; }
            set { configPath = value; }
        }

        public bool ConfigExt
        {
            get { return configExt; }
            set { configExt = value; }
        }    
        
        public int BatchFPSize
        {
            get { return batchFPSize; }
            set { batchFPSize = value; }
        }
        
        public decimal BatchFPMulti
        {
            get { return batchFPMulti; }
            set { batchFPMulti = value; }
        }
        
        public int SplitFPFrames
        {
            get { return splitFPFrames; }
            set { splitFPFrames = value; }
        }
        
        public decimal ThreshFPSplit
        {
            get { return threshFPSplit; }
            set { threshFPSplit = value; }
        }

        public bool ThreshFPReSplitDont
        {
            get { return threshFPReSplitDont; }
            set { threshFPReSplitDont = value; }
        }

        public decimal ThreshFPReSplit
        {
            get { return threshFPReSplit; }
            set { threshFPReSplit = value; }
        }
        
        public decimal ThreshTPAccuracy
        {
            get { return threshTPAccuracy; }
            set { threshTPAccuracy = value; }
        }        
        
        public int GopsReEnc
        {
            get { return gopsReEnc; }
            set { gopsReEnc = value; }
        }

        public bool GopsReEncDont
        {
            get { return gopsReEncDont; }
            set { gopsReEncDont = value; }
        }
        
        public decimal RatioTPGopsReEnc
        {
            get { return ratioTPGopsReEnc; }
            set { ratioTPGopsReEnc = value; }
        }
        
        public int ReRateControlGopsReEnc
        {
            get { return reRateControlGopsReEnc; }
            set { reRateControlGopsReEnc = value; }
        }
        
        public decimal SizePrecision
        {
            get { return sizePrecision; }
            set { sizePrecision = value; }
        }

        public int X264Seek
        {
            get { return x264Seek; }
            set { x264Seek = value; }
        }
        
        public int X264Frames
        {
            get { return x264Frames; }
            set { x264Frames = value; }
        }
        
        public int X264MinQuantizer
        {
            get { return x264MinQuantizer; }
            set { x264MinQuantizer = value; }
        }
        
        public int X264MaxQuantizer
        {
            get { return x264MaxQuantizer; }
            set { x264MaxQuantizer = value; }
        }
        
        public int X264MaxQuantDelta
        {
            get { return x264MaxQuantDelta; }
            set { x264MaxQuantDelta = value; }
        }
        
        public decimal X264QuantCompression
        {
            get { return x264QuantCompression; }
            set { x264QuantCompression = value; }
        }
        
        public decimal X264TempComplexityBlur
        {
            get { return x264TempComplexityBlur; }
            set { x264TempComplexityBlur = value; }
        }
        
        public decimal X264TempQuanBlurCC
        {
            get { return x264TempQuanBlurCC; }
            set { x264TempQuanBlurCC = value; }
        }
        
        public decimal X264IPFactor
        {
            get { return x264IPFactor; }
            set { x264IPFactor = value; }
        }

        public decimal X264PBFactor
        {
            get { return x264PBFactor; }
            set { x264PBFactor = value; }
        }		
		#endregion
        
        public override bool UsesSAR
        {
            get { return true; }
        }
        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two x264farmSettings instances        
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public bool IsAltered(x264farmSettings otherSettings)
        {
            if (
                this.BitrateQuantizer != otherSettings.BitrateQuantizer ||
                this.EncodingMode != otherSettings.EncodingMode ||
                this.FirstPassSettings != otherSettings.FirstPassSettings ||
                this.SecondPassSettings != otherSettings.SecondPassSettings ||
                this.FirstPassAVS != otherSettings.FirstPassAVS ||
                this.FirstPassAVSFast != otherSettings.FirstPassAVSFast ||
                this.PreseekFrames != otherSettings.PreseekFrames ||
                this.ForceSPReEnc != otherSettings.ForceSPReEnc ||
                this.RestartTotal != otherSettings.RestartTotal ||
                this.SavediskOnMerge != otherSettings.SavediskOnMerge ||
                this.NoCompression != otherSettings.NoCompression ||
                this.ConfigPath != otherSettings.ConfigPath ||
                this.ConfigExt != otherSettings.ConfigExt ||
                this.BatchFPSize != otherSettings.BatchFPSize ||
                this.SplitFPFrames != otherSettings.SplitFPFrames ||
                this.ThreshFPSplit != otherSettings.ThreshFPSplit ||
                this.ThreshTPAccuracy != otherSettings.ThreshTPAccuracy ||
                this.GopsReEnc != otherSettings.GopsReEnc ||
                this.RatioTPGopsReEnc != otherSettings.RatioTPGopsReEnc ||
                this.ReRateControlGopsReEnc != otherSettings.ReRateControlGopsReEnc ||
                this.SizePrecision != otherSettings.SizePrecision ||
                this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
                this.Zones != otherSettings.Zones ||
                this.X264Seek != otherSettings.X264Seek ||
                this.X264Frames != otherSettings.X264Frames ||
                this.X264MinQuantizer != otherSettings.X264MinQuantizer ||
                this.X264MaxQuantizer != otherSettings.X264MaxQuantizer ||
                this.X264MaxQuantDelta != otherSettings.X264MaxQuantDelta ||
                this.X264QuantCompression != otherSettings.X264QuantCompression ||
                this.X264TempComplexityBlur != otherSettings.X264TempComplexityBlur ||
                this.X264TempQuanBlurCC != otherSettings.X264TempQuanBlurCC ||
                this.X264IPFactor != otherSettings.X264IPFactor ||
                this.X264PBFactor != otherSettings.X264PBFactor ||
                this.Logfile != otherSettings.Logfile
                )
                return true;
            else
                return false;
        }
	}
}
