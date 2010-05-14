
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace MediaInfoWrapper
{
//disables the xml comment warning on compilation
#pragma warning disable 1591
    public enum MediaInfoStreamKind
    {
        General,
        Video,
        Audio,
        Text,
        Chapters,
        Image
    }

    public enum MediaInfoInfoKind
    {
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }

    public enum InfoOptions
    {
        ShowInInform,
        Support,
        ShowInSupported,
        TypeOfValue
    }
    #pragma warning restore 1591
     
        /// <summary>
        /// When called with a proper file target, returns a MediaInfo object filled with list of media tracks containing
        /// every information MediaInfo.dll can collect.
        /// Tracks are accessibles as properties.
        /// </summary>
    public class MediaInfo : IDisposable
    {

        [DllImport("MediaInfo.dll")]
        internal static extern int MediaInfo_Close(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        internal static extern int MediaInfo_Count_Get(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, int StreamNumber);
        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfo_Delete(IntPtr Handle);
        [DllImport("MediaInfo.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr MediaInfo_Get(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, uint StreamNumber, string Parameter, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfInfo, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfSearch);
        [DllImport("MediaInfo.dll", CharSet = CharSet.Unicode)]
        internal static extern string MediaInfo_GetI(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, uint StreamNumber, uint Parameter, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfInfo);
        [DllImport("MediaInfo.dll", CharSet = CharSet.Unicode)]
        internal static extern string MediaInfo_Inform(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] uint Reserved);
        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_New();
        [DllImport("MediaInfo.dll", CharSet = CharSet.Unicode)]
        internal static extern int MediaInfo_Open(IntPtr Handle, string FileName);
        [DllImport("MediaInfo.dll", CharSet = CharSet.Unicode)]
        internal static extern string MediaInfo_Option(IntPtr Handle, string OptionString, string Value);
        [DllImport("MediaInfo.dll")]
        internal static extern int MediaInfo_State_Get(IntPtr Handle);


        private List<VideoTrack> _Video;
        private List<GeneralTrack> _General;
        private List<AudioTrack> _Audio;
        private List<TextTrack> _Text;
        private List<ChaptersTrack> _Chapters;
        private Int32 _VideoCount;
        private Int32 _GeneralCount;
        private Int32 _AudioCount;
        private Int32 _TextCount;
        private Int32 _ChaptersCount;
        private string _InfoComplete;
        private string _InfoStandard;
        private string _InfoCustom;
        private string _FileName;

        private IntPtr Handle;
       //public static const string MediaInfoPath="MediaInfo.dll";
       
        /// <summary>
        /// Constructor :
        /// When called with a proper file target, returns a MediaInfo object filled with list of media tracks containing
        /// every information MediaInfo.dll can collect.
        /// Tracks are accessibles as properties.
        /// </summary>
        /// <param name="path"></param>
        public MediaInfo(string path)
        {
            //if (!CheckFileExistence("MediaInfo.dll")) return;
            if (!CheckFileExistence(path)) return;
            _FileName = path;
            
            this.Handle = MediaInfo.MediaInfo_New();
            MediaInfo.MediaInfo_Open(this.Handle, path);
            try
            {
                getStreamCount();
                getAllInfos();
            }
            finally //ensure MediaInfo_Close is called even if something goes wrong 
            {
                MediaInfo.MediaInfo_Close(this.Handle);
            }

        }

        #region Disposable Pattern
        private bool disposed;

        ~MediaInfo()
        {
            Dispose(false);
        }

        protected bool IsDisposed
        {
            get { return disposed; }
        }

        protected void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }

        /// <summary>Call this one to kill the wrapper, and close his handle to the MediaInfo.dll, you should never need it anyway </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }
                DisposeUnmanagedResources();
            }
            this.disposed = true;
        }

        protected virtual void DisposeManagedResources()
        {
        }
        protected virtual void DisposeUnmanagedResources()
        {
            MediaInfo.MediaInfo_Close(this.Handle);
            MediaInfo.MediaInfo_Delete(this.Handle);
        }
        #endregion

        /// <summary>
        /// Simply checks file presence else throws a FileNotFoundException
        /// </summary>
        protected bool CheckFileExistence(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("File not found :" + filepath, filepath);
                
            }
            else return true;
        }
        private unsafe string GetSpecificMediaInfo(MediaInfoStreamKind KindOfStream, int trackindex, string NameOfParameter)
        {
            IntPtr p = MediaInfo.MediaInfo_Get(this.Handle, KindOfStream, Convert.ToUInt32(trackindex), NameOfParameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name);
            char* p2 = (char*)p.ToPointer();
            string s = "";
            while (*p2 != '\0')
            {
                s += *p2;
                ++p2;
            }
            return s;
        }

        private void getStreamCount()
        {
            _AudioCount = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Audio, -1);
            _VideoCount = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Video, -1);
            _GeneralCount = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.General, -1);
            _TextCount = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Text, -1);
            _ChaptersCount = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Chapters, -1);
        }
        private void getAllInfos()
        {
            getVideoInfo();
            getAudioInfo();
            getChaptersInfo();
            getTextInfo();
            getGeneralInfo();
        }
        /// <summary>
        /// More detailled standard information.
        /// </summary>
        public String InfoComplete
        {
            get
            {
                if (String.IsNullOrEmpty(this._InfoComplete))
                {   MediaInfo.MediaInfo_Open(this.Handle,_FileName);
                    MediaInfo.MediaInfo_Option(this.Handle, "Complete", "1");
                    _InfoComplete = MediaInfo.MediaInfo_Inform(this.Handle, 0);
                    MediaInfo.MediaInfo_Close(this.Handle);
        
                }
                return _InfoComplete;
            }

        }
                /// <summary>
        /// Detailled standard information.
        /// </summary>
        public String InfoStandard
        {
            get
            {
                if (String.IsNullOrEmpty(this._InfoStandard))
                {
                    MediaInfo.MediaInfo_Open(this.Handle, _FileName);
                    MediaInfo.MediaInfo_Option(this.Handle, "Complete", "");
                    _InfoStandard = MediaInfo.MediaInfo_Inform(this.Handle, 0);
                    MediaInfo.MediaInfo_Close(this.Handle);
        
                }
                return _InfoStandard;
            }
        }
        /// <summary>
        /// Lists every available property in every track
        /// </summary>
        /// <returns></returns>
        public string InfoCustom
        {
          get{
            if (String.IsNullOrEmpty(_InfoCustom))
            {
            string s = "";
            
            s += "General" + Environment.NewLine;
            s += ListEveryAvailablePropery<GeneralTrack>(General);
            s += Environment.NewLine;
            s += "Video" + Environment.NewLine;
            s += ListEveryAvailablePropery<VideoTrack>(Video);
            s += Environment.NewLine;            
            s += "Audio" + Environment.NewLine;
            s += ListEveryAvailablePropery<AudioTrack>(Audio);
            s += Environment.NewLine;
            s += "Text" + Environment.NewLine;
            s += ListEveryAvailablePropery<TextTrack>(Text);
            s += Environment.NewLine;
            s += "Chapters" + Environment.NewLine;
            s += ListEveryAvailablePropery<ChaptersTrack>(Chapters);

            _InfoCustom=s;
            
            }
              return _InfoCustom;
          }
        }

        /// <summary>
        /// Uses reflection to get every property for every track in a tracklist and get its value.
        /// </summary>
        /// <typeparam name="T1">Type of tracklist, for instance List'VideoTrack'</typeparam>
        /// <param name="L">tracklist, for instance Video</param>
        /// <returns>A formatted string listing every property for every track</returns>
        private string ListEveryAvailablePropery<T1>(List<T1> L)
        {   
            string s = "";
            foreach (T1 track in L)
            {
                foreach (PropertyInfo p in track.GetType().GetProperties() )
                {
                    s += (p.GetValue(track, null).ToString() == "") ? p.Name + " : Not available"+Environment.NewLine  : p.Name + " : " + p.GetValue(track, null) + Environment.NewLine;
                }
            }
            return s;
        }

            /// <summary>
            /// Lists media info dll capacities
            /// </summary>
            /// <returns></returns>
            public static string Capacities()
            {
                return MediaInfo_Option((IntPtr)0, "Info_Capacities", "");
            }
            /// <summary>
            /// Lists media info parameter list for MediaInfo_Get
            /// </summary>
            /// <returns></returns>
            private static string ParameterList()
            {
                return MediaInfo_Option((IntPtr)0, "Info_Parameters", "");
            }
            /// <summary>
            /// Lists all supported codecs
            /// </summary>
            /// <returns></returns>
            public static string KnownCodecs()
            {
                return MediaInfo_Option((IntPtr)0, "Info_Codecs", "");
            }



    
    
