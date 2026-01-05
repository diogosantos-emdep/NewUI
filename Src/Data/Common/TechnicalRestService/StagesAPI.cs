using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class StagesAPI
    {

        private int _IdStages = 0;
        [IgnoreDataMember]
        public int IdStages
        {
            get { return _IdStages; }
            set { _IdStages = value; }
        }
      
        private int _IdSequence = 0;
        [IgnoreDataMember]
        public int IdSequence
        {
            get { return _IdSequence; }
            set { _IdSequence = value; }
        }
        private string _Code = string.Empty;
        [IgnoreDataMember]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name = string.Empty;
        [IgnoreDataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }

        }
      
    }
}
