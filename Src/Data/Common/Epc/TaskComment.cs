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
    [Table("task_comments")]
    [DataContract(IsReference = true)]
   public class TaskComment : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTaskComment;
        Int64 idTask;
        Int32 idUser;
        string comment;
        DateTime? commentDate;
        ProjectTask projectTask;
        User user;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskComment")]
        [DataMember]
        public Int64 IdTaskComment
        {
            get
            {
                return idTaskComment;
            }

            set
            {
                idTaskComment = value;
                OnPropertyChanged("IdTaskComment");
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

        [Column("Comment")]
        [DataMember]
        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        [Column("CommentDate")]
        [DataMember]
        public DateTime? CommentDate
        {
            get
            {
                return commentDate;
            }

            set
            {
                commentDate = value;
                OnPropertyChanged("CommentDate");
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
