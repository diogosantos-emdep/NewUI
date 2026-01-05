using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Data;
using System.ComponentModel;
using Emdep.Geos.UI.Commands;
using System.Diagnostics;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Mvvm.UI;
using System.Runtime.CompilerServices;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Grid.Native;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Mvvm.UI.Interactivity;
using System.Windows.Markup;
using System.Windows.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using System.Net;
using System.Runtime.Serialization.Json;
using Emdep.Geos.Utility;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Data.Filtering;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class LeadsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable //, ISupportServices
    {
        #region TaskLog

        //[0001][07/06/2018][lsharma][CRM-M040-04][(#49889) Lost offer information in grid columns]
        //Created New List LostReasonsByOffeLeadsList and refill column after edit grid if user changes status to lost
        //[0002][27/06/2018][lsharma][CRM-M041-14][Cancelled offers do not appear because some hardcode is there]
        //M051-24	(#60635) not possible to change the sales owner [adadinathina]
        //[003][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities


        #endregion

        #region Services

        public IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        string myFilterString;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;
        private DataTable dttable;
        private DataTable dttableCopy;
        private DataSet ds;
        public DataRowView selectedObject;
        //private List<Offer> leadsList;
        //private List<OptionsByOffer> optionsByOfferLeadsList;
        private IList<Template> templates;

        bool isBusy;
        ObservableCollection<GeosStatus> geosStatusList;

        private bool isViewRangeControl;
        private string gridRowHeight;

        private bool isFirstTimeOpen;
        private bool isInit;
        private Timer tmrOnce;

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;


        private LeadsViewModel leadsVM;
        bool isTimelineColumnChooserVisible;

        List<string> failedPlants;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        IServiceContainer serviceContainer = null;

        private ObservableCollection<LookupValue> leadSourceList;
        private List<CarOEM> caroemsList;

        List<LogEntryByOffer> changeLogsEntry;
        private List<CarProject> geosProjectsList;
        List<OfferOption> offerOptions;
        private bool isFromFilterString = false;

        private bool isFilterEnabled;
        private DateTime? old_startDate;
        private DateTime? old_endDate;

        //private List<LostReasonsByOffer> lostReasonsByOffeLeadsList;
        private List<TimelineGrid> timelineGridList;
        private List<OptionsByOfferGrid> optionsByOfferGridList;

        private ObservableCollection<TileBarFilters> filterStatusListOfTile;
        private ObservableCollection<GeosStatus> StatusList;
        private TileBarFilters selectedTileBarItem;
        private List<GridColumn> GridColumnList;
        private bool isEdit;
        TableView view;
        private string userSettingsKey="CRM_Timeline_";
        #endregion      // Declaration.

        #region  public Properties  
        //public List<LostReasonsByOffer> LostReasonsByOffeLeadsList
        //{
        //    get { return lostReasonsByOffeLeadsList; }
        //    set { lostReasonsByOffeLeadsList = value; }
        //}
        public int LeadsSleepDays { get; set; }

        public string TimelineGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "TimelineGridSetting.Xml";
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public List<OfferOption> commonOptionsByOfferList { get; set; }
        public List<OptionsByOffer> tempOptionsByOfferList { get; set; }
        public List<int> ConfidentialLevelList { get; private set; }
        public List<LookupValue> BusinessUnitList { get; set; }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        //public ObservableCollection<SalesOwnerList> SalesOwnerList { get; set; }

        public List<CarProject> GeosProjectsList
        {
            get { return geosProjectsList; }
            set
            {
                geosProjectsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosProjectsList"));
            }
        }

        public List<LogEntryByOffer> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }
        public ObservableCollection<LookupValue> LeadSourceList
        {
            get { return leadSourceList; }
            set
            {
                leadSourceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeadSourceList"));
            }
        }
        public List<CarOEM> CaroemsList
        {
            get { return caroemsList; }
            set
            {
                caroemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CaroemsList"));
            }
        }

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }



        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        public bool IsTimelineColumnChooserVisible
        {
            get { return isTimelineColumnChooserVisible; }
            set
            {
                isTimelineColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTimelineColumnChooserVisible"));
            }
        }

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }
        public DataSet Ds
        {
            get { return ds; }
            set { ds = value; }
        }
        //public List<Offer> LeadsList
        //{
        //    get { return leadsList; }
        //    set { leadsList = value; }
        //}

        public List<TimelineGrid> TimelineGridList
        {
            get { return timelineGridList; }
            set { timelineGridList = value; }
        }

        public List<OptionsByOfferGrid> OptionsByOfferGridList
        {
            get { return optionsByOfferGridList; }
            set { optionsByOfferGridList = value; }
        }

        //public List<OptionsByOffer> OptionsByOfferLeadsList
        //{
        //    get { return optionsByOfferLeadsList; }
        //    set { optionsByOfferLeadsList = value; }
        //}


        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;

                //This condition for if user clear the filter then clear date also.
                if (string.IsNullOrEmpty(myFilterString))
                {
                    isFromFilterString = true;
                    StartSelectionDate = DateTime.MinValue;
                    FinishSelectionDate = DateTime.MinValue;
                }

                isFromFilterString = false;

                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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

        public bool IsFilterEnabled
        {
            get { return isFilterEnabled; }
            set
            {
                if (isFilterEnabled == value)
                    return;
                isFilterEnabled = value;
                isFromFilterString = true;
                if (!value)
                {
                    old_startDate = StartSelectionDate;
                    old_endDate = FinishSelectionDate;
                    StartSelectionDate = DateTime.MinValue;
                    FinishSelectionDate = DateTime.MinValue;
                }
                else if (old_startDate.HasValue)
                {
                    if (old_startDate.Value > DateTime.MinValue)
                    {
                        if (old_startDate.Value == StartSelectionDate)
                        {

                            StartSelectionDate = old_startDate.Value;
                            FinishSelectionDate = old_endDate.Value;
                            old_startDate = null;
                            old_endDate = null;
                        }
                        else
                        {
                            if (StartSelectionDate == DateTime.MinValue && FinishSelectionDate == DateTime.MinValue)
                            {
                                StartSelectionDate = old_startDate.Value;
                                FinishSelectionDate = old_endDate.Value;
                                old_startDate = null;
                                old_endDate = null;
                            }
                        }
                    }
                    else
                    {
                        old_startDate = StartSelectionDate;
                        old_endDate = FinishSelectionDate;
                    }
                }
                isFromFilterString = false;
            }
        }
        public IList<Template> Templates
        {
            get { return templates; }
            set { templates = value; }
        }

        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }

        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }

        public DataRowView SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
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

        public ObservableCollection<GeosStatus> GeosStatusList
        {
            get { return geosStatusList; }
            set { geosStatusList = value; }
        }

        public bool IsFirstTimeOpen
        {
            get { return isFirstTimeOpen; }
            set { isFirstTimeOpen = value; }
        }

        public bool IsViewRangeControl
        {
            get { return isViewRangeControl; }
            set
            {
                isViewRangeControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewRangeControl"));
            }
        }

        public string GridRowHeight
        {
            get { return gridRowHeight; }
            set
            {
                gridRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeight"));
            }
        }

        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        public ICustomGridService GridService
        {
            get
            {
                return ServiceContainer.GetService<ICustomGridService>();
            }
        }

        public ObservableCollection<TileBarFilters> FilterStatusListOfTile
        {
            get { return filterStatusListOfTile; }
            set
            {
                filterStatusListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStatusListOfTile"));
            }
        }
        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }
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
        public string FilterStringCriteria { get; set; }
        public string FilterStringName { get; set; }
        #endregion // Properties

        #region public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region public ICommand

        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandNewLeadClick { get; set; }
        //public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ViewHideRangeControlCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshLeadViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand UpdateMultipleRowsLeadViewCommand { get; set; }
        public ICommand SaveActionCommand { get; set; }
        public ICommand TableView_ShownEditorCommand { get; set; }
        public ICommand CommandFilterStatusTileClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        //[003] added
        public ICommand CommandShowFilterPopupClick { get; set; }


        #endregion // Command

        #region  Constructor

        public LeadsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor LeadsViewModel...", category: Category.Info, priority: Priority.Low);

                IsInit = true;
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                CustomCellAppearanceCommand = new DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                CommandGridDoubleClick = new DelegateCommand<object>(LeadsEditViewWindowShow);

                //CommandShowFilterPopupClick = new DelegateCommand<object>(LeadsShowFilterValue);

                CommandNewLeadClick = new RelayCommand(new Action<object>(LeadsNewViewWindowShow));

                PrintButtonCommand = new RelayCommand(new Action<object>(PrintLeadsGrid));
                ViewHideRangeControlCommand = new DelegateCommand<object>(ViewHideRangeControl);
                SalesOwnerPopupClosedCommand = new DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                RefreshLeadViewCommand = new RelayCommand(new Action<object>(RefreshLeadDetails));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportLeadsGridButtonCommandAction));
                UpdateMultipleRowsLeadViewCommand = new DelegateCommand<object>(UpdateMultipleRowsCommandAction);
                SaveActionCommand = new DelegateCommand<object>(SaveAction);
                TableView_ShownEditorCommand = new DelegateCommand<EditorEventArgs>(TableView_ShownEditor);
                CommandFilterStatusTileClick = new DelegateCommand<object>(CommandFilterStatusTileClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                StatusList = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());
                //[003]added
                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);

                IsViewRangeControl = true;
                GridRowHeight = "100";
                FillStatusList();
                FillLeadGridDetails();
                FillConfidentialLevelList();
                FillCaroemsList();
                FillBusinessUnitList();
                FillLeadSourceList();
                FillGeosProjectsList();
                FillFilterTileBar();
                AddCustomSetting();
                GeosApplication.Instance.Logger.Log("Constructor LeadsViewModel executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            IsInit = false;
        }

       

        #endregion // Constructor

        #region Methods

        public void SaveAction(object obj)
        {
            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBox.Show("You need to save changes before executing this action. Proceed?", "Save Changes", MessageBoxButton.YesNo);
                //if ()
                //{
                //    MultipleCellEditHelper.IsValueChanged = false;
                //    CanExecute = true;
                //    //RefreshSource();
                //}
            }
        }
        /// <summary>
        /// Method For Multiple Row Edit
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateMultipleRowsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor UpdateMultipleRowsCommandAction...", category: Category.Info, priority: Priority.Low);

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

                view = obj as TableView;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                Offer OfferData = new Offer();
                ActiveSite offerActiveSite;
                bool isoffersave = false;
                bool isAllSave = false;
                string offerCode = string.Empty;
                long offerId = 0;
                Int32 ConnectPlantId = 0;
                IList<Offer> FinalLeadsList = new List<Offer>();
                byte? cellIdBusinessUnit = null;
                byte? cellIdSource = null;
                int? cellIdOEM = null;
                long? CellIdCarProject = null;
                bool IsChecked = false;
                int? cellIdSalesOwner = null;
                cellIdBusinessUnit = Convert.ToByte(BusinessUnitList.Where(b => b.Value == MultipleCellEditHelper.BusinessUnit).Select(u => u.IdLookupValue).FirstOrDefault());
                cellIdSource = Convert.ToByte(LeadSourceList.Where(b => b.Value == MultipleCellEditHelper.LeadSource).Select(u => u.IdLookupValue).FirstOrDefault());
                cellIdOEM = CaroemsList.Where(b => b.Name == MultipleCellEditHelper.OEM).Select(u => u.IdCarOEM).FirstOrDefault();
                CellIdCarProject = GeosProjectsList.Where(b => b.Name == MultipleCellEditHelper.Project).Select(u => u.IdCarProject).FirstOrDefault();
                cellIdSalesOwner = GeosApplication.Instance.SalesOwnerList.Where(b => b.SalesOwner == MultipleCellEditHelper.SalesOwner).Select(u => u.IdSalesOwner).FirstOrDefault();

                for (int i = GeosStatusList.Count - 1; i >= 0; i--)
                {
                    if (GeosStatusList[i].IdOfferStatusType == 3 || GeosStatusList[i].IdOfferStatusType == 5 || GeosStatusList[i].IdOfferStatusType == 6 || GeosStatusList[i].IdOfferStatusType == 7 | GeosStatusList[i].IdOfferStatusType == 8
                        || GeosStatusList[i].IdOfferStatusType == 9 || GeosStatusList[i].IdOfferStatusType == 10 || GeosStatusList[i].IdOfferStatusType == 11 || GeosStatusList[i].IdOfferStatusType == 12 || GeosStatusList[i].IdOfferStatusType == 13 || GeosStatusList[i].IdOfferStatusType == 14)
                    {
                        GeosStatusList.RemoveAt(i);
                    }
                }

                DataRow[] foundRow = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();
                foreach (DataRow item in foundRow)
                {
                    ChangeLogsEntry = new List<LogEntryByOffer>();
                    DataRow dr1 = item;
                    //((System.Data.DataRowView)(item)).Row;
                    ConnectPlantId = Convert.ToInt32(dr1["ConnectPlantId"].ToString());
                    offerId = Convert.ToInt64(dr1["Idoffer"].ToString());
                    offerCode = dr1["Code"].ToString();
                    offerActiveSite = (ActiveSite)dr1["ActiveSite"];
                    IList<Offer> TempLeadsList = new List<Offer>();
                    //[001] added Change Method
                    //[002] added 
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    //[002] Changed service method and controller 
                    TempLeadsList.Add(CrmStartUp.GetOfferDetailsById_V2040(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                    OfferData = TempLeadsList[0];
                    bool IsEditableOffer = false;
                    foreach (var CurrentStatus in GeosStatusList)
                    {
                        if (OfferData.GeosStatus.IdOfferStatusType == CurrentStatus.IdOfferStatusType)
                            IsEditableOffer = true;
                    }

                    //----------[Sprint-71] [11-09-2019] [sdesai]------
                    //[GEOS Workbench / GEOS2-1740] [Close date is not updated in "In Production" offers]
                    if (OfferData.IsGoAheadProduction)
                        IsEditableOffer = true;

                    if (IsEditableOffer == true)
                    {
                        //Updating Following Column values
                        if (OfferData.Description != dr1["Description"].ToString())
                        {
                            string OfferDescriptionOld = OfferData.Description;
                            OfferData.Description = dr1["Description"].ToString();
                            string OfferDescriptionNew = OfferData.Description;
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDescriptionChanged").ToString(), OfferDescriptionOld, OfferDescriptionNew), IdLogEntryType = 9 });
                        }
                        if (!DBNull.Value.Equals((double)dr1["Amount"]))
                        {
                            if (OfferData.Value != (double)dr1["Amount"])
                            {
                                string OfferAmountOld = OfferData.Value.ToString();
                                OfferData.Value = (double)dr1["Amount"];
                                string OfferAmountNew = OfferData.Value.ToString();
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewAmountChanged").ToString(), OfferAmountOld + " " + GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString(), OfferAmountNew + " " + GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()), IdLogEntryType = 8 });
                            }
                        }
                        if (!DBNull.Value.Equals(dr1["OfferCloseDate"]))
                        {
                            if (OfferData.DeliveryDate != null && OfferData.DeliveryDate.Value.Date != Convert.ToDateTime(dr1["OfferCloseDate"]).Date)
                            {
                                if ((DateTime)dr1["OfferCloseDate"] >= GeosApplication.Instance.ServerDateTime.Date)
                                {
                                    string OfferDeliveryDateOld = Convert.ToDateTime(OfferData.DeliveryDate).ToShortDateString();
                                    OfferData.DeliveryDate = Convert.ToDateTime(dr1["OfferCloseDate"]);
                                    string OfferDeliveryDateNew = Convert.ToDateTime(OfferData.DeliveryDate).ToShortDateString();
                                    ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewDeliveryDateChanged").ToString(), OfferDeliveryDateOld, OfferDeliveryDateNew), IdLogEntryType = 7 });
                                }

                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsMsgUpdateOfferFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        if (Convert.ToDateTime(OfferData.DeliveryDate).Date < GeosApplication.Instance.ServerDateTime.Date)
                        {
                            // if (OfferData.GeosStatus.SalesStatusType.IdSalesStatusType != 5)
                            dr1["IsCloseDateExceed"] = true;
                            //else
                            //   dr1["IsCloseDateExceed"] = false;
                        }
                        else
                        {
                            dr1["IsCloseDateExceed"] = false;
                        }

                        if (OfferData.ProbabilityOfSuccess != Convert.ToInt32(dr1["OfferConfidentialLevel"]))
                        {
                            string OfferConfidentialLevelOld = OfferData.ProbabilityOfSuccess.ToString();
                            OfferData.ProbabilityOfSuccess = Convert.ToInt32(dr1["OfferConfidentialLevel"]);
                            if (OfferData.ProbabilityOfSuccess <= 10)
                                OfferData.ProbabilityOfSuccess = 10;
                            else if (OfferData.ProbabilityOfSuccess > 10 && OfferData.ProbabilityOfSuccess <= 20)
                                OfferData.ProbabilityOfSuccess = 20;
                            else if (OfferData.ProbabilityOfSuccess > 20 && OfferData.ProbabilityOfSuccess <= 30)
                                OfferData.ProbabilityOfSuccess = 30;
                            else if (OfferData.ProbabilityOfSuccess > 30 && OfferData.ProbabilityOfSuccess <= 40)
                                OfferData.ProbabilityOfSuccess = 40;
                            else if (OfferData.ProbabilityOfSuccess > 40 && OfferData.ProbabilityOfSuccess <= 50)
                                OfferData.ProbabilityOfSuccess = 50;
                            else if (OfferData.ProbabilityOfSuccess > 50 && OfferData.ProbabilityOfSuccess <= 60)
                                OfferData.ProbabilityOfSuccess = 60;
                            else if (OfferData.ProbabilityOfSuccess > 60 && OfferData.ProbabilityOfSuccess <= 70)
                                OfferData.ProbabilityOfSuccess = 70;
                            else if (OfferData.ProbabilityOfSuccess > 70 && OfferData.ProbabilityOfSuccess <= 80)
                                OfferData.ProbabilityOfSuccess = 80;
                            else if (OfferData.ProbabilityOfSuccess > 80 && OfferData.ProbabilityOfSuccess <= 90)
                                OfferData.ProbabilityOfSuccess = 90;
                            else if (OfferData.ProbabilityOfSuccess > 90 && OfferData.ProbabilityOfSuccess <= 100)
                                OfferData.ProbabilityOfSuccess = 100;

                            string OfferConfidentialLevelNew = OfferData.ProbabilityOfSuccess.ToString();
                            ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewConfidenceChanged").ToString(), OfferConfidentialLevelOld + " %", OfferConfidentialLevelNew + " %"), IdLogEntryType = 8 });
                        }

                        if (cellIdBusinessUnit.HasValue)
                        {
                            byte? businessUnitIdCurrent = Convert.ToByte(BusinessUnitList.Where(bus => bus.Value == dr1["BusinessUnit"].ToString()).Select(bunn => bunn.IdLookupValue).FirstOrDefault());
                            if (OfferData.IdBusinessUnit == null)
                                OfferData.IdBusinessUnit = 0;
                            if (OfferData.IdBusinessUnit != businessUnitIdCurrent)
                            {
                                string businessUnitNameOld = string.Empty;
                                string businessUnitNameNew = string.Empty;
                                if (OfferData.IdBusinessUnit != null)
                                {
                                    businessUnitNameOld = BusinessUnitList.Where(bu => bu.IdLookupValue == Convert.ToByte(OfferData.IdBusinessUnit.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                OfferData.IdBusinessUnit = businessUnitIdCurrent;
                                if (OfferData.IdBusinessUnit != null)
                                {
                                    businessUnitNameNew = BusinessUnitList.Where(bu => bu.IdLookupValue == Convert.ToByte(OfferData.IdBusinessUnit.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewBusinessUnitChanged").ToString(), businessUnitNameOld, businessUnitNameNew), IdLogEntryType = 8 });
                            }
                        }

                        if (cellIdSource.HasValue)
                        {
                            byte? leadSourceIdCurrent = Convert.ToByte(LeadSourceList.Where(bus => bus.Value == dr1["Source"].ToString()).Select(bunn => bunn.IdLookupValue).FirstOrDefault());
                            if (OfferData.IdSource == null)
                            {
                                OfferData.IdSource = 0;
                            }
                            if (OfferData.IdSource != leadSourceIdCurrent)
                            {
                                string leadSourceNameOld = string.Empty;
                                string leadSourceNameNew = string.Empty;
                                if (OfferData.IdSource != null)
                                {
                                    leadSourceNameOld = LeadSourceList.Where(ls => ls.IdLookupValue == Convert.ToByte(OfferData.IdSource.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                OfferData.IdSource = leadSourceIdCurrent;
                                if (OfferData.IdSource != null)
                                {
                                    leadSourceNameNew = LeadSourceList.Where(ls => ls.IdLookupValue == Convert.ToByte(OfferData.IdSource.ToString())).Select(bun => bun.Value).SingleOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSourceChanged").ToString(), leadSourceNameOld, leadSourceNameNew), IdLogEntryType = 8 });
                            }
                        }

                        if (cellIdOEM.HasValue)
                        {
                            int? CaroemIdCurrent = CaroemsList.Where(bus => bus.Name == dr1["OEM"].ToString()).Select(bunn => bunn.IdCarOEM).FirstOrDefault();
                            if (OfferData.IdCarOEM == null)
                                OfferData.IdCarOEM = 0;
                            if (OfferData.IdCarOEM != CaroemIdCurrent)
                            {
                                string CaroemNameOld = string.Empty;
                                string CaroemNameNew = string.Empty;
                                if (OfferData.IdCarOEM != null)
                                {
                                    CaroemNameOld = CaroemsList.Where(coem => coem.IdCarOEM == OfferData.IdCarOEM).Select(gcoem => gcoem.Name).SingleOrDefault();
                                }
                                OfferData.IdCarOEM = CaroemIdCurrent;

                                if (OfferData.IdCarOEM != null)
                                {
                                    CaroemNameNew = CaroemsList.Where(coem => coem.IdCarOEM == OfferData.IdCarOEM).Select(gcoem => gcoem.Name).SingleOrDefault();
                                }

                                if (CaroemNameOld == "---")
                                    CaroemNameOld = "None";
                                if (CaroemNameNew == "---")
                                    CaroemNameNew = "None";
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewCarsOemChanged").ToString(), CaroemNameOld, CaroemNameNew), IdLogEntryType = 8 });
                            }
                        }

                        if (cellIdSalesOwner.HasValue)
                        {
                            int? SalesOwnerIdCurrent = GeosApplication.Instance.SalesOwnerList.Where(csaleowner => csaleowner.SalesOwner == dr1["SalesOwner"].ToString()).Select(gsaleowner => gsaleowner.IdSalesOwner).FirstOrDefault();
                            if (OfferData.IdSalesOwner == null)
                                OfferData.IdSalesOwner = 0;
                            if (OfferData.IdSalesOwner != SalesOwnerIdCurrent)
                            {
                                string SalesOwnerOld = string.Empty;
                                string SalesOwnerNew = string.Empty;
                                if (OfferData.IdSalesOwner != null)
                                {
                                    SalesOwnerOld = GeosApplication.Instance.SalesOwnerList.Where(csaleowner => csaleowner.IdSalesOwner == OfferData.IdSalesOwner).Select(gsaleowner => gsaleowner.SalesOwner).FirstOrDefault();
                                }
                                OfferData.IdSalesOwner = SalesOwnerIdCurrent;

                                if (OfferData.IdSalesOwner != null)
                                {
                                    SalesOwnerNew = GeosApplication.Instance.SalesOwnerList.Where(csaleowner => csaleowner.IdSalesOwner == OfferData.IdSalesOwner).Select(gsaleowner => gsaleowner.SalesOwner).FirstOrDefault();
                                }
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewSalesOwnerChanged").ToString(), SalesOwnerOld, SalesOwnerNew), IdLogEntryType = 8 });
                            }
                        }

                        if (CellIdCarProject.HasValue)
                        {
                            long? CarProjectIdCurrent = GeosProjectsList.Where(bus => bus.Name == dr1["Project"].ToString()).Select(bunn => bunn.IdCarProject).FirstOrDefault();
                            if (OfferData.IdCarProject == null || OfferData.IdCarProject == 0)
                                OfferData.IdCarProject = null;
                            if (CarProjectIdCurrent == null || CarProjectIdCurrent == 0)
                                CarProjectIdCurrent = null;

                            if (OfferData.IdCarProject != CarProjectIdCurrent)
                            {
                                string CarProjectNameOld = string.Empty;
                                string CarProjectNameNew = string.Empty;
                                if (OfferData.IdCarProject != null)
                                {
                                    CarProjectNameOld = GeosProjectsList.Where(carproj => carproj.IdCarProject == OfferData.IdCarProject).Select(carproject => carproject.Name).SingleOrDefault();
                                }
                                if (CarProjectNameOld == null || CarProjectNameOld == string.Empty)
                                    CarProjectNameOld = "None";

                                OfferData.IdCarProject = CarProjectIdCurrent;

                                if (OfferData.IdCarProject != null)
                                {
                                    CarProjectNameNew = GeosProjectsList.Where(carproj => carproj.IdCarProject == OfferData.IdCarProject).Select(carproject => carproject.Name).SingleOrDefault();
                                }
                                if (CarProjectNameNew == null || CarProjectNameNew == string.Empty)
                                    CarProjectNameNew = "None";
                                ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewProjectChanged").ToString(), CarProjectNameOld, CarProjectNameNew), IdLogEntryType = 8 });
                            }
                        }

                        OfferData.OptionsByOffers.Clear();
                        tempOptionsByOfferList = new List<OptionsByOffer>();
                        List<Int64> tempremovelist = new List<Int64>() { 6, 19, 21, 23, 25, 27 };
                        offerOptions = offerOptions.Where(t => !tempremovelist.Contains(t.IdOfferOption)).ToList();

                        foreach (var item22 in view.AutoFilterRowData.FixedNoneCellData)
                        {
                            if (OfferOptions.Any(op => op.Name == item22.Column.FieldName))
                            {
                                OptionsByOffer optionsByOffer = new OptionsByOffer();
                                optionsByOffer.IdOffer = OfferData.IdOffer;
                                optionsByOffer.IdOption = Convert.ToInt64(OfferOptions.Where(op => op.Name == item22.Column.FieldName).Select(ott => ott.IdOfferOption).FirstOrDefault());
                                optionsByOffer.OfferOption = offerOptions.FirstOrDefault(x => x.IdOfferOption == optionsByOffer.IdOption);
                                if (dr1[item22.Column.FieldName] != DBNull.Value)
                                    optionsByOffer.Quantity = Convert.ToInt32(dr1[item22.Column.FieldName]);
                                optionsByOffer.IsSelected = true;
                                if (dr1[item22.Column.FieldName] != DBNull.Value && optionsByOffer.Quantity != 0)
                                {
                                    tempOptionsByOfferList.Add(optionsByOffer);
                                }
                                //ChangeLogsEntry.Add(new LogEntryByOffer() { IdOffer = OfferData.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewQuantityChanged").ToString(), quantityOld, quantityNew), IdLogEntryType = 8 });
                            }
                        }

                        if (ChangeLogsEntry != null)
                            OfferData.LogEntryByOffers = ChangeLogsEntry;
                        OfferData.OptionsByOffers = tempOptionsByOfferList;
                        OfferData.ModifiedIn = DateTime.Now;
                        OfferData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        //OfferData.Site.ConnectPlantConstr = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == TempLeadsList[0].Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                        OfferData.OfferActiveSite = offerActiveSite;
                        FinalLeadsList.Add(OfferData);
                    }

                    dr1["ischecked"] = false;
                }

                foreach (Offer item in FinalLeadsList)
                {
                    //[002] Added
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(item.OfferActiveSite.SiteServiceProvider);
                    //[002] Changed service method and controller 
                    isoffersave = CrmStartUpOfferActiveSite.UpdateOfferForParticularColumn_V2040(item, item.OfferActiveSite.IdSite, GeosApplication.Instance.ActiveUser.IdUser);
                    if (isoffersave)
                        isAllSave = true;
                    else
                        isAllSave = false;
                }

                if (isAllSave)
                {
                    //DataRow[] foundRow1 = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray().ToList().ForEach(a => a.IsNew = 0);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsMsgUpdateOfferSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeadsMsgUpdateOfferFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                }
                MultipleCellEditHelper.SetIsValueChanged(view, false);
                MultipleCellEditHelper.IsValueChanged = false;
                GeosApplication.Instance.Logger.Log("Constructor UpdateMultipleRowsCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Lead Grid details.
        /// </summary>
        private void FillLeadGridDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                if (GeosApplication.Instance.IdUserPermission == 21 || GeosApplication.Instance.IdUserPermission == 22)
                {
                    // Called for 1st Time.
                    AddDataTableColumns();
                    FillLeadsByUser();
                    FillDataTable();
                }
                else
                {
                    AddDataTableColumns();
                    FillLeads();
                    FillDataTable();
                }

                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadGridDetails() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                }
                MultipleCellEditHelper.IsValueChanged = false;
            }

            MultipleCellEditHelper.IsValueChanged = false;
            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
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

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                AddDataTableColumns();
                FillLeadsByUser();
                FillDataTable();
                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
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

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                AddDataTableColumns();
                FillLeadsByUser();
                FillDataTable();

                if (FailedPlants != null && FailedPlants.Count > 0)
                {
                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else
            {
                Dttable.Rows.Clear();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
            }

            FillFilterTileBar();
            AddCustomSetting();

            Timer tmrOnce = new Timer();
            tmrOnce.Tick += tmrOnce_Tick;
            tmrOnce.Interval = new TimeSpan(001);
            tmrOnce.Start();

            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method for refresh Lead Grid From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshLeadDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshLeadDetails ...", category: Category.Info, priority: Priority.Low);

            view = MultipleCellEditHelper.Viewtableview;
            if (MultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);
                }

                MultipleCellEditHelper.IsValueChanged = false;
            }

            MultipleCellEditHelper.IsValueChanged = false;

            if (view != null)
            {
                MultipleCellEditHelper.SetIsValueChanged(view, false);
            }

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

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

            StartSelectionDate = DateTime.MinValue;
            FinishSelectionDate = DateTime.MinValue;
            // code for hide column chooser if empty
            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;

            int visibleFalseCoulumn = 0;
            foreach (GridColumn column in gridControl.Columns)
            {
                if (column.Visible == false
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                  && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                {
                    visibleFalseCoulumn++;
                }
            }

            if (visibleFalseCoulumn > 0)
            {
                IsTimelineColumnChooserVisible = true;
            }
            else
            {
                IsTimelineColumnChooserVisible = false;
            }

            FillLeadGridDetails();
            FillBusinessUnitList();
            FillLeadSourceList();
            FillFilterTileBar();
            AddCustomSetting();

            detailView.SearchString = null;
            Timer tmrOnce = new Timer();
            tmrOnce.Tick += tmrOnce_Tick;
            tmrOnce.Interval = new TimeSpan(001);
            tmrOnce.Start();
            if (FilterStatusListOfTile.Count > 0)
                SelectedTileBarItem = FilterStatusListOfTile[0];

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshLeadDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This ticker for restore grid layout on refresh button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrOnce_Tick(object sender, EventArgs e)
        {
            GridService.Refresh();

            ((Timer)sender).IsEnabled = false;
            ((Timer)sender).Dispose();
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// [002][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="obj"></param>
        private void LeadsEditViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                if ((System.Data.DataRowView)detailView.DataControl.CurrentItem != null)
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        // DXSplashScreen.Show<SplashScreenView>(); 
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

                    string offerCode = ((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[2].ToString();
                    long offerId = Convert.ToInt64(((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[1].ToString());
                    Int32 ConnectPlantId = Convert.ToInt32(((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[0].ToString());
                    ActiveSite offerActiveSite = (ActiveSite)((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[36];
                    IList<Offer> TempLeadsList = new List<Offer>();
                    //[001] added Change Method
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    //[001] Changed service method and controller
                    TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));
                    //[001] Changed service method and controller
                    LostReasonsByOffer lostReasonsByOffer = CrmStartUpOfferActiveSite.GetLostReasonsByOffer_V2040(offerId);

                    if (lostReasonsByOffer != null)
                    {
                        TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                    LeadsEditView leadsEditView = new LeadsEditView();

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                    {
                        leadsEditViewModel.IsControlEnable = true;
                        //leadsEditViewModel.IsControlEnableorder = false;
                        leadsEditViewModel.IsStatusChangeAction = true;
                        leadsEditViewModel.IsAcceptControlEnableorder = false;

                        //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                        //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                        leadsEditViewModel.IsAcceptEnable = false;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                        {
                            leadsEditViewModel.IsAcceptEnable = true;
                        }

                        leadsEditViewModel.InItLeadsEditReadonly(TempLeadsList);
                    }
                    else
                    {
                        leadsEditViewModel.ForLeadOpen = true;
                        leadsEditViewModel.InIt(TempLeadsList);
                    }

                    EventHandler handle = delegate { leadsEditView.Close(); };
                    leadsEditViewModel.RequestClose += handle;
                    leadsEditView.DataContext = leadsEditViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    //IsTimelineColumnChooserVisible = false;
                    leadsEditView.ShowDialogWindow();

                    if (leadsEditViewModel.OfferData != null)
                    {
                        FillGeosProjectsList();

                        DataRow dataRow = Dttable.AsEnumerable().FirstOrDefault(row => Convert.ToInt64(row["Idoffer"]) == offerId && Convert.ToInt32(row["ConnectPlantId"]) == ConnectPlantId);

                        dataRow["Idoffer"] = leadsEditViewModel.OfferData.IdOffer;

                        Int64? idCategory = CrmStartUp.GetIdProductCategoryByIdOffer(leadsEditViewModel.OfferData.IdOffer, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault().ConnectPlantConstr);

                        if (idCategory > 0)
                        {
                            ProductCategory objProCat = new ProductCategory();
                            objProCat = leadsEditViewModel.ListProductCategory.Where(i => i.IdProductCategory == idCategory.Value).FirstOrDefault();

                            if (objProCat.MergedCategoryAndProduct != null)
                            {
                                objProCat.MergedCategoryAndProduct = objProCat.MergedCategoryAndProduct.Replace("-", "");
                                string[] strArray = objProCat.MergedCategoryAndProduct.Split('>');
                                if (strArray.Count() < 2)
                                {
                                    dataRow["Category1"] = strArray[0];
                                    dataRow["Category2"] = null;
                                }
                                else
                                {
                                    dataRow["Category1"] = strArray[0];
                                    dataRow["Category2"] = strArray[1];
                                }
                            }
                        }

                        dataRow["Code"] = leadsEditViewModel.OfferData.Code;

                        if (leadsEditViewModel.OfferData.Site != null)
                        {
                            if (leadsEditViewModel.OfferData.Site.Customers != null && leadsEditViewModel.OfferData.Site.Customers.Count > 0)
                            {
                                dataRow["Group"] = leadsEditViewModel.OfferData.Site.Customers[0].CustomerName;
                            }

                            dataRow["ConnectPlantId"] = leadsEditViewModel.OfferData.OfferActiveSite.IdSite;
                            dataRow["Site"] = GeosApplication.Instance.CompanyList.Where(cmplst => cmplst.ConnectPlantId == dataRow["ConnectPlantId"].ToString()).Select(slt => slt.Alias).FirstOrDefault();
                            //= GeosApplication.Instance.EmdepSiteList.Where(esn => esn.IdCompany == int.Parse(dataRow["ConnectPlantId"].ToString())).Select(esns => esns.Alias).FirstOrDefault();
                            dataRow["ActiveSite"] = leadsEditViewModel.OfferData.OfferActiveSite;
                            dataRow["IdSite"] = leadsEditViewModel.OfferData.Site.IdCompany;
                            dataRow["Plant"] = leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                            //Name.Substring(0, leadsEditViewModel.OfferData.Site.Name.IndexOf("("));

                            if (leadsEditViewModel.OfferData.Site.Country != null)
                            {
                                dataRow["Country"] = leadsEditViewModel.OfferData.Site.Country.Name;
                                dataRow["Region"] = leadsEditViewModel.OfferData.Site.Country.Zone != null ? leadsEditViewModel.OfferData.Site.Country.Zone.Name : "";
                            }

                            // dataRow["Country"] = System.Text.RegularExpressions.Regex.Match(leadsEditViewModel.OfferData.Site.Name, @"\(([^)]*)\)").Groups[1].Value; //leadsEditViewModel.OfferData.Site.Country.Name.ToString();
                        }

                        dataRow["Description"] = leadsEditViewModel.OfferData.Description;

                        if (leadsEditViewModel.OfferData.GeosStatus != null)
                        {
                            dataRow["Status"] = leadsEditViewModel.OfferData.GeosStatus.Name;
                            dataRow["HtmlColor"] = leadsEditViewModel.OfferData.GeosStatus.HtmlColor;
                            // dataRow["IdOfferStatusType"] = leadsEditViewModel.OfferData.GeosStatus.IdOfferStatusType;
                        }

                        dataRow["Amount"] = leadsEditViewModel.OfferData.OfferValue;
                        dataRow["OfferCloseDate"] = leadsEditViewModel.OfferData.DeliveryDate;
                        dataRow["IsCloseDateExceed"] = false;
                        dataRow["OfferConfidentialLevel"] = leadsEditViewModel.OfferData.ProbabilityOfSuccess.ToString();

                        dataRow["SalesOwner"] = leadsEditViewModel.OfferData.SalesOwner != null ? leadsEditViewModel.OfferData.SalesOwner.FullName : "";

                        //[002] added
                        if (leadsEditViewModel.ListAddedOfferContact != null)
                        {
                            List<OfferContact> OfferContactlist = leadsEditViewModel.ListAddedOfferContact.Where(x => x.IsDeleted != true).ToList();

                            if (OfferContactlist.Count > 0)
                            {
                                dataRow["OfferedTo"] = String.Join("\n", OfferContactlist.Select(x => x.People.FullName).Distinct().ToArray());
                            }
                            else
                            {
                                dataRow["OfferedTo"] = null;
                            }
                        }
                        else
                            dataRow["OfferedTo"] = null;

                        if (leadsEditViewModel.OfferOwnerList.Count > 0)
                        {
                            User user = leadsEditViewModel.OfferOwnerList.Where(x => x.IdUser == leadsEditViewModel.OfferData.OfferedBy).FirstOrDefault();

                            if (!string.IsNullOrEmpty(user.FullName))
                            {
                                dataRow["OfferOwner"] = user.FullName;

                            }
                            else
                                dataRow["OfferOwner"] = null;
                        }


                        if (leadsEditViewModel.OfferData.CarProject != null)
                            dataRow["Project"] = leadsEditViewModel.OfferData.CarProject.Name;
                        if (leadsEditViewModel.OfferData.CarOEM != null)
                            dataRow["OEM"] = leadsEditViewModel.OfferData.CarOEM.Name;
                        if (leadsEditViewModel.OfferData.Source != null)
                            dataRow["Source"] = leadsEditViewModel.OfferData.Source.Value;
                        if (leadsEditViewModel.OfferData.BusinessUnit != null)
                            dataRow["BusinessUnit"] = leadsEditViewModel.OfferData.BusinessUnit.Value;

                        if (leadsEditViewModel.OfferData.RFQReception == null)
                        {
                            dataRow["RFQReceptionDate"] = DBNull.Value;
                        }
                        else if (leadsEditViewModel.OfferData.RFQReception != null && leadsEditViewModel.OfferData.RFQReception == DateTime.MinValue)
                            dataRow["RFQReceptionDate"] = DBNull.Value;
                        else
                            dataRow["RFQReceptionDate"] = leadsEditViewModel.OfferData.RFQReception.Value;

                        if (leadsEditViewModel.OfferData.Rfq == null)
                        {
                            dataRow["Rfq"] = DBNull.Value;
                        }
                        else
                        {
                            dataRow["Rfq"] = leadsEditViewModel.OfferData.Rfq;
                        }
                        if (leadsEditViewModel.OfferData.SendIn == null)
                        {
                            dataRow["QuoteSentDate"] = DBNull.Value;
                        }
                        else if (leadsEditViewModel.OfferData.SendIn != null && leadsEditViewModel.OfferData.SendIn == DateTime.MinValue)
                            dataRow["QuoteSentDate"] = DBNull.Value;
                        else
                            dataRow["QuoteSentDate"] = leadsEditViewModel.OfferData.SendIn.Value;
                        if (leadsEditViewModel.OfferData.GeosStatus.IdOfferStatusType == 1)
                        {
                            if (leadsEditViewModel.OfferData.LastActivityDate == null)
                            {
                                dataRow["SleepDays"] = Convert.ToInt32((GeosApplication.Instance.ServerDateTime.Date - TempLeadsList[0].LastActivityDate.Value.Date).TotalDays);
                            }
                            else
                            {
                                dataRow["SleepDays"] = Convert.ToInt32((GeosApplication.Instance.ServerDateTime.Date - leadsEditViewModel.OfferData.LastActivityDate.Value.Date).TotalDays);
                            
                            }
                        }

                        foreach (var item in leadsEditViewModel.OfferData.OptionsByOffers)
                        {
                            if (item.OfferOption != null && item.IsSelected && item.Quantity > 0)
                            {
                                dataRow[item.OfferOption.Name] = item.Quantity;
                            }
                            else
                            {
                                dataRow[item.OfferOption.Name] = "";
                            }
                        }

                        //[CRM-M040-04]
                        if (leadsEditViewModel.OfferData.LostReasonsByOffer != null)
                        {
                            if (leadsEditViewModel.OfferData.LostReasonsByOffer.Competitor != null)
                                dataRow["LostCompetitor"] = leadsEditViewModel.OfferData.LostReasonsByOffer.Competitor.Name;

                            if (leadsEditViewModel.OfferData.LostReasonsByOffer.OfferLostReason != null)
                                dataRow["LostReason"] = leadsEditViewModel.OfferData.LostReasonsByOffer.OfferLostReason.Name;

                            dataRow["LostDescription"] = leadsEditViewModel.OfferData.LostReasonsByOffer.Comments;
                        }
                        Dttable.AcceptChanges();
                    }

                    if (leadsEditViewModel.OfferDataLst != null && leadsEditViewModel.OfferDataLst.Count > 0)
                    {
                        foreach (var OfferDataItem in leadsEditViewModel.OfferDataLst)
                        {
                            DataRow dataRow;

                            //if offer is old then updates its value on grid.

                            if (TempLeadsList[0].IdOffer == OfferDataItem.IdOffer)
                                dataRow = Dttable.AsEnumerable().FirstOrDefault(row => Convert.ToInt64(row["Idoffer"]) == offerId);
                            else
                                dataRow = Dttable.NewRow();

                            dataRow["Idoffer"] = OfferDataItem.IdOffer;

                            dataRow["Code"] = OfferDataItem.Code;

                            if (OfferDataItem.Site != null)
                            {
                                if (OfferDataItem.Site.Customers != null && OfferDataItem.Site.Customers.Count > 0)
                                {
                                    dataRow["Group"] = OfferDataItem.Site.Customers[0].CustomerName;
                                }

                                dataRow["ConnectPlantId"] = OfferDataItem.OfferActiveSite.IdSite;
                                dataRow["Site"] = GeosApplication.Instance.CompanyList.Where(cmplst => cmplst.ConnectPlantId == dataRow["ConnectPlantId"].ToString()).Select(slt => slt.Alias).FirstOrDefault();
                                dataRow["ActiveSite"] = OfferDataItem.OfferActiveSite;
                                dataRow["IdSite"] = OfferDataItem.Site.IdCompany;
                                dataRow["Plant"] = OfferDataItem.Site.SiteNameWithoutCountry;

                                if (OfferDataItem.Site.Country != null)
                                {
                                    dataRow["Country"] = OfferDataItem.Site.Country.Name;
                                    dataRow["Region"] = OfferDataItem.Site.Country.Zone != null ? OfferDataItem.Site.Country.Zone.Name : "";
                                }
                            }

                            dataRow["Description"] = OfferDataItem.Description;

                            if (OfferDataItem.GeosStatus != null)
                            {
                                dataRow["Status"] = OfferDataItem.GeosStatus.Name;
                                dataRow["HtmlColor"] = OfferDataItem.GeosStatus.HtmlColor;
                            }

                            dataRow["Amount"] = OfferDataItem.OfferValue;
                            dataRow["OfferCloseDate"] = OfferDataItem.DeliveryDate;
                            dataRow["IsCloseDateExceed"] = false;
                            dataRow["OfferConfidentialLevel"] = OfferDataItem.ProbabilityOfSuccess.ToString();

                            //Fill List For Different Sales Owner
                            var SalesOwners = CrmStartUp.GetSalesOwnerBySiteId(OfferDataItem.Site.IdCompany);
                            if (!GeosApplication.Instance.SalesOwnerList.Any(x => x.IdSite == OfferDataItem.Site.IdCompany))
                            {
                                foreach (var item in SalesOwners)
                                {
                                    SalesOwnerList _salesOwner = new SalesOwnerList();
                                    _salesOwner.IdOffer = OfferDataItem.IdOffer;
                                    _salesOwner.IdSalesOwner = item.IdPerson;
                                    _salesOwner.SalesOwner = item.FullName;
                                    if (OfferDataItem.Site != null)
                                    {
                                        _salesOwner.IdSite = Convert.ToInt32(OfferDataItem.Site.IdCompany);
                                        //_salesOwner.IdSite = Convert.ToInt16(OfferDataItem.Site.ConnectPlantId);
                                    }
                                    //else
                                    //{
                                    //    _salesOwner.IdSite = 0;
                                    //}
                                    GeosApplication.Instance.SalesOwnerList.Add(_salesOwner);
                                }
                            }

                            dataRow["SalesOwner"] = OfferDataItem.SalesOwner != null ? OfferDataItem.SalesOwner.FullName : "";

                            foreach (var item in OfferDataItem.OptionsByOffers)
                            {
                                if (item.OfferOption != null && item.IsSelected && item.Quantity > 0)
                                {
                                    dataRow[item.OfferOption.Name] = item.Quantity;
                                }
                                else
                                {
                                    dataRow[item.OfferOption.Name] = "";
                                }
                            }

                            OfferDataItem.CarProject = (CarProject)GeosProjectsList.Where(item1 => item1.IdCarProject == OfferDataItem.IdCarProject).FirstOrDefault();
                            OfferDataItem.CarOEM = (CarOEM)CaroemsList.Where(item2 => item2.IdCarOEM == OfferDataItem.IdCarOEM).FirstOrDefault();
                            OfferDataItem.BusinessUnit = (LookupValue)BusinessUnitList.Where(item3 => item3.IdLookupValue == OfferDataItem.IdBusinessUnit).FirstOrDefault();
                            OfferDataItem.Source = (LookupValue)LeadSourceList.Where(item4 => item4.IdLookupValue == OfferDataItem.IdSource).FirstOrDefault();

                            if (OfferDataItem.CarProject != null)
                                dataRow["Project"] = OfferDataItem.CarProject.Name;
                            if (OfferDataItem.CarOEM != null)
                                dataRow["OEM"] = OfferDataItem.CarOEM.Name;
                            if (OfferDataItem.Source != null)
                                dataRow["Source"] = OfferDataItem.Source.Value;
                            if (OfferDataItem.BusinessUnit != null)
                                dataRow["BusinessUnit"] = OfferDataItem.BusinessUnit.Value;

                            if (OfferDataItem.RFQReception == null)
                            {
                                dataRow["RFQReceptionDate"] = DBNull.Value;
                            }
                            else if (OfferDataItem.RFQReception != null && OfferDataItem.RFQReception == DateTime.MinValue)
                                dataRow["RFQReceptionDate"] = DBNull.Value;
                            else
                                dataRow["RFQReceptionDate"] = OfferDataItem.RFQReception.Value;

                            if (OfferDataItem.SendIn == null)
                            {
                                dataRow["QuoteSentDate"] = DBNull.Value;
                            }
                            else if (OfferDataItem.SendIn != null && OfferDataItem.SendIn == DateTime.MinValue)
                                dataRow["QuoteSentDate"] = DBNull.Value;
                            else
                                dataRow["QuoteSentDate"] = OfferDataItem.SendIn.Value;

                            //if offer is new then add it in Dttable.
                            if (TempLeadsList[0].IdOffer != OfferDataItem.IdOffer)
                                Dttable.Rows.Add(dataRow);
                        }

                        SelectedObject = Dttable.DefaultView[(Dttable.Rows.Count - 1)];
                    }
                }

                // code for hide column chooser if empty
                GridControl gridControl = (detailView).Grid;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsTimelineColumnChooserVisible = true;
                }
                else
                {
                    IsTimelineColumnChooserVisible = false;
                }

                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void LeadsShowFilterValue(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue...", category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in LeadsShowFilterValue() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        /// <summary>
        /// [000][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {

            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName != "OfferedTo")
                {
                    return;
                }

                List<object> filterItems = new List<object>();

              if (e.Column.FieldName == "OfferedTo")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("OfferedTo = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("OfferedTo <> ''")
                    });

                    foreach (DataRow row in Dttable.Rows)
                    {
                        string OfferedTo = row.Field<string>("OfferedTo");

                        if (OfferedTo == null)
                        {
                            continue;
                        }
                        else if (OfferedTo != null)
                        {

                            if (OfferedTo.Contains("\n"))
                            {
                                string tempOfferedTo = OfferedTo;

                                for (int index = 0; index < tempOfferedTo.Length; index++)
                                {
                                    string offeredTo = tempOfferedTo.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == offeredTo))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = offeredTo;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OfferedTo Like '%{0}%'", offeredTo));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempOfferedTo.Contains("\n"))
                                        tempOfferedTo = tempOfferedTo.Remove(0, offeredTo.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == OfferedTo))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = OfferedTo;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("OfferedTo Like '%{0}%'", OfferedTo));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }

                        }

                    }
                }
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        /// <param name="obj"></param>
        private void LeadsNewViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsNewViewWindowShow...", category: Category.Info, priority: Priority.Low);
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

                LeadAddViewModel leadAddViewModel = new LeadAddViewModel();
                LeadsAddView leadsAddView = new LeadsAddView();
                EventHandler handle = delegate { leadsAddView.Close(); };
                leadAddViewModel.RequestClose += handle;
                leadsAddView.DataContext = leadAddViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //IsTimelineColumnChooserVisible = false;
                leadsAddView.ShowDialogWindow();

                if (leadAddViewModel.OfferData != null)
                {
                    FillGeosProjectsList();
                    leadAddViewModel.OfferData.CreatedIn = GeosApplication.Instance.ServerDateTime.Date;
                    // TimelineGridList.Add(leadAddViewModel.OfferData);

                    DataRow dataRow = Dttable.Rows.Add();

                    dataRow["Idoffer"] = leadAddViewModel.OfferData.IdOffer;
                    dataRow["Code"] = leadAddViewModel.OfferData.Code;

                    dataRow["ConnectPlantId"] = leadAddViewModel.OfferData.OfferActiveSite.IdSite;

                    if (leadAddViewModel.OfferData.Site != null)
                    {
                        if (leadAddViewModel.OfferData.Site.Customers != null && leadAddViewModel.OfferData.Site.Customers.Count > 0)
                        {
                            dataRow["Group"] = leadAddViewModel.OfferData.Site.Customers[0].CustomerName;
                        }

                        dataRow["Site"] = GeosApplication.Instance.CompanyList.Where(cmplst => cmplst.ConnectPlantId == dataRow["ConnectPlantId"].ToString()).Select(slt => slt.Alias).FirstOrDefault();
                        //= GeosApplication.Instance.EmdepSiteList.Where(esn => esn.IdCompany == int.Parse(dataRow["ConnectPlantId"].ToString())).Select(esns => esns.Alias).FirstOrDefault();
                        dataRow["ActiveSite"] = new ActiveSite() { IdSite = GeosApplication.Instance.ActiveIdSite, SiteAlias = GeosApplication.Instance.SiteName, SiteServiceProvider = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString() };
                        dataRow["IdSite"] = leadAddViewModel.OfferData.Site.IdCompany;
                        dataRow["Plant"] = leadAddViewModel.OfferData.Site.SiteNameWithoutCountry;   // Name.Substring(0, leadAddViewModel.OfferData.Site.Name.IndexOf("("));

                        if (leadAddViewModel.OfferData.Site.Country != null)
                        {
                            dataRow["Country"] = leadAddViewModel.OfferData.Site.Country.Name;   // System.Text.RegularExpressions.Regex.Match(leadAddViewModel.OfferData.Site.Name, @"\(([^)]*)\)").Groups[1].Value; //leadsEditViewModel.OfferData.Site.Country.Name.ToString();
                            dataRow["Region"] = leadAddViewModel.OfferData.Site.Country.Zone != null ? leadAddViewModel.OfferData.Site.Country.Zone.Name : "";
                        }
                    }

                    dataRow["Description"] = leadAddViewModel.OfferData.Description;

                    //[000] added
                    if (leadAddViewModel.OfferData.OfferContacts != null)
                    {
                        if (leadAddViewModel.OfferData.OfferContacts.Count > 0)
                        {
                            dataRow["OfferedTo"] = String.Join("\n", leadAddViewModel.OfferData.OfferContacts.Select(x => x.People.FullName).Distinct().ToArray());
                        }
                        else
                        {
                            dataRow["OfferedTo"] = null;
                        }
                    }
                    else
                        dataRow["OfferedTo"] = null;
          
                    if (leadAddViewModel.OfferOwnerList.Count > 0)
                    {
                        User user = leadAddViewModel.OfferOwnerList.Where(x => x.IdUser == leadAddViewModel.OfferData.OfferedBy).FirstOrDefault();

                        if (!string.IsNullOrEmpty(user.FullName))
                        {
                            dataRow["OfferOwner"] = user.FullName;

                        }
                        else
                            dataRow["OfferOwner"] = null;
                    }

                    if (leadAddViewModel.OfferData.GeosStatus != null)
                    {
                        dataRow["Status"] = leadAddViewModel.OfferData.GeosStatus.Name;
                        dataRow["HtmlColor"] = leadAddViewModel.OfferData.GeosStatus.HtmlColor;
                    }
                    if (leadAddViewModel.Currencies[leadAddViewModel.SelectedIndexCurrency].Name != GeosApplication.Instance.UserSettings["SelectedCurrency"])
                    {
                        Currency toCurrency = leadAddViewModel.Currencies.FirstOrDefault(x => x.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"]);
                        leadAddViewModel.OfferData.Value = CrmStartUp.GetOfferAmountByCurrencyConversion(leadAddViewModel.OfferReturnValue.IdOffer, leadAddViewModel.OfferData.Value, leadAddViewModel.Currencies[leadAddViewModel.SelectedIndexCurrency].IdCurrency, toCurrency.IdCurrency, leadAddViewModel.OfferData.DeliveryDate, leadAddViewModel.OfferData.SendIn, leadAddViewModel.OfferData.RFQReception, DateTime.Now, null);
                    }
                    dataRow["Amount"] = leadAddViewModel.OfferData.Value;
                    dataRow["OfferCloseDate"] = leadAddViewModel.OfferData.DeliveryDate;
                    dataRow["IsCloseDateExceed"] = false;
                    dataRow["OfferConfidentialLevel"] = leadAddViewModel.OfferData.ProbabilityOfSuccess.ToString();

                    // *Below code for fill SalesOwner --Start

                    var SalesOwners = CrmStartUp.GetSalesOwnerBySiteId(leadAddViewModel.OfferData.Site.IdCompany);
                    if (!GeosApplication.Instance.SalesOwnerList.Any(x => x.IdSite == leadAddViewModel.OfferData.Site.IdCompany))
                    {
                        foreach (var item in SalesOwners)
                        {
                            SalesOwnerList _salesOwner = new SalesOwnerList();
                            _salesOwner.IdOffer = leadAddViewModel.OfferData.IdOffer;
                            _salesOwner.IdSalesOwner = item.IdPerson;
                            _salesOwner.SalesOwner = item.FullName;
                            if (leadAddViewModel.OfferData.Site != null)
                            {
                                _salesOwner.IdSite = Convert.ToInt32(leadAddViewModel.OfferData.Site.IdCompany);
                                //_salesOwner.IdSite = Convert.ToInt16(leadAddViewModel.OfferData.Site.ConnectPlantId);
                            }
                            //else
                            //{
                            //    _salesOwner.IdSite = 0;
                            //}
                            GeosApplication.Instance.SalesOwnerList.Add(_salesOwner);
                        }
                    }
                    if (leadAddViewModel.OfferData.SalesOwner != null)
                    {
                        dataRow["SalesOwner"] = leadAddViewModel.OfferData.SalesOwner.FullName;
                    }

                    foreach (var item in leadAddViewModel.OfferData.OptionsByOffers)
                    {
                        if (item.OfferOption != null && item.IsSelected && item.Quantity > 0)
                        {
                            dataRow[item.OfferOption.Name] = item.Quantity;
                        }
                    }

                    if (leadAddViewModel.OfferData.CarProject != null)
                        dataRow["Project"] = leadAddViewModel.OfferData.CarProject.Name;
                    if (leadAddViewModel.OfferData.CarOEM != null)
                        dataRow["OEM"] = leadAddViewModel.OfferData.CarOEM.Name;
                    if (leadAddViewModel.OfferData.Source != null)
                        dataRow["Source"] = leadAddViewModel.OfferData.Source.Value;
                    if (leadAddViewModel.OfferData.BusinessUnit != null)
                        dataRow["BusinessUnit"] = leadAddViewModel.OfferData.BusinessUnit.Value;

                    if (leadAddViewModel.OfferData.RFQReception == null)
                    {
                        dataRow["RFQReceptionDate"] = DBNull.Value;
                    }
                    else if (leadAddViewModel.OfferData.RFQReception != null && leadAddViewModel.OfferData.RFQReception == DateTime.MinValue)
                        dataRow["RFQReceptionDate"] = DBNull.Value;
                    else
                        dataRow["RFQReceptionDate"] = leadAddViewModel.OfferData.RFQReception.Value;

                    if (leadAddViewModel.OfferData.Rfq == null)
                    {
                        dataRow["Rfq"] = DBNull.Value;
                    }
                    else
                        dataRow["Rfq"] = leadAddViewModel.OfferData.Rfq;

                    if (leadAddViewModel.OfferData.SendIn == null)
                    {
                        dataRow["QuoteSentDate"] = DBNull.Value;
                    }
                    else if (leadAddViewModel.OfferData.SendIn != null && leadAddViewModel.OfferData.SendIn == DateTime.MinValue)
                        dataRow["QuoteSentDate"] = DBNull.Value;
                    else
                        dataRow["QuoteSentDate"] = leadAddViewModel.OfferData.SendIn.Value;

                   
                    SelectedObject = Dttable.DefaultView[(Dttable.Rows.Count - 1)];  //Dttable.Rows.Count - 1];  // dataRow;
                }

                // code for hide column chooser if empty
                GridControl detailView = (GridControl)obj;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in detailView.Columns)
                {
                    if (column.Visible == false
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                      && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsTimelineColumnChooserVisible = true;
                }
                else
                {
                    IsTimelineColumnChooserVisible = false;
                }
                ((GridControl)obj).Focus();

                GeosApplication.Instance.Logger.Log("Method LeadsNewViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsNewViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;
                if (File.Exists(TimelineGridSettingFilePath))
                {
                    // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(TimelineGridSettingFilePath);
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(TimelineGridSettingFilePath);
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.View.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(TimelineGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
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

                    if (column.Visible == false
                    && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "IsChecked"
                    && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Array"
                    && ((Emdep.Geos.UI.Helper.Column)column.DataContext).Settings.ToString() != "Hidden")
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsTimelineColumnChooserVisible = true;
                }
                else
                {
                    IsTimelineColumnChooserVisible = false;
                }
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();

                //foreach (TileBarFilters item in FilterStatusListOfTile)
                //{
                //    if (item.Id == 0 && !item.Caption.Equals(System.Windows.Application.Current.FindResource("LeadsViewCustomFilter").ToString()) || item.Caption.Equals(System.Windows.Application.Current.FindResource("LeadsViewTileBarCaption").ToString()))
                //    {
                //        gridControl.FilterString = item.FilterCriteria;
                //        item.EntitiesCount = gridControl.VisibleRowCount;
                //    }
                //    gridControl.FilterString = string.Empty;
                //}
                UpdateFilterString();
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                e.Allow = false;
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(TimelineGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsTimelineColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(TimelineGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// 21
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void FillLeadsByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadsByUser ...", category: Category.Info, priority: Priority.Low);
                TimelineGridList = new List<TimelineGrid>();
                //OptionsByOfferLeadsList = new List<OptionsByOffer>();
                //LostReasonsByOffeLeadsList = new List<LostReasonsByOffer>();

                OffersOptionsList offersOptionsLst = new OffersOptionsList();

                #region Roll 22

                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                        PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                        foreach (var itemPlantOwnerUsers in plantOwners)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.ShortName;

                                //==========================================================================================
                                List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
                                TimelineParams objTimelineParams = new TimelineParams();

                                objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                                objTimelineParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                                objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                                //[001] Added 
                                objTimelineParams.activeSite = new ActiveSite {  IdSite= Convert.ToInt32(itemPlantOwnerUsers.ConnectPlantId), SiteAlias = Convert.ToString(itemPlantOwnerUsers.Alias), SiteServiceProvider = itemPlantOwnerUsers.ServiceProviderUrl };
                                objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                                objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                                objTimelineParams.Roles = RoleType.SalesGlobalManager;
                              

                                timelineGridListtemp = GetTimelineGridData(objTimelineParams).ToList();
                                //==========================================================================================
                                //offersOptionsLst = CrmStartUp.GetOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, itemPlantOwnerUsers, GeosApplication.Instance.IdUserPermission);

                                TimelineGridList.AddRange(timelineGridListtemp);
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemPlantOwnerUsers.ShortName);

                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }

                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;

                        if (FailedPlants == null || FailedPlants.Count == 0)
                        {
                            IsShowFailedPlantWarning = false;
                            WarningFailedPlants = string.Empty;
                        }
                    }
                }
                #endregion

                #region Roll 21

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        PreviouslySelectedSalesOwners = salesOwnersIds;

                        foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;

                                //==========================================================================================
                                List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
                                TimelineParams objTimelineParams = new TimelineParams();

                                objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                objTimelineParams.idsSelectedUser = salesOwnersIds;
                                objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                                objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                                //[001] Added 
                                objTimelineParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl };
                                objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                                objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                                objTimelineParams.Roles = RoleType.SalesPlantManager;

                                timelineGridListtemp = GetTimelineGridData(objTimelineParams).ToList();
                                //==========================================================================================
                                TimelineGridList.AddRange(timelineGridListtemp);
                                //offersOptionsLst = CrmStartUp.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.ActiveUser.IdUser);
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                System.Threading.Thread.Sleep(1000);
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;

                        if (FailedPlants == null || FailedPlants.Count == 0)
                        {
                            IsShowFailedPlantWarning = false;
                            WarningFailedPlants = string.Empty;
                        }
                    }
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method FillLeadsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillLeadsByUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// [002][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [002][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        /// <param name="objTimelineParams"></param>
        /// <returns></returns>
        private ObservableCollection<TimelineGrid> GetTimelineGridData(TimelineParams objTimelineParams)
        {
            ObservableCollection<TimelineGrid> timelineGridList = new ObservableCollection<TimelineGrid>();
            //string ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

            // string ServiceUrl = "http://" + ServicePath + "/CrmRestService.svc" + "/GetTimelineGridDetails_V2036";
            // [002] Changed service method and Service endpoint
            string ServiceUrl = "http://" + objTimelineParams.activeSite.SiteServiceProvider + "/CrmRestService.svc" + "/GetTimelineGridDetails_V2040"; //change method

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(TimelineParams));

            json.WriteObject(request.GetRequestStream(), objTimelineParams);

            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(ObservableCollection<TimelineGrid>));

            timelineGridList = (ObservableCollection<TimelineGrid>)jsonResp.ReadObject(stream);

            stream.Flush();
            stream.Close();
            return timelineGridList;
        }


        /// <summary>
        /// 20
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void FillLeads()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeads ...", category: Category.Info, priority: Priority.Low);

                TimelineGridList = new List<TimelineGrid>();

                // Continue loop although some plant is not available and Show error message.

                foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;
                        //==========================================================================================
                        List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
                        TimelineParams objTimelineParams = new TimelineParams();

                        objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                        objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                        objTimelineParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();

                        objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                        //[001] Added 
                        objTimelineParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl };
                        objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                        objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                        objTimelineParams.Roles = RoleType.SalesAssistant;

                        timelineGridListtemp = GetTimelineGridData(objTimelineParams).ToList();
                        //==========================================================================================
                        //offlst = CrmStartUp.GetOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission);

                        TimelineGridList.AddRange(timelineGridListtemp);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(itemCompaniesDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(itemCompaniesDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(itemCompaniesDetails.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method FillLeads() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeads() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeads() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillLeads() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);

                StringBuilder builder = new StringBuilder();
                foreach (GeosStatus item in GeosStatusList)
                {
                    builder.Append("'").Append(item.Name).Append("'" + ",");
                }

                string result = builder.ToString();
                result = result.TrimEnd(',');

                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    string st = string.Format("[OfferCloseDate] >= #{0}# And [OfferCloseDate] < #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
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
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        private void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>() {

                        new Emdep.Geos.UI.Helper.Column() { FieldName="Idoffer",HeaderText="Idoffer", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="ConnectPlantId",HeaderText="ConnectPlantId", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Code",HeaderText="Code", Settings = SettingsType.OfferCode, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() {FieldName="IsChecked",HeaderText="IsChecked", Settings = SettingsType.IsChecked, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Group",HeaderText="Group", Settings = SettingsType.Default, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },

                        new Emdep.Geos.UI.Helper.Column() {FieldName="IdSite",HeaderText="IdSite", Settings = SettingsType.IsChecked, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Plant", Settings = SettingsType.Default, AllowCellMerge=false,Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },

                        //new Emdep.Geos.UI.Helper.Column() { FieldName="Project",HeaderText="Project", Settings = SettingsType.Default, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Project",HeaderText="Project", Settings = SettingsType.Project, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Country",HeaderText="Country", Settings = SettingsType.Default, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Region",HeaderText="Region", Settings = SettingsType.Default, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Description", HeaderText="Description",Settings = SettingsType.Description, AllowCellMerge=false,Width=250,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },

                        new Emdep.Geos.UI.Helper.Column() { FieldName="Status", HeaderText="Status",Settings = SettingsType.Status, AllowCellMerge=false,Width=50,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="HtmlColor", HeaderText="HtmlColor",Settings = SettingsType.Hidden, AllowCellMerge=false,Width=80,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Amount", HeaderText="Amount",Settings = SettingsType.Amount, AllowCellMerge=false,Width=170,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="OfferCloseDate", HeaderText="CloseDate",Settings = SettingsType.CloseDate, AllowCellMerge=false,Width=90,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="OfferStatusid", HeaderText="OfferStatusid",Settings = SettingsType.Hidden, AllowCellMerge=false,Width=90,AllowEditing=true,Visible=false   ,IsVertical= false,FixedWidth=true },


                        new Emdep.Geos.UI.Helper.Column() { FieldName="OfferConfidentialLevel", HeaderText="Confidence Level",Settings = SettingsType.PercentText, AllowCellMerge=false,Width=80,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="SalesOwner",HeaderText="Sales Owner", Settings = SettingsType.SalesOwner, AllowCellMerge=false,Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="IsCloseDateExceed", HeaderText="IsCloseDateExceed",Settings = SettingsType.Hidden, AllowCellMerge=false,Width=80,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName = "IsSaleOwnerNull", HeaderText = "IsSaleOwnerNull", Settings = SettingsType.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },

                        new Emdep.Geos.UI.Helper.Column() { FieldName="OEM",HeaderText="OEM", Settings = SettingsType.OEM, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Source",HeaderText="Source", Settings = SettingsType.Source, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="BusinessUnit",HeaderText="Business Unit", Settings = SettingsType.BusinessUnit, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="RFQReceptionDate", HeaderText="RFQ Reception Date",Settings = SettingsType.RFQDate, AllowCellMerge=false,Width=90,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="QuoteSentDate", HeaderText="Quote Sent Date",Settings = SettingsType.QuoteSentDate, AllowCellMerge=false,Width=90,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Rfq", HeaderText="RFQ",Settings = SettingsType.RFQ, AllowCellMerge=false,Width=90,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Category1",HeaderText="Category1", Settings = SettingsType.Category1, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Category2",HeaderText="Category2", Settings = SettingsType.Category2, AllowCellMerge=false, Width=120,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        //[CRM-M040-04]
                        new Emdep.Geos.UI.Helper.Column() { FieldName="LostCompetitor", HeaderText="Competitor",Settings = SettingsType.OthersFields, AllowCellMerge=false,Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="LostReason", HeaderText="Lost Reason",Settings = SettingsType.OthersFields, AllowCellMerge=false,Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="LostDescription", HeaderText="Lost Description",Settings = SettingsType.OthersFields, AllowCellMerge=false,Width=200,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },
                        //[001] added
                        new Emdep.Geos.UI.Helper.Column() { FieldName="OfferOwner",HeaderText="Offer Owner", Settings = SettingsType.OfferOwner, AllowCellMerge=false, Width=70,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="OfferedTo",HeaderText="Offered To", Settings = SettingsType.OfferedTo, AllowCellMerge=false, Width=70,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="ActiveSite",HeaderText="ActiveSite", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=70,AllowEditing=true,Visible=false,IsVertical= false,FixedWidth=true  },
                 };

                //new Column() { FieldName = "Site", HeaderText = "Site", Settings = SettingsType.Array, AllowCellMerge = true, Width = 50, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true },

                Dttable = new DataTable();

                Dttable.Columns.Add("ConnectPlantId", typeof(Int32));
                Dttable.Columns.Add("Idoffer", typeof(long));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("IsChecked", typeof(bool));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Region", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("IdSite", typeof(Int32));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Site", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("Amount", typeof(double));
                Dttable.Columns.Add("OfferCloseDate", typeof(DateTime));
                Dttable.Columns.Add("OfferConfidentialLevel", typeof(string));
                Dttable.Columns.Add("SalesOwner", typeof(string));
                Dttable.Columns.Add("IsCloseDateExceed", typeof(bool));
                Dttable.Columns.Add("IsSaleOwnerNull", typeof(bool));
                Dttable.Columns.Add("Project", typeof(string));
                Dttable.Columns.Add("OEM", typeof(string));
                Dttable.Columns.Add("Source", typeof(string));
                Dttable.Columns.Add("BusinessUnit", typeof(string));
                Dttable.Columns.Add("RFQReceptionDate", typeof(DateTime));
                Dttable.Columns.Add("QuoteSentDate", typeof(DateTime));
                Dttable.Columns.Add("OfferCloseDateMinValue", typeof(DateTime));
                Dttable.Columns.Add("Rfq", typeof(string));
                Dttable.Columns.Add("SleepDays", typeof(int));
                Dttable.Columns.Add("Category1", typeof(string));
                Dttable.Columns.Add("Category2", typeof(string));
                //[CRM-M040-04]
                Dttable.Columns.Add("LostCompetitor", typeof(string));
                Dttable.Columns.Add("LostReason", typeof(string));
                Dttable.Columns.Add("LostDescription", typeof(string));
                Dttable.Columns.Add("OfferStatusid", typeof(Int32));

                //[001]added
                Dttable.Columns.Add("OfferOwner", typeof(string));
                Dttable.Columns.Add("OfferedTo", typeof(string));

                Dttable.Columns.Add("ActiveSite", typeof(ActiveSite));
                // Total Summary
                TotalSummary = new ObservableCollection<Summary>();

                Templates = new List<Template>();
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total: {0}" });
                OfferOptions = CrmStartUp.GetAllOfferOptions();
                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                    }
                }

                //added site column in last on the grid.
                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "SleepDays", HeaderText = "Sleep(d)", Settings = SettingsType.SleepDays, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true });
                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Site", HeaderText = "Site", Settings = SettingsType.Fixed, AllowCellMerge = false, Width = 60, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true });

                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][20/11/2019][GEOS2-1806]- Add new field "Offer Owner" and "Offered To" in Opportunities
        /// </summary>
        private void FillDataTable()
        {
            GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
            Dttable.Rows.Clear();

            DttableCopy = Dttable.Copy();

            //SalesOwnerList = new ObservableCollection<SalesOwnerList>();
            GeosApplication.Instance.SalesOwnerList = new ObservableCollection<SalesOwnerList>();

            for (int i = 0; i < TimelineGridList.Count; i++)
            {
                var converter = new System.Windows.Media.BrushConverter();
                System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString(TimelineGridList[i].HtmlColor.ToString());
                TimelineGridList[i].HtmlBrushColor = brush;

                DataRow dr = DttableCopy.NewRow();
                dr["IsSaleOwnerNull"] = false;
                dr["SalesOwner"] = string.Empty;
                dr["BusinessUnit"] = string.Empty;
                dr["Source"] = string.Empty;
                dr["Project"] = string.Empty;
                dr["OEM"] = string.Empty;
                dr["Category1"] = string.Empty;
                dr["Category2"] = string.Empty;

                dr["LostCompetitor"] = string.Empty;
                dr["LostReason"] = string.Empty;
                dr["LostDescription"] = string.Empty;

                //[001] added
                dr["OfferOwner"] = string.Empty;
                dr["OfferedTo"] = string.Empty;


                dr["Idoffer"] = TimelineGridList[i].IdOffer;
                dr["IsChecked"] = false;

                dr["ConnectPlantId"] = TimelineGridList[i].ConnectPlantId;
                dr["ActiveSite"] = TimelineGridList[i].ActiveSite;

                dr["IdSite"] = TimelineGridList[i].IdSite;
                dr["Site"] = TimelineGridList[i].ActiveSite.SiteAlias;

                dr["Code"] = TimelineGridList[i].Code.ToString();

                dr["Country"] = TimelineGridList[i].Country.ToString();
                dr["Region"] = TimelineGridList[i].Region.ToString();
                dr["Group"] = TimelineGridList[i].Group.ToString();
                dr["Plant"] = TimelineGridList[i].Plant.ToString();
                dr["Description"] = TimelineGridList[i].Description.ToString();
                dr["Status"] = TimelineGridList[i].Status.ToString();
                dr["HtmlColor"] = TimelineGridList[i].HtmlColor.ToString();
                dr["Amount"] = TimelineGridList[i].Amount;

                // string[] OfferCloseDate = TimelineGridList[i].OfferCloseDate.Split('/');
                //  DateTime dtReturn=new DateTime();
                //  dr["OfferCloseDate"] = DateTime.ParseExact(TimelineGridList[i].OfferCloseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //if( DateTime.TryParse(TimelineGridList[i].OfferCloseDate,out dtReturn))
                // dr["OfferCloseDate"] = dtReturn;
                dr["OfferCloseDate"] = TimelineGridList[i].OfferCloseDate;

                dr["OfferStatusid"] = TimelineGridList[i].OfferStatusid;
                dr["OfferConfidentialLevel"] = TimelineGridList[i].OfferConfidentialLevel.ToString();
                dr["OfferCloseDateMinValue"] = GeosApplication.Instance.ServerDateTime.Date;
                dr["Project"] = TimelineGridList[i].Project;
                dr["OEM"] = TimelineGridList[i].Oem;
                dr["Source"] = TimelineGridList[i].Source;
                dr["BusinessUnit"] = TimelineGridList[i].BusinessUnit;

                //[001]added
                dr["OfferOwner"] = TimelineGridList[i].OfferOwner;
                dr["OfferedTo"] = TimelineGridList[i].OfferedTo;



                if (TimelineGridList[i].Rfq != null)
                    dr["Rfq"] = TimelineGridList[i].Rfq;
                else
                    dr["Rfq"] = DBNull.Value;

                if (TimelineGridList[i].RfqReceptionDate != null)
                {
                    //if (DateTime.TryParse(TimelineGridList[i].RfqReceptionDate.Value, out dtReturn))
                    //{
                    //    if (dtReturn == DateTime.MinValue)
                    //        dr["RFQReceptionDate"] = DBNull.Value;
                    //    else
                    //        dr["RFQReceptionDate"] = dtReturn;
                    //}

                    if (Convert.ToDateTime(TimelineGridList[i].RfqReceptionDate) == DateTime.MinValue)
                        dr["RFQReceptionDate"] = DBNull.Value;
                    else
                        dr["RFQReceptionDate"] = TimelineGridList[i].RfqReceptionDate;
                }
                else
                {
                    dr["RFQReceptionDate"] = DBNull.Value;
                }

                if (TimelineGridList[i].QuoteSentDate != null)
                {
                    //if (DateTime.TryParse(TimelineGridList[i].QuoteSentDate, out dtReturn))
                    //{
                    //    if (dtReturn == DateTime.MinValue)
                    //        dr["QuoteSentDate"] = DBNull.Value;
                    //    else
                    //        dr["QuoteSentDate"] = dtReturn;
                    //}

                    if (Convert.ToDateTime(TimelineGridList[i].QuoteSentDate) == DateTime.MinValue)
                        dr["QuoteSentDate"] = DBNull.Value;
                    else
                        dr["QuoteSentDate"] = TimelineGridList[i].QuoteSentDate;
                }
                else
                {
                    dr["QuoteSentDate"] = DBNull.Value;
                }

                if (TimelineGridList[i].Status != null && TimelineGridList[i].OfferStatusid == 1)
                {
                    if (TimelineGridList[i].LastActivityDate == null)
                    {
                        //if (DateTime.TryParse(TimelineGridList[i].QuoteSentDate, out dtReturn))
                        // TimelineGridList[i].LastActivityDate = dtReturn.ToShortDateString();

                        TimelineGridList[i].LastActivityDate = TimelineGridList[i].QuoteSentDate;
                    }
                }

                if (TimelineGridList[i].LastActivityDate != null)
                {
                    //if (DateTime.TryParse(TimelineGridList[i].LastActivityDate, out dtReturn))
                    //{
                    //    dr["SleepDays"] = Convert.ToDouble((GeosApplication.Instance.ServerDateTime.Date - dtReturn).TotalDays);
                    //}

                    dr["SleepDays"] = Convert.ToDouble((GeosApplication.Instance.ServerDateTime.Date - Convert.ToDateTime(TimelineGridList[i].LastActivityDate).Date).TotalDays);

                }
                else
                {
                    dr["SleepDays"] = DBNull.Value;
                }

                if (!(GeosApplication.Instance.SalesOwnerList.Any(sw => sw.IdSite == Convert.ToInt32(TimelineGridList[i].IdSite))))
                {
                    if (TimelineGridList[i].IdSalesResponsible != null)
                    {
                        SalesOwnerList _salesOwner = new SalesOwnerList();
                        _salesOwner.IdOffer = TimelineGridList[i].IdOffer;
                        _salesOwner.IdSalesOwner = TimelineGridList[i].IdSalesResponsible.Value;
                        _salesOwner.SalesOwner = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == TimelineGridList[i].IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault(); ;
                        _salesOwner.IsSiteResponsibleExist = true;
                        _salesOwner.IdSite = Convert.ToInt32(TimelineGridList[i].IdSite);

                        GeosApplication.Instance.SalesOwnerList.Add(_salesOwner);
                    }

                    if (TimelineGridList[i].IdSalesResponsibleAssemblyBU != null)
                    {
                        SalesOwnerList _salesOwner = new SalesOwnerList();
                        _salesOwner.IdOffer = TimelineGridList[i].IdOffer;
                        _salesOwner.IdSalesOwner = TimelineGridList[i].IdSalesResponsibleAssemblyBU.Value;
                        _salesOwner.SalesOwner = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == TimelineGridList[i].IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault(); ;
                        _salesOwner.IsSiteResponsibleExist = true;
                        _salesOwner.IdSite = Convert.ToInt32(TimelineGridList[i].IdSite);

                        GeosApplication.Instance.SalesOwnerList.Add(_salesOwner);
                    }
                }

                if (TimelineGridList[i].IdSalesOwner != null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == TimelineGridList[i].IdSalesOwner).Select(slt => slt.FullName).FirstOrDefault();//LeadsList[i].SalesOwner.FullName;
                    dr["IsSaleOwnerNull"] = false;
                }
                else if (TimelineGridList[i].IdSalesResponsible != null && TimelineGridList[i].IdSalesResponsibleAssemblyBU != null)
                {
                    dr["SalesOwner"] = "";
                    dr["IsSaleOwnerNull"] = false;
                }
                else if (TimelineGridList[i].IdSalesResponsible != null && TimelineGridList[i].IdSalesResponsibleAssemblyBU == null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == TimelineGridList[i].IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault();
                    dr["IsSaleOwnerNull"] = true;
                }
                else if (TimelineGridList[i].IdSalesResponsible == null && TimelineGridList[i].IdSalesResponsibleAssemblyBU != null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == TimelineGridList[i].IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault();
                    dr["IsSaleOwnerNull"] = true;
                }

                //DateTime dt = new DateTime();
                //string[] datarr = TimelineGridList[i].OfferCloseDate.Split('/');
                //if (DateTime.TryParse(TimelineGridList[i].LastActivityDate, out dtReturn))
                //{

                //    if (dtReturn.Date < GeosApplication.Instance.ServerDateTime.Date)
                //    {
                //        if (TimelineGridList[i].IdSalesStatusType != 5)
                //            dr["IsCloseDateExceed"] = true;
                //        else
                //            dr["IsCloseDateExceed"] = false;
                //    }
                //    else
                //    {
                //        dr["IsCloseDateExceed"] = false;
                //    }
                //}
                //// *fill SalesOwner --End

                if (Convert.ToDateTime(TimelineGridList[i].LastActivityDate).Date < GeosApplication.Instance.ServerDateTime.Date)
                {
                    if (TimelineGridList[i].IdSalesStatusType != 5)
                        dr["IsCloseDateExceed"] = true;
                    else
                        dr["IsCloseDateExceed"] = false;
                }
                else
                {
                    dr["IsCloseDateExceed"] = false;
                }

                dr["LostCompetitor"] = TimelineGridList[i].LostCompetitor;
                dr["LostReason"] = TimelineGridList[i].LostReason;
                dr["LostDescription"] = TimelineGridList[i].LostDescription;

                try
                {
                    foreach (OptionsByOfferGrid item in TimelineGridList[i].OptionsByOffers)
                    {
                        if (item.OfferOption != null)
                        {
                            if (item.IdOption.ToString() == "6" ||
                                 item.IdOption.ToString() == "19" ||
                                 item.IdOption.ToString() == "21" ||
                                 item.IdOption.ToString() == "23" ||
                                 item.IdOption.ToString() == "25" ||
                                 item.IdOption.ToString() == "27")
                            {
                                var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                int indexc = Columns.IndexOf(column);
                                Columns[indexc].Visible = false;
                            }
                            else if (DttableCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                            {
                                var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                int indexc = Columns.IndexOf(column);
                                Columns[indexc].Visible = true;
                                dr[item.OfferOption] = item.Quantity;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (TimelineGridList[i].IdProductCategory > 0)
                {
                    if (TimelineGridList[i].ProductCategory.Category != null)
                        dr["Category1"] = TimelineGridList[i].ProductCategory.Category.Name;

                    if (TimelineGridList[i].ProductCategory.IdParent == 0)
                    {
                        dr["Category1"] = TimelineGridList[i].ProductCategory.Name;
                    }
                    else
                    {
                        dr["Category2"] = TimelineGridList[i].ProductCategory.Name;
                    }

                }

              
                DttableCopy.Rows.Add(dr);
            }
            Dttable = DttableCopy;
            GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void PrintLeadsGrid(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid...", category: Category.Info, priority: Priority.Low);

                //if (MultipleCellEditHelper.IsValueChanged)
                //{
                //    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                //    if (MessageBoxResult == MessageBoxResult.Yes)
                //    {
                //        UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                //    }
                //    MultipleCellEditHelper.IsValueChanged = false;
                //}
                //MultipleCellEditHelper.IsValueChanged = false;

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                GeosApplication.Instance.Logger.Log("PrintLeadsGrid executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintLeadsGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportLeadsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                //if (MultipleCellEditHelper.IsValueChanged)
                //{
                //    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                //    if (MessageBoxResult == MessageBoxResult.Yes)
                //    {
                //        UpdateMultipleRowsCommandAction(MultipleCellEditHelper.Viewtableview);

                //    }
                //    MultipleCellEditHelper.IsValueChanged = false;
                //}
                //MultipleCellEditHelper.IsValueChanged = false;

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Timeline";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (.)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                    TableView _TimelineTableView = ((TableView)obj);

                    _TimelineTableView.ShowTotalSummary = false;
                    _TimelineTableView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Split all merged cells in the worksheet. 
                    foreach (var item in worksheet.Cells.GetMergedRanges())
                    {
                        item.UnMerge();
                    }

                    // Rotate only the OfferOptions.
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;
                    int countTechCol = 0;

                    foreach (GridColumn gridColumn in _TimelineTableView.VisibleColumns)
                    {
                        if (OfferOptions.Any(op => op.Name == gridColumn.FieldName))
                        {
                            string text = worksheetColumns[countTechCol].Heading + 1;

                            Cell cell = worksheet.Cells[text];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            cell.Alignment.RotationAngle = 90;
                        }

                        countTechCol++;
                    }

                    DevExpress.Spreadsheet.CellRange supplierNamesRange = worksheet.Range["A1:AZ1"];
                    supplierNamesRange.Font.Bold = true;
                    control.SaveDocument();

                    IsBusy = false;
                    _TimelineTableView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportLeadsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Status list.
        /// </summary>
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);
                GeosStatusList = new ObservableCollection<GeosStatus>(CrmStartUp.GetGeosOfferStatus().AsEnumerable());
                for (int i = GeosStatusList.Count - 1; i >= 0; i--)
                {
                    if (GeosStatusList[i].IdOfferStatusType == 4)
                    {
                        GeosStatusList.RemoveAt(i);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for resize Grid Row on design.
        /// </summary>
        /// <param name="obj"></param>
        private void ViewHideRangeControl(object obj)
        {
            if (IsViewRangeControl)
            {
                GridRowHeight = "0";
                IsViewRangeControl = false;
            }
            else
            {
                GridRowHeight = "100";
                IsViewRangeControl = true;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Method for fill ConfidentialLevel list.
        /// </summary>
        private void FillConfidentialLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillConfidentialLevelList ...", category: Category.Info, priority: Priority.Low);

                ConfidentialLevelList = new List<int>();
                ConfidentialLevelList.Add(10);
                ConfidentialLevelList.Add(20);
                ConfidentialLevelList.Add(30);
                ConfidentialLevelList.Add(40);
                ConfidentialLevelList.Add(50);
                ConfidentialLevelList.Add(60);
                ConfidentialLevelList.Add(70);
                ConfidentialLevelList.Add(80);
                ConfidentialLevelList.Add(90);
                ConfidentialLevelList.Add(100);

                GeosApplication.Instance.Logger.Log("Method FillConfidentialLevelList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillConfidentialLevelList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Car OEM list.
        /// </summary>
        private void FillCaroemsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCaroemsList ...", category: Category.Info, priority: Priority.Low);

                CaroemsList = CrmStartUp.GetCarOEM();
                CaroemsList.Insert(0, new CarOEM() { Name = "---" });
                GeosApplication.Instance.Logger.Log("Method FillCaroemsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCaroemsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill BusinessUnit list.
        /// </summary>
        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).ToList();

                BusinessUnitList = new List<LookupValue>();
                BusinessUnitList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                BusinessUnitList.AddRange(tempBusinessUnitList);

                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Lead Source list.
        /// </summary>
        private void FillLeadSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList ...", category: Category.Info, priority: Priority.Low);

                LeadSourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(4).AsEnumerable());
                LeadSourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillLeadSourceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadSourceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Car Project list.
        /// </summary>
        private void FillGeosProjectsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGeosProjectsList ...", category: Category.Info, priority: Priority.Low);

                GeosProjectsList = CrmStartUp.GetCarProject(0);
                GeosApplication.Instance.GeosCarProjectsList = GeosProjectsList;

                GeosApplication.Instance.Logger.Log("Method FillGeosProjectsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosProjectsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void TableView_ShownEditor(EditorEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableView_ShownEditor() ...", category: Category.Info, priority: Priority.Low);
                if (obj.Column.FieldName == "SalesOwner")
                {
                    ComboBoxEdit ed = (ComboBoxEdit)obj.Editor;
                    System.Data.DataRowView r = (DataRowView)obj.Row;
                    //ed.ItemsSource = GeosApplication.Instance.SalesOwnerList.Where(x => x.IdSite == Convert.ToInt32(r["IdSite"])).ToList().Distinct();

                    ObservableCollection<SalesOwnerList> tmpSalesOwnerList = new ObservableCollection<SalesOwnerList>(GeosApplication.Instance.SalesOwnerList.Where(x => x.IdSite == Convert.ToInt32(r["IdSite"])).ToList().Distinct());
                    string offerCode = r["Code"].ToString();
                    long offerId = Convert.ToInt64(r["IdOffer"].ToString());
                    Int32 ConnectPlantId = Convert.ToInt32(r["ConnectPlantId"].ToString());
                    ActiveSite offerActiveSite = (ActiveSite)r["ActiveSite"];
                    IList<Offer> TempLeadsList = new List<Offer>();
                    //[001] added Change Method
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                    if (!tmpSalesOwnerList.Any(i => i.IdSalesOwner == TempLeadsList[0].IdSalesOwner))
                    {
                        int index = tmpSalesOwnerList.Count;
                        tmpSalesOwnerList.Add(new SalesOwnerList { IdOffer = offerId, IdSite = TempLeadsList[0].IdCustomer, IdSalesOwner = TempLeadsList[0].SalesOwner.IdPerson, SalesOwner = TempLeadsList[0].SalesOwner.FullName });
                        tmpSalesOwnerList[index].IsSiteResponsibleExist = false;
                    }
                    ed.ItemsSource = tmpSalesOwnerList;
                    GeosApplication.Instance.Logger.Log("Method TableView_ShownEditor() executed successfully", category: Category.Info, priority: Priority.Low);
                    return;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableView_ShownEditor() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillFilterTileBar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar()...", category: Category.Info, priority: Priority.Low);
                FilterStatusListOfTile = new ObservableCollection<TileBarFilters>();
                ObservableCollection<GeosStatus> tempStatusList = new ObservableCollection<GeosStatus>();
                int _id = 1;

                if (Dttable != null)
                {
                    var DttableList = Dttable.AsEnumerable().ToList();
                    List<int> idOfferStatusTypeList = DttableList.Select(x => (int)x.ItemArray[33]).Distinct().ToList();
                    foreach (int item in idOfferStatusTypeList)
                    {
                        GeosStatus status = StatusList.FirstOrDefault(x => x.IdOfferStatusType == item);
                        if (status != null)
                            tempStatusList.Add(status);
                    }

                    FilterStatusListOfTile.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("LeadsViewTileBarCaption").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = Dttable.Rows.Count,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });

                    foreach (var item in tempStatusList)
                    {
                        FilterStatusListOfTile.Add(new TileBarFilters()
                        {
                            Caption = item.Name,
                            Id = _id++,
                            IdOfferStatusType = item.IdOfferStatusType,
                            BackColor = item.HtmlColor,
                            ForeColor = item.HtmlColor,
                            EntitiesCount = (DttableList.Where(x => (int)x.ItemArray[33] == item.IdOfferStatusType).ToList()).Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                FilterStatusListOfTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("LeadsViewCustomFilter").ToString()),
                    Id = _id,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

                if (FilterStatusListOfTile.Count > 0)
                    SelectedTileBarItem = FilterStatusListOfTile[0];

                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillFilterTileBar() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandFilterStatusTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction()...", category: Category.Info, priority: Priority.Low);

                long statusIdType = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).IdOfferStatusType;
                string FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                FilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                if (statusIdType != 0)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (GeosStatus item in StatusList)
                    {
                        if (item.IdOfferStatusType == statusIdType)
                            builder.Append("'").Append(item.Name).Append("'" + ",");
                    }

                    string result = builder.ToString();
                    result = result.TrimEnd(',');

                    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                    {
                        string st = string.Format("[OfferCloseDate] >= #{0}# And [OfferCloseDate] < #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                        st += string.Format(" And [Status] In ( " + result + ")");
                        MyFilterString = st;
                    }
                    else
                    {
                        MyFilterString = string.Format("[Status] In ( " + result + ")");
                    }
                }
                else if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("LeadsViewCustomFilter").ToString()))
                {
                    return;
                }
                else if (FilterString != null && !string.IsNullOrEmpty(FilterString))
                {
                    MyFilterString = FilterString;
                    FilterStringCriteria = FilterString;
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (GeosStatus item in StatusList)
                    {
                        builder.Append("'").Append(item.Name).Append("'" + ",");
                    }

                    string result = builder.ToString();
                    result = result.TrimEnd(',');

                    if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                    {
                        string st = string.Format("[OfferCloseDate] >= #{0}# And [OfferCloseDate] < #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                        st += string.Format(" And [Status] In ( " + result + ")");
                        MyFilterString = st;
                    }
                    else
                    {
                        MyFilterString = string.Format("[Status] In ( " + result + ")");
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterStatusTileClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableViewEx table = (TableViewEx)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
            GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
            ShowFilterEditor(obj);
        }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                FilterEditorView filterEditorView = new FilterEditorView();
                FilterEditorViewModel filterEditorViewModel = new FilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    filterEditorViewModel.FilterName = FilterStringName;
                    filterEditorViewModel.IsSave = true;
                    filterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    filterEditorViewModel.IsNew = true;

                filterEditorViewModel.Init(e.FilterControl, FilterStatusListOfTile);
                filterEditorView.DataContext = filterEditorViewModel;
                EventHandler handle = delegate { filterEditorView.Close(); };
                filterEditorViewModel.RequestClose += handle;
                filterEditorView.Title = titleText;
                filterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                filterEditorView.Grid.Children.Add(e.FilterControl);
                filterEditorView.ShowDialog();

                if (filterEditorViewModel.IsDelete && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave)
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        FilterStatusListOfTile.Remove(tileBarItem);
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
                else if (filterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave && !filterEditorViewModel.IsNew && filterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString() == "Status");
                        GridControl gridControl = ((GridControl)((System.Windows.FrameworkContentElement)(GridColumn)column).Parent);
                        int rowCount = gridControl.VisibleRowCount;
                        FilterStringName = filterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = filterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = rowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = filterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>(tileBarItem.Caption, tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (filterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave && filterEditorViewModel.IsNew && filterEditorViewModel.IsCancel)
                {
                    GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString() == "Status");
                    GridControl gridControl = ((GridControl)((System.Windows.FrameworkContentElement)(GridColumn)column).Parent);
                    int rowCount = gridControl.VisibleRowCount;
                    FilterStatusListOfTile.Add(new TileBarFilters()
                    {
                        Caption = filterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = filterEditorViewModel.FilterCriteria,
                        EntitiesCount = rowCount,
                        Height = 80,
                        width = 200
                    });
                    SelectedTileBarItem = FilterStatusListOfTile.LastOrDefault();
                    string filterName = userSettingsKey + filterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = filterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in GeosStatusList)
                {
                    if (FilterStringName != null)
                    {
                        if (FilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }

                if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("LeadsViewCustomFilter").ToString()) || FilterStringName.Equals(System.Windows.Application.Current.FindResource("LeadsViewTileBarCaption").ToString()))
                {
                    return;
                }

                TableViewEx table = (TableViewEx)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Status"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if(tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        int count = 0;
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                            count = Dttable.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).ToList().Count;

                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        FilterStatusListOfTile.Add(
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

        #endregion // Methods
    }

}
