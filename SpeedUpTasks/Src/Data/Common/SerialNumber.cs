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
    [Table("serialnumbers")]
    [DataContract]
    public class SerialNumber : ModelBase, IDisposable
    {
        #region Declaration

        private Int64 idSerialNumber;
        private string code;
        private string extraCode;
        private Int64 idWarehouseDeliveryNoteItem;
        private Int64? idWarehouseProduct;
        private Int32 idArticle;
        private Int32 idWarehouse;
        private object masterItem;
        private bool isScanned;
     
        #endregion

        #region Properties

        [Key]
        [Column("idserialnumber")]
        [DataMember]
        public long IdSerialNumber
        {
            get { return idSerialNumber; }
            set
            {
                idSerialNumber = value;
                OnPropertyChanged("IdSerialNumber");
            }
        }

        [Column("code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("extraCode")]
        [DataMember]
        public string ExtraCode
        {
            get { return extraCode; }
            set
            {
                extraCode = value;
                OnPropertyChanged("ExtraCode");
            }
        }

        [Column("idWarehouseDeliveryNoteItem")]
        [DataMember]
        public long IdWarehouseDeliveryNoteItem
        {
            get { return idWarehouseDeliveryNoteItem; }
            set
            {
                idWarehouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWarehouseDeliveryNoteItem");
            }
        }

        [Column("idWarehouseProduct")]
        [DataMember]
        public long? IdWarehouseProduct
        {
            get { return idWarehouseProduct; }
            set
            {
                idWarehouseProduct = value;
                OnPropertyChanged("IdWarehouseProduct");
            }
        }

        [Column("idArticle")]
        [DataMember]
        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("idWarehouse")]
        [DataMember]
        public Int32 IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }


        [DataMember]
        [NotMapped]
        public object MasterItem
        {
            get { return masterItem; }
            set
            {
                masterItem = value;
                OnPropertyChanged("MasterItem");
            }
        }


        [DataMember]
        [NotMapped]
        public bool IsScanned
        {
            get { return isScanned; }
            set
            {
                isScanned = value;
                OnPropertyChanged("IsScanned");
            }
        }


      
        #endregion

        #region Constructor

        public SerialNumber()
        {
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
