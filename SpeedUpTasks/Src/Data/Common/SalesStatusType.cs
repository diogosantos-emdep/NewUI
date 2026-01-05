using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("sales_status_types")]
    [DataContract]
    public class SalesStatusType : ModelBase, IDisposable
    {

        #region  Fields
        Int64 idSalesStatusType;
        string name;
        Int64? idImage;
        string htmlColor;
        IList<Offer> offers;
        object offersInObject;
        Int64 idSalesStatus;
        double totalAmount;
        People salesOwner;
        GeosStatus geosStatus;
        Int64 numberOfOffers;
        Currency currency;
        #endregion

        #region Constructor
        public SalesStatusType()
        {
           
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdSalesStatusType")]
        [DataMember]
        public Int64 IdSalesStatusType
        {
            get
            {
                return idSalesStatusType;
            }

            set
            {
                idSalesStatusType = value;
                OnPropertyChanged("IdSalesStatusType");
            }
        }

        [Column("IdImage")]
        [DataMember]
        public Int64? IdImage
        {
            get
            {
                return idImage;
            }
            set
            {
                idImage = value;
                OnPropertyChanged("IdImage");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [NotMapped]
        [DataMember]
        public object OffersInObject
        {
            get
            {
                return offersInObject;
            }
            set
            {
                offersInObject = value;
                OnPropertyChanged("OffersInObject");
            }
        }

        [NotMapped]
        [DataMember]
        public Currency Currency
        {
            get
            {
                return currency;
            }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual IList<Offer> Offers
        {
            get
            {
                return offers;
            }
            set
            {
                offers = value;
                OnPropertyChanged("Offers");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 NumberOfOffers
        {
            get
            {
                return numberOfOffers;
            }
            set
            {
                numberOfOffers = value;
                OnPropertyChanged("NumberOfOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdSalesStatus
        {
            get
            {
                return idSalesStatus;
            }

            set
            {
                idSalesStatus = value;
                OnPropertyChanged("IdSalesStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public double TotalAmount
        {
            get
            {
                return totalAmount;
            }

            set
            {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public People SalesOwner
        {
            get
            {
                return salesOwner;
            }

            set
            {
                salesOwner = value;
                OnPropertyChanged("SalesOwner");
            }
        }

        [NotMapped]
        [DataMember]
        public GeosStatus GeosStatus
        {
            get
            {
                return geosStatus;
            }

            set
            {
                geosStatus = value;
                OnPropertyChanged("GeosStatus");
            }
        }

        

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
