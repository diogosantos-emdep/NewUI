using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Articles : ModelBase, IDisposable, IDataErrorInfo
    {
        #region Declaration
        UInt32 idArticle;
        string reference;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        uint idArticleCategory;

        string imagePath;

        string supplierName;

        decimal weight;
        float length;
        float width;
        float height;

        //pcm status
        string pcmStatus;

        //warehouse status
        Int64 isObsolete;
        WarehouseStatus warehouseStatus;

        //delete
        Int64 isEnabled;

        ArticleCategories articleCategory;
        uint idPCMArticleCategory;
        PCMArticleCategory pcmArticleCategory;

        UInt32 idArticleSupplier;

        byte[] articleImageInBytes;
        string visibility;

        Int32? idPCMStatus;

        List<ArticleCompatibility> articleCompatibilityList;
        List<PCMArticleLogEntry> pCMArticleLogEntiryList;
        List<PCMArticleImage> pCMArticleImageList;
        List<ArticleDocument> pCMArticleAttachmentList;
        bool isPCMArticle;
        bool isUpdatedRow;

        uint idPCMArticle;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        List<PCMArticleCategory> categoryMenulist;
        PCMArticleCategory selectedCategory;
        bool isRtfText;
        string pCMDescription;
        byte isImageShareWithCustomer;

        List<LogEntriesByArticle> warehouseArticleLogEntiryList;

        float purchaseQtyMin;
        float purchaseQtyMax;

        byte isShareWithCustomer;
        byte isSparePartOnly;
        Int32 idECOSVisibility;
        string eCOSVisibilityValue;
        string eCOSVisibility;
        string eCOSVisibilityHTMLColor;
        List<LookupValue> eCOSVisibilityList;

        string isRichText;
        Visibility richTextTooltipVisibility;
        string statusHTMLColor;
        List<LookupValue> statusList;

        string pCMDescription_es;
        string pCMDescription_fr;
        string pCMDescription_ro;
        string pCMDescription_zh;
        string pCMDescription_pt;
        string pCMDescription_ru;

        Article warehouseArticle;
        bool isChecked;
        bool? isCheckedNullable;


        List<PLMArticlePrice> includedPLMArticleList;
        List<ArticleDecomposition> articleDecompostionList;
        List<PLMArticlePrice> notIncludedPLMArticleList;

        List<PLMArticlePrice> modifiedPLMArticleList;

        List<BasePriceLogEntry> basePriceLogEntryList;
        List<CustomerPriceLogEntry> customerPriceLogEntryList;
        List<ArticleCustomer> articleCustomerList;
        List<ArticleSuppliers> articleSuppliersList;
        string status;
        Visibility isImageVisible = System.Windows.Visibility.Hidden;
        bool isImageButtonEnabled = false;
        UInt32? pCMImageIdArticle;//[Sudhir.Jangra][GEOS2-2922][24/03/2023]
        string pCMImagePath;//[Sudhir.Jangra][GEOS2-2922][24/03/2023]
        PCMArticleImage pCMImage;//[Sudhir.Jangra][GEOS2-2922][24/03/2023]
        string articleImageCount;//[Sudhir.Jangra][GEOS2-2922][24/03/2023]
        List<Articles> pCMArticleImage;//[Sudhir.Jangra][GEOS2-2922][24/03/2023]
        ImageSource attachmentImage;//[Sudhir.Jangra][GEOS2-2922][28/03/2023]

        bool isHardLockLicensesEditView;//[Sudhir.Jangra][GEOS2-4441]

        DateTime warehouseCreationDate;//[Sudhir.Jangra][GEOS2-4809]
        DateTime pcmCreationDate;//[Sudhir.Jangra][GEOS2-4809]
       //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        List<LinkedArticle> linkedarticleList;
        #region Plants Column
        PlantPrice eAESPrice;
        PlantPrice eAMXPrice;
        PlantPrice eAROPrice;
        PlantPrice eBROPrice;
        PlantPrice eEPYPrice;
        PlantPrice eIBRPrice;
        PlantPrice eJMX1Price;
        PlantPrice eJMX2Price;
        PlantPrice eNRUPrice;
        PlantPrice ePINPrice;
        PlantPrice eSCNPrice;
        PlantPrice eSHNPrice;
        PlantPrice eSMA1Price;
        PlantPrice eSMA2Price;
        PlantPrice eSMXPrice;
        PlantPrice eTCNPrice;
        PlantPrice eTHQPrice;
        PlantPrice eTMAPrice;
        PlantPrice eTTNPrice;
        PlantPrice eCNIPrice;
        PlantPrice c1;
        PlantPrice c2;
        PlantPrice c3;
        PlantPrice c4;
        PlantPrice c5;
        #endregion

        #endregion

        #region Properties
        [DataMember]
        public bool IsPCMArticle
        {
            get
            {
                return isPCMArticle;
            }

            set
            {
                isPCMArticle = value;
                OnPropertyChanged("IsPCMArticle");
            }
        }

        [DataMember]
        public uint IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

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
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [DataMember]
        public uint IdArticleCategory
        {
            get
            {
                return idArticleCategory;
            }

            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
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
                return length;
            }

            set
            {
                length = value;
                OnPropertyChanged("Length");
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
        public string PCMStatus
        {
            get
            {
                return pcmStatus;
            }

            set
            {
                pcmStatus = value;
                Status = pcmStatus;
                OnPropertyChanged("PCMStatus");
            }
        }


        [DataMember]
        public string ECOSVisi
        {
            get
            {
                return pcmStatus;
            }

            set
            {
                pcmStatus = value;
                Status = pcmStatus;
                OnPropertyChanged("PCMStatus");
            }
        }

        [DataMember]
        public WarehouseStatus WarehouseStatus
        {
            get
            {
                return warehouseStatus;
            }

            set
            {
                warehouseStatus = value;
                OnPropertyChanged("WarehouseStatus");
            }
        }

        [DataMember]
        public long IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [DataMember]
        public ArticleCategories ArticleCategory
        {
            get
            {
                return articleCategory;
            }

            set
            {
                articleCategory = value;
                OnPropertyChanged("ArticleCategory");
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

        [DataMember]
        public uint IdPCMArticleCategory
        {
            get
            {
                return idPCMArticleCategory;
            }

            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged("IdPCMArticleCategory");
            }
        }

        [DataMember]
        public PCMArticleCategory PcmArticleCategory
        {
            get
            {
                return pcmArticleCategory;
            }

            set
            {
                pcmArticleCategory = value;
                OnPropertyChanged("PcmArticleCategory");
            }
        }

        [DataMember]
        public uint IdArticleSupplier
        {
            get
            {
                return idArticleSupplier;
            }

            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [DataMember]
        public long IsObsolete
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
        public string Visibility
        {
            get
            {
                return visibility;
            }

            set
            {
                visibility = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        [DataMember]
        public int? IdPCMStatus
        {
            get
            {
                return idPCMStatus;
            }

            set
            {
                idPCMStatus = value;
                OnPropertyChanged("IdPCMStatus");
            }
        }

        [DataMember]
        public List<ArticleCompatibility> ArticleCompatibilityList
        {
            get
            {
                return articleCompatibilityList;
            }

            set
            {
                articleCompatibilityList = value;
                OnPropertyChanged("ArticleCompatibilityList");
            }
        }

        [DataMember]
        public List<PCMArticleLogEntry> PCMArticleLogEntiryList
        {
            get
            {
                return pCMArticleLogEntiryList;
            }

            set
            {
                pCMArticleLogEntiryList = value;
                OnPropertyChanged("PCMArticleLogEntiryList");
            }
        }
        [DataMember]
        public List<PCMArticleImage> PCMArticleImageList
        {
            get
            {
                return pCMArticleImageList;
            }

            set
            {
                pCMArticleImageList = value;
                OnPropertyChanged("PCMArticleImageList");
            }
        }

        [DataMember]
        public List<ArticleDocument> PCMArticleAttachmentList
        {
            get
            {
                return pCMArticleAttachmentList;
            }

            set
            {
                pCMArticleAttachmentList = value;
                OnPropertyChanged("PCMArticleAttachmentList");
            }
        }

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

        [DataMember]
        public uint IdPCMArticle
        {
            get
            {
                return idPCMArticle;
            }

            set
            {
                idPCMArticle = value;
                OnPropertyChanged("IdPCMArticle");
            }
        }

        [DataMember]
        public UInt32 IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public List<PCMArticleCategory> CategoryMenulist
        {
            get
            {
                return categoryMenulist;
            }

            set
            {
                categoryMenulist = value;
                OnPropertyChanged("CategoryMenulist");
            }
        }

        [DataMember]
        public PCMArticleCategory SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        [DataMember]
        public bool IsRtfText
        {
            get
            {
                return isRtfText;
            }

            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [DataMember]
        public string PCMDescription
        {
            get
            {
                return pCMDescription;
            }

            set
            {
                pCMDescription = value;
                OnPropertyChanged("PCMDescription");
            }
        }

        [DataMember]
        public byte IsImageShareWithCustomer
        {
            get
            {
                return isImageShareWithCustomer;
            }

            set
            {
                isImageShareWithCustomer = value;
                OnPropertyChanged("IsImageShareWithCustomer");
            }
        }

        [DataMember]
        public List<LogEntriesByArticle> WarehouseArticleLogEntiryList
        {
            get
            {
                return warehouseArticleLogEntiryList;
            }

            set
            {
                warehouseArticleLogEntiryList = value;
                OnPropertyChanged("WarehouseArticleLogEntiryList");
            }
        }
        [DataMember]
        public Int32 IdECOSVisibility
        {
            get
            {
                return idECOSVisibility;
            }

            set
            {
                idECOSVisibility = value;
                OnPropertyChanged("IdECOSVisibility");
            }
        }

        [DataMember]
        public float PurchaseQtyMin
        {
            get
            {
                return purchaseQtyMin;
            }

            set
            {

                purchaseQtyMin = value;
                OnPropertyChanged("PurchaseQtyMin");
            }
        }

        [DataMember]
        public float PurchaseQtyMax
        {
            get
            {
                return purchaseQtyMax;
            }

            set
            {
                purchaseQtyMax = value;
                OnPropertyChanged("PurchaseQtyMax");
            }
        }

        [DataMember]
        public byte IsShareWithCustomer
        {
            get
            {
                return isShareWithCustomer;
            }

            set
            {
                isShareWithCustomer = value;
                OnPropertyChanged("IsShareWithCustomer");
            }
        }

        [DataMember]
        public byte IsSparePartOnly
        {
            get
            {
                return isSparePartOnly;
            }

            set
            {
                isSparePartOnly = value;
                OnPropertyChanged("IsSparePartOnly");
            }
        }

        [DataMember]
        public string ECOSVisibilityValue
        {
            get
            {
                return eCOSVisibilityValue;
            }

            set
            {
                eCOSVisibilityValue = value;
                eCOSVisibility = eCOSVisibilityValue;
                OnPropertyChanged("ECOSVisibilityValue");
            }
        }

        [DataMember]
        public string ECOSVisibility
        {
            get
            {
                return eCOSVisibility;
            }

            set
            {
                eCOSVisibility = value;
                OnPropertyChanged("ECOSVisibility");
            }
        }

        [DataMember]
        public string ECOSVisibilityHTMLColor
        {
            get
            {
                return eCOSVisibilityHTMLColor;
            }

            set
            {
                eCOSVisibilityHTMLColor = value;
                OnPropertyChanged("ECOSVisibilityHTMLColor");
            }
        }

        [DataMember]
        public List<LookupValue> ECOSVisibilityList
        {
            get
            {
                return eCOSVisibilityList;
            }

            set
            {
                eCOSVisibilityList = value;
                OnPropertyChanged("ECOSVisibilityList");
            }
        }

        [DataMember]
        public string IsRichText
        {
            get
            {
                return isRichText;
            }

            set
            {
                isRichText = value;
                OnPropertyChanged("IsRichText");
            }
        }

        [DataMember]
        public Visibility RichTextTooltipVisibility
        {
            get
            {
                return richTextTooltipVisibility;
            }

            set
            {
                richTextTooltipVisibility = value;
                OnPropertyChanged("RichTextTooltipVisibility");
            }
        }

        [DataMember]
        public string StatusHTMLColor
        {
            get
            {
                return statusHTMLColor;
            }

            set
            {
                statusHTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
            }
        }

        [DataMember]
        public List<LookupValue> StatusList
        {
            get
            {
                return statusList;
            }

            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        [DataMember]
        public string PCMDescription_es
        {
            get
            {
                return pCMDescription_es;
            }

            set
            {
                pCMDescription_es = value;
                OnPropertyChanged("PCMDescription_es");
            }
        }

        [DataMember]
        public string PCMDescription_fr
        {
            get
            {
                return pCMDescription_fr;
            }

            set
            {
                pCMDescription_fr = value;
                OnPropertyChanged("PCMDescription_fr");
            }
        }

        [DataMember]
        public string PCMDescription_ro
        {
            get
            {
                return pCMDescription_ro;
            }

            set
            {
                pCMDescription_ro = value;
                OnPropertyChanged("PCMDescription_ro");
            }
        }

        [DataMember]
        public string PCMDescription_zh
        {
            get
            {
                return pCMDescription_zh;
            }

            set
            {
                pCMDescription_zh = value;
                OnPropertyChanged("PCMDescription_zh");
            }
        }

        [DataMember]
        public string PCMDescription_pt
        {
            get
            {
                return pCMDescription_pt;
            }

            set
            {
                pCMDescription_pt = value;
                OnPropertyChanged("PCMDescription_pt");
            }
        }

        [DataMember]
        public string PCMDescription_ru
        {
            get
            {
                return pCMDescription_ru;
            }

            set
            {
                pCMDescription_ru = value;
                OnPropertyChanged("PCMDescription_ru");
            }
        }

        [DataMember]
        public Article WarehouseArticle
        {
            get
            {
                return warehouseArticle;
            }

            set
            {
                warehouseArticle = value;
                OnPropertyChanged("WarehouseArticle");
            }
        }

        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [DataMember]
        public bool? IsCheckedNullable
        {
            get
            {
                return isCheckedNullable;
            }

            set
            {
                isCheckedNullable = value;
                OnPropertyChanged("IsCheckedNullable");
            }
        }

        [DataMember]
        public List<PLMArticlePrice> IncludedPLMArticleList
        {
            get
            {
                return includedPLMArticleList;
            }

            set
            {
                includedPLMArticleList = value;
                OnPropertyChanged("IncludedPLMArticleList");
            }
        }

        [DataMember]
        public List<ArticleDecomposition> ArticleDecompostionList
        {
            get
            {
                return articleDecompostionList;
            }

            set
            {
                articleDecompostionList = value;
                OnPropertyChanged("ArticleDecompostionList");
            }
        }

        [DataMember]
        public List<PLMArticlePrice> NotIncludedPLMArticleList
        {
            get
            {
                return notIncludedPLMArticleList;
            }

            set
            {
                notIncludedPLMArticleList = value;
                OnPropertyChanged("NotIncludedPLMArticleList");
            }
        }

        [DataMember]
        public List<PLMArticlePrice> ModifiedPLMArticleList
        {
            get
            {
                return modifiedPLMArticleList;
            }

            set
            {
                modifiedPLMArticleList = value;
                OnPropertyChanged("ModifiedPLMArticleList");
            }
        }

        [DataMember]
        public List<BasePriceLogEntry> BasePriceLogEntryList
        {
            get
            {
                return basePriceLogEntryList;
            }

            set
            {
                basePriceLogEntryList = value;
                OnPropertyChanged("BasePriceLogEntryList");
            }
        }

        [DataMember]
        public List<CustomerPriceLogEntry> CustomerPriceLogEntryList
        {
            get
            {
                return customerPriceLogEntryList;
            }

            set
            {
                customerPriceLogEntryList = value;
                OnPropertyChanged("CustomerPriceLogEntryList");
            }
        }


        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public List<ArticleCustomer> ArticleCustomerList
        {
            get
            {
                return articleCustomerList;
            }

            set
            {
                articleCustomerList = value;
                OnPropertyChanged("ArticleCustomerList");
            }
        }

        [DataMember]
        public List<ArticleSuppliers> ArticleSuppliersList
        {
            get
            {
                return articleSuppliersList;
            }

            set
            {
                articleSuppliersList = value;
                OnPropertyChanged("ArticleSuppliersList");
            }
        }
        [DataMember]
        public Visibility IsImageVisible
        {
            get { return isImageVisible; }
            set
            {
                isImageVisible = value;
                OnPropertyChanged("IsImageVisible");
            }

        }
        [DataMember]
        public bool IsImageButtonEnabled
        {
            get { return isImageButtonEnabled; }
            set
            {
                isImageButtonEnabled = value;
                OnPropertyChanged("IsImageButtonEnabled");
            }
        }
        [DataMember]
        public UInt32? PCMImageIdArticle
        {
            get { return pCMImageIdArticle; }
            set
            {
                pCMImageIdArticle = value;
                OnPropertyChanged("PCMImageIdArticle");
            }
        }
        [DataMember]
        public string PCMImagePath
        {
            get { return pCMImagePath; }
            set
            {
                pCMImagePath = value;
                OnPropertyChanged("PCMImagePath");
            }
        }
        [DataMember]
        public PCMArticleImage PCMImage
        {
            get { return pCMImage; }
            set
            {
                pCMImage = value;
                OnPropertyChanged("PCMImage");
            }
        }
        [DataMember]
        public string ArticleImageCount
        {
            get { return articleImageCount; }
            set
            {
                articleImageCount = value;
                OnPropertyChanged("ArticleImageCount");
            }
        }
        [DataMember]
        public List<Articles> PCMArticleImage
        {
            get { return pCMArticleImage; }
            set
            {
                pCMArticleImage = value;
                OnPropertyChanged("PCMArticleImage");
            }
        }
        [DataMember]
        public ImageSource AttachmentImage
        {
            get { return attachmentImage; }
            set
            {
                attachmentImage = value;
                OnPropertyChanged("AttachmentImage");
            }
        }

        //[Sudhir.jangra][GEOS2-4441]
        [DataMember]
        public bool IsHardLockPluginEditView
        {
            get { return isHardLockLicensesEditView; }
            set
            {
                isHardLockLicensesEditView = value;
                OnPropertyChanged("IsHardLockPluginEditView");
            }
        }

        //[Sudhir.Jangra][GEOS2-4809]
        [DataMember]
        public DateTime WarehouseCreationDate
        {
            get { return warehouseCreationDate; }
            set
            {
                warehouseCreationDate = value;
                OnPropertyChanged("WarehouseCreationDate");
            }
        }

        //[Sudhir.jangra][GEOS2-4809]
        [DataMember]
        public DateTime PCMCreationDate
        {
            get { return pcmCreationDate; }
            set
            {
                pcmCreationDate = value;
                OnPropertyChanged("PCMCreationDate");
            }
        }
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        [DataMember]
        public List<LinkedArticle> LinkedArticleList
        {
            get
            {
                return linkedarticleList;
            }
            set
            {
                linkedarticleList = value;
                OnPropertyChanged("LinkedArticleList");
            }
        }


        #region Plants Columns
        [DataMember]
        public PlantPrice EAESPrice
        {
            get
            {
                return eAESPrice;
            }

            set
            {
                eAESPrice = value;
                OnPropertyChanged("EAESPrice");
            }
        }
        [DataMember]
        public PlantPrice EIBRPrice
        {
            get
            {
                return eIBRPrice;
            }

            set
            {
                eIBRPrice = value;
                OnPropertyChanged("EIBRPrice");
            }
        }
        [DataMember]
        public PlantPrice EAMXPrice
        {
            get
            {
                return eAMXPrice;
            }

            set
            {
                eAMXPrice = value;
                OnPropertyChanged("EAMXPrice");
            }
        }
        [DataMember]
        public PlantPrice EAROPrice
        {
            get
            {
                return eAROPrice;
            }

            set
            {
                eAROPrice = value;
                OnPropertyChanged("EAROPrice");
            }
        }

        [DataMember]
        public PlantPrice EBROPrice
        {
            get
            {
                return eBROPrice;
            }

            set
            {
                eBROPrice = value;
                OnPropertyChanged("eBROPrice");
            }
        }

        [DataMember]
        public PlantPrice EEPYPrice
        {
            get
            {
                return eEPYPrice;
            }

            set
            {
                eEPYPrice = value;
                OnPropertyChanged("EEPYPrice");
            }
        }

        [DataMember]
        public PlantPrice EJMX1Price
        {
            get
            {
                return eJMX1Price;
            }

            set
            {
                eJMX1Price = value;
                OnPropertyChanged("EJMX1Price");
            }
        }

        [DataMember]
        public PlantPrice EJMX2Price
        {
            get
            {
                return eJMX2Price;
            }

            set
            {
                eJMX2Price = value;
                OnPropertyChanged("EJMX2Price");
            }
        }

        [DataMember]
        public PlantPrice ENRUPrice
        {
            get
            {
                return eNRUPrice;
            }

            set
            {
                eNRUPrice = value;
                OnPropertyChanged("ENRUPrice");
            }
        }
        [DataMember]
        public PlantPrice EPINPrice
        {
            get
            {
                return ePINPrice;
            }

            set
            {
                ePINPrice = value;
                OnPropertyChanged("EPINPrice");
            }
        }

        [DataMember]
        public PlantPrice ESCNPrice
        {
            get
            {
                return eSCNPrice;
            }

            set
            {
                eSCNPrice = value;
                OnPropertyChanged("ESCNPrice");
            }
        }
        [DataMember]
        public PlantPrice ESHNPrice
        {
            get
            {
                return eSHNPrice;
            }

            set
            {
                eSHNPrice = value;
                OnPropertyChanged("ESHNPrice");
            }
        }
        [DataMember]
        public PlantPrice ESMA1Price
        {
            get
            {
                return eSMA1Price;
            }

            set
            {
                eSMA1Price = value;
                OnPropertyChanged("ESMA1Price");
            }
        }

        [DataMember]
        public PlantPrice ESMA2Price
        {
            get
            {
                return eSMA2Price;
            }

            set
            {
                eSMA2Price = value;
                OnPropertyChanged("ESMA2Price");
            }
        }

        [DataMember]
        public PlantPrice ESMXPrice
        {
            get
            {
                return eSMXPrice;
            }

            set
            {
                eSMXPrice = value;
                OnPropertyChanged("ESMXPrice");
            }
        }

        [DataMember]
        public PlantPrice ETCNPrice
        {
            get
            {
                return eTCNPrice;
            }

            set
            {
                eTCNPrice = value;
                OnPropertyChanged("ETCNPrice");
            }
        }

        [DataMember]
        public PlantPrice ETHQPrice
        {
            get
            {
                return eTHQPrice;
            }

            set
            {
                eTHQPrice = value;
                OnPropertyChanged("ETHQPrice");
            }
        }
        [DataMember]
        public PlantPrice ETMAPrice
        {
            get
            {
                return eTMAPrice;
            }

            set
            {
                eTMAPrice = value;
                OnPropertyChanged("ETMAPrice");
            }
        }

        [DataMember]
        public PlantPrice ETTNPrice
        {
            get
            {
                return eTTNPrice;
            }

            set
            {
                eTTNPrice = value;
                OnPropertyChanged("ETTNPrice");
            }
        }

        [DataMember]
        public PlantPrice C1
        {
            get
            {
                return c1;
            }

            set
            {
                c1 = value;
                OnPropertyChanged("C1");
            }
        }
        [DataMember]
        public PlantPrice C2
        {
            get
            {
                return c2;
            }

            set
            {
                c2 = value;
                OnPropertyChanged("C2");
            }
        }
        [DataMember]
        public PlantPrice C3
        {
            get
            {
                return c3;
            }

            set
            {
                c3 = value;
                OnPropertyChanged("C3");
            }
        }
        [DataMember]
        public PlantPrice C4
        {
            get
            {
                return c4;
            }

            set
            {
                c4 = value;
                OnPropertyChanged("C4");
            }
        }
        [DataMember]
        public PlantPrice C5
        {
            get
            {
                return c5;
            }

            set
            {
                c5 = value;
                OnPropertyChanged("C5");
            }
        }
        #endregion 

        #endregion

        #region Constructor

        public Articles()
        {
            C1 = new PlantPrice();
            C2 = new PlantPrice();
            C3 = new PlantPrice();
            C4 = new PlantPrice();
            C5 = new PlantPrice();
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            Articles articles = (Articles)this.MemberwiseClone();

            if (ArticleCategory != null)
                articles.ArticleCategory = (ArticleCategories)this.ArticleCategory.Clone();

            if (PcmArticleCategory != null)
                articles.PcmArticleCategory = (PCMArticleCategory)this.PcmArticleCategory.Clone();

            if (ArticleCompatibilityList != null)
                articles.ArticleCompatibilityList = ArticleCompatibilityList.Select(x => (ArticleCompatibility)x.Clone()).ToList();

            if (PCMArticleImageList != null)
                articles.PCMArticleImageList = PCMArticleImageList.Select(x => (PCMArticleImage)x.Clone()).ToList();

            if (PCMArticleLogEntiryList != null)
                articles.PCMArticleLogEntiryList = PCMArticleLogEntiryList.Select(x => (PCMArticleLogEntry)x.Clone()).ToList();

            if (PCMArticleAttachmentList != null)
                articles.PCMArticleAttachmentList = PCMArticleAttachmentList.Select(x => (ArticleDocument)x.Clone()).ToList();

            if (IncludedPLMArticleList != null)
                articles.IncludedPLMArticleList = IncludedPLMArticleList.Select(x => (PLMArticlePrice)x.Clone()).ToList();

            if (NotIncludedPLMArticleList != null)
                articles.NotIncludedPLMArticleList = NotIncludedPLMArticleList.Select(x => (PLMArticlePrice)x.Clone()).ToList();

            if (ModifiedPLMArticleList != null)
                articles.ModifiedPLMArticleList = ModifiedPLMArticleList.Select(x => (PLMArticlePrice)x.Clone()).ToList();

            //if (WarehouseArticle != null)
            //    articles.WarehouseArticle = (Data.Common.Article)this.WarehouseArticle.Clone();

            return articles;
        }

        public override string ToString()
        {
            return Reference + "-" + Description;
        }
        #endregion


        #region Validation

        public string Error
        {
            get { return GetError(); }
        }



        public string this[string columnName]
        {
            get { return GetError(columnName); }
        }

        string GetError(string name = null)
        {
            switch (name)
            {
                case "PurchaseQtyMin":
                    return PurchaseQtyMin > PurchaseQtyMax ? "Purchase Quantity min value must be equal or less than max value." : null;

                case "PurchaseQtyMax":
                    return PurchaseQtyMax < PurchaseQtyMin ? "Purchase Quantity max value must be equal or greater than min value." : null;

                case "IdECOSVisibility":
                    return IdECOSVisibility != 326 && (PurchaseQtyMax == 0 || PurchaseQtyMin == 0) ? "ECOS Visibility must be Read Only" : null;
                case "Description":
                    return string.IsNullOrEmpty(Description) ? "You cannot leave the Name field empty" : null;
                default:
                    return null;
            }
        }

        #endregion
    }
}
