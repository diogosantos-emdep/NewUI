using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("customerpurchaseordersbyoffer")]
    [DataContract]
   public class CustomerPurchaseOrdersByOffer : ModelBase, IDisposable
    {
        #region Fields
        Int64 idCustomerPurchaseOrder;
        Int64 idOffer;
        byte emailSent;
        string comments;
        Offer offer;
        CustomerPurchaseOrder customerPurchaseOrder;
        #endregion

        #region Constructor
        public CustomerPurchaseOrdersByOffer()
        {
            
        }
        #endregion

        #region Properties
        [Key]
        [Column("idCustomerPurchaseOrder")]
        [DataMember]
        public Int64 IdCustomerPurchaseOrder
        {
            get
            {
                return idCustomerPurchaseOrder;
            }

            set
            {
                idCustomerPurchaseOrder = value;
                OnPropertyChanged("IdCustomerPurchaseOrder");
            }
        }


        [Column("idOffer")]
        [ForeignKey("Offer")]
        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("emailSent")]
        [DataMember]
        public byte EmailSent
        {
            get
            {
                return emailSent;
            }

            set
            {
                emailSent = value;
                OnPropertyChanged("EmailSent");
            }
        }


        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

      
        [DataMember]
        public virtual Offer Offer
        {
            get
            {
                return offer;
            }

            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual CustomerPurchaseOrder CustomerPurchaseOrder
        {
            get
            {
                return customerPurchaseOrder;
            }

            set
            {
                customerPurchaseOrder = value;
                OnPropertyChanged("CustomerPurchaseOrder");
            }
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