///<summary> List of all the General streams available in the file, type GeneralTrack[trackindex] to access a specific track</summary>
public List<GeneralTrack> General
        {
            get
            {
                if (this._General == null)
                {
                   getGeneralInfo();
                }
                
                return this._General;
            }
        }
private void getGeneralInfo()
        {
            
                if (this._General == null)
                {
                    this._General = new List<GeneralTrack>();
                    int num1 = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.General, -1);
                    if (num1 > 0)
                    {
                        int num3 = num1 - 1;
                        for (int num2 = 0; num2 <= num3; num2++)
                        {GeneralTrack _tracktemp_ = new GeneralTrack();
                            
                                                    
_tracktemp_.Count= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Count");
_tracktemp_.StreamCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"StreamCount");
_tracktemp_.StreamKind= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"StreamKind");
_tracktemp_.StreamKindID= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"StreamKindID");
_tracktemp_.Inform= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Inform");
_tracktemp_.ID= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ID");
_tracktemp_.UniqueID= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"UniqueID");
_tracktemp_.GeneralCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"GeneralCount");
_tracktemp_.VideoCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"VideoCount");
_tracktemp_.AudioCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"AudioCount");
_tracktemp_.TextCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"TextCount");
_tracktemp_.ChaptersCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ChaptersCount");
_tracktemp_.ImageCount= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ImageCount");
_tracktemp_.CompleteName= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"CompleteName");
_tracktemp_.FolderName= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FolderName");
_tracktemp_.FileName= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileName");
_tracktemp_.FileExtension= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileExtension");
_tracktemp_.FileSize= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize");
_tracktemp_.FileSizeString= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize/String");
_tracktemp_.FileSizeString1= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize/String1");
_tracktemp_.FileSizeString2= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize/String2");
_tracktemp_.FileSizeString3= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize/String3");
_tracktemp_.FileSizeString4= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"FileSize/String4");
_tracktemp_.Format= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Format");
_tracktemp_.FormatString= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Format/String");
_tracktemp_.FormatInfo= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Format/Info");
_tracktemp_.FormatUrl= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Format/Url");
_tracktemp_.FormatExtensions= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Format/Extensions");
_tracktemp_.OveralBitRate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"OveralBitRate");
_tracktemp_.OveralBitRateString= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"OveralBitRate/String");
_tracktemp_.PlayTime= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"PlayTime");
_tracktemp_.PlayTimeString= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"PlayTime/String");
_tracktemp_.PlayTimeString1= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"PlayTime/String1");
_tracktemp_.PlayTimeString2= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"PlayTime/String2");
_tracktemp_.PlayTimeString3= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"PlayTime/String3");
_tracktemp_.Title= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Title");
_tracktemp_.TitleMore= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Title/More");
_tracktemp_.Domain= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Domain");
_tracktemp_.Collection= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Collection");
_tracktemp_.CollectionTotalParts= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Collection/Total_Parts");
_tracktemp_.Season= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Season");
_tracktemp_.Movie= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Movie");
_tracktemp_.MovieMore= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Movie/More");
_tracktemp_.Album= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Album");
_tracktemp_.AlbumTotalParts= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Album/Total_Parts");
_tracktemp_.AlbumSort= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Album/Sort");
_tracktemp_.Comic= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Comic");
_tracktemp_.ComicTotalParts= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Comic/Total_Parts");
_tracktemp_.Part= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Part");
_tracktemp_.PartTotalParts= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Part/Total_Parts");
_tracktemp_.PartPosition= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Part/Position");
_tracktemp_.Track= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Track");
_tracktemp_.TrackPosition= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Track/Position");
_tracktemp_.TrackMore= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Track/More");
_tracktemp_.TrackSort= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Track/Sort");
_tracktemp_.Chapter= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Chapter");
_tracktemp_.SubTrack= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"SubTrack");
_tracktemp_.OriginalAlbum= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Album");
_tracktemp_.OriginalMovie= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Movie");
_tracktemp_.OriginalPart= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Part");
_tracktemp_.OriginalTrack= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Track");
_tracktemp_.Author= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Author");
_tracktemp_.Artist= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Artist");
_tracktemp_.PerformerSort= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Performer/Sort");
_tracktemp_.OriginalPerformer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Performer");
_tracktemp_.Accompaniment= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Accompaniment");
_tracktemp_.MusicianInstrument= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Musician_Instrument");
_tracktemp_.Composer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Composer");
_tracktemp_.ComposerNationality= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Composer/Nationality");
_tracktemp_.Arranger= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Arranger");
_tracktemp_.Lyricist= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Lyricist");
_tracktemp_.OriginalLyricist= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Lyricist");
_tracktemp_.Conductor= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Conductor");
_tracktemp_.Actor= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Actor");
_tracktemp_.ActorCharacter= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Actor_Character");
_tracktemp_.WrittenBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"WrittenBy");
_tracktemp_.ScreenplayBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ScreenplayBy");
_tracktemp_.Director= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Director");
_tracktemp_.AssistantDirector= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"AssistantDirector");
_tracktemp_.DirectorOfPhotography= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"DirectorOfPhotography");
_tracktemp_.ArtDirector= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ArtDirector");
_tracktemp_.EditedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"EditedBy");
_tracktemp_.Producer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Producer");
_tracktemp_.CoProducer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"CoProducer");
_tracktemp_.ExecutiveProducer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ExecutiveProducer");
_tracktemp_.ProductionDesigner= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ProductionDesigner");
_tracktemp_.CostumeDesigner= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"CostumeDesigner");
_tracktemp_.Choregrapher= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Choregrapher");
_tracktemp_.SoundEngineer= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"SoundEngineer");
_tracktemp_.MasteredBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"MasteredBy");
_tracktemp_.RemixedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"RemixedBy");
_tracktemp_.ProductionStudio= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ProductionStudio");
_tracktemp_.Publisher= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Publisher");
_tracktemp_.PublisherURL= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Publisher/URL");
_tracktemp_.DistributedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"DistributedBy");
_tracktemp_.EncodedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"EncodedBy");
_tracktemp_.ThanksTo= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ThanksTo");
_tracktemp_.Technician= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Technician");
_tracktemp_.CommissionedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"CommissionedBy");
_tracktemp_.EncodedOriginalDistributedBy= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Original/DistributedBy");
_tracktemp_.RadioStation= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"RadioStation");
_tracktemp_.RadioStationOwner= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"RadioStation/Owner");
_tracktemp_.RadioStationURL= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"RadioStation/URL");
_tracktemp_.ContentType= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ContentType");
_tracktemp_.Subject= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Subject");
_tracktemp_.Synopsys= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Synopsys");
_tracktemp_.Summary= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Summary");
_tracktemp_.Description= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Description");
_tracktemp_.Keywords= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Keywords");
_tracktemp_.Period= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Period");
_tracktemp_.LawRating= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"LawRating");
_tracktemp_.IRCA= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"IRCA");
_tracktemp_.Language= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Language");
_tracktemp_.Medium= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Medium");
_tracktemp_.Product= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Product");
_tracktemp_.Country= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Country");
_tracktemp_.WrittenDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Written_Date");
_tracktemp_.RecordedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Recorded_Date");
_tracktemp_.ReleasedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Released_Date");
_tracktemp_.MasteredDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Mastered_Date");
_tracktemp_.EncodedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Date");
_tracktemp_.TaggedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Tagged_Date");
_tracktemp_.OriginalReleasedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Released_Date");
_tracktemp_.OriginalRecordedDate= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Original/Recorded_Date");
_tracktemp_.WrittenLocation= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Written_Location");
_tracktemp_.RecordedLocation= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Recorded_Location");
_tracktemp_.ArchivalLocation= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Archival_Location");
_tracktemp_.Genre= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Genre");
_tracktemp_.Mood= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Mood");
_tracktemp_.Comment= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Comment");
_tracktemp_.Rating= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Rating ");
_tracktemp_.EncodedApplication= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Application");
_tracktemp_.EncodedLibrary= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Library");
_tracktemp_.EncodedLibrarySettings= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Library_Settings");
_tracktemp_.EncodedOriginal= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Original");
_tracktemp_.EncodedOriginalUrl= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Encoded_Original/Url");
_tracktemp_.Copyright= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Copyright");
_tracktemp_.ProducerCopyright= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Producer_Copyright");
_tracktemp_.TermsOfUse= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"TermsOfUse");
_tracktemp_.CopyrightUrl= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Copyright/Url");
_tracktemp_.ISRC= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ISRC");
_tracktemp_.MSDI= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"MSDI");
_tracktemp_.ISBN= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"ISBN");
_tracktemp_.BarCode= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"BarCode");
_tracktemp_.LCCN= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"LCCN");
_tracktemp_.CatalogNumber= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"CatalogNumber");
_tracktemp_.LabelCode= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"LabelCode");
_tracktemp_.Cover= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Cover");
_tracktemp_.CoverDatas= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Cover_Datas");
_tracktemp_.BPM= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"BPM");
_tracktemp_.VideoCodecList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Video_Codec_List");
_tracktemp_.VideoLanguageList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Video_Language_List");
_tracktemp_.AudioCodecList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Audio_Codec_List");
_tracktemp_.AudioLanguageList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Audio_Language_List");
_tracktemp_.TextCodecList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Text_Codec_List");
_tracktemp_.TextLanguageList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Text_Language_List");
_tracktemp_.ChaptersCodecList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Chapters_Codec_List");
_tracktemp_.ChaptersLanguageList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Chapters_Language_List");
_tracktemp_.ImageCodecList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Image_Codec_List");
_tracktemp_.ImageLanguageList= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Image_Language_List");
_tracktemp_.Other= GetSpecificMediaInfo(MediaInfoStreamKind.General,num2,"Other");

                        
                         this._General.Add(_tracktemp_);

                    }
                    }
            
            
            }
        }
