// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;

namespace MeGUI.core.util
{
    delegate bool FileExists(string filename);

    class FileUtil
    {
        public static DirectoryInfo CreateTempDirectory()
        {
            while (true)
            {
                string file = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                if (!File.Exists(file) && !Directory.Exists(file))
                {
                    MainForm.Instance.DeleteOnClosing(file);
                    return Directory.CreateDirectory(file);
                }
            }
        }

        public static bool DeleteFile(string strFile)
        {
            try
            {
                File.Delete(strFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateZipFile(string path, string filename)
        {
            using (ZipOutputStream outputFile = new ZipOutputStream(File.OpenWrite(filename)))
            {
                foreach (string file in FileUtil.AllFiles(path))
                {
                    ZipEntry newEntry = new ZipEntry(file.Substring(path.Length).TrimStart('\\', '/'));
                    outputFile.PutNextEntry(newEntry);
                    FileStream input = File.OpenRead(file);
                    FileUtil.copyData(input, outputFile);
                    input.Close();
                }
            }
        }

        public static void ExtractZipFile(string file, string extractFolder)
        {
            ExtractZipFile(File.OpenRead(file), extractFolder);
        }

        public static void ExtractZipFile(Stream s, string extractFolder)
        {
            using (ZipFile inputFile = new ZipFile(s))
            {
                foreach (ZipEntry entry in inputFile)
                {
                    string pathname = Path.Combine(extractFolder, entry.Name);
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(pathname);
                    }
                    else // entry.isFile
                    {
                        System.Diagnostics.Debug.Assert(entry.IsFile);
                        FileUtil.ensureDirectoryExists(Path.GetDirectoryName(pathname));
                        Stream outputStream = File.OpenWrite(pathname);
                        FileUtil.copyData(inputFile.GetInputStream(entry), outputStream);
                        outputStream.Close();
                        File.SetLastWriteTime(pathname, entry.DateTime);
                    }
                }
            }
        }

        public static void DeleteDirectoryIfExists(string p, bool recursive)
        {
            if (Directory.Exists(p))
                Directory.Delete(p, recursive);
        }

        public static DirectoryInfo ensureDirectoryExists(string p)
        {
            if (Directory.Exists(p))
                return new DirectoryInfo(p);
            if (string.IsNullOrEmpty(p))
                throw new IOException("Can't create directory");
            ensureDirectoryExists(GetDirectoryName(p));
            System.Threading.Thread.Sleep(100);
            return Directory.CreateDirectory(p);
        }

        public static string GetDirectoryName(string file)
        {
            string path = string.Empty;
            try
            {
                path = Path.GetDirectoryName(file);
            }
            catch { }
            return path;
        }

        /// <summary>
        /// Generates a unique filename by adding numbers to the filename.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="fileExists"></param>
        /// <returns></returns>
        public static string getUniqueFilename(string original, FileExists fileExists)
        {
            if (!fileExists(original)) return original;
            string prefix = Path.Combine(Path.GetDirectoryName(original),
                Path.GetFileNameWithoutExtension(original)) + "_";
            string suffix = Path.GetExtension(original);
            for (int i = 0; true; i++)
            {
                string filename = prefix + i + suffix;
                if (!fileExists(filename)) return filename;
            }
        }

        public static List<string> AllFiles(string folder)
        {
            List<string> list = new List<string>();
            AddFiles(folder, list);
            return list;
        }

        private static void AddFiles(string folder, List<string> list)
        {
            list.AddRange(Directory.GetFiles(folder));
            foreach (string subFolder in Directory.GetDirectories(folder))
                AddFiles(subFolder, list);
        }

        private const int BUFFER_SIZE = 2 * 1024 * 1024; // 2 MBs
        public static void copyData(Stream input, Stream output)
        {
            int count = -1;
            byte[] data = new byte[BUFFER_SIZE];
            while ((count = input.Read(data, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(data, 0, count);
            }
        }

        /// <summary>
        /// Returns the full path and filename, but without the extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathWithoutExtension(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns TimeSpan value formatted
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToShortString(TimeSpan ts)
        {
            string time;
            time = ts.Hours.ToString("00");
            time = time + ":" + ts.Minutes.ToString("00");
            time = time + ":" + ts.Seconds.ToString("00");
            time = time + "." + ts.Milliseconds.ToString("000");
            return time;
        }

        /// <summary>
        /// Adds extra to the filename, modifying the filename but keeping the extension and folder the same.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static string AddToFileName(string filename, string extra)
        {
            return Path.Combine(
                Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + extra + Path.GetExtension(filename));
        }

        /// <summary>
        /// Returns true if the filename matches the filter specified. The format
        /// of the filter is the same as that of a FileDialog.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool MatchesFilter(string filter, string filename)
        {
            if (string.IsNullOrEmpty(filter))
                return true;

            bool bIsFolder = Directory.Exists(filename);

            filter = filter.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            filename = Path.GetFileName(filename).ToLower(System.Globalization.CultureInfo.InvariantCulture);
            string[] filters = filter.Split('|');

            for (int i = 1; i < filters.Length; i += 2)
            {
                string[] iFilters = filters[i].Split(';');
                foreach (string f in iFilters)
                {
                    if (f.IndexOf('*') > -1)
                    {
                        if (!f.StartsWith("*."))
                            throw new Exception("Invalid filter format");

                        if (f == "*.*" && filename.IndexOf('.') > -1)
                            return true;

                        if (f == "*." && bIsFolder)
                            return true;

                        string extension = f.Substring(1);
                        if (filename.EndsWith(extension))
                            return true;
                    }
                    else if (f == filename)
                        return true;
                    else return false;

                }
            }

            return false;
        }

        /// <summary>
        /// Backup File
        /// </summary>
        /// <param name"sourcePath">Path of the Source file</param>
        /// <param name="overwrite"></param>
        public static void BackupFile(string sourcePath, bool overwrite)
        {
            try
            {
                if (File.Exists(sourcePath))
                {
                    String targetPath;
                    if (sourcePath.Contains(System.Windows.Forms.Application.StartupPath))
                        targetPath = sourcePath.Replace(System.Windows.Forms.Application.StartupPath, System.Windows.Forms.Application.StartupPath + @"\backup");
                    else
                        targetPath = System.Windows.Forms.Application.StartupPath + @"\backup\" + (new FileInfo(sourcePath)).Name;
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);

                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(targetPath));

                    File.Move(sourcePath, targetPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while moving file: \n" + sourcePath + "\n" + ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if a directory is writable
        /// </summary>
        /// <param name"strPath">path to check</param>
        public static bool IsDirWriteable(string strPath)
        {
            try
            {
                bool bDirectoryCreated = false;

                // does the root directory exists
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                    bDirectoryCreated = true;
                }

                string newFilePath = string.Empty;
                // combine the random file name with the path
                do
                    newFilePath = Path.Combine(strPath, Path.GetRandomFileName());
                while (File.Exists(newFilePath));

                // create & delete the file
                FileStream fs = File.Create(newFilePath);
                fs.Close();
                File.Delete(newFilePath);

                if (bDirectoryCreated)
                    Directory.Delete(strPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to delete all files and directories listed 
        /// in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        public static LogItem DeleteIntermediateFiles(List<string> files, bool bAlwaysAddLog, bool askAboutDelete)
        {
            bool bShowLog = false;
            LogItem i = new LogItem(string.Format("[{0:G}] {1}", DateTime.Now, "Deleting intermediate files"));

            List<string> arrFiles = new List<string>();
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                    continue;
                else if (!File.Exists(file))
                    continue;
                if (!arrFiles.Contains(file))
                    arrFiles.Add(file);
            }

            if (arrFiles.Count > 0)
            {
                bShowLog = true;
                bool delete = true;

                if (askAboutDelete)
                    delete = MainForm.Instance.DialogManager.DeleteIntermediateFiles(arrFiles);
                if (!delete)
                    return null;

                // delete all files first
                foreach (string file in arrFiles)
                {
                    int iCounter = 0;
                    while (File.Exists(file))
                    {
                        try
                        {
                            File.Delete(file);
                            i.LogEvent("Successfully deleted " + file);
                        }
                        catch (IOException e)
                        {
                            if (++iCounter >= 3)
                            {
                                i.LogValue("Problem deleting " + file, e.Message, ImageType.Warning);
                                break;
                            }
                            else
                                System.Threading.Thread.Sleep(2000);
                        }
                    }
                }
            }

            // delete empty directories
            foreach (string file in files)
            {
                try
                {
                    if (Directory.Exists(file))
                    {
                        bShowLog = true;
                        if (Directory.GetFiles(file, "*.*", SearchOption.AllDirectories).Length == 0)
                        {
                            Directory.Delete(file, true);
                            i.LogEvent("Successfully deleted directory " + file);
                        }
                        else
                            i.LogEvent("Did not delete " + file + " as the directory is not empty.", ImageType.Warning);
                    }
                }
                catch (IOException e)
                {
                    i.LogValue("Problem deleting directory " + file, e.Message, ImageType.Warning);
                }
            }
            if (bAlwaysAddLog || bShowLog)
                return i;
            return null;
        }

        /// <summary>
        /// Detects the AviSynth version/date and writes it into the log
        /// </summary>
        /// <param name="oLog">the LogItem where the information should be added</param>
        public static void getAvisynthVersion(LogItem oLog)
        {
            string fileVersion = string.Empty;
            string fileDate = string.Empty;
            bool bLocal = false;
            bool bFound = false;

            string syswow64path = Environment.GetFolderPath(Environment.SpecialFolder.System)
                .ToLowerInvariant().Replace("\\system32", "\\SysWOW64");
#if x86
            // on x86, try the SysWOW64 folder first
            if (File.Exists(Path.Combine(syswow64path, "avisynth.dll")))
            {
                string path = Path.Combine(syswow64path, "avisynth.dll");
                FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(path);
                fileVersion = FileProperties.FileVersion;
                fileDate = File.GetLastWriteTimeUtc(path).ToString("dd-MM-yyyy");
                bFound = true;
            }
            else if (!Directory.Exists(syswow64path)
                && File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "avisynth.dll")))
#endif
#if x64
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "avisynth.dll")))
#endif
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "avisynth.dll");
                FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(path);
                fileVersion = FileProperties.FileVersion;
                fileDate = File.GetLastWriteTimeUtc(path).ToString("dd-MM-yyyy");
                bFound = true;
            }

            if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "avisynth.dll")))
            {
                string pathRoot = Path.GetDirectoryName(Application.ExecutablePath);
                if (File.Exists(MainForm.Instance.Settings.AviSynthPath))
                {
                    string pathTool = Path.GetDirectoryName(MainForm.Instance.Settings.AviSynthPath);
                    if (File.GetLastWriteTimeUtc(Path.Combine(pathRoot, "avisynth.dll")) != File.GetLastWriteTimeUtc(Path.Combine(pathTool, "avisynth.dll")))
                        File.Copy(Path.Combine(pathTool, "avisynth.dll"), Path.Combine(pathRoot, "Avisynth.dll"), true);
                    if (!File.Exists(Path.Combine(pathRoot, "devil.dll")) ||
                        File.GetLastWriteTimeUtc(Path.Combine(pathRoot, "devil.dll")) != File.GetLastWriteTimeUtc(Path.Combine(pathTool, "devil.dll")))
                        File.Copy(Path.Combine(pathTool, "devil.dll"), Path.Combine(pathRoot, "DevIL.dll"), true);
                }

                if (bFound && oLog != null)
                {
                    string strVersion = string.Empty;
                    if (string.IsNullOrEmpty(fileVersion))
                        strVersion = " (" + fileDate + ")";
                    else
                        strVersion = fileVersion.Replace(", ", ".").ToString() + " (" + fileDate + ")";
                    oLog.LogValue("AviSynth", strVersion + " (inactive)");
                }

                FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(Path.Combine(pathRoot, "avisynth.dll"));
                fileVersion = FileProperties.FileVersion;
                fileDate = File.GetLastWriteTimeUtc(Path.Combine(pathRoot, "avisynth.dll")).ToString("dd-MM-yyyy");
                bLocal = true;

                if (oLog != null)
                {
                    string strVersion = string.Empty;
                    if (string.IsNullOrEmpty(fileVersion))
                        strVersion = " (" + fileDate + ")";
                    else
                        strVersion = fileVersion.Replace(", ", ".").ToString() + " (" + fileDate + ")";
                    if (bFound)
                        strVersion += " (active)";
                    oLog.LogValue("AviSynth portable", strVersion);
                }
            }
            else
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "devil.dll")))
                    FileUtil.DeleteFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "devil.dll"));
                if (bFound && oLog != null)
                {
                    string strVersion = string.Empty;
                    if (string.IsNullOrEmpty(fileVersion))
                        strVersion = " (" + fileDate + ")";
                    else
                        strVersion = fileVersion.Replace(", ", ".").ToString() + " (" + fileDate + ")";
                    oLog.LogValue("AviSynth", strVersion);
                }
            }

            if (!bFound && !bLocal)
            {
                if (File.Exists(MainForm.Instance.Settings.AviSynthPath))
                {
                    if (oLog != null)
                        oLog.LogValue("AviSynth", "files will be copied into the MeGUI directory as AviSynth is not installed");
                    string path = Path.GetDirectoryName(MainForm.Instance.Settings.AviSynthPath);
                    try
                    {
                        File.Copy(Path.Combine(path, "avisynth.dll"), Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Avisynth.dll"), true);
                        File.Copy(Path.Combine(path, "devil.dll"), Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DevIL.dll"), true);
                        path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "avisynth.dll");
                        FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(path);
                        fileVersion = FileProperties.FileVersion;
                        fileDate = File.GetLastWriteTimeUtc(path).ToString("dd-MM-yyyy");
                        bLocal = true;

                        if (oLog != null)
                        {
                            string strVersion = string.Empty;
                            if (string.IsNullOrEmpty(fileVersion))
                                strVersion = " (" + fileDate + ")";
                            else
                                strVersion = fileVersion.Replace(", ", ".").ToString() + " (" + fileDate + ")";
                            oLog.LogValue("AviSynth portable", strVersion);
                        }
                    }
                    catch { }
                }
            }
            if (!bLocal && !bFound && oLog != null)
                oLog.LogValue("AviSynth", "not installed", ImageType.Error);
            MainForm.Instance.Settings.PortableAviSynth = bLocal;
        }

        /// <summary>
        /// Detects the file version/date and writes it into the log
        /// </summary>
        /// <param name="strName">the name in the log</param>
        /// <param name="strFile">the file to check</param>
        /// <param name="oLog">the LogItem where the information should be added</param>
        public static void GetFileInformation(string strName, string strFile, ref LogItem oLog)
        {
            string fileVersion = string.Empty;
            string fileDate = string.Empty;
            if (File.Exists(strFile))
            {
                FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(strFile);
                fileVersion = FileProperties.FileVersion;
                fileDate = File.GetLastWriteTimeUtc(strFile).ToString("dd-MM-yyyy");

                if (string.IsNullOrEmpty(fileVersion))
                    oLog.LogValue(strName, " (" + fileDate + ")");
                else
                    oLog.LogValue(strName, fileVersion.Replace(", ", ".").ToString() + " (" + fileDate + ")");
            }
            else
                if (strName.Contains("Haali"))
                    oLog.LogValue(strName, "not installed", ImageType.Warning);
                else
                    oLog.LogValue(strName, "not installed", ImageType.Error);
        }

        /// <summary>
        /// Create Chapters XML File from OGG Chapters File
        /// </summary>
        /// <param name="inFile">input</inFile>
        public static void CreateXMLFromOGGChapFile(string inFile)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                sb.AppendLine("<!-- GPAC 3GPP Text Stream -->");
                sb.AppendLine("<TextStream version=\"1.1\">");
                sb.AppendLine("<TextStreamHeader>");
                sb.AppendLine("<TextSampleDescription>");
                sb.AppendLine("</TextSampleDescription>");
                sb.AppendLine("</TextStreamHeader>");

                using (StreamReader sr = new StreamReader(inFile))
                {
                    string line = null;
                    string chapTitle = null;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        i++;
                        if (i % 2 == 1)
                            sb.Append("<TextSample sampleTime=\"" + line.Substring(line.IndexOf("=") + 1) + "\"");
                        else
                        {
                            chapTitle = System.Text.RegularExpressions.Regex.Replace(line.Substring(line.IndexOf("=") + 1), "\"", "&quot;");
                            sb.Append(" text=\"" + chapTitle + "\"></TextSample>" + Environment.NewLine);
                        }
                    }
                }
                sb.AppendLine("</TextStream>");

                using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(inFile), Path.GetFileNameWithoutExtension(inFile) + ".xml")))
                {
                    sw.Write(sb.ToString());
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        private static object _locker = new object();
        public static void WriteToFile(string fileName, string text, bool append)
        {
            try
            {
                lock (_locker)
                {
                    if (append)
                        System.IO.File.AppendAllText(fileName, text);
                    else
                        System.IO.File.WriteAllText(fileName, text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error writing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}