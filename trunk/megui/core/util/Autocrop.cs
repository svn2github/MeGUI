using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MeGUI.core.util
{
    public class Autocrop
    {
        public static CropValues autocrop(IVideoReader reader)
        {
            int pos = reader.FrameCount / 4;
            int tenPercent = reader.FrameCount / 20;
            CropValues[] cropValues = new CropValues[10];
            for (int i = 0; i < 10; i++)
            {
                Bitmap b = reader.ReadFrameBitmap(pos);
                cropValues[i] = getAutoCropValues(b);
                pos += tenPercent;
            }
            bool error = false;
            CropValues final = getFinalAutocropValues(cropValues);
            if (!error)
            {
                return final;
            }
            else
            {
                final.left = -1;
                final.right = -1;
                final.top = -1;
                final.bottom = -1;
                return final;
            }
        }

        /// <summary>
        /// iterates through a set of CropValues and tries to find a majority of matching crop values. If a match is found, the crop values are returned
        /// if not, the minimum found value is returned for the value in question
        /// </summary>
        /// <param name="values">the CropValues array to be analyzed</param>
        /// <returns>the final CropValues</returns>
        public static CropValues getFinalAutocropValues(CropValues[] values)
        {
            int matchingLeftValues = 0, matchingTopValues = 0, matchingRightValues = 0, matchingBottomValues = 0;
            int minLeft = values[0].left, minTop = values[0].top, minRight = values[0].right, minBottom = values[0].bottom;
            CropValues retval = values[0].Clone();
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].left == values[i - 1].left)
                {
                    retval.left = values[i].left;
                    matchingLeftValues++;
                }
                if (values[i].top == values[i - 1].top)
                {
                    retval.top = values[i].top;
                    matchingTopValues++;
                }
                if (values[i].right == values[i - 1].right)
                {
                    retval.right = values[i].right;
                    matchingRightValues++;
                }
                if (values[i].bottom == values[i - 1].bottom)
                {
                    retval.bottom = values[i].bottom;
                    matchingBottomValues++;
                }
                if (values[i].left < minLeft)
                    minLeft = values[i].left;
                if (values[i].top < minTop)
                    minTop = values[i].top;
                if (values[i].right < minRight)
                    minRight = values[i].right;
                if (values[i].bottom < minBottom)
                    minBottom = values[i].bottom;
            }
            if (matchingLeftValues < values.Length / 2) // we have less than 50% matching values, use minimum found
                retval.left = minLeft;
            if (matchingTopValues < values.Length / 2)
                retval.top = minTop;
            if (matchingRightValues < values.Length / 2)
                retval.right = minRight;
            if (matchingBottomValues < values.Length / 2)
                retval.bottom = minBottom;
            return retval;
        }

        private static bool isBadPixel(int pixel)
        {
            int comp = 12632256;
            int res = pixel & comp;
            return (res != 0);
        }
        /// <summary>
        /// iterates through the lines and columns of the bitmap and compares the pixel values with the value of the upper left corner pixel
        /// if a pixel that doesn't have the same color value is found, it is assumed that this is the first line that does not need to be cropped away
        /// </summary>
        /// <param name="b">the bitmap to be analyzed</param>
        /// <returns>struct containing the number of lines to be cropped away from the left, top, right and bottom</returns>
        public static unsafe CropValues getAutoCropValues(Bitmap b)
        {
            // When locking the pixels into memory, they are currently being converted from 24bpp to 32bpp. This incurs a small (5%) speed penalty,
            // but means that pixel management is easier, because each pixel is a 4-byte int.
            BitmapData image = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int* pointer = (int*)image.Scan0.ToPointer();
            int* lineBegin, pixel;
            int stride = image.Stride / 4;
            CropValues retval = new CropValues();
            bool lineFound = false;
            int badPixelThreshold = 50;
            int widthBadPixelThreshold = b.Width / badPixelThreshold;
            int heightBadPixelThreshold = b.Height / badPixelThreshold;
            int nbBadPixels = 0;

            lineBegin = pointer;
            for (int i = 0; i < b.Width; i++)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Height; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > heightBadPixelThreshold)
                    {
                        retval.left = i;
                        if (retval.left < 0)
                            retval.left = 0;
                        if (retval.left % 2 != 0)
                            retval.left++;
                        lineFound = true;
                        break;
                    }
                    pixel += stride;
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin += 1; // 4-byte Argb
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer;
            for (int i = 0; i < b.Height; i++)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Width; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > widthBadPixelThreshold)
                    {
                        retval.top = i;
                        if (retval.top < 0)
                            retval.top = 0;
                        if (retval.top % 2 != 0)
                            retval.top++;
                        lineFound = true;
                        break;
                    }
                    pixel += 1; // 4-byte Argb
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin += stride;
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer + b.Width - 1;
            for (int i = b.Width - 1; i >= 0; i--)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Height; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > heightBadPixelThreshold)
                    {
                        retval.right = b.Width - i - 1;
                        if (retval.right < 0)
                            retval.right = 0;
                        if (retval.right % 2 != 0)
                            retval.right++;
                        lineFound = true;
                        break;
                    }
                    pixel += stride;
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin -= 1; // Backwards across 4-byte Argb
            }
            nbBadPixels = 0;
            lineFound = false;
            lineBegin = pointer + stride * (b.Height - 1);
            for (int i = b.Height - 1; i >= 0; i--)
            {
                pixel = lineBegin;
                for (int j = 0; j < b.Width; j++)
                {
                    if (isBadPixel(*pixel))
                        nbBadPixels++;
                    if (nbBadPixels > widthBadPixelThreshold)
                    {
                        retval.bottom = b.Height - i - 1;
                        if (retval.bottom < 0)
                            retval.bottom = 0;
                        if (retval.bottom % 2 != 0)
                            retval.bottom++;
                        lineFound = true;
                        break;
                    }
                    pixel += 1;// 4-byte Argb
                }
                nbBadPixels = 0;
                if (lineFound)
                    break;
                lineBegin -= stride;
            }
            return retval;
        }
    }
}
