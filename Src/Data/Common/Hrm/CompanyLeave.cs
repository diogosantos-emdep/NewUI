using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("company_leaves")]
    [DataContract]
    public class CompanyLeave : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idCompanyLeave;
        Int32 idCompany;
        string name;
        string htmlColor;
        Company company;
        UInt64 position;

        #endregion

        #region Properties

        [Key]
        [Column("IdCompanyLeave")]
        [DataMember]
        public ulong IdCompanyLeave
        {
            get { return idCompanyLeave; }
            set
            {
                idCompanyLeave = value;
                OnPropertyChanged("IdCompanyLeave");
            }
        }

        [Column("IdCompany")]
        [DataMember]
        public int IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [Column("Position")]
        [DataMember]
        public ulong Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        #endregion

        #region Constructor

        public CompanyLeave()
        {
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
