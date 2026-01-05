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
    [Table("logentriesbyoffer")]
    [DataContract]
    public class LogEntryByOffer : ModelBase, IDisposable
    {
        #region Fields
        Int64 idLogEntry;
        Int64 idOffer;
        Int32 idUser;
        DateTime? dateTime;
        string comments;
        byte idLogEntryType;
        Offer offer;
        People people;
        LogEntryByOfferType logEntryByOfferType;
        bool isDeleted;
        bool isUpdate;
        bool isRtfText;
        string realText;

        #endregion

        #region Constructor
        public LogEntryByOffer()
        {

        }
        #endregion

        #region Properties

        [Key]
        [Column("idLogEntry")]
        [DataMember]
        public Int64 IdLogEntry
        {
            get { return idLogEntry; }
            set { idLogEntry = value; }
        }

        [Column("idOffer")]
        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set { idOffer = value; }
        }

        [Column("idUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [Column("idLogEntryType")]
        [DataMember]
        public byte IdLogEntryType
        {
            get { return idLogEntryType; }
            set { idLogEntryType = value; }
        }

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
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
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
        public virtual Offer Offer
        {
            get { return offer; }
            set { offer = value; }
        }

        [NotMapped]
        [DataMember]
        public virtual LogEntryByOfferType LogEntryByOfferType
        {
            get { return logEntryByOfferType; }
            set { logEntryByOfferType = value; }
        }

        [NotMapped]
        [DataMember]
        public virtual People People
        {
            get { return people; }
            set { people = value; }
        }

        [NotMapped]
        [DataMember]
        public bool IsRtfText
        {
            get { return isRtfText; }
            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [NotMapped]
        [DataMember]
        public string RealText
        {
            get { return realText; }
            set
            {
                realText = value;
                OnPropertyChanged("RealText");
            }
        }

        #endregion

        #region methods

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
