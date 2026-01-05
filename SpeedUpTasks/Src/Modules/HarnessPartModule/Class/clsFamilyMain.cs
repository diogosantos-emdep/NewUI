using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    class clsFamilyMain
    {
        private string _connectorFamilyName;

        public string ConnectorFamilyName
        {
            get { return _connectorFamilyName; }
            set { _connectorFamilyName = value; }
        }




        private ObservableCollection<clsFamilyInfo> _listFamilyMain;

        public ObservableCollection<clsFamilyInfo> ListFamilyMain
        {
            get { return _listFamilyMain; }
            set { _listFamilyMain = value; }
        }
   
    }
}
