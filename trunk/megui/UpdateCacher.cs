using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;
using MeGUI.core.util;


namespace MeGUI
{
    class UpdateCacher
    {
        public static void flushOldCachedFilesAsync(List<string> urls)
        {
            string updateCache = MeGUISettings.MeGUIUpdateCache;
            if (string.IsNullOrEmpty(updateCache)
                || !Directory.Exists(updateCache))
                return;

            DirectoryInfo fi = new DirectoryInfo(updateCache);
            FileInfo[] files = fi.GetFiles();

            for (int i = 0; i < urls.Count; ++i)
            {
                urls[i] = urls[i].ToLower();
            }

            foreach (FileInfo f in files)
            {
                if (urls.IndexOf(f.Name.ToLower()) < 0)
                {
                    if (DateTime.Now - f.LastWriteTime > new TimeSpan(7, 0, 0, 0, 0))
                        f.Delete();
                }
            }
        }

        private static void ensureSensibleCacheFolderExists()
        {
            if (string.IsNullOrEmpty(MeGUISettings.MeGUIUpdateCache))
                setSensibleCacheFolder();

            try
            {
                FileUtil.ensureDirectoryExists(MeGUISettings.MeGUIUpdateCache);
            }
            catch (IOException)
            {
                setSensibleCacheFolder();
                FileUtil.ensureDirectoryExists(MeGUISettings.MeGUIUpdateCache);
            }
        }


        private static void setSensibleCacheFolder()
        {
            MeGUISettings.MeGUIUpdateCache = Path.Combine(MainForm.Instance.MeGUIPath, "\\update_cache\\");
        }

        public static UpdateWindow.ErrorState DownloadFile(string url, Uri serverAddress,
            out Stream str, DownloadProgressChangedEventHandler wc_DownloadProgressChanged)
        {
            ensureSensibleCacheFolderExists();
            UpdateWindow.ErrorState er = UpdateWindow.ErrorState.Successful;
            string updateCache = MeGUISettings.MeGUIUpdateCache;

            string localFilename = Path.Combine(updateCache, url);
            FileInfo finfo = new FileInfo(localFilename);
            if (File.Exists(localFilename) && (finfo.Length == 0))
            {
                try
                {
                    finfo.Delete();
                }
                catch (IOException) { }
            }
            else if (File.Exists(localFilename))
                goto gotLocalFile;

            WebClient wc = new WebClient();
            ManualResetEvent mre = new ManualResetEvent(false);
            wc.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
            {
                if (e.Error != null)
                    er = UpdateWindow.ErrorState.CouldNotDownloadFile;

                mre.Set();
            };

            wc.DownloadProgressChanged += wc_DownloadProgressChanged;

            wc.DownloadFileAsync(new Uri(serverAddress, url), localFilename);
            mre.WaitOne();

        gotLocalFile:
            try
            {
                File.SetLastWriteTime(localFilename, DateTime.Now);
            }
            catch (IOException) { }

            try
            {
                str = File.OpenRead(localFilename);
            }
            catch (IOException)
            {
                str = null;
                return UpdateWindow.ErrorState.CouldNotDownloadFile;
            }

            return er;
        }

        public static void FlushFile(string p)
        {
            string localFilename = Path.Combine(MeGUISettings.MeGUIUpdateCache, p);
            try
            {
                File.Delete(localFilename);
            }
            catch (IOException) { }
        }
    }
}