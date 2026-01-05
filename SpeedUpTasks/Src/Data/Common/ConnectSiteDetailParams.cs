using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
  public  class ConnectSiteDetailParams
    {
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public int idSite { get; set; }
      
       

    }
}
