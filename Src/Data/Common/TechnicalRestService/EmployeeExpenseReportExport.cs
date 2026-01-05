using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class EmployeeExpenseReportExport
    {
        [DataMember(Order = 1)]
        public string IdExpenseReport { get; set; }

        [DataMember(Order = 2)]
        public string ParameterPlantOwner { get; set; }
        [DataMember(Order = 3)]
        public string Lang { get; set; }

        [DataMember(Order = 4)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 5)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 6)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 7)]
        public string login { get; set; }

        [DataMember(Order = 8)]
        public string Signatory { get; set; }
        [DataMember(Order = 9)]
        public string ExpensesFilePath { get; set; }

        [DataMember(Order = 10)]
        public CreateExpenseReportsSign ParameterSign { get; set; }

        [DataMember(Order = 11)]
        public string ExpensesTemplatePath { get; set; }

        [DataMember(Order = 12)]
        public string EmployeesExpensesAttachmentPath { get; set; }
        [DataMember(Order = 13)]
        public string CurrencyLayerAPI { get; set; }

        [DataMember(Order = 13)]
        public string ParameterMailServerName { get; set; }
        [DataMember(Order = 14)]
        public string ParameterMailServerPort { get; set; }
        [DataMember(Order = 15)]
        public string ParameterMailFrom { get; set; }
        [DataMember(Order = 16)]
        public string ParameterMailpassword { get; set; }
        [DataMember(Order = 17)]
        public string ParameterMailUser { get; set; }
        [DataMember(Order = 18)]
        public SendMailExpenses SendMailModel { get; set; }

        [DataMember(Order = 19)]
        public string FilePath { get; set; }

    }

    public class ExportExpenses
    {
        #region Fields
        public string _IdEmployeeExpenseReport = string.Empty;
        public string _ExpenseReportCode = string.Empty;

        public string _SubmitDate = string.Empty; //[plahange][APIGEOS-734][02/03/2023]
        public string _ReportDate = string.Empty;
        public string _ExpensesFromDate = string.Empty;
        public string _ExpensesToDate = string.Empty;
        public string _EmployeeName = string.Empty;
        public string _EmployeeDepartment = string.Empty;
        public string _EmployeeCode = string.Empty;
        public string _EmployeeJobTitleCode = string.Empty;
        public string _EmployeeJobTitle = string.Empty;
        public string _ReportPurpose = string.Empty;
        public string _ReportReason = string.Empty;
        public Double _TotalCompanyCreditCardAmount = 0;//[Sudhir.Jangra][APIGEOS-673][Added TotalCompanyCreditCardAmount]
        public Double _TotalCompanyVoucherAmount = 0;//[plahange][APIGEOS-675]
        public string _JobTitleOrganization = string.Empty;
        public string _PlantRegisteredName = string.Empty;
        public string _PlantRegistrationNumber = string.Empty;

        public string _ItemsStartRow = string.Empty;
        public string _ItemItemColumn = string.Empty;
        public string _ItemWeekColumn = string.Empty;
        public string _ItemDateColumn = string.Empty;
        public string _ItemDayColumn = string.Empty;
        public string _ItemSummaryColumn = string.Empty;
        public string _ItemCategory = string.Empty;
        public string _ItemPaymentMethodColumn = string.Empty;
        public string _ItemCost = string.Empty;

        public Int32 _IdCurrency = 0;
        public string _Currency = string.Empty;
        public string _Remarks = string.Empty;
        public string _Advances = string.Empty;
        public string Language = string.Empty;

        //public string   _ReportCode                 = string.Empty;
        public string _PlantAlias = string.Empty;//[001][kshinde][30/03/2022]

        public List<List_OF_Expenses> _ExportExpensesLst = null;
        public List<AccountingExpensesAttendeesPerson> _ExportExpensesAttendees = null;
        public List<ExportExpensesattachments> _ExportExpensesAttachments = null;
        #endregion
    }
    public class List_OF_Expenses
    {
        public Int64 _IdEmployeeExpenseAttendee = 0;
        public Int64 _IdEmployeeExpenseReport = 0;
        public Int64 _IdEmployeeExpense = 0;

        public string _ItemColumn = string.Empty;
        public string _WeekColumn = string.Empty;
        public string _DateColumn = string.Empty;
        public string _DayColumn = string.Empty;
        public string _SummaryColumn = string.Empty;
        public Int32 _IdCategory = 0;
        public string _Category = string.Empty;
        public string _PaymentMethodColumn = string.Empty;
        public Double _Cost = 0;
        public List<ExportExpensesAttendees> _ExportExpensesAttendees = null;
        [DataMember]
        public double _Tip = 0;
        public List<ExportExpensesattachments> _ExportExpensesAttachments = null;
        public int _columnNum = 0;
        public string _attachmentNumber = string.Empty;
    }
    public class ExportExpensesAttendees
    {
        private int _IdEmployeeExpenseAttendee = 0;
        [DataMember(Order = 1)]
        public int IdEmployeeExpenseAttendee
        {
            get { return _IdEmployeeExpenseAttendee; }
            set { _IdEmployeeExpenseAttendee = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 2)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _styleName = string.Empty;
        [DataMember(Order = 3)]
        public string styleName
        {
            get { return _styleName; }
            set { _styleName = value; }
        }

        private string _stylesymbol = string.Empty;
        [DataMember(Order = 4)]
        public string stylesymbol
        {
            get { return _stylesymbol; }
            set { _stylesymbol = value; }
        }
    }

    public class ExportExpensesattachments
    {
        private Int64 _IdEmployeeExpenseAttachment = 0;
        [DataMember(Order = 1)]
        public Int64 IdEmployeeExpenseAttachment
        {
            get { return _IdEmployeeExpenseAttachment; }
            set { _IdEmployeeExpenseAttachment = value; }
        }

        private Int64 _IdEmployeeExpense = 0;
        [DataMember(Order = 2)]
        public Int64 IdEmployeeExpense
        {
            get { return _IdEmployeeExpense; }
            set { _IdEmployeeExpense = value; }
        }

        private string _OriginalFileName = string.Empty;
        [DataMember(Order = 3)]
        public string OriginalFileName
        {
            get { return _OriginalFileName; }
            set { _OriginalFileName = value; }
        }

        private string _SavedFileName = string.Empty;
        [DataMember(Order = 4)]
        public string SavedFileName
        {
            get { return _SavedFileName; }
            set { _SavedFileName = value; }
        }

        private string _FileType = string.Empty;
        [DataMember(Order = 5)]
        public string FileType
        {
            get { return _FileType; }
            set { _FileType = value; }
        }

        private string _FileSize = string.Empty;
        [DataMember(Order = 6)]
        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }

        private Int32 _IsDeleted = 0;
        [DataMember(Order = 7)]
        public Int32 IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        [IgnoreDataMember]
        public string _attachmentNumber = string.Empty;
    }
    public class AccountingExpensesAttendeesPerson
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _FirstName = string.Empty;
        [DataMember(Order = 2)]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _lastName = string.Empty;
        [DataMember(Order = 3)]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        private string _Photo = string.Empty;
        [DataMember(Order = 4)]
        public string Photo
        {
            get { return _Photo; }
            set { _Photo = value; }
        }
        [DataMember(Order = 5)]
        public AttendeesGender Gender { get; set; }

        [DataMember(Order = 6)]
        public AttendeesPersonCompany Company { get; set; }
    }
    public class AttendeesGender
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 2)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }

    public class AttendeesPersonCompany
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 2)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [DataMember(Order = 3)]
        public AttendeesPersonCompanyCountry Country { get; set; }
    }
    public class AttendeesPersonCompanyCountry
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 2)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Iso2Code = string.Empty;
        [DataMember(Order = 3)]
        public string Iso2Code
        {
            get { return _Iso2Code; }
            set { _Iso2Code = value; }
        }
    }

    public class CurrencyExchangeapilayer
    {
        [Display(Order = 1)]
        public bool success { get; set; }
        [Display(Order = 2)]
        public string terms { get; set; }
        [Display(Order = 3)]
        public string privacy { get; set; }
        [Display(Order = 4)]
        public string timestamp { get; set; }
        [Display(Order = 6)]
        public string source { get; set; }
        public object quotes { get; set; }

    }

    public class CurrencyExchangeapilayerquotes
    {
        public double EUR { get; set; }
        public double RON { get; set; }
        public double USD { get; set; }
        public double MXN { get; set; }
        public double CNY { get; set; }
        public double HNL { get; set; }
        public double BRL { get; set; }
        public double TND { get; set; }
        public double MAD { get; set; }
        public double PYG { get; set; }
        public double RUB { get; set; }
        public double INR { get; set; }
        public double GBP { get; set; }
        public double CAD { get; set; }
        public double CHF { get; set; }
    }

    public class SendMailExpenses
    {
        [Display(Order = 1)]
        public string Source { get; set; }

        [Display(Order = 2)]
        public string Subject { get; set; }

        [Display(Order = 3)]
        public string TO { get; set; }

        [Display(Order = 4)]
        public string CC { get; set; }

        [Display(Order = 5)]
        public string Body { get; set; }

        [Display(Order = 6)]
        public bool IsHtmlBody { get; set; }
    }
}
