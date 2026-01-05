using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
    [DataContract]
    public class TempWorklog : ModelBase, IDisposable
    {
        #region  Fields

        DateTime? startTime;
        DateTime? endTime;
        Int64 idOT;
        string description;
        Int32 idUser;
        WorklogUser worklogUser;
        DateTime? sTime;
        DateTime? eTime;
        string otCode;
        string hours;
        string customerName;
        int seconds;
        int extraSeconds;
        int idSite;
        Company site;
        TimeSpan totalTime;
        #endregion

        #region Constructor
        public TempWorklog()
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
        public WorklogUser WorklogUser
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
