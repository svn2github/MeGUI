// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using MeGUI.packages.tools.besplitter;

namespace MeGUI.core.details._0_2_6_1017_jobloader
{
    public class Loader
    {

        public static TaggedJob loadJob(string name)
        {
            try
            {
                XmlDocument d = new XmlDocument();
                d.LoadXml(File.ReadAllText(name));

                TaggedJob t = new TaggedJob();
                t.EncodingSpeed = GetText(d, "EncodingSpeed");
                
                string s = GetText(d, "Start");
                if (s != null)
                    t.Start = DateTime.Parse(s);

                s = GetText(d, "End");
                if (s != null)
                    t.End = DateTime.Parse(s);

                t.Name = GetText(d, "Name");
                t.OwningWorker = GetText(d, "OwningWorker");
                t.EnabledJobNames = GetList(d, "EnabledJobNames");
                t.RequiredJobNames = GetList(d, "RequiredJobNames");

                switch (GetText(d, "Status"))
                {
                    case null:
                        break;

                    case "ABORTED":
                        t.Status = JobStatus.ABORTED;
                        break;

                    case "DONE":
                        t.Status = JobStatus.DONE;
                        break;

                    case "ERROR":
                        t.Status = JobStatus.ERROR;
                        break;

                    case "POSTPONED":
                        t.Status = JobStatus.POSTPONED;
                        break;

                    case "PROCESSING":
                        t.Status = JobStatus.PROCESSING;
                        break;

                    case "SKIP":
                        t.Status = JobStatus.SKIP;
                        break;

                    case "WAITING":
                        t.Status = JobStatus.WAITING;
                        break;
                }

                MeGUI.Job j2 = MeGUI.core.util.Util.XmlDeserialize<MeGUI.Job>(name);
                t.Job = j2;

                return t;
            }
            catch (Exception) { return null; }
        }

        private static string GetText(XmlDocument d, string name)
        {
            try
            {
                XmlNode n = d.SelectSingleNode("/Job/" + name);
                return n.InnerText;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static List<string> GetList(XmlDocument d, string name)
        {
            try
            {
                List<string> result = new List<string>();
                XmlNode n = d.SelectSingleNode("/Job/" + name);
                foreach (XmlNode x in n.ChildNodes)
                    if (x.Name == "string")
                        result.Add(x.Value);
                return result;
            }
            catch (Exception) { return null; }
        }
    }
}
