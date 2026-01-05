using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorFamily : ModelBase, IDisposable
    {
        #region Fields
        string familyname;
        string oldname;
        Int32 idFamily;
        Int32 idFamilyimageIdFamily;
        Int32 idSubFamilyConnector;
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
        string isInUse;
        string parent;
        string key;
        UInt64 idSCMSubFamilyImage;
        UInt64 idSCMFamilyImage;
        string savedFileName;
        string originalFileName;
        UInt32 createdBy;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt64 position;
        private List<ConnectorSubFamily> subFamilyList;
        byte[] connectorFamilyImageInBytes;
        string savedFileNameParent;
        List<FamilyImage> familyImagesList;
        string filePath; 
        LookUpValues connectorType;
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdFamily
        {
            get
            {
                return idFamily;
            }

            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }

        [DataMember]
        public Int32 IdSubFamilyConnector
        {
            get
            {
                return idSubFamilyConnector;
            }

            set
            {
                idSubFamilyConnector = value;
                OnPropertyChanged("IdSubFamilyConnector");
            }
        }
        [DataMember]
        public string Familyname
        {
            get
            {
                return familyname;
            }

            set
            {
                familyname = value;
                OnPropertyChanged("Familyname");
            }
        }
        [DataMember]
        public string OldName
        {
            get
            {
                return oldname;
            }

            set
            {
                oldname = value;
                OnPropertyChanged("OldName");
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
        public string IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged("IsInUse");
            }
        }

        [DataMember]
        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public UInt64 IdSCMFamilyImage
        {
            get
            {
                return idSCMFamilyImage;
            }

            set
            {
                idSCMFamilyImage = value;
                OnPropertyChanged("IdSCMFamilyImage");
            }
        }

        [DataMember]
        public UInt64 IdSCMSubFamilyImage
        {
            get
            {
                return idSCMSubFamilyImage;
            }

            set
            {
                idSCMSubFamilyImage = value;
                OnPropertyChanged("IdSCMSubFamilyImage");
            }
        }

        [DataMember]
        public string SavedFileName
        {
            get
            {
                return savedFileName;
            }

            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [DataMember]
        public string OriginalFileName
        {
            get
            {
                return originalFileName;
            }

            set
            {
                originalFileName = value;
                OnPropertyChanged("OriginalFileName");
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
        public UInt64 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [DataMember]
        public byte[] ConnectorFamilyImageInBytes
        {
            get
            {
                return connectorFamilyImageInBytes;
            }

            set
            {
                connectorFamilyImageInBytes = value;
                OnPropertyChanged("ConnectorFamilyImageInBytes");
            }
        }
        [DataMember]
        public string SavedFileNameParent
        {
            get
            {
                return savedFileNameParent;
            }

            set
            {
                savedFileNameParent = value;
                OnPropertyChanged("SavedFileNameParent");
            }
        }

        [DataMember]
        public List<FamilyImage> FamilyImagesList
        {
            get
            {
                return familyImagesList;
            }

            set
            {
                familyImagesList = value;
                OnPropertyChanged("FamilyImagesList");
            }
        }

        [DataMember]
        public Int32 IdFamilyimageIdFamily
        {
            get
            {
                return idFamilyimageIdFamily;
            }

            set
            {
                idFamilyimageIdFamily = value;
                OnPropertyChanged("IdFamilyimageIdFamily");
            }
        }

        [DataMember]
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        [DataMember]        
        public List<ConnectorSubFamily> SubFamilyList
        {
            get
            {
                return subFamilyList;
            }

            set
            {
                subFamilyList = value;
                OnPropertyChanged("SubFamilyList");
            }
        }

        [DataMember]
        public LookUpValues ConnectorType
        {
            get
            {
                return connectorType;
            }

            set
            {
                connectorType = value;
                OnPropertyChanged("ConnectorType");
            }
        }
        #endregion

        #region Constructor
        public ConnectorFamily()
        {

        }
        #endregion

        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            ConnectorFamily connectorFamily = (ConnectorFamily)this.MemberwiseClone();

            if (FamilyImagesList != null)
                connectorFamily.FamilyImagesList = FamilyImagesList.Select(x => (FamilyImage)x.Clone()).ToList();

            if (SubFamilyList != null)
                connectorFamily.SubFamilyList = SubFamilyList.Select(x => (ConnectorSubFamily)x.Clone()).ToList();

            return connectorFamily;
        }
        #endregion
    }
}
