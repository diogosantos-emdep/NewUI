using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Emdep.Geos.Data.Common.Epc;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
   public class TravelExpenses : ModelBase, IDisposable
    {
        #region Fields
        double totalAmountForBal;
        Int32 idEmployeeExpenseReport;
        string expenseCode;
        string expenseTitle;
        string expenseCurrency;
        DateTime startDate;
        DateTime endDate;
        int duration;
        string reporter;
        Int64 totalCount;
        double totalAmount;
        double givenAmount;
        double balance;
        Int32 idEmployee;
        string curSymbol;
        string iso;
        byte[] countryIconbytes;
        bool isNegative;
        string status;
        string remarks;
        byte idWorkflowStatus;
        //List<LogEntriesByTravelExpense> comments;
        List<LogEntriesByTravelExpense> logEntries;
        LogEntriesByTravelExpense logEntry;
        Int32 idCompany;
        string company;
        string htmlColor;
        LookupValue reason;
        int idCurrency;
        string reporterCode;
        int idCurrencyFrom;
        int idCurrencyTo;
        string linkedTripTitle; //[pramod.misal][GEOS2-4848][23-11-2023]     
        int? idLinkedTrip =null; //[pramod.misal][GEOS2-4848][23-11-2023]    
        DateTime? linkedFromDate; //[pramod.misal][GEOS2-4848][29-11-2023]    
        DateTime? linkedToDate; //[pramod.misal][GEOS2-4848][29-11-2023]  
        List<LogEntriesByTravelExpense> comments;   //[chitra.girigosavi][GEOS2-4824][07.11.2023]
        string linkedName;

        
        #endregion

        #region Constructor
        public TravelExpenses()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("ExpenseCode")]
        [DataMember]
        public string ExpenseCode
        {
            get
            {
                return expenseCode;
            }

            set
            {
                expenseCode = value;
                OnPropertyChanged("ExpenseCode");
            }
        }

        [Column("ExpenseTitle")]
        [DataMember]
        public string ExpenseTitle
        {
            get
            {
                return expenseTitle;
            }

            set
            {
                expenseTitle = value;
                OnPropertyChanged("ExpenseTitle");
            }
        }

        [Column("ExpenseCurrency")]
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

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("Duration")]
        [DataMember]
        public int Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        [Column("Reporter")]
        [DataMember]
        public string Reporter
        {
            get
            {
                return reporter;
            }

            set
            {
                reporter = value;
                OnPropertyChanged("Reporter");
            }
        }

        [Column("TotalCount")]
        [DataMember]
        public Int64 TotalCount
        {
            get
            {
                return totalCount;
            }

            set
            {
                totalCount = value;
                OnPropertyChanged("TotalCount");
            }
        }

        [Column("TotalAmount")]
        [DataMember]
        public double TotalAmount
        {
            get
            {
                return totalAmount;
            }

            set
            {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        [Column("GivenAmount")]
        [DataMember]
        public double GivenAmount
        {
            get
            {
                return givenAmount;
            }

            set
            {
                givenAmount = value;
                OnPropertyChanged("GivenAmount");
            }
        }

        [Column("Balance")]
        [DataMember]
        public double Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
                OnPropertyChanged("Balance");
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
        public Int32 IdEmployee
        {
            get
            {
                return idEmployee;
            }

            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
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
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsNegative
        {
            get { return isNegative; }
            set
            {
                isNegative = value;
                OnPropertyChanged("IsNegative");
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
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [DataMember]
        public byte IdWorkflowStatus
        {
            get
            {
                return idWorkflowStatus;
            }

            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }

        [DataMember]
        public Int32 IdCompany
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
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public List<LogEntriesByTravelExpense> Comments
        //{
        //    get
        //    {
        //        return comments;
        //    }

        //    set
        //    {
        //        comments = value;
        //        OnPropertyChanged("Comments");
        //    }
        //}

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByTravelExpense> LogEntries
        {
            get
            {
                return logEntries;
            }

            set
            {
                logEntries = value;
                OnPropertyChanged("LogEntries");
            }
        }

        [DataMember]
        public LogEntriesByTravelExpense LogEntry
        {
            get
            {
                return logEntry;
            }

            set
            {
                logEntry = value;
                OnPropertyChanged("LogEntry");
            }
        }
        //[rdixit][GEOS2-4022][24.11.2022]

        [DataMember]
        public LookupValue Reason
        {
            get
            {
                return reason;
            }

            set
            {
                reason = value;
                OnPropertyChanged("Reason");
            }
        }

        //[rdixit][GEOS2-4025][28.11.2022]
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
        public double TotalAmountForBal
        {
            get
            {
                return totalAmountForBal;
            }

            set
            {
                totalAmountForBal = value;
                OnPropertyChanged("TotalAmountForBal");
            }
        }

        [DataMember]
        public string ReporterCode
        {
            get
            {
                return reporterCode;
            }

            set
            {
                reporterCode = value;
                OnPropertyChanged("ReporterCode");
            }
        }

        [DataMember]
        public int IdCurrencyFrom
        {
            get
            {
                return idCurrencyFrom;
            }

            set
            {
                idCurrencyFrom = value;
                OnPropertyChanged("IdCurrencyFrom");
            }
        }

        [DataMember]
        public int IdCurrencyTo
        {
            get
            {
                return idCurrencyTo;
            }

            set
            {
                idCurrencyTo = value;
                OnPropertyChanged("IdCurrencyTo");
            }
        }

        //[pramod.misal][GEOS2-4848][23-11-2023]     
        [Column("LinkedTripTitle")]
        [DataMember]
        public string LinkedTripTitle
        {
            get
            {
                return linkedTripTitle;
            }

            set
            {
                linkedTripTitle = value;
                OnPropertyChanged("LinkedTripTitle");
            }
        }

        //[pramod.misal][GEOS2-4848][23-11-2023]     
        [DataMember]
        public int? IdLinkedTrip
        {
            get
            {
                return idLinkedTrip;
            }

            set
            {
                idLinkedTrip = value;
                OnPropertyChanged("IdLinkedTrip");
            }
        }

        //[pramod.misal][GEOS2-4848][29-11-2023]     
        [Column("LinkedFromDate")]
        [DataMember]
        public DateTime? LinkedFromDate
        {
            get
            {
                return linkedFromDate;
            }

            set
            {
                linkedFromDate = value;
                OnPropertyChanged("LinkedFromDate");
            }
        }


        //[pramod.misal][GEOS2-4848][29-11-2023]     
        [Column("LinkedToDate")]
        [DataMember]
        public DateTime? LinkedToDate
        {
            get
            {
                return linkedToDate;
            }

            set
            {
                linkedToDate = value;
                OnPropertyChanged("LinkedToDate");
            }
        }

        //[pramod.misal][GEOS2-4848][29-11-2023]     
        [Column("LinkedName")]
        [DataMember]
        public string LinkedName
        {
            get
            {
                return linkedName;
            }

            set
            {
                linkedName = value;
                OnPropertyChanged("LinkedName");
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

        [NotMapped]
        [DataMember]
        public List<LogEntriesByTravelExpense> Comments //[chitra.girigosavi][GEOS2-4824][07.11.2023]
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
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

        public override string ToString()
        {
            return LinkedTripTitle;
        }

        #endregion
    }
}
