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
    [Table("task_type")]
    [DataContract(IsReference = true)]
    public class TaskType:ModelBase,IDisposable
    {
        #region  Fields
        Int32 idTaskType;
        string name;
        IList<ProjectTask> projectTasks;
        #endregion

        #region Constructor
        public TaskType()
        {
            this.projectTasks = new List<ProjectTask>();
        }
        #endregion

        #region Properties
       
        [Key]
        [Column("IdTaskType")]
        [DataMember]
        public Int32 IdTaskType
        {
            get
            {
                return idTaskType;
            }
            set
            {
                idTaskType = value;
                OnPropertyChanged("IdTaskType");
            }
        }

        [Column("Name")]
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
        public virtual IList<ProjectTask> ProjectTasks
        {
            get
            {
                return projectTasks;
            }

            set
            {
                projectTasks = value;
                OnPropertyChanged("ProjectTasks");
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
