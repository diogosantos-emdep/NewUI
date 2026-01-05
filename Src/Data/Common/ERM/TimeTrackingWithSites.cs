using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.ERM
{
    public class TimeTrackingWithSites : ModelBase, IDisposable
    {
        #region Property
        public ObservableCollection<Site> siteList { get; set; }
        public List<TimeTracking> TimeTrackingList { get; set; }
        public List<int> AppSettingData { get; set; }
        //public string AppSettingData { get; set; }


        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //public override string ToString()
        //{
        //    return Name;
        //}

        #endregion
    }

    
}
