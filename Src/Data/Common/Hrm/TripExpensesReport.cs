using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class TripExpensesReport : ModelBase, IDisposable
    {

        Int64 _IdEmployeeTrip;
        [DataMember]
        public Int64 IdEmployeeTrip
        {
            get { return _IdEmployeeTrip; }
            set
            {
                _IdEmployeeTrip = value;
                OnPropertyChanged("IdEmployeeTrip");
            }
        }


        string _TripCode;
        [DataMember]
        public string TripCode
        {
            get { return _TripCode; }
            set
            {
                _TripCode = value;
                OnPropertyChanged("TripCode");
            }
        }
        Int64 _IdEmployeeExpenseReport;
        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return _IdEmployeeExpenseReport; }
            set
            {
                _IdEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }


        string _ExpenseReportCode;
        [DataMember]
        public string ExpenseReportCode
        {
            get { return _ExpenseReportCode; }
            set
            {
                _ExpenseReportCode = value;
                OnPropertyChanged("ExpenseReportCode");
            }
        }


        string _companyPlant;
        [DataMember]
        public string CompanyPlant
        {
            get { return _companyPlant; }
            set
            {
                _companyPlant = value;
                OnPropertyChanged("CompanyPlant");
            }
        }

        string _company;
        [DataMember]
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
        }
        DateTime _submitDate;
        [DataMember]
        public DateTime SubmitDate
        {
            get { return _submitDate; }
            set
            {
                _submitDate = value;
                OnPropertyChanged("SubmitDate");
            }
        }

        DateTime _fromDate;
        [DataMember]
        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        DateTime _toDate;
        [DataMember]
        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        string _tripTitle;
        [DataMember]
        public string TripTitle
        {
            get { return _tripTitle; }
            set
            {
                _tripTitle = value;
                OnPropertyChanged("TripTitle");
            }
        }

        string _tripReason;
        [DataMember]
        public string TripReason
        {
            get { return _tripReason; }
            set
            {
                _tripReason = value;
                OnPropertyChanged("TripReason");
            }
        }
        string _employeeName;
        [DataMember]
        public string EmployeeName
        {
            get { return _employeeName; }
            set
            {
                _employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }

        string _organization;
        [DataMember]
        public string Organization
        {
            get { return _organization; }
            set
            {
                _organization = value;
                OnPropertyChanged("Organization");
            }
        }

        string _departmentName;
        [DataMember]
        public string DepartmentName
        {
            get { return _departmentName; }
            set
            {
                _departmentName = value;
                OnPropertyChanged("DepartmentName");
            }
        }

        string _jobTitle;
        [DataMember]
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                _jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }

        List<TripExpensesReportTravelLiquidation> _TravelLiquidation;
        [DataMember]
        public List<TripExpensesReportTravelLiquidation> TravelLiquidation
        {
            get { return _TravelLiquidation; }
            set
            {
                _TravelLiquidation = value;
                OnPropertyChanged("TravelLiquidation");
            }
        }

        TripExpensesReportMealBudget _MealBudget;
        [DataMember]
        public TripExpensesReportMealBudget MealBudget
        {
            get { return _MealBudget; }
            set
            {
                _MealBudget = value;
                OnPropertyChanged("MealBudget");
            }
        }

        List<TripTravelExpenseReport> _TravelExpenses;
        [DataMember]
        public List<TripTravelExpenseReport> TravelExpenses
        {
            get { return _TravelExpenses; }
            set
            {
                _TravelExpenses = value;
                OnPropertyChanged("TravelExpenses");
            }
        }

        private List<TripEmployeeExpenseRemark> _ExpenseRemark;
        [DataMember]
        public List<TripEmployeeExpenseRemark> ExpenseRemark
        {
            get { return _ExpenseRemark; }
            set
            {
                _ExpenseRemark = value;
                OnPropertyChanged("ExpenseRemark");
            }
        }
		// [nsatpute][10-12-2024][GEOS2-6367]
        private List<TripExpenseReportRemark> expenseReportRemarks;
        [DataMember]
        public List<TripExpenseReportRemark> ExpenseReportRemarks
        {
            get { return expenseReportRemarks; }
            set
            {
                expenseReportRemarks = value;
                OnPropertyChanged("ExpenseReportRemarks");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    [DataContract]
    public class TripExpensesReportTravelLiquidation : ModelBase, IDisposable
    {
        Int64 _IdEmployeeExpenseReport;
        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return _IdEmployeeExpenseReport; }
            set
            {
                _IdEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }

        //pramod.misal geos2-5793 24.06.2024
        Int32 _idCompany;
        [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return _idCompany; }
            set
            {
                _idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        //pramod.misal geos2-5793 24.06.2024
        string alias;
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }


        string _ExpenseReportCode;
        [DataMember]
        public string ExpenseReportCode
        {
            get { return _ExpenseReportCode; }
            set
            {
                _ExpenseReportCode = value;
                OnPropertyChanged("ExpenseReportCode");
            }
        }

        Int64 _IdReportFrom;
        [DataMember]
        public Int64 IdReportFrom
        {
            get { return _IdReportFrom; }
            set
            {
                _IdReportFrom = value;
                OnPropertyChanged("IdReportFrom");
            }
        }

        Int64 _IdReportTo;
        [DataMember]
        public Int64 IdReportTo
        {
            get { return _IdReportTo; }
            set
            {
                _IdReportTo = value;
                OnPropertyChanged("IdReportTo");
            }
        }


        Int32 _IdCurrencyFrom;
        [DataMember]
        public Int32 IdCurrencyFrom
        {
            get { return _IdCurrencyFrom; }
            set
            {
                _IdCurrencyFrom = value;
                OnPropertyChanged("IdCurrencyFrom");
            }
        }

        string _PlantCurrency;
        [DataMember]
        public string PlantCurrency
        {
            get { return _PlantCurrency; }
            set
            {
                _PlantCurrency = value;
                OnPropertyChanged("PlantCurrency");
            }
        }

        Int32 _IdCurrencyTo;
        [DataMember]
        public Int32 IdCurrencyTo
        {
            get { return _IdCurrencyTo; }
            set
            {
                _IdCurrencyTo = value;
                OnPropertyChanged("IdCurrencyTo");
            }
        }

        string _CurrencyName;
        [DataMember]
        public string CurrencyName
        {
            get { return _CurrencyName; }
            set
            {
                _CurrencyName = value;
                OnPropertyChanged("CurrencyName");
            }
        }

        double _CashAdvanced;
        [DataMember]
        public double CashAdvanced
        {
            get { return _CashAdvanced; }
            set
            {
                _CashAdvanced = value;
                OnPropertyChanged("CashAdvanced");
            }
        }
        string cashAdvancedString;
        [DataMember]
        public string CashAdvancedString
        {
            get { return cashAdvancedString; }
            set
            {
                cashAdvancedString = value;
                OnPropertyChanged("CashAdvancedString");
            }
        }

        Int32 _IdPayment;
        [DataMember]
        public Int32 IdPayment
        {
            get { return _IdPayment; }
            set
            {
                _IdPayment = value;
                OnPropertyChanged("IdPayment");
            }
        }


        string _PaymentMethod;
        [DataMember]
        public string PaymentMethod
        {
            get { return _PaymentMethod; }
            set
            {
                _PaymentMethod = value;
                OnPropertyChanged("PaymentMethod");
            }
        }

        double _TotalExpense;
        [DataMember]
        public double TotalExpense
        {
            get { return _TotalExpense; }
            set
            {
                _TotalExpense = value;
                OnPropertyChanged("TotalExpense");
            }
        }

        double _TotalCash;
        [DataMember]
        public double TotalCash
        {
            get { return _TotalCash; }
            set
            {
                _TotalCash = value;
                OnPropertyChanged("TotalCash");
            }
        }

        double _PersonalCredit;
        [DataMember]
        public double PersonalCredit
        {
            get { return _PersonalCredit; }
            set
            {
                _PersonalCredit = value;
                OnPropertyChanged("PersonalCredit");
            }
        }

        double _BankTransfer;
        [DataMember]
        public double BankTransfer
        {
            get { return _BankTransfer; }
            set
            {
                _BankTransfer = value;
                OnPropertyChanged("BankTransfer");
            }
        }

        double _CompanyCard;
        [DataMember]
        public double CompanyCard
        {
            get { return _CompanyCard; }
            set
            {
                _CompanyCard = value;
                OnPropertyChanged("CompanyCard");
            }
        }

        double _Companyvoucher;
        [DataMember]
        public double Companyvoucher
        {
            get { return _Companyvoucher; }
            set
            {
                _Companyvoucher = value;
                OnPropertyChanged("Companyvoucher");
            }
        }


        double companyvoucherString;
        [DataMember]
        public double ompanyvoucherString
        {
            get { return companyvoucherString; }
            set
            {
                companyvoucherString = value;
                OnPropertyChanged("Companyvoucher");
            }
        }

        double _PaidByOthers;
        [DataMember]
        public double PaidByOthers
        {
            get { return _PaidByOthers; }
            set
            {
                _PaidByOthers = value;
                OnPropertyChanged("PaidByOthers");
            }
        }


        double _CurrencyConversionRate;
        [DataMember]
        public double CurrencyConversionRate
        {
            get { return _CurrencyConversionRate; }
            set
            {
                _CurrencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
            }
        }

        //Converted
        double _ConvertedCashAdvanced;
        [DataMember]
        public double ConvertedCashAdvanced
        {
            get { return _ConvertedCashAdvanced; }
            set
            {
                _ConvertedCashAdvanced = value;
                OnPropertyChanged("ConvertedCashAdvanced");
            }
        }

        double _ConvertedTotalExpense;
        [DataMember]
        public double ConvertedTotalExpense
        {
            get { return _ConvertedTotalExpense; }
            set
            {
                _ConvertedTotalExpense = value;
                OnPropertyChanged("ConvertedTotalExpense");
            }
        }

        double _ConvertedTotalCash;
        [DataMember]
        public double ConvertedTotalCash
        {
            get { return _ConvertedTotalCash; }
            set
            {
                _ConvertedTotalCash = value;
                OnPropertyChanged("ConvertedTotalCash");
            }
        }

        double _ConvertedPersonalCredit;
        [DataMember]
        public double ConvertedPersonalCredit
        {
            get { return _ConvertedPersonalCredit; }
            set
            {
                _ConvertedPersonalCredit = value;
                OnPropertyChanged("ConvertedPersonalCredit");
            }
        }

        double _ConvertedBankTransfer;
        [DataMember]
        public double ConvertedBankTransfer
        {
            get { return _ConvertedBankTransfer; }
            set
            {
                _ConvertedBankTransfer = value;
                OnPropertyChanged("ConvertedBankTransfer");
            }
        }

        double _ConvertedCompanyCard;
        [DataMember]
        public double ConvertedCompanyCard
        {
            get { return _ConvertedCompanyCard; }
            set
            {
                _ConvertedCompanyCard = value;
                OnPropertyChanged("ConvertedCompanyCard");
            }
        }

        double _ConvertedCompanyvoucher;
        [DataMember]
        public double ConvertedCompanyvoucher
        {
            get { return _ConvertedCompanyvoucher; }
            set
            {
                _ConvertedCompanyvoucher = value;
                OnPropertyChanged("ConvertedCompanyvoucher");
            }
        }

        double _ConvertedPaidByOthers;
        [DataMember]
        public double ConvertedPaidByOthers
        {
            get { return _ConvertedPaidByOthers; }
            set
            {
                _ConvertedPaidByOthers = value;
                OnPropertyChanged("ConvertedPaidByOthers");
            }
        }


        //PlantCurrency
        double _PlantCurrencyCashAdvanced;
        [DataMember]
        public double PlantCurrencyCashAdvanced
        {
            get { return _PlantCurrencyCashAdvanced; }
            set
            {
                _PlantCurrencyCashAdvanced = value;
                OnPropertyChanged("PlantCurrencyCashAdvanced");
            }
        }

        double _PlantCurrencyTotalExpense;
        [DataMember]
        public double PlantCurrencyTotalExpense
        {
            get { return _PlantCurrencyTotalExpense; }
            set
            {
                _PlantCurrencyTotalExpense = value;
                OnPropertyChanged("PlantCurrencyTotalExpense");
            }
        }


        double _PlantCurrencyTotalCash;
        [DataMember]
        public double PlantCurrencyTotalCash
        {
            get { return _PlantCurrencyTotalCash; }
            set
            {
                _PlantCurrencyTotalCash = value;
                OnPropertyChanged("PlantCurrencyTotalCash");
            }
        }

        double _PlantCurrencyPersonalCredit;
        [DataMember]
        public double PlantCurrencyPersonalCredit
        {
            get { return _PlantCurrencyPersonalCredit; }
            set
            {
                _PlantCurrencyPersonalCredit = value;
                OnPropertyChanged("PlantCurrencyPersonalCredit");
            }
        }

        double _PlantCurrencyBankTransfer;
        [DataMember]
        public double PlantCurrencyBankTransfer
        {
            get { return _PlantCurrencyBankTransfer; }
            set
            {
                _PlantCurrencyBankTransfer = value;
                OnPropertyChanged("PlantCurrencyBankTransfer");
            }
        }

        double _PlantCurrencyCompanyCard;
        [DataMember]
        public double PlantCurrencyCompanyCard
        {
            get { return _PlantCurrencyCompanyCard; }
            set
            {
                _PlantCurrencyCompanyCard = value;
                OnPropertyChanged("PlantCurrencyCompanyCard");
            }
        }

        double _PlantCurrencyCompanyvoucher;
        [DataMember]
        public double PlantCurrencyCompanyvoucher
        {
            get { return _PlantCurrencyCompanyvoucher; }
            set
            {
                _PlantCurrencyCompanyvoucher = value;
                OnPropertyChanged("PlantCurrencyCompanyvoucher");
            }
        }

        double _PlantCurrencyPaidByOthers;
        [DataMember]
        public double PlantCurrencyPaidByOthers
        {
            get { return _PlantCurrencyPaidByOthers; }
            set
            {
                _PlantCurrencyPaidByOthers = value;
                OnPropertyChanged("PlantCurrencyPaidByOthers");
            }
        }
		//[nsatpute][09.10.2025][GEOS2-6367]
        string totalExpenseString;
        [DataMember]
        public string TotalExpenseString
        {
            get { return totalExpenseString; }
            set
            {
                totalExpenseString = value;
                OnPropertyChanged("TotalExpenseString");
            }
        }

        string totalCashString;
        [DataMember]
        public string TotalCashString
        {
            get { return totalCashString; }
            set
            {
                totalCashString = value;
                OnPropertyChanged("TotalCashString");
            }
        }

        string personalCreditString;
        [DataMember]
        public string PersonalCreditString
        {
            get { return personalCreditString; }
            set
            {
                personalCreditString = value;
                OnPropertyChanged("PersonalCreditString");
            }
        }

        string bankTransferString;
        [DataMember]
        public string BankTransferString
        {
            get { return bankTransferString; }
            set
            {
                bankTransferString = value;
                OnPropertyChanged("BankTransferString");
            }
        }

        string companyCardString;
        [DataMember]
        public string CompanyCardString
        {
            get { return companyCardString; }
            set
            {
                companyCardString = value;
                OnPropertyChanged("CompanyCardString");
            }
        }

        string companyVoucherString;
        [DataMember]
        public string CompanyVoucherString
        {
            get { return companyVoucherString; }
            set
            {
                companyVoucherString = value;
                OnPropertyChanged("CompanyVoucherString");
            }
        }

        string paidByOthersString;
        [DataMember]
        public string PaidByOthersString
        {
            get { return paidByOthersString; }
            set
            {
                paidByOthersString = value;
                OnPropertyChanged("PaidByOthersString");
            }
        }

        string currencyConversionRateString;
        [DataMember]
        public string CurrencyConversionRateString
        {
            get { return currencyConversionRateString; }
            set
            {
                currencyConversionRateString = value;
                OnPropertyChanged("CurrencyConversionRateString");
            }
        }

        //Converted
        string convertedCashAdvancedString;
        [DataMember]
        public string ConvertedCashAdvancedString
        {
            get { return convertedCashAdvancedString; }
            set
            {
                convertedCashAdvancedString = value;
                OnPropertyChanged("ConvertedCashAdvancedString");
            }
        }

        string convertedTotalExpenseString;
        [DataMember]
        public string ConvertedTotalExpenseString
        {
            get { return convertedTotalExpenseString; }
            set
            {
                convertedTotalExpenseString = value;
                OnPropertyChanged("ConvertedTotalExpenseString");
            }
        }

        string convertedTotalCashString;
        [DataMember]
        public string ConvertedTotalCashString
        {
            get { return convertedTotalCashString; }
            set
            {
                convertedTotalCashString = value;
                OnPropertyChanged("ConvertedTotalCashString");
            }
        }

        string convertedPersonalCreditString;
        [DataMember]
        public string ConvertedPersonalCreditString
        {
            get { return convertedPersonalCreditString; }
            set
            {
                convertedPersonalCreditString = value;
                OnPropertyChanged("ConvertedPersonalCreditString");
            }
        }

        string convertedBankTransferString;
        [DataMember]
        public string ConvertedBankTransferString
        {
            get { return convertedBankTransferString; }
            set
            {
                convertedBankTransferString = value;
                OnPropertyChanged("ConvertedBankTransferString");
            }
        }

        string convertedCompanyCardString;
        [DataMember]
        public string ConvertedCompanyCardString
        {
            get { return convertedCompanyCardString; }
            set
            {
                convertedCompanyCardString = value;
                OnPropertyChanged("ConvertedCompanyCardString");
            }
        }

        string convertedCompanyVoucherString;
        [DataMember]
        public string ConvertedCompanyVoucherString
        {
            get { return convertedCompanyVoucherString; }
            set
            {
                convertedCompanyVoucherString = value;
                OnPropertyChanged("ConvertedCompanyVoucherString");
            }
        }

        string convertedPaidByOthersString;
        [DataMember]
        public string ConvertedPaidByOthersString
        {
            get { return convertedPaidByOthersString; }
            set
            {
                convertedPaidByOthersString = value;
                OnPropertyChanged("ConvertedPaidByOthersString");
            }
        }

        //PlantCurrency
        string plantCurrencyCashAdvancedString;
        [DataMember]
        public string PlantCurrencyCashAdvancedString
        {
            get { return plantCurrencyCashAdvancedString; }
            set
            {
                plantCurrencyCashAdvancedString = value;
                OnPropertyChanged("PlantCurrencyCashAdvancedString");
            }
        }

        string plantCurrencyTotalExpenseString;
        [DataMember]
        public string PlantCurrencyTotalExpenseString
        {
            get { return plantCurrencyTotalExpenseString; }
            set
            {
                plantCurrencyTotalExpenseString = value;
                OnPropertyChanged("PlantCurrencyTotalExpenseString");
            }
        }

        string plantCurrencyTotalCashString;
        [DataMember]
        public string PlantCurrencyTotalCashString
        {
            get { return plantCurrencyTotalCashString; }
            set
            {
                plantCurrencyTotalCashString = value;
                OnPropertyChanged("PlantCurrencyTotalCashString");
            }
        }

        string plantCurrencyPersonalCreditString;
        [DataMember]
        public string PlantCurrencyPersonalCreditString
        {
            get { return plantCurrencyPersonalCreditString; }
            set
            {
                plantCurrencyPersonalCreditString = value;
                OnPropertyChanged("PlantCurrencyPersonalCreditString");
            }
        }

        string plantCurrencyBankTransferString;
        [DataMember]
        public string PlantCurrencyBankTransferString
        {
            get { return plantCurrencyBankTransferString; }
            set
            {
                plantCurrencyBankTransferString = value;
                OnPropertyChanged("PlantCurrencyBankTransferString");
            }
        }

        string plantCurrencyCompanyCardString;
        [DataMember]
        public string PlantCurrencyCompanyCardString
        {
            get { return plantCurrencyCompanyCardString; }
            set
            {
                plantCurrencyCompanyCardString = value;
                OnPropertyChanged("PlantCurrencyCompanyCardString");
            }
        }

        string plantCurrencyCompanyVoucherString;
        [DataMember]
        public string PlantCurrencyCompanyVoucherString
        {
            get { return plantCurrencyCompanyVoucherString; }
            set
            {
                plantCurrencyCompanyVoucherString = value;
                OnPropertyChanged("PlantCurrencyCompanyVoucherString");
            }
        }

        string plantCurrencyPaidByOthersString;
        [DataMember]
        public string PlantCurrencyPaidByOthersString
        {
            get { return plantCurrencyPaidByOthersString; }
            set
            {
                plantCurrencyPaidByOthersString = value;
                OnPropertyChanged("PlantCurrencyPaidByOthersString");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    [DataContract]
    public class TripExpensesReportMealBudget : ModelBase, IDisposable
    {
        Int64 _IdEmployeeExpenseReport;
        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return _IdEmployeeExpenseReport; }
            set
            {
                _IdEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }

        Int64 _IdCompany;
        [DataMember]
        public Int64 IdCompany
        {
            get { return _IdCompany; }
            set
            {
                _IdCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        Int32 _IdCurrency;
        [DataMember]
        public Int32 IdCurrency
        {
            get { return _IdCurrency; }
            set
            {
                _IdCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        double _Amount;
        [DataMember]
        public double Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
            }
        }

        Int32 _IdEmployeeProfile;
        [DataMember]
        public Int32 IdEmployeeProfile
        {
            get { return _IdEmployeeProfile; }
            set
            {
                _IdEmployeeProfile = value;
                OnPropertyChanged("IdEmployeeProfile");
            }
        }

        string _Value;
        [DataMember]
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        string _jobTitle;
        [DataMember]
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                _jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }

        bool _DailyAmountMealbudget;
        [DataMember]
        public bool DailyAmountMealbudget
        {
            get { return _DailyAmountMealbudget; }
            set
            {
                _DailyAmountMealbudget = value;
                OnPropertyChanged("DailyAmountMealbudget");
            }
        }

        double _MealExpense;
        [DataMember]
        public double MealExpense
        {
            get { return _MealExpense; }
            set
            {
                _MealExpense = value;
                OnPropertyChanged("MealExpense");
            }
        }

        string _TripWeek;
        [DataMember]
        public string TripWeek
        {
            get { return _TripWeek; }
            set
            {
                _TripWeek = value;
                OnPropertyChanged("TripWeek");
            }
        }

        int _TripDays;
        [DataMember]
        public int TripDays
        {
            get { return _TripDays; }
            set
            {
                _TripDays = value;
                OnPropertyChanged("TripDays");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    [DataContract]
    public class TripTravelExpenseReport : ModelBase, IDisposable
    {
        #region Fileds
        UInt32 _idEmployeeExpense;
        string _ticketDate;
        UInt32 _idType;
        string _type;
        string _ticketSummary;
        string _ticketAmount;
        string _tip;
        Int32 _idCurrency;
        string _ticketCurrency;
        UInt32 _idPayment;
        string _paymentMethod;
        string _isFileIncluded;
        Int32 _idCompany;
        string _reportedPlant;
        string _totalTicketAmount;
        string _attendeeCount;
        string _totalCountedAmount;
        string _conversionRate;
        string _totalTicketAmountCurrencyPlant;
        string _totalCountedAmountCurrencyPlant;

        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public UInt32 IdEmployeeExpense
        {
            get { return _idEmployeeExpense; }
            set
            {
                _idEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }

        [NotMapped]
        [DataMember]
        public string TicketDate
        {
            get { return _ticketDate; }
            set
            {
                _ticketDate = value;
                OnPropertyChanged("TicketDate");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdType
        {
            get { return _idType; }
            set
            {
                _idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [NotMapped]
        [DataMember]
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }


        [NotMapped]
        [DataMember]
        public string TicketSummary
        {
            get { return _ticketSummary; }
            set
            {
                _ticketSummary = value;
                OnPropertyChanged("TicketSummary");
            }
        }

        [NotMapped]
        [DataMember]
        public string TicketAmount
        {
            get { return _ticketAmount; }
            set
            {
                _ticketAmount = value;
                OnPropertyChanged("TicketAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public string Tip
        {
            get { return _tip; }
            set
            {
                _tip = value;
                OnPropertyChanged("Tip");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdCurrency
        {
            get { return _idCurrency; }
            set
            {
                _idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public string TicketCurrency
        {
            get { return _ticketCurrency; }
            set
            {
                _ticketCurrency = value;
                OnPropertyChanged("TicketCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdPayment
        {
            get { return _idPayment; }
            set
            {
                _idPayment = value;
                OnPropertyChanged("IdPayment");
            }
        }

        [NotMapped]
        [DataMember]
        public string PaymentMethod
        {
            get { return _paymentMethod; }
            set
            {
                _paymentMethod = value;
                OnPropertyChanged("PaymentMethod");
            }
        }

        [NotMapped]
        [DataMember]
        public string IsFileIncluded
        {
            get { return _isFileIncluded; }
            set
            {
                _isFileIncluded = value;
                OnPropertyChanged("IsFileIncluded");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return _idCompany; }
            set
            {
                _idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReportedPlant
        {
            get { return _reportedPlant; }
            set
            {
                _reportedPlant = value;
                OnPropertyChanged("ReportedPlant");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalTicketAmount
        {
            get { return _totalTicketAmount; }
            set
            {
                _totalTicketAmount = value;
                OnPropertyChanged("TotalTicketAmount");
            }
        }


        [NotMapped]
        [DataMember]
        public string AttendeeCount
        {
            get { return _attendeeCount; }
            set
            {
                _attendeeCount = value;
                OnPropertyChanged("AttendeeCount");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalCountedAmount
        {
            get { return _totalCountedAmount; }
            set
            {
                _totalCountedAmount = value;
                OnPropertyChanged("TotalCountedAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConversionRate
        {
            get { return _conversionRate; }
            set
            {
                _conversionRate = value;
                OnPropertyChanged("ConversionRate");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalTicketAmountCurrencyPlant
        {
            get { return _totalTicketAmountCurrencyPlant; }
            set
            {
                _totalTicketAmountCurrencyPlant = value;
                OnPropertyChanged("TotalTicketAmountCurrencyPlant");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalCountedAmountCurrencyPlant
        {
            get { return _totalCountedAmountCurrencyPlant; }
            set
            {
                _totalCountedAmountCurrencyPlant = value;
                OnPropertyChanged("TotalCountedAmountCurrencyPlant");
            }
        }

        #endregion

        #region Constructor
        public TripTravelExpenseReport()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    [DataContract]
    public class TripEmployeeExpenseRemark : ModelBase, IDisposable
    {

        Int64 _IdEmployeeExpenseReport;
        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return _IdEmployeeExpenseReport; }
            set
            {
                _IdEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }

        private UInt32 _IdEmployeeExpense;
        [DataMember]
        public UInt32 IdEmployeeExpense
        {
            get { return _IdEmployeeExpense; }
            set
            {
                _IdEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }

        private string _TicketDate;
        [DataMember]
        public string TicketDate
        {
            get { return _TicketDate; }
            set
            {
                _TicketDate = value;
                OnPropertyChanged("TicketDate");
            }
        }

        private string _Type;
        [DataMember]
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }

        private string _Remarks;
        [DataMember]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
	// [nsatpute][10-12-2024][GEOS2-6367]
    [DataContract]
    public class TripExpenseReportRemark : ModelBase, IDisposable
    {
        private string reportCode;
        [DataMember]
        public string ReportCode
        {
            get { return reportCode; }
            set
            {
                reportCode = value;
                OnPropertyChanged("ReportCode");
            }
        }

        private string remark;
        [DataMember]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}
