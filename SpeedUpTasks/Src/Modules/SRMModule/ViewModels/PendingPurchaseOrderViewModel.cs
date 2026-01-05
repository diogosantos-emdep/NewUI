using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
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
using Emdep.Geos.Modules.SRM.Views;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.SRM;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class PendingPurchaseOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


        //ISRMService HrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Properties

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        private bool isBusy;
        private ObservableCollection<TileBarFilters> listofitem;
        private string myFilterString;
        private int selectedTileIndex;
        private List<WarehousePurchaseOrder> mainPurchaseOrderList;
        private List<WarehousePurchaseOrder> listPurchaseOrder;
        private WarehousePurchaseOrder selectedPurchaseOrder;





        #endregion

        #region Declarations

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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

        public WarehousePurchaseOrder SelectedPurchaseOrder
        {
            get
            {
                return selectedPurchaseOrder;
            }

            set
            {
                selectedPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPurchaseOrder"));
            }
        }


        #endregion



        #region ICommand

        public ICommand RefreshPurchaseOrderViewCommand { get; set; }
        public ICommand PrintPurchaseOrderViewCommand { get; set; }
        public ICommand ExportPurchaseOrderViewCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }

        public ICommand CommandWarehouseEditValueChanged { get; private set; }






        #endregion

        #region Constructor

        public PendingPurchaseOrderViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PendingPurchaseOrderViewModel....", category: Category.Info, priority: Priority.Low);

                //isInIt = true;

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

                RefreshPurchaseOrderViewCommand = new RelayCommand(new Action<object>(RefreshPurchaseOrderList));
                PrintPurchaseOrderViewCommand = new RelayCommand(new Action<object>(PrintPurchaseOrderList));
                ExportPurchaseOrderViewCommand = new RelayCommand(new Action<object>(ExportPurchaseOrderList));
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));

                CommandShowFilterPopupClick = new DelegateCommand<object>(LeadsShowFilterValue);
                CommandGridDoubleClick = new DelegateCommand<object>(PendingReceptionItemsWindowShow);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);


                //Fill data as per selected warehouse
                FillMainPurchaseOrderList();

                //Rearrange tile Arrange
                TileBarArrange(MainPurchaseOrderList);

                MyFilterString = string.Empty;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor PendingPurchaseOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PendingPurchaseOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }



        #endregion

        #region Methods

        public void Init()
        {
            
        }

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
                TileBarArrange(MainPurchaseOrderList);
                MyFilterString = string.Empty;
                detailView.SearchString = null;

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }


                //ViewPurchaseOrderView viewPurchaseOrderView = new ViewPurchaseOrderView();
                //ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                //EventHandler handle = delegate { viewPurchaseOrderView.Close(); };
                //viewPurchaseOrderViewModel.RequestClose += handle;
                //viewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                //viewPurchaseOrderView.ShowDialog();



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

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();

                documentViewModel.OpenPdf(SelectedPurchaseOrder, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                        SelectedPurchaseOrder = ListPurchaseOrder.FirstOrDefault();
                        MyFilterString = string.Empty;
                    }
                    else
                    {
                        MyFilterString = "[WorkflowStatus.Name] In ('" + str + "')";
                    }
                }

                GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LeadsShowFilterValue...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PendingReceptionItemsWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingReceptionItemsWindowShow....", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                ViewPurchaseOrderView ViewPurchaseOrderView = new ViewPurchaseOrderView();

                EventHandler handle = delegate { ViewPurchaseOrderView.Close(); };
                viewPurchaseOrderViewModel.RequestClose += handle;

                WarehousePurchaseOrder wpo = (WarehousePurchaseOrder)detailView.FocusedRow;
                Warehouses warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;
                viewPurchaseOrderViewModel.Init(wpo.IdWarehousePurchaseOrder, warehouse);
                ViewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                ViewPurchaseOrderView.Owner = Window.GetWindow(ownerInfo);
                ViewPurchaseOrderView.ShowDialog();

                if(viewPurchaseOrderViewModel.IsSaveChanges==true)
                {
                    wpo.IdWorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus.IdWorkflowStatus;
                    wpo.WorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus;

                    TileBarArrange(MainPurchaseOrderList);
                }

                if (!viewPurchaseOrderViewModel.IsMoreNeeded)
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
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);
            //When setting the warehouse from default the data should not be refreshed
            if (!SRMCommon.Instance.IsWarehouseChangedEventCanOccur)
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
            TileBarArrange(MainPurchaseOrderList);

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        private void TileBarArrange(List<WarehousePurchaseOrder> MainPurchaseOrderList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();


                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    EntitiesCount = MainPurchaseOrderList.Count()
                    //ImageUri = "AllTasks.png"
                });
                if(MainPurchaseOrderList==null)
                {
                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                }
                List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b=>b.WorkflowStatus).ToList();
                if (StatusList != null)
                {
                    foreach (WorkflowStatus Status in StatusList)
                    {
                        if (!Listofitem.Any(a => a.DisplayText == Status.Name))
                        {
                            Listofitem.Add(new TileBarFilters()
                            {
                                Caption = Status.Name,
                                DisplayText = Status.Name,
                                EntitiesCount = MainPurchaseOrderList.Count(x => x.IdWorkflowStatus == Status.IdWorkflowStatus),
                                BackColor = Status.HtmlColor
                            });
                        }
                    }
                }

                //Listofitem.Add(new TileBarFilters()
                //{
                //    Caption = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                //    DisplayText = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                //    EntitiesCount = MainPurchaseOrderList.Count(x => !x.IsPartialPending),
                //    ImageUri = "NotStarted.png"
                //});

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

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void FillMainPurchaseOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList...", category: Category.Info, priority: Priority.Low);

                if (SRM.SRMCommon.Instance.Selectedwarehouse != null)
                {
                    try
                    {
                        MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                        Warehouses Warehouse1 = SRM.SRMCommon.Instance.Selectedwarehouse;

                        var TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse(Warehouse1));
                       // var TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReceptionByWarehouse_V2035(Warehouse1));

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

        #endregion


    }
}
