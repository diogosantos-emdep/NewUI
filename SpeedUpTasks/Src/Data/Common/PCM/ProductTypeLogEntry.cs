using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ProductTypeLogEntry : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idLogEntryByCptype;
        UInt64 idCPType;
        UInt32 idUser;
        DateTime datetime;
        string comments;

        string userName;
        #endregion

        #region Constructor
        public ProductTypeLogEntry()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdLogEntryByCptype
        {
            get
            {
                return idLogEntryByCptype;
            }

            set
            {
                idLogEntryByCptype = value;
                OnPropertyChanged("IdLogEntryByCptype");
            }
        }

        [DataMember]
        public UInt64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
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
