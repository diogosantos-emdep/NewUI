using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsHarnessInfo : INotifyPropertyChanged
    {
        private ImageSource _visualAid;

        public ImageSource VisualAid
        {
            get { return _visualAid; }
            set { _visualAid = value; }
        }

        private string _harnessname;

        public string Harnessname
        {
            get { return _harnessname; }
            set { _harnessname = value; }
        }
        private string _harnessdescription;

        public string Harnessdescription
        {
            get { return _harnessdescription; }
            set { _harnessdescription = value; }
        }

        private List<ImageSource> _AllvisualAid;

        public List<ImageSource> AllvisualAid
        {
            get { return _AllvisualAid; }
            set { _AllvisualAid = value; }
        }

        public clsHarnessInfo()
        {
        }

        private bool isEdit;

        public bool IsEdit
        {
            get { return isEdit; }
            set { isEdit = value;
            NotifyPropertyChanged("isEdit");
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
