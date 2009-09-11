// ****************************************************************************
// 
// Copyright (C) 2009  Jarrett Vance
// 
// code from http://jvance.com/pages/ChapterGrabber.xhtml
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MeGUI
{
  public class MplsExtractor : ChapterExtractor
  {
    public override string[] Extensions
    {
      get { return new string[] { "mpls" }; }
    }

    public override bool SupportsMultipleStreams
    {
      get { return false; }
    }

    public override List<ChapterInfo> GetStreams(string location)
    {
      ChapterInfo pgc = new ChapterInfo();
      List<Chapter> chapters = new List<Chapter>();
      pgc.SourceName = location;
      pgc.SourceHash = ChapterExtractor.ComputeMD5Sum(location);
      pgc.SourceType = "Blu-Ray";
      pgc.Title = Path.GetFileNameWithoutExtension(location);

      FileInfo fileInfo = new FileInfo(location);
      byte[] data = File.ReadAllBytes(location);

      string fileType = ASCIIEncoding.ASCII.GetString(data, 0, 8);

      if ((fileType != "MPLS0100" && fileType != "MPLS0200")
        /*|| data[45] != 1*/)
      {
        throw new Exception(string.Format(
            "Playlist {0} has an unknown file type {1}.",
            fileInfo.Name, fileType));
      }

  //    Debug.WriteLine(string.Format("\tFileType: {0}", fileType));

      List<Clip> chapterClips = GetClips(data);
      pgc.Duration = new TimeSpan((long)(chapterClips.Sum(c => c.Length) * (double)TimeSpan.TicksPerSecond));
      OnStreamDetected(pgc);

      int chaptersIndex =
          ((int)data[12] << 24) +
          ((int)data[13] << 16) +
          ((int)data[14] << 8) +
          ((int)data[15]);

      int chaptersLength =
          ((int)data[chaptersIndex] << 24) +
          ((int)data[chaptersIndex + 1] << 16) +
          ((int)data[chaptersIndex + 2] << 8) +
          ((int)data[chaptersIndex + 3]);

      byte[] chapterData = new byte[chaptersLength];
      Array.Copy(data, chaptersIndex + 4, chapterData, 0, chaptersLength);

      int chapterCount = ((int)chapterData[0] << 8) + chapterData[1];
      int chapterOffset = 2;
      for (int chapterIndex = 0; chapterIndex < chapterCount; chapterIndex++)
      {
        if (chapterData[chapterOffset + 1] == 1)
        {
          int streamFileIndex =
              ((int)chapterData[chapterOffset + 2] << 8) +
              chapterData[chapterOffset + 3];

          Clip streamClip = chapterClips[streamFileIndex];

          long chapterTime =
              ((long)chapterData[chapterOffset + 4] << 24) +
              ((long)chapterData[chapterOffset + 5] << 16) +
              ((long)chapterData[chapterOffset + 6] << 8) +
              ((long)chapterData[chapterOffset + 7]);

          double chapterSeconds = (double)chapterTime / 45000D;
          double relativeSeconds = chapterSeconds - streamClip.TimeIn + streamClip.RelativeTimeIn;
          chapters.Add(new Chapter()
          {
              Name = "Chapter " + (chapterIndex + 1).ToString("D2"),//string.Empty,
              Time = new TimeSpan((long)(relativeSeconds * (double)TimeSpan.TicksPerSecond))
          });
        }
        chapterOffset += 14;
      }
      pgc.Chapters = chapters;

      //TODO: get real FPS
      pgc.FramesPerSecond = 25.0;

      OnChaptersLoaded(pgc);
      OnExtractionComplete();
      return new List<ChapterInfo>() { pgc };
    }

    List<Clip> GetClips(byte[] data)
    {
      List<Clip> chapterClips = new List<Clip>();

      int playlistIndex =
                    ((int)data[8] << 24) +
                    ((int)data[9] << 16) +
                    ((int)data[10] << 8) +
                    ((int)data[11]);

      // TODO: Hack for bad TSRemux output.
      int playlistLength = data.Length - playlistIndex - 4;
      int playlistLengthCorrect =
          ((int)data[playlistIndex] << 24) +
          ((int)data[playlistIndex + 1] << 16) +
          ((int)data[playlistIndex + 2] << 8) +
          ((int)data[playlistIndex + 3]);

      byte[] playlistData = new byte[playlistLength];
      Array.Copy(data, playlistIndex + 4,
          playlistData, 0, playlistData.Length);

      int streamFileCount = (((int)playlistData[2] << 8) + (int)playlistData[3]);

      int streamFileOffset = 6;
      for (int streamFileIndex = 0; streamFileIndex < streamFileCount; streamFileIndex++)
      {
          byte condition = (byte)(playlistData[streamFileOffset + 12] & 0xF);

        ulong timeIn =
            ((ulong)playlistData[streamFileOffset + 14] << 24) +
            ((ulong)playlistData[streamFileOffset + 15] << 16) +
            ((ulong)playlistData[streamFileOffset + 16] << 8) +
            ((ulong)playlistData[streamFileOffset + 17]);

        ulong timeOut =
            ((ulong)playlistData[streamFileOffset + 18] << 24) +
            ((ulong)playlistData[streamFileOffset + 19] << 16) +
            ((ulong)playlistData[streamFileOffset + 20] << 8) +
            ((ulong)playlistData[streamFileOffset + 21]);

        Clip streamClip = new Clip();

        streamClip.TimeIn = (double)timeIn / 45000D;
        streamClip.TimeOut = (double)timeOut / 45000D;
        streamClip.Length = streamClip.TimeOut - streamClip.TimeIn;
        streamClip.RelativeTimeIn = chapterClips.Sum(c => c.Length);
        streamClip.RelativeTimeOut = streamClip.RelativeTimeIn + streamClip.Length;
        chapterClips.Add(streamClip);

        //ignore angles

        streamFileOffset += 2 +
            ((int)playlistData[streamFileOffset] << 8) +
            ((int)playlistData[streamFileOffset + 1]);
      }
      return chapterClips;
    }

  }

  public class Clip
  {
    public int AngleIndex = 0;
    public string Name;
    public double TimeIn;
    public double TimeOut;
    public double RelativeTimeIn;
    public double RelativeTimeOut;
    public double Length;

    public ulong FileSize = 0;
    public ulong PayloadBytes = 0;
    public ulong PacketCount = 0;
    public double PacketSeconds = 0;

    public List<double> Chapters = new List<double>();
  }

}


