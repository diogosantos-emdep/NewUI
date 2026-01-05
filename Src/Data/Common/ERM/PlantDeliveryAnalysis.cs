using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantDeliveryAnalysis: ModelBase, IDisposable
    {
        #region Declaration
        byte idTemplate;
        Int32 idSite;
        string code;
        string oEM;
        string project;
        string pO;
        string name;
        Int64 idOffer;
        string offerCode;
        private DateTime? goAheadDate;
        private DateTime? poDate;
        private DateTime? samplesDate;
        private DateTime? shippingDate;
        private string daysSamplestoShippment;
        private string daystoShippment;
        private string amount;
        private string category1;
        private string category2;
        private string oTStatus;
        private string customerGroup;
        private string customerPlant;
        private Int32 idProductCategory;
        ProductCategoryGrid productCategoryGrid;
        Int32 idCustomer;
        private string region;
        private DateTime? deliveryDate;
        private string offerStatusType;
        private string templateName;
        
        private Int32 totalDays;
        private Int32 oTIdSite;
        private string sample;
        private string shippmentDaysOverDeliveryDate;//Aishwarya Ingale[Geos2-6431]
        private string serialNumber;//Aishwarya Ingale[Geos2-6431]
        #endregion

        #region Porperties
        [Column("IdSite")]
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        [Column("OEM")]
        [DataMember]
        public string OEM
        {
            get { return oEM; }
            set
            {
                oEM = value;
                OnPropertyChanged("OEM");
            }
        }

        [Column("Project")]
        [DataMember]
        public string Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }
        [Column("Code")]
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

        [Column("PO")]
        [DataMember]
        public string PO
        {
            get { return pO; }
            set
            {
                pO = value;
                OnPropertyChanged("PO");
            }
        }
        [DataMember]
        public DateTime? GoAheadDate
        {
            get
            {
                return goAheadDate;
            }

            set
            {
                goAheadDate = value;
                OnPropertyChanged("GoAheadDate");
            }
        }

        [DataMember]
        public DateTime? PODate
        {
            get
            {
                return poDate;
            }

            set
            {
                poDate = value;
                OnPropertyChanged("PODate");
            }
        }
        [DataMember]
        public DateTime? SamplesDate
        {
            get
            {
                return samplesDate;
            }

            set
            {
                samplesDate = value;
                OnPropertyChanged("SamplesDate");
            }
        }
        [DataMember]
        public DateTime? ShippingDate
        {
            get
            {
                return shippingDate;
            }

            set
            {
                shippingDate = value;
                OnPropertyChanged("ShippingDate");
            }
        }

        [DataMember]
        public string DaysSamplestoShippment
        {
            get
            {
                return daysSamplestoShippment;
            }

            set
            {
                daysSamplestoShippment = value;
                OnPropertyChanged("DaysSamplestoShippment");
            }
        }
        [DataMember]
        public string DaystoShippment
        {
            get
            {
                return daystoShippment;
            }

            set
            {
                daystoShippment = value;
                OnPropertyChanged("DaystoShippment");
            }
        }

        private string currencySymbol;
        [DataMember]
        public string CurrencySymbol
        {
            get
            {
                return currencySymbol;
            }

            set
            {
                currencySymbol = value;
                OnPropertyChanged("CurrencySymbol");
            }
        }

        [DataMember]
        public string Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private double amounts;
        [DataMember]
        public double Amounts
        {
            get
            {
                return amounts;
            }

            set
            {
                amounts = value;
                OnPropertyChanged("Amounts");
            }
        }


        private string amountsWithCurrency;
        [DataMember]
        public string AmountsWithCurrency
        {
            get
            {
                return amountsWithCurrency;
            }

            set
            {
                amountsWithCurrency = value;
                OnPropertyChanged("AmountsWithCurrency");
            }
        }

        [DataMember]
        public string Category1
        {
            get
            {
                return category1; 
            }

            set
            {
                category1 = value;
                OnPropertyChanged("Category1");
            }
        }
        [DataMember]
        public string Category2
        {
            get
            {
                return category2;
            }

            set
            {
                category2 = value;
                OnPropertyChanged("Category2");
            }
        }

        [DataMember]
        public string OTStatus
        {
            get
            {
                return oTStatus;
            }

            set
            {
                oTStatus = value;
                OnPropertyChanged("OTStatus");
            }
        }

        [DataMember]
        public string CustomerGroup
        {
            get
            {
                return customerGroup;
            }

            set
            {
                customerGroup = value;
                OnPropertyChanged("CustomerGroup");
            }
        }

        [DataMember]
        public string CustomerPlant
        {
            get
            {
                return customerPlant;
            }

            set
            {
                customerPlant = value;
                OnPropertyChanged("CustomerPlant");
            }
        }

        [DataMember]
        public Int32 IdProductCategory
        {
            get
            {
                return idProductCategory;
            }

            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public ProductCategoryGrid ProductCategoryGrid
        {
            get
            {
                return productCategoryGrid;
            }
            set
            {
                productCategoryGrid = value;
                OnPropertyChanged("ProductCategoryGrid");
            }
        }

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

        [DataMember]
        public DateTime? DeliveryDate
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


        [DataMember]
        public string OfferStatusType
        {
            get
            {
                return offerStatusType;
            }

            set
            {
                offerStatusType = value;
                OnPropertyChanged("OfferStatusType");
            }
        }

        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }
        [DataMember]
        public Int32 TotalDays
        {
            get
            {
                return totalDays;
            }

            set
            {
                totalDays = value;
                OnPropertyChanged("TotalDays");
            }
        }


        [DataMember]
        public Int32 OTIdSite
        {
            get
            {
                return oTIdSite;
            }

            set
            {
                oTIdSite = value;
                OnPropertyChanged("OTIdSite");
            }
        }

        [DataMember]
        public string Sample
        {
            get
            {
                return sample;
            }

            set
            {
                sample = value;
                OnPropertyChanged("Sample");
            }
        }

        //Aishwarya Ingale[Geos2-6431]
        [DataMember]
        public string ShippmentDaysOverDeliveryDate
        {
            get
            {
                return shippmentDaysOverDeliveryDate;
            }

            set
            {
                shippmentDaysOverDeliveryDate = value;
                OnPropertyChanged("ShippmentDaysOverDeliveryDate");
            }
        }

        [DataMember]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
      

        #endregion
    }
}
