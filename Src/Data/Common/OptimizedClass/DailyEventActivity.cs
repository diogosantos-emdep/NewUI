using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class DailyEventActivity
    {
        #region Fields
        Int64 idActivity;
        string subject;
        Int32 idActivityType;
        string activityType;
        string activityTypeHtmlColor;
        DateTime? fromDate;
        DateTime? toDate;
        Int32 idOwner;
        #endregion

        #region Properties
        [DataMember]
        public long IdActivity
        {
            get
            {
                return idActivity;
            }

            set
            {
                idActivity = value;
            }
        }

        [DataMember]
        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                subject = value;
            }
        }

        [DataMember]
        public int IdActivityType
        {
            get
            {
                return idActivityType;
            }

            set
            {
                idActivityType = value;
            }
        }

        [DataMember]
        public string ActivityType
        {
            get
            {
                return activityType;
            }

            set
            {
                activityType = value;
            }
        }

        [DataMember]
        public string ActivityTypeHtmlColor
        {
            get
            {
                return activityTypeHtmlColor;
            }

            set
            {
                activityTypeHtmlColor = value;
            }
        }

        [DataMember]
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
            }
        }

        [DataMember]
        public DateTime? ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
            }
        }

        [DataMember]
        public int IdOwner
        {
            get
            {
                return idOwner;
            }

            set
            {
                idOwner = value;
            }
        }

     

        #endregion
    }
}
