using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Emdep.Geos.UI.Common
{
    [Serializable]
    [XmlRoot("GeosServiceProviders")]
    public class GeosServiceProviders
    {
        #region Fields
        List<GeosServiceProvider> geosServiceProvider;
        #endregion

        #region Properties
        [XmlElement]
        public List<GeosServiceProvider> GeosServiceProvider
        {
            get { return geosServiceProvider; }
            set { geosServiceProvider = value; }
        }


        #endregion
    }
}
