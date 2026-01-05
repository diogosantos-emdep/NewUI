using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class SystemSettingsViewModel : INotifyPropertyChanged
    {
        #region Services
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        //ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        ObservableCollection<SystemSettings> systemSettingList;
        ObservableCollection<Warehouses> customMessage;
        bool isUserPermission;
        bool isUserEditPermission;
        #endregion

        #region Properties
        public ObservableCollection<Warehouses> WarehouseFooterMessageList
        {
            get { return customMessage; }
            set
            {
                customMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseFooterMessageList"));
            }
        }
        public ObservableCollection<SystemSettings> SystemSettingList
        {
            get { return systemSettingList; }
            set
            {
                systemSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemSettingList"));
            }
        }
        public bool IsUserPermission
        {
            get { return isUserPermission; }
            set
            {
                isUserPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserPermission"));
            }
        }
        public bool IsUserEditPermission
        {
            get { return isUserEditPermission; }
            set
            {
                isUserEditPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserEditPermission"));
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
        #endregion

        #region Public ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion

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
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);

                ObservableCollection<GeosAppSetting> temp = new ObservableCollection<GeosAppSetting>(SRMService.GetSystemSettings_V2390());

                SystemSettingList = temp.FirstOrDefault().SystemSettings;

                //retriving custom Message
                WarehouseFooterMessageList = new ObservableCollection<Warehouses>(SRMService.GetCustomMessageforHoliday_V2500());

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 84))
                {
                    IsUserPermission = false;
                }
                else
                {
                    IsUserPermission = true;
                }


                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 84))
                {
                    IsUserEditPermission = true;
                }
                else
                {
                    IsUserEditPermission = false;
                }

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

        #region Method
        public void Init()
        {

        }

        public void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()...", category: Category.Info, priority: Priority.Low);
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

                var listcount = SystemSettingList.Count();
                string result = "";

                for (int i = 0; i < listcount; i++)
                {
                    string warehouse = SystemSettingList[i].Warehouse;
                    string cc = SystemSettingList[i].CC;

                    if (i == 0)
                    {
                        result += $"({warehouse};{cc})";
                    }
                    else
                    {
                        result += $",({warehouse};{cc})";
                    }
                }
                var DefaultValue = result;

                bool data = SRMService.UpdateSystemSettings_V2390(DefaultValue);




                var customMessagecount = WarehouseFooterMessageList.Count();
                List<Warehouses> warehouses = new List<Warehouses>();

                for (int i = 0; i < customMessagecount; i++)
                {
                    long warehouseId = WarehouseFooterMessageList[i].IdWarehouse;
                    string customMessage = WarehouseFooterMessageList[i].CustomMessage;
                    DateTime? startdate = WarehouseFooterMessageList[i].ImportantNoticeStartDate;
                    DateTime? enddate = WarehouseFooterMessageList[i].ImportantNoticeEndDate;
                    bool Isenabled = WarehouseFooterMessageList[i].IsImportantNoticeEnabled;
                    Warehouses warehouse = new Warehouses
                    {
                        IdWarehouse = warehouseId,
                        CustomMessage = customMessage,
                        ImportantNoticeStartDate = startdate,
                        ImportantNoticeEndDate = enddate,
                        IsImportantNoticeEnabled=Isenabled
                    };
                    
                    warehouses.Add(warehouse);
                }

                // Now you have a list of default values for each record
                // Pass this list to your method
                bool success = SRMService.UpdateCustomMessageForPORequestMail_V2500(warehouses);



                RequestClose(null, null);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
