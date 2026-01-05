using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace Emdep.Geos.Data.Common
{

    [Table("employees")]
    [DataContract]
    public class Employee
    {
        #region Fields
        Int16 idEmployee;
        string corporateId;
        string photo;
        string firstName;
        string lastName;
        string corporateEmail;
        string corporatePhoneNumber;
        Int32 idDepartment;
        Int32 idJobDescription;
        DateTime? joinDate;
        DateTime? leaveDate;
        Int16 idSite;
        SByte isValidated;
        string phoneNumber;
        string phone2;
        string mobilePhone;
        Int32 userId;
        #endregion

        #region Properties
        [Key]
        [Column("IdEmployee")]
        [DataMember]
        public Int16 IdEmployee
        {
            get { return idEmployee; }
            set { idEmployee = value; }
        }

        [Column("CorporateId")]
        [DataMember]
        public string CorporateId
        {
            get { return corporateId; }
            set { corporateId = value; }
        }

        [Column("Photo")]
        [DataMember]
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        [Column("FirstName")]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Column("LastName")]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [Column("CorporateEmail")]
        [DataMember]
        public string CorporateEmail
        {
            get { return corporateEmail; }
            set { corporateEmail = value; }
        }

        [Column("CorporatePhoneNumber")]
        [DataMember]
        public string CorporatePhoneNumber
        {
            get { return CorporatePhoneNumber; }
            set { CorporatePhoneNumber = value; }
        }

        [Column("IdDepartment")]
        [DataMember]
        public int IdDepartment
        {
            get { return idDepartment; }
            set { idDepartment = value; }
        }

        [Column("IdJobDescription")]
        [DataMember]
        public int IdJobDescription
        {
            get { return idJobDescription; }
            set { idJobDescription = value; }
        }

        [Column("JoinDate")]
        [DataMember]
        public DateTime? JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        [Column("LeaveDate")]
        [DataMember]
        public DateTime? LeaveDate
        {
            get { return leaveDate; }
            set { leaveDate = value; }
        }

        [Column("IdSite")]
        [DataMember]
        public Int16 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [Column("IsValidated")]
        [DataMember]
        public sbyte IsValidated
        {
            get { return isValidated; }
            set { isValidated = value; }
        }

        #endregion
    }
}
