using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class ODNViewModel : INotifyPropertyChanged, IDisposable
    {
        #region TaskLog
        // WMS M057-14	Add new section pending ODN [adadibathina]
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

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

        #region Declaration
        private ObservableCollection<SupplierComplaint> supplierComplaintsList = new ObservableCollection<SupplierComplaint>();
        private string filterString;
        private bool isBusy;
        #endregion

        #region Properties

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public ObservableCollection<SupplierComplaint> SupplierComplaintsList
        {
            get { return supplierComplaintsList; }
            set { supplierComplaintsList = value; OnPropertyChanged(new PropertyChangedEventArgs("SupplierComplaintsList")); }
        }

        public string FilterString
        {
            get { return filterString; }
            set { filterString = value; OnPropertyChanged(new PropertyChangedEventArgs("FilterString")); }

        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(new PropertyChangedEventArgs("IsBusy")); }
        }

        //  public bool 

        #endregion

        #region ICommands

        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshPickingOdnViewCommand { get; set; }
        public ICommand PrintPickingOdnViewCommand { get; set; }
        public ICommand ExportPickingOdnViewCommand { get; set; }
        public ICommand ScanPickingOdnCommand { get; set; }

        #endregion //End Of Icommand

        #region Constructor 
        public ODNViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ODNViewModel() ", category: Category.Info, priority: Priority.Low);
                //  RelayCommand
                RefreshPickingOdnViewCommand = new RelayCommand(new Action<object>(OdnViewModelRefresh));
                ScanPickingOdnCommand = new RelayCommand(new Action<object>(Scan));
                CommandWarehouseEditValueChanged = new RelayCommand(new Action<object>(WarehouseEditValueChangedCommandAction));
                PrintPickingOdnViewCommand = new RelayCommand(new Action<object>(OdnViewModelPrint));
                ExportPickingOdnViewCommand = new RelayCommand(new Action<object>(OdnViewModelExport));
                FillSupplierComplaints();

                GeosApplication.Instance.Logger.Log("Constructor ODNViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods


        public void FillSupplierComplaints()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ODNViewModel Method FillSupplierComplaints", category: Category.Info, priority: Priority.Low);
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
                SupplierComplaintsList = new ObservableCollection<SupplierComplaint>(WarehouseService.GetSupplierComplaints(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString()));
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("ODNViewModel Method FillSupplierComplaints executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierComplaints() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierComplaints() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierComplaints() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("ODNViewModel WarehouseEditValueChangedCommandAction....", category: Category.Info, priority: Priority.Low);
            try

            {//When setting the warehouse from default the data should not be refreshed
                if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                    return;
                FilterString = string.Empty;
                OdnViewModelRefresh(new object());

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel WarehouseEditValueChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("ODNViewModel WarehouseEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void OdnViewModelRefresh(object obj)
        {
            GeosApplication.Instance.Logger.Log("ODNViewModel Refresh....", category: Category.Info, priority: Priority.Low);
            try
            {
                FilterString = string.Empty;
                FillSupplierComplaints();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel OdnViewModelRefresh method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("ODNViewModel Refresh() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void OdnViewModelPrint(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ODNViewModel Method OdnViewModelPrint()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ODNReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ODNReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OdnViewModelPrint()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel Method OdnViewModelPrint()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OdnViewModelExport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ODNViewModel Method OdnViewModelExport()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending ODN List";
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
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("ODNViewModel Method OdnViewModelExport()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel Method OdnViewModelExport()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Scan(object obj)
        {
            GeosApplication.Instance.Logger.Log("ODNViewModel Scan....", category: Category.Info, priority: Priority.Low);
            try
            {
                PickingODNScanView pickingOdnScanView = new PickingODNScanView();
                PickingODNScanViewModel pickingOdnScanViewModel = new PickingODNScanViewModel();
                pickingOdnScanViewModel.Init(SupplierComplaintsList);
                EventHandler handler = delegate { pickingOdnScanView.Close(); };
                pickingOdnScanViewModel.RequestClose += handler;
                pickingOdnScanView.DataContext = pickingOdnScanViewModel;
                var ownerInfo = (obj as FrameworkElement);
                pickingOdnScanView.Owner = Window.GetWindow(ownerInfo);
                pickingOdnScanView.ShowDialogWindow();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ODNViewModel Scan() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("ODNViewModel Scan() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose ODNViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose ODNViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

    }
}
