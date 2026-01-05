using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("task_users")]
    [DataContract(IsReference = true)]
   public class TaskUser : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTaskUser;
        Int32 idUser;
        Int64 idTask;
     
        ProjectTask projectTask;
        User user;
        ImageSource userProfileImage;
        #endregion

        #region Properties
        [Key]
        [Column("IdTaskUser")]
        [DataMember]
        public Int64 IdTaskUser
        {
            get
            {
                return idTaskUser;
            }

            set
            {
                idTaskUser = value;
                OnPropertyChanged("IdTaskUser");
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

        [NotMapped]
        [DataMember]
        public ImageSource UserProfileImage
        {
            get
            {
                return userProfileImage;
            }

            set
            {
                userProfileImage = value;
                OnPropertyChanged("UserProfileImage");
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
