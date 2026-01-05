using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Emdep.Geos.Data.Common.Epc
{
    [Table("project_milestones")]
    [DataContract(IsReference = true)]
    public partial class ProjectMilestone : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idProjectMilestone;
        Int64 idProject;
        DateTime? creationDate;
        string description;
        string milestoneTitle;
        Project project;
        DateTime targetDate;
        Int32 idProjectMilestoneStatus;
        string comments;
        ObservableCollection<ProjectMilestoneDate> projectMilestoneDates;
        #endregion

        #region Constructor
        public ProjectMilestone()
        {
       //  this.ProjectMilestoneDates = new List<ProjectMilestoneDate>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdProjectMilestone")]
        [DataMember]
        public Int64 IdProjectMilestone
        {
            get
            {
                return idProjectMilestone;
            }
            set
            {
                idProjectMilestone = value;
                OnPropertyChanged("IdProjectMilestone");
            }
        }

        [Column("idProject")]
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

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

     
        [Column("Description")]
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

        [Column("MilestoneTitle")]
        [DataMember]
        public string MilestoneTitle
        {
            get
            {
                return milestoneTitle;
            }
            set
            {
                milestoneTitle = value;
                OnPropertyChanged("MilestoneTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime TargetDate
        {
            get
            {
                return targetDate;
            }
            set
            {
                targetDate = value;
                OnPropertyChanged("TargetDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdProjectMilestoneStatus
        {
            get
            {
                return idProjectMilestoneStatus;
            }
            set
            {
                idProjectMilestoneStatus = value;
                OnPropertyChanged("IdProjectMilestoneStatus");
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
        public virtual ObservableCollection<ProjectMilestoneDate> ProjectMilestoneDates
        {
            get
            {
                return projectMilestoneDates;
            }
            set
            {
                projectMilestoneDates = value;
                OnPropertyChanged("ProjectMilestoneDates");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
