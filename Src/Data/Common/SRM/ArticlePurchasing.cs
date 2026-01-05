using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Emdep.Geos.Data.Common.SRM
{
	// [nsatpute][21-01-2025][GEOS2-5725]
    [DataContract]
    public class ArticlePurchasing : ModelBase, IDisposable
    {
        #region Declaration
        DateTime date;
        int idOrder;
        string order;
        string article;
        string supplier;
        string tradeName;
        string supplierReference;
        int poQuantity;
        int quantity;
        double price;
        double poValue;
        string warehouseDeliveryNoteCode;
        int idcurrency;
        string nameCurrPO;
        string nameCurrSelected;
        double exchangeRate11;
        string poWarehouse;
        int idWarehouse;
        double unitPrice;
        double discount;
        double exchangeRate;
        double transportCost;
        double importClearanceCost;
        double dutiesPercentage;
        double duties;
        double totalPriceWithNewCost;
        string warehouseItemDescription;
        string status;
        string curSymbol;
        string deliveryNote;
        string articleReceived;
        double totalPrice;
        string description;
        string manufacturer;
        string category;
        string country;
        DateTime lastReception;
        DateTime? latestPickingDate;
        string iso;
        string countryIconUrl;
        string warehouse;
        int quantityIn;
        int quantityOut;
        int totalIn;
        int totalOut;
        private string curSymbolPlant;//[pallavi.kale][GEOS2-9558][17.10.2025]

        #endregion

        #region Properties


        [DataMember]
        public int QuantityIn
        {
            get { return quantityIn; }
            set
            {
                quantityIn = value;
                OnPropertyChanged(nameof(QuantityIn));
            }
        }


        [DataMember]
        public int QuantityOut
        {
            get { return quantityOut; }
            set
            {
                quantityOut = value;
                OnPropertyChanged(nameof(QuantityOut));
            }
        }


        [DataMember]
        public int TotalIn
        {
            get { return totalIn; }
            set
            {
                totalIn = value;
                OnPropertyChanged(nameof(TotalIn));
            }
        }

        [DataMember]
        public int TotalOut
        {
            get { return totalOut; }
            set
            {
                totalOut = value;
                OnPropertyChanged(nameof(TotalOut));
            }
        }

        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged(nameof(CountryIconUrl));
            }
        }


        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged(nameof(Iso));
            }
        }

        [DataMember]
        public DateTime LastReception
        {
            get { return lastReception; }
            set
            {
                lastReception = value;
                OnPropertyChanged(nameof(LastReception));
            }
        }

        [DataMember]
        public DateTime? LatestPickingDate
        {
            get { return latestPickingDate; }
            set
            {
                latestPickingDate = value;
                OnPropertyChanged(nameof(LatestPickingDate));
            }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));
            }
        }



        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }


        [DataMember]
        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }


        [DataMember]
        public string DeliveryNote
        {
            get { return deliveryNote; }
            set
            {
                deliveryNote = value;
                OnPropertyChanged(nameof(DeliveryNote));
            }
        }


        [DataMember]
        public string ArticleReceived
        {
            get { return articleReceived; }
            set
            {
                articleReceived = value;
                OnPropertyChanged(nameof(ArticleReceived));
            }
        }

        [DataMember]
        public int IdOrder
        {
            get { return idOrder; }
            set
            {
                idOrder = value;
                OnPropertyChanged(nameof(IdOrder));
            }
        }

        [DataMember]
        public string Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged(nameof(Order));
            }
        }

        [DataMember]
        public string Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged(nameof(Article));
            }
        }

        [DataMember]
        public string Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }

        [DataMember]
        public string TradeName
        {
            get { return tradeName; }
            set
            {
                tradeName = value;
                OnPropertyChanged(nameof(TradeName));
            }
        }

        [DataMember]
        public string SupplierReference
        {
            get { return supplierReference; }
            set
            {
                supplierReference = value;
                OnPropertyChanged(nameof(SupplierReference));
            }
        }

        [DataMember]
        public int PoQuantity
        {
            get { return poQuantity; }
            set
            {
                poQuantity = value;
                OnPropertyChanged(nameof(PoQuantity));
            }
        }

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        [DataMember]
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        [DataMember]
        public double PoValue
        {
            get { return poValue; }
            set
            {
                poValue = value;
                OnPropertyChanged(nameof(PoValue));
            }
        }

        [DataMember]
        public string WarehouseDeliveryNoteCode
        {
            get { return warehouseDeliveryNoteCode; }
            set
            {
                warehouseDeliveryNoteCode = value;
                OnPropertyChanged(nameof(WarehouseDeliveryNoteCode));
            }
        }

        [DataMember]
        public int IdCurrency
        {
            get { return idcurrency; }
            set
            {
                idcurrency = value;
                OnPropertyChanged(nameof(IdCurrency));
            }
        }

        [DataMember]
        public string NameCurrPO
        {
            get { return nameCurrPO; }
            set
            {
                nameCurrPO = value;
                OnPropertyChanged(nameof(NameCurrPO));
            }
        }

        [DataMember]
        public string NameCurrSelected
        {
            get { return nameCurrSelected; }
            set
            {
                nameCurrSelected = value;
                OnPropertyChanged(nameof(NameCurrSelected));
            }
        }

        [DataMember]
        public double ExchangeRate11
        {
            get { return exchangeRate11; }
            set
            {
                exchangeRate11 = value;
                OnPropertyChanged(nameof(ExchangeRate11));
            }
        }

        [DataMember]
        public string PoWarehouse
        {
            get { return poWarehouse; }
            set
            {
                poWarehouse = value;
                OnPropertyChanged(nameof(PoWarehouse));
            }
        }


        [DataMember]
        public string Warehouse
        {
            get { return warehouse; }
            set
            {
                warehouse = value;
                OnPropertyChanged(nameof(Warehouse));
            }
        }

        [DataMember]
        public int IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged(nameof(IdWarehouse));
            }
        }

        [DataMember]
        public double UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
            }
        }

        [DataMember]
        public double TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        [DataMember]
        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }

        [DataMember]
        public double ExchangeRate
        {
            get { return exchangeRate; }
            set
            {
                exchangeRate = value;
                OnPropertyChanged(nameof(ExchangeRate));
            }
        }

        [DataMember]
        public double TransportCost
        {
            get { return transportCost; }
            set
            {
                transportCost = value;
                OnPropertyChanged(nameof(TransportCost));
            }
        }

        [DataMember]
        public double ImportClearanceCost
        {
            get { return importClearanceCost; }
            set
            {
                importClearanceCost = value;
                OnPropertyChanged(nameof(ImportClearanceCost));
            }
        }

        [DataMember]
        public double DutiesPercentage
        {
            get { return dutiesPercentage; }
            set
            {
                dutiesPercentage = value;
                OnPropertyChanged(nameof(DutiesPercentage));
            }
        }

        [DataMember]
        public double Duties
        {
            get { return duties; }
            set
            {
                duties = value;
                OnPropertyChanged(nameof(Duties));
            }
        }

        [DataMember]
        public double TotalPriceWithNewCost
        {
            get { return totalPriceWithNewCost; }
            set
            {
                totalPriceWithNewCost = value;
                OnPropertyChanged(nameof(TotalPriceWithNewCost));
            }
        }

        [DataMember]
        public string WarehouseItemDescription
        {
            get { return warehouseItemDescription; }
            set
            {
                warehouseItemDescription = value;
                OnPropertyChanged(nameof(WarehouseItemDescription));
            }
        }
        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


        [DataMember]
        public string CurSymbol
        {
            get
            {
                return curSymbol;
            }

            set
            {
                curSymbol = value;
                OnPropertyChanged("CurSymbol");
            }
        }

        //[pallavi.kale][GEOS2-9558][17.10.2025]
        [NotMapped]
        [DataMember]
        public string CurSymbolPlant
        {
            get
            {
                return curSymbolPlant;
            }

            set
            {
                curSymbolPlant = value;
                OnPropertyChanged("CurSymbolPlant");
            }
        }
        #endregion

        #region Constructor

        public ArticlePurchasing()
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
