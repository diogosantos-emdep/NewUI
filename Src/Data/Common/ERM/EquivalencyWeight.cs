using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class EquivalencyWeight : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idCPType;
        private float? equivalentWeight;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? createdIn;
        private DateTime? modifiedIn;
        private uint? createdBy;
        private UInt32 modifiedBy;
        private UInt64 iDCPTypeEquivalent;  //[GEOS2-4330][Rupali Sarode][11-04-2023]
        #endregion
        #region Properites

        [DataMember]
        public Int32 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

       

        [DataMember]
        public float? EquivalentWeight
        {
            get
            {
                return equivalentWeight;
            }

            set
            {
                equivalentWeight = value;
                OnPropertyChanged("EquivalentWeight");
            }
        }

        [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

      

        //[GEOS2-4330][Rupali Sarode][11-04-2023]
        [DataMember]
        public UInt64 IDCPTypeEquivalent
        {
            get
            {
                return iDCPTypeEquivalent;
            }

            set
            {
                iDCPTypeEquivalent = value;
                OnPropertyChanged("IDCPTypeEquivalent");
            }
        }
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public uint? CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        #endregion
        #region Constructor

        public EquivalencyWeight()
        {
        }

        #endregion
        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

       
        #endregion
    }
}
