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
using System.Text;
using System.Drawing;

namespace MeGUI
{
    public sealed class AvsReader: VideoReader
    {

        private AviSynthScriptEnvironment enviroment = null;
        private AviSynthClip clip = null;
        private int width, height, darX = -1, darY = -1;
        private double frameRate;

        public AviSynthClip Clip
        {
            get
            {
                return this.clip;
            }
        }

        public static AvsReader OpenScriptFile(string fileName)
        {
            return new AvsReader(fileName, false);
        }


        public static AvsReader ParseScript(string scriptBody)
        {
            return new AvsReader(scriptBody, true);
        }

        private AvsReader( string script, bool parse)
        {
            try
            {
                this.enviroment = new AviSynthScriptEnvironment();
                this.clip = parse? enviroment.ParseScript(script, AviSynthColorspace.RGB24) : enviroment.OpenScriptFile(script, AviSynthColorspace.RGB24);
                if (!this.clip.HasVideo)
                    throw new ArgumentException("Script doesn't contain video");
                this.height = this.clip.VideoHeight;
                this.width = this.clip.VideoWidth;
                this.frameRate = ((double)clip.raten) / ((double)clip.rated);
                this.darX = this.clip.GetIntVariable("MeGUI_darx", -1);
                this.darY = this.clip.GetIntVariable("MeGUI_dary", -1);
            }
            catch(Exception)
            {
                cleanup();
                throw;
            }
        }

        private void cleanup()
        {
            if (this.clip != null)
            {
                (this.clip as IDisposable).Dispose();
                this.clip = null;
            }
            if (this.enviroment != null)
            {
                (this.enviroment as IDisposable).Dispose();
                this.enviroment = null;
            }
        }

        public override void Close()
        {
            cleanup();
        }

        public override int DARX
        {
            get { return darX; }
        }

        public override int DARY
        {
            get { return darY; }
        }

        public override int Width
        {
            get { return this.width; }
        }

        public override int Height
        {
            get { return this.height; }
        }

        public override double Framerate
        {
            get { return this.frameRate; }
        }

        public override int FrameCount
        {
            get { return clip.num_frames; }
        }

        public override Bitmap ReadFrameBitmap(int position)
        {
            Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            try
            {
                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bmp.PixelFormat);
                try
                {
                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;
                    // Read data
                    clip.ReadFrame(ptr, bmpData.Stride, position);
                }
                finally
                {
                    // Unlock the bits.
                    bmp.UnlockBits(bmpData);
                }
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                return bmp;
            }
            catch (Exception)
            {
                bmp.Dispose();
                throw;
            }
        }
    }
}
