using DevExpress.Mvvm;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

// [Shweta.Thube][GEOS2 - 8061]
namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class NotificationSettingsViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");

        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events

        #region Declarations
        private string windowHeader;
        private bool inUse;
        private int use;
        private APMActionPlanTask manualAttachmentSetting;
        #endregion

        #region  public Properties
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }
        public int Use
        {
            get
            {
                return use;
            }

            set
            {
                use = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Use"));
            }
        }
        public APMActionPlanTask ManualAttachmentSetting
        {
            get
            {
                return manualAttachmentSetting;
            }

            set
            {
                manualAttachmentSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManualAttachmentSetting"));
            }
        }
        
        #endregion

        #region ICommands
        public ICommand CancelButtonCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }
        public APMActionPlanTask TempAPMActionPlanTask { get; private set; }
        #endregion

        #region Constructor
        public NotificationSettingsViewModel()
        {
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));

        }
        #endregion

        #region Methods
        private void CancelButtonCommandAction(object obj)
        {
            try
            {

                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {

                if (InUse == true)
                { Use = 1; }
                else
                { Use = 0; }

                TempAPMActionPlanTask = new APMActionPlanTask()
                {
                    IdAppSetting = ManualAttachmentSetting.IdAppSetting,
                    SettingInUse = (byte)Use,
                };

                bool IsUpdated = APMService.UpdateManualAttachmentSetting_V2640(TempAPMActionPlanTask);
                if (IsUpdated)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ManualAttachmentSettingSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }


                FillManualAttachmentSetting();
                if(ManualAttachmentSetting.SettingInUse == 1)
                {
                    InUse = true;
                }
                else
                {
                    InUse = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillManualAttachmentSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillManualAttachmentSetting ...", category: Category.Info, priority: Priority.Low);

                ManualAttachmentSetting = APMService.GetManualAttachmentSetting(156);

                GeosApplication.Instance.Logger.Log("Method FillManualAttachmentSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillManualAttachmentSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Validation

        #endregion
    }
}
