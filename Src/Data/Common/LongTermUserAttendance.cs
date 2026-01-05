using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class LongTermUserAttendance : ModelBase, IDisposable
    {
        #region Fields
        Int32 idLongTermAbsentUser;
        DateTime accountingDate;
        //string firstName;
        //string employeeCode;
        //string lastName;
        //int idGender;
        //Int32 isLongTermAbsent;
        //string shortName;
        //Int32 idSite;
        //string country;
        //int idCountry;

        #endregion

        #region Constructor
        public LongTermUserAttendance()
        {

        }
        #endregion

        #region Properties
        [Key]
        // [Column("IdLongTermAbsentUser")]
        [DataMember]
        public Int32 IdLongTermAbsentUser
        {
            get
            {
                return idLongTermAbsentUser;
            }

            set
            {
                idLongTermAbsentUser = value;
                OnPropertyChanged("IdSalesUser");
            }
        }

        [DataMember]
        public DateTime AccountingDate
        {
            get
            {
                return accountingDate;
            }

            set
            {
                accountingDate = value;
                OnPropertyChanged("AccountingDate");
            }
        }
        /* [DataMember]
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
         public string ShortName
         {
             get { return shortName; }
             set
             {
                 shortName = value;
                 OnPropertyChanged("ShortName");
             }
         }

         [DataMember]
         public Int32 IsLongTermAbsent
         {
             get
             {
                 return isLongTermAbsent;
             }

             set
             {
                 isLongTermAbsent = value;
                 OnPropertyChanged("IsLongTermAbsent");
             }
         }


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
         public int IdCountry
         {
             get { return idCountry; }
             set
             {
                 idCountry = value;
                 OnPropertyChanged("IdCountry");
             }
         }
         */

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
