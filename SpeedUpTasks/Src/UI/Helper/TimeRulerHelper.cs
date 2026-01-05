using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
   public class TimeRulerHelper
    {
        public static TimeRulerHelper Create()
        {
            return ViewModelSource.Create(() => new TimeRulerHelper());
        }

        public static TimeRulerHelper Create(string caption, TimeZoneInfo timeZoneInfo)
        {
            TimeRulerHelper timeRuler = TimeRulerHelper.Create();
            timeRuler.Caption = caption;
            timeRuler.TimeZone = timeZoneInfo;
            timeRuler.AlwaysShowTimeDesignator = false;
            timeRuler.ShowMinutes = false;
            return timeRuler;
        }

        protected TimeRulerHelper() { }
        public virtual string Caption { get; set; }
        public virtual TimeZoneInfo TimeZone { get; set; }
        public virtual bool AlwaysShowTimeDesignator { get; set; }
        public virtual bool ShowMinutes { get; set; }
    }
}
