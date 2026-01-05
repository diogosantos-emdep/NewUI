using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common
{
    [Table("notifications")]
    [DataContract]
    public class Notification
    {

        #region Fields

        Int64 id;
        DateTime time;
        Int32? fromUser;
        Int32? toUser;
        string title;
        string message;
        int idModule;
        string status;
        string timeInElapsed;
        ImageSource fromUserImage;
        ImageSource timeInClockImage;
        byte? isNew;
        User fromUserName;
        MailTemplateFormat mailTemplateFormat;

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
        public Int32? ToUser
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

        [Column("title")]
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [ForeignKey("FromUserName")]
        [Column("fromUser")]
        [DataMember]
        public Int32? FromUser
        {
            get { return fromUser; }
            set { fromUser = value; }
        }


        [Column("message")]
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [Column("idModule")]
        [DataMember]
        public int IdModule
        {
            get { return idModule; }
            set { idModule = value; }
        }

        [Column("status")]
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [Column("isNew")]
        [DataMember]
        public byte? IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        [NotMapped]
        [DataMember]
        public string TimeInElapsed
        {
            get { return timeInElapsed; }
            set { timeInElapsed = value; }
        }

        [NotMapped]
        [DataMember]
        public ImageSource FromUserImage
        {
            get { return fromUserImage; }
            set { fromUserImage = value; }
        }

        [NotMapped]
        [DataMember]
        public ImageSource TimeInClockImage
        {
            get { return timeInClockImage; }
            set { timeInClockImage = value; }
        }

        [DataMember]
        public virtual User FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        [NotMapped]
        [DataMember]
        public MailTemplateFormat MailTemplateFormat
        {
            get { return mailTemplateFormat; }
            set { mailTemplateFormat = value; }
        }

        #endregion
    }
}
