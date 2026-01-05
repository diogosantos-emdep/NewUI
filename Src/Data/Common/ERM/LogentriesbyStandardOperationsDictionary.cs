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
    [Table("log_entries_by_standard_operations_dictionary")]
    [DataContract]
    public class LogentriesbyStandardOperationsDictionary : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idLogEntryBySOD;
        UInt64 idstandardoperationsdictionary;
        DateTime datetime;
        Int32 idUser;
        string comments;
        User changeLogUser;

        #endregion

        #region Constructor
        public LogentriesbyStandardOperationsDictionary()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryBySOD")]
        [DataMember]
        public ulong IdLogEntryBySOD
        {
            get { return idLogEntryBySOD; }
            set
            {
                idLogEntryBySOD = value;
                OnPropertyChanged("IdLogEntryBySOD");
            }
        }

        [Column("Idstandardoperationsdictionary")]
        [DataMember]
        public ulong Idstandardoperationsdictionary
        {
            get { return idstandardoperationsdictionary; }
            set
            {
                idstandardoperationsdictionary = value;
                OnPropertyChanged("Idstandardoperationsdictionary");
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
