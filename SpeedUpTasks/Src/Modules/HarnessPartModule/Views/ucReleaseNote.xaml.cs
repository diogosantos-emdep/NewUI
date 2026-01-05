using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucReleaseNote.xaml
    /// </summary>
    public partial class ucReleaseNote : UserControl
    {
        public ucReleaseNote()
        {
            InitializeComponent();


            List<ReleaseNote> lstReleasNote = new List<ReleaseNote>();


            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/ok.png"), Status = "New", Logs = "Added the possibility to export the locations to Excel.", BgColor = "Green", IsShow=true });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/ok.png"), Status = "New", Logs = "Added an user preferences menu.", BgColor = "Green", IsShow = true });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/ok.png"), Status = "Fixed", Logs = "Bug in connector images re-sorting when setting some picture as principal.", BgColor = "Yellow", IsShow = false });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/ok.png"), Status = "Fixed", Logs = "Solved a bug in when using the components filter with the condition NOT.", BgColor = "Yellow", IsShow = false });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/con2.jpg"), Status = "New", Logs = "Added the possibility to filter the connectors without visual aid.", BgColor = "Green", IsShow = true });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/con2.jpg"), Status = "Fixed", Logs = "Added the possibility to export the locations to Excel.", BgColor = "Yellow", IsShow = false });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/con2.jpg"), Status = "Fixed", Logs = "Added the possibility to export the locations to Excel.", BgColor = "Yellow", IsShow = false });
            lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/con2.jpg"), Status = "New", Logs = "Added more file extensions in the documents browser.", BgColor = "Green", IsShow = true });
         //  lstReleasNote.Add(new ReleaseNote() { ImageStatus = GetImage("/Image/con2.jpg"), Status = "New", Logs = "Added more file extensions in the documents browser.", BgColor = "Green", IsShow = true });


            dgvReleaseNote.ItemsSource = lstReleasNote;

           

        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
   
    }
    public  class ReleaseNote
    
    {
        private ImageSource imageStatus;
        private string status;
        private string logs;
        private string bgColor;
        private bool isShow;

        public bool IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        List<ReleaseNote> lstReleasNoteImg;

        public List<ReleaseNote> LstReleasNoteImg
        {
            get { return lstReleasNoteImg; }
            set { lstReleasNoteImg = value; }
        }

        public string BgColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }
        public ImageSource ImageStatus
        {
            get { return imageStatus; }
            set { imageStatus = value; }
        }
        
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Logs
        {
            get { return logs; }
            set { logs = value; }
        }

       
    }          
}

