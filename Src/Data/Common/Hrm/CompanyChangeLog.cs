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
    [Table("company_changelog")]
    [DataContract]
    public class CompanyChangelog : ModelBase, IDisposable
    {

        #region Fields

        UInt64 idCompanyChangeLog;
        Int32 idCompany;
        DateTime changeLogDatetime;
        Int32 changeLogIdUser;
        string changeLogChange;
        User changeLogUser;

        #endregion

        #region Constructor
        public CompanyChangelog()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdCompanyChangeLog")]
        [DataMember]
        public ulong IdCompanyChangeLog
        {
            get { return idCompanyChangeLog; }
            set
            {
                idCompanyChangeLog = value;
                OnPropertyChanged("IdCompanyChangeLog");
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

        [Column("ChangeLogDatetime")]
        [DataMember]
        public DateTime ChangeLogDatetime
        {
            get { return changeLogDatetime; }
            set
            {
                changeLogDatetime = value;
                OnPropertyChanged("ChangeLogDatetime");
            }
        }

        [Column("ChangeLogIdUser")]
        [DataMember]
        public int ChangeLogIdUser
        {
            get { return changeLogIdUser; }
            set
            {
                changeLogIdUser = value;
                OnPropertyChanged("ChangeLogIdUser");
            }
        }

        [Column("ChangeLogChange")]
        [DataMember]
        public string ChangeLogChange
        {
            get { return changeLogChange; }
            set
            {
                changeLogChange = value;
                OnPropertyChanged("ChangeLogChange");
            }
        }

        [NotMapped]
        [DataMember]
        public User ChangeLogUser
        {
            get { return changeLogUser; }
            set
            {
                changeLogUser = value;
                OnPropertyChanged("ChangeLogUser");
            }
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
