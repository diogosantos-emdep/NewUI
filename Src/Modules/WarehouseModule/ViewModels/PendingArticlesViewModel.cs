using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using System.Windows;
using System.Windows.Media;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.UI.Commands;
using DevExpress.Export.Xl;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.CustomControls;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PendingArticlesViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }



        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        #endregion // Services

        #region Declaration
        private ObservableCollection<Article> listArticle;
        private bool isBusy;
        private string myFilterString;
        #endregion // Declaration

        #region Properties
        public long WarehouseId { get; set; }
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
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
        public ObservableCollection<Article> ListArticle
        {
            get
            {
                return listArticle;
            }

            set
            {
                listArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListArticle"));
            }
        }
        #endregion // Properties

        #region Commands

        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshPendingArticleViewCommand { get; set; }
        public ICommand PrintPendingArticleViewCommand { get; set; }
        public ICommand ExportPendingArticleViewCommand { get; set; }
        public ICommand ViewArticleHyperlinkClickCommand { get; private set; }
        public ICommand PendingArticleViewPOHyperlinkClickCommand { get; set; }



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
        public PendingArticlesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PendingArticlesViewModel....", category: Category.Info, priority: Priority.Low);

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
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                RefreshPendingArticleViewCommand = new RelayCommand(new Action<object>(RefreshPendingArticleList));
                PrintPendingArticleViewCommand = new RelayCommand(new Action<object>(PrintPendingArticleList));
                ExportPendingArticleViewCommand = new RelayCommand(new Action<object>(ExportPendingArticleList));
                PendingArticleViewPOHyperlinkClickCommand = new DelegateCommand<object>(PendingArticleViewPOHyperlinkClickAction);
                ViewArticleHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(ViewArticleHyperlinkClickCommandAction);

                FillArticleList();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor PendingArticlesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PendingArticlesViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion // Constructor

        #region Methods
        /// <summary>
        /// Method to Open PO on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void PendingArticleViewPOHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingArticleViewPOHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                PurchaseOrderItemDetailsViewModel purchaseOrderItemDetailsViewModel = new PurchaseOrderItemDetailsViewModel();
                PurchaseOrderItemDetailsView purchaseOrderItemDetailsView = new PurchaseOrderItemDetailsView();
                EventHandler handle = delegate { purchaseOrderItemDetailsView.Close(); };
                purchaseOrderItemDetailsViewModel.RequestClose += handle;

                Article apo = (Article)detailView.DataControl.CurrentItem;
                //Article apo = obj as Article;

                //   List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                //Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == apo.WarehousePurchaseOrder.IdWarehouse);
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                purchaseOrderItemDetailsViewModel.Init(apo.WarehousePurchaseOrder.IdWarehousePurchaseOrder, warehouse);
                purchaseOrderItemDetailsView.DataContext = purchaseOrderItemDetailsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                purchaseOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                purchaseOrderItemDetailsView.ShowDialog();
                //if (!purchaseOrderItemDetailsViewModel.IsMoreNeeded)
                //{
                //    var item = ListPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                // ListPurchaseOrder.Remove(item);
                //    ListPurchaseOrder = new List<WarehousePurchaseOrder>(ListPurchaseOrder);
                //}
                GeosApplication.Instance.Logger.Log("Method PendingArticleViewPOHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingArticleViewPOHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// This method Open article detail click on article reference - By Amit
        /// Sprint-46 Task
        /// </summary>
        /// <param name="obj"></param>
        public void ViewArticleHyperlinkClickCommandAction(object obj)
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
                Article article = (Article)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method WarehouseViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WarehouseViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
            FillArticleList();
            MyFilterString = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Fill Article List
        /// [001][cpatil][03-01-2020][GEOS2-1794]New columns in PO Pending Articles
        /// </summary>
        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList....", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    //[001] Changed service method GetPendingArticles_V2034 to GetPendingArticles_V2038
                    // shubham[skadam] GEOS2-3547 In Pending Reception Change the way how the PO Expected delivery date is calculated when we have PO items with Expected Delivery Dates info  12 Aug 2022
                    ListArticle = new ObservableCollection<Article>(WarehouseService.GetPendingArticles_V2300(WarehouseCommon.Instance.Selectedwarehouse));

                }
                else
                {
                    ListArticle = new ObservableCollection<Article>();
                }

                GeosApplication.Instance.Logger.Log("Method FillArticleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillArticleList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void RefreshPendingArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingArticleList()...", category: Category.Info, priority: Priority.Low);
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

                //fill data as per selected warehouse
                FillArticleList();
                MyFilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPendingArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingArticleList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingArticleListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingArticleListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPendingArticleList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingArticleList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Article List";
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
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion // Methods
    }
}
