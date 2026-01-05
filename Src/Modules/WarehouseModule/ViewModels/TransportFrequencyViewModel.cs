using DevExpress.Data.Filtering;
using DevExpress.DataProcessing;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI.Internal;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
//[nsatpute][GEOS2-9362][17.11.2025]
namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[nsatpute][18.11.2025][GEOS2-9364]
    public class TransportFrequencyViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("10.13.3.33:99");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region Icommands
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand SaveButtonCommand { get; set; }
        #endregion
        #region Constructor
        public TransportFrequencyViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TransportFrequencyViewModel ...", category: Category.Info, priority: Priority.Low);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
                SaveButtonCommand = new RelayCommand(new Action<object>(SaveButtonCommandAction));
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor TransportFrequencyViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor TransportFrequencyViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Private Members
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        private bool isBusy;
        private int local;
        private int ground;
        private int sea;
        private int air;
        private ObservableCollection<TransportFrequency> listFrequencies;
        private ObservableCollection<TransportFrequency> listFrequenciesCloned;
        private ObservableCollection<TransportFrequency> selectedObject;
        private ObservableCollection<LogEntriesByTransportFrequency> listLogEntries;
        private bool? isAllSelected;
        private bool isUpdatingFromSelection = false;
        private TransportFrequency summarySitePlant;
        private string summaryFrequencies;
        private bool isValueChanged;
        #endregion

        #region Properties


        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        public bool? IsAllSelected
        {
            get { return isAllSelected; }
            set
            {
                if (isAllSelected != value)
                {
                    if (value == null)
                        value = false;
                    isAllSelected = value;
                    OnPropertyChanged(nameof(IsAllSelected));

                    // Only execute SelectAll if the change came from user interaction
                    // AND it's a definite true/false value (not indeterminate)
                    if (!isUpdatingFromSelection && value.HasValue)
                    {
                        SelectAll(value.Value);
                    }
                }
            }
        }
        public bool IsValueChanged
        {
            get { return isValueChanged; }
            set
            {
                isValueChanged = value;
                OnPropertyChanged("IsValueChanged");
            }
        }
        public ObservableCollection<TransportFrequency> ListFrequencies
        {
            get { return listFrequencies; }
            set
            {
                listFrequencies = value;
                OnPropertyChanged("ListFrequencies");
            }
        }
        //[nsatpute][19.11.2025][GEOS2-9371]
        public ObservableCollection<TransportFrequency> ListFrequenciesCloned
        {
            get { return listFrequenciesCloned; }
            set
            {
                listFrequenciesCloned = value;
                OnPropertyChanged("ListFrequenciesCloned");
            }
        }
        public ObservableCollection<LogEntriesByTransportFrequency> ListLogEntries
        {
            get { return listLogEntries; }
            set
            {
                listLogEntries = value;
                OnPropertyChanged("ListLogEntries");
            }
        }
        public ObservableCollection<TransportFrequency> SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged("SelectedObject");
            }
        }
        public TransportFrequency SummarySitePlant
        {
            get { return summarySitePlant; }
            set
            {
                summarySitePlant = value;
                OnPropertyChanged("SummarySitePlant");
            }
        }
        public string SummaryFrequencies
        {
            get { return summaryFrequencies; }
            set
            {
                summaryFrequencies = value;
                OnPropertyChanged("SummaryFrequencies");
            }
        }
        private ObservableCollection<int> localValues;
        public ObservableCollection<int> LocalValues
        {
            get { return localValues; }
            set
            {
                localValues = value;
                OnPropertyChanged("LocalValues");
            }
        }
        private ObservableCollection<int> groundValues;
        public ObservableCollection<int> GroundValues
        {
            get { return groundValues; }
            set
            {
                groundValues = value;
                OnPropertyChanged("GroundValues");
            }
        }
        private ObservableCollection<int> seaValues;
        public ObservableCollection<int> SeaValues
        {
            get { return seaValues; }
            set
            {
                seaValues = value;
                OnPropertyChanged("SeaValues");
            }
        }
        private ObservableCollection<int> airValues;
        public ObservableCollection<int> AirValues
        {
            get { return airValues; }
            set
            {
                airValues = value;
                OnPropertyChanged("AirValues");
            }
        }

        #endregion
        #region Methods
        private void RefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["Transportfrequencyyearviewminimized_Doyouwanttosavechangesbe"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SaveButtonCommandAction(null);
                    }
                    IsValueChanged = false;
                }
                Init();
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in RefreshButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Transport_Frequency_Year_Report";
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

                    TableView tblStockHistory = ((TableView)obj);
                    tblStockHistory.ShowTotalSummary = false;
                    tblStockHistory.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    tblStockHistory.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    tblStockHistory.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                /*
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TransportFrequencyReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TransportFrequencyReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                */

                // Create the printable link for your control
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TransportFrequencyReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TransportFrequencyReportPrintFooterTemplate"];
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                // === Configure page settings ===
                pcl.PaperKind = System.Drawing.Printing.PaperKind.Custom;

                // Custom paper size is in HUNDREDTHS of an inch
                pcl.CustomPaperSize = new System.Drawing.Size(800, 1135);

                pcl.Landscape = true;

                // Create the document (MUST be called before preview)
                pcl.CreateDocument();

                // === Show preview window ===
                DocumentPreviewWindow window = new DocumentPreviewWindow()
                {
                    WindowState = WindowState.Maximized
                };

                // Assign the document to the preview control
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                // === Optional: set default printer ===
                var printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName =
                    GeosApplication.Instance.UserSettings["SelectedPrinter"];


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SaveButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

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
                //[nsatpute][24.11.2025][GEOS2-9367]
                List<TransportFrequency> modifiedRecords = null;
                if (ListFrequencies.Any(x => x.IsSelected))
                {
                    modifiedRecords = ListFrequencies.Where(x => x.IsSelected).ToList();
                }
                else
                {
                    modifiedRecords = ListFrequencies
                    .Where(current =>
                    {
                        var original = ListFrequenciesCloned.FirstOrDefault(x => x.IdCompany == current.IdCompany);
                        return original != null && !current.Equals(original);
                    })
                    .ToList();
                }
                if (modifiedRecords.Count > 0)
                {
                    DateTime changeLogDatetime = DateTime.Now;
                    List<LogEntriesByTransportFrequency> lstNewLogs = new List<LogEntriesByTransportFrequency>();
                    foreach (TransportFrequency tf in modifiedRecords)
                    {
                        TransportFrequency originalTf = ListFrequenciesCloned.FirstOrDefault(x => x.IdCompany == tf.IdCompany);
                        if (originalTf == null)
                        {
                            LogEntriesByTransportFrequency logLocal = new LogEntriesByTransportFrequency();
                            logLocal.ChangeLogDateTime = changeLogDatetime;
                            logLocal.Comment = $"Local frequency for plant {tf.Name} has updated from 0 to {tf.Local.ToString()}";
                            logLocal.IdUser = GeosApplication.Instance.ActiveUser.IdUser;                            
                            lstNewLogs.Add(logLocal);

                            LogEntriesByTransportFrequency logGround = new LogEntriesByTransportFrequency();
                            logGround.ChangeLogDateTime = changeLogDatetime;
                            logGround.Comment = $"Ground frequency for plant {tf.Name} has updated from 0 to {tf.Ground.ToString()}";
                            logGround.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            lstNewLogs.Add(logLocal);

                            LogEntriesByTransportFrequency logSea = new LogEntriesByTransportFrequency();
                            logSea.ChangeLogDateTime = changeLogDatetime;
                            logSea.Comment = $"Sea frequency for plant {tf.Name} has updated from 0 to {tf.Sea.ToString()}";
                            logSea.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            lstNewLogs.Add(logSea);
                            LogEntriesByTransportFrequency logAir = new LogEntriesByTransportFrequency();
                            logAir.ChangeLogDateTime = changeLogDatetime;
                            logAir.Comment = $"Air frequency for plant {tf.Name} has updated from 0 to {tf.Air.ToString()}";
                            logAir.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            lstNewLogs.Add(logAir);
                        }
                        else
                        {
                            if (originalTf.Local != tf.Local)
                            {
                                LogEntriesByTransportFrequency logLocal = new LogEntriesByTransportFrequency();
                                logLocal.ChangeLogDateTime = changeLogDatetime;
                                logLocal.Comment = $"Local frequency for plant {tf.Name} has updated from {originalTf.Local.ToString()} to {tf.Local.ToString()}";
                                logLocal.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                lstNewLogs.Add(logLocal);
                            }
                            if (originalTf.Ground != tf.Ground)
                            {
                                LogEntriesByTransportFrequency logGround = new LogEntriesByTransportFrequency();
                                logGround.ChangeLogDateTime = changeLogDatetime;
                                logGround.Comment = $"Ground frequency for plant {tf.Name} has updated from {originalTf.Ground.ToString()} to {tf.Ground.ToString()}";
                                logGround.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                lstNewLogs.Add(logGround);
                            }
                            if (originalTf.Sea != tf.Sea)
                            {
                                LogEntriesByTransportFrequency logSea = new LogEntriesByTransportFrequency();
                                logSea.ChangeLogDateTime = changeLogDatetime;
                                logSea.Comment = $"Sea frequency for plant {tf.Name} has updated from  {originalTf.Sea.ToString()}  to {tf.Sea.ToString()}";
                                logSea.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                lstNewLogs.Add(logSea);
                            }
                            if (originalTf.Air != tf.Air)
                            {
                                LogEntriesByTransportFrequency logAir = new LogEntriesByTransportFrequency();
                                logAir.ChangeLogDateTime = changeLogDatetime;
                                logAir.Comment = $"Air frequency for plant {tf.Name} has updated from  {originalTf.Air.ToString()}  to {tf.Air.ToString()}";
                                logAir.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                lstNewLogs.Add(logAir);
                            }
                        }
                    }
                    WarehouseService.SaveSiteTransportFrequencies(modifiedRecords.ToList());
                    WarehouseService.SaveLogEntrieByTransportFrequency(lstNewLogs);
                    CustomMessageBox.Show(Application.Current.FindResource("Transportfrequencyyearview_Transportfrequenciessavedsu").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);                    
                    FillFrequencies();
                    FillChangeLog();
                    IsValueChanged = false;
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.FindResource("Transportfrequencyyearview_Nochangestosave").ToString(), "Blue", CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Save - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in SaveButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
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
                FillFrequenciesForCombobox();
                FillFrequencies();
                FillChangeLog();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24.11.2025][GEOS2-9367]
        private void FillFrequencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFrequencies ...", category: Category.Info, priority: Priority.Low);
                // WarehouseService = new WarehouseServiceController("localhost:6699");
                ListFrequencies = new ObservableCollection<TransportFrequency>(WarehouseService.GetSiteTransportFrequencies());
                if (ListFrequencies is INotifyCollectionChanged observableCollection)
                {
                    observableCollection.CollectionChanged += (s, e) => UpdateSelectAllState();
                }
                if (ListFrequencies != null)
                {
                    foreach (var item in ListFrequencies)
                    {
                        if (item is INotifyPropertyChanged notifyItem)
                        {
                            notifyItem.PropertyChanged += (s, e) =>
                            {
                                if (e.PropertyName == "Local" || e.PropertyName == "Ground" || e.PropertyName == "Sea" || e.PropertyName == "Air")
                                {
                                    IsValueChanged = true;
                                }
                                if (e.PropertyName == nameof(item.IsSelected))
                                {
                                    UpdateSelectAllState();
                                }
                            };
                        }
                    }
                }
                ListFrequenciesCloned = new ObservableCollection<TransportFrequency>();
                ListFrequencies.ToList().ForEach(x => ListFrequenciesCloned.Add((TransportFrequency)x.Clone()));
                IsAllSelected = false;

                SummarySitePlant = ListFrequencies.FirstOrDefault(x => x.IdCompany == WarehouseCommon.Instance.Selectedwarehouse.Company.IdCompany);
                var frequencies = new List<string>();
                if (SummarySitePlant.Air > 0) frequencies.Add("Air");
                if (SummarySitePlant.Sea > 0) frequencies.Add("Sea");
                if (SummarySitePlant.Ground > 0) frequencies.Add("Ground");
                if (SummarySitePlant.Local > 0) frequencies.Add("Local");

                SummaryFrequencies = string.Join("/", frequencies);
                GeosApplication.Instance.Logger.Log("Method FillFrequencies() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFrequencies() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFrequencies() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        //[nsatpute][24.11.2025][GEOS2-9367]

        private void FillFrequenciesForCombobox()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFrequenciesForCombobox ...", category: Category.Info, priority: Priority.Low);
                List<GeosAppSetting> lstGeosAppSetting = WorkbenchStartUp.GetSelectedGeosAppSettings("174");
                if (lstGeosAppSetting != null && lstGeosAppSetting.Count > 0)
                    UpdateTransportValues(lstGeosAppSetting[0].DefaultValue);
                GeosApplication.Instance.Logger.Log("Method FillFrequenciesForCombobox() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFrequenciesForCombobox() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFrequenciesForCombobox() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        public void UpdateTransportValues(string input)
        {
            var parsedData = ParseValues(input);

            foreach (var item in parsedData)
            {
                switch (item.Key)
                {
                    case 1632:
                        AirValues = item.Value;
                        break;
                    case 1633:
                        SeaValues = item.Value;
                        break;
                    case 1634:
                        GroundValues = item.Value;
                        break;
                    case 0:
                        LocalValues = item.Value;
                        break;
                }
            }
        }
        private Dictionary<int, ObservableCollection<int>> ParseValues(string input)
        {
            var result = new Dictionary<int, ObservableCollection<int>>();

            // Remove parentheses and split by "),("
            var pairs = input.Trim('(', ')').Split(new[] { "),(" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                // Split key and values by colon
                var parts = pair.Split(':');
                if (parts.Length == 2)
                {
                    int key = int.Parse(parts[0]);

                    // Split values by comma and parse to integers, then create ObservableCollection
                    var values = new ObservableCollection<int>(
                        parts[1].Split(',')
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(int.Parse)
                            .ToList()
                    );

                    result[key] = values;
                }
            }

            return result;
        }
        //[nsatpute][24.11.2025][GEOS2-9367]
        private void FillChangeLog()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillChangeLog ...", category: Category.Info, priority: Priority.Low);
                ListLogEntries = new ObservableCollection<LogEntriesByTransportFrequency>(WarehouseService.GetLogEntriesByTransportFrequency());
                GeosApplication.Instance.Logger.Log("Method FillChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillChangeLog() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        //[nsatpute][24.11.2025][GEOS2-9367]
        private void SelectAll(bool select)
        {
            isUpdatingFromSelection = true;

            try
            {
                // Update both collections
                if (ListFrequencies != null)
                {
                    foreach (var item in ListFrequencies)
                    {
                        item.IsSelected = select;
                    }
                }

                if (ListFrequenciesCloned != null)
                {
                    foreach (var item in ListFrequenciesCloned)
                    {
                        item.IsSelected = select;
                    }
                }
            }
            finally
            {
                isUpdatingFromSelection = false;
            }
        }
        //[nsatpute][24.11.2025][GEOS2-9367]
        private void UpdateSelectAllState()
        {
            if (isUpdatingFromSelection) return;

            isUpdatingFromSelection = true;

            try
            {
                if (ListFrequencies == null || ListFrequencies.Count == 0)
                {
                    isAllSelected = false;
                }
                else
                {
                    int selectedCount = ListFrequencies.Count(item => item.IsSelected);
                    int totalCount = ListFrequencies.Count;

                    // Three visual states, but only two click actions
                    if (selectedCount == 0)
                    {
                        isAllSelected = false;        // Unchecked - no items selected
                    }
                    else if (selectedCount == totalCount)
                    {
                        isAllSelected = true;         // Checked - all items selected
                    }
                    else
                    {
                        isAllSelected = null;         // Indeterminate - some items selected
                    }
                }

                OnPropertyChanged(nameof(IsAllSelected));
            }
            finally
            {
                isUpdatingFromSelection = false;
            }
        }
        #endregion

        #region Event Handlers        
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
