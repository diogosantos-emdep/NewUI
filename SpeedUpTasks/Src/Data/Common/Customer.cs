using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("customer")]
    [DataContract]
    public class Customer
    {
        #region Fields
        Int32 idCustomer;
        Int32? idCompany;
        string customerName;
        byte idCustomerType;
        string logo;
        string patternForConnectorReferences;
        string web;
        sbyte isStillActive;
        string email;
        List<Company> companies;
        List<CustomerSort> customerSorts;
        #endregion

        #region Constructor
        public Customer()
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

        [Column("IdCompany")]
        [ForeignKey("Company")]
        [DataMember]
        public Int32? IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
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
        public string Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        [NotMapped]
        [DataMember]
        public string PatternForConnectorReferences
        {
            get { return patternForConnectorReferences; }
            set { patternForConnectorReferences = value; }
        }

        [NotMapped]
        [DataMember]
        public string Web
        {
            get { return web; }
            set { web = value; }
        }

        [NotMapped]
        [DataMember]
        public sbyte IsStillActive
        {
            get { return isStillActive; }
            set { isStillActive = value; }
        }

        [NotMapped]
        [DataMember]
        public List<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        [NotMapped]
        [DataMember]
        public List<CustomerSort> CustomerSorts
        {
            get { return customerSorts; }
            set { customerSorts = value; }
        }

        [DataMember]
        public virtual Company Company { get; set; }

      
        #endregion
    }
}
