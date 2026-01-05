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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class InventoryAuditViewModel: INotifyPropertyChanged
    {

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion


        #region  Task Log
       // [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region Declaration
        //[000] added
        private string searchString;

        private WarehouseInventoryAudit selectedInventoryAudit;

        private ObservableCollection<WarehouseInventoryAudit> warehouseInventoryAuditList;


        #endregion

        #region Properties
        //[000]added
        public virtual string ResultFileName { get; set; }
        public WarehouseInventoryAudit SelectedInventoryAudit
        {
            get
            {
                return selectedInventoryAudit;
            }

            set
            {
                selectedInventoryAudit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryAudit"));
            }
        }
        public ObservableCollection<WarehouseInventoryAudit> WarehouseInventoryAuditList
        {
            get
            {
                return warehouseInventoryAuditList;
            }

            set
            {
                warehouseInventoryAuditList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseInventoryAuditList"));
            }
        }

        public string SearchString
        {
            get { return searchString; }
            set { searchString = value; OnPropertyChanged(new PropertyChangedEventArgs("SearchString")); }
        }

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

        #endregion
        #region Commands
        //[000]added
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshInventoryAuditViewCommand { get; set; }

        public ICommand PrintInventoryAuditViewCommand { get; set; }
        public ICommand ExportInventoryAuditViewCommand { get; set; }

        public ICommand AddNewInventoryAuditCommand { get; private set; }

        public ICommand InventoryCommand { get; private set; }

        public ICommand CommandGridDoubleClick { get; set; }
        #endregion

        #region Constructor
        public InventoryAuditViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor InventoryAuditsViewModel()...", category: Category.Info, priority: Priority.Low);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(SelectedWarehouseDetailsCommandAction);
                RefreshInventoryAuditViewCommand = new RelayCommand(new Action<object>(RefreshInventoryAuditList));
                InventoryCommand = new RelayCommand(new Action<object>(Inventory));
                AddNewInventoryAuditCommand = new RelayCommand(new Action<object>(AddNewInventoryAudit));
                PrintInventoryAuditViewCommand = new RelayCommand(new Action<object>(PrintInventoryAuditList));
                ExportInventoryAuditViewCommand = new RelayCommand(new Action<object>(ExportInventoryAuditList));
                WarehouseInventoryAuditList = new ObservableCollection<WarehouseInventoryAudit>();
                CommandGridDoubleClick = new DelegateCommand<object>(OpenInventoryAuditDetailView);

                GeosApplication.Instance.Logger.Log("Constructor InventoryAuditsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
              
                GeosApplication.Instance.Logger.Log("Get an error in Constructor InventoryAuditsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Command Action 
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewInventoryAudit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method AddNewInventoryAudit ...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                AddNewInventoryAuditView addNewInventoryAuditsView = new AddNewInventoryAuditView();
                AddNewInventoryAuditViewModel addNewInventoryAuditsViewModel = new AddNewInventoryAuditViewModel();
                EventHandler handle = delegate { addNewInventoryAuditsView.Close(); };
                addNewInventoryAuditsViewModel.RequestClose += handle;
                addNewInventoryAuditsViewModel.Init();
                addNewInventoryAuditsView.DataContext = addNewInventoryAuditsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addNewInventoryAuditsView.Owner = Window.GetWindow(ownerInfo);
                addNewInventoryAuditsView.ShowDialogWindow();

                if (addNewInventoryAuditsViewModel.IsSave)
                {
                    WarehouseInventoryAuditList.Add(addNewInventoryAuditsViewModel.WarehouseInventoryAuditDetails);
                    SelectedInventoryAudit=addNewInventoryAuditsViewModel.WarehouseInventoryAuditDetails;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewInventoryAudit() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

              GeosApplication.Instance.Logger.Log("Get an error in AddNewInventoryAudit() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void Inventory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Inventory()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                InventoryAuditSelectView inventoryAuditSelectView = new InventoryAuditSelectView();
                InventoryAuditSelectViewModel inventoryAuditSelectViewModel = new InventoryAuditSelectViewModel();
                EventHandler handle = delegate { inventoryAuditSelectView.Close(); };
                inventoryAuditSelectViewModel.RequestClose += handle;
                inventoryAuditSelectViewModel.Init();
                inventoryAuditSelectView.DataContext = inventoryAuditSelectViewModel;
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                inventoryAuditSelectView.Owner = Window.GetWindow(ownerInfo);
                inventoryAuditSelectView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method Inventory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method Inventory()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// this method user for  refresh inventory audit list
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshInventoryAuditList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshInventoryAuditList()...", category: Category.Info, priority: Priority.Low);

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
                FillInventoryAuditDetails();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshInventoryAuditList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
              GeosApplication.Instance.Logger.Log("Get an error in Method RefreshInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  this method use for Inventory Audit list export to excel
        ///  [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void ExportInventoryAuditList(object obj)
        {
            
                try
                {
                    GeosApplication.Instance.Logger.Log("Method ExportInventoryAuditList...", category: Category.Info, priority: Priority.Low);

                    Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                    saveFile.DefaultExt = "xlsx";
                    saveFile.FileName = "Inventory Audit ";
                    saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    saveFile.FilterIndex = 1;
                    saveFile.Title = "Save Excel Report";
                    if (!(Boolean)saveFile.ShowDialog())
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                       // IsBusy = true;
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
                       // IsBusy = false;
                        activityTableView.ShowFixedTotalSummary = true;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        System.Diagnostics.Process.Start(ResultFileName);

                        GeosApplication.Instance.Logger.Log("Method ExportInventoryAuditList executed successfully.", category: Category.Info, priority: Priority.Low);
                    }
                }
                catch (Exception ex)
                {
                    //IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Get an error in Method ExportInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        /// <summary>
        /// this method use for print  Inventory Audit Report
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void PrintInventoryAuditList(object obj)
        {
            try
            {
                   GeosApplication.Instance.Logger.Log("Method PrintInventoryAuditList...", category: Category.Info, priority: Priority.Low);
                   // IsBusy = true;
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["InventoryAuditReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["InventoryAuditReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
               // IsBusy = false;
                window.Show();
                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    GeosApplication.Instance.Logger.Log("Method PrintInventoryAuditList executed successfully.", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    // IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in PrintInventoryAuditList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            
        }
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void OpenInventoryAuditDetailView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenInventoryAuditDetailView...", category: Category.Info, priority: Priority.Low);

                WarehouseInventoryAudit warehouseInventoryAudit = null;
                
                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    warehouseInventoryAudit = (WarehouseInventoryAudit)detailView.DataControl.CurrentItem;
                    SelectedInventoryAudit = warehouseInventoryAudit;
                }
                //else if (obj is WarehouseInventoryAudit)
                //{
                //    warehouseInventoryAudit = (WarehouseInventoryAudit)obj;
                //    SelectedInventoryAudit = warehouseInventoryAudit;
                //}

                if (warehouseInventoryAudit != null)
                {
                    EditInventoryAuditView editInventoryAuditView = new EditInventoryAuditView();
                    EditInventoryAuditViewModel editInventoryAuditViewModel = new EditInventoryAuditViewModel();

                    EventHandler handle = delegate { editInventoryAuditView.Close(); };
                    editInventoryAuditViewModel.RequestClose += handle;
                    editInventoryAuditViewModel.Init(warehouseInventoryAudit);
                    editInventoryAuditView.DataContext = editInventoryAuditViewModel;

                    // IsBusy = true;
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
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (obj as FrameworkElement);
                    editInventoryAuditView.Owner = Window.GetWindow(ownerInfo);
                    editInventoryAuditView.ShowDialog();
                    if (editInventoryAuditViewModel.IsUpdate)
                    {
                        warehouseInventoryAudit.StartDate = editInventoryAuditViewModel.UpdateWarehouseInventoryAuditDetails.StartDate ;
                        warehouseInventoryAudit.Name = editInventoryAuditViewModel.UpdateWarehouseInventoryAuditDetails.Name;
                        warehouseInventoryAudit.EndDate = editInventoryAuditViewModel.UpdateWarehouseInventoryAuditDetails.EndDate;
                    }
                }

              GeosApplication.Instance.Logger.Log("Method OpenInventoryAuditDetailView executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
              GeosApplication.Instance.Logger.Log("Get an error in Method OpenInventoryAuditDetailView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
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
                FillInventoryAuditDetails();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedWarehouseDetailsCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
               GeosApplication.Instance.Logger.Log("Get an error in Method SelectedWarehouseDetailsCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
      
        }

        #endregion

        #region Methods
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init....", category: Category.Info, priority: Priority.Low);

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
                FillInventoryAuditDetails();

              if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

             GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for fill  Inventory Audit list
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        private void FillInventoryAuditDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditDetails()...", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    try
                    {
                        WarehouseInventoryAuditList = new ObservableCollection<WarehouseInventoryAudit>(WarehouseService.GetAllWarehouseInventoryAudits(WarehouseCommon.Instance.Selectedwarehouse));
                        SelectedInventoryAudit = WarehouseInventoryAuditList.FirstOrDefault();
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                }
                else
                {
                    WarehouseInventoryAuditList = new ObservableCollection<WarehouseInventoryAudit>();
                }
              
                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditDetails() executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditDetails() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


    }
}
