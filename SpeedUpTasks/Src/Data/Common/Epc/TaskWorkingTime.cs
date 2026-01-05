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
    [Table("task_working_times")]
    [DataContract]
    public class TaskWorkingTime : ModelBase,IDisposable
    {
        #region  Fields
        Int64? idTaskWorkingTime;
        Int64 idTask;
        Int32 idUser;
        DateTime workingDate;
        Single workingTimeInHours;
        TimeSpan workingTimeInHoursInTimeSpan;
        string description;
        string taskTitle;
        string projectCode;
        User user;
      
        ProjectTask projectTask;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskWorkingTime")]
        [DataMember]
        public Int64? IdTaskWorkingTime
        {
            get
            {
                return idTaskWorkingTime;
            }

            set
            {
                idTaskWorkingTime = value;
                OnPropertyChanged("IdTaskWorkingTime");
            }
        }

        [Column("IdTask")]
        [ForeignKey("ProjectTask")]
        [DataMember]
        public Int64 IdTask
        {
            get
            {
                return idTask;
            }

            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
            }
        }

        [Column("IdUser")]
        [ForeignKey("User")]
        [DataMember]
        public Int32 IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [Column("WorkingDate")]
        [DataMember]
        public DateTime WorkingDate
        {
            get
            {
                return workingDate;
            }

            set
            {
                workingDate = value;
                OnPropertyChanged("WorkingDate");
            }
        }

        [Column("WorkingTimeInHours")]
        [DataMember]
        public Single WorkingTimeInHours
        {
            get
            {
                return workingTimeInHours;
            }

            set
            {
                workingTimeInHours = value;
                OnPropertyChanged("WorkingTimeInHours");
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

        [NotMapped]
        [DataMember]
        public string TaskTitle
        {
            get
            {
                return taskTitle;
            }

            set
            {
                taskTitle = value;
                OnPropertyChanged("TaskTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public string ProjectCode
        {
            get
            {
                return projectCode;
            }

            set
            {
                projectCode = value;
                OnPropertyChanged("ProjectCode");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan WorkingTimeInHoursInTimeSpan
        {
            get
            {
                return workingTimeInHoursInTimeSpan;
            }

            set
            {
                workingTimeInHoursInTimeSpan = value;
                OnPropertyChanged("WorkingTimeInHoursInTimeSpan");
            }
        }

        [DataMember]
        public virtual ProjectTask ProjectTask
        {
            get
            {
                return projectTask;
            }
            set
            {
                projectTask = value;
                OnPropertyChanged("ProjectTask");
            }
        }

        [DataMember]
        public virtual User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
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
