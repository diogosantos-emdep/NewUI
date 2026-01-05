using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class LogEntriesByCompanyLeaves : ModelBase, IDisposable
    {
        Int64 idCompanyLeavesChangeLog;
        Int32 idCompany;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        string userName;

        [DataMember]
        public Int64 IdCompanyLeavesChangeLog
        {
            get { return idCompanyLeavesChangeLog; }
            set
            {
                idCompanyLeavesChangeLog = value;
                OnPropertyChanged("IdCompanyLeavesChangeLog");
            }
        }

        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [DataMember]
        public Int32 ChangeLogIdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("ChangeLogIdUser");
            }
        }

        [DataMember]
        public DateTime? ChangeLogDatetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("ChangeLogDatetime");
            }
        }

        [DataMember]
        public string ChangeLogChange
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("ChangeLogChange");
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


        public void Dispose()
        {
        }
    }
}
