using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("log_entries_by_contact")]
    [DataContract]
    public class LogEntriesByContact : ModelBase, IDisposable
    {
        #region Fields

        Int64 idLogEntryByContact;
        Int32 idContact;
        Int32 idUser;
        People user;

        DateTime datetime;
        string comments;
        byte idLogEntryType;

        bool isDeleted;
        bool isUpdated;
        bool isRtfText;
        string realText;

        #endregion

        #region Constructor

        public LogEntriesByContact()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByContact")]
        [DataMember]
        public long IdLogEntryByContact
        {
            get { return idLogEntryByContact; }
            set
            {
                idLogEntryByContact = value;
                OnPropertyChanged("IdLogEntryByContact");
            }
        }

        [Column("IdContact")]
        [DataMember]
        public int IdContact
        {
            get { return idContact; }
            set
            {
                idContact = value;
                OnPropertyChanged("IdContact");
            }
        }

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [Column("Datetime")]
        [DataMember]
        public DateTime Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("IdLogEntryType")]
        [DataMember]
        public byte IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged("IsUpdated");
            }
        }

        [Column("IsRtfText")]
        [DataMember]
        public bool IsRtfText
        {
            get { return isRtfText; }
            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [NotMapped]
        [DataMember]
        public string RealText
        {
            get { return realText; }
            set
            {
                realText = value;
                OnPropertyChanged("RealText");
            }
        }

        [NotMapped]
        [DataMember]
        public People User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("RealText");
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            LogEntriesByContact obj = (LogEntriesByContact)this.MemberwiseClone();
            if (this.User != null)
                obj.User = (People)this.User.Clone();
            return obj;
        }

        #endregion
    }
}
