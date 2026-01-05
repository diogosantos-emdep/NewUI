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
    [Table("employee_contract_situations")]
    [DataContract]
    public class EmployeeContractSituation : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeContractSituation;
        Int32 idEmployee;

        UInt16 idContractSituation;
        ContractSituation contractSituation;

        UInt16 idProfessionalCategory;
        ProfessionalCategory professionalCategory;

        DateTime? contractSituationStartDate;
        DateTime? contractSituationEndDate;

        string contractSituationFileName;
        string contractSituationRemarks;

        Attachment attachment;
        byte[] contractSituationFileInBytes;
        bool isContractSituationFileDeleted;
        string oldFileName;

        Employee employee;
        Int32? idCompany;
        Company company;
        Int64? idEmployeeExitEvent;
        EmployeeExitEvent employeeExitEvent;
        DateTime? contractExpirationWarningDate;
        Int32 idSerialNo;

        Int32 idEmployeeStatus;
        bool isgreaterJobDescriptionthanToday;
        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeContractSituation")]
        [DataMember]
        public ulong IdEmployeeContractSituation
        {
            get { return idEmployeeContractSituation; }
            set
            {
                idEmployeeContractSituation = value;
                OnPropertyChanged("IdEmployeeContractSituation");
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

        [Column("IdContractSituation")]
        [DataMember]
        public ushort IdContractSituation
        {
            get { return idContractSituation; }
            set
            {
                idContractSituation = value;
                OnPropertyChanged("IdContractSituation");
            }
        }

        [NotMapped]
        [DataMember]
        public ContractSituation ContractSituation
        {
            get { return contractSituation; }
            set
            {
                contractSituation = value;
                OnPropertyChanged("ContractSituation");
            }
        }

        [Column("IdProfessionalCategory")]
        [DataMember]
        public ushort IdProfessionalCategory
        {
            get { return idProfessionalCategory; }
            set
            {
                idProfessionalCategory = value;
                OnPropertyChanged("IdProfessionalCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public ProfessionalCategory ProfessionalCategory
        {
            get { return professionalCategory; }
            set
            {
                professionalCategory = value;
                OnPropertyChanged("ProfessionalCategory");
            }
        }


        [Column("ContractSituationStartDate")]
        [DataMember]
        public DateTime? ContractSituationStartDate
        {
            get { return contractSituationStartDate; }
            set
            {
                contractSituationStartDate = value;
                OnPropertyChanged("ContractSituationStartDate");
            }
        }

        [Column("ContractSituationEndDate")]
        [DataMember]
        public DateTime? ContractSituationEndDate
        {
            get { return contractSituationEndDate; }
            set
            {
                contractSituationEndDate = value;
                OnPropertyChanged("ContractSituationEndDate");
            }
        }

        [Column("ContractSituationFileName")]
        [DataMember]
        public string ContractSituationFileName
        {
            get { return contractSituationFileName; }
            set
            {
                contractSituationFileName = value;
                OnPropertyChanged("ContractSituationFileName");
            }
        }

        [Column("ContractSituationRemarks")]
        [DataMember]
        public string ContractSituationRemarks
        {
            get { return contractSituationRemarks; }
            set
            {
                contractSituationRemarks = value;
                OnPropertyChanged("ContractSituationRemarks");
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
        public byte[] ContractSituationFileInBytes
        {
            get { return contractSituationFileInBytes; }
            set
            {
                contractSituationFileInBytes = value;
                OnPropertyChanged("ContractSituationFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsContractSituationFileDeleted
        {
            get { return isContractSituationFileDeleted; }
            set
            {
                isContractSituationFileDeleted = value;
                OnPropertyChanged("IsContractSituationFileDeleted");
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


        [Column("IdCompany")]
        [DataMember]
        public Int32? IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSerialNo
        {
            get { return idSerialNo; }
            set
            {
                idSerialNo = value;
                OnPropertyChanged("IdSerialNo");
            }
        }


        [Column("IdEmployeeExitEvent")]
        [DataMember]
        public Int64? IdEmployeeExitEvent
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
        public EmployeeExitEvent EmployeeExitEvent
        {
            get { return employeeExitEvent; }
            set
            {
                employeeExitEvent = value;
                OnPropertyChanged("EmployeeExitEvent");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime? ContractExpirationWarningDate
        {
            get { return contractExpirationWarningDate; }
            set
            {
                contractExpirationWarningDate = value;
                OnPropertyChanged("ContractExpirationWarningDate");
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

        #region Constructor

        public EmployeeContractSituation()
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
            EmployeeContractSituation employeeContractSituation = (EmployeeContractSituation)this.MemberwiseClone();

            if (employeeContractSituation.ContractSituation != null)
                employeeContractSituation.ContractSituation = (ContractSituation)this.ContractSituation.Clone();

            if (employeeContractSituation.ProfessionalCategory != null)
                employeeContractSituation.ProfessionalCategory = (ProfessionalCategory)this.ProfessionalCategory.Clone();

            if (employeeContractSituation.Attachment != null)
                employeeContractSituation.Attachment = (Attachment)this.Attachment.Clone();

            return employeeContractSituation;
        }

        #endregion
    }
}
