using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{

    [Table("project_analysis")]
    [DataContract]
    public class ProjectAnalysis : ModelBase,IDisposable
    {
        #region Fields
        Int64 idAnalysis;
        Int32 idAnalysisPriority;
        Int32 idAnalysisStatus;
        DateTime? analysisStartDate;
        DateTime? geosRequestDate;
        DateTime? geosDeliveryDate;
        DateTime? engDeliveryDate;
        string analysisTeamName;
        string analysisTeamRole;
        bool isValidated;
        DateTime? validationDate;
        bool isScope;
        bool isWBS;
        Int64 idWorkingOrder;
        Int64 idProject;
        Project project;
        LookupValue analysisStatus;
        LookupValue analysisPriority;
        #endregion

        #region Constructor
        public ProjectAnalysis()
        {
           // this.Offers = new List<Offer>();
          // this.projectTasks = new List<ProjectTask>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdAnalysis")]
        [DataMember]
        public Int64 IdAnalysis
        {
            get
            {
                return idAnalysis;
            }

            set
            {
                idAnalysis = value;
                OnPropertyChanged("IdAnalysis");
            }
        }

        [ForeignKey("AnalysisPriority")]
        [Column("IdAnalysisPriority")]
        [DataMember]
        public Int32 IdAnalysisPriority
        {
            get
            {
                return idAnalysisPriority;
            }

            set
            {
                idAnalysisPriority = value;
                OnPropertyChanged("IdAnalysisPriority");
            }
        }

        [ForeignKey("AnalysisStatus")]
        [Column("IdAnalysisStatus")]
        [DataMember]
        public Int32 IdAnalysisStatus
        {
            get
            {
                return idAnalysisStatus;
            }

            set
            {
                idAnalysisStatus = value;
                OnPropertyChanged("IdAnalysisStatus");
            }
        }

        [Column("AnalysisStartDate")]
        [DataMember]
        public DateTime? AnalysisStartDate
        {
            get
            {
                return analysisStartDate;
            }

            set
            {
                analysisStartDate = value;
                OnPropertyChanged("AnalysisStartDate");
            }
        }

        [Column("GeosRequestDate")]
        [DataMember]
        public DateTime? GeosRequestDate
        {
            get
            {
                return geosRequestDate;
            }

            set
            {
                geosRequestDate = value;
                OnPropertyChanged("GeosRequestDate");
            }
        }

        [Column("GeosDeliveryDate")]
        [DataMember]
        public DateTime? GeosDeliveryDate
        {
            get
            {
                return geosDeliveryDate;
            }

            set
            {
                geosDeliveryDate = value;
                OnPropertyChanged("GeosDeliveryDate");
            }
        }

        [Column("EngDeliveryDate")]
        [DataMember]
        public DateTime? EngDeliveryDate
        {
            get
            {
                return engDeliveryDate;
            }

            set
            {
                engDeliveryDate = value;
                OnPropertyChanged("EngDeliveryDate");
            }
        }

        [Column("AnalysisTeamName")]
        [DataMember]
        public string AnalysisTeamName
        {
            get
            {
                return analysisTeamName;
            }

            set
            {
                analysisTeamName = value;
                OnPropertyChanged("AnalysisTeamName");
            }
        }

        [Column("AnalysisTeamRole")]
        [DataMember]
        public string AnalysisTeamRole
        {
            get
            {
                return analysisTeamRole;
            }

            set
            {
                analysisTeamRole = value;
                OnPropertyChanged("AnalysisTeamRole");
            }
        }

        [Column("IsValidated")]
        [DataMember]
        public bool IsValidated
        {
            get
            {
                return isValidated;
            }

            set
            {
                isValidated = value;
                OnPropertyChanged("IsValidated");
            }
        }

        [Column("ValidationDate")]
        [DataMember]
        public DateTime? ValidationDate
        {
            get
            {
                return validationDate;
            }

            set
            {
                validationDate = value;
                OnPropertyChanged("ValidationDate");
            }
        }

        [Column("IsScope")]
        [DataMember]
        public bool IsScope
        {
            get
            {
                return isScope;
            }

            set
            {
                isScope = value;
                OnPropertyChanged("IsScope");
            }
        }

        [Column("IsWBS")]
        [DataMember]
        public bool IsWBS
        {
            get
            {
                return isWBS;
            }

            set
            {
                isWBS = value;
                OnPropertyChanged("IsWBS");
            }
        }

        [Column("IdWorkingOrder")]
        [DataMember]
        public Int64 IdWorkingOrder
        {
            get
            {
                return idWorkingOrder;
            }

            set
            {
                idWorkingOrder = value;
                OnPropertyChanged("IdWorkingOrder");
            }
        }

        [Column("IdProject")]
        [ForeignKey("Project")]
        [DataMember]
        public Int64 IdProject
        {
            get
            {
                return idProject;
            }

            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }


        [DataMember]
        public virtual Project Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        [DataMember]
        public virtual LookupValue AnalysisPriority
        {
            get
            {
                return analysisPriority;
            }

            set
            {
                analysisPriority = value;
                OnPropertyChanged("AnalysisPriority");
            }
        }

        [DataMember]
        public virtual LookupValue AnalysisStatus
        {
            get
            {
                return analysisStatus;
            }

            set
            {
                analysisStatus = value;
                OnPropertyChanged("AnalysisStatus");
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
