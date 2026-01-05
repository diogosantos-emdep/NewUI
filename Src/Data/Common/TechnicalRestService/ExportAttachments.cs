using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class ExportAttachments
    {
        private Int32 _IdTechnicalAssistanceReport = 0;
        [IgnoreDataMember]
        public Int32 IdTechnicalAssistanceReport
        {
            get { return _IdTechnicalAssistanceReport; }
            set { _IdTechnicalAssistanceReport = value; }
        }
        private string _AttachedFiles = string.Empty;
        [DataMember(Order = 2)]
        public string AttachedFiles
        {
            get { return _AttachedFiles; }
            set { _AttachedFiles = value; }
        }
        private string _Year = string.Empty;
        [DataMember(Order = 3)]
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _OfferCode = string.Empty;
        [DataMember(Order = 4)]
        public string OfferCode
        {
            get { return _OfferCode; }
            set { _OfferCode = value; }
        }

        private string _QUE_Code = string.Empty;
        [DataMember(Order = 5)]
        public string QUE_Code
        {
            get { return _QUE_Code; }
            set { _QUE_Code = value; }
        }

        private List<string> _ImagePaths = new List<string>();

        [DataMember(Order = 6)]
        public List<string> ImagePaths
        {
            get { return _ImagePaths; }
            set { _ImagePaths = value ?? new List<string>(); }
        }
    }
}