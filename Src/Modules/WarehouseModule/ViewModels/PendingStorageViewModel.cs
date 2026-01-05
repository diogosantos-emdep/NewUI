using DevExpress.Export.Xl;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PendingStorageViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private List<PendingStorageArticles> pendingStorageArticlesList;
        private List<PendingStorageArticles> pendingStorageArticlesGridList;

        private bool isBusy;
        private string myFilterString;
        private PendingStorageArticles selectedObject;

        #endregion // Declaration

        #region Properties

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
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

        public List<PendingStorageArticles> PendingStorageArticlesList
        {
            get { return pendingStorageArticlesList; }
            set
            {
                pendingStorageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageArticlesList"));
            }
        }

        public PendingStorageArticles SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }

        public List<PendingStorageArticles> PendingStorageArticlesGridList
        {
            get { return pendingStorageArticlesGridList; }
            set
            {
                pendingStorageArticlesGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageArticlesGridList"));
            }
        }

        #endregion // Properties

        #region Commands

        public ICommand ScanMaterialsCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshPendingStorageViewCommand { get; set; }
        public ICommand PrintPendingStorageViewCommand { get; set; }
        public ICommand ExportPendingStorageViewCommand { get; set; }
        public ICommand PendingStorageViewDNHyperlinkClickCommand { get; set; }
        public ICommand PendingStorageViewReferenceHyperlinkClickCommand { get; set; }

        #endregion // Commands

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

        public PendingStorageViewModel()
        {
            // List<LocationRefill> lre=new List<LocationRefill>(WarehouseService.GetRefillToList("2"));
         //Int64 minstock=   WarehouseService.GetMinimumStockBYLocationFullName(2, "A1-10-A01", 2647);
            GeosApplication.Instance.Logger.Log("Constructor PendingStorageViewModel....", category: Category.Info, priority: Priority.Low);

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

                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                RefreshPendingStorageViewCommand = new RelayCommand(new Action<object>(RefreshPendingStorageList));
                PrintPendingStorageViewCommand = new RelayCommand(new Action<object>(PrintPendingStorageList));
                ExportPendingStorageViewCommand = new RelayCommand(new Action<object>(ExportPendingStorageList));
                ScanMaterialsCommand = new DelegateCommand<object>(ScanAction);
                PendingStorageViewDNHyperlinkClickCommand = new DelegateCommand<object>(PendingStorageViewDNHyperlinkClickAction);
                PendingStorageViewReferenceHyperlinkClickCommand = new DelegateCommand<object>(PendingStorageViewReferenceHyperlinkClickCommandAction);

                //fill data as per selected warehouse
                FillPendingStorageList();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PendingStorageViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PendingStorageViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor Method PendingStorageViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Method

        /// <summary>
        /// Method to Open DN on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void PendingStorageViewDNHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingStorageViewDNHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
                TableView tblView = (TableView)obj;
                PendingStorageArticles psa = (PendingStorageArticles)tblView.FocusedRow;
                DXSplashScreen.Show<SplashScreenView>();
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(psa.IdWareHouseDeliveryNote);
                // wdn.WarehousePurchaseOrder.Warehouse = WarehousePurchaseOrder.Warehouse;

                //[pramo.misal][GEOS2-4543][10-10-2023]
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2440(psa.IdWareHouseDeliveryNote);
                //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
               // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2480(psa.IdWareHouseDeliveryNote);
                //[Sudhir.jangra][GEOS2-5457]
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2510(psa.IdWareHouseDeliveryNote);


                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (tblView as FrameworkElement);
                editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();
                GeosApplication.Instance.Logger.Log("Method PendingStorageViewDNHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingStorageViewDNHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);
            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;
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

            //fill data as per selected warehouse
            FillPendingStorageList();
            MyFilterString = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [002][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillPendingStorageList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingStorageList....", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {

                    //ObservableCollection<PendingStorageArticles> TempListPendingStorageArticle = new ObservableCollection<PendingStorageArticles>(WarehouseService.GetArticlesPendingStorage(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse));
                    // [001] Changed Service method
                    //Changed Service method from GetArticlesPendingStorage_V2034 to GetArticlesPendingStorage_V2350
                    PendingStorageArticlesList = WarehouseService.GetArticlesPendingStorage_V2350(WarehouseCommon.Instance.Selectedwarehouse);
                    SelectedObject = PendingStorageArticlesList.FirstOrDefault();
                    MergeByIdArticleAndIdWareHouseDeliveryNote();
                    #region Commented
                    //PendingStorageArticlesGridList = PendingStorageArticlesList.GroupBy(l => new { l.IdArticle, l.IdWareHouseDeliveryNote })
                    //                                        .Select(cl => new PendingStorageArticles
                    //                                        {
                    //                                            IdWarehouseLocation = cl.First().IdWarehouseLocation,
                    //                                            IdWarehouse = cl.First().IdWarehouse,
                    //                                            IdWareHouseDeliveryNote = cl.First().IdWareHouseDeliveryNote,
                    //                                            WarehouseDeliveryNote = cl.First().WarehouseDeliveryNote,
                    //                                            IdWareHouseDeliveryNoteItem = cl.First().IdWareHouseDeliveryNoteItem,
                    //                                            IdArticle = cl.First().IdArticle,
                    //                                            Quantity = cl.Sum(c => c.Quantity),
                    //                                            MinimumStock = cl.First().MinimumStock,
                    //                                            MaximumStock = cl.First().MaximumStock,
                    //                                            CurrentStock = cl.First().CurrentStock,
                    //                                            Reference = cl.First().Reference,
                    //                                            Description = cl.First().Description,
                    //                                            UnitPrice = cl.First().UnitPrice,
                    //                                            CostPrice = cl.First().CostPrice,
                    //                                            UploadedIn = cl.First().UploadedIn,
                    //                                            Code = cl.First().Code,
                    //                                            ArticleVisualAidsPath = cl.First().ArticleVisualAidsPath,
                    //                                            ImagePath = cl.First().ImagePath,
                    //                                            FullName = cl.First().FullName,
                    //                                            IdCurrency = cl.First().IdCurrency,
                    //                                            WarehouseLocation = cl.First().WarehouseLocation
                    //                                        }).ToList();
                    #endregion
                }
                else
                {
                    PendingStorageArticlesList = new List<PendingStorageArticles>();
                }
                GeosApplication.Instance.Logger.Log("Method FillPendingStorageList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingStorageList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingStorageList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillPendingStorageList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void RefreshPendingStorageList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingArticleList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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
                //fill data as per selected warehouse
                FillPendingStorageList();
                MyFilterString = string.Empty;
                detailView.SearchString = null;
                SelectedObject = null;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPendingStorageList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingStorageList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingStorageListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingStorageListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPendingStorageList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingStorageList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPendingStorageList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingStorageList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Storage List";
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
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportPendingStorageList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingStorageList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        /// <summary>
        /// Method for open scan Locate material window.
        /// </summary>
        /// <param name="obj"></param>
        private void ScanAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                //if (WarehouseCommon.Instance.FillStorageArticle.Equals(Application.Current.FindResource("MyPreferencesFillingByItem").ToString()))
                //{
                    TableView detailView = (TableView)obj;
                    StorageArticleView storageArticleView = new StorageArticleView();
                    StorageArticleViewModel storageArticleViewModel = new StorageArticleViewModel();
                    EventHandler handle = delegate { storageArticleView.Close(); };
                    storageArticleViewModel.RequestClose += handle;
                    storageArticleView.DataContext = storageArticleViewModel;
                    storageArticleViewModel.Init(PendingStorageArticlesList);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (detailView as FrameworkElement);
                    storageArticleView.Owner = Window.GetWindow(ownerInfo);
                    storageArticleView.ShowDialog();
                    MergeByIdArticleAndIdWareHouseDeliveryNote();
                // }
                //else
                //{
                //    PendingStorageScanView pendingStorageScanView = new PendingStorageScanView();
                //    PendingStorageScanViewModel pendingStorageScanViewModel = new PendingStorageScanViewModel();
                //    EventHandler handle = delegate { pendingStorageScanView.Close(); };
                //    pendingStorageScanViewModel.RequestClose += handle;
                //    pendingStorageScanViewModel.Init(PendingStorageArticlesList);
                //    pendingStorageScanView.DataContext = pendingStorageScanViewModel;
                //    pendingStorageScanView.ShowDialog();
                //    MergeByIdArticleAndIdWareHouseDeliveryNote();
                //}

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void MergeByIdArticleAndIdWareHouseDeliveryNote()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method MergeByIdArticleAndIdWareHouseDeliveryNote....", category: Category.Info, priority: Priority.Low);
                PendingStorageArticlesGridList = PendingStorageArticlesList.GroupBy(l => new { l.IdArticle, l.IdWareHouseDeliveryNote })
                                                      .Select(cl => new PendingStorageArticles
                                                      {
                                                          IdWarehouseLocation = cl.First().IdWarehouseLocation,
                                                          IdWarehouse = cl.First().IdWarehouse,
                                                          IdWareHouseDeliveryNote = cl.First().IdWareHouseDeliveryNote,
                                                          WarehouseDeliveryNote = cl.First().WarehouseDeliveryNote,
                                                          IdWareHouseDeliveryNoteItem = cl.First().IdWareHouseDeliveryNoteItem,
                                                          IdArticle = cl.First().IdArticle,
                                                          Quantity = cl.Sum(c => c.Quantity),
                                                          MinimumStock = cl.First().MinimumStock,
                                                          MaximumStock = cl.First().MaximumStock,
                                                          CurrentStock = cl.First().CurrentStock,
                                                          Reference = cl.First().Reference,
                                                          Description = cl.First().Description,
                                                          UnitPrice = cl.First().UnitPrice,
                                                          CostPrice = cl.First().CostPrice,
                                                          UploadedIn = cl.First().UploadedIn,
                                                          Code = cl.First().Code,
                                                          ArticleVisualAidsPath = cl.First().ArticleVisualAidsPath,
                                                          ImagePath = cl.First().ImagePath,
                                                          FullName = cl.First().FullName,
                                                          IdCurrency = cl.First().IdCurrency,
                                                          WarehouseLocation = cl.First().WarehouseLocation
                                                      }).ToList();
                GeosApplication.Instance.Logger.Log("Method MergeByIdArticleAndIdWareHouseDeliveryNote....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MergeByIdArticleAndIdWareHouseDeliveryNote...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void PendingStorageViewReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingStorageViewReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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
                PendingStorageArticles article = (PendingStorageArticles)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                long WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                if (articleDetailsViewModel.IsResult)
                {
                    List<ArticleWarehouseLocations> _articleWarehouseLocationsList = articleDetailsViewModel.ArticleWarehouseLocationsList.OrderBy(x => x.Position).ToList();
                    if (_articleWarehouseLocationsList.Count > 0)
                        article.WarehouseLocation = _articleWarehouseLocationsList.Last().WarehouseLocation;
                    else
                        article.WarehouseLocation = null;
                }
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method PendingStorageViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingStorageViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion //Methods
    }
}
