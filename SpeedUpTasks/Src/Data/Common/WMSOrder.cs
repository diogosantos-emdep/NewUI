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
    [DataContract]
    public class WMSOrder : ModelBase, IDisposable
    {
        #region Fields
        Int64 idOffer;
        string offerCode;
        string offerDescription;
        string group;
        string plant;
        byte isCritical;
        string carriageMethod;
        string poCode;
        DateTime? poDate;
        string offerAssignedTo;
        Int64 noOfItems;
        string offerStatus;
        string offerStatusColor;
        DateTime? firstShipmentDate;
        DateTime? lastShipmentDate;
        Int64? daysElaspedFirstShipment;
        Int64? daysElaspedLastShipment;
        byte idOfferType;
        string offerTypeName;
        Country country;
        byte idCountry;
        #endregion

        #region Constructor
        public WMSOrder()
        {

        }
        #endregion

        #region Properties

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
                offerCode = value;
                OnPropertyChanged("OfferCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string OfferDescription
        {
            get
            {
                return offerDescription;
            }

            set
            {
                offerDescription = value;
                OnPropertyChanged("OfferDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [NotMapped]
        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [NotMapped]
        [DataMember]
        public Country Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [NotMapped]
        [DataMember]
        public string CarriageMethod
        {
            get
            {
                return carriageMethod;
            }

            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }

        [NotMapped]
        [DataMember]
        public string POCode
        {
            get
            {
                return poCode;
            }

            set
            {
                poCode = value;
                OnPropertyChanged("POCode");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PODate
        {
            get
            {
                return poDate;
            }

            set
            {
                poDate = value;
                OnPropertyChanged("PODate");
            }
        }


        [NotMapped]
        [DataMember]
        public string OfferAssignedTo
        {
            get
            {
                return offerAssignedTo;
            }

            set
            {
                offerAssignedTo = value;
                OnPropertyChanged("OfferAssignedTo");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 NoOfItems
        {
            get
            {
                return noOfItems;
            }

            set
            {
                noOfItems = value;
                OnPropertyChanged("NoOfItems");
            }
        }


        [NotMapped]
        [DataMember]
        public string OfferStatus
        {
            get
            {
                return offerStatus;
            }

            set
            {
                offerStatus = value;
                OnPropertyChanged("OfferStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string OfferStatusColor
        {
            get
            {
                return offerStatusColor;
            }

            set
            {
                offerStatusColor = value;
                OnPropertyChanged("OfferStatusColor");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? FirstShipmentDate
        {
            get
            {
                return firstShipmentDate;
            }

            set
            {
                firstShipmentDate = value;
                OnPropertyChanged("FirstShipmentDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? LastShipmentDate
        {
            get
            {
                return lastShipmentDate;
            }

            set
            {
                lastShipmentDate = value;
                OnPropertyChanged("LastShipmentDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? DaysElaspedFirstShipment
        {
            get
            {
                return daysElaspedFirstShipment;
            }

            set
            {
                daysElaspedFirstShipment = value;
                OnPropertyChanged("DaysElaspedFirstShipment");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IsCritical
        {
            get
            {
                return isCritical;
            }
            set
            {
                isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdOfferType
        {
            get
            {
                return idOfferType;
            }
            set
            {
                idOfferType = value;
                OnPropertyChanged("IdOfferType");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdCountry
        {
            get
            {
                return idCountry;
            }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [NotMapped]
        [DataMember]
        public string OfferTypeName
        {
            get
            {
                return offerTypeName;
            }
            set
            {
                offerTypeName = value;
                OnPropertyChanged("OfferTypeName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? DaysElaspedLastShipment
        {
            get
            {
                return daysElaspedLastShipment;
            }

            set
            {
                daysElaspedLastShipment = value;
                OnPropertyChanged("DaysElaspedLastShipment");
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
