using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Emdep.Geos.UI.Common
{
    [Serializable]
   public class PrivateNetwork
    {
        #region Fields
        String ip;
        String port;
        String servicePath;
        #endregion

        #region Properties
        [XmlAttribute]
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        [XmlAttribute]
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        [XmlAttribute]
        public string ServicePath
        {
            get { return servicePath; }
            set { servicePath = value; }
        }
       
        #endregion
    }
}
