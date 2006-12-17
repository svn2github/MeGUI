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
using System.IO;
using System.Text;
using System.Globalization;

namespace MeGUI
{
	// generates a commandline for the given muxSettings instance
    public delegate string MuxCommandlineGenerator(MuxSettings muxSettings);
    
    /// <summary>
	/// Class that can generate the commanline from a codec settings object
	/// </summary>
	public class CommandLineGenerator
	{
		#region initialization
		private static StringBuilder sb = new StringBuilder();
		private static CultureInfo ci = new CultureInfo("en-us");
		#endregion
		#region helper methods
		/// <summary>
		/// returns the video output type (selected index of the codec type dropdown in the main gui)
		/// from an output pathname
		/// </summary>
		/// <param name="path">the video output filename</param>
		/// <returns>0 if the output is avi, 1 if the output is raw, 2 if the output is mp4 and -1 otherwise</returns>
		private static int getVideoOutputType(string path)
		{
            if (path != null)
            {
                string extension = Path.GetExtension(path).ToLower();
                if (extension.Equals(".avi"))
                    return 0;
                if (extension.Equals(".raw") || extension.Equals(".264") || extension.Equals(".m4v"))
                    return 1;
                if (extension.Equals(".mp4"))
                    return 2;
            }
            return -1;
		}
        private static void x264TriStateAdjustment(x264Settings xs)
        {
            switch (xs.Profile)
            {
                case 0:
                    xs.Cabac = false;
                    xs.NbBframes = 0;
                    xs.AdaptiveBFrames = false;
                    xs.BFramePyramid = false;
                    xs.I8x8mv = false;
                    xs.AdaptiveDCT = false;
                    xs.BframeBias = 0;
                    xs.BframePredictionMode = 1; // default
                    xs.QuantizerMatrixType = 0; // no matrix
                    xs.QuantizerMatrix = "";
                    xs.Lossless = false;
                    break;
                case 1:
                    xs.I8x8mv = false;
                    xs.AdaptiveDCT = false;
                    xs.QuantizerMatrixType = 0; // no matrix
                    xs.QuantizerMatrix = "";
                    break;
                case 2:
                    break;
            }
            if (xs.EncodingMode != 2 && xs.EncodingMode != 5)
                xs.Turbo = false;
            if (xs.Turbo)
            {
                xs.NbRefFrames = 1;
                xs.SubPelRefinement = 0;
                xs.METype = 0; // diamond search
                xs.I4x4mv = false;
                xs.P4x4mv = false;
                xs.I8x8mv = false;
                xs.P8x8mv = false;
                xs.B8x8mv = false;
                xs.AdaptiveDCT = false;
                xs.MixedRefs = false;
                xs.BRDO = false;
                xs.Trellis = false;
                xs.noFastPSkip = false;
                xs.WeightedBPrediction = false;
                xs.biME = false;
            }
            if (xs.Profile != 2) // lossless requires High Profile
                xs.Lossless = false;
            if (xs.NbRefFrames <= 1) // mixed references require at least two reference frames
                xs.MixedRefs = false;
            if (xs.NbBframes < 2) // pyramid requires at least two b-frames
                xs.BFramePyramid = false;
            if (xs.NbBframes == 0)
            {
            	xs.AdaptiveBFrames = false;
            	xs.biME = false;
            	xs.WeightedBPrediction = false;
            }
            if (!xs.Cabac) // trellis requires CABAC
                xs.X264Trellis = 0;
            if (xs.NbBframes == 0 || xs.SubPelRefinement < 5) // BRDO requires RDO and b-frames
                xs.BRDO = false;
            if (!xs.P8x8mv) // p8x8 requires p4x4
                xs.P4x4mv = false;
            if (xs.Lossless) // This needs CQ 0
            {
                xs.EncodingMode = 1;
                xs.BitrateQuantizer = 0;
            }
        }
		public static string generateVideoCommandline(VideoCodecSettings vSettings, 
			string input, string output, int parX, int parY)
		{
            if (vSettings is hfyuSettings)
                return generateHfyuCommandLine((hfyuSettings)vSettings, input, output, parX, parY);
            if (vSettings is lavcSettings)
				return generateLavcCommandLine((lavcSettings)vSettings, input, output, parX, parY);
			if (vSettings is x264Settings)
			{
				x264Settings xs = (x264Settings)vSettings;
                x264TriStateAdjustment(xs);
				return generateX264CLICommandline(xs, input, output, parX, parY);
		}
			if (vSettings is snowSettings)
				return generateSnowCommandLine((snowSettings)vSettings, input, output, parX, parY);
            if (vSettings is xvidSettings)
            {
                return generateXviDEncrawCommandline((xvidSettings)vSettings, input, output, parX, parY);
            }
			return "";
        }
		public static string generateAudioCommandline(MeGUISettings settings, AudioCodecSettings aSettings, 
			string input, string output)
		{
			if (aSettings.DelayEnabled && aSettings.Delay == 0)
				aSettings.DelayEnabled = false;
			
			return "";
		}
		#endregion
		#region lavc
        /// <summary>
        /// generates an lavc commandline from the lavcSettings object
        /// </summary>
        /// <param name="ls">lavcSettings object containing all the parameters for an lavc encoding session</param>
        /// <returns>commandline matching the input parameters</returns>
        private static string generateLavcCommandLine(lavcSettings ls, string input, string output, int parX, int parY)
        {
            sb = new StringBuilder();
            sb.Append("\"" + input + "\" -ovc lavc ");
            if (ls.EncodingMode == 4 || ls.EncodingMode == 7)
                ls.Turbo = false;
            switch (ls.EncodingMode)
            {
                case 0: // CBR
                    sb.Append("-lavcopts vbitrate=" + ls.BitrateQuantizer + ":"); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append("-lavcopts vqscale=" + ls.BitrateQuantizer + ":"); // add quantizer
                    break;
                case 2: // 2 pass first pass
                    sb.Append("-o NUL: -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=1:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & quantizer
                    if (ls.Turbo)
                        sb.Append("turbo:");
                    break;
                case 3: // 2 pass second pass
                case 4: // 2 pass automated encoding
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=2:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 5: // 3 pass first pass
                    sb.Append("-o NUL: -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=1:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & quantizer
                    break;
                case 6: // 3 pass 2nd pass
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=3:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 7:
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=3:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
            } // now add the rest of the lavc encoder options
            if (ls.KeyframeInterval != 250) // gop size of 250 is default
                sb.Append("keyint=" + ls.KeyframeInterval + ":");
            if (ls.NbBframes != 0) // 0 is default value
                sb.Append("vmax_b_frames=" + ls.NbBframes + ":");
            if (ls.AvoidHighMoBframes && (ls.EncodingMode == 2 || ls.EncodingMode == 5)) // is only available in first pass
                sb.Append("vb_strategy=1:");
            if (ls.MbDecisionAlgo > 0) // default is 0
                sb.Append("mbd=" + ls.MbDecisionAlgo + ":");
            if (ls.V4MV)
                sb.Append("v4mv:");
            if (ls.SCD != 0)
                sb.Append("sc_treshold=" + ls.SCD + ":");
            if (ls.QPel)
                sb.Append("qpel:");
            if (ls.LumiMasking != new decimal(0.0))
                sb.Append("lumi_mask=" + ls.LumiMasking.ToString(ci) + ":");
            if (ls.DarkMask != new decimal(0.0))
                sb.Append("dark_mask=" + ls.DarkMask.ToString(ci) + ":");
            if (ls.SubpelRefinement != 8)
                sb.Append("subq=" + ls.SubpelRefinement + ":");
            if (ls.GreyScale)
                sb.Append("gray:");
            if (ls.Interlaced)
            {
                sb.Append("ildct:ilme");
                if (ls.FieldOrder != -1)
                    sb.Append("top=" + ls.FieldOrder + ":");
            }
            if (ls.MERange != (decimal)0)
                sb.Append("me_range=" + ls.MERange.ToString(ci) + ":");
            if (ls.InitialBufferOccupancy != (decimal)0.9)
                sb.Append("vrc_init_occupancy=" + ls.InitialBufferOccupancy.ToString(ci) + ":");
            if (ls.BorderMask != (decimal)0.0)
                sb.Append("border_mask=" + ls.BorderMask.ToString(ci) + ":");
            if (ls.TemporalMask != (decimal)0.0)
                sb.Append("tcplx_mask=" + ls.TemporalMask.ToString(ci) + ":");
            if (ls.SpatialMask != (decimal)0.0)
                sb.Append("scplx_mask=" + ls.SpatialMask.ToString(ci) + ":");
            if (ls.NbThreads > 1)
                sb.Append("threads=" + ls.NbThreads + ":");
            if (ls.MinQuantizer != 2)
                sb.Append("vqmin=" + ls.MinQuantizer + ":");
            if (ls.MaxQuantizer != 31)
                sb.Append("vqmax=" + ls.MaxQuantizer + ":");
            if (ls.MaxQuantDelta != 3)
                sb.Append("vqdiff=" + ls.MaxQuantDelta + ":");
            if (ls.EncodingMode == 0 || ls.EncodingMode == 1)
            {
                if (ls.IPFactor != (decimal)0.8)
                    sb.Append("vi_qfactor=" + ls.IPFactor.ToString(ci) + ":");
                if (ls.PBFactor != (decimal)1.25)
                    sb.Append("vb_qfactor=" + ls.PBFactor.ToString(ci) + ":");
                if (ls.BQuantFactor != (decimal)1.25)
                    sb.Append("vb_qoffset=" + ls.BQuantFactor.ToString(ci) + ":");
            }
            if (ls.QuantizerBlur != new decimal(0.5))
                sb.Append("vqblur=" + ls.QuantizerBlur.ToString(ci) + ":");
            if (ls.QuantizerCompression != new decimal(0.5))
                sb.Append("vqcomp=" + ls.QuantizerCompression.ToString(ci) + ":");
            if (ls.Trellis)
                sb.Append("trell:");
            if (ls.MinBitrate != 0) // 0 = unlimited is default
                sb.Append("vrc_minrate=" + ls.MinBitrate + ":");
            if (ls.MaxBitrate != 0) // 0 = unlimited is default
                sb.Append("vrc_maxrate=" + ls.MaxBitrate + ":");
            if (ls.BufferSize != 0) // default = empty
                sb.Append("vrc_buf_size=" + ls.BufferSize + ":");
            if (ls.FilesizeTolerance != 8000) // default is 8000 kbits
                sb.Append("vratetol=" + ls.FilesizeTolerance + ":");
            if (ls.Zones != null && ls.Zones.Length > 0 && ls.CreditsQuantizer >= new decimal(1))
            {
                sb.Append("vrc_override=");
                foreach (Zone zone in ls.Zones)
                {
                    if (zone.mode == ZONEMODE.QUANTIZER)
                        sb.Append(zone.startFrame + "," + zone.endFrame + "," + zone.modifier + "/");
                    else
                        sb.Append(zone.startFrame + "," + zone.endFrame + ",-" + zone.modifier + "/");
                }
                sb.Remove(sb.Length - 1, 1); // remove trailing slash
                sb.Append(":");
            }
            if (!ls.IntraMatrix.Equals(""))
                sb.Append("intra_matrix=" + ls.IntraMatrix + ":");
            if (!ls.InterMatrix.Equals(""))
                sb.Append("inter_matrix=" + ls.InterMatrix + ":");
            if (parX > 0 && parY > 0)
                sb.Append("aspect=" + parX + "/" + parY + ":");
            if (ls.NbMotionPredictors != (decimal)0)
                sb.Append("last_pred=" + ls.NbMotionPredictors.ToString(ci) + ":");

            if (sb.ToString().EndsWith(":")) // remove the last :
                sb.Remove(sb.Length - 1, 1);
            //add the rest of the mencoder commandline regarding the output
            if (ls.EncodingMode != 2 && ls.EncodingMode != 5) // not 2 pass vbr first pass and 3 pass first pass, add output filename and output type
            {
                sb.Append(" -o \"" + output + "\" -of "); // rest of mencoder options
                int outputType = getVideoOutputType(output);
                if (outputType == 0) // AVI
                    sb.Append("avi -ffourcc " + ls.FourCCs[ls.FourCC] + " ");
                if (outputType >= 1) // RAW
                    sb.Append("rawvideo ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// generates an ffvhuff commandline ignoring the settings
        /// </summary>
        /// <param name="ls">just for compatibility. There is nothing useful in the settings</param>
        /// <param name="input">input avs script</param>
        /// <param name="output">output lossless file</param>
        /// <returns>commandline matching the input parameters</returns>
        private static string generateHfyuCommandLine(hfyuSettings ls, string input, string output, int parX, int parY)
        {
            sb = new StringBuilder();
            sb.Append("\"" + input + "\" -o \"" + output + "\" ");
            sb.Append("-of avi -forceidx "); // Make sure we don't get problems with filesizes > 2GB
            sb.Append("-ovc lavc -lavcopts vcodec=ffvhuff:vstrict=-2:pred=2:context=1");
            return sb.ToString();
        }
		#endregion
		#region snow
		/// <summary>
		/// generates a commandline for a snow job
		/// </summary>
		/// <param name="ss">snowSettings object containing all the parameters for an snow encoding session</param>
		/// <returns>commandline matching the input parameters</returns>
		private static string generateSnowCommandLine(snowSettings ss, string input, string output, int parX, int parY)
		{
			sb = new StringBuilder();
			sb.Append("\"" + input + "\" -ovc lavc ");
			switch (ss.EncodingMode)
			{
				case 0: // CBR
					sb.Append("-lavcopts vcodec=snow:vbitrate=" + ss.BitrateQuantizer + ":"); // add bitrate
					break;
				case 1: // CQ
					sb.Append("-lavcopts vcodec=snow:vqscale=" + ss.Quantizer + ":");
					break;
				case 2: // 2 pass first pass
					sb.Append("-o NUL: -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
					//sb.Append("-lavcopts vcodec=snow:vpass=1:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
					sb.Append("-lavcopts vcodec=snow:vpass=1:vqscale=5:"); // workaround for corrupted first passes
					break;
				case 3: // 2 pass second pass
				case 4: // automated twopass
					sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
					sb.Append("-lavcopts vcodec=snow:vpass=2:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
					break;
				case 5:
					sb.Append("-o NUL: -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
					//sb.Append("-lavcopts vcodec=snow:vpass=1:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
					sb.Append("-lavcopts vcodec=snow:vpass=1:vqscale=5:"); // workaround for corrupted first passes
					break;
				case 6:
					sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
					sb.Append("-lavcopts vcodec=snow:vpass=3:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
					break;
				case 7:
					sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
					sb.Append("-lavcopts vcodec=snow:vpass=3:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
					break;
			}
			if (ss.PredictionMode != 0)
				sb.Append("pred=" + ss.PredictionMode + ":");
			switch (ss.MECompFullpel)
			{
				case 0:
					break;
				case 1:
					sb.Append("cmp=1:");
					break;
				case 2:
					sb.Append("cmp=11:");
					break;
				case 3:
					sb.Append("cmp=12:");
					break;
			}
			switch (ss.MECompHpel)
			{
				case 0:
					break;
				case 1:
					sb.Append("subcmp=1:");
					break;
				case 2:
					sb.Append("subcmp=11:");
					break;
				case 3:
					sb.Append("subcmp=12:");
					break;
			}
			switch (ss.MBComp)
			{
				case 0:
					break;
				case 1:
					sb.Append("mbcmp=1:");
					break;
				case 2:
					sb.Append("mbcmp=11:");
					break;
				case 3:
					sb.Append("mbcmp=12:");
					break;
			}
			if (ss.QPel) // default is unchecked
				sb.Append("qpel:");
			if (ss.V4MV) // default is unchecked
				sb.Append("v4mv:");
			if (ss.NbMotionPredictors != 0)
				sb.Append("last_pred=" + ss.NbMotionPredictors.ToString(ci) + ":");
			sb.Append("vstrict=-2:");
			if (ss.Zones != null && ss.Zones.Length > 0 && ss.CreditsQuantizer >= new decimal(1))
			{
				sb.Append("-vrc_override=");
				foreach (Zone zone in ss.Zones)
				{
					if (zone.mode == ZONEMODE.QUANTIZER)
						sb.Append(zone.startFrame + "," + zone.endFrame + "," + zone.modifier + "/");
					else
						sb.Append(zone.startFrame + "," + zone.endFrame + ",-" + zone.modifier + "/");
				}
				sb.Remove(sb.Length - 1, 1); // remove trailing /(zone separator)
				sb.Append(":");
			}
			if (sb.ToString().EndsWith(":")) // remove the last :
				sb.Remove(sb.Length - 1, 1);
			if (ss.EncodingMode != 2)
			{
				sb.Append(" -o \"" + output + "\" -of "); // rest of mencoder options
				int outputType = getVideoOutputType(output);
				if (outputType == 0) // AVI
					sb.Append("avi -ffourcc " + ss.FourCCs[ss.FourCC] + " ");
				if (outputType >= 1) // RAW
					sb.Append("rawvideo ");
			}
			return sb.ToString();
		}
		#endregion
		#region xvid
        private static string generateXviDEncrawCommandline(xvidSettings xs, string input, string output, int parX, int parY)
        {
            sb = new StringBuilder();
            sb.Append("-i \"" + input + "\" ");
            switch (xs.EncodingMode)
            {
                case 0: // CBR
                    sb.Append("-single -bitrate " + xs.BitrateQuantizer + " "); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append("-single -cq " + xs.BitrateQuantizer + " "); // add quantizer
                    break;
                case 2: // 2 pass first pass
                    sb.Append("-pass1 " + "\"" + xs.Logfile + "\" -bitrate " + xs.BitrateQuantizer + " "); // add logfile
                    break;
                case 3: // 2 pass second pass
                case 4: // automated twopass
                    sb.Append("-pass2 " + "\"" + xs.Logfile + "\" -bitrate " + xs.BitrateQuantizer + " "); // add logfile
                    break;
            }
            if (xs.EncodingMode <= 1) // 1 pass modes
            {
                if (xs.ReactionDelayFactor != 16)
                    sb.Append("-reaction " + xs.ReactionDelayFactor + " ");
                if (xs.AveragingPeriod != 100)
                    sb.Append("-averaging " + xs.AveragingPeriod + " ");
                if (xs.RateControlBuffer != 100)
                    sb.Append("-smoother " + xs.RateControlBuffer + " ");
            }
            else // two pass modes
            {
                if (xs.KeyFrameBoost != 10)
                    sb.Append("-kboost " + xs.KeyFrameBoost + " ");
                if (xs.KeyframeThreshold != 1)
                    sb.Append("-kthresh " + xs.KeyframeThreshold + " ");
                if (xs.KeyframeReduction != 20)
                    sb.Append("-kreduction " + xs.KeyframeReduction + " ");
                if (xs.OverflowControlStrength != 5)
                    sb.Append("-ostrength " + xs.OverflowControlStrength + " ");
                if (xs.MaxOverflowImprovement != 5)
                    sb.Append("-oimprove " + xs.MaxOverflowImprovement + " ");
                if (xs.MaxOverflowDegradation != 5)
                    sb.Append("-odegrade " + xs.MaxOverflowDegradation + " ");
                if (xs.HighBitrateDegradation != 0)
                    sb.Append("-chigh " + xs.HighBitrateDegradation + " ");
                if (xs.LowBitrateImprovement != 0)
                    sb.Append("-clow " + xs.LowBitrateImprovement + " ");
                sb.Append("-overhead 0 ");
                if (xs.VbvBuffer != 0)
                    sb.Append("-vbvsize " + xs.VbvBuffer + " ");
                if (xs.VbvMaxRate != 0)
                    sb.Append("-vbvmax " + xs.VbvMaxRate + " ");
                if (xs.VbvPeakRate != 0)
                    sb.Append("-vbvpeak " + xs.VbvPeakRate + " ");
            }
            if (xs.Turbo)
                sb.Append("-turbo ");
            if (xs.KeyframeInterval != 300)
                sb.Append("-max_key_interval " + xs.KeyframeInterval + " ");
            if (!xs.PackedBitstream) // default is on in encraw
                sb.Append("-nopacked ");
            if (xs.MotionSearchPrecision != 6)
                sb.Append("-quality " + xs.MotionSearchPrecision + " ");
            if (xs.VHQMode != 1)
                sb.Append("-vhqmode " + xs.VHQMode + " ");
            if (xs.QPel)
                sb.Append("-qpel ");
            if (xs.GMC)
                sb.Append("-gmc ");
            if (xs.QuantType == 1)
                sb.Append("-qtype 1 ");
            else if (xs.QuantType == 2 && xs.CustomQuantizerMatrix.Length > 0)
                sb.Append("-qmatrix \"" + xs.CustomQuantizerMatrix + "\" ");
            if (xs.ClosedGOP)
                sb.Append("-closed_gop ");
            if (xs.AdaptiveQuant)
                sb.Append("-lumimasking ");
            if (xs.Interlaced)
            {
                sb.Append("-interlaced ");
                if (xs.BottomFieldFirst)
                    sb.Append("1 ");
                else
                    sb.Append("2 ");
            }
            if (xs.Greyscale)
                sb.Append("-grey ");
            if (xs.LumiMasking)
                sb.Append("-lumimasking ");
            if (!xs.Trellis)
                sb.Append("-notrellis ");
            if (!xs.ChromaMotion)
                sb.Append("-nochromame ");
            if (xs.MinQuantizer != 2)
                sb.Append("-imin " + xs.MinQuantizer + " ");
            if (xs.MaxQuantizer != 31)
                sb.Append("-imax " + xs.MaxQuantizer + " ");
            if (xs.MinPQuant != 2)
                sb.Append("-pmin " + xs.MinPQuant + " ");
            if (xs.MaxPQuant != 31)
                sb.Append("-pmax " + xs.MaxPQuant + " ");
            if (!xs.ClosedGOP)
                sb.Append("-noclosed_gop ");
            if (xs.FrameDropRatio != 0)
                sb.Append("-drop " + xs.FrameDropRatio + " ");
            if (xs.NbBframes > 0)
            {
                sb.Append("-max_bframes " + xs.NbBframes + " ");
                if (xs.VHQForBframes)
                    sb.Append("-bvhq ");
                if (xs.BQuantRatio != 150)
                    sb.Append("-bquant_ratio " + xs.BQuantRatio + " ");
                if (xs.BQuantOffset != 100)
                    sb.Append("-bquant_offset " + xs.BQuantOffset + " ");
                if (xs.MinBQuant != 2)
                    sb.Append("-bmin " + xs.MinBQuant + " ");
                if (xs.MaxBQuant != 31)
                    sb.Append("-bmax " + xs.MaxBQuant + " ");
            }
            if (parX > 0 && parY > 0) // custom PAR mode
            {
                sb.Append("-par " + parX + ":" + parY + " ");
            }
            sb.Append("-threads " + xs.NbThreads + " ");
            if (xs.Zones != null && xs.Zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1)
                && xs.EncodingMode != 1) // only for non CQ mode at the moment
            {
                foreach (Zone zone in xs.Zones)
                {
                    if (zone.mode == ZONEMODE.QUANTIZER)
                        sb.Append("-zq " + zone.startFrame + " " + zone.modifier + " ");
                    if (zone.mode == ZONEMODE.WEIGHT)
                    {
                        sb.Append("-zw " + zone.startFrame + " ");
                        double mod = (double)zone.modifier / 100.0;
                        sb.Append(mod.ToString(ci) + " ");
                    }
                }
            }
            if (xs.EncodingMode != 2) // not 2 pass vbr first pass, add output filename and output type
            {
                string extension = Path.GetExtension(output).ToLower();
                if (extension.Equals(".mkv"))
                    sb.Append(" -mkv \"" + output + "\"");
                else if (extension.Equals(".avi"))
                    sb.Append(" -avi \"" + output + "\"");
                else
                    sb.Append(" -o \"" + output + "\"");
            }
            if (!xs.CustomEncoderOptions.Equals("")) // add custom encoder options
                sb.Append(" " + xs.CustomEncoderOptions);
            return sb.ToString(); 
        }

		#endregion
		#region x264
		private static string generateX264CLICommandline(x264Settings xs, string input, string output, int parX, int parY)
		{
			sb = new StringBuilder();
			if (xs.EncodingMode == 4 || xs.EncodingMode == 7)
				xs.Turbo = false; // turn off turbo to prevent inconsistent commandline preview
			switch (xs.EncodingMode)
			{
				case 0: // ABR
					sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
					break;
				case 1: // CQ
					sb.Append("--qp " + xs.BitrateQuantizer + " ");
					break;
				case 2: // 2 pass first pass
					sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
					break;
				case 3: // 2 pass second pass
				case 4: // automated twopass
					sb.Append("--pass 2 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
					break;
				case 5: // 3 pass first pass
					sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
					break;
				case 6: // 3 pass 2nd pass
					sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
					break;
				case 7: // 3 pass 3rd pass
                case 8: // automated threepass, show third pass options
					sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
					break;
				case 9: // constant quality
					sb.Append("--crf " + xs.BitrateQuantizer + " ");
					break;
			} // now add the rest of the x264 encoder options
            
            // AVC Level
            if (xs.Level != 15) // unrestricted (x264.exe default)
                sb.Append("--level " + AVCLevels.getCLILevelNames()[xs.Level] + " ");
			if (xs.KeyframeInterval != 250) // gop size of 250 is default
				sb.Append("--keyint " + xs.KeyframeInterval + " ");
			if (xs.MinGOPSize != 25)
				sb.Append("--min-keyint " + xs.MinGOPSize + " ");
			if (xs.Turbo)
			{
				xs.NbRefFrames = 1;
				xs.SubPelRefinement = 0; // Q-Pel 1 iteration
				xs.METype = 0; // diamond search
				xs.I4x4mv = false;
				xs.P4x4mv = false;
				xs.I8x8mv = false;
				xs.P8x8mv = false;
				xs.B8x8mv = false;
				xs.AdaptiveDCT = false;
				xs.MixedRefs = false;
                xs.BRDO = false;
                xs.X264Trellis = 0; // disable trellis
                xs.noFastPSkip = false;
                xs.WeightedBPrediction = false;
                xs.biME = false;
			}
			if (xs.NbRefFrames != 1) // 1 ref frame is default
				sb.Append("--ref " + xs.NbRefFrames + " ");
			if (xs.MixedRefs)
				sb.Append("--mixed-refs ");
			if (xs.noFastPSkip)
				sb.Append("--no-fast-pskip ");
			if (xs.NbBframes != 0) // 0 is default value, adaptive and pyramid are conditional on b frames being enabled
			{
				sb.Append("--bframes " + xs.NbBframes + " ");
				if (!xs.AdaptiveBFrames)
					sb.Append("--no-b-adapt ");
                if (xs.NbBframes > 1 && xs.BFramePyramid) // pyramid needs a minimum of 2 b frames
					sb.Append("--b-pyramid ");
				if (xs.BRDO)
				    sb.Append("--b-rdo ");
			    if (xs.biME)
				    sb.Append("--bime ");
			    if (xs.WeightedBPrediction)
				sb.Append("--weightb ");
                if (xs.BframePredictionMode != 1)
                {
                    sb.Append("--direct ");
                    if (xs.BframePredictionMode == 0)
                        sb.Append("none ");
                    else if (xs.BframePredictionMode == 2)
                        sb.Append("temporal ");
                    else if (xs.BframePredictionMode == 3)
                        sb.Append("auto ");
                }
			}
			if (xs.Deblock) // deblocker active, add options
			{
				if (xs.AlphaDeblock != 0 || xs.BetaDeblock != 0) // 0 is default value
					sb.Append("--filter " + xs.AlphaDeblock + "," + xs.BetaDeblock + " ");
			}
			else // no deblocking
				sb.Append("--nf ");
			if (!xs.Cabac) // no cabac
				sb.Append("--no-cabac ");
			if (xs.SubPelRefinement + 1 != 5) // non default subpel refinement
			{
				int subq = xs.SubPelRefinement + 1;
				sb.Append("--subme " + subq + " ");
			}
			if (!xs.ChromaME)
				sb.Append("--no-chroma-me ");
			if (xs.X264Trellis > 0)
				sb.Append("--trellis " + xs.X264Trellis + " ");
			// now it's time for the macroblock types
			if (xs.P8x8mv || xs.B8x8mv || xs.I4x4mv || xs.I8x8mv || xs.P4x4mv || xs.AdaptiveDCT)
			{
				sb.Append("--analyse ");
				if (xs.I4x4mv && xs.P4x4mv && xs.I8x8mv && xs.P8x8mv && xs.B8x8mv)
					sb.Append("all ");
				else
				{
					if (xs.P8x8mv) // default is checked
						sb.Append("p8x8,");
					if (xs.B8x8mv) // default is checked
						sb.Append("b8x8,");
					if (xs.I4x4mv) // default is checked
						sb.Append("i4x4,");
					if (xs.P4x4mv) // default is unchecked
						sb.Append("p4x4,");
					if (xs.I8x8mv) // default is unchecked
						sb.Append("i8x8");
					if (sb.ToString().EndsWith(","))
						sb.Remove(sb.Length - 1, 1);
				}
				if (xs.AdaptiveDCT) // default is unchecked
					sb.Append(" --8x8dct ");
				if (!sb.ToString().EndsWith(" "))
					sb.Append(" ");
			}
			else
			{
				sb.Append("--analyse none ");
			}
			if (xs.EncodingMode != 1) // doesn't apply to CQ mode
			{
				if (xs.MinQuantizer != 10) // default min quantizer is 10
					sb.Append("--qpmin " + xs.MinQuantizer + " ");
				if (xs.MaxQuantizer != 51) // 51 is the default max quanitzer
					sb.Append("--qpmax " + xs.MaxQuantizer + " ");
				if (xs.MaxQuantDelta != 4) // 4 is the default value
					sb.Append("--qpstep " + xs.MaxQuantDelta + " ");
				if (xs.IPFactor != new decimal(1.4)) // 1.4 is the default value
					sb.Append("--ipratio " + xs.IPFactor.ToString(ci) + " ");
				if (xs.PBFactor != new decimal(1.3)) // 1.3 is the default value here
					sb.Append("--pbratio " + xs.PBFactor.ToString(ci) + " ");
				if (xs.ChromaQPOffset != new decimal(0.0))
					sb.Append("--chroma-qp-offset " + xs.ChromaQPOffset.ToString(ci) + " ");
				if (xs.VBVBufferSize > 0)
					sb.Append("--vbv-bufsize " + xs.VBVBufferSize + " ");
				if (xs.VBVMaxBitrate > 0)
					sb.Append("--vbv-maxrate " + xs.VBVMaxBitrate + " ");
				if (xs.VBVInitialBuffer != new decimal(0.9))
					sb.Append("--vbv-init " + xs.VBVInitialBuffer.ToString(ci) + " ");
				if (xs.BitrateVariance != 1)
					sb.Append("--ratetol " + xs.BitrateVariance.ToString(ci) + " ");
				if (xs.QuantCompression != new decimal(0.6))
					sb.Append("--qcomp " + xs.QuantCompression.ToString(ci) + " ");
				if (xs.EncodingMode > 1) // applies only to twopass
				{
					if (xs.TempComplexityBlur != 20)
						sb.Append("--cplxblur " + xs.TempComplexityBlur.ToString(ci) + " ");
					if (xs.TempQuanBlurCC != new decimal(0.5))
						sb.Append("--qblur " + xs.TempQuanBlurCC.ToString(ci) + " ");
				}
			}
			if (xs.SCDSensitivity != new decimal(40))
				sb.Append("--scenecut " + xs.SCDSensitivity.ToString(ci) + " ");
			if (xs.BframeBias != new decimal(0))
				sb.Append("--b-bias " + xs.BframeBias.ToString(ci) + " ");
			if (xs.METype + 1 != 2)
			{
				sb.Append("--me ");
				if (xs.METype + 1 == 1)
					sb.Append("dia ");
				if (xs.METype + 1 == 3)
					sb.Append("umh ");
				if (xs.METype + 1 == 4)
					sb.Append("esa ");
			}
			if (xs.MERange != 16)
				sb.Append("--merange " + xs.MERange + " ");
			if (xs.NbThreads != 1)
				sb.Append("--threads " + xs.NbThreads + " ");
            sb.Append("--thread-input ");
			if (xs.Zones != null && xs.Zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1))
			{
				sb.Append("--zones ");
				foreach (Zone zone in xs.Zones)
				{
					sb.Append(zone.startFrame + "," + zone.endFrame + ",");
					if (zone.mode == ZONEMODE.QUANTIZER)
					{
						sb.Append("q=");
						sb.Append(zone.modifier + "/");
					}
					if (zone.mode == ZONEMODE.WEIGHT)
					{
						sb.Append("b=");
						double mod = (double)zone.modifier / 100.0;
						sb.Append(mod.ToString(ci) + "/");
					}
				}
				sb.Remove(sb.Length - 1, 1);
				sb.Append(" ");
			}
			if (parX > 0 && parY > 0)
				sb.Append("--sar " + parX + ":" + parY + " ");
			if (xs.QuantizerMatrixType > 0) // custom matrices enabled
			{
				if (xs.QuantizerMatrixType == 1)
					sb.Append("--cqm \"jvt\" ");
				if (xs.QuantizerMatrixType == 2)
					sb.Append("--cqmfile \"" + xs.QuantizerMatrix + "\" ");
			}
			sb.Append("--progress "); // ensure that the progress is shown
            if (xs.NoDCTDecimate)
                sb.Append("--no-dct-decimate ");
            if (!xs.PSNRCalculation)
                sb.Append("--no-psnr ");
            if (!xs.SSIMCalculation)
                sb.Append("--no-ssim ");
            if (xs.EncodeInterlaced)
                sb.Append("--interlaced ");
            if (xs.NoiseReduction > 0)
                sb.Append("--nr " + xs.NoiseReduction + " ");
            //add the rest of the mencoder commandline regarding the output
			if (xs.EncodingMode == 2 || xs.EncodingMode == 5)
				sb.Append("--output NUL ");
			else
				sb.Append("--output " + "\"" + output + "\" ");
			sb.Append("\"" + input + "\" ");
            if (!xs.CustomEncoderOptions.Equals("")) // add custom encoder options
				sb.Append(xs.CustomEncoderOptions);
            return sb.ToString();
		}
		#endregion
		#region audio
		#endregion
		#region mp4box
		/// <summary>
		/// generates the commandline for mp4box
		/// </summary>
		/// <param name="settings">the settings object containing additional streams and the framerate</param>
		/// <param name="input">the video stream for this mux job</param>
		/// <param name="output">the name of the output file</param>
		/// <returns>mp4box commandline</returns>
        public static string generateMP4BoxCommandline(MuxSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            if (settings.VideoInput.Length > 0)
            {
                sb.Append("-add \"" + settings.VideoInput + "\"");
                if (!settings.VideoName.Equals(""))
                    sb.Append(":name=\"" + settings.VideoName + "\"");
            }
            if (settings.MuxedInput.Length > 0)
            {
                sb.Append(" -add \"" + settings.MuxedInput + "\"");
            }
            foreach (object o in settings.AudioStreams)
            {
                SubStream stream = (SubStream)o;
                sb.Append(" -add \"" + stream.path + "\"");
                if (stream.language != null && !stream.language.Equals(""))
                    sb.Append(":lang=" + stream.language);
                if (stream.name != null && !stream.name.Equals(""))
                    sb.Append(":name=\"" + stream.name + "\"");
            }
            foreach (object o in settings.SubtitleStreams)
            {
                SubStream stream = (SubStream)o;
                sb.Append(" -add \"" + stream.path + "\"");
                if (!stream.language.Equals(""))
                    sb.Append(":lang=" + stream.language);
            }

            if (!settings.ChapterFile.Equals("")) // a chapter file is defined
                sb.Append(" -chap \"" + settings.ChapterFile + "\"");

            if (settings.SplitSize > 0)
                sb.Append(" -splits " + settings.SplitSize);

            if (settings.Framerate > 0)
            {
                string fpsString = settings.Framerate.ToString(ci);
                sb.Append(" -fps " + fpsString);
            }
            sb.Append(" -new \"" + settings.MuxedOutput + "\"");
            return sb.ToString();
        }
		#endregion
		#region avi muxing
        public static string generateDivXMuxCommandline(MuxSettings settings)
        {
            sb = new StringBuilder();
            if (settings.MuxedInput.Length > 0)
                sb.AppendFormat("--remux \"{0}\" ", settings.MuxedInput);
            if (settings.VideoInput.Length > 0)
                sb.AppendFormat("-v \"{0}\" ", settings.VideoInput);
            foreach (SubStream audioStream in settings.AudioStreams)
                sb.AppendFormat("-a \"{0}\" ", audioStream.path);
            foreach (SubStream subStream in settings.SubtitleStreams)
                sb.AppendFormat("-s \"{0}\" ", subStream.path);
            sb.AppendFormat("-o \"{0}\" ", settings.MuxedOutput);
            return sb.ToString();
        }
		/// <summary>
		/// generates an avi muxing commandline for mencoder
		/// </summary>
		/// <param name="mencoderPath">pat to mencoder</param>
		/// <param name="settings">audio settings</param>
		/// <param name="input">avi video input</param>
		/// <param name="output">muxed output</param>
		/// <returns>mencoder commandline</returns>
		public static string generateAVIMuxCommandline(MuxSettings settings)
		{
            sb = new StringBuilder();
            sb.Append("-ovc copy -oac copy ");
            if (settings.MuxedInput.Length > 0)
            {
                sb.Append("\"" + settings.MuxedInput + "\" ");
            }
            if (settings.VideoInput.Length > 0)
            {
                sb.Append("\"" + settings.VideoInput + "\" ");
            }
            if (settings.AudioStreams.Count > 0)
            {
                SubStream stream = (SubStream)settings.AudioStreams[0];
                sb.Append("-audiofile \"" + stream.path + "\" ");
            }
            sb.Append(" -mc 0 -noskip -o \"" + settings.VideoInput + "\"");
            return sb.ToString();
        }
        /// <summary>
        /// generates the commandline for avc2avi
        /// </summary>
        /// <param name="settings">the settings for the mux job</param>
        /// <param name="input">the raw avc file to be muxed</param>
        /// <param name="output">the name of the output</param>
        /// <returns>the commandline</returns>
        public static string GenerateAVC2AVICommandline(MuxSettings settings)
        {
            sb = new StringBuilder();
            sb.Append("-f " + settings.Framerate.ToString(ci) + " -i \"" + settings.VideoInput + "\" -o \"" + settings.MuxedOutput + "\"");
            return sb.ToString();
        }
        #endregion
		#region mkv muxing
		/// <summary>
		/// generates the commandline for mp4box
		/// </summary>
		/// <param name="mkvmergePath">path of the mkvmerge executable</param>
		/// <param name="settings">the settings object containing additional streams and the framerate</param>
		/// <param name="input">the video stream for this mux job</param>
		/// <param name="output">the name of the output file</param>
		/// <returns>mkvmerge commandline</returns>
		public static string generateMkvmergeCommandline(MuxSettings settings)
		{
			StringBuilder sb = new StringBuilder();
         	sb.Append("-o \"" + settings.MuxedOutput + "\"");
			
         	if (settings.PARX > 0 && settings.PARY > 0)
				sb.Append(" --aspect-ratio 0:" + settings.PARX + "/" + settings.PARY);

            if (settings.MuxedInput.Length > 0)
                sb.Append(" \"" + settings.MuxedInput + "\"");
            
            if (settings.VideoInput.Length > 0)
                sb.Append(" -A -S \"" + settings.VideoInput + "\"");
			
         	foreach (object o in settings.AudioStreams)
			{
				SubStream stream = (SubStream)o;
				int trackID = 0;
				if (Path.GetExtension(stream.path.ToLower()).Equals(".mp4"))
					trackID = 1;
            	if (!stream.language.Equals(""))
					sb.Append(" --language " + trackID + ":" + stream.language);
				
            	sb.Append(" -a " + trackID + " -D -S \"" + stream.path + "\"");
			}
			
         	foreach (object o in settings.SubtitleStreams)
			{
				SubStream stream = (SubStream)o;
				if (!stream.language.Equals(""))
					sb.Append(" --language 0:" + stream.language);
				
            sb.Append(" -s 0 -D -A \"" + stream.path + "\"");
			}
			if (!settings.ChapterFile.Equals("")) // a chapter file is defined
				sb.Append(" --chapters \"" + settings.ChapterFile + "\"");
			
         	if (settings.SplitSize > 0)
				sb.Append(" -split " + settings.SplitSize + "MB");
			
         	sb.Append(" --no-clusters-in-meta-seek"); // ensures lower overhead

         	return sb.ToString();
		}
		#endregion
		#region dgindex
		public static string generateDGIndexCommandline(int demuxType, int trackID1, int trackID2, 
			string input, string output)
		{
			StringBuilder sb = new StringBuilder();
			string projName = Path.Combine(Path.GetDirectoryName(output), Path.GetFileNameWithoutExtension(output));
			sb.Append("-AIF=[" + input + "] -OF=[" + projName + "] -exit -minimize ");
			if (demuxType == 2)
				sb.Append("-OM=2"); // demux everything
			else if (demuxType == 1)
			{
				int t1 = trackID1 + 1;
				int t2 = trackID2 + 1;
				if (t1-1 != -1 && t2-1 == -1) // demux the first track
					sb.Append("-OM=1 -TN=" + t1); // demux only the selected track
				else if (t2-1 != -1 && t1-1 == -1) // demux the second track
					sb.Append("-OM=1 -TN=" + t2); // demux only the selected track
				else if (t1-1 == -1 && t2-1 == -1)
					sb.Append("-OM=0");
				else
					sb.Append("-OM=1 -TN=" + t1 + "," + t2); // demux everything
			}
			else if (demuxType == 0) // no audio demux
				sb.Append("-OM=0");
			return sb.ToString();
		}
		#endregion

        public static string generateMuxCommandline(MuxSettings muxSettings, MuxerType muxerType, MainForm mainForm)
        {
            MuxCommandlineGenerator generator = mainForm.MuxProvider.GetMuxer(muxerType).CommandlineGenerator;
            if (generator != null)
                return generator(muxSettings);
            return null;
        }

        public static string generateVobSubCommandline(string configFile)
        {
            return "vobsub.dll,Configure " + configFile;
        }
    }
}
