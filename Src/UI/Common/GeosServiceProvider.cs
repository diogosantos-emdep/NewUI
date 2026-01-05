using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Emdep.Geos.UI.Common
{
    [Serializable]
    public class GeosServiceProvider
    {
        #region Fields
        String name;
        bool isSelected;
        PrivateNetwork privateNetwork;
        PublicNetwork publicNetwork;
        string serviceProviderUrl;
        #endregion

        #region Properties
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        [XmlElement]
        public PrivateNetwork PrivateNetwork
        {
            get { return privateNetwork; }
            set { privateNetwork = value; }
        }

        [XmlElement]
        public PublicNetwork PublicNetwork
        {
            get { return publicNetwork; }
            set { publicNetwork = value; }
        }

        [XmlAttribute]
        public string ServiceProviderUrl
        {
            get { return serviceProviderUrl; }
            set { serviceProviderUrl = value; }
        }
        #endregion
    }
}
