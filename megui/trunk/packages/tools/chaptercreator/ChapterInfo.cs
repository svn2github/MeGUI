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
using System.Xml;
using System.Windows.Forms;

namespace MeGUI
{
    public class ChapterInfo
    {
        public string Title { get; set; }
        public string LangCode { get; set; }
        public string SourceName { get; set; }
        public int TitleNumber { get; set; }
        public string SourceType { get; set; }
        public string SourceHash { get; set; }
        public double FramesPerSecond { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Chapter> Chapters { get; set; }

        public override string ToString()
        {
            if (Chapters.Count != 1)
                return string.Format("{0} - {1}  -  {2}  -  [{3} Chapters]", Title, SourceName, string.Format("{0:00}:{1:00}:{2:00}.{3:000}", Duration.TotalHours, Duration.Minutes, Duration.Seconds, Duration.Milliseconds), Chapters.Count);
            else
                return string.Format("{0} - {1}  -  {2}  -  [{3} Chapter]", Title, SourceName, string.Format("{0:00}:{1:00}:{2:00}.{3:000}", Duration.TotalHours, Duration.Minutes, Duration.Seconds, Duration.Milliseconds), Chapters.Count);
        }

        public void ChangeFps(double fps)
        {
            for (int i = 0; i < Chapters.Count; i++)
            {
                Chapter c = Chapters[i];
                double frames = c.Time.TotalSeconds * FramesPerSecond;
                Chapters[i] = new Chapter() { Name = c.Name, Time = new TimeSpan((long)Math.Round(frames / fps * TimeSpan.TicksPerSecond)) };
            }

            double totalFrames = Duration.TotalSeconds * FramesPerSecond;
            Duration = new TimeSpan((long)Math.Round((totalFrames / fps) * TimeSpan.TicksPerSecond));
            FramesPerSecond = fps;
        }

        public void SaveText(string filename)
        {
            List<string> lines = new List<string>();
            int i = 0;
            foreach (Chapter c in Chapters)
            {
                i++;
                if (c.Time.ToString().Length == 8)
                    lines.Add("CHAPTER" + i.ToString("00") + "=" + c.Time.ToString() + ".000"); // better formating
                else if (c.Time.ToString().Length > 12)
                    lines.Add("CHAPTER" + i.ToString("00") + "=" + c.Time.ToString().Substring(0, 12)); // remove some duration length too long
                else
                    lines.Add("CHAPTER" + i.ToString("00") + "=" + c.Time.ToString());
                lines.Add("CHAPTER" + i.ToString("00") + "NAME=" + c.Name);
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveQpfile(string filename)
        {
            List<string> lines = new List<string>();
            foreach (Chapter c in Chapters)
                lines.Add(string.Format("{0} K", (long)Math.Round(c.Time.TotalSeconds * FramesPerSecond)));
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveCelltimes(string filename)
        {
            List<string> lines = new List<string>();
            foreach (Chapter c in Chapters)
                lines.Add(((long)Math.Round(c.Time.TotalSeconds * FramesPerSecond)).ToString());
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveTsmuxerMeta(string filename)
        {
            string text = "--custom-" + Environment.NewLine + "chapters=";
            foreach (Chapter c in Chapters)
                text += c.Time.ToString() + ";";
            text = text.Substring(0, text.Length - 1);
            File.WriteAllText(filename, text);
        }

        public void SaveTimecodes(string filename)
        {
            List<string> lines = new List<string>();
            foreach (Chapter c in Chapters)
                lines.Add(c.Time.ToString());
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveXml(string filename)
        {
            Random rndb = new Random();
            XmlTextWriter xmlchap = new XmlTextWriter(filename, Encoding.UTF8);
            xmlchap.Formatting = Formatting.Indented;
            xmlchap.WriteStartDocument();
            xmlchap.WriteComment("<!DOCTYPE Tags SYSTEM " + "\"" + "matroskatags.dtd" + "\"" + ">");
            xmlchap.WriteStartElement("Chapters");
            xmlchap.WriteStartElement("EditionEntry");
            xmlchap.WriteElementString("EditionFlagHidden", "0");
            xmlchap.WriteElementString("EditionFlagDefault", "0");
            xmlchap.WriteElementString("EditionUID", Convert.ToString(rndb.Next(1, Int32.MaxValue)));
            foreach (Chapter c in Chapters)
            {
                xmlchap.WriteStartElement("ChapterAtom");
                xmlchap.WriteStartElement("ChapterDisplay");
                xmlchap.WriteElementString("ChapterString", c.Name);
                xmlchap.WriteElementString("ChapterLanguage", String.IsNullOrEmpty(LangCode) ? "und" : LangCode);
                xmlchap.WriteEndElement();
                xmlchap.WriteElementString("ChapterUID", Convert.ToString(rndb.Next(1, Int32.MaxValue)));
                if (c.Time.ToString().Length == 8)
                    xmlchap.WriteElementString("ChapterTimeStart", c.Time.ToString() + ".0000000");
                else
                    xmlchap.WriteElementString("ChapterTimeStart", c.Time.ToString());
                xmlchap.WriteElementString("ChapterFlagHidden", "0");
                xmlchap.WriteElementString("ChapterFlagEnabled", "1");
                xmlchap.WriteEndElement();
            }
            xmlchap.WriteEndElement();
            xmlchap.WriteEndElement();
            xmlchap.Flush();
            xmlchap.Close();
        }                           
    }
}