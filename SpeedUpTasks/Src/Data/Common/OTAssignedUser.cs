using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("ot_assigned_users")]
    [DataContract]
    public class OTAssignedUser : ModelBase, IDisposable
    {

        #region Fields

        Int64 idOT;
        byte idStage;
        Int32 idUser;
        UserShortDetail userShortDetail;
        Int64 idOTAssignedUser;
        Stage stage;
        bool isEnabled=true;
        #endregion

       
        #region Properties

        [Key]
        [Column("IdOTAssignedUser")]
        [DataMember]
        public Int64 IdOTAssignedUser
        {
            get { return idOTAssignedUser; }
            set
            {
                idOTAssignedUser = value;
                OnPropertyChanged("IdOTAssignedUser");
            }
        }


        [Column("IdOT")]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [Column("IdStage")]
        [DataMember]
        public byte IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [NotMapped]
        [DataMember]
        public UserShortDetail UserShortDetail
        {
            get { return userShortDetail; }
            set
            {
                userShortDetail = value;
                OnPropertyChanged("UserShortDetail");
            }
        }

        [NotMapped]
        [DataMember]
        public Stage Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                OnPropertyChanged("Stage");
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

            OTAssignedUser otAssignedUser = (OTAssignedUser)this.MemberwiseClone();

            if (otAssignedUser.UserShortDetail != null)
                otAssignedUser.UserShortDetail = (UserShortDetail)this.UserShortDetail.Clone();

            if (otAssignedUser.Stage != null)
                otAssignedUser.Stage = (Stage)this.Stage.Clone();

         

            return otAssignedUser;
        }

        #endregion
    }
}
