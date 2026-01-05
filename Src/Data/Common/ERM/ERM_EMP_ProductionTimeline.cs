using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class ERM_EMP_ProductionTimeline : ModelBase, IDisposable
    {
        #region Fields
        private double timeValue;
        private string stageName;
        private string timeType;
        private Int32 stageSequence;
        private string details;//[GEOS2-6716][pallavi jadhav][13 01 2025]
        private Int32 idlookupValue;//[GEOS2-6716][pallavi jadhav][13 01 2025]
        #endregion
        #region Properites
        [DataMember]
        public double TimeValue
        {
            get
            {
                return timeValue;
            }
            set
            {
                timeValue = value;
                OnPropertyChanged("TimeValue");
            }
        }
        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
            }
        }
        [DataMember]
        public string TimeType
        {
            get
            {
                return timeType;
            }

            set
            {
                timeType = value;
                OnPropertyChanged("TimeType");
            }
        }
        [DataMember]
        public Int32 StageSequence
        {
            get
            {
                return stageSequence;
            }

            set
            {
                stageSequence = value;
                OnPropertyChanged("StageSequence");
            }
        }
        //[GEOS2-6716][pallavi jadhav][13 01 2025]
        [DataMember]
        public string Details
        {
            get
            {
                return details;
            }

            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }
        [DataMember]
        public Int32 IdlookupValue
        {
            get
            {
                return idlookupValue;
            }

            set
            {
                idlookupValue = value;
                OnPropertyChanged("IdlookupValue");
            }
        }
        //[GEOS2-6716][pallavi jadhav][13 01 2025]
        #endregion
        #region Constructor
        public ERM_EMP_ProductionTimeline()
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
