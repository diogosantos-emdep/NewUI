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
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WorkOrderScheduleViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion // Services

        #region Declaration

        //bool isBusy;
        private List<Ots> mainOtsList;
        private List<ModelAppointment> appointmentsMainOtsList;
        private List<Quotation> mainQuotationsList;

        #endregion // Declaration

        #region Properties
        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }

        public List<ModelAppointment> AppointmentsMainOtsList
        {
            get { return appointmentsMainOtsList; }
            set
            {
                appointmentsMainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppointmentsMainOtsList"));
            }
        }

        //** this variable is temporary for show background color of schedules.

        private AppointmentLabelCollection labels;
        public AppointmentLabelCollection Labels
        {
            get { return labels; }
            set
            {
                labels = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Labels"));
            }
        }

        public List<Quotation> MainQuotationsList
        {
            get { return mainQuotationsList; }
            set
            {
                mainQuotationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainQuotationsList"));
            }
        }
        //** this variable is temporary for show background color of schedules.

        private List<Emdep.Geos.Data.Common.ResourceStorage> resourceStorageList;
        public List<Emdep.Geos.Data.Common.ResourceStorage> ResourceStorageList
        {
            get { return resourceStorageList; }
            set
            {
                resourceStorageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResourceStorageList"));
            }
        }

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
        public WorkOrderScheduleViewModel()
        {
            PopupMenuShowingCommand = new DevExpress.Mvvm.DelegateCommand<SchedulerMenuEventArgs>(PopupMenuShowingAction);
            AllowEditScheduleCommand = new DevExpress.Mvvm.DelegateCommand<object>(AllowEditScheduleAction);
            RefreshScheduleViewCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshActivityDetails);
            CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);

            //**Temp code for show backgorund color.
            Labels = new AppointmentLabelCollection();

            List<GeosStatus> geosStatuslist = new List<GeosStatus>(CrmStartUp.GetGeosOfferStatus());

            foreach (var item in geosStatuslist)
            {
                Labels.Add(Labels.CreateNewLabel(item.IdOfferStatusType, item.Name, "", (Color)ColorConverter.ConvertFromString(item.SalesStatusType != null ? item.SalesStatusType.HtmlColor : "#FFFFFF")));
            }

            List<LookupValue> GetLookupValues = CrmStartUp.GetLookupValues(12).ToList();

            ResourceStorageList = new List<Data.Common.ResourceStorage>();
            LookupValue luv = new LookupValue();
            luv.Value = "N/A";
            luv.IdImage = 0;
            GetLookupValues.Insert(0, luv);

            LookupValue luvCritical = new LookupValue();
            luvCritical.Value = "isCritical";
            luvCritical.IdImage = -1;
            GetLookupValues.Add(luvCritical);

            foreach (LookupValue lookupValue in GetLookupValues)
            {
                Data.Common.ResourceStorage rs = new Data.Common.ResourceStorage();
                rs.Id = Convert.ToInt32(lookupValue.IdImage);
                rs.Model = Convert.ToString(lookupValue.Value);

                if (lookupValue.IdImage == 0)
                    rs.Picture = null;
                else if (lookupValue.IdImage == 1)
                    rs.Picture = ImageToByte(Emdep.Geos.Modules.Warehouse.Properties.Resources.Ground_32);
                else if (lookupValue.IdImage == 2)
                    rs.Picture = ImageToByte(Emdep.Geos.Modules.Warehouse.Properties.Resources.Sea_32);
                else if (lookupValue.IdImage == 3)
                    rs.Picture = ImageToByte(Emdep.Geos.Modules.Warehouse.Properties.Resources.Air_32);
                else if (lookupValue.IdImage == -1)
                    rs.Picture = ImageToByte(Emdep.Geos.Modules.Warehouse.Properties.Resources.Obsolete);

                ResourceStorageList.Add(rs);
            }

            //**Temp code for show backgorund color.

            FillAppointmentMainOTList();
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

            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

            FillAppointmentMainOTList();
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter Tiles 
        /// </summary>
        /// 
        public void FillAppointmentMainOTList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAppointmentMainOTList....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();
                    MainQuotationsList = new List<Quotation>();

                    //  List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                    //   foreach (var item in selectedwarehouselist.GroupBy(x => x.IdSite))
                    //{ }
                    //Warehouses Warehouse = WarehouseCommon.Instance.Selectedwarehouse;

                    TempMainOtsList = new List<Ots>(WarehouseService.GetPendingMaterialWorkOrdersByWarehouse(Convert.ToString(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse), WarehouseCommon.Instance.Selectedwarehouse));
                    MainOtsList.AddRange(TempMainOtsList);

                    // All quotations of type "SITES" where offer is forecasted.
                    MainQuotationsList.AddRange(WarehouseService.GetAllQuotationsOfTypeSites(GeosApplication.Instance.ActiveUser.IdUser, 1, null, null, WarehouseCommon.Instance.Selectedwarehouse));

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                    AppointmentsMainOtsList = new List<ModelAppointment>();
                }

                AppointmentsMainOtsList = new List<ModelAppointment>();

                if (MainOtsList != null && MainOtsList.Count > 0)
                    foreach (Ots ot in MainOtsList)
                    {
                        ModelAppointment modelAppointment = new ModelAppointment();
                        modelAppointment.AppointmentId = ot.IdOT;
                        modelAppointment.Label = Convert.ToInt64(ot.Quotation.Offer.IdStatus);

                        if (ot.Quotation.Offer.CarriageMethod != null)
                        {
                            modelAppointment.ResourceId = (int)ot.Quotation.Offer.CarriageMethod.IdImage;
                        }
                        else
                            modelAppointment.ResourceId = 0;

                        modelAppointment.IsCritical = Convert.ToBoolean(ot.Quotation.Offer.IsCritical);

                        if (!string.IsNullOrEmpty(ot.Quotation.Site.ShortName))
                        {
                            modelAppointment.Subject = ot.Quotation.Site.Name;
                        }
                        else
                        {
                            modelAppointment.Subject = string.Format("{0} - {1}", ot.Quotation.Site.Customer.CustomerName, ot.Quotation.Site.Name);
                        }

                        modelAppointment.Description = string.Format("{0} OT {1}", ot.Code, ot.NumOT);

                        if (ot.PoDate == null || ot.PoDate == DateTime.MinValue)
                        {
                            modelAppointment.StartTime = ot.DeliveryDate;
                        }
                        else
                        {
                            modelAppointment.StartTime = ot.PoDate; // item.DeliveryDate;
                        }

                        modelAppointment.EndTime = ot.DeliveryDate;
                        modelAppointment.Status = Convert.ToInt32(ot.Quotation.IdWarehouse);
                        modelAppointment.Tag = ot;

                        AppointmentsMainOtsList.Add(modelAppointment);
                    }

                if (MainQuotationsList != null && MainQuotationsList.Count > 0)
                    foreach (Quotation quotation in MainQuotationsList)
                    {
                        ModelAppointment modelAppointment = new ModelAppointment();
                        modelAppointment.AppointmentId = quotation.IdQuotation;
                        modelAppointment.Label = Convert.ToInt64(15);

                        if (quotation.Offer.CarriageMethod != null)
                            modelAppointment.ResourceId = (int)quotation.Offer.CarriageMethod.IdImage;
                        else
                            modelAppointment.ResourceId = 0;

                        modelAppointment.IsCritical = Convert.ToBoolean(quotation.Offer.IsCritical);

                        if (string.IsNullOrEmpty(quotation.Site.ShortName))
                        {
                            modelAppointment.Subject = quotation.Site.Name;
                        }
                        else
                        {
                            modelAppointment.Subject = string.Format("{0} - {1}", quotation.Site.Customer.CustomerName, quotation.Site.Name);
                        }

                        modelAppointment.Description = quotation.Code;

                        modelAppointment.StartTime = quotation.Offer.DeliveryDate.Value.Date;  // item.DeliveryDate;
                        modelAppointment.EndTime = quotation.Offer.DeliveryDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                        modelAppointment.Status = 1;
                        modelAppointment.Tag = quotation;
                        AppointmentsMainOtsList.Add(modelAppointment);
                    }

                AppointmentsMainOtsList = new List<ModelAppointment>(AppointmentsMainOtsList);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillAppointmentMainOTList() executed successfully", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in FillAppointmentMainOTList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  Method for refill data from server.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshActivityDetails(object obj)
        {
            FillAppointmentMainOTList();
        }

        /// <summary>
        /// Method for edit PO Schedule.
        /// </summary>
        /// <param name="obj"></param>
        private void AllowEditScheduleAction(object obj) //EditAppointmentFormEventArgs
        {
            try
            {
               // EditAppointmentFormEventArgs
                GeosApplication.Instance.Logger.Log("Method AllowEditAppointmentAction...", category: Category.Info, priority: Priority.Low);
                SchedulerControl detailView = (SchedulerControl)((object[])obj)[0];
                EditAppointmentFormEventArgs editAppointmentObject = (EditAppointmentFormEventArgs)((object[])obj)[1];

                editAppointmentObject.Cancel = true;
                if (obj == null) return;

                


                if (Convert.ToInt32(editAppointmentObject.Appointment.Id) > 0 && editAppointmentObject.Appointment.CustomFields["Tag"] is Ots)
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

                    GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

                    WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                    WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                    EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                    workOrderItemDetailsViewModel.RequestClose += handle;

                    int idOT = Convert.ToInt32(editAppointmentObject.Appointment.Id);

                    // List<Warehouses> selectedwarehouselist = WarehouseCommon.Instance.SelectedwarehouseList.Cast<Warehouses>().ToList();
                    // Warehouses objWarehouses = selectedwarehouselist.FirstOrDefault(x => x.IdWarehouse == Convert.ToInt32(obj.Appointment.StatusId));
                    //Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                    workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                    workOrderItemDetailsViewModel.Init(idOT, WarehouseCommon.Instance.Selectedwarehouse);
                    workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                    workOrderItemDetailsView.ShowDialogWindow();

                    //if download quantity is zero then remove item from main grid. 
                    if (workOrderItemDetailsViewModel.DownloadRemainingQuantity == 0)
                    {
                        MainOtsList.Remove(MainOtsList.Where(oti => oti.IdOT == idOT).FirstOrDefault());
                        AppointmentsMainOtsList.Remove(AppointmentsMainOtsList.Where(app => app.AppointmentId == idOT).FirstOrDefault());

                        MainOtsList = new List<Ots>(MainOtsList);
                        AppointmentsMainOtsList = new List<ModelAppointment>(AppointmentsMainOtsList);
                    }

                    GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);

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

        /// <summary>
        /// This method is used to get bytes from bitmap
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public void Dispose()
        {
        }

        #endregion // Methods
    }
}
