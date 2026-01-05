using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.TSM;
using System.Windows.Media;
using System.Windows.Input;

using DevExpress.Mvvm.Native;
using DevExpress.XtraReports.Parameters;

namespace Emdep.Geos.Modules.TSM.ViewModels
{
    public class EditUserViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        //[nsatpute][GEOS2-5388][30-01-2025]
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
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ITSMService TSMService = new TSMServiceController("localhost:6699");

        #endregion 

        #region Commands
        public ICommand AddUserCancelButtonCommand { get; set; }
        public ICommand AddUserAcceptButtonCommand { get; set; }

        #endregion

        #region Declaration
        private TSMUsers tsmUser;
        private string windowHeader;
        private bool isSave;
        bool isBusy;
       
        private string fullName;
        private bool isUpdateUser;
        private string username;
        private string organization;
        private List<LookupValue> _usersLookUpValuesList;
        private List<LookupValue> allPermissionList;
       
        private List<object> selectedPermissions;    
        bool isNew;
        private UInt32 idTechnicianUser;
        private UInt32 idCustomerApplication;
        
        private TSMUsers editUser;
        private string employeeCodeWithIdGender;
        #endregion

        #region Properties
        public UInt32 IdTechnicianUser
        {
            get { return idTechnicianUser; }
            set { idTechnicianUser = value; OnPropertyChanged(new PropertyChangedEventArgs("IdTechnicianUser")); }
        }

        public UInt32 IdCustomerApplication
        {
            get { return idCustomerApplication; }
            set { idCustomerApplication = value; OnPropertyChanged(new PropertyChangedEventArgs("IdCustomerApplication")); }
        }
        public List<LookupValue> UsersLookUpValuesList
        {
            get { return _usersLookUpValuesList; }
            set
            {
                _usersLookUpValuesList = value;
                OnPropertyChanged(nameof(UsersLookUpValuesList));
            }
        }
        public List<LookupValue> AllPermissionList
        {
            get { return allPermissionList; }
            set
            {
                _usersLookUpValuesList = value;
                OnPropertyChanged(nameof(allPermissionList));
            }
        }

        public List<object> SelectedPermissions
        {
            get { return selectedPermissions; }
            set
            {
                selectedPermissions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPermissions"));
              
            }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public TSMUsers EditUser
        {
            get { return editUser; }
            set { editUser = value; OnPropertyChanged(new PropertyChangedEventArgs("EditUser")); }
        }
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }

        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Organization"));
            }
        }
        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; }
        }
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; OnPropertyChanged(new PropertyChangedEventArgs("IsUpdateUser")); }
        }
        public TSMUsers User
        {
            get { return tsmUser; }
            set
            {
                SetProperty(ref tsmUser, value, () => User);
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
        //[nsatpute][GEOS2-5388][30-01-2025]
        public string EmployeeCodeWithIdGender
        {
            get { return employeeCodeWithIdGender; }
            set
            {
                employeeCodeWithIdGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCodeWithIdGender"));
            }
        }
        #endregion // Properties

        #region Constructor
        public EditUserViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditUserViewModel...", category: Category.Info, priority: Priority.Low);
                AddUserAcceptButtonCommand = new RelayCommand(new Action<object>(EditUserAcceptAction));
                AddUserCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                GeosApplication.Instance.Logger.Log("Constructor EditUserViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditUserViewModel() Constructor...", category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        private void EditUserAcceptAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditUserAcceptAction()...", category: Category.Info, priority: Priority.Low);
                EditUser.IdPermissions = new List<int>();
                if (SelectedPermissions != null)
                {
                    SelectedPermissions.ForEach(x => {
                        LookupValue permission = UsersLookUpValuesList.FirstOrDefault(y=> y.Value ==  ((LookupValue)x).Value);
                        if(permission != null)
                            EditUser.IdPermissions.Add(permission.IdLookupValue);
                    });                    
                }
                IsUpdateUser = TSMService.UpdateUserPermissions(EditUser, UsersLookUpValuesList);

                if (IsUpdateUser == false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserSaveFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UserUpdateSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method EditUserAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditUserAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = true;
            RequestClose(null, null);

        }
        public void EditInit(TSMUsers SelectedRow,  List<LookupValue> allPermissionList)
        {
            try
            {
                EditUser = SelectedRow;
                AllPermissionList = allPermissionList;
                SelectedPermissions = new List<object>();
                if(EditUser.IdPermissions != null)
                {
                    foreach( int i in EditUser.IdPermissions)
                    {
                        SelectedPermissions.Add(allPermissionList.FirstOrDefault(x => x.IdLookupValue == i));
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        #endregion

        #region validation
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

        #endregion

    }
}
