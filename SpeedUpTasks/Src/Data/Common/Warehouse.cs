using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouses")]
    [DataContract]
    public class Warehouses : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouse;
        string name;
        Int64 idSite;
        Company company;
        byte idCurrency;
        Int64 idArticleSupplier;
        Int64? idTransitLocation;
        byte isRegional;
        #endregion

        #region Properties

        [Key]
        [Column("IdWarehouse")]
        [DataMember]
        public long IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public long IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("IdArticleSupplier")]
        [DataMember]
        public long IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [Column("IdTransitLocation")]
        [DataMember]
        public Int64? IdTransitLocation
        {
            get { return idTransitLocation; }
            set
            {
                idTransitLocation = value;
                OnPropertyChanged("IdTransitLocation");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsRegional
        {
            get { return isRegional; }
            set
            {
                isRegional = value;
                OnPropertyChanged("IsRegional");
            }
        }

        #endregion

        #region Constructor
        public Warehouses()
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
