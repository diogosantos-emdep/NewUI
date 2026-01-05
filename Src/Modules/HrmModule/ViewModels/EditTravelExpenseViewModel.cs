using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.ComponentModel;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.LayoutControl;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.Hrm;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.Windows.Controls;
using System.Windows.Documents;
using Emdep.Geos.Utility;
using System.Net.Mail;
using DevExpress.Data.Filtering;
using iTextSharp.text.pdf;
using System.IO;
using DevExpress.Pdf;
using DevExpress.XtraPrinting.Export.Pdf;
using Emdep.Geos.Modules.Hrm.Reports;
using SysDrawingImage = System.Drawing.Image;
using DevExpress.XtraRichEdit;
using iTextSharp.text;
using DevExpress.XtraRichEdit.API.Native;
using System.Net;
using DevExpress.XtraRichEdit.Model;
using DevExpress.Mvvm.Native;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EditTravelExpenseViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {

        #region TaskLog
        /// <summary>
        /// [rdixit][GEOS2-3828][01.09.2022]
        /// </summary>
        #endregion

        #region Services      
        private INavigationService Service { get { return GetService<INavigationService>(); } }
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services

        #region Fields
        ObservableCollection<EmployeeTrips> linkedTripList1;
        List<string> linkedTripNamesList;
        string selectedTripText;
        /// <summary>
        /// 
        /// </summary>
        ObservableCollection<Expenses> employeeExpenseForWeeklyGrid;
        GeosAppSetting exchangeRateDeviation; int exchangeRateDeviationValue;
        Currency currFrom; Currency currTo; Currency originalCurrTo;
        bool isCurrExchangeColVisible;
        bool isCurrExchangeColReadOnly;
        string expenseColumnName = string.Empty;
        string expenseReportComment;
        EmployeeMealBudget mealExpenceAllowance;
        int selectedEmployeeIndex;
        int selectedCurrencyIndex;
        int selectedCompanyIndex;
        Employee selectedEmployee;
        ObservableCollection<Employee> employeesList;
        ObservableCollection<Employee> addemployeesList;
        Visibility isEmployeeSelectVisible;
        Visibility isEmployeeTextBlockVisible;
        private string error = string.Empty;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        bool isNew;
        bool isActive;
        bool isReadOnly = true;
        bool isEnable;
        bool isAcceptEnable;
        string windowHeader;
        bool isSaved;
        CultureInfo selectedCulture;
        int expenseCount;
        Company selectedcompanyList;
        private TravelExpenseStatus travelExpenseStatus;
        private List<TravelExpenseStatus> travelExpenseStatusList;
        private List<TravelExpenseStatus> travelExpenseStatusButtons;
        private TravelExpenseStatus selectedTravelExpenseStatusButton;
        private List<TravelExpenseWorkflowTransitions> workflowTransitionList;
        private TravelExpenseWorkflowTransitions workflowTransition;
        private double dialogHeight;
        private double dialogWidth;
        private MaximizedElementPosition maximizedElementPosition;
        string code;
        string title;
        int changeLogCount;
        DateTime startDate;
        DateTime endDate;
        int duration;
        double balance;
        string status;
        Currency selectedCurrency;
        ObservableCollection<Currency> currencyList;
        ObservableCollection<Expenses> employeeExpenseByExpenseReport;
        ObservableCollection<Company> companyList;
        string emplyeeName;
        Company selectedCompany;
        string remark;
        string balanceWithCurSymbol;
        string givenAmountWithCurSymbol;
        bool isBalanceNegative;
        TravelExpenses travelExpenseReport;
        bool isUpdated;
        string subTotal;
        string tip;
        string total;
        ObservableCollection<WeeklyTravelExpenses> weeklyExpensesList;
        WeeklyTravelExpenses selectedWeeklyExpense;
        List<WeekTravelExpenseList> weekTravelExpenseListLocal;
        string monday;
        string tuesday;
        string wednesday;
        string thursday;
        string friday;
        string saturday;
        string sunday;
        string header;
        int number_of_Weeks;
        ObservableCollection<LookupValue> travelExpenseReasonList;
        LookupValue selectedExpenseReason;
        int selectedExpenseReasonValue;
        List<LogEntriesByTravelExpense> expenseReportChangeLogList;
        double givenAmount;
        List<Expenses> employeeExpenseOriginalList;
        List<LogEntriesByTravelExpense> expensesLogList;
        ObservableCollection<LookupValue> expenseStatusList;
        Visibility isCurrExchangeVisible = Visibility.Hidden;

        #region GEOS2-4848
        //[pramod.misal][GEOS2-48480][22-11-2023]
        ObservableCollection<EmployeeTrips> linkedTriplList;
        EmployeeTrips selectedLinkedTrip;

        #endregion

        //COMMENTS  [chitra.girigosavi][GEOS2-4824][02.11.2023]
        ObservableCollection<LogEntriesByTravelExpense> commentsList;
        List<LogEntriesByTravelExpense> addcommentsList;
        List<LogEntriesByTravelExpense> updatedCommentsList;
        ObservableCollection<LogEntriesByTravelExpense> deleteCommentsList;
        LogEntriesByTravelExpense hrmComments;
        private bool isDeleted;
        private LogEntriesByTravelExpense selectedComment;
        ObservableCollection<LogEntriesByTravelExpense> newcommentsList;
        ObservableCollection<LogEntriesByTravelExpense> idlogentrybyitem;
        List<LogEntriesByTravelExpense> articleSuppliersComments;
        private string fullName;
        private string commentText;
        private string changeLogText;
        private TravelExpenses Travels;
        public ObservableCollection<TravelExpenses> finalExpenseList;
        public TravelExpenses selectedGridRow;
        private ImageSource travelImage;
        DateTime? commentdatetimeText = null;
        bool isBusy;
        //END COMMENTS

        ObservableCollection<EmployeeExpensePhotoInfo> photos;  //[pramod.misal][GEOS2-5077][18.01-2024]

        string geos_app_settingsReceiptsPDF_PageSize; //[pramod.misal][GEOS2-5077][24.01-2024]
        string geos_app_settingsHeader_Image; //[pramod.misal][GEOS2-5077][24.01-2024]
        string geos_app_settingsHeader_Footer_DateTime; //[pramod.misal][GEOS2-5077][24.01-2024]
        string geos_app_settings_Footer_Email;//[pramod.misal][GEOS2-5077][24.01-2024]
        string geos_app_settingsHeader_Footer_pageNumber;//[pramod.misal][GEOS2-5077][24.01-2024]
        string receiptsPDF = "ViewAllAttachedReceipts.pdf";
        ObservableCollection<EmployeeExpensePhotoInfo> attachmentList;
        #endregion

        #region Properties 
        public ObservableCollection<EmployeeExpensePhotoInfo> AttachmentList
        {
            get
            {
                return attachmentList;
            }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }
        }
        Currency originalCurrFrom;
        public Currency OriginalCurrTo
        {
            get
            {
                return originalCurrTo;
            }
            set
            {
                originalCurrTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalCurrTo"));
            }
        }
        public Currency OriginalCurrFrom
        {
            get
            {
                return originalCurrFrom;
            }
            set
            {
                originalCurrFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalCurrFrom"));
            }
        }
        Currency switchCurrency;
        public Currency SwitchCurrency
        {
            get
            {
                return switchCurrency;
            }
            set
            {
                switchCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SwitchCurrency"));
            }
        }
        public GeosAppSetting ExchangeRateDeviation
        {
            get
            {
                return exchangeRateDeviation;
            }
            set
            {
                exchangeRateDeviation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExchangeRateDeviation"));
            }
        }
        public int ExchangeRateDeviationValue
        {
            get
            {
                return exchangeRateDeviationValue;
            }
            set
            {
                exchangeRateDeviationValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExchangeRateDeviationValue"));
            }
        }
        public Currency CurrFrom
        {
            get
            {
                return currFrom;
            }
            set
            {
                currFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrFrom"));
            }
        }
        public Currency CurrTo
        {
            get
            {
                return currTo;
            }
            set
            {
                currTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrTo"));
            }
        }
        public int MealSummeryFlag
        {
            get; set;
        }
        public string ExpenseReportComment
        {
            get
            {
                return expenseReportComment;
            }
            set
            {
                expenseReportComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseReportComment"));
            }
        }
        public EmployeeMealBudget MealExpenceAllowance
        {
            get
            {
                return mealExpenceAllowance;
            }
            set
            {
                mealExpenceAllowance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MealExpenceAllowance"));
            }
        }
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaved"));
            }
        }

        #region [rdixit][09.01.2024][GEOS2-5112]
        //[rdixit][GEOS2-6979][02.04.2025]
        bool isEnableStatus;
        public bool IsEnableStatus
        {
            get
            {
                return isEnableStatus;
            }
            set
            {
                isEnableStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableStatus"));
            }
        }
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                isEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnable"));
            }
        }
        public bool IsAcceptEnable
        {
            get
            {
                return isAcceptEnable;
            }
            set
            {
                isAcceptEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable"));
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
                if (IsReadOnly)
                    IsEnable = false;
                else
                    IsEnable = true;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnly"));
            }
        }
        #endregion
        public CultureInfo SelectedCulture
        {
            get
            {
                return selectedCulture;
            }

            set
            {
                selectedCulture = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCulture"));
            }
        }
        public int ChangeLogCount
        {
            get
            {
                return changeLogCount;
            }

            set
            {
                changeLogCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogCount"));
            }
        }
        public int ExpenseCount
        {
            get
            {
                return expenseCount;
            }

            set
            {
                expenseCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseCount"));
            }
        }
        public bool IsUpdated
        {
            get
            {
                return isUpdated;
            }

            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }
        public TravelExpenses TravelExpenseReport
        {
            get
            {
                return travelExpenseReport;
            }

            set
            {
                travelExpenseReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelExpenseReport"));
            }
        }
        public bool IsCurrExchangeColVisible
        {
            get
            {
                return isCurrExchangeColVisible;
            }

            set
            {
                isCurrExchangeColVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCurrExchangeColVisible"));
            }
        }
        public bool IsCurrExchangeColReadOnly
        {
            get
            {
                return isCurrExchangeColReadOnly;
            }

            set
            {
                isCurrExchangeColReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCurrExchangeColReadOnly"));
            }
        }
        public TravelExpenseStatus TravelExpenseStatus
        {
            get
            {
                return travelExpenseStatus;
            }

            set
            {
                travelExpenseStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelExpenseStatus"));
            }
        }
        public string ExpenseColumnName
        {
            get
            {
                return expenseColumnName;
            }

            set
            {
                expenseColumnName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseColumnName"));
            }
        }
        public List<TravelExpenseStatus> TravelExpenseStatusList
        {
            get
            {
                return travelExpenseStatusList;
            }

            set
            {
                travelExpenseStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelExpenseStatusList"));
            }
        }
        public List<TravelExpenseStatus> TravelExpenseStatusButtons
        {
            get
            {
                return travelExpenseStatusButtons;
            }

            set
            {
                travelExpenseStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelExpenseStatusButtons"));
            }
        }
        public TravelExpenseStatus SelectedTravelExpenseStatusButton
        {
            get
            {
                return selectedTravelExpenseStatusButton;
            }

            set
            {
                selectedTravelExpenseStatusButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTravelExpenseStatusButton"));
            }
        }
        public List<TravelExpenseWorkflowTransitions> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        public TravelExpenseWorkflowTransitions WorkflowTransition
        {
            get
            {
                return workflowTransition;
            }

            set
            {
                workflowTransition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransition"));
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                if (EndDate > StartDate)
                    Duration = Convert.ToInt32((EndDate - StartDate).Days) + 1;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                if (EndDate > StartDate)
                    Duration = Convert.ToInt32((EndDate - StartDate).Days) + 1;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public int Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Duration"));
            }
        }
        public double Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Balance"));
            }
        }
        #region //[GEOS2-4026] [gulab lakade][23 01 2023]
        private double totalAmountForBal;
        public double TotalAmountForBal
        {
            get
            {
                return totalAmountForBal;
            }

            set
            {
                totalAmountForBal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalAmountForBal"));
            }
        }
        #endregion
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }
        public string BalanceWithCurSymbol
        {
            get { return balanceWithCurSymbol; }
            set
            {
                balanceWithCurSymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BalanceWithCurSymbol"));
            }
        }
        //[rdixit][GEOS2-3958][02.11.2022]
        public string GivenAmountWithCurSymbol
        {
            get { return givenAmountWithCurSymbol; }
            set
            {
                givenAmountWithCurSymbol = value;
                if (IsNew)  //[GEOS2-4026][gulab lakade][23 01 2023]
                {
                    BalanceWithCurSymbol = givenAmountWithCurSymbol;
                }
                else  //[GEOS2-4026] [gulab lakade][23 01 2023]
                {
                    if (string.IsNullOrEmpty(givenAmountWithCurSymbol))
                    {
                        givenAmountWithCurSymbol = "0";
                    }
                    BalanceWithCurSymbol = Convert.ToString(Convert.ToDecimal(givenAmountWithCurSymbol) - Convert.ToDecimal(TotalAmountForBal));
                    //BalanceWithCurSymbol = Convert.ToString(Convert.ToDouble(givenAmountWithCurSymbol)-Convert.ToDouble(TotalAmountForBal));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("GivenAmountWithCurSymbol"));
            }
        }

        public double GivenAmount
        {
            get { return givenAmount; }
            set
            {
                givenAmount = value;
                if (IsNew)
                {
                    BalanceWithCurSymbol = givenAmount.ToString();
                }
                else
                {
                    BalanceWithCurSymbol = Convert.ToString(Convert.ToDecimal(givenAmount) - Convert.ToDecimal(TotalAmountForBal));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("GivenAmount"));
            }
        }
        public bool IsBalanceNegative
        {
            get { return isBalanceNegative; }
            set
            {
                isBalanceNegative = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBalanceNegative"));
            }
        }
        public ObservableCollection<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }

            set
            {
                currencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyList"));
            }
        }
        public ObservableCollection<Expenses> EmployeeExpenseByExpenseReport
        {
            get
            {
                return employeeExpenseByExpenseReport;
            }

            set
            {
                employeeExpenseByExpenseReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExpenseByExpenseReport"));
            }
        }

        public ObservableCollection<Expenses> EmployeeExpenseForWeeklyGrid
        {
            get
            {
                return employeeExpenseForWeeklyGrid;
            }

            set
            {
                employeeExpenseForWeeklyGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExpenseForWeeklyGrid"));
            }
        }
        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }
        public string EmplyeeName
        {
            get
            {
                return emplyeeName;
            }

            set
            {
                emplyeeName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmplyeeName"));
            }
        }
        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }
        public Company SelectedCompanyList
        {
            get
            {
                return selectedcompanyList;
            }

            set
            {
                selectedcompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyList"));
            }
        }
        public string SubTotal
        {
            get
            {
                return subTotal;
            }

            set
            {
                subTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubTotal"));
            }
        }
        public string Tip
        {
            get
            {
                return tip;
            }

            set
            {
                tip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tip"));
            }
        }
        public string Total
        {
            get
            {
                return total;
            }

            set
            {
                total = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Total"));
            }
        }
        public ObservableCollection<WeeklyTravelExpenses> WeeklyExpensesList
        {
            get { return weeklyExpensesList; }
            set
            {
                weeklyExpensesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeeklyExpensesList"));
            }
        }
        public int Number_of_Weeks
        {
            get { return number_of_Weeks; }
            set
            {
                number_of_Weeks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Number_of_Weeks"));
            }
        }
        public WeeklyTravelExpenses SelectedWeeklyExpense
        {
            get { return selectedWeeklyExpense; }
            set
            {
                selectedWeeklyExpense = value;
                if (selectedWeeklyExpense != null)
                {
                    Monday = string.Format(System.Windows.Application.Current.FindResource("MonExpense").ToString(), selectedWeeklyExpense.MonDate);
                    Tuesday = string.Format(System.Windows.Application.Current.FindResource("TueExpense").ToString(), selectedWeeklyExpense.TueDate);
                    Wednesday = string.Format(System.Windows.Application.Current.FindResource("WedExpense").ToString(), selectedWeeklyExpense.WedDate);
                    Thursday = string.Format(System.Windows.Application.Current.FindResource("ThuExpense").ToString(), selectedWeeklyExpense.ThuDate);
                    Friday = string.Format(System.Windows.Application.Current.FindResource("FriExpense").ToString(), selectedWeeklyExpense.FriDate);
                    Saturday = string.Format(System.Windows.Application.Current.FindResource("SatExpense").ToString(), selectedWeeklyExpense.SatDate);
                    Sunday = string.Format(System.Windows.Application.Current.FindResource("SunExpense").ToString(), selectedWeeklyExpense.SunDate);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWeeklyExpense"));
            }
        }
        public List<WeekTravelExpenseList> WeekTravelExpenseListLocal
        {
            get { return weekTravelExpenseListLocal; }
            set
            {
                weekTravelExpenseListLocal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeekTravelExpenseListLocal"));
            }
        }
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }
        public string Monday
        {
            get
            {
                return monday;
            }

            set
            {
                monday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Monday"));
            }
        }
        public string Tuesday
        {
            get
            {
                return tuesday;
            }

            set
            {
                tuesday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tuesday"));
            }
        }
        public string Wednesday
        {
            get
            {
                return wednesday;
            }

            set
            {
                wednesday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Wednesday"));
            }
        }
        public string Thursday
        {
            get
            {
                return thursday;
            }

            set
            {
                thursday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Thursday"));
            }
        }
        public string Friday
        {
            get
            {
                return friday;
            }

            set
            {
                friday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Friday"));
            }
        }
        public string Saturday
        {
            get
            {
                return saturday;
            }

            set
            {
                saturday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Saturday"));
            }
        }
        public string Sunday
        {
            get
            {
                return sunday;
            }

            set
            {
                sunday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sunday"));
            }
        }
        //[rdixit][GEOS2-4022][24.11.2022]
        public ObservableCollection<LookupValue> TravelExpenseReasonList
        {
            get { return travelExpenseReasonList; }
            set
            {
                travelExpenseReasonList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravelExpenseReasonList"));
            }
        }
        public LookupValue SelectedExpenseReason
        {
            get
            {
                return selectedExpenseReason;
            }

            set
            {
                selectedExpenseReason = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExpenseReason"));
            }
        }
        public int SelectedExpenseReasonIndex
        {
            get
            {
                return selectedExpenseReasonValue;
            }

            set
            {
                selectedExpenseReasonValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExpenseReasonIndex"));
            }
        }
        public int SelectedCurrencyIndex
        {
            get
            {
                return selectedCurrencyIndex;
            }

            set
            {
                selectedCurrencyIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencyIndex"));
            }
        }

        public int SelectedEmployeeIndex
        {
            get
            {
                return selectedEmployeeIndex;
            }

            set
            {
                selectedEmployeeIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeIndex"));
            }
        }
        public int SelectedCompanyIndex
        {
            get
            {
                return selectedCompanyIndex;
            }

            set
            {
                selectedCompanyIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyIndex"));
            }
        }
        public Visibility IsEmployeeSelectVisible
        {
            get
            {
                return isEmployeeSelectVisible;
            }

            set
            {
                isEmployeeSelectVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeSelectVisible"));
            }
        }
        public Visibility IsEmployeeTextBlockVisible
        {
            get
            {
                return isEmployeeTextBlockVisible;
            }

            set
            {
                isEmployeeTextBlockVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeTextBlockVisible"));
            }
        }
        public ObservableCollection<Employee> EmployeesList
        {
            get { return employeesList; }
            set
            {
                employeesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeesList"));
            }
        }
        public ObservableCollection<Employee> AddEmployeesList
        {
            get { return addemployeesList; }
            set
            {
                addemployeesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddEmployeesList"));
            }
        }
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                //if (selectedEmployee != null && IsNew)        //[GEOS2-4026] [Gulab lakade][23 01 2023]
                if (selectedEmployee != null)
                {
                    if (SelectedEmployee.IdEmployee != 0)
                        SelectedEmployeeIndex = EmployeesList.ToList().FindIndex(x => x.IdEmployee == SelectedEmployee.IdEmployee);
                    EmplyeeName = SelectedEmployee.DisplayName;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployee"));
            }
        }
        public List<LogEntriesByTravelExpense> ExpenseReportChangeLogList
        {
            get
            {
                return expenseReportChangeLogList;
            }

            set
            {
                expenseReportChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseReportChangeLogList"));
            }
        }

        public Visibility IsCurrExchangeVisible
        {
            get
            {
                return isCurrExchangeVisible;
            }

            set
            {
                isCurrExchangeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCurrExchangeVisible"));
                if (IsCurrExchangeVisible == Visibility.Visible)
                    IsCurrExchangeColVisible = true;
                else
                    IsCurrExchangeColVisible = false;
            }
        }

        #region [rdixit][GEOS2-4471b][04.07.2023]
        public List<Expenses> EmployeeExpenseOriginalList
        {
            get
            {
                return employeeExpenseOriginalList;
            }

            set
            {
                employeeExpenseOriginalList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExpenseOriginalList"));
            }
        }
        public List<LogEntriesByTravelExpense> ExpensesLogList
        {
            get
            {
                return expensesLogList;
            }

            set
            {
                expensesLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpensesLogList"));
            }
        }
        public ObservableCollection<LookupValue> ExpenseStatusList
        {
            get { return expenseStatusList; }
            set
            {
                expenseStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseStatusList"));
            }
        }
        #endregion

        Expenses selectedExpense;
        public Expenses SelectedExpense
        {
            get
            {
                return selectedExpense;
            }

            set
            {
                selectedExpense = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExpense"));
            }
        }

        //[pramod.misal][GEOS2-4848][22-11-2023]
        public ObservableCollection<EmployeeTrips> LinkedTriplList
        {
            get { return linkedTriplList; }
            set
            {
                linkedTriplList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedTriplList"));
            }
        }
        public ObservableCollection<EmployeeTrips> LinkedTripList1
        {
            get { return linkedTripList1; }
            set
            {
                linkedTripList1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedTripList1"));
            }
        }
        ObservableCollection<EmployeeTrips> allLinkedTripList;
        public ObservableCollection<EmployeeTrips> AllLinkedTripList
        {
            get { return allLinkedTripList; }
            set
            {
                allLinkedTripList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllLinkedTripList"));
            }
        }
        public List<string> LinkedTripNamesList
        {
            get { return linkedTripNamesList; }
            set
            {
                linkedTripNamesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedTripNamesList"));
            }
        }
        public string SelectedTripText
        {
            get { return selectedTripText; }
            set
            {
                selectedTripText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTripText"));
            }
        }
        //[pramod.misal][GEOS2-4848][22-11-2023]
        public EmployeeTrips SelectedLinkedTrip
        {
            get { return selectedLinkedTrip; }
            set
            {
                selectedLinkedTrip = value;
                if (SelectedLinkedTrip != null)
                    SelectedTripText = SelectedLinkedTrip.LinkedTripTitle;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLinkedTrip"));
            }
        }

        //COMMENTS
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public ObservableCollection<LogEntriesByTravelExpense> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public List<LogEntriesByTravelExpense> UpdatedCommentsList
        {
            get { return updatedCommentsList; }
            set
            {
                updatedCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public ObservableCollection<LogEntriesByTravelExpense> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public List<LogEntriesByTravelExpense> AddCommentsList
        {
            get { return addcommentsList; }
            set
            {
                addcommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public LogEntriesByTravelExpense HrmComments
        {
            get { return hrmComments; }
            set
            {
                hrmComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HrmComments"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public LogEntriesByTravelExpense SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public string ChangeLogText
        {
            get { return changeLogText; }
            set
            {
                changeLogText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogText"));
            }
        }
        public TravelExpenses SelectedExpenseReport
        {
            get { return Travels; }
            set
            {
                // contact = value;
                SetProperty(ref Travels, value, () => SelectedExpenseReport);
            }
        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        public ObservableCollection<TravelExpenses> FinalExpenseList
        {
            get { return finalExpenseList; }
            set
            {
                finalExpenseList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalExpenseList"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public TravelExpenses SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }
        public ImageSource TravelImage
        {
            get { return travelImage; }
            set
            {
                travelImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactImage"));
            }
        }

        public DateTime? CommentDatetimeText
        {
            get { return commentdatetimeText; }
            set
            {
                commentdatetimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatetimeText"));
            }
        }

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public ObservableCollection<EmployeeExpensePhotoInfo> Photos
        {
            get { return photos; }
            set
            {
                photos = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Photos"));
            }
        }
        //END COMMENTS

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public string Geos_app_settingsReceiptsPDF_PageSize
        {
            get
            {
                return geos_app_settingsReceiptsPDF_PageSize;
            }
            set
            {
                geos_app_settingsReceiptsPDF_PageSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geos_app_settingsReceiptsPDF_PageSize"));
            }
        }

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public string Geos_app_settingsHeader_Image
        {
            get
            {
                return geos_app_settingsHeader_Image;
            }
            set
            {
                geos_app_settingsHeader_Image = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geos_app_settingsHeader_Image"));
            }
        }

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public string Geos_app_settingsHeader_Footer_DateTime
        {
            get
            {
                return geos_app_settingsHeader_Footer_DateTime;
            }
            set
            {
                geos_app_settingsHeader_Footer_DateTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geos_app_settingsHeader_Footer_DateTime"));
            }
        }

        //

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public string Geos_app_settings_Footer_Email
        {
            get
            {
                return geos_app_settings_Footer_Email;
            }
            set
            {
                geos_app_settings_Footer_Email = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geos_app_settings_Footer_Email"));
            }
        }

        //[pramod.misal][GEOS2-5077][18-01-2023]
        public string Geos_app_settingsHeader_Footer_pageNumber
        {
            get
            {
                return geos_app_settingsHeader_Footer_pageNumber;
            }
            set
            {
                geos_app_settingsHeader_Footer_pageNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Geos_app_settingsHeader_Footer_pageNumber"));
            }
        }

        ObservableCollection<LookupValue> expenseCategoriesList;
        public ObservableCollection<LookupValue> ExpenseCategoriesList
        {
            get { return expenseCategoriesList; }
            set
            {
                expenseCategoriesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseCategoriesList"));
            }
        }

        ObservableCollection<LookupValue> expenseTypesList;
        public ObservableCollection<LookupValue> ExpenseTypesList
        {
            get { return expenseTypesList; }
            set
            {
                expenseTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpenseTypesList"));
            }
        }

        List<Expenses> updateExpenseTypes;
        public List<Expenses> UpdateExpenseTypes
        {
            get
            {
                return updateExpenseTypes;
            }

            set
            {
                updateExpenseTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateExpenseTypes"));
            }
        }

        LookupValue selectedExpenseTypes;
        public LookupValue SelectedExpenseTypes
        {
            get
            {
                return selectedExpenseTypes;
            }

            set
            {
                selectedExpenseTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExpenseTypes"));
            }
        }


        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        List<EmployeeExpenseStatus> userinfo;
        public List<EmployeeExpenseStatus> UserInf
        {
            get { return userinfo; }
            set
            {
                userinfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(UserInf)));
            }
        }
        //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
        public Visibility isAnyExchangeRateZero = Visibility.Hidden;
        public Visibility IsAnyExchangeRateZero
        {
            get { return isAnyExchangeRateZero; }
            set
            {
                isAnyExchangeRateZero = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAnyExchangeRateZero"));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event EventHandler RequestClose;

        #endregion // End Of Events

        #region ICommand
        public ICommand PastingFromClipBoardCommond { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand ExchangevalueChangingCommand { get; set; }
        public ICommand CompanyPopupClosedCommand { get; set; }
        public ICommand CurrencyPopupClosedCommand { get; set; }
        public ICommand LoadedExpense { get; set; }
        public ICommand LoadeChangeLog { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand WeeklyExpenseTableViewLoadedCommand { get; set; }
        public ICommand CommandEditTravelExpenseCancel { get; set; }
        public ICommand TravelExpenseWorkflowButtonClickCommand { get; set; }
        public ICommand CurruncyExchangeButtonCommand { get; set; }
        public ICommand EditTravelExpenseCancelButtonCommand { get; set; }
        public ICommand EditTravelExpenseAcceptButtonCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand OpenLinkedImages { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand MouseClickAction_RightCommand { get; set; }
        public ICommand MouseClickAction_WrongCommand { get; set; }
        public ICommand CurrFromClickCommand { get; set; }
        public ICommand CurrToClickCommand { get; set; }
        public ICommand ChangeEmployeeCommand { get; set; }  //[pramod.misal][GEOS2-4848][23.11.2023]
        //Comments [chitra.girigosavi][GEOS2-4824][02.11.2023]
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand AddCommentsCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        //End Comments
        public ICommand ExportAttachementReportsButtonCommand { get; set; } //[pramod.misal][GEOS2-5077][17-01-2024]
        public ICommand EditValueChangedCommand { get; set; }
        //[rgadhave][GEOS2-5555][30.05.2024]
        public ICommand RemarkGridDoubleClickCommand { get; set; }
        #endregion

        #region Constructor
        public EditTravelExpenseViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor EditTravelExpenseViewModel()...", category: Category.Info, priority: Priority.Low);
            DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 20;
            DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 90;
            OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChanging);
            CustomShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(CustomShowFilterPopupAction);
            CommandEditTravelExpenseCancel = new DelegateCommand<object>(EditTravelExpenseCancelButtonCommandAction);
            TravelExpenseWorkflowButtonClickCommand = new DelegateCommand<object>(TravelExpenseWorkflowButtonClickCommandAction);
            CurruncyExchangeButtonCommand = new DelegateCommand<object>(CurruncyExchangeButtonCommandAction);
            EditTravelExpenseCancelButtonCommand = new DelegateCommand<object>(EditTravelExpenseCancelButtonCommandAction);
            EditTravelExpenseAcceptButtonCommand = new DelegateCommand<object>(EditTravelExpenseAcceptButtonCommandAction);
            OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
            OpenLinkedImages = new DelegateCommand<object>(OpenLinkedImagesMethod);
            OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
            WeeklyExpenseTableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(WeeklyExpenseTableViewLoadedCommandAction);
            LoadedExpense = new DelegateCommand<object>(LoadedExpenseAction);
            LoadeChangeLog = new DelegateCommand<object>(LoadeChangeLogAction);
            MouseClickAction_RightCommand = new DelegateCommand<object>(MouseClickAction_RightCommandAction);
            MouseClickAction_WrongCommand = new DelegateCommand<object>(MouseClickAction_WrongCommandAction);
            CompanyPopupClosedCommand = new DelegateCommand<object>(CompanyPopupClosedAction);
            CurrencyPopupClosedCommand = new DelegateCommand<object>(CurrencyPopupClosedAction);
            ExchangevalueChangingCommand = new DelegateCommand<object>(ExchangevalueChangingCommandAction);
            ExchangeRateDeviation = CRMService.GetGeosAppSettings(107);
            ExchangeRateDeviationValue = Convert.ToInt32(ExchangeRateDeviation.DefaultValue);
            CurrFromClickCommand = new DelegateCommand<object>(CurrFromClickCommandAction);
            CurrToClickCommand = new DelegateCommand<object>(CurrToClickCommandAction);
            ChangeEmployeeCommand = new DelegateCommand<object>(ChangeEmployeeCommandAction); //[pramod.misal][GEOS2-4848][23.11.2023]
            PastingFromClipBoardCommond = new DelegateCommand<object>(PastingFromClipBoardCommondAction);
            DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);//[chitra.girigosavi][GEOS2-4824][02.11.2023]
            AddCommentsCommand = new DelegateCommand<object>(AddCommentsCommandAction);//[chitra.girigosavi][GEOS2-4824][02.11.2023]
            CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);//[chitra.girigosavi][GEOS2-4824][02.11.2023]
            RemarkGridDoubleClickCommand = new DelegateCommand<object>(RemarkDoubleClickCommandAction);//[rgadhave][GEOS2-5555][30.05.2024]
            ExportAttachementReportsButtonCommand = new RelayCommand(new Action<object>(ExportAttachementReportAction));//[pramod.misal][GEOS2-5077][17-01-2024]
            TravelExpenseReport = new TravelExpenses();   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
            SelectedExpenseReport = new TravelExpenses();   //[chitra.girigosavi][GEOS2-4824][03.11.2023]

            SetPermission();
            try
            {
                //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
                EditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(EditValueChanged);
                FillExpenseCategories();
                FillExpenseTypes();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditTravelExpenseViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Constructor EditTravelExpenseViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            AllLinkedTripList = new ObservableCollection<EmployeeTrips>();


        }

        #endregion
        //[rdixit][GEOS2-6979][02.04.2025]
        void SetPermission()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetPermission ...", category: Category.Info, priority: Priority.Low);
               
                if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM || GeosApplication.Instance.IsHRMTravelManagerPermission || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                    IsReadOnly = false;
                else
                    IsReadOnly = true;

                if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM || GeosApplication.Instance.IsControlPermissionForHRM
                    || GeosApplication.Instance.IsPlant_FinancePermissionForHRM || GeosApplication.Instance.IsHRMTravelManagerPermission
                    || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                    IsAcceptEnable = true;
                else
                    IsAcceptEnable = false;

                if (GeosApplication.Instance.IsChangeAndAdminPermissionForHRM || GeosApplication.Instance.IsHRMTravelManagerPermission)
                    IsEnableStatus = true;
                else
                    IsEnableStatus = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SetPermission() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method SetPermission() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        //[rdixit][GEOS2-4848][12.12.2023]
        public void OnEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (e.NewValue != null)
                {
                    string newInput = Convert.ToString(e.NewValue);
                    if (string.IsNullOrEmpty(newInput))
                    {
                        SelectedTripText = newInput;
                        LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTripList1.Select(i => (EmployeeTrips)i.Clone()).ToList());
                    }
                    else
                    {
                        if (LinkedTripList1.Any(i => i.LinkedTripTitle.ToLower() == newInput.ToLower()))
                        {
                            SelectedLinkedTrip = LinkedTripList1.FirstOrDefault(i => i.LinkedTripTitle.ToLower() == newInput.ToLower());
                            LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTripList1.Select(i => (EmployeeTrips)i.Clone()).ToList());
                        }
                        else
                            LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTripList1.Where(j => j.LinkedTripTitle.ToLower().Contains(newInput.ToLower())).Select(i => (EmployeeTrips)i.Clone()).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnEditValueChanging() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #region Methods
        //[rdixit][14.06.2024][GEOS2-5744]
        private void PastingFromClipBoardCommondAction(object obj)
        {
            try
            {
                var view = (TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource;
                var grid = (DevExpress.Xpf.Grid.GridControl)view.Grid;
                var clipboardData = Clipboard.GetText();

                if (clipboardData != null)
                {
                    string[] rows = clipboardData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var selectedCells = view.GetSelectedCells();

                    if (rows.Length == 0 || selectedCells.Count == 0)
                    {
                        return;
                    }
                    if (selectedCells.ToList().Where(a => a.Column.FieldName == "ExchangeRate").Count() > 0)
                    {
                        int count = 0;
                        foreach (var selectedCell in selectedCells.ToList().Where(a => a.Column.FieldName == "ExchangeRate").ToList())
                        {
                            double newval = double.Parse(clipboardData, CultureInfo.CurrentCulture);
                            GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction newval : " + newval, category: Category.Info, priority: Priority.Low);
                            double Mindeviation = 0;
                            string src = newval.ToString("N", CultureInfo.CurrentCulture);
                            GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction src : " + src, category: Category.Info, priority: Priority.Low);
                            double desval = (double)(Math.Round(((Math.Round((Convert.ToDouble(ExchangeRateDeviationValue) / 100), 2)) * SelectedExpense.OriginalConversionRate), 2) + SelectedExpense.OriginalConversionRate);
                            GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction desval : " + desval, category: Category.Info, priority: Priority.Low);
                            string des = desval.ToString("N", CultureInfo.CurrentCulture);
                            GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction des : " + des, category: Category.Info, priority: Priority.Low);
                            if ((newval > desval) || newval < Mindeviation)
                            {
                                CustomMessageBox.Show("The following introduced exchange rates differ too much from the \n official exchange rate value in that day: \n \n [" +
                                SelectedExpense.ExpenseDate.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                                System.Globalization.CultureInfo.CurrentCulture) + "] Manual = " + src + " | Official = " + Math.Round(SelectedExpense.OriginalConversionRate, 2).ToString(), "Yellow",
                                CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                break;
                            }
                            else
                            {
                                if (rows.Length > count)
                                {
                                    double numericValue;
                                    bool isNumber = double.TryParse(rows[count].ToString(), out numericValue);
                                    if (isNumber == true)
                                        grid.SetCellValue(selectedCell.RowHandle, selectedCell.Column, Convert.ToDouble(rows[count]));
                                }
                                count++;
                                ((Expenses)selectedCell.Row).TransactionOperation = ModelBase.TransactionOperations.Update;
                                ((Expenses)selectedCell.Row).DisplayExchangeRate = Math.Round(newval, 4).ToString();
                                ((Expenses)selectedCell.Row).ExchangeRate = (float)Math.Round(newval, 4);
                                EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == ((Expenses)selectedCell.Row).IdEmployeeExpense).TransactionOperation = ModelBase.TransactionOperations.Update;
                            }
                        }
                    }
                }
                ((System.Windows.RoutedEventArgs)obj).Handled = true;
                view.CommitEditing();
                view.UpdateLayout();
                grid.UpdateLayout();
                grid.RefreshData();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PastingFromClipBoardCommondAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region [GEOS2-4301]
        private void CompanyPopupClosedAction(object obj)
        {
            try
            {
                GetEmployeesByIdComp(SelectedCompanyList);
                CurrFrom = CurrencyList.FirstOrDefault(i => i.IdCurrency == SelectedCompanyList.IdCurrency);
                CurrTo = (Currency)SelectedCurrency.Clone();
                if (CurrFrom.IdCurrency != CurrTo.IdCurrency)
                {
                    IsCurrExchangeVisible = Visibility.Visible;
                }
                else
                    IsCurrExchangeVisible = Visibility.Hidden;

                UpdateExpenseWithNewToAndFromCurr();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CompanyPopupClosedAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CurrencyPopupClosedAction(object obj)
        {
            try
            {
                CurrTo = (Currency)SelectedCurrency.Clone();
                CurrFrom = CurrencyList.FirstOrDefault(i => i.IdCurrency == SelectedCompanyList.IdCurrency);

                if (CurrFrom.IdCurrency != CurrTo.IdCurrency)
                    IsCurrExchangeVisible = Visibility.Visible;
                else
                    IsCurrExchangeVisible = Visibility.Hidden;

                UpdateExpenseWithNewToAndFromCurr();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CurrencyPopupClosedAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region [rdixit][GEOS2-4471][04.07.2023]
        private void MouseClickAction_WrongCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_WrongCommandAction()....", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    Expenses selectedExpense = (Expenses)obj;
                    if (selectedExpense.ExistAsAttendee != 1)
                    {
                        if (EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus == 1545)
                            EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus = 1598;
                        else
                            EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus = 1545;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_WrongCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_WrongCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }
        private void MouseClickAction_RightCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_WrongCommandAction()....", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    Expenses selectedExpense = (Expenses)obj;
                    if (selectedExpense.ExistAsAttendee != 1)
                    {
                        if (EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus == 1544)
                            EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus = 1598;
                        else
                            EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == selectedExpense.IdEmployeeExpense).IdStatus = 1544;
                    }

                }
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_RightCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method MouseClickAction_RightCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        public void FillExpenseStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExpenseStatus()...", category: Category.Info, priority: Priority.Low);

                if (ExpenseStatusList == null)
                {
                    ExpenseStatusList = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(86));

                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseStatus()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseStatus()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseStatus()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region [rdixit][GEOS2-7121][08.11.2023]
        private void CurrFromClickCommandAction(object obj)
        {
            try
            {
                Currency to = (Currency)CurrTo.Clone();
                Currency from = (Currency)CurrFrom.Clone();
                from.CurrencyIconImage = null; to.CurrencyIconImage = null;
                SwitchCurrency = from;
                MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(SelectedCompanyList.IdCompany, CurrFrom.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                #region APILayerCurrencyConversions
                //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
                if (MealExpenceAllowance.CurrencyConversion == null)
                {
                    try
                    {
                        Currency currencyFrom = new Currency();
                        currencyFrom.IdCurrency = CurrFrom.IdCurrency;
                        currencyFrom.Name = CurrFrom.Name;
                        Currency currencyTo = new Currency();
                        currencyTo.IdCurrency = SelectedCurrency.IdCurrency;
                        currencyTo.Name = SelectedCurrency.Name;
                        //HrmService = new HrmServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
                        bool result = HrmService.APILayerCurrencyConversions_V2560(StartDate, currencyFrom, currencyTo);
                        //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(SelectedCompanyList.IdCompany, CurrFrom.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                #endregion
                //Service EmployeeExpenseByExpenseReport_V2440 with EmployeeExpenseByExpenseReport_V2450 by [rdixit][GEOS2-4721][10.11.2023]
                //EmployeeExpenseForWeeklyGrid = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2450(TravelExpenseReport.IdEmployeeExpenseReport,
                //to, from));

                //Shubham[skadam] GEOS2-5145 Expenses Shared in different currency. 18 01 2024
                EmployeeExpenseForWeeklyGrid = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2480(TravelExpenseReport.IdEmployeeExpenseReport,
               to, from));
                FillWeeklyExpenseList();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateExpenseWithNewToAndFromCurr()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CurrToClickCommandAction(object obj)
        {
            try
            {
                Currency to = (Currency)CurrTo.Clone();
                Currency from = (Currency)CurrFrom.Clone();
                SwitchCurrency = to;
                from.CurrencyIconImage = null; to.CurrencyIconImage = null;
                MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(SelectedCompanyList.IdCompany, CurrTo.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                #region APILayerCurrencyConversions
                //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
                if (MealExpenceAllowance.CurrencyConversion == null)
                {
                    try
                    {
                        Currency currencyFrom = new Currency();
                        currencyFrom.IdCurrency = CurrFrom.IdCurrency;
                        currencyFrom.Name = CurrFrom.Name;
                        Currency currencyTo = new Currency();
                        currencyTo.IdCurrency = SelectedCurrency.IdCurrency;
                        currencyTo.Name = SelectedCurrency.Name;
                        //HrmService = new HrmServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
                        bool result = HrmService.APILayerCurrencyConversions_V2560(StartDate, currencyFrom, currencyTo);
                        //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(SelectedCompanyList.IdCompany, CurrTo.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                #endregion
                //Service EmployeeExpenseByExpenseReport_V2440 with EmployeeExpenseByExpenseReport_V2450 by [rdixit][GEOS2-4721][10.11.2023]
                //EmployeeExpenseForWeeklyGrid = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2450(TravelExpenseReport.IdEmployeeExpenseReport,
                //    from, to));
                //Shubham[skadam] GEOS2-5145 Expenses Shared in different currency. 18 01 2024
                EmployeeExpenseForWeeklyGrid = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2480(TravelExpenseReport.IdEmployeeExpenseReport,
                   from, to));
                FillWeeklyExpenseList();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateExpenseWithNewToAndFromCurr()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        private void LoadeChangeLogAction(object obj)
        {
            MealSummeryFlag = 0;
        }
        private void LoadedExpenseAction(object obj)
        {
            MealSummeryFlag = 0;
        }
        public void Init(TravelExpenses travelExpense)
        {
            try
            {

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                GetCurrencies();
                GetCompanies();
                FillExpenseReason();//[rdixit][GEOS2-4022][24.11.2022]
                FillExpenseStatus();
                #region [GEOS2-4026] [Gulab lakade] [23 01 2023]
                if (travelExpense.ExpenseCurrency != "---" && !string.IsNullOrEmpty(travelExpense.ExpenseCurrency))
                {

                    SelectedCurrencyIndex = CurrencyList.ToList().FindIndex(x => x.Name == travelExpense.ExpenseCurrency);
                    SelectedCurrency = CurrencyList.Where(i => i.Name == travelExpense.ExpenseCurrency).FirstOrDefault();
                    travelExpense.IdCurrency = Convert.ToInt32(SelectedCurrency.IdCurrency);
                }
                else
                {
                    SelectedCurrencyIndex = 0;
                    SelectedCurrency = CurrencyList.FirstOrDefault();
                }
                //[rdixit][GEOS2-4301][04.08.2023]
                if (travelExpense.IdCurrencyFrom != 0)
                    CurrFrom = (Currency)CurrencyList.FirstOrDefault(i => i.IdCurrency == travelExpense.IdCurrencyFrom).Clone();
                else
                    CurrFrom = (Currency)CurrencyList.FirstOrDefault(i => i.IdCurrency == SelectedCompanyList.IdCurrency).Clone();

                if (travelExpense.IdCurrencyTo != 0)
                    CurrTo = (Currency)CurrencyList.FirstOrDefault(i => i.IdCurrency == travelExpense.IdCurrencyTo).Clone();
                else
                    CurrTo = (Currency)SelectedCurrency.Clone();
                if (travelExpense.IdCompany != 0)//[rdixit][GEOS2-4022][24.11.2022]
                {
                    SelectedCompanyList = CompanyList.Where(i => i.IdCompany == travelExpense.IdCompany).FirstOrDefault();
                    SelectedCompanyIndex = CompanyList.ToList().FindIndex(x => x.IdCompany == travelExpense.IdCompany);
                }
                else
                {
                    SelectedCompanyList = CompanyList.Where(i => i.IdCompany == 0).FirstOrDefault();
                    SelectedCompanyIndex = 0;
                }
                SwitchCurrency = (Currency)SelectedCurrency.Clone();
                OriginalCurrTo = (Currency)CurrencyList.Where(i => i.Name == travelExpense.ExpenseCurrency).FirstOrDefault().Clone();
                OriginalCurrFrom = (Currency)CurrencyList.FirstOrDefault(i => i.IdCurrency == SelectedCompanyList.IdCurrency).Clone();
                if (CurrTo.IdCurrency != CurrFrom.IdCurrency)
                    IsCurrExchangeVisible = Visibility.Visible;
                else
                    IsCurrExchangeVisible = Visibility.Hidden;
                #endregion
                GetAllExpensesByExpenseReport(travelExpense.IdEmployeeExpenseReport);
                #region[rdixit][21.06.2023][GEOS2-4438]
                List<Expenses> ReporterExpenses = EmployeeExpenseByExpenseReport.Where(x => x.AttendiesCount > 1 && x.ExistAsAttendee != 1).ToList();
                if (ReporterExpenses != null)
                {
                    foreach (var item in ReporterExpenses)
                    {
                        item.ExpenseAttendees.Add(new ExpenseAttendees() { AttendeeName = travelExpense.Reporter, IsReporter = Visibility.Visible });
                    }
                }
                List<Expenses> ReporterAsAttendeeExpenses = EmployeeExpenseByExpenseReport.Where(x => x.AttendiesCount > 1 && x.ExistAsAttendee == 1).ToList();
                if (ReporterAsAttendeeExpenses != null)
                {
                    foreach (var item in ReporterAsAttendeeExpenses)
                    {
                        item.ExpenseAttendees.Add(new ExpenseAttendees() { AttendeeName = travelExpense.Reporter, IsReporter = Visibility.Hidden });
                    }
                }
                #endregion
                TravelExpenseReport = travelExpense;
                IsEmployeeSelectVisible = Visibility.Hidden;
                IsEmployeeTextBlockVisible = Visibility.Visible;
                IsNew = false;
                IsActive = false;
                FillExpenseGrid();


                CommentsList = new ObservableCollection<LogEntriesByTravelExpense>(HrmService.GetTravelExpenseComments_V2470(TravelExpenseReport.IdEmployeeExpenseReport).OrderByDescending(c => c.Datetime));

                if (CommentsList.Count > 0)
                {
                    var latestComment = CommentsList.First();
                    CommentText = latestComment.Comments;
                    CommentDatetimeText = latestComment.Datetime;
                }
                else
                {
                    CommentText = string.Empty; // or some default value if there are no comments
                }

                foreach (var item in CommentsList)
                {
                    SetUserProfileImage(item);
                }

                //[rdixit][GEOS2-3829][21.09.2022]
                #region Change_Log
                ExpenseReportChangeLogList = new List<LogEntriesByTravelExpense>(HrmService.GetTravelExpenseChangelogs_V2470(TravelExpenseReport.IdEmployeeExpenseReport));
                if (ExpenseReportChangeLogList.Count > 0)
                // ChangeLogCount = ExpenseReportChangeLogList.Count;
                {
                    ChangeLogText = ExpenseReportChangeLogList[0].Comments;
                }
                else
                {
                    ChangeLogText = string.Empty;
                }
                #endregion
                if (TravelExpenseReport.Reason != null)//[rdixit][GEOS2-4022][24.11.2022]
                {
                    TravelExpenseReport.Reason = TravelExpenseReasonList.Where(j => j.IdLookupValue == TravelExpenseReport.Reason.IdLookupValue).FirstOrDefault();
                    SelectedExpenseReason = TravelExpenseReport.Reason;
                    SelectedExpenseReasonIndex = TravelExpenseReasonList.ToList().FindIndex(x => x.IdLookupValue == TravelExpenseReport.Reason.IdLookupValue);
                }
                else
                {
                    SelectedExpenseReason = TravelExpenseReasonList.Where(i => i.IdLookupValue == 0).FirstOrDefault();
                    TravelExpenseReport.Reason = SelectedExpenseReason;
                    SelectedExpenseReasonIndex = 0;
                }

                GetEmployeesByIdComp(SelectedCompanyList);

                ExpenseColumnName = string.Format(System.Windows.Application.Current.FindResource("ExpenseColumn").ToString(), "[" + CurrFrom.Name + "-" + CurrTo.Name + "]");
                MaximizedElementPosition = MaximizedElementPosition.Right;
                TravelExpenseStatusList = new List<TravelExpenseStatus>(HrmService.GetAllTravelExpenseStatus());
                WorkflowTransitionList = new List<TravelExpenseWorkflowTransitions>(HrmService.GetAllWorkflowTransitions_V2370());
                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == travelExpense.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                TravelExpenseStatusButtons = new List<TravelExpenseStatus>();
                foreach (byte statusbutton in GetCurrentButtons)
                {
                    TravelExpenseStatusButtons.Add(TravelExpenseStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }

                SetButtonsEnableMethod();

                if (travelExpense != null)
                {
                    FullName = SelectedEmployee.FullName;  //[chitra.girigosavi][GEOS2-4824][02.11.2023] 
                    Code = travelExpense.ExpenseCode;
                    SelectedCurrency.Name = travelExpense.ExpenseCurrency;
                    SelectedCurrency.CurrencyIconImage = ByteArrayToBitmapImage(travelExpense.CountryIconBytes);

                    StartDate = travelExpense.StartDate;
                    EndDate = travelExpense.EndDate;
                    TotalAmountForBal = Convert.ToDouble(travelExpense.TotalAmountForBal); //[GEOS2-4026] [gulab lakade][23 01 2023]
                    Balance = (travelExpense.GivenAmount - travelExpense.TotalAmountForBal);
                    GivenAmount = travelExpense.GivenAmount;
                    if (Balance < 0)
                        IsBalanceNegative = true;
                    else
                        IsBalanceNegative = false;
                    Duration = travelExpense.Duration;
                    Status = travelExpense.Status;
                    Title = travelExpense.ExpenseTitle;
                    if (travelExpense.IdEmployee != 0)
                    {
                        SelectedEmployeeIndex = EmployeesList.ToList().FindIndex(x => x.IdEmployee == travelExpense.IdEmployee);
                        EmplyeeName = travelExpense.Reporter;
                    }
                    Remark = travelExpense.Remarks;
                    TravelExpenseStatus = new TravelExpenseStatus();
                    TravelExpenseStatus.Name = travelExpense.Status;
                    TravelExpenseStatus.IdWorkflowStatus = travelExpense.IdWorkflowStatus;
                    if (TravelExpenseStatus != null)
                    {
                        if (TravelExpenseStatus.IdWorkflowStatus == 25)
                            IsCurrExchangeColReadOnly = true;
                        else
                            IsCurrExchangeColReadOnly = false;
                    }
                    TravelExpenseStatus.HtmlColor = TravelExpenseStatusList.Where(a => a.IdWorkflowStatus == travelExpense.IdWorkflowStatus).FirstOrDefault().HtmlColor;
                    //Service updated from GetMealExpenseByEmployyeAndCompany with GetMealExpenseByEmployyeAndCompany_V2440 [rdixit][11.10.2023][GEOS2-4721]
                    //[rdixit] [15 03 2023][GEOS2-4178]   
                    MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(travelExpense.IdCompany, SelectedCurrency.IdCurrency, StartDate, travelExpense.IdEmployee);
                    #region APILayerCurrencyConversions
                    //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
                    if (MealExpenceAllowance.CurrencyConversion == null)
                    {
                        try
                        {
                            Currency currencyFrom = new Currency();
                            currencyFrom.IdCurrency = CurrFrom.IdCurrency;
                            currencyFrom.Name = CurrFrom.Name;
                            Currency currencyTo = new Currency();
                            currencyTo.IdCurrency = SelectedCurrency.IdCurrency;
                            currencyTo.Name = SelectedCurrency.Name;
                            //HrmService = new HrmServiceController("localhost:6699");
                            //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
                            bool result = HrmService.APILayerCurrencyConversions_V2560(StartDate, currencyFrom, currencyTo);
                            //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(travelExpense.IdCompany, SelectedCurrency.IdCurrency, StartDate, travelExpense.IdEmployee);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    #endregion
                    FillWeeklyExpenseList(); //[rdixit] [23 01 2023][GEOS2-4090]
                    SelectedEmployee.OwnerImage = TravelImage; //[chitra.girigosavi][GEOS2-4824][02.11.2023]

                    #region  [GEOS2-4848]
                    //[pramod.misal][GEOS2-4848][01.12.2023]
                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                        int empid = travelExpense.IdEmployee;//pramod.misal

                        //LinkedTriplList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdCompany_V2440(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                        LinkedTriplList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdEmpolyee_V2460(empid));
                        LinkedTriplList.ToList().ForEach(i => i.LinkedTripTitle = $"[{i.ArrivalDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture)} - {i.DepartureDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture)}] – {i.Title}");
                        LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTriplList.OrderByDescending(expense => expense.ArrivalDate).ToList());
                        EmployeeTrips defaultValue = new EmployeeTrips { LinkedTripTitle = "---", IdEmployeeTrip = 0 };
                        LinkedTriplList.Insert(0, defaultValue);
                        LinkedTripNamesList = LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).Select(k => k.LinkedTripTitle).ToList();
                        LinkedTripList1 = new ObservableCollection<EmployeeTrips>(LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).ToList());
                        AllLinkedTripList.AddRange(LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).ToList());
                        if (travelExpense.IdEmployee != 0)
                        {
                            SelectedLinkedTrip = LinkedTriplList.FirstOrDefault(i => i.IdEmployeeTrip == travelExpense.IdLinkedTrip);

                            if (SelectedLinkedTrip == null)
                            {
                                SelectedLinkedTrip = LinkedTriplList.FirstOrDefault();
                            }
                        }
                        else
                        {
                            SelectedLinkedTrip = LinkedTriplList.FirstOrDefault();
                        }
                        SelectedTripText = SelectedLinkedTrip.LinkedTripTitle;
                    }

                    #endregion GEOS2-4848
                    #region DisableExpenseType 
                    //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
                    try
                    {
                        if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 143) && up.Permission.IdGeosModule == 7)))
                        {
                            //EmployeeExpenseByExpenseReport.All(a => a.IsEnabled = true);
                            EmployeeExpenseByExpenseReport.ToList().ForEach(a => a.IsEnabled = true);
                        }
                        else
                        {
                            //EmployeeExpenseByExpenseReport.All(a => a.IsEnabled = false);
                            EmployeeExpenseByExpenseReport.ToList().ForEach(a => a.IsEnabled = false);
                        }
                        if (travelExpense.Status.ToLower().Equals("Approved".ToLower()) || travelExpense.Status.ToLower().Equals("Closed".ToLower()))
                        {
                            //EmployeeExpenseByExpenseReport.All(a => a.IsEnabled = false);
                            EmployeeExpenseByExpenseReport.ToList().ForEach(a => a.IsEnabled = false);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetCurrencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()...", category: Category.Info, priority: Priority.Low);
                #region GEOS2-4175
                //CurrencyList = new ObservableCollection<Currency>(PLMService.GetCurrencies_V2160());
                //Shubham[skadam] GEOS2-4175 Expense report not show proper for  status draft  03 02 2023
                //Service GetCurrencies_V2360 updated with GetCurrencies_V2440 by [rdixit][12.10.2023][GEOS-4721]
                CurrencyList = new ObservableCollection<Currency>(HrmService.GetCurrencies_V2440());
                #endregion
                if (CurrencyList != null)
                {
                    foreach (var bpItem in CurrencyList.GroupBy(tpa => tpa.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                    }
                }
                CurrencyList.Insert(0, new Currency() { Name = "---", IdCurrency = 0 });
                SelectedCurrency = CurrencyList.FirstOrDefault();
                SwitchCurrency = (Currency)SelectedCurrency.Clone();
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetCurrencies() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetCompanies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCompanies()...", category: Category.Info, priority: Priority.Low);
                //Service Method GetCompanies updated with GetCompanies_V2390 by rdixit on 08.05.2023 in Task GEOS2-4297
                //Service Method GetCompanies_V2390 updated with GetCompanies_V2420 by rdixit on 07.08.2023 in Task GEOS2-4297
                CompanyList = new ObservableCollection<Company>(HrmService.GetCompanies_V2420());
                if (CompanyList != null)
                {
                    foreach (var bpItem in CompanyList.GroupBy(tpa => tpa.Iso))
                    {
                        ImageSource CompanyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().ImageInBytes);
                        bpItem.ToList().Where(oti => oti.Iso == bpItem.Key).ToList().ForEach(oti => oti.SiteImage = CompanyFlagImage);
                    }
                }
                CompanyList.Insert(0, new Company() { Alias = "---", IdCompany = 0 });
                SelectedCompanyList = CompanyList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method GetCompanies()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        private void TravelExpenseWorkflowButtonClickCommandAction(object obj)
        {
            try
            {
                int status_id = Convert.ToInt32(obj);
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                TransitionWorkflowStatus(status_id);
                SetButtonsEnableMethod();
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method WorkflowButtonClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void TransitionWorkflowStatus(int currentStatus)
        {
            List<LogEntriesByTravelExpense> LogEntriesByTravelExpense = new List<LogEntriesByTravelExpense>();
            //[rdixit][GEOS2-4016][23.03.2023] 
            //TravelExpenseWorkflowTransitions workflowTransition = new TravelExpenseWorkflowTransitions();
            ExpenseReportComment = string.Empty;
            WorkflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
            if (WorkflowTransition.IsCommentRequired == 1)
            {
                DeclineStatusClickEvent();
                if (!string.IsNullOrEmpty(ExpenseReportComment))
                {
                    TravelExpenseStatus = TravelExpenseStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);
                    TravelExpenseReport.IdWorkflowStatus = TravelExpenseStatus.IdWorkflowStatus;
                    TravelExpenseReport.Status = TravelExpenseStatus.Name;
                    TravelExpenseReport.IdWorkflowStatus = TravelExpenseStatus.IdWorkflowStatus;

                    List<byte> GetCurrentButtons1 = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == TravelExpenseReport.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                    TravelExpenseStatusButtons = new List<TravelExpenseStatus>();

                    foreach (byte statusbutton in GetCurrentButtons1)
                    {
                        TravelExpenseStatusButtons.Add(TravelExpenseStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                    }
                    TravelExpenseStatusButtons = new List<TravelExpenseStatus>(TravelExpenseStatusButtons);
                }
            }
            else
            {
                TravelExpenseStatus = TravelExpenseStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);
                TravelExpenseReport.IdWorkflowStatus = TravelExpenseStatus.IdWorkflowStatus;
                TravelExpenseReport.Status = TravelExpenseStatus.Name;
                TravelExpenseReport.IdWorkflowStatus = TravelExpenseStatus.IdWorkflowStatus;

                List<byte> GetCurrentButtons1 = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == TravelExpenseReport.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                TravelExpenseStatusButtons = new List<TravelExpenseStatus>();

                foreach (byte statusbutton in GetCurrentButtons1)
                {
                    TravelExpenseStatusButtons.Add(TravelExpenseStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                TravelExpenseStatusButtons = new List<TravelExpenseStatus>(TravelExpenseStatusButtons);
            }
            if (TravelExpenseStatus != null)
            {
                if (TravelExpenseStatus.IdWorkflowStatus == 25)
                    IsCurrExchangeColReadOnly = true;
                else
                    IsCurrExchangeColReadOnly = false;
            }
        }
        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                HrmWorkflowDiagramViewModel HrmWorkflowDiagramViewModel = new HrmWorkflowDiagramViewModel();
                HrmWorkflowDiagramView HrmWorkflowDiagramView = new HrmWorkflowDiagramView();
                EventHandler handle = delegate { HrmWorkflowDiagramView.Close(); };
                HrmWorkflowDiagramViewModel.RequestClose += handle;
                HrmWorkflowDiagramViewModel.TravelExpenseStatusList = TravelExpenseStatusList;
                HrmWorkflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a => a.IdWorkflowTransition).ToList();
                HrmWorkflowDiagramView.DataContext = HrmWorkflowDiagramViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                HrmWorkflowDiagramView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditTravelExpenseCancelButtonCommandAction(object obj)
        {
            IsUpdated = false;
            if (TravelExpenseReport != null && Status != null && TravelExpenseStatusList != null)
            {
                TravelExpenseReport.Status = Status;
                TravelExpenseReport.IdWorkflowStatus = TravelExpenseStatusList.Where(a => a.Name == Status).FirstOrDefault().IdWorkflowStatus;
            }
            RequestClose(null, null);
        }
        private void EditTravelExpenseAcceptButtonCommandAction(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedTripText))
                    SelectedLinkedTrip = LinkedTripList1.FirstOrDefault(i => i.IdEmployeeTrip == 0);
                else
                    SelectedLinkedTrip = LinkedTripList1.FirstOrDefault(i => i.LinkedTripTitle.ToLower() == SelectedTripText.ToLower());

                if (SelectedLinkedTrip == null)
                    SelectedLinkedTrip = LinkedTripList1.FirstOrDefault(i => i.IdEmployeeTrip == 0);

                if (SelectedLinkedTrip.LinkedTripTitle != "---")
                {
                    //https://helpdesk.emdep.com/projects/IESD/queues/custom/3/IESD-97208
                    if (SelectedLinkedTrip.ArrivalDate.HasValue && SelectedLinkedTrip.DepartureDate.HasValue)
                    {
                        DateTime arrivalDate = SelectedLinkedTrip.ArrivalDate.Value.Date;
                        DateTime departureDate = SelectedLinkedTrip.DepartureDate.Value.Date;

                        if (!(arrivalDate >= StartDate.Date && arrivalDate <= EndDate.Date && departureDate >= StartDate.Date && departureDate <= EndDate.Date))
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DifferentExpenseDateNotAllowed"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                        }
                    }
                }

                SelectedExpenseReasonIndex = TravelExpenseReasonList.ToList().FindIndex(x => x.IdLookupValue == SelectedExpenseReason.IdLookupValue);
                SelectedCompanyIndex = CompanyList.ToList().FindIndex(x => x.IdCompany == SelectedCompanyList.IdCompany);
                SelectedCurrencyIndex = CurrencyList.ToList().FindIndex(x => x.IdCurrency == SelectedCurrency.IdCurrency);
                if (SelectedEmployee != null)
                    SelectedEmployeeIndex = EmployeesList.ToList().FindIndex(x => x.IdEmployee == SelectedEmployee.IdEmployee);
                else
                    SelectedEmployeeIndex = 0;

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedExpenseReasonValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCompanyIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCurrencyIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedEmployeeIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("Title"));

                if (error != null)
                {
                    return;
                }
                GeosApplication.Instance.Logger.Log("Method EditTravelExpenseAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsNew)//[rdixit][GEOS2-4025][24.11.2022]
                {
                    #region New Expense Report
                    TravelExpenses NewTravelExp = new TravelExpenses();
                    NewTravelExp.ExpenseCode = Code;
                    NewTravelExp.IdCompany = SelectedCompanyList.IdCompany;
                    NewTravelExp.IdCurrency = SelectedCurrency.IdCurrency;
                    NewTravelExp.IdEmployee = SelectedEmployee.IdEmployee;
                    NewTravelExp.Remarks = Remark;
                    NewTravelExp.ExpenseTitle = Title;
                    NewTravelExp.Reason = SelectedExpenseReason;
                    NewTravelExp.StartDate = StartDate;
                    NewTravelExp.EndDate = EndDate;
                    NewTravelExp.IdWorkflowStatus = TravelExpenseStatus.IdWorkflowStatus;
                    NewTravelExp.GivenAmount = GivenAmount;
                    NewTravelExp.IdCurrencyTo = CurrTo.IdCurrency;
                    NewTravelExp.IdCurrencyFrom = CurrFrom.IdCurrency;
                    NewTravelExp.IdLinkedTrip = SelectedLinkedTrip.IdEmployeeTrip == 0 ? null : (int?)SelectedLinkedTrip.IdEmployeeTrip;//[pramod.misal][GEOS2-4848][23.11.2023]
                    NewTravelExp.LinkedTripTitle = selectedLinkedTrip.LinkedTripTitle;//[pramod.misal][GEOS2-4848][24.11.2023]
                    SelectedEmployee.OwnerImage = TravelImage;

                    SelectedEmployee.ImageText = null;
                    if (Emdep.Geos.UI.Helper.ImageEditHelper.Base64String != null)
                    {
                        SelectedEmployee.ImageText = Emdep.Geos.UI.Helper.ImageEditHelper.Base64String;
                        byte[] imageBytes = Convert.FromBase64String(SelectedEmployee.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        SelectedEmployee.OwnerImage = ByteArrayToBitmapImage(imageBytes);
                        TravelImage = SelectedEmployee.OwnerImage;
                    }
                    else
                        SelectedEmployee.ImageText = null;

                    SelectedEmployee.OwnerImage = null;

                    if (ExpenseReportChangeLogList == null)
                    {
                        ExpenseReportChangeLogList = new List<LogEntriesByTravelExpense>();
                    }
                    NewTravelExp.LogEntry = new LogEntriesByTravelExpense()
                    {
                        IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = DateTime.Now,
                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseReportAddedChangeLog").ToString(), Code)
                    };
                    if (TravelExpenseReport.Comments == null)   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                        TravelExpenseReport.Comments = new List<LogEntriesByTravelExpense>();
                    if (AddCommentsList != null)
                    {
                        foreach (var item in AddCommentsList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            TravelExpenseReport.Comments.Add(item);
                        }
                    }
                    NewTravelExp.Comments = TravelExpenseReport.Comments;
                    if (NewTravelExp != null)
                    {
                        //service method updated from AddTravelExpenseReport to AddTravelExpenseReport_V2420 by  [rdixit][GEOS2-4301][04.08.2023]
                        //NewTravelExp = HrmService.AddTravelExpenseReport_V2420(NewTravelExp, GeosApplication.Instance.ActiveUser.IdUser);

                        //[pramod.misal][GEOS2-4848][23.11.2023]
                        //service method updated from AddTravelExpenseReport_V2420 to AddTravelExpenseReport_V2460 by [pramod.misal][GEOS2-4848][23.11.2023]
                        NewTravelExp = HrmService.AddTravelExpenseReport_V2470(NewTravelExp, GeosApplication.Instance.ActiveUser.IdUser, NewTravelExp.Comments);

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        if (NewTravelExp.IdEmployeeExpenseReport != 0)
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TravelExpenseReportAddSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsSaved = true;
                    }
                    #endregion
                }
                else
                {
                    #region Log_Entry & UpdateMethods

                    #region //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                    if (TravelExpenseReport.Comments == null)
                        TravelExpenseReport.Comments = new List<LogEntriesByTravelExpense>();
                    if (AddCommentsList != null)
                    {
                        foreach (var item in AddCommentsList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            TravelExpenseReport.Comments.Add(item);
                        }
                    }

                    if (UpdatedCommentsList != null)
                    {
                        foreach (var item in UpdatedCommentsList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Update;
                            TravelExpenseReport.Comments.Add(item);
                        }
                    }

                    if (DeleteCommentsList != null)
                    {
                        foreach (var item in DeleteCommentsList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            TravelExpenseReport.Comments.Add(item);
                        }
                    }
                    #endregion

                    #region [GEOS2-4024][Rupali Sarode][20-01-2023]
                    string oldRemarks = string.Empty;
                    string newRemarks = string.Empty;
                    string oldGivenAmount = string.Empty;


                    oldRemarks = Convert.ToString(TravelExpenseReport.Remarks).Trim().ToUpper();
                    newRemarks = Convert.ToString(Remark).Trim().ToUpper();
                    oldGivenAmount = Convert.ToString(TravelExpenseReport.GivenAmount);
                    #endregion

                    #region Update report details change log
                    TravelExpenseReport.LogEntries = new List<LogEntriesByTravelExpense>();

                    if (Status != Convert.ToString(TravelExpenseReport.Status) || SelectedExpenseReason.IdLookupValue != TravelExpenseReport.Reason.IdLookupValue)
                    {
                        //[GEOS2-4016][23.03.2023][rdixit]
                        //if (WorkflowTransition != null)
                        //{
                        //    if (WorkflowTransition.IsCommentRequired == 1)
                        //        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        //        {
                        //            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                        //            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        //            Datetime = DateTime.Now,
                        //            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                        //            Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportCommentLog").ToString(), ExpenseReportComment)
                        //        });
                        //}
                        if (Status != Convert.ToString(TravelExpenseReport.Status))
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseStatusChangedLog").ToString(), Status, Convert.ToString(TravelExpenseReport.Status))
                            });
                        }
                        if (SelectedExpenseReason.IdLookupValue != TravelExpenseReport.Reason.IdLookupValue)
                        {
                            if (Convert.ToString(TravelExpenseReport.Reason.Value) == "---")
                            {
                                TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                                {
                                    IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseReasonChangedLog").ToString(), "None", SelectedExpenseReason.Value)
                                });
                            }
                            else if (Convert.ToString(SelectedExpenseReason.Value) == "---")
                            {
                                TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                                {
                                    IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseReasonChangedLog").ToString(), TravelExpenseReport.Reason.Value, "None")
                                });
                            }
                            else
                            {
                                TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                                {
                                    IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseReasonChangedLog").ToString(), TravelExpenseReport.Reason.Value, SelectedExpenseReason.Value)
                                });
                            }

                            TravelExpenseReport.Reason = SelectedExpenseReason;
                        }

                    }
                    //[GEOS2-4024][Rupali Sarode][20-01-2023]
                    if (SelectedCompanyList.IdCompany != TravelExpenseReport.IdCompany)
                    {
                        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        {
                            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = DateTime.Now,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseCompanyChangedLog").ToString(), Convert.ToString(TravelExpenseReport.Company), SelectedCompanyList.Alias)
                        });
                        TravelExpenseReport.IdCompany = SelectedCompanyList.IdCompany;
                    }


                    if (SelectedCurrency.Name != TravelExpenseReport.ExpenseCurrency)
                    {
                        string oldCurrency = Convert.ToString(CurrencyList.Where(x => x.Name == TravelExpenseReport.ExpenseCurrency).FirstOrDefault().Name);
                        if (Convert.ToString(oldCurrency) == "---")
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseCurrencyChangedLog").ToString(), Convert.ToString("None"), Convert.ToString(SelectedCurrency.Name))
                            });
                            TravelExpenseReport.IdCurrency = SelectedCurrency.IdCurrency;
                        }
                        else if (Convert.ToString(SelectedCurrency.Name) == "---")
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseCurrencyChangedLog").ToString(), Convert.ToString(oldCurrency), Convert.ToString("None"))
                            });
                            TravelExpenseReport.IdCurrency = SelectedCurrency.IdCurrency;
                        }
                        else
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseCurrencyChangedLog").ToString(), Convert.ToString(oldCurrency), Convert.ToString(SelectedCurrency.Name))
                            });
                            TravelExpenseReport.IdCurrency = SelectedCurrency.IdCurrency;
                        }



                    }
                    if (SelectedEmployee.IdEmployee != TravelExpenseReport.IdEmployee)
                    {
                        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        {
                            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = DateTime.Now,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseEmployeeChangedLog").ToString(), Convert.ToString(TravelExpenseReport.Reporter), EmplyeeName)
                        });
                        TravelExpenseReport.IdEmployee = SelectedEmployee.IdEmployee;
                    }
                    if (Title != TravelExpenseReport.ExpenseTitle)
                    {
                        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        {
                            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = DateTime.Now,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseTitleChangedLog").ToString(), Convert.ToString(TravelExpenseReport.ExpenseTitle), Title)
                        });
                        TravelExpenseReport.ExpenseTitle = Title;
                    }

                    if (StartDate != TravelExpenseReport.StartDate)
                    {
                        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        {
                            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = DateTime.Now,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseStartDateChangedLog").ToString(), Convert.ToString((TravelExpenseReport.StartDate).ToShortDateString()), StartDate.ToShortDateString())
                        });
                        TravelExpenseReport.StartDate = StartDate;
                    }

                    if (EndDate != TravelExpenseReport.EndDate)
                    {
                        TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                        {
                            IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = DateTime.Now,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseEndDateChangedLog").ToString(), Convert.ToString((TravelExpenseReport.EndDate).ToShortDateString()), EndDate.ToShortDateString())
                        });
                        TravelExpenseReport.EndDate = EndDate;
                    }

                    if (newRemarks != oldRemarks)
                    {
                        if (!string.IsNullOrEmpty(newRemarks) && string.IsNullOrEmpty(oldRemarks))
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseRemarksChangedLog").ToString(), "None", Convert.ToString(newRemarks))
                            });
                            TravelExpenseReport.Remarks = newRemarks;
                        }
                        else if (string.IsNullOrEmpty(newRemarks) && !string.IsNullOrEmpty(oldRemarks))
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseRemarksChangedLog").ToString(), oldRemarks, Convert.ToString("None"))
                            });
                            TravelExpenseReport.Remarks = newRemarks;
                        }
                        else if (!string.IsNullOrEmpty(newRemarks) && !string.IsNullOrEmpty(oldRemarks))
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseRemarksChangedLog").ToString(), oldRemarks, Convert.ToString(newRemarks))
                            });
                            TravelExpenseReport.Remarks = newRemarks;
                        }
                    }

                    if (GivenAmount.ToString() != oldGivenAmount)
                    {
                        //[GEOS2-4391][rdixit][26.04.2023]
                        double subamount = Math.Round((GivenAmount - TravelExpenseReport.GivenAmount), 2, MidpointRounding.AwayFromZero);
                        if (subamount > 0)
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseGivenAmountAddChangedLog").ToString(),
                                oldGivenAmount, Convert.ToString(GivenAmount), Math.Abs(subamount))
                            });
                        }
                        else
                        {
                            TravelExpenseReport.LogEntries.Add(new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TravelExpenseGivenAmountDeductChangedLog").ToString(),
                                oldGivenAmount, Convert.ToString(GivenAmount), Math.Abs(subamount), subamount)
                            });
                        }
                        TravelExpenseReport.GivenAmount = GivenAmount;
                    }
                    #endregion

                    #region [rdixit][GEOS2-4471][04.07.2023] Status change log
                    ExpensesLogList = new List<LogEntriesByTravelExpense>();
                    if (EmployeeExpenseOriginalList != null)
                    {
                        string ReportedtoApprove = string.Empty, ReportedtoRejected = string.Empty, RejectedtoApprove = string.Empty, ApprovetoRejected = string.Empty;
                        string RejectedtoReported = string.Empty, ApprovetoReported = string.Empty;

                        foreach (var item in EmployeeExpenseOriginalList)
                        {
                            Expenses Expense = EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == item.IdEmployeeExpense);
                            if (Expense.IdStatus != item.IdStatus)
                            {
                                Expense.TransactionOperation = ModelBase.TransactionOperations.Update;
                                LookupValue fromStatus = ExpenseStatusList.FirstOrDefault(i => i.IdLookupValue == item.IdStatus);
                                LookupValue toStatus = ExpenseStatusList.FirstOrDefault(i => i.IdLookupValue == Expense.IdStatus);
                                ExpensesLogList.Add(
                                    new LogEntriesByTravelExpense()
                                    {
                                        IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                        IdEmployeeExpense = Expense.IdEmployeeExpense,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = DateTime.Now,
                                        UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseStatusChangedLog").ToString(), fromStatus.Value, toStatus.Value)
                                    });
                                //[shweta.thube][GEOS2-6802][04.02.2025]
                                if (item.IdStatus == 1598 && Expense.IdStatus == 1545)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(ReportedtoRejected))
                                        ReportedtoRejected = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    else
                                        ReportedtoRejected = ReportedtoRejected + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                }
                                if (item.IdStatus == 1598 && Expense.IdStatus == 1544)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(ReportedtoApprove))
                                    {
                                        //ReportedtoApprove = item.IdEmployeeExpense.ToString();
                                        ReportedtoApprove = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    }
                                    else
                                    {
                                        ReportedtoApprove = ReportedtoApprove + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    }

                                }
                                if (item.IdStatus == 1544 && Expense.IdStatus == 1545)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(ApprovetoRejected))
                                        ApprovetoRejected = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    else
                                        ApprovetoRejected = ApprovetoRejected + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                }
                                if (item.IdStatus == 1545 && Expense.IdStatus == 1544)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(RejectedtoApprove))
                                        RejectedtoApprove = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    else
                                        RejectedtoApprove = RejectedtoApprove + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                }
                                if (item.IdStatus == 1544 && Expense.IdStatus == 1598)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(ApprovetoReported))
                                        ApprovetoReported = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    else
                                        ApprovetoReported = ApprovetoReported + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                }
                                if (item.IdStatus == 1545 && Expense.IdStatus == 1598)
                                {
                                    DateTime parsedTime = DateTime.Parse(item.ExpenseTime);
                                    string formattedTime = parsedTime.ToString("HH:mm");

                                    if (string.IsNullOrEmpty(RejectedtoReported))
                                        RejectedtoReported = item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                    else
                                        RejectedtoReported = RejectedtoReported + ", " + item.Summary.ToString() + " " + item.ExpenseDate.ToString("dd-MM-yyyy") + " " + formattedTime;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(ReportedtoApprove))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportReportedToApprovedChangedLog").ToString(), ReportedtoApprove)
                            });

                        if (!string.IsNullOrEmpty(ReportedtoRejected))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportReportedToRejectedChangedLog").ToString(), ReportedtoRejected)
                            });

                        if (!string.IsNullOrEmpty(RejectedtoApprove))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportRejectedToApprovedChangedLog").ToString(), RejectedtoApprove)
                            });

                        if (!string.IsNullOrEmpty(ApprovetoRejected))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportApprovedToRejectedChangedLog").ToString(), ApprovetoRejected)
                            });
                        if (!string.IsNullOrEmpty(RejectedtoReported))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportRejectedToReportedChangedLog").ToString(), RejectedtoReported)
                            });

                        if (!string.IsNullOrEmpty(ApprovetoReported))
                            TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportApprovedToReportedChangedLog").ToString(), ApprovetoReported)
                            });
                    }
                    #endregion

                    #region[rdixit][GEOS2-4301][04.08.2023]
                    //[rdixit][GEOS2-4301][04.08.2023]
                    if (TravelExpenseReport.IdCurrencyFrom != CurrFrom.IdCurrency)
                    {
                        string PrevFromCurr = "None";
                        if (CurrencyList.FirstOrDefault(i => i.IdCurrency == TravelExpenseReport.IdCurrencyFrom) != null)
                            PrevFromCurr = CurrencyList.FirstOrDefault(i => i.IdCurrency == TravelExpenseReport.IdCurrencyFrom).Name;
                        TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportFromCurruncyExchange").ToString(), PrevFromCurr, CurrFrom.Name)
                            });
                        TravelExpenseReport.IdCurrencyFrom = CurrFrom.IdCurrency;
                    }

                    if (TravelExpenseReport.IdCurrencyTo != CurrTo.IdCurrency)
                    {
                        string PrevToCurr = "None";
                        if (CurrencyList.FirstOrDefault(i => i.IdCurrency == TravelExpenseReport.IdCurrencyTo) != null)
                            PrevToCurr = CurrencyList.FirstOrDefault(i => i.IdCurrency == TravelExpenseReport.IdCurrencyTo).Name;
                        TravelExpenseReport.LogEntries.Add(
                            new LogEntriesByTravelExpense()
                            {
                                IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportToCurruncyExchange").ToString(), PrevToCurr, CurrTo.Name)
                            });
                        TravelExpenseReport.IdCurrencyTo = CurrTo.IdCurrency;
                    }
                    List<Expenses> UpdatedExpenseList = EmployeeExpenseByExpenseReport.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList();
                    if (UpdatedExpenseList != null)
                    {
                        foreach (Expenses item in UpdatedExpenseList)
                        {
                            float desval = (float)(Math.Round(((Math.Round((Convert.ToDouble(ExchangeRateDeviationValue) / 100), 2)) * item.OriginalConversionRate), 2) + item.OriginalConversionRate);
                            if (item.ExchangeRate > desval)
                                item.ExchangeRate = desval;
                        }
                    }
                    #endregion

                    #region start GEOS2-4848
                    //[pramod.misal][GEOS2-5079][27.11.2023]                
                    string prevLinkedTripTitle = "None";
                    if (SelectedLinkedTrip.IdEmployeeTrip != 0)
                    {
                        if (TravelExpenseReport.IdLinkedTrip != SelectedLinkedTrip.IdEmployeeTrip)
                        {
                            if (TravelExpenseReport.IdLinkedTrip != null && TravelExpenseReport.IdLinkedTrip != 0)
                                prevLinkedTripTitle = AllLinkedTripList.FirstOrDefault(i => i.IdEmployeeTrip == TravelExpenseReport.IdLinkedTrip).LinkedTripTitle;
                            TravelExpenseReport.LogEntries.Add(
                                new LogEntriesByTravelExpense()
                                {
                                    IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = DateTime.Now,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportLinkedTripsChangedLog").ToString(), prevLinkedTripTitle, SelectedLinkedTrip.LinkedTripTitle)
                                });

                            TravelExpenseReport.IdLinkedTrip = SelectedLinkedTrip.IdEmployeeTrip;
                            TravelExpenseReport.LinkedTripTitle = SelectedLinkedTrip.LinkedTripTitle;
                        }
                    }
                    else
                    {

                        if (SelectedLinkedTrip.LinkedTripTitle == "---")
                        {
                            prevLinkedTripTitle = TravelExpenseReport.LinkedTripTitle;
                            TravelExpenseReport.IdLinkedTrip = null;
                        }
                        else
                        {
                            prevLinkedTripTitle = AllLinkedTripList.FirstOrDefault(i => i.IdEmployeeTrip == TravelExpenseReport.IdLinkedTrip).LinkedTripTitle;
                        }

                        if (prevLinkedTripTitle != null)
                        {
                            TravelExpenseReport.LogEntries.Add(
                               new LogEntriesByTravelExpense()
                               {
                                   IdLogEntryType = 258,   //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                                   IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                   Datetime = DateTime.Now,
                                   UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                   Comments = string.Format(System.Windows.Application.Current.FindResource("ExpenseReportLinkedTripsChangedLog").ToString(), prevLinkedTripTitle, "None")
                               });
                        }


                    }

                    #region ExpenseTypes
                    //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
                    if (UpdateExpenseTypes != null)
                    {
                        try
                        {
                            foreach (Expenses updatedExpense in UpdateExpenseTypes)
                            {
                                if (EmployeeExpenseOriginalList != null)
                                {
                                    Expenses Expense = EmployeeExpenseOriginalList.FirstOrDefault(i => i.IdEmployeeExpense == updatedExpense.IdEmployeeExpense);
                                    Expense.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    LookupValue OldExpenseTypes = ExpenseTypesList.Where(ex => ex.IdLookupValue == Expense.IdExpenseType).FirstOrDefault();
                                    LookupValue ExpenseTypes = ExpenseTypesList.Where(ex => ex.IdLookupValue == updatedExpense.IdExpenseType).FirstOrDefault();
                                    UpdatedExpenseList.Add(updatedExpense);
                                    string log = string.Format(System.Windows.Application.Current.FindResource("EmployeeExpenseReportChangedLog").ToString(), OldExpenseTypes.Value, ExpenseTypes.Value);
                                    ExpensesLogList.Add(
                                       new LogEntriesByTravelExpense()
                                       {
                                           IdLogEntryType = 258,
                                           IdEmployeeExpense = Expense.IdEmployeeExpense,
                                           IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                           Datetime = DateTime.Now,
                                           UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                           Comments = string.Format(log)
                                       });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Method EditTravelExpenseAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                    }

                    #endregion
                    #region Service Comments
                    //IsUpdated = HrmService.UpdateTravelExpenses_V2340(TravelExpenseReport, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now);
                    //Service Method updated from UpdateTravelExpenses_V2350 to UpdateTravelExpenses_V2410 by [rdixit][GEOS2-4471][04.07.2023];

                    //Service UpdateTravelExpenses_V2410 updated with UpdateTravelExpenses_V2420 by [rdixit][GEOS2-4301][04.08.2023]
                    //IsUpdated = HrmService.UpdateTravelExpenses_V2420(TravelExpenseReport, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now, ExpensesLogList, UpdatedExpenseList);

                    //[pramod.misal][GEOS2-4848][23.11.2023]
                    //IsUpdated = HrmService.UpdateTravelExpenses_V2460(TravelExpenseReport, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now, ExpensesLogList, UpdatedExpenseList);
                    //IsUpdated = HrmService.UpdateTravelExpenses_V2470(TravelExpenseReport, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now, ExpensesLogList, UpdatedExpenseList); //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                    //[chitra.girigosavi][GEOS2-4824][03.11.2023]
                    //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024

                    //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
                    #endregion

                    if (TravelExpenseReport.IdWorkflowStatus == 42)
                    {
                        EmployeeExpenseStatus User = new EmployeeExpenseStatus();
                        UserInf = HrmService.GetRejectSendMail_V2540(TravelExpenseReport.IdEmployeeExpenseReport);
                    }
                    //[rdixit][GEOS2-6979][02.04.2025]
                    IsUpdated = HrmService.UpdateTravelExpenses_V2630(TravelExpenseReport, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now, ExpensesLogList, UpdatedExpenseList);

                    if (IsUpdated == true)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TravelExpenseUpdateMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(),                    
                            CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }

                    //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
                    if (TravelExpenseReport.IdWorkflowStatus == 42)
                    {
                        if (WorkflowTransition.IsNotificationRaised == 1)
                        {
                            EmployeeExpenseStatus user = UserInf.First();
                            SendMailMethod_V2540(user);
                        }
                    }
                    if (TravelExpenseReport.IdWorkflowStatus != 42)
                    {
                        if (IsUpdated == true && Status != Convert.ToString(TravelExpenseReport.Status))
                        {
                            if (WorkflowTransition != null)
                            {
                                if (WorkflowTransition.IsNotificationRaised == 1)
                                    SendMailMethod();
                            }
                        }
                    }
                    #endregion
                    #endregion
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method EditTravelExpenseAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method EditTravelExpenseAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            IsUpdated = true;
        }
        private void GetAllExpensesByExpenseReport(int IdEmployeeExpenseReport)//[GEOS2-3957][rdixit][07.10.2022]
        {
            try
            {
                ObservableCollection<LookupValue> TravelExpenseLookupValues = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(85));
                GeosApplication.Instance.Logger.Log("Method GetAllExpensesByExpenseReport()...", category: Category.Info, priority: Priority.Low);
                #region Service Comments
                //EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport(IdEmployeeExpenseReport));
                //Service Method updated from EmployeeExpenseByExpenseReport to EmployeeExpenseByExpenseReport_V2370 by [rdixit][09.03.2023][GEOS2-4239]
                //Service Method updated from EmployeeExpenseByExpenseReport_V2370 to EmployeeExpenseByExpenseReport_V2400 by [rdixit][09.03.2023][GEOS2-4438]
                //Service Method updated from EmployeeExpenseByExpenseReport_V2400 to EmployeeExpenseByExpenseReport_V2410 by [rdixit][03.07.2023][GEOS2-4439]
                //Service Method updated from EmployeeExpenseByExpenseReport_V2420 to EmployeeExpenseByExpenseReport_V2430 by [rdixit][06.09.2023][GEOS2-4798]
                //Service Method updated from EmployeeExpenseByExpenseReport_V2430 to EmployeeExpenseByExpenseReport_V2440 by [rdixit][12.10.2023][GEOS2-4721]
                //Service EmployeeExpenseByExpenseReport_V2440 with EmployeeExpenseByExpenseReport_V2450 by [rdixit][GEOS2-4721][10.11.2023]
                //EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2450(IdEmployeeExpenseReport, OriginalCurrFrom, OriginalCurrTo));
                //Service EmployeeExpenseByExpenseReport_V2480 with EmployeeExpenseByExpenseReport_V2510 by [rajashri][GEOS2-5514][26.04.2024]
                //[rdixit][17.07.2024][GEOS2-5767]
                #endregion
                OriginalCurrTo.CurrencyIconImage = null;
                OriginalCurrFrom.CurrencyIconImage = null;           
                EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2540(IdEmployeeExpenseReport, OriginalCurrFrom, OriginalCurrTo));
                EmployeeExpenseForWeeklyGrid = new ObservableCollection<Expenses>(EmployeeExpenseByExpenseReport.Select(i => (Expenses)i.Clone()).ToList());
                if (EmployeeExpenseByExpenseReport != null)
                    SelectedExpense = EmployeeExpenseByExpenseReport.FirstOrDefault();
                if (EmployeeExpenseByExpenseReport?.Count > 0)
                {
                    EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(EmployeeExpenseByExpenseReport.ToList().OrderBy(i => i.ExpenseDate).ThenBy(t => t.ExpenseTime));
                    //[rdixit][GEOS2-6979][02.04.2025]
                    EmployeeExpenseByExpenseReport.ToList().ForEach(i =>
                    {
                        i.ExpPosition = TravelExpenseLookupValues.FirstOrDefault(k => k.IdLookupValue == i.IdExpenseType).Position;
                        i.IsNotExistAsAttendee = i.IsNotExistAsAttendee && (GeosApplication.Instance.IsChangeAndAdminPermissionForHRM
                        || GeosApplication.Instance.IsHRMTravelManagerPermission);
                    });

                    EmployeeExpenseOriginalList = new List<Expenses>(EmployeeExpenseByExpenseReport.Select(i => (Expenses)i.Clone()));
                    DateTime MaxDate = EmployeeExpenseByExpenseReport.Select(i => i.ExpenseDate).Max();
                    DateTime MinDate = EmployeeExpenseByExpenseReport.Select(i => i.ExpenseDate).Min();
                    CultureInfo myCI = CultureInfo.CurrentCulture;
                    System.Globalization.Calendar myCal = myCI.Calendar;
                    CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                    int Number_of_Weeks = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(MaxDate, myCWR, myFirstDOW) - CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(MinDate, myCWR, myFirstDOW) + 1;
                    ExpenseCount = EmployeeExpenseByExpenseReport.Count;
                    CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures);
                    SelectedCulture = ci.ToList().Where(i => i.NumberFormat.CurrencySymbol == EmployeeExpenseByExpenseReport.First().CurSymbol).FirstOrDefault();
                    SubTotal = Math.Round((EmployeeExpenseByExpenseReport.Where(k => k.ExistAsAttendee != 1).Sum(i => i.AmtCal)), 2, MidpointRounding.AwayFromZero).ToString("N", CultureInfo.CurrentCulture) + EmployeeExpenseByExpenseReport.First().CurSymbol;
                    Tip = Math.Round((EmployeeExpenseByExpenseReport.Where(k => k.ExistAsAttendee != 1).Sum(i => i.Tipcal)), 2, MidpointRounding.AwayFromZero).ToString("N", CultureInfo.CurrentCulture) + EmployeeExpenseByExpenseReport.First().CurSymbol;
                    Total = Math.Round((EmployeeExpenseByExpenseReport.Where(k => k.ExistAsAttendee != 1).Sum(i => i.AmtCal) + EmployeeExpenseByExpenseReport.Sum(i => i.Tipcal)), 2, MidpointRounding.AwayFromZero).ToString("N", CultureInfo.CurrentCulture) + EmployeeExpenseByExpenseReport.First().CurSymbol;
                }
                GeosApplication.Instance.Logger.Log("Method GetAllExpensesByExpenseReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllExpensesByExpenseReport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllExpensesByExpenseReport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetAllExpensesByExpenseReport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
        }
        private void OpenLinkedImagesMethod(object obj)//[GEOS2-3957][rdixit][07.10.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenLinkedImagesMethod....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }


                var Empexpense = (Expenses)obj;
                int id = Empexpense.IdEmployeeExpense;
                EmployeeExpensePhotosViewModel EmployeeExpensePhotosViewModel = new EmployeeExpensePhotosViewModel();
                EmployeeExpensePhotosView EmployeeExpensePhotosView = new EmployeeExpensePhotosView();
                EmployeeExpensePhotosViewModel.EmployeeExpensePhotosHeader = System.Windows.Application.Current.FindResource("EmployeeExpensePhotosHeader").ToString();
                EventHandler handle1 = delegate { EmployeeExpensePhotosView.Close(); };
                EmployeeExpensePhotosViewModel.RequestClose += handle1;
                EmployeeExpensePhotosViewModel.GetImages(id);
                EmployeeExpensePhotosView.DataContext = EmployeeExpensePhotosViewModel;
                EmployeeExpensePhotosView.ShowDialogWindow();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenLinkedImagesMethod...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][GEOS2-8483][28.10.2025] WeekNº Expense Report - GHRM
        private void FillWeeklyExpenseList() //[GEOS2-3958][rdixit][05.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWeeklyExpenseList()...", category: Category.Info, priority: Priority.Low);
                if (SwitchCurrency.IdCurrency == SelectedCurrency.IdCurrency)
                {
                    #region Same Curr
                    string WeeklyExpensesHeader = string.Empty;

                    #region Week Dates Setting
                    DateTime MaxDate = EndDate;//[rdixit] [23 01 2023][GEOS2-4090]
                    DateTime MinDate = StartDate;//[rdixit] [23 01 2023][GEOS2-4090]
                    DateTime EmptyExpDate = StartDate;
                    DateTime LoopStartDate = StartDate;
                    int Day;
                    if (LoopStartDate.DayOfWeek.ToString().ToLower() == "sunday")
                        Day = LoopStartDate.DayOfWeek - DayOfWeek.Monday - 5;
                    else
                        Day = DayOfWeek.Monday - LoopStartDate.DayOfWeek;
                    //here you can set your Week Start Day
                    LoopStartDate = LoopStartDate.AddDays(Day);

                    CultureInfo myCI = CultureInfo.CurrentCulture;
                    System.Globalization.Calendar myCal = myCI.Calendar;
                    //[001] changed rule
                    myCI.DateTimeFormat.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
                    CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek myFirstDOW = DayOfWeek.Monday;
                    int minWeek = myCal.GetWeekOfYear(MinDate, myCWR, myFirstDOW);
                    int maxWeek = myCal.GetWeekOfYear(MaxDate, myCWR, myFirstDOW);
                    Number_of_Weeks = maxWeek - minWeek + 1;
                    //int Week_Count = 1;  //[rdixit] [23 01 2023][GEOS2-4090]
                    #endregion

                    WeeklyExpensesList = new ObservableCollection<WeeklyTravelExpenses>();

                    while (LoopStartDate <= EndDate)
                    {
                        int i = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(LoopStartDate, myCWR, myFirstDOW);
                        Header = "Week" + (i);
                        ObservableCollection<Expenses> SelectedWeekAllExpensesList = new ObservableCollection<Expenses>();
                        SelectedWeekAllExpensesList = new ObservableCollection<Expenses>(EmployeeExpenseForWeeklyGrid.Where(j => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(j.ExpenseDate, myCWR, myFirstDOW) == i));
                        if (!(SelectedWeekAllExpensesList.Count == 0))
                        {
                            WeekTravelExpenseListLocal = new List<WeekTravelExpenseList>();

                            //[rdixit] [23 01 2023][GEOS2-4090] To add Expense type as per position order
                            var Type = SelectedWeekAllExpensesList.OrderBy(k => k.ExpPosition).GroupBy(j => j.ExpenseType);
                            foreach (var item in Type)
                            {
                                List<Expenses> exp = new List<Expenses>();
                                exp.AddRange(item.ToList());
                                WeekTravelExpenseList WeekTravelExpense = new WeekTravelExpenseList();
                                //string ExpenseType = exp.FirstOrDefault().CategoryName;
                                WeekTravelExpense.CurSymbol = SwitchCurrency.Symbol;
                                WeekTravelExpense.ExpenseType = item.FirstOrDefault().ExpenseType;
                                WeekTravelExpense.MealExpense = MealExpenceAllowance.CurrencyConversion.ConvertedAmount;
                                WeekTravelExpense.Category = item.FirstOrDefault().CategoryName;
                                #region Add_Days_Expenses
                                //[rdixit][11.07.2023][GEOS2-4439]
                                #region Monday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()))
                                {
                                    //[GEOS2-4439][rdixit][03.07.2023][Added code for every day in week]
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.MonExpensescal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.MonExpensescal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.MonMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.MonIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.MonToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.MonToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.MonExpensescal = 00;
                                    //WeekTravelExpense.IsMonExchangeRateZero = true;
                                }
                                #endregion

                                #region Tuesday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.TueExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.TueExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.TueMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.TueIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.TueToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.TueToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.TueExpensesCal = 00;
                                    //WeekTravelExpense.IsTueExchangeRateZero=true;
                                }
                                #endregion

                                #region Wednesday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.WedExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.WedExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.WedMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.WedIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.WedToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.WedToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.WedExpensesCal = 00;
                                    //WeekTravelExpense.IsWedExchangeRateZero = true;
                                }
                                #endregion

                                #region Thursday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.ThuExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.ThuExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.ThuMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.ThuIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.ThuToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.ThuToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.ThuExpensesCal = 00;
                                    //WeekTravelExpense.IsThuExchangeRateZero = true;
                                }
                                #endregion

                                #region Friday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.FriExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.FriExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.FriMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.FriIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.FriToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.FriToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.FriExpensesCal = 00;
                                    //WeekTravelExpense.IsFriExchangeRateZero = true;
                                }
                                #endregion

                                #region Saturday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.SatExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.SatExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.SatMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.SatIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.SatToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.SatToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.SatExpensesCal = 00;
                                    //WeekTravelExpense.IsSatExchangeRateZero = true;
                                }
                                #endregion

                                #region Sunday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()))
                                {
                                    //if (ExpenseType.ToLower() == "meal")
                                    //    WeekTravelExpense.SunExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Sum(k => k.AmtCal + k.Tipcal), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    WeekTravelExpense.SunExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Sum(k => k.AvgExpense), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.SunMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Sum(k => k.AvgmealExpense), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.SunIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.SunToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.SunToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                    //WeekTravelExpense.SunExpensesCal = 00;
                                    //WeekTravelExpense.IsSunExchangeRateZero = true;
                                }
                                //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
                                //if (WeekTravelExpense.IsMonExchangeRateZero == true ||
                                //    WeekTravelExpense.IsTueExchangeRateZero == true ||
                                //    WeekTravelExpense.IsWedExchangeRateZero == true ||
                                //    WeekTravelExpense.IsThuExchangeRateZero == true ||
                                //    WeekTravelExpense.IsFriExchangeRateZero == true ||
                                //    WeekTravelExpense.IsSatExchangeRateZero == true ||
                                //    WeekTravelExpense.IsSunExchangeRateZero == true)
                                //{
                                //    IsAnyExchangeRateZero = Visibility.Visible;
                                //}
                                if (SwitchCurrency.IdCurrency == SelectedCurrency.IdCurrency)
                                {
                                    IsAnyExchangeRateZero = Visibility.Hidden;
                                }
                                    #endregion

                                    WeekTravelExpense.WeekTotalExpensesCal = Math.Round(WeekTravelExpense.MonExpensescal + WeekTravelExpense.TueExpensesCal + WeekTravelExpense.WedExpensesCal + WeekTravelExpense.ThuExpensesCal
                                                                     + WeekTravelExpense.FriExpensesCal + WeekTravelExpense.SatExpensesCal + WeekTravelExpense.SunExpensesCal, 2, MidpointRounding.AwayFromZero);

                                #endregion

                                #region To display
                                WeekTravelExpense.MonExpenses = WeekTravelExpense.MonExpensescal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.TueExpenses = WeekTravelExpense.TueExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.WedExpenses = WeekTravelExpense.WedExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.ThuExpenses = WeekTravelExpense.ThuExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.FriExpenses = WeekTravelExpense.FriExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.SatExpenses = WeekTravelExpense.SatExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.SunExpenses = WeekTravelExpense.SunExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.WeekTotalExpenses = WeekTravelExpense.WeekTotalExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                #endregion

                                WeekTravelExpenseList Week_Dates = new WeekTravelExpenseList();
                                if (SelectedWeekAllExpensesList.Any(j => j.ExpenseDate != null) && WeekTravelExpenseListLocal.Count == 0)
                                {
                                    DateTime weekdate = SelectedWeekAllExpensesList.Where(j => j.ExpenseDate != null).Select(k => k.ExpenseDate).FirstOrDefault();
                                    int Days;
                                    if (weekdate.DayOfWeek.ToString().ToLower() == "sunday")
                                        Days = weekdate.DayOfWeek - DayOfWeek.Monday - 5;
                                    else
                                        Days = DayOfWeek.Monday - weekdate.DayOfWeek;
                                    //here you can set your Week Start Day
                                    DateTime WeekStartDate = weekdate.AddDays(Days);
                                    Week_Dates.MonExpenses = WeekStartDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.TueExpenses = WeekStartDate.AddDays(1).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.WedExpenses = WeekStartDate.AddDays(2).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.ThuExpenses = WeekStartDate.AddDays(3).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.FriExpenses = WeekStartDate.AddDays(4).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.SatExpenses = WeekStartDate.AddDays(5).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.SunExpenses = WeekStartDate.AddDays(6).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

                                    if (WeekTravelExpenseListLocal.Count == 0)
                                        WeekTravelExpenseListLocal.Insert(0, Week_Dates);

                                }
                                WeekTravelExpenseListLocal.Add(WeekTravelExpense);
                            }

                            WeeklyTravelExpenses WeeklyTravelExp = new WeeklyTravelExpenses();
                            WeeklyTravelExp.Header = Header;
                            WeeklyTravelExp.WeekTravelExpenseList = new ObservableCollection<WeekTravelExpenseList>(WeekTravelExpenseListLocal);
                            WeeklyTravelExp.WeekTravelExpenseList.RemoveAt(0);
                            WeeklyTravelExp.MonDate = WeekTravelExpenseListLocal.FirstOrDefault().MonExpenses;
                            WeeklyTravelExp.TueDate = WeekTravelExpenseListLocal.FirstOrDefault().TueExpenses;
                            WeeklyTravelExp.WedDate = WeekTravelExpenseListLocal.FirstOrDefault().WedExpenses;
                            WeeklyTravelExp.ThuDate = WeekTravelExpenseListLocal.FirstOrDefault().ThuExpenses;
                            WeeklyTravelExp.FriDate = WeekTravelExpenseListLocal.FirstOrDefault().FriExpenses;
                            WeeklyTravelExp.SatDate = WeekTravelExpenseListLocal.FirstOrDefault().SatExpenses;
                            WeeklyTravelExp.SunDate = WeekTravelExpenseListLocal.FirstOrDefault().SunExpenses;
                            WeeklyExpensesList.Add(WeeklyTravelExp);

                        }
                        else //[rdixit] [23 01 2023][GEOS2-4090]
                        {
                            //To Add weeks included in the trip period but not contain expenses
                            WeeklyTravelExpenses WeeklyTravelExp = new WeeklyTravelExpenses();
                            WeekTravelExpenseList Week_Dates = new WeekTravelExpenseList();
                            WeeklyTravelExp.Header = Header;
                            DateTime weekdate = EmptyExpDate;
                            int Days;
                            if (weekdate.DayOfWeek.ToString().ToLower() == "sunday")
                                Days = weekdate.DayOfWeek - DayOfWeek.Monday - 5;
                            else
                                Days = DayOfWeek.Monday - weekdate.DayOfWeek;
                            //here you can set your Week Start Day
                            DateTime WeekStartDate = weekdate.AddDays(Days);
                            WeeklyTravelExp.MonDate = WeekStartDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.TueDate = WeekStartDate.AddDays(1).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.WedDate = WeekStartDate.AddDays(2).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.ThuDate = WeekStartDate.AddDays(3).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.FriDate = WeekStartDate.AddDays(4).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.SatDate = WeekStartDate.AddDays(5).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.SunDate = WeekStartDate.AddDays(6).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyExpensesList.Add(WeeklyTravelExp);
                        }
                        SelectedWeeklyExpense = WeeklyExpensesList.FirstOrDefault(); //[rdixit] [23 01 2023][GEOS2-4090]
                        EmptyExpDate = EmptyExpDate.AddDays(7); //[rdixit] [23 01 2023][GEOS2-4090]
                        LoopStartDate = LoopStartDate.AddDays(7);
                    }
                    #endregion
                }
                else
                {
                    #region Diff Curr
                    string WeeklyExpensesHeader = string.Empty;

                    #region Week Dates Setting
                    DateTime MaxDate = EndDate;//[rdixit] [23 01 2023][GEOS2-4090]
                    DateTime MinDate = StartDate;//[rdixit] [23 01 2023][GEOS2-4090]
                    DateTime EmptyExpDate = StartDate;
                    DateTime LoopStartDate = StartDate;
                    int Day;
                    if (LoopStartDate.DayOfWeek.ToString().ToLower() == "sunday")
                        Day = LoopStartDate.DayOfWeek - DayOfWeek.Monday - 5;
                    else
                        Day = DayOfWeek.Monday - LoopStartDate.DayOfWeek;
                    //here you can set your Week Start Day
                    LoopStartDate = LoopStartDate.AddDays(Day);

                    CultureInfo myCI = CultureInfo.CurrentCulture;
                    System.Globalization.Calendar myCal = myCI.Calendar;
                    myCI.DateTimeFormat.CalendarWeekRule = CalendarWeekRule.FirstFullWeek;
                    CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek myFirstDOW = DayOfWeek.Monday;
                    int minWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(MinDate, myCWR, myFirstDOW);
                    int maxWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(MaxDate, myCWR, myFirstDOW);
                    Number_of_Weeks = maxWeek - minWeek + 1;
                    //int Week_Count = 1;  //[rdixit] [23 01 2023][GEOS2-4090]
                    #endregion

                    WeeklyExpensesList = new ObservableCollection<WeeklyTravelExpenses>();

                    while (LoopStartDate <= EndDate)
                    {
                        int i = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(LoopStartDate, myCWR, myFirstDOW);
                        Header = "Week" + (i);
                        ObservableCollection<Expenses> SelectedWeekAllExpensesList = new ObservableCollection<Expenses>();
                        SelectedWeekAllExpensesList = new ObservableCollection<Expenses>(EmployeeExpenseForWeeklyGrid.Where(j => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(j.ExpenseDate, myCWR, myFirstDOW) == i));
                   
                        if (!(SelectedWeekAllExpensesList.Count == 0))
                        {
                            WeekTravelExpenseListLocal = new List<WeekTravelExpenseList>();

                            var Type = SelectedWeekAllExpensesList.OrderBy(k => k.ExpPosition).GroupBy(j => j.ExpenseType);
                            foreach (var item in Type)
                            {
                                List<Expenses> exp = new List<Expenses>();
                                exp.AddRange(item.ToList());
                                WeekTravelExpenseList WeekTravelExpense = new WeekTravelExpenseList();

                                WeekTravelExpense.CurSymbol = SwitchCurrency.Symbol;
                                WeekTravelExpense.ExpenseType = item.FirstOrDefault().ExpenseType;
                                WeekTravelExpense.MealExpense = MealExpenceAllowance.CurrencyConversion.ConvertedAmount;
                                WeekTravelExpense.Category = item.FirstOrDefault().CategoryName;
                                #region Add_Days_Expenses

                                #region Monday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()))
                                {
                                    WeekTravelExpense.MonExpensescal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.MonMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.MonIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Monday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.MonToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.MonToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                    if (WeekTravelExpense.MonExpensescal==0)
                                    {
                                        WeekTravelExpense.IsMonExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.MonExpensescal = 00;
                                }
                                #endregion

                                #region Tuesday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()))
                                {
                                    WeekTravelExpense.TueExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.TueMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.TueIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Tuesday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.TueToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.TueToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }
                                    if (WeekTravelExpense.TueExpensesCal == 0)
                                    {
                                        WeekTravelExpense.IsTueExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.TueExpensesCal = 00;
                                }
                                #endregion

                                #region Wednesday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()))
                                {
                                    WeekTravelExpense.WedExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.WedMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.WedIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Wednesday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.WedToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.WedToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }

                                    if (WeekTravelExpense.WedExpensesCal==0)
                                    {
                                        WeekTravelExpense.IsWedExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.WedExpensesCal = 00;
                                }
                                #endregion

                                #region Thursday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()))
                                {
                                    WeekTravelExpense.ThuExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.ThuMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.ThuIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Thursday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.ThuToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.ThuToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }

                                    if (WeekTravelExpense.ThuExpensesCal==0)
                                    {
                                        WeekTravelExpense.IsThuExchangeRateZero = true;
                                    }

                                }
                                else
                                {
                                    WeekTravelExpense.ThuExpensesCal = 00;
                                }
                                #endregion

                                #region Friday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()))
                                {
                                    WeekTravelExpense.FriExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.FriMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.FriIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Friday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.FriToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.FriToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }

                                    if (WeekTravelExpense.FriExpensesCal==0)
                                    {
                                        WeekTravelExpense.IsFriExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.FriExpensesCal = 00;
                                }
                                #endregion

                                #region Saturday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()))
                                {
                                    WeekTravelExpense.SatExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.SatMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.SatIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Saturday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.SatToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.SatToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }

                                    }
                                    if (WeekTravelExpense.SatExpensesCal==0)
                                    {
                                        WeekTravelExpense.IsSatExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.SatExpensesCal = 00;
                                }
                                #endregion

                                #region Sunday
                                if (exp.Any(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()))
                                {
                                    WeekTravelExpense.SunExpensesCal = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Sum(k => k.AvgExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    WeekTravelExpense.SunMealExpenses = Math.Round(exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Sum(k => k.AvgmealExpense * k.StartDateExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString() && j.ExpenseAttendees != null && j.ExpenseAttendees.Count > 1).ToList().Count > 0)
                                    {
                                        WeekTravelExpense.SunIsAttendee = Visibility.Visible;
                                        if (exp.Where(j => j.ExpenseDate.DayOfWeek.ToString() == WeekDays.Sunday.ToString()).Any(a => a.ExistAsAttendee == 1))
                                        {
                                            WeekTravelExpense.SunToolTip = System.Windows.Application.Current.FindResource("ExistAsAttendeeInfoIconToolTip").ToString();
                                        }
                                        else
                                        {
                                            WeekTravelExpense.SunToolTip = System.Windows.Application.Current.FindResource("ExistAsReporterInfoIconToolTip").ToString();
                                        }
                                    }

                                    if (WeekTravelExpense.SunExpensesCal==0)
                                    {
                                        WeekTravelExpense.IsSunExchangeRateZero = true;
                                    }
                                }
                                else
                                {
                                    WeekTravelExpense.SunExpensesCal = 00;
                                }

                                if (WeekTravelExpense.IsMonExchangeRateZero == true ||
                                  WeekTravelExpense.IsTueExchangeRateZero == true ||
                                  WeekTravelExpense.IsWedExchangeRateZero == true ||
                                  WeekTravelExpense.IsThuExchangeRateZero == true ||
                                  WeekTravelExpense.IsFriExchangeRateZero == true ||
                                  WeekTravelExpense.IsSatExchangeRateZero == true ||
                                  WeekTravelExpense.IsSunExchangeRateZero == true)
                                {
                                    IsAnyExchangeRateZero = Visibility.Visible;
                                }
                                #endregion

                                WeekTravelExpense.WeekTotalExpensesCal = Math.Round(WeekTravelExpense.MonExpensescal + WeekTravelExpense.TueExpensesCal + WeekTravelExpense.WedExpensesCal + WeekTravelExpense.ThuExpensesCal
                                                                     + WeekTravelExpense.FriExpensesCal + WeekTravelExpense.SatExpensesCal + WeekTravelExpense.SunExpensesCal, 2, MidpointRounding.AwayFromZero);

                                #endregion

                                #region To display
                                WeekTravelExpense.MonExpenses = WeekTravelExpense.MonExpensescal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.TueExpenses = WeekTravelExpense.TueExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.WedExpenses = WeekTravelExpense.WedExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.ThuExpenses = WeekTravelExpense.ThuExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.FriExpenses = WeekTravelExpense.FriExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.SatExpenses = WeekTravelExpense.SatExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.SunExpenses = WeekTravelExpense.SunExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                WeekTravelExpense.WeekTotalExpenses = WeekTravelExpense.WeekTotalExpensesCal.ToString("n", CultureInfo.CurrentCulture);
                                #endregion


                                WeekTravelExpenseList Week_Dates = new WeekTravelExpenseList();
                                if (SelectedWeekAllExpensesList.Any(j => j.ExpenseDate != null) && WeekTravelExpenseListLocal.Count == 0)
                                {
                                    DateTime weekdate = SelectedWeekAllExpensesList.Where(j => j.ExpenseDate != null).Select(k => k.ExpenseDate).FirstOrDefault();
                                    int Days;
                                    if (weekdate.DayOfWeek.ToString().ToLower() == "sunday")
                                        Days = weekdate.DayOfWeek - DayOfWeek.Monday - 5;
                                    else
                                        Days = DayOfWeek.Monday - weekdate.DayOfWeek;
                                    //here you can set your Week Start Day
                                    DateTime WeekStartDate = weekdate.AddDays(Days);
                                    Week_Dates.MonExpenses = WeekStartDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.TueExpenses = WeekStartDate.AddDays(1).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.WedExpenses = WeekStartDate.AddDays(2).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.ThuExpenses = WeekStartDate.AddDays(3).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.FriExpenses = WeekStartDate.AddDays(4).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.SatExpenses = WeekStartDate.AddDays(5).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                                    Week_Dates.SunExpenses = WeekStartDate.AddDays(6).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

                                    if (WeekTravelExpenseListLocal.Count == 0)
                                        WeekTravelExpenseListLocal.Insert(0, Week_Dates);

                                }
                                WeekTravelExpenseListLocal.Add(WeekTravelExpense);
                            }
                            WeeklyTravelExpenses WeeklyTravelExp = new WeeklyTravelExpenses();
                            WeeklyTravelExp.Header = Header;
                            WeeklyTravelExp.WeekTravelExpenseList = new ObservableCollection<WeekTravelExpenseList>(WeekTravelExpenseListLocal);
                            WeeklyTravelExp.WeekTravelExpenseList.RemoveAt(0);
                            WeeklyTravelExp.MonDate = WeekTravelExpenseListLocal.FirstOrDefault().MonExpenses;
                            WeeklyTravelExp.TueDate = WeekTravelExpenseListLocal.FirstOrDefault().TueExpenses;
                            WeeklyTravelExp.WedDate = WeekTravelExpenseListLocal.FirstOrDefault().WedExpenses;
                            WeeklyTravelExp.ThuDate = WeekTravelExpenseListLocal.FirstOrDefault().ThuExpenses;
                            WeeklyTravelExp.FriDate = WeekTravelExpenseListLocal.FirstOrDefault().FriExpenses;
                            WeeklyTravelExp.SatDate = WeekTravelExpenseListLocal.FirstOrDefault().SatExpenses;
                            WeeklyTravelExp.SunDate = WeekTravelExpenseListLocal.FirstOrDefault().SunExpenses;
                            WeeklyExpensesList.Add(WeeklyTravelExp);
                           
                        }
                        else
                        {
                            WeeklyTravelExpenses WeeklyTravelExp = new WeeklyTravelExpenses();
                            WeekTravelExpenseList Week_Dates = new WeekTravelExpenseList();
                            WeeklyTravelExp.Header = Header;
                            DateTime weekdate = EmptyExpDate;
                            int Days;
                            if (weekdate.DayOfWeek.ToString().ToLower() == "sunday")
                                Days = weekdate.DayOfWeek - DayOfWeek.Monday - 5;
                            else
                                Days = DayOfWeek.Monday - weekdate.DayOfWeek;
                            //here you can set your Week Start Day
                            DateTime WeekStartDate = weekdate.AddDays(Days);
                            WeeklyTravelExp.MonDate = WeekStartDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.TueDate = WeekStartDate.AddDays(1).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.WedDate = WeekStartDate.AddDays(2).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.ThuDate = WeekStartDate.AddDays(3).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.FriDate = WeekStartDate.AddDays(4).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.SatDate = WeekStartDate.AddDays(5).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyTravelExp.SunDate = WeekStartDate.AddDays(6).Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            WeeklyExpensesList.Add(WeeklyTravelExp);
                        }
                       
                        SelectedWeeklyExpense = WeeklyExpensesList.FirstOrDefault();
                        EmptyExpDate = EmptyExpDate.AddDays(7);
                        LoopStartDate = LoopStartDate.AddDays(7);
                    }
                    #endregion
                }

                GeosApplication.Instance.Logger.Log("Method FillWeeklyExpenseList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWeeklyExpenseList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void FillExpenseReason()//[rdixit][GEOS2-4022][24.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExpenseReason()...", category: Category.Info, priority: Priority.Low);

                if (TravelExpenseReasonList == null)
                {
                    TravelExpenseReasonList = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(104));
                }
                TravelExpenseReasonList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseReason()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseReason()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseReason()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void AddInit()//[rdixit][GEOS2-4025][24.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddInit()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = MaximizedElementPosition.Right;
                GetCurrencies();
                GetCompanies();
                FillExpenseReason();
                IsNew = true;
                IsActive = true;
                IsEmployeeSelectVisible = Visibility.Visible;
                IsEmployeeTextBlockVisible = Visibility.Hidden;
                Code = HrmService.GetTravelExpenseReportLatestCode();
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
                Duration = Convert.ToInt32((EndDate - StartDate).Days) + 1;
                GivenAmount = 0;
                WorkflowTransitionList = new List<TravelExpenseWorkflowTransitions>(HrmService.GetAllWorkflowTransitions());
                TravelExpenseStatusList = new List<TravelExpenseStatus>(HrmService.GetAllTravelExpenseStatus());
                TravelExpenseStatus = new TravelExpenseStatus();
                TravelExpenseStatus = TravelExpenseStatusList.Where(a => a.IdWorkflowStatus == 21).FirstOrDefault();
                CurrTo = (Currency)SelectedCurrency.Clone();
                CurrFrom = CurrencyList.FirstOrDefault(i => i.IdCurrency == SelectedCompanyList.IdCurrency);
                //Activestatus = true;
                //[pramod.misal][GEOS2-4848][08-12-2023]
                EmployeeTrips defaultValue = new EmployeeTrips { LinkedTripTitle = "---", IdEmployeeTrip = 0 };
                LinkedTriplList = new ObservableCollection<EmployeeTrips>();
                LinkedTriplList.Insert(0, defaultValue);
                SelectedLinkedTrip = LinkedTriplList.FirstOrDefault();
                FullName = SelectedEmployee.FullName;
                FillExpenseGrid();

                CommentsList = new ObservableCollection<LogEntriesByTravelExpense>(HrmService.GetTravelExpenseComments_V2470(TravelExpenseReport.IdEmployeeExpenseReport));
                if (commentsList.Count > 0)
                {
                    //CommentText = commentsList[0].Comments; // Get the first comment
                    var latestComment = CommentsList.OrderByDescending(c => c.Datetime).First();
                    CommentText = latestComment.Comments;
                    CommentDatetimeText = latestComment.Datetime;
                }
                else
                {
                    CommentText = string.Empty; // or some default value if there are no comments
                }
                foreach (var item in CommentsList)
                {
                    SetUserProfileImage(item);
                }

                #region  [GEOS2-4848]               
                //if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                //{
                //    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                //    LinkedTriplList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdCompany_V2440(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                //    LinkedTriplList.ToList().ForEach(i => i.LinkedTripTitle = $"[{i.ArrivalDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} - {i.DepartureDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}] – {i.Title}");
                //    LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTriplList.OrderByDescending(expense => expense.ArrivalDate).ToList());
                //    EmployeeTrips defaultValue = new EmployeeTrips { LinkedTripTitle = "---", IdEmployeeTrip = 0 };
                //    LinkedTriplList.Insert(0, defaultValue);
                //    SelectedLinkedTrip = LinkedTriplList.FirstOrDefault();
                //}

                #endregion GEOS2-4848


                GeosApplication.Instance.Logger.Log("Method AddInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddInit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetEmployeesByIdComp(Company company)//[rdixit][GEOS2-4025][24.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetEmployeesByIdCompany()...", category: Category.Info, priority: Priority.Low);

                //EmployeesList = new ObservableCollection<Employee>(HrmService.GetEmployeesByIdSite(company.IdCompany));
                #region rajashri [GEOS2-4997][30-11-2023]
                EmployeesList = new ObservableCollection<Employee>(HrmService.GetEmployeesByIdSite_V2460(company.IdCompany));
                if (IsActive)
                {
                    var ActiveEmployes = EmployeesList.Where(e => e.IdEmployeeStatus == 136).ToList(); ActiveEmployes.ForEach(e => e.IsActive = true);
                    EmployeesList = new ObservableCollection<Employee>(ActiveEmployes);
                    EmployeesList.Insert(0, new Employee() { IdEmployee = 0, DisplayName = "---", IsActive = true });
                    SelectedEmployee = EmployeesList.FirstOrDefault();
                }
                else
                {
                    bool isexist = EmployeesList.Any(i => i.IdEmployee == TravelExpenseReport.IdEmployee);
                    if (!isexist)
                    {
                        EmployeesList.Add(new Employee
                        {
                            IdEmployee = TravelExpenseReport.IdEmployee,
                            DisplayName = TravelExpenseReport.Reporter,
                            IdEmployeeStatus = 137
                        });
                    }
                    EmployeesList.Where(e => e.IdEmployeeStatus == 136).ToList().ForEach(e => e.IsActive = true);

                    EmployeesList.Where(e => e.IdEmployeeStatus == 137 || e.IdEmployeeStatus == 138).ToList().ForEach(e => e.IsActive = false);
                    EmployeesList.Insert(0, new Employee() { IdEmployee = 0, DisplayName = "---", IsActive = true });
                    SelectedEmployee = EmployeesList.FirstOrDefault(e => e.IdEmployee == TravelExpenseReport.IdEmployee);
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method GetEmployeesByIdCompany()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetEmployeesByIdCompany() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetEmployeesByIdCompany() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetEmployeesByIdCompany() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)//[rdixit][GEOS2-4025][24.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                if (error != null)
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void WeeklyExpenseTableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            SelectedWeeklyExpense = WeeklyExpensesList.FirstOrDefault();
            GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
            List<DevExpress.Xpf.Grid.GridSummaryItem> summuries = new List<DevExpress.Xpf.Grid.GridSummaryItem>(gridControl.TotalSummary);

            Style RedForColorStyle = new Style() { TargetType = typeof(Run) };
            RedForColorStyle.Setters.Add(new Setter() { Property = Run.ForegroundProperty, Value = System.Windows.Media.Brushes.Red });
            double ConvertedAmount = 0;
            Style GreenForColorStyle = new Style() { TargetType = typeof(Run) };
            GreenForColorStyle.Setters.Add(new Setter() { Property = Run.ForegroundProperty, Value = System.Windows.Media.Brushes.Green });

            ObservableCollection<WeekTravelExpenseList> weeklyExpensesListTemp = (ObservableCollection<WeekTravelExpenseList>)gridControl.ItemsSource;
            //[rdixit][GEOS2-4514][08.06.2023]
            #region WeekMealAllowanceTotal
            double mealAllowanceTotal = 0;
            if (MealExpenceAllowance.CurrencyConversion != null)
                ConvertedAmount = MealExpenceAllowance.CurrencyConversion.ConvertedAmount;
            if (MealSummeryFlag == 0)
            {
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.SunDate) && Convert.ToDateTime(SelectedWeeklyExpense.SunDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.MonDate) && Convert.ToDateTime(SelectedWeeklyExpense.MonDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.TueDate) && Convert.ToDateTime(SelectedWeeklyExpense.TueDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.WedDate) && Convert.ToDateTime(SelectedWeeklyExpense.WedDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.ThuDate) && Convert.ToDateTime(SelectedWeeklyExpense.ThuDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.FriDate) && Convert.ToDateTime(SelectedWeeklyExpense.FriDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= Convert.ToDateTime(SelectedWeeklyExpense.SatDate) && Convert.ToDateTime(SelectedWeeklyExpense.SatDate) <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }

            }
            else
            {
                int j = MealSummeryFlag * 7;
                DateTime d1 = Convert.ToDateTime(SelectedWeeklyExpense.SunDate).AddDays(j);
                DateTime d2 = Convert.ToDateTime(SelectedWeeklyExpense.MonDate).AddDays(j);
                DateTime d3 = Convert.ToDateTime(SelectedWeeklyExpense.TueDate).AddDays(j);
                DateTime d4 = Convert.ToDateTime(SelectedWeeklyExpense.WedDate).AddDays(j);
                DateTime d5 = Convert.ToDateTime(SelectedWeeklyExpense.ThuDate).AddDays(j);
                DateTime d6 = Convert.ToDateTime(SelectedWeeklyExpense.FriDate).AddDays(j);
                DateTime d7 = Convert.ToDateTime(SelectedWeeklyExpense.SatDate).AddDays(j);
                if (StartDate <= d1 && d1 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d2 && d2 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d3 && d3 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d4 && d4 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d5 && d5 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d6 && d6 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }
                if (StartDate <= d7 && d7 <= EndDate) { mealAllowanceTotal = Math.Round((mealAllowanceTotal + ConvertedAmount), 2); }

            }
            #endregion
            if (weeklyExpensesListTemp != null)
            {

                List<WeekTravelExpenseList> weeklyExpensesListTemp1 = weeklyExpensesListTemp.Where(i => i.Category.ToLower() == "meal").ToList();
                if (weeklyExpensesListTemp1 != null)
                {
                    #region calculate Week Days Meal Exp
                    double MonMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.MonMealExpenses), 2);
                    double TueMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.TueMealExpenses), 2);
                    double WedMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.WedMealExpenses), 2);
                    double ThuMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.ThuMealExpenses), 2);
                    double FriMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.FriMealExpenses), 2);
                    double SatMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.SatMealExpenses), 2);
                    double SunMealExp = Math.Round(weeklyExpensesListTemp1.Sum(j => j.SunMealExpenses), 2);
                    //double mealAllowanceTotal = Math.Round((7 * MealExpenceAllowance.Amount), 2);
                    double mealtotal = Math.Round((MonMealExp + TueMealExp + WedMealExp + ThuMealExp + FriMealExp + SatMealExp + SunMealExp), 2);
                    #endregion

                    #region Assign to summary
                    summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().DisplayFormat = " {0}" + MonMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().DisplayFormat = " {0}" + TueMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().DisplayFormat = " {0}" + WedMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().DisplayFormat = " {0}" + ThuMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().DisplayFormat = " {0}" + FriMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().DisplayFormat = " {0}" + SatMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().DisplayFormat = " {0}" + SunMealExp.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;

                    summuries.Where(i => i.FieldName == "TotalExpenses1").FirstOrDefault().DisplayFormat = " {0}" + mealAllowanceTotal.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;
                    summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().DisplayFormat = " {0}" + mealtotal.ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol;

                    summuries.Where(t => t.FieldName == "MonExpenses1" || t.FieldName == "TueExpenses1" || t.FieldName == "WedExpenses1" || t.FieldName == "ThuExpenses1" || t.FieldName == "FriExpenses1"
                    || t.FieldName == "SatExpenses1" || t.FieldName == "SunExpenses1").ToList().ForEach(j => j.DisplayFormat = " {0}" + Math.Round(MealExpenceAllowance.CurrencyConversion.ConvertedAmount, 2).ToString("n", CultureInfo.CurrentCulture) + SwitchCurrency.Symbol);
                    #endregion

                    #region Apply Style
                    if (mealtotal > mealAllowanceTotal)
                        summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if(mealtotal > 0)
                        summuries.Where(i => i.FieldName == "TotalExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (MonMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (MonMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && MonMealExp > 0)
                        summuries.Where(i => i.FieldName == "MonExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;
                    //[pramod.misal][10-06-2025][GEOS2-8024]
                    if (TueMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (TueMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && TueMealExp > 0)
                        summuries.Where(i => i.FieldName == "TueExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (WedMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (WedMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && WedMealExp > 0)
                        summuries.Where(i => i.FieldName == "WedExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (ThuMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (ThuMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && ThuMealExp > 0)
                        summuries.Where(i => i.FieldName == "ThuExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (FriMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (FriMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && FriMealExp > 0)
                        summuries.Where(i => i.FieldName == "FriExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (SatMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (SatMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && SatMealExp > 0)
                        summuries.Where(i => i.FieldName == "SatExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;

                    if (SunMealExp > MealExpenceAllowance.CurrencyConversion.ConvertedAmount)
                        summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().TotalSummaryElementStyle = RedForColorStyle;
                    else if (SunMealExp < MealExpenceAllowance.CurrencyConversion.ConvertedAmount && SunMealExp > 0)
                        summuries.Where(i => i.FieldName == "SunExpenses2").FirstOrDefault().TotalSummaryElementStyle = GreenForColorStyle;
                    #endregion
                }
            }
            MealSummeryFlag++;
        }//[rdixit][GEOS2-4178][15.03.2023]      
        private void DeclineStatusClickEvent()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeclineStatusClickEvent....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                EditTravelExpenseCommentViewModel CommentViewModel = new EditTravelExpenseCommentViewModel();
                EditTravelExpenseCommentView CommentView = new EditTravelExpenseCommentView();
                EventHandler handle1 = delegate { CommentView.Close(); };
                CommentViewModel.RequestClose += handle1;
                CommentViewModel.WindowHeader = System.Windows.Application.Current.FindResource("ExpenseReportCommentTitle").ToString();
                CommentView.DataContext = CommentViewModel;
                CommentView.ShowDialogWindow();

                if (CommentViewModel.IsUpdated == true)
                    ExpenseReportComment = CommentViewModel.Comment;
                else
                    ExpenseReportComment = string.Empty;

                //[chitra.girigosavi][GEOS2-4824][26.12.2023]
                if (CommentViewModel.SelectedComment != null)
                {

                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<LogEntriesByTravelExpense>();

                    CommentViewModel.SelectedComment.IdEmployeeExpenseReport = Travels.IdEmployeeExpenseReport;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<LogEntriesByTravelExpense>();

                    AddCommentsList.Add(new LogEntriesByTravelExpense()
                    {
                        IdUser = CommentViewModel.SelectedComment.IdUser,
                        IdEmployeeExpenseReport = CommentViewModel.SelectedComment.IdEmployeeExpenseReport,
                        Comments = CommentViewModel.SelectedComment.Comments,
                        IsRtfText = CommentViewModel.SelectedComment.IsRtfText
                    });
                    CommentsList.Insert(0, CommentViewModel.SelectedComment);
                    SelectedComment = CommentViewModel.SelectedComment;
                    //CommentText = SelectedComment.Comments;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method DeclineStatusClickEvent...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }  //[rdixit][GEOS2-4016][23.03.2023]        
        public void SendMailMethod()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("EditTravelExpenseViewModel Method SendMailMethod()....", category: Category.Info, priority: Priority.Low);
                bool IsEmailSend = HrmService.GetEmployeeExpenseReportTemplate(code, SelectedEmployee, TravelExpenseStatus, ExpenseReportComment, GeosApplication.Instance.ActiveUser.CompanyEmail);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailMethod() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailMethod() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SendMailMethod() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }//[rdixit][GEOS2-4017][24.03.2023]
        private void CustomShowFilterPopupAction(DevExpress.Xpf.Grid.FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                List<object> filterItems = new List<object>();
                if (e.Column.FieldName != "AttendiesCount")
                {
                    return;
                }
                #region Currency
                if (e.Column.FieldName == "AttendiesCount")
                {
                    foreach (var dataObject in EmployeeExpenseByExpenseReport)
                    {
                        if (dataObject.AttendiesCount == null)
                        {
                            continue;
                        }
                        else if (dataObject.AttendiesCount != null)
                        {

                            if (!filterItems.Any(x => Convert.ToInt32(((CustomComboBoxItem)x).DisplayValue) == dataObject.AttendiesCount))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.AttendiesCount;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("AttendiesCount Like '%{0}%'", dataObject.AttendiesCount));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                    }
                }
                #endregion               
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        public void CurruncyExchangeButtonCommandAction(object obj)
        {
            try
            {

                var TempCurr = CurrFrom;
                CurrFrom = CurrTo;
                CurrTo = TempCurr;
                UpdateExpenseWithNewToAndFromCurr();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CurruncyExchangeButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }//[rdixit][GEOS2-4301][04.08.2023]      
        public enum WeekDays { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }  //[GEOS2-3958][rdixit][05.11.2022]     
        public void ExchangevalueChangingCommandAction(object obj)
        {
            if (TravelExpenseStatus.IdWorkflowStatus != 25 && SelectedExpense.IsNotExistAsAttendee == true)
            {
                //[rdixit][09.05.2024][GEOS2-5615]
                DevExpress.Xpf.Grid.CellValueChangedEventArgs arg = (DevExpress.Xpf.Grid.CellValueChangedEventArgs)obj;
                if (arg.Column.FieldName == "ExchangeRate")//[rdixit][09.05.2024][GEOS2-5615]
                {
                    if (!string.IsNullOrEmpty(arg.Value?.ToString()))
                    {
                        //CultureInfo customCulture = new CultureInfo("en-US");
                        //double newval = Convert.ToDouble(arg.Value, customCulture);
                        double newval = double.Parse(arg.Value?.ToString(), CultureInfo.CurrentCulture);
                        GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction newval : " + newval, category: Category.Info, priority: Priority.Low);
                        double Mindeviation = 0;
                        string src = newval.ToString("N", CultureInfo.CurrentCulture);
                        GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction src : " + src, category: Category.Info, priority: Priority.Low);
                        double desval = (double)(Math.Round(((Math.Round((Convert.ToDouble(ExchangeRateDeviationValue) / 100), 2)) * SelectedExpense.OriginalConversionRate), 2) + SelectedExpense.OriginalConversionRate);
                        GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction desval : " + desval, category: Category.Info, priority: Priority.Low);
                        string des = desval.ToString("N", CultureInfo.CurrentCulture);
                        GeosApplication.Instance.Logger.Log("Get ExchangevalueChangingCommandAction des : " + des, category: Category.Info, priority: Priority.Low);
                        if ((newval > desval) || newval < Mindeviation)
                        {
                            CustomMessageBox.Show("The following introduced exchange rates differ too much from the \n official exchange rate value in that day: \n \n [" +
                               SelectedExpense.ExpenseDate.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, System.Globalization.CultureInfo.CurrentCulture) + "] Manual = " + src + " | Official = " + Math.Round(SelectedExpense.OriginalConversionRate, 2).ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            if (SelectedExpense.ExchangeRate > desval)
                            {
                                SelectedExpense.DisplayExchangeRate = Math.Round(desval, 4).ToString();
                                SelectedExpense.TransactionOperation = ModelBase.TransactionOperations.Update;
                                EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == SelectedExpense.IdEmployeeExpense).TransactionOperation = ModelBase.TransactionOperations.Update;
                                SelectedExpense.ExchangeRate = (float)Math.Round(desval, 4);
                            }
                            else
                            {
                                SelectedExpense.DisplayExchangeRate = Math.Round(SelectedExpense.ExchangeRate, 4).ToString();
                                SelectedExpense.ExchangeRate = (float)Math.Round(newval, 4);
                            }
                        }
                        else
                        {
                            SelectedExpense.TransactionOperation = ModelBase.TransactionOperations.Update;
                            SelectedExpense.DisplayExchangeRate = Math.Round(newval, 4).ToString();
                            SelectedExpense.ExchangeRate = (float)Math.Round(newval, 4);
                            EmployeeExpenseByExpenseReport.FirstOrDefault(i => i.IdEmployeeExpense == SelectedExpense.IdEmployeeExpense).TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }   //[GEOS2-4302][rdixit][18.08.2023]
        void UpdateExpenseWithNewToAndFromCurr()
        {
            try
            {
                MealSummeryFlag = 0;
                MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(TravelExpenseReport.IdCompany, CurrTo.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                #region APILayerCurrencyConversions
                //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
                if (MealExpenceAllowance.CurrencyConversion == null)
                {
                    try
                    {
                        Currency currencyFrom = new Currency();
                        currencyFrom.IdCurrency = CurrFrom.IdCurrency;
                        currencyFrom.Name = CurrFrom.Name;
                        Currency currencyTo = new Currency();
                        currencyTo.IdCurrency = SelectedCurrency.IdCurrency;
                        currencyTo.Name = SelectedCurrency.Name;
                        //HrmService = new HrmServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
                        bool result = HrmService.APILayerCurrencyConversions_V2560(StartDate, currencyFrom, currencyTo);
                        //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        MealExpenceAllowance = HrmService.GetMealExpenseByEmployyeAndCompany_V2440(TravelExpenseReport.IdCompany, CurrTo.IdCurrency, StartDate, TravelExpenseReport.IdEmployee);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                #endregion
                //Service Method updated from EmployeeExpenseByExpenseReport_V2420 to EmployeeExpenseByExpenseReport_V2430 by [rdixit][06.09.2023][GEOS2-4798]
                OriginalCurrTo.CurrencyIconImage = null;
                Currency to = (Currency)CurrTo.Clone();
                to.CurrencyIconImage = null;
                //Service EmployeeExpenseByExpenseReport_V2440 with EmployeeExpenseByExpenseReport_V2450 by [rdixit][GEOS2-4721][10.11.2023]
                //EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2450(TravelExpenseReport.IdEmployeeExpenseReport, OriginalCurrTo, CurrTo));
                //Shubham[skadam] GEOS2-5145 Expenses Shared in different currency. 18 01 2024
                EmployeeExpenseByExpenseReport = new ObservableCollection<Expenses>(HrmService.EmployeeExpenseByExpenseReport_V2480(TravelExpenseReport.IdEmployeeExpenseReport, OriginalCurrTo, CurrTo));
                if (EmployeeExpenseByExpenseReport != null)
                {
                    //[rdixit][GEOS2-6979][02.04.2025]
                    EmployeeExpenseByExpenseReport.ToList().ForEach(i =>
                    {                       
                        i.IsNotExistAsAttendee = i.IsNotExistAsAttendee && (GeosApplication.Instance.IsChangeAndAdminPermissionForHRM
                        || GeosApplication.Instance.IsHRMTravelManagerPermission);
                    });
                    if (CurrFrom.IdCurrency != TravelExpenseReport.IdCurrencyFrom || CurrTo.IdCurrency != TravelExpenseReport.IdCurrencyTo)
                        EmployeeExpenseByExpenseReport.ToList().ForEach(i => { i.TransactionOperation = ModelBase.TransactionOperations.Update; i.ExchangeRate = 0; });
                }
                ExpenseColumnName = string.Format(System.Windows.Application.Current.FindResource("ExpenseColumn").ToString(), "[" + CurrFrom.Name + "-" + CurrTo.Name + "]");

                // FillWeeklyExpenseList();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateExpenseWithNewToAndFromCurr() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateExpenseWithNewToAndFromCurr()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Comments [chitra.girigosavi][GEOS2-4824][02.11.2023]
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public void DeleteCommentCommandAction(object parameter)
        {
            if (SelectedComment.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

                LogEntriesByTravelExpense commentObject = (LogEntriesByTravelExpense)parameter;


                bool result = false;
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteCommentMessageBox"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (CommentsList != null && CommentsList.Count > 0)
                        {
                            LogEntriesByTravelExpense Comment = (LogEntriesByTravelExpense)commentObject;
                            //result = SAMService.DeleteComment_V2340(Comment.IdComment,Site);
                            CommentsList.Remove(Comment);

                            if (DeleteCommentsList == null)
                                DeleteCommentsList = new ObservableCollection<LogEntriesByTravelExpense>();

                            DeleteCommentsList.Add(new LogEntriesByTravelExpense()
                            {
                                IdUser = Comment.IdUser,
                                IdEmployeeExpenseReport = Comment.IdEmployeeExpenseReport,
                                Comments = Comment.Comments,
                                IsRtfText = Comment.IsRtfText,
                                IdEmployeeExpenseReportChangeLog = Comment.IdEmployeeExpenseReportChangeLog,
                            });
                            CommentsList = new ObservableCollection<LogEntriesByTravelExpense>(CommentsList);
                            HrmComments = Comment;
                            IsDeleted = true;
                        }

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteComment").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(),
                    CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["DeleteExpenseReportCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38) && up.Permission.IdGeosModule == 7))
                {
                    GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                    GridControl gridControlView = (GridControl)obj;
                    AddEditHRMTravelExpenseReportComments addCommentsView = new AddEditHRMTravelExpenseReportComments();
                    AddEditHRMTravelExpenseReportCommentsViewModel addCommentsViewModel = new AddEditHRMTravelExpenseReportCommentsViewModel();
                    EventHandler handle = delegate { addCommentsView.Close(); };
                    addCommentsViewModel.RequestClose += handle;
                    addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                    var ownerInfo = (gridControlView as FrameworkElement);
                    addCommentsView.Owner = Window.GetWindow(ownerInfo);
                    //addCommentsViewModel.IsNew = true;
                    //addCommentsViewModel.Init();

                    addCommentsView.DataContext = addCommentsViewModel;
                    addCommentsView.ShowDialog();
                    if (addCommentsViewModel.SelectedComment != null)
                    {

                        if (CommentsList == null)
                            CommentsList = new ObservableCollection<LogEntriesByTravelExpense>();

                        addCommentsViewModel.SelectedComment.IdEmployeeExpenseReport = Travels.IdEmployeeExpenseReport;

                        if (AddCommentsList == null)
                            AddCommentsList = new List<LogEntriesByTravelExpense>();

                        AddCommentsList.Add(new LogEntriesByTravelExpense()
                        {
                            IdUser = addCommentsViewModel.SelectedComment.IdUser,
                            IdEmployeeExpenseReport = addCommentsViewModel.SelectedComment.IdEmployeeExpenseReport,
                            Comments = addCommentsViewModel.SelectedComment.Comments,
                            IsRtfText = addCommentsViewModel.SelectedComment.IsRtfText
                        });
                        CommentsList.Insert(0, addCommentsViewModel.SelectedComment);
                        SelectedComment = addCommentsViewModel.SelectedComment;
                        CommentDatetimeText = DateTime.Now;
                        //CommentText = SelectedComment.Comments;
                    }
                    GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        private void CommentDoubleClickCommandAction(object obj)
        {
            if (SelectedComment.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                LogEntriesByTravelExpense logcomments = (LogEntriesByTravelExpense)obj;
                //int idlogentrybyidcontact = logcomments.IdLogEntryByContact;
                AddEditHRMTravelExpenseReportComments editCommentsView = new AddEditHRMTravelExpenseReportComments();
                AddEditHRMTravelExpenseReportCommentsViewModel editCommentsViewModel = new AddEditHRMTravelExpenseReportCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.NewItemComment = SelectedComment.Comments;
                editCommentsViewModel.IdLogEntryByItem = SelectedComment.IdEmployeeExpenseReportChangeLog;
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();

                if (editCommentsViewModel.SelectedComment != null)
                {
                    SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdEmployeeExpenseReportChangeLog == SelectedComment.IdEmployeeExpenseReportChangeLog).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdEmployeeExpenseReportChangeLog == SelectedComment.IdEmployeeExpenseReportChangeLog).Datetime = GeosApplication.Instance.ServerDateTime;

                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<LogEntriesByTravelExpense>();

                    editCommentsViewModel.SelectedComment.IdEmployeeExpenseReport = SelectedExpenseReport.IdEmployeeExpenseReport;
                    UpdatedCommentsList.Add(new LogEntriesByTravelExpense()
                    {
                        Datetime = SelectedComment.Datetime,
                        IdUser = SelectedComment.IdUser,
                        IdEmployeeExpenseReport = SelectedComment.IdEmployeeExpenseReport,
                        Comments = SelectedComment.Comments,
                        IsRtfText = SelectedComment.IsRtfText,
                        IdEmployeeExpenseReportChangeLog = SelectedComment.IdEmployeeExpenseReportChangeLog
                    });
                }
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["EditExpenseReportCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public void SetUserProfileImage(LogEntriesByTravelExpense comment)
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);
                user = WorkbenchStartUp.GetUserById(Convert.ToInt32(comment.IdUser));
                // user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                var UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);

                if (UserProfileImageByte != null)
                    comment.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        //if (user.IdUserGender == 1)
                        //    comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Hrm;component/Assets/Images/FemaleUser_White.png");
                        ////UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        //else if (user.IdUserGender == 2)
                        //    comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Hrm;component/Assets/Images/MaleUser_White.png");
                        //else if (user.IdUserGender == null)
                        comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        //if (user.IdUserGender == 1)
                        //    comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Hrm;component/Assets/Images/FemaleUser_Blue.png");
                        //else if (user.IdUserGender == 2)
                        //    comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Hrm;component/Assets/Images/MaleUser_Blue.png");
                        //else if (user.IdUserGender == null)
                        comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillExpenseGrid()
        {
            try
            {
                //[GEOS2-3943][rdixit][04.01.2022] Added User Permission Validation
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38 || up.IdPermission == 39) && up.Permission.IdGeosModule == 7)))
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    #region Commented
                    //GetTravelExpenses Method Updated with GetTravelExpenses_V2320 [rdixit][GEOS2-3829][26.09.2022]
                    //GetTravelExpenses_V2320 Method Updated with GetTravelExpenses_V2340 [rdixit][GEOS2-4022][24.11.2022]
                    //GetTravelExpenses_V2340 Method Updated with GetTravelExpenses_V2360 [rdixit][GEOS2-4066][03.01.2023]
                    //GetTravelExpenses_V2360 Method Updated with GetTravelExpenses_V2410 [rdixit][GEOS2-4472][05.07.2023]
                    //GetTravelExpenses_V2410 Method updated with GetTravelExpenses_V2420 [rdixit][GEOS2-4301][04.08.2023]
                    #endregion

                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2420(plantOwnersIds));

                    #region start of  GEOS2-4848
                    //[pramod.misal][GEOS2-4848][28-11-2023]
                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2460(plantOwnersIds));
                    //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 18 12 2023
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(HrmService.GetTravelExpenses_V2470(plantOwnersIds));

                    if (FinalExpenseList.Count > 0)
                    {

                        foreach (var item in FinalExpenseList)
                        {
                            if (item.IdLinkedTrip != null && item.LinkedFromDate.HasValue)
                            {
                                //string formattedStartDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);
                                //string formattedEndDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);

                                string formattedEndDate = item.LinkedFromDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);
                                string formattedStartDate = item.LinkedToDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);

                                if (formattedStartDate != null && formattedEndDate != null && item.LinkedName != null)
                                {
                                    item.LinkedTripTitle = $"[{formattedStartDate} - {formattedEndDate}] – {item.LinkedName}";
                                }

                            }


                        }
                    }

                    #endregion GEOS2-4848

                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 40 || up.IdPermission == 41) && up.Permission.IdGeosModule == 7))
                {
                    List<UserPermission> Permissions = GeosApplication.Instance.ActiveUser.UserPermissions.Where(i => i.Permission.IdGeosModule == 7).ToList();
                    UserPermission userwithPermission = Permissions.Where(up => (up.IdPermission == 40 || up.IdPermission == 41)).FirstOrDefault();
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    //GetAllTravelExpensewithPermission_V2360 Method updated with GetAllTravelExpensewithPermission_V2420 [rdixit][GEOS2-4301][04.08.2023]
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Alias));
                    //FinalExpenseList = new ObservableCollection<TravelExpenses>(
                    //    HrmService.GetAllTravelExpensewithPermission_V2420(
                    //        plantOwnersIds, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.ActiveEmployee.Organization,
                    //        userwithPermission.IdPermission));
                    //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 18 12 2023
                    FinalExpenseList = new ObservableCollection<TravelExpenses>(
                       HrmService.GetAllTravelExpensewithPermission_V2470(
                           plantOwnersIds, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.ActiveEmployee.Organization,
                           userwithPermission.IdPermission));
                }
                else
                {
                    FinalExpenseList = new ObservableCollection<TravelExpenses>();
                }

                if (FinalExpenseList.Count > 0)
                    SelectedGridRow = FinalExpenseList.FirstOrDefault();
                IsBusy = false;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExpenseGrid() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExpenseGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        private void ExportAttachementReportAction(object obj)
        {
            try
            {
                string FinalReportPath = @"c:/Temp/ExpenseReportTemp/" + TravelExpenseReport.ExpenseCode;
                GeosApplication.Instance.Logger.Log("Method ExportAttachementReportAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                string idEmployeeExpenses = string.Join(",", EmployeeExpenseByExpenseReport.Select(expense => expense.IdEmployeeExpense).ToList());
                #region [rdixit][06.02.2024][GEOS2-5077]
                //AttachmentList = new ObservableCollection<EmployeeExpensePhotoInfo>(HrmService.HRM_GetAllExpenseAttachmentsByExpenseReportId(TravelExpenseReport.IdEmployeeExpenseReport));
                //Shubham[skadam] GEOS2-6037 HRM Expense Report - Time to Generate Expense tickets 05 08 2024
                //AttachmentList = new ObservableCollection<EmployeeExpensePhotoInfo>(HrmService.HRM_GetAllExpenseAttachmentsByExpenseReportId_V2500(TravelExpenseReport.IdEmployeeExpenseReport));
                //HrmService = new HrmServiceController("localhost:6699");
                //Shubham[skadam] GEOS2-6500 HRM Expenses report not working properly.  22 01 2024
                AttachmentList = new ObservableCollection<EmployeeExpensePhotoInfo>(HrmService.HRM_GetAllExpenseAttachmentsByExpenseReportId_V2600(TravelExpenseReport.IdEmployeeExpenseReport));
                int pageno = 1; bool foundattachment = false;
                foreach (var item in EmployeeExpenseByExpenseReport)
                {
                    item.PageNo = pageno;
                    pageno++;
                }
                if (AttachmentList != null)
                {
                    #region Paging
                    string pagesize = HrmService.Geos_app_settingsReceiptsPDF_PageSize();
                    List<string> Tempapp_settings = pagesize.Split(';').ToList();
                    List<float> header_PageSize_Settings = new List<float>();
                    foreach (var item in Tempapp_settings)
                    {
                        header_PageSize_Settings.Add((float)Convert.ToDouble(Regex.Match(item, @"\d+").Value));
                    }

                    Dictionary<string, string> DictonaryArticle = new Dictionary<string, string>();
                    try
                    {
                        #region DictonaryArticle
                        foreach (var exportExpensesItem in AttachmentList.GroupBy(i => i.IdEmployeeExpense))
                        {
                            int numcount = EmployeeExpenseByExpenseReport.FirstOrDefault(p => p.IdEmployeeExpense == exportExpensesItem.Key).PageNo;
                            int numcount1 = 0;
                            var tempExpensesAttachment = exportExpensesItem.Where(w => w.IdEmployeeExpense == exportExpensesItem.Key);
                            if (tempExpensesAttachment.Count() == 1)
                            {
                                var Attachment = tempExpensesAttachment.FirstOrDefault();
                                if (Attachment.FileType.ToLower() == ".pdf".ToLower())
                                {

                                getPdfInByte:
                                    if (Attachment.PdfInByte != null)
                                    {
                                        PdfReader pdfReader = new PdfReader(Attachment.PdfInByte);
                                        int numberOfPages = pdfReader.NumberOfPages;
                                        for (int p = 1; p <= numberOfPages; p++)
                                        {
										//Shubham[skadam] GEOS2-7898 Expenses images not showing proper for EXP25.0236  18 04 2025
                                        Getpage:
                                            try
                                            {
                                                DictonaryArticle.Add(numcount + "\n ;Page_" + p, Attachment.OriginalFileName.ToString() + "$" + Attachment.IdEmployeeExpense);
                                            }
                                            catch (Exception ex)
                                            {
                                                p = p + 1;
                                                goto Getpage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            byte[] PdfBytes1 = new byte[0];
                                            //using (WebClient webClient = new WebClient())
                                            //{
                                            //    PdfBytes1 = webClient.DownloadData(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + Attachment.IdEmployeeExpense + "\\" + Attachment.OriginalFileName.ToString());
                                            //    Attachment.PdfInByte = PdfBytes1;
                                            //    goto getPdfInByte;
                                            //}
                                            PdfBytes1 = Utility.ImageUtil.GetImageByWebClient(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + Attachment.IdEmployeeExpense + "\\" + Attachment.OriginalFileName.ToString());
                                            Attachment.PdfInByte = PdfBytes1;
                                            goto getPdfInByte;
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                                else
                                {
									//Shubham[skadam] GEOS2-7898 Expenses images not showing proper for EXP25.0236  18 04 2025
                                    try
                                    {
                                        DictonaryArticle.Add(string.Format("{0}", numcount), Attachment.SavedFileName + "$" + Attachment.IdEmployeeExpense);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var temp in tempExpensesAttachment)
                                {
                                    numcount1 = numcount1 + 1;
                                    if (temp.FileType.ToLower() == ".pdf".ToLower())
                                    {
                                    pdfInByte:
                                        if (temp.PdfInByte != null)
                                        {
                                            PdfReader pdfReader = new PdfReader(temp.PdfInByte);
                                            int numberOfPages = pdfReader.NumberOfPages;
                                            for (int p = 1; p <= numberOfPages; p++)
                                            {
                                            //Shubham[skadam] GEOS2-7898 Expenses images not showing proper for EXP25.0236  18 04 2025
                                            Getpage:
                                                try
                                                {

                                                    DictonaryArticle.Add(numcount + "\n ;Page_" + p, temp.OriginalFileName.ToString() + "$" + temp.IdEmployeeExpense);
                                                }
                                                catch (Exception ex)
                                                {
                                                    p = p + 1;
                                                    goto Getpage;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                byte[] PdfBytes1 = new byte[0];
                                                //using (WebClient webClient = new WebClient())
                                                //{
                                                //    PdfBytes1 = webClient.DownloadData(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + temp.IdEmployeeExpense + "\\" + temp.OriginalFileName.ToString());
                                                //    temp.PdfInByte = PdfBytes1;
                                                //    goto pdfInByte;
                                                //}
                                                PdfBytes1 = Utility.ImageUtil.GetImageByWebClient(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + temp.IdEmployeeExpense + "\\" + temp.OriginalFileName.ToString());
                                                temp.PdfInByte = PdfBytes1;
                                                goto pdfInByte;
                                            }
                                            catch (Exception ex)
                                            {
                                                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                            }
                                        }
                                    }
                                    else
                                    {
										//Shubham[skadam] GEOS2-7898 Expenses images not showing proper for EXP25.0236  18 04 2025
                                        try
                                        {
                                            DictonaryArticle.Add(string.Format("{0}.{1}", numcount, numcount1.ToString()), temp.SavedFileName + "$" + temp.IdEmployeeExpense);
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    #endregion
                    if (!Directory.Exists(FinalReportPath))
                    {
                        Directory.CreateDirectory(FinalReportPath);
                    }
                    else
                    {
                        Directory.Delete(FinalReportPath, true);
                        Directory.CreateDirectory(FinalReportPath);
                    }

                    #region createPDFDocument
                    //Dictionary<string, string> sortedDictionary = DictonaryArticle.OrderBy(entry => GetNumericPart(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
                    //Shubham[skadam] GEOS2-6500 HRM Expenses report not working properly.  22 01 2024
                    Dictionary<string, string> sortedDictionary = sortedDictionary = DictonaryArticle.OrderBy(entry => GetFirstNumber(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
                    iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, header_PageSize_Settings[0],
                    header_PageSize_Settings[1], Convert.ToInt32(header_PageSize_Settings[2]) + 65, Convert.ToInt32(header_PageSize_Settings[3]) - 20);
                    if (!Directory.Exists(FinalReportPath))
                    {
                        Directory.CreateDirectory(FinalReportPath);
                    }
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Path.Combine(FinalReportPath, receiptsPDF), FileMode.Create));
                    document.Open();
                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 0;
                    table.SpacingAfter = 0;
                    int tableCount = 0;
                    try
                    {
                        #region DownloadData
                        foreach (var item in sortedDictionary)// for (int i = 1; i <= sortedDictionary.Count; i++)
                        {
                            PdfPTable innertable = new PdfPTable(1);
                            innertable.SpacingBefore = 0;
                            innertable.SpacingAfter = 0;
                            innertable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            string[] tokens1 = item.Value.Split('$');
                            int idexpense = Convert.ToInt32(tokens1[1]);
                            string filename = tokens1[0];
                            string[] tokens = item.Key.Split(';');
                            string last = tokens[tokens.Length - 1];
                            Phrase phrase = new Phrase();
                            phrase.Add(new Chunk(tokens.FirstOrDefault(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, iTextSharp.text.Font.BOLD)));
                            if (!tokens.FirstOrDefault().Equals(last))
                            {
                                phrase.Add(new Chunk(last, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 05, iTextSharp.text.Font.BOLD)));
                            }
                            PdfPCell cell2 = new PdfPCell(phrase);
                            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell2.VerticalAlignment = Element.ALIGN_JUSTIFIED_ALL;
                            cell2.Border = 0;
                            cell2.PaddingLeft = 0;
                            cell2.PaddingRight = 0;
                            cell2.PaddingTop = 1;
                            cell2.PaddingBottom = 0;
                            innertable.AddCell(cell2);
                            byte[] imageBytes1 = new byte[0];
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    imageBytes1 = webClient.DownloadData(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + idexpense + "\\" + filename);
                            //}
                            try
                            {
                                imageBytes1 = Utility.ImageUtil.GetImageByWebClient(@"https://api.emdep.com/GEOS/Images.aspx?FilePath=/Documents/Employees\Expenses\" + idexpense + "\\" + filename);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                            if (imageBytes1.Length != 0)
                            {
                                foundattachment = true;
                                System.Drawing.Image BitmaptoImage = null;
                                if (Path.GetExtension(filename).ToLower() == ".pdf")
                                {
                                    int largestEdgeLength = 1000;
                                    using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
                                    {
                                        if (imageBytes1 != null)
                                        {
                                            try
                                            {
                                                // Load a document.
                                                MemoryStream stream = new MemoryStream(imageBytes1);
                                                processor.LoadDocument(stream);
                                                Bitmap image = processor.CreateBitmap(GetPageNumber(last), largestEdgeLength);
                                                BitmaptoImage = (System.Drawing.Image)image;
                                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ConvertImage(BitmaptoImage));
                                                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(img, true);
                                                cell.Border = 0;
                                                innertable.AddCell(cell);
                                                table.AddCell(innertable);
                                            }
                                            catch (Exception ex)
                                            {
                                                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        byte[] imageBytesNew = new byte[0];
                                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes1);
                                        try
                                        {
                                            // Get the original image dimensions
                                            int originalWidth, originalHeight;
                                            using (var inputStream = new MemoryStream(imageBytes1))
                                            using (var originalImage = System.Drawing.Image.FromStream(inputStream))
                                            {
                                                originalWidth = originalImage.Width;
                                                originalHeight = originalImage.Height;
                                            }
                                            int newWidth = Convert.ToInt32(img.Width);
                                            // Set new dimensions (maintaining aspect ratio)
                                            //int newHeight = 1300;
                                            int newHeight = Convert.ToInt32(AttachmentList.FirstOrDefault().NewHeight);
                                            //int newWidth = (originalWidth * newHeight) / originalHeight;
                                            // Call the ResizeImage function
                                            // Get resized image bytes
                                            if (originalHeight >= newHeight)
                                            {
                                                imageBytesNew = ResizeImageBytes(imageBytes1, newWidth, newHeight);
                                                img = iTextSharp.text.Image.GetInstance(imageBytesNew);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(img, true);
                                        cell.Border = 0;
                                        innertable.AddCell(cell);
                                        table.AddCell(innertable);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

                                    }

                                }

                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    if ((DictonaryArticle.Count - tableCount) % 2 != 0)
                    {
                        table.AddCell("");
                    }
                    if (foundattachment)
                    {
                        document.Add(table);
                        document.Close();
                        #endregion

                        #region Expense_Report_ReceiptsPDF_Header_Image
                        Dictionary<string, byte[]> geos_app_settingsHeader_Image = HrmService.Geos_app_settingsHeader_Image();
                        List<string> Tempapp_Image_Settings = geos_app_settingsHeader_Image.FirstOrDefault().Key.Split(';').ToList();
                        List<string> header_Image_Settings = new List<string>();
                        int skipFirst = 0;
                        foreach (var item in Tempapp_Image_Settings)
                        {
                            header_Image_Settings.Add(Regex.Match(item, @"\d+").Value);
                            skipFirst = skipFirst + 1;
                        }
                        #endregion
                        #region Expense_Report_ReceiptsPDF_Footer_DateTime
                        string geos_app_settingsHeader_Footer_DateTime = HrmService.Geos_app_settingsHeader_Footer_DateTime();
                        List<string> Tempapp_Settings_Footer_DateTime = geos_app_settingsHeader_Footer_DateTime.Split(';').ToList();
                        List<string> settings_Footer_DateTime = new List<string>();
                        foreach (var item in Tempapp_Settings_Footer_DateTime)
                        {
                            settings_Footer_DateTime.Add(Regex.Match(item, @"\d+").Value);
                        }
                        #endregion
                        #region Expense_Report_ReceiptsPDF_Footer_Email
                        string geos_app_settings_Footer_Email = HrmService.Geos_app_settings_Footer_Email();

                        List<string> Tempapp_Settings_Footer_Email = geos_app_settings_Footer_Email.Split(';').ToList();
                        List<string> settings_Footer_Email = new List<string>();
                        int skipEmail = 0;
                        foreach (var item in Tempapp_Settings_Footer_Email)
                        {
                            if (skipEmail == 0)
                            {
                                settings_Footer_Email.Add(item);
                            }
                            else
                            {
                                settings_Footer_Email.Add(Regex.Match(item, @"\d+").Value);
                            }
                            skipEmail = skipEmail + 1;
                        }
                        #endregion
                        #region Expense_Report_ReceiptsPDF_Footer_pageNumber
                        string geos_app_settingsHeader_Footer_pageNumber = HrmService.Geos_app_settingsHeader_Footer_pageNumber();
                        List<string> Tempapp_Settings_Footer_pageNumber = geos_app_settingsHeader_Footer_pageNumber.Split(';').ToList();
                        List<string> settings_Footer_pageNumber = new List<string>();
                        foreach (var item in Tempapp_Settings_Footer_pageNumber)
                        {
                            settings_Footer_pageNumber.Add(Regex.Match(item, @"\d+").Value);
                        }
                        #endregion

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                        #region Open Final Report
                        byte[] bytes = null;
                        try
                        {
                            if (File.Exists(Path.Combine(FinalReportPath, receiptsPDF)))
                            {
                                FileInfo fileinfo = new FileInfo(Path.Combine(FinalReportPath, receiptsPDF));
                                string ext = fileinfo.Extension;
                                if (ext.ToLower() == ".pdf")
                                {
                                    using (System.IO.FileStream stream = new System.IO.FileStream(Path.Combine(FinalReportPath, receiptsPDF), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                    {
                                        bytes = new byte[stream.Length];
                                        int numBytesToRead = (int)stream.Length;
                                        int numBytesRead = 0;
                                        while (numBytesToRead > 0)
                                        {
                                            // Read may return anything from 0 to numBytesToRead.	
                                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                                            // Break when the end of the file is reached.	
                                            if (n == 0)
                                                break;
                                            numBytesRead += n;
                                            numBytesToRead -= n;
                                        }
                                    }
                                }
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        if (bytes != null)
                        {
                            try
                            {
                                #region Header&Footer                                               
                                var pdfReader = new iTextSharp.text.pdf.PdfReader(bytes);
                                using (Stream output = new FileStream(Path.Combine(FinalReportPath, receiptsPDF), FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    using (iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(pdfReader, output))
                                    {
                                        for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
                                        {
                                            pdfStamper.FormFlattening = false;
                                            iTextSharp.text.Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);
                                            iTextSharp.text.pdf.PdfContentByte pdfData = pdfStamper.GetOverContent(pageIndex);
                                            pdfData.SetFontAndSize(iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.TIMES_BOLD, iTextSharp.text.pdf.BaseFont.CP1252,
                                            iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED), 15);
                                            iTextSharp.text.pdf.PdfGState graphicsState = new iTextSharp.text.pdf.PdfGState
                                            {
                                                FillOpacity = 10F
                                            };
                                            pdfData.SetGState(graphicsState);
                                            iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.TIMES_BOLD, iTextSharp.text.pdf.BaseFont.CP1252,
                                            iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                                            pdfData.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD);
                                            pdfData.SetFontAndSize(bf, 10);

                                            #region Header       

                                            float x = (float)Convert.ToDouble(header_Image_Settings[0]);
                                            float y = (float)Convert.ToDouble(Convert.ToInt32(header_Image_Settings[1]) + 10);
                                            float w = (float)Convert.ToDouble(header_Image_Settings[2]);
                                            float h = (float)Convert.ToDouble(header_Image_Settings[3]);

                                            if (geos_app_settingsHeader_Image.FirstOrDefault().Value != null)
                                            {
                                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(geos_app_settingsHeader_Image.FirstOrDefault().Value);
                                                image.ScaleAbsolute(w, h);
                                                image.SetAbsolutePosition(x, y);
                                                pdfData.AddImage(image);
                                            }

                                            #endregion

                                            #region Footer  
                                            pdfData.BeginText();
                                            DateTime dt = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                                            dt.ToString(SelectedCulture.DateTimeFormat.ShortDatePattern, SelectedCulture);
                                            String Headertext1 = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                            pdfData.ShowTextAligned(1, Headertext1, (float)Convert.ToDouble(settings_Footer_DateTime[0]), (float)Convert.ToDouble(settings_Footer_DateTime[1]), 0);
                                            pdfData.EndText();
                                            pdfData.BeginText();
                                            string Footertext = settings_Footer_Email[0];
                                            pdfData.ShowTextAligned(1, Footertext, (float)Convert.ToDouble(settings_Footer_Email[1]), (float)Convert.ToDouble(settings_Footer_Email[2]), 0);
                                            pdfData.EndText();
                                            pdfData.BeginText();
                                            string pageNumber = pageIndex.ToString();
                                            pdfData.ShowTextAligned(1, pageNumber, (float)Convert.ToDouble(settings_Footer_pageNumber[0]), (float)Convert.ToDouble(settings_Footer_pageNumber[1]), 0);
                                            pdfData.EndText();
                                            #endregion
                                        }
                                    }
                                    output.Close();
                                    output.Dispose();
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }

                            try
                            {
                                if (File.Exists(Path.Combine(FinalReportPath, receiptsPDF)))
                                {
                                    FileInfo fileinfo = new FileInfo(Path.Combine(FinalReportPath, receiptsPDF));
                                    string ext = fileinfo.Extension;
                                    if (ext.ToLower() == ".pdf")
                                    {
                                        using (System.IO.FileStream stream = new System.IO.FileStream(Path.Combine(FinalReportPath, receiptsPDF), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                        {
                                            bytes = new byte[stream.Length];
                                            int numBytesToRead = (int)stream.Length;
                                            int numBytesRead = 0;
                                            while (numBytesToRead > 0)
                                            {
                                                // Read may return anything from 0 to numBytesToRead.	
                                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                                                // Break when the end of the file is reached.	
                                                if (n == 0)
                                                    break;
                                                numBytesRead += n;
                                                numBytesToRead -= n;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (FileNotFoundException ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                            // Open PDF in another window
                            EmployeeDocumentView documentView = new EmployeeDocumentView();
                            EmployeeDocumentViewModel documentViewModel = new EmployeeDocumentViewModel();
                            documentViewModel.OpenPdfFromBytes(bytes, "Expense_Report");
                            documentView.DataContext = documentViewModel;
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            documentView.Show();


                            //System.Diagnostics.Process.Start(Path.Combine(FinalReportPath, receiptsPDF));
                        }
                        #endregion
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentNotExist"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentNotExist"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
                #endregion

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ExportAttachementReportAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ExportAttachementReportAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ExportAttachementReportAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttachementReportAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        } //[pramod.misal][GEOS2-5077][17-01-2024]

        // shubham[skadam] APIGEOS-659  endpoint Accounting/Employees/Export https://helpdesk.emdep.com/browse/APIGEOS-462 APIGEOS-659 APIGEOS-618 not working as expected 12 11 2022
        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int height)
        {
            double ratio = (double)height / image.Height;
            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }
        public static byte[] ResizeImageBytes(byte[] originalBytes, int newWidth, int newHeight)
        {
            using (var inputStream = new MemoryStream(originalBytes))
            {
                // Load the original image from bytes
                using (var originalImage = System.Drawing.Image.FromStream(inputStream))
                {
                    // Create a new bitmap with the desired size
                    using (var resizedBitmap = new Bitmap(newWidth, newHeight))
                    {
                        // Draw the original image onto the new bitmap
                        using (var graphics = Graphics.FromImage(resizedBitmap))
                        {
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                            // Draw the image with the new dimensions
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        // Convert the resized bitmap to a byte array
                        using (var outputStream = new MemoryStream())
                        {
                            resizedBitmap.Save(outputStream, ImageFormat.Jpeg); // Save as JPEG or change format as needed
                            return outputStream.ToArray();
                        }
                    }
                }
            }
        }

        public static void ResizeImage(byte[] imageBytes, int newWidth, int newHeight, string outputFilePath)
        {
            // Create an iTextSharp Image instance from the byte array
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes);

            // Set the new dimensions for the image
            img.ScaleAbsolute(newWidth, newHeight);

            // Optionally, write the image to a PDF for demonstration purposes
            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                // Initialize the document and writer
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                // Open the document
                document.Open();

                // Add the resized image to the document
                document.Add(img);

                // Close the document
                document.Close();
            }
        }
        public static int GetNumericPart(string input)
        {
            // Define the regular expression pattern to match numerical digits
            string[] parts = input.Split(';');
            if (parts.Length > 1)
            {
                return GetPageNumber(parts[1]);
            }
            else
            {
                return GetPageNumber(parts[0]);
            }
        }
        public iTextSharp.text.Image ConvertImage(System.Drawing.Image drawingImage)
        {
            // Convert System.Drawing.Image to iTextSharp.text.Image
            using (MemoryStream ms = new MemoryStream())
            {
                drawingImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // You can change the format if needed
                return iTextSharp.text.Image.GetInstance(ms.ToArray());
            }
        }
        public static int GetPageNumber(string input)
        {
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);
            if (matches.Count > 0)
            {
                return int.Parse(matches[0].Value);
            }
            return 0;
        }
        //Shubham[skadam] GEOS2-6500 HRM Expenses report not working properly.  22 01 2024
        // Extract the first number from the input string
        public static int GetFirstNumber(string input)
        {
            // Remove brackets for cleaner processing
            input = input.Trim('[', ']');

            // Match the first sequence of digits in the string
            Regex numberRegex = new Regex(@"\d+");

            // Try to find the first number in the input
            System.Text.RegularExpressions.Match match = numberRegex.Match(input);
            if (match.Success && int.TryParse(match.Value, out int number))
            {
                return number;
            }

            // If no numbers are found, return 0
            return 0;
        }

        public static int GetNumericPartNew(string input)
        {
            // Split the string on ';'
            string[] parts = input.Split(';');
            // Get the numeric part from the appropriate segment
            if (parts.Length > 1)
            {
                return GetPageNumber(parts[1]);
            }
            else
            {
                return GetPageNumber(parts[0]);
            }
        }

        public static int GetPageNumberNew(string input)
        {
            // Match numerical digits in the string
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);

            // Return the first numeric match, or 0 if no match
            if (matches.Count > 0 && int.TryParse(matches[0].Value, out int result))
            {
                return result;
            }
            return 0;
        }

        private void ChangeEmployeeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {

                    if (SelectedEmployee != null)
                    {
                        LinkedTriplList = new ObservableCollection<EmployeeTrips>(HrmService.GetEmployeeTripsBySelectedIdEmpolyee_V2460(selectedEmployee.IdEmployee));

                        LinkedTriplList.ToList().ForEach(i => i.LinkedTripTitle = $"[{i.ArrivalDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture)} - {i.DepartureDate?.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture)}] – {i.Title}");
                        LinkedTriplList = new ObservableCollection<EmployeeTrips>(LinkedTriplList.OrderByDescending(expense => expense.ArrivalDate).ToList());
                        EmployeeTrips defaultValue = new EmployeeTrips { LinkedTripTitle = "---", IdEmployeeTrip = 0 };
                        LinkedTriplList.Insert(0, defaultValue);
                        LinkedTripList1 = new ObservableCollection<EmployeeTrips>(LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).ToList());
                        AllLinkedTripList.AddRange(LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).ToList());
                        LinkedTripNamesList = LinkedTriplList.Select(i => (EmployeeTrips)i.Clone()).Select(k => k.LinkedTripTitle).ToList();
                        if (selectedEmployee.IdEmployee != 0)
                        {
                            SelectedLinkedTrip = LinkedTriplList.FirstOrDefault();
                        }

                    }


                }
                GeosApplication.Instance.Logger.Log("Method ChangeEmployeeCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeEmployeeCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        void SetButtonsEnableMethod()
        {
            foreach (var item in TravelExpenseStatusButtons)
            {
                if (item.IdWorkflowStatus == 21) //draft
                {
                    if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM 
                        || GeosApplication.Instance.IsHRMTravelManagerPermission 
                        || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 22) //Submitted
                {
                    if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM 
                        || GeosApplication.Instance.IsHRMTravelManagerPermission
                        || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 23) //Accepted
                {
                    if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM
                        || GeosApplication.Instance.IsHRMTravelManagerPermission
                        || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 24) //Approved
                {
                    if (GeosApplication.Instance.IsControlPermissionForHRM || GeosApplication.Instance.IsAdminPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 25) //Closed
                {
                    if (GeosApplication.Instance.IsPlant_FinancePermissionForHRM || GeosApplication.Instance.IsAdminPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 39) //Declined
                {
                    if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM 
                        || GeosApplication.Instance.IsHRMTravelManagerPermission 
                        || GeosApplication.Instance.IsTravel_AssistantPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }

                else if (item.IdWorkflowStatus == 42) //Rejected
                {
                    if (GeosApplication.Instance.IsChangeOrAdminPermissionForHRM)
                        item.IsEnable = true;
                    else
                        item.IsEnable = false;
                }
                else
                    item.IsEnable = false;
            }
        }   //[rdixit][09.01.2024][GEOS2-5112]

        //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
        public void FillExpenseCategories()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExpenseCategories()...", category: Category.Info, priority: Priority.Low);

                if (ExpenseCategoriesList == null)
                {
                    ExpenseCategoriesList = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(88));
                }
                //TravelExpenseReasonList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseCategories()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseCategories()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseCategories()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
        public void FillExpenseTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExpenseTypes()...", category: Category.Info, priority: Priority.Low);

                if (ExpenseTypesList == null)
                {
                    //CRMService = new CrmServiceController("localhost:6699");
                    ExpenseTypesList = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(85));
                    ExpenseTypesList.OrderBy(o => o.Position);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExpenseTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
        public void EditValueChanged(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueChanged ...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38) && up.Permission.IdGeosModule == 7)))
                {
                    var comboBoxEdit = e.Source as ComboBoxEdit;
                    string OldValue = (string)e.OldValue;
                    string NewValue = (string)e.NewValue;
                    if (!string.IsNullOrEmpty(NewValue) && !string.IsNullOrEmpty(OldValue))
                    {
                        if (comboBoxEdit.SelectedItem is LookupValue)
                        {
                            LookupValue SelectedItem = (LookupValue)comboBoxEdit.SelectedItem;
                            LookupValue OldExpenseTypesItem = ExpenseTypesList.Where(ex => ex.IdLookupValue == SelectedExpense.IdExpenseType).FirstOrDefault();
                            LookupValue ExpenseTypes = ExpenseTypesList.Where(ex => ex.IdLookupValue == SelectedItem.IdLookupValue).FirstOrDefault();
                            LookupValue OldCategories = ExpenseCategoriesList.Where(w => w.IdLookupValue == SelectedExpense.IdCategory).FirstOrDefault();
                            LookupValue NewCategories = ExpenseCategoriesList.Where(w => w.IdLookupValue == SelectedItem.IdParentNew).FirstOrDefault();
                            //Expenses tempSelectedExpenses = SelectedExpense;
                            //var temp2 = EmployeeExpenseByExpenseReport;
                            //Expenses tempExpense = EmployeeExpenseByExpenseReport.Where(w => w.IdEmployeeExpense == tempSelectedExpenses.IdEmployeeExpense).FirstOrDefault();
                            if (UpdateExpenseTypes == null)
                            {
                                UpdateExpenseTypes = new List<Expenses>();
                            }
                            if (ExpenseTypes.InUse != false)
                            {
                                SelectedExpense.IdExpenseType = ExpenseTypes.IdLookupValue;
                                SelectedExpense.ExpenseType = ExpenseTypes.Value;
                                SelectedExpense.IdCategory = NewCategories.IdLookupValue;
                                SelectedExpense.CategoryName = NewCategories.Value;

                                if (!UpdateExpenseTypes.Any(w => w.IdEmployeeExpense == SelectedExpense.IdEmployeeExpense))
                                {
                                    UpdateExpenseTypes.Add(SelectedExpense);
                                }
                                else
                                {
                                    UpdateExpenseTypes.RemoveAll(w => w.IdEmployeeExpense == SelectedExpense.IdEmployeeExpense);
                                    UpdateExpenseTypes.Add(SelectedExpense);
                                }
                            }
                            else
                            {
                                string EmployeeExpenseTypeErrorMessage = string.Format(System.Windows.Application.Current.FindResource("EmployeeExpenseTypeErrorMessage").ToString(), ExpenseTypes.Value);
                                //CustomMessageBox.Show(EmployeeExpenseTypeErrorMessage, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                Expenses tempOriginalEmployeeExpense = EmployeeExpenseOriginalList.Where(w => w.IdEmployeeExpense == SelectedExpense.IdEmployeeExpense).FirstOrDefault();
                                LookupValue setExpenseTypes = ExpenseTypesList.Where(ex => ex.IdLookupValue == tempOriginalEmployeeExpense.IdExpenseType).FirstOrDefault();
                                LookupValue setCategories = ExpenseCategoriesList.Where(w => w.IdLookupValue == setExpenseTypes.IdParentNew).FirstOrDefault();
                                SelectedExpense.IdExpenseType = setExpenseTypes.IdLookupValue;
                                SelectedExpense.ExpenseType = setExpenseTypes.Value;
                                SelectedExpense.IdCategory = setCategories.IdLookupValue;
                                SelectedExpense.CategoryName = setCategories.Value;
                                SelectedExpenseTypes = setExpenseTypes;
                            }

                        }
                    }
                }
                else
                {
                    #region comboBoxEdit
                    var comboBoxEdit = e.Source as ComboBoxEdit;
                    string OldValue = (string)e.OldValue;
                    string NewValue = (string)e.NewValue;
                    if (!string.IsNullOrEmpty(NewValue) && !string.IsNullOrEmpty(OldValue))
                    {
                        if (comboBoxEdit.SelectedItem is LookupValue)
                        {
                            LookupValue SelectedItem = (LookupValue)comboBoxEdit.SelectedItem;
                            LookupValue OldExpenseTypesItem = ExpenseTypesList.Where(ex => ex.IdLookupValue == SelectedExpense.IdExpenseType).FirstOrDefault();
                            LookupValue ExpenseTypes = ExpenseTypesList.Where(ex => ex.IdLookupValue == SelectedItem.IdLookupValue).FirstOrDefault();
                            LookupValue OldCategories = ExpenseCategoriesList.Where(w => w.IdLookupValue == SelectedExpense.IdCategory).FirstOrDefault();
                            LookupValue NewCategories = ExpenseCategoriesList.Where(w => w.IdLookupValue == SelectedItem.IdParentNew).FirstOrDefault();
                            Expenses tempSelectedExpenses = SelectedExpense;
                            var temp2 = EmployeeExpenseByExpenseReport;
                            Expenses tempExpense = EmployeeExpenseByExpenseReport.Where(w => w.IdEmployeeExpense == tempSelectedExpenses.IdEmployeeExpense).FirstOrDefault();
                            SelectedExpense.IdExpenseType = OldExpenseTypesItem.IdLookupValue;
                            SelectedExpense.ExpenseType = OldExpenseTypesItem.Value;
                            SelectedExpense.IdCategory = OldCategories.IdLookupValue;
                            SelectedExpense.CategoryName = OldCategories.Value;
                        }
                    }
                    #endregion

                }

                GeosApplication.Instance.Logger.Log("Method EditValueChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditValueChanged() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }


        //[rgadhave][GEOS2-5555][30.05.2024]
        private void RemarkDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);

            AddHRMRemarkView remarkView = new AddHRMRemarkView();
            AddHRMRemarkViewModel editCommentsViewModel = new AddHRMRemarkViewModel();
            EventHandler handle = delegate { remarkView.Close(); };
            editCommentsViewModel.RequestClose += handle;
            editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("RemarksHeader").ToString();
            editCommentsViewModel.NewItemComment = SelectedExpense.Remarks;
            remarkView.DataContext = editCommentsViewModel;
            remarkView.ShowDialog();
            if(editCommentsViewModel.IsSave)
            {
                SelectedExpense.Remarks = editCommentsViewModel.NewItemComment;
                SelectedExpense.TransactionOperation = ModelBase.TransactionOperations.Update;
            }
        }
        #endregion
        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        public void SendMailMethod_V2540(EmployeeExpenseStatus user)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("EditTravelExpenseViewModel Method SendMailMethod_V2540()....", category: Category.Info, priority: Priority.Low);
                bool IsEmailSend = HrmService.GetEmployeeExpenseReportTemplate_V2540(code, SelectedEmployee, TravelExpenseStatus, ExpenseReportComment, user.CompanyEmail);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailMethod_V2540() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailMethod_V2540() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SendMailMethod_V2540() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #region Validation [rdixit][GEOS2-4025][24.11.2022]
        bool allowValidation = false;
        private bool preventSelectionChange;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => SelectedExpenseReasonIndex)] +
                    me[BindableBase.GetPropertyName(() => StartDate)] +
                    me[BindableBase.GetPropertyName(() => EndDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedCompanyIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedCurrencyIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedEmployeeIndex)] +
                    me[BindableBase.GetPropertyName(() => Title)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string selectedReasonValue = BindableBase.GetPropertyName(() => SelectedExpenseReasonIndex);
                string ExpStartDate = BindableBase.GetPropertyName(() => StartDate);
                string ExpEndDate = BindableBase.GetPropertyName(() => EndDate);
                string SelectedCompanyValue = BindableBase.GetPropertyName(() => SelectedCompanyIndex);
                string SelectedCurrencyValue = BindableBase.GetPropertyName(() => SelectedCurrencyIndex);
                string SelectedEmployeeValue = BindableBase.GetPropertyName(() => SelectedEmployeeIndex);
                string ExpTitle = BindableBase.GetPropertyName(() => Title);

                if (columnName == selectedReasonValue)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(selectedReasonValue, SelectedExpenseReasonIndex, null);
                }
                if (columnName == ExpStartDate)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(ExpStartDate, StartDate, EndDate);
                }
                if (columnName == ExpEndDate)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(ExpEndDate, StartDate, EndDate);
                }
                if (columnName == SelectedCompanyValue)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(SelectedCompanyValue, SelectedCompanyIndex, null);
                }
                if (columnName == SelectedCurrencyValue)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(SelectedCurrencyValue, SelectedCurrencyIndex, null);
                }
                if (columnName == SelectedEmployeeValue)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(SelectedEmployeeValue, SelectedEmployeeIndex, null);
                }
                if (columnName == ExpTitle)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(ExpTitle, Title, null);
                }
                return null;
            }
        }
        #endregion

    }
}
