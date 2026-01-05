using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{
    public class VisibilityPerBU : ModelBase, IDisposable
    {
        #region Field
        private UInt32 idEmployee;
        private string employeeCode;
        private string firstName;
        private string lastName;
        UInt32 idGender;
        Int32 idOrganizationCode;
        private string idsActionPlanBuAccess;
        string organizationCode;
        int idEmployeeStatus;
        bool isTaskField;
        string fullName;
        string jobCode;
        private string jDScope;
        Int32 idJDScope;
        private string jobDescription;
        Int32 idJobDescription;
        string nativeName;
        string employeeContactMobile;
        string employeeContactEmail;
        string lengthOfService;
        DateTime? contractSituationStartDate;
        UInt64 idEmployeeContractSituation;
        List<VisibilityPerBU> finalEmployeeList;
        //private string tAU;
        //private string electricTestBoards;
        //private string engineering;
        //private string assembly;
        //private string advanced;
        private string contractSituation;
        Int32 idLookupBusinessUnit;
        Int32 createdBy;
        private string finalJobDescription;
        LookupValue gender;
        bool isUpdatedRow;
        private string businessUnits;
        Int32 idTAUBuAccess;
        Int32 idLookupElectricTestBoards;
        private string isVisible;
        List<EmployeeContractSituation> employeeContractSituations;//[shweta.thube][GEOS2-7241][27/05/2025]
        List<EmployeeExitEvent> employeeExitEvents; //[shweta.thube][GEOS2-7241][27/05/2025]
        private string employeeJobTitles; //[shweta.thube][GEOS2-7241][27/05/2025]
        private List<string> employeeJobCodes; //[shweta.thube][GEOS2-7241][27/05/2025]
        private string empJobCodes; //[shweta.thube][GEOS2-7241][27/05/2025]
        private string employeeContactCompanyMobiles;//[shweta.thube][GEOS2-7241]
        private List<string> employeeContactCompanyMobileList;//[shweta.thube][GEOS2-7241]
        private List<string> employeeContactCompanyEmailList;//[shweta.thube][GEOS2-7241]
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public UInt32 IdEmployee
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
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }
        [NotMapped]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [NotMapped]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        
        [NotMapped]
        [DataMember]
        public Int32 IdOrganizationCode
        {
            get { return idOrganizationCode; }
            set
            {
                idOrganizationCode = value;
                OnPropertyChanged("IdOrganizationCode");
            }
        }
        

        [NotMapped]
        [DataMember]
        public string IdsActionPlanBuAccess
        {
            get { return idsActionPlanBuAccess; }
            set
            {
                idsActionPlanBuAccess = value;
                OnPropertyChanged("IdsActionPlanBuAccess");
            }
        }
        [NotMapped]
        [DataMember]
        public string Organization
        {
            get { return organizationCode; }
            set
            {
                organizationCode = value;
                OnPropertyChanged("Organization");
            }
        }
        [NotMapped]
        [DataMember]
        public int IdEmployeeStatus
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
        public string JobCode
        {
            get { return jobCode; }
            set
            {
                jobCode = value;
                OnPropertyChanged("JobCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string JDScope
        {
            get { return jDScope; }
            set
            {
                jDScope = value;
                OnPropertyChanged("JDScope");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdJDScope
        {
            get { return idJDScope; }
            set
            {
                idJDScope = value;
                OnPropertyChanged("IdJDScope");
            }
        }

        [NotMapped]
        [DataMember]
        public string JobDescription
        {
            get { return jobDescription; }
            set
            {
                jobDescription = value;
                OnPropertyChanged("JobDescription");
            }
        }
        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public string NativeName
        {
            get { return nativeName; }
            set
            {
                nativeName = value;
                OnPropertyChanged("NativeName");
            }
        }
        [NotMapped]
        [DataMember]
        public string EmployeeContactMobile
        {
            get { return employeeContactMobile; }
            set
            {
                employeeContactMobile = value;
                OnPropertyChanged("EmployeeContactMobile");
            }
        }
        [NotMapped]
        [DataMember]
        public string EmployeeContactEmail
        {
            get { return employeeContactEmail; }
            set
            {
                employeeContactEmail = value;
                OnPropertyChanged("EmployeeContactEmail");
            }
        }
        [NotMapped]
        [DataMember]
        public string LengthOfService
        {
            get { return lengthOfService; }
            set
            {
                lengthOfService = value;
                OnPropertyChanged("LengthOfService");
            }
        }
        [NotMapped]
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
        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public List<VisibilityPerBU> FinalEmployeeList
        {
            get { return finalEmployeeList; }
            set
            {
                finalEmployeeList = value;
                OnPropertyChanged("FinalEmployeeList");
            }
        }
        //[NotMapped]
        //[DataMember]
        //public string TAU
        //{
        //    get { return tAU; }
        //    set
        //    {
        //        tAU = value;
        //        OnPropertyChanged("TAU");
        //    }
        //}
        //[NotMapped]
        //[DataMember]
        //public string ElectricTestBoards
        //{
        //    get { return electricTestBoards; }
        //    set
        //    {
        //        electricTestBoards = value;
        //        OnPropertyChanged("ElectricTestBoards");
        //    }
        //}
        //[NotMapped]
        //[DataMember]
        //public string Engineering
        //{
        //    get { return engineering; }
        //    set
        //    {
        //        engineering = value;
        //        OnPropertyChanged("Engineering");
        //    }
        //}
        //[NotMapped]
        //[DataMember]
        //public string Assembly
        //{
        //    get { return assembly; }
        //    set
        //    {
        //        assembly = value;
        //        OnPropertyChanged("Assembly");
        //    }
        //}
        //[NotMapped]
        //[DataMember]
        //public string Advanced
        //{
        //    get { return advanced; }
        //    set
        //    {
        //        advanced = value;
        //        OnPropertyChanged("Advanced");
        //    }
        //}
        [NotMapped]
        [DataMember]
        public string ContractSituation
        {
            get { return contractSituation; }
            set
            {
                contractSituation = value;
                OnPropertyChanged("ContractSituation");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdLookupBusinessUnit
        {
            get { return idLookupBusinessUnit; }
            set
            {
                idLookupBusinessUnit = value;
                OnPropertyChanged("IdLookupBusinessUnit");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [NotMapped]
        [DataMember]
        public string FinalJobDescription
        {
            get { return finalJobDescription; }
            set
            {
                finalJobDescription = value;
                OnPropertyChanged("FinalJobDescription");
            }
        }

        string profileImagePath;
        [DataMember]
        public string ProfileImagePath
        {
            get { return profileImagePath; }

            set
            {
                profileImagePath = value;
                OnPropertyChanged("ProfileImagePath");
            }
        }
        [Column("IdGender")]
        [DataMember]
        public UInt32 IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }
        [NotMapped]
        [DataMember]
        public LookupValue Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }
        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }
        [NotMapped]
        [DataMember]
        public string BusinessUnits
        {
            get { return businessUnits; }
            set
            {
                businessUnits = value;
                OnPropertyChanged("BusinessUnits");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdTAUBuAccess
        {
            get { return idTAUBuAccess; }
            set
            {
                idTAUBuAccess = value;
                OnPropertyChanged("IdTAUBuAccess");
            }
        }
        
        [NotMapped]
        [DataMember]
        public Int32 IdLookupElectricTestBoards
        {
            get { return idLookupElectricTestBoards; }
            set
            {
                idLookupElectricTestBoards = value;
                OnPropertyChanged("IdLookupElectricTestBoards");
            }
        }
        
        [NotMapped]
        [DataMember]
        public string IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        [NotMapped]
        [DataMember]
        public List<EmployeeContractSituation> EmployeeContractSituations
        {
            get { return employeeContractSituations; }
            set
            {
                employeeContractSituations = value;
                OnPropertyChanged("EmployeeContractSituations");
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        [NotMapped]
        [DataMember]
        public List<EmployeeExitEvent> EmployeeExitEvents
        {
            get { return employeeExitEvents; }
            set
            {
                employeeExitEvents = value;
                OnPropertyChanged("EmployeeExitEvents");
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        [NotMapped]
        [DataMember]
        public string EmployeeJobTitles
        {
            get { return employeeJobTitles; }
            set
            {
                employeeJobTitles = value;
                OnPropertyChanged("EmployeeJobTitles");
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        [NotMapped]
        [DataMember]
        public List<string> EmployeeJobCodes
        {
            get { return employeeJobCodes; }
            set
            {
                employeeJobCodes = value;
                OnPropertyChanged("EmployeeJobCodes");
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        [NotMapped]
        [DataMember]
        public string EmpJobCodes
        {
            get { return empJobCodes; }
            set
            {
                empJobCodes = value;
                OnPropertyChanged("EmpJobCodes");
            }
        }
        //[shweta.thube][GEOS2-7241]
        [NotMapped]
        [DataMember]
        public string EmployeeContactCompanyMobiles
        {
            get { return employeeContactCompanyMobiles; }
            set
            {
                employeeContactCompanyMobiles = value;
                OnPropertyChanged("EmployeeContactCompanyMobiles");
            }
        }
        //[shweta.thube][GEOS2-7241]
        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactCompanyMobileList
        {
            get { return employeeContactCompanyMobileList; }
            set
            {
                employeeContactCompanyMobileList = value;
                OnPropertyChanged("EmployeeContactCompanyMobileList");
            }
        }
        //[shweta.thube][GEOS2-7241]
        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactCompanyEmailList
        {
            get { return employeeContactCompanyEmailList; }
            set
            {
                employeeContactCompanyEmailList = value;
                OnPropertyChanged("EmployeeContactCompanyEmailList");
            }
        }
        #endregion

        #region Constructor
        public VisibilityPerBU()
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
