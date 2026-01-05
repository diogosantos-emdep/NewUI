using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ProfessionalTask : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idProfessionalTask;
        string code;
        string description;
        string jobRequirement;
        UInt64 idProfessionalObjective;
        bool inUse;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        ProfessionalObjective professionalObjective;

        List<ProfessionalSkill> professionalSkillList;

        string skills;
        #endregion

        #region Constructor

        public ProfessionalTask()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public ulong IdProfessionalTask
        {
            get
            {
                return idProfessionalTask;
            }

            set
            {
                idProfessionalTask = value;
                OnPropertyChanged("IdProfessionalTask");
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
        public string JobRequirement
        {
            get
            {
                return jobRequirement;
            }

            set
            {
                jobRequirement = value;
                OnPropertyChanged("JobRequirement");
            }
        }

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
        public ProfessionalObjective ProfessionalObjective
        {
            get
            {
                return professionalObjective;
            }

            set
            {
                professionalObjective = value;
                OnPropertyChanged("ProfessionalObjective");
            }
        }

        [DataMember]
        public List<ProfessionalSkill> ProfessionalSkillList
        {
            get
            {
                return professionalSkillList;
            }

            set
            {
                professionalSkillList = value;
                OnPropertyChanged("ProfessionalSkillList");
            }
        }

        [DataMember]
        public string Skills
        {
            get
            {
                return skills;
            }

            set
            {
                skills = value;
                OnPropertyChanged("Skills");
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
            ProfessionalTask professionalTask = (ProfessionalTask)this.MemberwiseClone();

            if (ProfessionalObjective != null)
                professionalTask.ProfessionalObjective = (ProfessionalObjective)this.ProfessionalObjective.Clone();


            if (ProfessionalSkillList != null)
                professionalTask.ProfessionalSkillList = ProfessionalSkillList.Select(x => (ProfessionalSkill)x.Clone()).ToList();

            return professionalTask;
        }
        #endregion
    }
}
