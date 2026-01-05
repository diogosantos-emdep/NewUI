using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ProfessionalObjective : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idProfessionalObjective;
        string code;
        string description;
        uint idJobDescription;
        bool inUse;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        JobDescription jobDescription;
        string codeWithDescription;
        #endregion

        #region Constructor

        public ProfessionalObjective()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public ulong IdProfessionalObjective
        {
            get
            {
                return idProfessionalObjective;
            }

            set
            {
                idProfessionalObjective = value;
                OnPropertyChanged("IdProfessionalObjective");
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
        public uint IdJobDescription
        {
            get
            {
                return idJobDescription;
            }

            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [DataMember]
        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [DataMember]
        public uint CreatedBy
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
        public uint ModifiedBy
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
        public JobDescription JobDescription
        {
            get
            {
                return jobDescription;
            }

            set
            {
                jobDescription = value;
                OnPropertyChanged("JobDescription");
            }
        }

        [DataMember]
        public string CodeWithDescription
        {
            get
            {
                return codeWithDescription;
            }

            set
            {
                codeWithDescription = value;
                OnPropertyChanged("CodeWithDescription");
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
            ProfessionalObjective professionalObjective = (ProfessionalObjective)this.MemberwiseClone();

            if (JobDescription != null)
                professionalObjective.JobDescription = (JobDescription)this.JobDescription.Clone();

            return professionalObjective;
        }
        #endregion
    }
}
