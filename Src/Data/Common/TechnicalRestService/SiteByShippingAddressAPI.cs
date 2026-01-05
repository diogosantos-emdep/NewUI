
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    [DataContract]
    public class SiteByShippingAddressAPI
    {
        #region Fields

        private UInt32 idSite;
        private string name;
        private UInt64 idShippingAddress;

        #endregion

        #region Property

        [DataMember]
        [IgnoreDataMember]
        public UInt32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                
            }

        }

        [DataMember]
        [IgnoreDataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public UInt64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
            }
        }

        #endregion

       



    }
}

