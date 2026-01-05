using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.TSM
{
 
    public class TSMUsers : ModelBase, IDisposable
    {
        //[GEOS2-5388][pallavi.kale][13.01.2025]
        #region Field
        private UInt32 idEmployee;
        private UInt32 idTechnicianUser;
        private UInt32 idCustomerApplication;
        private Int32 engineeringCustomerApplication;
        private string employeeCode;       
        private string fullName;
        UInt32 idGender;
        string country;
        LookupValue gender;   
        private string organization;     
        string region;
        byte[] photoInBytes;
        private string userName;
        string employeeCodeWithIdGender;
        List<LookupValue> permissions;
        List<int> idPermissions;
        private Country countryObj;//[GEOS2-6993][pallavi.kale][26.02.2025]
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
        public UInt32 IdTechnicianUser
        {
            get { return idTechnicianUser; }
            set
            {
                idTechnicianUser = value;
                OnPropertyChanged("IdTechnicianUser");
            }
        }

        [DataMember]
        public UInt32 IdCustomerApplication
        {
            get { return idCustomerApplication; }
            set
            {
                idCustomerApplication = value;
                OnPropertyChanged("IdCustomerApplication");
            }
        }

        [DataMember]
        public Int32 EngineeringCustomerApplication
        {
            get { return engineeringCustomerApplication; }
            set
            {
                engineeringCustomerApplication = value;
                OnPropertyChanged("EngineeringCustomerApplication");
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
        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged("Organization");
            }

        }
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

        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

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
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        [DataMember]
        public List<LookupValue> Permissions
        {
            get { return permissions; }
            set
            {
                permissions = value;
                OnPropertyChanged("Permissions");
            }
        }
        [DataMember]
        public List<int> IdPermissions
        {
            get { return idPermissions; }
            set
            {
                idPermissions = value;
                OnPropertyChanged("IdPermissions");
            }
        }
        //[GEOS2-6993][pallavi.kale][26.02.2025]
        [NotMapped]
        [DataMember]
        public Country CountryObj
        {
            get { return countryObj; }
            set
            {
                countryObj = value;
                OnPropertyChanged("CountryObj");
            }
        }
        #endregion

        #region Constructor
        public TSMUsers()
        {

        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return "X";
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override object Clone()
        {
            TSMUsers clone = (TSMUsers)this.MemberwiseClone();
            // If you have any deep copy fields, you need to handle them separately
            if (this.IdPermissions != null)
                clone.IdPermissions = new List<int>(this.IdPermissions);
            return clone;
        }
        #endregion
    }
}