///<summary> Number of General streams in the file</summary>
public Int32 GeneralCount
        {
            get
            {
              return this._GeneralCount;
            }
        }
///<summary> List of all the Video streams available in the file, type VideoTrack[trackindex] to access a specific track</summary>
public List<VideoTrack> Video
        {
            get
            {
                if (this._Video == null)
                {
                   getVideoInfo();
                }
                
                return this._Video;
            }
        }
private void getVideoInfo()
        {
            
                if (this._Video == null)
                {
                    this._Video = new List<VideoTrack>();
                    int num1 = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Video, -1);
                    if (num1 > 0)
                    {
                        int num3 = num1 - 1;
                        for (int num2 = 0; num2 <= num3; num2++)
                        {VideoTrack _tracktemp_ = new VideoTrack();
                            
                                                    
_tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Count");
_tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"StreamCount");
_tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"StreamKind");
_tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"StreamKindID");
_tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Inform");
_tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"ID");
_tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"UniqueID");
_tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Title");
_tracktemp_.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Codec");
_tracktemp_.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Codec/String");
_tracktemp_.CodecInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Codec/Info");
_tracktemp_.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Codec/Url");
_tracktemp_.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "CodecID");
_tracktemp_.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "CodecID/Info");
_tracktemp_.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"BitRate");
_tracktemp_.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"BitRate/String");
_tracktemp_.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"BitRate_Mode");
_tracktemp_.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Encoded_Library");
_tracktemp_.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Encoded_Library_Settings");
_tracktemp_.Width = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Width");
_tracktemp_.Height = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Height");
_tracktemp_.AspectRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"AspectRatio");
_tracktemp_.AspectRatioString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"AspectRatio/String");
_tracktemp_.FrameRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"FrameRate");
_tracktemp_.FrameRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"FrameRate/String");
_tracktemp_.FrameCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"FrameCount");
_tracktemp_.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"BitDepth");
_tracktemp_.BitsPixelFrame = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Bits/(Pixel*Frame)");
_tracktemp_.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Delay");
_tracktemp_.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Duration");
_tracktemp_.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Duration/String");
_tracktemp_.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Duration/String1");
_tracktemp_.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Duration/String2");
_tracktemp_.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Duration/String3");
_tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Language");
_tracktemp_.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Language/String");
_tracktemp_.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Video,num2,"Language_More");
_tracktemp_.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format");
_tracktemp_.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format/Info");
_tracktemp_.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Profile");
_tracktemp_.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings");
_tracktemp_.FormatSettingsBVOP = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_BVOP");
_tracktemp_.FormatSettingsBVOPString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_BVOP/String");
_tracktemp_.FormatSettingsCABAC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_CABAC");
_tracktemp_.FormatSettingsCABACString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_CABAC/String");
_tracktemp_.FormatSettingsGMC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_GMC");
_tracktemp_.FormatSettingsGMCString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_GMAC/String");
_tracktemp_.FormatSettingsMatrix = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_Matrix");
_tracktemp_.FormatSettingsMatrixData = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_Matrix_Data");
_tracktemp_.FormatSettingsMatrixString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_Matrix/String");
_tracktemp_.FormatSettingsPulldown = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_Pulldown");
_tracktemp_.FormatSettingsQPel = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_QPel");
_tracktemp_.FormatSettingsQPelString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_QPel/String");
_tracktemp_.FormatSettingsRefFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_RefFrames");
_tracktemp_.FormatSettingsRefFramesString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Settings_RefFrames/String");
_tracktemp_.ScanType = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "ScanType");
_tracktemp_.ScanTypeString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "ScanType/String");
_tracktemp_.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format/Url");
_tracktemp_.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Video, num2, "Format_Version");                   
                         this._Video.Add(_tracktemp_);
                    }
                    }
            
            
            }
        }
