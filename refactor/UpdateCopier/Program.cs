using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace UpdateCopier
{
    public class CommandlineUpgradeData
    {
        public List<string> filename = new List<string>();
        public List<string> tempFilename = new List<string>();
        public string newVersion;
    }
    class Program
    {
        static void showCommandlineErrorMessage(string[] args)
        {
            StringBuilder cmdline = new StringBuilder();
            foreach (string arg in args)
                cmdline.AppendLine(arg);
            MessageBox.Show("Error in commandline update arguments: there aren't enough. No program files will be updated. Commandline:\r\n"
                + cmdline.ToString(), "Error in commandline",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Main(string[] args)
        {
            StringBuilder commandline = new StringBuilder();

            Dictionary<string, CommandlineUpgradeData> filesToCopy = new Dictionary<string,CommandlineUpgradeData>();
            
            bool restart = false;
            string appName = null;
            string lastComponentName = null;
            for (int i = 0; i < args.Length; i += 1)
            {
                if (args[i] == "--restart")
                {
                    if (args.Length > i + 1)
                    {
                        appName = args[i + 1];
                        restart = true;
                        i++;
                    }
                    else
                    {
                        showCommandlineErrorMessage(args);
                        return;
                    }
                }
                else if (args[i] == "--app")
                {
                    if (args.Length > i + 1)
                    {
                        appName = args[i + 1];
                        restart = false;
                        i++;
                    }
                    else
                    {
                        showCommandlineErrorMessage(args);
                        return;
                    }
                }
                else if (args[i] == "--component")
                {
                    if (args.Length > i + 2)
                    {
                        CommandlineUpgradeData data = new CommandlineUpgradeData();
                        data.newVersion = args[i + 2];
                        filesToCopy.Add(args[i+1], data);
                        lastComponentName = args[i+1];
                        i += 2;
                    }
                    else
                    {
                        showCommandlineErrorMessage(args);
                        return;
                    }
                }
                else
                {
                    if (args.Length > i + 1 && lastComponentName != null)
                    {
                        filesToCopy[lastComponentName].filename.Add(args[i]);
                        filesToCopy[lastComponentName].tempFilename.Add(args[i + 1]);
                        i++;
                    }
                    else
                    {
                        showCommandlineErrorMessage(args);
                        return;
                    }
                }
            }

            Thread.Sleep(2000);
            List<Exception> errorsEncountered = new List<Exception>();
            foreach (string file in filesToCopy.Keys)
            {
                bool succeeded = true;
                for (int i = 0; i < filesToCopy[file].tempFilename.Count; i++)
                {
                    try
                    {
                        File.Delete(filesToCopy[file].filename[i]);
                        File.Move(filesToCopy[file].tempFilename[i], filesToCopy[file].filename[i]);
                    }
                    catch (IOException)
                    {
                        succeeded = false;
                    }
                    catch (Exception e)
                    {
                        succeeded = false;
                        errorsEncountered.Add(e);
                    }
                }
                if (succeeded)
                    commandline.AppendFormat(@"--upgraded ""{0}"" ""{1}"" ", file, filesToCopy[file].newVersion);
                else
                    commandline.AppendFormat(@"--upgrade-failed ""{0}"" ", file);
            }
            if (!restart)
                commandline.Append("--dont-start");
            Process proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = appName;
            pstart.Arguments = commandline.ToString();
            pstart.UseShellExecute = false;
            proc.StartInfo = pstart;
            if (!proc.Start())
            {
                if (errorsEncountered.Count == 0)
                {
                    MessageBox.Show("Files updated but failed to restart MeGUI. You'll have to start it yourself.", "Failed to restart MeGUI",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string message = "The following errors were encountered when updating MeGUI:\r\n";
                    foreach (Exception e in errorsEncountered)
                        message += e.Message + "\r\n";
                    message += "Failed to restart MeGUI";
                    MessageBox.Show(message, "Errors in update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (errorsEncountered.Count == 0)
                return;
            string message1 = "The following errors were encountered when updating MeGUI:\r\n";
            foreach (Exception e in errorsEncountered)
                message1 += e.Message + "\r\n";
            MessageBox.Show(message1, "Errors in update", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
