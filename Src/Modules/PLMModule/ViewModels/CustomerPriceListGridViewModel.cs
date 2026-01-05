using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.PLM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using System.Data;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Data;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.Utility;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using Emdep.Geos.Data.Common.PLM;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    public class CustomerPriceListGridViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      //  IPLMService PLMService = new PLMServiceController("localhost:6699");
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declarations

        private ObservableCollection<CustomerPrice> customerPricelist;
        private CustomerPrice selectedCustomerPrice;

        private bool isDeleted;
        private bool isBusy;



        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private bool isCPLColumnChooserVisible;
        private bool _isCPLArticleEnable;
     
        #endregion

        #region Properties

        public string CPLGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "CPLGridSetting.Xml";

        public ObservableCollection<CustomerPrice> CustomerPricelist
        {
            get { return customerPricelist; }
            set
            {
                customerPricelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPricelist"));
            }
        }

        public CustomerPrice SelectedCustomerPrice
        {
            get
            {
                return selectedCustomerPrice;
            }
            set
            {
                selectedCustomerPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPrice"));
            }
        }

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
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

        public bool IsCPLColumnChooserVisible
        {
            get
            {
                return isCPLColumnChooserVisible;
            }

            set
            {
                isCPLColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLColumnChooserVisible"));
            }
        }
        public bool IsCPLArticleEnable
        {
            get { return _isCPLArticleEnable; }
            set
            {
                _isCPLArticleEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLArticleEnable"));
            }
        }

      
        #endregion

        #region ICommands


        public ICommand AddCustomerPriceListCommand { get; set; }
        public ICommand RefreshCustomerPriceListCommand { get; set; }
        public ICommand PrintCustomerPriceListCommand { get; set; }
        public ICommand ExportCustomerPriceListCommand { get; set; }
        public ICommand DeleteCustomerPriceCommand { get; set; }
        public ICommand EditCustomerPriceCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }

        public ICommand CPLGridControlLoadedCommand { get; set; }
        public ICommand CPLItemListTableViewLoadedCommand { get; set; }
        public ICommand CPLGridControlUnloadedCommand { get; set; }
        #endregion

        #region Constructor

        public CustomerPriceListGridViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor CustomerPriceListGridViewModel ...", category: Category.Info, priority: Priority.Low);

                AddCustomerPriceListCommand = new RelayCommand(new Action<object>(AddCustomerPriceAction));
                RefreshCustomerPriceListCommand = new RelayCommand(new Action<object>(RefreshCustomerPriceAction));
                PrintCustomerPriceListCommand = new RelayCommand(new Action<object>(PrintCustomerPriceAction));
                ExportCustomerPriceListCommand = new RelayCommand(new Action<object>(ExportCustomerPriceAction));
                DeleteCustomerPriceCommand = new RelayCommand(new Action<object>(DeleteCustomerPriceAction));
                EditCustomerPriceCommand = new DelegateCommand<object>(async (obj) => await EditCustomerPriceAction(obj));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CPLGridControlLoadedCommand = new DelegateCommand<object>(CPLGridControlLoadedAction);
                CPLItemListTableViewLoadedCommand = new DelegateCommand<object>(CPLItemListTableViewLoadedAction);
                CPLGridControlUnloadedCommand = new DelegateCommand<object>(CPLGridControlUnloadedCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor CustomerPriceListGridViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor BasePriceListGridViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
        {
            FillCustomerPrice();
            IsCPLArticleEnable = true;
        }

        public void FillCustomerPrice()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                //CustomerPricelist = new ObservableCollection<CustomerPrice>(PLMService.GetCustomerPrices_V2160());
                //Shubham[skadam] GEOS2-4300 [ Only Modules ] Able to add and access to the tab Modules/Detections/Articles a Customer Price List [3/3] [#PLM25] 20 04 2023
                //CustomerPricelist = new ObservableCollection<CustomerPrice>(PLMService.GetCustomerPriceListItem_User_ById(GeosApplication.Instance.ActiveUser.IdUser));
                CustomerPricelist = new ObservableCollection<CustomerPrice>(PLMService.GetCustomerPriceListItem_User_ById_V2380(GeosApplication.Instance.ActiveUser.IdUser));

                if (CustomerPricelist != null)
                {
                    foreach (var bpItem in CustomerPricelist.GroupBy(tpa => tpa.Currency.Name))
                    {

                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Currency.Name == bpItem.Key).ToList().ForEach(oti => oti.Currency.CurrencyIconImage = currencyFlagImage);
                    }
                    foreach (var item in CustomerPricelist)
                    {

                        if (item.PVPCurrencyWithBytes != null)
                        {
                            foreach (var item1 in item.PVPCurrencyWithBytes.GroupBy(tpa => tpa.Name))
                            {
                                ImageSource currencyFlagImage = ByteArrayToBitmapImage(item1.ToList().FirstOrDefault().CurrencyIconbytes);
                                item1.ToList().Where(oti => oti.Name == item1.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                            }
                        }

                    }
                }


                if (CustomerPricelist.Count > 0)
                    SelectedCustomerPrice = CustomerPricelist.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPrice() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerPrice() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCustomerPrice()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomerPriceAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;

                    AddEditCustomerPriceView addCustomerPriceView = new AddEditCustomerPriceView();
                    AddEditCustomerPriceViewModel addCustomerPriceViewModel = new AddEditCustomerPriceViewModel();

                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    addCustomerPriceView.DataContext = addCustomerPriceViewModel;
                    EventHandler handle = delegate { addCustomerPriceView.Close(); };
                    addCustomerPriceViewModel.RequestClose += handle;
                    addCustomerPriceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCustomerPriceList").ToString();
                    addCustomerPriceViewModel.IsDuplicateCustomerPriceButtonVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-3349][27/12/2022]
                    addCustomerPriceViewModel.IsNew = true;

                    addCustomerPriceViewModel.IsFirstTimeLoad = true;
                    addCustomerPriceViewModel.SaveButtonVisibility = Visibility.Hidden;
                    bool status = IsCPLColumnChooserVisible;
                    IsCPLColumnChooserVisible = false;
					//[nsatpute][28.05.2025][GEOS2-6689]
                    addCustomerPriceViewModel.InitAsync(System.Windows.Application.Current.FindResource("AddCustomerPriceList").ToString());

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (detailView as FrameworkElement);
                    addCustomerPriceView.Owner = Window.GetWindow(ownerInfo);
                    addCustomerPriceView.ShowDialog();

                    if (addCustomerPriceViewModel.IsNewCustomerPriceSave)
                    {
                        CustomerPrice tempCustomerPrice = new CustomerPrice();
                        tempCustomerPrice.IdPermission = "62";
                        tempCustomerPrice.Code = addCustomerPriceViewModel.NewPrice.Code;
                        tempCustomerPrice.IdCustomerPriceList = addCustomerPriceViewModel.NewPrice.IdCustomerPriceList;
                        tempCustomerPrice.IdBasePriceList = addCustomerPriceViewModel.NewPrice.IdBasePriceList;
                        tempCustomerPrice.BasePriceName = addCustomerPriceViewModel.NewPrice.BasePriceName.Trim();
                        tempCustomerPrice.Name = addCustomerPriceViewModel.NewPrice.Name.Trim();
                        tempCustomerPrice.Description = addCustomerPriceViewModel.NewPrice.Description.Trim();
                        tempCustomerPrice.Remark = addCustomerPriceViewModel.NewPrice.Remark;

                        tempCustomerPrice.IdCurrency = (byte)addCustomerPriceViewModel.NewPrice.IdCurrency;

                        if (tempCustomerPrice.Currency == null)
                            tempCustomerPrice.Currency = new Currency();

                        List<Currency> CurrencyList = new List<Currency>();

                        foreach (Currency cur in addCustomerPriceViewModel.SelectedSaleCurrency)
                        {
                            CurrencyList.Add(cur);
                        }
                        tempCustomerPrice.ArticleCount = addCustomerPriceViewModel.NewPrice.CustomerPriceListItems.Select(a => a.IdArticle).Distinct().Count();
                        tempCustomerPrice.DetectionCount = addCustomerPriceViewModel.NewPrice.CustomerPriceListDetections.Select(a => a.IdDetection).Distinct().Count();
                        if ((tempCustomerPrice.ArticleCount != null && tempCustomerPrice.ArticleCount > 0) || (tempCustomerPrice.DetectionCount != null && tempCustomerPrice.DetectionCount > 0))
                        {
                            tempCustomerPrice.SaleCurrencies = String.Join("\n", CurrencyList.Select(a => a.Name));
                            if (CurrencyList != null)
                            {
                                foreach (var bpItem in CurrencyList.GroupBy(tpa => tpa.Name))
                                {
                                    ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                                    bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);

                                }
                            }
                            tempCustomerPrice.PVPCurrencyWithBytes = CurrencyList;

                        }
                        else
                        {
                            tempCustomerPrice.SaleCurrencies = String.Join("\n", addCustomerPriceViewModel.SaleCurrencyList.Select(a => a.Name));
                            if (addCustomerPriceViewModel.SaleCurrencyList != null)
                            {
                                foreach (var bpItem in addCustomerPriceViewModel.SaleCurrencyList.GroupBy(tpa => tpa.Name))
                                {
                                    ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                                    bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);

                                }
                            }
                            tempCustomerPrice.PVPCurrencyWithBytes = (addCustomerPriceViewModel.SaleCurrencyList).ToList();

                        }


                        tempCustomerPrice.Currency.Name = addCustomerPriceViewModel.CurrencyName;
                        tempCustomerPrice.Currency.CurrencyIconImage = addCustomerPriceViewModel.CurrencyIcon;
                        tempCustomerPrice.IdStatus = (uint)addCustomerPriceViewModel.SelectedStatus.IdLookupValue;
                        tempCustomerPrice.Status = addCustomerPriceViewModel.SelectedStatus;
                        tempCustomerPrice.ExpiryDate = addCustomerPriceViewModel.NewPrice.ExpiryDate;
                        tempCustomerPrice.EffectiveDate = addCustomerPriceViewModel.NewPrice.EffectiveDate;
                        if (addCustomerPriceViewModel.CPLCustomerList != null && addCustomerPriceViewModel.CPLCustomerList.Count > 0)
                        {
                            tempCustomerPrice.Group = string.Join("\n", addCustomerPriceViewModel.CPLCustomerList.Select(c => c.GroupName).Distinct().ToArray());
                            tempCustomerPrice.Region = string.Join("\n", addCustomerPriceViewModel.CPLCustomerList.Select(c => c.RegionName).Distinct().ToArray());
                            tempCustomerPrice.Country = string.Join("\n", addCustomerPriceViewModel.CPLCustomerList.Select(c => c.Country.Name).Distinct().ToArray());
                            tempCustomerPrice.Plant = string.Join("\n", addCustomerPriceViewModel.CPLCustomerList.Select(c => c.Plant.Name).Distinct().ToArray());
                        }
                        tempCustomerPrice.LastUpdated = DateTime.Now;

                        CustomerPricelist.Add(tempCustomerPrice);
                        CustomerPricelist = new ObservableCollection<CustomerPrice>(CustomerPricelist.OrderByDescending(a => a.Code));
                        detailView.ImmediateUpdateRowPosition = true;
                        detailView.EnableImmediatePosting = true;
                        RefreshCustomerPriceAction(detailView);
                    }
                    IsCPLColumnChooserVisible = status;
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomerPriceAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomerPriceAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshCustomerPriceAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillCustomerPrice();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshCustomerPriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCustomerPriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCustomerPriceAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCustomerPriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCustomerPriceAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["CustomerPriceListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["CustomerPriceListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintCustomerPriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCustomerPriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCustomerPriceAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Customer Prices";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

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

                    GeosApplication.Instance.Logger.Log("Method ExportCustomerPriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCustomerPriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        private void DeleteCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCustomerPriceAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedCustomerPrice.IdStatus == 223)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveCPLValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["CustomerPriceDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = PLMService.IsDeletedCustomerPriceList_V2180(SelectedCustomerPrice.IdCustomerPriceList, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsDeleted)
                    {
                        CustomerPricelist.Remove(SelectedCustomerPrice);
                        CustomerPricelist = new ObservableCollection<CustomerPrice>(CustomerPricelist);
                        SelectedCustomerPrice = CustomerPricelist.FirstOrDefault();
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerPriceDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCustomerPriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCustomerPriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCustomerPriceAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteCustomerPriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

		//[nsatpute][11.06.2025][GEOS2-6689]
        private async Task EditCustomerPriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerPriceAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;
                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;

                    AddEditCustomerPriceView editCustomerPriceView = new AddEditCustomerPriceView();
                    AddEditCustomerPriceViewModel editCustomerPriceViewModel = new AddEditCustomerPriceViewModel();

                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    EventHandler handle = delegate { editCustomerPriceView.Close(); };
                    editCustomerPriceViewModel.RequestClose += handle;
                    editCustomerPriceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCustomerPriceList").ToString();
                    editCustomerPriceViewModel.IsNew = false;
                    editCustomerPriceViewModel.IsFirstTimeLoad = true;
                    bool status = IsCPLColumnChooserVisible;
                    IsCPLColumnChooserVisible = false;
                    //[nsatpute][28.05.2025][GEOS2-6689]
                    //var tasks = new List<Task>();
                    editCustomerPriceViewModel.LoadBasePriceData(SelectedCustomerPrice);
                    editCustomerPriceViewModel.FillCurrencyConversionList_All();
                    editCustomerPriceViewModel.FillArticleCostPriceList(SelectedCustomerPrice.IdCurrency);
                    editCustomerPriceViewModel.EditInitAsync(System.Windows.Application.Current.FindResource("EditCustomerPriceList").ToString());                    
                    editCustomerPriceViewModel.EditInitCustomerPrice(SelectedCustomerPrice);
                    editCustomerPriceView.DataContext = editCustomerPriceViewModel;
                    
                    //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (detailView as FrameworkElement);
                    editCustomerPriceView.Owner = Window.GetWindow(ownerInfo);
                    editCustomerPriceViewModel.IsEnabledSaveButton = false;//[Sudhir.Jangra][GEOS2-3132][                        
                    editCustomerPriceView.ShowDialog();
                    CustomerPrice tempCustomerPrice = new CustomerPrice();
                    if (editCustomerPriceViewModel.IsNewCustomerPriceSave)
                    {
                        if (editCustomerPriceViewModel.IsDuplicateSave)//[Sudhir.Jangra][GEOS2-3349][28/12/2022]
                        {
                            CustomerPrice NewCustomerPrice = new CustomerPrice();
                            NewCustomerPrice.IdBasePriceList = editCustomerPriceViewModel.NewPrice.IdBasePriceList;
                            NewCustomerPrice.Code = editCustomerPriceViewModel.NewPrice.Code;
                            NewCustomerPrice.Name = editCustomerPriceViewModel.NewPrice.Name.Trim();
                            NewCustomerPrice.Description = editCustomerPriceViewModel.NewPrice.Description.Trim();
                            NewCustomerPrice.IdCurrency = (byte)editCustomerPriceViewModel.NewPrice.IdCurrency;
                            NewCustomerPrice.Currency = ((Currency)editCustomerPriceViewModel.SelectedCurrency);
                            ImageSource currencyFlagImage = ByteArrayToBitmapImage(NewCustomerPrice.Currency.CurrencyIconbytes);
                            NewCustomerPrice.Currency.CurrencyIconImage = currencyFlagImage;
                            if (editCustomerPriceViewModel.NewPrice.CurrencyList != null)
                            {
                                foreach (var cpItem in editCustomerPriceViewModel.NewPrice.CurrencyList.GroupBy(tpa => tpa.Name))
                                {
                                    ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(cpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                                    cpItem.ToList().Where(oti => oti.Name == cpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);
                                }
                            }
                            NewCustomerPrice.ArticleCount = editCustomerPriceViewModel.ArticleCount;
                            NewCustomerPrice.DetectionCount = editCustomerPriceViewModel.DetectionCount;
                            tempCustomerPrice.PVPCurrencyWithBytes = editCustomerPriceViewModel.NewPrice.CurrencyList;
                            tempCustomerPrice.IdStatus = (uint)editCustomerPriceViewModel.SelectedStatus.IdLookupValue;
                            NewCustomerPrice.Status = editCustomerPriceViewModel.SelectedStatus;
                            NewCustomerPrice.EffectiveDate = editCustomerPriceViewModel.NewPrice.EffectiveDate;
                            NewCustomerPrice.Remark = editCustomerPriceViewModel.NewPrice.Remark;
                            NewCustomerPrice.ExpiryDate = editCustomerPriceViewModel.NewPrice.ExpiryDate;
                            NewCustomerPrice.LastUpdated = DateTime.Now;

                            CustomerPricelist.Add(NewCustomerPrice);
                            CustomerPricelist = new ObservableCollection<CustomerPrice>(CustomerPricelist.OrderByDescending(a => a.Code));
                           
                        }
                        //
                        else
                        {

                       
                        TableView tableView = (TableView)obj;
                        tempCustomerPrice = (CustomerPrice)tableView.DataControl.CurrentItem;
                        tempCustomerPrice.Code = editCustomerPriceViewModel.UpdatedItem.Code;
                        tempCustomerPrice.IdCustomerPriceList = editCustomerPriceViewModel.IdCustomerPriceList;
                        tempCustomerPrice.IdBasePriceList = editCustomerPriceViewModel.UpdatedItem.IdBasePriceList;
                        tempCustomerPrice.BasePriceName = editCustomerPriceViewModel.UpdatedItem.BasePriceName.Trim();
                        tempCustomerPrice.Name = editCustomerPriceViewModel.UpdatedItem.Name.Trim();
                        tempCustomerPrice.Description = editCustomerPriceViewModel.UpdatedItem.Description.Trim();
                        tempCustomerPrice.Remark = editCustomerPriceViewModel.UpdatedItem.Remark;
                        tempCustomerPrice.IdCurrency = (byte)editCustomerPriceViewModel.UpdatedItem.IdCurrency;
                        tempCustomerPrice.Currency.Name = editCustomerPriceViewModel.CurrencyName;
                        //  ImageSource currencyFlagImage = ByteArrayToBitmapImage(editCustomerPriceViewModel.SelectedCurrency.CurrencyIconbytes);
                        tempCustomerPrice.Currency.CurrencyIconImage = editCustomerPriceViewModel.CurrencyIcon;


                        List<Currency> CurrencyList = new List<Currency>();

                        if (editCustomerPriceViewModel.IsEnabledSaveButton && editCustomerPriceViewModel.SaveButtonVisibility == Visibility.Visible)
                        {
                            foreach (Currency cur in editCustomerPriceViewModel.ClonedCustomerPrice.CurrencyList)
                            {
                                CurrencyList.Add(cur);
                            }
                        }
                        else
                        {
                            foreach (Currency cur in editCustomerPriceViewModel.SelectedSaleCurrency)
                            {
                                CurrencyList.Add(cur);
                            }
                        }

                        tempCustomerPrice.ArticleCount = editCustomerPriceViewModel.ArticleCount;
                        tempCustomerPrice.DetectionCount = editCustomerPriceViewModel.DetectionCount;
                        //if ((tempCustomerPrice.ArticleCount != null && tempCustomerPrice.ArticleCount > 0) || (tempCustomerPrice.DetectionCount != null && tempCustomerPrice.DetectionCount > 0))
                        //{
                        tempCustomerPrice.SaleCurrencies = String.Join("\n", CurrencyList.Select(a => a.Name));

                        if (CurrencyList != null)
                        {
                            foreach (var bpItem in CurrencyList.GroupBy(tpa => tpa.Name))
                            {
                                ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                                bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);

                            }
                        }
                        tempCustomerPrice.PVPCurrencyWithBytes = CurrencyList;


                        //}
                        //else
                        //{
                        //    tempCustomerPrice.SaleCurrencies = String.Join("\n", editCustomerPriceViewModel.SaleCurrencyList.Select(a => a.Name));
                        //}
                        //tempCustomerPrice.PVPCurrencies = String.Join("\n", editCustomerPriceViewModel.SelectedSaleCurrency);
                        tempCustomerPrice.IdStatus = (uint)editCustomerPriceViewModel.SelectedStatus.IdLookupValue;
                        tempCustomerPrice.Status = editCustomerPriceViewModel.SelectedStatus;
                        tempCustomerPrice.EffectiveDate = editCustomerPriceViewModel.UpdatedItem.EffectiveDate;
                        tempCustomerPrice.ExpiryDate = editCustomerPriceViewModel.UpdatedItem.ExpiryDate;
                        if (editCustomerPriceViewModel.CPLCustomerList != null && editCustomerPriceViewModel.CPLCustomerList.Count > 0)
                        {
                            tempCustomerPrice.Group = string.Join("\n", editCustomerPriceViewModel.CPLCustomerList.Select(c => c.GroupName).Distinct().ToArray());
                            tempCustomerPrice.Region = string.Join("\n", editCustomerPriceViewModel.CPLCustomerList.Select(c => c.RegionName).Distinct().ToArray());
                            tempCustomerPrice.Country = string.Join("\n", editCustomerPriceViewModel.CPLCustomerList.Select(c => c.Country.Name).Distinct().ToArray());
                            tempCustomerPrice.Plant = string.Join("\n", editCustomerPriceViewModel.CPLCustomerList.Select(c => c.Plant.Name).Distinct().ToArray());
                        }
                        }
                        // tempCustomerPrice.Group= editCustomerPriceViewModel.UpdatedItem.cp
                        tempCustomerPrice.LastUpdated = DateTime.Now;
                        detailView.ImmediateUpdateRowPosition = true;
                        detailView.EnableImmediatePosting = true;
                        RefreshCustomerPriceAction(detailView);
                    }
                  
                   
                    IsCPLColumnChooserVisible = status;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditCustomerPriceAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditCustomerPriceAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "SaleCurrencies" && e.Column.FieldName != "Plant" && e.Column.FieldName != "Group" && e.Column.FieldName != "Region" && e.Column.FieldName != "Country")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "SaleCurrencies")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([SaleCurrencies])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([SaleCurrencies])")
                    });

                    foreach (var dataObject in CustomerPricelist)
                    {
                        if (dataObject.SaleCurrencies == null)
                        {
                            continue;
                        }
                        else if (dataObject.SaleCurrencies != null)
                        {
                            if (dataObject.SaleCurrencies.Contains("\n"))
                            {
                                string tempPVPCurrencies = dataObject.SaleCurrencies;
                                for (int index = 0; index < tempPVPCurrencies.Length; index++)
                                {
                                    string empPVPCurrencies = tempPVPCurrencies.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPVPCurrencies))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPVPCurrencies;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SaleCurrencies Like '%{0}%'", empPVPCurrencies));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPVPCurrencies.Contains("\n"))
                                        tempPVPCurrencies = tempPVPCurrencies.Remove(0, empPVPCurrencies.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == CustomerPricelist.Where(y => y.SaleCurrencies == dataObject.SaleCurrencies).Select(slt => slt.SaleCurrencies).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = CustomerPricelist.Where(y => y.SaleCurrencies == dataObject.SaleCurrencies).Select(slt => slt.SaleCurrencies).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SaleCurrencies Like '%{0}%'", CustomerPricelist.Where(y => y.SaleCurrencies == dataObject.SaleCurrencies).Select(slt => slt.SaleCurrencies).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Group")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Group])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Group])")
                    });

                    foreach (var dataObject in CustomerPricelist)
                    {
                        if (dataObject.Group == null)
                        {
                            continue;
                        }
                        else if (dataObject.Group != null)
                        {
                            if (dataObject.Group.Contains("\n"))
                            {
                                string tempGroup = dataObject.Group;
                                for (int index = 0; index < tempGroup.Length; index++)
                                {
                                    string empGroup = tempGroup.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empGroup))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empGroup;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Group Like '%{0}%'", empGroup));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempGroup.Contains("\n"))
                                        tempGroup = tempGroup.Remove(0, empGroup.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == CustomerPricelist.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = CustomerPricelist.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Group Like '%{0}%'", CustomerPricelist.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Region")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Region])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Region])")
                    });

                    foreach (var dataObject in CustomerPricelist)
                    {
                        if (dataObject.Region == null)
                        {
                            continue;
                        }
                        else if (dataObject.Region != null)
                        {
                            if (dataObject.Region.Contains("\n"))
                            {
                                string tempRegion = dataObject.Region;
                                for (int index = 0; index < tempRegion.Length; index++)
                                {
                                    string empRegion = tempRegion.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empRegion))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empRegion;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Region Like '%{0}%'", empRegion));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempRegion.Contains("\n"))
                                        tempRegion = tempRegion.Remove(0, empRegion.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == CustomerPricelist.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = CustomerPricelist.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Region Like '%{0}%'", CustomerPricelist.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Country")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Country])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Country])")
                    });

                    foreach (var dataObject in CustomerPricelist)
                    {
                        if (dataObject.Country == null)
                        {
                            continue;
                        }
                        else if (dataObject.Country != null)
                        {
                            if (dataObject.Country.Contains("\n"))
                            {
                                string tempCountry = dataObject.Country;
                                for (int index = 0; index < tempCountry.Length; index++)
                                {
                                    string empCountry = tempCountry.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empCountry))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empCountry;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", empCountry));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempCountry.Contains("\n"))
                                        tempCountry = tempCountry.Remove(0, empCountry.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == CustomerPricelist.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = CustomerPricelist.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", CustomerPricelist.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "Plant")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Plant])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Plant])")
                    });

                    foreach (var dataObject in CustomerPricelist)
                    {
                        if (dataObject.Plant == null)
                        {
                            continue;
                        }
                        else if (dataObject.Plant != null)
                        {
                            if (dataObject.Plant.Contains("\n"))
                            {
                                string tempPlants = dataObject.Plant;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plant Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == CustomerPricelist.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = CustomerPricelist.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plant Like '%{0}%'", CustomerPricelist.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }



                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

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

        #endregion

        #region Column Chooser
        private void CPLGridControlLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CPLGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(CPLGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(CPLGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(CPLGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, CPLVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, CPLVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 1)
                {
                    IsCPLColumnChooserVisible = true;
                }
                else
                {
                    IsCPLColumnChooserVisible = false;
                }

                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CPLGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on CPLGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void CPLVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CPLVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(CPLGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsCPLColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method CPLVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in CPLVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void CPLVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CPLVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(CPLGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method CPLVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in CPLVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CPLItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }

        private void CPLGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CPLGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(CPLGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method CPLGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on CPLGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }

}
