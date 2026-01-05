using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class SystemSettingsViewModel:INotifyPropertyChanged
    {
        #region TaskLog
        //[Sprint_66]__[GEOS2-1603]__[Deshabilitar opcion inventario]__[sdesai]
        //[002][Sprint_72]__[GEOS2-1656]__[Add article Sleeping days column in warehouse section]__[sdesai]
        //[003][Sprint_78][avpawar][GEOS2-1889][Add a new setting to specify the customers requiring one order one box]
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private List<GeosAppSetting> geosAppSettingList;
        private ObservableCollection<Tuple<Int32, string>> selectedCustomerList;
        private ObservableCollection<Tuple<Int32, string>> customerList;


        
        private bool isBusy;
        #endregion

        #region Properties
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public ObservableCollection<Tuple<Int32, string>> SelectedCustomerList
        {
            get
            {
                return selectedCustomerList;
            }

            set
            {
                selectedCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerList"));
            }
        }

        public ObservableCollection<Tuple<Int32, string>> CustomerList
        {
            get
            {
                return customerList;
            }

            set
            {
                customerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerList"));
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

        #endregion

        #region Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events

        #region public ICommand

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddCustomerRowMouseDoubleClickCommand { get; set; }
        public ICommand CustomerCancelCommand { get; set; }
        #endregion // ICommand

        #region Constructor
        public SystemSettingsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel ...", category: Category.Info, priority: Priority.Low);

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
                    CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptButtonCommand = new RelayCommand(new Action<object>(SaveSystemSettings));
                AddCustomerRowMouseDoubleClickCommand = new DelegateCommand<object>(AddCustomerRowMouseDoubleClickCommandAction);
                CustomerCancelCommand = new DelegateCommand<object>(CustomerCancelCommandAction);
                FillGeosAppSettingList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// [001][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        /// </summary>
        private void FillGeosAppSettingList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()...", category: Category.Info, priority: Priority.Low);

                GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("24,25,30");
                if(GeosAppSettingList.Count > 0)
                {
                    string[] GeosAppSettingDefaultValues = GeosAppSettingList[0].DefaultValue.Split(',');

                    if (GeosAppSettingDefaultValues.Any(x => x.Contains(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString())))
                        WarehouseCommon.Instance.IsWarehouseInInventory = true;  
                    else
                        WarehouseCommon.Instance.IsWarehouseInInventory = false;

                    // [001] added
                    string[] RefillEmptyLocationsDefaultValues = GeosAppSettingList[1].DefaultValue.Split(',');

                    if (RefillEmptyLocationsDefaultValues.Any(x => x.Contains(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString())))
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = true;
                    else
                        WarehouseCommon.Instance.IsRefillEmptyLocationsFirst = false;

                    //[002]added
                    WarehouseCommon.Instance.ArticleSleepDays = Convert.ToInt32(GeosAppSettingList[2].DefaultValue);
                    FillCustomerList();

                }

               


                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGeosAppSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Fill Customer List
        /// </summary>
        private void FillCustomerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerList ...", category: Category.Info, priority: Priority.Low);


                // CustomerList = WarehouseService.GetEMDEPSites(); ;

                CustomerList = new ObservableCollection<Tuple<int, string>>(WarehouseService.GetEMDEPSites());

                SelectedCustomerList = new ObservableCollection<Tuple<int, string>>(WarehouseService.GetCustomersWithOneOrderBox().ToList());
                List<Int32> idsSites = SelectedCustomerList.Select(i => i.Item1).ToList();

                foreach (Tuple<Int32, string> item in CustomerList.Where(ik => idsSites.Contains(ik.Item1)).ToList())
                {
                    CustomerList.Remove(item);
                }

                GeosApplication.Instance.Logger.Log("Method FillCustomerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][24-07-2019][GEOS2-1667]Priorize first empty locations in Refill
        /// </summary>
        /// <param name="obj"></param>
        private void SaveSystemSettings(object obj)
        {
            try
            {
                string IsRefillEmptyLocations=string.Empty;
                GeosApplication.Instance.Logger.Log("Method SaveSystemSettings()...", category: Category.Info, priority: Priority.Low);


                bool result = WarehouseService.UpdateGeosAppSetting(24, WarehouseCommon.Instance.IsWarehouseInInventory, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                //[001] added
                bool IsResult = WarehouseService.UpdateGeosAppSetting(25, WarehouseCommon.Instance.IsRefillEmptyLocationsFirst, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                
                GeosAppSetting geosAppSetting = GeosAppSettingList[2];
                geosAppSetting.DefaultValue = WarehouseCommon.Instance.ArticleSleepDays.ToString();
                bool isSleepDay = WarehouseService.UpdateGeosAppSettingById(geosAppSetting);

                string OneOrderOneBox = string.Join(",", SelectedCustomerList.Select(x => x.Item1.ToString()).ToArray());
                GeosAppSetting geosAppSettingObj = new GeosAppSetting();
                geosAppSettingObj.DefaultValue = OneOrderOneBox;
                geosAppSettingObj.IdAppSetting = 34;
                bool isOneOrderOneBox = WarehouseService.UpdateGeosAppSettingById(geosAppSettingObj);

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method SaveSystemSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSystemSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveSystemSettings() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveSystemSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        ///  [003][Added][Add a new setting to specify the customers requiring one order one box]
        /// This Method for Add Customer
        /// </summary>
        /// <param name="e"></param>
        public void AddCustomerRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is Tuple<Int32,string>)
                {
                    Tuple<Int32, string> customer = obj as Tuple<Int32, string>;

                    if (!SelectedCustomerList.Any(x => x.Item1 == customer.Item1))
                    {
                        Tuple<Int32, string> selectedCustomer = CustomerList.FirstOrDefault(x => x.Item1 == customer.Item1);


                        if (selectedCustomer != null)
                        {
                            SelectedCustomerList.Add(selectedCustomer);                           
                            CustomerList.Remove(selectedCustomer);
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddCustomerRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddCustomerRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
        }


        /// <summary>
        ///  [003][Added][Add a new setting to specify the customers requiring one order one box]
        /// Method to delete Customer from Customers.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomerCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CustomerCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is Tuple<Int32,string>)
                {
                    Tuple<Int32, string> customer = obj as Tuple<Int32, string>;
                    CustomerList.Add((Tuple<Int32, string>)SelectedCustomerList.FirstOrDefault(x => x.Item1 == customer.Item1));
                    SelectedCustomerList.Remove(SelectedCustomerList.FirstOrDefault(x => x.Item1 == customer.Item1));
                }

                GeosApplication.Instance.Logger.Log("Method CustomerCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomerCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


    }
}
