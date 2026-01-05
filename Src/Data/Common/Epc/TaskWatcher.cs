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
    [Table("task_watchers")]
    [DataContract]
   public class TaskWatcher : ModelBase,IDisposable
   {
        #region  Fields
        Int64 idTask;
        Int32 idUser;
        Int64 idTaskWatcher;
       ProjectTask projectTask;
       User user;
       #endregion

       #region Properties
       [Key]
       [Column("IdTaskWatcher")]
       [DataMember]
       public Int64 IdTaskWatcher
       {
           get
           {
               return idTaskWatcher;
           }

           set
           {
               idTaskWatcher = value;
               OnPropertyChanged("IdTaskWatcher");
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
