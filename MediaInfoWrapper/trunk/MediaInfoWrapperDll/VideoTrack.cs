using System;

namespace MediaInfoWrapper
{
    ///<summary>Contains properties for a VideoTrack </summary>
    public class VideoTrack
    {
        private string _Count;
        private string _StreamCount;
        private string _StreamKind;
        private string _StreamKindID;
        private string _StreamOrder;
        private string _Inform;
        private string _ID;
        private string _UniqueID;
        private string _Title;
        private string _Codec;
        private string _CodecString;
        private string _CodecInfo;
        private string _CodecUrl;
        private string _CodecID;
        private string _CodecIDInfo;
        private string _BitRate;
        private string _BitRateString;
        private string _BitRateMode;
        private string _EncodedLibrary;
        private string _EncodedLibrarySettings;
        private string _Width;
        private string _Height;
        private string _AspectRatio;
        private string _AspectRatioString;
        private string _FrameRate;
        private string _FrameRateString;
        private string _FrameRateOriginal;
        private string _FrameRateOriginalString;
        private string _FrameRateMode;
        private string _FrameRateModeString;
        private string _FrameCount;
        private string _BitDepth;
        private string _BitsPixelFrame;
        private string _Delay;
        private string _Duration;
        private string _DurationString;
        private string _DurationString1;
        private string _DurationString2;
        private string _DurationString3;
        private string _Language;
        private string _LanguageString;
        private string _LanguageMore;
        private string _Format;
        private string _FormatInfo;
        private string _FormatUrl;
        private string _FormatVersion;
        private string _FormatProfile;
        private string _FormatSettings;
        private string _FormatSettingsBVOP;
        private string _FormatSettingsBVOPString;
        private string _FormatSettingsQPel;
        private string _FormatSettingsQPelString;
        private string _FormatSettingsGMC;
        private string _FormatSettingsGMCString;
        private string _FormatSettingsMatrix;
        private string _FormatSettingsMatrixString;
        private string _FormatSettingsMatrixData;
        private string _FormatSettingsCABAC;
        private string _FormatSettingsCABACString;
        private string _FormatSettingsRefFrames;
        private string _FormatSettingsRefFramesString;
        private string _FormatSettingsPulldown;
        private string _ScanType;
        private string _ScanTypeString;
        private string _Default;
        private string _DefaultString;
        private string _Forced;
        private string _ForcedString;

        ///<summary> Count of objects available in this stream </summary>
        public string Count
        {
            get
            {
                if (String.IsNullOrEmpty(this._Count))
                    this._Count = "";
                return _Count;
            }
            set
            {
                this._Count = value;
            }
        }

