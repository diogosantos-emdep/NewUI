using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SRM.Views;
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
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class ArticleSupplierViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISRMService SRMService = new SRMServiceController("localhost:6699"); 

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

        #region Declarations 

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        private bool isBusy;
        private List<ArticleSupplier> articleSupplierList;
        private ArticleSupplier selectedArticleSupplier;

        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public List<ArticleSupplier> ArticleSupplierList
        {
            get
            {
                return articleSupplierList;
            }

            set
            {
                articleSupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSupplierList"));
            }
        }

        public ArticleSupplier SelectedArticleSupplier
        {
            get
            {
                return selectedArticleSupplier;
            }

            set
            {
                selectedArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplier"));
            }
        }


        #endregion

        #region ICommand

        public ICommand RefreshCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand GridDoubleClickCommand { get; set; }

        public ICommand WarehouseEditValueChangedCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }

        public ICommand AddNewCompaniesCommand { get; set; }//[Sudhir.jangra][GEOS2-4738][26/09/2023]

        #endregion

        #region Constructor
        public ArticleSupplierViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ArticleSupplierViewModel....", category: Category.Info, priority: Priority.Low);

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
              
                RefreshCommand = new RelayCommand(new Action<object>(RefreshCommandAction));
                PrintCommand = new RelayCommand(new Action<object>(PrintCommandAction));
                ExportCommand = new RelayCommand(new Action<object>(ExportCommandAction));
                GridDoubleClickCommand = new DelegateCommand<object>(GridDoubleClickCommandAction);
                WarehouseEditValueChangedCommand = new DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                DeleteCommand = new DelegateCommand<object>(DeleteCommandAction);
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));

                AddNewCompaniesCommand = new RelayCommand(new Action<object>(AddNewCompaniesCommandAction));//[Sudhir.Jangra][GEos2-4738]

                //Fill data as per selected warehouse
                FillArticleSupplierList();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor ArticleSupplierViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ArticleSupplierViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods

        public void Init()
        {

        }

        public void RefreshCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                FillArticleSupplierList();
                detailView.SearchString = null;

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ArticleSupplierListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ArticleSupplierListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Supplier List";
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

                    //activityTableView.Grid.DataController.GetAllFilteredAndSortedRows()
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GridDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction....", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                if ((ArticleSupplier)detailView.FocusedRow != null)
                {
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

                    EditArticleSupplierViewModel editArticleSupplierViewModel = new EditArticleSupplierViewModel();
                    EditArticleSupplierView editArticleSupplierView = new EditArticleSupplierView();

                    EventHandler handle = delegate { editArticleSupplierView.Close(); };
                    editArticleSupplierViewModel.RequestClose += handle;

                    ArticleSupplier wpo = (ArticleSupplier)detailView.FocusedRow;
                    SRM.SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == wpo.Warehouse.IdWarehouse);
                    Warehouses warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;
                    editArticleSupplierViewModel.Init((ulong)wpo.IdArticleSupplier, warehouse);
                    editArticleSupplierView.DataContext = editArticleSupplierViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    editArticleSupplierView.Owner = Window.GetWindow(ownerInfo);
                    editArticleSupplierView.ShowDialog();
                   
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    if (editArticleSupplierViewModel.IsSaveChanges)
                    {


                        FillArticleSupplierList();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GridDoubleClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
            FillArticleSupplierList();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void FillArticleSupplierList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleSupplierList...", category: Category.Info, priority: Priority.Low);

                if (SRM.SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    try
                    {
                        //GEOS2-4403 Sudhir.Jangra 03/05/2023
                        List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                        long plantOwnerIds;
                        string plantConnection;
                        List<ArticleSupplier> tempMainSupplierList = new List<ArticleSupplier>();
                        ArticleSupplierList = new List<ArticleSupplier>();
                        foreach (var item in plantOwners)
                        {
                            plantOwnerIds = item.IdWarehouse;
                            plantConnection = item.Company.ConnectPlantConstr;
                            SRMService = new SRMServiceController((item != null && item.Company.ServiceProviderUrl != null) ? item.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            


                            // tempMainSupplierList = new List<ArticleSupplier>(SRMService.GetArticleSuppliersByWarehouse_V2390(plantOwnerIds, plantConnection));
                            //[Sudhir.jangra][GEOS2-4740][26/09/2023] Created Sp And Service Version wise.
                            tempMainSupplierList = new List<ArticleSupplier>(SRMService.GetArticleSuppliersByWarehouse_V2440(plantOwnerIds, plantConnection));


                            if (tempMainSupplierList != null)
                            {
                                tempMainSupplierList.ForEach(i => i.Warehouse = item);
                                foreach (var articleSupplieritem in tempMainSupplierList.GroupBy(tpa => tpa.Country.Iso))
                                {
                                    ImageSource countryFlagImage = ByteArrayToBitmapImage(articleSupplieritem.ToList().FirstOrDefault().Country.CountryIconBytes);
                                    articleSupplieritem.ToList().Where(ari => ari.Country.Iso == articleSupplieritem.Key).ToList().ForEach(ari => ari.Country.CountryIconImage = countryFlagImage);
                                }
                            }
                            //[rdixit][GEOS2-4662][12.07.2023]
                            if (tempMainSupplierList != null)
                                ArticleSupplierList.AddRange(tempMainSupplierList.Where(j => !ArticleSupplierList.Select(p => p.IdArticleSupplier).Contains(j.IdArticleSupplier)).ToList());
                        }
                        SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault();
                        #region 


                        //Warehouses Warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;
                        //ArticleSupplierList = new List<ArticleSupplier>(SRMService.GetArticleSuppliersByWarehouse_V2340(Warehouse));//Service Method GetArticleSuppliersByWarehouse_V2250 replaced with GetArticleSuppliersByWarehouse_V2340 by [GEOS2-4027][rdixit][18.11.2022]
                        //if (ArticleSupplierList != null)
                        //{
                        //     foreach (var articleSupplieritem in ArticleSupplierList.GroupBy(tpa => tpa.Country.Iso))
                        //    {

                        //        ImageSource countryFlagImage = ByteArrayToBitmapImage(articleSupplieritem.ToList().FirstOrDefault().Country.CountryIconBytes);
                        //        articleSupplieritem.ToList().Where(ari => ari.Country.Iso == articleSupplieritem.Key).ToList().ForEach(ari => ari.Country.CountryIconImage = countryFlagImage);
                        //    }
                        //}
                        //SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault();
                        #endregion
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    ArticleSupplierList = new List<ArticleSupplier>(ArticleSupplierList);
                }
                else
                {
                    ArticleSupplierList = new List<ArticleSupplier>();
                }
                FillAccountAge();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillArticleSupplierList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void DeleteCommandAction(object obj)
        {
            TableView detailView = (TableView)obj;
            ArticleSupplier selectedSupplier = (ArticleSupplier)detailView.FocusedRow;
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SRMSupplier_delete"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ArticleSupplierList.Remove(selectedSupplier);
                    //bool IsDeleted = SRMService.DeleteArticleSupplier(selectedSupplier.IdArticleSupplier, GeosApplication.Instance.ActiveUser.IdUser);
                    bool IsDeleted = SRMService.DeleteArticleSupplier_V2690(selectedSupplier.IdArticleSupplier, GeosApplication.Instance.ActiveUser.IdUser); //[pallavi.kale][GEOS2-9806][24.11.2025]
                    ArticleSupplierList = new List<ArticleSupplier>(ArticleSupplierList);
                    SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault();
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplier_DeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillAccountAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountAge ...", category: Category.Info, priority: Priority.Low);
                int i = 0;
                if (ArticleSupplierList != null)
                {
                    foreach (ArticleSupplier age in ArticleSupplierList)
                    {
                        ArticleSupplierList[i].Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - ArticleSupplierList[i].CreatedIn.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - ArticleSupplierList[i].CreatedIn.Year)) / 12, 1);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for open website on browser . 
        /// </summary>
        /// <param name="obj"></param>
        public void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);

                string website = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(website) && website != "-" && !website.Contains("@"))
                {
                    string[] websiteArray = website.Split(' ');
                    System.Diagnostics.Process.Start(websiteArray[0]);
                }

                GeosApplication.Instance.Logger.Log("Method OpenWebsite() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenWebsite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

      

        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);

                string emailAddess = Convert.ToString(obj);

                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new System.Windows.Media.Imaging.BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
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

        //[sudhir.jangra][GEOS2-4738]
        private void AddNewCompaniesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewCompaniesCommandAction....", category: Category.Info, priority: Priority.Low);

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

                AddArticleSupplierViewModel addArticleSupplierViewModel = new AddArticleSupplierViewModel();
                AddArticleSupplierView addArticleSupplierView = new AddArticleSupplierView();

                    EventHandler handle = delegate { addArticleSupplierView.Close(); };
                addArticleSupplierViewModel.RequestClose += handle;
                addArticleSupplierViewModel.IsAddSupplierViewOpened = true;
             
                addArticleSupplierViewModel.Init();
                addArticleSupplierView.DataContext = addArticleSupplierViewModel;

                addArticleSupplierView.ShowDialog();

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    if (addArticleSupplierViewModel.IsSaveChanges|| addArticleSupplierViewModel.IsAdded)
                    {
                        FillArticleSupplierList();
                        SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault(x => x.IdArticleSupplier == addArticleSupplierViewModel.NewArticleSupplier.IdArticleSupplier);
                    }
                
                GeosApplication.Instance.Logger.Log("Method AddNewCompaniesCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewCompaniesCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
