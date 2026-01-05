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
    [Table("task_assistances")]
    [DataContract]
    public class TaskAssistance : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTask;
        Int32 idRequestFrom;
        Int32 idRequestTo;
        Int64 idTaskAssistance;
        DateTime? requestDate;
        DateTime? endDate;
        ProjectTask projectTask;
        User requestFrom;
        User requestTo;
        Int32? idTaskAssistanceStatus;
        LookupValue taskAssistanceStatus;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskAssistance")]
        [DataMember]
        public Int64 IdTaskAssistance
        {
            get
            {
                return idTaskAssistance;
            }

            set
            {
                idTaskAssistance = value;
                OnPropertyChanged("IdTaskAssistance");
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

        [Column("IdTaskAssistanceStatus")]
        [ForeignKey("TaskAssistanceStatus")]
        [DataMember]
        public Int32? IdTaskAssistanceStatus
        {
            get
            {
                return idTaskAssistanceStatus;
            }

            set
            {
                idTaskAssistanceStatus = value;
                OnPropertyChanged("IdTaskAssistanceStatus");
            }
        }

        [Column("IdRequestFrom",Order = 0)]
        [ForeignKey("RequestFrom")]
        [DataMember]
        public Int32 IdRequestFrom
        {
            get
            {
                return idRequestFrom;
            }
            set
            {
                idRequestFrom = value;
                OnPropertyChanged("IdRequestFrom");
            }
        }

        [Column("IdRequestTo", Order = 1)]
        [ForeignKey("RequestTo")]
        [DataMember]
        public Int32 IdRequestTo
        {
            get
            {
                return idRequestTo;
            }
            set
            {
                idRequestTo = value;
                OnPropertyChanged("IdRequestTo");
            }
        }

        [DataMember]
        [Column("RequestDate")]
        public DateTime? RequestDate
        {
            get
            {
                return requestDate;
            }

            set
            {
                requestDate = value;
                OnPropertyChanged("RequestTo");
            }
        }

        [DataMember]
        [Column("EndDate")]
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
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
        [InverseProperty("TaskRequestFromAssistances")]
        public virtual User RequestFrom
        {
            get
            {
                return requestFrom;
            }

            set
            {
                requestFrom = value;
                OnPropertyChanged("RequestFrom");
            }
        }

        [DataMember]
        [InverseProperty("TaskRequestToAssistances")]
        public virtual User RequestTo
        {
            get
            {
                return requestTo;
            }

            set
            {
                requestTo = value;
                OnPropertyChanged("RequestTo");
            }
        }


        [DataMember]
        public virtual LookupValue TaskAssistanceStatus
        {
            get
            {
                return taskAssistanceStatus;
            }

            set
            {
                taskAssistanceStatus = value;
                OnPropertyChanged("TaskAssistanceStatus");
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
