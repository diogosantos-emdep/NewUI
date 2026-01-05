using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
    /// </summary>
    public class CustomerPlant: ModelBase, IDisposable
    {
        #region declare
        private int idCustomerPlant;
        private int idCustomer;
        private string customerPlantName;
        private int idCountry;
        string iso;
        string countryIconUrl;
        string city;
        string country;
        bool isEnabled;
        #endregion

        #region Properties

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }


        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }


        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }

        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }

        public int IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {

                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }

        public int IdCustomer
        {
            get
            {
                return idCustomer;
            }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        public string CustomerPlantName
        {
            get
            {
                return customerPlantName;
            }
            set
            {
                customerPlantName = value;
                OnPropertyChanged("CustomerPlantName");
            }
        }

        public int IdCountry
        {
            get
            {
                return idCountry;
            }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            { isEnabled = value; }
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