        ///<summary> Count of streams of that kind available </summary>
        public string StreamCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamCount))
                    this._StreamCount = "";
                return _StreamCount;
            }
            set
            {
                this._StreamCount = value;
            }
        }

        ///<summary> Stream name </summary>
        public string StreamKind
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKind))
                    this._StreamKind = "";
                return _StreamKind;
            }
            set
            {
                this._StreamKind = value;
            }
        }

        ///<summary> When multiple streams, number of the stream </summary>
        public string StreamKindID
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKindID))
                    this._StreamKindID = "";
                return _StreamKindID;
            }
            set
            {
                this._StreamKindID = value;
            }
        }

        ///<summary>Stream order in the file, whatever is the kind of stream (base=0)</summary>
        public string StreamOrder
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamOrder))
                    this._StreamOrder = "";
                return _StreamOrder;
            }
            set
            {
                this._StreamOrder = value;
            }
        }

        ///<summary> Last   Inform   call </summary>
        public string Inform
        {
            get
            {
                if (String.IsNullOrEmpty(this._Inform))
                    this._Inform = "";
                return _Inform;
            }
            set
            {
                this._Inform = value;
            }
        }

        ///<summary> A ID for this stream in this file </summary>
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this._ID))
                    this._ID = "";
                return _ID;
            }
            set
            {
                this._ID = value;
            }
        }

        ///<summary> A unique ID for this stream, should be copied with stream copy </summary>
        public string UniqueID
        {
            get
            {
                if (String.IsNullOrEmpty(this._UniqueID))
                    this._UniqueID = "";
                return _UniqueID;
            }
            set
            {
                this._UniqueID = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (String.IsNullOrEmpty(this._Title))
                    this._Title = "";
                return _Title;
            }
            set
            {
                this._Title = value;
            }
        }

        ///<summary> Codec used </summary>
        public string Codec
        {
            get
            {
                if (String.IsNullOrEmpty(this._Codec))
                    this._Codec = "";
                return _Codec;
            }
            set
            {
                this._Codec = value;
            }
        }
        
        ///<summary> Codec used (text) </summary>
        public string CodecString
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecString))
                    this._CodecString = "";
                return _CodecString;
            }
            set
            {
                this._CodecString = value;
            }
        }

        ///<summary> Info about codec </summary>
        public string CodecInfo
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecInfo))
                    this._CodecInfo = "";
                return _CodecInfo;
            }
            set
            {
                this._CodecInfo = value;
            }
        }

        ///<summary> Link </summary>
        public string CodecUrl
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecUrl))
                    this._CodecUrl = "";
                return _CodecUrl;
            }
            set
            {
                this._CodecUrl = value;
            }
        }

        ///<summary> Codec ID used </summary>
        public string CodecID
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecID))
                    this._CodecID = "";
                return _CodecID;
            }
            set
            {
                this._CodecID = value;
            }
        }

        ///<summary> Info about codec ID </summary>
        public string CodecIDInfo
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDInfo))
                    this._CodecIDInfo = "";
                return _CodecIDInfo;
            }
            set
            {
                this._CodecIDInfo = value;
            }
        }

        ///<summary> Bit rate in bps </summary>
        public string BitRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRate))
                    this._BitRate = "";
                return _BitRate;
            }
            set
            {
                this._BitRate = value;
            }
        }

        ///<summary> Bit rate (with measurement) </summary>
        public string BitRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateString))
                    this._BitRateString = "";
                return _BitRateString;
            }
            set
            {
                this._BitRateString = value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) </summary>
        public string BitRateMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMode))
                    this._BitRateMode = "";
                return _BitRateMode;
            }
            set
            {
                this._BitRateMode = value;
            }
        }

        ///<summary> Software used to create the file </summary>
        public string EncodedLibrary
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibrary))
                    this._EncodedLibrary = "";
                return _EncodedLibrary;
            }
            set
            {
                this._EncodedLibrary = value;
            }
        }

        ///<summary> Parameters used by the software </summary>
        public string EncodedLibrarySettings
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibrarySettings))
                    this._EncodedLibrarySettings = "";
                return _EncodedLibrarySettings;
            }
            set
            {
                this._EncodedLibrarySettings = value;
            }
        }

        ///<summary> Width </summary>
        public string Width
        {
            get
            {
                if (String.IsNullOrEmpty(this._Width))
                    this._Width = "";
                return _Width;
            }
            set
            {
                this._Width = value;
            }
        }

        ///<summary> Height </summary>
        public string Height
        {
            get
            {
                if (String.IsNullOrEmpty(this._Height))
                    this._Height = "";
                return _Height;
            }
            set
            {
                this._Height = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatio
        {
            get
            {
                if (String.IsNullOrEmpty(this._AspectRatio))
                    this._AspectRatio = "";
                return _AspectRatio;
            }
            set
            {
                this._AspectRatio = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatioString
        {
            get
            {
                if (String.IsNullOrEmpty(this._AspectRatioString))
                    this._AspectRatioString = "";
                return _AspectRatioString;
            }
            set
            {
                this._AspectRatioString = value;
            }
        }

        ///<summary> Frame rate </summary>
        public string FrameRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRate))
                    this._FrameRate = "";
                return _FrameRate;
            }
            set
            {
                this._FrameRate = value;
            }
        }

        ///<summary> Frame rate </summary>
        public string FrameRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateString))
                    this._FrameRateString = "";
                return _FrameRateString;
            }
            set
            {
                this._FrameRateString = value;
            }
        }

        ///<summary> Frame rate original </summary>
        public string FrameRateOriginal
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateOriginal))
                    this._FrameRateOriginal = "";
                return _FrameRateOriginal;
            }
            set
            {
                this._FrameRateOriginal = value;
            }
        }

        ///<summary> Frame rate original</summary>
        public string FrameRateOriginalString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateOriginalString))
                    this._FrameRateOriginalString = "";
                return _FrameRateOriginalString;
            }
            set
            {
                this._FrameRateOriginalString = value;
            }
        }

        ///<summary> Frame rate mode</summary>
        public string FrameRateMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateMode))
                    this._FrameRateMode = "";
                return _FrameRateMode;
            }
            set
            {
                this._FrameRateMode = value;
            }
        }

        ///<summary> Frame rate mode</summary>
        public string FrameRateModeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameRateModeString))
                    this._FrameRateModeString = "";
                return _FrameRateModeString;
            }
            set
            {
                this._FrameRateModeString = value;
            }
        }

        ///<summary> Frame count </summary>
        public string FrameCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._FrameCount))
                    this._FrameCount = "";
                return _FrameCount;
            }
            set
            {
                this._FrameCount = value;
            }
        }

        ///<summary> example for MP3 : 16 bits </summary>
        public string BitDepth
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitDepth))
                    this._BitDepth = "";
                return _BitDepth;
            }
            set
            {
                this._BitDepth = value;
            }
        }

        ///<summary> bits_(Pixel Frame) (like Gordian Knot) </summary>
        public string BitsPixelFrame
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitsPixelFrame))
                    this._BitsPixelFrame = "";
                return _BitsPixelFrame;
            }
            set
            {
                this._BitsPixelFrame = value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) IN MS </summary>
        public string Delay
        {
            get
            {
                if (String.IsNullOrEmpty(this._Delay))
                    this._Delay = "";
                return _Delay;
            }
            set
            {
                this._Delay = value;
            }
        }

        ///<summary> Duration of the stream </summary>
        public string Duration
        {
            get
            {
                if (String.IsNullOrEmpty(this._Duration))
                    this._Duration = "";
                return _Duration;
            }
            set
            {
                this._Duration = value;
            }
        }

        ///<summary> Duration (formated) </summary>
        public string DurationString
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString))
                    this._DurationString = "";
                return _DurationString;
            }
            set
            {
                this._DurationString = value;
            }
        }

        ///<summary> Duration in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DurationString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString1))
                    this._DurationString1 = "";
                return _DurationString1;
            }
            set
            {
                this._DurationString1 = value;
            }
        }

        ///<summary> Duration in format : XXx YYy only, YYy omited if zero </summary>
        public string DurationString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString2))
                    this._DurationString2 = "";
                return _DurationString2;
            }
            set
            {
                this._DurationString2 = value;
            }
        }

        ///<summary> Duration in format : HH:MM:SS.MMM </summary>
        public string DurationString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString3))
                    this._DurationString3 = "";
                return _DurationString3;
            }
            set
            {
                this._DurationString3 = value;
            }
        }

        ///<summary> Language (2 letters) </summary>
        public string Language
        {
            get
            {
                if (String.IsNullOrEmpty(this._Language))
                    this._Language = "";
                return _Language;
            }
            set
            {
                this._Language = value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageString))
                    this._LanguageString = "";
                return _LanguageString;
            }
            set
            {
                this._LanguageString = value;
            }
        }

        ///<summary> More info about Language (director s comment...) </summary>
        public string LanguageMore
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageMore))
                    this._LanguageMore = "";
                return _LanguageMore;
            }
            set
            {
                this._LanguageMore = value;
            }
        }

        ///<summary> Format </summary>
        public string Format
        {
            get
            {
                if (String.IsNullOrEmpty(this._Format))
                    this._Format = "";
                return _Format;
            }
            set
            {
                this._Format = value;
            }
        }

        ///<summary> Info about Format </summary>
        public string FormatInfo
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatInfo))
                    this._FormatInfo = "";
                return _FormatInfo;
            }
            set
            {
                this._FormatInfo = value;
            }
        }

        ///<summary> Url about Format </summary>
        public string FormatUrl
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatUrl))
                    this._FormatUrl = "";
                return _FormatUrl;
            }
            set
            {
                this._FormatUrl = value;
            }
        }

        ///<summary> Version of the Format </summary>
        public string FormatVersion
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatVersion))
                    this._FormatVersion = "";
                return _FormatVersion;
            }
            set
            {
                this._FormatVersion = value;
            }
        }

        ///<summary> Profile of the Format </summary>
        public string FormatProfile
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatProfile))
                    this._FormatProfile = "";
                return _FormatProfile;
            }
            set
            {
                this._FormatProfile = value;
            }
        }

        ///<summary> Settings of the Format </summary>
        public string FormatSettings
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettings))
                    this._FormatSettings = "";
                return _FormatSettings;
            }
            set
            {
                this._FormatSettings = value;
            }
        }

        ///<summary> BVOP Info </summary>
        public string FormatSettingsBVOP
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsBVOP))
                    this._FormatSettingsBVOP = "";
                return _FormatSettingsBVOP;
            }
            set
            {
                this._FormatSettingsBVOP = value;
            }
        }

        ///<summary> BVOP Info (string format)</summary>
        public string FormatSettingsBVOPString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsBVOPString))
                    this._FormatSettingsBVOPString = "";
                return _FormatSettingsBVOPString;
            }
            set
            {
                this._FormatSettingsBVOPString = value;
            }
        }

        ///<summary> Qpel Info </summary>
        public string FormatSettingsQPel
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsQPel))
                    this._FormatSettingsQPel = "";
                return _FormatSettingsQPel;
            }
            set
            {
                this._FormatSettingsQPel = value;
            }
        }

        ///<summary> Qpel Info (string format)</summary>
        public string FormatSettingsQPelString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsQPelString))
                    this._FormatSettingsQPelString = "";
                return _FormatSettingsQPelString;
            }
            set
            {
                this._FormatSettingsQPelString = value;
            }
        }

        ///<summary> GMC Info </summary>
        public string FormatSettingsGMC
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsGMC))
                    this._FormatSettingsGMC = "";
                return _FormatSettingsGMC;
            }
            set
            {
                this._FormatSettingsGMC = value;
            }
        }

        ///<summary> GMC Info (string format)</summary>
        public string FormatSettingsGMCString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsGMCString))
                    this._FormatSettingsGMCString = "";
                return _FormatSettingsGMCString;
            }
            set
            {
                this._FormatSettingsGMCString = value;
            }
        }

        ///<summary> Matrix Info </summary>
        public string FormatSettingsMatrix
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsMatrix))
                    this._FormatSettingsMatrix = "";
                return _FormatSettingsMatrix;
            }
            set
            {
                this._FormatSettingsMatrix = value;
            }
        }

        ///<summary> Matrix Info (string format)</summary>
        public string FormatSettingsMatrixString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsMatrixString))
                    this._FormatSettingsMatrixString = "";
                return _FormatSettingsMatrixString;
            }
            set
            {
                this._FormatSettingsMatrixString = value;
            }
        }

        ///<summary> Matrix Info (data format)</summary>
        public string FormatSettingsMatrixData
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsMatrixData))
                    this._FormatSettingsMatrixData = "";
                return _FormatSettingsMatrixData;
            }
            set
            {
                this._FormatSettingsMatrixData = value;
            }
        }

        ///<summary> CABAC Info </summary>
        public string FormatSettingsCABAC
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsCABAC))
                    this._FormatSettingsCABAC = "";
                return _FormatSettingsCABAC;
            }
            set
            {
                this._FormatSettingsCABAC = value;
            }
        }

        ///<summary> CABAC Info (string format)</summary>
        public string FormatSettingsCABACString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsCABACString))
                    this._FormatSettingsCABACString = "";
                return _FormatSettingsCABACString;
            }
            set
            {
                this._FormatSettingsCABACString = value;
            }
        }

        ///<summary> RefFrames Info </summary>
        public string FormatSettingsRefFrames
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsRefFrames))
                    this._FormatSettingsRefFrames = "";
                return _FormatSettingsRefFrames;
            }
            set
            {
                this._FormatSettingsRefFrames = value;
            }
        }

        ///<summary> RefFrames Info (string format)</summary>
        public string FormatSettingsRefFramesString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsRefFramesString))
                    this._FormatSettingsRefFramesString = "";
                return _FormatSettingsRefFramesString;
            }
            set
            {
                this._FormatSettingsRefFramesString = value;
            }
        }

        ///<summary> Pulldown Info </summary>
        public string FormatSettingsPulldown
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsPulldown))
                    this._FormatSettingsPulldown = "";
                return _FormatSettingsPulldown;
            }
            set
            {
                this._FormatSettingsPulldown = value;
            }
        }

        ///<summary> ScanType Info </summary>
        public string ScanType
        {
            get
            {
                if (String.IsNullOrEmpty(this._ScanType))
                    this._ScanType = "";
                return _ScanType;
            }
            set
            {
                this._ScanType = value;
            }
        }

        ///<summary> ScanType Info (string format)</summary>
        public string ScanTypeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ScanTypeString))
                    this._ScanTypeString = "";
                return _ScanTypeString;
            }
            set
            {
                this._ScanTypeString = value;
            }
        }

        ///<summary> Default Info </summary>
        public string Default
        {
            get
            {
                if (String.IsNullOrEmpty(this._Default))
                    this._Default = "";
                return _Default;
            }
            set
            {
                this._Default = value;
            }
        }

        ///<summary> Default Info (string format)</summary>
        public string DefaultString
        {
            get
            {
                if (String.IsNullOrEmpty(this._DefaultString))
                    this._DefaultString = "";
                return _DefaultString;
            }
            set
            {
                this._DefaultString = value;
            }
        }

        ///<summary> Forced Info </summary>
        public string Forced
        {
            get
            {
                if (String.IsNullOrEmpty(this._Forced))
                    this._Forced = "";
                return _Forced;
            }
            set
            {
                this._Forced = value;
            }
        }

        ///<summary> Forced Info (string format)</summary>
        public string ForcedString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ForcedString))
                    this._ForcedString = "";
                return _ForcedString;
            }
            set
            {
                this._ForcedString = value;
            }
        }
    }
}
