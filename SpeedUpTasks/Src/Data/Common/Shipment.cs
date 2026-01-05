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
    [Table("shipments")]
    [DataContract]
    public class Shipment : ModelBase, IDisposable
    {
        #region Fields
        Int64  idShipment;
        DateTime deliveryDate;
        Int32 idTransportAgency;
        string shipmentNumber;
        Int32 idDeliveryNote;
        DateTime deliveryNoteDate;
        string emdepCode;
        sbyte closed;
        sbyte emailSent;
        Int32 createdBy;
        string comments;
        Int64 idSender;
        Int64 idRecipient;
        DateTime createdIn;
        Int32 modifiedBy;
        DateTime modifiedIn;
        string driverFullName;
        string driverId;
        string driverDrivingLicenseID;
        string driverVehicleInformation;
        DateTime shippingDate;
        string remisionNumber;
        TransportAgency transportAgency;
        People people;
        #endregion

        #region Constructor
        public Shipment()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdShipment")]
        [DataMember]
        public Int64 IdShipment
        {
            get
            {
                return idShipment;
            }

            set
            {
                idShipment = value;
                OnPropertyChanged("IdShipment");
            }
        }

        [Column("DeliveryDate")]
        [DataMember]
        public DateTime DeliveryDate
        {
            get
            {
                return deliveryDate;
            }

            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }

        [Column("IdTransportAgency")]
        [DataMember]
        public Int32 IdTransportAgency
        {
            get
            {
                return idTransportAgency;
            }

            set
            {
                idTransportAgency = value;
                OnPropertyChanged("IdTransportAgency");
            }
        }


        [Column("ShipmentNumber")]
        [DataMember]
        public string ShipmentNumber
        {
            get
            {
                return shipmentNumber;
            }

            set
            {
                shipmentNumber = value;
                OnPropertyChanged("ShipmentNumber");
            }
        }


        [Column("IdDeliveryNote")]
        [DataMember]
        public Int32 IdDeliveryNote
        {
            get
            {
                return idDeliveryNote;
            }

            set
            {
                idDeliveryNote = value;
                OnPropertyChanged("IdDeliveryNote");
            }
        }


        [Column("DeliveryNoteDate")]
        [DataMember]
        public DateTime DeliveryNoteDate
        {
            get
            {
                return deliveryNoteDate;
            }

            set
            {
                deliveryNoteDate = value;
                OnPropertyChanged("DeliveryNoteDate");
            }
        }


        [Column("EmdepCode")]
        [DataMember]
        public string EmdepCode
        {
            get
            {
                return emdepCode;
            }

            set
            {
                emdepCode = value;
                OnPropertyChanged("EmdepCode");
            }
        }

        [Column("Closed")]
        [DataMember]
        public sbyte Closed
        {
            get
            {
                return closed;
            }

            set
            {
                closed = value;
                OnPropertyChanged("Closed");
            }
        }


        [Column("EmailSent")]
        [DataMember]
        public sbyte EmailSent
        {
            get
            {
                return emailSent;
            }

            set
            {
                emailSent = value;
                OnPropertyChanged("EmailSent");
            }
        }


        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
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



        [Column("IdSender")]
        [DataMember]
        public Int64 IdSender
        {
            get
            {
                return idSender;
            }

            set
            {
                idSender = value;
                OnPropertyChanged("IdSender");
            }
        }


        [Column("IdRecipient")]
        [DataMember]
        public Int64 IdRecipient
        {
            get
            {
                return idRecipient;
            }

            set
            {
                idRecipient = value;
                OnPropertyChanged("IdRecipient");
            }
        }


        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }


        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }


        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }


        [Column("DriverFullName")]
        [DataMember]
        public string DriverFullName
        {
            get
            {
                return driverFullName;
            }

            set
            {
                driverFullName = value;
                OnPropertyChanged("DriverFullName");
            }
        }


        [Column("DriverId")]
        [DataMember]
        public string DriverId
        {
            get
            {
                return driverId;
            }

            set
            {
                driverId = value;
                OnPropertyChanged("DriverId");
            }
        }


        [Column("DriverDrivingLicenseID")]
        [DataMember]
        public string DriverDrivingLicenseID
        {
            get
            {
                return driverDrivingLicenseID;
            }

            set
            {
                driverDrivingLicenseID = value;
                OnPropertyChanged("DriverDrivingLicenseID");
            }
        }

        [Column("DriverVehicleInformation")]
        [DataMember]
        public string DriverVehicleInformation
        {
            get
            {
                return driverVehicleInformation;
            }

            set
            {
                driverVehicleInformation = value;
                OnPropertyChanged("DriverVehicleInformation");
            }
        }

        [Column("ShippingDate")]
        [DataMember]
        public DateTime ShippingDate
        {
            get
            {
                return shippingDate;
            }

            set
            {
                shippingDate = value;
                OnPropertyChanged("ShippingDate");
            }
        }

        [Column("RemisionNumber")]
        [DataMember]
        public string RemisionNumber
        {
            get
            {
                return remisionNumber;
            }

            set
            {
                remisionNumber = value;
                OnPropertyChanged("RemisionNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public TransportAgency TransportAgency
        {
            get
            {
                return transportAgency;
            }

            set
            {
                transportAgency = value;
                OnPropertyChanged("TransportAgency");
            }
        }


        [NotMapped]
        [DataMember]
        public People People
        {
            get
            {
                return people;
            }

            set
            {
                people = value;
                OnPropertyChanged("People");
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
