using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
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
    public class PlanningSimulatorViewModel: INotifyPropertyChanged
    {
        #region TaskLog

        //[Sprint_66]__[GEOS2-1510]__[New section "Planning Simulator"]__[sdesai]

        #endregion

        #region Services

       IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Declaration
        private ObservableCollection<PlanningSimulator> planningSimulatorList;
        private PlanningSimulator selectedPlanningSimulator;
        #endregion

        #region Properties
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public ObservableCollection<PlanningSimulator> PlanningSimulatorList
        {
            get
            {
                return planningSimulatorList;
            }

            set
            {
                planningSimulatorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningSimulatorList"));
            }
        }
        public PlanningSimulator SelectedPlanningSimulator
        {
            get
            {
                return selectedPlanningSimulator;
            }

            set
            {
                selectedPlanningSimulator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlanningSimulator"));
            }
        }
        #endregion

        #region Public ICommands
        public ICommand CommandWarehouseEditValueChanged { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public ICommand ReferenceHyperlinkClickCommand { get; set; }
        public ICommand OtHyperlinkClickCommand { get; set; }
        public ICommand POHyperlinkClickCommand { get; set; }

        #endregion

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

        public PlanningSimulatorViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PlanningSimulatorViewModel()...", category: Category.Info, priority: Priority.Low);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportToExcelCommand = new RelayCommand(new Action<object>(ExportToExcelCommandAction));
                ReferenceHyperlinkClickCommand= new DelegateCommand<object>(ReferenceHyperlinkClickCommandAction);
                OtHyperlinkClickCommand = new DelegateCommand<object>(OtHyperlinkClickCommandAction);
                POHyperlinkClickCommand = new DelegateCommand<object>(POHyperlinkClickCommandAction);
                FillPlanningSimulatorList();
                GeosApplication.Instance.Logger.Log("Constructor PlanningSimulatorViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PlanningSimulatorViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                //When setting the warehouse from default the data should not be refreshed
                if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                    return;

                FillPlanningSimulatorList();
                GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseEditValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                detailView.SearchString = null;
                FillPlanningSimulatorList();
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction...", category: Category.Info, priority: Priority.Low);
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

            TableView planningSimulatorView = ((TableView)obj);
            planningSimulatorView.PrintAutoWidth = true;
            PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
            pcl.Margins.Bottom = 5;
            pcl.Margins.Top = 5;
            pcl.Margins.Left = 5;
            pcl.Margins.Right = 5;
            pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PlanningSimulatorReportPrintHeaderTemplate"];
            pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PlanningSimulatorReportPrintFooterTemplate"];
            pcl.Landscape = true;
            pcl.CreateDocument(false);

            DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
            window.PreviewControl.DocumentSource = pcl;
            window.Show();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        private void ExportToExcelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ExportToExcelCommandAction...", category: Category.Info, priority: Priority.Low);
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.DefaultExt = "xlsx";
            saveFile.FileName = "Planning Simulator";
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

                ResultFileName = saveFile.FileName;
                TableView planningSimulatorTableView = ((TableView)obj);
                planningSimulatorTableView.PrintAutoWidth = false;
                planningSimulatorTableView.ShowTotalSummary = false;
                planningSimulatorTableView.ShowFixedTotalSummary = false;
                planningSimulatorTableView.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });
                planningSimulatorTableView.ShowFixedTotalSummary = true;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                System.Diagnostics.Process.Start(ResultFileName);
                GeosApplication.Instance.Logger.Log("Method ExportToExcelCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
           
        }

        private void ReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReferenceHyperlinkClickCommandAction...", category: Category.Info, priority: Priority.Low);
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
                PlanningSimulator article = (PlanningSimulator)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ReferenceHyperlinkClickCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ReferenceHyperlinkClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OtHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OtHyperlinkClickCommandAction...", category: Category.Info, priority: Priority.Low);
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
                PlanningSimulator planningSimulator = (PlanningSimulator)detailView.Grid.SelectedItem;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(planningSimulator.IdOT, WarehouseCommon.Instance.Selectedwarehouse, planningSimulator.Reference);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method OtHyperlinkClickCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OtHyperlinkClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void POHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method POHyperlinkClickCommandAction...", category: Category.Info, priority: Priority.Low);
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
                PurchaseOrderItemDetailsViewModel purchaseOrderItemDetailsViewModel = new PurchaseOrderItemDetailsViewModel();
                PurchaseOrderItemDetailsView purchaseOrderItemDetailsView = new PurchaseOrderItemDetailsView();
                EventHandler handle = delegate { purchaseOrderItemDetailsView.Close(); };
                purchaseOrderItemDetailsViewModel.RequestClose += handle;
                PlanningSimulator planningSimulator = (PlanningSimulator)detailView.DataControl.CurrentItem;
                purchaseOrderItemDetailsViewModel.Init(planningSimulator.IdWarehousePurchaseOrder, WarehouseCommon.Instance.Selectedwarehouse);
                purchaseOrderItemDetailsView.DataContext = purchaseOrderItemDetailsViewModel;
                purchaseOrderItemDetailsView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method POHyperlinkClickCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POHyperlinkClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        ///  [001][cpatil][06-12-2019][GEOS2-1867]Some articles does not appear in Work Orders->Pending Articles section
        ///  [002][cpatil][23-09-2021][GEOS2-2174]Add new column "CurrentStock" in Planning Simulator
        /// </summary>
        private void FillPlanningSimulatorList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method POHyperlinkClickCommandAction...", category: Category.Info, priority: Priority.Low);
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

                //[001] Changed service method GetPlanningSimulator_V2034 to GetPlanningSimulator_V2038
                //[002] Changed service method GetPlanningSimulator_V2038 to GetPlanningSimulator_V2190
                PlanningSimulatorList = new ObservableCollection<PlanningSimulator>(WarehouseService.GetPlanningSimulator_V2190(WarehouseCommon.Instance.Selectedwarehouse));

                if (PlanningSimulatorList.Count > 0)
                    SelectedPlanningSimulator = PlanningSimulatorList[0];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method POHyperlinkClickCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
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
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POHyperlinkClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            } 
        }

        #endregion
    }
}
