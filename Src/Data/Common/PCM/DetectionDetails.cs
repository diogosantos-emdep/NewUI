using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Emdep.Geos.Data.Common.PLM;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DetectionDetails : ModelBase, IDisposable
    {
        #region Fields
        CPLCustomer pCMCustomerExc;
        CPLCustomer pCMCustomerInc;
        List<DetectionTypes> testList;
        List<DetectionGroup> detectionAllGroupList;
        UInt32 idDetections;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_ro;
        string name_zh;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_pt;
        string description_ro;
        string description_zh;
        string description_ru;
        UInt32 orderNumber;
        string nameToShow;
        UInt64 family;
        UInt32 weldOrder;
        string code;
        char? orientation;
        UInt64 idTestType;
        string infoLink;
        UInt32 idDetectionType;
        UInt64 isMandatoryVisualAid;
        UInt32 createdBy;
        DateTime? createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        TestTypes testTypes;
        DetectionTypes detectionTypes;

        List<DetectionImage> detectionImageList;
        DetectionImage detectionImage;
        ObservableCollection<DetectionImage> detectionImageCount;
        List<DetectionAttachedDoc> detectionAttachedDocList;
        List<DetectionAttachedLink> detectionAttachedLinkList;

        UInt32? idGroup;
        DetectionGroup detectionGroup;
        List<DetectionGroup> detectionGroupList;
        DetectionOrderGroup detectionOrderGroup;
        List<RegionsByCustomer> customerList;

        List<DetectionLogEntry> detectionLogEntryList;

        DateTime lastUpdate;

        UInt32 idStatus;
        LookupValue status;
        List<ProductTypes> productTypesList;

        float purchaseQtyMin;
        float purchaseQtyMax;

        byte isShareWithCustomer;
        byte isSparePartOnly;
        Int32 idECOSVisibility;
        string eCOSVisibilityValue;
        string eCOSVisibilityHTMLColor;
        List<LookupValue> eCOSVisibilityList;

        string isRichText;
        Visibility richTextTooltipVisibility;
        string statusHTMLColor;
        List<LookupValue> statusList;
        Int32? idPCMStatus;
        //pcm status
        string pcmStatus;
        bool isUpdatedRow;

        List<PLMDetectionPrice> includedPLMDetectionList;
        List<PLMDetectionPrice> notIncludedPLMDetectionList;

        List<PLMDetectionPrice> modifiedPLMDetectionList;

        List<BasePriceLogEntry> basePriceLogEntryList;
        List<CustomerPriceLogEntry> customerPriceLogEntryList;

        List<CPLCustomer> customerListByDetection;
        UInt64 idScope;
        LookupValue scope;
        Template template;//[Sudhir.Jangra][GEOS2-4091][27/01/2023]
        bool isImageExist = false;//[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        Visibility isImageVisible = Visibility.Hidden;//[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        string imageCount;//[Sudhir.Jangra][GEOS2-2922][20/03/2023]

        ObservableCollection<ProductTypeLogEntry> productTypeChangeLogList;//[Sudhir.Jangra][GEOS2-4460][28/06/2023]

        List<DetectionLogEntry> detectionCommentsList;//[sudhir.jangra][GEOS-4935]

        #endregion

        #region Constructor

        public DetectionDetails()
        {
            PCMCustomerInc = new CPLCustomer();
            PCMCustomerExc = new CPLCustomer();
        }

        #endregion

        #region Properties
        [DataMember]
        List<System.Drawing.Bitmap> mergePDFDocument;
        public List<System.Drawing.Bitmap> MergePDFDocument
        {
            get
            {
                return mergePDFDocument;
            }

            set
            {
                mergePDFDocument = value;
                OnPropertyChanged("MergePDFDocument");
            }
        }
        [DataMember]
        public UInt32 IdDetections
        {
            get
            {
                return idDetections;
            }

            set
            {
                idDetections = value;
                OnPropertyChanged("IdDetections");
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public string Name_es
        {
            get
            {
                return name_es;
            }

            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
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
        public UInt32 OrderNumber
        {
            get
            {
                return orderNumber;
            }

            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        [DataMember]
        public string NameToShow
        {
            get
            {
                return nameToShow;
            }

            set
            {
                nameToShow = value;
                OnPropertyChanged("NameToShow");
            }
        }

        [DataMember]
        public UInt64 Family
        {
            get
            {
                return family;
            }

            set
            {
                family = value;
                OnPropertyChanged("Family");
            }
        }

        [DataMember]
        public UInt32 WeldOrder
        {
            get
            {
                return weldOrder;
            }

            set
            {
                weldOrder = value;
                OnPropertyChanged("WeldOrder");
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
        public char? Orientation
        {
            get
            {
                return orientation;
            }

            set
            {
                orientation = value;
                OnPropertyChanged("Orientation");
            }
        }

        [DataMember]
        public UInt64 IdTestType
        {
            get
            {
                return idTestType;
            }

            set
            {
                idTestType = value;
                OnPropertyChanged("IdTestType");
            }
        }

        [DataMember]
        public string InfoLink
        {
            get
            {
                return infoLink;
            }

            set
            {
                infoLink = value;
                OnPropertyChanged("InfoLink");
            }
        }

        [DataMember]
        public UInt32 IdDetectionType
        {
            get
            {
                return idDetectionType;
            }

            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
            }
        }

        [DataMember]
        public UInt64 IsMandatoryVisualAid
        {
            get
            {
                return isMandatoryVisualAid;
            }

            set
            {
                isMandatoryVisualAid = value;
                OnPropertyChanged("IsMandatoryVisualAid");
            }
        }

        [DataMember]
        public UInt32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public TestTypes TestTypes
        {
            get
            {
                return testTypes;
            }

            set
            {
                testTypes = value;
                OnPropertyChanged("TestTypes");
            }
        }

        [DataMember]
        public DetectionTypes DetectionTypes
        {
            get
            {
                return detectionTypes;
            }

            set
            {
                detectionTypes = value;
                OnPropertyChanged("DetectionTypes");
            }
        }

        [DataMember]
        public List<DetectionAttachedDoc> DetectionAttachedDocList
        {
            get
            {
                return detectionAttachedDocList;
            }

            set
            {
                detectionAttachedDocList = value;
                OnPropertyChanged("DetectionAttachedDocList");
            }
        }

        [DataMember]
        public List<DetectionAttachedLink> DetectionAttachedLinkList
        {
            get
            {
                return detectionAttachedLinkList;
            }

            set
            {
                detectionAttachedLinkList = value;
                OnPropertyChanged("DetectionAttachedLinkList");
            }
        }

        [DataMember]
        public List<DetectionImage> DetectionImageList
        {
            get
            {
                return detectionImageList;
            }

            set
            {
                detectionImageList = value;
                OnPropertyChanged("DetectionImageList");
            }
        }

        [DataMember]
        public uint? IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public DetectionGroup DetectionGroup
        {
            get
            {
                return detectionGroup;
            }

            set
            {
                detectionGroup = value;
                OnPropertyChanged("DetectionGroup");
            }
        }

        [DataMember]
        public List<DetectionGroup> DetectionGroupList
        {
            get
            {
                return detectionGroupList;
            }

            set
            {
                detectionGroupList = value;
                OnPropertyChanged("DetectionGroupList");
            }
        }

        [DataMember]
        public DetectionOrderGroup DetectionOrderGroup
        {
            get
            {
                return detectionOrderGroup;
            }

            set
            {
                detectionOrderGroup = value;
                OnPropertyChanged("DetectionOrderGroup");
            }
        }

        [DataMember]
        public List<RegionsByCustomer> CustomerList
        {
            get
            {
                return customerList;
            }

            set
            {
                customerList = value;
                OnPropertyChanged("CustomerList");
            }
        }

        [DataMember]
        public List<DetectionLogEntry> DetectionLogEntryList
        {
            get
            {
                return detectionLogEntryList;
            }

            set
            {
                detectionLogEntryList = value;
                OnPropertyChanged("DetectionLogEntryList");
            }
        }

        [DataMember]
        public DateTime LastUpdate
        {
            get
            {
                return lastUpdate;
            }

            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [DataMember]
        public uint IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public LookupValue Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public List<ProductTypes> ProductTypesList
        {
            get
            {
                return productTypesList;
            }

            set
            {
                productTypesList = value;
                OnPropertyChanged("ProductTypesList");
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
                OnPropertyChanged("ECOSVisibilityValue");
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
        public string PCMStatus
        {
            get
            {
                return pcmStatus;
            }

            set
            {
                pcmStatus = value;
                OnPropertyChanged("PCMStatus");
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
        public List<PLMDetectionPrice> IncludedPLMDetectionList
        {
            get
            {
                return includedPLMDetectionList;
            }

            set
            {
                includedPLMDetectionList = value;
                OnPropertyChanged("IncludedPLMDetectionList");
            }
        }

        [DataMember]
        public List<PLMDetectionPrice> NotIncludedPLMDetectionList
        {
            get
            {
                return notIncludedPLMDetectionList;
            }

            set
            {
                notIncludedPLMDetectionList = value;
                OnPropertyChanged("NotIncludedPLMDetectionList");
            }
        }

        [DataMember]
        public List<PLMDetectionPrice> ModifiedPLMDetectionList
        {
            get
            {
                return modifiedPLMDetectionList;
            }

            set
            {
                modifiedPLMDetectionList = value;
                OnPropertyChanged("ModifiedPLMDetectionList");
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
        public List<CPLCustomer> CustomerListByDetection
        {
            get
            {
                return customerListByDetection;
            }

            set
            {
                customerListByDetection = value;
                OnPropertyChanged("CustomerListByDetection");
            }
        }

        [DataMember]
        public UInt64 IdScope
        {
            get
            {
                return idScope;
            }

            set
            {
                idScope = value;
                OnPropertyChanged("_IdScope");
            }
        }

        public LookupValue Scope
        {
            get
            {
                return scope;
            }

            set
            {
                scope = value;
                OnPropertyChanged("Scope");
            }
        }
        //[rdixit][GEOS2-3970][01.12.2022] 
        [DataMember]
        public List<DetectionGroup> DetectionAllGroupList
        {
            get
            {
                return detectionAllGroupList;
            }

            set
            {
                detectionAllGroupList = value;
                OnPropertyChanged("DetectionAllGroupList");
            }
        }


        [DataMember]
        public List<DetectionTypes> TestList
        {
            get
            {
                return testList;
            }

            set
            {
                testList = value;
                OnPropertyChanged("TestList");
            }
        }
        //[Sudhir.Jangra][GEOS2-4091][27/01/2023]
        [DataMember]
        public Template Template
        {
            get { return template; }
            set
            {
                template = value;
                OnPropertyChanged("Template");
            }
        }

        [DataMember]
        public bool IsImageExist
        {
            get { return isImageExist; }
            set
            {
                isImageExist = value;
                OnPropertyChanged("IsImageExist");
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
        public string ImageCount
        {
            get { return imageCount; }
            set
            {
                imageCount = value;
                OnPropertyChanged("ImageCount");
            }
        }
        [DataMember]
        public DetectionImage DetectionImage
        {
            get { return detectionImage; }
            set
            {
                detectionImage = value;
                OnPropertyChanged("DetectionImage");
            }
        }
        [DataMember]
        public ObservableCollection<DetectionImage> DetectionImageCount
        {
            get { return detectionImageCount; }
            set
            {
                detectionImageCount = value;
                OnPropertyChanged("DetectionImageCount");
            }

        }

        [DataMember]
        public ObservableCollection<ProductTypeLogEntry> ProductTypeChangeLogList
        {
            get { return productTypeChangeLogList; }
            set
            {
                productTypeChangeLogList = value;
                OnPropertyChanged("ProductTypeChangeLogList");
            }
        }


        //[Sudhir.jangra][GEOS2-4935]
        [DataMember]
        public List<DetectionLogEntry> DetectionCommentsList
        {
            get { return detectionCommentsList; }
            set
            {
                detectionCommentsList = value;
                OnPropertyChanged("DetectionCommentsList");
            }
        }

        [DataMember]
        public CPLCustomer PCMCustomerInc
        {
            get
            {
                return pCMCustomerInc;
            }

            set
            {
                pCMCustomerInc = value;
                OnPropertyChanged("PCMCustomerInc");
            }
        }

        [DataMember]
        public CPLCustomer PCMCustomerExc
        {
            get
            {
                return pCMCustomerExc;
            }

            set
            {
                pCMCustomerExc = value;
                OnPropertyChanged("PCMCustomerExc");
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
            DetectionDetails detectionDetails = (DetectionDetails)this.MemberwiseClone();
            if (PCMCustomerExc != null)
                detectionDetails.PCMCustomerExc = (CPLCustomer)this.PCMCustomerExc.Clone();

            if (PCMCustomerInc != null)
                detectionDetails.PCMCustomerInc = (CPLCustomer)this.PCMCustomerInc.Clone();
            if (TestTypes != null)
                detectionDetails.TestTypes = (TestTypes)this.TestTypes.Clone();

            if (detectionTypes != null)
                detectionDetails.DetectionTypes = (DetectionTypes)this.detectionTypes.Clone();

            if (DetectionAttachedDocList != null)
                detectionDetails.DetectionAttachedDocList = DetectionAttachedDocList.Select(x => (DetectionAttachedDoc)x.Clone()).ToList();

            if (DetectionAttachedLinkList != null)
                detectionDetails.DetectionAttachedLinkList = DetectionAttachedLinkList.Select(x => (DetectionAttachedLink)x.Clone()).ToList();

            if (DetectionImageList != null)
                detectionDetails.DetectionImageList = DetectionImageList.Select(x => (DetectionImage)x.Clone()).ToList();

            if (DetectionGroup != null)
                detectionDetails.DetectionGroup = (DetectionGroup)this.DetectionGroup.Clone();

            if (DetectionGroupList != null)
                detectionDetails.DetectionGroupList = DetectionGroupList.Select(x => (DetectionGroup)x.Clone()).ToList();

            if (DetectionOrderGroup != null)
                detectionDetails.DetectionOrderGroup = (DetectionOrderGroup)this.DetectionOrderGroup.Clone();

            if (CustomerList != null)
                detectionDetails.CustomerList = CustomerList.Select(x => (RegionsByCustomer)x.Clone()).ToList();

            if (DetectionLogEntryList != null)
                detectionDetails.DetectionLogEntryList = DetectionLogEntryList.Select(x => (DetectionLogEntry)x.Clone()).ToList();

            if (Status != null)
                detectionDetails.Status = (LookupValue)this.Status.Clone();

            //GEOS2-3199
            if (IncludedPLMDetectionList != null)
                detectionDetails.IncludedPLMDetectionList = IncludedPLMDetectionList.Select(x => (PLMDetectionPrice)x.Clone()).ToList();

            if (NotIncludedPLMDetectionList != null)
                detectionDetails.NotIncludedPLMDetectionList = NotIncludedPLMDetectionList.Select(x => (PLMDetectionPrice)x.Clone()).ToList();

            if (ModifiedPLMDetectionList != null)
                detectionDetails.ModifiedPLMDetectionList = ModifiedPLMDetectionList.Select(x => (PLMDetectionPrice)x.Clone()).ToList();

            if(DetectionCommentsList!=null)
                detectionDetails.DetectionCommentsList = DetectionCommentsList.Select(x => (DetectionLogEntry)x.Clone()).ToList();

            return detectionDetails;
        }

        public void UpdateAllMaxCostFromNullToZero()
        {
            UpdateAllMaxCostFromNullToZero(this.IncludedPLMDetectionList);
            UpdateAllMaxCostFromNullToZero(this.NotIncludedPLMDetectionList);
        }

        public static void UpdateAllMaxCostFromNullToZero(DetectionDetails detectionDetails)
        {
            UpdateAllMaxCostFromNullToZero(detectionDetails.IncludedPLMDetectionList);
            UpdateAllMaxCostFromNullToZero(detectionDetails.NotIncludedPLMDetectionList);
        }

        public static void UpdateAllMaxCostFromNullToZero(List<PLMDetectionPrice> PLMDetectionPriceList)
        {
            if (PLMDetectionPriceList != null)
            {
                foreach (PLMDetectionPrice item in PLMDetectionPriceList)
                {
                    if (item.MaxCost == null)
                        item.MaxCost = 0;
                }
            }
        }

        #endregion
    }
}
