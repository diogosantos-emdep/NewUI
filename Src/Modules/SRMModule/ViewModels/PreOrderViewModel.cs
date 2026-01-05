using DevExpress.Data.Filtering;
using DevExpress.Diagram.Core.Shapes.Native;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Docking.Platform;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
    public class PreOrderViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null)
          ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        private string myFilterString;
        private bool isBusy;
        private bool isWorkOrderColumnChooserVisible;
        public string ReOrderGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "WMM_PreOrderGridSetting.Xml";
        private ObservableCollection<PreOrder> reOrderList;
        private PreOrder selectedList;
        Visibility isCalendarVisible;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";
        private string fromDate;
        private string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        #endregion // Declaration

        #region Properties
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
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }
        public ObservableCollection<PreOrder> ReOrderList
        {
            get { return reOrderList; }
            set
            {
                reOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReOrderList"));
            }
        }
        PreOrder selectedReOrder;
        public PreOrder SelectedReOrder
        {
            get { return selectedReOrder; }
            set
            {
                selectedReOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReOrder"));
            }
        }
        public long WarehouseId { get; set; }
        #endregion // Properties

        #region Commands
        public ICommand OpenGraphCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand OpenPOCommand { get; set; }
        public ICommand AddPreOrderCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RefreshReOrderViewCommand { get; set; }
        public ICommand PrintReOrderViewCommand { get; set; }
        public ICommand ExportReOrderViewCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; set; }

        public ICommand ViewCodeHyperlinkClickCommand { get; set; }
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
        public PreOrderViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor ReOrderViewModel....", category: Category.Info, priority: Priority.Low);
            try
            {              
                RefreshReOrderViewCommand = new RelayCommand(new Action<object>(RefreshReOrderList));
                PrintReOrderViewCommand = new RelayCommand(new Action<object>(PrintReOrderList));
                ExportReOrderViewCommand = new RelayCommand(new Action<object>(ExportReOrderList));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                OpenPOCommand = new RelayCommand(new Action<object>(OpenPOCommandAction));
                AddPreOrderCommand = new RelayCommand(new Action<object>(AddPreOrderCommandAction));
                CommandWarehouseEditValueChanged = new DelegateCommand<object>(CommandWarehouseEditValueChangedCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                ViewCodeHyperlinkClickCommand = new RelayCommand(new Action<object>(ViewCodeHyperlinkClickCommandAction));
                OpenGraphCommand = new DelegateCommand<object>(OpenGraphCommandAction);
                MyFilterString = string.Empty;
                setDefaultPeriod();
                IsButtonStatus = 7;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ReOrderViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ReOrderViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor Method ReOrderViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Constructor

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                ShowProcessing();
                FillReOrderList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void ShowProcessing()
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
        }
        private void FillReOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReOrderList....", category: Category.Info, priority: Priority.Low);
                if (SRMCommon.Instance.Selectedwarehouse != null)
                {
                    SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    ReOrderList = new ObservableCollection<PreOrder>(SRMService.GetAllPreOrder_V2680(SRMCommon.Instance.Selectedwarehouse,DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));
                }
                else
                {
                    ReOrderList = new ObservableCollection<PreOrder>();
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method FillReOrderList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReOrderList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillReOrderList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandWarehouseEditValueChangedCommandAction(object obj)
        {
            try
            {
                ShowProcessing();
                FillReOrderList();
                CloseSplashScreen();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CommandWarehouseEditValueChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                CloseSplashScreen();
            }

        }
        private void CloseSplashScreen()
        {
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }

        //[GEOS2-8252][31.10.2025][rdixit]
        private void ViewCodeHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....", category: Category.Info, priority: Priority.Low);

                //AddEditPreOrderViewModel addEditPreOrderModel = new AddEditPreOrderViewModel();
                //AddEditPreOrderView addEditPreOrderView = new AddEditPreOrderView();
                //EventHandler handle = delegate { addEditPreOrderView.Close(); };
                //addEditPreOrderModel.RequestClose += handle;
                //addEditPreOrderView.DataContext = addEditPreOrderModel;
                //var ownerInfo = (obj as FrameworkElement);
                //addEditPreOrderView.Owner = Window.GetWindow(ownerInfo);
                //addEditPreOrderModel.IsEnabled = false;
                //addEditPreOrderModel.WindowHeader = Application.Current.FindResource("EditPreOrder").ToString();
                //addEditPreOrderModel.EditInit(SelectedReOrder);
                //addEditPreOrderView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddPreOrderCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddPreOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....", category: Category.Info, priority: Priority.Low);

                AddEditPreOrderViewModel addEditPreOrderModel = new AddEditPreOrderViewModel();
                AddEditPreOrderView addEditPreOrderView = new AddEditPreOrderView();
                EventHandler handle = delegate { addEditPreOrderView.Close(); };
                addEditPreOrderModel.RequestClose += handle;
                addEditPreOrderView.DataContext = addEditPreOrderModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditPreOrderView.Owner = Window.GetWindow(ownerInfo);
                addEditPreOrderModel.IsEnabled = true;
                addEditPreOrderModel.WindowHeader = Application.Current.FindResource("AddPreOrder").ToString();
                addEditPreOrderModel.Init();
                addEditPreOrderView.ShowDialog();
                ShowProcessing();
                FillReOrderList();
                MyFilterString = string.Empty;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddPreOrderCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[rdixit][GEOS2-8247][04.11.2025]
        private void OpenGraphCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....", category: Category.Info, priority: Priority.Low);

                ArticleStockGraphViewModel articleStockGraphViewModel = new ArticleStockGraphViewModel();
                ArticleStockGraphView articleStockGraphView = new ArticleStockGraphView();
                EventHandler handle = delegate { articleStockGraphView.Close(); };
                articleStockGraphViewModel.RequestClose += handle;
                articleStockGraphView.DataContext = articleStockGraphViewModel;
                //var ownerInfo = (obj as FrameworkElement);
                //articleStockGraphView.Owner = Window.GetWindow(ownerInfo);
                //articleStockGraphViewModel.Init(SelectedReOrder);
                //articleStockGraphViewModel.LoadExcelToDataTable("C:\\Users\\rdixit\\Downloads\\ArticleStockGraphData.xlsx");
                //articleStockGraphView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method AddPreOrderCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddPreOrderCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void OpenPOCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPOCommandAction....", category: Category.Info, priority: Priority.Low);

                WarehousePurchaseOrder wpo = (WarehousePurchaseOrder)obj;
                ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                ViewPurchaseOrderView ViewPurchaseOrderView = new ViewPurchaseOrderView();

                EventHandler handle = delegate { ViewPurchaseOrderView.Close(); };
                viewPurchaseOrderViewModel.RequestClose += handle;

                var POWarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == wpo.IdWarehouse);
                SRMService = new SRMServiceController((POWarehouse != null && POWarehouse.Company.ServiceProviderUrl != null) ? POWarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                wpo.IdWarehousePurchaseOrder = SRMService.GetPOIdByCode(POWarehouse, wpo.Code);            
                viewPurchaseOrderViewModel.Init(wpo, POWarehouse);
                ViewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                ViewPurchaseOrderView.ShowDialog();            
             
                GeosApplication.Instance.Logger.Log("Method OpenPOCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPOCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void RefreshReOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshReOrderList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                IsBusy = true;
                ShowProcessing();
                FillReOrderList();
                MyFilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshReOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshReOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintReOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintReOrderList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                ShowProcessing();

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ReOrderPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ReOrderPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                PrintTool printTool = new PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintReOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintReOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportReOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportReOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Preorder List";
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
                    ShowProcessing();
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

                    GeosApplication.Instance.Logger.Log("Method ExportReOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportReOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(ReOrderGridSettingFilePath))
                {
                    ((GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ReOrderGridSettingFilePath);
                    GridControl GridControlSTDetails = ((GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ReOrderGridSettingFilePath);

                GridControl gridControl = ((GridViewBase)obj.OriginalSource).Grid;
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

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((FrameworkContentElement)sender).Parent).SaveLayoutToXml(ReOrderGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((FrameworkContentElement)sender).Parent).SaveLayoutToXml(ReOrderGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
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
        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                ShowProcessing();

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
                else if (IsButtonStatus == 8)//this year
                {
                    FromDate = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
                    ToDate = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
                }
                else if (IsButtonStatus == 9)//last year
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

                Init();

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
        private void setDefaultPeriod()
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = DateTime.Now.AddMonths(-12);
                DateTime EndToDate = DateTime.Now;
                StartDate = StartFromDate;
                EndDate = EndToDate;
                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-8112][rdixit][05.08.2025]
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);
            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "PO")
                {
                    if (ReOrderList != null)
                    {
                        foreach (var dataObject in ReOrderList)
                        {
                            if (dataObject.PoList == null)
                            {
                                continue;
                            }

                            if (dataObject.PO != null)
                            {
                                if (dataObject.PO.Contains("\n"))
                                {
                                    string Polist = dataObject.PO;

                                    for (int index = 0; index < Polist.Length; index++)
                                    {
                                        string po = Polist.Split('\n').First();

                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == po))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = po;
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PO Like '%{0}%'", po));
                                            filterItems.Add(customComboBoxItem);
                                        }
                                        if (Polist.Contains("\n"))
                                            Polist = Polist.Remove(0, po.Length + 1);
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    string po = ReOrderList.FirstOrDefault(y => y.PO == dataObject.PO)?.PO;
                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == po))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = po;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PO Like '%{0}%'", po));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
               
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion
    }
}
