using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Data.Common.OTM
{
    //[ashish.malkhede][23-01-2025][GEOS2-6735]
    [Table("CustomerCountriesDetails")]
    [DataContract]
    public class CustomerCountriesDetails : ModelBase, IDisposable
    {
        #region Fields
        Int32 idCustomer;
        string customerName;
        byte idCustomerType;
        Int32 idsite;
        string site;
        Int32 idCountries;
        string countries;
        string region;
        sbyte isStillActive;
        string email;
        #endregion

        #region Constructor
        public CustomerCountriesDetails()
        {
            // this.Companies = new List<Company>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdCustomer")]
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }

        [DataMember]
        public Int32 Idsite
        {
            get { return idsite; }
            set { idsite = value; }
        }

        [Column("CustomerName")]
        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        [NotMapped]
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [NotMapped]
        [DataMember]
        public byte IdCustomerType
        {
            get { return idCustomerType; }
            set { idCustomerType = value; }
        }

        [NotMapped]
        [DataMember]
        public string Site
        {
            get { return site; }
            set { site = value; }
        }

        [DataMember]
        public Int32 IdCountries
        {
            get { return idCountries; }
            set { idCountries = value; }
        }
        [NotMapped]
        [DataMember]
        public string Countries
        {
            get { return countries; }
            set { countries = value; }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        [NotMapped]
        [DataMember]
        public sbyte IsStillActive
        {
            get { return isStillActive; }
            set { isStillActive = value; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
