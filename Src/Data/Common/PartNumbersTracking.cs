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
    [Table("partnumberstracking")]
    [DataContract]
    public class PartNumbersTracking : ModelBase, IDisposable
    {
        #region Fields

        Int64 idPartNumberTracking;
        Int64 idPartNumber;
        byte idStage;
        DateTime? startDate;
        DateTime? endDate;
        Int32? idOperator;
        Int64 currentTime;
        bool rework;
        Int32? idCause;
        bool paused;
        string remarks;
        People stageOperator;

        #endregion

        #region Properties

        [Key]
        [Column("IdPartNumberTracking")]
        [DataMember]
        public Int64 IdPartNumberTracking
        {
            get { return idPartNumberTracking; }
            set
            {
                idPartNumberTracking = value;
                OnPropertyChanged("IdPartNumberTracking");
            }
        }

        [Column("IdPartNumber")]
        [DataMember]
        public Int64 IdPartNumber
        {
            get { return idPartNumber; }
            set
            {
                idPartNumber = value;
                OnPropertyChanged("IdPartNumber");
            }
        }

        [Column("IdStage")]
        [DataMember]
        public byte IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("IdOperator")]
        [DataMember]
        public Int32? IdOperator
        {
            get { return idOperator; }
            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }

        [Column("CurrentTime")]
        [DataMember]
        public Int64 CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        [Column("Rework")]
        [DataMember]
        public bool Rework
        {
            get { return rework; }
            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        [Column("IdCause")]
        [DataMember]
        public Int32? IdCause
        {
            get { return idCause; }
            set
            {
                idCause = value;
                OnPropertyChanged("IdCause");
            }
        }

        [Column("Paused")]
        [DataMember]
        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                OnPropertyChanged("Paused");
            }
        }

        [Column("Remarks")]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [NotMapped]
        [DataMember]
        public People StageOperator
        {
            get { return stageOperator; }
            set
            {
                stageOperator = value;
                OnPropertyChanged("StageOperator");
            }
        }

        #endregion

        #region Constructor
        public PartNumbersTracking()
        {
        }
        #endregion

        #region Methods

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
