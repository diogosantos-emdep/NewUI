using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_ProductionTimeline : ModelBase, IDisposable
    {
        #region Field
        //Stage
        private Int32 idStage;
        private string stageCode;
        //Employee
        private Int32 idEmployee;
        private string employeeCode;
        private string employeeName;
        //Attendance
        private DateTime? attendanceStartDate;
        private DateTime? attendanceEndDate;
        private Int32 attendanceTimeDifference;
        //Jobdescription
        private DateTime? jobDescriptionStartDate;
        private Int32 jobDescriptionUsage;
        private DateTime? jobDescriptionEndDate;//[GEOS2-6035][gulabrao lakade][01 08 2024]
        //Counter part
        private Int64 idCounterpart;
        private DateTime? counterpartStartDate;
        private DateTime? counterpartEndDate;
        private Int32 counterpartTimeDifference;
        //Leave
        private DateTime? leaveStartDate;
        private DateTime? leaveEndDate;
        private Int32 leavesAllDayEvent;
        private Int32 dailyHoursCount;
        //Site
        private Int32 idSite;
        private string siteName;

        //private Int32 idCounterpartTracking;
        byte[] profileImageInBytes;
        TimeSpan counterPartStartTime;
        TimeSpan counterPartEndTime;
        TimeSpan breakTime;
        TimeSpan leaveStartTime;
        TimeSpan leaveEndTime;
       
        TimeSpan shiftStartTime;
        TimeSpan shiftEndTime;
        private DateTime? breakStartDate;
        private DateTime? breakEndDate;
        TimeSpan breakStartTime;
        TimeSpan breakEndTime;
        private string stageName;
        private Int64 idCounterpartTracking;
        //HTMLCOlORbreakTime
        private string attendanceType;
        private string attendanceTypeColor;
        private string leaveType;
        private string leaveTypeColor;
        private string pauseType;
        private string pauseTypeColor;
        private Int32 sequence;
        private bool? attendanceTypeInUse;
        private bool? leaveTypeInUse;
        private bool? pauseTypeInUse;
        //[GEOS2-5233][gulab lakade][16 01 2024]
        private string holidayName;
        private string holidayType;
        private DateTime? holidayStartDate;
        private DateTime? holidayEndDate;
        private bool? iSHolidayEvent;
        //[GEOS2-5233][gulab lakade][16 01 2024]

        private string attendanceWeek; //[GEOS2-5238][gulab lakade][29 01 2024]
        private Int32 idOperator;   //[GEOS2-5418] [gulab lakade] [23 02 2024]
        private Int32 idUser;   //[GEOS2-5418] [gulab lakade] [23 02 2024]
        private Int32 idJobDescription;
        private Int32 isNightShift;  // [GEOS2-5515][gulab lakade][28 03 2024]
        private Int64 idOT;
        private Int64 idDrawing;
        private Int64 idOTItem;
        private string switchToStage;
        private string workingDay; //[GEOS2-5911][gulab lakade][25 07 2024]
        private Int32 month;
        private string shiftName; //[GEOS2-6040][gulab lakade] [13 08 2024]
        private Int32 idCompanyShift;//[GEOS2-6040][gulab lakade] [13 08 2024]
        #region [GEOS2-6554][gulab lakade][28 11 2024]
        private Int32? nonOT_IdStage;
        private string nonOT_Code;
        private string nonOT_Name;
       
        private string sW_Place_Code;
        private string sW_Place_Name;
        private Int32 isMainJobDescription;
        private Int32 pauseTypeId;
        private string nonOtComment;
        private string comment;// [pallavi jadhav][GEOS2-6716][04 12 2024]
        #endregion
        private Int32 productionActivityTimeType; //[GEOS2-7099][gulab lakade][07 04 2025]
        private string designSystem;// [rani dhamankar][21-04-2025][GEOS2-7098]
        private bool rework; // [rani dhamankar][21-04-2025][GEOS2-7098]
        List<TimeTrackingCurrentStage> timeTrackingAddingPostServer;// [rani dhamankar][21-04-2025][GEOS2-7098]
        #endregion
        #region Property


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
        public Int32 IdEmployee
        {
            get
            {
                return idEmployee;
            }

            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [DataMember]
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }

            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }
        [DataMember]
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }

            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        [DataMember]
        public DateTime? AttendanceStartDate
        {
            get
            {
                return attendanceStartDate;
            }

            set
            {
                attendanceStartDate = value;
                OnPropertyChanged("AttendanceStartDate");
            }
        }

        [DataMember]
        public DateTime? AttendanceEndDate
        {
            get
            {
                return attendanceEndDate;
            }

            set
            {
                attendanceEndDate = value;
                OnPropertyChanged("AttendanceEndDate");
            }
        }

        [DataMember]
        public Int32 AttendanceTimeDifference
        {
            get
            {
                return attendanceTimeDifference;
            }

            set
            {
                attendanceTimeDifference = value;
                OnPropertyChanged("AttendanceTimeDifference");
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
        public Int32 JobDescriptionUsage
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
        public Int64 IdCounterpart
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
            public DateTime? CounterpartStartDate
            {
                get
                {
                    return counterpartStartDate;
                }

                set
                {
                    counterpartStartDate = value;
                    OnPropertyChanged("CounterpartStartDate");
                }
            }

            [DataMember]
            public DateTime? CounterpartEndDate
            {
                get
                {
                    return counterpartEndDate;
                }

                set
                {
                    counterpartEndDate = value;
                    OnPropertyChanged("CounterpartEndDate");
                }
            }

            [DataMember]
            public Int32 CounterpartTimeDifference
            {
                get
                {
                    return counterpartTimeDifference;
                }

                set
                {
                    counterpartTimeDifference = value;
                    OnPropertyChanged("CounterpartTimeDifference");
                }
            }

        [DataMember]
        public DateTime? LeaveStartDate
        {
            get
            {
                return leaveStartDate;
            }

            set
            {
                leaveStartDate = value;
                OnPropertyChanged("LeaveStartDate");
            }
        }

        [DataMember]
        public DateTime? LeaveEndDate
        {
            get
            {
                return leaveEndDate;
            }

            set
            {
                leaveEndDate = value;
                OnPropertyChanged("LeaveEndDate");
            }
        }

        [DataMember]
        public Int32 LeavesAllDayEvent
        {
            get
            {
                return leavesAllDayEvent;
            }

            set
            {
                leavesAllDayEvent = value;
                OnPropertyChanged("LeavesAllDayEvent");
            }
        }
        [DataMember]
        public Int32 DailyHoursCount
        {
            get
            {
                return dailyHoursCount;
            }

            set
            {
                dailyHoursCount = value;
                OnPropertyChanged("DailyHoursCount");
            }
        }

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

        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }

            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }
        [NotMapped]
        [DataMember]
        public byte[] ProfileImageInBytes
        {
            get { return profileImageInBytes; }

            set
            {
                profileImageInBytes = value;
                OnPropertyChanged("ProfileImageInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan CounterPartStartTime
        {
            get { return counterPartStartTime ; }

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
        public TimeSpan BreakTime
        {
            get { return breakTime; }

            set
            {
                breakTime = value;
                OnPropertyChanged("BreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan LeaveStartTime
        {
            get { return leaveStartTime; }

            set
            {
                leaveStartTime = value;
                OnPropertyChanged("LeaveStartTime");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan LeaveEndTime
        {
            get { return leaveEndTime; }

            set
            {
                leaveEndTime = value;
                OnPropertyChanged("LeaveEndTime");
            }
        }

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
        [NotMapped]
        [DataMember]
        public DateTime? BreakStartDate
        {
            get { return breakStartDate; }

            set
            {
                breakStartDate = value;
                OnPropertyChanged("BreakStartDate");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? BreakEndDate
        {
            get { return breakEndDate; }

            set
            {
                breakEndDate = value;
                OnPropertyChanged("BreakEndDate");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan BreakStartTime
        {
            get { return breakStartTime ; }

            set
            {
                breakStartTime = value;
                OnPropertyChanged("BreakStartTime");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan BreakEndTime
        {
            get { return breakEndTime; }

            set
            {
                breakEndTime = value;
                OnPropertyChanged("BreakEndTime");
            }
        }

        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
            }
        }

        [DataMember]
        public Int64 IdCounterpartTracking
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

        //HTMLCOlOR
        [DataMember]
        public string AttendanceType
        {
            get
            {
                return attendanceType;
            }

            set
            {
                attendanceType = value;
                OnPropertyChanged("AttendanceType");
            }
        }
        [DataMember]
        public string AttendanceTypeColor
        {
            get
            {
                return attendanceTypeColor;
            }

            set
            {
                attendanceTypeColor = value;
                OnPropertyChanged("AttendanceTypeColor");
            }
        }
        [DataMember]
        public string LeaveType
        {
            get
            {
                return leaveType;
            }

            set
            {
                leaveType = value;
                OnPropertyChanged("LeaveType");
            }
        }
        [DataMember]
        public string LeaveTypeColor
        {
            get
            {
                return leaveTypeColor;
            }

            set
            {
                leaveTypeColor = value;
                OnPropertyChanged("LeaveTypeColor");
            }
        }
        [DataMember]
        public string PauseType
        {
            get
            {
                return pauseType;
            }

            set
            {
                pauseType = value;
                OnPropertyChanged("PauseType");
            }
        }
        [DataMember]
        public string PauseTypeColor
        {
            get
            {
                return pauseTypeColor;
            }

            set
            {
                pauseTypeColor = value;
                OnPropertyChanged("PauseTypeColor");
            }
        }
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        [DataMember]
        public bool? AttendanceTypeInUse
        {
            get
            {
                return attendanceTypeInUse;
            }

            set
            {
                attendanceTypeInUse = value;
                OnPropertyChanged("AttendanceTypeInUse");
            }
        }
        [DataMember]
        public bool? LeaveTypeInUse
        {
            get
            {
                return leaveTypeInUse;
            }

            set
            {
                leaveTypeInUse = value;
                OnPropertyChanged("LeaveTypeInUse");
            }
        }

        [DataMember]
        public bool? PauseTypeInUse
        {
            get
            {
                return pauseTypeInUse;
            }

            set
            {
                pauseTypeInUse = value;
                OnPropertyChanged("PauseTypeInUse");
            }
        }
        //[GEOS2-5233][gulab lakade][16 01 2024]
        [DataMember]
        public string HolidayName
        {
            get
            {
                return holidayName;
            }

            set
            {
                holidayName = value;
                OnPropertyChanged("HolidayName");
            }
        }
        [DataMember]
        public string HolidayType
        {
            get
            {
                return holidayType;
            }

            set
            {
                holidayType = value;
                OnPropertyChanged("HolidayType");
            }
        }
        [DataMember]
        public DateTime? HolidayStartDate
        {
            get { return holidayStartDate; }

            set
            {
                holidayStartDate = value;
                OnPropertyChanged("HolidayStartDate");
            }
        }
        [DataMember]
        public DateTime? HolidayEndDate
        {
            get { return holidayEndDate; }

            set
            {
                holidayEndDate = value;
                OnPropertyChanged("HolidayEndDate");
            }
        }
       
        [DataMember]
        public bool? ISHolidayEvent
        {
            get
            {
                return iSHolidayEvent;
            }

            set
            {
                iSHolidayEvent = value;
                OnPropertyChanged("ISHolidayEvent");
            }
        }
        //[GEOS2-5233][gulab lakade][16 01 2024]
        //[GEOS2-5238][gulab lakade][29 01 2024]
        [DataMember]
        public string AttendanceWeek
        {
            get
            {
                return attendanceWeek;
            }

            set
            {
                attendanceWeek = value;
                OnPropertyChanged("AttendanceWeek");
            }
        }
        //[GEOS2-5418] [gulab lakade] [23 02 2024]
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
        public Int32 IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }
        // [GEOS2-5515][gulab lakade][28 03 2024]
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
        // [GEOS2-5515][gulab lakade][28 03 2024]

        [DataMember]
        public Int64 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }
        [DataMember]
        public Int64 IdDrawing
        {
            get
            {
                return idDrawing;
            }

            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }
        [DataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }
        [DataMember]
        public string SwitchToStage
        {
            get
            {
                return switchToStage;
            }

            set
            {
                switchToStage = value;
                OnPropertyChanged("SwitchToStage");
            }
        }
        //[GEOS2-5911][gulab lakade][25 07 2024]
        [DataMember]
        public string WorkingDay
        {
            get
            {
                return workingDay;
            }

            set
            {
                workingDay = value;
                OnPropertyChanged("WorkingDay");
            }
        }
        //end [GEOS2-5911][gulab lakade][25 07 2024]

        [DataMember]
        public Int32 Month
        {
            get
            {
                return month;
            }

            set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }

        //[GEOS2-6035][gulabrao lakade][01 08 2024]
        [DataMember]
        public DateTime? JobDescriptionEndDate
        {
            get
            {
                return jobDescriptionEndDate;
            }

            set
            {
                jobDescriptionEndDate = value;
                OnPropertyChanged("JobDescriptionEndDate");
            }
        }
        //[GEOS2-6035][gulabrao lakade][01 08 2024]
        //start[GEOS2-6040][gulab lakade] [13 08 2024]
        [DataMember]
        public string ShiftName
        {
            get
            {
                return shiftName;
            }

            set
            {
                shiftName = value;
                OnPropertyChanged("ShiftName");
            }
        }
        [DataMember]
        public Int32 IdCompanyShift
        {
            get
            {
                return idCompanyShift;
            }

            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }
        //end [GEOS2-6040][gulab lakade] [13 08 2024]
        #region [GEOS2-6554][gulab lakade][28 11 2024]
        [DataMember]
        public Int32? NonOT_IdStage
        {
            get
            {
                return nonOT_IdStage; 
            }

            set
            {
                nonOT_IdStage = value;
                OnPropertyChanged("NonOT_IdStage");
            }
        }
        [DataMember]
        public string NonOT_Code
        {
            get
            {
                return nonOT_Code;
            }

            set
            {
                nonOT_Code = value;
                OnPropertyChanged("NonOT_Code");
            }
        }
        [DataMember]
        public string NonOT_Name
        {
            get
            {
                return nonOT_Name;
            }

            set
            {
                nonOT_Name = value;
                OnPropertyChanged("NonOT_Name");
            }
        }

        [DataMember]
        public string SW_Place_Code
        {
            get
            {
                return sW_Place_Code;
            }

            set
            {
                sW_Place_Code = value;
                OnPropertyChanged("SW_Place_Code");
            }
        }
        [DataMember]
        public string SW_Place_Name
        {
            get
            {
                return sW_Place_Name;
            }

            set
            {
                sW_Place_Name = value;
                OnPropertyChanged("SW_Place_Name");
            }
        }
        [DataMember]
        public Int32 IsMainJobDescription
        {
            get
            {
                return isMainJobDescription;
            }

            set
            {
                isMainJobDescription = value;
                OnPropertyChanged("IsMainJobDescription");
            }
        }

        [DataMember]
        public Int32 PauseTypeId
        {
            get
            {
                return pauseTypeId;
            }

            set
            {
                pauseTypeId = value;
                OnPropertyChanged("PauseTypeId");
            }
        }
        [DataMember]
        public string NonOtComment
        {
            get
            {
                return nonOtComment;
            }

            set
            {
                nonOtComment = value;
                OnPropertyChanged("NonOtComment");
            }
        }
        #endregion
        #region// [pallavi jadhav][GEOS2-6716][04 12 2024]

        [DataMember]
        public string Comment   // [pallavi jadhav][GEOS2-6716][04 12 2024]
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        #endregion
        //[GEOS2-7099][gulab lakade][07 04 2025]
        [DataMember]
        public Int32 ProductionActivityTimeType
        {
            get { return productionActivityTimeType; }
            set
            {
                productionActivityTimeType = value;
                OnPropertyChanged("ProductionActivityTimeType");
            }
        }
        //[GEOS2-7099][gulab lakade][07 04 2025]
        #endregion


        #region  [rani dhamankar][21-04-2025][GEOS2-7098]
        [DataMember]
        public string DesignSystem
        {
            get
            {
                return designSystem;
            }

            set
            {
                designSystem = value;
                OnPropertyChanged("DesignSystem");
            }
        }
        [DataMember]
        public bool Rework
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
        public List<TimeTrackingCurrentStage> TimeTrackingAddingPostServer
        {
            get
            {
                return timeTrackingAddingPostServer;
            }

            set
            {
                timeTrackingAddingPostServer = value;
                OnPropertyChanged("TimeTrackingAddingPostServer");
            }
        }
        #endregion
        #region Constructor
        public ERM_ProductionTimeline()
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
