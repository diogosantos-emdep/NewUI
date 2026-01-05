using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    public class OffersSerilize
    {
        #region Fields
        List<OfferSerilize> offerList;
        #endregion

        #region Properties
        [XmlAttribute]
        public List<OfferSerilize> OfferList
        {
            get { return offerList; }
            set { offerList = value; }
        }

        #endregion
    }
}
