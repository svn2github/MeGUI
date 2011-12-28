using System;

namespace MediaInfoWrapper
{
    ///<summary>Contains properties for a GeneralTrack </summary>
    public class GeneralTrack
    {
        private string _Count;
        private string _StreamCount;
        private string _StreamKind;
        private string _StreamKindID;
        private string _Inform;
        private string _ID;
        private string _UniqueID;
        private string _GeneralCount;
        private string _VideoCount;
        private string _AudioCount;
        private string _TextCount;
        private string _ChaptersCount;
        private string _ImageCount;
        private string _CompleteName;
        private string _FolderName;
        private string _FileName;
        private string _FileExtension;
        private string _FileSize;
        private string _FileSizeString;
        private string _FileSizeString1;
        private string _FileSizeString2;
        private string _FileSizeString3;
        private string _FileSizeString4;
        private string _Format;
        private string _FormatString;
        private string _FormatInfo;
        private string _FormatUrl;
        private string _FormatExtensions;
        private string _OveralBitRate;
        private string _OveralBitRateString;
        private string _PlayTime;
        private string _PlayTimeString;
        private string _PlayTimeString1;
        private string _PlayTimeString2;
        private string _PlayTimeString3;
        private string _Title;
        private string _TitleMore;
        private string _Domain;
        private string _Collection;
        private string _CollectionTotalParts;
        private string _Season;
        private string _Movie;
        private string _MovieMore;
        private string _Album;
        private string _AlbumTotalParts;
        private string _AlbumSort;
        private string _Comic;
        private string _ComicTotalParts;
        private string _Part;
        private string _PartTotalParts;
        private string _PartPosition;
        private string _Track;
        private string _TrackPosition;
        private string _TrackMore;
        private string _TrackSort;
        private string _Chapter;
        private string _SubTrack;
        private string _OriginalAlbum;
        private string _OriginalMovie;
        private string _OriginalPart;
        private string _OriginalTrack;
        private string _Author;
        private string _Artist;
        private string _PerformerSort;
        private string _OriginalPerformer;
        private string _Accompaniment;
        private string _MusicianInstrument;
        private string _Composer;
        private string _ComposerNationality;
        private string _Arranger;
        private string _Lyricist;
        private string _OriginalLyricist;
        private string _Conductor;
        private string _Actor;
        private string _ActorCharacter;
        private string _WrittenBy;
        private string _ScreenplayBy;
        private string _Director;
        private string _AssistantDirector;
        private string _DirectorOfPhotography;
        private string _ArtDirector;
        private string _EditedBy;
        private string _Producer;
        private string _CoProducer;
        private string _ExecutiveProducer;
        private string _ProductionDesigner;
        private string _CostumeDesigner;
        private string _Choregrapher;
        private string _SoundEngineer;
        private string _MasteredBy;
        private string _RemixedBy;
        private string _ProductionStudio;
        private string _Publisher;
        private string _PublisherURL;
        private string _DistributedBy;
        private string _EncodedBy;
        private string _ThanksTo;
        private string _Technician;
        private string _CommissionedBy;
        private string _EncodedOriginalDistributedBy;
        private string _RadioStation;
        private string _RadioStationOwner;
        private string _RadioStationURL;
        private string _ContentType;
        private string _Subject;
        private string _Synopsys;
        private string _Summary;
        private string _Description;
        private string _Keywords;
        private string _Period;
        private string _LawRating;
        private string _IRCA;
        private string _Language;
        private string _Medium;
        private string _Product;
        private string _Country;
        private string _WrittenDate;
        private string _RecordedDate;
        private string _ReleasedDate;
        private string _MasteredDate;
        private string _EncodedDate;
        private string _TaggedDate;
        private string _OriginalReleasedDate;
        private string _OriginalRecordedDate;
        private string _WrittenLocation;
        private string _RecordedLocation;
        private string _ArchivalLocation;
        private string _Genre;
        private string _Mood;
        private string _Comment;
        private string _Rating;
        private string _EncodedApplication;
        private string _EncodedLibrary;
        private string _EncodedLibrarySettings;
        private string _EncodedOriginal;
        private string _EncodedOriginalUrl;
        private string _Copyright;
        private string _ProducerCopyright;
        private string _TermsOfUse;
        private string _CopyrightUrl;
        private string _ISRC;
        private string _MSDI;
        private string _ISBN;
        private string _BarCode;
        private string _LCCN;
        private string _CatalogNumber;
        private string _LabelCode;
        private string _Cover;
        private string _CoverDatas;
        private string _BPM;
        private string _VideoCodecList;
        private string _VideoLanguageList;
        private string _AudioCodecList;
        private string _AudioLanguageList;
        private string _TextCodecList;
        private string _TextLanguageList;
        private string _ChaptersCodecList;
        private string _ChaptersLanguageList;
        private string _ImageCodecList;
        private string _ImageLanguageList;
        private string _Other;

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

