using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("professionaltrainingresultlist")]
    [DataContract]
    public class ProfessionalTrainingResults : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idProfessionalTrainingResult;
        UInt64 idProfessionalTraining;
        UInt64 idProfessionalTrainingSkillResult;
        Int32 idEmployee;
        Int32 idProfessionalSkill;
        string idEmp;
        Employee employee;
        float? duration;
        Attachment attachment;
        Int32? idCreator;
        DateTime? creationDate;
        Int32? idModifier;
        DateTime? modificationDate;
        UInt32 idClassification;
        string employeeCode;
        string firstname;
        string description;
        LookupValue classification;
        string classificationValue;
        string lastname;
        string skillName;
        List<ProfessionalSkill> selectedTrainingResultSkill;
        private List<Employee> employeeListForResult;
        List<ProfessionalTrainingResults> selectedTraineeList;
        List<ProfessionalSkill> traineeSkillList;
        private float? resultValue;
        float? skillDuration1;
        float? skillDuration2;
        float? skillDuration3;
        float? skillDuration4;
        float? skillDuration5;
        bool isEnable;
        float? results;
        byte[] traineeDocumentFileInBytes;
        string resultFileName;
        bool isResultFileDeleted;
        private int srNo;
        string resultRemark;
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

        [NotMapped]
        [DataMember]
        public bool IsResultFileDeleted
        {
            get { return isResultFileDeleted; }
            set
            {
                isResultFileDeleted = value;
                OnPropertyChanged("IsResultFileDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public string ResultFileName
        {
            get { return resultFileName; }
            set
            {
                resultFileName = value;
                OnPropertyChanged("ResultFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public string ResultRemark
        {
            get { return resultRemark; }
            set
            {
                resultRemark = value;
                OnPropertyChanged("ResultRemark");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] TraineeDocumentFileInBytes
        {
            get { return traineeDocumentFileInBytes; }
            set
            {
                traineeDocumentFileInBytes = value;
                OnPropertyChanged("TraineeDocumentFileInBytes");
            }
        }
        [DataMember]
        public float? Results
        {
            get { return results; }
            set
            {
                results = value;
                OnPropertyChanged(("Results"));
            }
        }
        [DataMember]
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                OnPropertyChanged(("IsEnable"));
            }
        }
        [DataMember]
        public float? ResultValue
        {
            get { return resultValue; }
            set
            {
                resultValue = value;
                OnPropertyChanged(("ResultValue"));
            }
        }
        [DataMember]
        public float? SkillDuration1
        {
            get { return skillDuration1; }
            set
            {
                skillDuration1 = value;
                OnPropertyChanged(("SkillDuration1"));
            }
        }
        [DataMember]
        public float? SkillDuration2
        {
            get { return skillDuration2; }
            set
            {
                skillDuration2 = value;
                OnPropertyChanged(("SkillDuration2"));
            }
        }
        [DataMember]
        public float? SkillDuration3
        {
            get { return skillDuration3; }
            set
            {
                skillDuration3 = value;
                OnPropertyChanged(("SkillDuration3"));
            }
        }
        [DataMember]
        public float? SkillDuration4
        {
            get { return skillDuration4; }
            set
            {
                skillDuration4 = value;
                OnPropertyChanged(("SkillDuration4"));
            }
        }
        [DataMember]
        public float? SkillDuration5
        {
            get { return skillDuration5; }
            set
            {
                skillDuration5 = value;
                OnPropertyChanged(("SkillDuration5"));
            }
        }
        [DataMember]
        public List<Employee> EmployeeListForResult
        {
            get
            {
                return employeeListForResult;
            }

            set
            {
                employeeListForResult = value;
                OnPropertyChanged("EmployeeListForResult");
            }
        }
        [Key]
        [Column("IdProfessionalTrainingResult")]
        [DataMember]
        public UInt64 IdProfessionalTrainingResult
        {
            get { return idProfessionalTrainingResult; }
            set
            {
                idProfessionalTrainingResult = value;
                OnPropertyChanged("IdProfessionalTrainingResult");
            }
        }
        [Key]
        [Column("idProfessionalSkill")]
        [DataMember]
        public Int32 IdProfessionalSkill
        {
            get { return idProfessionalSkill; }
            set
            {
                idProfessionalSkill = value;
                OnPropertyChanged("idProfessionalSkill");
            }
        }
        [Column("EmployeeCode")]
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        [Column("FirstName")]
        [DataMember]
        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged("FirstName");
            }
        }
        [Column("LastName")]
        [DataMember]
        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged("LastName");
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

        [Column("IdProfessionalTraining")]
        [DataMember]
        public UInt64 IdProfessionalTraining
        {
            get { return idProfessionalTraining; }
            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged("IdProfessionalTraining");
            }
        }

        [Column("IdProfessionalTrainingSkillResult")]
        [DataMember]
        public UInt64 IdProfessionalTrainingSkillResult
        {
            get { return idProfessionalTrainingSkillResult; }
            set
            {
                idProfessionalTrainingSkillResult = value;
                OnPropertyChanged("IdProfessionalTrainingSkillResult");
            }
        }
        [Column("IdEmployee")]
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
        [Column("IdEmp")]
        [DataMember]
        public string  IdEmp
        {
            get { return idEmp; }
            set
            {
                idEmp = value;
                OnPropertyChanged("IdEmp");
            }
        }

        [DataMember]
        [NotMapped]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }
        [Column("Duration")]
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
        [Column("Attachment")]
        [DataMember]
        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged("Attachment");
            }
        }
        [NotMapped]
        [DataMember]
        public UInt32 IdClassification
        {
            get { return idClassification; }
            set
            {
                idClassification = value;
                OnPropertyChanged("IdClassification");
            }
        }
        [NotMapped]
        [DataMember]
        public LookupValue Classification
        {
            get { return classification; }
            set
            {
                classification = value;
                OnPropertyChanged("Classification");
            }
        }
        public string ClassificationValue
        {
            get { return classificationValue; }
            set
            {
                classificationValue = value;
                OnPropertyChanged("ClassificationValue");
            }
        }
        [NotMapped]
        [DataMember]
        public string SkillName
        {
            get { return skillName; }
            set
            {
                skillName = value;
                OnPropertyChanged("SkillName");
            }
        }
        [Column("IdCreator")]
        [DataMember]
        public Int32? IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }

        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }

        }

        [Column("IdModifier")]
        [DataMember]
        public Int32? IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }

        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }

        }
        [NotMapped]
        [DataMember]
        public List<ProfessionalSkill> SelectedTrainingResultSkillList
        {
            get
            {
                return selectedTrainingResultSkill;
            }

            set
            {
                this.selectedTrainingResultSkill = value;
                OnPropertyChanged(nameof(SelectedTrainingResultSkillList));
            }
        }
        [NotMapped]
        [DataMember]
        public List<ProfessionalTrainingResults> SelectedTraineesList
        {
            get
            {
                return selectedTraineeList;
            }

            set
            {
                this.selectedTraineeList = value;
                OnPropertyChanged(nameof(SelectedTraineesList));
            }
        }
        [DataMember]
        [NotMapped]
        public List<ProfessionalSkill> TraineeSkillList
        {
            get { return traineeSkillList; }
            set
            {
                traineeSkillList = value;
                OnPropertyChanged("TraineeSkillList");
            }
        }
        #endregion

        #region Constructor

        public ProfessionalTrainingResults()
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
            ProfessionalTrainingResults professionalTrainingResults = (ProfessionalTrainingResults)this.MemberwiseClone();
            if (professionalTrainingResults.SelectedTrainingResultSkillList != null)
                professionalTrainingResults.SelectedTrainingResultSkillList = SelectedTrainingResultSkillList.Select(x => (ProfessionalSkill)x.Clone()).ToList();
            return professionalTrainingResults;
        }

        #endregion
    }
}
