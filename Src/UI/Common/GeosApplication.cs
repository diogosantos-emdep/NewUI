using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;
using System.Net.NetworkInformation;
using System.Windows;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Utility;
using Prism.Logging;
using System.Threading;
using Emdep.Geos.UI.Common;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Media;
using System.Windows.Input;
using System.Globalization;
using System.Drawing.Printing;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.OptimizedClass;
using System.Net;
using System.Management;
using System.Net.Sockets;
using System.Printing;
using System.Text.RegularExpressions;
using System.Media;
using System.Windows.Interop;
using System.Windows.Forms;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.Crm;

namespace Emdep.Geos.UI.Common
{
    /// <summary>
    /// Create this Class for Manage Login User
    /// </summary>
    public sealed class GeosApplication : Prism.Mvvm.BindableBase
    {
        #region TaskLog
        //[WMS M049-16][Print Label in picking][adadibathina]
        //[WMS M052][Print Label in picking][adadibathina][adding SharedPrinter]
        //[WMS	M055-06] Add OK and NOK beeps in picking storage and transfer[adadibathina]
        //GEOS2-258 Sprint-60 GetSharedPrinterName executed in each picking![adadibathina]
        #endregion

        #region Declaration

        private static readonly GeosApplication instance = new GeosApplication();

        private DateTime selectedyearStarDate;
        private DateTime selectedyearEndDate;
        private string remainingDays;

        private List<UserManagerDtl> salesOwnerUsersList;
        private List<object> selectedSalesOwnerUsersList;
        private Visibility cmbSalesOwnerUsers;

        private List<Company> plantOwnerUsersList;
        private List<object> selectedPlantOwnerUsersList;
        private Visibility cmbPlantOwnerUsers;

        private GeosWorkbenchVersion geosWorkbenchVersionNumber;

        private GeosServiceProviders geosServiceProviders;
        private GeosServiceProviders geosServiceProvidersold;
        private GeosWorkbenchVersion geosWorkbenchVersion;

        protected FontFamily _FontFamilyAsPerTheme;
        protected string splashScreenMessage = "";
		//[nsatpute][08.07.2025][GEOS2-7205]
        protected string customSplashScreenMessage = "";
        protected string customeSplashScreenInformationMessage = "";

        public List<Company> EmdepSiteList;

        protected string _CultureAsperCurrency;
        CultureInfo _CurrentCultureCurrencySymbol;
        private CultureInfo _CurrentCulturenew;
        private List<Ots> mainOtsList = new List<Ots>();
        private Visibility tileBarVisibility;
        private bool isCarOEMExist;
        private bool isButtonRowVisible;
        private bool isGroupNameExist;
        private bool isCityNameExist;
        private List<CarProject> geosCarProjectsList;
        private List<Competitor> competitorList;
        private List<Tag> tagList;
        private ObservableCollection<Notification> notificationsListCommon = new ObservableCollection<Notification>();
        private List<CRMSections> crmsectionsList;
        private List<CRMSections> selectedCrmsectionsList;
        public List<string> PrinterList { get; set; }
        public List<string> SharedPrinterList { get; set; }
        private bool isLoginUserManager;
        public List<long> FinancialYearLst { get; set; }

        public DateTime maxDate;
        private List<City> cityList;

        private List<DomainUser> domainUsers;
        private ObservableCollection<LookupValue> attendanceTypeList;
        private ObservableCollection<LookupValue> employeeLeaveList;
        private ObservableCollection<LookupValue> attendanceStatusList;
        private string currentSiteTimeZone;
        public List<SalesStatusTypeDetail> salesStatusTypeList;
        public List<UserCommonDetail> userCommonDetailList;
        //[cpatil][24-02-2020][GEOS2-1977] The code added in the offer code must be taken from the application selected site
        public Int32 activeIdSite;
        private string labelPrinter;
        CultureInfo _CurrentCultureCurrencySymbol_warehouse;
        private string labelPrinterForPrinter2;
        public static INavigationService NavigationServiceOnGeosWorkbenchScreen;

        private List<BulkPicking> articleBulkPickingList;//[Sudhir.Jangra][GEOS2-4414][08/09/2023]
        public bool IsWMSManageInspectionPoints { get; set; }
        ObservableCollection<ArticleCostPrice> articleCostPriceList;
        List<Tuple<ulong, string>> articleCostPlantPriceList;
        List<Tuple<ulong, string, uint>> basePriceNameList;
        List<BasePriceListByItem> basePriceListByItem;
        List<CurrencyConversion> currencyConversionList;
        private EmployeeAnnualAdditionalLeave compensationLeave;//[Sudhir.jangra][GEOS2-5336]
        private List<Article> samWorkOrderArticles;
        private bool isSAMReport;
        private List<SCMSections> scmSectionsList;  //[shweta.thube][GEOS2-6630][04.04.2025]
        private List<SCMSections> selectedScmSectionsList;  //[shweta.thube][GEOS2-6630][04.04.2025]
        private ObservableCollection<StatusMessage> statusMessages; //[nsatpute][08.07.2025][GEOS2-7205]
        private bool isTSMCustomerViewVisible = true; // [pallavi.kale][04-09-2025] [GEOS2-8949]

        #endregion

        #region Properties

        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }
        public Prism.Logging.ILoggerFacade Logger { get; set; }

        public User ActiveUser { get; set; }
        public User ImpersonateUser { get; set; }
        public string SiteName { get; set; }
        public string CurrentCulture { get; set; }
        public Notification Notification { get; set; }
        public DateTime ServerDateTime { get; set; }
        public ResourceDictionary dict { get; set; }

        public Action ServerDeactiveMethod { get; set; }
        public Action ServerActiveMethod { get; set; }

        public Dictionary<string, string> ApplicationSettings { get; set; }
        public Dictionary<string, string> UserSettings { get; set; }
        public string ServicePath { get; set; }

        private Dictionary<string, object> objectPool;

        //for if user have permission 23 then make crm non editable.
        public bool IsPermissionReadOnly { get; set; }
        public bool IsPermissionEnabled { get; set; }
        public bool IsCommercialUser { get; set; }
        public bool IsLoadOneTime { get; set; }
        public bool IsPermissionAuditor { get; set; }
        private SoundPlayer player { get; set; }

