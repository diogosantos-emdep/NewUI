using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   
    [DataContract]
    public class GeosProviderDetail
    {
        #region Fields

        Int32 idCompany;
        string alias;
        Int32 idGeosProvider;
        string serviceServerPublicPort;
        string serviceProviderUrl;
        #endregion

        #region Properties
     
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set { idCompany = value;  }
        }


        [DataMember]
        public string Alias
        {
            get { return alias; }
            set { alias = value;  }
        }

        [DataMember]
        public Int32 IdGeosProvider
        {
            get { return idGeosProvider; }
            set { idGeosProvider = value; }
        }

        [DataMember]
        public string ServiceServerPublicPort
        {
            get { return serviceServerPublicPort; }
            set { serviceServerPublicPort = value;  }
        }

        [DataMember]
        public string ServiceProviderUrl
        {
            get { return serviceProviderUrl; }
            set { serviceProviderUrl = value; }
        }

        #endregion





    }
}

