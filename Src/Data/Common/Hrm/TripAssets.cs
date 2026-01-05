using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
	// [nsatpute][11-09-2024][GEOS2-5929]
    [DataContract]
    public class TripAssets : ModelBase, IDisposable
    {

        #region Fields
        private string publicIdentifier;
        private string brand;
        private string model;
        private string location;
        private int idAsset;
        private int idType;
        private string assetName;
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string PublicIdentifier
        {
            get { return publicIdentifier; }
            set
            {
                publicIdentifier = value;
                OnPropertyChanged("PublicIdentifier");
            }
        }

        [NotMapped]
        [DataMember]
        public string Brand
        {
            get { return brand; }
            set
            {
                brand = value;
                OnPropertyChanged("Brand");
            }
        }

        [NotMapped]
        [DataMember]
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }

        [NotMapped]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [NotMapped]
        [DataMember]
        public string AssetType
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdAsset
        {
            get { return idAsset; }
            set
            {
                idAsset = value;
                OnPropertyChanged("IdAsset");
            }
        }


        [NotMapped]
        [DataMember]
        public int IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [NotMapped]
        [DataMember]
        public string AssetName
        {
            get { return assetName; }
            set
            {
                assetName = value;
                OnPropertyChanged("AssetName");
            }
        }

        #endregion


        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}