        public bool IsPermissionNameEditInPCMArticle { get; set; }
        public bool IsPermissionWMSgridValueColumn { get; set; }

        //[cpatil][20-09-2021][GEOS2-3342]
        public bool IsPCMPermissionNameECOS_Synchronization { get; set; }
        //[cpatil][06-10-2021][GEOS2-3336]
        public bool IsPLMPermissionNameECOS_Synchronization { get; set; }
        public bool IsPLMPermissionView { get; set; }
        public bool IsPLMPermissionChange { get; set; }
        public bool IsPCMEditFreePluginsPermission { get; set; }
        public bool IsPLMPermissionAdmin { get; set; }
        public bool IsSCMPermissionAdmin { get; set; }
        public bool IsSCMPermissionReadOnly { get; set; }
        public bool IsSCMEditConnectorStatus { get; set; }
        public bool IsSCMReportsAuditor { get; set; }
        //[rdixit][GEOS2-5477][20.05.2024]
        public bool IsSCMREditConnectorBasic { get; set; }        
        //[rdixit][GEOS2-5480][29.05.2024]
        public bool IsSCMEditConnectorLinks { get; set; }

        //[rdixit][GEOS2-5478][29.05.2024]
        public bool IsSCMEditConnectorAdvanced { get; set; }
        //[pramod.misal][GEOS2-5481][23.05.2024]
        public bool IsSCMEditFiltersManager { get; set; }
        //[pramod.misal][GEOS2-5482][24.05.2024]
        public bool IsSCMEditFamiliesManager { get; set; }
        //[pramod.misal][GEOS2-5483][27.05.2024]
        public bool IsSCMEditPropertiesManager { get; set; }

        //[pramod.misal][GEOS2-5525][12.08.2024]
        public bool IsSCMEditLocationsManager { get; set; }

        //[pramod.misal][GEOS2-5525][12.08.2024]
        public bool IsOTMCancelPO { get; set; }
        //[pramod.misal][GEOS2-5525][12.08.2024]
        public bool IsOTMFullUpdatePO { get; set; }

        //[pramod.misal][GEOS2-5525][12.08.2024]
        public bool IsOTMViewOnly { get; set; }

        //[pramod.misal][GEOS2-5525][12.08.2024]
        public bool IsOTMUnlinkOfferfromPO { get; set; }

        //[ashish.malkhede] [GEOS2-6463][18.12.2024]
        public string EmployeeCode { get; set; }

        public List<UserPermissionByBPLPriceList> AllUserPermissionsByBPLPriceList { get; set; }
        public List<UserPermissionByCPLPriceList> AllUserPermissionsByCPLPriceList { get; set; }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public bool PO_Template_Manager { get; set; }

        //[pallavi.kale][GEOS2-5386][16.01.2025]
        public bool IsTSMUsersViewOnly { get; set; }
        public bool IsTSMUsersEdit { get; set; }

        public bool IsPCMAddEditPermissionForHardLockLicense { get; set; }//[Sudhir.jangra][GEos2-4901]
        //[rdixit][18.05.2023][GEOS2-4273]
        public bool IsCRMactionsLauncherPermission { get; set; }

        //
        public bool UpdateMinMaxArticleStockPermission { get; set; }
        public ObservableCollection<Notification> NotificationsListCommon
        {
            get { return notificationsListCommon; }
            set
            {
                this.notificationsListCommon = value;
                this.OnPropertyChanged("NotificationsListCommon");
            }
        }
		//[nsatpute][25-06-2025][GEOS2-8641]
        private ObservableCollection<FileDetail> downloadedReportFiles;

        public ObservableCollection<FileDetail> DownloadedReportFiles
        {
            get { return downloadedReportFiles; }
            set { downloadedReportFiles = value; this.OnPropertyChanged("DownloadedReportFiles"); }
        }

        private List<GeosAppSetting> timeTrackingActivePlantList;

        public List<GeosAppSetting> TimeTrackingActivePlantList
        {
            get { return timeTrackingActivePlantList; }
            set { timeTrackingActivePlantList = value; this.OnPropertyChanged("TimeTrackingActivePlantList"); }
        }

        private List<GeosAppSetting> trackingTimeGeosAppSettingList;

