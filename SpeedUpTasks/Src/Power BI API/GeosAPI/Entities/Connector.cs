using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
   public class Connector
    {
        private string _Cavities = string.Empty;
       
        public string Cavities
        {
            get { return _Cavities; }
            set { _Cavities = value; }
        }

        private string _ConnectorFamily = string.Empty;
       
        public string ConnectorFamily
        {
            get { return _ConnectorFamily; }
            set { _ConnectorFamily = value; }
        }

        private string _ConnectorGender = string.Empty;
       
        public string ConnectorGender
        {
            get { return _ConnectorGender; }
            set { _ConnectorGender = value; }
        }
    }
}
