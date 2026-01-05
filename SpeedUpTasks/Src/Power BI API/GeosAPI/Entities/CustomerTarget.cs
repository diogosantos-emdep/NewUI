using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class CustomerTarget
    {
        //private string _CustomerFullName=string.Empty;
        //[DataMember(Order=1)]
        //public string CustomerFullName
        //{
        //    get { return _CustomerFullName; }
        //    set { _CustomerFullName = value; }
        //}

        private string _Year = string.Empty;
        [DataMember(Order = 1)]
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _Group;
        [DataMember(Order = 2)]
        public string Group
        {
            get { return _Group; }
            set
            {
                if (value == null)
                    _Group = "";
                else
                    _Group = value;
            }
        }

        private string _Plant;
        [DataMember(Order = 3)]
        public string Plant
        {
            get { return _Plant; }
            set
            {
                if (value == null)
                    _Plant = "";
                else
                    _Plant = value;
            }
        }

        private string _Country;
        [DataMember(Order = 4)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }



        private string _TargetAmount = string.Empty;
        [DataMember(Order = 5)]
        public string Amount
        {
            get { return _TargetAmount; }
            set { _TargetAmount = value; }
        }

        private string _TargetCurrency = string.Empty;
        [DataMember(Order = 6)]
        public string Currency
        {
            get { return _TargetCurrency; }
            set { _TargetCurrency = value; }
        }

    }
}
