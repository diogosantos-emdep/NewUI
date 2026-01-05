using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("logentriesbyarticle")]
    [DataContract]
    public class LogEntriesByArticle : ModelBase, IDisposable
    {
        #region Fields

        byte idLogEntryType;
        Int64 idLogEntry;
        Int64 idArticle;
        Int32 idUser;
        DateTime logDateTime;
        string comments;
     

        #endregion

        #region Constructor

        public LogEntriesByArticle()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntry")]
        [DataMember]
        public long IdLogEntry
        {
            get { return idLogEntry; }
            set
            {
                idLogEntry = value;
                OnPropertyChanged("IdLogEntry");
            }
        }

        [Column("IdLogEntryType")]
        [DataMember]
        public byte IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
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

        [Column("logDateTime")]
        [DataMember]
        public DateTime LogDateTime
        {
            get { return logDateTime; }
            set
            {
                logDateTime = value;
                OnPropertyChanged("LogDateTime");
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

        [Column("IdArticle")]
        [DataMember]
        public Int64 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
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
