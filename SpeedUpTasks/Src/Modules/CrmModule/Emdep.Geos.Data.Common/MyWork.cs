using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class MyWork
    {
        #region Declaration

        string group;
        string company;
        string activityType;
        string subject;
        string status;
        string priority;
        string startDate;
        string dueDate;

        #endregion

        #region public Properties

        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
            }
        }

        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
            }
        }

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

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public string Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
            }
        }

        public string StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
            }
        }

        public string DueDate
        {
            get
            {
                return dueDate;
            }

            set
            {
                dueDate = value;
            }
        }

        #endregion
    }
}
