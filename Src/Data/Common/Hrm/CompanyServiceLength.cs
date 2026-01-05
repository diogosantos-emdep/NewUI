using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
	//[nsatpute][04-07-2024][GEOS2-5681]
    [DataContract]
    public class CompanyServiceLength : ModelBase, IDisposable
    {

        private int idAnnualHoliday;
        private int yearOfService;
        private string siteShortName;
        private float hodidays;
        private int idCompany;
        private string modifiedBy;
        private DateTime modifiedIn;

        [NotMapped]
        [DataMember]
        public int IdAnnualHoliday
        {
            get { return idAnnualHoliday; }
            set { idAnnualHoliday = value; OnPropertyChanged("IdAnnualHoliday"); }
        }

        [NotMapped]
        [DataMember]
        public int IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; OnPropertyChanged("IdCompany"); }
        }

        [NotMapped]
        [DataMember]
        public int YearOfService
        {
            get { return yearOfService; }
            set { yearOfService = value; OnPropertyChanged("YearOfService"); }
        }

        [NotMapped]
        [DataMember]
        public string SiteShortName
        {
            get { return siteShortName; }
            set { siteShortName = value; OnPropertyChanged("SiteShortName"); }
        }

        [NotMapped]
        [DataMember]
        public float Holidays
        {
            get { return hodidays; }
            set { hodidays = value; OnPropertyChanged("Hodidays"); }
        }

        [NotMapped]
        [DataMember]
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; OnPropertyChanged("ModifiedBy"); }
        }


        [NotMapped]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set { modifiedIn = value; OnPropertyChanged("ModifiedIn"); }
        }

        List<LogEntriesByCompanyLeaves> changeLog;
        [NotMapped]
        [DataMember]
        public List<LogEntriesByCompanyLeaves> ChangeLog
        {
            get { return changeLog; }
            set { changeLog = value; OnPropertyChanged("ChangeLog"); }
        }

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
