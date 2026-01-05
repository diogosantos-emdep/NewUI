using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class clsTestProbes
    {
        private string _reference;

        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _function;

        public string Function
        {
            get { return _function; }
            set { _function = value; }
        }
        private string _feinmetallreference;

        public string Feinmetallreference
        {
            get { return _feinmetallreference; }
            set { _feinmetallreference = value; }
        }
        private string _ptrreference;

        public string Ptrreference
        {
            get { return _ptrreference; }
            set { _ptrreference = value; }
        }

    }
}
