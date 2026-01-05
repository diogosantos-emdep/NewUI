using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMIncomingArticleStockPlanning : ModelBase, IDisposable
    {
        // [rani dhamankar][24-02-2025][GEOS2-6889]
        #region Field    
        //private Int32 idArticle;
        private string pono;
        private string supplier;
        private Int32 quantity;
        private DateTime deliveryDate;

        #endregion

        #region Property
         [DataMember]
        public string Pono
        {
            get
            {
                return pono;
            }

            set
            {
                pono = value;
                OnPropertyChanged("Pono");
            }
        }
        [DataMember]
        public string Supplier
        {
            get
            {
                return supplier;
            }

            set
            {
                supplier = value;
                OnPropertyChanged("Supplier");
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
        public ERMIncomingArticleStockPlanning()
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
