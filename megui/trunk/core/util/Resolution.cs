// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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
        /// calculates the ideal mod vertical resolution that matches the desired horizontal resolution
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
        public static int SuggestVerticalResolution(double readerHeight, double readerWidth, Dar inputDAR, CropValues cropping, int horizontalResolution,
            bool signalAR, out Dar? dar, int mod, decimal acceptableAspectErrorPercent)
        {
            decimal fractionOfWidth = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) / (decimal)readerWidth;
            decimal inputWidthOnHeight = ((decimal)readerWidth - (decimal)cropping.left - (decimal)cropping.right) /
                                          ((decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom);
            decimal sourceHorizontalResolution = (decimal)readerHeight * inputDAR.AR * fractionOfWidth;
            decimal sourceVerticalResolution = (decimal)readerHeight - (decimal)cropping.top - (decimal)cropping.bottom;
            decimal realAspectRatio = getAspectRatio(inputDAR.AR, sourceHorizontalResolution / sourceVerticalResolution, acceptableAspectErrorPercent); // the aspect ratio of the video
            decimal resizedVerticalResolution = (decimal)horizontalResolution / realAspectRatio;

            int scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod)) * mod;

            if (signalAR)
            {
                resizedVerticalResolution = (decimal)horizontalResolution / inputWidthOnHeight; // Scale vertical resolution appropriately
                scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / (decimal)mod) * mod);

                int parX = 0;
                int parY = 0;
                int iLimit = 101;
                decimal distance = 999999;
                if (acceptableAspectErrorPercent == 0)
                    iLimit = 100001;
                for (int i = 1; i < iLimit; i++)
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
            }
            else
                dar = null;

            return scriptVerticalResolution;
        }

        /// <summary>
        /// finds the aspect ratio closest to the one giving as parameter (which is an approximation using the selected DAR for the source and the cropping values)
        /// </summary>
        /// <param name="calculatedAR">the aspect ratio to be approximated</param>
        /// <returns>the aspect ratio that most closely matches the input</returns>
        private static decimal getAspectRatio(decimal inputAR, decimal calculatedAR, decimal acceptableAspectErrorPercent)
        {
            decimal aspectError = inputAR / calculatedAR;
            if (Math.Abs(aspectError - 1.0M) * 100.0M < acceptableAspectErrorPercent)
                return inputAR;
            else
                return calculatedAR;
        }

        /// <summary>
        /// calculates the aspect ratio error
        /// </summary>
        /// <param name="inputWidth">height of the source (without cropping!)</param>
        /// <param name="inputHeight">width of the source (without cropping!)</param>
        /// <param name="outputWidth">the desired width of the output</param>
        /// <param name="outputHeight">the desired height of the output</param>
        /// <param name="Cropping">the crop values for the source</param>
        /// <param name="inputDar">custom input display aspect ratio to be taken into account</param>
        /// <param name="signalAR">whether or not we're going to signal the aspect ratio (influences the resizing)</param>
        /// <param name="outputDar">custom output display aspect ratio to be taken into account</param>
        /// <returns>the aspect ratio error in percent</returns>
        public static decimal GetAspectRatioError(int inputWidth, int inputHeight, int outputWidth, int outputHeight, CropValues Cropping, Dar? inputDar, bool signalAR, Dar? outputDar)
        {
            if (inputHeight <= 0 || inputWidth <= 0 || outputHeight <= 0 || outputWidth <= 0)
                return 0;

            // get input dimension with SAR 1:1
            int iHeight = inputHeight - Cropping.top - Cropping.bottom;
            decimal iWidth = inputWidth - Cropping.left - Cropping.right;
            if (inputDar.HasValue)
            {
                Sar s = inputDar.Value.ToSar(inputWidth, inputHeight);
                iWidth = iWidth * s.X / s.Y;
            }

            // get output dimension with SAR 1:1
            int oHeight = outputHeight;
            decimal oWidth = outputWidth;
            if (signalAR && outputDar.HasValue)
            {
                Sar s = outputDar.Value.ToSar(outputWidth, outputHeight);
                oWidth = oWidth * s.X / s.Y;
            }

            return (iHeight * oWidth) / (iWidth * oHeight) - 1;
        }

        /// <summary>
        /// calculates the DAR value based upon the video information 
        /// </summary>
        /// <param name="width">width of the video</param>
        /// <param name="height">height of the video</param>
        /// <param name="dar">display aspect ratio </param>
        /// <param name="par">pixel aspect ratio </param>
        /// <param name="darString">display aspect ratio string - e.g. 16:9 or 4:3</param>
        /// <returns>the DAR value</returns>
        public static Dar GetDAR(int width, int height, string darValue, decimal? par, string darString)
        {
            if (!String.IsNullOrEmpty(darString) && width == 720 && (height == 576 || height == 480))
            {
                Dar newDar = Dar.A1x1;
                if (!MainForm.Instance.Settings.UseITUValues)
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.STATIC16x9;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.STATIC4x3;
                }
                else if (height == 576)
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.ITU16x9PAL;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.ITU4x3PAL;
                }
                else
                {
                    if (darString.Equals("16:9"))
                        newDar = Dar.ITU16x9NTSC;
                    else if (darString.Equals("4:3"))
                        newDar = Dar.ITU4x3NTSC;
                }
                if (!newDar.Equals(Dar.A1x1))
                    return newDar;
            }

            if (par == null || par <= 0)
                par = 1;

            decimal? dar = easyParseDecimal(darValue);
            if (dar != null && dar > 0)
            {
                decimal correctDar = (decimal)width * (decimal)par / height;
                if (Math.Abs(Math.Round(correctDar, 3) - Math.Round((decimal)dar, 3)) <= 0.001M)
                    return new Dar((ulong)(Math.Round(width * (decimal)par)), (ulong)height);
                else
                    return new Dar((decimal)dar);
            }

            if (darValue.Contains(":"))
                return new Dar(ulong.Parse(darValue.Split(':')[0]), ulong.Parse(darValue.Split(':')[1]));
            else
                return new Dar((ulong)width, (ulong)height);
        }

        private static decimal? easyParseDecimal(string value)
        {
            try
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
                return decimal.Parse(value, culture);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}