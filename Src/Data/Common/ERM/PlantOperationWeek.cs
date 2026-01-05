using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantOperationWeek : ModelBase, IDisposable
    {
        #region Field
        private string calenderWeek;
        private DateTime firstDateofweek;
        private DateTime lastDateofWeek;
        #endregion
        #region Property
        [DataMember]
        public string CalenderWeek
        {
            get { return calenderWeek; }
            set
            {
                calenderWeek = value;
                OnPropertyChanged("CalenderWeek");
            }
        }
        [DataMember]
        public DateTime FirstDateofweek
        {
            get
            {
                return firstDateofweek;
            }

            set
            {
                firstDateofweek = value;
                OnPropertyChanged("FirstDateofweek");
            }
        }
        [DataMember]
        public DateTime LastDateofWeek
        {
            get
            {
                return lastDateofWeek;
            }

            set
            {
                lastDateofWeek = value;
                OnPropertyChanged("LastDateofWeek");
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
        #region Constructor
        public PlantOperationWeek()
        {

        }
        #endregion
    }
}
