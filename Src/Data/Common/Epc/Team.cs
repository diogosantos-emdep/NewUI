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
    [Table("teams")]
    [DataContract(IsReference = true)]
    public class Team :ModelBase,IDisposable
    {
        #region  Fields
        byte? idParent;
        byte idTeam;
        string name;
        string parentChildName;
        Team parentTeam;
        IList<Team> childrens;
        IList<UserTeam> userTeams;
        IList<ProjectTeam> projectTeams;
        #endregion

          #region Constructor
        public Team()
        {
            this.UserTeams = new List<UserTeam>();
            this.ProjectTeams = new List<ProjectTeam>();
        }
        #endregion

        #region Properties
       
        [Key]
        [Column("IdTeam")]
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

        [Column("IdParent")]
        [ForeignKey("ParentTeam")]
        [DataMember]
        public byte? IdParent
        {
            get
            {
                return idParent;
            }
            set
            {
                this.idParent = value;
                OnPropertyChanged("IdParent");
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

        [NotMapped]
        [DataMember]
        public string ParentChildName
        {
            get
            {
                if(ParentTeam!=null)
               return ParentTeam.Name + '-' + Name;
                else
                    return Name;
            }
            set
            {
               
            }
        }

        [DataMember]
        public virtual IList<Team> Childrens
        {
            get
            {
                return childrens;
            }

            set
            {
                childrens = value;
                OnPropertyChanged("Childrens");
            }
        }

        [DataMember]
        public virtual Team ParentTeam
        {
            get
            {
                return parentTeam;
            }

            set
            {
                parentTeam = value;
                OnPropertyChanged("ParentTeam");
            }
        }


        [DataMember]
        public virtual IList<UserTeam> UserTeams
        {
            get
            {
                return userTeams;
            }
            set
            {
                userTeams = value;
                OnPropertyChanged("UserTeams");
            }
        }


        [DataMember]
        public virtual IList<ProjectTeam> ProjectTeams
        {
            get
            {
                return projectTeams;
            }
            set
            {
                projectTeams = value;
                OnPropertyChanged("ProjectTeams");
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
