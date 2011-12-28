using System;

namespace MediaInfoWrapper
{
    ///<summary>Contains properties for a AudioTrack </summary>
    public class AudioTrack
    {
        private string _Count;
        private string _StreamCount;
        private string _StreamKind;
        private string _StreamKindString;
        private string _StreamKindID;
        private string _StreamKindPos;
        private string _Inform;
        private string _ID;
        private string _IDString;
        private string _UniqueID;
        private string _MenuID;
        private string _MenuIDString;
        private string _Format;
        private string _FormatInfo;
        private string _FormatUrl;
        private string _FormatVersion;
        private string _FormatProfile;
        private string _FormatSettings;
        private string _FormatSettingsSBR;
        private string _FormatSettingsSBRString;
        private string _FormatSettingsPS;
        private string _FormatSettingsPSString;
        private string _FormatSettingsFloor;
        private string _FormatSettingsFirm;
        private string _FormatSettingsEndianness;
        private string _FormatSettingsSign;
        private string _FormatSettingsLaw;
        private string _FormatSettingsITU;
        private string _MuxingMode;
        private string _CodecID;
        private string _CodecIDInfo;
        private string _CodecIDHint;
        private string _CodecIDUrl;
        private string _CodecIDDescription;
        private string _Duration;
        private string _DurationString;
        private string _DurationString1;
        private string _DurationString2;
        private string _DurationString3;
        private string _BitRate;
        private string _BitRateString;
        private string _BitRateMode;
        private string _BitRateModeString;
        private string _BitRateMinimum;
        private string _BitRateMinimumString;
        private string _BitRateNominal;
        private string _BitRateNominalString;
        private string _BitRateMaximum;
        private string _BitRateMaximumString;
        private string _Channels;
        private string _ChannelsString;
        private string _ChannelPositions;
        private string _ChannelPositionsString2;
        private string _ChannelMode;
        private string _SamplingRate;
        private string _SamplingRateString;
        private string _SamplingCount;
        private string _BitDepth;
        private string _BitDepthString;
        private string _CompressionRatio;
        private string _Delay;
        private string _DelayString;
        private string _DelayString1;
        private string _DelayString2;
        private string _DelayString3;
        private string _VideoDelay;
        private string _VideoDelayString;
        private string _VideoDelayString1;
        private string _VideoDelayString2;
        private string _VideoDelayString3;
        private string _ReplayGainGain;
        private string _ReplayGainGainString;
        private string _ReplayGainPeak;
        private string _StreamSize;
        private string _StreamSizeString;
        private string _StreamSizeString1;
        private string _StreamSizeString2;
        private string _StreamSizeString3;
        private string _StreamSizeString4;
        private string _StreamSizeString5;
        private string _StreamSizeProportion;
        private string _Alignment;
        private string _AlignmentString;
        private string _InterleaveVideoFrames;
        private string _InterleaveDuration;
        private string _InterleaveDurationString;
        private string _InterleavePreload;
        private string _InterleavePreloadString;
        private string _Title;
        private string _EncodedLibrary;
        private string _EncodedLibraryString;
        private string _EncodedLibraryName;
        private string _EncodedLibraryVersion;
        private string _EncodedLibraryDate;
        private string _EncodedLibrarySettings;
        private string _Language;
        private string _LanguageString;
        private string _LanguageMore;
        private string _EncodedDate;
        private string _TaggedDate;
        private string _Encryption;
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
                    this._Count="";
                return _Count;
            }
            set
            {
                this._Count=value;
            }
        }

        ///<summary> Count of streams of that kind available </summary>
        public string StreamCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamCount))
                    this._StreamCount="";
                return _StreamCount;
            }
            set
            {
                this._StreamCount=value;
            }
        }

        ///<summary> Stream name </summary>
        public string StreamKind
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKind))
                    this._StreamKind="";
                return _StreamKind;
            }
            set
            {
                this._StreamKind=value;
            }
        }

        ///<summary> Stream name string formated</summary>
        public string StreamKindString
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKindString))
                    this._StreamKindString = "";
                return _StreamKindString;
            }
            set
            {
                this._StreamKindString = value;
            }
        }

        ///<summary> id of the stream </summary>
        public string StreamKindID
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKindID))
                    this._StreamKindID="";
                return _StreamKindID;
            }
            set
            {
                this._StreamKindID=value;
            }
        }

        ///<summary> When multiple streams, number of the stream </summary>
        public string StreamKindPos
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamKindPos))
                    this._StreamKindPos = "";
                return _StreamKindPos;
            }
            set
            {
                this._StreamKindPos = value;
            }
        }

        ///<summary> Last   Inform   call </summary>
        public string Inform
        {
            get
            {
                if (String.IsNullOrEmpty(this._Inform))
                    this._Inform="";
                return _Inform;
            }
            set
            {
                this._Inform=value;
            }
        }

        ///<summary> A ID for the stream </summary>
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this._ID))
                    this._ID="";
                return _ID;
            }
            set
            {
                this._ID=value;
            }
        }

        ///<summary> A ID for the stream  string formated</summary>
        public string IDString
        {
            get
            {
                if (String.IsNullOrEmpty(this._IDString))
                    this._IDString = "";
                return _IDString;
            }
            set
            {
                this._IDString = value;
            }
        }

        ///<summary> A unique ID for this stream, should be copied with stream copy </summary>
        public string UniqueID
        {
            get
            {
                if (String.IsNullOrEmpty(this._UniqueID))
                    this._UniqueID="";
                return _UniqueID;
            }
            set
            {
                this._UniqueID=value;
            }
        }

        ///<summary> the Menu ID for this stream</summary>
        public string MenuID
        {
            get
            {
                if (String.IsNullOrEmpty(this._MenuID))
                    this._MenuID = "";
                return _MenuID;
            }
            set
            {
                this._MenuID = value;
            }
        }

        ///<summary> the Menu ID for this stream string formated</summary>
        public string MenuIDString
        {
            get
            {
                if (String.IsNullOrEmpty(this._MenuIDString))
                    this._MenuIDString = "";
                return _MenuIDString;
            }
            set
            {
                this._MenuIDString = value;
            }
        }

        ///<summary> the Format used</summary>
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

        ///<summary> Info about the Format used</summary>
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

        ///<summary> Webpage of the Format</summary>
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

        ///<summary> the Version of the Format used</summary>
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

        ///<summary> the Profile of the Format used</summary>
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

        ///<summary> the Settings of the Format used</summary>
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

        ///<summary> the SBR flag</summary>
        public string FormatSettingsSBR
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsSBR))
                    this._FormatSettingsSBR = "";
                return _FormatSettingsSBR;
            }
            set
            {
                this._FormatSettingsSBR = value;
            }
        }

        ///<summary> the SBR flag set as string</summary>
        public string FormatSettingsSBRString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsSBRString))
                    this._FormatSettingsSBRString = "";
                return _FormatSettingsSBRString;
            }
            set
            {
                this._FormatSettingsSBRString = value;
            }
        }

        ///<summary> the PS flag</summary>
        public string FormatSettingsPS
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsPS))
                    this._FormatSettingsPS = "";
                return _FormatSettingsPS;
            }
            set
            {
                this._FormatSettingsPS = value;
            }
        }

        ///<summary> the PS flag set as string</summary>
        public string FormatSettingsPSString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsPSString))
                    this._FormatSettingsPSString = "";
                return _FormatSettingsPSString;
            }
            set
            {
                this._FormatSettingsPSString = value;
            }
        }

        ///<summary> the Floor used in the stream</summary>
        public string FormatSettingsFloor
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsFloor))
                    this._FormatSettingsFloor = "";
                return _FormatSettingsFloor;
            }
            set
            {
                this._FormatSettingsFloor = value;
            }
        }

        ///<summary> the Firm used in the settings</summary>
        public string FormatSettingsFirm
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsFirm))
                    this._FormatSettingsFirm = "";
                return _FormatSettingsFirm;
            }
            set
            {
                this._FormatSettingsFirm = value;
            }
        }

        ///<summary> the Endianness used in the stream</summary>
        public string FormatSettingsEndianness
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsEndianness))
                    this._FormatSettingsEndianness = "";
                return _FormatSettingsEndianness;
            }
            set
            {
                this._FormatSettingsEndianness = value;
            }
        }

        ///<summary> the Sign used in the stream</summary>
        public string FormatSettingsSign
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsSign))
                    this._FormatSettingsSign = "";
                return _FormatSettingsSign;
            }
            set
            {
                this._FormatSettingsSign = value;
            }
        }

        ///<summary> the Law used in the stream</summary>
        public string FormatSettingsLaw
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsLaw))
                    this._FormatSettingsLaw = "";
                return _FormatSettingsLaw;
            }
            set
            {
                this._FormatSettingsLaw = value;
            }
        }

        ///<summary> the ITU Format used in the stream</summary>
        public string FormatSettingsITU
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatSettingsITU))
                    this._FormatSettingsITU = "";
                return _FormatSettingsITU;
            }
            set
            {
                this._FormatSettingsITU = value;
            }
        }

        ///<summary> how the stream has been muxed </summary>
        public string MuxingMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._MuxingMode))
                    this._MuxingMode = "";
                return _MuxingMode;
            }
            set
            {
                this._MuxingMode = value;
            }
        }

        ///<summary> the ID of the Codec, found in the container </summary>
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

        ///<summary> Info about the CodecID </summary>
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

        ///<summary> the Hint/popular name for the CodecID  </summary>
        public string CodecIDHint
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDHint))
                    this._CodecIDHint = "";
                return _CodecIDHint;
            }
            set
            {
                this._CodecIDHint = value;
            }
        }

        ///<summary> homepage for more details about the CodecID </summary>
        public string CodecIDUrl
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDUrl))
                    this._CodecIDUrl = "";
                return _CodecIDUrl;
            }
            set
            {
                this._CodecIDUrl = value;
            }
        }

        ///<summary> the description of the Codec ID </summary>
        public string CodecIDDescription
        {
            get
            {
                if (String.IsNullOrEmpty(this._CodecIDDescription))
                    this._CodecIDDescription = "";
                return _CodecIDDescription;
            }
            set
            {
                this._CodecIDDescription = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (String.IsNullOrEmpty(this._Title))
                    this._Title="";
                return _Title;
            }
            set
            {
                this._Title=value;
            }
        }

        ///<summary> Bit rate in bps </summary>
        public string BitRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRate))
                    this._BitRate="";
                return _BitRate;
            }
            set
            {
                this._BitRate=value;
            }
        }

        ///<summary> Bit rate (with measurement) </summary>
        public string BitRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateString))
                    this._BitRateString="";
                return _BitRateString;
            }
            set
            {
                this._BitRateString=value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) </summary>
        public string BitRateMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMode))
                    this._BitRateMode="";
                return _BitRateMode;
            }
            set
            {
                this._BitRateMode=value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) formated as string </summary>
        public string BitRateModeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateModeString))
                    this._BitRateModeString = "";
                return _BitRateModeString;
            }
            set
            {
                this._BitRateModeString = value;
            }
        }

        ///<summary> Minimum Bit rate mode </summary>
        public string BitRateMinimum
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMinimum))
                    this._BitRateMinimum = "";
                return _BitRateMinimum;
            }
            set
            {
                this._BitRateMinimum = value;
            }
        }

        ///<summary> Minimum Bit rate mode formated as string </summary>
        public string BitRateMinimumString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMinimumString))
                    this._BitRateMinimumString = "";
                return _BitRateMinimumString;
            }
            set
            {
                this._BitRateMinimumString = value;
            }
        }

        ///<summary> Nominal Bit rate </summary>
        public string BitRateNominal
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateNominal))
                    this._BitRateNominal = "";
                return _BitRateNominal;
            }
            set
            {
                this._BitRateNominal = value;
            }
        }

        ///<summary> Nominal Bit rate formated as string </summary>
        public string BitRateNominalString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateNominalString))
                    this._BitRateNominalString = "";
                return _BitRateNominalString;
            }
            set
            {
                this._BitRateNominalString = value;
            }
        }

        ///<summary> Max Bit rate </summary>
        public string BitRateMaximum
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMaximum))
                    this._BitRateMaximum = "";
                return _BitRateMaximum;
            }
            set
            {
                this._BitRateMaximum = value;
            }
        }

        ///<summary> Max Bit rate formated as string </summary>
        public string BitRateMaximumString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitRateMaximumString))
                    this._BitRateMaximumString = "";
                return _BitRateMaximumString;
            }
            set
            {
                this._BitRateMaximumString = value;
            }
        }

        ///<summary> Number of channels </summary>
        public string Channels
        {
            get
            {
                if (String.IsNullOrEmpty(this._Channels))
                    this._Channels="";
                return _Channels;
            }
            set
            {
                this._Channels=value;
            }
        }

        ///<summary> Number of channels </summary>
        public string ChannelsString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelsString))
                    this._ChannelsString="";
                return _ChannelsString;
            }
            set
            {
                this._ChannelsString=value;
            }
        }

        ///<summary> Positions of channels </summary>
        public string ChannelPositions
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelPositions))
                    this._ChannelPositions = "";
                return _ChannelPositions;
            }
            set
            {
                this._ChannelPositions = value;
            }
        }

        ///<summary> Positions of channels (x/y.z format) </summary>
        public string ChannelPositionsString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelPositionsString2))
                    this._ChannelPositionsString2 = "";
                return _ChannelPositionsString2;
            }
            set
            {
                this._ChannelPositionsString2 = value;
            }
        }

        ///<summary> Channel mode </summary>
        public string ChannelMode
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChannelMode))
                    this._ChannelMode="";
                return _ChannelMode;
            }
            set
            {
                this._ChannelMode=value;
            }
        }

        ///<summary> Sampling rate </summary>
        public string SamplingRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._SamplingRate))
                    this._SamplingRate="";
                return _SamplingRate;
            }
            set
            {
                this._SamplingRate=value;
            }
        }

        ///<summary> in KHz </summary>
        public string SamplingRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._SamplingRateString))
                    this._SamplingRateString="";
                return _SamplingRateString;
            }
            set
            {
                this._SamplingRateString=value;
            }
        }

        ///<summary> Frame count </summary>
        public string SamplingCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._SamplingCount))
                    this._SamplingCount="";
                return _SamplingCount;
            }
            set
            {
                this._SamplingCount=value;
            }
        }

        ///<summary> BitDepth in bits (8, 16, 20, 24) </summary>
        public string BitDepth
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitDepth))
                    this._BitDepth="";
                return _BitDepth;
            }
            set
            {
                this._BitDepth=value;
            }
        }

        ///<summary> BitDepth in bits (8, 16, 20, 24) formated as string </summary>
        public string BitDepthString
        {
            get
            {
                if (String.IsNullOrEmpty(this._BitDepthString))
                    this._BitDepthString = "";
                return _BitDepthString;
            }
            set
            {
                this._BitDepthString = value;
            }
        }

        ///<summary> Current Stream Size divided by uncompressed Stream Size </summary>
        public string CompressionRatio
        {
            get
            {
                if (String.IsNullOrEmpty(this._CompressionRatio))
                    this._CompressionRatio = "";
                return _CompressionRatio;
            }
            set
            {
                this._CompressionRatio = value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) </summary>
        public string Delay
        {
            get
            {
                if (String.IsNullOrEmpty(this._Delay))
                    this._Delay="";
                return _Delay;
            }
            set
            {
                this._Delay=value;
            }
        }

        ///<summary> Delay in format : XXx YYy only, YYy omitted if zero </summary>
        public string DelayString
        {
            get
            {
                if (String.IsNullOrEmpty(this._DelayString))
                    this._DelayString = "";
                return _DelayString;
            }
            set
            {
                this._DelayString = value;
            }
        }

        ///<summary> Delay in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DelayString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._DelayString1))
                    this._DelayString1 = "";
                return _DelayString1;
            }
            set
            {
                this._DelayString1 = value;
            }
        }

        ///<summary> Delay in format : XXx YYy only, YYy omitted if zero </summary>
        public string DelayString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._DelayString2))
                    this._DelayString2 = "";
                return _DelayString2;
            }
            set
            {
                this._DelayString2 = value;
            }
        }

        ///<summary> Delay in format : HH:MM:SS.MMM </summary>
        public string DelayString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._DelayString3))
                    this._DelayString3 = "";
                return _DelayString3;
            }
            set
            {
                this._DelayString3 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) </summary>
        public string VideoDelay
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoDelay))
                    this._VideoDelay="";
                return _VideoDelay;
            }
            set
            {
                this._VideoDelay=value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoDelayString))
                    this._VideoDelayString = "";
                return _VideoDelayString;
            }
            set
            {
                this._VideoDelayString = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoDelayString1))
                    this._VideoDelayString1 = "";
                return _VideoDelayString1;
            }
            set
            {
                this._VideoDelayString1 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoDelayString2))
                    this._VideoDelayString2 = "";
                return _VideoDelayString2;
            }
            set
            {
                this._VideoDelayString2 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoDelayString3))
                    this._VideoDelayString3 = "";
                return _VideoDelayString3;
            }
            set
            {
                this._VideoDelayString3 = value;
            }
        }

        ///<summary> Play time of the stream </summary>
        public string Duration
        {
            get
            {
                if (String.IsNullOrEmpty(this._Duration))
                    this._Duration="";
                return _Duration;
            }
            set
            {
                this._Duration = value;
            }
        }

        ///<summary> Play time (formated) </summary>
        public string DurationString
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString))
                    this._DurationString="";
                return _DurationString;
            }
            set
            {
                this._DurationString=value;
            }
        }

        ///<summary> Play time in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DurationString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString1))
                    this._DurationString1="";
                return _DurationString1;
            }
            set
            {
                this._DurationString1 = value;
            }
        }

        ///<summary> Play time in format : XXx YYy only, YYy omited if zero </summary>
        public string DurationString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString2))
                    this._DurationString2="";
                return _DurationString2;
            }
            set
            {
                this._DurationString2=value;
            }
        }

        ///<summary> Play time in format : HH:MM:SS.MMM </summary>
        public string DurationString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._DurationString3))
                    this._DurationString3="";
                return _DurationString3;
            }
            set
            {
                this._DurationString3=value;
            }
        }

        ///<summary> The gain to apply to reach 89dB SPL on playback </summary>
        public string ReplayGainGain
        {
            get
            {
                if (String.IsNullOrEmpty(this._ReplayGainGain))
                    this._ReplayGainGain = "";
                return _ReplayGainGain;
            }
            set
            {
                this._ReplayGainGain = value;
            }
        }

        ///<summary> The gain to apply to reach 89dB SPL on playback formated as string </summary>
        public string ReplayGainGainString
        {
            get
            {
                if (String.IsNullOrEmpty(this._ReplayGainGainString))
                    this._ReplayGainGainString = "";
                return _ReplayGainGainString;
            }
            set
            {
                this._ReplayGainGainString = value;
            }
        }

        ///<summary> The maximum absolute peak value of the item </summary>
        public string ReplayGainPeak
        {
            get
            {
                if (String.IsNullOrEmpty(this._ReplayGainPeak))
                    this._ReplayGainPeak = "";
                return _ReplayGainPeak;
            }
            set
            {
                this._ReplayGainPeak = value;
            }
        }

        ///<summary> Streamsize in bytes </summary>
        public string StreamSize
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSize))
                    this._StreamSize = "";
                return _StreamSize;
            }
            set
            {
                this._StreamSize = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString))
                    this._StreamSizeString = "";
                return _StreamSizeString;
            }
            set
            {
                this._StreamSizeString = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString1))
                    this._StreamSizeString1 = "";
                return _StreamSizeString1;
            }
            set
            {
                this._StreamSizeString1 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString2))
                    this._StreamSizeString2 = "";
                return _StreamSizeString2;
            }
            set
            {
                this._StreamSizeString2 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString3))
                    this._StreamSizeString3 = "";
                return _StreamSizeString3;
            }
            set
            {
                this._StreamSizeString3 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString4
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString4))
                    this._StreamSizeString4 = "";
                return _StreamSizeString4;
            }
            set
            {
                this._StreamSizeString4 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString5
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeString5))
                    this._StreamSizeString5 = "";
                return _StreamSizeString5;
            }
            set
            {
                this._StreamSizeString5 = value;
            }
        }

        ///<summary> Stream size divided by file size </summary>
        public string StreamSizeProportion
        {
            get
            {
                if (String.IsNullOrEmpty(this._StreamSizeProportion))
                    this._StreamSizeProportion = "";
                return _StreamSizeProportion;
            }
            set
            {
                this._StreamSizeProportion = value;
            }
        }

        ///<summary> How this stream file is aligned in the container </summary>
        public string Alignment
        {
            get
            {
                if (String.IsNullOrEmpty(this._Alignment))
                    this._Alignment = "";
                return _Alignment;
            }
            set
            {
                this._Alignment = value;
            }
        }

        ///<summary> Where this stream file is aligned in the container </summary>
        public string AlignmentString
        {
            get
            {
                if (String.IsNullOrEmpty(this._AlignmentString))
                    this._AlignmentString = "";
                return _AlignmentString;
            }
            set
            {
                this._AlignmentString = value;
            }
        }

        ///<summary> Between how many video frames the stream is inserted </summary>
        public string InterleaveVideoFrames
        {
            get
            {
                if (String.IsNullOrEmpty(this._InterleaveVideoFrames))
                    this._InterleaveVideoFrames = "";
                return _InterleaveVideoFrames;
            }
            set
            {
                this._InterleaveVideoFrames = value;
            }
        }

        ///<summary> Between how much time (ms) the stream is inserted </summary>
        public string InterleaveDuration
        {
            get
            {
                if (String.IsNullOrEmpty(this._InterleaveDuration))
                    this._InterleaveDuration = "";
                return _InterleaveDuration;
            }
            set
            {
                this._InterleaveDuration = value;
            }
        }

        ///<summary> Between how much time and video frames the stream is inserted (with measurement) </summary>
        public string InterleaveDurationString
        {
            get
            {
                if (String.IsNullOrEmpty(this._InterleaveDurationString))
                    this._InterleaveDurationString = "";
                return _InterleaveDurationString;
            }
            set
            {
                this._InterleaveDurationString = value;
            }
        }

        ///<summary> How much time is buffered before the first video frame </summary>
        public string InterleavePreload
        {
            get
            {
                if (String.IsNullOrEmpty(this._InterleavePreload))
                    this._InterleavePreload = "";
                return _InterleavePreload;
            }
            set
            {
                this._InterleavePreload = value;
            }
        }

        ///<summary> How much time is buffered before the first video frame (with measurement) </summary>
        public string InterleavePreloadString
        {
            get
            {
                if (String.IsNullOrEmpty(this._InterleavePreloadString))
                    this._InterleavePreloadString = "";
                return _InterleavePreloadString;
            }
            set
            {
                this._InterleavePreloadString = value;
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

        ///<summary> Software used to create the file formated as string </summary>
        public string EncodedLibraryString
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibraryString))
                    this._EncodedLibraryString = "";
                return _EncodedLibraryString;
            }
            set
            {
                this._EncodedLibraryString = value;
            }
        }

        ///<summary> Name of the Software used to create the file </summary>
        public string EncodedLibraryName
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibraryName))
                    this._EncodedLibraryName = "";
                return _EncodedLibraryName;
            }
            set
            {
                this._EncodedLibraryName = value;
            }
        }


        ///<summary> Version of the Software used to create the file </summary>
        public string EncodedLibraryVersion
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibraryVersion))
                    this._EncodedLibraryVersion = "";
                return _EncodedLibraryVersion;
            }
            set
            {
                this._EncodedLibraryVersion = value;
            }
        }

        ///<summary> Date of the Software used to create the file </summary>
        public string EncodedLibraryDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedLibraryDate))
                    this._EncodedLibraryDate = "";
                return _EncodedLibraryDate;
            }
            set
            {
                this._EncodedLibraryDate = value;
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

        ///<summary> Language (2 letters) </summary>
        public string Language
        {
            get
            {
                if (String.IsNullOrEmpty(this._Language))
                    this._Language="";
                return _Language;
            }
            set
            {
                this._Language=value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageString))
                    this._LanguageString="";
                return _LanguageString;
            }
            set
            {
                this._LanguageString=value;
            }
        }

        ///<summary> More info about Language (director s comment...) </summary>
        public string LanguageMore
        {
            get
            {
                if (String.IsNullOrEmpty(this._LanguageMore))
                    this._LanguageMore="";
                return _LanguageMore;
            }
            set
            {
                this._LanguageMore=value;
            }
        }

        ///<summary> UTC time that the encoding of this item was completed began. </summary>
        public string EncodedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedDate))
                    this._EncodedDate = "";
                return _EncodedDate;
            }
            set
            {
                this._EncodedDate = value;
            }
        }

        ///<summary> UTC time that the tags were done for this item. </summary>
        public string TaggedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._TaggedDate))
                    this._TaggedDate = "";
                return _TaggedDate;
            }
            set
            {
                this._TaggedDate = value;
            }
        }

        ///<summary> encryption string. </summary>
        public string Encryption
        {
            get
            {
                if (String.IsNullOrEmpty(this._Encryption))
                    this._Encryption = "";
                return _Encryption;
            }
            set
            {
                this._Encryption = value;
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
