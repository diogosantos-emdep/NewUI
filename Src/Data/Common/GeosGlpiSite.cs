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
    [Table("geos_glpi_sites")]
    [DataContract]
    public class GeosGlpiSite
    {
        #region Fields
        Int32 id;
        Int32 geosSiteId;
        Int32 glpiSiteId;
        #endregion

        #region Properties
        [Key]
        [Column("id")]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }


        [Column("geos_siteid")]
        [DataMember]
        public Int32 GeosSiteId
        {
            get { return geosSiteId; }
            set { geosSiteId = value; }
        }

        [Column("glpi_siteid")]
        [DataMember]
        public Int32 GlpiSiteId
        {
            get { return glpiSiteId; }
            set { glpiSiteId = value; }
        }

        #endregion
    }
}
