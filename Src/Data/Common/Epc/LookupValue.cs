using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("lookup_values")]
    [DataContract(IsReference = true)]
    public class LookupValue : ModelBase, IDisposable
    {
        #region  Fields

        Int32 idLookupValue;
        string value;
        string htmlColor;
        LookupKey lookupKey;
        byte idLookupKey;
        Int64? idImage;
        string imageName;
        byte[] imageData;
        Int32? position;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        double? percentage;
        List<SalesStatusType> salesStatusTypes;
        object tag;
        bool inUse;
        DateTime? exchangeRateDate;
        double salesQuotaAmountWithExchangeRate;
        UInt64 count;
        double? average;
        decimal countValue;
        string abbreviation;
        List<String> lookupValueImages;

        Int32 idRegion;
        string region;
        double currencyConversionRate;
        double convertedAmount;
        int days;
        decimal hours;
        bool isUpdatedRow;
        Int32? idParent;
        string categoryName;
        Int32? idParentNew;
        string categoryType;//[Sudhir.Jangra][GEOS2-5549]
        string equipmentType;//[Sudhir.Jangra][GEOS2-5549]
        List<TemplateTag> tagValueList;
        int idTemplateSettings;
        int idEmployee;
        //[Rahul.Gadhave][GEOS2-6542][Date:27-11-2024]
        private bool isEnabled = true; // Default value
        //[Rahul.Gadhave][GEOS2-8307][24-06-2025]
        byte[] imageInBytes;
        bool isSelected; //[nsatpute][31.10.2025][GEOS2-8801]
        #endregion

        #region Properties

        [Key]
        [Column("IdLookupValue")]
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }

        [Column("Value")]
        [DataMember]
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [Column("IdLookupKey")]
        [ForeignKey("LookupKey")]
        [DataMember]
        public byte IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged("LookupKey");
            }
        }

        [Column("IdImage")]
        [DataMember]
        public Int64? IdImage
        {
            get { return idImage; }
            set
            {
                idImage = value;
                OnPropertyChanged("IdImage");
            }
        }

        [Column("ImageName")]
        [DataMember]
        public string ImageName
        {
            get { return imageName; }
            set
            {
                imageName = value;
                OnPropertyChanged("ImageName");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] ImageData
        {
            get { return imageData; }
            set
            {
                imageData = value;
                OnPropertyChanged("ImageData");
            }
        }

        [Column("Position")]
        [DataMember]
        public Int32? Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [NotMapped]
        [DataMember]
        public Double SalesQuotaAmount
        {
            get { return salesQuotaAmount; }
            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged("SalesQuotaAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public Double SalesQuotaAmountWithExchangeRate
        {
            get { return salesQuotaAmountWithExchangeRate; }
            set
            {
                salesQuotaAmountWithExchangeRate = value;
                OnPropertyChanged("SalesQuotaAmountWithExchangeRate");
            }
        }

        [NotMapped]
        [DataMember]
        public object Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ExchangeRateDate
        {
            get { return exchangeRateDate; }
            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdSalesQuotaCurrency
        {
            get { return idSalesQuotaCurrency; }
            set
            {
                idSalesQuotaCurrency = value;
                OnPropertyChanged("IdSalesQuotaCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SalesStatusType> SalesStatusTypes
        {
            get { return salesStatusTypes; }
            set
            {
                salesStatusTypes = value;
                OnPropertyChanged("SalesStatusTypes");
            }
        }

        [DataMember]
        public virtual LookupKey LookupKey
        {
            get { return lookupKey; }
            set
            {
                lookupKey = value;
                OnPropertyChanged("LookupKey");
            }
        }

        [Column("InUse")]
        [DataMember]
        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Average
        {
            get { return average; }
            set
            {
                average = value;
                OnPropertyChanged("Average");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal CountValue
        {
            get { return countValue; }
            set
            {
                countValue = value;
                OnPropertyChanged("CountValue");
            }
        }

        [Column("Abbreviation")]
        [DataMember]
        public string Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }

        [NotMapped]
        [DataMember]
        public List<String> LookupValueImages
        {
            get { return lookupValueImages; }
            set
            {
                lookupValueImages = value;
                OnPropertyChanged("LookupValueImages");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [NotMapped]
        [DataMember]
        public double CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
            }
        }

        [NotMapped]
        [DataMember]
        public double ConvertedAmount
        {
            get
            {
                return convertedAmount;
            }

            set
            {
                convertedAmount = value;
                OnPropertyChanged("ConvertedAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public int Days
        {
            get
            {
                return days;
            }

            set
            {
                days = value;
                OnPropertyChanged("Days");
            }
        }


        [NotMapped]
        [DataMember]
        public decimal Hours
        {
            get
            {
                return hours;
            }

            set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [NotMapped]
        [DataMember]
        public string CategoryName
        {
            get
            {
                return categoryName;
            }

            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdParent
        {
            get
            {
                return idParent;
            }

            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }


        [Column("IdParent")]
        [DataMember]
        public Int32? IdParentNew
        {
            get
            {
                return idParentNew;
            }

            set
            {
                idParentNew = value;
                OnPropertyChanged("IdParentNew");
            }
        }

        [NotMapped]
        [DataMember]
        public string CategoryType
        {
            get { return categoryType; }
            set
            {
                categoryType = value;
                OnPropertyChanged("CategoryType");
            }
        }

        [NotMapped]
        [DataMember]
        public string EquipmentType
        {
            get { return equipmentType; }
            set
            {
                equipmentType = value;
                OnPropertyChanged("EquipmentType");
            }
        }

        [NotMapped]
        [DataMember]
        public List<TemplateTag> TagValueList
        {
            get { return tagValueList; }
            set
            {
                tagValueList = value;
                OnPropertyChanged("TagValueList");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdTemplateSettings
        {
            get { return idTemplateSettings; }
            set
            {
                idTemplateSettings = value;
                OnPropertyChanged("IdTemplateSettings");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        //[Rahul.Gadhave][GEOS2-6542][Date:27-11-2024]
        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        //[Rahul.Gadhave][GEOS2-8307][24-06-2025]
        [NotMapped]
        [DataMember]
        public byte[] ImageInBytes
        {
            get { return imageInBytes; }
            set
            {
                imageInBytes = value;
                OnPropertyChanged("ImageInBytes");
            }
        }
		//[nsatpute][31.10.2025][GEOS2-8801]
        [NotMapped]
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
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

        public override string ToString()
        {
            return Value;
        }

        #endregion
    }
}
