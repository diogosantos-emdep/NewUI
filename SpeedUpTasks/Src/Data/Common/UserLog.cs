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
     [Table("user_logs")]
     [DataContract]
    public class UserLog
    {
        #region Fields
        Int32 idUserLog;
         string log;
         DateTime? logIn;
         Byte? idLogEntryType;
         Int32? idUser;
        #endregion

         #region Properties
         [Key]
        [Column("IdUserLog")]
        [DataMember]
         public Int32 IdUserLog
        {
            get { return idUserLog; }
            set { idUserLog = value; }
        }

        [Column("Log")]
        [DataMember]
        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        [Column("LogIn")]
        [DataMember]
        public DateTime? LogIn
        {
            get { return logIn; }
            set { logIn = value; }
        }

        
        [DataMember]
        public Byte? IdLogEntryType
        {
            get { return idLogEntryType; }
            set { idLogEntryType = value; }
        }

        
        [DataMember]
        public Int32? IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [ForeignKey("IdUser")]
        [DataMember]
        public virtual User Users { get; set; }

        [ForeignKey("IdLogEntryType")]
        [DataMember]
        public virtual LogEntryType LogEntryTypes { get; set; }
         #endregion
    }
}
