using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.OTM;
using System.Windows.Media.Imaging;
using Emdep.Geos.Modules.OTM.CommonClass;
using DevExpress.Xpf.WindowsUI;
using System.Windows.Controls;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Utility;
using DevExpress.Xpf.Core.Serialization;
using System.IO;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm.Native;
using System.Drawing;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Emdep.Geos.UI.Validations;
using DevExpress.XtraBars.Ribbon.Drawing;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraScheduler.Outlook.Native;
using System.Text.RegularExpressions;
using System.Globalization;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.Xpf.Core.Native;
using DevExpress.Export;
using System.Collections.Concurrent;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.OptimizedClass;
namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class PORequestsViewModel : NavigationViewModelBase, ITabViewModel, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion
        #region public ICommand

        public ICommand OpenAttachmentCommand { get; set; }
        public ICommand ExportPORequestsViewCommand { get; set; }
        public ICommand PrintPORequestsViewCommand { get; set; }
        public ICommand RefreshPORequestsViewCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CommandFilterPOStatusTileClick { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CommandPlantEditValueChanged { get; set; }
        public ICommand UpdatedCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }
        public ICommand TabSelectionChangedCommand { get; set; }
        //[pramod.misal][04.02.2025][GEOS2-6726]
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }// <!--[pramod.misal][04.02.2025][GEOS2-6726]-->

        public ICommand CommandPoRequestGridDoubleClick { get; set; } // <!--[pramod.misal][14.04.2025][GEOS2-7244]-->

        public ICommand TableViewUnloadedLoadedCommand { get; set; }
        #endregion
        #region Declaration       
        DateTime startDate;
        DateTime endDate;
        string fromDate;
        string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        private bool isBusy;
        private bool isLeftBandVisible=false;
        private bool isRightBandVisible = false;

        private string myFilterString;
        private string myFilterPORegisteredString;
        private ObservableCollection<PORequestDetails> poRequestList;
        private ObservableCollection<POEmployeeInfo> pOEmployeeInfoList;
        private ObservableCollection<PORegisteredDetails> poRegisteredList;
        ObservableCollection<Company> companyList;
        List<object> selectedLocation;
        private Duration _currentDuration;
        List<object> selectedCompanies;
        string amountCurrencySymbol;
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        private bool isFromFilterString = false;
        private List<GridColumn> GridColumnList;
        private bool isEdit;
        private string userSettingsKey = "OTM_PORequestes_";
        private string userSettingsKey_PORegistered = "OTM_PORegistered_";
        public string OTM_PORequestesGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PORequestesGridSetting.Xml";
        public string OTM_PORegisteredGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PORegisteredGridSetting.Xml";
        private int visibleRowCount;
        private ObservableCollection<POStatus> pOStatusList;
        private TileBarFilters selectedFilter;
        private ObservableCollection<PO> pOList;
        private int selectedSearchIndex;
        private bool isPlantsListLoaded;
        private bool isPOrequestsColumnChooserVisible;
        private ObservableCollection<POType> pOTypeList;
        public ObservableCollection<GridSummaryItem> TotalSummary { get; private set; }
        private ObservableCollection<POType> typeOfPos;
        private ObservableCollection<Customer> customers;
        private ObservableCollection<Currency> currencies;
        private ObservableCollection<CustomerPlant> customerPlant;
        private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        string offer;
        private int? number;
        private string poValueRangeFrom;
        private string poValueRangeTo;
        private bool isReceptionDate;
        DateTime? receptionDateFrom;
        DateTime? receptionDateTo;
        private bool iscreationDate;
        DateTime? creationDateFrom;
        DateTime? creationDateTo;
        private bool isupdateDate;
        DateTime? updateDateFrom;
        DateTime? updateDateTo;
        private bool iscancellationDate;
        DateTime? cancellationDateFrom;
        DateTime? cancellationDateTo;
        PORegisteredDetailFilter filter;
        string currencysetting;
        private List<string> poSender;
        private Data.Common.Company selectedItem;
        string sender;
        private string informationError;
        private int selectedCurrency;
        private int selectedPOSender;
        private int selectedCustomerPlant;
        private int selectedPoType;
        private int selectedGroup;
        private ObservableCollection<CustomerContacts> customerContactsList;
        const string shortDateFormat = "dd/MM/yyyy";
        private TileBarFilters _selectedTileBarItem;
        private TileBarFilters _selectedTypeTileBarItem;
        private ObservableCollection<TileBarFilters> _filterStatusListOfTile;
        private ObservableCollection<TileBarFilters> _filterTypeListOfTile;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;
        private Recipient _selectedRecipient;
        private PORequestDetails selectedPoRequest;
        #endregion
        #region Properties
        public virtual int Position { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsPlantUpdated { get; set; }
        private Company selectedSinglePlant;
        public Company SelectedSinglePlant
        {
            get { return selectedSinglePlant; }
            set
            {
                selectedSinglePlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSinglePlant"));
                IsPlantUpdated = true;
            }
        }
        public PORegisteredDetailFilter Filter
        {
            get { return filter; }
            set { filter = value; }
        }
        public bool IsPOrequestsColumnChooserVisible
        {
            get { return isPOrequestsColumnChooserVisible; }
            set
            {
                isPOrequestsColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOrequestsColumnChooserVisible"));
            }
        }

        public bool IsPlantsListLoaded
        {
            get
            {
                return isPlantsListLoaded;
            }
            set
            {
                isPlantsListLoaded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlantsListLoaded"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public ObservableCollection<POStatus> POStatusList
        {
            get
            {
                return pOStatusList;
            }
            set
            {
                pOStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POStatusList"));
            }
        }
        public ObservableCollection<POType> POTypeList
        {
            get
            {
                return pOTypeList;
            }
            set
            {
                pOTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POTypeList"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public ObservableCollection<PORequestDetails> PoRequestesList
        {
            get
            {
                return poRequestList;
            }
            set
            {
                poRequestList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoRequestesList"));
            }
        }
        public PORequestDetails SelectedPoRequest
        {
            get
            {
                return selectedPoRequest;
            }
            set
            {
                selectedPoRequest = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPoRequest"));
            }
        }
        public ObservableCollection<POEmployeeInfo> POEmployeeInfoList
        {
            get
            {
                return pOEmployeeInfoList;
            }
            set
            {
                pOEmployeeInfoList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POEmployeeInfoList"));
            }
        }
        public ObservableCollection<CustomerContacts> CustomerContactsList
        {
            get
            {
                return customerContactsList;
            }
            set
            {
                customerContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerContactsList"));
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07.11.2024][GEOS2-6460]
        /// </summary>
        public ObservableCollection<PORegisteredDetails> PoRegisteredList
        {
            get
            {
                return poRegisteredList;
            }
            set
            {
                poRegisteredList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoRegisteredList"));
            }
        }

        public int SelectedGroup
        {
            get
            {
                return selectedGroup;
            }
            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedGroup"));
                if (selectedGroup > 0)
                {
                    CustomerPlants = new ObservableCollection<CustomerPlant>();
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList().GroupBy(cpl => cpl.IdCountry).Select(group => group.First()).ToList());
                    CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList());
                    if (CustomerPlants.Count > 0)
                        SelectedCustomerPlant = 1;
                    else
                        SelectedCustomerPlant = 0;
                }
                else
                {
                    SelectedCustomerPlant = -1;
                    CustomerPlants = null;
                }
            }
        }
        public int SelectedCustomerPlant
        {
            get
            {
                return selectedCustomerPlant;
            }
            set
            {
                selectedCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlant"));
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
        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }
            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }
        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }
            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
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

        public bool IsLeftBandVisible
        {
            get { return isLeftBandVisible; }
            set
            {
                isLeftBandVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLeftBandVisible"));
            }
        }

        
        public bool IsRightBandVisible
        {
            get { return isRightBandVisible; }
            set
            {
                isRightBandVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRightBandVisible"));
            }
        }

        public string CurrencyInSetting
        {
            get { return currencysetting; }
            set
            {
                currencysetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyInSetting"));
            }
        }

        public ObservableCollection<TileBarFilters> FilterStatusListOfTiles
        {
            get { return _filterStatusListOfTile; }
            set
            {
                _filterStatusListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStatusListOfTiles"));
            }
        }
        public ObservableCollection<TileBarFilters> FilterTypeListOfTiles
        {
            get { return _filterTypeListOfTile; }
            set
            {
                _filterTypeListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterTypeListOfTiles"));
            }
        }
        public DateTime StartSelectionDate
        {
            get { return startSelectionDate; }
            set
            {
                startSelectionDate = value;
                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("StartSelectionDate"));
            }
        }
        public DateTime FinishSelectionDate
        {
            get { return finishSelectionDate; }
            set
            {
                finishSelectionDate = value;
                if (!isFromFilterString)
                    UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("FinishSelectionDate"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public TileBarFilters SelectedTileBarItems
        {
            get { return _selectedTileBarItem; }
            set
            {
                _selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItems"));
            }
        }
        public TileBarFilters SelectedTypeTileBarItems
        {
            get { return _selectedTypeTileBarItem; }
            set
            {
                _selectedTypeTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTypeTileBarItems"));
            }
        }
        public string CustomFilterStringName { get; set; }
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }
        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }
        public int SelectedSearchIndex
        {
            get { return selectedSearchIndex; }
            set
            {
                selectedSearchIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSearchIndex"));
            }
        }
        public ObservableCollection<POType> TypeOfPos
        {
            get
            {
                return typeOfPos;
            }
            set
            {
                typeOfPos = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeOfPos"));
            }
        }
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return customers;
            }
            set
            {
                customers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customers"));
            }
        }
        public ObservableCollection<Currency> Currencies
        {
            get
            {
                return currencies;
            }
            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
            }
        }
        public ObservableCollection<CustomerPlant> CustomerPlants
        {
            get
            {
                return customerPlant;
            }
            set
            {
                customerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlants"));
            }
        }
        public ObservableCollection<CustomerPlant> EntireCompanyPlantList
        {
            get
            {
                return entireCompanyPlantList;
            }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public string PoValueRangeFrom
        {
            get { return poValueRangeFrom; }
            set
            {
                poValueRangeFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoValueRangeFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("PoValueRangeTo"));
            }
        }
        public string PoValueRangeTo
        {
            get { return poValueRangeTo; }
            set
            {
                poValueRangeTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoValueRangeTo"));
            }
        }
        public bool IsReceptionDate
        {
            get
            {
                return isReceptionDate;
            }
            set
            {
                isReceptionDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReceptionDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateTo"));
            }
        }
        public DateTime? ReceptionDateFrom
        {
            get { return receptionDateFrom; }
            set
            {
                receptionDateFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateTo")); // [nsatpute][01-12-2024][GEOS2-6462]
            }
        }
        public DateTime? ReceptionDateTo
        {
            get { return receptionDateTo; }
            set
            {
                receptionDateTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReceptionDateTo"));
            }
        }
        public bool IscreationDate
        {
            get
            {
                return iscreationDate;
            }
            set
            {
                iscreationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IscreationDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDateTo"));
            }
        }
        public DateTime? CreationDateFrom
        {
            get { return creationDateFrom; }
            set
            {
                creationDateFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDateTo")); // [nsatpute][01-12-2024][GEOS2-6462]
            }
        }
        public DateTime? CreationDateTo
        {
            get { return creationDateTo; }
            set
            {
                creationDateTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDateTo"));
            }
        }
        public bool IsupdateDate
        {
            get
            {
                return isupdateDate;
            }
            set
            {
                isupdateDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsupdateDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDateTo"));
            }
        }
        public DateTime? UpdateDateFrom
        {
            get { return updateDateFrom; }
            set
            {
                updateDateFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDateTo")); // [nsatpute][01-12-2024][GEOS2-6462]
            }
        }
        public DateTime? UpdateDateTo
        {
            get { return updateDateTo; }
            set
            {
                updateDateTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDateTo"));
            }
        }
        public DateTime? CancellationDateFrom
        {
            get { return cancellationDateFrom; }
            set
            {
                cancellationDateFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDateTo")); // [nsatpute][01-12-2024][GEOS2-6462]
            }
        }
        public DateTime? CancellationDateTo
        {
            get { return cancellationDateTo; }
            set
            {
                cancellationDateTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDateTo"));
            }
        }
        public List<string> POSender
        {
            get
            {
                return poSender;
            }
            set
            {
                poSender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POSender"));
            }
        }
        public List<object> SelectedCompanies
        {
            get
            {
                return selectedCompanies;
            }
            set
            {
                selectedCompanies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanies"));
            }
        }
        public string AmountCurrencySymbol
        {
            get { return amountCurrencySymbol; }
            set
            {
                amountCurrencySymbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountCurrencySymbol"));
            }
        }
        public virtual string TabName { get; set; }
        public virtual object TabContent { get; protected set; }
        public virtual object ParentViewModel { get; set; }
        private EventHandler _flyoutClosedHandler;
        #endregion
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion
        #region Constructor
        public PORequestsViewModel()
        {
            try
            {
                Processing();
                GeosApplication.Instance.Logger.Log("Constructor PORequestsViewModel....", category: Category.Info, priority: Priority.Low);
                OpenAttachmentCommand = new RelayCommand(new Action<object>(OpenAttachmentCommandAction));//[pooja.jadhav][17.02.2025][GEOS2-6724]    
                ExportPORequestsViewCommand = new RelayCommand(new Action<object>(ExportPORequestsViewAction));
                PrintPORequestsViewCommand = new RelayCommand(new Action<object>(PrintPORequestsViewAction));
                CommandPlantEditValueChanged = new RelayCommand(new Action<object>(PlantsListClosedCommandAction));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                //ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CommandPoRequestGridDoubleClick = new DelegateCommand<object>(EditPoRequestAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new RelayCommand(new Action<object>(ApplyCommandAction));
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                CloseButtonCommand = new DelegateCommand<object>(CloseButtonCommandAction);
                CommandFilterPOStatusTileClick = new RelayCommand(new Action<object>(CommandFilterPOStatusTileClickAction));
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandPOStatusTileBarClickDoubleClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandActions);
                POStatusList = new ObservableCollection<POStatus>(OTMService.OTM_GetAllPOWorkflowStatus());
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewunloadedLoadedCommandAction);
                RefreshPORequestsViewCommand = new RelayCommand(new Action<object>(RefreshPoRequestesList));
                UpdatedCommand = new DelegateCommand<object>(UpdatedCommandAction);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);               
                MyFilterString = string.Empty;
                IsPlantsListLoaded = true;
                ReceptionDateFrom = null;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                DateTime baseDate = DateTime.Today;
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                OTMCommon.Instance.FromDate = thisMonthStart.ToString(shortDateFormat);
                OTMCommon.Instance.ToDate = thisMonthEnd.ToString(shortDateFormat);
                POTypeList = new ObservableCollection<POType>(OTMService.OTM_GetAllPOTypeStatus());
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                SelectedSinglePlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                //OTMCommon.Instance.SelectedSinglePlant = SelectedSinglePlant;
                OTMCommon.Instance.SelectedSinglePlantForPO = SelectedSinglePlant;//[pramod.misal][GEOS2-9147][29.08.2025] https://helpdesk.emdep.com/browse/GEOS2-9147
                //IsLoaded = true;
                InitPoRequest();
                PORequestesGridSettingXamlFileDelete();//[pramod.misal][GEOS2-9196][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9196
                OTMCommon.Instance.IsPorequest = true;
                OTMCommon.Instance.IsRegisterPO = false;
                GeosApplication.Instance.Logger.Log("Constructor PORequestsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in PORequestsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Methods

        //[pramod.misal][GEOS2-9196][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9196

        private async void TableViewunloadedLoadedCommandAction(RoutedEventArgs obj)
        {
            //if (OTMCommon.Instance.IsPorequest)
            //{
                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);

            //}           

        }
        public async void PORequestesGridSettingXamlFileDelete()
        {

            //For PoRequest for V2.6.6.0
            List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();
            if (GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2660"))
            {
                if (GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2660"].ToString() == "0")
                {
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        File.Delete(OTM_PORequestesGridSettingFilePath);
                        GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2660"] = "1";
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    }
                }
            }


            //For PoRequest for V2.6.7.0 [pramod.misal][GEOS2-01-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9043]
            List<Tuple<string, string>> userConfigurations_V2670 = new List<Tuple<string, string>>();
            if (GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2670"))
            {
                if (GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2670"].ToString() == "0")
                {
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        File.Delete(OTM_PORequestesGridSettingFilePath);
                        GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2670"] = "1";
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            userConfigurations_V2670.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(userConfigurations_V2670, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    }
                }
            }

            //For PoRequest for V2.6.8.0 [pramod.misal][GEOS2-01-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9043]
            List<Tuple<string, string>> userConfigurations_V2680 = new List<Tuple<string, string>>();
            if (GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2680"))
            {
                if (GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2680"].ToString() == "0")
                {
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        File.Delete(OTM_PORequestesGridSettingFilePath);
                        GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2680"] = "1";
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            userConfigurations_V2670.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(userConfigurations_V2670, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    }
                }
            }

            //For PoRequest for V2.6.8.0 [pramod.misal][GEOS2-01-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9043]
            List<Tuple<string, string>> userConfigurations_V2690 = new List<Tuple<string, string>>();
            if (GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2690"))
            {
                if (GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2690"].ToString() == "0")
                {
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        File.Delete(OTM_PORequestesGridSettingFilePath);
                        GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2690"] = "1";
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            userConfigurations_V2670.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(userConfigurations_V2670, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    }
                }
            }
        }

        public void Dispose()
        {
        }
        private void FilterEditorCreatedCommandActions(FilterEditorEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction()...", category: Category.Info, priority: Priority.Low);
                ShowProcessing();
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowFilterEditor(obj);
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method FilterEditorCreatedCommandAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                customFilterEditorViewModel.Init(e.FilterControl, FilterStatusListOfTiles);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();
                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        FilterStatusListOfTiles.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");
                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        VisibleRowCount = gridControl.VisibleRowCount;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");
                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }
                        SelectedTileBarItems = FilterStatusListOfTiles.LastOrDefault();
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    VisibleRowCount = gridControl.VisibleRowCount;
                    FilterStatusListOfTiles.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });
                    string filterName = "";
                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedTileBarItems = FilterStatusListOfTiles.LastOrDefault();
                }
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][04.02.2025][GEOS2 - 6726]
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {

            try
            {
                #region DateTime

                #region DateTime old code
                //if (e.Column.FieldName == "DateTime")
                //{

                //    List<object> filterItems = new List<object>();

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("IsNull([DateTime])")
                //    });

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("!IsNull([DateTime])")
                //    });



                //    HashSet<DateTime> uniqueDates = new HashSet<DateTime>();

                //    foreach (var poRequest in PoRequestesList)
                //    {
                //        if (poRequest.DateTime != null && poRequest.DateTime != DateTime.MinValue)
                //        {
                //            DateTime dateValue = poRequest.DateTime;

                //            if (uniqueDates.Add(dateValue))
                //            {
                //                filterItems.Add(new CustomComboBoxItem
                //                {
                //                    DisplayValue = dateValue.ToString("dd-MM-yyyy"), // Format date for display
                //                    DisplayValue = dateValue.ToString("d", CultureInfo.CurrentCulture),
                //                    EditValue = CriteriaOperator.Parse("DateTime = ?", dateValue)
                //                });
                //            }
                //        }
                //    }

                //    e.ComboBoxEdit.ItemsSource = filterItems
                //       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                //       .ToList();
                //}

                //if (e.Column.FieldName == "DateTime")
                //{
                //    List<object> filterItems = new List<object>();

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("IsNull([DateTime])")
                //    });

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("!IsNull([DateTime])")
                //    });

                //    HashSet<DateTime> uniqueDates = new HashSet<DateTime>();

                //    foreach (var poRequest in PoRequestesList)
                //    {
                //        if (poRequest.DateTime != null && poRequest.DateTime != DateTime.MinValue)
                //        {
                //            DateTime dateOnly = poRequest.DateTime.Date;

                //            if (uniqueDates.Add(dateOnly))
                //            {
                //                DateTime nextDay = dateOnly.AddDays(1);

                //                filterItems.Add(new CustomComboBoxItem
                //                {
                //                    DisplayValue = dateOnly.ToString("d", CultureInfo.CurrentCulture),
                //                    EditValue = CriteriaOperator.Parse("[DateTime] >= ? AND [DateTime] < ?", dateOnly, nextDay)
                //                });
                //            }
                //        }
                //    }

                //    e.ComboBoxEdit.ItemsSource = filterItems
                //       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                //       .ToList();
                //}

                #endregion 

                if (e.Column.FieldName == "Date")
                {
                    List<object> filterItems = new List<object>();

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Date])")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Date])")
                    });

                    Dictionary<DateTime, CustomComboBoxItem> dateItemsMap = new Dictionary<DateTime, CustomComboBoxItem>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (poRequest.DateTime != null && poRequest.DateTime != DateTime.MinValue)
                        {
                            DateTime dateOnly = poRequest.DateTime.Date;

                            if (!dateItemsMap.ContainsKey(dateOnly))
                            {
                                DateTime nextDay = dateOnly.AddDays(1);

                                var item = new CustomComboBoxItem
                                {
                                    DisplayValue = dateOnly.ToString("d", CultureInfo.CurrentCulture),
                                    EditValue = CriteriaOperator.Parse("[Date] >= ? AND [Date] < ?", dateOnly, nextDay)
                                };

                                dateItemsMap[dateOnly] = item;
                            }
                        }
                    }

                    // Add sorted date items (latest first)
                    var sortedDateItems = dateItemsMap
                        .OrderByDescending(kv => kv.Key)
                        .Select(kv => (object)kv.Value);

                    // Insert sorted items after blanks/non-blanks
                    filterItems.AddRange(sortedDateItems);

                    e.ComboBoxEdit.ItemsSource = filterItems;
                }


                if (e.Column.FieldName == "Time")
                {
                    List<object> filterItems = new List<object>();

                    // Blank
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Time])")
                    });

                    // Non blank
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Not IsNullOrEmpty([Time])")
                    });

                    // Collect unique time strings
                    HashSet<string> timeSet = new HashSet<string>();

                    foreach (var po in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(po.Time))
                        {
                            // Ensure format HH:mm (cut seconds if coming from DB)
                            string timeOnly = po.Time.Length >= 5 ? po.Time.Substring(0, 5) : po.Time;

                            if (!timeSet.Contains(timeOnly))
                                timeSet.Add(timeOnly);
                        }
                    }

                    // Sort descending (latest time first)
                    var sortedTimes = timeSet
                        .OrderByDescending(t => t)
                        .Select(t => new CustomComboBoxItem
                        {
                            DisplayValue = t,
                            EditValue = CriteriaOperator.Parse("[Time] = ?", t)
                        });

                    // Add to list
                    foreach (var item in sortedTimes)
                        filterItems.Add(item);

                    e.ComboBoxEdit.ItemsSource = filterItems;
                }








                #endregion

                #region ToRecipientName [pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                if (e.Column.FieldName == "ToRecipientName")
                {
                    List<object> filterItems = new List<object>();
                    HashSet<string> uniqueNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([ToRecipientName])")   // ToRecipientName is equal to ' '
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([ToRecipientName])")
                        EditValue = CriteriaOperator.Parse("ToRecipientName Like '%{0}%'",null)

                        

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([ToRecipientName])")
                        EditValue = CriteriaOperator.Parse("ToRecipientName Not Like '%{0}%'", null)


                    });

                    try
                    {
                        //PoRequestesList
                        // Iterate through the PO request list
                        foreach (var poRequest in PoRequestesList)
                        {
                            if (poRequest.ToRecipientNameList != null)
                            {
                                foreach (var recipient in poRequest.ToRecipientNameList)
                                {
                                    if (!string.IsNullOrWhiteSpace(recipient.RecipientName))
                                    {
                                        // Split names by Alessandro Giuseppe%'\")"}
                                        var names = recipient.RecipientName.Split(',')
                                                    .Select(name => name.Trim());

                                        foreach (var name in names)
                                        {
                                            try
                                            {
                                                if (!uniqueNames.Contains(name))
                                                {
                                                    uniqueNames.Add(name);

                                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                    customComboBoxItem.DisplayValue = name.Trim();
                                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("ToRecipientName Like '%{0}%'", name));

                                                    filterItems.Add(customComboBoxItem);

                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                                
                                            }

                                            
                                        }
                                    }
                                }
                            }

                        }

                        e.ComboBoxEdit.ItemsSource = filterItems
                               .OrderBy(item => Convert.ToString(((CustomComboBoxItem)item).DisplayValue))
                               .ToList();
                    }
                    catch (Exception ex)
                    {

                        
                    }
                    

                }
                #endregion

                #region CCName [pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                if (e.Column.FieldName == "CCName")
                {
                    List<object> filterItems = new List<object>();
                    HashSet<string> uniqueNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);



                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([ToRecipientName])")   // ToRecipientName is equal to ' '
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([CCName])")
                        EditValue = CriteriaOperator.Parse("CCName Like '%{0}%'", null)
                       
                    });
                  

                    filterItems.Add(new CustomComboBoxItem()
                    {   
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([CCName])")
                        EditValue = CriteriaOperator.Parse("CCName Not Like '%{0}%'", null)
                        
                    });

                    try
                    {
                        // Iterate through the PO request list
                        foreach (var poRequest in PoRequestesList)
                        {
                            if (poRequest.CCNameList != null)
                            {
                                foreach (var recipient in poRequest.CCNameList)
                                {
                                    if (!string.IsNullOrWhiteSpace(recipient.CCName))
                                    {
                                        // Split names by comma and trim spaces
                                        var names = recipient.CCName.Split(',')
                                                    .Select(name => name.Trim());

                                        foreach (var name in names)
                                        {
                                            try
                                            {
                                                if (!uniqueNames.Contains(name))
                                                {
                                                    uniqueNames.Add(name); // Add to HashSet to prevent duplicates

                                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                    customComboBoxItem.DisplayValue = name.Trim();
                                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CCName Like '%{0}%'", name));
                                                    filterItems.Add(customComboBoxItem);

                                                }

                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        
                    }
                    

                    e.ComboBoxEdit.ItemsSource = filterItems
                           .OrderBy(item => Convert.ToString(((CustomComboBoxItem)item).DisplayValue))
                           .ToList();

                }

                #endregion

                #region SenderName
                if (e.Column.FieldName == "SenderName")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([SenderName])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.SenderName))
                        {
                            string trimmedName = poRequest.SenderName.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"SenderName Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Subject
                if (e.Column.FieldName == "Subject")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty(Trim([Subject]))")
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Subject])") // Removed Trim

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        //EditValue = CriteriaOperator.Parse("!IsNullOrEmpty(Trim([Subject]))")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Subject])") // Removed Trim

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Subject))
                        {
                            string trimmedName = poRequest.Subject;

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    //EditValue = CriteriaOperator.Parse($"Subject Like '%{trimmedName}%'")
                                    //EditValue = CriteriaOperator.Parse("Subject Like ?", $"%{trimmedName.Replace("'", "''")}%")
                                    EditValue = CriteriaOperator.Parse("Contains([Subject], ?)", trimmedName)

                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Requester
                if (e.Column.FieldName == "Requester")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Requester])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Requester])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Ensures case-insensitive uniqueness

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Requester))
                        {
                            // Split the names based on ',' and trim spaces
                            string[] names = poRequest.Requester.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var name in names)
                            {
                                string trimmedName = name.Trim(); // Remove extra spaces

                                if (!string.IsNullOrWhiteSpace(trimmedName) && uniqueNames.Add(trimmedName)) // Add only if unique
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("Requester Like ?", $"%{trimmedName}%") // Matches partial values
                                    });
                                }
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region AttachmentCnt
                if (e.Column.FieldName == "AttachmentCnt")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([AttachmentCnt])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([AttachmentCnt])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.AttachmentCnt))
                        {
                            string trimmedName = poRequest.AttachmentCnt.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"AttachmentCnt Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Attachements
                if (e.Column.FieldName == "Attachments")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Attachments])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Attachments])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Ensures case-insensitive uniqueness

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Attachments))
                        {
                            // Split the names based on ',' and trim spaces
                            string[] names = poRequest.Attachments.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var name in names)
                            {
                                string trimmedName = name.Trim(); // Remove extra spaces

                                if (!string.IsNullOrWhiteSpace(trimmedName) && uniqueNames.Add(trimmedName)) // Add only if unique
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("Attachments Like ?", $"%{trimmedName}%") // Matches partial values
                                    });
                                }
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region PO Found
                if (e.Column.FieldName == "POFound")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([POFound])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([POFound])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.POFound))
                        {
                            string trimmedName = poRequest.POFound.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"POFound Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Offer
                if (e.Column.FieldName == "Offer")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Offer])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Offer])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Offer))
                        {
                            string trimmedName = poRequest.Offer.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"Offer Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Inbox
                if (e.Column.FieldName == "Inbox")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Inbox])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Inbox])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Inbox))
                        {
                            string trimmedName = poRequest.Inbox;

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"Inbox Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }
                #endregion
                // [nsatpute][10-03-2025][GEOS2-6722]
                #region Customer
                if (e.Column.FieldName == "Customer")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Customer])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Customer])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Customer))
                        {
                            // Split the customer string by line breaks and process each customer
                            string[] customers = poRequest.Customer.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var customer in customers)
                            {
                                string trimmedName = customer.Trim();

                                if (uniqueNames.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse($"Customer Like '%{trimmedName}%'")
                                    });
                                }
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region InvoioceTO
                if (e.Column.FieldName == "InvoioceTO")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([InvoioceTO])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([InvoioceTO])")

                    });


                    HashSet<string> uniqueInvoiceTO = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.InvoioceTO))
                        {
                            // Split the invoice string by line breaks and process each invoice
                            string[] invoices = poRequest.InvoioceTO.Split(new[] { "\r\n" }, StringSplitOptions.None);
                           
                            foreach (var invoice in invoices)
                            {
                                string trimmedName = invoice.Trim();
                                if (uniqueInvoiceTO.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("InvoioceTO Like ?", $"%{trimmedName}%")
                                    });
                                }
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region DateIssued
                if (e.Column.FieldName == "DateIssued")
                {


                    List<object> filterItems = new List<object>();

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([DateIssued])")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([DateIssued])")
                    });



                    HashSet<DateTime> uniqueDates = new HashSet<DateTime>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (poRequest.DateIssued.HasValue && poRequest.DateIssued.Value != DateTime.MinValue)
                        {
                            DateTime dateValue = poRequest.DateIssued.Value.Date; // Use only Date part to ensure uniqueness

                            if (uniqueDates.Add(dateValue)) // Add only if unique
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = dateValue.ToString("d", CultureInfo.CurrentCulture), // Format based on system culture
                                    EditValue = CriteriaOperator.Parse("DateIssued = ?", dateValue) // Ensure proper filtering
                                });
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                       .ToList();
                }

                #endregion

                #region Number
                if (e.Column.FieldName == "PONumber")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([PONumber])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([PONumber])")

                    });

                    HashSet<string> uniquePONumbers = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.PONumber))
                        {
                            // Split the PO Number string by line breaks and process each PO Number
                            string[] poNumbers = poRequest.PONumber.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var poNumber in poNumbers)
                            {
                                string trimmedName = poNumber.Trim();

                                if (uniquePONumbers.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse($"PONumber Like '%{trimmedName}%'")
                                    });
                                }
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region offer
                //if (e.Column.FieldName == "Offers")
                //{
                //    List<object> filterItems = new List<object>();
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                //        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Offers])")

                //    });

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                //        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Offers])")

                //    });

                //    HashSet<string> uniqueOffers = new HashSet<string>();

                //    foreach (var poRequest in PoRequestesList)
                //    {
                //        if (!string.IsNullOrWhiteSpace(poRequest.Offers))
                //        {
                //            // Split the offers string by line breaks and process each offer
                //            string[] offers = poRequest.Offers.Split(new[] { "\r\n" }, StringSplitOptions.None);

                //            foreach (var offer in offers)
                //            {
                //                string trimmedName = offer.Trim();

                //                if (uniqueOffers.Add(trimmedName))
                //                {
                //                    filterItems.Add(new CustomComboBoxItem
                //                    {
                //                        DisplayValue = trimmedName,
                //                        EditValue = CriteriaOperator.Parse($"Offers Like '%{trimmedName}%'")
                //                    });
                //                }
                //            }
                //        }
                //    }


                //    e.ComboBoxEdit.ItemsSource = filterItems
                //       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                //    .ToList();
                //}

                #endregion

                #region Contact
                if (e.Column.FieldName == "Contact")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Contact])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Contact])")

                    });

                    HashSet<string> uniqueContacts = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Contact))
                        {
                            // Split the contact string by line breaks and process each contact
                            string[] contacts = poRequest.Contact.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var contact in contacts)
                            {
                                string trimmedName = contact.Trim();

                                if (uniqueContacts.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse($"Contact Like '%{trimmedName}%'")
                                    });
                                }
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region TransferAmount
                if (e.Column.FieldName == "TransferAmount")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([TransferAmount])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([TransferAmount])")

                    });

                    HashSet<double> uniqueAmounts = new HashSet<double>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (poRequest.TransferAmount.HasValue) // Ensure TransferAmount is not null
                        {
                            // Use the value of TransferAmount
                            double amount = poRequest.TransferAmount.Value;

                            if (uniqueAmounts.Add(amount)) // Add only unique values
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = amount.ToString("N2"), // Format as needed
                                    EditValue = CriteriaOperator.Parse($"[TransferAmount] = {amount}")
                                });
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Currency
                if (e.Column.FieldName == "Currency")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Currency])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Currency])")

                    });

                    HashSet<string> uniqueCurrency = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Currency))
                        {
                            // Split the currency string by line breaks and process each currency
                            string[] currencies = poRequest.Currency.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var currency in currencies)
                            {
                                string trimmedName = currency.Trim();

                                if (uniqueCurrency.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse($"Currency Like '%{trimmedName}%'")
                                    });
                                }
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Ship to
                if (e.Column.FieldName == "ShipTo")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([ShipTo])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([ShipTo])")

                    });

                    HashSet<string> uniqueShipTo = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.ShipTo))
                        {
                            string[] shipTos = poRequest.ShipTo.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var ship in shipTos)
                            {
                                string trimmedShipTo = ship.Trim();
                                string escapedShipTo = trimmedShipTo.Replace("'", "''");
                                if (uniqueShipTo.Add(escapedShipTo))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = escapedShipTo,
                                        EditValue = CriteriaOperator.Parse($"ShipTo Like '%{escapedShipTo}%'")
                                    });
                                }
                            }
                        }

                    }
                    e.ComboBoxEdit.ItemsSource = filterItems
                        .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                        .ToList();
                }

                #endregion

                #region POIncoterms
                if (e.Column.FieldName == "POIncoterms")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([POIncoterms])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([POIncoterms])")

                    });

                    HashSet<string> uniquePOIncoterms = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.POIncoterms))
                        {
                            // Split the PO Incoterms string by line breaks and process each Incoterm
                            string[] incoterms = poRequest.POIncoterms.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var incoterm in incoterms)
                            {
                                string trimmedIncoterm = incoterm.Trim();

                                if (uniquePOIncoterms.Add(trimmedIncoterm))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedIncoterm,
                                        EditValue = CriteriaOperator.Parse($"POIncoterms Like '%{trimmedIncoterm}%'")
                                    });
                                }
                            }
                        }
                    }


                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region POPaymentTerm
                if (e.Column.FieldName == "POPaymentTerm")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([POPaymentTerm])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([POPaymentTerm])")

                    });

                    HashSet<string> uniquePOPaymentTerm = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.POPaymentTerm))
                        {
                            // Split the payment term string by line breaks and process each payment term
                            string[] paymentTerms = poRequest.POPaymentTerm.Split(new[] { "\r\n" }, StringSplitOptions.None);

                            foreach (var paymentTerm in paymentTerms)
                            {
                                string trimmedPaymentTerm = paymentTerm.Trim();

                                if (uniquePOPaymentTerm.Add(trimmedPaymentTerm))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedPaymentTerm,
                                        EditValue = CriteriaOperator.Parse($"POPaymentTerm Like '%{trimmedPaymentTerm}%'")
                                    });
                                }
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region PORequestStatus
                if (e.Column.FieldName == "PORequestStatus.Value")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([PORequestStatus.Value])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([PORequestStatus.Value])")

                    });

                    HashSet<string> uniquePOPaymentTerm = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (poRequest.PORequestStatus != null)
                        {
                            if (!string.IsNullOrWhiteSpace(poRequest.PORequestStatus.Value))
                            {
                                string trimmedPOStatus = poRequest.PORequestStatus.Value.Trim();
                                string escapedPOStatus = trimmedPOStatus.Replace("'", "''");

                                if (uniquePOPaymentTerm.Add(trimmedPOStatus))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedPOStatus,
                                        EditValue = CriteriaOperator.Parse($"PORequestStatus.Value Like '%{escapedPOStatus}%'")
                                    });
                                }
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Group
                if (e.Column.FieldName == "Group")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Group])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Group])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Group))
                        {
                            string trimmedName = poRequest.Group.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"Group Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region Plant
                if (e.Column.FieldName == "Plant")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Plant])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Plant])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Plant))
                        {
                            string trimmedName = poRequest.Plant.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"Plant Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion

                #region LinkedOffers
                if (e.Column.FieldName == "Offers")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        //EditValue = CriteriaOperator.Parse("IsNullOrEmpty([SenderName])")   // ToRecipientName is equal to ' '
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Offers])")

                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        //EditValue = CriteriaOperator.Parse("!IsNull([SenderName])")
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Offers])")

                    });

                    HashSet<string> uniqueNames = new HashSet<string>();

                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Offers))
                        {
                            string trimmedName = poRequest.Offers.Trim();

                            if (uniqueNames.Add(trimmedName))
                            {
                                filterItems.Add(new CustomComboBoxItem
                                {
                                    DisplayValue = trimmedName,
                                    EditValue = CriteriaOperator.Parse($"Offers Like '%{trimmedName}%'")
                                    //EditValue = CriteriaOperator.Parse("Offer Like ?", $"%{trimmedName.Replace("'", "''")}%")

                                    //EditValue = CriteriaOperator.Parse($"Offer Like '%{trimmedName}%'")
                                });
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems
                       .OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString())
                    .ToList();
                }

                #endregion
                #region Offers
                if (e.Column.FieldName == "Offers")
                {
                    List<object> filterItems = new List<object>();

                    // Blank
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNullOrEmpty([Offers])")
                    });

                    // Non-blank
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([Offers])")
                    });

                    // Unique individual names from comma-separated values
                    HashSet<string> uniqueNames = new HashSet<string>();
                    foreach (var poRequest in PoRequestesList)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.Offers))
                        {
                            string[] names = poRequest.Offers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var name in names)
                            {
                                string trimmedName = name.Trim();
                                if (!string.IsNullOrWhiteSpace(trimmedName) && uniqueNames.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("Offers Like ?", $"%{trimmedName}%")
                                    });
                                }
                            }
                        }
                    }

                    // Final assignment (REMOVE the null line!)
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString()).ToList();
                }

                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenAttachmentCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()...", category: Category.Info, priority: Priority.Low);

                ShowProcessing();


                Emailattachment Attachment = (Emailattachment)obj;
                //OTMService = new OTMServiceController("localhost:6699");
                Attachment.FileDocInBytes = OTMService.GetPoAttachmentByte(Attachment.AttachmentPath);
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                string fileExtension = Path.GetExtension(Attachment.AttachmentPath).ToLower();

                if (Attachment.FileDocInBytes != null)
                {
                    documentViewModel.OpenPORequestAttachment(Attachment);
                    if (documentViewModel.IsPresent)
                    {
                        documentView.DataContext = documentViewModel;
                        CloseProcessing();
                        documentView.Show();
                    }
                    GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CloseProcessing();
                    CustomMessageBox.Show(string.Format("Could not find file {0}", Attachment.AttachmentName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    
                }
                

            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttachmentCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }  
        //[pramod.misal][GEOS2-6460][28-11-2024]
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
        private void UpdatedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdatedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj != null && obj is TableView && IsUpdate == true)
                {
                    TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    AddPORegisteredCustomSettingCount(gridControl);
                    IsUpdate = false;
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdatedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportPORequestsViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPORequestsViewAction()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PO Requests List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();
                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    ShowProcessing();
                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportCompletedOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCompletedOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintPORequestsViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCompletedOrderList()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                ShowProcessing();
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PORequestsPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PORequestsPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCompletedOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCompletedOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }      
        public void InitPoRequest()//[GEOS2-6861][rdixit][16.01.2025]
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method InitPoRequest()...", category: Category.Info, priority: Priority.Low);
                Processing();
                FillPOEmployeeInfo(); //[pramod.misal][19.02.2025][GEOS2 - 6719]
                FillPoRequestesListByPlant();
                FillPOStausTilebar();               
                AddCustomSetting();
                MyFilterString = string.Empty;
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method InitPoRequest()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRequest() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRequest() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method InitPoRequest()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }      
        public void FillPOEmployeeInfo()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitPoRegister()...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                POEmployeeInfoList = new ObservableCollection<POEmployeeInfo>(OTMService.GetPOEmployeeInfoList_V2610());
                GeosApplication.Instance.Logger.Log("Method InitPoRegister()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRegister() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRegister() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method InitPoRegister()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FlyoutControl_Closed(object sender, EventArgs e, GridControl grid, MenuFlyout menu)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                //flyout.Closed -= (sender1, e1) => FlyoutControl_Closed(sender, e, grid);
                UnsubscribeFlyoutClosed(menu);
                Processing();
                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;
                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                if (IsButtonStatus == 1)//this month
                {
                    OTMCommon.Instance.FromDate = thisMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = thisMonthEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    OTMCommon.Instance.FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    OTMCommon.Instance.FromDate = lastMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = lastMonthEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    OTMCommon.Instance.FromDate = thisWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = thisWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    OTMCommon.Instance.FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    OTMCommon.Instance.FromDate = lastWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = lastWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    OTMCommon.Instance.FromDate = StartDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = EndDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;

                }
                else if (IsButtonStatus == 8)//this year
                {
                    // setDefaultPeriod();
                    DateTime StartMDate = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime EndToMDate = new DateTime(DateTime.Now.Year, 12, 31);
                    OTMCommon.Instance.FromDate = StartMDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = EndToMDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 9)//last year
                {
                    OTMCommon.Instance.FromDate = StartFromDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = EndToDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    OTMCommon.Instance.FromDate = Date_F.ToShortDateString();
                    OTMCommon.Instance.ToDate = Date_T.ToShortDateString();
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 0) //custome range
                {
                    OTMCommon.Instance.FromDate = StartDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDate = EndDate.ToString(shortDateFormat);
                    //IsButtonStatus = 0;
                }
                IsUpdate = true;
                InitPoRequest();
                IsBusy = false;
                //TableView detailView = (TableView)obj;
                //GridControl gridControl = (obj).Grid;
                AddCustomSettingCount(grid);
                //TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void UnsubscribeFlyoutClosed(MenuFlyout menu)
        {
            if (_flyoutClosedHandler != null)
            {
                menu.FlyoutControl.Closed -= _flyoutClosedHandler;
            }
        }
        private Action Processing()
        {
            IsBusy = true;
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
            return null;
        }
        private void PlantsListClosedCommandAction(object obj)
        {
            if (OTMCommon.Instance.IsPlantUpdated)
            {
                try
                {
                    SelectedSinglePlant = OTMCommon.Instance.SelectedSinglePlantForPO;//[pramod.misal][GEOS2-9147][29.08.2025] https://helpdesk.emdep.com/browse/GEOS2-9147
                    GeosApplication.Instance.Logger.Log("Method PlantsListClosedCommandAction ...", category: Category.Info, priority: Priority.Low);
                    IsPlantsListLoaded = true;
                    if (SelectedSinglePlant == null)
                        return;
                    //Processing();
                    TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    InitPoRequest();
                    AddCustomSettingCount(gridControl);
                    OTMCommon.Instance.IsPlantUpdated = false;
                    //CloseProcessing();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method PlantsListClosedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;
            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Visible;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }
        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);
                Object[] objArray = (Object[])obj;
                MenuFlyout menu = (MenuFlyout)objArray[0];
                GridControl grid = (GridControl)objArray[1];
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                //menu.FlyoutControl.Closed += FlyoutControl_Closed;
                // Define a named delegate for the Closed event
                _flyoutClosedHandler = (sender, e) => FlyoutControl_Closed(sender, e, grid, menu);
                // Subscribe to the event using the named delegate
                menu.FlyoutControl.Closed += _flyoutClosedHandler;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }
        //[rdixit][25.11.2024][GEOS2-6460]
        private void CloseButtonCommandAction(object obj)
        {
            if (obj is PO)
            {
                //try
                //{
                //    OTMMainViewModel.PosList = new ObservableCollection<PO>();
                //    if ((obj as PO).Header == "Registered PO")
                //    {
                //        OTMMainViewModel.PosList.Add(new PO()
                //        {
                //            Header = "PO Requests",
                //            PORequestDetailsList = new ObservableCollection<PORequestDetails>(),
                //            PORegisteredDetailsList = new ObservableCollection<PORegisteredDetails>()
                //        });
                //    }
                //    else
                //    {
                //        OTMMainViewModel.PosList.Add(new PO()
                //        {
                //            Header = "Registered PO",
                //            PORequestDetailsList = new ObservableCollection<PORequestDetails>(),
                //            PORegisteredDetailsList = new ObservableCollection<PORegisteredDetails>()
                //        });
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                try
                {
                    if (OTMMainViewModel.PosList == null)
                        return;
                    var itemToRemove = OTMMainViewModel.PosList.FirstOrDefault(p => p.Header == (obj as PO).Header);
                    if (itemToRemove != null)
                    {
                        OTMMainViewModel.PosList.Remove(itemToRemove);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);
                StringBuilder builder = new StringBuilder();
                //foreach (WorkflowStatus item in TripStatusList)
                //{
                //    builder.Append("'").Append(item.Name).Append("'" + ",");
                //}
                string result = builder.ToString();
                result = result.TrimEnd(',');
                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    string st = string.Format("[ArrivalDate] >= #{0}# And [DepartutreDate] <= #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                    st += string.Format(" And [Status] In ( " + result + ")");
                    MyFilterString = st;
                }
                else
                {
                    MyFilterString = string.Format("[Status] In ( " + result + ")");
                }
                GeosApplication.Instance.Logger.Log("UpdateFilterString executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateFilterString() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public void FillPOStausTilebar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPOStausTilebar()...", category: Category.Info, priority: Priority.Low);
                FilterStatusListOfTiles = new ObservableCollection<TileBarFilters>();
                ObservableCollection<POStatus> tempStatusList = new ObservableCollection<POStatus>();
                int _id = 1;
                if (PoRequestesList != null)
                {
                    var DttableList = PoRequestesList.AsEnumerable().ToList();
                    List<int> idOfferStatusTypeList = DttableList.Select(x => (int)x.IdPORequestStatus).Distinct().ToList();
                    foreach (int item in idOfferStatusTypeList)
                    {
                        POStatus status = POStatusList.FirstOrDefault(x => x.IdLookupValue == item);
                        if (status != null)
                            tempStatusList.Add(status);
                    }
                    FilterStatusListOfTiles.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("POReuestsReportTileBarCaption").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = PoRequestesList.Count,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });
                    foreach (var item in tempStatusList)
                    {
                        FilterStatusListOfTiles.Add(new TileBarFilters()
                        {
                            Caption = item.Name,
                            Id = _id++,
                            IdOfferStatusType = item.IdLookupValue,
                            BackColor = item.HtmlColor,
                            FilterCriteria = "[PORequestStatus.Value] In ('" + item.Name + "')",
                            ForeColor = item.HtmlColor,
                            EntitiesCount = (DttableList.Where(x => (int)x.IdPORequestStatus == item.IdLookupValue).ToList()).Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }
                FilterStatusListOfTiles.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("POReportCustomFilter").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    FilterCriteria = (System.Windows.Application.Current.FindResource("POReportCustomFilter").ToString()),
                    Height = 30,
                    width = 200,
                });
                if (FilterStatusListOfTiles.Count > 0)
                    SelectedTileBarItems = FilterStatusListOfTiles[0];
                GeosApplication.Instance.Logger.Log("Method FillPOStausTilebar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillPOStausTilebar() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        private void CommandFilterPOStatusTileClickAction(object obj)
        {
            //if (!IsLoaded)
            //{
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFilterPOStatusTileClickAction....", category: Category.Info, priority: Priority.Low);
                if (FilterStatusListOfTiles != null)
                {
                    if (FilterStatusListOfTiles.Count > 0)
                    {
                        var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                        if (temp.AddedItems.Count > 0)
                        {
                            var selectedFilter = temp.AddedItems[0] as TileBarFilters;
                            if (selectedFilter == null) return;
                            string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                            string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                            string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                            CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                            if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("POReportCustomFilter").ToString()))
                                return;
                            if (str == null)
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                {
                                    if (!string.IsNullOrEmpty(_FilterString))
                                    {
                                        if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                                        {
                                            string st = string.Format("[ArrivalDate] >= #{0}# And [DepartutreDate] <= #{1}# And", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                                            st += _FilterString;
                                            MyFilterString = st;
                                        }
                                        else
                                            MyFilterString = _FilterString;
                                    }
                                    else
                                        MyFilterString = string.Empty;
                                }
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                            {
                                if (str.Equals("All"))
                                {
                                    MyFilterString = string.Empty;
                                    PoRequestesList = PoRequestesList;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(_FilterString))
                                    {
                                        if (!string.IsNullOrEmpty(_FilterString))
                                            MyFilterString = _FilterString;
                                        else
                                            MyFilterString = string.Empty;
                                    }
                                    else
                                        MyFilterString = string.Empty;
                                }
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CommandFilterPOStatusTileClickAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterPOStatusTileClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            //}
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        private void CommandPOStatusTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandPOStatusTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in POStatusList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }
                if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("POReportCustomFilter").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("POReuestsReportTileBarCaption").ToString()))
                {
                    return;
                }
                DevExpress.Xpf.Grid.TableView table = (DevExpress.Xpf.Grid.TableView)obj;
                GridControl gridControl = (table).Grid;
                gridControl.FilterString = FilterStatusListOfTiles?.FirstOrDefault(i => i.Caption == CustomFilterStringName)?.FilterCriteria;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandPOStatusTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandPOStatusTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-9358][05.11.2025] https://helpdesk.emdep.com/browse/GEOS2-9358
        private async void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                gridControl.BeginInit();

                // Attach serializer allow property handler
                gridControl.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                // Restore layout only after UI tree is ready
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        try
                        {
                            gridControl.RestoreLayoutFromXml(OTM_PORequestesGridSettingFilePath);
                            GeosApplication.Instance.Logger.Log("Layout restored successfully.", category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Layout restore failed: " + ex.Message, category: Category.Warn, priority: Priority.Low);
                        }
                    }
                }, System.Windows.Threading.DispatcherPriority.Loaded);

                // ✅ Reconcile fixed columns and bands after restore
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        ReconcileBandsAndFixedColumns(gridControl);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("ReconcileBandsAndFixedColumns failed: " + ex.Message, category: Category.Warn, priority: Priority.Low);
                    }
                }, System.Windows.Threading.DispatcherPriority.Background);

                // Add property change handlers
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    descriptor?.AddValueChanged(column, VisibleChanged);

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    descriptorColumnPosition?.AddValueChanged(column, VisibleIndexChanged);
                }

                gridControl.EndInit();
                gridControl.RefreshData();
                gridControl.UpdateLayout();

                // Add total summary
                TableView detailView = (TableView)obj.OriginalSource;
                detailView.ShowTotalSummary = false; //[pramod.misal][GEOS2-10064][05.11.2025] https://helpdesk.emdep.com/browse/GEOS2-10064
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.Add(new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}"
                });

                CloseProcessing();
                IsPOrequestsColumnChooserVisible = false;
                AddCustomSettingCount(gridControl);

                GeosApplication.Instance.Logger.Log("TableViewLoadedCommandAction executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in TableViewLoadedCommandAction: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsLoaded = true;
        }

        private List<string> GetLeftFixedColumnNamesFromXml(string xmlPath)
        {
            var leftFixedColumns = new List<string>();

            if (!File.Exists(xmlPath))
                return leftFixedColumns;

            try
            {
                var xdoc = XDocument.Load(xmlPath);

                // Find all bands where Fixed = Left
                var leftBands = xdoc.Descendants("property")
                    .Where(x => (string)x.Attribute("name") == "Fixed" && (string)x.Value == "Left")
                    .Select(x => x.Parent) // Go up to band node
                    .Distinct();

                foreach (var band in leftBands)
                {
                    // Find all <property name="FieldName"> within this band
                    var fieldNames = band.Descendants("property")
                        .Where(x => (string)x.Attribute("name") == "FieldName")
                        .Select(x => x.Value)
                        .ToList();

                    leftFixedColumns.AddRange(fieldNames);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error parsing XML for fixed left columns: {ex.Message}",
                    category: Category.Warn, priority: Priority.Low);
            }

            return leftFixedColumns.Distinct().ToList();
        }

        private List<string> GetRightFixedColumnNamesFromXml(string xmlPath)
        {
            var rightFixedColumns = new List<string>();

            if (!File.Exists(xmlPath))
                return rightFixedColumns;

            try
            {
                var xdoc = XDocument.Load(xmlPath);

                var rightBands = xdoc.Descendants("property")
                    .Where(x => (string)x.Attribute("name") == "Fixed" && (string)x.Value == "Right")
                    .Select(x => x.Parent)
                    .Distinct()
                    .ToList();

                foreach (var band in rightBands)
                {
                    var fieldNames = band
                        .Descendants("property")
                        .Where(x => (string)x.Attribute("name") == "FieldName")
                        .Select(x => x.Value)
                        .ToList();

                    rightFixedColumns.AddRange(fieldNames);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(
                    $"Error parsing XML for right fixed columns: {ex.Message}",
                    category: Category.Warn,
                    priority: Priority.Low);
            }

            return rightFixedColumns.Distinct().ToList();
        }


        //
        private void ReconcileBandsAndFixedColumns(GridControl grid)
        {
            if (grid == null) return;

            // Get or create fixed bands
            GridControlBand leftBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedLeftBand")
                ?? new GridControlBand { Header = "Pinned Left", Tag = "fixedLeftBand", Fixed = FixedStyle.Left };
            if (!grid.Bands.Contains(leftBand)) grid.Bands.Add(leftBand);
            leftBand.Columns.Clear();
            leftBand.Visible = false;

            GridControlBand rightBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedRightBand")
                ?? new GridControlBand { Header = "Pinned Right", Tag = "fixedRightBand", Fixed = FixedStyle.Right };
            if (!grid.Bands.Contains(rightBand)) grid.Bands.Add(rightBand);
            rightBand.Columns.Clear();
            rightBand.Visible = false;

            // ✅ Parse XML
            var leftFixedColumns = GetLeftFixedColumnNamesFromXml(OTM_PORequestesGridSettingFilePath);
            var rightFixedColumns = GetRightFixedColumnNamesFromXml(OTM_PORequestesGridSettingFilePath);
            var columnVisibility = GetColumnVisibilityFromXml(OTM_PORequestesGridSettingFilePath); // new helper

            IsLeftBandVisible = GetBandVisibilityFromXml(OTM_PORequestesGridSettingFilePath, "Left");
            IsRightBandVisible = GetBandVisibilityFromXml(OTM_PORequestesGridSettingFilePath, "Right");

            if (leftFixedColumns.Count == 0)
                IsLeftBandVisible = false;
            if (rightFixedColumns.Count == 0)
                IsRightBandVisible = false;

            leftBand.Visible = IsLeftBandVisible;
            rightBand.Visible = IsRightBandVisible;

            foreach (var col in grid.Columns.ToList())
            {
                GridControlBand parentBand = col.ParentBand as GridControlBand;

                // ✅ Read visibility state from XML (default true)
                bool isVisible = !columnVisibility.ContainsKey(col.FieldName) || columnVisibility[col.FieldName];

                // Apply left-fixed logic
                if (leftFixedColumns.Contains(col.FieldName))
                {
                    if (parentBand != null && parentBand != leftBand)
                        parentBand.Columns.Remove(col);
                    if (!leftBand.Columns.Contains(col))
                        leftBand.Columns.Add(col);

                    col.Fixed = FixedStyle.Left;
                    col.Visible = isVisible;
                    continue;
                }

                // Apply right-fixed logic
                if (rightFixedColumns.Contains(col.FieldName))
                {
                    if (parentBand != null && parentBand != rightBand)
                        parentBand.Columns.Remove(col);
                    if (!rightBand.Columns.Contains(col))
                        rightBand.Columns.Add(col);

                    col.Fixed = FixedStyle.Right;
                    col.Visible = isVisible;
                    continue;
                }

                // Normal column
                var originalBand = FixedColumnHelper.GetParentBand(col);
                if (originalBand != null && parentBand != originalBand)
                {
                    parentBand?.Columns.Remove(col);
                    originalBand.Columns.Add(col);
                    originalBand.Visible = true;
                }

                col.Fixed = FixedStyle.None;
                col.Visible = isVisible;
            }
        }


        private Dictionary<string, bool> GetColumnVisibilityFromXml(string filePath)
        {
            var visibilityDict = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var doc = XDocument.Load(filePath);
                var properties = doc.Descendants("property")
                                    .Where(x => x.Attribute("name")?.Value == "FieldName")
                                    .Select(x => x.Parent);

                foreach (var prop in properties)
                {
                    var fieldName = prop?.Elements("property")
                                         .FirstOrDefault(p => p.Attribute("name")?.Value == "FieldName")?.Value;
                    var visibleValue = prop?.Elements("property")
                                           .FirstOrDefault(p => p.Attribute("name")?.Value == "Visible")?.Value;

                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        bool visible = string.Equals(visibleValue, "true", StringComparison.OrdinalIgnoreCase);
                        visibilityDict[fieldName] = visible;
                    }
                }
            }
            catch
            {
                // ignore parsing errors
            }
            return visibilityDict;
        }


        private bool GetBandVisibilityFromXml(string xmlFilePath, string fixedSide)
        {
            if (!File.Exists(xmlFilePath))
                return true; // default visible

            try
            {
                var xml = XDocument.Load(xmlFilePath);

                // Find all <property> elements with a child <property name="Fixed">Left/Right</property>
                var bandNodes = xml.Descendants("property")
                    .Where(p => p.Elements("property")
                        .Any(c =>
                            (string)c.Attribute("name") == "Fixed" &&
                            string.Equals((string)c.Value, fixedSide, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                foreach (var bandNode in bandNodes)
                {
                    // Look for <property name="Visible">...</property> inside this band
                    var visibleElement = bandNode.Elements("property")
                        .FirstOrDefault(x => (string)x.Attribute("name") == "Visible");

                    if (visibleElement != null)
                    {
                        var visibleValue = visibleElement.Value.Trim();
                        return string.Equals(visibleValue, "true", StringComparison.OrdinalIgnoreCase);
                    }
                }

                // default visible if not found
                return true;
            }
            catch (Exception ex)
            {
                
                return true;
            }
        }






        //Working proper
        private void ReconcileBandsAndFixedColumns2810(GridControl grid)
        {
            if (grid == null) return;

            // Get or create fixed bands
            GridControlBand leftBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedLeftBand")
                ?? new GridControlBand { Header = "Fixed Left", Tag = "fixedLeftBand", Fixed = FixedStyle.Left };
            if (!grid.Bands.Contains(leftBand)) grid.Bands.Add(leftBand);
            leftBand.Columns.Clear();
            leftBand.Visible = false;

            GridControlBand rightBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedRightBand")
                ?? new GridControlBand { Header = "Fixed Right", Tag = "fixedRightBand", Fixed = FixedStyle.Right };
            if (!grid.Bands.Contains(rightBand)) grid.Bands.Add(rightBand);
            rightBand.Columns.Clear();
            rightBand.Visible = false;

            // ✅ Parse XML to find left fixed columns
            var leftFixedColumns = GetLeftFixedColumnNamesFromXml(OTM_PORequestesGridSettingFilePath);
            var leftRightColumns = GetRightFixedColumnNamesFromXml(OTM_PORequestesGridSettingFilePath);



            foreach (var col in grid.Columns.ToList())
            {
                GridControlBand parentBand = col.ParentBand as GridControlBand;
                //FixedStyle colFixed = FixedColumnHelper.GetFixedState(col);

                if (leftFixedColumns.Contains(col.FieldName))
                {
                    if (parentBand != null && parentBand != leftBand)
                        parentBand.Columns.Remove(col);

                    if (!leftBand.Columns.Contains(col))
                        leftBand.Columns.Add(col);

                    col.Fixed = FixedStyle.Left;
                    col.Visible = true;
                    continue;
                }
                if (leftRightColumns.Contains(col.FieldName))
                {
                    if (parentBand != null && parentBand != rightBand)
                        parentBand.Columns.Remove(col);

                    if (!rightBand.Columns.Contains(col))
                        rightBand.Columns.Add(col);

                    col.Fixed = FixedStyle.Right;
                    col.Visible = true;
                    continue;
                }
                else
                {
                    var originalBand = FixedColumnHelper.GetParentBand(col);
                    if (originalBand != null && parentBand != originalBand)
                    {
                        parentBand?.Columns.Remove(col);
                        originalBand.Columns.Add(col);
                        originalBand.Visible = true;
                    }

                    col.Fixed = FixedStyle.None;
                    col.Visible = true;
                }
            }

            leftBand.Visible = leftBand.Columns.Count > 0;
            rightBand.Visible = rightBand.Columns.Count > 0;
            
        }



        private void ReconcileBandsAndFixedColumns2(GridControl grid)
        {
            if (grid == null) return;

            // Get or create fixed bands
            GridControlBand leftBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedLeftBand");
            if (leftBand == null)
            {
                leftBand = new GridControlBand { Header = "Fixed Left", Tag = "fixedLeftBand", Fixed = FixedStyle.Left };
                grid.Bands.Add(leftBand);
            }
            leftBand.Columns.Clear();
            leftBand.Visible = false;

            GridControlBand rightBand = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == "fixedRightBand");
            if (rightBand == null)
            {
                rightBand = new GridControlBand { Header = "Fixed Right", Tag = "fixedRightBand", Fixed = FixedStyle.Right };
                grid.Bands.Add(rightBand);
            }
            rightBand.Columns.Clear();
            rightBand.Visible = false;

            foreach (var col in grid.Columns.ToList())
            {
                // Only process columns that have Fixed = Left or Right
                if (col.Fixed == FixedStyle.Left)
                {
                    // Remove from current parent band if needed
                    if (col.ParentBand is GridControlBand currentBand && currentBand != leftBand)
                        currentBand.Columns.Remove(col);

                    if (!leftBand.Columns.Contains(col))
                        leftBand.Columns.Add(col);

                    col.Visible = true;
                }
                else if (col.Fixed == FixedStyle.Right)
                {
                    // Remove from current parent band if needed
                    if (col.ParentBand is GridControlBand currentBand && currentBand != rightBand)
                        currentBand.Columns.Remove(col);

                    if (!rightBand.Columns.Contains(col))
                        rightBand.Columns.Add(col);

                    col.Visible = true;
                }
                else
                {
                    // Columns that are not fixed should stay in original band
                    var originalParent = FixedColumnHelper.GetParentBand(col);
                    if (originalParent != null && col.ParentBand != originalParent)
                    {
                        // Remove from current band
                        if (col.ParentBand is GridControlBand currentBand)
                            currentBand.Columns.Remove(col);

                        originalParent.Columns.Add(col);
                        originalParent.Visible = true;
                        col.Visible = true;
                    }
                    else
                    {
                        // Column is not fixed and has no original parent, just ensure visible
                        col.Visible = true;
                    }
                }
            }


            // Show fixed bands only if they have columns
            leftBand.Visible = leftBand.Columns.Count > 0;
            rightBand.Visible = rightBand.Columns.Count > 0;
        }



        private FixedStyle GetFixedFromXml(string fieldName)
        {
            if (!File.Exists(OTM_PORequestesGridSettingFilePath))
                return FixedStyle.None;

            var doc = XDocument.Load(OTM_PORequestesGridSettingFilePath);

            var fieldNode = doc.Descendants("property")
                .FirstOrDefault(x =>
                    x.Element("FieldName") != null &&
                    x.Element("FieldName").Value == fieldName);

            if (fieldNode != null)
            {
                var fixedNode = fieldNode.Parent?.Parent?.Element("Fixed"); // go 2 levels up to band
                if (fixedNode != null && fixedNode.Value == "Left")
                    return FixedStyle.Left;
                if (fixedNode != null && fixedNode.Value == "Right")
                    return FixedStyle.Right;
            }

            return FixedStyle.None;
        }



        private void ReconcileBandsAndFixedColumns1(GridControl grid)
        {
            if (grid == null) return;

            // Find or create fixed bands
            GridControlBand leftFixedBand = grid.Bands.FirstOrDefault(b => (b.Tag?.ToString() == "fixedLeftBand"));
            GridControlBand rightFixedBand = grid.Bands.FirstOrDefault(b => (b.Tag?.ToString() == "fixedRightBand"));

            if (leftFixedBand == null)
            {
                leftFixedBand = new GridControlBand() { Header = "Fixed Left", Tag = "fixedLeftBand", Fixed = FixedStyle.Left };
                grid.Bands.Add(leftFixedBand);
            }

            if (rightFixedBand == null)
            {
                rightFixedBand = new GridControlBand() { Header = "Fixed Right", Tag = "fixedRightBand", Fixed = FixedStyle.Right };
                grid.Bands.Add(rightFixedBand);
            }

            // Move columns back to correct band based on Fixed property
            foreach (GridColumn col in grid.Columns.ToList())
            {
                try
                {
                    if (col.Fixed == FixedStyle.Left)
                    {
                        if (col.ParentBand == null || col.ParentBand.Fixed != FixedStyle.Left)
                        {
                            (col.ParentBand as GridControlBand)?.Columns.Remove(col);
                            leftFixedBand.Columns.Add(col);
                        }
                    }
                    else if (col.Fixed == FixedStyle.Right)
                    {
                        if (col.ParentBand == null || col.ParentBand.Fixed != FixedStyle.Right)
                        {
                            (col.ParentBand as GridControlBand)?.Columns.Remove(col);
                            rightFixedBand.Columns.Add(col);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Reconcile column " + col.FieldName + ": " + ex.Message, category: Category.Warn, priority: Priority.Low);
                }
            }
        }

    




        private async void TableViewLoadedCommandActionold(RoutedEventArgs obj)
        {
           
                try
                {
                    GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                    int visibleFalseCoulumn = 0;
                    GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    gridControl.BeginInit();
                    if (File.Exists(OTM_PORequestesGridSettingFilePath))
                    {
                        await Task.Run(() =>
                        {
                            // UI components must not be accessed directly here
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                gridControl.RestoreLayoutFromXml(OTM_PORequestesGridSettingFilePath);
                            });
                        });

                    }
                    await Task.Run(() =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            gridControl.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                        });
                    });

                    //This code for save grid layout.
                    await Task.Run(() =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            gridControl.SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
                        });
                    });

                    foreach (GridColumn column in gridControl.Columns)
                    {
                        DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                        if (descriptor != null)
                        {
                            descriptor.AddValueChanged(column, VisibleChanged);
                        }
                        DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                        if (descriptorColumnPosition != null)
                        {
                            descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                        }
                        if (column.Visible == false)
                        {
                            visibleFalseCoulumn++;
                        }
                    }
                    CloseProcessing();
                    IsPOrequestsColumnChooserVisible = false;

                    TableView datailView = ((TableView)((RoutedEventArgs)obj).OriginalSource);
                    datailView.ShowTotalSummary = true;
                    gridControl.TotalSummary.Clear();
                    gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });

                    //[002] End
                    gridControl.EndInit();
                    ((GridViewBase)obj.OriginalSource).Grid.RefreshData();
                    ((GridViewBase)obj.OriginalSource).Grid.UpdateLayout();
                    AddCustomSettingCount(gridControl);
                    GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
           
            IsLoaded = true;           
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.Property.Name == "Fixed" || e.Property.Name == "Tag" || e.Property.Name == "Visible" || e.Property.Name == "VisibleIndex")
                e.Allow = true;
        }


        // [pramod.misal][04-10-2024][GEOS2-6520]
        private void OnAllowPropertyold(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;
                if (e.DependencyProperty == UI.Helper.TableViewEx.SearchStringProperty)
                    e.Allow = false;
                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    var view = column.View as TableView;
                    GridControl gridControl = view.DataControl as GridControl;
                    gridControl.SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
                    ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                var view = column.View as TableView;
                GridControl gridControl = view.DataControl as GridControl;
                gridControl.SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
                ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        private async void AddCustomSettingCount(GridControl gridControl)
        {
            try
            {
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                foreach (var item in tempUserSettings)
                {
                    try
                    {
                        MyFilterString = FilterStatusListOfTiles.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        var updatedCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                        FilterStatusListOfTiles
                            .Where(j => j.FilterCriteria == item.Value)
                            .ToList()
                            .ForEach(j => j.EntitiesCount = updatedCount);
                        // FilterStatusListOfTiles.ForEach(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSettingCount() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method AddCustomSettingCount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSettingCount() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [ashish.malkhede][14-11-2024][GEOS2-6460]
        private void AddPORegisteredCustomSettingCount(GridControl gridControl)
        {
            try
            {
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey_PORegistered)).ToList();
                foreach (var item in tempUserSettings)
                {
                    try
                    {
                        MyFilterString = FilterTypeListOfTiles.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        var updatedCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                        FilterTypeListOfTiles
                            .Where(j => j.FilterCriteria == item.Value)
                            .ToList()
                            .ForEach(j => j.EntitiesCount = updatedCount);
                        // FilterStatusListOfTiles.ForEach(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddPORegisteredCustomSettingCount() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method AddCustomSettingCount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSettingCount() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        int count = 0;
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        if (FilterStatusListOfTiles == null)
                        {
                            FilterStatusListOfTiles = new ObservableCollection<TileBarFilters>();
                        }
                        FilterStatusListOfTiles.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsKey, ""),
                                Id = 0,
                                BackColor = null,
                                ForeColor = null,
                                FilterCriteria = item.Value,
                                EntitiesCount = count,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public void RefreshPoRequestesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPoRequestesList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                IsBusy = true;
                ShowProcessing();
                InitPoRequest();
                gridControl.RefreshData();
                gridControl.UpdateLayout();
                AddCustomSettingCount(gridControl);
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Method RefreshPoRequestesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPoRequestesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }   
        
        void ShowProcessing()
        {
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
        }

        void CloseProcessing()
        {
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private BitmapImage IconFromExtension(string fileExtension)
        {
            BitmapImage bitmapImage = new BitmapImage();
            if (GeosApplication.FileExtentionIcon == null)
                GeosApplication.FileExtentionIcon = new Dictionary<string, BitmapImage>();
            if (GeosApplication.FileExtentionIcon.Any(i => i.Key.ToString().ToLower() == fileExtension.ToLower()))
            {
                bitmapImage = GeosApplication.FileExtentionIcon.FirstOrDefault(i => i.Key.ToString().ToLower() == fileExtension.ToLower()).Value;
                return bitmapImage;
            }
            else
            {
                // Create a temporary file with the given extension to extract the icon.
                Guid newGuid = Guid.NewGuid();
                string tempFile = Path.Combine(Path.GetTempPath(), newGuid + fileExtension);
                // Create an empty file with the specified extension if it doesn't exist.
                if (!File.Exists(tempFile))
                {
                    File.Create(tempFile).Dispose(); // Create and immediately close the file
                }
                // Extract the associated icon from the file
                Icon icon = Icon.ExtractAssociatedIcon(tempFile);
                // Convert the Icon to a BitmapImage with transparency
                if (icon != null)
                {
                    using (Bitmap bitmap = icon.ToBitmap())
                    {
                        // Make the background transparent by modifying the Bitmap's pixel format
                        bitmap.MakeTransparent(System.Drawing.Color.Transparent); // Replace black with transparency
                        using (MemoryStream iconStream = new MemoryStream())
                        {
                            bitmap.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png); // Save as PNG to retain transparency
                            iconStream.Seek(0, SeekOrigin.Begin); // Reset the stream position
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = iconStream;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            // Cleanup temp file
                            if (File.Exists(tempFile))
                            {
                                File.Delete(tempFile);
                            }
                            GeosApplication.FileExtentionIcon.Add(fileExtension, bitmapImage);
                            return bitmapImage;
                        }
                    }
                }
            }
            return null; // Return null if no icon is found
        }
        private void FillPoRequestesListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeTripsListByPlant ...", category: Category.Info, priority: Priority.Low);
                PoRequestesList = new ObservableCollection<PORequestDetails>();
                ObservableCollection<PORequestDetails> TempMainPORequestList = new ObservableCollection<PORequestDetails>();
                long plantIds;
                string plantConnection;
                // Added By [Rahul.Gadhave][GEOS2-6461][Date:12-11-2024]
                int CurrentCurrencyId = 0;
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                var selectedCurrency = GeosApplication.Instance.Currencies.SingleOrDefault(cur => cur.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString());
                if (selectedCurrency != null)
                {
                    CurrentCurrencyId = selectedCurrency.IdCurrency;
                }
                //End
                if (SelectedSinglePlant != null)
                {
                    try
                    {
                        
                        plantIds = SelectedSinglePlant.IdCompany;
                        plantConnection = SelectedSinglePlant.ConnectPlantConstr;               
                        if (SelectedSinglePlant == null)
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                            Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                            OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                        }
                        else
                        {
                            Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == SelectedSinglePlant.IdCompany);
                            OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                                selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        }
                        try
                        {
                            //OTMService = new OTMServiceController("localhost:6699");
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2580(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection));
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2610_V1(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection));
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2630(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection));
                            //[pramod.misal][GEOS2-7247][02.05.2025]
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2640(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection));
                            //[Rahul.Gadhave][GEOS2-8339][Date:06-06-2025]
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2650(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds));
                            //[pramod.misal][GEOS2-8772][Date:30-06-2025]https://helpdesk.emdep.com/browse/GEOS2-8772
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2660(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds));
                            //TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2670(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds));

                            //[pramod.misal][20.11.2025][PredefinedGeometryStock2DModel-9429] https://helpdesk.emdep.com/browse/GEOS2-9429  Commented GetPORequestDetails_V2670 for GEOS2-9429
                            TempMainPORequestList = new ObservableCollection<PORequestDetails>(OTMService.GetPORequestDetails_V2690(DateTime.ParseExact(OTMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDate, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds));
                            OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                            //[ashish.malkhede][17.03.2025][GEOS2 - 7042]
                            #region Optimize code

                            try
                            {
                                foreach (var x in TempMainPORequestList)
                                {
                                    // Initialize EmailAttachmentList if it's null
                                    if (x.EmailAttachmentList == null)
                                    {
                                        x.EmailAttachmentList = new List<Emailattachment>();
                                    }
                                    // Initialize EmailAttachmentDetailsList if it's null
                                    if (x.EmailAttachmentDetailsList == null)
                                    {
                                        x.EmailAttachmentDetailsList = new List<EmailAttachmentDetails>();
                                    }

                                    // Split attachments only once
                                    var attachments = x.Attachments.Split(';');

                                    // Determine the directory path if applicable
                                    string directoryPath = !string.IsNullOrEmpty(x.Path) && Path.IsPathRooted(x.Path)
                                                           ? Path.GetDirectoryName(x.Path)
                                                           : string.Empty;

                                    // Collect Emailattachments in a list
                                    List<Emailattachment> emailAttachments = new List<Emailattachment>(attachments.Length);
                                    try
                                    {
                                        foreach (var att in attachments)
                                        {
                                            if (string.IsNullOrEmpty(att)) continue;

                                            string filePath = string.IsNullOrEmpty(directoryPath) ? string.Empty : Path.Combine(directoryPath, att);

                                            // Add the attachment details to the list
                                            emailAttachments.Add(new Emailattachment()
                                            {
                                                //IdAttachementType = x.IdAttachementType,
                                                AttachmentName = att,
                                                AttachmentImage = IconFromExtension(Path.GetExtension(att)),
                                                AttachmentCnt = string.Empty, // Placeholder for attachment count

                                                AttachmentPath = filePath
                                            });
                                        }
                                        // Set AttachmentCnt for the first item (or any other logic you prefer)
                                        if (emailAttachments.Count > 0)
                                        {
                                            emailAttachments[0].AttachmentCnt = x.AttachmentCnt;
                                        }

                                        // Add to the EmailAttachmentList (AddRange minimizes allocation overhead)
                                        x.EmailAttachmentList.AddRange(emailAttachments);

                                        // Add EmailAttachmentDetails to the EmailAttachmentDetailsList
                                        x.EmailAttachmentDetailsList.Add(new EmailAttachmentDetails()
                                        {
                                            AttachmentCnt = x.AttachmentCnt,
                                            AttachmentList = emailAttachments
                                        });
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                           
                            #endregion

                           
                            //[ashish.malkhede][GEOS2-7724][02-04-2025]
                            #region new code sender, to and cc
                            foreach (var x in TempMainPORequestList)
                            {
                                if (long.TryParse(x.SenderIdPerson, out long parsedId))
                                {
                                    var employeeInfo = POEmployeeInfoList.FirstOrDefault(e => e.IdEmployee == parsedId);

                                    if (employeeInfo != null)
                                    {
                                        x.SenderEmployeeCode = employeeInfo.EmployeeCode;
                                        x.SenderJobDescriptionTitle = employeeInfo.JobDescriptionTitle;
                                    }

                                }
                            }

                            foreach (var x in TempMainPORequestList)
                            {
                                List<string> employeeCodes = new List<string>();
                                if (!string.IsNullOrEmpty(x.ToIdPerson))
                                {

                                    var ToIds = x.ToIdPerson.Split(',');
                                    //List<string> employeeCodes = new List<string>();
                                    foreach (var id in ToIds)
                                    {
                                        if (id == "null")
                                        {
                                            employeeCodes.Add(null);
                                        }
                                        else
                                        {
                                            if (long.TryParse(id, out long parsedId))
                                            {
                                                var employeeInfo = POEmployeeInfoList.FirstOrDefault(e => e.IdEmployee == parsedId);

                                                if (employeeInfo != null)
                                                {
                                                    employeeCodes.Add(employeeInfo.EmployeeCode);
                                                }
                                            }
                                        }

                                    }
                                    x.ToRecipientNameEmployeeCodes = string.Join(",", employeeCodes);

                                }
                                else if (!string.IsNullOrEmpty(x.Recipient)) //[pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                                {

                                    var ccEmails = x.Recipient.Split(';'); // 
                                    foreach (var email in ccEmails)
                                    {
                                        var employeeInfo = POEmployeeInfoList.FirstOrDefault(e =>
                                            !string.IsNullOrEmpty(e.Email) &&
                                            e.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
                                        
                                        if (employeeInfo != null)
                                        {
                                            employeeCodes.Add(employeeInfo.EmployeeCode);
                                        }
                                        
                                    }
                                    x.ToRecipientNameEmployeeCodes = string.Join(",", employeeCodes);
                                    

                                }

                                

                            }
                            foreach (var x in TempMainPORequestList)
                            {
                                List<string> employeeCodes = new List<string>();
                                if (!string.IsNullOrEmpty(x.CCIdPerson) && x.CCIdPerson != null)
                                {
                                    var CCIds = x.CCIdPerson.Split(',');
                                   // List<string> employeeCodes = new List<string>();
                                    foreach (var id in CCIds)
                                    {
                                        if (id == "null")
                                        {
                                            employeeCodes.Add(null);
                                        }
                                        else
                                        {
                                            if (long.TryParse(id, out long parsedId))
                                            {
                                                var employeeInfo = POEmployeeInfoList.FirstOrDefault(e => e.IdEmployee == parsedId);

                                                if (employeeInfo != null)
                                                {
                                                    employeeCodes.Add(employeeInfo.EmployeeCode);
                                                }

                                            }
                                        }
                                    }
                                    x.CCNameEmployeeCodes = string.Join(",", employeeCodes);

                                }
                                else
                                {
                                    var ccEmails = x.CCRecipient.Split(';'); // [pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                                    foreach (var email in ccEmails)
                                    {
                                        var employeeInfo = POEmployeeInfoList.FirstOrDefault(e =>
                                            !string.IsNullOrEmpty(e.Email) &&
                                            e.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

                                        if (employeeInfo != null)
                                        {
                                            employeeCodes.Add(employeeInfo.EmployeeCode);
                                        }
                                    }

                                    x.CCNameEmployeeCodes = string.Join(",", employeeCodes);
                                }
                               
                            }
                            #endregion

                            #region [pramod.misal][04.02.2025][GEOS2-6726]
                            TempMainPORequestList.ForEach(x =>
                            {
                                //var matchingEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.FullName == x.ToRecipientName);
                                if (!string.IsNullOrEmpty(x.ToRecipientName))
                                {
                                    List<string> recipientName = x.ToRecipientName?.Split(',').ToList() ?? new List<string>();
                                    List<string> recipientMail = x.Recipient?.Split(';').ToList() ?? new List<string>();
                                    List<string> empCode = x.ToRecipientNameEmployeeCodes?.Split(',').ToList() ?? new List<string>();
                                    //List<string> jdTitle = x.ToRecipientNameJobDescriptionTitles?.Split(',').ToList() ?? new List<string>();
                                    //List<string> jdTitle = POEmployeeInfoList.Where(emp => recipientName.Contains(emp.FullName)).Select(emp => emp.JobDescriptionTitle).ToList();
                                    List<string> jdTitle = new List<string>();
                                    foreach (var item in empCode)
                                    {
                                        //var temp =    POEmployeeInfoList.FirstOrDefault(emp => item == emp.EmployeeCode.Trim());
                                        jdTitle.Add(POEmployeeInfoList
                                                                    .Where(emp => item == emp.EmployeeCode.TrimStart())
                                                                    .Select(emp => emp.JobDescriptionTitle).FirstOrDefault());
                                    }
                                    x.ToRecipientNameList = new List<ToRecipientName>();
                                    ToRecipientName rec = null;

                                    for (int i = 0; i < recipientName.Count; i++)
                                    {
                                        rec = new ToRecipientName();
                                        rec.RecipientName = recipientName[i];
                                        rec.Recipientmail = recipientMail.Count > i ? recipientMail[i] : null;
                                        rec.ToRecipientNameEmployeeCodes = empCode.Count > i ? empCode[i] : null;
                                        rec.ToRecipientNameJobDescriptionTitles = jdTitle.Count > i ? jdTitle[i] : null;
                                        if (string.IsNullOrEmpty(rec.RecipientName) && !string.IsNullOrEmpty(rec.Recipientmail))
                                        {
                                            rec.RecipientName = rec.Recipientmail;
                                        }
                                        x.ToRecipientNameList.Add(rec);
                                    }
                                }
                                else//[pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                                {
                                    if (string.IsNullOrEmpty(x.ToRecipientName))
                                    {
                                        List<string> recipientName = x.ToRecipientName?.Split(',').ToList() ?? new List<string>();
                                        List<string> recipientMail = x.Recipient?.Split(';').ToList() ?? new List<string>();
                                        List<string> empCode = x.ToRecipientNameEmployeeCodes?.Split(',').ToList() ?? new List<string>();
                                        //List<string> jdTitle = x.ToRecipientNameJobDescriptionTitles?.Split(',').ToList() ?? new List<string>();
                                        //List<string> jdTitle = POEmployeeInfoList.Where(emp => recipientName.Contains(emp.FullName)).Select(emp => emp.JobDescriptionTitle).ToList();
                                        List<string> jdTitle = new List<string>();
                                        foreach (var item in empCode)
                                        {
                                            //var temp =    POEmployeeInfoList.FirstOrDefault(emp => item == emp.EmployeeCode.Trim());
                                            jdTitle.Add(POEmployeeInfoList
                                                                        .Where(emp => item == emp.EmployeeCode.TrimStart())
                                                                        .Select(emp => emp.JobDescriptionTitle).FirstOrDefault());
                                        }
                                        x.ToRecipientNameList = new List<ToRecipientName>();
                                        ToRecipientName rec = null;



                                        foreach (var name in recipientMail)
                                        {
                                            if (!string.IsNullOrEmpty(name))
                                            {
                                                //var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => string.Equals(emp.FullName, name, StringComparison.OrdinalIgnoreCase));
                                                var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.Email?.ToLower().Contains(name.ToLower()) == true);
                                                rec = new ToRecipientName();
                                                if (matchedEmployee != null)
                                                {
                                                    //rec = new ToRecipientName();
                                                    rec.RecipientName = matchedEmployee.FullName;
                                                    if (string.IsNullOrEmpty(rec.RecipientName))
                                                    {
                                                        rec.RecipientName = matchedEmployee.Email;
                                                    }


                                                    rec.ToRecipientNameEmployeeCodes = matchedEmployee.EmployeeCode;
                                                    rec.ToRecipientNameJobDescriptionTitles = matchedEmployee.JobDescriptionTitle;
                                                    rec.Recipientmail = matchedEmployee.Email;

                                                    x.ToRecipientNameList.Add(rec);
                                                }
                                                else
                                                {
                                                    rec.RecipientName = name;
                                                    rec.Recipientmail = name;
                                                    x.ToRecipientNameList.Add(rec);

                                                }

                                            }
                                            //else
                                            //{
                                            //    if (!string.IsNullOrEmpty(name))
                                            //    {
                                            //        rec.RecipientName = name;
                                            //        rec.Recipientmail = name;
                                            //        x.ToRecipientNameList.Add(rec);

                                            //    }
                                                    

                                            //}





                                        }


                                    }
                                }
                                if (!string.IsNullOrEmpty(x.CCName))
                                {
                                    //var matchingEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.FullName == x.CCName);
                                    List<string> ccNames = x.CCName?.Split(',').ToList() ?? new List<string>();
                                    List<string> ccMails = x.CCRecipient?.Split(';').ToList() ?? new List<string>();
                                    List<string> ccEmployeeCodes = x.CCNameEmployeeCodes?.Split(',').ToList() ?? new List<string>();
                                    //List<string> ccJobDescriptionTitles = x.CCNameJobDescriptionTitles?.Split(',').ToList() ?? new List<string>();
                                    //List<string> ccJobDescriptionTitles = POEmployeeInfoList.Where(emp => ccNames.Contains(emp.FullName)).Select(emp => emp.JobDescriptionTitle).ToList();
                                    List<string> ccJobDescriptionTitles = new List<string>();
                                    foreach (var item in ccEmployeeCodes)
                                    {
                                        //var temp =    POEmployeeInfoList.FirstOrDefault(emp => item == emp.EmployeeCode.Trim());
                                        ccJobDescriptionTitles.Add(POEmployeeInfoList
                                                                    .Where(emp => item == emp.EmployeeCode.TrimStart())
                                                                    .Select(emp => emp.JobDescriptionTitle).FirstOrDefault());
                                    }
                                    x.CCNameList = new List<ToCCName>();
                                    ToCCName cc = null;
                                    for (int i = 0; i < ccNames.Count; i++)
                                    {
                                        cc = new ToCCName();
                                        cc.CCName = ccNames[i];
                                        cc.CCEmail = ccMails.Count > i ? ccMails[i] : null;
                                        cc.CCNameEmployeeCodes = ccEmployeeCodes.Count > i ? ccEmployeeCodes[i] : null;
                                        cc.CCNameJobDescriptionTitles = ccJobDescriptionTitles.Count > i ? ccJobDescriptionTitles[i] : null;
                                        if (string.IsNullOrEmpty(cc.CCName) && !string.IsNullOrEmpty(cc.CCEmail))
                                        {
                                            cc.CCName = cc.CCEmail;
                                        }
                                        x.CCNameList.Add(cc);
                                    }
                                }
                                else //[pramod.misal][GEOS2-9318][26-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9318
                                {
                                    if (string.IsNullOrEmpty(x.CCName))
                                    {
                                        //var matchingEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.FullName == x.CCName);
                                        List<string> ccNames = x.CCName?.Split(',').ToList() ?? new List<string>();
                                        List<string> ccMails = x.CCRecipient?.Split(';').ToList() ?? new List<string>();
                                        List<string> ccEmployeeCodes = x.CCNameEmployeeCodes?.Split(',').ToList() ?? new List<string>();
                                        //List<string> ccJobDescriptionTitles = x.CCNameJobDescriptionTitles?.Split(',').ToList() ?? new List<string>();
                                        //List<string> ccJobDescriptionTitles = POEmployeeInfoList.Where(emp => ccNames.Contains(emp.FullName)).Select(emp => emp.JobDescriptionTitle).ToList();
                                        List<string> ccJobDescriptionTitles = new List<string>();
                                        foreach (var item in ccEmployeeCodes)
                                        {
                                            //var temp =    POEmployeeInfoList.FirstOrDefault(emp => item == emp.EmployeeCode.Trim());
                                            ccJobDescriptionTitles.Add(POEmployeeInfoList
                                                                        .Where(emp => item == emp.EmployeeCode.TrimStart())
                                                                        .Select(emp => emp.JobDescriptionTitle).FirstOrDefault());
                                        }
                                        x.CCNameList = new List<ToCCName>();
                                        ToCCName cc = null;
                                
                                        foreach (var name in ccMails)
                                        {
                                            //var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => string.Equals(emp.FullName, name, StringComparison.OrdinalIgnoreCase));
                                            if (!string.IsNullOrEmpty(name))
                                            {
                                                var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.Email?.ToLower().Contains(name.ToLower()) == true);
                                                cc = new ToCCName();
                                                if (matchedEmployee != null)
                                                {
                                                    //rec = new ToRecipientName();
                                                    cc.CCName = matchedEmployee.FullName;
                                                    if (string.IsNullOrEmpty(cc.CCName))
                                                    {
                                                        cc.CCName = matchedEmployee.Email;
                                                    }


                                                    cc.CCNameEmployeeCodes = matchedEmployee.EmployeeCode;
                                                    cc.CCNameJobDescriptionTitles = matchedEmployee.JobDescriptionTitle;
                                                    cc.CCEmail = matchedEmployee.Email;

                                                    x.CCNameList.Add(cc);
                                                }
                                                else
                                                {
                                                    
                                                        cc.CCName = name;
                                                        cc.CCEmail = name;
                                                        x.CCNameList.Add(cc);
                                                

                                                }



                                            }
                                            //else
                                            //{
                                            //    if (!string.IsNullOrEmpty(name))
                                            //    {
                                            //        cc.CCName = name;
                                            //        cc.CCEmail = name;
                                            //        x.CCNameList.Add(cc);

                                            //    }
                                                

                                            //}




                                        }

                                    }
                                }
                            });
                            #endregion

                            #region [pramod.misal][11.03.2025][GEOS2-6719]
                            CheckByOffer(TempMainPORequestList);
                            #endregion

                            ProcessCustomerData(TempMainPORequestList);
                            PoRequestesList.AddRange(TempMainPORequestList);
                            PoRequestesList.ForEach(x => { x.DateIssued = x.DateIssued == DateTime.MinValue ? (DateTime?)null : x.DateIssued; });
                            PoRequestesList.ForEach(x => { x.TransferAmount = x.TransferAmount == 0 ? (double?)null : x.TransferAmount; });

                            if (PoRequestesList.Count > 0)
                            {
                                SelectedPoRequest = PoRequestesList.FirstOrDefault();
                            }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                        FillPOStausTilebar();
                        MyFilterString = string.Empty;
                    }
                }
               
                GeosApplication.Instance.Logger.Log("Method FillEmployeeTripsListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void CheckByOffer(ObservableCollection<PORequestDetails> TempMainPORequestList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckByOffer ...", category: Category.Info, priority: Priority.Low);
                TempMainPORequestList.ForEach(x =>
                {
                    //[Rahul.Gadhave][GEOS2-GEOS2-10249][Date:21-11-2025]
                    if (string.IsNullOrEmpty(x.Group) && x.POLinkedOffers != null)
                    {
                        x.Group = x.POLinkedOffers.Groupname;
                        x.IdCustomerGroup = x.POLinkedOffers.IdCustomerGroup;
                    }
                    if (string.IsNullOrEmpty(x.Plant) && x.POLinkedOffers != null)
                    {
                        x.Plant = x.POLinkedOffers.Plant;
                        x.IdCustomerPlant = x.POLinkedOffers.IdCustomerPlant;
                    }
                });
                GeosApplication.Instance.Logger.Log("Method CheckByOffer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckByOffer Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void ProcessCustomerData(ObservableCollection<PORequestDetails> tempMainPORequestList)
        {
            string[] dateFormats = { "dd-MM-yyyy H:mm:ss", "dd/MM/yyyy H:mm:ss", "yyyy-MM-dd H:mm:ss", "M/d/yyyy h:mm:ss tt" };

            Parallel.ForEach(tempMainPORequestList, x =>
            {
                if (string.IsNullOrEmpty(x.Customer)) return;

                string[] customers = x.Customer.Split(';');
                string[] invoiceTos = string.IsNullOrEmpty(x.InvoioceTO) ? new string[customers.Length] : x.InvoioceTO.Split(';');
                string[] poNumbers = string.IsNullOrEmpty(x.PONumber) ? new string[customers.Length] : x.PONumber.Split(';');
                //string[] offers = string.IsNullOrEmpty(x.Offers) ? new string[customers.Length] : x.Offers.Split(';');
                string[] contacts = string.IsNullOrEmpty(x.Contact) ? new string[customers.Length] : x.Contact.Split(';');
                string[] currencies = string.IsNullOrEmpty(x.Currency) ? new string[customers.Length] : x.Currency.Split(';');
                string[] shipTos = string.IsNullOrEmpty(x.ShipTo) ? new string[customers.Length] : x.ShipTo.Split(';');
                string[] incoterms = string.IsNullOrEmpty(x.POIncoterms) ? new string[customers.Length] : x.POIncoterms.Split(';');
                string[] dateIssuedStrings = string.IsNullOrEmpty(x.DateIssuedString) ? new string[customers.Length] : x.DateIssuedString.Split(';');
                string[] transferAmounts = string.IsNullOrEmpty(x.TransferAmount?.ToString()) ? new string[customers.Length] : x.TransferAmount.ToString().Split(';');
                string[] poPaymentTerms = string.IsNullOrEmpty(x.POPaymentTerm) ? new string[customers.Length] : x.POPaymentTerm.Split(';');
                x.Customer = ConvertArrayToString(customers);
                x.InvoioceTO = ConvertArrayToString(invoiceTos);
                x.PONumber = ConvertArrayToString(poNumbers);
                //x.Offers = ConvertArrayToString(offers);
                x.Contact = ConvertArrayToString(contacts);
                x.Currency = ConvertArrayToString(currencies);
                x.ShipTo = ConvertArrayToString(shipTos);
                x.POIncoterms = ConvertArrayToString(incoterms);
                x.DateIssuedString = ConvertArrayToString(dateIssuedStrings);
                x.TransferAmountString = ConvertArrayToString(transferAmounts);
                x.POPaymentTerm = ConvertArrayToString(poPaymentTerms);

                /* var tempPoData = new ConcurrentBag<POData>();*/ // Thread-safe collection for parallel use
                var tempPoData = new ConcurrentDictionary<int, POData>();

                Parallel.For(0, customers.Length, i =>
                {
                    try
                    {
                        string amt = (i < transferAmounts.Length && decimal.TryParse(transferAmounts[i], NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                            ? string.Format(CultureInfo.CurrentCulture, "{0:N2}", value)
                            : string.Empty;

                        string formattedDate = (i < dateIssuedStrings.Length && DateTime.TryParseExact(dateIssuedStrings[i], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime exactDate))
                            ? exactDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)
                            : string.Empty;

                        tempPoData[i] = new POData
                        {
                            Customer = customers[i],
                            InvoiceTO = i < invoiceTos.Length ? invoiceTos[i] : string.Empty,
                            PONumber = i < poNumbers.Length ? poNumbers[i] : string.Empty,
                            //Offers = i < offers.Length ? offers[i] : string.Empty,
                            Contact = i < contacts.Length ? contacts[i] : string.Empty,
                            Currency = i < currencies.Length ? currencies[i] : string.Empty,
                            ShipTo = i < shipTos.Length ? shipTos[i] : string.Empty,
                            Incoterm = i < incoterms.Length ? incoterms[i] : string.Empty,
                            DateIssued = formattedDate,
                            POPaymentTerm = i < poPaymentTerms.Length ? poPaymentTerms[i] : string.Empty,
                            TransferAmount = amt
                        };
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in ProcessCustomerData() Method: " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                });

                // Merge the results back to the original collection in a thread-safe manner
                //lock (x.PoData)
                //{
                //    foreach (var data in tempPoData)
                //    {
                //        x.PoData.Add(data);
                //    }
                //}
                lock (x.PoData)
                {
                    foreach (var key in tempPoData.Keys.OrderBy(k => k))
                    {
                        x.PoData.Add(tempPoData[key]);
                    }
                }
            });
        }
        //[pramod.misal][14.04.2025][GEOS2-]
        private void EditPoRequestAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditPoRequestAction...", category: Category.Info, priority: Priority.Low);
                ShowProcessing();
                TableView detailView = (TableView)obj;
                if ((PORequestDetails)detailView.DataControl.CurrentItem != null)
                {
                    EditPORequestsViewModel editPORequestsViewModel = new EditPORequestsViewModel();
                    EditPORequestsView editPORequestsView = new EditPORequestsView();
                    editPORequestsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditPORequestsViewTitle").ToString();
                    editPORequestsViewModel.EditInIt((PORequestDetails)detailView.DataControl.CurrentItem);
                    EventHandler handle = delegate { editPORequestsView.Close(); };
                    editPORequestsViewModel.RequestClose += handle;
                    editPORequestsView.DataContext = editPORequestsViewModel;

                    var ownerInfo = (detailView as FrameworkElement);
                    Window ownerWindow = Window.GetWindow(ownerInfo);
                    if (ownerWindow != null && ownerWindow.IsVisible)
                    {
                        editPORequestsView.Owner = ownerWindow;
                    }
                    editPORequestsView.ShowDialogWindow();

                    #region  //[pramod.misal][GEOS2-8772][30-06-2025]
                    bool isPOFound = editPORequestsViewModel.POAttachementsList.Any(att => att.SelectedIndexAttachementType == 1);
                    var targetEmailId = editPORequestsViewModel.PORequestdetail.IdEmail;
                    var matchingItem = PoRequestesList.FirstOrDefault(s => s.IdEmail == targetEmailId);
                    if (isPOFound && matchingItem != null)
                    {                
                            matchingItem.POFound = "Yes";                       
                    }
                    else
                    {
                        matchingItem.POFound = "No";
                    }
                    #endregion
                    GeosApplication.Instance.Logger.Log("Method EditPoRequestAction() executed successfully...", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditPoRequestAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            if (e.ColumnFieldName == "ToRecipientName")
            {
                var toRecipients = e.Value as List<ToRecipientName>;
                if (toRecipients != null)
                {
                    e.Value = string.Join("\n", toRecipients.Select(x => x.RecipientName));
                    e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
                }
            }
            else if (e.ColumnFieldName == "CCName")
            {
                var cCNames = e.Value as List<ToCCName>;
                if (cCNames != null)
                {
                    e.Value = string.Join("\n", cCNames.Select(x => x.CCName));
                    e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
                }
            }
            e.Handled = true;
        }
        private string ConvertArrayToString(string[] array)
        {
            if (array == null || array.Length == 0)
                return string.Empty;

            return string.Join(Environment.NewLine, array.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()));
        }
        #endregion
        #region Validation
        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
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
                    me[BindableBase.GetPropertyName(() => ReceptionDateFrom)] +
                    me[BindableBase.GetPropertyName(() => ReceptionDateTo)] +
                    me[BindableBase.GetPropertyName(() => CreationDateFrom)] +
                    me[BindableBase.GetPropertyName(() => CreationDateTo)] +
                    me[BindableBase.GetPropertyName(() => UpdateDateFrom)] +
                    me[BindableBase.GetPropertyName(() => UpdateDateTo)] +
                    me[BindableBase.GetPropertyName(() => CancellationDateFrom)] +
                    me[BindableBase.GetPropertyName(() => CancellationDateTo)] +
                    me[BindableBase.GetPropertyName(() => PoValueRangeFrom)] +
                    me[BindableBase.GetPropertyName(() => PoValueRangeTo)];
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                return null;
            }
        }
        // [nsatpute][01-12-2024][GEOS2-6462]
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                if (columnName == "PoValueRangeFrom" && PoValueRangeFrom == null && PoValueRangeTo != null)
                {
                    return PORequestsValidation.GetEmptyErrorMessage(columnName);
                }
                if (double.TryParse(PoValueRangeFrom, out double fromValue) &&
                    double.TryParse(PoValueRangeTo, out double toValue))
                {
                    if (fromValue > toValue)
                    {
                        if (columnName == "PoValueRangeTo")
                        {
                            return PORequestsValidation.GetErrorMessage("PoValueRangeTo", PoValueRangeTo);
                        }
                    }
                }
                if (IsReceptionDate)
                {
                    if (columnName == "ReceptionDateFrom" && ReceptionDateFrom == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "ReceptionDateTo" && ReceptionDateTo == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "ReceptionDateTo" && ReceptionDateFrom != null && ReceptionDateTo != null && ReceptionDateFrom > ReceptionDateTo)
                    {
                        return PORequestsValidation.GetErrorMessage("ReceptionDateTo", ReceptionDateTo);
                    }
                }
                if (IscreationDate)
                {
                    if (columnName == "CreationDateFrom" && CreationDateFrom == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "CreationDateTo" && CreationDateTo == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "CreationDateTo" && CreationDateFrom != null && CreationDateTo != null && CreationDateFrom > CreationDateTo)
                    {
                        return PORequestsValidation.GetErrorMessage("CreationDateTo", CreationDateTo);
                    }
                }
                if (IsupdateDate)
                {
                    if (columnName == "UpdateDateFrom" && UpdateDateFrom == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "UpdateDateTo" && UpdateDateTo == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "UpdateDateTo" && UpdateDateFrom != null && UpdateDateTo != null && UpdateDateFrom > UpdateDateTo)
                    {
                        return PORequestsValidation.GetErrorMessage("UpdateDateTo", UpdateDateTo);
                    }
                }
                if (iscancellationDate)
                {
                    if (columnName == "CancellationDateFrom" && CancellationDateFrom == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "CancellationDateTo" && CancellationDateTo == null)
                    {
                        return PORequestsValidation.GetEmptyErrorMessage(columnName);
                    }
                    if (columnName == "CancellationDateTo" && CancellationDateFrom != null && CancellationDateTo != null && CancellationDateFrom > CancellationDateTo)
                    {
                        return PORequestsValidation.GetErrorMessage("CancellationDateTo", CancellationDateTo);
                    }
                }
                return null;
            }
        }
        #endregion
    }
}