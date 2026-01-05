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
    [Table("user_managers")]
    [DataContract]
    public class UserManagerDtl
    {
        #region Fields
        Int32 idUserManager;
        Int32 idManager;
        Int32 idUser;
        User user;
        #endregion

        #region Constructor
        public UserManagerDtl()
        {
        
        }
        #endregion


        #region Properties
        [Key]
        [Column("IdUserManager")]
        [DataMember]
        public Int32 IdUserManager
        {
            get { return idUserManager; }
            set { idUserManager = value; }
        }

        [Column("IdManager")]
        [DataMember]
        public Int32 IdManager
        {
            get { return idManager; }
            set { idManager = value; }
        }

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [NotMapped]
        [DataMember]
        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }
      
        #endregion
    }
}
