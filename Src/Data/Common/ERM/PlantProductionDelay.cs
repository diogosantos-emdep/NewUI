using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantProductionDelay : ModelBase, IDisposable
    {
        #region Field
        private Int64 idOffer;
        private Int32 idSite;
        private string mergeCode;
        private string deliveryWeek;

        private DateTime? deliveryDate;

        private string pOType;
        //private string status;
        private string customerName;
        private string siteName;
        private string shortName;
        private string countryName;
        
        private Int32 idProductCategory;
        Quotation quotation;
       private Int64 producedModules;
        private Int64 modules;
        Int64 idQuotation;
        #endregion
        #region Property
        [DataMember]
        public Int64 IdQuotation
        {
            get { return idQuotation; }
            set
            {
                idQuotation = value;
                OnPropertyChanged("IdQuotation");
            }
        }
        // [NotMapped]
        [DataMember]
        public Quotation Quotation
        {
            get { return quotation; }
            set
            {
                quotation = value;
                OnPropertyChanged("Quotation");
            }
        }
        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }
        
        
       
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


        [DataMember]
        public string MergeCode
        {
            get
            {
                return mergeCode;
            }

            set
            {
                mergeCode = value;
                OnPropertyChanged("MergeCode");
            }
        }
        [DataMember]
        public string DeliveryWeek
        {
            get
            {
                return deliveryWeek;
            }

            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
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
        public string POType
        {
            get
            {
                return pOType;
            }

            set
            {
                pOType = value;
                OnPropertyChanged("POType");
            }
        }
        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }
        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }

            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }
        [DataMember]
        public string ShortName
        {
            get
            {
                return shortName;
            }

            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }
        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        [DataMember]
        public Int32 IdProductCategory
        {
            get { return idProductCategory; }
            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }
        [DataMember]
        public Int64 ProducedModules
        {
            get { return producedModules; }
            set
            {
                producedModules = value;
                OnPropertyChanged("ProducedModules");
            }
        }
        [DataMember]
        public Int64 Modules
        {
            get { return modules; }
            set
            {
                modules = value;
                OnPropertyChanged("Modules");
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
