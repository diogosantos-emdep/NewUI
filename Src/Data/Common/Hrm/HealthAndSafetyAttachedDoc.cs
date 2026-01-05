using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.Jangra][GEOS2-5549]
    [DataContract]
    public class HealthAndSafetyAttachedDoc : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idJobDescriptionHealthAndSafetyAttachedDoc;
        UInt64 idJobDescription;
        UInt32 idCompany;
        string title;
        string remarks;
        string savedFileName;
        byte[] healthAndSafetyAttachedDocInBytes;
        string originalFileName;

        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime modifiedIn;
        string plant;
        string idPlants;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public UInt32 IdJobDescriptionHealthAndSafetyAttachedDoc
        {
            get { return idJobDescriptionHealthAndSafetyAttachedDoc; }
            set
            {
                idJobDescriptionHealthAndSafetyAttachedDoc = value;
                OnPropertyChanged("IdJobDescriptionHealthAndSafetyAttachedDoc");
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
        public UInt32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
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
        public byte[] HealthAndSafetyAttachedDocInBytes
        {
            get { return healthAndSafetyAttachedDocInBytes; }
            set
            {
                healthAndSafetyAttachedDocInBytes = value;
                OnPropertyChanged("HealthAndSafetyAttachedDocInBytes");
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
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [NotMapped]
        [DataMember]
        public string IdPlants
        {
            get { return idPlants; }
            set
            {
                idPlants = value;
                OnPropertyChanged("IdPlants");
            }
        }
        #endregion

        #region Constructor
        public HealthAndSafetyAttachedDoc()
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
