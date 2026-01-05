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
    public class ArticleForecast : ModelBase, IDisposable
    {
        #region Declaration
        Int64 id;
        string type;
        string code;
        string customerORSupplier;
        Int64 quantity;
        DateTime deliveryDate;
        #endregion

        #region Properties

        [DataMember]
        public Int64 Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

    
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

      
        [DataMember]
        public string CustomerORSupplier
        {
            get { return customerORSupplier; }
            set
            {
                customerORSupplier = value;
                OnPropertyChanged("CustomerORSupplier");
            }
        }

      
        [DataMember]
        public Int64 Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }


        [DataMember]
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }


        #endregion

        #region Constructor

        public ArticleForecast()
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
