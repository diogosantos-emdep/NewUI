using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_OT_Working_Times : ModelBase, IDisposable
    {
        #region Field
        private DateTime? startDate;
        private DateTime? endDate;
        private Int32 idOperator;
        private Int32? oTWorking_IdStage;
        TimeSpan startTime;
        TimeSpan endTime;
        private string calenderWeek;
        private string stageCode;
        #endregion
        #region Property
        [NotMapped]
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
        [NotMapped]
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
      
        
        
        [DataMember]
        public Int32 IdOperator
        {
            get
            {
                return idOperator;
            }

            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }
       
        [DataMember]
        public Int32? OTWorking_IdStage
        {
            get
            {
                return oTWorking_IdStage;
            }

            set
            {
                oTWorking_IdStage = value;
                OnPropertyChanged("OTWorking_IdStage");
            }
        }
        [DataMember]
        public string StageCode
        {
            get
            {
                return stageCode;
            }

            set
            {
                stageCode = value;
                OnPropertyChanged("StageCode");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan StartTime
        {
            get { return startTime; }

            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan EndTime
        {
            get { return endTime; }

            set
            {
               endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [DataMember]
        public string CalenderWeek
        {
            get { return calenderWeek; }
            set
            {
                calenderWeek = value;
                OnPropertyChanged("CalenderWeek");
            }
        }
        #endregion
        #region Constructor
        public ERM_OT_Working_Times()
        {

        }
        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
