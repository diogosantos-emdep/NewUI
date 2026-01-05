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
    [Table("task_logs")]
    [DataContract]
    public class TaskLog :ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTaskLog;
        Int64 idTask;
        Int64 idLogType;
        string description;
        DateTime? logDate;
        ProjectTask projectTask;
        Int32? idUser;
        User user;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskLog")]
        [DataMember]
        public Int64 IdTaskLog
        {
            get
            {
                return idTaskLog;
            }

            set
            {
                idTaskLog = value;
                OnPropertyChanged("IdTaskLog");
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

        [Column("IdLogType")]
        [DataMember]
        public Int64 IdLogType
        {
            get
            {
                return idLogType;
            }

            set
            {
                idLogType = value;
                OnPropertyChanged("IdLogType");
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

        [Column("LogDate")]
        [DataMember]
        public DateTime? LogDate
        {
            get
            {
                return logDate;
            }

            set
            {
                logDate = value;
                OnPropertyChanged("LogDate");
            }
        }


        [Column("IdUser")]
        [DataMember]
        public Int32? IdUser
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
