using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.jangra][GEOS2-5549]
    [DataContract]
    public class EquipmentAndToolsAttachedDoc : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idJobDescriptionEquipment;
        UInt32 idJobDescriptionEquipmentAndToolsAttachedDoc;
        UInt64 idJobDescription;
        bool isMandatory;
        Int32 idLookupValue;
        Int32 idParent;
        string equipmentType;
        DateTime? startDate;
        DateTime? endDate;
        string savedFileName;
        string originalFileName;
        string remarks;
        byte[] equipmentAndToolsAttachedDocInBytes;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime modifiedIn;
        string categoryType;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public UInt32 IdJobDescriptionEquipmentAndToolsAttachedDoc
        {
            get { return idJobDescriptionEquipmentAndToolsAttachedDoc; }
            set
            {
                idJobDescriptionEquipmentAndToolsAttachedDoc = value;
                OnPropertyChanged("IdJobDescriptionEquipmentAndToolsAttachedDoc");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt64 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        [NotMapped]
        [DataMember]
        public string EquipmentType
        {
            get { return equipmentType; }
            set
            {
                equipmentType = value;
                OnPropertyChanged("EquipmentType");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsMandatory
        {
            get { return isMandatory; }
            set
            {
                isMandatory = value;
                OnPropertyChanged("IsMandatory");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [NotMapped]
        [DataMember]
        public string SavedFileName
        {
            get { return savedFileName; }
            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public string OriginalFileName
        {
            get { return originalFileName; }
            set
            {
                originalFileName = value;
                OnPropertyChanged("OriginalFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] EquipmentAndToolsAttachedDocInBytes
        {
            get { return equipmentAndToolsAttachedDocInBytes; }
            set
            {
                equipmentAndToolsAttachedDocInBytes = value;
                OnPropertyChanged("EquipmentAndToolsAttachedDocInBytes");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdJobDescriptionEquipment
        {
            get { return idJobDescriptionEquipment; }
            set
            {
                idJobDescriptionEquipment = value;
                OnPropertyChanged("IdJobDescriptionEquipment");
            }
        }

        [NotMapped]
        [DataMember]
        public string CategoryType
        {
            get { return categoryType; }
            set
            {
                categoryType = value;
                OnPropertyChanged("CategoryType");
            }
        }
        #endregion

        #region Constructor
        public EquipmentAndToolsAttachedDoc()
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
            EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc = (EquipmentAndToolsAttachedDoc)this.MemberwiseClone();
            return equipmentAndToolsAttachedDoc;
        }


        #endregion
    }
}
