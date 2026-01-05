using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class OrganizationHR : ModelBase,IDisposable
    {
        #region Fields
        Int32 idEmployee;
        string firstName;
        string lastName;
        string nativeName;
        string employeeContactValue;
        string gender;
        Int32 idSite;
        string country;
        int idCountry;
        string employeeCode;
        string plantName;
        #endregion

        #region Properties
        [Key]
       // [Column("IdEmployee")]
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

        //[Column("EmployeeCode")]
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


        //[Column("FirstName")]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        //[Column("LastName")]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [DataMember]
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

       // [Column("NativeName")]
        [DataMember]
        public string NativeName
        {
            get { return nativeName; }
            set { nativeName = value; OnPropertyChanged("NativeName"); }
        }

        //[Column("EmployeeContactValue")]
        [DataMember]
        public string EmployeeContactValue
        {
            get { return employeeContactValue; }
            set { employeeContactValue = value;
                OnPropertyChanged("EmployeeContactValue");
            }
        }
        
        //[Column("IdSite")]
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
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }

        //[Column("Country")]
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

        //[Column("IdCountry")]
        [DataMember]
        public int IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

       
        #endregion

        #region Constructor
        public OrganizationHR()
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
