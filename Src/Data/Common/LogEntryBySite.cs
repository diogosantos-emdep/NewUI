using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("log_entries_by_site")]
    [DataContract]
    public class LogEntryBySite
    {
        #region Fields
        Int64 idLogEntryBySite;
        Int64 idSite;
        Int32 idUser;
        DateTime? dateTime;
        string comments;
        //byte idLogEntryType;
        Company site;
        People people;
       // LogEntryByOfferType logEntryByOfferType;
        bool isDeleted;
        bool isUpdate;
        #endregion

        #region Constructor
        public LogEntryBySite()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdLogEntriesBySite")]
        [DataMember]
        public Int64 IdLogEntryBySite
        {
            get { return idLogEntryBySite; }
            set { idLogEntryBySite = value; }
        }

        [Column("idSite")]
        [DataMember]
        public Int64 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [Column("idUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        //[Column("idLogEntryType")]
        //[DataMember]
        //public byte IdLogEntryType
        //{
        //    get { return idLogEntryType; }
        //    set { idLogEntryType = value; }
        //}

        [Column("datetime")]
        [DataMember]
        public DateTime? DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; }
        }

        [NotMapped]
        [DataMember]
        public virtual Company Site
        {
            get { return site; }
            set { site = value; }
        }

        //[NotMapped]
        //[DataMember]
        //public virtual LogEntryByOfferType LogEntryByOfferType
        //{
        //    get { return logEntryByOfferType; }
        //    set { logEntryByOfferType = value; }
        //}

        [NotMapped]
        [DataMember]
        public virtual People People
        {
            get { return people; }
            set { people = value; }
        }
        #endregion
    }
}
