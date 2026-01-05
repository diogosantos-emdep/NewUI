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
    [Table("counterparts")]
    [DataContract]
    public class Counterpart : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idCounterpart;
        Int64 idOTItem;
        string code;
        UInt64 idCounterpartType;
        UInt64 idShipment;
        UInt64 idPackingBox;

        CpType cpType;

        #endregion

        #region Constructor

        public Counterpart()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdCounterpart")]
        [DataMember]
        public ulong IdCounterpart
        {
            get { return idCounterpart; }
            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }

        [Column("IdOTItem")]
        [DataMember]
        public long IdOTItem
        {
            get { return idOTItem; }
            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [Column("Code")]
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

        [Column("IdCounterpartType")]
        [DataMember]
        public ulong IdCounterpartType
        {
            get { return idCounterpartType; }
            set
            {
                idCounterpartType = value;
                OnPropertyChanged("IdCounterpartType");
            }
        }

        [Column("IdShipment")]
        [DataMember]
        public ulong IdShipment
        {
            get { return idShipment; }
            set
            {
                idShipment = value;
                OnPropertyChanged("IdShipment");
            }
        }

        [Column("IdPackingBox")]
        [DataMember]
        public ulong IdPackingBox
        {
            get { return idPackingBox; }
            set
            {
                idPackingBox = value;
                OnPropertyChanged("IdPackingBox");
            }
        }

        [Column("CpType")]
        [DataMember]
        public CpType CpType
        {
            get { return cpType; }
            set
            {
                cpType = value;
                OnPropertyChanged("CpType");
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
