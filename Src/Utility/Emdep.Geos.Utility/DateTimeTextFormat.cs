using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Save this file as Emdep.Geos.Utility.cs
// Compile with: csc Emdep.Geos.Utility.cs /doc:Results.xml
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// This class is to get datetime in datetimeformat.
    /// </summary>
    public class DateTimeTextFormat
    {
        /// <summary>
        /// This method is to get date time in text format(Eg:-DateTime-10 hours ago/18 hours ago)
        /// </summary>
        /// <param name="dt">Get Date in DateTime format</param>
        /// <returns>Date Time in Text Format</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         DateTime dt = System.DateTime.Now;
        ///         string DateTimeText = DateTimeTextFormat.GetDateTimeTextFormat(dt);
        ///         
        ///         OUTPUT:- just now
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetDateTimeTextFormat(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
    }
}
