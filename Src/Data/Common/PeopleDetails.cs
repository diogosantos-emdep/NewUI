using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;
namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class PeopleDetails
    {
        [DataMember]
        public int IdPerson { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string FullName
        {
            get { return Name + " " + Surname; }
            set { }
        }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Int32? IdSite { get; set; }

        [DataMember]
        public Int32? IdCustomer { get; set; }
        // [DataMember]
        //public ImageSource OwnerImage { get; set; }
        // [DataMember]
        // public bool IsSiteResponsibleExist { get; set; }
        // [DataMember]
        // public bool IsSalesResponsible { get; set; }
        // [DataMember]
        // public bool IsSelected { get; set; }
        // [DataMember]
        // public string AnnualSalesTargetAmount { get; set; }
        // [DataMember]
        // public string AnnualSalesTargetCurrency { get; set; }
        // [DataMember]
        // public string CustomerBusiness { get; set; }
        // [DataMember]
        // public Int64 CustomerSizeInSquareMeters { get; set; }
        // [DataMember]
        // public Int64 CustomerNumberOfEmployees { get; set; }
    }
}
