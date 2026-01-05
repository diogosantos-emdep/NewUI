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
    [Table("optionsbyoffer")]
    [DataContract]
    public class OptionsByOffer : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idOffer;
        Int64 idOption;
        Int32? quantity;
        Offer offer;
        OfferOption offerOption;
        List<Template> templates;
        Int32 idOfferPlant;
        bool isSelected;
        #endregion

        #region Constructor
        public OptionsByOffer()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOffer")]
        [ForeignKey("Offer")]
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

        [Column("IdOption")]
        [ForeignKey("OfferOption")]
        [DataMember]
        public Int64 IdOption
        {
            get
            {
                return idOption;
            }
            set
            {
                idOption = value;
                OnPropertyChanged("IdOption");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public Int32? Quantity
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
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdOfferPlant
        {
            get
            {
                return idOfferPlant;
            }
            set
            {
                idOfferPlant = value;
                OnPropertyChanged("IdOfferPlant");
            }
        }

        [DataMember]
        public virtual Offer Offer
        {
            get
            {
                return offer;
            }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }

        
        [DataMember]
        public virtual OfferOption OfferOption
        {
            get
            {
                return offerOption;
            }
            set
            {
                offerOption = value;
                OnPropertyChanged("OfferOption");
            }
        }

        [DataMember]
        public virtual List<Template> Templates
        {
            get
            {
                return templates;
            }
            set
            {
                templates = value;
                OnPropertyChanged("Templates");
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
