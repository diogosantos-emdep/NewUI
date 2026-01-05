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
    public class DetectionDetails : ModelBase, IDisposable
    {
        #region Fields

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

        #endregion

        #region Constructor

        public DetectionDetails()
        {

        }

        #endregion

        #region Properties

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
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override object Clone()
        {
            DetectionDetails detectionDetails = (DetectionDetails)this.MemberwiseClone();

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

            return detectionDetails;
        }
        #endregion
    }
}
