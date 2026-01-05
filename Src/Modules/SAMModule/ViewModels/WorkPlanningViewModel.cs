using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class WorkPlanningViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
                
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services Region

        #region Declaration

        private ObservableCollection<TileBarFilters> quickFilterList = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        private List<Ots> mainOtsList = new List<Ots>();

        List<OTWorkingTime> listLoggedHoursForOT_User_Date;
        List<PlannedHoursForOT_User_Date> listPlannedHoursForOT_User_Date;

        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private ObservableCollection<BandItem> bands;
        private IList<Template> templates;
        List<OfferOption> offerOptions;
        private DataTable dttable;
        private DataTable dttableCopy;
        TableView view;
        bool isAllSave = false;
        private string filterString;
        private bool isBusy;
        private TileBarFilters selectedFilter;
        private int visibleRowCount;
        private string userSettingsKey = "SAM_WorkPlanningView_";
        private bool isEdit;
        private bool isSave;
        private bool isWorkOrderColumnChooserVisible;
        public string SAM_WorkPlanningView_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SAM_WorkPlanningView_Setting.Xml";
        private object geosAppSettingList;
        private List<TileBarFilters> filterList;

        private List<Ots> mainOtsList_New = new List<Ots>();
        private int totalCount;
        private int visibleChildRowCount;
        private bool isRemarkReadOnly;
        int emptyCellsTotalCount = 0;
        int oldid = 0;
        int emptyCellsTotalCountOld = 0;
        int count = 0;
        double pre = 0;
        double totalAmt = 0;
        #endregion // End Of Declaration

        #region Properties
        // public ObservableCollection<Summary> tempGroupSummary { get; private set; }
        public ObservableCollection<Summary> GroupSummary { get; private set; }

        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
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
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        public virtual string ResultFileName { get; set; }

        public virtual bool DialogResult { get; set; }
        public List<int> ProgressList { get; private set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        
        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }

        public List<Ots> FilterWiseListOfWorkOrder
        {
            get { return filterWiseListOfWorkOrder; }
            set
            {
                filterWiseListOfWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
            }
        }

        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }

        public ObservableCollection<Template> ListOfTemplate
        {
            get { return listOfTemplate; }
            set
            {
                listOfTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("listOfTemplate"));
            }
        }

        public ObservableCollection<TileBarFilters> QuickFilterList
        {
            get { return quickFilterList; }
            set
            {
                quickFilterList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuickFilterList"));
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

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }

        public string CustomFilterStringName { get; set; }

        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }

        public object GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public List<Ots> MainOtsList_New
        {
            get { return mainOtsList_New; }
            set
            {
                mainOtsList_New = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList_New"));
            }
        }
        public int TotalCount
        {
            get
            {
                return totalCount;
            }

            set
            {
                totalCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }
        public int VisibleChildRowCount
        {
            get { return visibleChildRowCount; }
            set
            {
                visibleChildRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleChildRowCount"));
            }
        }


        public bool IsRemarkReadOnly
        {
            get { return isRemarkReadOnly; }
            set
            {
                isRemarkReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemarkReadOnly"));
            }
        }

		//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        List<string> failedPlants;
        List<string> successPlantList; 
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
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


        #endregion //End Of Properties

        #region Icommands
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand UpdateMultipleRowsWorkOrderCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand1 { get; set; }
        public ICommand ShowingEditorCommand { get; set; }
        
        public ICommand WorkOrderViewHyperlinkClickCommand { get; set; }
        #endregion //End Of Icommand

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Constructor

        public WorkPlanningViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkPlanningViewModel....", category: Category.Info, priority: Priority.Low);

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
                               
                UpdateMultipleRowsWorkOrderCommand = new DelegateCommand<object>(UpdateMultipleRowsCommandAction);
                CommandFilterTileClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportWorkOrderList));
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                CustomUnboundColumnDataCommand = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                CustomUnboundColumnDataCommand1 = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                ShowingEditorCommand = new DelegateCommand<object>(ShowingEditorCommandAction);
                WorkOrderViewHyperlinkClickCommand = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                    IsRemarkReadOnly = false;
                else
                    IsRemarkReadOnly = true;
               /* 
                FillListOfColor();  // Called only once for colors
                LoadData();
                FillWorkOrderGridDetails();
                FillWorkOrderFilterList();
                AddCustomSetting();
                FilterString = string.Empty;
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
               */
                GeosApplication.Instance.Logger.Log("Constructor WorkPlanningViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkPlanningViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                    IsRemarkReadOnly = false;
                else
                    IsRemarkReadOnly = true;

                FillListOfColor();  // Called only once for colors
                //LoadData();
                await FillMainOtListAsync();
                FillWorkOrderGridDetails();
                FillWorkOrderFilterList();
                AddCustomSetting();
                FilterString = string.Empty;
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        public void  Init()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
            try
            {
                MainOtsList = new List<Ots>();
                listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                AddDataTableColumnsWithBands();
                FillDataTable();
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        int count = 0;
                        try
                        {
                            string filter = item.Value;
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                            count = Dttable.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).GroupBy(a => a.ItemArray[2].ToString()).ToList().Count;

                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        QuickFilterList.Add(
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

        #endregion //End Of Constructor

        #region Methods

        private void LoadData()
        {
            FillMainOtList();            
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task IsAllSaveAsync(TableView obj)
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
                await FillMainOtListAsync();
                FillDataTable();
                MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(view, false);
                MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SAMWorkPlanningUpdateSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LoadDataAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private async Task RefreshWorkOrderListAsync(object obj)
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
                var tableView = (TableView)obj;
                tableView.BeginInit();
                await FillMainOtListAsync();
                FillDataTable();
                tableView.EndInit();
                FillWorkOrderFilterList();
                FilterString = string.Empty;
                IsBusy = false;

                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LoadDataAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private async Task LoadDataAsync()
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

                await FillMainOtListAsync();
                FillWorkOrderGridDetails();
                FillWorkOrderFilterList();
                FilterString = string.Empty;

                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LoadDataAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void UpdateMultipleRowsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction in SAM Work Order...", category: Category.Info, priority: Priority.Low);

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

                DataRow[] foundRows = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();

                if (foundRows != null)
                {
                    GeosApplication.Instance.Logger.Log($"Updated rows found={foundRows.Length}", category: Category.Info, priority: Priority.Low);
                }

                isAllSave = true;
                var isSave = true;
                foreach (DataRow itemRow in foundRows)
                {
                    foreach (DataColumn itemColumn in Dttable.Columns)
                    {
                        DateTime result;
                        bool isDateColumn = DateTime.TryParse(itemColumn.ColumnName, out result);
                        if (isDateColumn)
                        {
                            // Get Planned Time for current OT, user, date
                            var originalItem = listPlannedHoursForOT_User_Date.FirstOrDefault(
                                x => x.IdOT == Convert.ToInt64(itemRow["IdOT"]) &&
                                x.IdUser == Convert.ToInt64(itemRow["IdUser"]) &&
                                x.PlanningDate.HasValue && x.PlanningDate.Value.Date == result.Date
                                );
                          
                            float? originalItemTimeEstimationInFloat = null;

                            if (originalItem != null)
                            {
                                originalItemTimeEstimationInFloat = originalItem.TimeEstimationInHours;
                            }

                            float? UpdatedItemTimeEstimationInFloat = null;
                            if (itemRow[itemColumn.ColumnName] != DBNull.Value)
                            {
                                UpdatedItemTimeEstimationInFloat = Convert.ToSingle(itemRow[itemColumn.ColumnName]);
                            }

                            if (originalItem == null && UpdatedItemTimeEstimationInFloat.HasValue)
                            {
                                if (originalItemTimeEstimationInFloat.HasValue)
                                {
                                    GeosApplication.Instance.Logger.Log($"Original value found={originalItemTimeEstimationInFloat.Value}", category: Category.Info, priority: Priority.Low);
                                }
                                if (UpdatedItemTimeEstimationInFloat.HasValue)
                                {
                                    GeosApplication.Instance.Logger.Log($"Updated Value found={UpdatedItemTimeEstimationInFloat.Value}", category: Category.Info, priority: Priority.Low);
                                }

                                List<PlannedHoursForOT_User_Date> listOfUpdatedItems = new List<PlannedHoursForOT_User_Date>();
                                var updatedItem = new PlannedHoursForOT_User_Date();

                                updatedItem.IdOT = Convert.ToInt64(itemRow["IdOT"]);
                                updatedItem.IdUser = Convert.ToInt32(itemRow["IdUser"]);
                              
                                updatedItem.PlanningDate = result;
                                
                                if (!UpdatedItemTimeEstimationInFloat.HasValue)
                                {
                                    updatedItem.TimeEstimationInHours = 0;
                                }
                                else
                                {
                                    updatedItem.TimeEstimationInHours = UpdatedItemTimeEstimationInFloat.Value;
                                }

                                updatedItem.TransactionOperation = ModelBase.TransactionOperations.Add;
                                listOfUpdatedItems.Add(updatedItem);

                                var currentOtSite = MainOtsList.FirstOrDefault(x => x.IdOT == updatedItem.IdOT).Site;

                                isSave = SAMService.UpdateOTUserPlanningsFromGrid_V2250(currentOtSite, listOfUpdatedItems);
                                if (!isSave)
                                {
                                    isAllSave = false;
                                    break;
                                }
                            }

                            if (originalItem != null && originalItemTimeEstimationInFloat.HasValue && UpdatedItemTimeEstimationInFloat.HasValue &&
                                originalItemTimeEstimationInFloat.Value != UpdatedItemTimeEstimationInFloat.Value)
                            {
                                if (originalItemTimeEstimationInFloat.HasValue)
                                {
                                    GeosApplication.Instance.Logger.Log($"Original value found={originalItemTimeEstimationInFloat.Value}", category: Category.Info, priority: Priority.Low);
                                }
                                if (UpdatedItemTimeEstimationInFloat.HasValue)
                                {
                                    GeosApplication.Instance.Logger.Log($"Updated Value found={UpdatedItemTimeEstimationInFloat.Value}", category: Category.Info, priority: Priority.Low);
                                }

                                List<PlannedHoursForOT_User_Date> listOfUpdatedItems = new List<PlannedHoursForOT_User_Date>();
                                var updatedItem = (PlannedHoursForOT_User_Date) originalItem.Clone();

                                if (!UpdatedItemTimeEstimationInFloat.HasValue)
                                {
                                    updatedItem.TimeEstimationInHours = 0;
                                }
                                else
                                {
                                    updatedItem.TimeEstimationInHours = UpdatedItemTimeEstimationInFloat.Value;
                                }

                                updatedItem.TransactionOperation = ModelBase.TransactionOperations.Update;
                                listOfUpdatedItems.Add(updatedItem);

                                var currentOtSite = MainOtsList.FirstOrDefault(x => x.IdOT == updatedItem.IdOT).Site;

                                isSave = SAMService.UpdateOTUserPlanningsFromGrid_V2250(currentOtSite, listOfUpdatedItems);
                                if(!isSave)
                                {
                                    isAllSave = false;
                                    break;
                                }
                            }                            
                        }
                    }

                    if (!isSave)
                    {
                        isAllSave = false;
                        break;
                    }
                    
                }

                if (isAllSave)
                {
                    /*
                    LoadData();
                    FillDataTable();
                    MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(view, false);
                    MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SAMWorkPlanningUpdateSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    */
                    IsAllSaveAsync(view);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SAMWorkPlanningUpdateFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() in SAM Work Order executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsCommandAction() Method in SAM Work Order" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);

            view = MultipleCellEditHelperSAMWorkPlanning.Viewtableview;
            if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkPlanning.Viewtableview);
                }

                MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
            }

            MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;

            if (view != null)
            {
                MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(view, false);
            }
            
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            /*
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

            LoadData();
            FillWorkOrderGridDetails();
            FillWorkOrderFilterList();
            FilterString = string.Empty;

            if (QuickFilterList.Count > 0)
                SelectedFilter = QuickFilterList.FirstOrDefault();

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            */
            LoadDataAsync();
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        public void RefreshWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                view = MultipleCellEditHelperSAMWorkPlanning.Viewtableview;
                if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkPlanning.Viewtableview);
                    }

                    MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
                }

                MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;

                if (view != null)
                {
                    MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(view, false);
                }
                /*
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
                
                var tableView = (TableView)obj;
                tableView.BeginInit();
                LoadData();
                
                FillDataTable();
                tableView.EndInit();
                FillWorkOrderFilterList();
                FilterString = string.Empty;
                IsBusy = false;

                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                */
                RefreshWorkOrderListAsync(obj);
                GeosApplication.Instance.Logger.Log("Method RefreshWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync_old()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync...", category: Category.Info, priority: Priority.Low);
                var errors = new List<(int IdCompany, string PlantName, string ErrorMessage)>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    MainOtsList = new List<Ots>();
                    listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                    listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            try
                            {
                                // Execute service call in background thread and capture out parameters
                                var result = await Task.Run(() =>
                                {
                                    List<OTWorkingTime> localLogged;
                                    List<PlannedHoursForOT_User_Date> localPlanned;

                                    var otsList = SAMService.GetAllAssignedWorkordersForPlanning_V2250(plant, out localLogged, out localPlanned);

                                    return (OtsList: otsList, Logged: localLogged, Planned: localPlanned);
                                });

                                // Filter and accumulate results
                                var filtered = result.OtsList.Where(x => !string.IsNullOrEmpty(x.OperatorNames)).ToList();

                                listLoggedHoursForOT_User_Date.AddRange(result.Logged);
                                listPlannedHoursForOT_User_Date.AddRange(result.Planned);
                                MainOtsList.AddRange(filtered);
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                lock (errors)
                                    errors.Add((plant.IdCompany, plant.Alias, ex.Detail.ErrorMessage));

                                GeosApplication.Instance.Logger.Log( $"Plant {plant.IdCompany} failed: {ex.Detail.ErrorMessage}",  category: Category.Warn, priority: Priority.Medium);
                            }
                            catch (Exception ex)
                            {
                                lock (errors)
                                    errors.Add((plant.IdCompany, plant.Alias, ex.Message));

                                GeosApplication.Instance.Logger.Log($"Plant {plant.IdCompany} failed: {ex.Message}", category: Category.Warn, priority: Priority.Medium);
                            }
                        }
                        // ✅ Show errors if any
                        //if (errors.Any())
                        //{
                        //    var errorText = string.Join(Environment.NewLine, errors.Select(e => $"Plant: {e.PlantName} failed: {e.ErrorMessage}"));
                        //    CustomMessageBox.Show(errorText, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //}

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtListAsync() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtListAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtListAsync() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtList_Async()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                    listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();

                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }

                    var semaphore = new SemaphoreSlim(10); // throttle parallelism
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                            List<OTWorkingTime> localListLoggedHoursForOT_User_Date;
                            List<PlannedHoursForOT_User_Date> localListPlannedHoursForOT_User_Date;

                            var samService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            try
                            {
                                var tempMainOtsList = new List<Ots>(SAMService.GetAllAssignedWorkordersForPlanning_V2250(plant,out localListLoggedHoursForOT_User_Date, out localListPlannedHoursForOT_User_Date) );
                                tempMainOtsList = tempMainOtsList.Where(x => !(string.IsNullOrEmpty(x.OperatorNames))).ToList();
                                lock (MainOtsList)
                                {
                                    MainOtsList.AddRange(tempMainOtsList);
                                    listLoggedHoursForOT_User_Date.AddRange(localListLoggedHoursForOT_User_Date);
                                    listPlannedHoursForOT_User_Date.AddRange(localListPlannedHoursForOT_User_Date);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }

                                return tempMainOtsList;
                            }
                            finally
                            {
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx? faultEx.Detail.ErrorMessage: ex.Message;

                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format( (string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                            }

                            GeosApplication.Instance.Logger.Log(
                                $"Error loading OTs for {plant.Alias}: {errorMessage}",
                                Category.Exception,
                                Priority.Low
                            );

                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    var results = await Task.WhenAll(tasks);
                    MainOtsList = results.SelectMany(r => r).ToList();
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Critical error in FillMainOtListAsync: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync_notworking()
        {
            try
            {
                if (DXSplashScreen.IsActive)DXSplashScreen.Close();
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
                }, x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() }, null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;

                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...",Category.Info, Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                    listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    var semaphore = new SemaphoreSlim(10); // throttle concurrency
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;

                            List<OTWorkingTime> localLoggedHours;
                            List<PlannedHoursForOT_User_Date> localPlannedHours;
                            var samService = new SAMServiceController( GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            try
                            {
                                var tempMainOtsList = new List<Ots>(SAMService.GetAllAssignedWorkordersForPlanning_V2250(plant,out localLoggedHours,out localPlannedHours));
                                tempMainOtsList = tempMainOtsList.Where(x => !string.IsNullOrEmpty(x.OperatorNames)).ToList();
                                lock (MainOtsList)
                                {
                                    MainOtsList.AddRange(tempMainOtsList);
                                    listLoggedHoursForOT_User_Date.AddRange(localLoggedHours);
                                    listPlannedHoursForOT_User_Date.AddRange(localPlannedHours);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null)  statusMsg.IsSuccess = 1;
                                }

                                return tempMainOtsList;
                            }
                            finally
                            {
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx ? faultEx.Detail.ErrorMessage: ex.Message;
                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                var statusMsg = GeosApplication.Instance.StatusMessages .FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null)  statusMsg.IsSuccess = 2;
                            }
                            GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}",Category.Exception,Priority.Low);
                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    var results = await Task.WhenAll(tasks);
                    MainOtsList = results.SelectMany(r => r).ToList();
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.",Category.Info,Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Critical error in FillMainOtListAsync: " + ex.ToString(), Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }
		//Shubham[skadam] [V.2.6.7.0] GEOS2-8849&GEOS2-8855&GEOS2-8859 SAM module very slow when trying to load informations - Orders ->Structures 16 09 2025
        private async Task FillMainOtListAsync()
        {
            try
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
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
                },
                x => new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() },
                null, null);

                MainOtsList = new List<Ots>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = string.Empty;

                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync started...", Category.Info, Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                    listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();
                    // Add initial status for all plants
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    var semaphore = new SemaphoreSlim(10); // throttle concurrency
                    var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                            List<OTWorkingTime> localLoggedHours = new List<OTWorkingTime>();
                            List<PlannedHoursForOT_User_Date> localPlannedHours = new List<PlannedHoursForOT_User_Date>();
                            var samService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            try
                            {
                                // Run blocking call on background thread
                                var result = await Task.Run(() =>
                                {
                                    var ots = SAMService.GetAllAssignedWorkordersForPlanning_V2250( plant, out localLoggedHours,out localPlannedHours);
                                    return new{Ots = ots,Logged = localLoggedHours,Planned = localPlannedHours};
                                });

                                var filtered = result.Ots.Where(x => !string.IsNullOrEmpty(x.OperatorNames)).ToList();

                                lock (MainOtsList)
                                {
                                    MainOtsList.AddRange(filtered);
                                    listLoggedHoursForOT_User_Date.AddRange(result.Logged);
                                    listPlannedHoursForOT_User_Date.AddRange(result.Planned);
                                    SuccessPlantList.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }

                                return filtered;
                            }
                            finally
                            {
                                if (samService is ICommunicationObject commObj)
                                {
                                    try { commObj.Close(); }
                                    catch { commObj.Abort(); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex is FaultException<ServiceException> faultEx? faultEx.Detail.ErrorMessage: ex.Message;
                            lock (FailedPlants)
                            {
                                FailedPlants.Add(plant.Alias);
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"),string.Join(",", FailedPlants));
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 2;
                            }
                            GeosApplication.Instance.Logger.Log($"Error loading OTs for {plant.Alias}: {errorMessage}",Category.Exception,Priority.Low);
                            return new List<Ots>();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToArray();

                    var results = await Task.WhenAll(tasks);
                    MainOtsList = results.SelectMany(r => r).ToList();
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method FillMainOtListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Critical error in FillMainOtListAsync: " + ex.ToString(), Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }

        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();
                    listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
                    listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            List<OTWorkingTime> localListLoggedHoursForOT_User_Date;
                            List<PlannedHoursForOT_User_Date> localListPlannedHoursForOT_User_Date;

                            TempMainOtsList = new List<Ots>(SAMService.GetAllAssignedWorkordersForPlanning_V2250(plant,
                                out localListLoggedHoursForOT_User_Date, out localListPlannedHoursForOT_User_Date));
                            TempMainOtsList= TempMainOtsList.Where(x => !(x.OperatorNames == null || x.OperatorNames == string.Empty)).ToList();
                            listLoggedHoursForOT_User_Date.AddRange(localListLoggedHoursForOT_User_Date);
                            listPlannedHoursForOT_User_Date.AddRange(localListPlannedHoursForOT_User_Date);

                            MainOtsList.AddRange(TempMainOtsList);                            
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }
                
                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void FillWorkOrderGridDetails()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x =>{ return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() };}, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading data...";
                GeosApplication.Instance.Logger.Log(" FillWorkOrderGridDetails...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                
                AddDataTableColumnsWithBands();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(" FillWorkOrderGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderGridDetails() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
                Dttable.Rows.Clear();
                DttableCopy = Dttable.Copy();
                MainOtsList_New = new List<Ots>();
                
                    foreach (Ots ot in MainOtsList)
                    {
                    if (ot.UserShortDetails != null)
                    {
                        foreach (var itemUser in ot.UserShortDetails)
                        {
                            // Planned                    
                            DataRow drPlanned = DttableCopy.NewRow();
                            drPlanned["IsChecked"] = false;
                            drPlanned["OtsObject"] = ot;
                            drPlanned["IdOt"] = ot.IdOT.ToString();
                            drPlanned["Code"] = ot.MergeCode.ToString();
                            if (ot.Quotation.Site.ShortName != null)
                            {
                                drPlanned["Customer"] = ot.Quotation.Site.ShortName;
                            }
                            else
                            {
                                drPlanned["Customer"] = ot.Quotation.Site.Customer.CustomerName.ToString() + " - " + ot.Quotation.Site.Name.ToString();
                            }

                            drPlanned["IdUser"] = itemUser.IdUser.ToString();
                            drPlanned["OperatorNames"] = ot.OperatorNames.ToString();
                            drPlanned["User"] = itemUser.UserName.ToString();                            
                            drPlanned["TimeTypePlannedOrLogged"] = "Planned";

                            float totalplannedHours = 0;
                            foreach (DataColumn item in DttableCopy.Columns)
                            {
                                // "dd/MM/yyyy"
                                DateTime result;
                                float resultHours;
                                bool isDateColumn = DateTime.TryParse(item.ColumnName, out result);
                                if (isDateColumn)
                                {
                                    // Get Planned Time for current OT, user, date
                                    var plannedHours = listPlannedHoursForOT_User_Date.FirstOrDefault(
                                        x => x.IdOT == ot.IdOT
                                        &&
                                        x.IdUser == itemUser.IdUser &&
                                        x.PlanningDate.HasValue && x.PlanningDate.Value.Date == result.Date
                                        );
                                    if (plannedHours != null && plannedHours.TimeEstimationInHours.HasValue)
                                    {
                                        //if(plannedHours.TimeEstimationInHours.Contains(","))
                                        //{ }

                                        //if (float.TryParse(plannedHours.TimeEstimationInHours, out resultHours))
                                            totalplannedHours += plannedHours.TimeEstimationInHours.Value;
                                        
                                        drPlanned[item.ColumnName] = plannedHours.TimeEstimationInHours.Value;
                                        
                                        //if (drPlanned[item.ColumnName].ToString().Contains(","))
                                        //{ }
                                    }
                                }
                            }
                            drPlanned["Total"] = totalplannedHours.ToString("#0.00");
                            drPlanned["IsItLoggedTimeRowWithValueMoreThanPlannedTime"] = "-1";
                            drPlanned["IsItLoggedTimeRow"] = "0";

                            if(!IsRemarkReadOnly)
                                drPlanned["IsItEditablePlannedTimeRow"] = "1";
                            else
                                drPlanned["IsItEditablePlannedTimeRow"] = "0";

                            DttableCopy.Rows.Add(drPlanned);
                            MainOtsList_New.Add(ot);

                            // Logged
                            DataRow drLogged = DttableCopy.NewRow();
                            drLogged["IsChecked"] = false;
                            drLogged["IdOt"] = ot.IdOT.ToString();
                            drLogged["Code"] = ot.MergeCode.ToString();
                            if (ot.Quotation.Site.ShortName != null)
                            {
                                drLogged["Customer"] = ot.Quotation.Site.ShortName;
                            }
                            else
                            {
                                drLogged["Customer"] = ot.Quotation.Site.Customer.CustomerName.ToString() + " - " + ot.Quotation.Site.Name.ToString();
                            }

                            drLogged["IdUser"] = itemUser.IdUser.ToString();
                            drLogged["OperatorNames"] = ot.OperatorNames.ToString();
                            drLogged["User"] = itemUser.UserName.ToString();
                            drLogged["TimeTypePlannedOrLogged"] = "Logged";

                            double totalLoggedHours = 0;
                            foreach (DataColumn item in DttableCopy.Columns)
                            {
                                DateTime result;
                                bool isDateColumn = DateTime.TryParse(item.ColumnName, out result);
                                if (isDateColumn)
                                {
                                    // Get Planned Time for current OT, user, date
                                    var loggedHours = listLoggedHoursForOT_User_Date.Where(x => x.StartTime.HasValue
                                        ).FirstOrDefault(
                                        x => x.IdOT == ot.IdOT &&
                                        x.IdOperator == itemUser.IdUser &&
                                        x.StartTime.Value.Date == result.Date
                                        );
                                    if (loggedHours != null)
                                    {
                                        drLogged[item.ColumnName] = Convert.ToSingle(loggedHours.TotalTime.TotalHours);
                                        totalLoggedHours += loggedHours.TotalTime.TotalHours;
                                    }
                                }
                            }
                            drLogged["Total"] = totalLoggedHours.ToString("#0.00");
                            if (totalLoggedHours > totalplannedHours)
                            {
                                drLogged["IsItLoggedTimeRowWithValueMoreThanPlannedTime"] = "1";
                            }
                            else
                            {
                                drLogged["IsItLoggedTimeRowWithValueMoreThanPlannedTime"] = "0";
                            }
                            drLogged["IsItLoggedTimeRow"] = "1";
                            drLogged["IsItEditablePlannedTimeRow"] = "0";
                            DttableCopy.Rows.Add(drLogged);
                            MainOtsList_New.Add(ot);
                        }
                    }
                }
                Dttable = DttableCopy;
                if (GeosApplication.Instance.MainOtsList.Count == 0)
                {
                    GeosApplication.Instance.MainOtsList = MainOtsList_New;
                }


                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            }
        
        private void AddDataTableColumnsWithBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumnsWithBands ...", category: Category.Info, priority: Priority.Low);

                if (GroupSummary == null)
                {
                    GroupSummary = new ObservableCollection<Summary>(); }
                else
                {
                    GroupSummary.Clear();
                }

                if (Bands == null)
                    Bands = new ObservableCollection<BandItem>();
                else
                    Bands.Clear();

                var bandOrder = new BandItem
                {
                    BandName = "Order",
                    BandHeader = "Order",
                    FixedStyle = FixedStyle.Left,
                    OverlayHeaderByChildren = true,
                    Columns = new ObservableCollection<ColumnItem>()
                };
                bandOrder.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    //new Emdep.Geos.UI.Helper.ColumnItem() {ColumnFieldName="IsChecked",HeaderText="IsChecked", Settings = WorkPlanningSettingsType.IsChecked,
                    //    Visible=false,IsVertical= false}
                    //new ColumnItem { ColumnFieldName = "IdOt", HeaderText = "IdOt", Width = 80, IsVertical = false,
                    //                   WorkPlanningSettingsType = WorkPlanningSettingsType.Hidden, Visible = false },
                    new ColumnItem { ColumnFieldName = "Code", HeaderText = "", IsVertical = false,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.OfferCode, Visible = true } });
                Bands.Add(bandOrder);

                var bandCustomer = new BandItem
                {
                    BandName = "Customer",
                    BandHeader = "Customer",
                    FixedStyle = FixedStyle.Left,
                    OverlayHeaderByChildren = true,
                    Columns = new ObservableCollection<ColumnItem>()
                };
                bandCustomer.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    new ColumnItem() { ColumnFieldName = "Customer", HeaderText = "", IsVertical = false,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.Customer, Visible = true } });
                              
                Bands.Add(bandCustomer);
                
                var bandTimeTypePlannedOrLogged = new BandItem
                {
                    BandName = "TimeTypePlannedOrLogged",
                    BandHeader = "",
                    FixedStyle = FixedStyle.Left,
                    OverlayHeaderByChildren = true,
                    Columns = new ObservableCollection<ColumnItem>()
                };
                bandTimeTypePlannedOrLogged.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    new ColumnItem() { ColumnFieldName = "TimeTypePlannedOrLogged", HeaderText = "", IsVertical = false,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.TimeTypePlannedOrLogged, Visible = true } });

                bandTimeTypePlannedOrLogged.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    new ColumnItem() { ColumnFieldName = "User", HeaderText = "", IsVertical = false,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.User, Visible = false, Width=0 } });
               
                Bands.Add(bandTimeTypePlannedOrLogged);
                Dttable = new DataTable();
                Dttable.Columns.Add("IsChecked", typeof(bool));
                Dttable.Columns.Add("IdOt", typeof(long));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("Customer", typeof(string));
                Dttable.Columns.Add("IdUser", typeof(string));
                Dttable.Columns.Add("OperatorNames", typeof(string));
                Dttable.Columns.Add("User", typeof(string));
                Dttable.Columns.Add("TimeTypePlannedOrLogged", typeof(string));
                Dttable.Columns.Add("OtsObject", typeof(Ots));
                
                //Find Min Date in all data
                //Find Max Date in all data
                DateTime? MinDateInAllData = null;
                DateTime? MaxDateInAllData = null;

                foreach (var item in MainOtsList)
                {
                    if (item.ExpectedStartDate != null)
                    {
                        MinDateInAllData = item.ExpectedStartDate.Value;
                        break;
                    }
                }

                foreach (var item in MainOtsList)
                {
                    if (item.ExpectedEndDate != null)
                    {
                        MaxDateInAllData = item.ExpectedEndDate.Value;
                        break;
                    }
                }

                for (int i = 0; i < MainOtsList.Count; i++)
                {
                    if (MainOtsList[i].ExpectedStartDate != null && MinDateInAllData > MainOtsList[i].ExpectedStartDate.Value &&
                        MainOtsList[i].ExpectedStartDate.Value != DateTime.MinValue)
                    {
                        MinDateInAllData = MainOtsList[i].ExpectedStartDate.Value;
                    }

                    if (MainOtsList[i].ExpectedEndDate != null && MaxDateInAllData < MainOtsList[i].ExpectedEndDate.Value &&
                        MainOtsList[i].ExpectedEndDate.Value != DateTime.MinValue)
                    {
                        MaxDateInAllData = MainOtsList[i].ExpectedEndDate.Value;
                    }
                }

                for (int i = 0; i < listPlannedHoursForOT_User_Date.Count; i++)
                {
                    if (listPlannedHoursForOT_User_Date[i].PlanningDate.HasValue)
                    {
                        var currentDate = listPlannedHoursForOT_User_Date[i].PlanningDate.Value;

                        if (MinDateInAllData > currentDate)
                        {
                            MinDateInAllData = currentDate;
                        }

                        if (MaxDateInAllData < currentDate)
                        {
                            MaxDateInAllData = currentDate;
                        }
                    }
                }

                for (int i = 0; i < listLoggedHoursForOT_User_Date.Count; i++)
                {
                    if (listLoggedHoursForOT_User_Date[i].StartTime.HasValue)
                    {
                        var currentDate = listLoggedHoursForOT_User_Date[i].StartTime.Value;

                        if (MinDateInAllData > currentDate)
                        {
                            MinDateInAllData = currentDate;
                        }

                        if (MaxDateInAllData < currentDate)
                        {
                            MaxDateInAllData = currentDate;
                        }
                    }
                }

                if (MinDateInAllData != null && MaxDateInAllData != null)
                {
                    var totalDays = (MaxDateInAllData.Value - MinDateInAllData.Value).TotalDays;
                    GeosApplication.Instance.Logger.Log($"First Date Column='{MinDateInAllData.Value.ToLongDateString()}'," +
                        $"Last Date Column='{MaxDateInAllData.Value.ToLongDateString()}'," +
                        $"TotalDays={totalDays}", category: Category.Info, priority: Priority.Low);

                    if(totalDays>500)
                    {
                        GeosApplication.Instance.Logger.Log($"Note that total days {totalDays} is more than 500. It can slow down application.", category: Category.Warn, priority: Priority.Low);
                    }
                    
                    DateTime currentDate = MinDateInAllData.Value;
                    CultureInfo cul = CultureInfo.CurrentCulture;
                    //var currentDateCalendarWeek = "CW" + cul.Calendar.GetWeekOfYear((DateTime)currentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                    var currentDateCalendarWeek = "CW" + cul.Calendar.GetWeekOfYear((DateTime)currentDate, CalendarWeekRule.FirstDay, cul.DateTimeFormat.FirstDayOfWeek).ToString().PadLeft(2, '0'); //[pmisal][24.04.2023][GEOS2-3738]
                    string lastDateCalendarWeek = currentDateCalendarWeek;

                    var bandcurrentDateCalendarWeek = new BandItem
                    {
                        BandName = currentDateCalendarWeek,
                        BandHeader = currentDateCalendarWeek,
                        FixedStyle = FixedStyle.None,
                        OverlayHeaderByChildren = true,
                        Columns = new ObservableCollection<ColumnItem>()
                    };

                    while (currentDate <= MaxDateInAllData)
                    {
                        //var currentDateToString = currentDate.ToString("dd/MM/yyyy");
                        // var currentDateToString = currentDate.ToString("MM/dd/yyyy"); 
                        var currentDateToString = currentDate.ToString(cul.DateTimeFormat.ShortDatePattern); //[pmisal][24.04.2023][GEOS2-3738]
                        //currentDateCalendarWeek = "CW" + cul.Calendar.GetWeekOfYear((DateTime)currentDate, CalendarWeekRule.FirstFourD-ayWeek, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                        currentDateCalendarWeek = "CW" + cul.Calendar.GetWeekOfYear((DateTime)currentDate, CalendarWeekRule.FirstFourDayWeek,cul.DateTimeFormat.FirstDayOfWeek).ToString().PadLeft(2, '0');  //[pmisal][24.04.2023][GEOS2-3738]
                        if (lastDateCalendarWeek != currentDateCalendarWeek)
                        { // Add Band
                            if (bandcurrentDateCalendarWeek.Columns != null && bandcurrentDateCalendarWeek.Columns.Count > 0)
                                Bands.Add(bandcurrentDateCalendarWeek);
                            else { }

                            bandcurrentDateCalendarWeek = new BandItem
                            {
                                BandName = currentDateCalendarWeek,
                                BandHeader = currentDateCalendarWeek,
                                FixedStyle = FixedStyle.None,
                                OverlayHeaderByChildren = true,
                                Columns = new ObservableCollection<ColumnItem>()
                            };
                        }

                        Dttable.Columns.Add(currentDateToString, typeof(float));
                        bandcurrentDateCalendarWeek.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                            new ColumnItem() { ColumnFieldName = currentDateToString, HeaderText = currentDateToString, IsVertical = true,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.Array, Visible = true } });

                        GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = currentDateToString, DisplayFormat = "n2" });

                        lastDateCalendarWeek = currentDateCalendarWeek;
                        currentDate = currentDate.AddDays(1);
                    }


                    #region // Start of  previous while loop
                    //---------------------------------------------------------------------------------------------------
                    //while (currentDate <= MaxDateInAllData)
                    //{
                    //    var currentDateToString = currentDate.ToString("dd/MM/yyyy");
                    //    currentDateCalendarWeek = "CW" + cul.Calendar.GetWeekOfYear((DateTime)currentDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString().PadLeft(2, '0');

                    //    // Check if the current date is a Saturday or Sunday
                    //    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        // Create a new BandItem for the weekend and add it to the Bands collection
                    //        bandcurrentDateCalendarWeek = new BandItem
                    //        {
                    //            BandName = currentDateCalendarWeek,
                    //            BandHeader = currentDateCalendarWeek,
                    //            FixedStyle = FixedStyle.None,
                    //            OverlayHeaderByChildren = true,
                    //            Columns = new ObservableCollection<ColumnItem>()
                    //        };
                    //        Dttable.Columns.Add(currentDateToString, typeof(float));
                    //        bandcurrentDateCalendarWeek.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    //                new ColumnItem() { ColumnFieldName = currentDateToString, HeaderText = currentDateToString, IsVertical = true,
                    //                           WorkPlanningSettingsType = WorkPlanningSettingsType.Array, Visible = true } });

                    //        GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = currentDateToString, DisplayFormat = "n2" });

                    //        lastDateCalendarWeek = currentDateCalendarWeek;
                    //        currentDate = currentDate.AddDays(1);


                    //    }


                    //}
                    //----------------------------------------------------------------------------------------------

                    #endregion // End previous while loop

                    if (bandcurrentDateCalendarWeek.Columns != null && bandcurrentDateCalendarWeek.Columns.Count > 0)
                        Bands.Add(bandcurrentDateCalendarWeek);
                    //else { }
                }
                GroupSummary.Add(new Summary() { Type = SummaryItemType.None, FieldName = "User", DisplayFormat = "" });
                Dttable.Columns.Add("Total", typeof(string));
                Dttable.Columns.Add("IsItLoggedTimeRowWithValueMoreThanPlannedTime", typeof(string));
                Dttable.Columns.Add("IsItLoggedTimeRow", typeof(string));
                Dttable.Columns.Add("IsItEditablePlannedTimeRow", typeof(string));

                var bandTotal = new BandItem
                {
                    BandName = "Total",
                    BandHeader = "Total",
                    FixedStyle = FixedStyle.Right,
                    OverlayHeaderByChildren = true,
                    Columns = new ObservableCollection<ColumnItem>()
                };
                bandTotal.Columns.AddRange(new ObservableCollection<ColumnItem>() {
                    new ColumnItem() { ColumnFieldName = "Total", HeaderText = "", IsVertical = false,
                                       WorkPlanningSettingsType = WorkPlanningSettingsType.Total, Visible = true } });
                Bands.Add(bandTotal);
                
                GroupSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Total", DisplayFormat = "n2" });

                GeosApplication.Instance.Logger.Log("Method AddDataTableColumnsWithBands executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumnsWithBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumnsWithBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumnsWithBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        } 

        public void FillWorkOrderFilterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading filter...";
                ListOfTemplate = new ObservableCollection<Template>();
                FillWorkOrderFilterTiles(ListOfTemplate, MainOtsList);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][19-04-2022][GEOS2-3469]
        public void FillWorkOrderFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);

                QuickFilterList = new ObservableCollection<TileBarFilters>();

                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWO").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(),
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                foreach (Ots ot in MainListWorkOrder)
                {
                    if (ot.UserShortDetails != null && ot.UserShortDetails.Count > 0)
                    {
                        foreach (UserShortDetail user in ot.UserShortDetails)
                        {

                            ImageSource userImage = null;
                            if (user.UserImageInBytes == null)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                   
                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png"));

                                    else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png"));
                                }
                                else
                                {

                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png"));
                                        else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png"));
                                }

                            }
                            else
                            {
                                userImage = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes);
                            }

                            if (!QuickFilterList.Any(x => x.Id == user.IdUser))
                            {
                                QuickFilterList.Add(new TileBarFilters()
                                {
                                    Caption = user.UserName,
                                    Id = user.IdUser,
                                    Type = user.UserName,
                                    EntitiesCount = MainListWorkOrder.Count(x => !string.IsNullOrEmpty(x.OperatorNames) && x.OperatorNames.Split(',').ToList().Contains(user.UserName)),
                                    Image = userImage,
                                    EntitiesCountVisibility = Visibility.Visible,
                                    Height = 80,
                                    width = 200
                                });
                            }
                        }
                    }
                }
                //[001]
                //QuickFilterList.Add(new TileBarFilters()
                //{
                //    Caption = string.Format(System.Windows.Application.Current.FindResource("UnassignedWO").ToString()),
                //    Id = 0,
                //    EntitiesCount = MainListWorkOrder.Count(x => string.IsNullOrEmpty(x.OperatorNames)),
                //    EntitiesCountVisibility = Visibility.Visible,
                //    Height = 80,
                //    width = 200
                //});

                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });
                filterList = new List<TileBarFilters>();
                filterList = QuickFilterList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void ShowSelectedFilterGridAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (QuickFilterList.Count > 0)
                {
                    System.Windows.Controls.SelectionChangedEventArgs obj = (System.Windows.Controls.SelectionChangedEventArgs)e;

                    string Template = null;
                    string _FilterString = null;

                    if (obj.AddedItems.Count > 0)
                    {
                        Template = ((TileBarFilters)(obj.AddedItems)[0]).Type;
                        _FilterString = ((TileBarFilters)(obj.AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)(obj.AddedItems)[0]).Caption;
                    }

                    if (CustomFilterStringName != null && CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    //if (CustomFilterStringName == "Unassigned")
                    //{
                    //    FilterString = "IsNullOrEmpty([User])";
                    //    return;
                    //}

                    if (Template == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            FilterString = _FilterString;
                        else
                            FilterString = string.Empty;
                    }
                    else
                    {
                        //FilterString = "[OperatorNames] In ('" + Template + "')";
                        FilterString = "Contains([User], '" + Template + "')";
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

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
                
                TableView detailView = (TableView)obj;
                
                long OtId= Convert.ToInt64(((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[1].ToString());
                Ots FoundRow = MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault();
                //[001]
                if (FoundRow.Quotation.IdDetectionsTemplate == 24)
                {
                    WorkOrderItemDetailsForStructureViewModel workOrderItemDetailsForStructureViewModel = new WorkOrderItemDetailsForStructureViewModel();
                    WorkOrderItemDetailsForStructureView workOrderItemDetailsForStructureView = new WorkOrderItemDetailsForStructureView();

                    EventHandler handle1 = delegate { workOrderItemDetailsForStructureView.Close(); };
                    workOrderItemDetailsForStructureViewModel.RequestClose += handle1;
                    workOrderItemDetailsForStructureViewModel.Init(FoundRow);
                    workOrderItemDetailsForStructureView.DataContext = workOrderItemDetailsForStructureViewModel;
                    workOrderItemDetailsForStructureView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                }
                else
                {
                    WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                    WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                    EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                    workOrderItemDetailsViewModel.RequestClose += handle;
                    workOrderItemDetailsViewModel.Init(FoundRow);
                    workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    if (workOrderItemDetailsViewModel.WorkflowStatus != null)
                    {
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList_New.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsViewModel.Remark;
                    FillDataTable();
                }
                
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.DataAware;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                //pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintHeaderTemplate"];
                //pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Planning";
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    gridControl = (activityTableView).Grid;
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
                    DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.DataAware;
                    options.CustomizeCell += Settings_CustomizeCell;
                    options.CustomizeCell += Settings_CustomizeDateCell;
                    //options.CustomizeCell += Options_CustomizeCell;
                    options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    pre = 0;
                    totalAmt = 0;
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        GridControl gridControl;
        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        private void Settings_CustomizeCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                ShrinkToFit = true,
                WrapText = true,
            };
            #region Total

          
            if (e.ColumnFieldName == "Total" && e.AreaType == DevExpress.Export.SheetAreaType.DataArea)
            {
                
                if (e.RowHandle == 0)
                {
                   
                }
                else
                {
                    var idValue = gridControl.GetCellValue(e.RowHandle, "Total");
                    var gridControlidValue1 = gridControl.GetCellValue(e.RowHandle - 1, "Total");
                    double currentCellValue;
                    if (e.Value != null)
                    {
                        if (double.TryParse(e.Value.ToString(), out currentCellValue))
                        {
                            if (e.Value != null && double.TryParse(e.Value.ToString(), out currentCellValue))
                            {
                                if (e.RowHandle%2!=0)
                                {
                                    if (Convert.ToDouble(gridControlidValue1) >= Convert.ToDouble(e.Value))
                                    {

                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Green);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //e.Formatting.BackColor = System.Drawing.Color.Green;
                                    }
                                    else
                                    {
                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Red);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //e.Formatting.BackColor = System.Drawing.Color.Red;
                                    }
                                }
                              
                            }

                        }

                    }

                }
                #endregion
                //if (Convert.ToDouble(idValue) == Convert.ToDouble(e.Value))
                //{
                //    e.Formatting.BackColor = System.Drawing.Color.Red;
                //    e.Handled = true;
                //}
                e.Handled = true;
            }
        }
        private void Settings_CustomizeDateCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                ShrinkToFit = true,
                WrapText = true,
            };

            DateTime temp;
            if (e.Value != null && !DateTime.TryParse(e.Value.ToString(), out temp))
            {
                //e.Formatting.Alignment = new XlCellAlignment()
                //{
                //    WrapText = true,
                //    TextRotation = 90,
                //};
            }
            double cellValueRound;
            if (e.Value != null && double.TryParse(e.Value.ToString(), out cellValueRound))
            {
                cellValueRound = Math.Round(cellValueRound, 2);
                e.Value = Math.Round(cellValueRound, 2);
            }
            if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out temp))
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    cellValue = Math.Round(cellValue, 2);
                    e.Value = Math.Round(cellValue, 2);
                }
                else
                {
                    string htmlColor = "DarkGray";
                    bool isWeekend = temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday;
                    if (isWeekend)
                    {
                        //List<GeosAppSetting> GeosAppSettingListStatic = GeosAppSettingListStatic = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17,67,68");
                        //var setting = GeosAppSettingListStatic.FirstOrDefault(x => x.IdAppSetting == 68)?.DefaultValue;

                        //var themewiseSetting = setting.Split(',');
                        //if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        //{
                        //    htmlColor = themewiseSetting[0].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                        //}
                        //else
                        //{
                        //    htmlColor = themewiseSetting[1].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                        //}
                        //ColorizeCell(e.ColumnFieldName, e.RowHandle, e.Formatting,System.Drawing.Color.DarkGray);
                        ColorizeCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.FromName(htmlColor));
                    }

                    e.Formatting.Alignment = new XlCellAlignment()
                    {
                        //WrapText = true,
                        TextRotation = 90,
                        ShrinkToFit = true,
                        HorizontalAlignment = XlHorizontalAlignment.Left
                    };
                }

            }
            e.Handled = true;

        }
        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                ShrinkToFit = true,
                 WrapText = true,               
            };
            count = count + 1;
            if (e.ColumnFieldName != null && e.ColumnFieldName == "Total")
            {

                double currentCellValue;
                if (e.Value != null)
                {
                    if (double.TryParse(e.Value.ToString(), out currentCellValue))
                    {
                        if (totalAmt!= Convert.ToDouble(e.Value))
                        {




                            if (pre == 0)
                            {

                                if (e.Value != null && double.TryParse(e.Value.ToString(), out currentCellValue))
                                {
                                    pre = Math.Round(currentCellValue, 2);
                                    totalAmt = totalAmt + pre;
                                    totalAmt = Math.Round(totalAmt, 2);
                                }
                            }
                            else if (pre != 0)
                            {
                                if (e.Value != null && double.TryParse(e.Value.ToString(), out currentCellValue))
                                {
                                    if (pre >= Convert.ToDouble(e.Value))
                                    {

                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Green);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //totalAmt= Math.Round(totalAmt, 2);
                                    }
                                    else
                                    {
                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Red);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //totalAmt = Math.Round(totalAmt, 2);
                                    }
                                }
                            }


                        }
                        else if(totalAmt==0)
                        {
                            if (pre == 0)
                            {

                                if (e.Value != null && double.TryParse(e.Value.ToString(), out currentCellValue))
                                {
                                    pre = Math.Round(currentCellValue, 2);
                                    totalAmt = totalAmt + pre;
                                    //totalAmt = Math.Round(totalAmt, 2);
                                }
                            }
                            else if (pre != 0)
                            {
                                if (e.Value != null && double.TryParse(e.Value.ToString(), out currentCellValue))
                                {
                                    if (pre >= Convert.ToDouble(e.Value))
                                    {

                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Green);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //totalAmt = Math.Round(totalAmt, 2);
                                    }
                                    else
                                    {
                                        ColorizeTotalCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.Red);
                                        pre = 0;
                                        totalAmt = totalAmt + Math.Round(currentCellValue, 2);
                                        //totalAmt = Math.Round(totalAmt, 2);
                                    }
                                }
                            }
                        }
                        else if (totalAmt == Convert.ToDouble(e.Value))
                        {
                            totalAmt = 0;
                        }
                        else if (Math.Round(totalAmt,2) == Convert.ToDouble(e.Value))
                        {
                            totalAmt = 0;
                        }
                    }
                }
               
            }

            DateTime temp;
            if (e.Value!=null && !DateTime.TryParse(e.Value.ToString(), out temp))
            {
                //e.Formatting.Alignment = new XlCellAlignment()
                //{
                //    WrapText = true,
                //    TextRotation = 90,
                //};
            }

            if (e.ColumnFieldName != null && e.ColumnFieldName == "Total")
            {
                //var Type = ((System.Data.DataRowView)e.Value);
                //DataRowView dr = (DataRowView)Type;
            }

            double cellValueRound;
            if (e.Value != null && double.TryParse(e.Value.ToString(), out cellValueRound))
            {
                cellValueRound = Math.Round(cellValueRound, 2);
                e.Value = Math.Round(cellValueRound, 2);
            }
            if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out temp))
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    cellValue=Math.Round(cellValue, 2);
                    e.Value = Math.Round(cellValue, 2);
                }
                else
                {
                    string htmlColor = "DarkGray";
                    bool isWeekend = temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday;
                    if (isWeekend)
                    {
                        //List<GeosAppSetting> GeosAppSettingListStatic = GeosAppSettingListStatic = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17,67,68");
                        //var setting = GeosAppSettingListStatic.FirstOrDefault(x => x.IdAppSetting == 68)?.DefaultValue;

                        //var themewiseSetting = setting.Split(',');
                        //if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        //{
                        //    htmlColor = themewiseSetting[0].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                        //}
                        //else
                        //{
                        //    htmlColor = themewiseSetting[1].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                        //}
                        //ColorizeCell(e.ColumnFieldName, e.RowHandle, e.Formatting,System.Drawing.Color.DarkGray);
                        ColorizeCell(e.ColumnFieldName, e.RowHandle, e.Formatting, System.Drawing.Color.FromName(htmlColor));
                    }
                     
                    e.Formatting.Alignment = new XlCellAlignment()
                    {
                        //WrapText = true,
                        TextRotation = 90,
                        ShrinkToFit = true,
                        HorizontalAlignment = XlHorizontalAlignment.Left
                    };
                }
               
            }          
            e.Handled = true;
        }
        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        private void SetBackgroundColor(CustomizeCellEventArgs e, string colorFieldName)
        {
            if (e.DataSourceRowIndex < 0) return;


           var temp= Dttable.Rows[e.DataSourceRowIndex];
            var temp2=temp.ItemArray[1];   
            e.Handled = true;
        }

        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        {
            //if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
            //    e.DocumentColumn.WidthInPixels = 90;
        }
        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        //  private void ColorizeCell(string fieldName, int rowHandle, object appearanceObject, System.Drawing.Color Color)
        private void ColorizeCell(string fieldName, int rowHandle, object appearanceObject, System.Drawing.Color Color)
        {
            if (!string.IsNullOrWhiteSpace(fieldName) && rowHandle >= 0)
            {
                DevExpress.Utils.AppearanceObject app = appearanceObject as DevExpress.Utils.AppearanceObject;
                if (app != null)
                    app.BackColor = Color;
                else
                {
                    XlFormattingObject obj = appearanceObject as XlFormattingObject;
                    if (obj != null)
                        obj.BackColor = Color;
                }
            }
        }
        //shubham[skadam] GEOS2-3468 SAM Calendar Planner View - 4
        private void ColorizeTotalCell(string fieldName, int rowHandle, object appearanceObject, System.Drawing.Color Color)
        {
            if (!string.IsNullOrWhiteSpace(fieldName) && rowHandle >= 0)
            {
                XlFormattingObject obj = appearanceObject as XlFormattingObject;
                if (obj != null)
                    obj.Font.Color = Color;
            }
        }
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
         
            GridControl gridControl = ((DevExpress.Xpf.Grid.TableView)obj.OriginalSource).Grid;
            ShowFilterEditor(obj);

            }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
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

                customFilterEditorViewModel.Init(e.FilterControl, QuickFilterList);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        QuickFilterList.Remove(tileBarItem);
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
                    string filter = customFilterEditorViewModel.FilterCriteria;
                    CriteriaOperator op = CriteriaOperator.Parse(filter);
                    int rowcount = Dttable.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).GroupBy(a => a.ItemArray[2].ToString()).ToList().Count;
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = rowcount;
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
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    string filter = customFilterEditorViewModel.FilterCriteria;
                    CriteriaOperator op = CriteriaOperator.Parse(filter);
                    int rowcount = Dttable.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).GroupBy(a => a.ItemArray[2].ToString()).ToList().Count;

                    QuickFilterList.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = rowcount
                    });

                    string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = QuickFilterList.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);

                foreach(var item in filterList)
                {
                    if(CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Caption))
                            return;
                    }
                }
                TableView table = (TableView)obj;
                GridControl gridControl = (GridControl)(table).DataControl;
                List<GridColumn> GridColumnList = gridControl.Columns.Where(x => x.FieldName != null).ToList();
                string columnName = FilterString.Substring(FilterString.IndexOf("[") + 1, FilterString.IndexOf("]") - 2 - FilterString.IndexOf("[") + 1);
                GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals(columnName));
                IsEdit = true;
                table.ShowFilterEditor(column);

                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowingEditorCommandAction(object e)
        {
           
        }
              
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
               GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
             //   var template = (((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid).GroupSummaryGeneratorTemplate;
                int visibleFalseCoulumn = 0;

                if (File.Exists(SAM_WorkPlanningView_SettingFilePath))
                {
                  //  ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(SAM_WorkPlanningView_SettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView StandardTimeTableView = (TableView)GridControlSTDetails.View;
                    if (StandardTimeTableView.SearchString != null)
                    {
                        StandardTimeTableView.SearchString = null;
                    }
                    // GroupSummary = GroupSummary ;
                    //  GridControlSTDetails.GroupSummaryGeneratorTemplate = template;
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(SAM_WorkPlanningView_SettingFilePath);

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

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }
                                
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
		//[nsatpute][07-02-2025][GEOS2-6921]
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                var frameworkElement = sender as System.Windows.FrameworkContentElement;

                if (frameworkElement != null)
                {
                    var parent = frameworkElement.Parent;

                    while (parent != null && !(parent is GridControl))
                    {
                        parent = (parent as System.Windows.FrameworkContentElement)?.Parent;
                    }

                    if (parent is GridControl gridControl)
                    {
                        gridControl.SaveLayoutToXml(SAM_WorkPlanningView_SettingFilePath);
                    }
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(SAM_WorkPlanningView_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                GridControl gridControlInstance = ((WorkPlanningView)obj.OriginalSource).grid; // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControlInstance.View;

                tableView.SearchString = string.Empty;
                if (gridControlInstance.GroupCount > 0)
                    gridControlInstance.ClearGrouping();
                gridControlInstance.ClearSorting();
                gridControlInstance.FilterString = null;
                gridControlInstance.SaveLayoutToXml(SAM_WorkPlanningView_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17,67,68");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CustomUnboundColumnDataAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomUnboundColumnDataAction...", category: Category.Info, priority: Priority.Low);

                CellValueChangedEventArgs obj = (CellValueChangedEventArgs)e;
                DataRowView dr = (DataRowView)obj.Row;
                double totaHours = 0;
                foreach (DataColumn item in Dttable.Columns)
                {
                    // "dd/MM/yyyy"
                    DateTime result;
                    bool isDateColumn = DateTime.TryParse(item.ColumnName, out result);
                    if (isDateColumn)
                    {
                        if (obj.Column.Header.ToString() == item.ColumnName)
                        {
                            if (obj.Value != null)
                            {
                                var currentHoursString = obj.Value.ToString(); // This for currently updated row
                                if (!string.IsNullOrEmpty(currentHoursString))
                                {
                                    double currentHoursdouble;
                                    bool isValidValue = Double.TryParse(currentHoursString, out currentHoursdouble);
                                    if (isValidValue) totaHours += currentHoursdouble;
                                }
                            }
                        }
                        else
                        {
                            var currentHoursString = dr.Row[item.ColumnName].ToString();
                            if (!string.IsNullOrEmpty(currentHoursString))
                            {
                                double currentHoursdouble;
                                bool isValidValue = Double.TryParse(currentHoursString, out currentHoursdouble);
                                if (isValidValue) totaHours += currentHoursdouble;
                            }
                        }
                    }
                }

                dr["Total"] = totaHours.ToString("#0.00");

                Ots currentOT = null;
                int? currentIdUser = null;
                if (dr.Row["OtsObject"] != null) currentOT = (Ots)dr.Row["OtsObject"];

                if(dr.Row["IdUser"] != null) currentIdUser = Convert.ToInt32(dr.Row["IdUser"]);

                if (currentOT != null && currentIdUser != null)
                {
                    var totalLoggedHours = listLoggedHoursForOT_User_Date.Where(x => x.StartTime.HasValue && x.EndTime.HasValue &&
                        x.IdOT == currentOT.IdOT && x.IdOperator == currentIdUser).Sum(x => x.TotalTime.TotalHours);

                    foreach (DataRow itemRow in Dttable.Rows)
                    {
                        if (itemRow["idOT"].ToString() == currentOT.IdOT.ToString() &&
                            itemRow["IdUser"].ToString() == currentIdUser.ToString() &&
                            itemRow["TimeTypePlannedOrLogged"].ToString() == "Logged")
                        {
                            if (totalLoggedHours > totaHours)
                            {
                                itemRow["IsItLoggedTimeRowWithValueMoreThanPlannedTime"] = "1";
                            }
                            else
                            {
                                itemRow["IsItLoggedTimeRowWithValueMoreThanPlannedTime"] = "0";
                            }
                            break;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CustomUnboundColumnDataAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomUnboundColumnDataAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            Dttable = null;
            DttableCopy = null;
        }

        #endregion //End Of Methods
    }


}
