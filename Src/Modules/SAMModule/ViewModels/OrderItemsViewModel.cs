using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Newtonsoft.Json;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    class OrderItemsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {


        #region Services

        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");
        #endregion // End Services Region

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declaration
        private List<OTs> mainItemOtsList = new List<OTs>();
        bool isBusy;
        private object geosAppSettingList;
        private DataTable dttable;
        private DataTable dttableCopy;
        private string filterString;
        private bool isWorkOrderColumnChooserVisible;
        public string PendingItemsGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SAM_ItemsGridSetting.Xml";
        // private object geosAppSettingList;
        #endregion

        #region Properties
        public List<OTs> MainItemOtsList
        {
            get { return mainItemOtsList; }
            set
            {
                mainItemOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainItemOtsList"));
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
        public object GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }

        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }
        public virtual string ResultFileName { get; set; }

        public virtual bool DialogResult { get; set; }

        #region GEOS2-8857
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
        List<string> failedPlants;
        List<string> successPlantList;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
        }

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        #endregion

        #endregion

        #region ICommands

        public ICommand RefreshItemCommand { get; set; }
        public ICommand PrintItemCommand { get; set; }
        public ICommand ExportItemCommand { get; set; }
        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand WorkOrderViewHyperlinkClickCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        public ICommand EditCommand { get; set; }
        #endregion

        #region Constructor

        public OrderItemsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OrderItemsViewModel....", category: Category.Info, priority: Priority.Low);

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
                FillListOfColor();
                //Init();
                RefreshItemCommand = new RelayCommand(new Action<object>(RefreshItems));
                PrintItemCommand = new RelayCommand(new Action<object>(PrintItemList));
                ExportItemCommand = new RelayCommand(new Action<object>(ExportItemList));
                WorkOrderViewHyperlinkClickCommand = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                EditCommand = new RelayCommand(new Action<object>(EditAction));
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor OrdersReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrdersReportViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion //End Of Constructor

        #region Methods
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
            try
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                await FillMainItemOtListAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Init()
        {
            FillMainItemOtList();
            //FillListOfColor();
        }

        private void FillMainItemOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {

                    List<OTs> TempMainItemOtsList = new List<OTs>();
                    MainItemOtsList = new List<OTs>();
                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            //Service GetAllOrderItemsList_V2350 updated with GetAllOrderItemsList_V2430 by [rdixit][28.08.2023][GEOS2-4754]
                            TempMainItemOtsList = new List<OTs>(SAMService.GetAllOrderItemsList_V2430(plant));
                            TempMainItemOtsList.ForEach(f => f.Site = plant);
                            MainItemOtsList.AddRange(TempMainItemOtsList);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    MainItemOtsList = new List<OTs>(MainItemOtsList);
                }
                else
                {
                    MainItemOtsList = new List<OTs>();
                }


                GeosApplication.Instance.Logger.Log("Method FillMainOtList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainOtList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
        private async Task FillMainItemOtListAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainItemOtListAsync...", Category.Info, Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList == null)
                {
                    MainItemOtsList = new List<OTs>();
                    return;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants ...";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                var successPlants = new List<string>();
                var failedPlants = new List<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                List<OTs> TempMainItemOtsList = new List<OTs>();
                MainItemOtsList = new List<OTs>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    try
                    {
                        // Initialize plant status
                        foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                        }
                        try
                        {
                            var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                    // Run service call asynchronously using Task.Run (because service is sync)
                                    var list = await Task.Run(() =>SAMService.GetAllOrderItemsList_V2430(plant));
                                    // Assign site to each OT
                                    list.ForEach(f => f.Site = plant);
                                    // Run service call asynchronously
                                    lock (MainItemOtsList)
                                    {
                                        MainItemOtsList.AddRange(list);
                                        successPlants.Add(plant.Alias);
                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lock (FailedPlants)
                                    {
                                        if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                            FailedPlants.Add(plant.Alias);
                                        IsShowFailedPlantWarning = true;
                                        WarningFailedPlants = string.Format((string)System.Windows.Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                        var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                        if (statusMsg != null) statusMsg.IsSuccess = 2;
                                    }
                                    GeosApplication.Instance.Logger.Log($"Error fetching work log data for plant {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                                }
                            });
                            await Task.WhenAll(tasks);
                            MainItemOtsList = new List<OTs>(MainItemOtsList);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() method " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() Method - ServiceUnexceptedException " + ex.Message, Category.Exception, Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                            GeosApplication.Instance.Logger.Log("Error in Method FillMainItemOtListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainItemOtListAsync() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainItemOtListAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                else
                {
                    MainItemOtsList = new List<OTs>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainItemOtListAsync() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }

        private void RefreshItems(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()...", category: Category.Info, priority: Priority.Low);

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
                /*
                FillMainItemOtList();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                var tableView = (TableView)obj;
                tableView.BeginInit();
                tableView.SearchString = null;
                FilterString = string.Empty;
                tableView.EndInit();
                */
                 RefreshItemsAsync(obj);//Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025



                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                //  CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDetectionView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
        public async Task RefreshItemsAsync(object obj)
        {
            try
            {
                await FillMainItemOtListAsync();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                var tableView = (TableView)obj;
                tableView.BeginInit();
                tableView.SearchString = null;
                FilterString = string.Empty;
                tableView.EndInit();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshItemsAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

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
                var ot = (OTs)detailView.DataControl.CurrentItem;

                long OtId = ot.IdOT;
                Ots FoundRow = new Ots();
                OTs ots = MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault();
                try
                {

                    FoundRow = JsonConvert.DeserializeObject<Ots>(JsonConvert.SerializeObject(ots));
                }
                catch (Exception ex)
                {
                    FoundRow.IdOT = ots.IdOT;
                    FoundRow.Site = ots.Site;
                }
                //AddEditWorkOrderItemViewModel workOrderViewModel = new AddEditWorkOrderItemViewModel();
                //AddEditWorkOrderItemView workOrderView = new AddEditWorkOrderItemView();

                //    EventHandler handle1 = delegate { workOrderView.Close(); };
                //    workOrderViewModel.RequestClose += handle1;
                //    workOrderViewModel.Init(FoundRow);
                //    workOrderView.DataContext = workOrderViewModel;
                //    workOrderView.ShowDialogWindow();
                //    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (FoundRow.Quotation.IdDetectionsTemplate == 24 || FoundRow.Quotation.IdDetectionsTemplate == 8)
                {
                    WorkOrderItemDetailsForStructureViewModel workOrderItemDetailsForStructureViewModel = new WorkOrderItemDetailsForStructureViewModel();
                    WorkOrderItemDetailsForStructureView workOrderItemDetailsForStructureView = new WorkOrderItemDetailsForStructureView();

                    EventHandler handle1 = delegate { workOrderItemDetailsForStructureView.Close(); };
                    workOrderItemDetailsForStructureViewModel.RequestClose += handle1;
                    workOrderItemDetailsForStructureViewModel.Init(FoundRow);
                    workOrderItemDetailsForStructureView.DataContext = workOrderItemDetailsForStructureViewModel;
                    workOrderItemDetailsForStructureView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                }
                else if (FoundRow.Quotation.IdDetectionsTemplate == 19)
                {
                    WorkOrderItemDetailsForElectrificationViewModel workOrderItemDetailsForElectrificationViewModel = new WorkOrderItemDetailsForElectrificationViewModel();
                    WorkOrderItemDetailsForElectrificationView workOrderItemDetailsForElectrificationView = new WorkOrderItemDetailsForElectrificationView();

                    EventHandler handle = delegate { workOrderItemDetailsForElectrificationView.Close(); };
                    workOrderItemDetailsForElectrificationViewModel.RequestClose += handle;
                    workOrderItemDetailsForElectrificationViewModel.Init(FoundRow);
                    workOrderItemDetailsForElectrificationView.DataContext = workOrderItemDetailsForElectrificationViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsForElectrificationView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }


                    MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsForElectrificationViewModel.Remark;
                    //FillDataTable();
                }
                else if (FoundRow.Quotation.IdDetectionsTemplate == 18)
                {
                    WorkOrderItemDetailsForQualityCertificationViewModel workOrderItemDetailsForQualityCertificationViewModel = new WorkOrderItemDetailsForQualityCertificationViewModel();
                    WorkOrderItemDetailsForQualityCertificationView workOrderItemDetailsForQualityCertificationView = new WorkOrderItemDetailsForQualityCertificationView();

                    EventHandler handle = delegate { workOrderItemDetailsForQualityCertificationView.Close(); };
                    workOrderItemDetailsForQualityCertificationViewModel.RequestClose += handle;
                    workOrderItemDetailsForQualityCertificationViewModel.Init(FoundRow);
                    workOrderItemDetailsForQualityCertificationView.DataContext = workOrderItemDetailsForQualityCertificationViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsForQualityCertificationView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    if (workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus != null)
                    {
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsForQualityCertificationViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsForQualityCertificationViewModel.Remark;
                    //FillDataTable();
                    if (workOrderItemDetailsForQualityCertificationViewModel.IsCancel == false)
                    {
                        //Init();
                        InitAsync();//Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
                    }
                }
                else
                {
                    WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                    WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                    EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                    workOrderItemDetailsViewModel.RequestClose += handle;
                    workOrderItemDetailsViewModel.Init(FoundRow);
                    workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    workOrderItemDetailsView.ShowDialogWindow();
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    if (workOrderItemDetailsViewModel.WorkflowStatus != null)
                    {
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus;
                        MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().WorkflowStatus.IdWorkflowStatus = workOrderItemDetailsViewModel.WorkflowStatus.IdWorkflowStatus;
                    }
                    MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault().Observations = workOrderItemDetailsViewModel.Remark;
                    //FillDataTable();
                    if (workOrderItemDetailsViewModel.IsCancel == false)
                    {
                        //Init();
                        InitAsync();//Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
                    }

                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region GEOS2-3682
        private void EditAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                TableView detailView = (TableView)obj;
                var ot = (OTs)detailView.DataControl.CurrentItem;

                long OtId = ot.IdOT;
                Ots FoundRow = new Ots();
                OTs ots = MainItemOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault();
                try
                {

                    FoundRow = JsonConvert.DeserializeObject<Ots>(JsonConvert.SerializeObject(ots));
                }
                catch (Exception ex)
                {
                    FoundRow.IdOT = ots.IdOT;
                    FoundRow.Site = ots.Site;
                }
                //OT = SAMService.GetSAMOrderItemsInformationByIdOt_V2340(tempOtItem.IdOT, OtSite, Convert.ToUInt32(tempOtItem.RevisionItem.WarehouseProduct.IdArticle));

                EditItemView editItemView = new EditItemView();
                EditItemViewModel editItemViewModel = new EditItemViewModel();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EventHandler handle = delegate { editItemView.Close(); };
                editItemViewModel.RequestClose += handle;
                editItemViewModel.IsNew = false;
                editItemView.DataContext = editItemViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                // DevExpress.Xpf.Grid.TreeListView tableView = (DevExpress.Xpf.Grid.TreeListView)obj;
                // var ownerInfo = (tableView as FrameworkElement);
                // OtItem tempOtItem = (OtItem)tableView.DataControl.CurrentItem;
                //editItemView.Owner = Window.GetWindow(ownerInfo);
                editItemViewModel.EditInit(FoundRow, ots.Article.IdArticle, ots.IdRevisionItem);
                editItemView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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
            /*
            FillMainItemOtList();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            */
            PlantOwnerPopupClosedActionAsync();//Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }
        //Shubham[skadam] [V.2.6.9.0] GEOS2-8857 SAM module very slow when trying to load informations - Orders ->Items (3/6) 25 11 2025
        public async Task PlantOwnerPopupClosedActionAsync()
        {
            try
            {
                await FillMainItemOtListAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerPopupClosedActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintItemList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()...", category: Category.Info, priority: Priority.Low);
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
                //pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintHeaderTemplate"];
                //pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportItemList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Items Report";
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
                    GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()...", category: Category.Info, priority: Priority.Low);
                if (e.ColumnFieldName == "CostDeviation")
                {
                    e.Formatting.Alignment = new XlCellAlignment() { HorizontalAlignment = XlHorizontalAlignment.Right };
                    e.Formatting.FormatType = DevExpress.Utils.FormatType.Numeric;
                    e.Formatting.NumberFormat = "0%";
                }

                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method Options_CustomizeCell()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Options_CustomizeCell()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(PendingItemsGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PendingItemsGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(PendingItemsGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingItemsGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingItemsGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }
        #endregion //End Of Method


    }
}
