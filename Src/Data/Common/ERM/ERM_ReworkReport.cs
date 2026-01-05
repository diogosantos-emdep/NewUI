using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_ReworkReport : ModelBase, IDisposable
    {
        #region Field
        List<ERM_CounterPartFailurData> counterPartFailurData;//[GEOS2-5111][gulab lakade][20 12 2023]
        List<TimeTracking> timetrackingData;//[GEOS2-5111][gulab lakade][20 12 2023]

        #endregion
        #region Property
        [DataMember]
        public List<ERM_CounterPartFailurData> CounterPartFailurData
        {
            get
            {
                return counterPartFailurData;
            }

            set
            {
                counterPartFailurData = value;
                OnPropertyChanged("CounterPartFailurData");
            }
        }
        [DataMember]
        public List<TimeTracking> TimetrackingData
        {
            get
            {
                return timetrackingData;
            }

            set
            {
                timetrackingData = value;
                OnPropertyChanged("TimetrackingData");
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
