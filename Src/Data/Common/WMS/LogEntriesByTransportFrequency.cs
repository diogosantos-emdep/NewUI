using Emdep.Geos.Data.Common.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
	//[nsatpute][21.11.2025][GEOS2-9367]
    [DataContract]
    public class LogEntriesByTransportFrequency : ModelBase, IDisposable
    {
        #region Fields
        uint idChangeLog;
        int idTransportFrequency;
        int idUser;
        DateTime changeLogDateTime;
        string comment;
        string userName;
        #endregion

        #region Properties
        [DataMember]
        public uint IdChangeLog
        {
            get { return idChangeLog; }
            set
            {
                idChangeLog = value;
                OnPropertyChanged("IdChangeLog");
            }
        }

        [DataMember]
        public int IdTransportFrequency
        {
            get { return idTransportFrequency; }
            set
            {
                idTransportFrequency = value;
                OnPropertyChanged("IdTransportFrequency");
            }
        }

        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [DataMember]
        public DateTime ChangeLogDateTime
        {
            get { return changeLogDateTime; }
            set
            {
                changeLogDateTime = value;
                OnPropertyChanged("ChangeLogDateTime");
            }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
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
        public LogEntriesByTransportFrequency()
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
