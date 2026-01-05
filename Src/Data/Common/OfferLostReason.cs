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
    [Table("offerlostreasons")]
    [DataContract(IsReference = true)]
    public class OfferLostReason : ModelBase, IDisposable
    {
        #region  Fields
        Byte idLostReason;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_ro;
        string name_ru;
        string name_zh;
        #endregion

        #region Constructor
        public OfferLostReason()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLostReason")]
        [DataMember]
        public Byte IdLostReason
        {
            get
            {
                return idLostReason;
            }
            set
            {
                idLostReason = value;
                OnPropertyChanged("IdLostReason");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Name_es")]
        [DataMember]
        public string Name_es
        {
            get
            {
                return name_es;
            }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [Column("Name_fr")]
        [DataMember]
        public string Name_fr
        {
            get
            {
                return name_fr;
            }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [Column("Name_pt")]
        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [Column("Name_ro")]
        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [Column("Name_ru")]
        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [Column("Name_zh")]
        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
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