//    public static void Test()
//    {

//                string title = playlist.Name;
//                string discSize = string.Format(
//                    "{0:N0}", BDROM.Size);

//                TimeSpan playlistTotalLength =
//                    new TimeSpan((long)(playlist.TotalLength * 10000000));
//                string totalLength = string.Format(
//                    "{0:D1}:{1:D2}:{2:D2}",
//                    playlistTotalLength.Hours,
//                    playlistTotalLength.Minutes,
//                    playlistTotalLength.Seconds);
//                string totalSize = string.Format(
//                    "{0:N0}", playlist.TotalSize);
//                string totalBitrate = string.Format(
//                    "{0:F2}",
//                    Math.Round((double)playlist.TotalBitRate / 10000) / 100);

//                TimeSpan playlistAngleLength =
//                    new TimeSpan((long)(playlist.AngleLength * 10000000));
//                string angleLength = string.Format(
//                    "{0:D1}:{1:D2}:{2:D2}",
//                    playlistAngleLength.Hours,
//                    playlistAngleLength.Minutes,
//                    playlistAngleLength.Seconds);
//                string angleSize = string.Format(
//                    "{0:N0}", playlist.AngleSize);
//                string angleBitrate = string.Format(
//                    "{0:F2}",
//                    Math.Round((double)playlist.AngleBitRate / 10000) / 100);

                  
//                    double chapterPosition = 0; 
//                    double chapterBits = 0;
//                    long chapterFrameCount = 0;
//                    double chapterSeconds = 0;
//                    double chapterMaxFrameSize = 0;
//                    double chapterMaxFrameLocation = 0;

//                    ushort diagPID  = playlist.VideoStreams[0].PID;

//                    int chapterIndex = 0;
//                    int clipIndex = 0;
//                    int diagIndex = 0;

//                    while (chapterIndex < playlist.Chapters.Count)
//                    {
//                        //TSStreamClip clip = null;
//                        //TSStreamFile file = null;

//                        //if (clipIndex < playlist.StreamClips.Count)
//                        //{
//                        //    clip = playlist.StreamClips[clipIndex];
//                        //    file = clip.StreamFile;
//                        //}

//                        double chapterStart = playlist.Chapters[chapterIndex];
//                        double chapterEnd = (chapterIndex < playlist.Chapters.Count - 1) ?
//                              playlist.Chapters[chapterIndex + 1] :
//                              chapterEnd = playlist.TotalLength;

//#region diag
//                        //if (clip != null &&
//                        //    clip.AngleIndex == 0 &&
//                        //    file != null &&
//                        //    file.StreamDiagnostics.ContainsKey(diagPID))
//                        //{
//                        //    diagList = file.StreamDiagnostics[diagPID];

//                        //    while (diagIndex < diagList.Count &&
//                        //        chapterPosition < chapterEnd)
//                        //    {
//                        //        TSStreamDiagnostics diag = diagList[diagIndex++];

//                        //        chapterPosition =
//                        //            diag.Marker -
//                        //            clip.TimeIn +
//                        //            clip.RelativeTimeIn;

//                        //        double seconds = diag.Interval;
//                        //        double bits = diag.Bytes * 8.0;

//                        //        chapterBits += bits;
//                        //        chapterSeconds += seconds;

//                        //        if (diag.Tag != null)
//                        //        {
//                        //            chapterFrameCount++;
//                        //        }

//                        //        if (bits > chapterMaxFrameSize * 8)
//                        //        {
//                        //            chapterMaxFrameSize = bits / 8;
//                        //            chapterMaxFrameLocation = chapterPosition;
//                        //        }
                                
//                        //    }
//                        //}

//                        //if (diagList == null ||
//                        //    diagIndex == diagList.Count)
//                        //{
//                        //    if (clipIndex < playlist.StreamClips.Count)
//                        //    {
//                        //        clipIndex++; diagIndex = 0;
//                        //    }
//                        //    else
//                        //    {
//                        //        chapterPosition = chapterEnd;
//                        //    }
//                        //}
//#endregion diag

//                        if (chapterPosition >= chapterEnd)
//                        {
//                            ++chapterIndex;

//                            //TimeSpan chapterMaxFrameSpan = new TimeSpan((long)(chapterMaxFrameLocation * 10000000));
//                            TimeSpan chapterStartSpan = new TimeSpan((long)(chapterStart * 10000000));
//                            TimeSpan chapterEndSpan = new TimeSpan((long)(chapterEnd * 10000000));
//                            TimeSpan chapterLengthSpan = new TimeSpan((long)(chapterLength * 10000000));

//                            //reset
//                            chapterBits = 0;
//                            chapterSeconds = 0;
//                            chapterFrameCount = 0;
//                            chapterMaxFrameSize = 0;
//                            chapterMaxFrameLocation = 0;

//                            //new chapter
//                        }
//                    }
//                }
//    }