///<summary> Number of Video streams in the file</summary>
public Int32 VideoCount
        {
            get
            {
              return this._VideoCount;
            }
        }
///<summary> List of all the Audio streams available in the file, type AudioTrack[trackindex] to access a specific track</summary>
public List<AudioTrack> Audio
        {
            get
            {
                if (this._Audio == null)
                {
                   getAudioInfo();
                }
                
                return this._Audio;
            }
        }
private void getAudioInfo()
        {
            
                if (this._Audio == null)
                {
                    this._Audio = new List<AudioTrack>();
                    int num1 = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Audio, -1);
                    if (num1 > 0)
                    {
                        int num3 = num1 - 1;
                        for (int num2 = 0; num2 <= num3; num2++)
                        {AudioTrack _tracktemp_ = new AudioTrack();
                            
                                                    
_tracktemp_.Count= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Count");
_tracktemp_.StreamCount= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"StreamCount");
_tracktemp_.StreamKind= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"StreamKind");
_tracktemp_.StreamKindString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamKind/String");
_tracktemp_.StreamKindID= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"StreamKindID");
_tracktemp_.StreamKindPos = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamKindPos");
_tracktemp_.Inform= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Inform");
_tracktemp_.ID= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"ID");
_tracktemp_.IDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ID/String");
_tracktemp_.UniqueID= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"UniqueID");
_tracktemp_.MenuID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "MenuID");
_tracktemp_.MenuIDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "MenuID/String");
_tracktemp_.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format");
_tracktemp_.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format/Info");
_tracktemp_.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format/Url");
_tracktemp_.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Version");
_tracktemp_.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Profile");
_tracktemp_.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings");
_tracktemp_.FormatSettingsSBR = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_SBR");
_tracktemp_.FormatSettingsSBRString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_SBR/String");
_tracktemp_.FormatSettingsPS = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_PS");
_tracktemp_.FormatSettingsPSString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_PS/String");
_tracktemp_.FormatSettingsFloor = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_Floor");
_tracktemp_.FormatSettingsFirm = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_Firm");
_tracktemp_.FormatSettingsEndianness = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_Endianness");
_tracktemp_.FormatSettingsSign = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_Sign");
_tracktemp_.FormatSettingsLaw = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_Law");
_tracktemp_.FormatSettingsITU = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Format_Settings_ITU");
_tracktemp_.MuxingMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "MuxingMode");
_tracktemp_.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CodecID");
_tracktemp_.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CodecID/Info");
_tracktemp_.CodecIDUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CodecID/Url");
_tracktemp_.CodecIDHint = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CodecID/Hint");
_tracktemp_.CodecIDDescription = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CodecID_Description");
_tracktemp_.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Duration");
_tracktemp_.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Duration/String");
_tracktemp_.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Duration/String1");
_tracktemp_.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Duration/String2");
_tracktemp_.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Duration/String3");
_tracktemp_.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Mode");
_tracktemp_.BitRateModeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Mode/String");
_tracktemp_.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate");
_tracktemp_.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate/String");
_tracktemp_.BitRateMinimum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Minimum");
_tracktemp_.BitRateMinimumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Minimum/String");
_tracktemp_.BitRateNominal = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Nominal");
_tracktemp_.BitRateNominalString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Nominal/String");
_tracktemp_.BitRateMaximum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Maximum");
_tracktemp_.BitRateMaximumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitRate_Maximum/String");
_tracktemp_.Channels = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Channel(s)");
_tracktemp_.ChannelsString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Channel(s)/String");
_tracktemp_.ChannelMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ChannelMode");
_tracktemp_.ChannelPositions = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ChannelPositions");
_tracktemp_.ChannelPositionsString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ChannelPositions/String2");
_tracktemp_.SamplingRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "SamplingRate");
_tracktemp_.SamplingRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "SamplingRate/String");
_tracktemp_.SamplingCount = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "SamplingCount");
_tracktemp_.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitDepth");
_tracktemp_.BitDepthString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "BitDepth/String");
_tracktemp_.CompressionRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "CompressionRatio");
_tracktemp_.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Delay");
_tracktemp_.DelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Delay/String");
_tracktemp_.DelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Delay/String1");
_tracktemp_.DelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Delay/String2");
_tracktemp_.DelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Delay/String3");
_tracktemp_.VideoDelay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Video_Delay");
_tracktemp_.VideoDelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Video_Delay/String");
_tracktemp_.VideoDelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Video_Delay/String1");
_tracktemp_.VideoDelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Video_Delay/String2");
_tracktemp_.VideoDelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Video_Delay/String3");
_tracktemp_.ReplayGainGain = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ReplayGain_Gain");
_tracktemp_.ReplayGainGainString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ReplayGain_Gain/String");
_tracktemp_.ReplayGainPeak = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "ReplayGain_Peak");
_tracktemp_.StreamSize = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize");
_tracktemp_.StreamSizeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String");
_tracktemp_.StreamSizeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String1");
_tracktemp_.StreamSizeString2  = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String2");
_tracktemp_.StreamSizeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String3");
_tracktemp_.StreamSizeString4 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String4");
_tracktemp_.StreamSizeString5 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize/String5");
_tracktemp_.StreamSizeProportion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "StreamSize_Proportion");
_tracktemp_.Alignment = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Alignment");
_tracktemp_.AlignmentString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Alignment/String");
_tracktemp_.InterleaveVideoFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Interleave_VideoFrames");
_tracktemp_.InterleaveDuration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Interleave_Duration");
_tracktemp_.InterleaveDurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Interleave_Duration/String");
_tracktemp_.InterleavePreload = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Interleave_Preload");
_tracktemp_.InterleavePreloadString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Interleave_Preload/String");
_tracktemp_.Title= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Title");
_tracktemp_.EncodedLibrary= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Encoded_Library");
_tracktemp_.EncodedLibraryString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encoded_Library/String");
_tracktemp_.EncodedLibraryName = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encoded_Library/Name");
_tracktemp_.EncodedLibraryVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encoded_Library/Version");
_tracktemp_.EncodedLibraryDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encoded_Library/Date");
_tracktemp_.EncodedLibrarySettings= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Encoded_Library_Settings");
_tracktemp_.Language= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Language");
_tracktemp_.LanguageString= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Language/String");
_tracktemp_.LanguageMore= GetSpecificMediaInfo(MediaInfoStreamKind.Audio,num2,"Language_More");
_tracktemp_.EncodedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encoded_Date");
_tracktemp_.TaggedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Tagged_Date");
_tracktemp_.Encryption = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, num2, "Encryption");
                        
                         this._Audio.Add(_tracktemp_);

                    }
                    }
            
            
            }
        }
