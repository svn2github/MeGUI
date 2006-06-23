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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace MeGUI
{
	public enum AspectRatio: int {ITU16x9 = 0, ITU4x3, A1x1, CUSTOM};
	public enum FieldOperation: int {NONE = 0, FF, RAW};

	/// <summary>
	/// Summary description for d2vReader.
	/// </summary>
	public class d2vReader : VideoReader	
	{
        private AvsReader reader;
		private string fileName;
		private int fieldOperation;
		private AspectRatio aspectRatio;
        private int darX = -1, darY = -1;
		private double filmPercentage;
		/// <summary>
		/// initializes the d2v reader
		/// </summary>
		/// <param name="fileName">the dvd2avi project file that this reader will process</param>
        public d2vReader(string fileName, bool loadVideo)
		{
			this.fileName = fileName;
            if (loadVideo)
            {
                openD2V();
            }
            this.readFileProperties();
        }
		/// <summary>
		/// opens a dvd2avi project file
		/// </summary>
		/// <returns>true if there was no error opening the file, false otherwise</returns>
		public bool openD2V()
		{
            this.reader = AvsReader.ParseScript("DGDecode_Mpeg2Source(\"" + this.fileName + "\")");
            return true;
		}
		/// <summary>
		/// ensures proper closure and freeing of all resources
		/// </summary>
		public override void Close()
		{
            closeD2V();
            GC.SuppressFinalize(this);
		}
		/// <summary>
		/// reads the d2v file, which is essentially a text file
		/// the first few lines contain the video properties in plain text and the 
		/// last line contains the film percentage
		/// this method reads all this information and stores it internally, then 
		/// closes the d2v file again
		/// </summary>
		private void readFileProperties()
		{
            using(StreamReader sr = new StreamReader(fileName))
            {
				string line = sr.ReadLine();
				while ((line = sr.ReadLine()) != null)
				{
					if (line.IndexOf("Aspect_Ratio") != -1) // this is the aspect ratio line
					{
						string ar = line.Substring(13);
                        if (ar.Equals("16:9"))
                            this.aspectRatio = AspectRatio.ITU16x9;
                        else if (ar.Equals("4:3"))
                            this.aspectRatio = AspectRatio.ITU4x3;
                        else if (ar.Equals("1:1"))
                            this.aspectRatio = AspectRatio.A1x1;
                        else
                            this.aspectRatio = AspectRatio.CUSTOM;

                        double AR = VideoUtil.getAspectRatio(aspectRatio);
                        if (AR > 0)
                            VideoUtil.approximate(AR, out darX, out darY);
                    }
					if (line.IndexOf("Field_Operation") != -1)
					{
						string fieldOp = line.Substring(16, 1);
						this.fieldOperation = Int32.Parse(fieldOp);
					}
					if (line.IndexOf("FINISHED") != -1)
					{
						int end = line.IndexOf("%");
						string percentage = line.Substring(10, end - 10);
						this.filmPercentage = Double.Parse(percentage, System.Globalization.CultureInfo.InvariantCulture);
					}
				}
			}
		}

		/// <summary>
		/// returns the desired frame as bitmap object
		/// </summary>
		/// <param name="position">the position of the frame to be returned w.r.t the beginning of the stream</param>
		/// <returns>Bitmap containing the desired frame or null if an error ocurred</returns>
		public override Bitmap ReadFrameBitmap(int position)
		{
            return this.reader.ReadFrameBitmap(position);
		}
		/// <summary>
		/// closes the dvd2avi project file
		/// </summary>
		/// <returns>true if everything happened according to plan, false if an error ocurred</returns>
		public bool closeD2V()
		{
            if (this.reader != null)
                reader.Close();
			return true;

		}
		#region properties
        public override int DARX
        {
            get { return darX; }
        }

        public override int DARY
        {
            get { return darY; }
        }
        /// <summary>
		/// gets the vertical resolution of the video
		/// </summary>
		public override int Width
		{
			get {return this.reader.Width;}
		}
		/// <summary>
		///  gets the horizontal resolution of the video
		/// </summary>
		public override int Height
		{
            get { return this.reader.Height; }
		}
		/// <summary>
		/// gets the number of frames of the video
		/// </summary>
		public override int FrameCount
		{
            get { return this.reader.FrameCount; }
		}
		/// <summary>
		/// gets the framerate of the video
		/// </summary>
		public override double Framerate
		{
            get { return this.reader.Framerate; }
		}
		/// <summary>
		/// returns the percentage of film of this source
		/// </summary>
		public double FilmPercentage
		{
			get {return this.filmPercentage;}
		}
		/// <summary>
		/// returns the aspect ratio of this source
		/// </summary>
		public int AR
		{
			get {return (int)this.aspectRatio;}
		}
		/// <summary>
		/// returns the field operation performed on this source
		/// </summary>
		public int FieldOperation
		{
			get {return this.fieldOperation;}
		}
		#endregion
	}
}