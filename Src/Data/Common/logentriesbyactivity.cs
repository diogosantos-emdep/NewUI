using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("log_entries_by_activity")]
    [DataContract]
    public class LogEntriesByActivity : ModelBase, IDisposable
    {
        #region Fields

        Int64 idLogEntryByActivity;
        Int64 idActivity;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        byte? idLogEntryType;
        People people;
        bool isDeleted;
        bool isUpdated;
        bool isRtfText;
        string realText;

        #endregion

        #region Constructor

        public LogEntriesByActivity()
        {

        }

        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByActivity")]
        [DataMember]
        public Int64 IdLogEntryByActivity
        {
            get { return idLogEntryByActivity; }
            set
            {
                idLogEntryByActivity = value;
                OnPropertyChanged("IdLogEntryByActivity");
            }
        }

        [Column("IdActivity")]
        [DataMember]
        public Int64 IdActivity
        {
            get { return idActivity; }
            set
            {
                idActivity = value;
                OnPropertyChanged("IdActivity");
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
        public DateTime? Datetime
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
        public byte? IdLogEntryType
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
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
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

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
