using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsStorageLocationLanes
        //: INotifyPropertyChanged
    {
        private string _locationInfo;

        public string LocationInfo
        {
            get { return _locationInfo; }
            set { _locationInfo = value;
            //NotifyPropertyChanged("_locationInfo");
             }
        }

        private string _cavities;

        public string Cavities
        {
            get { return _cavities; }
            set { _cavities = value; }
        }


        private string _cavitiesConnetor;

        public string CavitiesConnetor
        {
            get { return _cavitiesConnetor; }
            set { _cavitiesConnetor = value; }
        }

        //private int _qty;

        //public int Qty
        //{
        //    get { return _qty; }
        //    set { _qty = value; }
        //}

        private string _qty;

        public string Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        private ObservableCollection<clsGrid> _listclsGrid;

        public ObservableCollection<clsGrid> ListclsGrid
        {
            get { return _listclsGrid; }
            set { _listclsGrid = value; }
        }
        private ObservableCollection<Brush> _listColor;

        public ObservableCollection<Brush> ListColor
        {
            get { return _listColor; }
            set { _listColor = value; }
        }

        private Brush _colorbcode;

        public Brush Colorbcode
        {
            get { return _colorbcode; }
            set { _colorbcode = value; }
        }

        //private void NotifyPropertyChanged(string info)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
