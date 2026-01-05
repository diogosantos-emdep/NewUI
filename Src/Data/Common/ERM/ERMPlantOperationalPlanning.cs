using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMPlantOperationalPlanning : ModelBase, IDisposable
    {


        #region Field
        #region Field
        private Int32 idEmployee;
        private Int32 idCompany;
        //private Int32 idSite;
        private Int32 idJobDescription;
        private decimal jobDescriptionUsage;
        private DateTime? jobDescriptionStartDate;
        private DateTime? endDate;


        #endregion
        private Visibility isVisibility;
        private string calenderWeek;
        List<ERMEmployeeInformation> employeeInformation;
        //List<ERMEmployeeJobDescription> employeeJobDescription;
        List<ERMEmployeeLeave> employeeLeave;
        List<ERMCompanyHoliday> companyHoliday;
        List<PlantOperationalPlanningRealInfo> plantOperationalPlanningRealInfo; //[GEOS2-4553][Rupali Sarode][12-06-2023]
     
        #endregion
        #region Property
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
        // [NotMapped]
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
        #endregion
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
        public List<ERMEmployeeInformation> EmployeeInformation
        {
            get
            {
                return employeeInformation;
            }

            set
            {
                employeeInformation = value;
                OnPropertyChanged("EmployeeInformation");
            }
        }

        //[DataMember]
        //public List<ERMEmployeeJobDescription> EmployeeJobDescription
        //{
        //    get
        //    {
        //        return employeeJobDescription;
        //    }

        //    set
        //    {
        //        employeeJobDescription = value;
        //        OnPropertyChanged("EmployeeJobDescription");
        //    }
        //}
        [DataMember]
        public List<ERMEmployeeLeave> EmployeeLeave
        {
            get
            {
                return employeeLeave;
            }

            set
            {
                employeeLeave = value;
                OnPropertyChanged("EmployeeLeave");
            }
        }
        [DataMember]
        public List<ERMCompanyHoliday> CompanyHoliday
        {
            get
            {
                return companyHoliday;
            }

            set
            {
                companyHoliday = value;
                OnPropertyChanged("CompanyHoliday");
            }
        }

        [DataMember]
        public List<PlantOperationalPlanningRealInfo> PlantOperationalPlanningRealInfo
        {
            get
            {
                return plantOperationalPlanningRealInfo;
            }

            set
            {
                plantOperationalPlanningRealInfo = value;
                OnPropertyChanged("PlantOperationalPlanningRealInfo");
            }
        }

        public Visibility IsVisibility
        {
            get
            {
                return isVisibility;
            }

            set
            {
                isVisibility = value;
                OnPropertyChanged("IsVisibility");
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
        #region Constructor
        public ERMPlantOperationalPlanning()
        {

        }
        #endregion
    }

    public class ERMNonOTItemType : ModelBase, IDisposable
    {
        #region Field
        private Int32 idReason;
        private string reasonValue;
        private Int32 real;
        #endregion Field


        #region Properties

        [DataMember]
        public Int32 IdReason
        {
            get
            {
                return idReason;
            }

            set
            {
                idReason = value;
                OnPropertyChanged("IdReason");
            }
        }


        [DataMember]
        public string ReasonValue
        {
            get
            {
                return reasonValue;
            }

            set
            {
                reasonValue = value;
                OnPropertyChanged("ReasonValue");
            }
        }
        public Int32 Real
        {
            get { return real; }
            set
            {
                real = value;
                OnPropertyChanged("Real");
            }
        }

        #endregion


        #region Constructor
        public ERMNonOTItemType()
        {

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
