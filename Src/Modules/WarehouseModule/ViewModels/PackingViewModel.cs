using DevExpress.Export;
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
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class PackingViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region


        #region Declaration

        private ObservableCollection<TileBarFilters> listOfFilterTile = new ObservableCollection<TileBarFilters>();
        private List<Ots> workOrderOtsList = new List<Ots>();
        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();
        private string myFilterString;
        private int selectedTileIndex;
        private bool isBusy;
        private object geosAppSettingList;

        #endregion // End Of Declaration

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
        public List<Ots> FilterWiseListOfWorkOrder
        {
            get { return filterWiseListOfWorkOrder; }
            set
            {
                filterWiseListOfWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
            }
        }

        public List<Ots> WorkOrderOtsList
        {
            get { return workOrderOtsList; }
            set
            {
                workOrderOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderOtsList"));
            }
        }
        public ObservableCollection<TileBarFilters> ListOfFilterTile
        {
            get { return listOfFilterTile; }
            set
            {
                listOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfFilterTile"));
            }
        }
        public object GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public int SelectedTileIndex
        {
            get { return selectedTileIndex; }
            set
            {
                selectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndex"));
            }
        }
        private string _OTStatus;

        public string OTStatus
        {
            get { return _OTStatus; }
            set
            {
                _OTStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs(OTStatus));
            }
        }

        #endregion //End Of Properties

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

        #region Icommands
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshPackingWorkOrderViewCommand { get; set; }
        public ICommand PrintPackingWorkOrderViewCommand { get; set; }
        public ICommand ExportPackingWorkOrderViewCommand { get; set; }
        public ICommand ScanWorkOderCommand { get; set; }

        public ICommand SendEmailCommand { get; set; }

        public ICommand PackingScanWorkOderCommand { get; set; }

        #endregion //End Of Icommand

        #region Constructor
        public PackingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor  PackingViewModel()....", category: Category.Info, priority: Priority.Low);

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

                CommandFilterTileClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                CommandGridDoubleClick = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                RefreshPackingWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPackingWorkOrderList));
                PrintPackingWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintPackingWorkOrderList));
                ExportPackingWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportPackingWorkOrder));
                ScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrder);
                SendEmailCommand = new DevExpress.Mvvm.DelegateCommand<object>(SendEmail);

                FillWorkOrderList();
                FillPackingWorkOrderFilterTiles(WorkOrderOtsList);
                FillListofColor();
                MyFilterString = string.Empty;

                try
                {
                    PackingScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(PackingScanWorkOrder);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in  PackingViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor  PackingViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in  PackingViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// this method use for fill Packing Work order
        /// [001][cpatil][06-10-2021][GEOS2-3389][Add the Country Flag in the Packing Grid]
        /// [002][cpatil][06-10-2021][GEOS2-3392][Add a new column “Expected Delivery Week” in Packing Grid]
        /// </summary>
        private void FillWorkOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    WorkOrderOtsList = new List<Ots>();
                    try
                    {
                        //WorkOrderOtsList = new List<Ots>(WarehouseService.GetPackingWorkOrdersByWarehouse_V2320(WarehouseCommon.Instance.Selectedwarehouse));
                        WorkOrderOtsList = new List<Ots>(WarehouseService.GetPackingWorkOrdersByWarehouse_V2540(WarehouseCommon.Instance.Selectedwarehouse)); //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
                        //[001]
                        if (WorkOrderOtsList != null)
                        {
                            CultureInfo cul = CultureInfo.CurrentCulture;
                            WorkOrderOtsList.Where(x => x.DeliveryDate != null).ToList().ForEach(x => { x.DeliveryWeek = x.DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)x.DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0'); });
                            // [002]
                            foreach (var otitem in WorkOrderOtsList.GroupBy(tpa => tpa.Quotation.Site.Country.Iso))
                            {

                                ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().Quotation.Site.Country.CountryIconBytes);
                                otitem.ToList().Where(oti => oti.Quotation.Site.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.Quotation.Site.Country.CountryIconImage = countryFlagImage);
                            }
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                }
                else
                {
                    WorkOrderOtsList = new List<Ots>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillWorkOrderList() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
        /// 
        /// </summary>
        /// <param name="WorkOrderList"></param>
        public void FillPackingWorkOrderFilterTiles(List<Ots> WorkOrderList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPackingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);

                ListOfFilterTile = new ObservableCollection<TileBarFilters>();

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()),
                    Id = 0,
                    EntitiesCount = WorkOrderList.Count(),
                });

                List<Ots> siteRelatedWorkOrderList = WorkOrderList.Where(x => x.Quotation.Site.ShortName != null).ToList();
                List<string> siteList = siteRelatedWorkOrderList.Select(x => x.Quotation.Site.ShortName).Distinct().ToList();
                List<Company> distinctSiteList = new List<Company>();
                //Shubham[skadam] GEOS2-5703 Show the current packing % status (1/3) 14 06 2024
                try
                {
                     distinctSiteList = siteRelatedWorkOrderList.Where(x => !string.IsNullOrEmpty(x.Quotation.Site.ShortName))
                    .GroupBy(x => x.Quotation.Site.ShortName) .Select(g => g.First().Quotation.Site).ToList();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillPackingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                foreach (string siteName in siteList)
                {
                    #region CountryIconBytes
                    byte[] CountryIconBytes = null;
                    try
                    {
                        //Shubham[skadam] GEOS2-5703 Show the current packing % status (1/3) 14 06 2024
                        CountryIconBytes = distinctSiteList.FirstOrDefault(f => f.ShortName.Equals(siteName)).Country.CountryIconBytes;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillPackingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = siteName,
                        Image = ByteArrayToBitmapImage(CountryIconBytes),
                        EntitiesCount = siteRelatedWorkOrderList.Count(x => x.Quotation.Site.ShortName == siteName),
                    });
                }

                if (ListOfFilterTile.Count > 0)
                    SelectedTileIndex = 0;

                GeosApplication.Instance.Logger.Log("Method FillPackingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPackingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ShowSelectedFilterGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);
                if (ListOfFilterTile.Count > 0)
                {
                    //string siteName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                    string siteName = string.Empty;
                    var array = (object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems;
                    if (array.Length > 0)
                    {
                        siteName = ((TileBarFilters)array[0]).Caption;
                    }

                    if (siteName != string.Empty)
                    {
                        if (siteName.ToUpper() == "ALL")
                        {
                            FilterWiseListOfWorkOrder = WorkOrderOtsList;
                        }
                        else
                        {
                            FilterWiseListOfWorkOrder = WorkOrderOtsList.Where(x => x.Quotation.Site.ShortName == siteName).ToList();
                        }

                        MyFilterString = string.Empty;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method use for (Refresh purpose) 
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshPackingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPackingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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

                FillWorkOrderList();
                FillPackingWorkOrderFilterTiles(WorkOrderOtsList);
                MyFilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPackingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPackingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for print Packing work order 
        /// </summary>
        /// <param name="obj"></param>
        private void PrintPackingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPackingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PackingWorkOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PackingWorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPackingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPackingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for Export packing Work order into Excel file
        /// </summary>
        /// <param name="obj"></param>
        private void ExportPackingWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPackingWorkOrder()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Packing Work Order";
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
                    // options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportPackingWorkOrder()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPackingWorkOrder()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment()
            {
                WrapText = true
            };
            e.Handled = true;
        }

        //private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        //{
        //    if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
        //        e.DocumentColumn.WidthInPixels = 90;
        //}


        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                FillWorkOrderList();
                FillPackingWorkOrderFilterTiles(WorkOrderOtsList);
                MyFilterString = string.Empty;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseEditValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for showing Grid's selected row Item detailed Window
        /// <para>[001][skale][2019-09-04][s63][GEOS2-1522] Work order automatically deleted from packing section.</para>
        /// </summary>
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }


                TableView detailView = (TableView)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;

                int idOT = (int)((Emdep.Geos.Data.Common.Ots)detailView.FocusedRow).IdOT;
                Ots ot = (Ots)detailView.FocusedRow;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(ot.IdOT, warehouse);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                if (workOrderItemDetailsViewModel.IsSaveChanges == true)
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

                    FillWorkOrderList();
                    FillPackingWorkOrderFilterTiles(WorkOrderOtsList);
                    MyFilterString = string.Empty;
                    detailView.SearchString = null;
                    IsBusy = false;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                  

                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillListofColor()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListofColor...", category: Category.Info, priority: Priority.Low);

                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");

                GeosApplication.Instance.Logger.Log("Method FillListofColor executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListofColor() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListofColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListofColor() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                PackingScanView packingScanView = new PackingScanView();
                PackingScanViewModel packingScanViewModel = new PackingScanViewModel();
                packingScanViewModel.Init(WorkOrderOtsList.Select(x => x.Quotation.Site).ToList());
                EventHandler handler = delegate { packingScanView.Close(); };
                packingScanViewModel.RequestClose += handler;
                packingScanView.DataContext = packingScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                packingScanView.Owner = Window.GetWindow(ownerInfo);
                packingScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PackingViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this Method use for Send Email
        /// [001][skale][15-01-2020][GEOS2-1840] Ready for shipping automatic email
        /// </summary>
        /// <param name="obj"></param>
        private void SendEmail(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method SendEmail...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                SendReadyExpeditionEmailView sendReadyExpeditionEmailView = new SendReadyExpeditionEmailView();
                SendReadyExpeditionEmailViewModel sendReadyExpeditionEmailViewModel = new SendReadyExpeditionEmailViewModel();
                sendReadyExpeditionEmailViewModel.Init(FilterWiseListOfWorkOrder);
                EventHandler handler = delegate { sendReadyExpeditionEmailView.Close(); };
                sendReadyExpeditionEmailViewModel.RequestClose += handler;
                sendReadyExpeditionEmailView.DataContext = sendReadyExpeditionEmailViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                sendReadyExpeditionEmailView.Owner = Window.GetWindow(ownerInfo);
                sendReadyExpeditionEmailView.ShowDialogWindow();
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
                detailView.UpdateLayout();
                var temp = FilterWiseListOfWorkOrder.Where(s => sendReadyExpeditionEmailViewModel.SelectedOtsList.Select(i => i.IdOT).Contains(s.IdOT));
                temp.ToList().ForEach(s => s.DeliveryDate = sendReadyExpeditionEmailViewModel.SelectedOtsList.FirstOrDefault().DeliveryDate);
                GeosApplication.Instance.Logger.Log("PackingViewModel Method SendEmail executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in PackingViewModel SendEmail() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PackingScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method PackingScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                PackingScanWizzardView packingScanView = new PackingScanWizzardView();
                PackingScanWizzardViewModel packingScanViewModel = new PackingScanWizzardViewModel();
                packingScanViewModel.WorkOrderOtsList = WorkOrderOtsList;
                packingScanViewModel.TableView = detailView;
                packingScanViewModel.WindowHeader = Application.Current.FindResource("PackingScanWorkOrderHeader").ToString();
                packingScanViewModel.Init(WorkOrderOtsList.Select(x => x.Quotation.Site).ToList());
                EventHandler handler = delegate { packingScanView.Close(); };
                packingScanViewModel.RequestClose += handler;
                packingScanView.DataContext = packingScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                packingScanView.Owner = Window.GetWindow(ownerInfo);
                packingScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("PackingViewModel Method PackingScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PackingScanWorkOrder  method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


    }
}
