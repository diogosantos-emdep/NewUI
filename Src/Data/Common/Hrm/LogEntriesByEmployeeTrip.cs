using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[Sudhir.Jangra][GEOS2-4816]
    [DataContract]
    public class LogEntriesByEmployeeTrip : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idEmployeeChangeLog;
        UInt32 idEmployeeTrip;
        UInt32 idUser;
        DateTime dateTimeChangeLog;
        string comments;
        string userName;
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdEmployeeChangeLog
        {
            get { return idEmployeeChangeLog; }
            set
            {
                idEmployeeChangeLog = value;
                OnPropertyChanged("IdEmployeeChangeLog");
            }
        }

        [DataMember]
        public UInt32 IdEmployeeTrip
        {
            get { return idEmployeeTrip; }
            set
            {
                idEmployeeTrip = value;
                OnPropertyChanged("IdEmployeeTrip");
            }
        }

        [DataMember]
        public UInt32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [DataMember]
        public DateTime DateTimeChangeLog
        {
            get { return dateTimeChangeLog; }
            set
            {
                dateTimeChangeLog = value;
                OnPropertyChanged("DateTimeChangeLog");
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

        #region Constructor
        public LogEntriesByEmployeeTrip()
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
