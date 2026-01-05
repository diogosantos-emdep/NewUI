using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{

    [Table("offeroptions")]
    [DataContract]
    public class OfferOption : ModelBase, IDisposable
    {

        #region  Fields
        Int64 idOfferOption;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        Int64 sortOrder;
        string name_ru;
        sbyte isObsolete;
        Int32 idOfferOptionType;
        OfferOptionType offerOptionType;
        List<OptionsByOffer> optionsByOffers;
        #endregion

        #region Constructor
        public OfferOption()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("idOfferOption")]
        [DataMember]
        public Int64 IdOfferOption
        {
            get
            {
                return idOfferOption;
            }
            set
            {
                idOfferOption = value;
                OnPropertyChanged("IdOfferOption");
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

        [Column("IsObsolete")]
        [DataMember]
        public sbyte IsObsolete
        {
            get
            {
                return isObsolete;
            }
            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }

        [ForeignKey("OfferOptionType")]
        [Column("IdOfferOptionType")]
        [DataMember]
        public Int32 IdOfferOptionType
        {
            get
            {
                return idOfferOptionType;
            }
            set
            {
                idOfferOptionType = value;
                OnPropertyChanged("IdOfferOptionType");
            }
        }

        [DataMember]
        public virtual OfferOptionType OfferOptionType
        {
            get
            {
                return offerOptionType;
            }
            set
            {
                offerOptionType = value;
                OnPropertyChanged("OfferOptionType");
            }
        }

        [DataMember]
        public virtual List<OptionsByOffer> OptionsByOffers
        {
            get
            {
                return optionsByOffers;
            }
            set
            {
                optionsByOffers = value;
                OnPropertyChanged("OptionsByOffers");
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
