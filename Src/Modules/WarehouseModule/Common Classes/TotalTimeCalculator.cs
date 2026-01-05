using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.Warehouse
{

    public static class TotalTimeCalculator
    {
        public static void GetTotalTimeByCreatingDateTimeSlots(
            DateTime? taskStartDateTime, DateTime? taskEndDateTime,
            out TimeSpan? totalWorkingTime, StageAvailability weeklyWorkSchedule,
            TimeSpan? itemTotalTime)
        {
            if (taskStartDateTime == null || taskEndDateTime == null)
            {
                totalWorkingTime = null;
                return;
            }

            var dateSlots = DateSlotsCalculator.CreateDateSlots(taskStartDateTime.Value, taskEndDateTime.Value);

            if (dateSlots == null)
            {
                totalWorkingTime = null;
                return;
            }
            else
            {
                totalWorkingTime = new TimeSpan();
            }

            TimeSpan? totalCalendarTime = new TimeSpan();

            TimeSpan? totalNonWorkingTime = new TimeSpan();
            TimeSpan? totalCalendarTimeWithoutNonWorkingTime = new TimeSpan();

            foreach (var item in dateSlots)
            {
                TimeSpan? totalCalendarTimeForItem;
                TimeSpan? totalWorkingTimeForItem;
                TimeSpan? totalNonWorkingTimeForItem;
                TimeSpan? totalCalendarTimeWithoutNonWorkingTimeForItem;
                List<TimeSlot> timeSlotsList;

                TimeSpan? weekdayStartTime;
                TimeSpan? weekdayEndTime;
                GetWeekdayStartTimeAndWeekdayEndTime(
                    weeklyWorkSchedule, item.SlotStartDateTime,
                    out weekdayStartTime, out weekdayEndTime);

                TimeSlotsCalculator.GetTotalTime(item.SlotStartDateTime,
                    item.SlotEndDateTime,
                    weekdayStartTime.Value, weekdayEndTime.Value,
                    out totalCalendarTimeForItem,
                    out totalWorkingTimeForItem, out totalNonWorkingTimeForItem,
                    out totalCalendarTimeWithoutNonWorkingTimeForItem,
                    out timeSlotsList);

                //Console.WriteLine("The date slot = " +
                //Newtonsoft.Json.JsonConvert.SerializeObject(item,
                //Newtonsoft.Json.Formatting.Indented
                //    ));

                //Console.WriteLine("In the date slot, time slots list = " +
                //Newtonsoft.Json.JsonConvert.SerializeObject(timeSlotsList,
                //Newtonsoft.Json.Formatting.Indented
                //    ));

                //Console.WriteLine($"totalCalendarTime {totalCalendarTime} += {totalCalendarTimeForItem}" +
                //$",\n totalWorkingTime {totalWorkingTime} += {totalWorkingTimeForItem}" +
                //$",\n totalNonWorkingTime {totalNonWorkingTime} += {totalNonWorkingTimeForItem}" +
                //$",\n totalCalendarTimeWithoutNonWorkingTime {totalCalendarTimeWithoutNonWorkingTime} += {totalCalendarTimeWithoutNonWorkingTimeForItem}");

                totalCalendarTime += totalCalendarTimeForItem;
                totalWorkingTime += totalWorkingTimeForItem;
                totalNonWorkingTime += totalNonWorkingTimeForItem;
                totalCalendarTimeWithoutNonWorkingTime += totalCalendarTimeWithoutNonWorkingTimeForItem;
            }

            //Console.WriteLine($"\n\nFinal\n totalCalendarTime {totalCalendarTime}" +
            //$",\n totalWorkingTime {totalWorkingTime}" +
            //$",\n totalNonWorkingTime {totalNonWorkingTime}" +
            //$",\n totalCalendarTimeWithoutNonWorkingTime {totalCalendarTimeWithoutNonWorkingTime}");

            if (itemTotalTime != totalCalendarTime)
            {
                throw new Exception($"itemTotalTime {itemTotalTime} != totalCalendarTime {totalCalendarTime}");
            }
            //else
            //{
            //    Console.WriteLine("itemTotalTime == totalCalendarTime");
            //}
        }

        private static void GetWeekdayStartTimeAndWeekdayEndTime(
            StageAvailability weeklyWorkSchedule,
            DateTime slotStartDateTime, out TimeSpan? weekdayStartTime,
            out TimeSpan? weekdayEndTime)
        {
            switch (slotStartDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeSunday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeSunday;
                    break;
                case DayOfWeek.Monday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeMonday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeMonday;
                    break;
                case DayOfWeek.Tuesday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeTuesday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeTuesday;
                    break;
                case DayOfWeek.Wednesday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeWednesday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeWednesday;
                    break;
                case DayOfWeek.Thursday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeThursday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeThursday;
                    break;
                case DayOfWeek.Friday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeFriday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeFriday;
                    break;
                case DayOfWeek.Saturday:
                    weekdayStartTime = weeklyWorkSchedule.StartTimeSaturday;
                    weekdayEndTime = weeklyWorkSchedule.EndTimeSaturday;
                    break;
                default:
                    weekdayStartTime = null;
                    weekdayEndTime = null;
                    break;
            }

            if (weekdayStartTime == null || weekdayEndTime == null)
            {
                weekdayStartTime = new TimeSpan();
                weekdayEndTime = new TimeSpan();
                return;
            }
        }
    }

    public static class TimeSlotsCalculator
    {
        public static List<TimeSlot> CreateTimeSlots(
            DateTime taskStartTime, DateTime taskEndTime,
            TimeSpan weekdayStartTime, TimeSpan weekdayEndTime)
        {
            var CalendarDayStartTime = taskStartTime.Date;
            var CalendarDayEndTime = CalendarDayStartTime.AddDays(1);
            var officeDayStartTime = CalendarDayStartTime.Add(weekdayStartTime);
            var officeDayEndTime = CalendarDayStartTime.Add(weekdayEndTime);

            var timeSlotsList = new List<TimeSlot>();

            if (officeDayStartTime > officeDayEndTime)
            {
                return null;
                //throw new Exception($"Invalid input for creating time slots." +
                //    $" office Start Time={officeDayStartTime}"+
                //    $" office End Time={officeDayEndTime}");
            }
            if (taskStartTime >= taskEndTime)
            {
                return null;
                //throw new Exception($"Invalid input for creating time slots." +
                //    $" task Start Time={taskStartTime}" +
                //    $" task End Time={taskEndTime}");
            }

            if (taskStartTime <= officeDayStartTime)
            {
                var timeSlot = new TimeSlot
                {
                    SlotStartTime = taskStartTime,
                    IsTimeSlotInWorkingHours = false
                };

                if (taskEndTime < officeDayStartTime)
                {
                    timeSlot.SlotEndTime = taskEndTime;
                }
                else if (taskEndTime >= officeDayStartTime)
                {
                    timeSlot.SlotEndTime = officeDayStartTime;
                }

                timeSlotsList.Add(timeSlot);
                taskStartTime = timeSlot.SlotEndTime;
            }

            if ((taskStartTime >= officeDayStartTime && taskStartTime <= officeDayEndTime) && (taskStartTime < taskEndTime))
            {
                var timeSlot = new TimeSlot
                {
                    SlotStartTime = taskStartTime,
                    IsTimeSlotInWorkingHours = true
                };

                if (taskEndTime <= officeDayEndTime)
                {
                    timeSlot.SlotEndTime = taskEndTime;
                }
                else if (taskEndTime > officeDayEndTime)
                {
                    timeSlot.SlotEndTime = officeDayEndTime;
                }

                timeSlotsList.Add(timeSlot);
                taskStartTime = timeSlot.SlotEndTime;
            }

            if (taskStartTime >= officeDayEndTime && (taskStartTime < taskEndTime))
            {
                var timeSlot = new TimeSlot
                {
                    SlotStartTime = taskStartTime,
                    IsTimeSlotInWorkingHours = false
                };
                if (taskEndTime < CalendarDayEndTime)
                {
                    timeSlot.SlotEndTime = taskEndTime;
                }
                else
                {
                    timeSlot.SlotEndTime = CalendarDayEndTime;
                }

                timeSlotsList.Add(timeSlot);
                taskStartTime = timeSlot.SlotEndTime;
            }

            return timeSlotsList;
        }

        public static void GetTotalTime(
            DateTime taskStartTime, DateTime taskEndTime,
            TimeSpan weekdayStartTime, TimeSpan weekdayEndTime,
            out TimeSpan? totalCalendarTime,
            out TimeSpan? totalWorkingTime, out TimeSpan? totalNonWorkingTime,
            out TimeSpan? totalCalendarTimeWithoutNonWorkingTime,
            out List<TimeSlot> timeSlotsList)
        {
            //timeSlotsList = new List<TimeSlot>();
            timeSlotsList = CreateTimeSlots(
            taskStartTime, taskEndTime,
            weekdayStartTime, weekdayEndTime);

            if (timeSlotsList == null)
            {
                totalCalendarTime = new TimeSpan();
                totalWorkingTime = new TimeSpan();
                totalNonWorkingTime = new TimeSpan();
                totalCalendarTimeWithoutNonWorkingTime = new TimeSpan();
                return;
            }
            else
            {
                totalCalendarTime = new TimeSpan();
                totalWorkingTime = new TimeSpan();
                totalNonWorkingTime = new TimeSpan();
                totalCalendarTimeWithoutNonWorkingTime = new TimeSpan();
            }
            foreach (var item in timeSlotsList)
            {
                var slotDuration = (item.SlotEndTime - item.SlotStartTime);
                totalCalendarTime += slotDuration;

                if (item.IsTimeSlotInWorkingHours)
                {
                    totalWorkingTime += slotDuration;
                }
                else
                {
                    totalNonWorkingTime += slotDuration;
                }
            }

            totalCalendarTimeWithoutNonWorkingTime = totalCalendarTime - totalNonWorkingTime;
        }

    }

    public static class DateSlotsCalculator
    {
        public static List<DateSlot> CreateDateSlots(
            DateTime taskStartDateTime, DateTime taskEndDateTime)
        {
            if (taskStartDateTime >= taskEndDateTime)
            {
                return null;
                //throw new Exception($"Invalid input for creating date slots." +
                //    $" task Start Time={taskStartDateTime}" +
                //    $" task End Time={taskEndDateTime}");
            }

            var dateSlotsList = new List<DateSlot>();
            while (taskStartDateTime < taskEndDateTime)
            {
                var dateSlot = new DateSlot();

                dateSlot.SlotStartDateTime = taskStartDateTime;

                if (taskStartDateTime.Date.AddDays(1) <= taskEndDateTime)
                {
                    dateSlot.SlotEndDateTime = taskStartDateTime.Date.AddDays(1);
                }
                else
                {
                    dateSlot.SlotEndDateTime = taskEndDateTime;
                }
                dateSlotsList.Add(dateSlot);
                taskStartDateTime = dateSlot.SlotEndDateTime;
            }

            return dateSlotsList;
        }
    }

    public class DateSlot
    {
        DateTime slotStartDateTime;
        DateTime slotEndDateTime;

        public DateTime SlotStartDateTime
        {
            get
            {
                return slotStartDateTime;
            }

            set
            {
                this.slotStartDateTime = value;
            }
        }

        public DateTime SlotEndDateTime
        {
            get
            {
                return slotEndDateTime;
            }

            set
            {
                this.slotEndDateTime = value;
            }
        }
    }

    public class TimeSlot
    {
        DateTime slotStartTime;
        DateTime slotEndTime;
        bool isTimeSlotInWorkingHours;

        public DateTime SlotStartTime
        {
            get
            {
                return slotStartTime;
            }

            set
            {
                this.slotStartTime = value;
            }
        }

        public DateTime SlotEndTime
        {
            get
            {
                return slotEndTime;
            }

            set
            {
                this.slotEndTime = value;
            }
        }

        public bool IsTimeSlotInWorkingHours
        {
            get
            {
                return isTimeSlotInWorkingHours;
            }

            set
            {
                this.isTimeSlotInWorkingHours = value;
            }
        }
    }
}
