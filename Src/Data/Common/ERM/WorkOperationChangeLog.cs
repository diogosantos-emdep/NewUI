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
    [Table("Work_Operation_Change_Log")]
    [DataContract]
    public class WorkOperationChangeLog : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idLogEntryByWO;
        UInt64 idWorkOperation;
        DateTime datetime;
        Int32 idUser;
        string comments;
        User changeLogUser;

        #endregion

        #region Constructor
        public WorkOperationChangeLog()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByWO")]
        [DataMember]
        public ulong IdLogEntryByWO
        {
            get { return idLogEntryByWO; }
            set
            {
                idLogEntryByWO = value;
                OnPropertyChanged("IdLogEntryByWO");
            }
        }

        [Column("IdWorkOperation")]
        [DataMember]
        public ulong IdWorkOperation
        {
            get { return idWorkOperation; }
            set
            {
                idWorkOperation = value;
                OnPropertyChanged("IdWorkOperation");
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
