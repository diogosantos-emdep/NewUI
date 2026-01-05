using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_WeekDays : ModelBase, IDisposable
    {
        #region Fields
        private string dayName;
       
        #endregion


        #region Properties

        [DataMember]
        public string DayName
        {
            get { return dayName; }
            set
            {
                dayName = value;
                OnPropertyChanged("DayName");
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
