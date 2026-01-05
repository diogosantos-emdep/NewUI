using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.TSM;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.TSM.CommonClass;
using Emdep.Geos.Modules.TSM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.Expando;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.TSM.ViewModels
{
    //[GEOS2-8981][pallavi.kale][28.11.2025]
    public class WorklogReportViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ITSMService TSMService = new TSMServiceController("localhost:6699");
        #endregion

        #region Declaration
        public string WorklogReportGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WorklogReportGridSettingFilePath.Xml";
        private bool isWorklogReportColumnChooserVisible;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private bool isBusy;
        private string myFilterString;
        private Duration currentDuration;
        private int isButtonStatus;
        private Visibility isCalendarVisible;
        private DateTime startDate;
        private DateTime endDate;
        private ObservableCollection<TSMWorkLogReport> workLogReportGridList;
        private TSMWorkLogReport selectedWorkLogReport;
        private EventHandler _flyoutClosedHandler;
        const string shortDateFormat = "dd/MM/yyyy";
        private string totalTimeINHoursAndMinutes;
        private ObservableCollection<TileBarFilters> quickFilterList = new ObservableCollection<TileBarFilters>();
        private string customFilterHTMLColor;
        private TileBarFilters selectedTileBarItem;
        private List<GridColumn> gridColumnList;
        private bool isEdit;
        private string userSettingsKeyForWorklogReport = "TSM_Worklog_Filter_";
        private int visibleRowCount;
        private TileBarFilters previousSelectedTopTileBarItem;
        private TileBarFilters selectedFilter;
        private string filterString;
        private List<TileBarFilters> filterList;
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();
        #endregion // End Of Declaration

        #region Properties
        public bool IsWorklogReportColumnChooserVisible
        {
            get { return isWorklogReportColumnChooserVisible; }
            set
            {
                isWorklogReportColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorklogReportColumnChooserVisible"));
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
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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
        public ObservableCollection<TSMWorkLogReport> WorkLogReportGridList
        {
            get
            {
                return workLogReportGridList;
            }
            set
            {
                workLogReportGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogReportGridList"));
            }
        }
        public TSMWorkLogReport SelectedWorkLogReport
        {
            get
            {
                return selectedWorkLogReport;
            }
            set
            {
                selectedWorkLogReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkLogReport"));
            }
        }
        public bool IsUpdate { get; set; }
        public string TotalTimeInHoursAndMinutes
        {
            get { return totalTimeINHoursAndMinutes; }
            set
            {
                totalTimeINHoursAndMinutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeInHoursAndMinutes"));
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
        public string CustomFilterStringName { get; set; }
        public string CustomFilterHTMLColor
        {
            get { return customFilterHTMLColor; }
            set
            {
                customFilterHTMLColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterHTMLColor"));
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
        public List<GridColumn> GridColumnList
        {
            get { return gridColumnList; }
            set
            {
                gridColumnList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridColumnList"));
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
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }
        public TileBarFilters PreviousSelectedTopTileBarItem
        {
            get { return previousSelectedTopTileBarItem; }
            set
            {
                previousSelectedTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTopTileBarItem"));
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
        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }
        public List<TileBarFilters> FilterList
        {
            get { return filterList; }
            set
            {
                filterList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterList"));
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
        #endregion //End Of Properties

        #region Icommands
        public ICommand PlantOwnerPopupClosed { get; set; }
        public ICommand WorklogReportGridControlLoadedCommand { get; set; }
        public ICommand WorklogReportTableViewLoadedCommand { get; set; }
        public ICommand WorklogReportGridControlUnloadedCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand OTCodeHyperlinkClickCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
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
        public WorklogReportViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorklogReportViewModel....", category: Category.Info, priority: Priority.Low);
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
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                WorklogReportGridControlLoadedCommand = new RelayCommand(new Action<object>(WorklogReportGridControlLoadedCommandAction));
                WorklogReportTableViewLoadedCommand = new RelayCommand(new Action<object>(WorklogReportItemListTableViewLoadedCommandAction));
                WorklogReportGridControlUnloadedCommand = new RelayCommand(new Action<object>(WorklogReportGridControlUnloadedCommandAction));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
                PeriodCommand = new RelayCommand(new Action<object>(PeriodCommandAction));
                PeriodCustomRangeCommand = new RelayCommand(new Action<object>(PeriodCustomRangeCommandAction));
                ApplyCommand = new RelayCommand(new Action<object>(ApplyCommandAction));
                CancelCommand = new RelayCommand(new Action<object>(CancelCommandAction));
                OTCodeHyperlinkClickCommand = new RelayCommand(new Action<object>(OTCodeHyperlinkClickCommandAction));
                CommandShowFilterPopupClick = new DelegateCommand<object>(ShowSelectedFilterGridAction); 
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction); 
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction); 
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                DateTime baseDate = DateTime.Today;
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                TSMCommon.Instance.FromDate = thisMonthStart.ToString(shortDateFormat);
                TSMCommon.Instance.ToDate = thisMonthEnd.ToString(shortDateFormat);
                //FillWorkLogReportList();
                //FillWorkLogReportFilterList();
                //AddCustomSetting();
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor WorklogReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorklogReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion //End Of Constructor

        #region Methods
        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);

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
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
           FillWorkLogReportList();
           SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
            //FillWorkLogReportFilterList();
            //AddCustomSetting();
            FilterString = string.Empty;
            if (QuickFilterList.Count > 0)
                SelectedFilter = QuickFilterList.FirstOrDefault();
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = " Worklog Report List";
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
                    TableView worklogReportTableView = ((TableView)obj);
                    worklogReportTableView.ShowTotalSummary = false;
                    worklogReportTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    worklogReportTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    worklogReportTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    (worklogReportTableView).DataControl.RefreshData();
                    (worklogReportTableView).DataControl.UpdateLayout();
                    GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView worklogReportTableView = (TableView)obj;
                GridControl gridControl = (worklogReportTableView).Grid;
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

                //FillWorkLogReportList();
                SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                worklogReportTableView.SearchString = null;
                MyFilterString = string.Empty;
                IsBusy = false;
                gridControl.RefreshData();
                gridControl.UpdateLayout();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

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
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingOrdersViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingOrdersViewCustomPrintFooterTemplate"];
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

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);
                Object[] objArray = (Object[])obj;
                MenuFlyout menu = (MenuFlyout)objArray[0];
                GridControl grid = (GridControl)objArray[1];
                currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                _flyoutClosedHandler = (sender, e) => FlyoutControl_Closed(sender, e, grid, menu);
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

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
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
        private void FlyoutControl_Closed(object sender, EventArgs e, GridControl grid, MenuFlyout menu)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = currentDuration;
                UnsubscribeFlyoutClosed(menu);
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
                    TSMCommon.Instance.FromDate = thisMonthStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = thisMonthEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    TSMCommon.Instance.FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    TSMCommon.Instance.FromDate = lastMonthStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = lastMonthEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    TSMCommon.Instance.FromDate = thisWeekStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = thisWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    TSMCommon.Instance.FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    TSMCommon.Instance.FromDate = lastWeekStart.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = lastWeekEnd.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    TSMCommon.Instance.FromDate = StartDate.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = EndDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;

                }
                else if (IsButtonStatus == 8)//this year
                {
                    DateTime StartMDate = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime EndToMDate = new DateTime(DateTime.Now.Year, 12, 31);
                    TSMCommon.Instance.FromDate = StartMDate.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = EndToMDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 9)//last year
                {
                    TSMCommon.Instance.FromDate = StartFromDate.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = EndToDate.ToString(shortDateFormat);
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    TSMCommon.Instance.FromDate = Date_F.ToShortDateString();
                    TSMCommon.Instance.ToDate = Date_T.ToShortDateString();
                    IsButtonStatus = 0;
                }
                else if (IsButtonStatus == 0) //custome range
                {
                    TSMCommon.Instance.FromDate = StartDate.ToString(shortDateFormat);
                    TSMCommon.Instance.ToDate = EndDate.ToString(shortDateFormat);
                    //IsButtonStatus = 0;
                }
                IsUpdate = true;
                FillWorkLogReportList();
               // AddCustomSetting();
                IsBusy = false;
                if (DXSplashScreen.IsActive){DXSplashScreen.Close();}
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
      
        private void FillWorkLogReportList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportList...", category: Category.Info, priority: Priority.Low);
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
                if (TSMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    ObservableCollection<TSMWorkLogReport> TempWorkLogReportGridList = new ObservableCollection<TSMWorkLogReport>();
                    WorkLogReportGridList = new ObservableCollection<TSMWorkLogReport>();
                    try
                    {
                        foreach (Company plant in TSMCommon.Instance.SelectedPlantOwnerList)
                        {
                            //TSMService = new TSMServiceController("localhost:6699");
                            TempWorkLogReportGridList = new ObservableCollection<TSMWorkLogReport>(TSMService.GetOTWorkLogTimesByPeriodAndSite_V2690(DateTime.ParseExact(TSMCommon.Instance.FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(TSMCommon.Instance.ToDate, "dd/MM/yyyy", null), plant.IdCompany, plant));

                            if (TempWorkLogReportGridList != null)
                                TempWorkLogReportGridList.ToList().ForEach(i => i.Site = plant);
                            foreach (var item in TempWorkLogReportGridList)
                            {
                                WorkLogReportGridList.Add(item);
                            }
                           
                        }

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWorkLogReportList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWorkLogReportList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    WorkLogReportGridList = new ObservableCollection<TSMWorkLogReport>(WorkLogReportGridList);
                }
                else
                {
                    WorkLogReportGridList = new ObservableCollection<TSMWorkLogReport>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkLogReportList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void OTCodeHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OTCodeHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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
                TableView detailView = (TableView)obj;
                ViewWorkOrderViewModel viewWorkOrderViewModel = new ViewWorkOrderViewModel();
                ViewWorkOrderView viewWorkOrderView = new ViewWorkOrderView();

                EventHandler handle = delegate { viewWorkOrderView.Close(); };
                viewWorkOrderViewModel.RequestClose += handle;
                Ots ot = new Ots();
                //TempWorklog temp = (TempWorklog)detailView.DataControl.CurrentItem;
                //ot.IdOT = temp.IdOT;
                //ot.Site = temp.Site;
                //ot.IdSite = temp.IdSite;

                viewWorkOrderViewModel.Init(ot);
                viewWorkOrderView.DataContext = viewWorkOrderViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                viewWorkOrderView.Owner = Window.GetWindow(ownerInfo);
                viewWorkOrderView.ShowDialogWindow();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method OTCodeHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OTCodeHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowSelectedFilterGridAction(object e)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);
            //    if (QuickFilterList.Count > 0)
            //    {
            //        System.Windows.Controls.SelectionChangedEventArgs obj = (System.Windows.Controls.SelectionChangedEventArgs)e;
            //        string Template = null;
            //        string _FilterString = null;
            //        if (obj.AddedItems.Count > 0)
            //        {
            //            //int IdTemplate = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Id;
            //            Template = ((TileBarFilters)(obj.AddedItems)[0]).Type;
            //            _FilterString = ((TileBarFilters)(obj.AddedItems)[0]).FilterCriteria;
            //            CustomFilterStringName = ((TileBarFilters)(obj.AddedItems)[0]).Caption;
            //        }
            //        if (CustomFilterStringName != null && CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
            //            return;
            //        if (CustomFilterStringName == "Unassigned")
            //        {
            //            FilterString = "IsNullOrEmpty([OperatorNames])";
            //            return;
            //        }
            //        if (Template == null)
            //        {
            //            if (!string.IsNullOrEmpty(_FilterString))
            //                FilterString = _FilterString;
            //            else
            //                FilterString = string.Empty;
            //        }
            //        else
            //        {
            //            //FilterString = "[OperatorNames] In ('" + Template + "')";
            //            FilterString = "Contains([OperatorNames], '" + Template + "')";
            //        }
            //    }
            //    GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TreeListView table = (TreeListView)obj.OriginalSource;
            TreeListControl gridControl = (TreeListControl)(table).DataControl;
            ShowFilterEditor(obj);
        }
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
            //    CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
            //    CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
            //    string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
            //    if (IsEdit)
            //    {
            //        customFilterEditorViewModel.FilterName = CustomFilterStringName;
            //        customFilterEditorViewModel.IsSave = true;
            //        customFilterEditorViewModel.IsNew = false;
            //        IsEdit = false;
            //    }
            //    else
            //        customFilterEditorViewModel.IsNew = true;
            //    customFilterEditorViewModel.Init(e.FilterControl, QuickFilterList);
            //    customFilterEditorView.DataContext = customFilterEditorViewModel;
            //    EventHandler handle = delegate { customFilterEditorView.Close(); };
            //    customFilterEditorViewModel.RequestClose += handle;
            //    customFilterEditorView.Title = titleText;
            //    customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
            //    customFilterEditorView.Grid.Children.Add(e.FilterControl);
            //    customFilterEditorView.ShowDialog();
            //    if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
            //    {
            //        TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
            //        if (tileBarItem != null)
            //        {
            //            QuickFilterList.Remove(tileBarItem);
            //            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
            //            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
            //            {
            //                string key = setting.Key;
            //                if (setting.Key.Contains(userSettingsKey))
            //                    key = setting.Key.Replace(userSettingsKey, "");
            //                if (!key.Equals(tileBarItem.Caption))
            //                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
            //            }
            //            ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            //            GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            //        }
            //    }
            //    else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
            //    {
            //        TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
            //        if (tileBarItem != null)
            //        {
            //            CustomFilterStringName = customFilterEditorViewModel.FilterName;
            //            string filterCaption = tileBarItem.Caption;
            //            tileBarItem.Caption = customFilterEditorViewModel.FilterName;
            //            tileBarItem.EntitiesCount = VisibleChildRowCount;
            //            tileBarItem.EntitiesCountVisibility = Visibility.Visible;
            //            tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
            //            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
            //            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
            //            {
            //                string key = setting.Key;
            //                if (setting.Key.Contains(userSettingsKey))
            //                    key = setting.Key.Replace(userSettingsKey, "");
            //                if (!key.Equals(filterCaption))
            //                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
            //                else
            //                    lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
            //            }
            //            ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            //            GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            //        }
            //    }
            //    else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
            //    {
            //        QuickFilterList.Add(new TileBarFilters()
            //        {
            //            Caption = customFilterEditorViewModel.FilterName,
            //            Id = 0,
            //            BackColor = null,
            //            ForeColor = null,
            //            EntitiesCountVisibility = Visibility.Visible,
            //            FilterCriteria = customFilterEditorViewModel.FilterCriteria,
            //            Height = 80,
            //            width = 200,
            //            EntitiesCount = VisibleChildRowCount
            //        });
            //        string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
            //        GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
            //        ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            //        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            //        SelectedFilter = QuickFilterList.LastOrDefault();
            //    }
            //    GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        private void AddCustomSetting()
        {
            //try
            //{
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //    DXSplashScreen.Show(x =>
            //    {
            //        Window win = new Window()
            //        {
            //            ShowActivated = false,
            //            WindowStyle = WindowStyle.None,
            //            ResizeMode = ResizeMode.NoResize,
            //            AllowsTransparency = true,
            //            Background = new SolidColorBrush(Colors.Transparent),
            //            ShowInTaskbar = false,
            //            Topmost = true,
            //            SizeToContent = SizeToContent.WidthAndHeight,
            //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //        };
            //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
            //        win.Topmost = false;
            //        return win;
            //    }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
            //    GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading filter...";
            //    GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
            //    List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
            //    if (tempUserSettings != null && tempUserSettings.Count > 0)
            //    {
            //        foreach (var item in tempUserSettings)
            //        {
            //            ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Ots)), item.Value, false);
            //            List<Ots> tempList = new List<Ots>();
            //            foreach (var ot in MainOtsList_New)
            //            {
            //                if (evaluator.Fit(ot))
            //                    tempList.Add(ot);
            //            }
            //            FilterString = item.Value;
            //            QuickFilterList.Add(new TileBarFilters()
            //            {
            //                Caption = item.Key.Replace(userSettingsKey, ""),
            //                Id = 0,
            //                BackColor = null,
            //                ForeColor = null,
            //                FilterCriteria = item.Value,
            //                EntitiesCount = tempList.Count,
            //                EntitiesCountVisibility = Visibility.Visible,
            //                Height = 80,
            //                width = 200
            //            });
            //        }
            //    }
            //    GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //}
        }
        private void CommandTileBarDoubleClickAction(object obj)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
            //    foreach (var item in filterList)
            //    {
            //        if (CustomFilterStringName != null)
            //        {
            //            if (CustomFilterStringName.Equals(item.Caption))
            //                return;
            //        }
            //    }
            //    TreeListView table = (TreeListView)obj;
            //    TreeListControl gridControl = (TreeListControl)(table).DataControl;
            //    List<TreeListColumn> GridColumnList = gridControl.Columns.Where(x => x.FieldName != null).ToList();
            //    string columnName = FilterString.Substring(FilterString.IndexOf("[") + 1, FilterString.IndexOf("]") - 2 - FilterString.IndexOf("[") + 1);
            //    TreeListColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals(columnName));
            //    IsEdit = true;
            //    table.ShowFilterEditor(column);
            //    GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        public void FillWorkLogReportFilterList()
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);
            //    ListOfTemplate = new ObservableCollection<Template>();
            //    FillWorkLogReportFilterTiles(ListOfTemplate, MainOtsList);
            //    GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (FaultException<ServiceException> ex)
            //{
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
            //    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //}
            //catch (ServiceUnexceptedException ex)
            //{
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        public void FillWorkLogReportFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);
            //    QuickFilterList = new ObservableCollection<TileBarFilters>();
            //    QuickFilterList.Add(new TileBarFilters()
            //    {
            //        Caption = string.Format(System.Windows.Application.Current.FindResource("AllWO").ToString()),
            //        Id = 0,
            //        EntitiesCount = MainListWorkOrder.Count(),
            //        //ImageUri = "Template.png",
            //        //BackColor = "Wheat",
            //        EntitiesCountVisibility = Visibility.Visible,
            //        Height = 80,
            //        width = 200
            //    });
            //    foreach (Ots ot in MainListWorkOrder)
            //    {
            //        if (ot.UserShortDetails != null && ot.UserShortDetails.Count > 0)
            //        {
            //            foreach (UserShortDetail user in ot.UserShortDetails)
            //            {
            //                ImageSource userImage = null;
            //                //[001] added
            //                if (user.UserImageInBytes == null)
            //                {
            //                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
            //                    {
            //                        if (user != null && user.IdUserGender == 1)
            //                            userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png"));
            //                        else if (user != null && user.IdUserGender == 2)
            //                            userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png"));
            //                    }
            //                    else
            //                    {
            //                        if (user != null && user.IdUserGender == 1)
            //                            userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png"));
            //                        else if (user != null && user.IdUserGender == 2)
            //                            userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png"));
            //                    }
            //                }
            //                else
            //                {
            //                    userImage = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes);
            //                }
            //                if (!QuickFilterList.Any(x => x.Id == user.IdUser))
            //                {
            //                    QuickFilterList.Add(new TileBarFilters()
            //                    {
            //                        Caption = user.UserName,
            //                        Id = user.IdUser,
            //                        Type = user.UserName,
            //                        EntitiesCount = MainListWorkOrder.Count(x => !string.IsNullOrEmpty(x.OperatorNames) && x.OperatorNames.Split(',').ToList().Contains(user.UserName)),
            //                        // Image = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes),   
            //                        Image = userImage,
            //                        //BackColor = item.HtmlColor,
            //                        EntitiesCountVisibility = Visibility.Visible,
            //                        Height = 80,
            //                        width = 200
            //                    });
            //                }
            //            }
            //        }
            //    }
            //    QuickFilterList.Add(new TileBarFilters()
            //    {
            //        Caption = string.Format(System.Windows.Application.Current.FindResource("UnassignedWO").ToString()),
            //        Id = 0,
            //        EntitiesCount = MainListWorkOrder.Count(x => string.IsNullOrEmpty(x.OperatorNames)),
            //        //ImageUri = "Template.png",
            //        //BackColor = "Wheat",
            //        EntitiesCountVisibility = Visibility.Visible,
            //        Height = 80,
            //        width = 200
            //    });
            //    QuickFilterList.Add(new TileBarFilters()
            //    {
            //        Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
            //        Id = 0,
            //        BackColor = null,
            //        ForeColor = null,
            //        EntitiesCountVisibility = Visibility.Collapsed,
            //        Height = 30,
            //        width = 200,
            //    });
            //    filterList = new List<TileBarFilters>();
            //    filterList = QuickFilterList.ToList();
            //    GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        #endregion //End Of Methods
        #region Column Chooser
        private void WorklogReportGridControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogReportGridControlLoadedCommandAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(WorklogReportGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(WorklogReportGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(WorklogReportGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, WorklogReportVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, WorklogReportVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsWorklogReportColumnChooserVisible = true;
                }
                else
                {
                    IsWorklogReportColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method WorklogReportGridControlLoadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on WorklogReportGridControlLoadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void WorklogReportVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogReportVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorklogReportGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsWorklogReportColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method WorklogReportVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in WorklogReportVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void WorklogReportVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogReportVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorklogReportGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method WorklogReportVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in WorklogReportVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void WorklogReportItemListTableViewLoadedCommandAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new System.Windows.Point(20, 180),
                Size = new System.Windows.Size(250, 250)
            };
        }
        private void WorklogReportGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogReportGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(WorklogReportGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method WorklogReportGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on WorklogReportGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        #endregion
    }
}
