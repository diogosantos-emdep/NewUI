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
using Emdep.Geos.UI.Commands;
using DevExpress.Export.Xl;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.UI.Helper;
using DevExpress.Data.Filtering.Helpers;
using Emdep.Geos.Utility;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class PendingArticleViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        private ObservableCollection<Article> listArticle;
        private bool isBusy;
        private string myFilterString;
        private int selectedTileIndex;
        private ObservableCollection<TileBarFilters> listofitem;
        private bool isEdit;

        private string userSettingsKey = "SRM_Article_";
        private TileBarFilters selectedFilter;
        private int visibleRowCount;
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

        public string CustomFilterStringName { get; set; }

        private List<GridColumn> GridColumnList;

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
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

        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
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
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }

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
        public PendingArticleViewModel()
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
                CommandShowFilterPopupClick = new DelegateCommand<object>(ArticleShowFilterValue);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);

                FillArticleList();
                TileBarArrange();
                AddCustomSetting();
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
                ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                ViewPurchaseOrderView viewPurchaseOrderView = new ViewPurchaseOrderView();
                EventHandler handle = delegate { viewPurchaseOrderView.Close(); };
                viewPurchaseOrderViewModel.RequestClose += handle;

                Article apo = (Article)detailView.DataControl.CurrentItem;
                //Article apo = obj as Article;

                //   List<Warehouses> selectedwarehouselist = SRMCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                //Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == apo.WarehousePurchaseOrder.IdWarehouse);
                SRM.SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == apo.Warehouse.IdWarehouse);
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                viewPurchaseOrderViewModel.Init(apo.WarehousePurchaseOrder, warehouse);
                viewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                viewPurchaseOrderView.Owner = Window.GetWindow(ownerInfo);
                viewPurchaseOrderView.ShowDialog();
                //if (!viewPurchaseOrderViewModel.IsMoreNeeded)
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                Article article = (Article)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == article.Warehouse.IdWarehouse); 
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(article.Reference, warehouseId, warehouse, ArticleSleepDays);
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
            if (!SRMCommon.Instance.IsWarehouseChangedEventCanOccur)
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
                    return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            //fill data as per selected warehouse
            FillArticleList();
            TileBarArrange();
            AddCustomSetting();
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
                //GEOS2-4402 Sudhir.Jangra 03/05/2023
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    ObservableCollection<Article> TempMainArticleList = new ObservableCollection<Article>();
                    ListArticle = new ObservableCollection<Article>();
                    long plantOwnerIds;
                    string plantConnection;
                    foreach (var item in plantOwners)
                    {
                        plantOwnerIds = item.IdWarehouse;
                        plantConnection = item.Company.ConnectPlantConstr;
                        SRMService = new SRMServiceController((item != null && item.Company.ServiceProviderUrl != null) ? item.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        TempMainArticleList = new ObservableCollection<Article>(SRMService.GetPendingArticles_V2390(plantOwnerIds, plantConnection));
                        if(TempMainArticleList!=null)
                        {
                            TempMainArticleList.ToList().ForEach(i => i.Warehouse = item);
							ListArticle.AddRange(TempMainArticleList);
                        }
                        
                    }
                }
                else
                {
                    ListArticle = new ObservableCollection<Article>();
                }
                #region 
                //if (SRMCommon.Instance.Selectedwarehouse != null)
                //{
                //    //[001] [warehouse] Changed service method GetPendingArticles_V2034 to GetPendingArticles_V2038
                //    //[001] [warehouse] Changed service method GetPendingArticles_V2038 to [SRMService] GetPendingArticles
                //    ListArticle = new ObservableCollection<Article>(SRMService.GetPendingArticles(SRMCommon.Instance.Selectedwarehouse));
                //}
                //else
                //{
                //    ListArticle = new ObservableCollection<Article>();
                //}
                #endregion
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                //fill data as per selected warehouse
                FillArticleList();
                TileBarArrange();
                detailView.SearchString = null;
                AddCustomSetting();
                MyFilterString = string.Empty;
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
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

        private void TileBarArrange()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();


                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    EntitiesCount = ListArticle.Count(),
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });
                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_OnTime").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_OnTime").ToString()),
                    EntitiesCount = ListArticle.Where(a => a.WarehousePurchaseOrder.Delay >= 0).Count(),
                    BackColor = "Green",
                    ForeColor = "Green",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });
                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_Delayed").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_Delayed").ToString()),
                    EntitiesCount = ListArticle.Where(a => a.WarehousePurchaseOrder.Delay < 0).Count(),
                    BackColor = "Red",
                    ForeColor = "Red",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });
                //if (MainPurchaseOrderList == null)
                //{
                //    MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                //}
                //List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b => b.WorkflowStatus).ToList();
                //if (StatusList != null)
                //{
                //    foreach (WorkflowStatus Status in StatusList)
                //    {
                //        if (!Listofitem.Any(a => a.DisplayText == Status.Name))
                //        {
                //            Listofitem.Add(new TileBarFilters()
                //            {
                //                Caption = Status.Name,
                //                DisplayText = Status.Name,
                //                EntitiesCount = MainPurchaseOrderList.Count(x => x.IdWorkflowStatus == Status.IdWorkflowStatus),
                //                BackColor = Status.HtmlColor,
                //                EntitiesCountVisibility = Visibility.Visible,
                //                Height = 80,
                //                width = 200
                //            });
                //        }
                //    }
                //}

                //Listofitem.Add(new TileBarFilters()
                //{
                //    Caption = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                //    DisplayText = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
                //    EntitiesCount = MainPurchaseOrderList.Count(x => !x.IsPartialPending),
                //    ImageUri = "NotStarted.png"
                //});

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

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
        private void ArticleShowFilterValue(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleShowFilterValue....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                    string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                    CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    if (str == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            MyFilterString = _FilterString;
                        else
                            MyFilterString = string.Empty;
                    }
                    else if (str.Equals("All"))
                    {
                        MyFilterString = string.Empty;
                    }
                    else if (str.Equals(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_OnTime").ToString()))
                    {
                        MyFilterString = "[WarehousePurchaseOrder.Delay] >= 0 And [WarehousePurchaseOrder.Delay] Is Not Null";
                    }
                    else if (str.Equals(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_Delayed").ToString()))
                    {
                        MyFilterString = "[WarehousePurchaseOrder.Delay] < 0 And [WarehousePurchaseOrder.Delay] Is Not Null";
                    }


                    //string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;

                    //if (str.Equals(string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString())))
                    //{
                    //    ListPurchaseOrder = MainPurchaseOrderList;
                    //    SelectedPurchaseOrder = ListPurchaseOrder.FirstOrDefault();
                    //    MyFilterString = string.Empty;
                    //}
                    //else
                    //{
                    //    MyFilterString = "[WorkflowStatus.Name] In ('" + str + "')";
                    //}
                }

                GeosApplication.Instance.Logger.Log("Method ArticleShowFilterValue....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleShowFilterValue...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);

                if (CustomFilterStringName != null)
                {
                    if (CustomFilterStringName.Equals(string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_OnTime").ToString()))
                        || CustomFilterStringName.Equals(string.Format(System.Windows.Application.Current.FindResource("SRMPendingArticleFilter_Delayed").ToString())))
                    {
                        return;
                    }
                }

                if (CustomFilterStringName == "CUSTOM FILTERS" || CustomFilterStringName == "All")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Template"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    foreach (var item in tempUserSettings)
                    {
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Article)), item.Value, false);
                        List<Article> tempList = new List<Article>();

                        foreach (var po in ListArticle)
                        {
                            if (evaluator.Fit(po))
                                tempList.Add(po);
                        }

                        MyFilterString = item.Value;
                        Listofitem.Add(new TileBarFilters()
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
                Views.CustomFilterEditorView customFilterEditorView = new Views.CustomFilterEditorView();
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

                customFilterEditorViewModel.Init(e.FilterControl, Listofitem);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        Listofitem.Remove(tileBarItem);
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
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = ((DevExpress.Xpf.Grid.GridViewBase)e.OriginalSource).Grid.VisibleRowCount;
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
                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = ((DevExpress.Xpf.Grid.GridViewBase)e.OriginalSource).Grid.VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = Listofitem.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion // Methods
    }
}
