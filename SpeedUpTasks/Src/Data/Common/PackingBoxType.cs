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
    [Table("packingboxtypes")]
    [DataContract]
    public class PackingBoxType : ModelBase, IDisposable
    {
        #region Fields
        Int64 idPackingBoxType;
        string code;
        double length;
        double width;
        double height;
        double netWeight;
        string sizeMeasurementUnit;
        string weightMeasurementUnit;
        Int64 sortOrder;
        #endregion

        #region Constructor
        public PackingBoxType()
        {

        }
        #endregion

        #region Properties
        [Key]
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

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
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
                OnPropertyChanged("SizeMeasurementUnit");
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


        [Column("SortOrder")]
        [DataMember]
        public Int64 SortOrder
        {
            get
            {
                return sortOrder;
            }

            set
            {
                sortOrder = value;
                OnPropertyChanged("SortOrder");
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
