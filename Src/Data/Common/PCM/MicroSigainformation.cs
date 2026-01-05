using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class MicroSigainformation : ModelBase
    {
        string reference;
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        string ipi;
        [DataMember]
        public string IPI
        {
            get
            {
                return ipi;
            }
            set
            {
                ipi = value;
                OnPropertyChanged("IPI");
            }
        }

        string ncm;
        [DataMember]
        public string NCM
        {
            get
            {
                return ncm;
            }
            set
            {
                ncm = value;
                OnPropertyChanged("NCM");
            }
        }
    }

    public class microSigainformationRootObject
    {
        public List<MicroSigainformation> Values { get; set; }
        //public List<Dictionary<string, object>> Data { get; set; }
    }
}
