using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.PCM
{
    public class FourRegionsWithCustomerCount : INotifyPropertyChanged
    {
        private int count;

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            }
        }


        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionName"));
            }
        }

        private string regionName;



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
