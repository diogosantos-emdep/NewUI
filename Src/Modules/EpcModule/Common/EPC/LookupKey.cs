using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class LookupKey
    {
        int idLookupKey;

        public int IdLookupKey
        {
            get { return idLookupKey; }
            set { idLookupKey = value; }
        }
        string lookupName;

        public string LookupName
        {
            get { return lookupName; }
            set { lookupName = value; }
        }
    }
}
