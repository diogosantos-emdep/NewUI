using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [Table("log_entries_by_Stages")]
    [DataContract]
    public class LogentriesbyStages : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idLogEntryByStages;
        UInt32 idStage;
        DateTime datetime;
        Int32 idUser;
        string comments;
        User changeLogUser;

        #endregion

        #region Constructor
        public LogentriesbyStages()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByStages")]
        [DataMember]
        public ulong IdLogEntryByStages
        {
            get { return idLogEntryByStages; }
            set
            {
                idLogEntryByStages = value;
                OnPropertyChanged("IdLogEntryByStages");
            }
        }

        [Column("IdStage")]
        [DataMember]
        public UInt32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
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

        [Column("IdUser")]
        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
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

        [NotMapped]
        [DataMember]
        public User ChangeLogUser
        {
            get { return changeLogUser; }
            set
            {
                changeLogUser = value;
                OnPropertyChanged("ChangeLogUser");
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
