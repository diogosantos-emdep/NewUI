using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class PlantTarget
    {
        private string _Year = string.Empty;
        [DataMember(Order = 1)]
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _PlantName = string.Empty;
        [DataMember(Order=2)]
        public string Plant
        {
            get { return _PlantName; }
            set { _PlantName = value; }
        }

        private string _BusinessUnitName = string.Empty;
        [DataMember(Order = 3)]
        public string BusinessUnit
        {
            get { return _BusinessUnitName; }
            set { _BusinessUnitName = value; }
        }

       
        private string _TargetAmount = string.Empty;
        [DataMember(Order = 4)]
        public string Amount
        {
            get { return _TargetAmount; }
            set { _TargetAmount = value; }
        }

        private string _TargetCurrency = string.Empty;
        [DataMember(Order = 5)]
        public string Currency
        {
            get { return _TargetCurrency; }
            set { _TargetCurrency = value; }
        }


    }
}
