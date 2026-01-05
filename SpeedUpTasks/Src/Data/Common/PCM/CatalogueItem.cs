using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class CatalogueItem : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idCatalogueItem;
        string code;
        UInt64 idTemplate;
        UInt64 idCPType;
        string name;
        string name_es;
        string name_fr;
        string name_ro;
        string name_zh;
        string name_pt;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        UInt32? parent;
        Int32 idStatus;
        UInt32 createdBy;
        DateTime? createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        Int64 isEnabled;

        Template template;
        ProductTypes productType;
        DateTime? lastUpdate;
        LookupValue status;

        List<Ways> wayList;
        List<Options> optionList;
        List<Detections> detectionList;
        List<SpareParts> sparePartList;
        List<ProductTypes> productTypeList;
        List<ConnectorFamilies> familyList;
        List<CatalogueItemAttachedDoc> fileList;
        List<CatalogueItemAttachedLink> catalogueItemAttachedLinkList;

        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdCatalogueItem
        {
            get { return idCatalogueItem; }
            set
            {
                idCatalogueItem = value;
                OnPropertyChanged("IdCatalogueItem");
            }
        }

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
        public UInt64 IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }
        [DataMember]
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }
        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }
        [DataMember]
        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        [DataMember]
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }
        [DataMember]
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }
        [DataMember]
        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }
        [DataMember]
        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }
        [DataMember]
        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }
        [DataMember]
        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }
        
        [DataMember]
        public UInt32? Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
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
        public ProductTypes ProductType
        {
            get { return productType; }
            set
            {
                productType = value;
                OnPropertyChanged("ProductType");
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
        public List<ProductTypes> ProductTypeList
        {
            get { return productTypeList; }
            set
            {
                productTypeList = value;
                OnPropertyChanged("ProductTypeList");
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
        public List<CatalogueItemAttachedDoc> FileList
        {
            get
            {
                return fileList;
            }

            set
            {
                fileList = value;
                OnPropertyChanged("FileList");
            }
        }
        [DataMember]
        public List<CatalogueItemAttachedLink> CatalogueItemAttachedLinkList
        {
            get
            {
                return catalogueItemAttachedLinkList;
            }

            set
            {
                catalogueItemAttachedLinkList = value;
                OnPropertyChanged("CatalogueItemAttachedLinkList");
            }
        }
        #endregion

        #region Constructor
        public CatalogueItem()
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
            CatalogueItem catalogueitem = (CatalogueItem)this.MemberwiseClone();

            if (Template != null)
                catalogueitem.Template = (Template)this.Template.Clone();

            if (ProductType != null)
                catalogueitem.ProductType = (ProductTypes)this.ProductType.Clone();

            if (Status != null)
                catalogueitem.Status = (LookupValue)this.Status.Clone();

            if (WayList != null)
                catalogueitem.WayList = WayList.Select(x => (Ways)x.Clone()).ToList();

            if (OptionList != null)
                catalogueitem.OptionList = OptionList.Select(x => (Options)x.Clone()).ToList();

            if (DetectionList != null)
                catalogueitem.DetectionList = DetectionList.Select(x => (Detections)x.Clone()).ToList();

            if (SparePartList != null)
                catalogueitem.SparePartList = SparePartList.Select(x => (SpareParts)x.Clone()).ToList();

            if (ProductTypeList != null)
                catalogueitem.ProductTypeList = ProductTypeList.Select(x => (ProductTypes)x.Clone()).ToList();

            if (FamilyList != null)
                catalogueitem.FamilyList = FamilyList.Select(x => (ConnectorFamilies)x.Clone()).ToList();

            if (FileList != null)
                catalogueitem.FileList = FileList.Select(x => (CatalogueItemAttachedDoc)x.Clone()).ToList();

            if (CatalogueItemAttachedLinkList != null)
                catalogueitem.CatalogueItemAttachedLinkList = CatalogueItemAttachedLinkList.Select(x => (CatalogueItemAttachedLink)x.Clone()).ToList();

            return catalogueitem;
        }
        #endregion
    }
}
