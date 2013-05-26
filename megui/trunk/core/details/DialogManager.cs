// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Windows;
using System.Windows.Forms;

using Utils.MessageBoxExLib;

namespace MeGUI
{
    public enum DuplicateResponse { OVERWRITE, RENAME, SKIP, ABORT };

    public class DialogManager
    {
        private MainForm mainForm;

        public DialogManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Creates a message box with the given text, title and icon. Also creates a 'don't show me again' checkbox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>The newly created message box</returns>
        private MessageBoxEx createMessageBox(string text, string caption, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = new MessageBoxEx();
            msgBox.Caption = caption;
            msgBox.Text = text;
            msgBox.Icon = icon;
            msgBox.AllowSaveResponse = true;
            msgBox.SaveResponseText = "Don't ask me this again";
            return msgBox;
        }        
        /// <summary>
        /// Shows a message dialog (without a question) with a 'don't ask again' checkbox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>Whether to show this again</returns>
        private bool showMessage(string text, string caption, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);
            msgBox.AddButtons(MessageBoxButtons.OK);
            msgBox.Show();
            return !msgBox.SaveResponseChecked;
        }
        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="icon">The icon to display</param>
        /// <param name="askAgain">Returns whether to show this dialog again</param>
        /// <returns>true if the user pressed yes, false otherwise</returns>
        private bool askAbout(string text, string caption, MessageBoxIcon icon, out bool askAgain)
        {
            return askAbout(text, caption, "Yes", "No", icon, out askAgain);
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="button1Text">The text on the first button</param>
        /// <param name="button2Text">The text on the second button</param>
        /// <param name="icon">The icon to display</param>
        /// <param name="askAgain">Returns whether to ask again</param>
        /// <returns>true if button 1 was pressed, false otherwise</returns>
        private bool askAbout(string text, string caption, string button1Text, string button2Text,
            MessageBoxIcon icon, out bool askAgain)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);

            msgBox.AddButton(button1Text, "true");
            msgBox.AddButton(button2Text, "false");

