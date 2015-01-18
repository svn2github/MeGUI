// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MeGUI
{
    public class MkvExtractor : ChapterExtractor
    {
        public override bool SupportsMultipleStreams
        {
            get { return false; }
        }

        public override string[] Extensions
        {
            get { return new string[] { "mkv" }; }
        }

        public override List<ChapterInfo> GetStreams(string location)
        {
            List<ChapterInfo> pgcs = new List<ChapterInfo>();
            List<Chapter> list = new List<Chapter>();
            string tempChapterFile = String.Empty;
            
            do
                tempChapterFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), Path.GetRandomFileName());
            while (File.Exists(tempChapterFile));

            MediaInfoFile oInfo = new MediaInfoFile(location);
            if (!oInfo.hasMKVChapters() || !oInfo.extractMKVChapters(tempChapterFile))
            {
                OnExtractionComplete();
                return pgcs;
            }

            int num = 0;
            TimeSpan ts = new TimeSpan(0);
            string time = String.Empty;
            string name = String.Empty;
            bool onTime = true;
            string[] lines = File.ReadAllLines(tempChapterFile);
            foreach (string line in lines)
            {
                if (onTime)
                {
                    num++;
                    //read time
                    time = line.Replace("CHAPTER" + num.ToString("00") + "=", "");
                    ts = TimeSpan.Parse(time);
                }
                else
                {
                    //read name
                    name = line.Replace("CHAPTER" + num.ToString("00") + "NAME=", "");
                    //add it to list
                    list.Add(new Chapter() { Name = name, Time = ts });
                }
                onTime = !onTime;
            }

            pgcs.Add(new ChapterInfo()
            {
                Chapters = list,
                SourceName = location,
                SourceHash = ChapterExtractor.ComputeMD5Sum(tempChapterFile),
                FramesPerSecond = oInfo.VideoInfo.FPS,
                Title = Path.GetFileNameWithoutExtension(location)
            });

            try { File.Delete(tempChapterFile); } catch {}
            
            OnStreamDetected(pgcs[0]);
            OnChaptersLoaded(pgcs[0]);
            OnExtractionComplete();
            return pgcs;
        }
    }
}
