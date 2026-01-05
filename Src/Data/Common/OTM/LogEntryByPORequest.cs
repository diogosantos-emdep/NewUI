using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [001][ashish.malkhede][06-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7254
    /// </summary>
    [Table("LogEntryByPORequest")]
    [DataContract]
    public class LogEntryByPORequest : ModelBase, IDisposable
    {
        #region Fields
        Int64 idLogEntry;
        Int64 idPORequest;
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
        long idEmail;
        string fileName;
        #endregion

        #region Constructor
        public LogEntryByPORequest()
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
        public Int64 IdPORequest
        {
            get { return idPORequest; }
            set { idPORequest = value; }
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

        [NotMapped]
        [DataMember]
        public long IdEmail
        {
            get { return idEmail; }
            set
            {
                idEmail = value;
                OnPropertyChanged("IdEmail");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
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
