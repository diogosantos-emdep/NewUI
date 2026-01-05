using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class LookupValue
    {
        int idLookupValue;

        public int IdLookupValue
        {
            get { return idLookupValue; }
            set { idLookupValue = value; }
        }

        string value;

        public string Value1
        {
            get { return this.value; }
            set { this.value = value; }
        }


        int idLookupKey;

        public int IdLookupKey
        {
            get { return idLookupKey; }
            set { idLookupKey = value; }
        }

    }
}
