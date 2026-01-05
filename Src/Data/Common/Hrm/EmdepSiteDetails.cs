using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class EmdepSiteDetails : INotifyPropertyChanged
    {
		// [nsatpute][14-11-2024][GEOS2-5747]
	
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private int idCountry;
        private string countryName;
        private int idSite;
        private string siteName;
        private int idRegion;
        private string regionName;
        #endregion
        #region Properties
        [DataMember]
        public int IdCountry
        {
            get { return idCountry; }
            set
            {
                if (idCountry != value)
                {
                    idCountry = value;
                    OnPropertyChanged(nameof(IdCountry));
                }
            }
        }

        [DataMember]
        public string CountryName
        {
            get { return countryName; }
            set
            {
                if (countryName != value)
                {
                    countryName = value;
                    OnPropertyChanged(nameof(CountryName));
                }
            }
        }

        [DataMember]
        public int IdSite
        {
            get { return idSite; }
            set
            {
                if (idSite != value)
                {
                    idSite = value;
                    OnPropertyChanged(nameof(IdSite));
                }
            }
        }

        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                if (siteName != value)
                {
                    siteName = value;
                    OnPropertyChanged(nameof(SiteName));
                }
            }
        }

        [DataMember]
        public int IdRegion
        {
            get { return idRegion; }
            set
            {
                if (idRegion != value)
                {
                    idRegion = value;
                    OnPropertyChanged(nameof(IdRegion));
                }
            }
        }

        [DataMember]
        public string RegionName
        {
            get { return regionName; }
            set
            {
                if (regionName != value)
                {
                    regionName = value;
                    OnPropertyChanged(nameof(RegionName));
                }
            }
        }
        #endregion

        #region Methods
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
