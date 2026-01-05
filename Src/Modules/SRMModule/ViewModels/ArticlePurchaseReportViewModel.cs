using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using DevExpress.Mvvm.UI;
using System.Windows.Input;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Prism.Logging;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.SRM.Views;
using System.Windows.Controls;
using System.Windows.Media;
using Emdep.Geos.Data.Common.SRM;


namespace Emdep.Geos.Modules.SRM.ViewModels
{

    public class ArticlePurchaseReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        // [nsatpute][21-01-2025][GEOS2-5725]
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Service
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        ISRMService SRMReportService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMReportService = new SRMServiceController("localhost:6699");
        #endregion

        #region  public event
        // [nsatpute][21-01-2025][GEOS2-5725]
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
        bool isBusy;
        ObservableCollection<ArticlePurchasing> purchasingReportList;
        #endregion

        #region  public Properties

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }



        public ObservableCollection<ArticlePurchasing> PurchasingReportList
        {
            get { return purchasingReportList; }
            set
            {
                purchasingReportList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PurchasingReportList"));
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


        #endregion

        #region Constructor

        public ArticlePurchaseReportViewModel(DateTime fromDate, DateTime toDate, long idArticleSupplier, int idArticle, List<object> SelectedItems)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ArticlePurchaseReportViewModel ...", category: Category.Info, priority: Priority.Low);
                Processing();
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintWorkLogReoprtAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWorkLogReoprtAction));
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(SelectedWarehouseDetailsCommandAction);
                PurchasingReportList = new ObservableCollection<ArticlePurchasing>();
                foreach(object  item in SelectedItems)
                {
                    Warehouses wr = (Warehouses)item;
                    SRMReportService = new SRMServiceController(wr.Company.ServiceProviderUrl);
                    //SRMReportService = new SRMServiceController("localhost:6699");
                    //PurchasingReportList.AddRange(SRMReportService.GetPurchasingReport(fromDate, toDate, idArticleSupplier, idArticle, wr.IdWarehouse));
                    PurchasingReportList.AddRange(SRMReportService.GetPurchasingReport_V2680(fromDate, toDate, idArticleSupplier, idArticle, wr.IdWarehouse)); //[pallavi.kale][GEOS2-9558][17.10.2025]
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Constructor ArticlePurchaseReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ArticlePurchaseReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods


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
                    win.Topmost = true;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }
        private void SelectedWarehouseDetailsCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedWarehouseDetailsCommandAction...", category: Category.Info, priority: Priority.Low);


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedWarehouseDetailsCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedWarehouseDetailsCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        private void ExportWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Purchasing List";
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
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
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
        #endregion

    }
}
