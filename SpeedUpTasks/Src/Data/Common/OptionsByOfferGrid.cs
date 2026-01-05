using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    [DataContract]
    public class OptionsByOfferGrid : INotifyPropertyChanged, ICloneable
    {
        #region  Fields
      
        Int64 idOffer;
        Int64 idOption;
        Int32? quantity;
        string offerOption;
        Int32 idOfferPlant;
        bool isSelected;
        #endregion

        #region Constructor
        public OptionsByOfferGrid()
        {
        }
        #endregion

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;
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
        public string OfferOption
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



        #endregion
        #region Methods
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
      
        #endregion
    }
}
