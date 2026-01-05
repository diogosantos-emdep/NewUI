using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class SalesQuota
    {
       

        private string _Year = string.Empty;
        [DataMember(Order = 1)]
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _EmployeeFullName=string.Empty;
        [DataMember(Order = 2)]
        public string EmployeeFullName
        {
            get { return _EmployeeFullName; }
            set { _EmployeeFullName = value; }
        }

       

        private string _TargetAmount = string.Empty;
        [DataMember(Order = 3)]
        public string Amount
        {
            get { return _TargetAmount; }
            set { _TargetAmount = value; }
        }

        private string _TargetCurrency = string.Empty;
        [DataMember(Order = 4)]
        public string Currency
        {
            get { return _TargetCurrency; }
            set { _TargetCurrency = value; }
        }


    }
}
