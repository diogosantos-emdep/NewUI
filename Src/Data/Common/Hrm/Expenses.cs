using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class Expenses : ModelBase, IDisposable
    {

        #region Fields
        int pageNo;
        double avgmealExpense;
        int idEmployeeExpense;
        int idExpenseReporter;
        Int32 idCountry;
        string countryName;
        string city;
        Int32 idCreator;
        Int32 idEmployeeExpenseReport;
        string linkedExpenseCode;
        string linkedExpensTitle;
        string expenseCurrency;
        double amtcal;
        double tipcal;
        string amount;
        string curSymbol;
        string iso2Code;
        int idExpenseType;
        string expenseType;
        int idCategory;
        string categoryName;
        string summary;
        string tip;
        int idCurrency;
        Int32 idPaymentMethod;
        string paymentMethod;
        DateTime expenseDate;
        DateTime creationDate;
        int idStatus;
        string status;
        bool isAttachmentExist = false;
        string expenseTime;
        private Visibility isAttachmentExistVisibility;
        Int32? expposition;
        List<ExpenseAttendees> expenseAttendees;
        int existAsAttendee =0;
        double avgExpense;
        float exchangeRate;
        string displayExchangeRate;
        float originalConversionRate;
        float startDateExchangeRate;
        
        #endregion

        #region Constructor
        public Expenses()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdEmployeeExpense")]
        [DataMember]
        public Int32 IdEmployeeExpense
        {
            get
            {
                return idEmployeeExpense;
            }

            set
            {
                idEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }

        [DataMember]
        public Visibility IsAttachmentExistVisibility
        {
            get
            {
                return isAttachmentExistVisibility;
            }

            set
            {
                isAttachmentExistVisibility = value;
                OnPropertyChanged("IsAttachmentExistVisibility");
            }
        }

        [DataMember]
        public Int32 IdEmployeeExpenseReport
        {
            get
            {
                return idEmployeeExpenseReport;
            }

            set
            {
                idEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }

        [DataMember]
        public string LinkedExpenseCode
        {
            get
            {
                return linkedExpenseCode;
            }

            set
            {
                linkedExpenseCode = value;
                OnPropertyChanged("LinkedExpenseCode");
            }
        }

        [DataMember]
        public string LinkedExpensTitle
        {
            get
            {
                return linkedExpensTitle;
            }

            set
            {
                linkedExpensTitle = value;
                OnPropertyChanged("LinkedExpensTitle");
            }
        }

        [DataMember]
        public int IdExpenseType
        {
            get
            {
                return idExpenseType;
            }

            set
            {
                idExpenseType = value;
                OnPropertyChanged("IdExpenseType");
            }
        }

        [DataMember]
        public string ExpenseType
        {
            get
            {
                return expenseType;
            }

            set
            {
                expenseType = value;
                OnPropertyChanged("ExpenseType");
            }
        }

        [DataMember]
        public double AmtCal
        {
            get
            {
                return amtcal;
            }

            set
            {
                amtcal = value;
                OnPropertyChanged("AmtCal");
            }
        }
        [DataMember]
        public double Tipcal
        {
            get
            {
                return tipcal;
            }

            set
            {
                tipcal = value;
                OnPropertyChanged("Tipcal");
            }
        }

        [DataMember]
        public int IdCategory
        {
            get
            {
                return idCategory;
            }

            set
            {
                idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        [DataMember]
        public string CategoryName
        {
            get
            {
                return categoryName;
            }

            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        [DataMember]
        public string Summary
        {
            get
            {
                return summary;
            }

            set
            {
                summary = value;
                OnPropertyChanged("Summary");
            }
        }

        [DataMember]
        public string Amount
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
        public string Tip
        {
            get
            {
                return tip;
            }

            set
            {
                tip = value;
                OnPropertyChanged("Tip");
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
        public string ExpenseCurrency
        {
            get
            {
                return expenseCurrency;
            }

            set
            {
                expenseCurrency = value;
                OnPropertyChanged("ExpenseCurrency");
            }
        }

        [DataMember]
        public string CurSymbol
        {
            get
            {
                return curSymbol;
            }

            set
            {
                curSymbol = value;
                OnPropertyChanged("CurSymbol");
            }
        }

        [DataMember]
        public Int32 IdPaymentMethod
        {
            get
            {
                return idPaymentMethod;
            }

            set
            {
                idPaymentMethod = value;
                OnPropertyChanged("IdPaymentMethod");
            }
        }

        [DataMember]
        public string PaymentMethod
        {
            get { return paymentMethod; }
            set
            {
                paymentMethod = value;
                OnPropertyChanged("PaymentMethod");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public Int32 IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        [DataMember]
        public Int32 IdCountry
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

        [DataMember]
        public string CountryName
        {
            get { return countryName; }
            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        [DataMember]
        public string Iso2Code
        {
            get
            {
                return iso2Code;
            }

            set
            {
                iso2Code = value;
                OnPropertyChanged("Iso2Code");
            }
        }

        [DataMember]
        public string ExpenseTime
        {
            get
            {
                return expenseTime;
            }

            set
            {
                expenseTime = value;
                OnPropertyChanged("ExpenseTime");
            }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public int IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public DateTime ExpenseDate
        {
            get
            {
                return expenseDate;
            }

            set
            {
                expenseDate = value;
                OnPropertyChanged("ExpenseDate");
            }
        }

        [DataMember]
        public bool IsAttachmentExist
        {
            get
            {
                return isAttachmentExist;
            }

            set
            {
                isAttachmentExist = value;
                OnPropertyChanged("IsAttachmentExist");
            }
        }

        [DataMember]
        public Int32? ExpPosition
        {
            get { return expposition; }
            set
            {
                expposition = value;
                OnPropertyChanged("ExpPosition");
            }
        }

        [DataMember]
        public List<ExpenseAttendees> ExpenseAttendees
        {
            get { return expenseAttendees; }
            set
            {
                expenseAttendees = value;
                OnPropertyChanged("ExpenseAttendees");
            }
        }
        int attendiesCount;
        [DataMember]
        public int AttendiesCount
        {
            get
            {
                return attendiesCount;
            }

            set
            {
                attendiesCount = value;
                OnPropertyChanged("AttendiesCount");
            }
        }
        string totalAttendiesName;
        [DataMember]
        public string TotalAttendiesName
        {
            get
            {
                return totalAttendiesName;
            }

            set
            {
                totalAttendiesName = value;
                OnPropertyChanged("TotalAttendiesName");
            }
        }        
        [DataMember]
        public double AvgmealExpense
        {
            get
            {
                return avgmealExpense;
            }

            set
            {
                avgmealExpense = value;
                OnPropertyChanged("AvgmealExpense");
            }
        }
        bool isNotExistAsAttendee;
        [DataMember]
        public bool IsNotExistAsAttendee
        {
            get
            {
                return isNotExistAsAttendee;
            }

            set
            {
                isNotExistAsAttendee = value;
                OnPropertyChanged("IsNotExistAsAttendee");
            }
        }
        [DataMember]
        public int ExistAsAttendee
        {
            get
            {
                return existAsAttendee;
            }

            set
            {
                existAsAttendee = value;
                OnPropertyChanged("ExistAsAttendee");
            }
        }
        [DataMember]
        public int IdExpenseReporter
        {
            get
            {
                return idExpenseReporter;
            }

            set
            {
                idExpenseReporter = value;
                OnPropertyChanged("IdExpenseReporter");
            }
        }

        [DataMember]
        public double AvgExpense
        {
            get
            {
                return avgExpense;
            }

            set
            {
                avgExpense = value;
                OnPropertyChanged("AvgExpense");
            }
        }

        [DataMember]
        public float ExchangeRate
        {
            get
            {
                return exchangeRate;
            }

            set
            {
                exchangeRate = value;
                OnPropertyChanged("ExchangeRate");
            }
        }

        [DataMember]
        public float OriginalConversionRate
        {
            get
            {
                return originalConversionRate;
            }

            set
            {
                originalConversionRate = value;
                OnPropertyChanged("OriginalConversionRate");
            }
        }
        [DataMember]
        public string DisplayExchangeRate
        {
            get
            {
                return displayExchangeRate;
            }

            set
            {
                displayExchangeRate = value;
                OnPropertyChanged("DisplayExchangeRate");
            }
        }

        [DataMember]
        public float StartDateExchangeRate
        {
            get
            {
                return startDateExchangeRate;
            }

            set
            {
                startDateExchangeRate = value;
                OnPropertyChanged("StartDateExchangeRate");
            }
        }
        [DataMember]
        public int PageNo
        {
            get
            {
                return pageNo;
            }

            set
            {
                pageNo = value;
                OnPropertyChanged("PageNo");
            }
        }

        //[rushikesh.gaikwad] [GEOS2-5555][23.05.2024]
        private string remarks;
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
                IsImageVisible = !string.IsNullOrEmpty(value);
            }
        }

        private bool _isImageVisible;

        public bool IsImageVisible
        {
            get { return _isImageVisible; }
            set
            {
                if (_isImageVisible != value)
                {
                    _isImageVisible = value;
                    OnPropertyChanged(nameof(IsImageVisible));
                }
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
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
