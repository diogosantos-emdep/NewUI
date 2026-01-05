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
    
    [DataContract]
    public class EmployeeShortDetailForMail : ModelBase, IDisposable
    {

        #region Fields

        Int32 idEmployee;
        string fullName;
        DateTime contractStartDate;
        DateTime exitDate;
        string companyAlias;
        List<string> emailidsList;
        Int32 idDepartment;
        Int32 idCountry;
        Int32 idRegion;
        bool isJoin;
        string position;
        string employeeCode;
        byte[] profileImageInBytes;
        Int32 idGender;
        string employeeDepatment;
        #endregion

        #region Properties


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
        public DateTime ContractStartDate
        {
            get { return contractStartDate; }
            set
            {
                contractStartDate = value;
                OnPropertyChanged("ContractStartDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime ExitDate
        {
            get { return exitDate; }
            set
            {
               exitDate = value;
                OnPropertyChanged("ExitDate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<string> EmailidsList
        {
            get { return emailidsList; }
            set
            {
                emailidsList = value;
                OnPropertyChanged("EmailidsList");
            }
        }

        [NotMapped]
        [DataMember]
        public string CompanyAlias
        {
            get { return companyAlias; }
            set
            {
                companyAlias = value;
                OnPropertyChanged("companyAlias");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsJoin
        {
            get { return isJoin; }
            set
            {
                isJoin = value;
                OnPropertyChanged("IsJoin");
            }
        }

        [DataMember]
        public int IdDepartment
        {
            get
            {
                return idDepartment;
            }

            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }

        [DataMember]
        public int IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [DataMember]
        public int IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [DataMember]
        public string Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [DataMember]
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }

            set
            {
               employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        
        [DataMember]
        public byte[] ProfileImageInBytes
        {
            get { return profileImageInBytes; }

            set
            {
                profileImageInBytes = value;
                OnPropertyChanged("ProfileImageInBytes");
            }
        }


        [DataMember]
        public Int32 IdGender
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
        public string EmployeeDepatment
        {
            get { return employeeDepatment; }
            set
            {
                employeeDepatment = value;
                OnPropertyChanged("EmployeeDepatment");
            }
        }
        #endregion

        #region Constructor

        public EmployeeShortDetailForMail()
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
