using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
     
   public class clsharnessPartAccessoryTypes
    {
        private int _idType;

        public int IdType
        {
            get { return _idType; }
            set { _idType = value; }
        }
        private string _typeName;

        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

       
    }
}