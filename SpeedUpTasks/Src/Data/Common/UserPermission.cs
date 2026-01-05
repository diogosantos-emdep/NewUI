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

    [Table("user_permissions")]
    [DataContract]
    public class UserPermission
    {

        #region Fields
        int id;
        int idUser;
        int idPermission;
        bool isDeleted;
        #endregion

        #region Constructor
        public UserPermission()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("Id")]
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("IdUser")]
        [ForeignKey("User")]
        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [Column("IdPermission")]
        [ForeignKey("Permission")]
        [DataMember]
        public int IdPermission
        {
            get { return idPermission; }
            set { idPermission = value; }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        public virtual Permission Permission { get; set; }
        #endregion

    }
}