///<summary> Number of Audio streams in the file</summary>
public Int32 AudioCount
        {
            get
            {
              return this._AudioCount;
            }
        }
///<summary> List of all the Text streams available in the file, type TextTrack[trackindex] to access a specific track</summary>
public List<TextTrack> Text
        {
            get
            {
                if (this._Text == null)
                {
                   getTextInfo();
                }
                
                return this._Text;
            }
        }
private void getTextInfo()
        {
            
                if (this._Text == null)
                {
                    this._Text = new List<TextTrack>();
                    int num1 = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Text, -1);
                    if (num1 > 0)
                    {
                        int num3 = num1 - 1;
                        for (int num2 = 0; num2 <= num3; num2++)
                        {TextTrack _tracktemp_ = new TextTrack();
                            
                                                    
_tracktemp_.Count= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Count");
_tracktemp_.StreamCount= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"StreamCount");
_tracktemp_.StreamKind= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"StreamKind");
_tracktemp_.StreamKindID= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"StreamKindID");
_tracktemp_.Inform= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Inform");
_tracktemp_.ID= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"ID");
_tracktemp_.UniqueID= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"UniqueID");
_tracktemp_.Title= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Title");
_tracktemp_.Codec= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Codec");
_tracktemp_.CodecString= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Codec/String");
_tracktemp_.CodecUrl= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Codec/Url");
_tracktemp_.Delay= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Delay");
_tracktemp_.Video0Delay= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Video0_Delay");
_tracktemp_.PlayTime= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"PlayTime");
_tracktemp_.PlayTimeString= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"PlayTime/String");
_tracktemp_.PlayTimeString1= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"PlayTime/String1");
_tracktemp_.PlayTimeString2= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"PlayTime/String2");
_tracktemp_.PlayTimeString3= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"PlayTime/String3");
_tracktemp_.Language= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Language");
_tracktemp_.LanguageString= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Language/String");
_tracktemp_.LanguageMore= GetSpecificMediaInfo(MediaInfoStreamKind.Text,num2,"Language_More");

                        
                         this._Text.Add(_tracktemp_);

                    }
                    }
            
            
            }
        }
