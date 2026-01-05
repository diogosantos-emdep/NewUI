using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorLogEntry : ModelBase, IDisposable
    {

        #region Fields
        UInt32 idLogEntryByConnector;
        UInt64 idConnector;
        UInt32 idUser;
        DateTime datetime;
        string comments;
        string userName;
        UInt32 idLogEntryType;
        People people;
        UInt32 modifiedBy;
        DateTime modifiedDate;
        bool isDelVisible;

        #endregion

        #region Constructor
        public ConnectorLogEntry()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdLogEntryByConnector
        {
            get
            {
                return idLogEntryByConnector;
            }

            set
            {
                idLogEntryByConnector = value;
                OnPropertyChanged("IdLogEntryByConnector");
            }
        }

        [DataMember]
        public UInt64 IdConnector
        {
            get
            {
                return idConnector;
            }

            set
            {
                idConnector = value;
                OnPropertyChanged("IdConnector");
            }
        }

        [DataMember]
        public UInt32 IdUser
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
        public DateTime Datetime
        {
            get
            {
                return datetime;
            }

            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [DataMember]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
      
        [DataMember]
        public UInt32 IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
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
        public UInt32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
       
        [DataMember]
        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set
            {
                modifiedDate = value;
                OnPropertyChanged("ModifiedDate");
            }
        }

        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set
            {
                if (isDelVisible != value)
                {
                    isDelVisible = value;
                    OnPropertyChanged(nameof(IsDelVisible));
                }
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
            ConnectorLogEntry connectorLogEntry = (ConnectorLogEntry)this.MemberwiseClone();

            if (connectorLogEntry.People != null)
                connectorLogEntry.People = (People)this.People.Clone();

            return connectorLogEntry;
        }

        #endregion
    }
}
