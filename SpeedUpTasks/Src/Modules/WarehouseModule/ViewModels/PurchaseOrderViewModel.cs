using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Logging;
using Emdep.Geos.UI.Helper;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Printing;
using Microsoft.Win32;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PurchaseOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

        #region public ICommand

        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshPurchaseOrderViewCommand { get; set; }
        public ICommand PrintPurchaseOrderViewCommand { get; set; }
        public ICommand ExportPurchaseOrderViewCommand { get; set; }
        #endregion //public ICommand

        #region Declaration

        public event EventHandler RequestClose;
        private DateTime expectedDateMinValue;

        private List<WarehousePurchaseOrder> mainPurchaseOrderList;
        private List<WarehousePurchaseOrder> listPurchaseOrder;

        private ObservableCollection<TileBarFilters> listofitem;

        private string myFilterString;
        private bool isInIt = false;
        private int selectedTileIndex;
        private bool isBusy;
        #endregion //Declaration       

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

        public List<WarehousePurchaseOrder> MainPurchaseOrderList
        {
            get
            {
                return mainPurchaseOrderList;
            }

            set
            {
                mainPurchaseOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainPurchaseOrderList"));
            }
        }

        public List<WarehousePurchaseOrder> ListPurchaseOrder
        {
            get
            {
                return listPurchaseOrder;
            }

            set
            {
                listPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPurchaseOrder"));

            }
        }

        public ObservableCollection<TileBarFilters> Listofitem
        {
            get
            {
                return listofitem;
            }

            set
            {
                listofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listofitem"));

            }
        }
        public FilterItem SelectedItem { get; set; }

        public DateTime ExpectedDateMinValue
        {
            get
            {
                return expectedDateMinValue;
            }

            set
            {
                expectedDateMinValue = value;
            }
        }

        public int SelectedTileIndex
        {
            get
            {
                return selectedTileIndex;
            }

            set
            {
                selectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndex"));
            }
        }

        #endregion // End Properties Region

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // end public events region

        #region Constructor
        public PurchaseOrderViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor FilterTreeViewModel....", category: Category.Info, priority: Priority.Low);

                isInIt = true;

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

                CommandShowFilterPopupClick = new DelegateCommand<object>(LeadsShowFilterValue);
                CommandGridDoubleClick = new DelegateCommand<object>(PendingReceptionItemsWindowShow);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);

                ExpectedDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
                RefreshPurchaseOrderViewCommand = new RelayCommand(new Action<object>(RefreshPurchaseOrderList));
                PrintPurchaseOrderViewCommand = new RelayCommand(new Action<object>(PrintPurchaseOrderList));
                ExportPurchaseOrderViewCommand = new RelayCommand(new Action<object>(ExportPurchaseOrderList));
                //Fill data as per selected warehouse
                FillMainPurchaseOrderList();

                //Rearrange tile Arrange
                TileBarArrange();

                MyFilterString = string.Empty;

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor FilterTreeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterTreeViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // end constructor region

        #region Methods

        public void RefreshPurchaseOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);
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

                FillMainPurchaseOrderList();
                //Rearrange tile Arrange
                TileBarArrange();
                MyFilterString = string.Empty;
                detailView.SearchString = null;

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPurchaseOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PurchaseOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PurchaseOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPurchaseOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Purchase Order List";
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
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);
            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

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

            //fill data as per selected warehouse
            FillMainPurchaseOrderList();

            //rearrange tile Arrange
            TileBarArrange();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for arrange tile after get all data.
        /// </summary>
        private void TileBarArrange()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();


                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    EntitiesCount = MainPurchaseOrderList.Count(),
                    ImageUri = "AllTasks.png"
                });

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("PartialPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("PartialPendingPurchaseOrder").ToString()),
                    EntitiesCount = MainPurchaseOrderList.Count(x => x.IsPartialPending),
                    ImageUri = "InProgress.png"
                });

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                    EntitiesCount = MainPurchaseOrderList.Count(x => !x.IsPartialPending),
                    ImageUri = "NotStarted.png"
                });

                // After change index it will automatically redirect to method LeadsShowFilterValue(object obj) and arrange the tile section count.
                if (Listofitem.Count > 0)
                    SelectedTileIndex = 0;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdod for fill purchase order list. 
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        /// </summary>
        private void FillMainPurchaseOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    //ObservableCollection<WarehousePurchaseOrder> TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>();


                    //List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();

                    //foreach (var item in selectedwarehouselist.GroupBy(x => x.IdSite))
                    //{
                    //    try
                    //    {
                    //        string idsWarehouse = string.Join(",", from wh in item select wh.IdWarehouse);
                    //        TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReceptionByWarehouse(idsWarehouse, item.First()));
                    //        MainPurchaseOrderList.AddRange(TempMainPurchaseOrderList);
                    //    }
                    //    catch (FaultException<ServiceException> ex)
                    //    {
                    //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    }
                    //    catch (ServiceUnexceptedException ex)
                    //    {
                    //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    //    }
                    //}



                    try
                    {
                        MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                        Warehouses Warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                     ///[001] service method change
                        ObservableCollection<WarehousePurchaseOrder> TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReceptionByWarehouse_V2044(Warehouse));

                        if (TempMainPurchaseOrderList != null)
                        {
                            foreach (var otitem in TempMainPurchaseOrderList.GroupBy(tpa => tpa.ArticleSupplier.Country.Iso))
                            {

                                ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().ArticleSupplier.Country.CountryIconBytes);
                                otitem.ToList().Where(oti => oti.ArticleSupplier.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.ArticleSupplier.Country.CountryIconImage = countryFlagImage);
                            }
                        }
                        MainPurchaseOrderList.AddRange(TempMainPurchaseOrderList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }



                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>(MainPurchaseOrderList);
                }

                else
                {
                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }


            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method shows the Pending Reception Items
        /// </summary>
        private void PendingReceptionItemsWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingReceptionItemsWindowShow....", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                PurchaseOrderItemDetailsViewModel purchaseOrderItemDetailsViewModel = new PurchaseOrderItemDetailsViewModel();
                PurchaseOrderItemDetailsView purchaseOrderItemDetailsView = new PurchaseOrderItemDetailsView();

                EventHandler handle = delegate { purchaseOrderItemDetailsView.Close(); };
                purchaseOrderItemDetailsViewModel.RequestClose += handle;

                WarehousePurchaseOrder wpo = (WarehousePurchaseOrder)detailView.FocusedRow;
                //int idWarehousePurchaseOrder = wpo.IdWarehousePurchaseOrder;
                //     List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                //Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == wpo.IdWarehouse);
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                purchaseOrderItemDetailsViewModel.Init(wpo.IdWarehousePurchaseOrder, warehouse);
                purchaseOrderItemDetailsView.DataContext = purchaseOrderItemDetailsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                purchaseOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                purchaseOrderItemDetailsView.ShowDialog();

                if (!purchaseOrderItemDetailsViewModel.IsMoreNeeded)
                {
                    var item = ListPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                    ListPurchaseOrder.Remove(item);
                    ListPurchaseOrder = new List<WarehousePurchaseOrder>(ListPurchaseOrder);
                }
                GeosApplication.Instance.Logger.Log("Method PendingReceptionItemsWindowShow....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingReceptionItemsWindowShow...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method filters Pending Reception Items as per user selection
        /// </summary>
        private void LeadsShowFilterValue(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;

                    if (str.Equals(string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString())))
                    {
                        ListPurchaseOrder = MainPurchaseOrderList;
                    }
                    if (str.Equals(string.Format(System.Windows.Application.Current.FindResource("PartialPendingPurchaseOrder").ToString())))
                    {
                        ListPurchaseOrder = MainPurchaseOrderList.Where(x => x.IsPartialPending == true).ToList();
                    }
                    if (str.Equals(string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString())))
                    {
                        ListPurchaseOrder = MainPurchaseOrderList.Where(x => x.IsPartialPending == false).ToList();
                    }

                    MyFilterString = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LeadsShowFilterValue...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }
    }

    #endregion // end methods region

}
