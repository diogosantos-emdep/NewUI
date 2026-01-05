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
    [Table("lostreasonsbyoffer")]
    [DataContract(IsReference = true)]
    public class LostReasonsByOffer : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idOffer;
        string idLostReasonList;
        Int64? idCompetitor;
        string comments;
        Competitor competitor;
        Int64 numberOfOffers;
        OfferLostReason offerLostReason;
        Int32 idOfferPlant;
        #endregion

        #region Constructor
        public LostReasonsByOffer()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOffer")]
        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdLostReasonList")]
        [DataMember]
        public string IdLostReasonList
        {
            get
            {
                return idLostReasonList;
            }
            set
            {
                idLostReasonList = value;
                OnPropertyChanged("IdLostReasonList");
            }
        }

        [Column("IdCompetitor")]
        [DataMember]
        public Int64? IdCompetitor
        {
            get
            {
                return idCompetitor;
            }
            set
            {
                idCompetitor = value;
                OnPropertyChanged("IdCompetitor");
            }
        }

        [Column("Comments")]
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

        [NotMapped]
        [DataMember]
        public Int64 NumberOfOffers
        {
            get
            {
                return numberOfOffers;
            }
            set
            {
                numberOfOffers = value;
                OnPropertyChanged("NumberOfOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public Competitor Competitor
        {
            get
            {
                return competitor;
            }
            set
            {
                competitor = value;
                OnPropertyChanged("Competitor");
            }
        }

        [NotMapped]
        [DataMember]
        public OfferLostReason OfferLostReason
        {
            get
            {
                return offerLostReason;
            }
            set
            {
                offerLostReason = value;
                OnPropertyChanged("OfferLostReason");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdOfferPlant
        {
            get
            {
                return idOfferPlant;
            }
            set
            {
                idOfferPlant = value;
                OnPropertyChanged("IdOfferPlant");
            }
        }

        #endregion
        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
