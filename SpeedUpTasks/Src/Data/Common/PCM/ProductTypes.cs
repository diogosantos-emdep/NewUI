using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ProductTypes : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idCPType;
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
        Int64 extraCost;
        UInt64 minimumTestPoints;
        string infoLink;
        UInt32 idDefaultWayType;
        UInt64 standard;
        string code;
        string reference;
        Int32 idStatus;
        UInt32 createdBy;
        DateTime? createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        Int64 isEnabled;

        UInt64 idTemplate;

        Template template;
        DateTime? lastUpdate;
        LookupValue status;
        DefaultWayType defaultWayType;

        List<Ways> wayList;
        List<Options> optionList;
        List<Detections> detectionList;
        List<SpareParts> sparePartList;
        List<ConnectorFamilies> familyList;
        List<Template> templateList;

        List<ProductTypeImage> productTypeImageList;
        List<ProductTypeAttachedDoc> productTypeAttachedDocList;
        List<ProductTypeAttachedLink> productTypeAttachedLinkList;

        List<ProductTypeLogEntry> productTypeLogEntryList;


        bool isTemplate_NotExist;
        List<Options> optionList_Group;
        List<Detections> detectionList_Group;
        List<RegionsByCustomer> customerList;

        UInt64 idTemplate_old;

        List<ProductTypeCompatibility> productTypeCompatibilityList;
        #endregion

        #region Constructor

        public ProductTypes()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public UInt64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
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
        public Int64 ExtraCost
        {
            get
            {
                return extraCost;
            }

            set
            {
                extraCost = value;
                OnPropertyChanged("ExtraCost");
            }
        }

        [DataMember]
        public UInt64 MinimumTestPoints
        {
            get
            {
                return minimumTestPoints;
            }

            set
            {
                minimumTestPoints = value;
                OnPropertyChanged("MinimumTestPoints");
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
        public UInt32 IdDefaultWayType
        {
            get
            {
                return idDefaultWayType;
            }

            set
            {
                idDefaultWayType = value;
                OnPropertyChanged("IdDefaultWayType");
            }
        }

        [DataMember]
        public UInt64 Standard
        {
            get
            {
                return standard;
            }

            set
            {
                standard = value;
                OnPropertyChanged("Standard");
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
        public UInt64 IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public UInt32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

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
        public DateTime? LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [DataMember]
        public LookupValue Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [DataMember]
        public List<Ways> WayList
        {
            get { return wayList; }
            set
            {
                wayList = value;
                OnPropertyChanged("WayList");
            }
        }

        [DataMember]
        public List<Options> OptionList
        {
            get { return optionList; }
            set
            {
                optionList = value;
                OnPropertyChanged("OptionList");
            }
        }

        [DataMember]
        public List<Detections> DetectionList
        {
            get { return detectionList; }
            set
            {
                detectionList = value;
                OnPropertyChanged("DetectionList");
            }
        }

        [DataMember]
        public List<SpareParts> SparePartList
        {
            get { return sparePartList; }
            set
            {
                sparePartList = value;
                OnPropertyChanged("SparePartList");
            }
        }

        [DataMember]
        public List<ConnectorFamilies> FamilyList
        {
            get { return familyList; }
            set
            {
                familyList = value;
                OnPropertyChanged("FamilyList");
            }
        }

        [DataMember]
        public Int64 IsEnabled
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
        public List<ProductTypeImage> ProductTypeImageList
        {
            get
            {
                return productTypeImageList;
            }

            set
            {
                productTypeImageList = value;
                OnPropertyChanged("ProductTypeImageList");
            }
        }

        [DataMember]
        public List<ProductTypeAttachedDoc> ProductTypeAttachedDocList
        {
            get
            {
                return productTypeAttachedDocList;
            }

            set
            {
                productTypeAttachedDocList = value;
                OnPropertyChanged("ProductTypeAttachedDocList");
            }
        }
        [DataMember]
        public List<ProductTypeAttachedLink> ProductTypeAttachedLinkList
        {
            get
            {
                return productTypeAttachedLinkList;
            }

            set
            {
                productTypeAttachedLinkList = value;
                OnPropertyChanged("ProductTypeAttachedLinkList");
            }
        }

        [DataMember]
        public DefaultWayType DefaultWayType
        {
            get
            {
                return defaultWayType;
            }

            set
            {
                defaultWayType = value;
                OnPropertyChanged("DefaultWayType");
            }
        }

        [DataMember]
        public List<ProductTypeLogEntry> ProductTypeLogEntryList
        {
            get
            {
                return productTypeLogEntryList;
            }

            set
            {
                productTypeLogEntryList = value;
                OnPropertyChanged("ProductTypeLogEntryList");
            }
        }

        [DataMember]
        public List<Template> TemplateList
        {
            get { return templateList; }
            set
            {
                templateList = value;
                OnPropertyChanged("TemplateList");
            }
        }

        [DataMember]
        public bool IsTemplate_NotExist
        {
            get
            {
                return isTemplate_NotExist;
            }

            set
            {
                isTemplate_NotExist = value;
                OnPropertyChanged("IsTemplate_NotExist");
            }
        }

        [DataMember]
        public List<Options> OptionList_Group
        {
            get
            {
                return optionList_Group;
            }

            set
            {
                optionList_Group = value;
                OnPropertyChanged("OptionList_Group");
            }
        }

        [DataMember]
        public List<Detections> DetectionList_Group
        {
            get
            {
                return detectionList_Group;
            }

            set
            {
                detectionList_Group = value;
                OnPropertyChanged("DetectionList_Group");
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
        public UInt64 IdTemplate_old
        {
            get { return idTemplate_old; }
            set
            {
                idTemplate_old = value;
                OnPropertyChanged("IdTemplate_old");
            }
        }

        [DataMember]
        public List<ProductTypeCompatibility> ProductTypeCompatibilityList
        {
            get
            {
                return productTypeCompatibilityList;
            }

            set
            {
                productTypeCompatibilityList = value;
                OnPropertyChanged("ProductTypeCompatibilityList");
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
            ProductTypes productTypes = (ProductTypes)this.MemberwiseClone();

            if (Template != null)
                productTypes.Template = (Template)this.Template.Clone();

            if (Status != null)
                productTypes.Status = (LookupValue)this.Status.Clone();

            if (DefaultWayType != null)
                productTypes.DefaultWayType = (DefaultWayType)this.DefaultWayType.Clone();

            if (TemplateList != null)
                productTypes.TemplateList = TemplateList.Select(x => (Template)x.Clone()).ToList();

            if (WayList != null)
                productTypes.WayList = WayList.Select(x => (Ways)x.Clone()).ToList();

            if (OptionList != null)
                productTypes.OptionList = OptionList.Select(x => (Options)x.Clone()).ToList();

            if (DetectionList != null)
                productTypes.DetectionList = DetectionList.Select(x => (Detections)x.Clone()).ToList();

            if (SparePartList != null)
                productTypes.SparePartList = SparePartList.Select(x => (SpareParts)x.Clone()).ToList();

            if (FamilyList != null)
                productTypes.FamilyList = FamilyList.Select(x => (ConnectorFamilies)x.Clone()).ToList();

            if (ProductTypeImageList != null)
                productTypes.ProductTypeImageList = ProductTypeImageList.Select(x => (ProductTypeImage)x.Clone()).ToList();

            if (ProductTypeAttachedDocList != null)
                productTypes.ProductTypeAttachedDocList = ProductTypeAttachedDocList.Select(x => (ProductTypeAttachedDoc)x.Clone()).ToList();

            if (ProductTypeAttachedLinkList != null)
                productTypes.ProductTypeAttachedLinkList = ProductTypeAttachedLinkList.Select(x => (ProductTypeAttachedLink)x.Clone()).ToList();

            if (ProductTypeLogEntryList != null)
                productTypes.ProductTypeLogEntryList = ProductTypeLogEntryList.Select(x => (ProductTypeLogEntry)x.Clone()).ToList();

            if (OptionList_Group != null)
                productTypes.OptionList_Group = OptionList_Group.Select(x => (Options)x.Clone()).ToList();

            if (DetectionList_Group != null)
                productTypes.DetectionList_Group = DetectionList_Group.Select(x => (Detections)x.Clone()).ToList();

            if (CustomerList != null)
                productTypes.CustomerList = CustomerList.Select(x => (RegionsByCustomer)x.Clone()).ToList();

            if (ProductTypeCompatibilityList != null)
                productTypes.ProductTypeCompatibilityList = ProductTypeCompatibilityList.Select(x => (ProductTypeCompatibility)x.Clone()).ToList();

            return productTypes;
        }

        public override string ToString()
        {
            return Name;
        }


        #endregion
    }
}
