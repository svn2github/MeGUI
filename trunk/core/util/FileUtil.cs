using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MeGUI.core.util
{
    delegate bool FileExists(string filename);

    class FileUtil
    {
        public static void ensureDirectoryExists(string p)
        {
            if (Directory.Exists(p)) return;
            if (string.IsNullOrEmpty(p)) throw new IOException("Can't create directory");
            ensureDirectoryExists(Path.GetDirectoryName(p));
            Directory.CreateDirectory(p);
        }
        /// <summary>
        /// Generates a filename not in the list
        /// </summary>
        /// <param name="original"></param>
        /// <param name="filenames"></param>
        /// <returns></returns>
        public static string getUniqueFilename(string original, List<string> filenames)
        {
            return getUniqueFilename(original, new FileExists(delegate(string test)
            {
                return filenames.Contains(test);
            }));
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
    }
}
