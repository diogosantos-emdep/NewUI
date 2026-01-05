using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.DataProcessing;
using DevExpress.Xpf.Scheduling.Visual;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.Xpf.Charts;
using System.Text;
using DevExpress.XtraRichEdit.Model;
using System.Runtime.InteropServices.ComTypes;
using DevExpress.Mvvm.Native;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[nsatpute][12.09.2025][GEOS2-8791]
    public class ScheduleEventsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("10.13.3.33:99");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Task log
        // [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section (unused code)
        #endregion

        #region Declaration

        private bool isInIt = false;
        private bool IsBusy;
        //public string WMS_Grid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WMS_MinMaxArticleStock_Setting.Xml";        
        private ObservableCollection<MonthlyData> monthList;
        private bool isEditable;
        // [nsatpute][17-09-2025][GEOS2-8793]
        private bool isPrinting;
        private ObservableCollection<LookupValue> legendList;
        private ObservableCollection<long> scheduleYears;
        private long selectedYear;
        private ObservableCollection<LookupValue> eventTypeList;
		//[nsatpute][31.10.2025][GEOS2-8801]
        private double savedScrollPosition = 0;
        public event Action RequestScrollPreservation;
        public event Action RequestScrollRestoration;
        #endregion // Declaration

        #region Properties      

        public ObservableCollection<MonthlyData> MonthList
        {
            get { return monthList; }
            set
            {
                monthList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MonthList"));
            }
        }
        public bool IsEditable
        {
            get { return isEditable; }
            set
            {
                isEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditable"));
            }
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        public bool IsPrinting
        {
            get { return isPrinting; }
            set
            {
                isPrinting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPrinting"));
            }
        }
        public ObservableCollection<LookupValue> LegendList
        {
            get { return legendList; }
            set
            {
                legendList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LegendList"));
            }
        }
        //[nsatpute][22.09.2025][GEOS2-8793]
        public ObservableCollection<long> ScheduleYears
        {
            get { return scheduleYears; }
            set
            {
                scheduleYears = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScheduleYears"));
            }
        }
        //[nsatpute][22.09.2025][GEOS2-8793]  
        public long SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedYear"));
            }
        }

        public ObservableCollection<LookupValue> EventTypeList
        {
            get { return eventTypeList; }
            set
            {
                eventTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EventTypeList"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        #endregion

        #region Commands

        public ICommand WarehouseValueChangedCommand { get; private set; }
        public ICommand CommandTransferMaterial { get; private set; }
        public ICommand RefreshScheduleEventCommand { get; set; }
        public ICommand PrintWarehouseArticleViewCommand { get; set; }
        public ICommand ExportWarehouseArticleViewCommand { get; set; }
        public ICommand SelectedYearChangedCommand { get; set; }
        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand AddNewScheduleButtonCommand { get; set; }
        public ICommand LegendItemCheckedCommand { get; set; }
        #endregion 

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        #endregion // Events

        #region Constructor

        public ScheduleEventsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ScheduleEventsViewModel()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;


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



                RefreshScheduleEventCommand = new RelayCommand(new Action<object>(RefreshScheduleEventCommandAction));
                PrintWarehouseArticleViewCommand = new RelayCommand(new Action<object>(PrintWarehouseArticleList));
                ExportWarehouseArticleViewCommand = new RelayCommand(new Action<object>(ExportWarehouseArticleList));
                SelectedYearChangedCommand = new RelayCommand(new Action<object>(SelectedYearChangedCommandAction));
                AddNewScheduleButtonCommand = new RelayCommand(new Action<object>(AddNewScheduleButtonCommandAction));
                LegendItemCheckedCommand = new RelayCommand(new Action<object>(LegendItemCheckedCommandAction));
                GridDoubleClickCommand = new DelegateCommand<object>(GridDoubleClickCommandAction);
                WarehouseValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseValueChangedCommandAction);
                isInIt = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor ScheduleEventsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ScheduleEventsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods

        //[nsatpute][22.09.2025][GEOS2-8793]
        public void RefreshScheduleEventCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshScheduleEventCommandAction()...", category: Category.Info, priority: Priority.Low);
                // Save scroll position before refresh
                RequestScrollPreservation?.Invoke();
                IsBusy = true;
                Init();
                IsBusy = false;
                // Restore scroll position after refresh
                RequestScrollRestoration?.Invoke();
                GeosApplication.Instance.Logger.Log("Method RefreshScheduleEventCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshScheduleEventCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                FillAllDaysOfYear();
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintWarehouseArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);
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
                // [nsatpute][17-09-2025][GEOS2-8793]
                var tableView = (TableView)obj;
                var gridControl = tableView.Grid;
                var printableItems = new List<PrintableMonthItem>();
                IEnumerable items = gridControl.ItemsSource as IEnumerable;
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        dynamic row = item;
                        printableItems.Add(new PrintableMonthItem
                        {
                            Day = row.Day?.ToString(),
                            January = row.JanuaryWeekend?.ToString(),
                            February = row.FebruaryWeekend?.ToString(),
                            March = row.MarchWeekend?.ToString(),
                            April = row.AprilWeekend?.ToString(),
                            May = row.MayWeekend?.ToString(),
                            June = row.JuneWeekend?.ToString(),
                            July = row.JulyWeekend?.ToString(),
                            August = row.AugustWeekend?.ToString(),
                            September = row.SeptemberWeekend?.ToString(),
                            October = row.OctoberWeekend?.ToString(),
                            November = row.NovemberWeekend?.ToString(),
                            December = row.DecemberWeekend?.ToString()
                        });
                    }
                }

                // Create a GridControl for printing
                var printGridControl = new DevExpress.Xpf.Grid.GridControl();
                printGridControl.ItemsSource = printableItems;
                var printView = new TableView();
                printGridControl.View = printView;

                // Add columns (with cell templates that include Border for background)
                printGridControl.Columns.Add(CreatePrintableColumn("Day", "Day", 30));
                printGridControl.Columns.Add(CreatePrintableColumn("January", "January", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("February", "February", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("March", "March", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("April", "April", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("May", "May", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("June", "June", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("July", "July", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("August", "August", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("September", "September", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("October", "October", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("November", "November", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("December", "December", 80));
                printGridControl.Columns.Add(CreatePrintableColumn("Day", "Day", 30));

                PrintableControlLink pcl = new PrintableControlLink(printView)
                {
                    Margins = new System.Drawing.Printing.Margins(5, 5, 5, 5),
                    Landscape = true,
                    CustomPaperSize = new System.Drawing.Size(2200, 800),
                    PageHeaderTemplate = (DataTemplate)tableView.Resources["Scheduleeventsview_Schedule"]
                };
                pcl.ReportHeaderData = Application.Current.Resources["Scheduleeventsview_Schedule"].ToString();
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ScheduleEventsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ScheduleReportPrintFooterTemplate"];
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWarehouseArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // Helper to create a GridColumn with a cell template using Border background
        private GridColumn CreatePrintableColumn(string fieldName, string header, double width)
        {
            var column = new GridColumn()
            {
                FieldName = fieldName,
                Header = header,
                Width = width
            };
            // DataTemplate for background color (set color as needed or bind to property)
            var cellTemplateXaml = $@"
                    <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                        <Border  Padding='2'>
                            <TextBlock Background='Red' Text='{{Binding {fieldName}}}'/>
                        </Border>
                    </DataTemplate>";
            column.CellTemplate = (DataTemplate)System.Windows.Markup.XamlReader.Parse(cellTemplateXaml);
            return column;
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        private void ExportWarehouseArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Schedule Event Report";
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

                    ResultFileName = saveFile.FileName;
                    TableView activityTableView = ((TableView)obj);

                    // Store original values
                    bool originalShowTotalSummary = activityTableView.ShowTotalSummary;
                    bool originalShowFixedTotalSummary = activityTableView.ShowFixedTotalSummary;

                    // Modify for export
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.ShowPageTitle = DevExpress.Utils.DefaultBoolean.True;
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    // Reset immediately after export
                    activityTableView.ShowTotalSummary = originalShowTotalSummary;
                    activityTableView.ShowFixedTotalSummary = originalShowFixedTotalSummary;

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWarehouseArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        public void Init()
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
                IsEditable = GeosApplication.Instance.IsWMS_SchedulePermission;
                FillScheduleYears();
                FillAllDaysOfYear();
                FillEventTypeList();
                FilLegendList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);                
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        public void FillAllDaysOfYear()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllDaysOfYear()...", category: Category.Info, priority: Priority.Low);

                if (Warehouse.WarehouseCommon.Instance.SelectedYear == 0)
                {
                    Warehouse.WarehouseCommon.Instance.SelectedYear = SelectedYear = DateTime.Now.Year;
                }
                else if (SelectedYear == 0)
                {
                    SelectedYear = Warehouse.WarehouseCommon.Instance.SelectedYear;
                }
                MonthList = GenerateData(SelectedYear);
                FillScheduleEvents();
                GeosApplication.Instance.Logger.Log("Method FillAllDaysOfYear()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAllDaysOfYear()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private ObservableCollection<MonthlyData> GenerateData(long year)
        {
            var data = new ObservableCollection<MonthlyData>();

            try
            {
                GeosApplication.Instance.Logger.Log("Method GenerateData()...", category: Category.Info, priority: Priority.Low);

                var culture = CultureInfo.CurrentCulture;
                var calendar = culture.Calendar;

                bool isLeapYear = DateTime.IsLeapYear((int)year);
                int februaryDays = isLeapYear ? 29 : 28;

                for (int day = 1; day <= 31; day++)
                {
                    var monthlyData = new MonthlyData { Day = day };

                    // Set values for each month
                    SetMonthData(monthlyData, "January", 1, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "February", 2, day, februaryDays, year, culture, calendar);
                    SetMonthData(monthlyData, "March", 3, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "April", 4, day, 30, year, culture, calendar);
                    SetMonthData(monthlyData, "May", 5, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "June", 6, day, 30, year, culture, calendar);
                    SetMonthData(monthlyData, "July", 7, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "August", 8, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "September", 9, day, 30, year, culture, calendar);
                    SetMonthData(monthlyData, "October", 10, day, 31, year, culture, calendar);
                    SetMonthData(monthlyData, "November", 11, day, 30, year, culture, calendar);
                    SetMonthData(monthlyData, "December", 12, day, 31, year, culture, calendar);

                    data.Add(monthlyData);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GenerateData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GenerateData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


            return data;
        }

        private void SetMonthData(MonthlyData monthlyData, string monthName, int month, int day, int maxDays, long year, CultureInfo culture, Calendar calendar)
        {
            var dayProperty = typeof(MonthlyData).GetProperty(monthName);
            var showButtonProperty = typeof(MonthlyData).GetProperty(monthName + "ShowButton");
            var backColorProperty = typeof(MonthlyData).GetProperty(monthName + "BackColor");
            var dayOfWeekProperty = typeof(MonthlyData).GetProperty(monthName + "DayOfWeek");
            var weekNumberProperty = typeof(MonthlyData).GetProperty(monthName + "WeekNumber");
            var saturdayProperty = typeof(MonthlyData).GetProperty(monthName + "Saturday");
            var sundayProperty = typeof(MonthlyData).GetProperty(monthName + "Sunday");
            var weekendProperty = typeof(MonthlyData).GetProperty(monthName + "Weekend");
            var monthlyDataProperty = typeof(MonthlyData).GetProperty(monthName + "Data");
            // New properties
            var buttonBackgroundColorProperty = typeof(MonthlyData).GetProperty(monthName + "ButtonBackgroundColor");
            var buttonForegroundColorProperty = typeof(MonthlyData).GetProperty(monthName + "ButtonForegroundColor");
            var fontWeightProperty = typeof(MonthlyData).GetProperty(monthName + "FontWeight");

            if (day <= maxDays)
            {
                dayProperty.SetValue(monthlyData, day);
                showButtonProperty.SetValue(monthlyData, Visibility.Visible);
                backColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.Transparent);

                var date = new DateTime((int)year, month, day);
                var dayOfWeek = date.DayOfWeek;
                var weekNumber = calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);

                dayOfWeekProperty.SetValue(monthlyData, dayOfWeek);
                weekNumberProperty.SetValue(monthlyData, weekNumber);

                // Set Saturday and Sunday values - only show if it's that specific day
                string saturdayValue = dayOfWeek == DayOfWeek.Saturday ? $"Saturday" : null;
                string sundayValue = dayOfWeek == DayOfWeek.Sunday ? $"Sunday CW{weekNumber}" : null;

                saturdayProperty.SetValue(monthlyData, saturdayValue);
                sundayProperty.SetValue(monthlyData, sundayValue);

                // Set the combined weekend property
                string weekendValue = saturdayValue ?? sundayValue ?? string.Empty;
                weekendProperty.SetValue(monthlyData, weekendValue);
                monthlyDataProperty.SetValue(monthlyData, weekendValue);
                // Set button styles based on holiday (Sunday or Saturday)
                if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday)
                {
                    // [nsatpute][07.10.2025][GEOS2-8797]
                    buttonBackgroundColorProperty.SetValue(monthlyData, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5D7723")));
                    buttonForegroundColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.White);
                    fontWeightProperty.SetValue(monthlyData, System.Windows.FontWeights.Bold);
                }
                else
                {
                    // Normal styling: transparent background, black text, normal weight
                    buttonBackgroundColorProperty.SetValue(monthlyData, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent));
                    buttonForegroundColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.Black);
                    fontWeightProperty.SetValue(monthlyData, System.Windows.FontWeights.Normal);
                }
            }
            else
            {
                dayProperty.SetValue(monthlyData, null);
                showButtonProperty.SetValue(monthlyData, Visibility.Hidden);
                backColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.Black);
                dayOfWeekProperty.SetValue(monthlyData, null);
                weekNumberProperty.SetValue(monthlyData, null);
                saturdayProperty.SetValue(monthlyData, null);
                sundayProperty.SetValue(monthlyData, null);
                weekendProperty.SetValue(monthlyData, string.Empty);

                // Set default styles for non-existing days
                buttonBackgroundColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.Transparent);
                buttonForegroundColorProperty.SetValue(monthlyData, System.Windows.Media.Brushes.Black);
                fontWeightProperty.SetValue(monthlyData, System.Windows.FontWeights.Normal);
            }
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FindVisualChild()...", category: Category.Info, priority: Priority.Low);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                        return (T)child;
                    else
                    {
                        T childOfChild = FindVisualChild<T>(child);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FindVisualChild()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FindVisualChild()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
        public class PrintableMonthItem
        {
            public string Day { get; set; }
            public string January { get; set; }
            public string February { get; set; }
            public string March { get; set; }
            public string April { get; set; }
            public string May { get; set; }
            public string June { get; set; }
            public string July { get; set; }
            public string August { get; set; }
            public string September { get; set; }
            public string October { get; set; }
            public string November { get; set; }
            public string December { get; set; }
        }
        // [nsatpute][17-09-2025][GEOS2-8793]
		//[nsatpute][31.10.2025][GEOS2-8801]
        private void FilLegendList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilLegendList()...", category: Category.Info, priority: Priority.Low);
                if (LegendList == null)
                {
                    IList<LookupValue> temptypeList = CrmService.GetLookupValues(101);
                    foreach (LookupValue val in temptypeList) { val.IsSelected = true; }
                    LegendList = new ObservableCollection<LookupValue>(temptypeList);
                }
                GeosApplication.Instance.Logger.Log("Method FilLegendList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FilLegendList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FilLegendList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FilLegendList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [nsatpute][22-09-2025][GEOS2-8793]
        private void FillScheduleYears()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScheduleYears()...", category: Category.Info, priority: Priority.Low);
                List<long> tempList = WarehouseService.GetWarehouseScheduleYears();
                ScheduleYears = new ObservableCollection<long>(tempList);
                GeosApplication.Instance.Logger.Log("Method FillScheduleYears()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleYears " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleYears " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleYears()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewScheduleButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewScheduleButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                AddNewWarehouseScheduleEvent(null, MonthList, null, 0);
                GeosApplication.Instance.Logger.Log("Method AddNewScheduleButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewScheduleButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][31.10.2025][GEOS2-8801]
        private void LegendItemCheckedCommandAction(object obj)
        {
            RefreshScheduleEventCommandAction(null);
        }

        //[nsatpute][22.09.2025][GEOS2-8795]
        private void GridDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction...", category: Category.Info, priority: Priority.Low);
            try
            {
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (detailView.FocusedRow is MonthlyData selectedDay)
                {
                    var monthEventsMap = new Dictionary<string, Func<MonthlyData, List<ScheduleEvent>>>
                    {
                        ["JanuaryData"] = d => d.JanuaryScheduleEvents,
                        ["FebruaryData"] = d => d.FebruaryScheduleEvents,
                        ["MarchData"] = d => d.MarchScheduleEvents,
                        ["AprilData"] = d => d.AprilScheduleEvents,
                        ["MayData"] = d => d.MayScheduleEvents,
                        ["JuneData"] = d => d.JuneScheduleEvents,
                        ["JulyData"] = d => d.JulyScheduleEvents,
                        ["AugustData"] = d => d.AugustScheduleEvents,
                        ["SeptemberData"] = d => d.SeptemberScheduleEvents,
                        ["OctoberData"] = d => d.OctoberScheduleEvents,
                        ["NovemberData"] = d => d.NovemberScheduleEvents,
                        ["DecemberData"] = d => d.DecemberScheduleEvents
                    };

                    string fieldName = detailView.FocusedColumn.FieldName;

                    if (monthEventsMap.TryGetValue(fieldName, out var getEvents))
                    {
                        string monthData = GetMonthData(selectedDay, fieldName);
                        if (monthData == null || monthData.Contains("Saturday") || monthData.Contains("Sunday"))
                            return;

                        var events = getEvents(selectedDay);

                        if (events?.Count > 0)
                        {
                            EditEvent(selectedDay, MonthList, fieldName, SelectedYear);
                        }
                        else
                        {
                            AddEvent(selectedDay, MonthList, fieldName, SelectedYear);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GridDoubleClickCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][25.09.2025][GEOS2-8797]
        private void WarehouseValueChangedCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                FillAllDaysOfYear();
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method WarehouseValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                IsBusy = false;
            }

        }
        private string GetMonthData(MonthlyData selectedDay, string fieldName)
        {
            switch (fieldName)
            {
                case "JanuaryData": return selectedDay.JanuaryData;
                case "FebruaryData": return selectedDay.FebruaryData;
                case "MarchData": return selectedDay.MarchData;
                case "AprilData": return selectedDay.AprilData;
                case "MayData": return selectedDay.MayData;
                case "JuneData": return selectedDay.JuneData;
                case "JulyData": return selectedDay.JulyData;
                case "AugustData": return selectedDay.AugustData;
                case "SeptemberData": return selectedDay.SeptemberData;
                case "OctoberData": return selectedDay.OctoberData;
                case "NovemberData": return selectedDay.NovemberData;
                case "DecemberData": return selectedDay.DecemberData;
                default: return string.Empty;
            }
        }
        private void AddEvent(MonthlyData selectedDay, ObservableCollection<MonthlyData> MonthList, string monthName, long year)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEvent() executed successfully", category: Category.Info, priority: Priority.Low);
                AddNewWarehouseScheduleEvent(selectedDay, MonthList, monthName, year);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddEvent() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][14.10.2025][GEOS2-8799]
        private void EditEvent(MonthlyData selectedDay, ObservableCollection<MonthlyData> MonthList, string monthName, long year)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEvent() executed successfully", category: Category.Info, priority: Priority.Low);
                EditWarehouseScheduleEvent(selectedDay, MonthList, monthName, year);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddEvent() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24.09.2025][GEOS2-8795]
        private void AddNewWarehouseScheduleEvent(MonthlyData selectedDay = null, ObservableCollection<MonthlyData> MonthList = null, string monthName = null, long year = 0)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewWarehouseScheduleEvent ...", category: Category.Info, priority: Priority.Low);
                AddScheduleEventViewModel addScheduleEventViewModel = new AddScheduleEventViewModel();
                if (selectedDay == null || monthName == null)
                {
                    addScheduleEventViewModel.StartDate = addScheduleEventViewModel.StartTime = addScheduleEventViewModel.EndDate = addScheduleEventViewModel.EndTime = DateTime.Today;
                }
                else
                {
                    int day = selectedDay.Day;
                    int month = 0;
                    var monthDict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                                            {
                                                {"JanuaryData", 1},
                                                {"FebruaryData", 2},
                                                {"MarchData", 3},
                                                {"AprilData", 4},
                                                {"MayData", 5},
                                                {"JuneData", 6},
                                                {"JulyData", 7},
                                                {"AugustData", 8},
                                                {"SeptemberData", 9},
                                                {"OctoberData", 10},
                                                {"NovemberData", 11},
                                                {"DecemberData", 12}
                                            };

                    month = monthDict.TryGetValue(monthName, out int result) ? result : 0;
                    DateTime dt = new DateTime((int)year, month, day);
                    addScheduleEventViewModel.StartDate = addScheduleEventViewModel.StartTime = addScheduleEventViewModel.EndDate = addScheduleEventViewModel.EndTime = dt;
                }
                addScheduleEventViewModel.Init(MonthList, EventTypeList);
                AddScheduleEventView addScheduleEventView = new AddScheduleEventView();
                EventHandler handle = delegate { addScheduleEventView.Close(); };
                addScheduleEventViewModel.RequestClose += handle;
                addScheduleEventView.DataContext = addScheduleEventViewModel;
                addScheduleEventView.ShowDialogWindow();

                if (addScheduleEventViewModel.IsSave)
                {
                    RefreshScheduleEventCommandAction(null);  
                }
                GeosApplication.Instance.Logger.Log("Method AddNewWarehouseScheduleEvent() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewWarehouseScheduleEvent() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][14.10.2025][GEOS2-8799]
        private void EditWarehouseScheduleEvent(MonthlyData selectedDay, ObservableCollection<MonthlyData> MonthList, string monthName, long year)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWarehouseScheduleEvent ...", category: Category.Info, priority: Priority.Low);
                EditScheduleEventViewModel editScheduleEventViewModel = new EditScheduleEventViewModel();
                EditScheduleEventMultipleViewModel editScheduleEventMultipleViewModel = new EditScheduleEventMultipleViewModel();
                List<ScheduleEvent> eventList = null;
                switch (monthName)
                {
                    case "JanuaryData":
                        eventList = selectedDay.JanuaryScheduleEvents;
                        break;
                    case "FebruaryData":
                        eventList = selectedDay.FebruaryScheduleEvents;
                        break;
                    case "MarchData":
                        eventList = selectedDay.MarchScheduleEvents;
                        break;
                    case "AprilData":
                        eventList = selectedDay.AprilScheduleEvents;
                        break;
                    case "MayData":
                        eventList = selectedDay.MayScheduleEvents;
                        break;
                    case "JuneData":
                        eventList = selectedDay.JuneScheduleEvents;
                        break;
                    case "JulyData":
                        eventList = selectedDay.JulyScheduleEvents;
                        break;
                    case "AugustData":  
                        eventList = selectedDay.AugustScheduleEvents;
                        break;
                    case "SeptemberData":
                        eventList = selectedDay.SeptemberScheduleEvents;
                        break;
                    case "OctoberData": 
                        eventList = selectedDay.OctoberScheduleEvents;
                        break;
                    case "NovemberData":    
                        eventList = selectedDay.NovemberScheduleEvents;
                        break;
                    case "DecemberData":
                        eventList = selectedDay.DecemberScheduleEvents;
                        break;
                }
                if (eventList.Count == 1)
                {
                    editScheduleEventViewModel.Init(MonthList, EventTypeList, eventList[0]);
                    EditScheduleEventView editScheduleEventView = new EditScheduleEventView();
                    EventHandler handle = delegate { editScheduleEventView.Close(); };
                    editScheduleEventViewModel.RequestClose += handle;
                    editScheduleEventView.DataContext = editScheduleEventViewModel;
                    editScheduleEventView.ShowDialogWindow();
                    //[nsatpute][16.10.2025][GEOS2-8799]
                    if (editScheduleEventViewModel.IsSave)
                    {
                        RefreshScheduleEventCommandAction(null);
                    }
                }
                else if (eventList.Count > 1)
                {
                    editScheduleEventMultipleViewModel.Init(MonthList, EventTypeList, eventList);
                    EditScheduleEventMultipleView editScheduleEventMultipleView = new EditScheduleEventMultipleView();
                    EventHandler handle = delegate { editScheduleEventMultipleView.Close(); };
                    editScheduleEventMultipleViewModel.RequestClose += handle;
                    editScheduleEventMultipleView.DataContext = editScheduleEventMultipleViewModel;
                    editScheduleEventMultipleView.ShowDialogWindow();
                    //[nsatpute][16.10.2025][GEOS2-8799]
                    if (editScheduleEventMultipleViewModel.IsSave)
                    {
                        RefreshScheduleEventCommandAction(null);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditWarehouseScheduleEvent() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseScheduleEvent() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[nsatpute][22.09.2025][GEOS2-8795]
        private void RefreshMonthlyData()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshMonthlyData()...", category: Category.Info, priority: Priority.Low);
                string[] months = { "January", "February", "March", "April", "May", "June",
                       "July", "August", "September", "October", "November", "December" };

                foreach (MonthlyData mdata in MonthList)
                {
                    foreach (string month in months)
                    {
                        ProcessMonthEventsOptimized(mdata, month);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshMonthlyData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshMonthlyData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ProcessMonthEventsOptimized(MonthlyData mdata, string monthName)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ProcessMonthEventsOptimized()...", category: Category.Info, priority: Priority.Low);
                var eventsProp = mdata.GetType().GetProperty(monthName + "ScheduleEvents");
                var events = eventsProp?.GetValue(mdata) as List<ScheduleEvent>;

                if (events?.Count > 0)
                {
                    var weekendProp = mdata.GetType().GetProperty(monthName + "Weekend");
                    var showButtonProp = mdata.GetType().GetProperty(monthName + "ShowButton");
                    var bgColorProp = mdata.GetType().GetProperty(monthName + "ButtonBackgroundColor");
                    var fgColorProp = mdata.GetType().GetProperty(monthName + "ButtonForegroundColor");

                    bool hasObservations = false;
                    Brush firstButtonColor = null;

                    foreach (ScheduleEvent schEvent in events)
                    {
                        if (!string.IsNullOrEmpty(schEvent.Observations))
                        {
                            if (!hasObservations)
                            {
                                firstButtonColor = schEvent.ButtonColor;
                                hasObservations = true;
                            }
                        }
                    }

                    if (hasObservations)
                    {
                        weekendProp?.SetValue(mdata, EventTypeList.FirstOrDefault(x => x.IdLookupValue == events[0].IdTypeEvent).Value);
                        showButtonProp?.SetValue(mdata, Visibility.Visible);
                        bgColorProp?.SetValue(mdata, firstButtonColor);
                        fgColorProp?.SetValue(mdata, Brushes.Black);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ProcessMonthEventsOptimized()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ProcessMonthEventsOptimized()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[nsatpute][25.09.2025][GEOS2-8797]
        private void FillScheduleEvents()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScheduleEvents()...", category: Category.Info, priority: Priority.Low);
                WarehouseService = new WarehouseServiceController(WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl);
                List<ScheduleEvent> tempList = new List<ScheduleEvent>();
                try
                {
                    tempList = WarehouseService.GetWarehouseScheduleEventsByIdWarehouse(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, SelectedYear);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleEvents " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleEvents " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    UI.CustomControls.CustomMessageBox.Show("Error connecting to warehouse", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleEvents()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                List<ScheduleEvent> filteredList = tempList.Where(eve =>
                    eve.IdLogistic == 0 ||
                    LegendList.Any(x => x.IsSelected && x.IdLookupValue == eve.IdLogistic)
                ).ToList();

                // 1632 Air 1633 Sea 1634 Ground
                foreach (var x in MonthList)
                {
                    x.JanuaryCircleVisibility = Visibility.Collapsed;
                    x.FebruaryCircleVisibility = Visibility.Collapsed;
                    x.MarchCircleVisibility = Visibility.Collapsed;
                    x.AprilCircleVisibility = Visibility.Collapsed;
                    x.MayCircleVisibility = Visibility.Collapsed;
                    x.JuneCircleVisibility = Visibility.Collapsed;
                    x.JulyCircleVisibility = Visibility.Collapsed;
                    x.AugustCircleVisibility = Visibility.Collapsed;
                    x.SeptemberCircleVisibility = Visibility.Collapsed;
                    x.OctoberCircleVisibility = Visibility.Collapsed;
                    x.NovemberCircleVisibility = Visibility.Collapsed;
                    x.DecemberCircleVisibility = Visibility.Collapsed;
                }
                foreach (ScheduleEvent eve in filteredList)
                {
                    DateTime selectedDate = eve.StartDate;

                    MonthlyData selectedDay = MonthList.FirstOrDefault(x => x.Day == selectedDate.Day);
                    //if(eve.IdTypeEvent)
                    eve.ButtonColor = (SolidColorBrush)new BrushConverter().ConvertFrom(EventTypeList.FirstOrDefault(x => x.IdLookupValue == eve.IdTypeEvent).HtmlColor);
                    switch (selectedDate.Month)
                    {
                        case 1:
                            if (selectedDay.JanuaryScheduleEvents == null)
                                selectedDay.JanuaryScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.JanuaryScheduleEvents.Add(eve);
                            selectedDay.JanuaryData = eve.Observations;
                            if (selectedDay.JanuaryScheduleEvents.Count > 0)
                            {
                                selectedDay.JanuaryCircleVisibility = Visibility.Visible;
                                selectedDay.JanuaryScheduleEvents = selectedDay.JanuaryScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.JanuaryScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.JanuaryFirstEllipse = "G";
                                    selectedDay.JanuaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.JanuaryScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.JanuaryFirstEllipse = "S";
                                    selectedDay.JanuaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.JanuaryScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.JanuaryFirstEllipse = "A";
                                    selectedDay.JanuaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.JanuaryScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.JanuaryScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.JanuarySecondEllipse = "G";
                                        selectedDay.JanuarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.JanuaryScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.JanuarySecondEllipse = "S";
                                        selectedDay.JanuarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.JanuaryScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.JanuarySecondEllipse = "A";
                                        selectedDay.JanuarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.JanuaryCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 2:
                            if (selectedDay.FebruaryScheduleEvents == null)
                                selectedDay.FebruaryScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.FebruaryScheduleEvents.Add(eve);
                            selectedDay.FebruaryData = eve.Observations;
                            if (selectedDay.FebruaryScheduleEvents.Count > 0)
                            {
                                selectedDay.FebruaryCircleVisibility = Visibility.Visible;
                                selectedDay.FebruaryScheduleEvents = selectedDay.FebruaryScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.FebruaryScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.FebruaryFirstEllipse = "G";
                                    selectedDay.FebruaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.FebruaryScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.FebruaryFirstEllipse = "S";
                                    selectedDay.FebruaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.FebruaryScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.FebruaryFirstEllipse = "A";
                                    selectedDay.FebruaryFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.FebruaryScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.FebruaryScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.FebruarySecondEllipse = "G";
                                        selectedDay.FebruarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.FebruaryScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.FebruarySecondEllipse = "S";
                                        selectedDay.FebruarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.FebruaryScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.FebruarySecondEllipse = "A";
                                        selectedDay.FebruarySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.FebruaryCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 3:
                            if (selectedDay.MarchScheduleEvents == null)
                                selectedDay.MarchScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.MarchScheduleEvents.Add(eve);
                            selectedDay.MarchData = eve.Observations;
                            if (selectedDay.MarchScheduleEvents.Count > 0)
                            {
                                selectedDay.MarchCircleVisibility = Visibility.Visible;
                                selectedDay.MarchScheduleEvents = selectedDay.MarchScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.MarchScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.MarchFirstEllipse = "G";
                                    selectedDay.MarchFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.MarchScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.MarchFirstEllipse = "S";
                                    selectedDay.MarchFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.MarchScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.MarchFirstEllipse = "A";
                                    selectedDay.MarchFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.MarchScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.MarchScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.MarchSecondEllipse = "G";
                                        selectedDay.MarchSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.MarchScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.MarchSecondEllipse = "S";
                                        selectedDay.MarchSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.MarchScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.MarchSecondEllipse = "A";
                                        selectedDay.MarchSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.MarchCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 4:
                            if (selectedDay.AprilScheduleEvents == null)
                                selectedDay.AprilScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.AprilScheduleEvents.Add(eve);
                            selectedDay.AprilData = eve.Observations;
                            if (selectedDay.AprilScheduleEvents.Count > 0)
                            {
                                selectedDay.AprilCircleVisibility = Visibility.Visible;
                                selectedDay.AprilScheduleEvents = selectedDay.AprilScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.AprilScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.AprilFirstEllipse = "G";
                                    selectedDay.AprilFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.AprilScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.AprilFirstEllipse = "S";
                                    selectedDay.AprilFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.AprilScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.AprilFirstEllipse = "A";
                                    selectedDay.AprilFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.AprilScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.AprilScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.AprilSecondEllipse = "G";
                                        selectedDay.AprilSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.AprilScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.AprilSecondEllipse = "S";
                                        selectedDay.AprilSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.AprilScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.AprilSecondEllipse = "A";
                                        selectedDay.AprilSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.AprilCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 5:
                            if (selectedDay.MayScheduleEvents == null)
                                selectedDay.MayScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.MayScheduleEvents.Add(eve);
                            selectedDay.MayData = eve.Observations;
                            if (selectedDay.MayScheduleEvents.Count > 0)
                            {
                                selectedDay.MayCircleVisibility = Visibility.Visible;
                                selectedDay.MayScheduleEvents = selectedDay.MayScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.MayScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.MayFirstEllipse = "G";
                                    selectedDay.MayFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.MayScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.MayFirstEllipse = "S";
                                    selectedDay.MayFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.MayScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.MayFirstEllipse = "A";
                                    selectedDay.MayFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.MayScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.MayScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.MaySecondEllipse = "G";
                                        selectedDay.MaySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.MayScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.MaySecondEllipse = "S";
                                        selectedDay.MaySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.MayScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.MaySecondEllipse = "A";
                                        selectedDay.MaySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.MayCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 6:
                            if (selectedDay.JuneScheduleEvents == null)
                                selectedDay.JuneScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.JuneScheduleEvents.Add(eve);
                            selectedDay.JuneData = eve.Observations;
                            if (selectedDay.JuneScheduleEvents.Count > 0)
                            {
                                selectedDay.JuneCircleVisibility = Visibility.Visible;
                                selectedDay.JuneScheduleEvents = selectedDay.JuneScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.JuneScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.JuneFirstEllipse = "G";
                                    selectedDay.JuneFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.JuneScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.JuneFirstEllipse = "S";
                                    selectedDay.JuneFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.JuneScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.JuneFirstEllipse = "A";
                                    selectedDay.JuneFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.JuneScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.JuneScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.JuneSecondEllipse = "G";
                                        selectedDay.JuneSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.JuneScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.JuneSecondEllipse = "S";
                                        selectedDay.JuneSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.JuneScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.JuneSecondEllipse = "A";
                                        selectedDay.JuneSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.JuneCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 7:
                            if (selectedDay.JulyScheduleEvents == null)
                                selectedDay.JulyScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.JulyScheduleEvents.Add(eve);
                            selectedDay.JulyData = eve.Observations;
                            if (selectedDay.JulyScheduleEvents.Count > 0)
                            {
                                selectedDay.JulyCircleVisibility = Visibility.Visible;
                                selectedDay.JulyScheduleEvents = selectedDay.JulyScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.JulyScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.JulyFirstEllipse = "G";
                                    selectedDay.JulyFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.JulyScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.JulyFirstEllipse = "S";
                                    selectedDay.JulyFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.JulyScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.JulyFirstEllipse = "A";
                                    selectedDay.JulyFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.JulyScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.JulyScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.JulySecondEllipse = "G";
                                        selectedDay.JulySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.JulyScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.JulySecondEllipse = "S";
                                        selectedDay.JulySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.JulyScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.JulySecondEllipse = "A";
                                        selectedDay.JulySecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.JulyCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 8:
                            if (selectedDay.AugustScheduleEvents == null)
                                selectedDay.AugustScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.AugustScheduleEvents.Add(eve);
                            selectedDay.AugustData = eve.Observations;
                            if (selectedDay.AugustScheduleEvents.Count > 0)
                            {
                                selectedDay.AugustCircleVisibility = Visibility.Visible;
                                selectedDay.AugustScheduleEvents = selectedDay.AugustScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.AugustScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.AugustFirstEllipse = "G";
                                    selectedDay.AugustFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.AugustScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.AugustFirstEllipse = "S";
                                    selectedDay.AugustFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.AugustScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.AugustFirstEllipse = "A";
                                    selectedDay.AugustFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.AugustScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.AugustScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.AugustSecondEllipse = "G";
                                        selectedDay.AugustSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.AugustScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.AugustSecondEllipse = "S";
                                        selectedDay.AugustSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.AugustScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.AugustSecondEllipse = "A";
                                        selectedDay.AugustSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.AugustCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 9:
                            if (selectedDay.SeptemberScheduleEvents == null)
                                selectedDay.SeptemberScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.SeptemberScheduleEvents.Add(eve);
                            selectedDay.SeptemberData = eve.Observations;
                            if (selectedDay.SeptemberScheduleEvents.Count > 0)
                            {
                                selectedDay.SeptemberCircleVisibility = Visibility.Visible;
                                selectedDay.SeptemberScheduleEvents = selectedDay.SeptemberScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.SeptemberScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.SeptemberFirstEllipse = "G";
                                    selectedDay.SeptemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.SeptemberScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.SeptemberFirstEllipse = "S";
                                    selectedDay.SeptemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.SeptemberScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.SeptemberFirstEllipse = "A";
                                    selectedDay.SeptemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.SeptemberScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.SeptemberScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.SeptemberSecondEllipse = "G";
                                        selectedDay.SeptemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.SeptemberScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.SeptemberSecondEllipse = "S";
                                        selectedDay.SeptemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.SeptemberScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.SeptemberSecondEllipse = "A";
                                        selectedDay.SeptemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.SeptemberCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 10:
                            if (selectedDay.OctoberScheduleEvents == null)
                                selectedDay.OctoberScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.OctoberScheduleEvents.Add(eve);
                            selectedDay.OctoberData = eve.Observations;
                            if (selectedDay.OctoberScheduleEvents.Count > 0)
                            {
                                selectedDay.OctoberCircleVisibility = Visibility.Visible;
                                selectedDay.OctoberScheduleEvents = selectedDay.OctoberScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.OctoberScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.OctoberFirstEllipse = "G";
                                    selectedDay.OctoberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.OctoberScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.OctoberFirstEllipse = "S";
                                    selectedDay.OctoberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.OctoberScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.OctoberFirstEllipse = "A";
                                    selectedDay.OctoberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.OctoberScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.OctoberScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.OctoberSecondEllipse = "G";
                                        selectedDay.OctoberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.OctoberScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.OctoberSecondEllipse = "S";
                                        selectedDay.OctoberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.OctoberScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.OctoberSecondEllipse = "A";
                                        selectedDay.OctoberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.OctoberCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 11:
                            if (selectedDay.NovemberScheduleEvents == null)
                                selectedDay.NovemberScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.NovemberScheduleEvents.Add(eve);
                            selectedDay.NovemberData = eve.Observations;
                            if (selectedDay.NovemberScheduleEvents.Count > 0)
                            {
                                selectedDay.NovemberCircleVisibility = Visibility.Visible;
                                selectedDay.NovemberScheduleEvents = selectedDay.NovemberScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.NovemberScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.NovemberFirstEllipse = "G";
                                    selectedDay.NovemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.NovemberScheduleEvents[0].IdLogistic == 1633)
                                {
                                    selectedDay.NovemberFirstEllipse = "S";
                                    selectedDay.NovemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.NovemberScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.NovemberFirstEllipse = "A";
                                    selectedDay.NovemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.NovemberScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.NovemberScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.NovemberSecondEllipse = "G";
                                        selectedDay.NovemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.NovemberScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.NovemberSecondEllipse = "S";
                                        selectedDay.NovemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.NovemberScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.NovemberSecondEllipse = "A";
                                        selectedDay.NovemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.NovemberCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                        case 12:
                            if (selectedDay.DecemberScheduleEvents == null)
                                selectedDay.DecemberScheduleEvents = new List<ScheduleEvent>();
                            selectedDay.DecemberScheduleEvents.Add(eve);
                            selectedDay.DecemberData = eve.Observations;
                            if (selectedDay.DecemberScheduleEvents.Count > 0)
                            {
                                selectedDay.DecemberCircleVisibility = Visibility.Visible;
                                selectedDay.DecemberScheduleEvents = selectedDay.DecemberScheduleEvents.OrderByDescending(x => x.IdLogistic).ToList();
                                if (selectedDay.DecemberScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.DecemberFirstEllipse = "G";
                                    selectedDay.DecemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                }
                                else if (selectedDay.DecemberScheduleEvents[0].IdLogistic == 1634)
                                {
                                    selectedDay.DecemberFirstEllipse = "S";
                                    selectedDay.DecemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                }
                                else if (selectedDay.DecemberScheduleEvents[0].IdLogistic == 1632)
                                {
                                    selectedDay.DecemberFirstEllipse = "A";
                                    selectedDay.DecemberFirstEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                }
                                if (selectedDay.DecemberScheduleEvents.Count > 1)
                                {
                                    if (selectedDay.DecemberScheduleEvents[1].IdLogistic == 1634)
                                    {
                                        selectedDay.DecemberSecondEllipse = "G";
                                        selectedDay.DecemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B97A57");
                                    }
                                    else if (selectedDay.DecemberScheduleEvents[1].IdLogistic == 1633)
                                    {
                                        selectedDay.DecemberSecondEllipse = "S";
                                        selectedDay.DecemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00A2E8");
                                    }
                                    else if (selectedDay.DecemberScheduleEvents[1].IdLogistic == 1632)
                                    {
                                        selectedDay.DecemberSecondEllipse = "A";
                                        selectedDay.DecemberSecondEllipseHtmlColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#99D9EA");
                                    }
                                }
                            }
                            else
                            {
                                selectedDay.DecemberCircleVisibility = Visibility.Collapsed;
                            }
                            break;
                    }
                }
                RefreshMonthlyData();
                GeosApplication.Instance.Logger.Log("Method FillScheduleEvents()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillScheduleEvents()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][22.09.2025][GEOS2-8795]
        private void FillEventTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEventTypeList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temptypeList = CrmService.GetLookupValues(187);
                EventTypeList = new ObservableCollection<LookupValue>(temptypeList);
                GeosApplication.Instance.Logger.Log("Method FillEventTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEventTypeList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEventTypeList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEventTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

    }
}
