using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
   
    [DataContract]
    public class UserSiteGeosServiceProvider
    {
        #region Fields
        Int32 idSite;
        string shortName;
        string serviceProviderUrl;
       
        #endregion

        #region Properties
              
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [DataMember]
        public string ShortName
        {
            get { return shortName; }
            set { shortName = value; }
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