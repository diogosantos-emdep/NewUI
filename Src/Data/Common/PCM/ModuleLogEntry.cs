using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ModuleLogEntry : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idLogEntryByModule;
        UInt32 idModule;
        UInt32 idModuleType;
        UInt32 idUser;
        DateTime datetime;
        string comments;
        string userName;


        UInt32 idLogEntryType;
        People people;
        UInt32 modifiedBy;
        DateTime modifiedDate;
        #endregion

        #region Constructor
        public ModuleLogEntry()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdLogEntryByModule
        {
            get
            {
                return idLogEntryByModule;
            }

            set
            {
                idLogEntryByModule = value;
                OnPropertyChanged("IdLogEntryByModule");
            }
        }

        //[DataMember]
        //public UInt32 IdDetection
        //{
        //    get
        //    {
        //        return idDetection;
        //    }

        //    set
        //    {
        //        idDetection = value;
        //        OnPropertyChanged("IdDetection");
        //    }
        //}

        //[DataMember]
        //public UInt32 IdDetectionType
        //{
        //    get
        //    {
        //        return idDetectionType;
        //    }

        //    set
        //    {
        //        idDetectionType = value;
        //        OnPropertyChanged("IdDetectionType");
        //    }
        //}

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
