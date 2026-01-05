using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_NO_OT_Time : ModelBase, IDisposable
    {
        #region Field
        private DateTime? breakStartDate;
        private DateTime? breakEndDate;
        private string pauseType;
        private string pauseTypeColor;
        private bool? pauseTypeInUse;
        private Int32 idOperator;
        private string switchToStage;
        private Int32? nonOT_IdStage;
        private string calenderWeek;
        private string nonOT_Code;
        TimeSpan breakStartTime;
        TimeSpan breakEndTime;
        private string nonOT_Name;
        private string sW_Place_Name;
        private string sW_Place_Code;
        private Int32 pauseTypeId;
        private string nonOtComment;
        #endregion
        #region Property
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
        [NotMapped]
        [DataMember]
        public TimeSpan BreakStartTime
        {
            get { return breakStartTime; }

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
        #region Constructor
        public ERM_NO_OT_Time()
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
