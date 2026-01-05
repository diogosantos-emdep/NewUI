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
    [Table("users")]
    [DataContract]
    public class EmployeeContact
    {
        #region Fields
        int idEmployeeContact;
        int idEmployee;
        int phoneNumber;
        int address;
        int city;
        int zipCode;
        int region;
        int idCountry;
        string phone2;
        string mobilePhone;
        Int32 idUser;
        #endregion

        #region Properties
        [Key]
        [Column("IdEmployeeContact")]
        [DataMember]
        public int IdEmployeeContact
        {
            get { return idEmployeeContact; }
            set { idEmployeeContact = value; }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set { idEmployee = value; }
        }

        [Column("PhoneNumber")]
        [DataMember]
        public int PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [Column("Address")]
        [DataMember]
        public int Address
        {
            get { return address; }
            set { address = value; }
        }

        [Column("City")]
        [DataMember]
        public int City
        {
            get { return city; }
            set { city = value; }
        }

        [Column("ZipCode")]
        [DataMember]
        public int ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        [Column("Region")]
        [DataMember]
        public int Region
        {
            get { return region; }
            set { region = value; }
        }

        [Column("IdCountry")]
        [DataMember]
        public int IdCountry
        {
            get { return idCountry; }
            set { idCountry = value; }
        }

        [Column("Phone2")]
        [DataMember]
        public string Phone2
        {
            get { return phone2; }
            set { phone2 = value; }
        }

        [Column("MobilePhone")]
        [DataMember]
        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        [Column("Iduser")]
        [ForeignKey("User")]
        [DataMember]
        public Int32 Iduser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [DataMember]
        public virtual User User { get; set; }
        #endregion
    }
}
