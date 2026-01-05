using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows;

namespace Emdep.Geos.Data.Common
{
    [Table("packingboxes")]
    [DataContract]
    public class PackingBox : ModelBase, IDisposable
    {
        #region Fields
        Int64 idPackingBox;
        Int32 idSite;
        string boxNumber;
        double length;
        double width;
        double height;
        string sizeMeasurementUnit;
        string weightMeasurementUnit;
        Int64 idPackingBoxType;
        double netWeight;
        double grossWeight;
        sbyte isClosed;
        string comments;
        Int64? idShipment;
        Shipment shipment;
        PackingBoxType packingBoxType;
        Int64 quantity;
        string packingBoxDimension;
        Int64 itemsInBox;
        Int64? idCountryGroup;
        CountryGroup countryGroup;
        sbyte isStackable;
        Visibility isVisibleCountryGroup;
        long? idWarehouse;
        #endregion

        #region Constructor
        public PackingBox()
        {

        }
        #endregion

        #region Properties

        [Column("IdWarehouse")]
        [DataMember]
        public long? IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Key]
        [Column("IdPackingBox")]
        [DataMember]
        public Int64 IdPackingBox
        {
            get
            {
                return idPackingBox;
            }

            set
            {
                idPackingBox = value;
                OnPropertyChanged("IdPackingBox");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("idSite");
            }
        }

        [Column("BoxNumber")]
        [DataMember]
        public string BoxNumber
        {
            get
            {
                return boxNumber;
            }

            set
            {
                boxNumber = value;
                OnPropertyChanged("BoxNumber");
            }
        }


        [Column("Length")]
        [DataMember]
        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }


        [Column("Width")]
        [DataMember]
        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }


        [Column("Height")]
        [DataMember]
        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }


        [Column("SizeMeasurementUnit")]
        [DataMember]
        public string SizeMeasurementUnit
        {
            get
            {
                return sizeMeasurementUnit;
            }

            set
            {
                sizeMeasurementUnit = value;
                OnPropertyChanged("sizeMeasurementUnit");
            }
        }

        [Column("WeightMeasurementUnit")]
        [DataMember]
        public string WeightMeasurementUnit
        {
            get
            {
                return weightMeasurementUnit;
            }

            set
            {
                weightMeasurementUnit = value;
                OnPropertyChanged("WeightMeasurementUnit");
            }
        }


        [Column("IdPackingBoxType")]
        [DataMember]
        public Int64 IdPackingBoxType
        {
            get
            {
                return idPackingBoxType;
            }

            set
            {
                idPackingBoxType = value;
                OnPropertyChanged("IdPackingBoxType");
            }
        }


        [Column("NetWeight")]
        [DataMember]
        public double NetWeight
        {
            get
            {
                return netWeight;
            }

            set
            {
                netWeight = value;
                OnPropertyChanged("NetWeight");
            }
        }


        [Column("GrossWeight")]
        [DataMember]
        public double GrossWeight
        {
            get
            {
                return grossWeight;
            }

            set
            {
                grossWeight = value;
                OnPropertyChanged("GrossWeight");
            }
        }


        [Column("IsClosed")]
        [DataMember]
        public sbyte IsClosed
        {
            get
            {
                return isClosed;
            }

            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
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


        [Column("IdShipment")]
        [DataMember]
        public Int64? IdShipment
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

        [NotMapped]
        [DataMember]
        public Int64 Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [NotMapped]
        [DataMember]
        public string PackingBoxDimension
        {
            get
            {
                return packingBoxDimension;
            }

            set
            {
                packingBoxDimension = value;
                OnPropertyChanged("PackingBoxDimension");
            }
        }

        [NotMapped]
        [DataMember]
        public Shipment Shipment
        {
            get
            {
                return shipment;
            }

            set
            {
                shipment = value;
                OnPropertyChanged("Shipment");
            }
        }


        [NotMapped]
        [DataMember]
        public PackingBoxType PackingBoxType
        {
            get
            {
                return packingBoxType;
            }

            set
            {
                packingBoxType = value;
                OnPropertyChanged("PackingBoxType");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 ItemsInBox
        {
            get
            {
                return itemsInBox;
            }

            set
            {
                itemsInBox = value;
                OnPropertyChanged("ItemsInBox");
            }
        }
        [Column("IdCountryGroup")]
        [DataMember]
        public Int64? IdCountryGroup
        {
            get
            {
                return idCountryGroup;
            }

            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }


        [DataMember]
        public CountryGroup CountryGroup
        {
            get
            {
                return countryGroup;
            }

            set
            {
                countryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        [DataMember]
        public Visibility IsVisibleCountryGroup
        {
            get
            {
                return isVisibleCountryGroup;
            }

            set
            {
                isVisibleCountryGroup = value;
                OnPropertyChanged("IsVisibleCountryGroup");
            }
        }

        [DataMember]
        public sbyte IsStackable
        {
            get { return isStackable; }
            set { isStackable = value; OnPropertyChanged("IsStackable"); }
        }

        Int64 idCarriageMethod;
        [DataMember]
        public Int64 IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }
        string carriageMethod;
        [DataMember]
        public string CarriageMethod
        {
            get { return carriageMethod; }
            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }

        string carriageMethodHtmlColor;
        [DataMember]
        public string CarriageMethodHtmlColor
        {
            get { return carriageMethodHtmlColor; }
            set
            {
                carriageMethodHtmlColor = value;
                OnPropertyChanged("CarriageMethodHtmlColor");
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
