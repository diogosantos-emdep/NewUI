using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class ExportWorkLogs
    {
        private Int32 _IdTechnicalAssistanceReport = 0;
        [IgnoreDataMember]
        public Int32 IdTechnicalAssistanceReport
        {
            get { return _IdTechnicalAssistanceReport; }
            set { _IdTechnicalAssistanceReport = value; }
        }

        public string _WorklogDate = string.Empty;
        [DataMember(Order = 1)]
        public string WorklogDate
        {
            get { return _WorklogDate; }
            set { _WorklogDate = value; }
        }
        [DataMember(Order = 2)]
        string _TechnicianName = string.Empty;
        public string TechnicianName
        {
            get { return _TechnicianName; }
            set { _TechnicianName = value; }
        }

        private string _StartTime = string.Empty;
        [DataMember(Order = 3)]
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        private string _EndTime = string.Empty;
        [DataMember(Order = 4)]
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        private string _TotalTime = string.Empty;
        [DataMember(Order = 5)]
        public string TotalTime
        {
            get { return _TotalTime; }
            set { _TotalTime = value; }
        }
        private string _Type = string.Empty; //chitra.girigosavi[19/08/2024][APIGEOS-1195]
        [DataMember(Order = 5)]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
}