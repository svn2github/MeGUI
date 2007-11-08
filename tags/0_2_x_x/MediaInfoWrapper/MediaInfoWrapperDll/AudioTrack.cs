using System;

namespace MediaInfoWrapper
{
///<summary>Contains properties for a AudioTrack </summary>
public class AudioTrack
{private string _Count;
private string _StreamCount;
private string _StreamKind;
private string _StreamKindID;
private string _Inform;
private string _ID;
private string _UniqueID;
private string _Title;
private string _Codec;
private string _CodecString;
private string _CodecInfo;
private string _CodecUrl;
private string _BitRate;
private string _BitRateString;
private string _BitRateMode;
private string _EncodedLibrary;
private string _EncodedLibrarySettings;
private string _Channels;
private string _ChannelsString;
private string _ChannelMode;
private string _SamplingRate;
private string _SamplingRateString;
private string _SamplingCount;
private string _Resolution;
private string _Delay;
private string _Video0Delay;
private string _PlayTime;
private string _PlayTimeString;
private string _PlayTimeString1;
private string _PlayTimeString2;
private string _PlayTimeString3;
private string _Language;
private string _LanguageString;
private string _LanguageMore;

///<summary> Count of objects available in this stream </summary>
public string Count
{get
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
{get
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
{get
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


///<summary> When multiple streams, number of the stream </summary>
public string StreamKindID
{get
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


///<summary> Last   Inform   call </summary>
public string Inform
{get
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


///<summary> A ID for this stream in this file </summary>
public string ID
{get
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


///<summary> A unique ID for this stream, should be copied with stream copy </summary>
public string UniqueID
{get
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


///<summary> Name of the track </summary>
public string Title
{get
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


///<summary> Codec used </summary>
public string Codec
{get
 {
if (String.IsNullOrEmpty(this._Codec))
this._Codec="";
return _Codec;
}
set
 {
this._Codec=value;
}
}


///<summary> Codec used (test) </summary>
public string CodecString
{get
 {
if (String.IsNullOrEmpty(this._CodecString))
this._CodecString="";
return _CodecString;
}
set
 {
this._CodecString=value;
}
}


///<summary> Info about codec </summary>
public string CodecInfo
{get
 {
if (String.IsNullOrEmpty(this._CodecInfo))
this._CodecInfo="";
return _CodecInfo;
}
set
 {
this._CodecInfo=value;
}
}


///<summary> Link </summary>
public string CodecUrl
{get
 {
if (String.IsNullOrEmpty(this._CodecUrl))
this._CodecUrl="";
return _CodecUrl;
}
set
 {
this._CodecUrl=value;
}
}


///<summary> Bit rate in bps </summary>
public string BitRate
{get
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
{get
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
{get
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


///<summary> Software used to create the file </summary>
public string EncodedLibrary
{get
 {
if (String.IsNullOrEmpty(this._EncodedLibrary))
this._EncodedLibrary="";
return _EncodedLibrary;
}
set
 {
this._EncodedLibrary=value;
}
}


///<summary> Parameters used by the software </summary>
public string EncodedLibrarySettings
{get
 {
if (String.IsNullOrEmpty(this._EncodedLibrarySettings))
this._EncodedLibrarySettings="";
return _EncodedLibrarySettings;
}
set
 {
this._EncodedLibrarySettings=value;
}
}


///<summary> Number of channels </summary>
public string Channels
{get
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
{get
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


///<summary> Channel mode </summary>
public string ChannelMode
{get
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
{get
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
{get
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
{get
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


///<summary> Resolution in bits (8, 16, 20, 24) </summary>
public string Resolution
{get
 {
if (String.IsNullOrEmpty(this._Resolution))
this._Resolution="";
return _Resolution;
}
set
 {
this._Resolution=value;
}
}


///<summary> Delay fixed in the stream (relative) </summary>
public string Delay
{get
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


///<summary> Delay fixed in the stream (absolute _ video0) </summary>
public string Video0Delay
{get
 {
if (String.IsNullOrEmpty(this._Video0Delay))
this._Video0Delay="";
return _Video0Delay;
}
set
 {
this._Video0Delay=value;
}
}


///<summary> Play time of the stream </summary>
public string PlayTime
{get
 {
if (String.IsNullOrEmpty(this._PlayTime))
this._PlayTime="";
return _PlayTime;
}
set
 {
this._PlayTime=value;
}
}


///<summary> Play time (formated) </summary>
public string PlayTimeString
{get
 {
if (String.IsNullOrEmpty(this._PlayTimeString))
this._PlayTimeString="";
return _PlayTimeString;
}
set
 {
this._PlayTimeString=value;
}
}


///<summary> Play time in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
public string PlayTimeString1
{get
 {
if (String.IsNullOrEmpty(this._PlayTimeString1))
this._PlayTimeString1="";
return _PlayTimeString1;
}
set
 {
this._PlayTimeString1=value;
}
}


///<summary> Play time in format : XXx YYy only, YYy omited if zero </summary>
public string PlayTimeString2
{get
 {
if (String.IsNullOrEmpty(this._PlayTimeString2))
this._PlayTimeString2="";
return _PlayTimeString2;
}
set
 {
this._PlayTimeString2=value;
}
}


///<summary> Play time in format : HH:MM:SS.MMM </summary>
public string PlayTimeString3
{get
 {
if (String.IsNullOrEmpty(this._PlayTimeString3))
this._PlayTimeString3="";
return _PlayTimeString3;
}
set
 {
this._PlayTimeString3=value;
}
}


///<summary> Language (2 letters) </summary>
public string Language
{get
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
{get
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
{get
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



}
}
