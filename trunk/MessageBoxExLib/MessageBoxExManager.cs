using System;
using System.IO;
using System.Collections;
using System.Resources;
using System.Reflection;

namespace Utils.MessageBoxExLib
{
	/// <summary>
	/// Manages a collection of MessageBoxes. Basically manages the
	/// saved response handling for messageBoxes.
	/// </summary>
	public class MessageBoxExManager
	{
		#region Fields
        private static Hashtable _standardButtonsText = new Hashtable();
		#endregion
        #region Static ctor
        static MessageBoxExManager()
        {
            _standardButtonsText[MessageBoxExButtons.Ok.ToString()] = "Ok";
            _standardButtonsText[MessageBoxExButtons.Cancel.ToString()] = "Cancel";
            _standardButtonsText[MessageBoxExButtons.Yes.ToString()] = "Yes";
            _standardButtonsText[MessageBoxExButtons.No.ToString()] = "No";
            _standardButtonsText[MessageBoxExButtons.Abort.ToString()] = "Abort";
            _standardButtonsText[MessageBoxExButtons.Retry.ToString()] = "Retry";
            _standardButtonsText[MessageBoxExButtons.Ignore.ToString()] = "Ignore";
        }
        #endregion
        #region internal methods
        /// <summary>
        /// Returns the localized string for standard button texts like,
        /// "Ok", "Cancel" etc.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetLocalizedString(string key)
        {
            if(_standardButtonsText.ContainsKey(key))
            {
                return (string)_standardButtonsText[key];
            }
            else
            {
                return null;
            }
        }
		#endregion
	}
}
