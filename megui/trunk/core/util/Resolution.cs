// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Diagnostics;
using System.Text;

namespace MeGUI.core.util
{
    public class Resolution
    {
        /// <summary>
		/// if enabled, each change of the horizontal resolution triggers this method
		/// it calculates the ideal mod vertical resolution that matches the desired horizontal resolution
		/// </summary>
		/// <param name="readerHeight">height of the source</param>
		/// <param name="readerWidth">width of the source</param>
		/// <param name="customDAR">custom display aspect ratio to be taken into account for resizing</param>
		/// <param name="cropping">the crop values for the source</param>
		/// <param name="horizontalResolution">the desired horizontal resolution of the output</param>
		/// <param name="signalAR">whether or not we're going to signal the aspect ratio (influences the resizing)</param>
		/// <param name="sarX">horizontal pixel aspect ratio (used when signalAR = true)</param>
		/// <param name="sarY">vertical pixel aspect ratio (used when signalAR = true)</param>
        /// <param name="mod">the MOD value</param>
		/// <returns>the suggested horizontal resolution</returns>
		public static int suggestResolution(double readerHeight, double readerWidth, double customDAR, CropValues cropping, int horizontalResolution,
			bool signalAR, out Dar? dar, int mod)
		{
            decimal fractionOfWidth = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) / (decimal)readerWidth;
            decimal inputWidthOnHeight = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) /
                                          ((decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom);
            decimal sourceHorizontalResolution = (decimal)readerHeight * (decimal)customDAR * fractionOfWidth;
            decimal sourceVerticalResolution = (decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom;
            decimal realAspectRatio = sourceHorizontalResolution / sourceVerticalResolution; // the real aspect ratio of the video
			decimal resizedVerticalResolution = (decimal)horizontalResolution / realAspectRatio;

            int scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod)) * mod;

            if (signalAR)
			{
                resizedVerticalResolution = (decimal)horizontalResolution / inputWidthOnHeight; // Scale vertical resolution appropriately
                scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod) * mod);

                int parX = 0;
                int parY = 0;
                decimal distance = 999999;
                for (int i = 1; i < 1001; i++)
                {
                    // We create a fraction with integers, and then convert back to a decimal, and see how big the rounding error is
                    decimal fractionApproximation = (decimal)Math.Round(realAspectRatio * ((decimal)i)) / (decimal)i;
                    decimal approximationDifference = Math.Abs(realAspectRatio - fractionApproximation);
                    if (approximationDifference < distance)
                    {
                        distance = approximationDifference;
                        parY = i;
                        parX = (int)Math.Round(realAspectRatio * ((decimal)parY));
                    }
                }
                Debug.Assert(parX > 0 && parY > 0);
                dar = new Dar((ulong)parX, (ulong)parY);
				
				return scriptVerticalResolution;
			}
			else
			{
                dar = null;
				return scriptVerticalResolution;
			}
		}

        /// <summary>
        /// rounds the output PAR to the closest matching predefined xvid profile
        /// </summary>
        /// <param name="sarX">horizontal component of the pixel aspect ratio</param>
        /// <param name="sarY">vertical component of the pixel aspect ratio</param>
        /// <param name="height">height of the desired output</param>
        /// <param name="width">width of the desired output</param>
        /// <returns>the closest profile match</returns>
        public static int roundXviDPAR(int parX, int parY, int height, int width)
        {
            double par = (double) parX / (double) parY;
            double[] pars = { 1, 1.090909, 1.454545, 0.090909, 1.212121 };
            double minDist = 1000;
            int closestIndex = 0;
            for (int i = 0; i < pars.Length; i++)
            {
                double first = Math.Max(par, pars[i]);
                double second = Math.Min(par, pars[i]);
                double dist = first - second;
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
    }
}
