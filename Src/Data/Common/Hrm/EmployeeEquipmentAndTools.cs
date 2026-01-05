using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.Jangra][GEOS2-5579]
    [DataContract]
    public class EmployeeEquipmentAndTools : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idJobDescriptionEquipment;
        private Int32 idJobDescription;
        private Int32 idCategory;
        private string category;
        private Int32 idEquipment;
        private string equipmentType;
        private bool isMandatory;
        private string remarks;
        private string expectedDuration;
        private DateTime startDate;
        private DateTime endDate;
        private string originalFileName;
        private string savedFileName;
        private byte[] equipmentAndToolsAttachedDocInBytes;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 idEmployeeEquipment;
        private string fileLocation;
        private Int32 idEmployee;
        private string oldFileName;
        private bool isFieldExpired;
        #endregion


        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdJobDescriptionEquipment
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
        public Int32 IdJobDescription
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
        public Int32 IdCategory
        {
            get { return idCategory; }
            set
            {
                idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdEquipment
        {
            get { return idEquipment; }
            set
            {
                idEquipment = value;
                OnPropertyChanged("IdEquipment");
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
        public string ExpectedDuration
        {
            get { return expectedDuration; }
            set
            {
                expectedDuration = value;
                OnPropertyChanged("ExpectedDuration");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime StartDate
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
        public DateTime EndDate
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
        public Int32 CreatedBy
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
        public Int32 IdEmployeeEquipment
        {
            get { return idEmployeeEquipment; }
            set
            {
                idEmployeeEquipment = value;
                OnPropertyChanged("IdEmployeeEquipment");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileLocation
        {
            get { return fileLocation; }
            set
            {
                fileLocation = value;
                OnPropertyChanged("FileLocation");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [NotMapped]
        [DataMember]
        public string OldFileName
        {
            get { return oldFileName; }
            set
            {
                oldFileName = value;
                OnPropertyChanged("OldFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsFieldExpired
        {
            get { return isFieldExpired; }
            set
            {
                isFieldExpired = value;
                OnPropertyChanged("IsFieldExpired");
            }
        }

        #endregion


        #region Constructor
        public EmployeeEquipmentAndTools()
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
            return this.MemberwiseClone();
        }
        #endregion
    }
}
