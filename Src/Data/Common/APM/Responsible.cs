using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.APM
{//[Sudhir.Jangra][GEOS2-5977]
    public class Responsible : ModelBase, IDisposable
    {
        #region Field
        private UInt32 idEmployee;
        private string employeeCode;
        private string firstName;
        private string lastName;
        private int idGender;
        private Int32 idCompanyShift;
        Currency currency;
        Int32 idOrganizationCode;
        string organizationCode;
        Int32 idOrganizationRegion;
        string organizationRegion;
        int idEmployeeStatus;
        bool isInActive;
        bool isTaskField;
        string fullName;
        string employeeDepartments;
        string jobCode;
        ImageSource ownerImage;
        byte[] imageInBytes;//[Sudhir.Jangra][GEOS2-6397]
        private string employeeCodeWithIdGender;//[Sudhir.Jangra][GEOs2-6017]
        private Int32 idUser;//[Sudhir.Jangra][GEOS2-6787]
        private string userName;//[Sudhir.Jangra][GEOS2-6787]
        private string responsibleDisplayName;//[Sudhir.Jangra][GEOS2-6897]
        private string jobDescriptionTitle;//[Shweta.Thube][GEOS2-7008]
        private Int32 idTask;//[Shweta.Thube][GEOS2-7008]
        #endregion

        #region Properties
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
        [DataMember]
        public bool IsInActive
        {
            get { return isInActive; }
            set
            {
                isInActive = value;
                OnPropertyChanged("IdEmployee");
            }
        }
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
        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }

        [DataMember]
        public Int32 IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public Currency Currency
        {
            get
            {
                return currency;
            }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

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

        [DataMember]
        public Int32 IdOrganizationRegion
        {
            get { return idOrganizationRegion; }
            set
            {
                idOrganizationRegion = value;
                OnPropertyChanged("IdOrganizationRegion");
            }
        }

        [DataMember]
        public string OrganizationRegion
        {
            get { return organizationRegion; }
            set
            {
                organizationRegion = value;
                OnPropertyChanged("OrganizationRegion");
            }
        }

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
        public bool IsTaskField
        {
            get { return isTaskField; }
            set
            {
                isTaskField = value;
                OnPropertyChanged("IsTaskField");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDepartments
        {
            get { return employeeDepartments; }
            set
            {
                employeeDepartments = value;
                OnPropertyChanged("EmployeeDepartments");
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
        public ImageSource OwnerImage
        {
            get { return ownerImage; }
            set
            {
                ownerImage = value;
                OnPropertyChanged("OwnerImage");
            }
        }

        //[Sudhir.Jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public byte[] ImageInBytes
        {
            get { return imageInBytes; }
            set
            {
                imageInBytes = value;
                OnPropertyChanged("ImageInBytes");
            }
        }

        //[Sudhir.Jangra][GEOs2-6017]
        [NotMapped]
        [DataMember]
        public string EmployeeCodeWithIdGender
        {
            get { return employeeCodeWithIdGender; }
            set
            {
                employeeCodeWithIdGender = value;
                OnPropertyChanged("EmployeeCodeWithIdGender");
            }
        }

        //[Sudhir.Jangra][GEOS2-6787]
        [NotMapped]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }
        //[Sudhir.Jangra][GEOS2-6787]
        [NotMapped]
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        //[Sudhir.Jangra][GEOS2-6897]
        [NotMapped]
        [DataMember]
        public string ResponsibleDisplayName
        {
            get { return responsibleDisplayName; }
            set
            {
                responsibleDisplayName = value;
                OnPropertyChanged("ResponsibleDisplayName");
            }
        }

        [NotMapped]
        [DataMember]
        public string JobDescriptionTitle
        {
            get { return jobDescriptionTitle; }
            set
            {
                jobDescriptionTitle = value;
                OnPropertyChanged("JobDescriptionTitle");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdTask
        {
            get { return idTask; }
            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
            }
        }
        #endregion

        #region Constructor
        public Responsible()
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
