using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("project_followups")]
    [DataContract(IsReference = true)]
    public class ProjectFollowup :ModelBase,IDisposable
    {
        #region Fields
        Int64 idProjectFollowup;
        DateTime? followupDate;
        string description;
        Int32? idUser;
        User user;
        Int64? idProject;
        Project project;
        #endregion

        #region Properties
        [Key]
        [Column("IdProjectFollowup")]
        [DataMember]
        public Int64 IdProjectFollowup
        {
            get
            {
                return idProjectFollowup;
            }

            set
            {
                idProjectFollowup = value;
                OnPropertyChanged("IdProjectFollowup");
            }
        }

        [Column("FollowupDate")]
        [DataMember]
        public DateTime? FollowupDate
        {
            get
            {
                return followupDate;
            }

            set
            {
                followupDate = value;
                OnPropertyChanged("FollowupDate");
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

        [Column("IdUser")]
        [ForeignKey("User")]
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

        [Column("IdProject")]
        [ForeignKey("Project")]
        [DataMember]
        public Int64? IdProject
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
