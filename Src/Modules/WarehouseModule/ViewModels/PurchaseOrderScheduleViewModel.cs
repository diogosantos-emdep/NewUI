using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PurchaseOrderScheduleViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private ObservableCollection<WarehousePurchaseOrder> mainPurchaseOrderList;
        private ObservableCollection<ModelAppointment> appointmentsMainList;

        #endregion // Declaration

        #region Properties
       
        public ObservableCollection<WarehousePurchaseOrder> MainPurchaseOrderList
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

        public ObservableCollection<ModelAppointment> AppointmentsMainList
        {
            get { return appointmentsMainList; }
            set
            {
                appointmentsMainList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentsMainList"));
            }
        }

        //** this variable is temporary for show background color of schedules.

        private AppointmentLabelCollection labels;
        public AppointmentLabelCollection Labels
        {
            get
            {
                return labels;
            }

            set
            {
                labels = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Labels"));
            }
        }

        //** this variable is temporary for show background color of schedules.

        #endregion // Properties

        #region Commands

        public ICommand AllowEditScheduleCommand { get; private set; }
        public ICommand PopupMenuShowingCommand { get; private set; }
        public ICommand RefreshScheduleViewCommand { get; private set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }

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
        public PurchaseOrderScheduleViewModel()
        {
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                // DXSplashScreen.Show<SplashScreenView>(); 
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

            PopupMenuShowingCommand = new DelegateCommand<SchedulerMenuEventArgs>(PopupMenuShowingAction);
            AllowEditScheduleCommand = new DelegateCommand<object>(AllowEditScheduleAction);
            RefreshScheduleViewCommand = new DelegateCommand<object>(RefreshActivityDetails);
            CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
        

            //**Temp code for show backgorund color.
            // Labels = new AppointmentLabelCollection();
            // ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            // Labels.Clear();
            // List<LookupValue> GetLookupValues = CrmStartUp.GetLookupValues(9).ToList();
            //foreach (var item in GetLookupValues)
            //{
            //    Labels.Add(Labels.CreateNewLabel(item.IdLookupValue, item.Value, item.Value, (Color)ColorConverter.ConvertFromString(item.HtmlColor != null ? item.HtmlColor : "#FFFFFF")));
            //}

            Labels = new AppointmentLabelCollection();
            Labels.Clear();

            //[WMS-M037-11] Hardcoded colors in pending PO----Mayuri 
            List<GeosAppSetting> PendingPOColorList = WorkbenchService.GetSelectedGeosAppSettings("2,3,4,5");

            foreach (var item in PendingPOColorList)
            {
                Labels.Add(Labels.CreateNewLabel(item.IdAppSetting, item.AppSettingName, item.AppSettingName, (Color)ColorConverter.ConvertFromString(item.DefaultValue != null ? item.DefaultValue : "#FFFFFF")));
            }

            //**Temp code for show backgorund color.

            FillMainPurchaseOrderList();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }


        #endregion // Constructor

        #region Methods

        /// <summary>
        /// after selection  fill list as per warehouse.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);

            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
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


            FillMainPurchaseOrderList();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Methdod for fill purchase order list. 
        /// </summary>
        private void FillMainPurchaseOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList...", category: Category.Info, priority: Priority.Low);

                if(WarehouseCommon.Instance.Selectedwarehouse != null )
                {
                    ObservableCollection<WarehousePurchaseOrder> TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>();
                    MainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>();

                    //List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();

                 //   foreach (var item in selectedwarehouselist.GroupBy(x => x.IdSite))
                    {
                      
                        Warehouses Warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                       
                        TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReceptionByWarehouse(Warehouse.IdWarehouse.ToString(), Warehouse));
                        MainPurchaseOrderList.AddRange(TempMainPurchaseOrderList);
                    }
                    
                    //MainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReception());
                }
                else
                {
                    MainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>();
                    AppointmentsMainList = new ObservableCollection<ModelAppointment>();
                }

                if (MainPurchaseOrderList != null && MainPurchaseOrderList.Count > 0)
                {
                    AppointmentsMainList = new ObservableCollection<ModelAppointment>();
         
                    foreach (var item in MainPurchaseOrderList)
                    {
                        ModelAppointment modelActivity = new ModelAppointment();
                        modelActivity.AppointmentId = item.IdWarehousePurchaseOrder;
                        if (item.DeliveryDate != null)
                        {
                            modelActivity.StartTime = item.DeliveryDate.Value;
                            modelActivity.EndTime = item.DeliveryDate.Value.AddMinutes(30);

                            if (item.DeliveryDate.Value.Date < GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt64(Labels[0].Id);
                            else if (item.DeliveryDate.Value.Date == GeosApplication.Instance.ServerDateTime.Date)
                                modelActivity.Label = Convert.ToInt64(Labels[1].Id);
                            else if (item.DeliveryDate.Value.Date > GeosApplication.Instance.ServerDateTime.Date)
                            {
                                if (item.DeliveryDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date.AddDays(7))
                                    modelActivity.Label = Convert.ToInt64(Labels[3].Id);
                                else
                                    modelActivity.Label = Convert.ToInt64(Labels[2].Id);
                            } 
                        }
                        else
                        {
                            DateTime baseTime = DateTime.Today;
                            modelActivity.StartTime = baseTime.AddMinutes(30);
                            modelActivity.EndTime = baseTime.AddMinutes(30);
                            modelActivity.Label = Convert.ToInt64(Labels[1].Id);
                        }
                        modelActivity.Subject = item.ArticleSupplier.Name;
                        modelActivity.Description = item.Code;

                        // modelActivity.Subject = "[" + item.Code + "] " + item.ArticleSupplier.Name;
                        // modelActivity.Description = "[" + item.Code + "] " + item.ArticleSupplier.Name;
                        //modelActivity.StartTime = item.DeliveryDate;
                        //modelActivity.EndTime = item.DeliveryDate;
                        // modelActivity.AllDay = true;                    
                        modelActivity.Status = Convert.ToInt32(item.IdWarehouse);
                        AppointmentsMainList.Add(modelActivity);
                    }

                    AppointmentsMainList = new ObservableCollection<ModelAppointment>(AppointmentsMainList);
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
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
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        ///  Method for refill data from server.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshActivityDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshActivityDetails...", category: Category.Info, priority: Priority.Low);

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                // DXSplashScreen.Show<SplashScreenView>(); 
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

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshActivityDetails executed successfully.", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for edit PO Schedule.
        /// </summary>
        /// <param name="obj"></param>
        private void AllowEditScheduleAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction...", category: Category.Info, priority: Priority.Low);
                SchedulerControl detailView = (SchedulerControl)((object[])obj)[0];
                EditAppointmentFormEventArgs editAppointmentObject = (EditAppointmentFormEventArgs)((object[])obj)[1];
                editAppointmentObject.Cancel = true;
                if (obj == null) return;
                if (Convert.ToInt32(editAppointmentObject.Appointment.Id) > 0)
                {
                    // Activity activity = ActivityList.FirstOrDefault(Act => Act.IdActivity == (Convert.ToInt64(obj.Appointment.Id)));

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


                    PurchaseOrderItemDetailsViewModel purchaseOrderItemDetailsViewModel = new PurchaseOrderItemDetailsViewModel();
                    PurchaseOrderItemDetailsView purchaseOrderItemDetailsView = new PurchaseOrderItemDetailsView();

                    EventHandler handle = delegate { purchaseOrderItemDetailsView.Close(); };
                    purchaseOrderItemDetailsViewModel.RequestClose += handle;

                    int idOt = Convert.ToInt32(editAppointmentObject.Appointment.Id);
                    //List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                    //Warehouses objWarehouse = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == Convert.ToInt32(obj.Appointment.StatusId));

                    Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                    purchaseOrderItemDetailsViewModel.Init(idOt, warehouse);
                    purchaseOrderItemDetailsView.DataContext = purchaseOrderItemDetailsViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    purchaseOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                    purchaseOrderItemDetailsView.ShowDialogWindow();

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction executed successfully.", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AllowEditAppointmentAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method for hide option from Calendar view on Right click.
        /// </summary>
        /// <param name="parameter"></param>
        private void PopupMenuShowingAction(SchedulerMenuEventArgs parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingAction ...", category: Category.Info, priority: Priority.Low);

                if (parameter.Menu.Name == SchedulerMenuItemName.AppointmentMenu)
                {
                    object open = parameter.Menu.Items.FirstOrDefault(x => x is SchedulerBarItem && (string)((SchedulerBarItem)x).Content == "Open");

                    parameter.Menu.Items.Clear();
                    parameter.Menu.Items.Add((SchedulerBarItem)open);

                }

                GeosApplication.Instance.Logger.Log("Method PopupMenuShowingAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PopupMenuShowingAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            
        }


        #endregion // Methods
    }
}
