using System;
using System.Drawing;

namespace MeGUI
{
	/// <summary>
	/// VideoReader is an unused superclass for the avireader and d2vreader
	/// </summary>
	public abstract class VideoReader: IDisposable
	{
		public abstract void Close();

        public abstract int Width
        {
            get;
        }

        public abstract int Height        
        {
            get;
        }

        public abstract double Framerate
        {
            get;
        }


        public abstract int FrameCount
        {
            get;
        }

        public abstract int DARX
        {
            get;
        }

        public abstract int DARY
        {
            get;
        }

        public abstract Bitmap ReadFrameBitmap(int position);

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Close();
        }

        #endregion
    }
}
