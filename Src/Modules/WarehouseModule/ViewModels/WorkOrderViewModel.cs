using DevExpress.Data.Filtering.Helpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.Globalization;
using System.IO;
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
    public class WorkOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
      
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //  IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        #endregion // End Services Region

        #region Declaration

        private ObservableCollection<TileBarFilters> listOfFilterTile = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();

        private List<Ots> mainOtsList = new List<Ots>();
        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();

        private string filterString;
        private bool isBusy;
        private TileBarFilters selectedTileBarItem;
        private int visibleRowCount;
        private string userSettingsKey = "WMS_WorkOrder_";
        private bool isEdit;
        private bool isWorkOrderColumnChooserVisible;
        public string WorkOrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WorkOrderGridSettingFilePath.Xml";
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

        public ObservableCollection<TileBarFilters> ListOfFilterTile
        {
            get { return listOfFilterTile; }
            set
            {
                listOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfFilterTile"));
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
        public string CustomFilterStringName { get; set; }

        public bool IsWorkOrderColumnChooserVisible
        {
            get
            {
                return isWorkOrderColumnChooserVisible;
            }

            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
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

        #endregion //End Of Properties

        #region Icommands
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand ScanWorkOderCommand { get; set; }
        public ICommand RefundWorkOrderCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }

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
        public WorkOrderViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);

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
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPendingWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintPendingWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportPendingWorkOrderList));
                ScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrder);
                RefundWorkOrderCommand = new DelegateCommand<object>(RefundWorkOrderCommandAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CustomCellAppearanceCommand = new DelegateCommand<CustomRowAppearanceEventArgs>(CustomCellAppearanceAction);

                FillMainOtList();
                FillListofColor();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                if (ListOfFilterTile.Count > 0)
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //End Of Constructor

        #region Methods


        /// <summary>
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        /// This method is for to convert from Bytearray to ImageSource
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
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        ///[002][cpatil][06-10-2021][GEOS2-3391]Add a new column “Expected Delivery Week” in Picking Grid
        ///[003][cpatil][13-09-2023][GEOS2-4417]
        ///[004][cpatil][28-10-2023][GEOS2-4948]
        /// </summary>
        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();

                    try
                    {

                        ///[001]service method change
                        //  TempMainOtsList = new List<Ots>(WarehouseService.GetPendingMaterialWorkOrdersByWarehouse_V2320(WarehouseCommon.Instance.Selectedwarehouse));
                        //[Sudhir.Jangra][GEOS2-4541][21/08/2023]
                        ///[004]service method change
                       // TempMainOtsList = new List<Ots>(WarehouseService.GetPendingMaterialWorkOrdersByWarehouse_V2450(WarehouseCommon.Instance.Selectedwarehouse));
                        TempMainOtsList = new List<Ots>(WarehouseService.GetPendingMaterialWorkOrdersByWarehouse_V2540(WarehouseCommon.Instance.Selectedwarehouse));  //[rahul.gadhave] [GEOS2-5676] [16-07-2024]

                        if (TempMainOtsList != null)
                        {
                            //[002]
                            CultureInfo cul = CultureInfo.CurrentCulture;
                            TempMainOtsList.Where(x => x.DeliveryDate != null).ToList().ForEach(x => { x.DeliveryWeek = x.DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)x.DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0'); });
                            foreach (var otitem in TempMainOtsList.GroupBy(tpa => tpa.Quotation.Site.Country.Iso))
                            {
                                ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().Quotation.Site.Country.CountryIconBytes);
                                otitem.ToList().Where(oti => oti.Quotation.Site.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.Quotation.Site.Country.CountryIconImage = countryFlagImage);
                            }
                        }
                        MainOtsList.AddRange(TempMainOtsList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
            FillMainOtList();
            //Update count for Templates
            FillPendingWorkOrderFilterList();
            AddCustomSetting();
            FilterString = string.Empty;
            if (ListOfFilterTile.Count > 0)
                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter List
        /// </summary>
        public void FillPendingWorkOrderFilterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);

                ListOfTemplate = new ObservableCollection<Template>(WarehouseService.GetTemplatesByIdTemplateType(2));
                FillPendingWorkOrderFilterTiles(ListOfTemplate, MainOtsList);

                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter Tiles 
        /// </summary>
        public void FillPendingWorkOrderFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);

                ListOfFilterTile = new ObservableCollection<TileBarFilters>();

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(),
                    ImageUri = "Template.png",
                    BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                foreach (Template item in FilterList)
                {
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = item.Name.ToString(),
                        Id = item.IdTemplate,
                        Type = item.Name,
                        EntitiesCount = MainListWorkOrder.Count(x => x.Quotation.IdDetectionsTemplate == item.IdTemplate),
                        ImageUri = "Template.png",
                        BackColor = item.HtmlColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });
                }

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for showing Grid as per Filter Tile Selection
        /// </summary>
        private void ShowSelectedFilterGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (ListOfFilterTile.Count > 0)
                {
                    //int IdTemplate = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Id;
                    string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                    string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                    CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    if (Template == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            FilterString = _FilterString;
                        else
                            FilterString = string.Empty;
                    }
                    else
                    {
                        FilterString = "[Quotation.Template.Name] In ('" + Template + "')";
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
        /// Method for showing Grid's selected row Item detailed Window
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;

                Ots ot = (Ots)detailView.Grid.SelectedItem;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(ot.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
              
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();

                //if download quantity is zero then remove item from main grid. 
                if (workOrderItemDetailsViewModel.DownloadRemainingQuantity == 0)
                {
                    MainOtsList.Remove(MainOtsList.Where(oti => oti.IdOT == ot.IdOT).FirstOrDefault());
                    MainOtsList = new List<Ots>(MainOtsList);
                    FillPendingWorkOrderFilterList();
                }
                else if (workOrderItemDetailsViewModel.IsSaveChanges == true)
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

                  
                    FillMainOtList();
                    FillPendingWorkOrderFilterList();
                    AddCustomSetting();
                    FilterString = string.Empty;
                    detailView.SearchString = null;
                    IsBusy = false;
                    if (ListOfFilterTile.Count > 0)
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void RefreshPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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

                // code for hide column chooser if empty
                TableView tableView = (TableView)gridControl.View;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
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

                FillMainOtList();
                FillPendingWorkOrderFilterList();
                AddCustomSetting();
                FilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (ListOfFilterTile.Count > 0)
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Work Order List";
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
                    options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
                    activityTableView.ExportToXlsx(ResultFileName, options);
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

        /// <summary>
        /// Open Scan ot view
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("WorkOrderViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("WorkOrderScanHeader").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                workOrderScanViewModel.IsRefund = false;
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderScanView.Owner = Window.GetWindow(ownerInfo);
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("WorkOrderViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        {
            if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
                e.DocumentColumn.WidthInPixels = 90;
        }

        public void Dispose()
        {
        }

        private void RefundWorkOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("RefundWorkOrderHeader").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                workOrderScanViewModel.IsRefund = true;
                EventHandler handler = delegate { workOrderScanView.Close(); };
                workOrderScanViewModel.RequestClose += handler;
                workOrderScanView.DataContext = workOrderScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderScanView.Owner = Window.GetWindow(ownerInfo);
                workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
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

                customFilterEditorViewModel.Init(e.FilterControl, ListOfFilterTile);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        ListOfFilterTile.Remove(tileBarItem);
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
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
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
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedTileBarItem = ListOfFilterTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Ots)), item.Value, false);
                        List<Ots> tempList = new List<Ots>();
                        foreach (var ot in MainOtsList)
                        {
                            if (evaluator.Fit(ot))
                                tempList.Add(ot);
                        }
                        FilterString = item.Value;
                        ListOfFilterTile.Add(
                        new TileBarFilters()
                        {
                            Caption = item.Key.Replace(userSettingsKey, ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCount = tempList.Count,
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

        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                if (ListOfTemplate.Any(x => x.Name == CustomFilterStringName) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()))
                    return;
                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
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

        /// <summary>
        /// [001][skhade][2019-10-17][GEOS2-1548] Add a new column "#Out of Stock" in picking -> Work orders
        /// </summary>
        /// <param name="e"></param>
        private void CustomCellAppearanceAction(CustomRowAppearanceEventArgs e)
        {
            if (((CustomCellAppearanceEventArgs)e).Column != null && ((CustomCellAppearanceEventArgs)e).Column.Name == "OutOfStock")
            {
                e.Result = e.ConditionalValue;
                e.Handled = true;
            }
        }

        /// <summary>
        /// [001][psutar][2019-06-20][GEOS2-65] Allow to save grid configuration in work order section
        /// Method for saving grid layoutInvokeDelegateCommand
        /// [002][skhade][2019-10-17][GEOS2-1548] Add a new column "#Out of Stock" in picking -> Work orders
        /// </summary>
        /// <param name="obj"></param>
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(WorkOrderGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(WorkOrderGridSettingFilePath);
                    GridControl GridControlEmpolyeeDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView EmployeeProfileTableView = (TableView)GridControlEmpolyeeDetails.View;

                    if (EmployeeProfileTableView.SearchString != null)
                    {
                        EmployeeProfileTableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(WorkOrderGridSettingFilePath);

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

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

                //002 - This code is added because format conditions are deleted when restore from old layout.
                if (datailView.FormatConditions == null || datailView.FormatConditions.Count == 0)
                {
                    var profitFormatCondition = new FormatCondition()
                    {
                        Expression = "[OutOfStockItemCount] = [OtItemCount]",
                        FieldName = "OutOfStockItemCount",
                        Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                        {
                            Background = Brushes.Red
                        }
                    };
                    datailView.FormatConditions.Add(profitFormatCondition);
                }

                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

            if (e.Property.Name == "FormatConditions")
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorkOrderGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    isWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(WorkOrderGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000] [avpawar] [26-06-2019] [GEOS2-1630] [Put OT colors in work orders grid in picking]
        /// </summary>
        private void FillListofColor()
        {
            GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
        }


        #endregion //End Of Methods
    }
}
