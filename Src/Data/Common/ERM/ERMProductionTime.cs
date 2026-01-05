using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMProductionTime : ModelBase, IDisposable
    {
        #region Fields
        string deliveryWeek;
        string category;
        string otCode;
        Int64 modules;
        Int64 producedModules;
        List<OptionsByOfferGrid> optionsByOffers;
        Int64 idOffer;
        Int64 idOption;
        Quotation quotation;
        Int64 idQuotation;
        Int64 inProductionModules;
        Int32 idProductSubCategory;
        private Int32 idOfferSite;
        private Int32 idProject;
        private string project;
        private string offerSiteName;
        private Int32 idCustomer;
        private string customer;
        Int64 idProductCategory;
        string code;
        ProductCategoryGrid productCategoryGrid;
        private DateTime? deliveryDate;
        private string deliveryDateHtmlColor;
        private string mergeCode;
        #endregion

        #region Properties 

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
        public Int64 Modules
        {
            get { return modules; }
            set
            {
                modules = value;
                OnPropertyChanged("Modules");
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
        public Int64 InProductionModules
        {
            get { return inProductionModules; }
            set
            {
                inProductionModules = value;
                OnPropertyChanged("InProductionModules");
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
        public string OtCode
        {
            get
            {
                return otCode;
            }

            set
            {
                otCode = value;
                OnPropertyChanged("OtCode");
            }
        }

        [DataMember]
        public List<OptionsByOfferGrid> OptionsByOffers
        {
            get
            {
                return optionsByOffers;
            }

            set
            {
                optionsByOffers = value;
                OnPropertyChanged("OptionsByOffers");
            }
        }

        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [DataMember]
        public Int64 IdOption
        {
            get
            {
                return idOption;
            }
            set
            {
                idOption = value;
                OnPropertyChanged("IdOption");
            }
        }

      
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
        public Int64 IdQuotation
        {
            get { return idQuotation; }
            set
            {
                idQuotation = value;
                OnPropertyChanged("IdQuotation");
            }
        }

       
        [DataMember]
        public Int32 IdProductSubCategory
        {
            get
            {
                return idProductSubCategory;
            }
            set
            {
                idProductSubCategory = value;
                OnPropertyChanged("IdProductSubCategory");
            }
        }

        [DataMember]
        public Int32 IDOfferSite
        {
            get { return idOfferSite; }
            set
            {
                idOfferSite = value;
                OnPropertyChanged("IDOfferSite");
            }
        }

        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

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

        [DataMember]
        public string OfferSiteName
        {
            get { return offerSiteName; }
            set
            {
                offerSiteName = value;
                OnPropertyChanged("OfferSiteName");
            }
        }

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
        [DataMember]
        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public Int64 IdProductCategory
        {
            get { return idProductCategory; }
            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }

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
        public string DeliveryDateHtmlColor
        {
            get
            {
                return deliveryDateHtmlColor;
            }

            set
            {
                deliveryDateHtmlColor = value;
                OnPropertyChanged("DeliveryDateHtmlColor");
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
        #endregion

        #region Constructor
        public ERMProductionTime()
        {
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
