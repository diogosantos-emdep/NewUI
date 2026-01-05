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
     [Table("permissions")]
     [DataContract(IsReference = true)]
   public class Permission
    {
        #region Fields
          int idPermission;
          string permissionName;
          Byte idGeosModule;
          bool isUserPermission;
          #endregion

        #region Constructor
        public Permission()
        {
            this.UserPermissions = new List<UserPermission>();
            this.PermissionTemplates = new List<PermissionTemplate>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdPermission")]
        [DataMember]
        public int IdPermission
        {
            get { return idPermission; }
            set { idPermission = value; }
        }

        [Column("PermissionName")]
        [DataMember]
        public string PermissionName
        {
            get { return permissionName; }
            set { permissionName = value; }
        }

        [Column("IdGeosModule")]
        [DataMember]
        public Byte IdGeosModule
        {
            get { return idGeosModule; }
            set { idGeosModule = value; }
        }


        [NotMapped]
        [DataMember]
        public bool IsUserPermission
        {
            get { return isUserPermission; }
            set { isUserPermission = value; }
        }

        [ForeignKey("IdGeosModule")]
        [DataMember]
        public virtual GeosModule GeosModule { get; set; }

        [DataMember]
        public virtual List<UserPermission> UserPermissions { get; set; }

        [DataMember]
        public virtual List<PermissionTemplate> PermissionTemplates { get; set; }
        #endregion
    }
}
