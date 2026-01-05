using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    public class OptionByOfferSerilize
    {
        #region Fields
        string idoffer;
        string idOption;
        string quantity;
        string offerOption;
        string idOfferOptionType;
        string offerOptionType;
        #endregion
        #region Properties
        public string Idoffer
        {
            get
            {
                return idoffer;
            }

            set
            {
                idoffer = value;
            }
        }

        public string IdOption
        {
            get
            {
                return idOption;
            }

            set
            {
                idOption = value;
            }
        }

        public string Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        public string OfferOption
        {
            get
            {
                return offerOption;
            }

            set
            {
                offerOption = value;
            }
        }

        public string IdOfferOptionType
        {
            get
            {
                return idOfferOptionType;
            }

            set
            {
                idOfferOptionType = value;
            }
        }

        public string OfferOptionType
        {
            get
            {
                return offerOptionType;
            }

            set
            {
                offerOptionType = value;
            }
        }

        #endregion
    }
}
