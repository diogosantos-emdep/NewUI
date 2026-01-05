using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("training_changelog")]
    [DataContract]
    public class TrainingChangeLog : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idTrainingChangeLog;
        Int32 idEmployee;
        UInt64 idProfessionalTraining;
        DateTime changeLogDatetime;
        Int32 changeLogIdUser;
        string changeLogChange;
        User changeLogUser;

        #endregion

        #region Constructor
        public TrainingChangeLog()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdTrainingChangeLog")]
        [DataMember]
        public ulong IdTrainingChangeLog
        {
            get { return idTrainingChangeLog; }
            set
            {
                idTrainingChangeLog = value;
                OnPropertyChanged("IdTrainingChangeLog");
            }
        }
        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [Column("IdProfessionalTraining")]
        [DataMember]
        public UInt64 IdProfessionalTraining
        {
            get { return idProfessionalTraining; }
            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged("IdProfessionalTraining");
            }
        }

        [Column("ChangeLogDatetime")]
        [DataMember]
        public DateTime ChangeLogDatetime
        {
            get { return changeLogDatetime; }
            set
            {
                changeLogDatetime = value;
                OnPropertyChanged("ChangeLogDatetime");
            }
        }

        [Column("ChangeLogIdUser")]
        [DataMember]
        public int ChangeLogIdUser
        {
            get { return changeLogIdUser; }
            set
            {
                changeLogIdUser = value;
                OnPropertyChanged("ChangeLogIdUser");
            }
        }

        [Column("ChangeLogChange")]
        [DataMember]
        public string ChangeLogChange
        {
            get { return changeLogChange; }
            set
            {
                changeLogChange = value;
                OnPropertyChanged("ChangeLogChange");
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
