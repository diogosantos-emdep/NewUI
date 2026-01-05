using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_WeeklyProduction : ModelBase, IDisposable
    {
        #region Fields
        private List<ERM_WeekDays> eRM_WeekDays;
        private List<WeeklyProductionReport> weeklyProductionReport;
        #endregion


        #region Properties

        [DataMember]
        public List<ERM_WeekDays> ERM_WeekDays
        {
            get { return eRM_WeekDays; }
            set
            {
                eRM_WeekDays = value;
                OnPropertyChanged("ERM_WeekDays");
            }
        }
        [DataMember]
        public List<WeeklyProductionReport> WeeklyProductionReport
        {
            get
            {
                return weeklyProductionReport;
            }

            set
            {
                weeklyProductionReport = value;
                OnPropertyChanged("WeeklyProductionReport");
            }
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
