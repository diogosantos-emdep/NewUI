using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class ErrorMessage
    {
        string _code;
        string _info;

        public string code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string info
        {
            get
            {
                return _info;
            }

            set
            {
                _info = value;
            }
        }
    }
}