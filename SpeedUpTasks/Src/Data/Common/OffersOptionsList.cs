using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   public class OffersOptionsList : ModelBase, IDisposable
    {
        #region Fields
        IList<Offer> offers;
        List<OptionsByOffer> optionsByOffers;
        List<LostReasonsByOffer> lostReasonsByOffers;
        #endregion

        #region Constructor
        public OffersOptionsList()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public IList<Offer> Offers
        {
            get
            {
                return offers;
            }

            set
            {
                offers = value;
                OnPropertyChanged("Offers");
            }
        }

        [NotMapped]
        [DataMember]
        public List<OptionsByOffer> OptionsByOffers
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


        [NotMapped]
        [DataMember]
        public List<LostReasonsByOffer> LostReasonsByOffers
        {
            get
            {
                return lostReasonsByOffers;
            }

            set
            {
                lostReasonsByOffers = value;
                OnPropertyChanged("LostReasonsByOffers");
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
