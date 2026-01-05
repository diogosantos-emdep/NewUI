using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    class clsFamilyInfo
    {
        private string listfamilyName;

        public string ListfamilyName
        {
            get { return listfamilyName; }
            set { listfamilyName = value; }
        }

        
        private ObservableCollection<clsFamilyGrid> _listFamilyInfo;

        public ObservableCollection<clsFamilyGrid> ListFamilyInfo
        {
            get { return _listFamilyInfo; }
            set { _listFamilyInfo = value; }
        }
    }
}
