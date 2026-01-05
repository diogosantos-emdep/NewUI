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
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WOPendingArticlesViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion // Services

        #region Declaration

        private ObservableCollection<OtItem> WoPendingArticlelist;
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

        public ObservableCollection<OtItem> WOPendingArticlelist
        {
            get
            {
                return WoPendingArticlelist;
            }

            set
            {
                WoPendingArticlelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WOPendingArticlelist"));
            }
        }

        #endregion // Properties

        #region Commands

        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand RefreshWOPendingArticleViewCommand { get; set; }
        public ICommand PrintWOPendingArticleViewCommand { get; set; }
        public ICommand ExportWOPendingArticleViewCommand { get; set; }
        public ICommand ViewArticleHyperlinkClickCommand { get; private set; }
        public ICommand PendingArticleViewWOHyperlinkClickCommand { get; set; }

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
        public WOPendingArticlesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WOPendingArticlesViewModel....", category: Category.Info, priority: Priority.Low);

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
                RefreshWOPendingArticleViewCommand = new RelayCommand(new Action<object>(RefreshWOPendingArticleCommandAction));
                PrintWOPendingArticleViewCommand = new RelayCommand(new Action<object>(PrintWOPendingArticleCommandAction));
                ExportWOPendingArticleViewCommand = new RelayCommand(new Action<object>(ExportWOPendingArticleCommandAction));
                PendingArticleViewWOHyperlinkClickCommand = new DelegateCommand<object>(PendingArticleViewWOHyperlinkClickAction);
                ViewArticleHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(ViewArticleHyperlinkClickCommandAction);

                FillWOPendingArticleList();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor WOPendingArticlesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WOPendingArticlesViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method to Open Work order  on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void PendingArticleViewWOHyperlinkClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingArticleViewWOHyperlinkClickAction....", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;

                OtItem otItem = (OtItem)detailView.FocusedRow;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(otItem.IdOT, warehouse);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method PendingArticleViewWOHyperlinkClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingArticleViewWOHyperlinkClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// This method Open article detail click on article reference 
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

                OtItem otItem = (OtItem)detailView.DataControl.CurrentItem;

                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;

                //Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                //WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(otItem.RevisionItem.WarehouseProduct.Article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult)
                {
                    otItem.RevisionItem.WarehouseProduct.Article.MyWarehouse.MinimumStock = articleDetailsViewModel.MinimumQuantity;
                }

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
            FillWOPendingArticleList();
            MyFilterString = string.Empty;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Fill Article List
        /// [001][cpatil][04-12-2019][GEOS2-1867]Some articles does not appear in Work Orders->Pending Articles section
        ///  [002][smazhar][22-06-2020][GEOS2-2346]Add Country flag
        ///  [003][cpatil][02-11-2022][GEOS2-3960]
        ///  [004][cpatil][06-09-2023][GEOS2-4416]
        /// </summary>
        private void FillWOPendingArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWOPendingArticleList....", category: Category.Info, priority: Priority.Low);

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {

                    WOPendingArticlelist = new ObservableCollection<OtItem>();
                    Warehouses Warehouse = WarehouseCommon.Instance.Selectedwarehouse;


                    //[001] Changed service method GetPendingMaterialArticles_V2034 to GetPendingMaterialArticles_V2038
                    //  ObservableCollection<OtItem> TempListPendingArticle = new ObservableCollection<OtItem>(WarehouseService.GetPendingMaterialArticles_V2038(Warehouse));
                    ////[002] Changed service method
                    // [003] Changed service method
                    // [004] Changed service method
                    //ObservableCollection<OtItem> TempListPendingArticle = new ObservableCollection<OtItem>(WarehouseService.GetPendingMaterialArticles_V2430(Warehouse));
                    //Shubham[skadam] GEOS2-5817 Pending articles erroneos 09 08 2024
                    ObservableCollection<OtItem> TempListPendingArticle = new ObservableCollection<OtItem>(WarehouseService.GetPendingMaterialArticles_V2550(Warehouse));
                    if (TempListPendingArticle != null)
                    {
                        foreach (var otitem in TempListPendingArticle.GroupBy(tpa => tpa.Ot.Quotation.Site.Country.Iso))
                        {

                            ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().Ot.Quotation.Site.Country.CountryIconBytes);
                            otitem.ToList().Where(oti => oti.Ot.Quotation.Site.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.Ot.Quotation.Site.Country.CountryIconImage = countryFlagImage);
                        }
                    }
                    WOPendingArticlelist.AddRange(TempListPendingArticle);
                }
                else
                {
                    WoPendingArticlelist = new ObservableCollection<OtItem>();
                }

                GeosApplication.Instance.Logger.Log("Method FillWOPendingArticleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWOPendingArticleList() " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource      
        ///[001][smazhar][22-06-2020][GEOS2-2346]Add Country flag
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

        public void RefreshWOPendingArticleCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWOPendingArticleList()...", category: Category.Info, priority: Priority.Low);
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
                FillWOPendingArticleList();
                MyFilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWOPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWOPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintWOPendingArticleCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWOPendingArticleList()...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.Logger.Log("Method PrintWOPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWOPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWOPendingArticleCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWOPendingArticleList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Order Pending Article ";
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

                    GeosApplication.Instance.Logger.Log("Method ExportWOPendingArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWOPendingArticleList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods


    }
}
