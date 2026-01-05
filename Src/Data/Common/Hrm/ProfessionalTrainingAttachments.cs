using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("professionaltrainingAttachmentlist")]
    [DataContract]
    public class ProfessionalTrainingAttachments : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idProfessionalTrainingAttachment;
        UInt64 idProfessionalTraining;
        string savedFileName;
        string description;
        string originalFileName;
        UInt32 idCreator;
        DateTime createdIn;
        UInt32 idModifier;
        DateTime? modifiedIn;
        private int srNo;
        byte[] profTrainigAttachedDocInBytes;
        DateTime? updatedDate;
        string attachedFileName;
        #endregion

        #region Constructor

        public ProfessionalTrainingAttachments()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public Int32 SrNo
        {
            get { return srNo; }
            set
            {
                srNo = value;
                OnPropertyChanged("SrNo");
            }
        }
        [DataMember]
        public UInt32 IdProfessionalTrainingAttachment
        {
            get
            {
                return idProfessionalTrainingAttachment;
            }

            set
            {
                idProfessionalTrainingAttachment = value;
                OnPropertyChanged("IdProfessionalTrainingAttachment");
            }
        }

        [DataMember]
        public UInt64 IdProfessionalTraining
        {
            get
            {
                return idProfessionalTraining;
            }

            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged("IdProfessionalTraining");
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
        public UInt32 IdModifier
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
        public byte[] ProfTrainigAttachedDocInBytes
        {
            get
            {
                return profTrainigAttachedDocInBytes;
            }

            set
            {
               profTrainigAttachedDocInBytes = value;
                OnPropertyChanged("ProfTrainigAttachedDocInBytes");
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
        public string AttachedFileName
        {
            get
            {
                return attachedFileName;
            }

            set
            {
                attachedFileName = value;
                OnPropertyChanged("AttachedFileName");
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
            return this.MemberwiseClone();
        }

        #endregion
    }
}
