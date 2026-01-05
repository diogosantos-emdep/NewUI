using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DetectionLogEntry : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idLogEntryByDetection;
        UInt32 idDetection;
        UInt32 idDetectionType;
        UInt32 idUser;
        DateTime datetime;
        string comments;
        string userName;

        #endregion

        #region Constructor
        public DetectionLogEntry()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdLogEntryByDetection
        {
            get
            {
                return idLogEntryByDetection;
            }

            set
            {
                idLogEntryByDetection = value;
                OnPropertyChanged("IdLogEntryByDetection");
            }
        }

        [DataMember]
        public UInt32 IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
            }
        }

        [DataMember]
        public UInt32 IdDetectionType
        {
            get
            {
                return idDetectionType;
            }

            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
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
