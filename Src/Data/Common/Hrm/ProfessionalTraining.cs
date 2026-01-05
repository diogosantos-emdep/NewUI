using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ProfessionalTraining : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idProfessionalTraining;
        string code;
        string name;
        string description;
        DateTime expectedDate;
        DateTime? finalizationDate;
        UInt32 idStatus;
        LookupValue status;
        UInt32 idType;
        LookupValue type;
        UInt32 idAcceptance;
        LookupValue acceptance;
        string acceptanceValue;
        UInt32 idResponsible;
        Employee responsible;
        UInt32? idTrainer;
        Employee trainer;
        string externalTrainer;
        string externalEntity;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        float duration;
        private List<ProfessionalSkill> professionalSkillList;
        private List<Employee> traineeList;
        private List<ProfessionalTrainingResults> professionalTrainingResults;
        string resultSkill1;
        string resultSkill2;
        string resultSkill3;
        string resultSkill4;
        string resultSkill5;
        bool visibleResultSkill1;
        bool visibleResultSkill2;
        bool visibleResultSkill3;
        bool visibleResultSkill4;
        bool visibleResultSkill5;
        string skillName1;
        string skillName2;
        string skillName3;
        string skillName4;
        string skillName5;
        string resultFileName;
        List<EmployeeChangelog> employeeProfessionalTrainingChangeLog;
        private string remarkForProfEdu;
        private List<ProfessionalTrainingAttachments> professionalTrainingAttachmentList;
        List<TrainingChangeLog> trainingAllChangeLog;
        #endregion

        #region Constructor

        public ProfessionalTraining()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public List<TrainingChangeLog> TrainingAllChangeLog
        {
            get { return trainingAllChangeLog; }
            set
            {
                trainingAllChangeLog = value;
                OnPropertyChanged("TrainingAllChangeLog");
            }
        }

        [DataMember]
        public List<EmployeeChangelog> EmployeeProfessionalTrainingChangeLog
        {
            get { return employeeProfessionalTrainingChangeLog; }
            set
            {
                employeeProfessionalTrainingChangeLog = value;
                OnPropertyChanged("EmployeeProfessionalTrainingChangeLog");
            }
        }

        [DataMember]
        public string RemarkForProfEdu
        {
            get { return remarkForProfEdu; }
            set
            {
                remarkForProfEdu = value;
                OnPropertyChanged("RemarkForProfEdu");
            }
        }
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
        [DataMember]
        public string SkillName1
        {
            get { return skillName1; }
            set
            {
                skillName1 = value;
                OnPropertyChanged("SkillName1");
            }
        }
        [DataMember]
        public string SkillName2
        {
            get { return skillName2; }
            set
            {
                skillName2 = value;
                OnPropertyChanged("SkillName2");
            }
        }
        [DataMember]
        public string SkillName3
        {
            get { return skillName3; }
            set
            {
                skillName3 = value;
                OnPropertyChanged("SkillName3");
            }
        }
        [DataMember]
        public string SkillName4
        {
            get { return skillName4; }
            set
            {
                skillName4 = value;
                OnPropertyChanged("SkillName4");
            }
        }
        [DataMember]
        public string SkillName5
        {
            get { return skillName5; }
            set
            {
                skillName5 = value;
                OnPropertyChanged("SkillName5");
            }
        }
        [DataMember]
        public string ResultSkill1
        {
            get { return resultSkill1; }
            set
            {
                resultSkill1 = value;
                OnPropertyChanged("ResultSkill1");
            }
        }
        [DataMember]
        public string ResultSkill2
        {
            get { return resultSkill2; }
            set
            {
                resultSkill2 = value;
                OnPropertyChanged("ResultSkill2");
            }
        }
        [DataMember]
        public string ResultSkill3
        {
            get { return resultSkill3; }
            set
            {
                resultSkill3 = value;
                OnPropertyChanged("ResultSkill3");
            }
        }
        [DataMember]
        public string ResultSkill4
        {
            get { return resultSkill4; }
            set
            {
                resultSkill4 = value;
                OnPropertyChanged("ResultSkill4");
            }
        }
        [DataMember]
        public string ResultSkill5
        {
            get { return resultSkill5; }
            set
            {
               resultSkill5 = value;
                OnPropertyChanged("ResultSkill5");
            }
        }
        [DataMember]
        public bool VisibleResultSkill1
        {
            get { return visibleResultSkill1; }
            set
            {
                visibleResultSkill1 = value;
                OnPropertyChanged("VisibleResultSkill1");
            }
        }
        [DataMember]
        public bool VisibleResultSkill2
        {
            get { return visibleResultSkill2; }
            set
            {
                visibleResultSkill2 = value;
                OnPropertyChanged("VisibleResultSkill2");
            }
        }
        [DataMember]
        public bool VisibleResultSkill3
        {
            get { return visibleResultSkill3; }
            set
            {
                visibleResultSkill3 = value;
                OnPropertyChanged("VisibleResultSkill3");
            }
        }
        [DataMember]
        public bool VisibleResultSkill4
        {
            get { return visibleResultSkill4; }
            set
            {
                visibleResultSkill4 = value;
                OnPropertyChanged("VisibleResultSkill4");
            }
        }
        [DataMember]
        public bool VisibleResultSkill5
        {
            get { return visibleResultSkill5; }
            set
            {
                visibleResultSkill5 = value;
                OnPropertyChanged("VisibleResultSkill5");
            }
        }
     

        [DataMember]
        public ulong IdProfessionalTraining
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
        public DateTime ExpectedDate
        {
            get
            {
                return expectedDate;
            }

            set
            {
                expectedDate = value;
                OnPropertyChanged("ExpectedDate");
            }
        }

        [DataMember]
        public DateTime? FinalizationDate
        {
            get
            {
                return finalizationDate;
            }

            set
            {
                finalizationDate = value;
                OnPropertyChanged("FinalizationDate");
            }
        }

        [DataMember]
        public uint IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public LookupValue Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public uint IdType
        {
            get
            {
                return idType;
            }

            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [DataMember]
        public LookupValue Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [DataMember]
        public uint IdAcceptance
        {
            get
            {
                return idAcceptance;
            }

            set
            {
                idAcceptance = value;
                OnPropertyChanged("IdAcceptance");
            }
        }
        [DataMember]
        public LookupValue Acceptance
        {
            get
            {
                return acceptance;
            }

            set
            {
                acceptance = value;
                OnPropertyChanged("Acceptance");
            }
        }

        [DataMember]
        public string AcceptanceValue
        {
            get
            {
                return acceptanceValue;
            }

            set
            {
                acceptanceValue = value;
                OnPropertyChanged("AcceptanceValue");
            }
        }

        [DataMember]
        public uint IdResponsible
        {
            get
            {
                return idResponsible;
            }

            set
            {
                idResponsible = value;
                OnPropertyChanged("IdResponsible");
            }
        }

        [DataMember]
        public Employee Responsible
        {
            get
            {
                return responsible;
            }

            set
            {
                responsible = value;
                OnPropertyChanged("Responsible");
            }
        }

        [DataMember]
        public uint? IdTrainer
        {
            get
            {
                return idTrainer;
            }

            set
            {
                idTrainer = value;
                OnPropertyChanged("IdTrainer");
            }
        }

        [DataMember]
        public Employee Trainer
        {
            get
            {
                return trainer;
            }

            set
            {
                trainer = value;
                OnPropertyChanged("Trainer");
            }
        }

        [DataMember]
        public string ExternalTrainer
        {
            get
            {
                return externalTrainer;
            }

            set
            {
                externalTrainer = value;
                OnPropertyChanged("ExternalTrainer");
            }
        }

        [DataMember]
        public string ExternalEntity
        {
            get
            {
                return externalEntity;
            }

            set
            {
                externalEntity = value;
                OnPropertyChanged("ExternalEntity");
            }
        }

        [DataMember]
        public float Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;
                OnPropertyChanged("Duration");
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
        public List<Employee> TraineeList
        {
            get
            {
                return traineeList;
            }

            set
            {
                traineeList = value;
                OnPropertyChanged("TraineeList");
            }
        }

        [DataMember]
        public List<ProfessionalTrainingResults> ProfessionalTrainingResultList
        {
            get
            {
                return professionalTrainingResults;
            }

            set
            {
               professionalTrainingResults = value;
                OnPropertyChanged("ProfessionalTrainingResults");
            }
        }
        [DataMember]
        public List<ProfessionalTrainingAttachments> ProfessionalTrainingAttachmentList
        {
            get
            {
                return professionalTrainingAttachmentList;
            }

            set
            {
               professionalTrainingAttachmentList = value;
                OnPropertyChanged("ProfessionalTrainingAttachmentList");
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
            ProfessionalTraining professionalTraining = (ProfessionalTraining)this.MemberwiseClone();

            if (Status != null)
                professionalTraining.Status = (LookupValue)this.Status.Clone();

            if (Responsible != null)
                professionalTraining.Responsible = (Employee)this.Responsible.Clone();

            if (Trainer != null)
                professionalTraining.Trainer = (Employee)this.Trainer.Clone();

            if (ProfessionalSkillList != null)
                professionalTraining.ProfessionalSkillList = ProfessionalSkillList.Select(x => (ProfessionalSkill)x.Clone()).ToList();

            if (TraineeList != null)
                professionalTraining.TraineeList = TraineeList.Select(x => (Employee)x.Clone()).ToList();

            if (ProfessionalTrainingResultList != null)
                professionalTraining.ProfessionalTrainingResultList = ProfessionalTrainingResultList.Select(x => (ProfessionalTrainingResults)x.Clone()).ToList();


            if (ProfessionalTrainingAttachmentList != null)
                professionalTraining.ProfessionalTrainingAttachmentList = ProfessionalTrainingAttachmentList.Select(x => (ProfessionalTrainingAttachments)x.Clone()).ToList();

            return professionalTraining;
        }
        #endregion
    }
}
