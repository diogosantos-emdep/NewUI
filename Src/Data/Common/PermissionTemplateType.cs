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
     [Table("permission_template_type")]
    [DataContract]
    public class PermissionTemplateType
    {
          #region Fields
          int idPermissionTemplateType;
          string permissionTemplateTypeName;
          #endregion

          #region Constructor
          public PermissionTemplateType()
        {
            this.PermissionTemplates = new List<PermissionTemplate>();
        }
        #endregion

        #region Properties
         [Key]
        [Column("IdPermissionTemplateType")]
        [DataMember]
          public int IdPermissionTemplateType
        {
            get { return idPermissionTemplateType; }
            set { idPermissionTemplateType = value; }
        }

        [Column("PermissionTemplateTypeName")]
        [DataMember]
        public string PermissionTemplateTypeName
        {
            get { return permissionTemplateTypeName; }
            set { permissionTemplateTypeName = value; }
        }

        [DataMember]
        public virtual List<PermissionTemplate> PermissionTemplates { get; set; }
        #endregion
    }
}
