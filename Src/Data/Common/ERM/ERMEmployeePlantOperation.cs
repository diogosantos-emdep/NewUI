using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMEmployeePlantOperation : ModelBase, IDisposable
    {
        #region Field
        private Int32 idEmployee;
        private string calenderWeek;
        private Int32 idCompany;
        private string employeeName;
        private Int32 idJobDescription;
        private decimal jobDescriptionUsage;
        private DateTime? jobDescriptionStartDate;
        private DateTime? endDate;
        private decimal hRExpected;
        private decimal hRPlan;
        private Int32 idStage;
        private List<PlantOperationalPlanningRealInfo> employeePlantOperationalRealTimeList;

        private string reasonValue;
        private float timeDifferenceInMinutes;
        private float realTime;  //[Rupali Sarode][GEOS2-4553][12-06-2023]
        private string plantName;    //[GEOS2-4839][gulab lakade][18 09 2023]
        //private Int32 realTime;
        //private DateTime realStartDate;
        //private DateTime realEndDate;


        #endregion
        #region Property
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
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
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        [DataMember]
        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [DataMember]
        public decimal JobDescriptionUsage
        {
            get
            {
                return jobDescriptionUsage;
            }

            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }
        [DataMember]
        public DateTime? JobDescriptionStartDate
        {
            get
            {
                return jobDescriptionStartDate;
            }

            set
            {
                jobDescriptionStartDate = value;
                OnPropertyChanged("JobDescriptionStartDate");
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
        public decimal HRExpected
        {
            get { return hRExpected; }
            set
            {
                hRExpected = value;
                OnPropertyChanged("HRExpected");
            }
        }

        [DataMember]
        public decimal HRPlan
        {
            get { return hRPlan; }
            set
            {
                hRPlan = value;
                OnPropertyChanged("HRPlan");
            }
        }
        [DataMember]
        public Int32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [DataMember]
        public List<PlantOperationalPlanningRealInfo> EmployeePlantOperationalRealTimeList
        {
            get { return employeePlantOperationalRealTimeList; }
            set
            {
                employeePlantOperationalRealTimeList = value;
                OnPropertyChanged("EmployeePlantOperationalRealTimeList");
            }
        }

        [DataMember]
        public string ReasonValue
        {
            get { return reasonValue; }
            set
            {
                reasonValue = value;
                OnPropertyChanged("ReasonValue");
            }
        }

        [DataMember]
        public float TimeDifferenceInMinutes
        {
            get { return timeDifferenceInMinutes; }
            set
            {
                timeDifferenceInMinutes = value;
                OnPropertyChanged("TimeDifferenceInMinutes");
            }
        }

        [DataMember]
        public float RealTime
        {
            get { return realTime; }
            set
            {
                realTime = value;
                OnPropertyChanged("RealTime");
            }
        }
        //[GEOS2-4839][gulab lakade][18 09 2023]
        [DataMember]
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }
        //end [GEOS2-4839][gulab lakade][18 09 2023]
        //[DataMember]
        //public DateTime RealStartDate
        //{
        //    get { return realStartDate; }
        //    set
        //    {
        //        realStartDate = value;
        //        OnPropertyChanged("RealStartDate");
        //    }
        //}

        //[DataMember]
        //public DateTime RealEndDate
        //{
        //    get { return realEndDate; }
        //    set
        //    {
        //        realEndDate = value;
        //        OnPropertyChanged("RealEndDate");
        //    }
        //}


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
        #region Constructor
        public ERMEmployeePlantOperation()
        {

        }
        #endregion
    }
}
