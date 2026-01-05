using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Spreadsheet;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[001][Ranjana Dixit][GEOS2-3627][18.04.2022] 
    class StockBySupplierGridViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Service

        IWarehouseService WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WmsService = new WarehouseServiceController("localhost:6699");

        #endregion

        #region Declaration
        bool isBusy;
        public bool istrue = true;
        private ObservableCollection<StockBySupplier> stockbysupplierList = new ObservableCollection<StockBySupplier>();
        private string filterString;
        private Warehouses selectedStockBySupplierwarehouse;
        Currency selectedCurrency;
        private bool isValueColumnVisible;
        //public int totalQty;
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public string FilterString
        {
            get { return filterString; }
            set { filterString = value; OnPropertyChanged(new PropertyChangedEventArgs("FilterString")); }

        }

        public bool  IsLoadedServiceMethodData { get; set; }
        public virtual string ResultFileName { get; set; }
        public ObservableCollection<StockBySupplier> StockbysupplierList
        {
            get
            {
                return stockbysupplierList;
            }

            set
            {
                stockbysupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StockbysupplierList"));
            }
        }

        //public Int32 TotalQty
        //{
        //    get
        //    {
        //        return totalQty;
        //    }

        //    set
        //    {
        //        totalQty = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TotalQty"));
        //    }
        //}

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

        public bool IsValueColumnVisible
        {
            get { return isValueColumnVisible; }
            set
            {
                isValueColumnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsValueColumnVisible"));
            }
        }
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

        #endregion // End Of Events 

        #region Event Handlers
        //[001][Ranjana Dixit][GEOS2-3627][13.04.2022]
        public event EventHandler RequestClose;
        public event EventHandler ComboBox_SelectionChanged;
        #endregion

        #region Icommands
        public ICommand RefreshStockSupplierCommand { get; set; }
        public ICommand PrintStockSupplierCommand { get; set; }
        public ICommand ExportStockSupplierCommand { get; set; }
        public ICommand WarehouseViewReferenceHyperlinkClickCommand { get; private set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        #endregion

        #region Constructor
        public StockBySupplierGridViewModel()
        {
            try
            {
                //FillStockBySupplier1(ws);
                GeosApplication.Instance.Logger.Log("Constructor StockBySupplierViewModel....", category: Category.Info, priority: Priority.Low);
                PrintStockSupplierCommand = new RelayCommand(new Action<object>(PrintStockSupplierAction));
                ExportStockSupplierCommand = new RelayCommand(new Action<object>(ExportStockSupplierAction));
                RefreshStockSupplierCommand = new RelayCommand(new Action<object>(RefreshStockSupplierAction));
                SelectedItemChangedCommand = new DelegateCommand<SelectionChangedEventArgs>(cmb_SelectionChanged);
                WarehouseViewReferenceHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseViewReferenceHyperlinkClickCommandAction);      
                    FilterString = string.Empty;
                    WarehouseCommon.Instance.IsRegionalWarehouseList = WarehouseCommon.Instance.WarehouseList.Where(i => i.IsRegional == 1).ToList();
                    if (WarehouseCommon.Instance.IsRegionalWarehouseList.Count > 1)
                    {
                        WarehouseCommon.Instance.SelectedStockBySupplierwarehouse = null; 
                    }
                    else
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
                        WarehouseCommon.Instance.SelectedStockBySupplierwarehouse = WarehouseCommon.Instance.IsRegionalWarehouseList.FirstOrDefault();
                        FillStockBySupplier();
                     
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    }
                }
                GeosApplication.Instance.Logger.Log("Constructor StockBySupplierViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StockBySupplierViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method      
        public void FillStockBySupplier()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method FillStockBySupplier", category: Category.Info, priority: Priority.Low);
                if(WarehouseCommon.Instance.SelectedStockBySupplierwarehouse!=null)
                {
                    if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 51 && up.Permission.IdGeosModule == 6))
                    {
                        IsValueColumnVisible = true;
                        List<Currency> CurrencyList = GeosApplication.Instance.Currencies.ToList();
                        SelectedCurrency = CurrencyList.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                        //[Sudhir.Jangra][GEOS2-5486]
                        ObservableCollection<StockBySupplier> temp = new ObservableCollection<StockBySupplier>(WmsService.GetStockBySupplier_V2510(WarehouseCommon.Instance.SelectedStockBySupplierwarehouse, SelectedCurrency));
                        for (int i = 0; i < temp.Count; i++)
                        {
                            temp[i].ValueWithSelectedCurrencySymbol = string.Format($"{temp[i].Price.ToString("n2", CultureInfo.CurrentCulture)} {SelectedCurrency.Symbol}");
                        }
                        StockbysupplierList = new ObservableCollection<StockBySupplier>(temp);
                    }
                    else
                    {
                        IsValueColumnVisible = false;
                        StockbysupplierList = new ObservableCollection<StockBySupplier>(WmsService.GetStockBySupplier(WarehouseCommon.Instance.SelectedStockBySupplierwarehouse));
                    }
                }

               


                GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method FillStockBySupplier executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStockBySupplier() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStockBySupplier() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupplierComplaints() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void PrintStockSupplierAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintStockSupplierAction()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ManagementOrderPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ManagementOrderPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method PrintStockSupplierAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintStockSupplierAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void ExportStockSupplierAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method ExportStockSupplierAction()...", category: Category.Info, priority: Priority.Low);
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
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Stock By Supplier List";
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
                    GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method ExportStockSupplierAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in StockBySupplierGridViewModel Method ExportStockSupplierAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void RefreshStockSupplierAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method RefreshStockSupplierAction()....", category: Category.Info, priority: Priority.Low);
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
                FilterString = string.Empty;
                FillStockBySupplier();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StockBySupplierGridViewModel RefreshStockSupplierAction method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel RefreshStockSupplierAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void cmb_SelectionChanged(SelectionChangedEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel Method cmb_SelectionChanged()....", category: Category.Info, priority: Priority.Low);

            try
            {
                if (WarehouseCommon.Instance.SelectedStockBySupplierwarehouse != null)
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
                    FilterString = string.Empty;
                    FillStockBySupplier();
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
            }               
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StockBySupplierGridViewModel cmb_SelectionChanged method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
              GeosApplication.Instance.Logger.Log("StockBySupplierGridViewModel cmb_SelectionChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
        public void WarehouseViewReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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
                StockBySupplier ABSupplier = (StockBySupplier)detailView.DataControl.CurrentItem;
                int index = detailView.FocusedRowHandle;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                //Warehouses warehouse = WarehouseCommon.Instance.SelectedStockBySupplierwarehouse;
                //var WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.InitFromStockBySupplier(ABSupplier.Reference, WarehouseCommon.Instance.SelectedStockBySupplierwarehouse);
                bool isPermissionEnabledInWarehouseCommon = WarehouseCommon.Instance.IsPermissionEnabled;
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
