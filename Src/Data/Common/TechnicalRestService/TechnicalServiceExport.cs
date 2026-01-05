using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TechnicalServiceExport
    {
        [DataMember(Order = 1)]
        public string IdTechnicalAssistanceReport { get; set; }

        [DataMember(Order = 2)]
        public string ParameterPlantOwner { get; set; }
        [DataMember(Order = 3)]
        public string Lang { get; set; }

        [DataMember(Order = 4)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 5)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 6)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 7)]
        public string login { get; set; }

        [DataMember(Order = 8)]
        public string Signatory { get; set; }
        [DataMember(Order = 9)]
        public string TechnicalAssistanceReportPath { get; set; }
        [DataMember(Order = 10)]
        public byte[] ParameterImageBytes { get; set; }

        [Display(Order = 11)]
        public HttpContent File { get; set; }

        [DataMember(Order = 10)]
        public CreateExpenseReportsSign ParameterSign { get; set; }

        [DataMember(Order = 11)]
        public string TechnicalAssistanceReportTemplatePath { get; set; }

        [DataMember(Order = 12)]
        public string WorkingOrdersPath { get; set; }
    }
}
