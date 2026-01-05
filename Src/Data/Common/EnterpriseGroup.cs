using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("enterprises_group")]
    [DataContract(IsReference = true)]
   public class EnterpriseGroup
    {
        #region Fields
        int idEnterpriseGroup;
        string name;
        #endregion

        #region Constructor
        public EnterpriseGroup()
        {
            this.Companies = new List<Company>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdEnterpriseGroup")]
        [DataMember]
        public int IdEnterpriseGroup
        {
            get { return idEnterpriseGroup; }
            set { idEnterpriseGroup = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public virtual List<Company> Companies { get; set; }

        #endregion
   }
}
