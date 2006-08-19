using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MeGUI
{
	/// <summary>
	/// Summary description for LogWindow.
	/// </summary>
	public class LogWindow : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox log;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LogWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.log = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// log
			// 
			this.log.Location = new System.Drawing.Point(0, 0);
			this.log.Multiline = true;
			this.log.Name = "log";
			this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.log.Size = new System.Drawing.Size(432, 400);
			this.log.TabIndex = 0;
			this.log.Text = "";
			// 
			// LogWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 398);
			this.Controls.Add(this.log);
			this.Name = "LogWindow";
			this.ResumeLayout(false);

		}
		#endregion

		public void setText(string text)
		{
			log.Text = text;
		}
		public void setTitle(string title)
		{
			this.Name = title;
			this.Text = title;
		}
	}
}
