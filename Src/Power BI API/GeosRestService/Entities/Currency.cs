using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Currency
    {
        private string _Code;
        [DataMember(Order = 1)]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Symbol;
        [DataMember(Order = 2)]
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private string _Name;
        [DataMember(Order = 3)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
}
