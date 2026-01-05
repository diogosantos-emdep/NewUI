using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeMealBudget : ModelBase, IDisposable
    {
        #region Fields
        int idCompanyEmployeeMealBudget;
        int idCompany;
        int idEmployeeProfile;
        double amount;
        int idCurrency;
        string currencySymbol;
        int isVoucherIncluded;
        string displayAmount;
        string employeeProfile;
        CurrencyConversion currencyConversion;
        #endregion

        #region Properties
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

        [DataMember]
        public int IdEmployeeProfile
        {
            get
            {
                return idEmployeeProfile;
            }

            set
            {
                idEmployeeProfile = value;
                OnPropertyChanged("IdEmployeeProfile");
            }
        }

        [DataMember]
        public int IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [DataMember]
        public int IsVoucherIncluded
        {
            get
            {
                return isVoucherIncluded;
            }

            set
            {
                isVoucherIncluded = value;
                OnPropertyChanged("IsVoucherIncluded");
            }
        }

        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }
        [DataMember]
        public string DisplayAmount
        {
            get
            {
                return displayAmount;
            }

            set
            {
                displayAmount = value;
                OnPropertyChanged("DisplayAmount");
            }
        }
        [DataMember]
        public string CurrencySymbol
        {
            get
            {
                return currencySymbol;
            }

            set
            {
                currencySymbol = value;
                OnPropertyChanged("CurrencySymbol");
            }
        }

        [DataMember]
        public string EmployeeProfile
        {
            get
            {
                return employeeProfile;
            }

            set
            {
                employeeProfile = value;
                OnPropertyChanged("EmployeeProfile");
            }
        }
     
        [DataMember]
        public CurrencyConversion CurrencyConversion
        {
            get
            {
                return currencyConversion;
            }

            set
            {
                currencyConversion = value;
                OnPropertyChanged("CurrencyConversion");
            }
        }
        #endregion

        #region Constructor
        public EmployeeMealBudget()
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
