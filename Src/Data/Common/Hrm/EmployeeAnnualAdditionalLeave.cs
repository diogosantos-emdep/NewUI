using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_annual_leaves_additional")]
    [DataContract]
    public class EmployeeAnnualAdditionalLeave : ModelBase, IDisposable
    {
        #region Fields

        Int64 idEmployeeAnnualLeave;
        Int32 idEmployee;
        Int32 year;
        Int64 idLeave;
        decimal regularHoursCount;
        //decimal additionalHoursCount;
        Employee employee;
        CompanyLeave companyLeave;
        //decimal enjoyed;
        //decimal remaining;
        //Int32 leaveBackground;
        //decimal totalHoursCount;
        //string regularHoursInDays;
        //string additionalHoursInDays;
        //string totalHoursInDays;
        //string enjoyedHoursInDays;
        //string remainingHoursInDays;
        //JobDescription jobDescription;
        //string companyLocation;
        private decimal backlogHoursCount;

        Int64 idEmployeeAnnualLeavesAdditional;
        //   Int32 idLookupValue;
        decimal additionalLeaveTotalHours;
        string additionalLeaveTotalHoursInDays;
        private int idAdditionalLeaveReason;


        string additionalLeaveReasonName;
        Int32? additionalLeaveLookupKey;//76
        Int32? additionalLeavePosition;
        string additionalLeaveHtmlColor;
        bool additionalLeaveInUse;

        private int convertedDays;
        private decimal convertedHours;
        private string comments = string.Empty;
        private DateTime? accountingDate;
        //GEOS2-2607
        //string backlogHoursInDays;
        //string enjoyedTillYesterdayInDays;
        //string enjoyingFromTodayInDays;
        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeAnnualLeave")]
        [DataMember]
        public Int64 IdEmployeeAnnualLeave
        {
            get { return idEmployeeAnnualLeave; }
            set
            {
                idEmployeeAnnualLeave = value;
                OnPropertyChanged("IdEmployeeAnnualLeave");
            }
        }

        [Column("IdEmployee")]
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

        [Column("Year")]
        [DataMember]
        public Int32 Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [Column("IdLeave")]
        [DataMember]
        public Int64 IdLeave
        {
            get { return idLeave; }
            set
            {
                idLeave = value;
                OnPropertyChanged("IdLeave");
            }
        }

        [Obsolete("Use EmployeeAnnualLeave.RegularHoursCount")]
        [Column("RegularHoursCount")]
        [DataMember]
        public decimal RegularHoursCount
        {
            get { return regularHoursCount; }
            set
            {
                regularHoursCount = value;

                OnPropertyChanged("RegularHoursCount");
            }
        }

        //[Column("AdditionalHoursCount")]
        //[DataMember]
        //public decimal AdditionalHoursCount
        //{
        //    get { return additionalHoursCount; }
        //    set
        //    {
        //        additionalHoursCount = value;

        //        OnPropertyChanged("AdditionalHoursCount");
        //    }
        //}


        [NotMapped]
        [DataMember]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyLeave CompanyLeave
        {
            get { return companyLeave; }
            set
            {
                companyLeave = value;
                OnPropertyChanged("CompanyLeave");
            }
        }

        //[Obsolete("Use EnjoyedTillYesterday and EnjoyingFromToday")]
        //[NotMapped]
        //[DataMember]
        //public decimal Enjoyed
        //{
        //    get { return enjoyed; }
        //    set
        //    {
        //        enjoyed = value;
        //        OnPropertyChanged("Enjoyed");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public decimal Remaining
        //{
        //    get { return remaining; }
        //    set
        //    {
        //        remaining = value;
        //        OnPropertyChanged("Remaining");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public int LeaveBackground
        //{
        //    get { return leaveBackground; }
        //    set
        //    {
        //        leaveBackground = value;
        //        OnPropertyChanged("LeaveBackground");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public decimal TotalHoursCount
        //{
        //    get { return totalHoursCount; }
        //    set
        //    {
        //        totalHoursCount = value;

        //        OnPropertyChanged("TotalHoursCount");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string BacklogHoursInDays
        //{
        //    get { return backlogHoursInDays; }
        //    set
        //    {
        //        backlogHoursInDays = value;

        //        OnPropertyChanged(nameof(BacklogHoursInDays));
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string RegularHoursInDays
        //{
        //    get { return regularHoursInDays; }
        //    set
        //    {
        //        regularHoursInDays = value;

        //        OnPropertyChanged("RegularHoursInDays");
        //    }
        //}

        //[Obsolete("Use SelectedAdditionalLeaves")]
        //[NotMapped]
        //[DataMember]
        //public string AdditionalHoursInDays
        //{
        //    get { return additionalHoursInDays; }
        //    set
        //    {
        //        additionalHoursInDays = value;

        //        OnPropertyChanged("AdditionalHoursInDays");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string TotalHoursInDays
        //{
        //    get { return totalHoursInDays; }
        //    set
        //    {
        //        totalHoursInDays = value;

        //        OnPropertyChanged("TotalHoursInDays");
        //    }
        //}


        ////GEOS2-2607
        //[NotMapped]
        //[DataMember]
        //public string EnjoyedTillYesterdayInDays
        //{
        //    get { return enjoyedTillYesterdayInDays; }
        //    set
        //    {
        //        enjoyedTillYesterdayInDays = value;
        //        OnPropertyChanged(nameof(EnjoyedTillYesterdayInDays));
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string EnjoyingFromTodayInDays
        //{
        //    get { return enjoyingFromTodayInDays; }
        //    set
        //    {
        //        enjoyingFromTodayInDays = value;
        //        OnPropertyChanged(nameof(EnjoyingFromTodayInDays));
        //    }
        //}

        //[Obsolete("Use EnjoyedTillYesterdayInDays, EnjoyingFromTodayInDays")]
        //[NotMapped]
        //[DataMember]
        //public string EnjoyedHoursInDays
        //{
        //    get { return enjoyedHoursInDays; }
        //    set
        //    {
        //        enjoyedHoursInDays = value;
        //        OnPropertyChanged("EnjoyedHoursInDays");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string RemainingHoursInDays
        //{
        //    get { return remainingHoursInDays; }
        //    set
        //    {
        //        remainingHoursInDays = value;

        //        OnPropertyChanged("RemainingHoursInDays");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public JobDescription JobDescription
        //{
        //    get
        //    {
        //        return jobDescription;
        //    }

        //    set
        //    {
        //        jobDescription = value;
        //        OnPropertyChanged("JobDescription");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string CompanyLocation
        //{
        //    get
        //    {
        //        return companyLocation;
        //    }

        //    set
        //    {
        //        companyLocation = value;
        //        OnPropertyChanged("CompanyLocation");
        //    }
        //}

        [Obsolete("Use EmployeeAnnualLeave.BacklogHoursCount")]
        [Column("BacklogHoursCount")]
        [DataMember]
        public decimal BacklogHoursCount
        {
            get { return backlogHoursCount; }
            set
            {
                backlogHoursCount = value;

                OnPropertyChanged("BacklogHoursCount");
            }
        }

        [Key]
        [Column("IdEmployeeAnnualLeavesAdditional")]
        [DataMember]
        public Int64 IdEmployeeAnnualLeavesAdditional
        {
            get { return idEmployeeAnnualLeavesAdditional; }
            set
            {
                idEmployeeAnnualLeavesAdditional = value;
                OnPropertyChanged("IdEmployeeAnnualLeavesAdditional");
            }
        }

        //[Key]
        //[Column("IdEmployeeAnnualLeave")]
        //[DataMember]
        //public Int64 IdEmployeeAnnualLeave
        //{
        //    get { return idEmployeeAnnualLeave; }
        //    set
        //    {
        //        idEmployeeAnnualLeave = value;
        //        OnPropertyChanged("IdEmployeeAnnualLeave");
        //    }
        //}



        [Column("IdAdditionalLeaveReason")]
        [DataMember]
        public int IdAdditionalLeaveReason
        {
            get
            {
                return idAdditionalLeaveReason;
            }

            set
            {
                idAdditionalLeaveReason = value;
                OnPropertyChanged("IdAdditionalLeaveReason");
            }
        }


        [Column("Hours")]
        [DataMember]
        public decimal AdditionalLeaveTotalHours
        {
            get
            {
                return additionalLeaveTotalHours;
            }

            set
            {
                additionalLeaveTotalHours = value;
                OnPropertyChanged("AdditionalLeaveTotalHours");
            }
        }

        [NotMapped]
        [DataMember]
        public string AdditionalLeaveTotalHoursInDays
        {
            get
            {
                return additionalLeaveTotalHoursInDays;
            }

            set
            {
                additionalLeaveTotalHoursInDays = value;
                OnPropertyChanged(nameof(AdditionalLeaveTotalHoursInDays));
            }
        }

        [NotMapped]
        [DataMember]
        public string AdditionalLeaveReasonName
        {
            get
            {
                return additionalLeaveReasonName;
            }

            set
            {
                this.additionalLeaveReasonName = value;
                OnPropertyChanged(nameof(AdditionalLeaveReasonName));
            }
        }

        [NotMapped]
        [DataMember]
        public int? AdditionalLeaveLookupKey
        {
            get
            {
                return additionalLeaveLookupKey;
            }

            set
            {
                this.additionalLeaveLookupKey = value;
                OnPropertyChanged(nameof(AdditionalLeaveLookupKey));
            }
        }

        [NotMapped]
        [DataMember]
        public int? AdditionalLeavePosition
        {
            get
            {
                return additionalLeavePosition;
            }

            set
            {
                this.additionalLeavePosition = value;
                OnPropertyChanged(nameof(AdditionalLeavePosition));
            }
        }

        [NotMapped]
        [DataMember]
        public string AdditionalLeaveHtmlColor
        {
            get
            {
                return additionalLeaveHtmlColor;
            }

            set
            {
                this.additionalLeaveHtmlColor = value;
                OnPropertyChanged(nameof(AdditionalLeaveHtmlColor));
            }
        }

        [NotMapped]
        [DataMember]
        public bool AdditionalLeaveInUse
        {
            get
            {
                return additionalLeaveInUse;
            }

            set
            {
                this.additionalLeaveInUse = value;
                OnPropertyChanged(nameof(AdditionalLeaveInUse));
            }
        }

        [NotMapped]
        [DataMember]
        public int ConvertedDays
        {
            get
            {
                return convertedDays;
            }

            set
            {
                this.convertedDays = value;
                OnPropertyChanged(nameof(ConvertedDays));
            }
        }

        [NotMapped]
        [DataMember]
        public decimal ConvertedHours
        {
            get
            {
                return convertedHours;
            }

            set
            {
                this.convertedHours = value;
                OnPropertyChanged(nameof(ConvertedHours));
            }
        }

        [NotMapped]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                this.comments = value.Trim();
                OnPropertyChanged(nameof(Comments));
            }
        }

        private List<EmployeeAnnualAdditionalLeave> oldadditionalLeaveListForGrid;
        public List<EmployeeAnnualAdditionalLeave> OldAdditionalLeaveListForGrid
        {
            get { return oldadditionalLeaveListForGrid; }
            set
            {
                oldadditionalLeaveListForGrid = value;
                OnPropertyChanged(nameof(OldAdditionalLeaveListForGrid));
            }
        }

        //[Sudhir.Jangra][GEOS2-5336]
        [NotMapped]
        [DataMember]
        public DateTime? AccountingDate
        {
            get { return accountingDate; }
            set
            {
                accountingDate = value;
                OnPropertyChanged("AccountingDate");
            }
        }
        #endregion

        #region Constructor

        public EmployeeAnnualAdditionalLeave()
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
            //return this.MemberwiseClone();
            var newClone = (EmployeeAnnualAdditionalLeave)this.MemberwiseClone();


            if (OldAdditionalLeaveListForGrid != null)
                newClone.OldAdditionalLeaveListForGrid = OldAdditionalLeaveListForGrid.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList();

            return newClone;
        }

        #endregion
    }
}
