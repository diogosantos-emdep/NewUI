using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.HRM
{
    [Table("notifications")]
    [DataContract]
    public class Notification
    {
        #region Fields
        Int64 id;
        Int64? toUser;
        DateTime time;
        Int64? fromUser;
        Int64? fromEmployee;
        string image;
        string message;
        string action;
        string type;
        string status;
        string timeInElapsed;
        #endregion

        #region Properties

        [Key]
        [Column("id")]
        [DataMember]
        public Int64 Id
        {
            get { return id; }
            set { id = value; }
        }


        [Column("toUser")]
        [DataMember]
        public Int64? ToUser
        {
            get { return toUser; }
            set { toUser = value; }
        }

        [Column("time")]
        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        [Column("fromUser")]
        [DataMember]
        public Int64? FromUser
        {
            get { return fromUser; }
            set { fromUser = value; }
        }

        [Column("fromEmployee")]
        [DataMember]
        public Int64? FromEmployee
        {
            get { return fromEmployee; }
            set { fromEmployee = value; }
        }

        [Column("image")]
        [DataMember]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        [Column("message")]
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [Column("action")]
        [DataMember]
        public string Action
        {
            get { return action; }
            set { action = value; }
        }


        [Column("type")]
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [Column("status")]
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [NotMapped]
        [DataMember]
        public string TimeInElapsed
        {
            get { return timeInElapsed; }
            set { timeInElapsed = value; }
        }
        #endregion
    }
}
