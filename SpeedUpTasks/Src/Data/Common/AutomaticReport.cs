using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("automatic_reports")]
    [DataContract]
    public class AutomaticReport : ModelBase, IDisposable
    {
        #region Fields

        Int16 idAutomaticReport;
        string name;
        DateTime startDate;
        string interval;
        byte isEnabled;

        #endregion

        #region Constructor

        public AutomaticReport()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("Id")]
        [DataMember]
        public Int16 IdAutomaticReport
        {
            get { return idAutomaticReport; }
            set
            {
                idAutomaticReport = value;
                OnPropertyChanged("IdAutomaticReport");
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

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("Interval")]
        [DataMember]
        public string Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                OnPropertyChanged("Interval");
            }
        }

        [Column("IsEnabled")]
        [DataMember]
        public byte IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
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
