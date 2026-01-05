using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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
   public class SearchUserViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Commands
        public ICommand SelectUserAcceptButtonCommand { get; set; }
        public ICommand SelectUserCancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declartion
        private bool isSave;
        private bool isBusy;
        private int selectedUser;
        private int allUsers;
        private List<User> listUser;
        private List<string> userGenderList;
        #endregion

        #region Public Properties

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; }
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

        public int SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser")); }
        }

        public User SearchUsers { get; set; }

        public int AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("ListUser")); }
        }

        public List<User> ListUser
        {
            get { return listUser; }
            set { listUser = value; OnPropertyChanged(new PropertyChangedEventArgs("ListUser")); }
        }
        
        public List<string> UserGenderList
        {
            get { return userGenderList; }
            set
            {
                userGenderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserGenderList"));
            }
        }
        #endregion

        #region Constructor
        public SearchUserViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SearchUserViewModel...", category: Category.Info, priority: Priority.Low);

                SelectUserAcceptButtonCommand = new RelayCommand(new Action<object>(ExisintUserAcceptAction));
                SelectUserCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillUser();
                GeosApplication.Instance.Logger.Log("Constructor SearchUserViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SearchUserViewModel() Constructor...", category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void FillUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("FillUser...", category: Category.Info, priority: Priority.Low);

                ListUser = WorkbenchStartUp.GetAllUser().OrderBy(o => o.Login).ToList();
                SelectedUser = -1;
                GeosApplication.Instance.Logger.Log("FillUser executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ExisintUserAcceptAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("ExisintUserAcceptAction...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            string error = EnableValidationAndGetError();
            OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser"));

            if (error != null)
            {
                IsBusy = false;
                return;
            }

            if (!IsSave)
            {
                try
                {
                    IsSave = true;
                    
                    AllUsers = ListUser[SelectedUser].IdUser;

                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method ExisintUserAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ExisintUserAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ExisintUserAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ExisintUserAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            else
            {
                //ContactData = null;
                IsSave = false;
            }

            IsBusy = true;
            RequestClose(null, null);
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

        #region Validation
        public string Error
        {
            get
            {
                return "";
            }
        }

        public string this[string columnName]
        {
            get
            {
                return "";
            }
        }
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
                  
                    me[BindableBase.GetPropertyName(() => SelectedUser)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                
                string SelectedUserProp = BindableBase.GetPropertyName(() => SelectedUser);

               
                if (columnName == SelectedUserProp)
                    return UserValidation.GetErrorMessage(SelectedUserProp, SelectedUser);

                return null;
            }
        }

        #endregion
    }
}
