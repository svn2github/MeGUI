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
	/// this class contains the values for all the lavc settings in the GUI
	/// </summary>
	public class lavcSettings: VideoCodecSettings
	{
		private int mbDecisionAlgo, scd, subpelRefinement, maxQuantDelta, minBitrate, maxBitrate, 
			bufferSize, filesizeTolerance, fieldOrder;
		private decimal lumiMasking, darkMask, ipFactor, pbFactor, bQuantOffset, quantizerBlur, quantizerCompression, 
			meRange, initialBufferOccupancy, borderMask, spatialMask, temporalMask, nbMotionPredictors;
		private bool avoidHighMoBframes, greyscale, interlaced;
		private string intraMatrix, interMatrix;

        public override bool UsesSAR
        {
            get { return false; }
        }
        public override VideoCodec Codec
        {
            get { return VideoCodec.ASP; }
        }
        public override VideoEncoderType EncoderType
        {
            get { return VideoEncoderType.LMP4; }
        }
        /// <summary>
		/// default constructor
		/// initializes all the variables at the codec's default
		/// </summary>
		public lavcSettings():base()
		{
			EncodingMode = 0;
			BitrateQuantizer = 800;
			KeyframeInterval = 250;
			NbBframes = 0;
			mbDecisionAlgo = 0;
			scd = 0;
			subpelRefinement = 8;
			greyscale = false;
			interlaced = false;
			fieldOrder = -1;
			MinQuantizer = 2;
			MaxQuantizer = 31;
			maxQuantDelta = 2;
			CreditsQuantizer = new decimal(20);
			minBitrate = 0;
			maxBitrate = 0;
			bufferSize = 0;
			filesizeTolerance = 8000;
			lumiMasking = 0;
			darkMask = new decimal(0.0);
			ipFactor = new decimal(0.8);
			pbFactor = new decimal(1.25);
			bQuantOffset = new decimal(1.25);
			quantizerBlur = 0;
			quantizerCompression = 0;
			Turbo = false;
			avoidHighMoBframes = false;
			V4MV = false;
			QPel = false;
			Trellis = false;
			intraMatrix = "";
			interMatrix = "";
			meRange = 0;
			initialBufferOccupancy = (decimal)0.9;
			borderMask = (decimal)0;
			spatialMask = (decimal)0;
			temporalMask = (decimal)0;
			nbMotionPredictors = (decimal)0;
            FourCCs = xvidSettings.FourCCsForMPEG4ASP;
		}
        
		#region properties
		public int MbDecisionAlgo
		{
			get {return mbDecisionAlgo;}
			set {mbDecisionAlgo = value;}
		}
		public int SCD
		{
			get {return scd;}
			set {scd = value;}
		}
		public int SubpelRefinement
		{
			get {return subpelRefinement;}
			set {subpelRefinement = value;}
		}
		public int MaxQuantDelta
		{
			get {return maxQuantDelta;}
			set {maxQuantDelta = value;}
		}
		public int MinBitrate
		{
			get {return minBitrate;}
			set {minBitrate = value;}
		}
		public int MaxBitrate
		{
			get {return maxBitrate;}
			set {maxBitrate = value;}
		}
		public int BufferSize
		{
			get {return bufferSize;}
			set {bufferSize = value;}
		}
		public int FilesizeTolerance
		{
			get {return filesizeTolerance;}
			set {filesizeTolerance = value;}
		}
		public int FieldOrder
		{
			get {return fieldOrder;}
			set {fieldOrder = value;}
		}
		public decimal MERange
		{
			get {return meRange;}
			set {meRange = value;}
		}
		public decimal LumiMasking
		{
			get {return lumiMasking;}
			set {lumiMasking = value;}
		}
		public decimal DarkMask
		{
			get {return darkMask;}
			set {darkMask = value;}
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
		public decimal BQuantFactor
		{
			get {return bQuantOffset;}
			set {bQuantOffset = value;}
		}
		public decimal QuantizerBlur
		{
			get {return quantizerBlur;}
			set {quantizerBlur = value;}
		}
		public decimal QuantizerCompression
		{
			get {return quantizerCompression;}
			set {quantizerCompression = value;}
		}
		public decimal InitialBufferOccupancy
		{
			get {return initialBufferOccupancy;}
			set {initialBufferOccupancy = value;}
		}
		public decimal BorderMask
		{
			get {return borderMask;}
			set {borderMask = value;}
		}
		public decimal SpatialMask
		{
			get {return spatialMask;}
			set {spatialMask = value;}
		}
		public decimal TemporalMask
		{
			get {return temporalMask;}
			set {temporalMask = value;}
		}
		public decimal NbMotionPredictors
		{
			get {return nbMotionPredictors;}
			set {nbMotionPredictors = value;}
		}
		public bool Interlaced
		{
			get {return interlaced;}
			set {interlaced = value;}
		}
		public bool GreyScale
		{
			get {return greyscale;}
			set {greyscale = value;}
		}
		public bool AvoidHighMoBframes
		{
			get {return avoidHighMoBframes;}
			set {avoidHighMoBframes = value;}
		}
		public string IntraMatrix
		{
			get {return intraMatrix;}
			set {intraMatrix = value;}
		}
		public string InterMatrix
		{
			get {return interMatrix;}
			set {interMatrix = value;}
		}
		#endregion


        /// <summary>
        ///  Handles assessment of whether the encoding options vary between two lavcSettings instances
        /// The following are excluded from the comparison:
        /// BitrateQuantizer
        /// CreditsQuantizer
        /// Logfile
        /// Nbthreads
        /// SARX
        /// SARY
        /// Zones
        /// </summary>
        /// <param name="otherSettings"></param>
        /// <returns>true if the settings differ</returns>
        public override bool IsAltered(VideoCodecSettings settings)
        {
            if (!(settings is lavcSettings))
                return true;
            lavcSettings otherSettings = (lavcSettings)settings;
            if (
                this.AvoidHighMoBframes != otherSettings.AvoidHighMoBframes ||
                this.BorderMask != otherSettings.BorderMask ||
                this.BQuantFactor != otherSettings.BQuantFactor ||
                this.BufferSize != otherSettings.BufferSize ||
                this.CustomEncoderOptions != otherSettings.CustomEncoderOptions ||
                this.DarkMask != otherSettings.DarkMask ||
                this.EncodingMode != otherSettings.EncodingMode ||
                this.FieldOrder != otherSettings.FieldOrder ||
                this.FilesizeTolerance != otherSettings.FilesizeTolerance ||
                this.FourCC != otherSettings.FourCC ||
                // this.FourCCs != otherSettings.FourCCs ||
                this.GreyScale != otherSettings.GreyScale ||
                this.InitialBufferOccupancy != otherSettings.InitialBufferOccupancy ||
                this.Interlaced != otherSettings.Interlaced ||
                this.InterMatrix != otherSettings.InterMatrix ||
                this.IntraMatrix != otherSettings.IntraMatrix ||
                this.IPFactor != otherSettings.IPFactor ||
                this.KeyframeInterval != otherSettings.KeyframeInterval ||
                this.LumiMasking != otherSettings.LumiMasking ||
                this.MaxBitrate != otherSettings.MaxBitrate ||
                this.MaxQuantDelta != otherSettings.MaxQuantDelta ||
                this.MaxQuantizer != otherSettings.MaxQuantizer ||
                this.MbDecisionAlgo != otherSettings.MbDecisionAlgo ||
                this.MERange != otherSettings.MERange ||
                this.MinBitrate != otherSettings.MinBitrate ||
                this.MinQuantizer != otherSettings.MinQuantizer ||
                this.NbBframes != otherSettings.NbBframes ||
                this.NbMotionPredictors != otherSettings.NbMotionPredictors ||
                this.PBFactor != otherSettings.PBFactor ||
                this.QPel != otherSettings.QPel ||
                this.QuantizerBlur != otherSettings.QuantizerBlur ||
                this.QuantizerCompression != otherSettings.QuantizerCompression ||
                this.SCD != otherSettings.SCD ||
                this.SpatialMask != otherSettings.SpatialMask ||
                this.SubpelRefinement != otherSettings.SubpelRefinement ||
                this.TemporalMask != otherSettings.TemporalMask ||
                this.Trellis != otherSettings.Trellis ||
                this.Turbo != otherSettings.Turbo ||
                this.V4MV != otherSettings.V4MV
            )
                return true;
            else
                return false;
        }
    }
}