using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class LogNewJobDescription : ModelBase, IDisposable
    {
        #region Fields
        Int64 idChangeLog;
        Int64 idGeosModule;
        Int64 idObject;
        DateTime datetime;
        Int32 idUser;
        string change;
        string user;
        List<LogNewJobDescription> logEntries;
        
        #endregion
        #region Constructor
        public LogNewJobDescription()
        {

        }
        #endregion
        #region Properties
        [NotMapped]
        [DataMember]
        public List<LogNewJobDescription> LogEntries
        {
            get
            {
                return logEntries;
            }

            set
            {
                logEntries = value;
                OnPropertyChanged("LogEntries");
            }
        }
        [Key]
        [Column("IdChangeLog")]
        [DataMember]
        public Int64 IdChangeLog
        {
            get { return idChangeLog; }
            set
            {
                idChangeLog = value;
                OnPropertyChanged("IdChangeLog");
            }
        }
       
        [Column("IdGeosModule")]
        [DataMember]
        public Int64 IdGeosModule
        {
            get { return idGeosModule; }
            set
            {
                idGeosModule = value;
                OnPropertyChanged("IdGeosModule");
            }
        }
        [Column("IdGeosModule")]
        [DataMember]
        public Int64 IdObject
        {
            get { return idObject; }
            set
            {
                idObject = value;
                OnPropertyChanged("IdObject");
            }
        }
        [Column("Datetime")]
        [DataMember]
        public DateTime Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }
        [Column("IdUser")]
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
        [Column("Change")]
        [DataMember]
        public string Change
        {
            get { return change; }
            set
            {
                change = value;
                OnPropertyChanged("Change");
            }
        }
        [NotMapped]
        [DataMember]
        public string User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        //public override object Clone()
        //{
        //    LogNewJobDecription obj = (LogNewJobDecription)this.MemberwiseClone();
        //    if (this.User != null)
        //        obj.User = (People)this.User.Clone();
        //    return obj;
        //}
        #endregion

    }
}
