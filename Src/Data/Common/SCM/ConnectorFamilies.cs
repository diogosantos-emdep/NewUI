using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorFamilies : ModelBase, IDisposable
    {

        #region Fields
        Int32 idFamily;
        Int32 idFamilyimage;
        Int32 idSubFamily;
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
        sbyte inUseYesNo;
        string isInUseParent;
        sbyte inUseYesNoParent;
        string parentDescription;
        string subFamiliesInUseYesNo;
        sbyte subFamiliesInUse;
        string subFamilyName;
        string parent;
        string key;
        string parentName;
        UInt64 idSCMSubFamilyImage;
        UInt64 idSCMFamilyImage;
        string savedFileName;
      
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt64 position;

        byte[] connectorFamilyImageInBytes;
        DateTime? updatedDate;
        ImageSource attachmentImage;
        byte[] connectorFamilyImageInBytesParent;
        string savedFileNameParent;
        UInt64 positionParent;
        UInt64 positionParenttemp;
        UInt64 positiontemp;
        List<FamilyImage> familyImagesList;
        string filePath;
        string parentFilePath;
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

        //public Int32 IdFamilySubimage
        //{
        //    get
        //    {
        //        return idFamily;
        //    }

        //    set
        //    {
        //        idFamily = value;
        //        OnPropertyChanged("IdFamily");
        //    }
        //}

        public Int32 IdFamilyimage
        {
            get
            {
                return idFamilyimage;
            }

            set
            {
                idFamilyimage = value;
                OnPropertyChanged("IdFamilyimage");
            }
        }

        [DataMember]
        public Int32 IdSubFamily
        {
            get
            {
                return idSubFamily;
            }

            set
            {
                idSubFamily = value;
                OnPropertyChanged("IdSubFamily");
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

     
        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public sbyte InUseYesNo
        {
            get { return inUseYesNo; }
            set
            {
                inUseYesNo = value;
                OnPropertyChanged("InUseYesNo");
            }
        }
        [NotMapped]
        [DataMember]
        public string IsInUseParent
        {
            get { return isInUseParent; }
            set
            {
                isInUseParent = value;
                OnPropertyChanged("IsInUseParent");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte InUseYesNoParent
        {
            get { return inUseYesNoParent; }
            set
            {
                inUseYesNoParent = value;
                OnPropertyChanged("InUseYesNoParent");
            }
        }
        [DataMember]
        public string ParentDescription
        {
            get
            {
                return parentDescription;
            }

            set
            {
                parentDescription = value;
                OnPropertyChanged("ParentDescription");
            }
        }
        [NotMapped]
        [DataMember]
        public string SubFamiliesInUseYesNo
        {
            get { return subFamiliesInUseYesNo; }
            set
            {
                subFamiliesInUseYesNo = value;
                OnPropertyChanged("SubFamiliesInUseYesNo");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte SubFamiliesIsInUse
        {
            get { return subFamiliesInUse; }
            set
            {
                subFamiliesInUse = value;
                OnPropertyChanged("SubFamiliesInUse");
            }
        }
        [DataMember]
        public string SubFamilyName
        {
            get
            {
                return subFamilyName;
            }

            set
            {
                subFamilyName = value;
                OnPropertyChanged("SubFamilyName");
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
        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
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
        public DateTime CreatedIn
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
        public byte[] ConnectorFamilyImageInBytesParent
        {
            get
            {
                return connectorFamilyImageInBytesParent;
            }

            set
            {
                connectorFamilyImageInBytesParent = value;
                OnPropertyChanged("ConnectorFamilyImageInBytesParent");
            }
        }
        [DataMember]
        public DateTime? UpdatedDate
        {
            get
            {
                return updatedDate;
            }

            set
            {
                updatedDate = value;
                OnPropertyChanged("UpdatedDate");
            }
        }

        [DataMember]
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }

            set
            {
                attachmentImage = value;
                OnPropertyChanged("AttachmentImage");
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
        public UInt64 PositionParent
        {
            get
            {
                return positionParent;
            }

            set
            {
                positionParent = value;
                OnPropertyChanged("PositionParent");
            }
        }
        [DataMember]
        [NotMapped]
        public UInt64 PositionParenttemp
        {
            get
            {
                return positionParenttemp;
            }

            set
            {
                positionParenttemp = value;
                OnPropertyChanged("positionParenttemp");
            }
        }

        [DataMember]
        [NotMapped]
        public UInt64 Positiontemp
        {
            get
            {
                return positiontemp;
            }

            set
            {
                positiontemp = value;
                OnPropertyChanged("Positiontemp");
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
        public string ParentFilePath
        {
            get
            {
                return parentFilePath;
            }

            set
            {
                parentFilePath = value;
                OnPropertyChanged("ParentFilePath");
            }
        }

        [DataMember]
        private List<Subfamily> subFamilyList;
        public List<Subfamily> SubFamilyList
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

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
