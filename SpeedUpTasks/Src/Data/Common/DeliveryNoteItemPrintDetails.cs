using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Drawing;

namespace Emdep.Geos.Data.Common
{
    public class DeliveryNoteItemPrintDetails : ModelBase, IDisposable
    {
        #region Fields

        string name;
        string supplierName;
        string producerName;
        string code;
        string barcode;
        string reference;
        string madeIn;
        string tradeGroup;
        string zone;

        Int64 quantity;
        Image headerImage;
        Image barcodeImage;
        DateTime deliveryNoteDate;
        string deliveryNoteDateString;

        #endregion

        #region Constructor
        public DeliveryNoteItemPrintDetails()
        {
        }
        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string SupplierName
        {
            get { return supplierName; }
            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        public string ProducerName
        {
            get { return producerName; }
            set
            {
                producerName = value;
                OnPropertyChanged("ProducerName");
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        public string MadeIn
        {
            get { return madeIn; }
            set
            {
                madeIn = value;
                OnPropertyChanged("MadeIn");
            }
        }

        public string TradeGroup
        {
            get { return tradeGroup; }
            set
            {
                tradeGroup = value;
                OnPropertyChanged("TradeGroup");
            }
        }

        public string Zone
        {
            get { return zone; }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }

        public long Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public Image HeaderImage
        {
            get
            {
                return headerImage;
            }

            set
            {
                headerImage = value;
                OnPropertyChanged("HeaderImage");
            }
        }

        public Image BarcodeImage
        {
            get { return barcodeImage; }
            set
            {
                barcodeImage = value;
                OnPropertyChanged("BarcodeImgage");
            }
        }

        public DateTime DeliveryNoteDate
        {
            get { return deliveryNoteDate; }
            set
            {
                deliveryNoteDate = value;
                OnPropertyChanged("DeliveryNoteDate");
            }
        }

        public string Barcode
        {
            get
            {
                return barcode;
            }

            set
            {
                barcode = value;
                OnPropertyChanged("Barcode");
            }
        }

        public string DeliveryNoteDateString
        {
            get
            {
                return deliveryNoteDateString;
            }

            set
            {
                deliveryNoteDateString = value;
                OnPropertyChanged("DeliveryNoteDateString");
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
