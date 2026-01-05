using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class RfqReceptionViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service

        #endregion

        #region Declaration

        public bool IsSave { get; set; }
        private DateTime? rfqReceptionDate;
        private bool isBusy;

        #endregion

        #region Properties

        public DateTime? RFQReceptionDate
        {
            get { return rfqReceptionDate; }
            set
            {
                rfqReceptionDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RFQReceptionDate"));
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

        #region ICommands

        public ICommand RfqReceptionCancelButtonCommand { get; set; }
        public ICommand RfqReceptionAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;

            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => RFQReceptionDate)];

                return RequiredValidationRule.GetErrorMessage(error, RFQReceptionDate, 2);

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string rfqReceptionProp = BindableBase.GetPropertyName(() => RFQReceptionDate);

                if (columnName == rfqReceptionProp)
                    return RequiredValidationRule.GetErrorMessage(rfqReceptionProp, RFQReceptionDate, 2);

                return null;
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

        #region Constructor

        public RfqReceptionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RfqReceptionViewModel ...", category: Category.Info, priority: Priority.Low);

                RfqReceptionCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                RfqReceptionAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(RfqReceptionAcceptAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("RFQReceptionDate"));

                GeosApplication.Instance.Logger.Log("Constructor RfqReceptionViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RfqReceptionViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods

        private void RfqReceptionAcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RfqReceptionAcceptAction ...", category: Category.Info, priority: Priority.Low);

                if (RFQReceptionDate != null)
                {
                    IsSave = true;
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method RfqReceptionAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                // OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in RfqReceptionAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                //OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in RfqReceptionAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                //OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in RfqReceptionAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
