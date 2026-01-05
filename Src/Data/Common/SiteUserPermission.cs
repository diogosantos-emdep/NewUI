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
    [Table("site_user_permission")]
    public class SiteUserPermission
    {
         #region Fields
         Int32 id;
         Int32? idCompany;
         Int32? idUser;
         bool isDeleted;
         #endregion

        #region Constructor
        public SiteUserPermission()
        {
         
        }
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

        [Column("IdCompany")]
        [ForeignKey("Company")]
        [DataMember]
        public Int32? IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        [Column("IdUser")]
        [ForeignKey("User")]
        [DataMember]
        public Int32? IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [DataMember]
        public virtual Company Company { get; set; }

        [DataMember]
        public virtual User User { get; set; }
        #endregion
    }
}
