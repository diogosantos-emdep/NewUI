using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class ExportParts
    {
        private Int32 _IdTechnicalAssistanceReport = 0;
        [IgnoreDataMember]
        public Int32 IdTechnicalAssistanceReport
        {
            get { return _IdTechnicalAssistanceReport; }
            set { _IdTechnicalAssistanceReport = value; }
        }

        private string _Reference = string.Empty;
        [DataMember(Order = 1)]
        public string Reference
        {
            get { return _Reference; }
            set { _Reference = value; }
        }
        private string _Description = string.Empty;
        [DataMember(Order = 2)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private Int32 _Quantity = 0;
        [DataMember(Order = 3)]
        public Int32 Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }
    }
}