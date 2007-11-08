using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MediaInfoWrapper;
using System.IO;
namespace MediaInfoWrapperTest
{
    /// <summary>
    /// Displays an example form with a file browser showing infos about mediafiles
    /// using the mediawrapper.dll helper class and mediainfo.dll
    /// </summary>
   public partial class MediaInfoForm : Form
    {
       string FileName = "";
       MediaInfo M;
       /// <summary>
       /// Constructor
       /// </summary>
       public MediaInfoForm()
        {
            InitializeComponent();
            this.Text = "Media Information (Thanks to MediaInfo.dll)";

        }
       /// <summary>
       /// Constructor with the form initialized to the specified file
       /// </summary>
       /// <param name="filepath"></param>
       public MediaInfoForm(string filepath)
       {

           InitializeComponent();
           this.Text = "Media Information (Thanks to MediaInfo.dll)";
           FileName = filepath;
       }
        

        

        private void Form1_Load(object sender, EventArgs e)
        {


        }
       

       private void toolStripButton1_Click(object sender, EventArgs e)
       {
           OpenFileDialog O=new OpenFileDialog();
           O.RestoreDirectory = true;
           O.ShowDialog();
           if (O.FileName == "") return;
           
               FileName = O.FileName;
               toolStripTextBox1.Text = FileName;
               M = new MediaInfo(FileName);
               T.Text = M.InfoStandard;
           

       }

       private void toolStripButton2_Click(object sender, EventArgs e)
       {
           if (File.Exists(FileName)) T.Text = M.InfoStandard ;
       }

       private void toolStripButton3_Click(object sender, EventArgs e)
       {

           if (File.Exists(FileName)) T.Text = InfoCustom(FileName);
       }

       private void toolStripButton4_Click(object sender, EventArgs e)
       {
           T.Text= MediaInfo.Capacities();
           
       }

       private void toolStripButton5_Click(object sender, EventArgs e)
       {   
           T.Text = MediaInfo.KnownCodecs();
       }

       private void toolStripButton6_Click(object sender, EventArgs e)
       {
           if (File.Exists(FileName)) T.Text = M.InfoComplete ;
       }

       private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
       {
           if (e.KeyCode == Keys.Enter)
           {
               if (System.IO.File.Exists(toolStripTextBox1.Text )) FileName=toolStripTextBox1.Text;
                   M = new MediaInfo(FileName);
                   T.Text = M.InfoStandard;               

               
           }
       }


       // Can be replaced by InfoCustom property of mediainfo
       string InfoCustom(string filepath)
       {

           M = new MediaInfo(filepath);
           string s = "";

           s += "General" + Environment.NewLine;
           s += ListEveryAvailablePropery<GeneralTrack>(M.General);
           s += Environment.NewLine;
           s += "Video" + Environment.NewLine;
           s += ListEveryAvailablePropery<VideoTrack>(M.Video);
           s += Environment.NewLine;
           s += "Audio" + Environment.NewLine;
           s += ListEveryAvailablePropery<AudioTrack>(M.Audio);
           s += Environment.NewLine;
           s += "Text" + Environment.NewLine;
           s += ListEveryAvailablePropery<TextTrack>(M.Text);
           s += Environment.NewLine;
           s += "Chapters" + Environment.NewLine;
           s += ListEveryAvailablePropery<ChaptersTrack>(M.Chapters);

           return s;

       }

       private string ListEveryAvailablePropery<T1>(List<T1> L)
       {
           string s = "";
           foreach (T1 track in L)
           {
               foreach (PropertyInfo p in track.GetType().GetProperties())
               {
                   s += (p.GetValue(track, null).ToString() == "") ? p.Name + " : Not available" + Environment.NewLine : p.Name + " : " + p.GetValue(track, null) + Environment.NewLine;
               }
           }
           return s;
       }
 




    }
}