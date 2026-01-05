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
    [Table("user_teams")]
    [DataContract]
   public class UserTeam : ModelBase,IDisposable
    {
        #region  Fields
        Int32 idUser;
        byte idTeam;
        Int32 idUserTeam;
        Team team;
        User user;
        #endregion

        #region Properties

        [Key]
        [Column("IdUserTeam")]
        [DataMember]
        public Int32 IdUserTeam
        {
            get
            {
                return idUserTeam;
            }
            set
            {
                this.idUserTeam = value;
                OnPropertyChanged("IdUserTeam");
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