            string sResult = msgBox.Show();
            askAgain = !msgBox.SaveResponseChecked;
            return (sResult.Equals("true"));
        }

        /// <summary>
        /// Shows a custom dialog built on the MessageBoxEx system
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The window title to display</param>
        /// <param name="button1Text">The text on the first button</param>
        /// <param name="button2Text">The text on the second button</param>
        /// <param name="button2Text">The text on the third button</param>
        /// <param name="icon">The icon to display</param>
        /// <returns>0, 1 or 2 depending on the button pressed</returns>
        private int askAbout3(string text, string caption, string button1Text, string button2Text,
            string button3Text, MessageBoxIcon icon)
        {
            MessageBoxEx msgBox = createMessageBox(text, caption, icon);

            msgBox.AddButton(button1Text, "0");
            msgBox.AddButton(button2Text, "1");
            msgBox.AddButton(button3Text, "2");

            msgBox.AllowSaveResponse = false;

            string sResult = msgBox.Show();
            return Int32.Parse(sResult);
        }

        public bool overwriteJobOutput(string outputname)
        {
            if (mainForm.Settings.DialogSettings.AskAboutOverwriteJobOutput)
            {
                bool askAgain;
                bool bResult = askAbout("The output file, '" + outputname + "' already exists. Would you like to overwrite?",
                    "File Already Exists", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutOverwriteJobOutput = askAgain;
                mainForm.Settings.DialogSettings.OverwriteJobOutputResponse = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.OverwriteJobOutputResponse;
        }

        public bool overwriteProfile(string profname)
        {
            if (mainForm.Settings.DialogSettings.AskAboutDuplicates)
            {
                if (!MainForm.Instance.Settings.AutoUpdateSession)
                {
                    bool askAgain;
                    bool bResult = askAbout("Problem adding profile '"
                        + profname + "':\r\none with the same name already exists. \r\nWhat do you want to do?",
                         "Duplicate profile", "Overwrite profile", "Skip profile", MessageBoxIcon.Exclamation, out askAgain);

                    mainForm.Settings.DialogSettings.AskAboutDuplicates = askAgain;
                    mainForm.Settings.DialogSettings.DuplicateResponse = bResult;
                    return bResult;
                }
                else
                    return false; 
            }
            return mainForm.Settings.DialogSettings.DuplicateResponse;
        }
         
        public bool useOneClick()
        {
            if (mainForm.Settings.DialogSettings.AskAboutVOBs)
            {
                bool askAgain;
                bool bResult = askAbout("Do you want to open this file with the One Click\r\n" +
                    "Encoder (automated, easy to use) or the File\r\n" +
                    "Indexer (manual, advanced)?", "Please choose your preferred tool", 
                    "One Click Encoder", "File Indexer", MessageBoxIcon.Question, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutVOBs = askAgain;
                mainForm.Settings.DialogSettings.UseOneClick = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.UseOneClick;
        }

        public int AVSCreatorOpen(string videoInput)
        {
            int iResult = -1;
            MediaInfoFile iFile = new MediaInfoFile(videoInput);
            FileIndexerWindow.IndexType oIndexer;

            if (!iFile.recommendIndexer(out oIndexer))
                return iResult;

            if (oIndexer != FileIndexerWindow.IndexType.D2V && oIndexer != FileIndexerWindow.IndexType.DGA &&
                oIndexer != FileIndexerWindow.IndexType.DGI && oIndexer != FileIndexerWindow.IndexType.FFMS)
                return iResult;

            if (iFile.ContainerFileTypeString.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVI"))
            {
                iResult = askAbout3("Do you want to open this file with\r\n" +
                    "- One Click Encoder (full automated, easy to use) or\r\n" +
                    "- File Indexer (manual, advanced) or \r\n" +
                    "- AviSource (manual, expert, may cause problems)?", "Please choose your prefered way to open this file",
                    "One Click Encoder", "File Indexer", "AviSource", MessageBoxIcon.Question);
            }
            else
            {
                iResult = askAbout3("Do you want to open this file with\r\n" +
                    "- One Click Encoder (full automated, easy to use) or\r\n" +
                    "- File Indexer (manual, advanced) or \r\n" +
                    "- DirectShowSource (manual, expert, may cause problems)?", "Please choose your prefered way to open this file",
                    "One Click Encoder", "File Indexer", "DirectShowSource", MessageBoxIcon.Question);
            }
            return iResult;
        }

        public bool createJobs(string error)
        {
            if (mainForm.Settings.DialogSettings.AskAboutError)
            {
                bool askAgain;
                bool bResult = askAbout(string.Format("Your AviSynth clip has the following problem:\r\n{0}\r\nContinue anyway?", error),
                    "Problem in AviSynth script", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutError = askAgain;
                mainForm.Settings.DialogSettings.ContinueDespiteError = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.ContinueDespiteError;
        }

        public bool addConvertToYV12(string colorspace)
        {
            if (mainForm.Settings.DialogSettings.AskAboutYV12)
            {
                bool askAgain;
                bool bResult = askAbout("The colorspace of your clip is not in YV12...\r\n" +
                                        "Do you want me to add ConvertToYV12() to the end of your script ?",
                                        "Incorrect Colorspace", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutYV12 = askAgain;
                mainForm.Settings.DialogSettings.AddConvertToYV12 = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.AddConvertToYV12;
        }

        public bool DeleteIntermediateFiles(List<string> arrFiles)
        {
            if (mainForm.Settings.DialogSettings.AskAboutIntermediateDelete)
            {
                string strFiles = string.Empty; ;
                foreach (string file in arrFiles)
                    strFiles += "\r\n" + file;

                bool askAgain;
                bool bResult = askAbout("Do you really want to delete the intermediate files below?\r\nThese files may still be required as the job did not finish successfully.\r\n" + strFiles,
                                        "Confirm deletion of intermediate files", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutIntermediateDelete = askAgain;
                mainForm.Settings.DialogSettings.IntermediateDelete = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.IntermediateDelete;
        }
    }
}
