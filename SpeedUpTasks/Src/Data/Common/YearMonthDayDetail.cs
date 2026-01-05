using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   public class YearMonthDayDetail
    {
        int years;
        int months;
        int days;
        int daysRequired;
        int cntContract;
        int daysRemaining;
        public int Years
        {
            get
            {
                return years;
            }

            set
            {
                years = value;
             
            }
        }

     
        public int Months
        {
            get
            {
                return months;
            }

            set
            {
                months = value;
              
            }
        }

        public int DaysRequired
        {
            get
            {
                return daysRequired;
            }

            set
            {
                daysRequired = value;

            }
        }


        public int Days
        {
            get
            {
                return days;
            }

            set
            {
                days = value;

            }
        }

        public int CntContract
        {
            get
            {
                return cntContract;
            }

            set
            {
                cntContract = value;

            }
        }

        public int DaysRemaining
        {
            get
            {
                return daysRemaining;
            }

            set
            {
                daysRemaining = value;

            }
        }
    }
}
