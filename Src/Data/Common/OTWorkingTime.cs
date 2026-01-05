using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("ot_working_times")]
    [DataContract]
    public class OTWorkingTime : ModelBase, IDisposable
    {

        #region Fields

        Int64 idOT;
        Int64 idOTItem;
        byte idStage;
        Int32 idOperator;
        DateTime? startTime;
        DateTime? endTime;
        string startTimeInHoursAndMinutes;
        string endTimeInHoursAndMinutes;
        UserShortDetail userShortDetail;
        TimeSpan totalTime;
        Int64 idOTWorkingTime;
        Stage stage;
        bool isTimerStarted;
        string totalTimeInString;
        OTWorkingTime addedOTWorkingTime;
        Company company;
        string hours;
        TimeSpan workLogStartTime;
        TimeSpan workLogEndTime;
        TransactionOperations transactionOperation;
        #endregion

        #region Constructor
        public OTWorkingTime()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOTWorkingTime")]
        [DataMember]
        public Int64 IdOTWorkingTime
        {
            get { return idOTWorkingTime; }
            set
            {
                idOTWorkingTime = value;
                OnPropertyChanged("idOTWorkingTime");
            }
        }

       
        [Column("IdOT")]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
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

        [Column("IdOperator")]
        [DataMember]
        public Int32 IdOperator
        {
            get { return idOperator; }
            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }

        [Column("StartTime")]
        [DataMember]
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [Column("EndTime")]
        [DataMember]
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public UserShortDetail UserShortDetail
        {
            get { return userShortDetail; }
            set
            {
                userShortDetail = value;
                OnPropertyChanged("UserShortDetail");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }


        [NotMapped]
        [DataMember]
        public String StartTimeInHoursAndMinutes
        {
            get { return startTimeInHoursAndMinutes; }
            set
            {
               startTimeInHoursAndMinutes = value;
                OnPropertyChanged("StartTimeInHoursAndMinutes");
            }
        }

        [NotMapped]
        [DataMember]
        public String EndTimeInHoursAndMinutes
        {
            get { return endTimeInHoursAndMinutes; }
            set
            {
                endTimeInHoursAndMinutes = value;
                OnPropertyChanged("EndTimeInHoursAndMinutes");
            }
        }

        [NotMapped]
        [DataMember]
        public Stage Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                OnPropertyChanged("Stage");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsTimerStarted
        {
            get { return isTimerStarted; }
            set
            {
                isTimerStarted = value;
                OnPropertyChanged("IsTimerStarted");
            }
        }
        public enum Process
        {
            Start = 0, Stop = 1
        }

        public Process WorkLogProcess
        {
            get;
            set;
        }

        [NotMapped]
        [DataMember]
        public string TotalTimeInString
        {
            get { return totalTimeInString; }
            set
            {
                totalTimeInString = value;
                OnPropertyChanged("TotalTimeInString");
            }
        }

        [NotMapped]
        [DataMember]
        public OTWorkingTime AddedOTWorkingTime
        {
            get { return addedOTWorkingTime; }
            set
            {
                addedOTWorkingTime = value;
                OnPropertyChanged("AddedOTWorkingTime");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }
        [NotMapped]
        [DataMember]
        public string Hours
        {
            get
            {
                return hours;
            }

            set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }

        [Column("IdOTItem")]
        [DataMember]
        public Int64 IdOTItem
        {
            get { return idOTItem; }
            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        public TimeSpan WorkLogStartTime
        {
            get { return workLogStartTime; }
            set
            {
                workLogStartTime = value;
                OnPropertyChanged("WorkLogStartTime");
            }
        }
        public TimeSpan WorkLogEndTime
        {
            get { return workLogEndTime; }
            set
            {
                workLogEndTime = value;
                OnPropertyChanged("WorkLogEndTime");
            }
        }
        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return transactionOperation;
            }
            set
            {
                transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
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
