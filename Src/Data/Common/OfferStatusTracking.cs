using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("offer_status_tracking")]
    [DataContract]
    public class OfferStatusTracking : ModelBase, IDisposable
    {
        #region Fields
        Int64 idOfferStatusTracking;
        Int64 idOffer;
        Int64 idOfferStatusType;
        DateTime startDate;
        string endDate;
        Double? noOfDays;
        SalesStatusType salesStatusType;
        #endregion

        #region Constructor
        public OfferStatusTracking()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdOfferStatusTracking")]
        [DataMember]
        public Int64 IdOfferStatusTracking
        {
            get
            {
                return idOfferStatusTracking;
            }

            set
            {
                idOfferStatusTracking = value;
                OnPropertyChanged("IdOfferStatusTracking");
            }
        }

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

        [Column("IdOfferStatusType")]
        [DataMember]
        public Int64 IdOfferStatusType
        {
            get
            {
                return idOfferStatusType;
            }

            set
            {
                idOfferStatusType = value;
                OnPropertyChanged("IdOfferStatusType");
            }
        }



        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }


        [Column("EndDate")]
        [DataMember]
        public string EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [NotMapped]
        [DataMember]
        public SalesStatusType SalesStatusType
        {
            get
            {
                return salesStatusType;
            }

            set
            {
                salesStatusType = value;
                OnPropertyChanged("SalesStatusType");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? NoOfDays
        {
            get
            {
                return noOfDays;
            }

            set
            {
                noOfDays = value;
                OnPropertyChanged("NoOfDays");
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
