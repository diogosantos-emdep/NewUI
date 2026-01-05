using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [Table("ModuleEquivalenceWeight_change_log")]
    [DataContract]
    public class LogEntryByModuleEquivalenceWeight : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idCPType;
        UInt64 idLogEntryByMEW;
        UInt64 iDCPTypeEquivalent;
        DateTime datetime;
        Int32 idUser;
        string comments;
        User changeLogUser;

        #endregion

        #region Constructor
        public LogEntryByModuleEquivalenceWeight()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByMEW")]
        [DataMember]
        public ulong IdLogEntryByMEW
        {
            get { return idLogEntryByMEW; }
            set
            {
                idLogEntryByMEW = value;
                OnPropertyChanged("IdLogEntryByMEW");
            }
        }

        [Column("IdCPType")]
        [DataMember]
        public Int32 IdCPType
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
        //[Column("IDCPTypeEquivalent")]
        //[DataMember]
        //public ulong IDCPTypeEquivalent
        //{
        //    get { return iDCPTypeEquivalent; }
        //    set
        //    {
        //        iDCPTypeEquivalent = value;
        //        OnPropertyChanged("IDCPTypeEquivalent");
        //    }
        //}

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
        public int IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [Column("Comments")]
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

        [NotMapped]
        [DataMember]
        public User ChangeLogUser
        {
            get { return changeLogUser; }
            set
            {
                changeLogUser = value;
                OnPropertyChanged("ChangeLogUser");
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
