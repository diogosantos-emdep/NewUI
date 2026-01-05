using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{
    [DataContract]
    public class LogEntriesByActionPlan : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActionPlanChangeLog;
        Int64 idActionPlan;
        Int32 idUser;
        DateTime? datetime;
        People people;
        string comments;
        string userName;
        #endregion

        #region Constructor

        public LogEntriesByActionPlan()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public Int64 IdActionPlanChangeLog
        {
            get { return idActionPlanChangeLog; }
            set
            {
                idActionPlanChangeLog = value;
                OnPropertyChanged("IdActionPlanChangeLog");
            }
        }
        [DataMember]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
            }
        }
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }
        [DataMember]
        public DateTime? Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }
        [DataMember]
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
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