        ///<summary> Count of video streams </summary>
        public string GeneralCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._GeneralCount))
                    this._GeneralCount = "";
                return _GeneralCount;
            }
            set
            {
                this._GeneralCount = value;
            }
        }

        ///<summary> Count of video streams </summary>
        public string VideoCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoCount))
                    this._VideoCount = "";
                return _VideoCount;
            }
            set
            {
                this._VideoCount = value;
            }
        }

        ///<summary> Count of audio streams </summary>
        public string AudioCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._AudioCount))
                    this._AudioCount = "";
                return _AudioCount;
            }
            set
            {
                this._AudioCount = value;
            }
        }

        ///<summary> Count of text streams </summary>
        public string TextCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._TextCount))
                    this._TextCount = "";
                return _TextCount;
            }
            set
            {
                this._TextCount = value;
            }
        }

        ///<summary> Count of chapters streams </summary>
        public string ChaptersCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChaptersCount))
                    this._ChaptersCount = "";
                return _ChaptersCount;
            }
            set
            {
                this._ChaptersCount = value;
            }
        }

        ///<summary> Count of image streams </summary>
        public string ImageCount
        {
            get
            {
                if (String.IsNullOrEmpty(this._ImageCount))
                    this._ImageCount = "";
                return _ImageCount;
            }
            set
            {
                this._ImageCount = value;
            }
        }

        ///<summary> Complete name (Folder+Name+Extension) </summary>
        public string CompleteName
        {
            get
            {
                if (String.IsNullOrEmpty(this._CompleteName))
                    this._CompleteName = "";
                return _CompleteName;
            }
            set
            {
                this._CompleteName = value;
            }
        }

        ///<summary> Folder name only </summary>
        public string FolderName
        {
            get
            {
                if (String.IsNullOrEmpty(this._FolderName))
                    this._FolderName = "";
                return _FolderName;
            }
            set
            {
                this._FolderName = value;
            }
        }

        ///<summary> File name only </summary>
        public string FileName
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileName))
                    this._FileName = "";
                return _FileName;
            }
            set
            {
                this._FileName = value;
            }
        }

        ///<summary> File extension only </summary>
        public string FileExtension
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileExtension))
                    this._FileExtension = "";
                return _FileExtension;
            }
            set
            {
                this._FileExtension = value;
            }
        }

        ///<summary> File size in bytes </summary>
        public string FileSize
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSize))
                    this._FileSize = "";
                return _FileSize;
            }
            set
            {
                this._FileSize = value;
            }
        }

        ///<summary> File size (with measure) </summary>
        public string FileSizeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSizeString))
                    this._FileSizeString = "";
                return _FileSizeString;
            }
            set
            {
                this._FileSizeString = value;
            }
        }

        ///<summary> File size (with measure, 1 digit mini) </summary>
        public string FileSizeString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSizeString1))
                    this._FileSizeString1 = "";
                return _FileSizeString1;
            }
            set
            {
                this._FileSizeString1 = value;
            }
        }

        ///<summary> File size (with measure, 2 digit mini) </summary>
        public string FileSizeString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSizeString2))
                    this._FileSizeString2 = "";
                return _FileSizeString2;
            }
            set
            {
                this._FileSizeString2 = value;
            }
        }

        ///<summary> File size (with measure, 3 digit mini) </summary>
        public string FileSizeString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSizeString3))
                    this._FileSizeString3 = "";
                return _FileSizeString3;
            }
            set
            {
                this._FileSizeString3 = value;
            }
        }

        ///<summary> File size (with measure, 4 digit mini) </summary>
        public string FileSizeString4
        {
            get
            {
                if (String.IsNullOrEmpty(this._FileSizeString4))
                    this._FileSizeString4 = "";
                return _FileSizeString4;
            }
            set
            {
                this._FileSizeString4 = value;
            }
        }

        ///<summary> Format (short name) </summary>
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

        ///<summary> Format (full name) </summary>
        public string FormatString
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatString))
                    this._FormatString = "";
                return _FormatString;
            }
            set
            {
                this._FormatString = value;
            }
        }

        ///<summary> More information about this format </summary>
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

        ///<summary> Url about this format </summary>
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

        ///<summary> Known extensions of format </summary>
        public string FormatExtensions
        {
            get
            {
                if (String.IsNullOrEmpty(this._FormatExtensions))
                    this._FormatExtensions = "";
                return _FormatExtensions;
            }
            set
            {
                this._FormatExtensions = value;
            }
        }

        ///<summary> BitRate of all streams </summary>
        public string OveralBitRate
        {
            get
            {
                if (String.IsNullOrEmpty(this._OveralBitRate))
                    this._OveralBitRate = "";
                return _OveralBitRate;
            }
            set
            {
                this._OveralBitRate = value;
            }
        }

        ///<summary> BitRate of all streams (with measure) </summary>
        public string OveralBitRateString
        {
            get
            {
                if (String.IsNullOrEmpty(this._OveralBitRateString))
                    this._OveralBitRateString = "";
                return _OveralBitRateString;
            }
            set
            {
                this._OveralBitRateString = value;
            }
        }

        ///<summary> Play time of the file </summary>
        public string PlayTime
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTime))
                    this._PlayTime = "";
                return _PlayTime;
            }
            set
            {
                this._PlayTime = value;
            }
        }

        ///<summary> Play time in format : XXx YYy only, YYy omited if zero </summary>
        public string PlayTimeString
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTimeString))
                    this._PlayTimeString = "";
                return _PlayTimeString;
            }
            set
            {
                this._PlayTimeString = value;
            }
        }

        ///<summary> Play time in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string PlayTimeString1
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTimeString1))
                    this._PlayTimeString1 = "";
                return _PlayTimeString1;
            }
            set
            {
                this._PlayTimeString1 = value;
            }
        }

        ///<summary> Play time in format : XXx YYy only, YYy omited if zero </summary>
        public string PlayTimeString2
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTimeString2))
                    this._PlayTimeString2 = "";
                return _PlayTimeString2;
            }
            set
            {
                this._PlayTimeString2 = value;
            }
        }

        ///<summary> Play time in format : HH:MM:SS.MMM </summary>
        public string PlayTimeString3
        {
            get
            {
                if (String.IsNullOrEmpty(this._PlayTimeString3))
                    this._PlayTimeString3 = "";
                return _PlayTimeString3;
            }
            set
            {
                this._PlayTimeString3 = value;
            }
        }

        ///<summary> (Generic)Title of file </summary>
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

        ///<summary> (Generic)More info about the title of file </summary>
        public string TitleMore
        {
            get
            {
                if (String.IsNullOrEmpty(this._TitleMore))
                    this._TitleMore = "";
                return _TitleMore;
            }
            set
            {
                this._TitleMore = value;
            }
        }

        ///<summary> Level=8. Eg : Starwars, Stargate, U2 </summary>
        public string Domain
        {
            get
            {
                if (String.IsNullOrEmpty(this._Domain))
                    this._Domain = "";
                return _Domain;
            }
            set
            {
                this._Domain = value;
            }
        }

        ///<summary> Level=7. Name of the collection. Eg : Starwars movies, Stargate movie, Stargate SG-1, Stargate Atlantis </summary>
        public string Collection
        {
            get
            {
                if (String.IsNullOrEmpty(this._Collection))
                    this._Collection = "";
                return _Collection;
            }
            set
            {
                this._Collection = value;
            }
        }

        ///<summary> Total count of seasons </summary>
        public string CollectionTotalParts
        {
            get
            {
                if (String.IsNullOrEmpty(this._CollectionTotalParts))
                    this._CollectionTotalParts = "";
                return _CollectionTotalParts;
            }
            set
            {
                this._CollectionTotalParts = value;
            }
        }

        ///<summary> Level=6. Name of the season. Eg : Strawars first Trilogy, Season 1 </summary>
        public string Season
        {
            get
            {
                if (String.IsNullOrEmpty(this._Season))
                    this._Season = "";
                return _Season;
            }
            set
            {
                this._Season = value;
            }
        }

        ///<summary> Level=5. Name of the movie. Eg : Starwars, a new hope </summary>
        public string Movie
        {
            get
            {
                if (String.IsNullOrEmpty(this._Movie))
                    this._Movie = "";
                return _Movie;
            }
            set
            {
                this._Movie = value;
            }
        }

        ///<summary> More information about this movie </summary>
        public string MovieMore
        {
            get
            {
                if (String.IsNullOrEmpty(this._MovieMore))
                    this._MovieMore = "";
                return _MovieMore;
            }
            set
            {
                this._MovieMore = value;
            }
        }

        ///<summary> Level=5. Name of album. Eg : The joshua tree </summary>
        public string Album
        {
            get
            {
                if (String.IsNullOrEmpty(this._Album))
                    this._Album = "";
                return _Album;
            }
            set
            {
                this._Album = value;
            }
        }

        ///<summary> Total number of track or parts (depend if there is a part) </summary>
        public string AlbumTotalParts
        {
            get
            {
                if (String.IsNullOrEmpty(this._AlbumTotalParts))
                    this._AlbumTotalParts = "";
                return _AlbumTotalParts;
            }
            set
            {
                this._AlbumTotalParts = value;
            }
        }

        ///<summary> Sort order </summary>
        public string AlbumSort
        {
            get
            {
                if (String.IsNullOrEmpty(this._AlbumSort))
                    this._AlbumSort = "";
                return _AlbumSort;
            }
            set
            {
                this._AlbumSort = value;
            }
        }

        ///<summary> Level=5. Name of the comic. </summary>
        public string Comic
        {
            get
            {
                if (String.IsNullOrEmpty(this._Comic))
                    this._Comic = "";
                return _Comic;
            }
            set
            {
                this._Comic = value;
            }
        }

        ///<summary> Total number of pages in the comic </summary>
        public string ComicTotalParts
        {
            get
            {
                if (String.IsNullOrEmpty(this._ComicTotalParts))
                    this._ComicTotalParts = "";
                return _ComicTotalParts;
            }
            set
            {
                this._ComicTotalParts = value;
            }
        }

        ///<summary> Level=4. Name of the part. Eg : CD1, CD2 </summary>
        public string Part
        {
            get
            {
                if (String.IsNullOrEmpty(this._Part))
                    this._Part = "";
                return _Part;
            }
            set
            {
                this._Part = value;
            }
        }

        ///<summary> Total count of track or episode </summary>
        public string PartTotalParts
        {
            get
            {
                if (String.IsNullOrEmpty(this._PartTotalParts))
                    this._PartTotalParts = "";
                return _PartTotalParts;
            }
            set
            {
                this._PartTotalParts = value;
            }
        }

        ///<summary> Part number </summary>
        public string PartPosition
        {
            get
            {
                if (String.IsNullOrEmpty(this._PartPosition))
                    this._PartPosition = "";
                return _PartPosition;
            }
            set
            {
                this._PartPosition = value;
            }
        }

        ///<summary> Level=3. Name of the track. Eg : track1, track 2 </summary>
        public string Track
        {
            get
            {
                if (String.IsNullOrEmpty(this._Track))
                    this._Track = "";
                return _Track;
            }
            set
            {
                this._Track = value;
            }
        }

        ///<summary> Track number or episode number... </summary>
        public string TrackPosition
        {
            get
            {
                if (String.IsNullOrEmpty(this._TrackPosition))
                    this._TrackPosition = "";
                return _TrackPosition;
            }
            set
            {
                this._TrackPosition = value;
            }
        }

        ///<summary> More information about this track (subtitle) </summary>
        public string TrackMore
        {
            get
            {
                if (String.IsNullOrEmpty(this._TrackMore))
                    this._TrackMore = "";
                return _TrackMore;
            }
            set
            {
                this._TrackMore = value;
            }
        }

        ///<summary> Sort order </summary>
        public string TrackSort
        {
            get
            {
                if (String.IsNullOrEmpty(this._TrackSort))
                    this._TrackSort = "";
                return _TrackSort;
            }
            set
            {
                this._TrackSort = value;
            }
        }

        ///<summary> Level=3. Name of the chapter. </summary>
        public string Chapter
        {
            get
            {
                if (String.IsNullOrEmpty(this._Chapter))
                    this._Chapter = "";
                return _Chapter;
            }
            set
            {
                this._Chapter = value;
            }
        }

        ///<summary> Level=2, Name of the subtrack. </summary>
        public string SubTrack
        {
            get
            {
                if (String.IsNullOrEmpty(this._SubTrack))
                    this._SubTrack = "";
                return _SubTrack;
            }
            set
            {
                this._SubTrack = value;
            }
        }

        ///<summary> Original name of album, serie... </summary>
        public string OriginalAlbum
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalAlbum))
                    this._OriginalAlbum = "";
                return _OriginalAlbum;
            }
            set
            {
                this._OriginalAlbum = value;
            }
        }

        ///<summary> Original name of the movie </summary>
        public string OriginalMovie
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalMovie))
                    this._OriginalMovie = "";
                return _OriginalMovie;
            }
            set
            {
                this._OriginalMovie = value;
            }
        }

        ///<summary> Name of the part in the original support </summary>
        public string OriginalPart
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalPart))
                    this._OriginalPart = "";
                return _OriginalPart;
            }
            set
            {
                this._OriginalPart = value;
            }
        }

        ///<summary> Original name of the track in the original support </summary>
        public string OriginalTrack
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalTrack))
                    this._OriginalTrack = "";
                return _OriginalTrack;
            }
            set
            {
                this._OriginalTrack = value;
            }
        }

        ///<summary> (Generic)Performer of the file </summary>
        public string Author
        {
            get
            {
                if (String.IsNullOrEmpty(this._Author))
                    this._Author = "";
                return _Author;
            }
            set
            {
                this._Author = value;
            }
        }

        ///<summary> Duplicate of Performer </summary>
        public string Artist
        {
            get
            {
                if (String.IsNullOrEmpty(this._Artist))
                    this._Artist = "";
                return _Artist;
            }
            set
            {
                this._Artist = value;
            }
        }

        ///<summary> Sort order </summary>
        public string PerformerSort
        {
            get
            {
                if (String.IsNullOrEmpty(this._PerformerSort))
                    this._PerformerSort = "";
                return _PerformerSort;
            }
            set
            {
                this._PerformerSort = value;
            }
        }

        ///<summary> Original artist(s)_performer(s). </summary>
        public string OriginalPerformer
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalPerformer))
                    this._OriginalPerformer = "";
                return _OriginalPerformer;
            }
            set
            {
                this._OriginalPerformer = value;
            }
        }

        ///<summary> Band_orchestra_accompaniment_musician. </summary>
        public string Accompaniment
        {
            get
            {
                if (String.IsNullOrEmpty(this._Accompaniment))
                    this._Accompaniment = "";
                return _Accompaniment;
            }
            set
            {
                this._Accompaniment = value;
            }
        }

        ///<summary> Name of the character an actor or actress plays in this movie. </summary>
        public string MusicianInstrument
        {
            get
            {
                if (String.IsNullOrEmpty(this._MusicianInstrument))
                    this._MusicianInstrument = "";
                return _MusicianInstrument;
            }
            set
            {
                this._MusicianInstrument = value;
            }
        }

        ///<summary> Name of the original composer. </summary>
        public string Composer
        {
            get
            {
                if (String.IsNullOrEmpty(this._Composer))
                    this._Composer = "";
                return _Composer;
            }
            set
            {
                this._Composer = value;
            }
        }

        ///<summary> Nationality of the main composer of the item, mostly for classical music. </summary>
        public string ComposerNationality
        {
            get
            {
                if (String.IsNullOrEmpty(this._ComposerNationality))
                    this._ComposerNationality = "";
                return _ComposerNationality;
            }
            set
            {
                this._ComposerNationality = value;
            }
        }

        ///<summary> The person who arranged the piece. e.g. Ravel. </summary>
        public string Arranger
        {
            get
            {
                if (String.IsNullOrEmpty(this._Arranger))
                    this._Arranger = "";
                return _Arranger;
            }
            set
            {
                this._Arranger = value;
            }
        }

        ///<summary> The person who wrote the lyrics for a musical item. </summary>
        public string Lyricist
        {
            get
            {
                if (String.IsNullOrEmpty(this._Lyricist))
                    this._Lyricist = "";
                return _Lyricist;
            }
            set
            {
                this._Lyricist = value;
            }
        }

        ///<summary> Original lyricist(s)_text writer(s). </summary>
        public string OriginalLyricist
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalLyricist))
                    this._OriginalLyricist = "";
                return _OriginalLyricist;
            }
            set
            {
                this._OriginalLyricist = value;
            }
        }

        ///<summary> The artist(s) who performed the work. In classical music this would be the conductor, orchestra, soloists. </summary>
        public string Conductor
        {
            get
            {
                if (String.IsNullOrEmpty(this._Conductor))
                    this._Conductor = "";
                return _Conductor;
            }
            set
            {
                this._Conductor = value;
            }
        }

        ///<summary> Real name of actor or actress playing a role in the movie. </summary>
        public string Actor
        {
            get
            {
                if (String.IsNullOrEmpty(this._Actor))
                    this._Actor = "";
                return _Actor;
            }
            set
            {
                this._Actor = value;
            }
        }

        ///<summary> Name of the character an actor or actress plays in this movie. </summary>
        public string ActorCharacter
        {
            get
            {
                if (String.IsNullOrEmpty(this._ActorCharacter))
                    this._ActorCharacter = "";
                return _ActorCharacter;
            }
            set
            {
                this._ActorCharacter = value;
            }
        }

        ///<summary> The author of the story or script. </summary>
        public string WrittenBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._WrittenBy))
                    this._WrittenBy = "";
                return _WrittenBy;
            }
            set
            {
                this._WrittenBy = value;
            }
        }

        ///<summary> The author of the screenplay or scenario (used for movies and TV shows). </summary>
        public string ScreenplayBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._ScreenplayBy))
                    this._ScreenplayBy = "";
                return _ScreenplayBy;
            }
            set
            {
                this._ScreenplayBy = value;
            }
        }

        ///<summary> Name of the director. </summary>
        public string Director
        {
            get
            {
                if (String.IsNullOrEmpty(this._Director))
                    this._Director = "";
                return _Director;
            }
            set
            {
                this._Director = value;
            }
        }

        ///<summary> Name of the assistant director. </summary>
        public string AssistantDirector
        {
            get
            {
                if (String.IsNullOrEmpty(this._AssistantDirector))
                    this._AssistantDirector = "";
                return _AssistantDirector;
            }
            set
            {
                this._AssistantDirector = value;
            }
        }

        ///<summary> The name of the director of photography, also known as cinematographer. </summary>
        public string DirectorOfPhotography
        {
            get
            {
                if (String.IsNullOrEmpty(this._DirectorOfPhotography))
                    this._DirectorOfPhotography = "";
                return _DirectorOfPhotography;
            }
            set
            {
                this._DirectorOfPhotography = value;
            }
        }

        ///<summary> The person who oversees the artists and craftspeople who build the sets. </summary>
        public string ArtDirector
        {
            get
            {
                if (String.IsNullOrEmpty(this._ArtDirector))
                    this._ArtDirector = "";
                return _ArtDirector;
            }
            set
            {
                this._ArtDirector = value;
            }
        }

        ///<summary> Editor name </summary>
        public string EditedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._EditedBy))
                    this._EditedBy = "";
                return _EditedBy;
            }
            set
            {
                this._EditedBy = value;
            }
        }

        ///<summary> Name of the producer of the movie. </summary>
        public string Producer
        {
            get
            {
                if (String.IsNullOrEmpty(this._Producer))
                    this._Producer = "";
                return _Producer;
            }
            set
            {
                this._Producer = value;
            }
        }

        ///<summary> The name of a co-producer. </summary>
        public string CoProducer
        {
            get
            {
                if (String.IsNullOrEmpty(this._CoProducer))
                    this._CoProducer = "";
                return _CoProducer;
            }
            set
            {
                this._CoProducer = value;
            }
        }

        ///<summary> The name of an executive producer. </summary>
        public string ExecutiveProducer
        {
            get
            {
                if (String.IsNullOrEmpty(this._ExecutiveProducer))
                    this._ExecutiveProducer = "";
                return _ExecutiveProducer;
            }
            set
            {
                this._ExecutiveProducer = value;
            }
        }

        ///<summary> Artist responsible for designing the overall visual appearance of a movie. </summary>
        public string ProductionDesigner
        {
            get
            {
                if (String.IsNullOrEmpty(this._ProductionDesigner))
                    this._ProductionDesigner = "";
                return _ProductionDesigner;
            }
            set
            {
                this._ProductionDesigner = value;
            }
        }

        ///<summary> The name of the costume designer. </summary>
        public string CostumeDesigner
        {
            get
            {
                if (String.IsNullOrEmpty(this._CostumeDesigner))
                    this._CostumeDesigner = "";
                return _CostumeDesigner;
            }
            set
            {
                this._CostumeDesigner = value;
            }
        }

        ///<summary> The name of the choregrapher. </summary>
        public string Choregrapher
        {
            get
            {
                if (String.IsNullOrEmpty(this._Choregrapher))
                    this._Choregrapher = "";
                return _Choregrapher;
            }
            set
            {
                this._Choregrapher = value;
            }
        }

        ///<summary> The name of the sound engineer or sound recordist. </summary>
        public string SoundEngineer
        {
            get
            {
                if (String.IsNullOrEmpty(this._SoundEngineer))
                    this._SoundEngineer = "";
                return _SoundEngineer;
            }
            set
            {
                this._SoundEngineer = value;
            }
        }

        ///<summary> The engineer who mastered the content for a physical medium or for digital distribution. </summary>
        public string MasteredBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._MasteredBy))
                    this._MasteredBy = "";
                return _MasteredBy;
            }
            set
            {
                this._MasteredBy = value;
            }
        }

        ///<summary> Interpreted, remixed, or otherwise modified by. </summary>
        public string RemixedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._RemixedBy))
                    this._RemixedBy = "";
                return _RemixedBy;
            }
            set
            {
                this._RemixedBy = value;
            }
        }

        ///<summary>   </summary>
        public string ProductionStudio
        {
            get
            {
                if (String.IsNullOrEmpty(this._ProductionStudio))
                    this._ProductionStudio = "";
                return _ProductionStudio;
            }
            set
            {
                this._ProductionStudio = value;
            }
        }

        ///<summary> Name of the organization producing the track (i.e. the  record label ). </summary>
        public string Publisher
        {
            get
            {
                if (String.IsNullOrEmpty(this._Publisher))
                    this._Publisher = "";
                return _Publisher;
            }
            set
            {
                this._Publisher = value;
            }
        }

        ///<summary> Publishers official webpage. </summary>
        public string PublisherURL
        {
            get
            {
                if (String.IsNullOrEmpty(this._PublisherURL))
                    this._PublisherURL = "";
                return _PublisherURL;
            }
            set
            {
                this._PublisherURL = value;
            }
        }

        ///<summary>   </summary>
        public string DistributedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._DistributedBy))
                    this._DistributedBy = "";
                return _DistributedBy;
            }
            set
            {
                this._DistributedBy = value;
            }
        }

        ///<summary> Name of the person or organisation that encoded_ripped the audio file. </summary>
        public string EncodedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedBy))
                    this._EncodedBy = "";
                return _EncodedBy;
            }
            set
            {
                this._EncodedBy = value;
            }
        }

        ///<summary> A very general tag for everyone else that wants to be listed. </summary>
        public string ThanksTo
        {
            get
            {
                if (String.IsNullOrEmpty(this._ThanksTo))
                    this._ThanksTo = "";
                return _ThanksTo;
            }
            set
            {
                this._ThanksTo = value;
            }
        }

        ///<summary> Technician. Identifies the technician who digitized the subject file. For example, Smith, John. </summary>
        public string Technician
        {
            get
            {
                if (String.IsNullOrEmpty(this._Technician))
                    this._Technician = "";
                return _Technician;
            }
            set
            {
                this._Technician = value;
            }
        }

        ///<summary> Commissioned. Lists the name of the person or organization that commissioned the subject of the file. e.g. Pope Julian II. </summary>
        public string CommissionedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._CommissionedBy))
                    this._CommissionedBy = "";
                return _CommissionedBy;
            }
            set
            {
                this._CommissionedBy = value;
            }
        }

        ///<summary> Source. Identifies the name of the person or organization who supplied the original subject of the file. For example, Trey Research. </summary>
        public string EncodedOriginalDistributedBy
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedOriginalDistributedBy))
                    this._EncodedOriginalDistributedBy = "";
                return _EncodedOriginalDistributedBy;
            }
            set
            {
                this._EncodedOriginalDistributedBy = value;
            }
        }

        ///<summary> Contains the name of the internet radio station from which the audio is streamed. </summary>
        public string RadioStation
        {
            get
            {
                if (String.IsNullOrEmpty(this._RadioStation))
                    this._RadioStation = "";
                return _RadioStation;
            }
            set
            {
                this._RadioStation = value;
            }
        }

        ///<summary> Contains the name of the owner of the internet radio station from which the audio is streamed. </summary>
        public string RadioStationOwner
        {
            get
            {
                if (String.IsNullOrEmpty(this._RadioStationOwner))
                    this._RadioStationOwner = "";
                return _RadioStationOwner;
            }
            set
            {
                this._RadioStationOwner = value;
            }
        }

        ///<summary> Official internet radio station homepage. </summary>
        public string RadioStationURL
        {
            get
            {
                if (String.IsNullOrEmpty(this._RadioStationURL))
                    this._RadioStationURL = "";
                return _RadioStationURL;
            }
            set
            {
                this._RadioStationURL = value;
            }
        }

        ///<summary> The type of the item. e.g. Documentary, Feature Film, Cartoon, Music Video, Music, Sound FX, etc. </summary>
        public string ContentType
        {
            get
            {
                if (String.IsNullOrEmpty(this._ContentType))
                    this._ContentType = "";
                return _ContentType;
            }
            set
            {
                this._ContentType = value;
            }
        }

        ///<summary> Describes the topic of the file, such as Aerial view of Seattle.. </summary>
        public string Subject
        {
            get
            {
                if (String.IsNullOrEmpty(this._Subject))
                    this._Subject = "";
                return _Subject;
            }
            set
            {
                this._Subject = value;
            }
        }

        ///<summary> A description of the story line of the item. </summary>
        public string Synopsys
        {
            get
            {
                if (String.IsNullOrEmpty(this._Synopsys))
                    this._Synopsys = "";
                return _Synopsys;
            }
            set
            {
                this._Synopsys = value;
            }
        }

        ///<summary> A plot outline or a summary of the story. </summary>
        public string Summary
        {
            get
            {
                if (String.IsNullOrEmpty(this._Summary))
                    this._Summary = "";
                return _Summary;
            }
            set
            {
                this._Summary = value;
            }
        }

        ///<summary> A short description of the contents, such as Two birds flying. </summary>
        public string Description
        {
            get
            {
                if (String.IsNullOrEmpty(this._Description))
                    this._Description = "";
                return _Description;
            }
            set
            {
                this._Description = value;
            }
        }

        ///<summary> Keywords to the item separated by a comma, used for searching. </summary>
        public string Keywords
        {
            get
            {
                if (String.IsNullOrEmpty(this._Keywords))
                    this._Keywords = "";
                return _Keywords;
            }
            set
            {
                this._Keywords = value;
            }
        }

        ///<summary> Describes the period that the piece is from or about. e.g. Renaissance. </summary>
        public string Period
        {
            get
            {
                if (String.IsNullOrEmpty(this._Period))
                    this._Period = "";
                return _Period;
            }
            set
            {
                this._Period = value;
            }
        }

        ///<summary> Depending on the country it s the format of the rating of a movie (P, R, X in the USA, an age in other countries or a URI defining a logo). </summary>
        public string LawRating
        {
            get
            {
                if (String.IsNullOrEmpty(this._LawRating))
                    this._LawRating = "";
                return _LawRating;
            }
            set
            {
                this._LawRating = value;
            }
        }

        ///<summary> The ICRA rating. (Previously RSACi) </summary>
        public string IRCA
        {
            get
            {
                if (String.IsNullOrEmpty(this._IRCA))
                    this._IRCA = "";
                return _IRCA;
            }
            set
            {
                this._IRCA = value;
            }
        }

        ///<summary> Language(s) of the item in the bibliographic ISO-639-2 form. </summary>
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

        ///<summary> Medium. Describes the original subject of the file, such as, computer image, drawing, lithograph, and so forth. Not necessarily the same as ISRF. </summary>
        public string Medium
        {
            get
            {
                if (String.IsNullOrEmpty(this._Medium))
                    this._Medium = "";
                return _Medium;
            }
            set
            {
                this._Medium = value;
            }
        }

        ///<summary> Product. Specifies the name of the title the file was originally intended for, such as Encyclopedia of Pacific Northwest Geography. </summary>
        public string Product
        {
            get
            {
                if (String.IsNullOrEmpty(this._Product))
                    this._Product = "";
                return _Product;
            }
            set
            {
                this._Product = value;
            }
        }

        ///<summary> Country </summary>
        public string Country
        {
            get
            {
                if (String.IsNullOrEmpty(this._Country))
                    this._Country = "";
                return _Country;
            }
            set
            {
                this._Country = value;
            }
        }

        ///<summary> The time that the composition of the music_script began. </summary>
        public string WrittenDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._WrittenDate))
                    this._WrittenDate = "";
                return _WrittenDate;
            }
            set
            {
                this._WrittenDate = value;
            }
        }

        ///<summary> The time that the recording began. </summary>
        public string RecordedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._RecordedDate))
                    this._RecordedDate = "";
                return _RecordedDate;
            }
            set
            {
                this._RecordedDate = value;
            }
        }

        ///<summary> The time that the item was originaly released. </summary>
        public string ReleasedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._ReleasedDate))
                    this._ReleasedDate = "";
                return _ReleasedDate;
            }
            set
            {
                this._ReleasedDate = value;
            }
        }

        ///<summary> The time that the item was tranfered to a digitalmedium. </summary>
        public string MasteredDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._MasteredDate))
                    this._MasteredDate = "";
                return _MasteredDate;
            }
            set
            {
                this._MasteredDate = value;
            }
        }

        ///<summary> The time that the encoding of this item was completed began. </summary>
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

        ///<summary> The time that the tags were done for this item. </summary>
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

        ///<summary> Contains a timestamp describing when the original recording of the audio was released. </summary>
        public string OriginalReleasedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalReleasedDate))
                    this._OriginalReleasedDate = "";
                return _OriginalReleasedDate;
            }
            set
            {
                this._OriginalReleasedDate = value;
            }
        }

        ///<summary> Contains a timestamp describing when the original recording of the audio was recorded. </summary>
        public string OriginalRecordedDate
        {
            get
            {
                if (String.IsNullOrEmpty(this._OriginalRecordedDate))
                    this._OriginalRecordedDate = "";
                return _OriginalRecordedDate;
            }
            set
            {
                this._OriginalRecordedDate = value;
            }
        }

        ///<summary> Location that the item was originaly designed_written. Information should be stored in the following format: country code, state_province, city where the coutry code is the same 2 octets as in Internet domains, or possibly ISO-3166. e.g. US, Texas, Austin or US, , Austin. </summary>
        public string WrittenLocation
        {
            get
            {
                if (String.IsNullOrEmpty(this._WrittenLocation))
                    this._WrittenLocation = "";
                return _WrittenLocation;
            }
            set
            {
                this._WrittenLocation = value;
            }
        }

        ///<summary> Location where track was recorded. (See COMPOSITION_LOCATION for format) </summary>
        public string RecordedLocation
        {
            get
            {
                if (String.IsNullOrEmpty(this._RecordedLocation))
                    this._RecordedLocation = "";
                return _RecordedLocation;
            }
            set
            {
                this._RecordedLocation = value;
            }
        }

        ///<summary> Archival Location. Indicates where the subject of the file is archived. </summary>
        public string ArchivalLocation
        {
            get
            {
                if (String.IsNullOrEmpty(this._ArchivalLocation))
                    this._ArchivalLocation = "";
                return _ArchivalLocation;
            }
            set
            {
                this._ArchivalLocation = value;
            }
        }

        ///<summary> The main genre of the audio or video. e.g. classical, ambient-house, synthpop, sci-fi, drama, etc. </summary>
        public string Genre
        {
            get
            {
                if (String.IsNullOrEmpty(this._Genre))
                    this._Genre = "";
                return _Genre;
            }
            set
            {
                this._Genre = value;
            }
        }

        ///<summary> Intended to reflect the mood of the item with a few keywords, e.g. Romantic, Sad, Uplifting, etc. </summary>
        public string Mood
        {
            get
            {
                if (String.IsNullOrEmpty(this._Mood))
                    this._Mood = "";
                return _Mood;
            }
            set
            {
                this._Mood = value;
            }
        }

        ///<summary> Any comment related to the content. </summary>
        public string Comment
        {
            get
            {
                if (String.IsNullOrEmpty(this._Comment))
                    this._Comment = "";
                return _Comment;
            }
            set
            {
                this._Comment = value;
            }
        }

        ///<summary> A numeric value defining how much a person likes the song_movie. The number is between 0 and 5 with decimal values possible (e.g. 2.7), 5(.0) being the highest possible rating. </summary>
        public string Rating
        {
            get
            {
                if (String.IsNullOrEmpty(this._Rating))
                    this._Rating = "";
                return _Rating;
            }
            set
            {
                this._Rating = value;
            }
        }

        ///<summary> Software. Identifies the name of the software package used to create the file, such as Microsoft WaveEdit. </summary>
        public string EncodedApplication
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedApplication))
                    this._EncodedApplication = "";
                return _EncodedApplication;
            }
            set
            {
                this._EncodedApplication = value;
            }
        }

        ///<summary> The software or hardware used to encode this item. e.g. LAME or XviD </summary>
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

        ///<summary> A list of the settings used for encoding this item. No specific format. </summary>
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

        ///<summary> Identifies the original recording media form from which the material originated, such as CD, cassette, LP, radio broadcast, slide, paper, etc. </summary>
        public string EncodedOriginal
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedOriginal))
                    this._EncodedOriginal = "";
                return _EncodedOriginal;
            }
            set
            {
                this._EncodedOriginal = value;
            }
        }

        ///<summary> Official audio source webpage. e.g. a movie. </summary>
        public string EncodedOriginalUrl
        {
            get
            {
                if (String.IsNullOrEmpty(this._EncodedOriginalUrl))
                    this._EncodedOriginalUrl = "";
                return _EncodedOriginalUrl;
            }
            set
            {
                this._EncodedOriginalUrl = value;
            }
        }

        ///<summary> Copyright attribution. </summary>
        public string Copyright
        {
            get
            {
                if (String.IsNullOrEmpty(this._Copyright))
                    this._Copyright = "";
                return _Copyright;
            }
            set
            {
                this._Copyright = value;
            }
        }

        ///<summary> The copyright information as per the productioncopyright holder. </summary>
        public string ProducerCopyright
        {
            get
            {
                if (String.IsNullOrEmpty(this._ProducerCopyright))
                    this._ProducerCopyright = "";
                return _ProducerCopyright;
            }
            set
            {
                this._ProducerCopyright = value;
            }
        }

        ///<summary> License information, e.g., All Rights Reserved,Any Use Permitted. </summary>
        public string TermsOfUse
        {
            get
            {
                if (String.IsNullOrEmpty(this._TermsOfUse))
                    this._TermsOfUse = "";
                return _TermsOfUse;
            }
            set
            {
                this._TermsOfUse = value;
            }
        }

        ///<summary> Copyright_legal information. </summary>
        public string CopyrightUrl
        {
            get
            {
                if (String.IsNullOrEmpty(this._CopyrightUrl))
                    this._CopyrightUrl = "";
                return _CopyrightUrl;
            }
            set
            {
                this._CopyrightUrl = value;
            }
        }

        ///<summary> International Standard Recording Code, excluding the ISRC prefix and including hyphens. </summary>
        public string ISRC
        {
            get
            {
                if (String.IsNullOrEmpty(this._ISRC))
                    this._ISRC = "";
                return _ISRC;
            }
            set
            {
                this._ISRC = value;
            }
        }

        ///<summary> This is a binary dump of the TOC of the CDROM that this item was taken from. </summary>
        public string MSDI
        {
            get
            {
                if (String.IsNullOrEmpty(this._MSDI))
                    this._MSDI = "";
                return _MSDI;
            }
            set
            {
                this._MSDI = value;
            }
        }

        ///<summary> International Standard Book Number. </summary>
        public string ISBN
        {
            get
            {
                if (String.IsNullOrEmpty(this._ISBN))
                    this._ISBN = "";
                return _ISBN;
            }
            set
            {
                this._ISBN = value;
            }
        }

        ///<summary> EAN-13 (13-digit European Article Numbering) or UPC-A (12-digit Universal Product Code) bar code identifier. </summary>
        public string BarCode
        {
            get
            {
                if (String.IsNullOrEmpty(this._BarCode))
                    this._BarCode = "";
                return _BarCode;
            }
            set
            {
                this._BarCode = value;
            }
        }

        ///<summary> Library of Congress Control Number. </summary>
        public string LCCN
        {
            get
            {
                if (String.IsNullOrEmpty(this._LCCN))
                    this._LCCN = "";
                return _LCCN;
            }
            set
            {
                this._LCCN = value;
            }
        }

        ///<summary> A label-specific catalogue number used to identify the release. e.g. TIC 01. </summary>
        public string CatalogNumber
        {
            get
            {
                if (String.IsNullOrEmpty(this._CatalogNumber))
                    this._CatalogNumber = "";
                return _CatalogNumber;
            }
            set
            {
                this._CatalogNumber = value;
            }
        }

        ///<summary> A 4-digit or 5-digit number to identify the record label, typically printed as (LC) xxxx or (LC) 0xxxx on CDs medias or covers, with only the number being stored. </summary>
        public string LabelCode
        {
            get
            {
                if (String.IsNullOrEmpty(this._LabelCode))
                    this._LabelCode = "";
                return _LabelCode;
            }
            set
            {
                this._LabelCode = value;
            }
        }

        ///<summary> Is there a cover </summary>
        public string Cover
        {
            get
            {
                if (String.IsNullOrEmpty(this._Cover))
                    this._Cover = "";
                return _Cover;
            }
            set
            {
                this._Cover = value;
            }
        }

        ///<summary> Cover, in binary format encoded BASE64 </summary>
        public string CoverDatas
        {
            get
            {
                if (String.IsNullOrEmpty(this._CoverDatas))
                    this._CoverDatas = "";
                return _CoverDatas;
            }
            set
            {
                this._CoverDatas = value;
            }
        }

        ///<summary> Average number of beats per minute </summary>
        public string BPM
        {
            get
            {
                if (String.IsNullOrEmpty(this._BPM))
                    this._BPM = "";
                return _BPM;
            }
            set
            {
                this._BPM = value;
            }
        }

        ///<summary> Video Codecs separated by _ </summary>
        public string VideoCodecList
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoCodecList))
                    this._VideoCodecList = "";
                return _VideoCodecList;
            }
            set
            {
                this._VideoCodecList = value;
            }
        }

        ///<summary> Video languages separated by _ </summary>
        public string VideoLanguageList
        {
            get
            {
                if (String.IsNullOrEmpty(this._VideoLanguageList))
                    this._VideoLanguageList = "";
                return _VideoLanguageList;
            }
            set
            {
                this._VideoLanguageList = value;
            }
        }

        ///<summary> Audio Codecs separated by _ </summary>
        public string AudioCodecList
        {
            get
            {
                if (String.IsNullOrEmpty(this._AudioCodecList))
                    this._AudioCodecList = "";
                return _AudioCodecList;
            }
            set
            {
                this._AudioCodecList = value;
            }
        }

        ///<summary> Audio languages separated by _ </summary>
        public string AudioLanguageList
        {
            get
            {
                if (String.IsNullOrEmpty(this._AudioLanguageList))
                    this._AudioLanguageList = "";
                return _AudioLanguageList;
            }
            set
            {
                this._AudioLanguageList = value;
            }
        }

        ///<summary> Text Codecs separated by _ </summary>
        public string TextCodecList
        {
            get
            {
                if (String.IsNullOrEmpty(this._TextCodecList))
                    this._TextCodecList = "";
                return _TextCodecList;
            }
            set
            {
                this._TextCodecList = value;
            }
        }

        ///<summary> Text languages separated by _ </summary>
        public string TextLanguageList
        {
            get
            {
                if (String.IsNullOrEmpty(this._TextLanguageList))
                    this._TextLanguageList = "";
                return _TextLanguageList;
            }
            set
            {
                this._TextLanguageList = value;
            }
        }

        ///<summary> Chapters Codecs separated by _ </summary>
        public string ChaptersCodecList
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChaptersCodecList))
                    this._ChaptersCodecList = "";
                return _ChaptersCodecList;
            }
            set
            {
                this._ChaptersCodecList = value;
            }
        }

        ///<summary> Chapters languages separated by _ </summary>
        public string ChaptersLanguageList
        {
            get
            {
                if (String.IsNullOrEmpty(this._ChaptersLanguageList))
                    this._ChaptersLanguageList = "";
                return _ChaptersLanguageList;
            }
            set
            {
                this._ChaptersLanguageList = value;
            }
        }

        ///<summary> Image Codecs separated by _ </summary>
        public string ImageCodecList
        {
            get
            {
                if (String.IsNullOrEmpty(this._ImageCodecList))
                    this._ImageCodecList = "";
                return _ImageCodecList;
            }
            set
            {
                this._ImageCodecList = value;
            }
        }

        ///<summary> Image languages separated by _ </summary>
        public string ImageLanguageList
        {
            get
            {
                if (String.IsNullOrEmpty(this._ImageLanguageList))
                    this._ImageLanguageList = "";
                return _ImageLanguageList;
            }
            set
            {
                this._ImageLanguageList = value;
            }
        }

        ///<summary> Other tags not known... </summary>
        public string Other
        {
            get
            {
                if (String.IsNullOrEmpty(this._Other))
                    this._Other = "";
                return _Other;
            }
            set
            {
                this._Other = value;
            }
        }
    }
}
