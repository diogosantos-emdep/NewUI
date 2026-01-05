using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using System;

namespace Emdep.Geos.Data.Common.ERM
{
    public class Counterpartstracking : ModelBase, IDisposable
    {
        #region Field
        private Int32 idStage;
        private Int32 idCounterpart;
       
        private Int64 currentTime;
        private bool? rework;
        private DateTime? startDate;
        private DateTime? endDate;
        private Int32 reworkNumber;
        private Int32 idWorkStation;
        private string remarks;
        private string fullName;
        private string stageCode;
        private Int32 idCounterpartTracking;
        private string failCode;//[GEOS2-5127][gulab lakade][20 12 2023]
        private string name;//[GEOS2-5127][gulab lakade][20 12 2023]
        private string availableDayName;//[GEOS2-6058][gulab lakade][02 10 2024]
        private string endDateWeek;//[GEOS2-6058][gulab lakade][02 10 2024]
        private Int32 idOperator;//[GEOS2-9220][gulab lakade][12 08 2025]
        private string calenderWeek;//[GEOS2-9220][gulab lakade][12 08 2025]
        private Int32 productionActivityTimeType;
        private Int32 idSite;//[GEOS2-7091][rani dhamankar] [19-09-2025]
        TimeSpan counterPartStartTime;
        TimeSpan counterPartEndTime;
        DateTime? counterpartStartDate;
        DateTime? counterpartEndDate;
        TimeSpan shiftStartTime; //[GEOS2-9560][gulab lakade][25 11 2025]
        TimeSpan shiftEndTime; //[GEOS2-9560][gulab lakade][25 11 2025]
        #endregion
        #region Property
        [DataMember]
        public Int32 IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }
        [DataMember]
        public Int32 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }
        [DataMember]
        public Int64 CurrentTime
        {
            get
            {
                return currentTime;
            }

            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }
        [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        [DataMember]
        public bool? Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [DataMember]
        public Int32 ReworkNumber
        {
            get
            {
                return reworkNumber;
            }

            set
            {
                reworkNumber = value;
                OnPropertyChanged("ReworkNumber");
            }
        }


        [DataMember]
        public Int32 IdWorkStation
        {
            get
            {
                return idWorkStation;
            }

            set
            {
                idWorkStation = value;
                OnPropertyChanged("IdWorkStation");
            }
        }

        [DataMember]
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [DataMember]
        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
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

        [DataMember]
        public Int32 IdCounterpartTracking
        {
            get
            {
                return idCounterpartTracking;
            }

            set
            {
                idCounterpartTracking = value;
                OnPropertyChanged("IdCounterpartTracking");
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        //[GEOS2-5127][gulab lakade][20 12 2023]
        [DataMember]
        public string FailCode
        {
            get
            {
                return failCode;
            }

            set
            {
                failCode = value;
                OnPropertyChanged("FailCode");
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        //[GEOS2-6058][gulab lakade][02 10 2024]
        [DataMember]
        public string AvailableDayName
        {
            get
            {
                return availableDayName;
            }

            set
            {
                availableDayName = value;
                OnPropertyChanged("AvailableDayName");
            }
        }
        [DataMember]
        public string EndDateWeek
        {
            get
            {
                return endDateWeek;
            }

            set
            {
                endDateWeek = value;
                OnPropertyChanged("EndDateWeek");
            }
        }
        //[GEOS2-6058][gulab lakade][02 10 2024]


        //[pallavi.jadhav][29 04 2025][GEOS2-7066]
        private decimal? expected;
        public decimal? Expected   
        {
            get
            {
                return expected;
            }

            set
            {
                expected = value;
                OnPropertyChanged("Expected");
            }
        }
        //[GEOS2-9220][gulab lakade][12 08 2025]
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
        public string CalenderWeek
        {
            get { return calenderWeek; }
            set
            {
                calenderWeek = value;
                OnPropertyChanged("CalenderWeek");
            }
        }
        [DataMember]
        public Int32 ProductionActivityTimeType
        {
            get
            {
                return productionActivityTimeType;
            }

            set
            {
                productionActivityTimeType = value;
                OnPropertyChanged("ProductionActivityTimeType");
            }
        }
        //[GEOS2-9220][gulab lakade][12 08 2025]

        [NotMapped]
        [DataMember]
        public TimeSpan CounterPartStartTime
        {
            get { return counterPartStartTime; }

            set
            {
                counterPartStartTime = value;
                OnPropertyChanged("CounterPartStartTime");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan CounterPartEndTime
        {
            get { return counterPartEndTime; }

            set
            {
                counterPartEndTime = value;
                OnPropertyChanged("CounterPartEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? CounterpartStartDate
        {
            get { return counterpartStartDate; }

            set
            {
                counterpartStartDate = value;
                OnPropertyChanged("CounterpartStartDate");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? CounterpartEndDate
        {
            get { return counterpartEndDate; }

            set
            {
                counterpartEndDate = value;
                OnPropertyChanged("CounterPartEndDate");
            }
        }
        private Int32 isNightShift;
        [NotMapped]
        [DataMember]
        public Int32 IsNightShift
        {
            get { return isNightShift; }
            set
            {
                isNightShift = value;
                OnPropertyChanged("IsNightShift");
            }
        }
        #region [GEOS2-9560][gulab lakade][25 11 2025]
        [NotMapped]
        [DataMember]
        public TimeSpan ShiftStartTime
        {
            get { return shiftStartTime; }

            set
            {
                shiftStartTime = value;
                OnPropertyChanged("ShiftStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan ShiftEndTime
        {
            get { return shiftEndTime; }

            set
            {
                shiftEndTime = value;
                OnPropertyChanged("ShiftEndTime");
            }
        }
        #endregion 
        #endregion

        #region [GEOS2-7091][rani dhamankar] [19-09-2025]
        [DataMember]
                public Int32 IdSite
                {
                    get
                    {
                        return idSite;
                    }

                    set
                    {
                        idSite = value;
                        OnPropertyChanged("IdSite");
                    }
                }
        #endregion
    }
}
