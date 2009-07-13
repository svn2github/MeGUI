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
	/// Summary description for x264Settings.
	/// </summary>
	[Serializable]
	public class x264Settings: VideoCodecSettings
	{
        public static string ID = "x264";

        public override void setAdjustedNbThreads(int nbThreads)
        {
            base.setAdjustedNbThreads(0);
        }

        public override void FixFileNames(System.Collections.Generic.Dictionary<string, string> substitutionTable)
        {
            base.FixFileNames(substitutionTable);
            if (QuantizerMatrixType == 2) // CQM
            {
                if (substitutionTable.ContainsKey(QuantizerMatrix))
                    QuantizerMatrix = substitutionTable[QuantizerMatrix];
            }
        }
        public override string[] RequiredFiles
        {
            get
            {
                List<string> list = new List<string>(base.RequiredFiles);
                if (QuantizerMatrixType == 2) // Custom profile
                    list.Add(QuantizerMatrix);
                return list.ToArray();
            }
        }
        int NewadaptiveBFrames, nbRefFrames, alphaDeblock, betaDeblock, subPelRefinement, maxQuantDelta, tempQuantBlur, 
			bframePredictionMode, vbvBufferSize, vbvMaxBitrate, meType, meRange, minGOPSize, macroBlockOptions,
			quantizerMatrixType, profile, x264Trellis, level, noiseReduction, deadZoneInter, deadZoneIntra, AQMode;
		decimal ipFactor, pbFactor, chromaQPOffset, vbvInitialBuffer, bitrateVariance, quantCompression, 
			tempComplexityBlur, tempQuanBlurCC, scdSensitivity, bframeBias, quantizerCrf, AQStrength, psyRDO, psyTrellis;
		bool deblock, cabac, p4x4mv, p8x8mv, b8x8mv, i4x4mv, i8x8mv, weightedBPrediction, encodeInterlaced,
			bFramePyramid, chromaME, adaptiveDCT, lossless, mixedRefs, NoFastPSkip, psnrCalc, noDctDecimate, ssimCalc, useQPFile, FullRange;
		string quantizerMatrix, qpfile;
		#region constructor
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public x264Settings():base(ID, VideoEncoderType.X264)
		{
            deadZoneInter = 21;
            deadZoneIntra = 11;
            encodeInterlaced = false;
			noFastPSkip = false;
            ssimCalc = false;
            psnrCalc = false;
			EncodingMode = 9;
			BitrateQuantizer = 23;
			KeyframeInterval = 250;
			nbRefFrames = 3;
			mixedRefs = true;
			NbBframes = 3;
			Turbo = false;
			deblock = true;
			alphaDeblock = 0;
			betaDeblock = 0;
			cabac = true;
			weightedBPrediction = true;
			NewadaptiveBFrames = 1;
			bFramePyramid = false;
			subPelRefinement = 6;
			psyRDO = new decimal(1.0);
            psyTrellis = new decimal(0.0);
            macroBlockOptions = 2;
            chromaME = true;
			p8x8mv = true;
			b8x8mv = true;
			p4x4mv = false;
			i4x4mv = true;
			i8x8mv = false;
			MinQuantizer = 10;
			MaxQuantizer = 51;
			maxQuantDelta = 4;
			CreditsQuantizer = new decimal(40);
			ipFactor = new decimal(1.4);
			pbFactor = new decimal(1.3);
			chromaQPOffset = new decimal(0.0);
			vbvBufferSize = -1;
			vbvMaxBitrate = -1;
			vbvInitialBuffer = new decimal(0.9);
			bitrateVariance = 1;
			quantCompression = new decimal(0.6);
			tempComplexityBlur = 20;
			tempQuanBlurCC = new decimal(0.5);
			bframePredictionMode = 1;
			scdSensitivity = new decimal(40);
			bframeBias = new decimal(0);
			meType = 1;
			meRange = 16;
			NbThreads = 0;
			minGOPSize = 25;
			adaptiveDCT = true;
			quantizerMatrix = "";
			quantizerMatrixType = 0; // none
			profile = 2; // high profile
			lossless = false;
			x264Trellis = 1;
			level = 15;
            base.MaxNumberOfPasses = 3;
            AQMode = 1;
            AQStrength = new decimal(1.0);
            useQPFile = false;
            qpfile = "";
            FullRange = false;
		}
		#endregion
		#region properties
        public decimal QuantizerCRF
        {
            get { return quantizerCrf; }
            set { quantizerCrf = value; }
        }
        public bool EncodeInterlaced
        {
            get { return encodeInterlaced; }
            set { encodeInterlaced = value; }
        }
        public bool NoDCTDecimate
        {
            get { return noDctDecimate; }
            set { noDctDecimate = value; }
        }

        public bool PSNRCalculation
        {
            get { return psnrCalc; }
            set { psnrCalc = value; }
        }
		public bool noFastPSkip
		{
			get {return NoFastPSkip;}
			set {NoFastPSkip = value;}
		}
		public int NoiseReduction
        {
            get { return noiseReduction; }
            set { noiseReduction = value; }
        }
        public bool MixedRefs
		{
			get {return mixedRefs;}
			set {mixedRefs = value;}
		}
		public int X264Trellis
		{
			get {return x264Trellis;}
			set {x264Trellis = value;}
		}
		public int NbRefFrames
		{
			get {return nbRefFrames;}
			set {nbRefFrames = value;}
		}
		public int AlphaDeblock
		{
			get {return alphaDeblock;}
			set {alphaDeblock = value;}
		}
		public int BetaDeblock
		{
			get {return betaDeblock;}
			set {betaDeblock = value;}
		}
		public int SubPelRefinement
		{
			get {return subPelRefinement;}
			set {subPelRefinement = value;}
		}
		public int MaxQuantDelta
		{
			get {return maxQuantDelta;}
			set {maxQuantDelta = value;}
		}
		public int TempQuantBlur
		{
			get {return tempQuantBlur;}
			set {tempQuantBlur = value;}
		}
		public int BframePredictionMode
		{
			get {return bframePredictionMode;}
			set {bframePredictionMode = value;}
		}
		public int VBVBufferSize
		{
			get {return vbvBufferSize;}
			set {vbvBufferSize = value;}
		}
		public int VBVMaxBitrate
		{
			get {return vbvMaxBitrate;}
			set {vbvMaxBitrate = value;}
		}
		public int METype
		{
			get {return meType;}
			set {meType = value;}
		}
		public int MERange
		{
			get {return meRange;}
			set {meRange = value;}
		}
		public int MinGOPSize
		{
			get {return minGOPSize;}
			set {minGOPSize = value;}
		}
		public int Profile
		{
			get {return profile;}
			set {profile = value;}
		}
		public int Level
		{
			get {return level;}
			set {level = value;}
		}
		public decimal IPFactor
		{
			get {return ipFactor;}
			set {ipFactor = value;}
		}
		public decimal PBFactor
		{
			get {return pbFactor;}
			set {pbFactor = value;}
		}
		public decimal ChromaQPOffset
		{
			get {return chromaQPOffset;}
			set {chromaQPOffset = value;}
		}
		public decimal VBVInitialBuffer
		{
			get {return vbvInitialBuffer;}
			set {vbvInitialBuffer = value;}
		}
		public decimal BitrateVariance
		{
			get {return bitrateVariance;}
			set {bitrateVariance = value;}
		}
		public decimal QuantCompression
		{
			get {return quantCompression;}
			set {quantCompression = value;}
		}
		public decimal TempComplexityBlur
		{
			get {return tempComplexityBlur;}
			set {tempComplexityBlur = value;}
		}
		public decimal TempQuanBlurCC
		{
			get {return tempQuanBlurCC;}
			set {tempQuanBlurCC = value;}
		}
		public decimal SCDSensitivity
		{
			get {return scdSensitivity;}
			set {scdSensitivity = value;}
		}
		public decimal BframeBias
		{
			get {return bframeBias;}
			set {bframeBias = value;}
		}
        public decimal PsyRDO
        {
            get { return psyRDO; }
            set { psyRDO = value; }
        }
        public decimal PsyTrellis
        {
            get { return psyTrellis; }
            set { psyTrellis = value; }
        }
		public bool Deblock
		{
			get {return deblock;}
			set {deblock = value;}
		}
		public bool Cabac
		{
			get {return cabac;}
			set {cabac = value;}
		}
        public bool UseQPFile
        {
            get { return useQPFile; }
            set { useQPFile = value; }
        }
		public bool WeightedBPrediction
		{
			get {return weightedBPrediction;}
			set {weightedBPrediction = value;}
		}
		public int NewAdaptiveBFrames
		{
			get {return NewadaptiveBFrames;}
			set {NewadaptiveBFrames = value;}
		}
		public bool BFramePyramid
		{
			get {return bFramePyramid;}
			set {bFramePyramid = value;}
		}
        public bool ChromaME
		{
			get {return chromaME;}
			set {chromaME = value;}
		}
        public int MacroBlockOptions
        {
            get { return macroBlockOptions; }
            set { macroBlockOptions = value; }
        }
        public bool P8x8mv
		{
			get {return p8x8mv;}
			set {p8x8mv = value;}
		}
		public bool B8x8mv
		{
			get {return b8x8mv;}
			set {b8x8mv = value;}
		}
		public bool I4x4mv
		{
			get {return i4x4mv;}
			set {i4x4mv = value;}
		}
		public bool I8x8mv
		{
			get {return i8x8mv;}
			set {i8x8mv = value;}
		}
		public bool P4x4mv
		{
			get {return p4x4mv;}
			set {p4x4mv = value;}
		}
		public bool AdaptiveDCT
		{
			get {return adaptiveDCT;}
			set {adaptiveDCT = value;}
		}
        public bool SSIMCalculation
        {
            get { return ssimCalc; }
            set { ssimCalc = value; }
        }
		public bool Lossless
		{
			get {return lossless;}
			set {lossless = value;}
		}
		public string QuantizerMatrix
		{
			get {return quantizerMatrix;}
			set {quantizerMatrix = value;}
		}
		public int QuantizerMatrixType
		{
			get {return quantizerMatrixType;}
			set {quantizerMatrixType = value;}
		}
        public int DeadZoneInter
        {
            get { return deadZoneInter; }
            set { deadZoneInter = value; }
        }
        public int DeadZoneIntra
        {
            get { return deadZoneIntra; }
            set { deadZoneIntra = value; }
        }
        public int AQmode
        {
            get { return AQMode; }
            set { AQMode = value; }
        }
        public decimal AQstrength
        {
            get { return AQStrength; }
            set { AQStrength = value; }
        }
        public string QPFile
        {
            get { return qpfile; }
            set { qpfile = value; }
        }
        public bool fullRange
        {
            get { return FullRange; }
            set { FullRange = value; }
        }
        #endregion
        public override bool UsesSAR
        {
            get { return true; }
        }
        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two x264Settings instances
        /// The following are excluded from the comparison:
        /// BitrateQuantizer
        /// CreditsQuantizer
        /// Logfile
        /// NbThreads
        /// SARX
        /// SARY
        /// Zones
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public bool IsAltered(x264Settings otherSettings)
        {
            if (
                this.NewAdaptiveBFrames != otherSettings.NewAdaptiveBFrames ||
                this.AdaptiveDCT != otherSettings.AdaptiveDCT ||
                this.AlphaDeblock != otherSettings.AlphaDeblock ||
                this.NoFastPSkip != otherSettings.NoFastPSkip ||
                this.B8x8mv != otherSettings.B8x8mv ||
                this.BetaDeblock != otherSettings.BetaDeblock ||
                this.BframeBias != otherSettings.BframeBias ||
                this.BframePredictionMode != otherSettings.BframePredictionMode ||
                this.BFramePyramid != otherSettings.BFramePyramid ||
                this.BitrateVariance != otherSettings.BitrateVariance ||
                this.PsyRDO != otherSettings.PsyRDO ||
                this.PsyTrellis != otherSettings.PsyTrellis ||
                this.Cabac != otherSettings.Cabac ||
                this.ChromaME != otherSettings.ChromaME ||
                this.ChromaQPOffset != otherSettings.ChromaQPOffset ||
                this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
                this.Deblock != otherSettings.Deblock ||
                this.EncodingMode != otherSettings.EncodingMode ||
                this.I4x4mv != otherSettings.I4x4mv ||
                this.I8x8mv != otherSettings.I8x8mv ||
                this.IPFactor != otherSettings.IPFactor ||
                this.KeyframeInterval != otherSettings.KeyframeInterval ||
                this.Level != otherSettings.Level ||
                this.Lossless != otherSettings.Lossless ||
                this.MaxQuantDelta != otherSettings.MaxQuantDelta ||
                this.MaxQuantizer != otherSettings.MaxQuantizer ||
                this.MERange != otherSettings.MERange ||
                this.METype != otherSettings.METype ||
                this.MinGOPSize != otherSettings.MinGOPSize ||
                this.MinQuantizer != otherSettings.MinQuantizer ||
                this.MixedRefs != otherSettings.MixedRefs ||
                this.NbBframes != otherSettings.NbBframes ||
                this.NbRefFrames != otherSettings.NbRefFrames ||
                this.noiseReduction != otherSettings.noiseReduction ||
                this.P4x4mv != otherSettings.P4x4mv ||
                this.P8x8mv != otherSettings.P8x8mv ||
                this.PBFactor != otherSettings.PBFactor ||
                this.Profile != otherSettings.Profile ||
                this.QPel != otherSettings.QPel ||
                this.QuantCompression != otherSettings.QuantCompression ||
                this.QuantizerMatrix != otherSettings.QuantizerMatrix ||
                this.QuantizerMatrixType != otherSettings.QuantizerMatrixType ||
                this.SCDSensitivity != otherSettings.SCDSensitivity ||
                this.SubPelRefinement != otherSettings.SubPelRefinement ||
                this.TempComplexityBlur != otherSettings.TempComplexityBlur ||
                this.TempQuanBlurCC != otherSettings.TempQuanBlurCC ||
                this.TempQuantBlur != otherSettings.TempQuantBlur ||
                this.Trellis != otherSettings.Trellis ||
                this.Turbo != otherSettings.Turbo ||
                this.V4MV != otherSettings.V4MV ||
                this.VBVBufferSize != otherSettings.VBVBufferSize ||
                this.VBVInitialBuffer != otherSettings.VBVInitialBuffer ||
                this.VBVMaxBitrate != otherSettings.VBVMaxBitrate ||
                this.WeightedBPrediction != otherSettings.WeightedBPrediction ||
                this.X264Trellis != otherSettings.X264Trellis ||
                this.AQmode != otherSettings.AQmode ||
                this.AQstrength != otherSettings.AQstrength ||
                this.UseQPFile != otherSettings.UseQPFile ||
                this.fullRange != otherSettings.fullRange
                )
                return true;
            else
                return false;
        }

        public void doTriStateAdjustment()
        {
            switch (Profile)
            {
                case 0:
                    Cabac = false;
                    NbBframes = 0;
                    NewAdaptiveBFrames = 0;
                    BFramePyramid = false;
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    BframeBias = 0;
                    BframePredictionMode = 1; // default
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    Lossless = false;
                    break;
                case 1:
                    I8x8mv = false;
                    AdaptiveDCT = false;
                    QuantizerMatrixType = 0; // no matrix
                    QuantizerMatrix = "";
                    break;
                case 2:
                    break;
            }
            if (EncodingMode != 2 && EncodingMode != 5)
                Turbo = false;
            if (Turbo)
            {
                NbRefFrames = 1;
                SubPelRefinement = 1;
                METype = 0; // diamond search
                I4x4mv = false;
                P4x4mv = false;
                I8x8mv = false;
                P8x8mv = false;
                B8x8mv = false;
                AdaptiveDCT = false;
                MixedRefs = false;
                Trellis = false;
                noFastPSkip = false;
                WeightedBPrediction = false;
            }
            if (Profile != 2) // lossless requires High Profile
                Lossless = false;
            if (NbRefFrames <= 1) // mixed references require at least two reference frames
                MixedRefs = false;
            if (NbBframes < 2) // pyramid requires at least two b-frames
                BFramePyramid = false;
            if (NbBframes == 0)
            {
                NewAdaptiveBFrames = 0;
                WeightedBPrediction = false;
            }
            if (!Cabac) // trellis requires CABAC
                X264Trellis = 0;
            if (!P8x8mv) // p8x8 requires p4x4
                P4x4mv = false;
            if (Lossless) // This needs CQ 0
            {
                EncodingMode = 1;
                BitrateQuantizer = 0;
            }
        }
	}
}
