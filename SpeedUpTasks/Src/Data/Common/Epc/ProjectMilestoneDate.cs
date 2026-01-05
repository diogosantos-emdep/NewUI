using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("project_milestone_dates")]
    [DataContract]
    public partial class ProjectMilestoneDate:ModelBase,IDisposable
    {
        #region  Fields
        Int64 idProjectMilestone;
        DateTime targetDate;
        Int32 idProjectMilestoneStatus;
        string comments;
        Int64 idProjectMilestoneDate;
        ProjectMilestone projectMilestone;
        LookupValue projectMilestoneStatus;
        #endregion

        #region Constructor
        public ProjectMilestoneDate()
        {

        }
        #endregion

        #region Properties

        [Key]
        [Column("IdProjectMilestoneDate")]
        [DataMember]
        public Int64 IdProjectMilestoneDate
        {
            get
            {
                return idProjectMilestoneDate;
            }
            set
            {
                idProjectMilestoneDate = value;
                OnPropertyChanged("IdProjectMilestoneDate");
            }
        }

        
        [Column("IdProjectMilestone")]
        [ForeignKey("ProjectMilestone")]
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

        [Column("TargetDate")]
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

        [Column("IdProjectMilestoneStatus")]
        [ForeignKey("ProjectMilestoneStatus")]
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


        [Column("Comments")]
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

        [DataMember]
        public virtual LookupValue ProjectMilestoneStatus
        {
            get
            {
                return projectMilestoneStatus;
            }
            set
            {
                projectMilestoneStatus = value;
                OnPropertyChanged("ProjectMilestoneStatus");
            }
        }

        [DataMember]
        public virtual ProjectMilestone ProjectMilestone
        {
            get
            {
                return projectMilestone;
            }
            set
            {
                projectMilestone = value;
                OnPropertyChanged("ProjectMilestone");
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
