using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
  
    [DataContract]
    public class PlannedVisitDetail : ModelBase, IDisposable
    {
        #region  Fields
        Int64? plannedAppCompletedTillCurrentDate;
        Int64? plannedAppInCurrentDate;
        Int64? plannedAppCompletedTillCurrentPeriod;
        Int64? plannedAppInCurrentPeriod;
    
        #endregion

        #region Constructor
        public PlannedVisitDetail()
        {
        }
        #endregion

        #region Properties

      
        [DataMember]
        public Int64? PlannedAppCompletedTillCurrentDate
        {
            get
            {
                return plannedAppCompletedTillCurrentDate;
            }
            set
            {
                plannedAppCompletedTillCurrentDate = value;
                OnPropertyChanged("PlannedAppCompletedTillCurrentDate");
            }
        }

       
        [DataMember]
        public Int64? PlannedAppInCurrentDate
        {
            get
            {
                return plannedAppInCurrentDate;
            }
            set
            {
                plannedAppInCurrentDate = value;
                OnPropertyChanged("PlannedAppInCurrentDate");
            }
        }

       [DataMember]
        public Int64? PlannedAppCompletedTillCurrentPeriod
        {
            get
            {
                return plannedAppCompletedTillCurrentPeriod;
            }
            set
            {
                plannedAppCompletedTillCurrentPeriod = value;
                OnPropertyChanged("PlannedAppCompletedTillCurrentPeriod");
            }
        }

       
        [DataMember]
        public Int64? PlannedAppInCurrentPeriod
        {
            get
            {
                return plannedAppInCurrentPeriod;
            }
            set
            {
                plannedAppInCurrentPeriod = value;
                OnPropertyChanged("PlannedAppInCurrentPeriod");
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
