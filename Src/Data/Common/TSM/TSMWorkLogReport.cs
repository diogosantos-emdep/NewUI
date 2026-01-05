using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.SAM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.TSM
{
    //[GEOS2-8981][pallavi.kale][28.11.2025]
    public class TSMWorkLogReport : ModelBase, IDisposable
    {
        #region  Fields

        private DateTime? startTime;
        private DateTime? endTime;
        private Int64 idOT;
        private string description;
        private Int32 idUser;
        private TSMWorklogUser worklogUser;
        private DateTime? sTime;
        private DateTime? eTime;
        private string otCode;
        private string hours;
        private string customerName;
        private int seconds;
        private int extraSeconds;
        private int idSite;
        private Company site;
        private TimeSpan totalTime;
        private string offerCode;
        private Ots ot;
        private Employee employee;
        
        #endregion

        #region Constructor
        public TSMWorkLogReport()
        {

        }
        #endregion

        #region Properties


        [DataMember]
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [DataMember]
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public int IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [DataMember]
        public TSMWorklogUser WorklogUser
        {
            get
            {
                return worklogUser;
            }

            set
            {
                worklogUser = value;
                OnPropertyChanged("WorklogUser");
            }
        }

        [DataMember]
        public DateTime? STime
        {
            get
            {
                return sTime;
            }

            set
            {
                sTime = value;
                OnPropertyChanged("STime");
            }
        }

        [DataMember]
        public DateTime? ETime
        {
            get
            {
                return eTime;
            }

            set
            {
                eTime = value;
                OnPropertyChanged("ETime");
            }
        }
        [DataMember]
        public string OtCode
        {
            get
            {
                return otCode;
            }

            set
            {
                otCode = value;
                OnPropertyChanged("OtCode");
            }
        }

        [DataMember]
        public string Hours
        {
            get
            {
                return hours;
            }

            set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }

        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        [DataMember]
        public int Seconds
        {
            get
            {
                return seconds;
            }

            set
            {
                seconds = value;
                OnPropertyChanged("Seconds");
            }
        }
        [DataMember]
        public int ExtraSeconds
        {
            get
            {
                return extraSeconds;
            }

            set
            {
                extraSeconds = value;
                OnPropertyChanged("ExtraSeconds");
            }
        }

        [DataMember]
        public int IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("ExtraSeconds");
            }
        }
        [DataMember]
        public Company Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }

        [DataMember]
        public TimeSpan TotalTime
        {
            get
            {
                return totalTime;
            }

            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

        [DataMember]
        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
                offerCode = value;
                OnPropertyChanged("OfferCode");
            }
        }
        [DataMember]
        public Ots Ot
        {
            get
            {
                return ot;
            }
            set
            {
                ot = value;
                OnPropertyChanged("Ot");
            }
        }
        [DataMember]
        public Employee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }
     
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
