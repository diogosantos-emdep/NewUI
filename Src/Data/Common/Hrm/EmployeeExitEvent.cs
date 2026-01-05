using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_exit_events")]
    [DataContract]
    public class EmployeeExitEvent : ModelBase, IDisposable, IDataErrorInfo
    {
        #region Fields

        Int64 idEmployeeExitEvent;
        DateTime? exitDate;
        DateTime? maxDate;

        Int32 idReason ;
        string remarks;
        string filename;

        Int32 idExitEvent;
        Int32 idEmployee;
        
        Employee employee;
        LookupValue exitReason;

        DateTime? minExitEventDate;
        int selectedExitEventReasonIndex;
      
        ObservableCollection<Attachment> exitEventattachmentList;
        Visibility isVisible;
       
        Attachment attachedFile;
        bool isEnable;
        byte[] exitEventBytes;
        bool isReadOnly;
        bool isExist;
        bool isExitDateEnable;
        string oldFileName;
        bool isExitEventFileDeleted;
        private int selectedEmpolyeeStatusIndex;
        string employeeProfileDetailExitEventValidationMessage;
        bool isDeleteExitEventEnabled;
        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeExitEvent")]
        [DataMember]
        public Int64 IdEmployeeExitEvent
        {
            get { return idEmployeeExitEvent; }
            set
            {
                idEmployeeExitEvent = value;
                OnPropertyChanged("IdEmployeeExitEvent");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? MaxDate
        {
            get { return maxDate; }
            set
            {
                maxDate = value;
                OnPropertyChanged("MaxDate");
            }
        }


        [Column("ExitDate")]
        [DataMember]
        public DateTime? ExitDate
        {
            get { return exitDate; }
            set
            {
                exitDate = value;
                OnPropertyChanged("ExitDate");
            }
        }

        [Column("IdReason")]
        [DataMember]
        public Int32 IdReason
        {
            get { return idReason; }
            set
            {
                idReason = value;
                OnPropertyChanged("IdReason");
            }
        }

        [Column("Remarks")]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [Column("FileName")]
        [DataMember]
        public string FileName
        {
            get { return filename; }
            set
            {
                filename = value;
                OnPropertyChanged("FileName");
            }
        }

        [Column("IdExitEvent")]
        [DataMember]
        public Int32 IdExitEvent
        {
            get { return idExitEvent; }
            set
            {
                idExitEvent = value;
                OnPropertyChanged("IdExitEvent");
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

        [NotMapped]
        [DataMember]
        public LookupValue ExitReason
        {
            get { return exitReason; }
            set
            {
                exitReason = value;
                OnPropertyChanged("ExitReason");
            }
        }

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
        public ObservableCollection<Attachment> ExitEventattachmentList
        {
            get { return exitEventattachmentList; }
            set
            {
                exitEventattachmentList = value;
                OnPropertyChanged("ExitEventattachmentList");
            }
        }

        [NotMapped]
        [DataMember]
        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        [NotMapped]
        [DataMember]
        public int SelectedExitEventReasonIndex
        {
            get { return selectedExitEventReasonIndex; }
            set
            {
                selectedExitEventReasonIndex = value;
                OnPropertyChanged("SelectedExitEventReasonIndex");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? MinExitEventDate
        {
            get { return minExitEventDate; }
            set
            {
                minExitEventDate = value;
                OnPropertyChanged("MinExitEventDate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LookupValue> ExitEventReasonList { get; set; }

        [NotMapped]
        [DataMember]
        public string Header { get; set; }

        [NotMapped]
        [DataMember]
        public Attachment AttachedFile
        {
            get { return attachedFile; }
            set
            {
                attachedFile = value;
                OnPropertyChanged("AttachedFile");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                OnPropertyChanged("IsEnable");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] ExitEventBytes
        {
            get { return exitEventBytes; }
            set
            {
                exitEventBytes = value;
                OnPropertyChanged("ExitEventBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsExitDateEnable
        {
            get { return isExitDateEnable; }
            set
            {
                isExitDateEnable = value;
                OnPropertyChanged("IsExitDateEnable");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsExist
        {
            get { return isExist; }
            set
            {
                isExist = value;
                OnPropertyChanged("IsExist");
            }
        }

       

        [NotMapped]
        [DataMember]
        public string OldFileName
        {
            get { return oldFileName; }
            set
            {
                oldFileName = value;
                OnPropertyChanged("OldFileName");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsExitEventFileDeleted
        {
            get { return isExitEventFileDeleted; }
            set
            {
                isExitEventFileDeleted = value;
                OnPropertyChanged("IsExitEventFileDeleted");
            }
        }


        [NotMapped]
        [DataMember]
        public int SelectedEmpolyeeStatusIndex
        {
            get { return selectedEmpolyeeStatusIndex; }
            set
            {
                selectedEmpolyeeStatusIndex = value;
                OnPropertyChanged("SelectedEmpolyeeStatusIndex");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeProfileDetailExitEventValidationMessage
        {
            get { return employeeProfileDetailExitEventValidationMessage; }
            set
            {
                employeeProfileDetailExitEventValidationMessage = value;
                OnPropertyChanged("EmployeeProfileDetailExitEventValidationMessage");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleteExitEventEnabled
        {
            get { return isDeleteExitEventEnabled; }
            set
            {
                isDeleteExitEventEnabled = value;
                OnPropertyChanged("IsDeleteExitEventEnabled");
            }
        }
        #endregion

        #region Constructor

        public EmployeeExitEvent()
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
            EmployeeExitEvent employeeExitEvent = (EmployeeExitEvent)this.MemberwiseClone();

            if (employeeExitEvent.Employee != null)
                employeeExitEvent.Employee = (Employee)this.Employee.Clone();

            if (employeeExitEvent.ExitReason != null)
                employeeExitEvent.ExitReason = (LookupValue)this.ExitReason.Clone();
                    

            return employeeExitEvent;
        }

        #endregion


        #region Validation
        public string Error

        {

            get

            {

                return this[string.Empty];

            }

        }

        public string this[string columnName]

        {
            get
            {
                string ReasonValidationMessage = "Employee Exit Reason should not be empty";
                string ExitDateValidationMessage = "Employee Exit Date should not be empty";
                if (columnName == "SelectedExitEventReasonIndex")
                {

                    if (SelectedEmpolyeeStatusIndex == 2)
                    {
                        if (SelectedExitEventReasonIndex == 0 || SelectedExitEventReasonIndex == -1)
                        {
                            EmployeeProfileDetailExitEventValidationMessage = ReasonValidationMessage;
                            return ReasonValidationMessage;
                        }

                    }
                    else
                    {
                        return null;
                    }


                }
                if (columnName == "ExitDate")
                {

                    if (SelectedEmpolyeeStatusIndex == 2)

                    {
                        if (ExitDate == null)
                        {
                            EmployeeProfileDetailExitEventValidationMessage = ExitDateValidationMessage;
                            return ExitDateValidationMessage;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                return null;
            }

        }


        #endregion

    }
}
