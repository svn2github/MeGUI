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

namespace MeGUI
{
  public class HddvdExtractor : ChapterExtractor
  {
    public override string[] Extensions
    {
      get { return new string[] { }; }
    }

    public override List<ChapterInfo> GetStreams(string location)
    {
      List<ChapterInfo> xpls = new List<ChapterInfo>();
      string path = Path.Combine(location, "ADV_OBJ");
      if (!Directory.Exists(path))
        throw new FileNotFoundException("Could not find ADV_OBJ folder on HD-DVD disc.");

      ChapterExtractor ex = new XplExtractor();
      ex.StreamDetected += (sender, args) => OnStreamDetected(args.ProgramChain);
      ex.ChaptersLoaded += (sender, args) => OnChaptersLoaded(args.ProgramChain);

      foreach (string file in Directory.GetFiles(path, "*.xpl"))
      {
          ChapterInfo pl = ex.GetStreams(file)[0];
          pl.SourceName = Path.GetFileName(file);
          xpls.Add(pl);
      }

      xpls = xpls.OrderByDescending(p => p.Duration).ToList();
      OnExtractionComplete();
      return xpls;
    }
  }
}
