using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utils.MessageBoxExLib
{
	/// <summary>
	/// An extended MessageBox with lot of customizing capabilities.
	/// </summary>
	public class MessageBoxEx
	{
		#region Fields
		private MessageBoxExForm _msgBox = new MessageBoxExForm();
		#endregion
		#region Properties
		/// <summary>
		/// Sets the caption of the message box
		/// </summary>
		public string Caption
		{
			set{_msgBox.Caption = value;}
		}

        /// <summary>
		/// Sets the text of the message box
		/// </summary>
		public string Text
		{
			set{_msgBox.Message = value;}
		}

		/// <summary>
		/// Sets the icon to show in the message box
		/// </summary>
		public Icon CustomIcon
		{
			set{_msgBox.CustomIcon = value;}
		}

		/// <summary>
		/// Sets the icon to show in the message box
		/// </summary>
		public MessageBoxIcon Icon
		{
			set{ _msgBox.StandardIcon = value;}
		}
		
		/// <summary>
		/// Sets the font for the text of the message box
		/// </summary>
		public Font Font
		{
			set{_msgBox.Font = value;}
		}

		/// <summary>
		/// Sets or Gets the ability of the  user to save his/her response
		/// </summary>
		public bool AllowSaveResponse
		{
			get{ return _msgBox.AllowSaveResponse; }
			set{ _msgBox.AllowSaveResponse = value; }
		}

		/// <summary>
		/// Sets the text to show to the user when saving his/her response
		/// </summary>
		public string SaveResponseText
		{
			set{_msgBox.SaveResponseText = value; }
		}

		/// <summary>
		/// Sets or Gets wether an alert sound is played while showing the message box.
		/// The sound played depends on the the Icon selected for the message box
		/// </summary>
		public bool PlayAlertSound
		{
			get{ return _msgBox.PlayAlertSound; }
			set{ _msgBox.PlayAlertSound = value; }
		}

        /// <summary>
        /// Sets or Gets the time in milliseconds for which the message box is displayed.
        /// </summary>
        public int Timeout
        {
            get{ return _msgBox.Timeout; }
            set{ _msgBox.Timeout = value; }
        }

        /// <summary>
        /// Controls the result that will be returned when the message box times out.
        /// </summary>
        public TimeoutResult TimeoutResult
        {
            get{ return _msgBox.TimeoutResult; }
            set{ _msgBox.TimeoutResult = value; }
        }

        /// <summary>
        /// Gets or sets the value of the save response checkbox
        /// </summary>
        public bool SaveResponseChecked
        {
            get { return _msgBox.SaveResponse; }
            set { _msgBox.SaveResponse = value; }
        }
		#endregion

		#region Methods
		/// <summary>
		/// Shows the message box
		/// </summary>
		/// <returns></returns>
		public string Show()
		{
			return Show(null);
		}

		/// <summary>
		/// Shows the messsage box with the specified owner
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public string Show(IWin32Window owner)
		{
			if(owner == null)
			{
				_msgBox.ShowDialog();
			}
			else
			{
				_msgBox.ShowDialog(owner);
			}
            Dispose();

			return _msgBox.Result;
		}

		/// <summary>
		/// Add a custom button to the message box
		/// </summary>
		/// <param name="button">The button to add</param>
		public void AddButton(MessageBoxExButton button)
		{
			if(button == null)
				throw new ArgumentNullException("button","A null button cannot be added");

			_msgBox.Buttons.Add(button);

			if(button.IsCancelButton)
			{
				_msgBox.CustomCancelButton = button;
			}
		}

		/// <summary>
		/// Add a custom button to the message box
		/// </summary>
		/// <param name="text">The text of the button</param>
		/// <param name="val">The return value in case this button is clicked</param>
		public void AddButton(string text, string val)
		{
			if(text == null)
				throw new ArgumentNullException("text","Text of a button cannot be null");

			if(val == null)
				throw new ArgumentNullException("val","Value of a button cannot be null");

			MessageBoxExButton button = new MessageBoxExButton();
			button.Text = text;
			button.Value = val;

			AddButton(button);
		}
        
		/// <summary>
		/// Add a standard button to the message box
		/// </summary>
		/// <param name="buttons">The standard button to add</param>
		public void AddButton(MessageBoxExButtons button)
		{
            string buttonText = MessageBoxExManager.GetLocalizedString(button.ToString());
            if(buttonText == null)
            {
                buttonText = button.ToString();
            }

            string buttonVal = button.ToString();

            MessageBoxExButton btn = new MessageBoxExButton();
            btn.Text = buttonText;
            btn.Value = buttonVal;

            if(button == MessageBoxExButtons.Cancel)
            {
                btn.IsCancelButton = true;
            }

			AddButton(btn);
		}

		/// <summary>
		/// Add standard buttons to the message box.
		/// </summary>
		/// <param name="buttons">The standard buttons to add</param>
		public void AddButtons(MessageBoxButtons buttons)
		{
			switch(buttons)
			{
				case MessageBoxButtons.OK:
					AddButton(MessageBoxExButtons.Ok);
					break;

				case MessageBoxButtons.AbortRetryIgnore:
					AddButton(MessageBoxExButtons.Abort);
					AddButton(MessageBoxExButtons.Retry);
					AddButton(MessageBoxExButtons.Ignore);
					break;

				case MessageBoxButtons.OKCancel:
					AddButton(MessageBoxExButtons.Ok);
					AddButton(MessageBoxExButtons.Cancel);
					break;

				case MessageBoxButtons.RetryCancel:
					AddButton(MessageBoxExButtons.Retry);
					AddButton(MessageBoxExButtons.Cancel);
					break;

				case MessageBoxButtons.YesNo:
					AddButton(MessageBoxExButtons.Yes);
					AddButton(MessageBoxExButtons.No);
					break;

				case MessageBoxButtons.YesNoCancel:
					AddButton(MessageBoxExButtons.Yes);
					AddButton(MessageBoxExButtons.No);
					AddButton(MessageBoxExButtons.Cancel);
					break;
			}
		}
		#endregion

		#region Ctor
		/// <summary>
		/// Ctor is internal because this can only be created by MBManager
		/// </summary>
		public MessageBoxEx()
		{
		}

		/// <summary>
		/// Called by the manager when it is disposed
		/// </summary>
		internal void Dispose()
		{
			if(_msgBox != null)
			{
				_msgBox.Dispose();
			}
		}
		#endregion
	}
}
