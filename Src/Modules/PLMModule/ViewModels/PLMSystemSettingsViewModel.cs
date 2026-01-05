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
using Emdep.Geos.Modules.PLM.Views;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
  public  class PLMSystemSettingsViewModel : INotifyPropertyChanged
    {
        #region Services
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Decalrartion
        private bool isBusy;
        private List<GeosAppSetting> systemSettinglist;
        private GeosAppSetting selectedSystemSetting;
        private string name;
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

        public List<GeosAppSetting> SystemSettinglist
        {
            get { return systemSettinglist; }
            set
            {
                systemSettinglist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemSettinglist"));
            }
        }

        public GeosAppSetting SelectedSystemSetting
        {
            get { return selectedSystemSetting; }
            set
            {
                selectedSystemSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSystemSetting"));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
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
        #region public ICommand


        public ICommand CancelButtonCommand { get; set; }
        public ICommand SynchronizeCommand { get; set; }


        #endregion // ICommand

        #region Constructor
        public PLMSystemSettingsViewModel()
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
                SynchronizeCommand = new RelayCommand(new Action<object>(SynchronizeAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SystemSettinglist = new List<GeosAppSetting>();
                SystemSettinglist.Add(new GeosAppSetting { IdAppSetting = 1, AppSettingName = "Product Prices" });
                SystemSettinglist.Add(new GeosAppSetting { IdAppSetting = 2, AppSettingName = "Detection Prices" });
          

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

        private void SynchronizeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SynchronizeAction()...", category: Category.Info, priority: Priority.Low);

                PLMSynchronizationView synchronizationView = new PLMSynchronizationView();
                PLMSynchronizationViewModel synchronizationViewModel = new PLMSynchronizationViewModel();
                EventHandler handle = delegate { synchronizationView.Close(); };
                synchronizationViewModel.RequestClose += handle;

                if (SelectedSystemSetting.IdAppSetting == 1)
                {
                    Name = SelectedSystemSetting.AppSettingName;
                }
                else if (SelectedSystemSetting.IdAppSetting == 2)
                {
                    Name = SelectedSystemSetting.AppSettingName;
                }
                else
                {
                    Name = SelectedSystemSetting.AppSettingName;
                }
                synchronizationViewModel.Init(Name);
                synchronizationView.DataContext = synchronizationViewModel;
                //EventHandler handle = delegate { synchronizationView.Close(); };
                //synchronizationViewModel.RequestClose += handle;
                var ownerInfo = (obj as FrameworkElement);
                synchronizationView.Owner = Window.GetWindow(ownerInfo);
                synchronizationView.ShowDialog();
                
               
                GeosApplication.Instance.Logger.Log("Method SynchronizeAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SynchronizeAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SynchronizeAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SynchronizeAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        #endregion
    }
}
