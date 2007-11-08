using System;

namespace MediaInfoWrapper
{
///<summary>Contains properties for a ChaptersTrack </summary>
public class ChaptersTrack
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
private string _CodecUrl;
private string _Total;
private string _Language;
private string _LanguageString;

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


///<summary> Codec used (test) </summary>
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


///<summary> Total number of chapters </summary>
public string Total
{get
 {
if (String.IsNullOrEmpty(this._Total))
this._Total="";
return _Total;
}
set
 {
this._Total=value;
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



}
}
