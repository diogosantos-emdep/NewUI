using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    class clsOtherReference
    {
        private string _reference;

        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        private string _empresa;

        public string Empresa
        {
            get { return _empresa; }
            set { _empresa = value; }
        }
    }
}
