using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("professional_Skills")]
    [DataContract]
    public class ProfessionalSkill : ModelBase, IDisposable
    {
        #region Fields

        Int32 idProfessionalSkill;
        string code;
        string name;
        string description;
        Int32 idSkillType;
        bool inUse;
        Int32 createdBy;
        DateTime createdIn;
        Int32? modifiedBy;
        DateTime? modifiedIn;
        LookupValue skillType;
        private string type;
        private float? duration;
        Int32 idProfessionalTrainingSkill;
        private int idProfessionalTraining;
        private bool isEnabled;
        private float? averageOfResult;
        private float? resultDuration;
        private UInt64 idProfessionalTrainingSkillResult;
        #endregion

        #region Properties
        [DataMember]
        public UInt64 IdProfessionalTrainingSkillResult
        {
            get { return idProfessionalTrainingSkillResult; }
            set
            {
                idProfessionalTrainingSkillResult = value;
                OnPropertyChanged(("IdProfessionalTrainingSkillResult"));
            }
        }
        [Column("AverageOfResult")]
        [DataMember]
        public float? AverageOfResult
        {
            get { return averageOfResult; }
            set
            {
                averageOfResult = value;
                OnPropertyChanged("AverageOfResult");
            }
        }

        [Key]
        [Column("IdProfessionalSkill")]
        [DataMember]
        public Int32 IdProfessionalSkill
        {
            get { return idProfessionalSkill; }
            set
            {
                idProfessionalSkill = value;
                OnPropertyChanged("IdProfessionalSkill");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }


        [Column("IdSkillType")]
        [DataMember]
        public Int32 IdSkillType
        {
            get { return idSkillType; }
            set
            {
                idSkillType = value;
                OnPropertyChanged("IdSkillType");
            }
        }

        [Column("InUse")]
        [DataMember]
        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [Column("CreatedBy")]
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

        [Column("CreatedIn")]
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

        [Column("ModifiedBy")]
        [DataMember]
        public Int32? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
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
        public LookupValue SkillType
        {
            get { return skillType; }
            set
            {
                skillType = value;
                OnPropertyChanged("SkillType");
            }
        }

        [NotMapped]
        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        [NotMapped]
        [DataMember]
        public float? Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }
        [NotMapped]
        [DataMember]
        public float? ResultDuration
        {
            get { return resultDuration; }
            set
            {
                if (value == null)
                    value = 0;

                resultDuration = value;
                OnPropertyChanged("ResultDuration");
            }
        }

        [DataMember]
        public Int32 IdProfessionalTrainingSkill
        {
            get { return idProfessionalTrainingSkill; }
            set
            {
                idProfessionalTrainingSkill = value;
                OnPropertyChanged("IdProfessionalTrainingSkill");
            }
        }

        [DataMember]
        public Int32 IdProfessionalTraining
        {
            get { return idProfessionalTraining; }
            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged("IdProfessionalTraining");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #endregion

        #region Constructor

        public ProfessionalSkill()
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
