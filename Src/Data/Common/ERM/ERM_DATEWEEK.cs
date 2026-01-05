using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_DATEWEEK : ModelBase, IDisposable
    {
        #region Field
        private DateTime? date;
        private string dateWeek;
        #endregion
        #region Property
        [DataMember]
        public DateTime? Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        [DataMember]
        public string DateWeek
        {
            get
            {
                return dateWeek;
            }

            set
            {
                dateWeek = value;
                OnPropertyChanged("DateWeek");
            }
        }
        #endregion
        #region Constructor
        public ERM_DATEWEEK()
        {

        }
        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
