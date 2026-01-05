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
    [Table("supplier")]
    [DataContract]
    public class Supplier 
    {
        #region Fields
        int idSupplier;
        int? idCompany;
        string supplierName;
        #endregion

        #region Properties
        [Key]
        [Column("idSupplier")]
        [DataMember]
        public int IdSupplier
        {
            get { return idSupplier; }
            set { idSupplier = value; }
        }

        [Column("IdCompany")]
        [ForeignKey("Company")]
        [DataMember]
        public int? IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        [Column("SupplierName")]
        [DataMember]
        public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        [DataMember]
        public virtual Company Company { get; set; }
        #endregion
    }
}
