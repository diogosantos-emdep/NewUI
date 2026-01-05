using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Prism.Logging;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Modules.SRM.Views;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class ManufacturersReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {   /// <summary>
        /// ///[pramod.misal][20-05-2025][GEOS2-5727]https://helpdesk.emdep.com/browse/GEOS2-5727
        /// </summary>
        

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
        ObservableCollection<ArticlePurchasing> manufacturersReportList;
        #endregion


        #region  public Properties

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

     

        public ObservableCollection<ArticlePurchasing> ManufacturersReportList
        {
            get { return manufacturersReportList; }
            set
            {
                manufacturersReportList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManufacturersReportList"));
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

        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }



        #endregion

        #region Constructor
        public ManufacturersReportViewModel(DateTime fromDate, DateTime toDate, int Article, List<object> SelectedItems)
        {
            try
            {
                Processing();
                GeosApplication.Instance.Logger.Log("Constructor ArticleReceptionReportViewModel ...", category: Category.Info, priority: Priority.Low);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintWorkLogReoprtAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWorkLogReoprtAction));
                ManufacturersReportList = new ObservableCollection<ArticlePurchasing>();

                foreach (object item in SelectedItems)
                {
                    Warehouses wr = (Warehouses)item;
                    //SRMReportService = new SRMServiceController(wr.Company.ServiceProviderUrl);
                    SRMReportService = new SRMServiceController("localhost:6699");

                    //ManufacturersReportList.AddRange(SRMReportService.GetManufacturingReport(fromDate, toDate, Article, wr.IdWarehouse));



                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Constructor ArticleReceptionReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ArticleReceptionReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion


        #region method

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

        private void ExportWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Manufacturers Report List";
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = true;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }


        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        public void Dispose()
        {

        }

        #endregion


    }
}
