using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class EmployeeExpenseStatus
    {
        private int? idModifier;
        [Column("IdModifier")]
        [DataMember]
        public int? IdModifier
        {
            get { return idModifier; }
            set
            {
                if (idModifier != value)
                {
                    idModifier = value;
                    OnPropertyChanged(nameof(IdModifier));
                }
            }
        }

        private string fullname;
        [Column("FullName")]
        [DataMember]
        public string FullName
        {
            get { return fullname; }
            set
            {
                if (fullname != value)
                {
                    fullname = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }
        private string companyEmail;

        [Column("CompanyEmail")]
        [DataMember]
        public string CompanyEmail
        {
            get { return companyEmail; }
            set
            {
                if (companyEmail != value)
                {
                    companyEmail = value;
                    OnPropertyChanged(nameof(CompanyEmail));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
