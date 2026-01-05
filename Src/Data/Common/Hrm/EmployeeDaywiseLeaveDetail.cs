using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class EmployeeDaywiseLeaveDetail
    {
        DateTime leaveDate;
        TimeSpan leavedHours;
        Int32 idLeave;
        TimeSpan startTime;
        TimeSpan endTime;
        sbyte isAllDayEvent;
        Int32 idEmpContractCompany;
        public Int32 IdLeave
        {
            get
            {
                return idLeave;
            }

            set
            {
                idLeave = value;

            }
        }

        public DateTime LeaveDate
        {
            get
            {
                return leaveDate;
            }

            set
            {
                leaveDate = value;

            }
        }
       
        public TimeSpan LeavedHours
        {
            get
            {
                return leavedHours;
            }

            set
            {
                leavedHours = value;

            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;

            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;

            }
        }


        public sbyte IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;

            }
        }

        public Int32 IdEmpContractCompany
        {
            get { return idEmpContractCompany; }
            set
            {
                idEmpContractCompany = value;

            }
        }
    }
}
