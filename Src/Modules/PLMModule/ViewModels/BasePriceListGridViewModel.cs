using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PLM.CommonClasses;
using Emdep.Geos.Modules.PLM.Views;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    public class BasePriceListGridViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IPLMService PLMService = new PLMServiceController("localhost:6699");
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

        private ObservableCollection<BasePrice> basePricelist;
        private BasePrice selectedBasePrice;

        private bool isDeleted;
        private bool isBusy;



        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        private bool isBPLColumnChooserVisible;


        #endregion

        #region Properties

        public string BPLGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "BPLGridSetting.Xml";

        public ObservableCollection<BasePrice> BasePricelist
        {
            get { return basePricelist; }
            set
            {
                basePricelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BasePricelist"));
            }
        }

        public BasePrice SelectedBasePrice
        {
            get
            {
                return selectedBasePrice;
            }
            set
            {
                selectedBasePrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBasePrice"));
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

        public bool IsBPLColumnChooserVisible
        {
            get
            {
                return isBPLColumnChooserVisible;
            }

            set
            {
                isBPLColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLColumnChooserVisible"));
            }
        }
        #endregion

        #region ICommands


        public ICommand AddBasePriceListCommand { get; set; }
        public ICommand RefreshBasePriceListCommand { get; set; }
        public ICommand PrintBasePriceListCommand { get; set; }
        public ICommand ExportBasePriceListCommand { get; set; }
        public ICommand DeleteBasePriceCommand { get; set; }
        public ICommand EditBasePriceCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }

        public ICommand BPLGridControlLoadedCommand { get; set; }
        public ICommand BPLItemListTableViewLoadedCommand { get; set; }
        public ICommand BPLGridControlUnloadedCommand { get; set; }

        #endregion

        #region Constructor

        public BasePriceListGridViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor BasePriceListGridViewModel ...", category: Category.Info, priority: Priority.Low);

                AddBasePriceListCommand = new RelayCommand(new Action<object>(AddBasePriceAction));
                RefreshBasePriceListCommand = new RelayCommand(new Action<object>(RefreshBasePriceAction));
                PrintBasePriceListCommand = new RelayCommand(new Action<object>(PrintBasePriceAction));
                ExportBasePriceListCommand = new RelayCommand(new Action<object>(ExportBasePriceAction));
                DeleteBasePriceCommand = new RelayCommand(new Action<object>(DeleteBasePriceAction));
                EditBasePriceCommand = new RelayCommand(new Action<object>(EditBasePriceAction));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);

                BPLGridControlLoadedCommand = new DelegateCommand<object>(BPLGridControlLoadedAction);
                BPLItemListTableViewLoadedCommand = new DelegateCommand<object>(BPLItemListTableViewLoadedAction);
                BPLGridControlUnloadedCommand = new DelegateCommand<object>(BPLGridControlUnloadedCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor BasePriceListGridViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
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
            FillBasePrice();
            AddEditBasePriceViewModel editBasePriceViewModel = new AddEditBasePriceViewModel();
            PLMCommon.Instance.ArticleCostPriceList = null;
            System.Threading.Tasks.Task.Run(async () =>  await editBasePriceViewModel.LoadAllDataInBackground());
            // SetMinMaxDatesAndFillCurrencyConversions();
        }

        public void FillBasePrice()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                //BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePrices_V2180());
                //BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePriceListItem_User_ById(GeosApplication.Instance.ActiveUser.IdUser));
                //Shubham[skadam] GEOS2-2886 [Only Modules ] - Able to  add and access to the tab Modules/Detection /Articles a Base Price List [3/3] [#PLM07] 21 03 2023
                BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePriceListItem_User_ById_V2370(GeosApplication.Instance.ActiveUser.IdUser));
                if (BasePricelist != null)
                {
                    foreach (var bpItem in BasePricelist.GroupBy(tpa => tpa.Currency.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().Currency.CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Currency.Name == bpItem.Key).ToList().ForEach(oti => oti.Currency.CurrencyIconImage = currencyFlagImage);

                    }
                    foreach (var item in BasePricelist)
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

                if (BasePricelist.Count > 0)
                    SelectedBasePrice = BasePricelist.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBasePrice() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBasePrice() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillBasePrice()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddBasePriceAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                AddEditBasePriceView addBasePriceView = new AddEditBasePriceView();
                AddEditBasePriceViewModel addBasePriceViewModel = new AddEditBasePriceViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addBasePriceView.DataContext = addBasePriceViewModel;
                EventHandler handle = delegate { addBasePriceView.Close(); };
                addBasePriceViewModel.RequestClose += handle;
                addBasePriceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddBasePriceList").ToString();
                addBasePriceViewModel.IsDuplicateBasePriceButtonVisible = Visibility.Collapsed;
                addBasePriceViewModel.IsNew = true;
                addBasePriceViewModel.IsFirstTimeLoad = true;
                addBasePriceViewModel.SaveButtonVisibility = Visibility.Hidden;
                bool status = IsBPLColumnChooserVisible;
                IsBPLColumnChooserVisible = false;
                addBasePriceViewModel.Init(System.Windows.Application.Current.FindResource("AddBasePriceList").ToString());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                TableView detailView = (TableView)obj;
                var ownerInfo = (detailView as FrameworkElement);
                addBasePriceView.Owner = Window.GetWindow(ownerInfo);
                addBasePriceView.ShowDialog();

                BasePrice tempBasePrice = new BasePrice();

                if (addBasePriceViewModel.IsNewBasePriceSave)
                {

                    tempBasePrice.IdPermission = "62";
                    tempBasePrice.IdBasePriceList = addBasePriceViewModel.NewBasePrice.IdBasePriceList;
                    tempBasePrice.Code = addBasePriceViewModel.NewBasePrice.Code;
                    tempBasePrice.Name = addBasePriceViewModel.NewBasePrice.Name.Trim();
                    tempBasePrice.Description = addBasePriceViewModel.NewBasePrice.Description.Trim();
                    tempBasePrice.IdCurrency = (byte)addBasePriceViewModel.NewBasePrice.IdCurrency;
                    tempBasePrice.Currency = ((Currency)addBasePriceViewModel.SelectedCurrency);
                    tempBasePrice.Plants = String.Join("\n", addBasePriceViewModel.NewBasePrice.PlantList.Select(a => a.Name));
                    tempBasePrice.PVPCurrencies = String.Join("\n", addBasePriceViewModel.NewBasePrice.CurrencyList.Select(a => a.Name));
                    if (addBasePriceViewModel.NewBasePrice.CurrencyList != null)
                    {
                        foreach (var bpItem in addBasePriceViewModel.NewBasePrice.CurrencyList.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                            bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);

                        }
                    }
                    tempBasePrice.PVPCurrencyWithBytes = addBasePriceViewModel.NewBasePrice.CurrencyList;
                    tempBasePrice.IdStatus = (uint)addBasePriceViewModel.SelectedStatus.IdLookupValue;
                    tempBasePrice.Status = addBasePriceViewModel.SelectedStatus;
                    tempBasePrice.EffectiveDate = addBasePriceViewModel.NewBasePrice.EffectiveDate;
                    tempBasePrice.Remark = addBasePriceViewModel.NewBasePrice.Remark;
                    tempBasePrice.ExpiryDate = addBasePriceViewModel.NewBasePrice.ExpiryDate;
                    tempBasePrice.LastUpdated = DateTime.Now;
                    tempBasePrice.ArticleCount = addBasePriceViewModel.NewBasePrice.BasePriceListItems.Select(a => a.IdArticle).Distinct().Count();
                    if (addBasePriceViewModel.NewBasePrice.AddDetectionList != null)
                        tempBasePrice.DetectionCount = addBasePriceViewModel.NewBasePrice.AddDetectionList.Select(a => a.IdDetection).Distinct().Count();

                    BasePricelist.Add(tempBasePrice);
                    BasePricelist = new ObservableCollection<BasePrice>(BasePricelist.OrderByDescending(a => a.Code));
                }
                IsBPLColumnChooserVisible = status;
                GeosApplication.Instance.Logger.Log("Method AddBasePriceAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddBasePriceAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshBasePriceAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillBasePrice();
                detailView.SearchString = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshBasePriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshBasePriceAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintBasePriceAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["BasePriceListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["BasePriceListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportBasePriceAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Base Prices";
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

                    GeosApplication.Instance.Logger.Log("Method ExportBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportBasePriceAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        private void DeleteBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteBasePriceAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedBasePrice.IsBPLExistInCPL == true)
                {
                    string CPLCodes = PLMService.GetCustomerPriceCodesByBPL(SelectedBasePrice.IdBasePriceList);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveBPLWithCPLValidation").ToString(), CPLCodes), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                else if (SelectedBasePrice.IdStatus == 223)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveBPLValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["BasePriceDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = PLMService.IsDeletedBasePriceList_V2180(SelectedBasePrice.IdBasePriceList, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsDeleted)
                    {
                        BasePricelist.Remove(SelectedBasePrice);
                        BasePricelist = new ObservableCollection<BasePrice>(BasePricelist);
                        SelectedBasePrice = BasePricelist.FirstOrDefault();
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BasePriceDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteBasePriceAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteBasePriceAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetectionItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

		//[nsatpute][GEOS2-7894][06.08.2025]
        private async void EditBasePriceAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBasePriceAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                AddEditBasePriceView editBasePriceView = new AddEditBasePriceView();
                AddEditBasePriceViewModel editBasePriceViewModel = new AddEditBasePriceViewModel();
                try
                {
                    if (DXSplashScreen.IsActive)
                        DXSplashScreen.Close();
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            Focusable = true,
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = false,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = false,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);

                    PLMCommon.Instance.ArticleCostPriceList = null;
                    editBasePriceViewModel.ArticleCostPriceList = null;
                    await editBasePriceViewModel.LoadAllDataInBackground();
                    //editBasePriceViewModel.LoadAllDataAsync();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditBasePriceAction().. DataInBackground() ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EventHandler handle = delegate { editBasePriceView.Close(); };
                editBasePriceViewModel.RequestClose += handle;
                editBasePriceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditBasePriceList").ToString();
                editBasePriceViewModel.IsNew = false;
                editBasePriceViewModel.IsFirstTimeLoad = true;
                bool status = IsBPLColumnChooserVisible;
                IsBPLColumnChooserVisible = false;
                editBasePriceViewModel.EditInitAsync(System.Windows.Application.Current.FindResource("EditBasePriceList").ToString());                
                editBasePriceViewModel.EditInitBasePrice(SelectedBasePrice);
                #region GEOS2-2889
                //Shubham[skadam] GEOS2-2889 [ Modules + Attachments ]- Duplicate the base price list [#PLM05] 24 04 2023
                editBasePriceViewModel.IsDuplicateBasePriceButtonVisible = Visibility.Visible;
                editBasePriceViewModel.IsDuplicateBasePriceButtonEnabled = true;
                #endregion
                editBasePriceView.DataContext = editBasePriceViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                TableView tableView = (TableView)obj;
                var ownerInfo = (tableView as FrameworkElement);
                editBasePriceView.Owner = Window.GetWindow(ownerInfo);
                editBasePriceView.ShowDialog();
                BasePrice tempBasePrice = new BasePrice();
                GridControl gridControl = (tableView).Grid;
                tempBasePrice = (BasePrice)tableView.DataControl.CurrentItem;
                if (editBasePriceViewModel.IsUpdatedBasePriceSave)
                {
                    tempBasePrice.IdBasePriceList = editBasePriceViewModel.UpdatedItem.IdBasePriceList;
                    tempBasePrice.Name = editBasePriceViewModel.UpdatedItem.Name.Trim();
                    tempBasePrice.Description = editBasePriceViewModel.UpdatedItem.Description.Trim();

                    List<Site> PlantList = new List<Site>();
                    List<Currency> CurrencyList = new List<Currency>();
                    if (editBasePriceViewModel.IsEnabledSaveButton && editBasePriceViewModel.SaveButtonVisibility == Visibility.Visible)
                    {
                        foreach (Site site in editBasePriceViewModel.ClonedBasePrice.PlantList)
                        {
                            PlantList.Add(site);
                        }
                        foreach (Currency cur in editBasePriceViewModel.ClonedBasePrice.CurrencyList)
                        {
                            CurrencyList.Add(cur);
                        }
                        tempBasePrice.IdCurrency = (byte)editBasePriceViewModel.ClonedBasePrice.IdCurrency;
                        tempBasePrice.Currency = ((Currency)editBasePriceViewModel.ClonedBasePrice.Currency);
                        if (editBasePriceViewModel.DtArticle != null)
                        {
                            var articleCount = editBasePriceViewModel.DtArticle.Rows.Count;
                            if (editBasePriceViewModel.AddArticles != null)
                                articleCount -= editBasePriceViewModel.AddArticles.Count;
                            if (editBasePriceViewModel.DeleteArticles != null)
                                articleCount += editBasePriceViewModel.DeleteArticles.Count;
                            tempBasePrice.ArticleCount = articleCount;
                        }
                        if (editBasePriceViewModel.DtDetection != null)
                        {
                            var detectionCount = editBasePriceViewModel.DtDetection.Rows.Count;
                            if (editBasePriceViewModel.AddDetections != null)
                                detectionCount -= editBasePriceViewModel.AddDetections.Count;
                            if (editBasePriceViewModel.DeleteDetections != null)
                                detectionCount += editBasePriceViewModel.DeleteDetections.Count;
                            tempBasePrice.DetectionCount = detectionCount;
                        }
                    }
                    else
                    {
                        foreach (Site site in editBasePriceViewModel.SelectedPlant)
                        {
                            PlantList.Add(site);
                        }
                        foreach (Currency cur in editBasePriceViewModel.SelectedSaleCurrency)
                        {
                            CurrencyList.Add(cur);
                        }
                        tempBasePrice.IdCurrency = (byte)editBasePriceViewModel.UpdatedItem.IdCurrency;
                        tempBasePrice.Currency = ((Currency)editBasePriceViewModel.SelectedCurrency);
                        tempBasePrice.ArticleCount = editBasePriceViewModel.DtArticle.Rows.Count;
                        tempBasePrice.DetectionCount = editBasePriceViewModel.DtDetection.Rows.Count;
                        tempBasePrice.ModuleCount = editBasePriceViewModel.DtModule.Rows.Count;
                    }
                    ImageSource currencyFlagImage = ByteArrayToBitmapImage(tempBasePrice.Currency.CurrencyIconbytes);
                    tempBasePrice.Currency.CurrencyIconImage = currencyFlagImage;
                    tempBasePrice.Plants = String.Join("\n", PlantList.Select(a => a.Name));
                    tempBasePrice.PVPCurrencies = String.Join("\n", CurrencyList.Select(a => a.Name));
                    if (CurrencyList != null)
                    {
                        foreach (var bpItem in CurrencyList.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                            bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);
                        }
                    }
                    tempBasePrice.PVPCurrencyWithBytes = CurrencyList;
                    if (tempBasePrice.PVPCurrencyWithBytes != null)
                    {
                        foreach (var item1 in tempBasePrice.PVPCurrencyWithBytes.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(item1.ToList().FirstOrDefault().CurrencyIconbytes);
                            item1.ToList().Where(oti => oti.Name == item1.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);
                        }
                    }
                    tempBasePrice.IdStatus = (uint)editBasePriceViewModel.SelectedStatus.IdLookupValue;
                    tempBasePrice.Status = editBasePriceViewModel.SelectedStatus;
                    tempBasePrice.EffectiveDate = editBasePriceViewModel.UpdatedItem.EffectiveDate;
                    tempBasePrice.Remark = editBasePriceViewModel.UpdatedItem.Remark;
                    tempBasePrice.ExpiryDate = editBasePriceViewModel.UpdatedItem.ExpiryDate;
                    tempBasePrice.LastUpdated = DateTime.Now;

                }
                else if (editBasePriceViewModel.IsDuplicateSave)
                {
                    BasePrice NewBasePrice = new BasePrice();
                    NewBasePrice.IdBasePriceList = editBasePriceViewModel.NewBasePrice.IdBasePriceList;
                    NewBasePrice.Code = editBasePriceViewModel.NewBasePrice.Code;
                    NewBasePrice.Name = editBasePriceViewModel.NewBasePrice.Name.Trim();
                    NewBasePrice.Description = editBasePriceViewModel.NewBasePrice.Description.Trim();
                    NewBasePrice.IdCurrency = (byte)editBasePriceViewModel.NewBasePrice.IdCurrency;
                    NewBasePrice.Currency = ((Currency)editBasePriceViewModel.SelectedCurrency);
                    ImageSource currencyFlagImage = ByteArrayToBitmapImage(NewBasePrice.Currency.CurrencyIconbytes);
                    tempBasePrice.Currency.CurrencyIconImage = currencyFlagImage;
                    NewBasePrice.Plants = String.Join("\n", editBasePriceViewModel.NewBasePrice.PlantList.Select(a => a.Name));
                    NewBasePrice.PVPCurrencies = String.Join("\n", editBasePriceViewModel.NewBasePrice.CurrencyList.Select(a => a.Name));
                    if (editBasePriceViewModel.NewBasePrice.CurrencyList != null)
                    {
                        foreach (var bpItem in editBasePriceViewModel.NewBasePrice.CurrencyList.GroupBy(tpa => tpa.Name))
                        {
                            ImageSource currencyFlagImage1 = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                            bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage1);
                        }
                    }
                    tempBasePrice.PVPCurrencyWithBytes = editBasePriceViewModel.NewBasePrice.CurrencyList;
                    tempBasePrice.IdStatus = (uint)editBasePriceViewModel.SelectedStatus.IdLookupValue;
                    NewBasePrice.Status = editBasePriceViewModel.SelectedStatus;
                    NewBasePrice.EffectiveDate = editBasePriceViewModel.NewBasePrice.EffectiveDate;
                    NewBasePrice.Remark = editBasePriceViewModel.NewBasePrice.Remark;
                    NewBasePrice.ExpiryDate = editBasePriceViewModel.NewBasePrice.ExpiryDate;
                    NewBasePrice.LastUpdated = DateTime.Now;
                    if (editBasePriceViewModel.NewBasePrice.BasePriceListItems != null && editBasePriceViewModel.DtArticle != null)
                    {
                        var articleCount = editBasePriceViewModel.NewBasePrice.BasePriceListItems.Select(a => a.IdArticle).Distinct().Count();
                        if (editBasePriceViewModel.AddArticles != null)
                            articleCount -= editBasePriceViewModel.AddArticles.Count;
                        if (editBasePriceViewModel.DeleteArticles != null)
                            articleCount += editBasePriceViewModel.DeleteArticles.Count;
                        NewBasePrice.ArticleCount = articleCount;
                    }
                    if (editBasePriceViewModel.NewBasePrice.AddDetectionList != null && editBasePriceViewModel.DtDetection != null)
                    {
                        var detectionCount = editBasePriceViewModel.NewBasePrice.AddDetectionList.Select(a => a.IdDetection).Distinct().Count();
                        if (editBasePriceViewModel.AddDetections != null)
                            detectionCount -= editBasePriceViewModel.AddDetections.Count;
                        if (editBasePriceViewModel.DeleteDetections != null)
                            detectionCount += editBasePriceViewModel.DeleteDetections.Count;
                        NewBasePrice.DetectionCount = detectionCount;
                    }
                    #region GEOS2-2889
                    if (editBasePriceViewModel.NewBasePrice.AddModuleList != null && editBasePriceViewModel.DtModule != null)
                    {
                        var moduleCount = editBasePriceViewModel.NewBasePrice.AddModuleList.Select(a => a.IdCPType).Distinct().Count();
                        if (editBasePriceViewModel.AddModules != null)
                            moduleCount -= editBasePriceViewModel.AddModules.Count;
                        if (editBasePriceViewModel.DeleteModules != null)
                            moduleCount += editBasePriceViewModel.DeleteModules.Count;
                        NewBasePrice.ModuleCount = moduleCount;
                    }
                    //Shubham[skadam] GEOS2-2889 [ Modules + Attachments ]- Duplicate the base price list [#PLM05] 24 04 2023
                    try
                    {
                        if (NewBasePrice.PVPCurrencyWithBytes == null)
                        {
                            List<Currency> CurrencyList = new List<Currency>();
                            foreach (Currency cur in editBasePriceViewModel.SelectedSaleCurrency)
                            {
                                CurrencyList.Add(cur);
                            }
                            NewBasePrice.PVPCurrencies = String.Join("\n", CurrencyList.Select(a => a.Name));
                            NewBasePrice.PVPCurrencyWithBytes = CurrencyList;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion
                    BasePricelist.Add(NewBasePrice);
                    BasePricelist = new ObservableCollection<BasePrice>(BasePricelist.OrderByDescending(a => a.Code));
                }
                IsBPLColumnChooserVisible = status;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditBasePriceAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditBasePriceAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "PVPCurrencies" && e.Column.FieldName != "Plants")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "PVPCurrencies")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PVPCurrencies])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PVPCurrencies])")
                    });

                    foreach (var dataObject in BasePricelist)
                    {
                        if (dataObject.PVPCurrencies == null)
                        {
                            continue;
                        }
                        else if (dataObject.PVPCurrencies != null)
                        {
                            if (dataObject.PVPCurrencies.Contains("\n"))
                            {
                                string tempPVPCurrencies = dataObject.PVPCurrencies;
                                for (int index = 0; index < tempPVPCurrencies.Length; index++)
                                {
                                    string empPVPCurrencies = tempPVPCurrencies.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPVPCurrencies))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPVPCurrencies;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PVPCurrencies Like '%{0}%'", empPVPCurrencies));
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == BasePricelist.Where(y => y.PVPCurrencies == dataObject.PVPCurrencies).Select(slt => slt.PVPCurrencies).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = BasePricelist.Where(y => y.PVPCurrencies == dataObject.PVPCurrencies).Select(slt => slt.PVPCurrencies).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PVPCurrencies Like '%{0}%'", BasePricelist.Where(y => y.PVPCurrencies == dataObject.PVPCurrencies).Select(slt => slt.PVPCurrencies).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Plants")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Plants])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Plants])")
                    });

                    foreach (var dataObject in BasePricelist)
                    {
                        if (dataObject.Plants == null)
                        {
                            continue;
                        }
                        else if (dataObject.Plants != null)
                        {
                            if (dataObject.Plants.Contains("\n"))
                            {
                                string tempPlants = dataObject.Plants;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", empPlants));
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == BasePricelist.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = BasePricelist.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", BasePricelist.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()));
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

        public void SetMinMaxDatesAndFillCurrencyConversions()
        {
            try
            {
                //if(PLMCommon.Instance.MinDate == DateTime.MinValue || PLMCommon.Instance.MaxDate == DateTime.MinValue)
                //{
                //    List<DateTime> Dates = PLMService.GetMinMaxArticleExchangeRateDate();
                //    if(Dates!=null && Dates.Count>0)
                //    {
                //        PLMCommon.Instance.MinDate = Dates[0];
                //        PLMCommon.Instance.MaxDate = Dates[1];
                //    }
                //}
                //if (PLMCommon.Instance.CurrencyConversionList == null || PLMCommon.Instance.CurrencyConversionList.Count == 0)
                //{
                //    PLMCommon.Instance.CurrencyConversionList = new List<CurrencyConversion>(PLMService.GetCurrencyConversionsMaxRate_V2110(PLMCommon.Instance.MinDate, PLMCommon.Instance.MaxDate));
                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetMinMaxDatesAndFillCurrencyConversions() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetMinMaxDatesAndFillCurrencyConversions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SetMinMaxDatesAndFillCurrencyConversions()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void BPLGridControlLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BPLGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(BPLGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(BPLGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(BPLGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, BPLVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, BPLVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 1)
                {
                    IsBPLColumnChooserVisible = true;
                }
                else
                {
                    IsBPLColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method BPLGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on BPLGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void BPLVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BPLVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(BPLGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsBPLColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method BPLVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in BPLVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void BPLVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BPLVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(BPLGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method BPLVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in BPLVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void BPLItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }

        private void BPLGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BPLGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(BPLGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method BPLGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on BPLGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
