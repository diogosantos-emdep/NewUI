using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
  
   
    public class ActiveSite 
    {
        #region Fields
        Int32  idSite;
        string siteAlias;
        string siteServiceProvider;

        #endregion

       
        #region Properties
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
            }
        }

        [DataMember]
        public string SiteAlias
        {
            get
            {
                return siteAlias;
            }

            set
            {
                siteAlias = value;
            }
        }

        [DataMember]
        public string SiteServiceProvider
        {
            get
            {
                return siteServiceProvider;
            }

            set
            {
                siteServiceProvider = value;
            }
        }

        
        #endregion

      
    }
}
