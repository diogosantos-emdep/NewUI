using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SRM
{
    // [rushikesh.gaikwad][05.06.2024][GEOS2-5463]
    [DataContract]
    public class Catalogue : ModelBase, IDisposable
    {
        #region Fields

        string reference;
        string supplierReference;
        string supplierName;
        string imagePath;
        string description;
        string category;
        string location;
        decimal weight;
        float lenght;
        float width;
        float height;
        Int32 stock;
        Decimal _value;
        Decimal costPrice;
        string costCurrency;
        DateTime? latestdeliverydate;
        Int32 minimumStock;
        Int32 maximumStock;
        string isObsolete;
        string registerSerialNumber;
        private ImageSource referenceImage;

        #region Properties

        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public string SupplierReference
        {
            get
            {
                return supplierReference;
            }
            set
            {
                supplierReference = value;
                OnPropertyChanged("SupplierReference");
            }
        }

        [DataMember]
        public string SupplierName
        {
            get
            {
                return supplierName;
            }
            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        [DataMember]
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        public byte[] articleImageInBytes;
        [DataMember]
        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
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
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [DataMember]
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [DataMember]
        public decimal Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        [DataMember]
        public float Length
        {
            get
            {
                return lenght;
            }

            set
            {
                lenght = value;
                OnPropertyChanged("Lenght");
            }
        }
        [DataMember]
        public float Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [DataMember]
        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [DataMember]
        public Int32 Stock
        {
            get
            {
                return stock;
            }

            set
            {
                stock = value;
                OnPropertyChanged("Stock");
            }
        }

        [DataMember]
        public Decimal Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        private string priceWithSelectedCurrencySymbol;
        [DataMember]
        public string PriceWithSelectedCurrencySymbol
        {
            get { return priceWithSelectedCurrencySymbol; }
            set { priceWithSelectedCurrencySymbol = value; OnPropertyChanged("PriceWithSelectedCurrencySymbol"); }
        }
        

        [DataMember]
        public string CostWithDecimal
        {
            get { return CostPrice.ToString("n2"); }
            set {  OnPropertyChanged("CostWithDecimal"); }
        }


        [DataMember]
        public Decimal CostPrice
        {
            get
            {
                return costPrice;
            }

            set
            {
                costPrice = value;
                OnPropertyChanged("CostPrice");
            }
        }

        [DataMember]
        public string CostCurrency
        {
            get
            {
                return costCurrency;
            }

            set
            {
                costCurrency = value;
                OnPropertyChanged("CostCurrency");
            }
        }
        [DataMember]
        public DateTime? LatestDeliveryDate
        {
            get
            {
                return latestdeliverydate; ;
            }

            set
            {
                latestdeliverydate = value;
                OnPropertyChanged("LatestDeliveryDate");
            }
        }

        [DataMember]
        public Int32 MinimumStock
        {
            get
            {
                return minimumStock;
            }

            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        [DataMember]
        public Int32 MaximumStock
        {
            get
            {
                return maximumStock;
            }
            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        [DataMember]
        public string IsObsolete
        {
            get
            {
                return isObsolete;
            }

            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }

        [DataMember]
        public string RegisterSerialNumber
        {
            get
            {
                return registerSerialNumber;
            }

            set
            {
                registerSerialNumber = value;
                OnPropertyChanged("RegisterSerialNumber");
            }
        }
        [DataMember]
        public ImageSource ReferenceImage
        {
            get { return referenceImage; }
            set
            {
                referenceImage = value;
                OnPropertyChanged("ReferenceImage");
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
#endregion