///<summary> Number of Text streams in the file</summary>
public Int32 TextCount
        {
            get
            {
              return this._TextCount;
            }
        }
///<summary> List of all the Chapters streams available in the file, type ChaptersTrack[trackindex] to access a specific track</summary>
public List<ChaptersTrack> Chapters
        {
            get
            {
                if (this._Chapters == null)
                {
                   getChaptersInfo();
                }
                
                return this._Chapters;
            }
        }
private void getChaptersInfo()
        {
            
                if (this._Chapters == null)
                {
                    this._Chapters = new List<ChaptersTrack>();
                    int num1 = MediaInfo.MediaInfo_Count_Get(this.Handle, MediaInfoStreamKind.Chapters, -1);
                    if (num1 > 0)
                    {
                        int num3 = num1 - 1;
                        for (int num2 = 0; num2 <= num3; num2++)
                        {ChaptersTrack _tracktemp_ = new ChaptersTrack();
                            
                                                    
_tracktemp_.Count= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Count");
_tracktemp_.StreamCount= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"StreamCount");
_tracktemp_.StreamKind= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"StreamKind");
_tracktemp_.StreamKindID= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"StreamKindID");
_tracktemp_.Inform= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Inform");
_tracktemp_.ID= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"ID");
_tracktemp_.UniqueID= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"UniqueID");
_tracktemp_.Title= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Title");
_tracktemp_.Codec= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Codec");
_tracktemp_.CodecString= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Codec/String");
_tracktemp_.CodecUrl= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Codec/Url");
_tracktemp_.Total= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Total");
_tracktemp_.Language= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Language");
_tracktemp_.LanguageString= GetSpecificMediaInfo(MediaInfoStreamKind.Chapters,num2,"Language/String");

                        
                         this._Chapters.Add(_tracktemp_);

                    }
                    }
            
            
            }
        }
///<summary> Number of Chapters streams in the file</summary>
public Int32 ChaptersCount
        {
            get
            {
              return this._ChaptersCount;
            }
        }

    }
}
