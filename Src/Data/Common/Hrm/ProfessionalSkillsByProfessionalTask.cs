using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ProfessionalSkillsByProfessionalTask : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idProfessionalSkillsByProfessionalTask;
        UInt64 idProfessionalTask;
        UInt64 idProfessionalSkill;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        ProfessionalTask professionalTask;
        ProfessionalSkill professionalSkill;
        #endregion

        #region Constructor

        public ProfessionalSkillsByProfessionalTask()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public ulong IdProfessionalSkillsByProfessionalTask
        {
            get
            {
                return idProfessionalSkillsByProfessionalTask;
            }

            set
            {
                idProfessionalSkillsByProfessionalTask = value;
                OnPropertyChanged("IdProfessionalSkillsByProfessionalTask");
            }
        }


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
        public ulong IdProfessionalSkill
        {
            get
            {
                return idProfessionalSkill;
            }

            set
            {
                idProfessionalSkill = value;
                OnPropertyChanged("IdProfessionalSkill");
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
        public ProfessionalTask ProfessionalTask
        {
            get
            {
                return professionalTask;
            }

            set
            {
                professionalTask = value;
                OnPropertyChanged("ProfessionalTask");
            }
        }

        [DataMember]
        public ProfessionalSkill ProfessionalSkill
        {
            get
            {
                return professionalSkill;
            }

            set
            {
                professionalSkill = value;
                OnPropertyChanged("ProfessionalSkill");
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
            ProfessionalSkillsByProfessionalTask professionalSkillsByProfessionalTask = (ProfessionalSkillsByProfessionalTask)this.MemberwiseClone();

            if (ProfessionalTask != null)
                professionalSkillsByProfessionalTask.ProfessionalTask = (ProfessionalTask)this.ProfessionalTask.Clone();

            if (ProfessionalSkill != null)
                professionalSkillsByProfessionalTask.ProfessionalSkill = (ProfessionalSkill)this.ProfessionalSkill.Clone();

            return professionalSkillsByProfessionalTask;
        }
        #endregion
    }
}