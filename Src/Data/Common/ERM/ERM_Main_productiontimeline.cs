using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_Main_productiontimeline : ModelBase, IDisposable
    {
        #region Field
        List<ERM_EmployeeDetails> eRM_EmployeeDetails;
        List<ERM_Employee_Attendance> eRM_Employee_Attendance;
        List<ERM_Employee_Attendance> eRM_Employee_MAX_Attendance;
        List<ERMEmployeeLeave> eRMEmployeeLeave;
        List<ERMCompanyHoliday> eRMCompanyHoliday;
        List<Counterpartstracking> counterpartstracking;
        List<ERM_NO_OT_Time> eRM_NO_OT_Time;
        
        List<DesignSharedItemsEmployeeDetails> designSharedItemsEmployeeDetails;//[GEOS2-7091][rani dhamankar] [04-09-2025]
        List<ERM_OT_Working_Times> eRM_OT_Working_Times; //[GEOS2-9393][pallavi jadhav][11 11 2025]
        #endregion
        #region Field

        [DataMember]
        public List<ERM_EmployeeDetails> ERM_EmployeeDetails
        {
            get
            {
                return eRM_EmployeeDetails;
            }

            set
            {
                eRM_EmployeeDetails = value;
                OnPropertyChanged("ERM_EmployeeDetails");
            }
        }
        [DataMember]
        public List<ERM_Employee_Attendance> ERM_Employee_Attendance
        {
            get
            {
                return eRM_Employee_Attendance;
            }

            set
            {
                eRM_Employee_Attendance = value;
                OnPropertyChanged("ERM_Employee_Attendance");
            }
        }
        [DataMember]
        public List<ERM_Employee_Attendance> ERM_Employee_MAX_Attendance
        {
            get
            {
                return eRM_Employee_MAX_Attendance;
            }

            set
            {
                eRM_Employee_MAX_Attendance = value;
                OnPropertyChanged("ERM_Employee_MAX_Attendance");
            }
        }
        [DataMember]
        public List<ERMEmployeeLeave> ERMEmployeeLeave
        {
            get
            {
                return eRMEmployeeLeave;
            }

            set
            {
                eRMEmployeeLeave = value;
                OnPropertyChanged("ERMEmployeeLeave");
            }
        }
        [DataMember]
        public List<ERMCompanyHoliday> ERMCompanyHoliday
        {
            get
            {
                return eRMCompanyHoliday;
            }

            set
            {
                eRMCompanyHoliday = value;
                OnPropertyChanged("ERMCompanyHoliday");
            }
        }
        [DataMember]
        public List<Counterpartstracking> Counterpartstracking
        {
            get
            {
                return counterpartstracking;
            }

            set
            {
                counterpartstracking = value;
                OnPropertyChanged("Counterpartstracking");
            }
        }

        [DataMember]
        public List<ERM_NO_OT_Time> ERM_NO_OT_Time
        {
            get
            {
                return eRM_NO_OT_Time;
            }

            set
            {
                eRM_NO_OT_Time = value;
                OnPropertyChanged("ERM_NO_OT_Time");
            }
        }
        #endregion

        #region [GEOS2-7091][rani dhamankar] [04-09-2025]
        [DataMember]
        public List<DesignSharedItemsEmployeeDetails> DesignSharedItemsEmployeeDetails
        {
            get
            {
                return designSharedItemsEmployeeDetails;
            }

            set
            {
                designSharedItemsEmployeeDetails = value;
                OnPropertyChanged("DesignSharedItemsEmployeeDetails");
            }
        }
        #endregion

        #region //[GEOS2-9393][pallavi jadhav][11 11 2025]
        [DataMember]
        public List<ERM_OT_Working_Times> ERM_OT_Working_Times
        {
            get
            {
                return eRM_OT_Working_Times;
            }

            set
            {
                eRM_OT_Working_Times = value;
                OnPropertyChanged("ERM_OT_Working_Times");
            }
        }
        #endregion

        #region Constructor
        public ERM_Main_productiontimeline()
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
