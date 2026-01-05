using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class clsHarnessPartCategory
    {
        private int _IdFamily;

        public int IdFamily
        {
            get { return _IdFamily; }
            set { _IdFamily = value; }
        }
        private int _ParentID;

        public int ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }
        private string _FamilyName;

        public string FamilyName
        {
            get { return _FamilyName; }
            set { _FamilyName = value; }
        }
        private int _idConnectorSubfamily;

        public int IdConnectorSubfamily
        {
            get { return _idConnectorSubfamily; }
            set { _idConnectorSubfamily = value; }
        }
        private string _NameSubfamily;

        public string NameSubfamily
        {
            get { return _NameSubfamily; }
            set { _NameSubfamily = value; }
        } 
    }
}
