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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class ArticlesReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Service

        IWarehouseService WmsService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IWarehouseService WmsService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region  public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Event

        #region Declaration

        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        private bool isBusy;
        private Duration _currentDuration;

        private ObservableCollection<WMSOrder> managementOrderGridList;
        private ObservableCollection<Offer> offersCollection;
        private WMSOrder selectedManagementOrder;
        private Offer selectedOffer;
        private string searchString;

        private Visibility isGridViewVisible;
        Visibility isCalendarVisible;

        private ChartControl chartControl;

        private bool isWorkOrderColumnChooserVisible;
        public string WorkOrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WMSArticlesReportGridSettingFilePath.Xml";
        const string shortDateFormat = "dd/MM/yyyy";



        #endregion

        #region  public Properties

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        public string FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ObservableCollection<WMSOrder> ManagementOrderGridList
        {
            get
            {
                return managementOrderGridList;
            }

            set
            {
                managementOrderGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManagementOrderGridList"));
            }
        }

        public ObservableCollection<Offer> OffersCollection
        {
            get
            {
                return offersCollection;
            }
            set
            {
                offersCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(OffersCollection)));
            }
        }

        private CultureInfo wmsCultureInfo;
        public CultureInfo WMSCultureInfo
        {
            get
            {
                return wmsCultureInfo;
            }
            set
            {
                wmsCultureInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(WMSCultureInfo)));
            }
        }

        public WMSOrder SelectedManagementOrder
        {
            get
            {
                return selectedManagementOrder;
            }

            set
            {
                selectedManagementOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedManagementOrder"));
            }
        }
        
        public Offer SelectedOffer
        {
            get
            {
                return selectedOffer;
            }

            set
            {
                selectedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedManagementOrder"));
            }
        }

        public string SearchString
        {
            get { return searchString; }
            set { searchString = value; OnPropertyChanged(new PropertyChangedEventArgs("SearchString")); }
        }

        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }

            set
            {
                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
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



        #endregion

        #region  public Commands

        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand DisableAppointmentCommand { get; set; }

        public ICommand CommandWarehouseEditValueChanged { get; private set; }

        public ICommand ShowGridViewCommand { get; set; }
        public ICommand ChartLoadCommand { get; set; }
        
        public ICommand WarehouseViewReferenceHyperlinkClickCommand { get; private set; }

        public ICommand WorkOrderHyperlinkClickCommand { get; set; }

        public ICommand TableViewLoadedCommand { get; set; }
        #endregion

        #region Constructor

        public ArticlesReportViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ArticlesReportViewModel ...", category: Category.Info, priority: Priority.Low);

                Processing();

                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshWorkLogReoprtAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintWorkLogReoprtAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWorkLogReoprtAction));

                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(SelectedWarehouseDetailsCommandAction);

                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowGridViewAction));
                WarehouseViewReferenceHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseViewReferenceHyperlinkClickCommandAction);
                WorkOrderHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderHyperlinkClickCommandAction);

                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);

                setDefaultPeriod();
                FillGridList();
                IsGridViewVisible = Visibility.Visible;
                IsCalendarVisible = Visibility.Collapsed;

                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                WMSCultureInfo = new CultureInfo(GeosApplication.CurrentCultureInfo.Name);
                WMSCultureInfo.NumberFormat.CurrencySymbol = GeosApplication.Instance.WMSCurrentCurrencySymbol;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Constructor ArticlesReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ArticlesReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods


        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(WorkOrderGridSettingFilePath))
                {
                    try
                    {
                        ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(WorkOrderGridSettingFilePath);
                        GridControl GridControlEmpolyeeDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                        TableView EmployeeProfileTableView = (TableView)GridControlEmpolyeeDetails.View;

                        if (EmployeeProfileTableView.SearchString != null)
                        {
                            EmployeeProfileTableView.SearchString = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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

                //if (visibleFalseCoulumn > 0)
                //{
                isWorkOrderColumnChooserVisible = true;
                //}
                //else
                //{
                //    isWorkOrderColumnChooserVisible = false;
                //}

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

                ////002 - This code is added because format conditions are deleted when restore from old layout.
                //if (datailView.FormatConditions == null || datailView.FormatConditions.Count == 0)
                //{
                //    var profitFormatCondition = new FormatCondition()
                //    {
                //        Expression = "[OutOfStockItemCount] = [OtItemCount]",
                //        FieldName = "OutOfStockItemCount",
                //        Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                //        {
                //            Background = Brushes.Red
                //        }
                //    };
                //    datailView.FormatConditions.Add(profitFormatCondition);
                //}

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
            //if (e.DependencyProperty == GridControl.FilterStringProperty)
            //    e.Allow = false;

            //if (e.Property.Name == "GroupCount")
            //    e.Allow = false;

            //if (e.DependencyProperty == TableViewEx.SearchStringProperty)
            //    e.Allow = false;

            //if (e.Property.Name == "FormatConditions")
            //    e.Allow = false;
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
        /// Method to Open Article View on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
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
                
                Offer selectedOffer = (Offer)detailView.DataControl.CurrentItem;
                Article article = selectedOffer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article; //  detailView.FocusedRow;

                int index = detailView.FocusedRowHandle;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                // List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                // Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == article.MyWarehouse.IdWarehouse);
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                var WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseId);
                bool isPermissionEnabledInWarehouseCommon = WarehouseCommon.Instance.IsPermissionEnabled;
                //WarehouseCommon.Instance.IsPermissionEnabled = false;
                //articleDetailsViewModel.IsAddComment = false;
                //articleDetailsViewModel.IsEditComment = false;
                //articleDetailsViewModel.IsDeleteComment = false;
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //WarehouseCommon.Instance.IsPermissionEnabled = isPermissionEnabledInWarehouseCommon;

                if (articleDetailsViewModel.IsResult)
                {
                    //MyWarehouse mw = (MyWarehouse)article.MyWarehouse.Clone();
                    //mw.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                    //mw.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                    //mw.LockedStock = articleDetailsViewModel.LockedStock;
                    //mw.Location = String.Join("\n", articleDetailsViewModel.ArticleWarehouseLocationsList.Where(x => x.IdWarehouseLocation != WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation).Select(x => x.WarehouseLocation.FullName).ToArray());
                    //article.MyWarehouse = mw;

                    //ArticlesList.RemoveAt(index);
                    //article.MyWarehouse.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                    //article.MyWarehouse.MaximumStock = articleDetailsViewModel.MaximumQuantity;
                    //ArticlesList.Insert(index, article);

                    //SelectedArticle = article;

                }

                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to open Work Order details view
        /// </summary>
        /// <param name="obj"></param>
        private void WorkOrderHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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
                Offer selectedOffer1 = (Offer)detailView.DataControl.CurrentItem;
               // Article article = selectedOffer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article; //  detailView.FocusedRow;
                //string selectedWOMergeCode = selectedOffer1.Quotations[0].Ots[0].IdOT;

                //WOItem unPackedItem = (WOItem)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(selectedOffer1.Quotations[0].Ots[0].IdOT, WarehouseCommon.Instance.Selectedwarehouse);
                //workOrderItemDetailsViewModel.
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workOrderItemDetailsView.ShowDialogWindow();
                //   FocusUserControl = true;
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method HyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillGridList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGridList()...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    try
                    {
                        //ManagementOrderGridList = new ObservableCollection<WMSOrder>(WmsService.GetOrders(WarehouseCommon.Instance.Selectedwarehouse, DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)));
                        //GEOS2-2370
                        //var warehouseCategories = new ObservableCollection<WarehouseCategory>(CrmStartUp.GetWarehouseCategories().OrderBy(c => c.BaseName).ToList());
                        //var idArticles = string.Join(",", warehouseCategories.Select(i => i.IdArticle));

                        var Company = WarehouseCommon.Instance.Selectedwarehouse.Company; // GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();



                        OffersCollection = new ObservableCollection<Offer>();
                        OffersCollection.AddRange(WmsService.GetArticlesReportDetails_V2130(
                            GeosApplication.Instance.ActiveUser.IdUser, Company,
                            DateTime.ParseExact(FromDate,shortDateFormat,null),
                            DateTime.ParseExact(ToDate, shortDateFormat, null),
                            GeosApplication.Instance.IdWMSCurrencyByRegion, string.Empty
                            ,WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse));

                        //IList<Currency> CurrenciesIList = CrmStartUp.GetCurrencies();
                        //OffersCollection[0].Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.MaterialType
                        //GeosApplication.Instance.WMSCurrentCurrencySymbol
                        //WMSCurrentCurrencySymbol
                        //OffersCollection[0].POReceivedInDate

                        //foreach (var otItem in OffersCollection)
                        //{
                        //    var article = otItem.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article;
                        //    if(article.Family==null)
                        //    {
                        //        article.Family = new LookupValue() {                                    
                        //            Value="TESTING..."
                        //        };
                        //    }
                        //    else
                        //    {

                        //    }

                        //}

                        foreach (var otItem in OffersCollection)
                        {
                            var ot = otItem.Quotations[0].Ots[0];
                            ot.MergeCode = ot.Code + " OT " + ot.NumOT;
                            //ot.MergeCode = ot.Quotation.Code + " " + ot.Quotation.Offer.OfferType.Name + " " + ot.NumOT;
                        }
                        SelectedOffer = OffersCollection.FirstOrDefault();
                        //if (ManagementOrderGridList != null)
                        //{
                        //    foreach (var otitem in ManagementOrderGridList.GroupBy(tpa => tpa.Country.Iso))
                        //    {

                        //        ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().Country.CountryIconBytes);
                        //        otitem.ToList().Where(oti => oti.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.Country.CountryIconImage = countryFlagImage);
                        //    }
                        //}

                        //SelectedManagementOrder = ManagementOrderGridList.FirstOrDefault();
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillGridList() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillGridList() Method - ServiceUnexceptedException " + ex.Message
                            + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                }
                else
                {
                    //ManagementOrderGridList = new ObservableCollection<WMSOrder>();
                    OffersCollection = new ObservableCollection<Offer>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Method FillGridList() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGridList() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private Action Processing()
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
            return null;
        }

        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

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

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString(shortDateFormat);
                    ToDate = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString(shortDateFormat);
                    ToDate = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString(shortDateFormat);
                    ToDate = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString(shortDateFormat);
                    ToDate = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString(shortDateFormat);
                    ToDate = EndDate.ToString(shortDateFormat);
                }
                else if(IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if(IsButtonStatus==9)//last year
                {
                    FromDate = StartFromDate.ToString(shortDateFormat);
                    ToDate = EndToDate.ToString(shortDateFormat);
                }

                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }

                if (IsGridViewVisible == Visibility.Visible)
                {
                    FillGridList();
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
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
            IsCalendarVisible = Visibility.Collapsed;

        }

        private void SelectedWarehouseDetailsCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedWarehouseDetailsCommandAction...", category: Category.Info, priority: Priority.Low);
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
                SearchString = string.Empty;
                FillGridList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedWarehouseDetailsCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedWarehouseDetailsCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void setDefaultPeriod()
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Articles Report";
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
                    Processing();

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    //activityTableView.ColumnHeaderStyle.Setters["FontBold"] = true;
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Rotate Columns...
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;

                    DevExpress.Spreadsheet.CellRange colRangeRow1 = worksheet.Rows[0];
                    colRangeRow1.Font.Bold = true;
                    control.SaveDocument();

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            if(e.ColumnFieldName.ToLowerInvariant().Contains("price"))
            {
                e.Formatting.NumberFormat = "\"" + GeosApplication.Instance.WMSCurrentCurrencySymbol + "\"" + " #,##,##0.00";
            }
            e.Handled = true;
        }

        private void PrintWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Processing();

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

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method PrintWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Processing();
                IsBusy = false;

                SearchString = string.Empty;
                FillGridList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkLogReoprtAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkLogReoprtAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowGridViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillGridList();
                IsGridViewVisible = Visibility.Visible;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #endregion
    }
}
