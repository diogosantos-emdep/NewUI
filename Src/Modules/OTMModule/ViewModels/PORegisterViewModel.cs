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

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class PORegisterViewModel : NavigationViewModelBase, ITabViewModel,INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion
        #region public ICommand
        public ICommand CommandPlantEditValueChanged { get; set; }
        public ICommand OpenAttachmentCommand { get; set; } 
        public ICommand PeriodRegCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }     
        public ICommand ApplyPORegisteredCommand { get; set; }
        public ICommand CancelPORegisteredCommand { get; set; }
        public ICommand RefreshPORegisteredViewCommand { get; set; }
        public ICommand ExportPORegisteredViewCommand { get; set; }
        public ICommand TableViewLoadedPORegisteredCommand { get; set; }
        public ICommand CommandFilterPOTypeTileClick { get; set; }
        public ICommand CommandTileBarPOTypeClickDoubleClick { get; set; }
        public ICommand FilterEditorPORegisteredCreatedCommand { get; set; }
        public ICommand FilterActionAcceptCommand { get; set; }
        public ICommand FilterActionCancelCommand { get; set; }
        //[Rahul.Gadhave][GEOS2-6463][Date:03-12-2024]
        public ICommand CommandRegisteredPOGridDoubleClick { get; set; }

        public ICommand TableViewUnloadedLoadedCommand { get; set; }
        #endregion
        #region Declaration
        ObservableCollection<PORegisteredDetails> pORegisteredDetailsList;
        const string shortDateFormat = "dd/MM/yyyy";
        private TileBarFilters _selectedTileBarItem;
        private TileBarFilters _selectedTypeTileBarItem;
        private ObservableCollection<TileBarFilters> _filterStatusListOfTile;
        private ObservableCollection<TileBarFilters> _filterTypeListOfTile;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;
        DateTime startDate;
        DateTime endDate;
        string fromDate;
        string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        private bool isBusy;
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
        private string userSettingsKey_PORegistered = "OTM_PORegistered_";
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
        public bool IsUpdate { get; set; }
        private Data.Common.Company selectedItem;
        string sender;
        private string informationError;
        private int selectedCurrency;
        private int selectedPOSender;
        private int selectedCustomerPlant;
        private int selectedPoType;
        private int selectedGroup;
        public bool IsPORegistered { get; set; }
        private ObservableCollection<CustomerContacts> customerContactsList;

        private Recipient _selectedRecipient;
        public Recipient SelectedRecipient
        {
            get => _selectedRecipient;
            set
            {
                _selectedRecipient = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRecipient"));
            }
        }
        #endregion
        #region Properties
        public bool IsPlantUpdated { get; set; }
        public string CurrencyInSetting
        {
            get { return currencysetting; }
            set
            {
                currencysetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyInSetting"));
            }
        }
        public virtual object ParentViewModel { get; set; }
        public virtual string TabName { get; set; }
        public virtual object TabContent { get; protected set; }
        ObservableCollection<PORegisteredDetails> PORegisteredDetailsList

        {
            get { return pORegisteredDetailsList; }
            set
            {
                pORegisteredDetailsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PORegisteredDetailsList"));
            }
        }

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
        public int SelectedPoType
        {
            get
            {
                return selectedPoType;
            }
            set
            {
                selectedPoType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPoType"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
                if (selectedGroup >= 0)
                {
                    CustomerPlants = new ObservableCollection<CustomerPlant>();
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList().GroupBy(cpl => cpl.IdCountry).Select(group => group.First()).ToList());
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList());
                    CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == Customers[SelectedGroup].IdCustomer || cpl.CustomerPlantName == "---").OrderBy(cpl => cpl.IdCountry).ThenBy(cpl => cpl.CustomerPlantName).ToList());


                    if (CustomerPlants.Count > 0)
                        SelectedCustomerPlant = -1;
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
        public int SelectedPOSender
        {
            get
            {
                return selectedPOSender;
            }
            set
            {
                selectedPOSender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPOSender"));
            }
        }
        public int SelectedCurrency
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
        public string MyFilterPORegisteredString
        {
            get { return myFilterPORegisteredString; }
            set
            {
                myFilterPORegisteredString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterPORegisteredString"));
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
        public int? Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Number"));
            }
        }
        public string Offer
        {
            get { return offer; }
            set
            {
                offer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Offer"));
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
        public bool IscancellationDate
        {
            get
            {
                return iscancellationDate;
            }
            set
            {
                iscancellationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IscancellationDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDateFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("CancellationDateTo"));
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
        public Data.Common.Company SelectedPlant
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        public string Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sender"));
            }
        }
        public virtual int Position { get; set; }
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
        public PORegisterViewModel()
        {
            try
            {
                Processing();
                GeosApplication.Instance.Logger.Log("Constructor PORequestsViewModel....", category: Category.Info, priority: Priority.Low);
                OpenAttachmentCommand = new RelayCommand(new Action<object>(OpenAttachmentCommandAction));//[pooja.jadhav][17.02.2025][GEOS2-6724]
                PeriodRegCommand = new DelegateCommand<object>(PeriodRegCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyPORegisteredCommand = new DelegateCommand<object>(ApplyCommandPORegisteredAction);
                CancelPORegisteredCommand = new DelegateCommand<object>(CancelCommandAction);
                ExportPORegisteredViewCommand = new RelayCommand(new Action<object>(ExportPORegisteredViewAction));
                RefreshPORegisteredViewCommand = new RelayCommand(new Action<object>(RefreshPORegisteredViewAction));
                TableViewLoadedPORegisteredCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedPORegisteredCommandAction);
                FilterActionAcceptCommand = new DelegateCommand<object>(FilterActionAcceptAction);
                FilterActionCancelCommand = new DelegateCommand<object>(FilterCancelCommandAction);
                CommandFilterPOTypeTileClick = new RelayCommand(new Action<object>(CommandFilterPOTypeTileClickAction));
                CommandTileBarPOTypeClickDoubleClick = new DelegateCommand<object>(CommandPOTypeTileBarClickDoubleClickAction);
                FilterEditorPORegisteredCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorPORegisteredCreatedCommandActions);
                //TableViewLoadedPORegisteredCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedPORegisteredCommandAction);
                CommandRegisteredPOGridDoubleClick = new DelegateCommand<object>(EditRegisteredPoAction);
                POTypeList = new ObservableCollection<POType>(OTMService.OTM_GetAllPOTypeStatus());
                CommandPlantEditValueChanged = new RelayCommand(new Action<object>(PlantsListClosedCommandAction));
                TableViewUnloadedLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewunloadedLoadedCommandAction);
                DateTime baseDate = DateTime.Today;
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                OTMCommon.Instance.FromDatePOreg = thisMonthStart.ToString(shortDateFormat);
                OTMCommon.Instance.ToDatePOreg = thisMonthEnd.ToString(shortDateFormat);
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                SelectedSinglePlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                OTMCommon.Instance.SelectedPlantForRegisteredPO = SelectedSinglePlant;
                //[pramod.misal][GEOS2-9147][19-08-2025]
                OTMCommon.Instance.SelectedSinglePlant = SelectedSinglePlant;
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                FillCompany();
                FillType();
                FillCustomers();
                FillCompanyPlantList();
                FillPOSender();
                InitPoRegister();
                PORegisteredGridSettingXamlFileDelete();//[pramod.misal][GEOS2-9196][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9196     
                GeosApplication.Instance.Logger.Log("Constructor PORequestsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
                OTMCommon.Instance.IsPorequest = false;
                OTMCommon.Instance.IsRegisterPO = true;

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PORequestsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Methods

        //[pramod.misal][GEOS2-9196][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9196
        public void PORegisteredGridSettingXamlFileDelete()
        {
            List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();
            if (GeosApplication.Instance.UserSettings.ContainsKey("PORegisteredGridSetting_IsFileDeleted_V2660"))
            {
                if (GeosApplication.Instance.UserSettings["PORegisteredGridSetting_IsFileDeleted_V2660"].ToString() == "0")
                {
                    if (File.Exists(OTM_PORegisteredGridSettingFilePath))
                    {
                        File.Delete(OTM_PORegisteredGridSettingFilePath);
                        GeosApplication.Instance.UserSettings["PORegisteredGridSetting_IsFileDeleted_V2660"] = "1";
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    }
                }
            }

        }
        private void PlantsListClosedCommandAction(object obj)
        {//[pramod.misal][GEOS2-9147][19-08-2025]
            if (OTMCommon.Instance.IsPlantUpdated)
            {
                try
                {
                    //[pramod.misal][GEOS2-9147][19-08-2025]
                    SelectedSinglePlant = OTMCommon.Instance.SelectedPlantForRegisteredPO;
                    GeosApplication.Instance.Logger.Log("Method PlantsListClosedCommandAction ...", category: Category.Info, priority: Priority.Low);              
                    if (SelectedSinglePlant == null)
                        return;
                    Processing();
                    TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    InitPoRegister();
                    AddPORegisteredCustomSettingCount(gridControl);
                    IsPlantUpdated = false;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method PlantsListClosedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        /// [pooja.jadhav][17.02.2025][GEOS2-6724]       
        private void OpenAttachmentCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()...", category: Category.Info, priority: Priority.Low);
                //[pramod.misal][09.04.2025][GEOS2-6724]       
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window
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
                    }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                }

                Emailattachment Attachment = (Emailattachment)obj;
                Attachment.FileDocInBytes = GetFileDocInBytes(Attachment);
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                if (Attachment.FileDocInBytes != null)
                {
                    documentViewModel.OpenPORequestAttachment(Attachment);
                    if (documentViewModel.IsPresent)
                    {
                        documentView.DataContext = documentViewModel;
                        documentView.Show();
                    }
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method OpenAttachmentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CustomMessageBox.Show(string.Format("Could not find file {0}", Attachment.AttachmentName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenAttachmentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttachmentCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
     
        /// [pooja.jadhav][17.02.2025][GEOS2-6724]       
        public byte[] GetFileDocInBytes(Emailattachment Attachment)
        {
            byte[] bytes = null;
            try
            {
                if (Attachment.AttachmentPath != null /*&& Attachment.AttachmentName != null*/)
                {
                    if (File.Exists(Attachment.AttachmentPath))
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(Attachment.AttachmentPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error GetFileDocInBytes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return bytes;
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
        // [ashish.malkhede][14-11-2024][GEOS2-6460]
        private void ExportPORegisteredViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPORegisteredViewAction()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "PO Registered List";
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
                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    //options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportCompletedPORegisteredList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCompletedPORegisteredList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [ashish.malkhede][14-11-2024][GEOS2-6460]
        public void RefreshPORegisteredViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPORegisteredViewAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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
                detailView.SearchString = null;
                InitPoRegister();//[GEOS2-6861][rdixit][16.01.2025]
                gridControl.RefreshData();
                gridControl.UpdateLayout();
                AddPORegisteredCustomSettingCount(gridControl);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPoRegisteredList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPoRegisteredList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SearchFilterData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SearchFilterData()...", category: Category.Info, priority: Priority.Low);
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
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
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
                FillPoRegisteredListByPlant();
                FillPOTypeTileBar();
             
                MyFilterString = string.Empty;
                AddPOregisteredCustomSetting();
                IsUpdate = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SearchFilterData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchFilterData() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchFilterData() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchFilterData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void FillCompany()
        {
            try
            {
                if (CompanyList == null || CompanyList?.Count == 0)
                    CompanyList = new ObservableCollection<Company>(OTMService.GetAllCompaniesDetails_V2580(GeosApplication.Instance.ActiveUser.IdUser));
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompany()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void FillType()
        {
            try
            {
                if (TypeOfPos == null || TypeOfPos?.Count == 0)
                    TypeOfPos = new ObservableCollection<POType>(OTMService.OTM_GetPOTypes_V2580());
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void FillCustomers()
        {
            try
            {
                if (Customers == null || Customers?.Count == 0)
                    Customers = new ObservableCollection<Customer>(OTMService.GetAllCustomers());
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCustomers()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void FillCompanyPlantList()
        {
            try
            {
                if (EntireCompanyPlantList == null || EntireCompanyPlantList?.Count == 0)
                    EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMService.OTM_GetCustomerPlant_V2580());
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyPlantList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void FillPOSender()
        {
            try
            {
                if (POSender == null || POSender?.Count == 0)
                    POSender = new List<string>(OTMService.OTM_GetPOSender_V2580());
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method InitPoRegister()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InitPoRegister()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitPoRegister()...", category: Category.Info, priority: Priority.Low);
                Processing();
                FillPoRegisteredListByPlant();
                FillPOTypeTileBar();
                GetCurrencies();
                MyFilterString = string.Empty;
                AddPOregisteredCustomSetting();
               
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method InitPoRegister()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRegister() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitPoRegister() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method InitPoRegister()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07.11.2024][GEOS2-6460]
        /// </summary>
        private void FillPoRegisteredListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPoRegisteredListByPlant ...", category: Category.Info, priority: Priority.Low);
                PORegisteredDetailsList = new ObservableCollection<PORegisteredDetails>();
                PoRegisteredList = new ObservableCollection<PORegisteredDetails>();
                ObservableCollection<PORegisteredDetails> TempMainPORequestList = new ObservableCollection<PORegisteredDetails>();
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
                CurrencyInSetting = GeosApplication.Instance.Currencies.Where(x => x.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"].ToString()).Select(s => s.Name).SingleOrDefault();
                AmountCurrencySymbol = GeosApplication.Instance.CurrentCurrencySymbol;
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);

                }

                if (OTMCommon.Instance.SelectedSinglePlant != null)
                {
                    if (Filter != null && SelectedCompanies != null && SelectedCompanies.Count > 0)
                    {
                        var c = SelectedCompanies.Cast<Company>().ToList();
                        foreach (var item in c)
                        {
                            try
                            {
                                plantIds = item.IdCompany;
                                plantConnection = item.ConnectPlantConstr;
                                Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == item.IdCompany);
                                OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                                    selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                //OTMService = new OTMServiceController("localhost:6699");
                                //TempMainPORequestList = new ObservableCollection<PORegisteredDetails>(OTMService.GetPORegisteredDetails_V2620(DateTime.ParseExact(OTMCommon.Instance.FromDatePOreg, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDatePOreg, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection, Filter));
                                TempMainPORequestList = new ObservableCollection<PORegisteredDetails>(OTMService.GetPORegisteredDetails_V2630(DateTime.ParseExact(OTMCommon.Instance.FromDatePOreg, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDatePOreg, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection, Filter));
                                PoRegisteredList.AddRange(TempMainPORequestList);
                                OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            plantIds = OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany;
                            plantConnection = OTMCommon.Instance.SelectedPlantForRegisteredPO.ConnectPlantConstr;
                            Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                            OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                            selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            //OTMService = new OTMServiceController("localhost:6699");
                            //TempMainPORequestList = new ObservableCollection<PORegisteredDetails>(OTMService.GetPORegisteredDetails_V2620(DateTime.ParseExact(OTMCommon.Instance.FromDatePOreg, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDatePOreg, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection, Filter));
                            TempMainPORequestList = new ObservableCollection<PORegisteredDetails>(OTMService.GetPORegisteredDetails_V2630(DateTime.ParseExact(OTMCommon.Instance.FromDatePOreg, "dd/MM/yyyy", null), DateTime.ParseExact(OTMCommon.Instance.ToDatePOreg, "dd/MM/yyyy", null), CurrentCurrencyId, plantIds, plantConnection, Filter));
                            PoRegisteredList.AddRange(TempMainPORequestList);
                            OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                        }
                    }

                    PORegisteredDetailsList = new ObservableCollection<PORegisteredDetails>(PoRegisteredList);
                    Filter = null;


                    GeosApplication.Instance.Logger.Log("Method FillEmployeeTripsListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPoRequestesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
            }         
        }
        private void FlyoutControl_ClosedCmd(object sender, EventArgs e, GridControl grid, MenuFlyout menu)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                UnsubscribeFlyoutClosed(menu);
                //flyout.Closed -= (sender1, e1) => FlyoutControl_ClosedCmd(sender, e, grid);
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
                    OTMCommon.Instance.FromDatePOreg = thisMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    OTMCommon.Instance.FromDatePOreg = lastOneMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    OTMCommon.Instance.FromDatePOreg = lastMonthStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    OTMCommon.Instance.FromDatePOreg = thisWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    OTMCommon.Instance.FromDatePOreg = lastOneWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    OTMCommon.Instance.FromDatePOreg = lastWeekStart.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    OTMCommon.Instance.FromDatePOreg = StartDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = EndDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 8)//this year
                {
                    // setDefaultPeriod();
                    DateTime StartMDate = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime EndToMDate = new DateTime(DateTime.Now.Year, 12, 31);
                    OTMCommon.Instance.FromDatePOreg = StartMDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = EndToMDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 9)//last year
                {
                    OTMCommon.Instance.FromDatePOreg = StartFromDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = EndToDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    OTMCommon.Instance.FromDatePOreg = Date_F.ToShortDateString();
                    OTMCommon.Instance.ToDatePOreg = Date_T.ToShortDateString();
                }
                IsUpdate = true;
                
                    InitPoRegister();
                AddPORegisteredCustomSettingCount(grid);
                  IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

        private void PeriodRegCommandAction(object obj)
        {
            if (obj == null)
                return;
            Button button = (Button)obj;
            if (button.Name == "ThisMonthPOReg")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonthPOReg")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeekPOReg")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeekPOReg")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeekPOReg")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRangePOReg")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYearPOReg")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYearPOReg")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12MonthsPOReg")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }
        private void FilterCancelCommandAction(object obj)
        {
            DropDownButton menu = (DropDownButton)obj;
            menu.IsPopupOpen = false;
            SelectedPoType = 0;
            Number = null;
            SelectedGroup = 0;
            SelectedCustomerPlant = 0;
            SelectedPOSender = 0;
            SelectedCurrency = 0;
            PoValueRangeFrom = null;
            PoValueRangeTo = null;
            Offer = null;
            Sender = null;
            IsReceptionDate = false;
            ReceptionDateFrom = null;
            ReceptionDateTo = null;
            IscreationDate = false;
            CreationDateFrom = null;
            CreationDateTo = null;
            IsupdateDate = false;
            UpdateDateFrom = null;
            UpdateDateTo = null;
            IscancellationDate = false;
            CancellationDateFrom = null;
            CancellationDateTo = null;
            SelectedCompanies = null;
        }

        // [ashish.malkhede][14-11-2024][GEOS2-6460]
        private void ApplyCommandPORegisteredAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);
                //MenuFlyout menu = (MenuFlyout)obj;
                Object[] objArray = (Object[])obj;
                MenuFlyout menu = (MenuFlyout)objArray[0];
                GridControl grid = (GridControl)objArray[1];
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                // menu.FlyoutControl.Closed += FlyoutControl_ClosedCmd;
                //menu.FlyoutControl.Closed += (sender, e) => FlyoutControl_ClosedCmd(sender, e, grid);

                // Define a named delegate for the Closed event
                _flyoutClosedHandler = (sender, e) => FlyoutControl_ClosedCmd(sender, e, grid, menu);

                // Subscribe to the event using the named delegate
                menu.FlyoutControl.Closed += _flyoutClosedHandler;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UnsubscribeFlyoutClosed(MenuFlyout menu)
        {
            if (_flyoutClosedHandler != null)
            {
                menu.FlyoutControl.Closed -= _flyoutClosedHandler;
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
        // [001] [ashish.malkhede][13-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        public void FillPOTypeTileBar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPOTypeTileBar()...", category: Category.Info, priority: Priority.Low);
                FilterTypeListOfTiles = new ObservableCollection<TileBarFilters>();
                ObservableCollection<POType> tempTypeList = new ObservableCollection<POType>();
                int _id = 1;
                if (PoRegisteredList != null)
                {
                    var DttableList = PoRegisteredList.AsEnumerable().ToList();
                    List<int> idOfferPOTypeList = DttableList.Select(x => (int)x.IdPOType).Distinct().ToList();
                    foreach (int item in idOfferPOTypeList)
                    {
                        POType type = POTypeList.FirstOrDefault(x => x.IdPoType == item);
                        if (type != null)
                            tempTypeList.Add(type);
                    }
                    FilterTypeListOfTiles.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("PORegisteredReportTileBarCaption").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = PoRegisteredList.Count,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });
                    foreach (var item in tempTypeList)
                    {
                        string imageUri = string.Empty;
                        string htmlcolor = string.Empty;
                        if (item.IdPoType == 1)
                        {
                            imageUri = "Star.png";
                            htmlcolor = "#ffffff";
                        }
                        else if (item.IdPoType == 2)
                        {
                            imageUri = "Green Flag.png";
                            htmlcolor = "#98fb98";
                        }
                        else
                        {
                            imageUri = "BlueFlag.png";
                            htmlcolor = "#98fb98";
                        }
                        FilterTypeListOfTiles.Add(new TileBarFilters()
                        {
                            Caption = item.Type,
                            Id = _id++,
                            IdOfferStatusType = item.IdPoType,
                            // BackColor = null,
                            FilterCriteria = "[Type] In ('" + item.Type + "')",
                            // ForeColor = htmlcolor,
                            BackColor = null,
                            ForeColor = null,
                            EntitiesCount = (DttableList.Where(x => (int)x.IdPOType == item.IdPoType).ToList()).Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200,
                            ImageUri = imageUri
                        });
                    }

                    //[pramod.misal][GEOS2-7048][20.03.2024]
                    int emdepCount = DttableList.Count(x => x.Group == "EMDEP"); // Count rows where Group is EMDEP
                    if (emdepCount > 0)
                    {
                        FilterTypeListOfTiles.Add(new TileBarFilters()
                        {
                            Caption = "Internal", 
                            Id = _id++,
                            FilterCriteria = "[Group] = 'EMDEP'",               
                            EntitiesCount = emdepCount, 
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200,
                            ImageUri = "EMDEPIcon.png" 

                        });

                    }
                    //[pramod.misal][GEOS2-7048][20.03.2024]
                    int nonEmdepCount = DttableList.Count(x => x.Group != "EMDEP");

                    if (nonEmdepCount>0)
                    {
                        FilterTypeListOfTiles.Add(new TileBarFilters()
                        {
                            Caption = "External",
                            Id = _id++,
                            FilterCriteria = "[Group] <> 'EMDEP'",                           
                            BackColor = null,
                            ForeColor = null,
                            EntitiesCount = nonEmdepCount,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200,
                            ImageUri = "Non_Emdep.png"
                        });

                    }
                   


                }
                FilterTypeListOfTiles.Add(new TileBarFilters()
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
                if (FilterTypeListOfTiles.Count > 0)
                    SelectedTypeTileBarItems = FilterTypeListOfTiles[0];
                GeosApplication.Instance.Logger.Log("Method FillPOTypeTileBar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method FillPOTypeTileBar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        private void CommandFilterPOTypeTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFilterPOTypeTileClickAction....", category: Category.Info, priority: Priority.Low);
                if (FilterTypeListOfTiles == null)
                {
                    return;
                }
                if (FilterTypeListOfTiles.Count > 0)
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
                                PoRegisteredList = PoRegisteredList;
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

                GeosApplication.Instance.Logger.Log("Method CommandFilterPOTypeTileClickAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterPOTypeTileClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        private void CommandPOTypeTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandPOTypeTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in POTypeList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Type))
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
                gridControl.FilterString = FilterTypeListOfTiles?.FirstOrDefault(i => i.Caption == CustomFilterStringName)?.FilterCriteria;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandPOTypeTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandPOStatusTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        private void FilterEditorPORegisteredCreatedCommandActions(FilterEditorEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorPORegisteredCreatedCommandActions()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowPORegisteredFilterEditor(obj);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterEditorPORegisteredCreatedCommandActions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FilterEditorPORegisteredCreatedCommandActions() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        private void ShowPORegisteredFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                customFilterEditorViewModel.Init(e.FilterControl, FilterTypeListOfTiles);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();
                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = FilterTypeListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        FilterTypeListOfTiles.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (setting.Key.Contains(userSettingsKey_PORegistered))
                                key = setting.Key.Replace(userSettingsKey_PORegistered, "");
                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = FilterTypeListOfTiles.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
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
                            if (setting.Key.Contains(userSettingsKey_PORegistered))
                                key = setting.Key.Replace(userSettingsKey_PORegistered, "");
                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey_PORegistered + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    VisibleRowCount = gridControl.VisibleRowCount;
                    FilterTypeListOfTiles.Add(new TileBarFilters()
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
                    filterName = userSettingsKey_PORegistered + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = FilterTypeListOfTiles.LastOrDefault();
                }
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private async void TableViewunloadedLoadedCommandAction(RoutedEventArgs obj)
        {
           // if (OTMCommon.Instance.IsRegisterPO)
            //{
                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
            //}
                

        }
        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        public void TableViewLoadedPORegisteredCommandAction(RoutedEventArgs obj)
        {
            //if (OTMCommon.Instance.IsRegisterPO)
            //{
                try
                {
                    GeosApplication.Instance.Logger.Log("Method TableViewLoadedPORegisteredCommandAction ...", category: Category.Info, priority: Priority.Low);

                    int visibleFalseCoulumn = 0;
                    if (File.Exists(OTM_PORegisteredGridSettingFilePath))
                    {
                        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OTM_PORegisteredGridSettingFilePath);
                        try
                        {
                            // Get the TableView and GridControl from the event source
                            TableView gridTableView = obj.OriginalSource as TableView;
                            GridControl tempgridControl = gridTableView.DataControl as GridControl;
                            // Manually parse the XML to enforce visibility settings
                            ApplyPORegisteredVisibilityFromXml(tempgridControl);
                        }
                        catch (Exception ex)
                        {
                        }
                        GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                        TableView tableView = (TableView)GridControlSTDetails.View;
                        if (tableView.SearchString != null)
                        {
                            tableView.SearchString = null;
                        }
                    }
               ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                    //This code for save grid layout.
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
                    GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    foreach (GridColumn column in gridControl.Columns)
                    {
                        DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                        if (descriptor != null)
                        {
                            descriptor.AddValueChanged(column, VisibleChangedPORegistered);
                        }
                        DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                        if (descriptorColumnPosition != null)
                        {
                            descriptorColumnPosition.AddValueChanged(column, VisibleIndexPORegisteredChanged);
                        }
                        if (column.Visible == false)
                        {
                            visibleFalseCoulumn++;
                        }
                    }

                    IsPOrequestsColumnChooserVisible = false;

                    TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                    datailView.ShowTotalSummary = true;

                    TotalSummary = new ObservableCollection<GridSummaryItem>();
                    TotalSummary.Add(new GridSummaryItem() { SummaryType = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                    //TotalSummary.Add(new GridSummaryItem() { SummaryType = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total Count {0}" });
                    gridControl.TotalSummary.Clear();
                    gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });
                    if (TotalSummary.Count > 0)
                    {
                        foreach (var sum in TotalSummary)
                        {
                            gridControl.TotalSummary.Add(sum);
                        }
                    }

                    //[002] End
                    AddPORegisteredCustomSettingCount(gridControl);
                    GeosApplication.Instance.Logger.Log("Method TableViewLoadedPORegisteredCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }

                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedPORegisteredCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            //}
           
        }
        private void ApplyPORegisteredVisibilityFromXml(GridControl gridControl)
        {
            try
            {
                XDocument layoutXml = XDocument.Load(OTM_PORegisteredGridSettingFilePath);
                foreach (GridColumn column in gridControl.Columns)
                {
                    // Find the column entry in the XML based on its FieldName
                    var columnElement = layoutXml.Descendants("property")
                        .Where(x => (string)x.Attribute("name") == "FieldName" && x.Value == column.FieldName)
                        .FirstOrDefault();
                    if (columnElement != null)
                    {
                        // Get the visibility setting from the XML
                        var visibleElement = columnElement.Parent.Descendants("property")
                            .Where(x => (string)x.Attribute("name") == "Visible")
                            .FirstOrDefault();
                        if (visibleElement != null)
                        {
                            bool isVisible = bool.Parse(visibleElement.Value);
                            column.Visible = isVisible;
                            GeosApplication.Instance.Logger.Log($"Column '{column.FieldName}' visibility set to: {isVisible}.", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ApplyVisibilityFromXml: " + ex.Message, category: Category.Exception, priority: Priority.High);
            }
        }

        // [pramod.misal][04-10-2024][GEOS2-6520]
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
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
        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        void VisibleChangedPORegistered(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChangedPORegistered ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    var view = column.View as TableView;
                    GridControl gridControl = view.DataControl as GridControl;
                    gridControl.SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
                    ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method VisibleChangedPORegistered() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChangedPORegistered() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [ashish.malkhede][15-11-2024][GEOS2-6460]
        void VisibleIndexPORegisteredChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexPORegisteredChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                var view = column.View as TableView;
                GridControl gridControl = view.DataControl as GridControl;
                gridControl.SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
                ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORegisteredGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method VisibleIndexPORegisteredChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexPORegisteredChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        // [ashish.malkhede][14-11-2024][GEOS2-6460]
        private void AddPOregisteredCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddPOregisteredCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey_PORegistered)).ToList();
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
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddPOregisteredCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        if (FilterTypeListOfTiles == null)
                        {
                            FilterTypeListOfTiles = new ObservableCollection<TileBarFilters>();
                        }
                        FilterTypeListOfTiles.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsKey_PORegistered, ""),
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
                GeosApplication.Instance.Logger.Log("Method AddPOregisteredCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddPOregisteredCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterActionAcceptAction(object obj)
        {
            // [nsatpute][01-12-2024][GEOS2-6462]
            allowValidation = true;
            string error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDateFrom"));
            PropertyChanged(this, new PropertyChangedEventArgs("ReceptionDateTo"));
            PropertyChanged(this, new PropertyChangedEventArgs("CreationDateFrom"));
            PropertyChanged(this, new PropertyChangedEventArgs("CreationDateTo"));
            PropertyChanged(this, new PropertyChangedEventArgs("UpdateDateFrom"));
            PropertyChanged(this, new PropertyChangedEventArgs("UpdateDateTo"));
            PropertyChanged(this, new PropertyChangedEventArgs("CancellationDateFrom"));
            PropertyChanged(this, new PropertyChangedEventArgs("CancellationDateTo"));
            PropertyChanged(this, new PropertyChangedEventArgs("PoValueRangeFrom"));
            PropertyChanged(this, new PropertyChangedEventArgs("PoValueRangeTo"));
            if (!string.IsNullOrEmpty(error))
            {
                allowValidation = false;
                return;
            }
            Filter = new PORegisteredDetailFilter();
            DropDownButton menu = (DropDownButton)obj;
            menu.IsPopupOpen = false;
            if (SelectedPoType != 0 && SelectedPoType !=-1)
                Filter.IdType = TypeOfPos[SelectedPoType].IdPoType;
            Filter.Number = Number;
            if (SelectedGroup != 0 && SelectedGroup != -1)
                Filter.IdGroup = Customers[SelectedGroup].IdCustomer;
            if (CustomerPlants != null)
            {
                if (SelectedCustomerPlant != 0 && SelectedCustomerPlant != -1)
                    Filter.IdCustomerPlant = CustomerPlants[SelectedCustomerPlant].IdCustomerPlant;
            }
            Filter.Sender = Sender;
            if (SelectedCurrency != 0 && SelectedCurrency != -1)
                Filter.IdCurrency = Currencies[SelectedCurrency].IdCurrency;
            Filter.PoValueRangeFrom = Convert.ToDouble(PoValueRangeFrom);
            Filter.PoValueRangeTo = Convert.ToDouble(PoValueRangeTo);
            Filter.Offer = Offer;
            if (PoValueRangeFrom != null && PoValueRangeTo == null)
            {
                PoValueRangeTo = "9999999999";
            }
            if (IsReceptionDate)
            {
                Filter.ReceptionDateFrom = ReceptionDateFrom;
                Filter.ReceptionDateTo = ReceptionDateTo;
            }
            if (IscreationDate)
            {
                Filter.CreationDateFrom = CreationDateFrom;
                Filter.CreationDateTo = CreationDateTo;
            }
            if (IsupdateDate)
            {
                Filter.UpdateDateFrom = UpdateDateFrom;
                Filter.UpdateDateTo = UpdateDateTo;
            }
            if (IscancellationDate)
            {
                Filter.CancellationDateFrom = CancellationDateFrom;
                Filter.CancellationDateTo = CancellationDateTo;
            }
            if (Filter.IdType == null && Filter.Number == null && Filter.IdGroup == null && Filter.IdCustomerPlant == null &&
                Filter.Sender == null && Filter.IdCurrency == null && Filter.PoValueRangeFrom == null && Filter.PoValueRangeTo == null &&
                Filter.Offer == null && Filter.ReceptionDateFrom == null && Filter.ReceptionDateTo == null && Filter.CreationDateFrom == null &&
                Filter.CreationDateTo == null && Filter.UpdateDateFrom == null && Filter.UpdateDateTo == null &&
                Filter.CancellationDateFrom == null && Filter.CancellationDateTo == null && SelectedCompanies == null)
            {
                Filter = null;
            }
            SearchFilterData();
            allowValidation = false;
            SelectedPoType = 0;
            Number = null;
            Sender = null;
            SelectedGroup = 0;
            SelectedCustomerPlant = 0;
            SelectedPOSender = 0;
            SelectedCurrency = 0;
            PoValueRangeFrom = null;
            PoValueRangeTo = null;
            Offer = null;
            IsReceptionDate = false;
            ReceptionDateFrom = null;
            ReceptionDateTo = null;
            IscreationDate = false;
            CreationDateFrom = null;
            CreationDateTo = null;
            IsupdateDate = false;
            UpdateDateFrom = null;
            UpdateDateTo = null;
            IscancellationDate = false;
            CancellationDateTo = null;
            CancellationDateFrom = null;
            SelectedCompanies = null;
        }
        public void Dispose()
        {
        }
        private void EditRegisteredPoAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditRegisteredPoAction...", category: Category.Info, priority: Priority.Low);
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
                TableView detailView = (TableView)obj;
                if ((PORegisteredDetails)detailView.DataControl.CurrentItem != null)
                {
                    //People data = (People)obj;
                    int personId = Convert.ToInt32(((PORegisteredDetails)detailView.DataControl.CurrentItem).IdPO);
                    EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
                    EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();
                    editRegisteredPOsViewModel.EditInIt((PORegisteredDetails)detailView.DataControl.CurrentItem);
                    EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                    editRegisteredPOsViewModel.RequestClose += handle;
                    editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    var ownerInfo = (detailView as FrameworkElement);
                    Window ownerWindow = Window.GetWindow(ownerInfo);
                    if (ownerWindow != null && ownerWindow.IsVisible)
                    {
                        editRegisteredPOsView.Owner = ownerWindow;
                    }
                    editRegisteredPOsView.ShowDialogWindow();
                    FillPOTypeTileBar();
                    GeosApplication.Instance.Logger.Log("Method EditContactAction() executed successfully...", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditContactAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-6460][28-11-2024]
        private void GetCurrencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-6861][rdixit][16.01.2025]
                if (Currencies == null || Currencies?.Count == 0)
                {
                    Currencies = new ObservableCollection<Currency>(OTMService.GetAllCurrencies_V2590());
                    if (Currencies != null)
                    {
                        //foreach (var bpItem in Currencies.GroupBy(tpa => tpa.Name))
                        //{
                        //    ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        //    bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                        //}
                        var orderedCurrencies = Currencies.OrderBy(currency => currency.Name).ToList();
                        foreach (var bpItem in orderedCurrencies.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                            bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                        }
                        Currencies = new ObservableCollection<Currency>(orderedCurrencies);
                    }
                }
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
