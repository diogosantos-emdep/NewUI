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
    [Table("project_teams")]
    [DataContract]
    public class ProjectTeam : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idProject;
        byte idTeam;
        Int32 idProjectTeam;
        Team team;
        Project project;
        #endregion

        #region Properties

        [Key]
        [Column("IdProjectTeam")]
        [DataMember]
        public Int32 IdProjectTeam
        {
            get
            {
                return idProjectTeam;
            }
            set
            {
                this.idProjectTeam = value;
                OnPropertyChanged("IdProjectTeam");
            }
        }

        [Column("IdTeam")]
        [ForeignKey("Team")]
        [DataMember]
        public byte IdTeam
        {
            get
            {
                return idTeam;
            }
            set
            {
                idTeam = value;
                OnPropertyChanged("IdTeam");
            }
        }

        [Column("IdProject")]
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

        [DataMember]
        public virtual Team Team
        {
            get
            {
                return team;
            }
            set
            {
                team = value;
                OnPropertyChanged("Team");
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
