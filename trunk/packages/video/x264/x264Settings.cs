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

namespace MeGUI
{
	/// <summary>
	/// Summary description for x264Settings.
	/// </summary>
	[Serializable]
	public class x264Settings: VideoCodecSettings
	{
		int nbRefFrames, alphaDeblock, betaDeblock, subPelRefinement, maxQuantDelta, tempQuantBlur, 
			bframePredictionMode, vbvBufferSize, vbvMaxBitrate, meType, meRange, minGOPSize, 
			quantizerMatrixType, profile, x264Trellis, level, noiseReduction;
		decimal ipFactor, pbFactor, chromaQPOffset, vbvInitialBuffer, bitrateVariance, quantCompression, 
			tempComplexityBlur, tempQuanBlurCC, scdSensitivity, bframeBias;
		bool deblock, cabac, p4x4mv, p8x8mv, b8x8mv, i4x4mv, i8x8mv, weightedBPrediction, adaptiveBFrames, encodeInterlaced,
			bFramePyramid, chromaME, adaptiveDCT, lossless, mixedRefs, bRDO, NoFastPSkip, BiME, psnrCalc, noDctDecimate, ssimCalc;
		string quantizerMatrix;
		#region constructor
        public override VideoCodec Codec
        {
            get { return VideoCodec.AVC; }
        }
        public override VideoEncoderType EncoderType
        {
            get { return VideoEncoderType.X264; }
        }
        /// <summary>
		/// default constructor, initializes codec default values
		/// </summary>
		public x264Settings():base()
		{
            encodeInterlaced = false;
			noFastPSkip = false;
            ssimCalc = false;
            psnrCalc = false;
			EncodingMode = 0;
			BitrateQuantizer = 700;
			KeyframeInterval = 250;
			nbRefFrames = 1;
			mixedRefs = false;
			NbBframes = 0;
			Turbo = false;
			deblock = true;
			alphaDeblock = 0;
			betaDeblock = 0;
			cabac = true;
			weightedBPrediction = false;
			adaptiveBFrames = false;
			bFramePyramid = false;
			subPelRefinement = 4;
			BiME = false;
			bRDO = false;
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
			bframePredictionMode = 2;
			scdSensitivity = new decimal(40);
			bframeBias = new decimal(0);
			meType = 1;
			meRange = 16;
			NbThreads = 0;
			minGOPSize = 25;
			adaptiveDCT = false;
			quantizerMatrix = "";
			quantizerMatrixType = 0; // none
			profile = 1; // main profile
			lossless = false;
			x264Trellis = 0;
			level = 15;
            base.MaxNumberOfPasses = 3;
		}
		#endregion
		#region properties
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
		public bool WeightedBPrediction
		{
			get {return weightedBPrediction;}
			set {weightedBPrediction = value;}
		}
		public bool AdaptiveBFrames
		{
			get {return adaptiveBFrames;}
			set {adaptiveBFrames = value;}
		}
		public bool BFramePyramid
		{
			get {return bFramePyramid;}
			set {bFramePyramid = value;}
		}
		public bool BRDO
		{
			get {return bRDO;}
			set {bRDO = value;}
		}
		public bool biME
		{
			get {return BiME;}
			set {BiME = value;}
		}
		public bool ChromaME
		{
			get {return chromaME;}
			set {chromaME = value;}
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
                this.AdaptiveBFrames != otherSettings.AdaptiveBFrames ||
                this.AdaptiveDCT != otherSettings.AdaptiveDCT ||
                this.AlphaDeblock != otherSettings.AlphaDeblock ||
                this.NoFastPSkip != otherSettings.NoFastPSkip ||
                this.B8x8mv != otherSettings.B8x8mv ||
                this.BetaDeblock != otherSettings.BetaDeblock ||
                this.BframeBias != otherSettings.BframeBias ||
                this.BframePredictionMode != otherSettings.BframePredictionMode ||
                this.BFramePyramid != otherSettings.BFramePyramid ||
                this.BitrateVariance != otherSettings.BitrateVariance ||
                this.biME != otherSettings.biME ||
                this.BRDO != otherSettings.BRDO ||
                this.Cabac != otherSettings.Cabac ||
                this.ChromaME != otherSettings.ChromaME ||
                this.ChromaQPOffset != otherSettings.ChromaQPOffset ||
                this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
                this.Deblock != otherSettings.Deblock ||
                this.EncodingMode != otherSettings.EncodingMode ||
                this.FourCC != otherSettings.FourCC ||
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
                this.X264Trellis != otherSettings.X264Trellis
                )
                return true;
            else
                return false;
        }
	}
}
