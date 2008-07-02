using System;
using System.Collections.Generic;
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
                bool askAgain;
                bool bResult = askAbout("Problem adding profile '"
                    + profname + "':\r\none with the same name already exists. \r\nWhat do you want to do?",
                     "Duplicate profile", "Overwrite profile", "Skip profile", MessageBoxIcon.Exclamation, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutDuplicates = askAgain;
                mainForm.Settings.DialogSettings.DuplicateResponse = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.DuplicateResponse;
        }
         

        public bool useOneClick()
        {
            if (mainForm.Settings.DialogSettings.AskAboutVOBs)
            {
                bool askAgain;
                bool bResult = askAbout("Do you want to open this with the One Click\r\n" +
                    "Encoder (automated, easy to use) or the D2V\r\n" +
                    "Creator (manual, advanced)?", "Please choose your weapon", 
                    "One Click Encoder", "D2V Creator", MessageBoxIcon.Question, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutVOBs = askAgain;
                mainForm.Settings.DialogSettings.UseOneClick = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.UseOneClick;
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
                bool bResult = askAbout(string.Format("Your AviSynth clip is is the wrong colorspace, {0}.\r\n" +
                    "The colorspace should by YV12. Do you want me to add ConvertToYV12() to the end of your script?",
                    colorspace),
                    "Incorrect Colorspace", MessageBoxIcon.Warning, out askAgain);

                mainForm.Settings.DialogSettings.AskAboutYV12 = askAgain;
                mainForm.Settings.DialogSettings.AddConvertToYV12 = bResult;
                return bResult;
            }
            return mainForm.Settings.DialogSettings.AddConvertToYV12;
        }

        public void showRDO2Warning()
        {
            if (mainForm.Settings.DialogSettings.WarnAboutRDO2)
            {
                mainForm.Settings.DialogSettings.WarnAboutRDO2 = 
                    showMessage("RDO 2 (subpel refinement) is not supported in SVN builds. Clipping it to RDO 1",
                    "RDO 2 not supported", MessageBoxIcon.Warning);
            }
        }
    }
}
