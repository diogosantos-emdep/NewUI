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
    [Table("permission_template")]
    [DataContract]
    public class PermissionTemplate
    {
          #region Fields
          int idPermissionTemplate;
          int idPermissionTemplateType;
          int idPermission;
          #endregion

          #region Constructor
          public PermissionTemplate()
        {
          
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdPermissionTemplate")]
        [DataMember]
          public int IdPermissionTemplate
        {
            get { return idPermissionTemplate; }
            set { idPermissionTemplate = value; }
        }

        [Column("IdPermissionTemplateType")]
        [ForeignKey("PermissionTemplateType")]
        [DataMember]
        public int IdPermissionTemplateType
        {
            get { return idPermissionTemplateType; }
            set { idPermissionTemplateType = value; }
        }


        [Column("IdPermission")]
        [ForeignKey("Permission")]
        [DataMember]
        public int IdPermission
        {
            get { return idPermission; }
            set { idPermission = value; }
        }

        [DataMember]
        public virtual Permission Permission { get; set; }

        [DataMember]
        public virtual PermissionTemplateType PermissionTemplateType { get; set; }
        #endregion
    }
}
