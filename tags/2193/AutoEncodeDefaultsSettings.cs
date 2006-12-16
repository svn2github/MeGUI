using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AutoEncodeDefaultsSettings
    {
        private bool splitOutput, fileSizeMode, bitrateMode, noTargetSizeMode, addAdditionalContent;
        private string container;
        private int splitSize, fileSize, bitrate;

        public AutoEncodeDefaultsSettings()
        {
            splitOutput = false;
            fileSizeMode = true;
            bitrateMode = false;
            noTargetSizeMode = false;
            addAdditionalContent = false;
            container = "MP4";
            splitSize = 0;
            fileSize = 700;
            bitrate = 700;
        }
        #region properties
        /// <summary>
        /// gets / sets if additional content should be added
        /// </summary>
        public bool AddAdditionalContent
        {
            get { return addAdditionalContent; }
            set { addAdditionalContent = value; }
        }
        /// <summary>
        /// gets / sets if the final size should be defined by the bitrate/encoding mode set in the video settings
        /// </summary>
        public bool NoTargetSizeMode
        {
            get { return noTargetSizeMode; }
            set { noTargetSizeMode = value; }
        }
        /// <summary>
        /// gets / sets if a fixed bitrate should be used for video encoding
        /// </summary>
        public bool BitrateMode
        {
            get { return bitrateMode; }
            set { bitrateMode = value; }
        }
        /// <summary>
        /// gets / sets if the output should be defined by a filesize
        /// </summary>
        public bool FileSizeMode
        {
            get { return fileSizeMode; }
            set { fileSizeMode = value; }
        }
        /// <summary>
        /// gets / sets if the output should be split or not
        /// </summary>
        public bool SplitOutput
        {
            get { return splitOutput; }
            set { splitOutput = value; }
        }
        /// <summary>
        ///  gets / sets the default container
        /// </summary>
        public string Container
        {
            get { return container; }
            set { container = value; }
        }
        /// <summary>
        /// gets / sets the output video bitrate
        /// </summary>
        public int Bitrate
        {
            get { return bitrate; }
            set { bitrate = value; }
        }
        /// <summary>
        /// gets / sets the output filesize
        /// </summary>
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        /// <summary>
        /// gets / sets the default split size
        /// </summary>
        public int SplitSize
        {
            get { return splitSize; }
            set { splitSize = value; }
        }
        #endregion
    }
}