        public List<GeosAppSetting> TrackingTimeGeosAppSettingList
        {
            get { return trackingTimeGeosAppSettingList; }
            set { trackingTimeGeosAppSettingList = value; this.OnPropertyChanged("TrackingTimeGeosAppSettingList"); }
        }

        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
        }

        public GeosServiceProviders GeosServiceProviders
        {
            get { return geosServiceProviders; }
            set { geosServiceProviders = value; }
        }

        public GeosServiceProviders GeosServiceProvidersold
        {
            get { return geosServiceProvidersold; }
            set { geosServiceProvidersold = value; }
        }

        public ObservableCollection<Assembly> ModulesList { get; set; }

        /// <summary>
        /// Class single instance 
        /// </summary>
        public static GeosApplication Instance
        {
            get { return instance; }
        }

        public List<UITheme> UIThemeList { get; set; }
        public List<Company> CompanyList { get; set; }
        public List<PeopleDetails> PeopleList { get; set; }
        public string UserSettingFilePath { get; set; }
        public string UserSettingFileName { get; set; }
        public string UserSettingFolderName { get; set; }

        public string ApplicationSettingFilePath { get; set; }
        public string ApplicationSettingFileName { get; set; }

        public string ApplicationLogFilePath { get; set; }
        public string ApplicationLogFileName { get; set; }

        public bool IsServiceActive { get; set; }
        public string PrivateNetworkIP { get; set; }
        public bool IsPrivateNetworkIP { get; set; }
        public string Ip { get; set; }
        public decimal Port { get; set; }
        public List<GeosServiceProvider> GeosServiceProviderList { get; set; }
        public IList<Currency> Currencies { get; set; }
        public Currency PCMCurrentCurrency { get; set; }//[rdixit][04.12.2023][GEOS2-4897]
        public byte IdCurrencyByRegion { get; set; }

        public byte IdWMSCurrencyByRegion { get; set; }

        public long CrmOfferYear { get; set; }
        public int CrmTopOffers { get; set; }

        public string PCMImage { get; set; }//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        public string PCMAttachment { get; set; }//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        public string PCMLinks { get; set; }//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        public int CrmTopCustomers { get; set; }
        public string SelectedPrinter { get; set; }
        public int IdUserPermission { get; set; }
        public string CurrentCurrencySymbol { get; set; }
        public string WMSCurrentCurrencySymbol { get; set; }
        //[sdeshpande][11-07-2022]
        public bool IsAdminPermissionERM { get; set; }
        //[sdeshpande][11-07-2022]
        public bool IsReadWOPermissionERM { get; set; }
        //[sdeshpande][11-07-2022]
        public bool IsEditWOPermissionERM { get; set; }
        //[sdeshpande][11-07-2022]
        public bool IsReadSODPermissionERM { get; set; }
        //[sdeshpande][11-07-2022]
        public bool IsEditSODPermissionERM { get; set; }
        public bool IsTimeTrackingReadPermissionERM { get; set; }
        //Akshay
        public string LabelSizeSettingForPrinter1 { get; set; }
        public string LabelSizeSettingForPrinter2 { get; set; }
        //public string SelectedPrinterForPrinter2 { get; set; }

        public string LabelPrinterForPrinter2
        {
            get { return labelPrinterForPrinter2; }
            set
            {
                labelPrinterForPrinter2 = value;
                //if LabelPrinter is changed set LabelPrinterSharedName according to it
                LabelPrinterSharedNameForPrinter2 = GetSharedNameOfPrinter(value);
            }
        }
        public string LabelPrinterSharedNameForPrinter2 { get; set; }
        public string LabelPrinterModelForPrinter2 { get; set; }
        public string ParallelPortForPrinter2 { get; set; }

        public bool IsPermissionAdminOnly { get; set; }
        //GEOS2-258 Sprint-60 
        public string LabelPrinter
        {
            get { return labelPrinter; }
            set
            {
                labelPrinter = value;
                //if LabelPrinter is changed set LabelPrinterSharedName according to it
                LabelPrinterSharedName = GetSharedNameOfPrinter(value);
            }
        }


        //GEOS2-258
        public string LabelPrinterSharedName { get; set; }
        public string LabelPrinterModel { get; set; }
        public string ParallelPort { get; set; }
        public IPAddress SystemIp { get; set; }
        public FontFamily FontFamilyAsPerTheme
        {
            get { return this._FontFamilyAsPerTheme; }
            set
            {
                if (this._FontFamilyAsPerTheme != value)
                {
                    this._FontFamilyAsPerTheme = value;
                    this.OnPropertyChanged("FontFamilyAsPerTheme");
                }
            }
        }

        public DateTime SelectedyearStarDate
        {
            get { return selectedyearStarDate; }
            set
            {
                this.selectedyearStarDate = value;
                this.OnPropertyChanged("SelectedyearStarDate");
            }
        }

        public DateTime SelectedyearEndDate
        {
            get { return selectedyearEndDate; }
            set
            {
                this.selectedyearEndDate = value;
                this.OnPropertyChanged("SelectedyearEndDate");
            }
        }

        public string RemainingDays
        {
            get { return remainingDays; }
            set
            {
                this.remainingDays = value;
                this.OnPropertyChanged("RemainingDays");
            }
        }

        public string SplashScreenMessage
        {
            get { return this.splashScreenMessage; }
            set
            {
                this.splashScreenMessage = value;
                this.OnPropertyChanged("SplashScreenMessage");
            }
        }
		//[nsatpute][08.07.2025][GEOS2-7205]
        public string CustomeSplashScreenMessage
        {
            get { return this.customSplashScreenMessage; }
            set
            {
                this.customSplashScreenMessage = value;
                this.OnPropertyChanged("CustomeSplashScreenMessage");
            }
        }
		//[nsatpute][08.07.2025][GEOS2-7205]
        public string CustomeSplashScreenInformationMessage
        {
            get { return this.customeSplashScreenInformationMessage; }
            set
            {
                this.customeSplashScreenInformationMessage = value;
                this.OnPropertyChanged("CustomeSplashScreenInformationMessage");
            }
        }

        public string CultureAsperCurrency
        {
            get { return this._CultureAsperCurrency; }
            set
            {
                if (this._CultureAsperCurrency != value)
                {
                    this._CultureAsperCurrency = value;
                    this.OnPropertyChanged("CultureAsperCurrency");
                }
            }
        }

        public CultureInfo CurrentCultureCurrencySymbol
        {
            get { return _CurrentCultureCurrencySymbol; }
            set
            {
                if (_CurrentCultureCurrencySymbol == value)
                    return;
                _CurrentCultureCurrencySymbol = value;
                OnPropertyChanged("CurrentCultureCurrencySymbol");
            }
        }

        public CultureInfo CurrentCulturenew
        {
            get { return _CurrentCulturenew; }
            set
            {
                if (_CurrentCulturenew == value)
                    return;
                _CurrentCulturenew = value;
                OnPropertyChanged("CurrentCulturenew");
            }
        }

        public List<UserManagerDtl> SalesOwnerUsersList
        {
            get { return salesOwnerUsersList; }
            set
            {
                salesOwnerUsersList = value;
                OnPropertyChanged("SalesOwnerUsersList");
            }
        }
		//[nsatpute][08.07.2025][GEOS2-7205]
        public ObservableCollection<StatusMessage>StatusMessages 
        {
            get { return statusMessages; }
            set
            {
                statusMessages = value;
                OnPropertyChanged("StatusMessages");
}
        }
        public List<object> SelectedSalesOwnerUsersList
        {
            get { return selectedSalesOwnerUsersList; }
            set
            {
                selectedSalesOwnerUsersList = value;
                OnPropertyChanged("SelectedSalesOwnerUsersList");

                ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null && GeosApplication.Instance.SelectedSalesOwnerUsersList.Count > 0)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true)));
                    //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyPlantByIdUser updated with GetSelectedUserCompanyPlantByIdUser_V2420
                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true)));
                }
            }
        }

        public Visibility CmbSalesOwnerUsers
        {
            get { return this.cmbSalesOwnerUsers; }
            set
            {
                this.cmbSalesOwnerUsers = value;
                this.OnPropertyChanged("CmbSalesOwnerUsers");
            }
        }

        public List<Company> PlantOwnerUsersList
        {
            get { return plantOwnerUsersList; }
            set
            {
                plantOwnerUsersList = value;
                OnPropertyChanged("PlantOwnerUsersList");
            }
        }

        public List<object> SelectedPlantOwnerUsersList
        {
            get { return selectedPlantOwnerUsersList; }
            set
            {
                selectedPlantOwnerUsersList = value;
                OnPropertyChanged("SelectedPlantOwnerUsersList");
            }
        }

        public Visibility CmbPlantOwnerUsers
        {
            get { return this.cmbPlantOwnerUsers; }
            set
            {
                this.cmbPlantOwnerUsers = value;
                this.OnPropertyChanged("CmbPlantOwnerUsers");
            }
        }

        public Dictionary<string, object> ObjectPool
        {
            get { return objectPool; }
            set
            {
                objectPool = value;
                OnPropertyChanged("ObjectPool");
            }
        }

        public GeosWorkbenchVersion GeosWorkbenchVersion
        {
            get { return geosWorkbenchVersion; }
            set { geosWorkbenchVersion = value; }
        }

        public Visibility TileBarVisibility
        {
            get { return tileBarVisibility; }
            set
            {
                tileBarVisibility = value;
                OnPropertyChanged("TileBarVisibility");
            }
        }

        public bool IsCarOEMExist
        {
            get { return isCarOEMExist; }
            set
            {
                isCarOEMExist = value;
                OnPropertyChanged("IsCarOEMExist");
            }
        }

        public bool IsGroupNameExist
        {
            get { return isGroupNameExist; }
            set
            {
                isGroupNameExist = value;
                OnPropertyChanged("IsGroupNameExist");
            }
        }

        public bool IsCityNameExist
        {
            get { return isCityNameExist; }
            set
            {
                isCityNameExist = value;
                OnPropertyChanged("IsCityNameExist");
            }
        }

        public List<CarProject> GeosCarProjectsList
        {
            get { return geosCarProjectsList; }
            set
            {
                geosCarProjectsList = value;
            }
        }

        public List<Competitor> CompetitorList
        {
            get { return competitorList; }
            set { competitorList = value; }
        }


        public List<Tag> TagList
        {
            get { return tagList; }
            set { tagList = value; }
        }

        public List<CRMSections> CrmsectionsList
        {
            get { return crmsectionsList; }
            set
            {
                crmsectionsList = value;
                OnPropertyChanged("CrmsectionsList");
            }
        }

        public List<CRMSections> SelectedCrmsectionsList
        {
            get { return selectedCrmsectionsList; }
            set
            {
                selectedCrmsectionsList = value;
                OnPropertyChanged("SelectedCrmsectionsList");
            }
        }

        public bool IsButtonRowVisible
        {
            get { return isButtonRowVisible; }
            set
            {
                isButtonRowVisible = value;
                OnPropertyChanged("IsButtonRowVisible");
            }
        }

        public bool IsLoginUserManager
        {
            get { return isLoginUserManager; }
            set
            {
                isLoginUserManager = value;
                OnPropertyChanged("IsLoginUserManager");
            }
        }

        public ObservableCollection<SalesOwnerList> SalesOwnerList { get; set; }
        public List<City> CityList
        {
            get { return cityList; }
            set { cityList = value; }
        }

        public List<DomainUser> DomainUsers
        {
            get { return domainUsers; }
            set
            {
                domainUsers = value;
                OnPropertyChanged("DomainUsers");
            }
        }

        public ObservableCollection<LookupValue> AttendanceTypeList
        {
            get { return attendanceTypeList; }
            set
            {
                attendanceTypeList = value;
                OnPropertyChanged("AttendanceTypeList");
            }
        }

        public ObservableCollection<LookupValue> EmployeeLeaveList
        {
            get { return employeeLeaveList; }
            set
            {
                employeeLeaveList = value;
                OnPropertyChanged("EmployeeLeaveList");
            }
        }


        public ObservableCollection<LookupValue> AttendanceStatusList
        {
            get { return attendanceStatusList; }
            set
            {
                attendanceStatusList = value;
                OnPropertyChanged("AttendanceStatusList");
            }
        }

        public List<SalesStatusTypeDetail> SalesStatusTypeList
        {
            get { return salesStatusTypeList; }
            set
            {
                salesStatusTypeList = value;
                OnPropertyChanged("SalesStatusTypeList");
            }
        }

        public List<UserCommonDetail> UserCommonDetailList
        {
            get { return userCommonDetailList; }
            set
            {
                userCommonDetailList = value;
                OnPropertyChanged("UserCommonDetailList");
            }
        }

        public static CultureInfo CurrentCultureInfo { get; set; }

        //[cpatil][24-02-2020][GEOS2-1977] The code added in the offer code must be taken from the application selected site
        public Int32 ActiveIdSite
        {
            get { return activeIdSite; }
            set
            {
                activeIdSite = value;
                OnPropertyChanged("ActiveIdSite");
            }
        }

        public CultureInfo CurrentCultureCurrencySymbol_warehouse
        {
            get
            {
                return _CurrentCultureCurrencySymbol_warehouse;
            }

            set
            {
                if (_CurrentCultureCurrencySymbol_warehouse == value)
                    return;
                _CurrentCultureCurrencySymbol_warehouse = value;
                OnPropertyChanged("CurrentCultureCurrencySymbol_warehouse");
            }
        }

        public bool IsPermissionForManageItemPrice { get; set; }
        public bool IsManageTrainingPermission { get; set; }
        public bool IsPermissionForSRMEdit_Supplier { get; set; }
        public bool IsPermissionReadOnlyForPCM { get; set; }
        public bool IsPermissionReadOnlyForPLM { get; set; }
        //[GEOS2-3943][rdixit][22.11.2022]
        public bool IsAdminPermissionForHRM { get; set; }

        public bool IsAdminInventoryAuditPermissionForWMS { get; set; }
        public bool IsEditInventoryAuditPermissionForWMS { get; set; }
        public bool IsRemoveInventoryAuditPermissionForWMS { get; set; }
        public bool IsCreateInventoryAuditPermissionForWMS { get; set; }
        //[rdixit][GEOS2-3943][04.01.2023]
        public bool IsChangeOrAdminPermissionForHRM { get; set; }
        //[rdixit][GEOS2-5112][09.01.2024]
        public bool IsTravel_AssistantPermissionForHRM { get; set; }
        //[rdixit][GEOS2-5112][09.01.2024]
        public bool IsControlPermissionForHRM { get; set; }
        //[rdixit][GEOS2-5112][09.01.2024]
        public bool IsPlant_FinancePermissionForHRM { get; set; }

        //[rdixit][GEOS2-5112][09.01.2024]
        public bool IsChangeermissionForHRM { get; set; }

        //[rdixit][GEOS2-5112][09.01.2024]
        public bool IsHRMPermissionEnabled { get; set; }

        //[rdixit][16.01.2024][GEOS2-5074]
        public bool IsWatchMySelfOnlyPermissionForHRM { get; set; }

        //[rdixit][GEOS2-3871][09.01.2023]
        public string CurrentSiteTimeZone
        {
            get { return currentSiteTimeZone; }
            set
            {
                currentSiteTimeZone = value;
                OnPropertyChanged("CurrentSiteTimeZone");
            }
        }

        //[sdeshpande][GEOS2-3841][11/1/2023]
        public bool IsReadWorkStagesPermissionERM { get; set; }
        //[sdeshpande][GEOS2-3841][11/1/2023]
        public bool IsEditWorkStagesPermissionERM { get; set; }

        // [Rupali Sarode][GEOS2-4155][03-02-2023]
        public bool IsReadProductionPlanPermissionERM { get; set; }

        // [Rupali Sarode][GEOS2-4155][03-02-2023]
        public bool IsEditProductionPlanPermissionERM { get; set; }
        //[rdixit][GEOS2-4419][27.04.2023]
        public static Dictionary<string, byte[]> ImageUrlBytePair { get; set; }

        //[rdixit][24.05.2023]
        public static bool IsImageURLException { get; set; }



        public bool IsEditBulkPickingPermissionWMS;//[Sudhir.Jangra][GEOS2-4414]
        //[Sudhir.Jangra][GEOS2-4414]
        public List<BulkPicking> ArticleBulkPickingList
        {
            get { return articleBulkPickingList; }
            set
            {
                articleBulkPickingList = value;
                OnPropertyChanged("ArticleBulkPickingList");
            }
        }

        //[pramod.misal][GEOS2-][12.10.2023]
        public bool IsWMSDeliveryNoteLockUnlockPermission { get; set; }

        //Aishwarya Ingale[5/02/2024]
        public bool IsEditDeliveryTimeDistributionERM { get; set; }
        //[rdixit][27.12.2023][GEOS2-4875]
        public bool PCMEditArticleCategoryMapping { get; set; }

        //[pramod.misal][GEOS2-5477][06.05.2024]
        public bool IsSCMViewConfigurationPermission { get; set; }


        public bool IsHRMAttendanceSplitPermission { get; set; }
        public bool IsViewPermissionERM { get; set; }


        public EmployeeAnnualAdditionalLeave CompensationLeave
        {
            get { return compensationLeave; }
            set
            {
                compensationLeave = value;
                OnPropertyChanged("CompensationLeave");
            }
        }

        public bool IsViewSupervisorERM { get; set; } //[Pallavi jadhav][GEOS2-5910][17-07-2024]

        //[nsatpute][31-07-2024][GEOS2-5473]New functions for Carpenters (2/3) 
        public List<Article> SamWorkOrderArticles
        {
            get { return samWorkOrderArticles; }
            set { samWorkOrderArticles = value; OnPropertyChanged("SamWorkOrderArticles"); }
        }

        //[rdixit][28.08.2024][GEOS2-5410]
        public bool IsSAMReport
        {
            get { return isSAMReport; }
            set { isSAMReport = value; OnPropertyChanged("IsSAMReport"); }
        }

        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
        public bool IsChangeAndAdminPermissionForHRM { get; set; }

        //[rushikesh.gaikwad][GEOS2-5801][13.09.2024]
        public bool IsSCMSampleRegistrationPermission { get; set; }

        //[Shweta.Thube][GEOS2-5981][05/10/2024]
        public bool IsAPMActionPlanPermission { get; set; }

        //[GEOS2-6499][rdixit][04.11.2024]
        public bool IsHRMManageEmployeeContactsPermission { get; set; }

        //[GEOS2-6499][rdixit][04.11.2024]
        public bool IsHRMManageShiftPermission { get; set; }


        //[GEOS2-6499][rdixit][04.11.2024]

        public ObservableCollection<ArticleCostPrice> ArticleCostPriceList
        {
            get { return articleCostPriceList; }
            set
            {
                this.articleCostPriceList = value;
                this.OnPropertyChanged("ArticleCostPriceList");
            }
        }


        public List<BasePriceListByItem> BasePriceListByItem
        {
            get
            {
                return basePriceListByItem;
            }
            set
            {
                basePriceListByItem = value;
                this.OnPropertyChanged("BasePriceListByItem");
            }
        }
        //[GEOS2-6522][rdixit][29.11.2024]
        public List<Tuple<ulong, string, uint>> BasePriceNameList
        {
            get
            {
                return basePriceNameList;
            }
            set
            {
                basePriceNameList = value;
                this.OnPropertyChanged("BasePriceNameList");
            }
        }
        public List<Tuple<ulong, string>> ArticleCostPlantPriceList
        {
            get { return articleCostPlantPriceList; }
            set
            {
                this.articleCostPlantPriceList = value;
                this.OnPropertyChanged("ArticleCostPlantPriceList");
            }
        }
        public List<CurrencyConversion> CurrencyConversionList
        {
            get
            {
                return currencyConversionList;
            }

            set
            {
                currencyConversionList = value;
                OnPropertyChanged("CurrencyConversionList");
            }
        }

        public static bool isServiceDown { get; set; }
      //[rdixit][GEOS2-6979][02.04.2025]
        public bool IsHRMTravelManagerPermission { get; set; }

        public static Dictionary<string, System.Windows.Media.Imaging.BitmapImage> FileExtentionIcon { get; set; }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        public List<SCMSections> ScmSectionsList
        {
            get { return scmSectionsList; }
            set
            {
                scmSectionsList = value;
                OnPropertyChanged("ScmSectionsList");
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        public List<SCMSections> SelectedScmSectionsList
        {
            get { return selectedScmSectionsList; }
            set
            {
                selectedScmSectionsList = value;
                OnPropertyChanged("SelectedScmSectionsList");
            }
        }
        // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
        DateTime selectedHRMAttendanceDate;
        public DateTime SelectedHRMAttendanceDate
        {
            get { return selectedHRMAttendanceDate; }
            set
            {
                this.selectedHRMAttendanceDate = value;
                this.OnPropertyChanged("SelectedHRMAttendanceDate");
            }
        }
        bool isWMManagerPermission;
        bool isWMSAdminPermission;

        public bool IsWMManagerPermission
        {
            get { return isWMManagerPermission; }
            set
            {
                this.isWMManagerPermission = value;
                this.OnPropertyChanged("IsWMManagerPermission");
            }
        }
        public bool IsWMSAdminPermission
        {
            get { return isWMSAdminPermission; }
            set
            {
                this.isWMSAdminPermission = value;
                this.OnPropertyChanged("IsWMSAdminPermission");
            }
        }

        //[nsatpute][12.09.2025][GEOS2-8789]
        private bool isWMS_SchedulePermission;
        public bool IsWMS_SchedulePermission
        {
            get { return isWMSAdminPermission; }
            set
            {
                this.isWMSAdminPermission = value;
                this.OnPropertyChanged("IsWMS_SchedulePermission");
            }
        }
    
         // [pallavi.kale][04-09-2025] [GEOS2-8949]
        public bool IsTSMCustomerViewVisible
        {
            get { return isTSMCustomerViewVisible; }
            set
            {
                isTSMCustomerViewVisible = value;
                OnPropertyChanged("IsTSMCustomerViewVisible");
            }
        }

        #endregion

        #region Constructor

        public GeosApplication()
        {
            SetLanguageDictionary();
            ApplicationSettings = new Dictionary<string, string>();
            IsServiceActive = true;
            ApplicationLogFileName = "log4net.config";
            ApplicationLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Emdep\Geos\" + ApplicationLogFileName;

            UserSettingFileName = "UserSettings.config";
            UserSettingFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Emdep\Geos\" + UserSettingFileName;
            UserSettingFolderName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Emdep\Geos\";

            ObjectPool = new Dictionary<string, object>();
            ApplicationSettingFileName = "ApplicationSettings.config";
            ApplicationSettingFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Emdep\Geos\" + ApplicationSettingFileName;

            ModulesList = new ObservableCollection<Assembly>();
            LoadModuleList();
            SystemIp = GetLocalIPAddress();
            player = new SoundPlayer();
            CurrentCultureInfo = Thread.CurrentThread.CurrentCulture;
        }

        #endregion

        #region Methods

        public List<string> FillPrinterList()
        {
            PrinterList = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                PrinterList.Add(printer);
            }

            return PrinterList;
        }

        public void LoadModuleList()
        {
            try
            {
                Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string[] list = Directory.GetFiles(path, "Emdep.Geos.Modules*.dll");

                if (list != null && list.Length > 0)
                {
                    foreach (string dll in list)
                        ModulesList.Add(Assembly.LoadFile(dll));
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SetCurrencySymbol(string currencySymbol)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            cultureInfo.NumberFormat.CurrencySymbol = currencySymbol;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            GeosApplication.Instance.CurrentCultureCurrencySymbol = cultureInfo;
        }

        public static void SetCurrencySymbol_warehouse(string currencySymbol)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            cultureInfo.NumberFormat.CurrencySymbol = currencySymbol;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            GeosApplication.Instance.CurrentCultureCurrencySymbol_warehouse = cultureInfo;
        }

        public void ExceptionHandlingOperation(ServiceExceptionType ex, string address, Action action)
        {
            IsServiceActive = true;
            if (ex == ServiceExceptionType.FaultException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("FaultException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.CommunicationException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("CommunicationException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.TimeoutException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("TimeoutException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.SecurityNegotiationException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("SecurityNegotiationException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.ProtocolException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ProtocolException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.UriFormatException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UriFormatException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            if (ex == ServiceExceptionType.MessageSecurityException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("MessageSecurityException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            if (ex == ServiceExceptionType.UnknownException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UnknownException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            if (ex == ServiceExceptionType.UnknownException)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UnknownException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            if (ex == ServiceExceptionType.EndpointNotFoundException)
            {
                if (address != null)
                {
                    string[] addressarray = address.Split(':');
                    address = addressarray[0];
                    try
                    {
                        var ping = new Ping();
                        var options = new PingOptions { DontFragment = true };

                        //just need some data. this sends 10 bytes.
                        var buffer = Encoding.ASCII.GetBytes(new string('z', 10));
                        var host = address;
                        var reply = ping.Send(host, 60, buffer, options);

                        if (reply != null && reply.Status == IPStatus.Success)
                        {
                            IsServiceActive = false;

                            return;
                        }
                        else
                        {
                            IsServiceActive = false;

                            return;
                        }
                    }
                    catch (System.Net.NetworkInformation.PingException exPing)
                    {
                        return;
                    }
                }
            }

            if (action != null)
            {
                action();
            }
        }

        public string ExceptionHandlingOperationString(ServiceExceptionType ex, string address, Action action)
        {
            IsServiceActive = true;

            if (ex == ServiceExceptionType.FaultException)
            {
                return System.Windows.Application.Current.FindResource("FaultException").ToString();
            }

            if (ex == ServiceExceptionType.CommunicationException)
            {
                return System.Windows.Application.Current.FindResource("CommunicationException").ToString();
            }

            if (ex == ServiceExceptionType.TimeoutException)
            {
                return System.Windows.Application.Current.FindResource("TimeoutException").ToString();
            }

            if (ex == ServiceExceptionType.SecurityNegotiationException)
            {
                return System.Windows.Application.Current.FindResource("SecurityNegotiationException").ToString();
            }

            if (ex == ServiceExceptionType.ProtocolException)
            {
                return System.Windows.Application.Current.FindResource("ProtocolException").ToString();
            }

            if (ex == ServiceExceptionType.UriFormatException)
            {
                return System.Windows.Application.Current.FindResource("UriFormatException").ToString();
            }

            if (ex == ServiceExceptionType.MessageSecurityException)
            {
                return System.Windows.Application.Current.FindResource("MessageSecurityException").ToString();
            }

            if (ex == ServiceExceptionType.UnknownException)
            {
                return System.Windows.Application.Current.FindResource("UnknownException").ToString();
            }

            if (ex == ServiceExceptionType.EndpointNotFoundException)
            {
                if (address != null)
                {
                    string[] addressarray = address.Split(':');
                    address = addressarray[0];
                    try
                    {
                        var ping = new Ping();
                        var options = new PingOptions { DontFragment = true };

                        //just need some data. this sends 10 bytes.
                        var buffer = Encoding.ASCII.GetBytes(new string('z', 10));
                        var host = address;
                        var reply = ping.Send(host, 60, buffer, options);

                        if (reply != null && reply.Status == IPStatus.Success)
                        {
                            IsServiceActive = false;

                            return System.Windows.Application.Current.FindResource("SeverException").ToString();
                        }
                        else
                        {
                            IsServiceActive = false;

                            return System.Windows.Application.Current.FindResource("InternetException").ToString();
                        }
                    }
                    catch (System.Net.NetworkInformation.PingException exPing)
                    {
                        return System.Windows.Application.Current.FindResource("PingException").ToString();
                    }
                }
            }
            if (action != null)
            {
                action();
            }
            return " ";
        }

        public string ExceptionHandlingOperationString(ServiceException serviceException, Action action)
        {
            IsServiceActive = true;
            string msg = string.Format(System.Windows.Application.Current.FindResource("FaultExceptionFromService").ToString(), serviceException.ErrorMessage);
            return msg.ToString();

            //if (action != null)
            //{
            //    action();
            //}
            return " ";
        }

        public void AddLog(string message, Category category, Priority priority)
        {
            Logger.Log(message, category, priority);
        }

        /// <summary>
        //[WMS M049-16] {get local ip}
        /// </summary>
        /// <returns></returns>
        private IPAddress GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        //[WMS M052] {get SharedName of printer}
        // </summary>
        /// <returns></returns>
        private string GetSharedNameOfPrinter(string printerName)
        {
            string sharedName = string.Empty;
            GeosApplication.Instance.Logger.Log(" GeosApplication Method GetSharedNameOfPrinter....", category: Category.Info, priority: Priority.Low);
            try
            {
                //string query = String.Format("SELECT * FROM Win32_Printer WHERE Name = '{0}'", GeosApplication.Instance.UserSettings["LabelPrinter"]);
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                //ManagementObjectCollection collection = searcher.Get();
                //ManagementObject printer = collection.Cast<ManagementObject>().ElementAt(0);
                //The part that changes the printer share name
                //sharedName = printer.Properties["ShareName"].Value.ToString();

                var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                foreach (var printer in printerQuery.Get())
                {
                    var name = printer.GetPropertyValue("Name");
                    if (name.ToString().Equals(printerName))
                    {
                        sharedName = printer.GetPropertyValue("ShareName")?.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetSharedNameOfPrinter...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log(" GeosApplication Method GetSharedNameOfPrinter executed successfully", category: Category.Info, priority: Priority.Low);
            return sharedName;
        }

        /// <summary>
        /// Method for set Language as per culture. 
        /// </summary>
        public void SetLanguageDictionary()
        {
            dict = new ResourceDictionary();
            string lan = Thread.CurrentThread.CurrentCulture.ToString();

            if (GeosApplication.Instance != null)
                lan = GeosApplication.Instance.CurrentCulture;

            //if (GeosApplication.Instance.CurrentCulture != null)
            //lan = GeosApplication.Instance.CurrentCulture;
            //dict.Source = new Uri("/Emdep.Geos.UI.Common;component/Resources/Language.xaml", UriKind.RelativeOrAbsolute);

            try
            {
                try
                {
                    dict.Source = new Uri("/Emdep.Geos.UI.Common;component/Resources/Language.xaml", UriKind.RelativeOrAbsolute);
                    //dict.Source = new Uri("/Emdep.Geos.UI.Common;component/Resources/Language." + lan + ".xaml", UriKind.RelativeOrAbsolute);
                }
                catch (Exception)
                {
                    dict.Source = new Uri("/Emdep.Geos.UI.Common;component/Resources/Language.xaml", UriKind.RelativeOrAbsolute);
                }


                System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
            }
        }

        public string GetApplicationSetting(ApplicationSetting setting)
        {
            switch (setting)
            {
                case ApplicationSetting.ServiceProviderIP:
                    return GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString();

                case ApplicationSetting.ServiceProviderPort:
                    return GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString();

                case ApplicationSetting.ServicePath:
                    return GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
            }

            return "";
        }

        /// <summary>
        /// Method for check  Ip is Private or Public  . 
        /// </summary>
        public bool IsPrivateNetworkIp()
        {
            try
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                if (ApplicationOperation.GetEmdepGroupIP("10.0.") != null)
                                {
                                    string PrivateIP = ApplicationOperation.GetEmdepGroupIP("10.0.");
                                    PrivateNetworkIP = PrivateIP;
                                    IsPrivateNetworkIP = true;
                                    return IsPrivateNetworkIP;
                                }
                                else
                                {
                                    string PublicIP = ApplicationOperation.GetEmdepGroupIP(ip.Address.ToString());
                                    PrivateNetworkIP = PublicIP;
                                    IsPrivateNetworkIP = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return IsPrivateNetworkIP;
        }

        /// <summary>
        /// Method for get all GeosService Providers in Application setting file  read ApplicationSettingFile.xmal file . 
        /// </summary>
        public GeosServiceProviders GetGeosServiceProviders(string ApplicationSettingFilePath)
        {
            List<GeosServiceProvider> GeosServiceProviderslist = new List<GeosServiceProvider>();
            XmlSerializer ser = new XmlSerializer(typeof(GeosServiceProviders));
            GeosServiceProviders geosServiceProviders = new GeosServiceProviders();
            XmlSerializerNamespaces xmlSerializerns = new XmlSerializerNamespaces();
            xmlSerializerns.Add("", "");
            try
            {
                using (FileStream readStream = new FileStream(ApplicationSettingFilePath, FileMode.Open))
                {
                    XmlSerializer XmlSerializer = new XmlSerializer(typeof(GeosServiceProviders));
                    GeosServiceProviders = new GeosServiceProviders();
                    GeosServiceProviders = (GeosServiceProviders)XmlSerializer.Deserialize(readStream);
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return GeosServiceProviders;
        }

        /// <summary>
        /// Method for Write ApplicationSettingFile.xmal file.
        /// </summary>
        //read ApplicationSettingFile.xmal file 
        public void WriteApplicationSettingFile(List<GeosServiceProvider> GeosServiceProviderList)
        {
            GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosServiceProviderList;
            XmlSerializer ser = new XmlSerializer(typeof(GeosServiceProviders));
            //StringWriterUTF8 writer = new StringWriterUTF8();
            XmlSerializerNamespaces xmlSerializerns = new XmlSerializerNamespaces();
            xmlSerializerns.Add("", "");
            StreamWriter WriterStream = new StreamWriter(GeosApplication.Instance.ApplicationSettingFilePath);
            ser.Serialize(WriterStream, GeosApplication.Instance.GeosServiceProviders, xmlSerializerns);
            WriterStream.Close();
        }

        /// <summary>
        /// Convert the string to camel case.
        /// </summary>
        /// <param name="the_string"></param>
        /// <returns></returns>
        public string ToCamelCase(string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(' ');

            // Combine the words.
            string result = string.Empty;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0)
                {
                    //if string have two word then add space.
                    result = result + " ";
                }
                else
                {
                    if (i > 0)
                    {
                        //if string have two word then add space.
                        result = result.Trim();
                        result = result + " ";
                    }

                    result +=
                        words[i].Substring(0, 1).ToUpper() +
                        words[i].Substring(1).ToLower();
                }
            }

            return result;
        }
        /// <summary>
        /// method for fill financial year.
        /// </summary>
        public List<long> FillFinancialYear()
        {
            maxDate = GeosApplication.Instance.ServerDateTime.Date;
            FinancialYearLst = new List<long>();
            for (long i = maxDate.Year + 1; i >= 2000; i--)
            {
                FinancialYearLst.Add(i);
            }
            return FinancialYearLst;
        }

        /// <summary>
        /// method for GetWeekNames.
        /// 
        /// [M049-07][20181110][adadibathina]
        /// 
        /// </summary>
        public List<object> GetWeekNames()
        {
            List<object> Days = new List<object>();
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            // all days of week
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            // get first day of week from current culture
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            // all days of week ordered from first day of week
            var daysOfWeekOrdered = daysOfWeek.OrderBy(x => (x - firstDayOfWeek + 7) % 7);
            foreach (var item in daysOfWeekOrdered)
            {
                Days.Add(item);
            }
            return Days;
        }

        /// <summary>
        /// Method To play sound
        /// [WMS M055-06]
        /// </summary>
        /// <param name="filePath" >full path has to be given and file must be in Wav formate</param>
        public void PlaySound(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Instance.Logger.Log("Method PlaySound().... filePath :" + filePath + " Dont Exists", category: Category.Exception, priority: Priority.Low);
                    return;
                }
                player = new System.Media.SoundPlayer(filePath);
                player.Play();
            }
            catch (Exception ex)
            {
                Instance.Logger.Log("Get an error in Method PlaySound()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //public List<DomainUser> GetUserDataFromTheActiveDirectory()
        //{
        //    List<DomainUser> domainUsersData = new List<DomainUser>();

        //    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "emdep"))
        //    {
        //        using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
        //        {
        //            foreach (Principal result in searcher.FindAll())
        //            {
        //                DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

        //                DomainUser domainUser = new DomainUser();
        //                domainUser.FirstName = Convert.ToString(de.Properties["givenName"].Value);
        //                domainUser.LastName = Convert.ToString(de.Properties["sn"].Value);
        //                domainUser.Email = Convert.ToString(de.Properties["mail"].Value);

        //                //Console.WriteLine("SAM Account name : " + de.Properties["samAccountName"].Value);
        //                //Console.WriteLine("User principle name: " + de.Properties["userPrincipleName"].Value);

        //                domainUsersData.Add(domainUser);
        //            }
        //        }
        //    }

        //    return domainUsersData.OrderBy(q => q.FirstName).ThenBy(r => r.LastName).ToList();
        //}

        /// <summary>
        /// This is used to get current working screen window monitor
        /// </summary>
        /// <param name="window">The Current window.</param>
        /// <returns></returns>
        public Screen GetWorkingScreenFrom(Window window = null)
        {
            try
            {
                if (window == null)
                    window = System.Windows.Application.Current.Windows[1];

                WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
                Screen screen = Screen.FromHandle(windowInteropHelper.Handle);

                return screen;
            }
            catch
            {
                return Screen.PrimaryScreen;
            }
        }

        public static string createExceptionDetailsMsg(Exception ex)
        {
            string ExceptionDetailsMsg = string.Empty;
            if (ex != null)
            {
                if (ex.Data != null)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.Data=" + ex.Data;

                if (ex.HResult != 0)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.HResult=" + ex.HResult;

                if (ex.HelpLink != null)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.HelpLink=" + ex.HelpLink;

                if (ex.InnerException != null)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.InnerException.Message=" + ex.InnerException.Message;

                if (ex.Source != null)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.Source=" + ex.Source;

                if (ex.StackTrace != null)
                    ExceptionDetailsMsg += Environment.NewLine + "ex.StackTrace=" + ex.StackTrace;
            }
            else
            {
                ExceptionDetailsMsg = "No exception";
            }

            return ExceptionDetailsMsg;
        }


        public uint convert1(string par1)
        {
            uint temp = Convert.ToUInt32(par1);

            return temp;
        }
        #endregion
    }
}
