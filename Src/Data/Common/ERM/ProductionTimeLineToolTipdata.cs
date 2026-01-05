using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ProductionTimeLineToolTipdata : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idStage;
        private Int32 idEmployee;
        private DateTime? attendanceStartDate;
        private string barTypeName;
        private string toolTipsType;
        private double toolTipsStartTime;
        private double toolTipsEndTime;
        #region [GEOS2-6554][gulab lakade][28 11 2024]
        private string logStationName;//[GEOS2-6554][gulab lakade][28 11 2024]
        private string logStationStage;//[GEOS2-6554][gulab lakade][28 11 2024]
        private Int32 pauseTypeId;//[GEOS2-6554][gulab lakade][28 11 2024]
        private string sW_Place_Name;//[GEOS2-6554][gulab lakade][28 11 2024]
        private Int32? nonOT_IdStage;
        private string nonOT_Code;
        private string nonOT_Name;
        private string nonOtComment;
        private DateTime? leaveStartDate;//[GEOS2-6965][rani dhamankar][12-03-2025]
        #endregion
        #endregion

        #region Properties 
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
        public string BarTypeName
        {
            get
            {
                return barTypeName;
            }

            set
            {
                barTypeName = value;
                OnPropertyChanged("BarTypeName");
            }
        }

        [DataMember]
        public string ToolTipsType
        {
            get
            {
                return toolTipsType;
            }

            set
            {
                toolTipsType = value;
                OnPropertyChanged("ToolTipsType");
            }
        }

       
        [DataMember]
        public double ToolTipsStartTime
        {
            get
            {
                return toolTipsStartTime;
            }

            set
            {
                toolTipsStartTime = value;
                OnPropertyChanged("ToolTipsStartTime");
            }
        }
        [DataMember]
        public double ToolTipsEndTime
        {
            get
            {
                return toolTipsEndTime;
            }

            set
            {
                toolTipsEndTime = value;
                OnPropertyChanged("ToolTipsEndTime");
            }
        }
        //start[GEOS2-6554][gulab lakade][28 11 2024]
        [DataMember]
        public string LogStationName
        {
            get
            {
                return logStationName;
            }

            set
            {
                logStationName = value;
                OnPropertyChanged("LogStationName");
            }
        }
        [DataMember]
        public string LogStationStage
        {
            get
            {
                return logStationStage;
            }

            set
            {
                logStationStage = value;
                OnPropertyChanged("LogStationStage");
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
        //end[GEOS2-6554][gulab lakade][28 11 2024]

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
        #endregion
        #region Constructor
        public ProductionTimeLineToolTipdata()
        {
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
