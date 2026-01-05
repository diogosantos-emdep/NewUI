using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.UI.CustomControls.ViewModels
{

    internal class PickingInfoMessageBoxViewModel : INotifyPropertyChanged
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagePath"));
            }
        }

        private List<PickingComment> commentList;

        public List<PickingComment> CommentList
        {
            get { return commentList; }
            set
            {
                commentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentList"));
            }
        }
        public PickingInfoMessageBoxViewModel()
        {
            //DownloadLogs = "test";


        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public class PickingComment : INotifyPropertyChanged
    {
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string oTCode;

        public string OTCode
        {
            get { return oTCode; }
            set
            {
                oTCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTCode"));
            }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        private string reference;

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }
    }

}
