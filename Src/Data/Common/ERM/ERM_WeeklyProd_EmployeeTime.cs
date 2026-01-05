using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_WeeklyProd_EmployeeTime : ModelBase, IDisposable
    {
        #region Field
        List<WeeklyProductionEmployeeTime> productionEmployeeTimelist;
        List<ERM_Lookup_Value> overTime;
        #endregion
        #region Property
        [DataMember]
        public List<WeeklyProductionEmployeeTime> ProductionEmployeeTimelist
        {
            get
            {
                return productionEmployeeTimelist;
            }

            set
            {
                productionEmployeeTimelist = value;
                OnPropertyChanged("ProductionEmployeeTimelist");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> OverTime
        {
            get
            {
                return overTime;
            }

            set
            {
                overTime = value;
                OnPropertyChanged("OverTime");
            }
        }

        #endregion
        #region method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
