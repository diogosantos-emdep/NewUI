using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMOutgoingArticleStockPlanning : ModelBase, IDisposable
    {
        // [rani dhamankar][17-02-2025][GEOS2-6887]
        #region Field    
        //private Int32 idArticle;
        private string oTCode;
        private string description;
        private string status;
        private string customer;
        private Int32 itemNumber;
        private Int32 iDDrawing;
        private Int32 quantity;
        private DateTime deliveryDate;

        #endregion

        #region Property
         [DataMember]
        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }
        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public Int32 ItemNumber
        {
            get
            {
                return itemNumber;
            }

            set
            {
                itemNumber = value;
                OnPropertyChanged("ItemNumber");
            }
        }

        [DataMember]
        public Int32 IDDrawing
        {
            get
            {
                return iDDrawing;
            }

            set
            {
                iDDrawing = value;
                OnPropertyChanged("IDDrawing");
            }
        }

        [DataMember]
        public Int32 Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
       
        [DataMember]
        public DateTime DeliveryDate
        {
            get
            {
                return deliveryDate;
            }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }


        #endregion

        #region Constructor
        public ERMOutgoingArticleStockPlanning()
        {

        }
        #endregion


        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
