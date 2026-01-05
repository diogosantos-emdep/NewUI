using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    //[PRAMOD.MISAL][GEOS2-4442][29-08-2023]
    [DataContract]
    public class FreePlugins : ModelBase
    {

        #region Declaration

        Int64 idplugin;
        Int64 idCustomer;
        Int32 idRegion;
        Int32 idCountry;
        UInt32 idSite;
        string name;
        string group;
        string region;
        string country;
        string plant;
        Country countryforicon;
        string iso;
        string iso3;

        //string idPermission;
        Int64 idCustomerPrevious;
        Int64 idRegionPrevious;
        Int64 idCountryPrevious;
        Int64 idPlantPrevious;
        #endregion


        #region Properties

        [DataMember]
        public Int64 IdPlugin
        {
            get
            {
                return idplugin;
            }

            set
            {
                idplugin = value;
                OnPropertyChanged("IdPlugin");
            }
        }

        [DataMember]
        public Int64 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [DataMember]
        public Int32 IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [DataMember]
        public Int32 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [DataMember]
        public UInt32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

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

        [DataMember]
        public string Iso
        {
            get
            {
                return iso;
            }

            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }

        [DataMember]
        public string Iso3
        {
            get
            {
                return iso3;
            }

            set
            {
                iso3 = value;
                OnPropertyChanged("Iso3");
            }
        }

        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public Country CountryforIcon
        {
            get { return countryforicon; }
            set
            {
                countryforicon = value;
                OnPropertyChanged("CountryforIcon");
            }
        }

        List<Int32> selectedRegion;
        [DataMember]
        public List<Int32> SelectedRegion
        {
            get
            {
                return selectedRegion;
            }

            set
            {
                selectedRegion = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        List<Int32> selectedCountry;
        [DataMember]
        public List<Int32> SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }

        List<UInt32> selectedPlant;
        [DataMember]
        public List<UInt32> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged("SelectedPlant");
            }
        }
        [DataMember]
        public Int64 IdCustomerPrevious
        {
            get { return idCustomerPrevious; }
            set
            {
                idCustomerPrevious = value;
                OnPropertyChanged("IdCustomerPrevious");
            }
        }

        [DataMember]
        public Int64 IdRegionPrevious
        {
            get { return idRegionPrevious; }
            set
            {
                idRegionPrevious = value;
                OnPropertyChanged("IdRegionPrevious");
            }
        }
        [DataMember]
        public Int64 IdCountryPrevious
        {
            get { return idCountryPrevious; }
            set
            {
                idCountryPrevious = value;
                OnPropertyChanged("IdCountryPrevious");
            }
        }
        [DataMember]
        public Int64 IdPlantPrevious
        {
            get { return idPlantPrevious; }
            set
            {
                idPlantPrevious = value;
                OnPropertyChanged("IdPlantPrevious");
            }
        }
        #endregion


        #region Constructor

        public FreePlugins()
        {


        }


        #endregion


    }
}
