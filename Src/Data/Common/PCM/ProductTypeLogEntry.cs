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

        UInt32 idLogEntryType;//[Sudhir.Jangra][GEOS2-4935]
        People people;//[Sudhir.Jangra][GEOS2-4935]
        UInt32 modifiedBy;//[Sudhir.Jangra][GEOS2-4935]
        DateTime modifiedDate;//[Sudhir.jangra][GEOS2-4935]
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

        //[Sudhir.Jangra][GEOS2-4935]
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

        //[Sudhir.Jangra][GEOS2-4935]
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


        //[Sudhir.Jangra][GEOS2-4935]
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

        //[Sudhir.jangra][GEOS2-4935]
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
