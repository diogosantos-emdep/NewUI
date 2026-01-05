using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm 
{
    [DataContract]
    public class MealAllowance : ModelBase, IDisposable
    {
        #region Fields
        string companyAlias;
        EmployeeMealBudget regularEmp;
        EmployeeMealBudget globalEmp;
        int idCompany;//[pramod.misal][GEOS2-5365][12-03-2024]
        int idCompanyEmployeeMealBudget;//[pramod.misal][GEOS2-5365][12-03-2024]
        #endregion

        #region Properties
        [DataMember]
        public string CompanyAlias
        {
            get
            {
                return companyAlias;
            }

            set
            {
                companyAlias = value;
                OnPropertyChanged("CompanyAlias");
            }
        }
        [DataMember]
        public EmployeeMealBudget RegularEmp
        {
            get
            {
                return regularEmp;
            }

            set
            {
                regularEmp = value;
                OnPropertyChanged("RegularEmp");
            }
        }

        [DataMember]
        public EmployeeMealBudget GlobalEmp
        {
            get
            {
                return globalEmp;
            }

            set
            {
                globalEmp = value;
                OnPropertyChanged("GlobalEmp");
            }
        }

        int idCountry;
        [DataMember]
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


        int idOfficialCurrency;
        [DataMember]
        public int IdOfficialCurrency
        {
            get
            {
                return idOfficialCurrency;
            }

            set
            {
                idOfficialCurrency = value;
                OnPropertyChanged("IdOfficialCurrency");
            }
        }

        string countryIso;
        [DataMember]
        public string CountryIso
        {
            get
            {
                return countryIso;
            }

            set
            {
                countryIso = value;
                OnPropertyChanged("CountryIso");
            }
        }

        string currName;
        [DataMember]
        public string CurrName
        {
            get
            {
                return currName;
            }

            set
            {
                currName = value;
                OnPropertyChanged("CurrName");
            }
        }


        string countryName;
        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }
            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        byte[] countryBytes;
        [DataMember]
        public byte[] CountryBytes
        {
            get { return countryBytes; }
            set
            {
                countryBytes = value;
                OnPropertyChanged("CountryBytes");
            }
        }

        string countryIconUrl;
        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }

        //[pramod.misal][GEOS2-5365][12-03-2024]
        [DataMember]
        public int IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        //[pramod.misal][GEOS2-5365][12-03-2024]
        [DataMember]
        public int IdCompanyEmployeeMealBudget
        {
            get
            {
                return idCompanyEmployeeMealBudget;
            }

            set
            {
                idCompanyEmployeeMealBudget = value;
                OnPropertyChanged("IdCompanyEmployeeMealBudget");
            }
        }

        #endregion

        #region Constructor
        public MealAllowance()
        { }
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
