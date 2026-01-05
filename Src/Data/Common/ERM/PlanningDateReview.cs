using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    
    public class PlanningDateReview : ModelBase, IDisposable
    {

        #region Fields

        UInt32 iDPlanningDate;
        UInt32 iDOT;
        UInt32 idCounterpart;
        DateTime? plannedDeliveryDate;
        UInt32 createdBy;
        DateTime? createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        Int32 idSite;//[GEOS2-5319][gulab lakade][15 11 2024]

        #endregion

        #region Properties
        [DataMember]
        public UInt32 IDPlanningDate
        {
            get { return iDPlanningDate; }
            set
            {
                iDPlanningDate = value;
                OnPropertyChanged("IDPlanningDate");
            }
        }

        [DataMember]
        public UInt32 IDOT
        {
            get { return iDOT; }
            set
            {
                iDOT = value;
                OnPropertyChanged("IDOT");
            }
        }

        [DataMember]
        public UInt32 IdCounterpart
        {
            get { return idCounterpart; }
            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }

        [DataMember]
        public DateTime? PlannedDeliveryDate
        {
            get { return plannedDeliveryDate; }
            set
            {
                plannedDeliveryDate = value;
                OnPropertyChanged("PlannedDeliveryDate");
            }
        }

        [DataMember]
        public UInt32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }
        //start[GEOS2-5319][gulab lakade][15 11 2024]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        //end [GEOS2-5319] [gulab lakade][15 11 2024]
        #endregion

        #region Constructor

        public PlanningDateReview()
        {
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
