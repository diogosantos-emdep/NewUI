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
    [Table("user_login_entries")]
    [DataContract]
    public class UserLoginEntry
    {
        #region Fields

        Int32 idUserLoginEntry;
        int idUser;
        DateTime loginTime;
        DateTime? logoutTime;
        string ipAddress;
        Int32 idCurrentGeosVersion;

        #endregion

        #region Properties

        [Key]
        [Column("IdUserLoginEntry")]
        [DataMember]
        public Int32 IdUserLoginEntry
        {
            get { return idUserLoginEntry; }
            set { idUserLoginEntry = value; }
        }

        [Column("IdUser")]
        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [Column("LoginTime")]
        [DataMember]
        public DateTime LoginTime
        {
            get { return loginTime; }
            set { loginTime = value; }
        }

        [Column("LogoutTime")]
        [DataMember]
        public DateTime? LogoutTime
        {
            get { return logoutTime; }
            set { logoutTime = value; }
        }

        [Column("IpAddress")]
        [DataMember]
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        [Column("IdCurrentGeosVersion")]
        [DataMember]
        public Int32 IdCurrentGeosVersion
        {
            get { return idCurrentGeosVersion; }
            set { idCurrentGeosVersion = value; }
        }

        //[ForeignKey("idUser")]
        //[DataMember]
        //public virtual User Users { get; set; }

        #endregion

    }
}
