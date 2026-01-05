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
    public class SendQuoteViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service
        #endregion

        #region Declaration
        public bool IsSave { get; set; }
        private bool isBusy;

        private DateTime? quoteSentDate;

        #endregion

        #region Properties

        public DateTime? QuoteSentDate
        {
            get
            {
                return quoteSentDate;
            }

            set
            {
                quoteSentDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuoteSentDate"));
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
        public ICommand SendQuoteCancelButtonCommand { get; set; }
        public ICommand SendQuoteAcceptButtonCommand { get; set; }

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
                    me[BindableBase.GetPropertyName(() => QuoteSentDate)];
                return RequiredValidationRule.GetErrorMessage(error, QuoteSentDate, 1);

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string quoteSentDateProp = BindableBase.GetPropertyName(() => QuoteSentDate);

                if (columnName == quoteSentDateProp)
                    return RequiredValidationRule.GetErrorMessage(quoteSentDateProp, QuoteSentDate, 1);

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
        public SendQuoteViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SendQuoteViewModel ...", category: Category.Info, priority: Priority.Low);

                SendQuoteCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                SendQuoteAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(SendQuoteAcceptAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("QuoteSentDate"));

                GeosApplication.Instance.Logger.Log("Constructor SendQuoteViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendQuoteViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods

        private void SendQuoteAcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendQuoteAcceptAction ...", category: Category.Info, priority: Priority.Low);

                if (QuoteSentDate != null)
                {
                    IsSave = true;
                    RequestClose(null, null);
                }
                else
                {
                }

                GeosApplication.Instance.Logger.Log("Method SendQuoteAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                // OfferData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SendQuoteAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SendQuoteAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SendQuoteAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
