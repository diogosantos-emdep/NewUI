using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.Glpi
{
    [Table("geos_glpi_itilcategories")]
    [DataContract]
    public class GeosGlpiItilCategory
    {
        #region Fields
        Int32 id;
        Int32? idModule;
        Int32? idItilCategory;
        #endregion

        #region Properties
        [Key]
        [Column("Id")]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("IdModule")]
        [DataMember]
        public Int32? IdModule
        {
            get { return idModule; }
            set { idModule = value; }
        }

        [Column("IdItilCategory")]
        [DataMember]
        public Int32? IdItilCategory
        {
            get { return idItilCategory; }
            set { idItilCategory = value; }
        }
        #endregion
    }
}
