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
    [Table("partnumbers")]
    [DataContract]
    public class PartNumbers : ModelBase, IDisposable
    {
        Int32 idPartNumberType;
        Int32 idPartNumber;
        Int32 idPackingBox;
        Int32 idOtItem;
        string iD;
        string code;
        PartNumbersTracking partNumbersTracking;

        [Column("IdPartNumberType")]
        [DataMember]
        public Int32 IdPartNumberType
        {
            get
            {
                return idPartNumberType;
            }

            set
            {
                idPartNumberType = value;
                OnPropertyChanged("IdPartNumberType");
            }
        }

        [Key]
        [Column("IdPartNumber")]
        [DataMember]
        public Int32 IdPartNumber
        {
            get
            {
                return idPartNumber;
            }

            set
            {
                idPartNumber = value;
                OnPropertyChanged("IdPartNumber");
            }
        }

        [Column("IdPackingBox")]
        [DataMember]
        public Int32 IdPackingBox
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

        [Column("IdOtItem")]
        [DataMember]
        public Int32 IdOtItem
        {
            get
            {
                return idOtItem;
            }

            set
            {
                idOtItem = value;
                OnPropertyChanged("IdOtItem");
            }
        }

        [Column("ID")]
        [DataMember]
        public string ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
                OnPropertyChanged("ID");
            }
        }

        [Column("IdProduct")]
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

        [NotMapped]
        [DataMember]
        public PartNumbersTracking PartNumbersTracking
        {
            get { return partNumbersTracking; }
            set
            {
                partNumbersTracking = value;
                OnPropertyChanged("PartNumbersTracking");
            }
        }

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
