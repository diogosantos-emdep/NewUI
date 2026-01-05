using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsStorageLocations:INotifyPropertyChanged
    {
        private string _locationName;

        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value;
           
            }
        }

        private ObservableCollection<clsStorageLocationLanes> _listTorageLocationLanes;

        public ObservableCollection<clsStorageLocationLanes> ListTorageLocationLanes
        {
            get { return _listTorageLocationLanes; }
            set
            {
                _listTorageLocationLanes = value;
                //NotifyPropertyChanged("_listTorageLocationLanes");


            }
        }

        //private bool isEdit;

        //public bool IsEdit
        //{
        //    get { return isEdit; }
        //    set
        //    {
        //        isEdit = value;
        //        NotifyPropertyChanged("isEdit");
        //    }
        //}
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
