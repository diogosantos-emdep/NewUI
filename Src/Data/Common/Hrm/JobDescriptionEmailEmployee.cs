using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.jangra][GEOS2-3693]
    [DataContract]
    public class JobDescriptionEmailEmployee : ModelBase, IDisposable
    {
        #region Fields
        Int32 idUser;
        Int32 idEmployee;
        string firstName;
        string lastName;
        string companyEmail;

        #endregion

        #region Properties
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
            get { return FirstName + ' ' + LastName; }
            set { }
        }
        [DataMember]
        public string CompanyEmail
        {
            get { return companyEmail; }
            set
            {
                companyEmail = value;
                OnPropertyChanged("CompanyEmail");
            }
        }
        #endregion

        #region Constructor

        #endregion

        #region Methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
