using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common
{
    public class OfferParams
    {

        [DataMember]
        public string offerCode { get; set; }
        [DataMember]
        public string offerYear { get; set; }
      
    }

}
