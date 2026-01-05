using Emdep.Geos.Data.Common.Epc;
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
    public class EmployeeDocument : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeDocument;
        Int32 idEmployee;
        Employee employee;
        string employeeDocumentName;
        Int32 employeeDocumentIdType;
        string employeeDocumentNumber;
        DateTime? employeeDocumentIssueDate;
        DateTime? employeeDocumentExpiryDate;

        LookupValue employeeDocumentType;

        string employeeDocumentFileName;
        string employeeDocumentRemarks;

        Attachment attachment;
        string oldFileName;
        bool isEmployeeDocumentFileDeleted;
        byte[] employeeDocumentFileInBytes;
        Int32 idEmployeeStatus;
        bool isgreaterJobDescriptionthanToday;
        #endregion

        #region Constructor

        public EmployeeDocument()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeDocument")]
        [DataMember]
        public ulong IdEmployeeDocument
        {
            get { return idEmployeeDocument; }
            set
            {
                idEmployeeDocument = value;
                OnPropertyChanged("IdEmployeeDocument");
            }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("EmployeeDocumentName")]
        [DataMember]
        public string EmployeeDocumentName
        {
            get { return employeeDocumentName; }
            set
            {
                employeeDocumentName = value;
                OnPropertyChanged("EmployeeDocumentName");
            }
        }

        [Column("EmployeeDocumentIdType")]
        [DataMember]
        public int EmployeeDocumentIdType
        {
            get { return employeeDocumentIdType; }
            set
            {
                employeeDocumentIdType = value;
                OnPropertyChanged("EmployeeDocumentIdType");
            }
        }

        [Column("EmployeeDocumentNumber")]
        [DataMember]
        public string EmployeeDocumentNumber
        {
            get { return employeeDocumentNumber; }
            set
            {
                employeeDocumentNumber = value;
                OnPropertyChanged("EmployeeDocumentNumber");
            }
        }

        [Column("EmployeeDocumentIssueDate")]
        [DataMember]
        public DateTime? EmployeeDocumentIssueDate
        {
            get { return employeeDocumentIssueDate; }
            set
            {
                employeeDocumentIssueDate = value;
                OnPropertyChanged("EmployeeDocumentIssueDate");
            }
        }

        [Column("EmployeeDocumentExpiryDate")]
        [DataMember]
        public DateTime? EmployeeDocumentExpiryDate
        {
            get { return employeeDocumentExpiryDate; }
            set
            {
                employeeDocumentExpiryDate = value;
                OnPropertyChanged("EmployeeDocumentExpiryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue EmployeeDocumentType
        {
            get { return employeeDocumentType; }
            set
            {
                employeeDocumentType = value;
                OnPropertyChanged("EmployeeDocumentType");
            }
        }

        [Column("EmployeeDocumentFileName")]
        [DataMember]
        public string EmployeeDocumentFileName
        {
            get { return employeeDocumentFileName; }
            set
            {
                employeeDocumentFileName = value;
                OnPropertyChanged("EmployeeDocumentFileName");
            }
        }

        [Column("EmployeeDocumentRemarks")]
        [DataMember]
        public string EmployeeDocumentRemarks
        {
            get { return employeeDocumentRemarks; }
            set
            {
                employeeDocumentRemarks = value;
                OnPropertyChanged("EmployeeDocumentRemarks");
            }
        }

        [NotMapped]
        [DataMember]
        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged("Attachment");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] EmployeeDocumentFileInBytes
        {
            get { return employeeDocumentFileInBytes; }
            set
            {
                employeeDocumentFileInBytes = value;
                OnPropertyChanged("EmployeeDocumentFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEmployeeDocumentFileDeleted
        {
            get { return isEmployeeDocumentFileDeleted; }
            set
            {
                isEmployeeDocumentFileDeleted = value;
                OnPropertyChanged("IsEmployeeDocumentFileDeleted");
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
        public Int32 IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsgreaterJobDescriptionthanToday
        {
            get { return isgreaterJobDescriptionthanToday; }
            set
            {
                isgreaterJobDescriptionthanToday = value;
                OnPropertyChanged("IsgreaterJobDescriptionthanToday");
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
            EmployeeDocument employeeDocument = (EmployeeDocument)this.MemberwiseClone();

            if (employeeDocument.EmployeeDocumentType != null)
                employeeDocument.EmployeeDocumentType = (LookupValue)this.EmployeeDocumentType.Clone();

            if (employeeDocument.Attachment != null)
                employeeDocument.Attachment = (Attachment)this.Attachment.Clone();

            return employeeDocument;
        }

        #endregion
    }
}
