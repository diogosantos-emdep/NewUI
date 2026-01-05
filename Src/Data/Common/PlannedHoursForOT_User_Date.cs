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
    [Table("ot_user_plannings")]
    [DataContract]
    public class PlannedHoursForOT_User_Date : ModelBase, IDisposable
    {

        #region Fields
        Int64 idOTUserPlanning;
        Int64 idOT;
        Int32 idUser;
        DateTime? planningDate;
        float? timeEstimationInHours;
        #endregion

        #region Constructor
        public PlannedHoursForOT_User_Date()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOTUserPlanning")]
        [DataMember]
        public Int64 IdOTUserPlanning
        {
            get { return idOTUserPlanning; }
            set
            {
                idOTUserPlanning = value;
                OnPropertyChanged("IdOTUserPlanning");
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
        
        [Column("Date")]
        [DataMember]
        public DateTime? PlanningDate
        {
            get { return planningDate; }
            set
            {
                planningDate = value;
                OnPropertyChanged("PlanningDate");
            }
        }

        [Column("TimeEstimation")]
        [DataMember]
        public float? TimeEstimationInHours
        {
            get { return timeEstimationInHours; }
            set
            {
                timeEstimationInHours = value;
                OnPropertyChanged("TimeEstimationInHours");
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
