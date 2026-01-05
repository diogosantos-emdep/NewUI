using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class OtItemShippingDetail
    {
        #region Fields

        string poCode;
        string otCode;
        string item;
        string reference;
        string description;
        Int64 quantity;
        string observation;
        string expectedSupplierDelivery;
     
        #endregion

        #region Properties

        [DataMember]
        public string POCode
        {
            get { return poCode; }
            set { poCode = value; }
        }


        [DataMember]
        public string Item
        {
            get { return item; }
            set { item = value; }
        }

        [DataMember]
        public string OTCode
        {
            get { return otCode; }
            set { otCode = value; }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        [DataMember]
        public Int64 Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }


        [DataMember]
        public string Observation
        {
            get { return observation; }
            set { observation = value; }
        }

        [DataMember]
        public string ExpectedSupplierDelivery
        {
            get { return expectedSupplierDelivery; }
            set { expectedSupplierDelivery = value; }
        }

        #endregion





    }
}

