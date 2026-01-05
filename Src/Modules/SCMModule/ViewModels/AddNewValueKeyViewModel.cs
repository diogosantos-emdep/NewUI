using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;



namespace Emdep.Geos.Modules.SCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4502]
    public class AddNewValueKeyViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        public void Dispose()
        {

        }
        #endregion

        #region Declaration
        private string name;
        private string error = string.Empty;
        ValueKey newValueKeyList;
        ValueKey updateValueKeyList;
        string informationError;
        bool isSave;
        Int32 idLookupValue;
        bool isNew;
        private string geosSettingsKey;
        private List<ValueKey> getLookupkeylist = new List<ValueKey>();
        private string windowHeader;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public ValueKey NewValueKeyList
        {
            get { return newValueKeyList; }
            set
            {
                newValueKeyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewValueKeyList"));
            }
        }
        public ValueKey UpdateValueKeyList
        {
            get { return updateValueKeyList; }
            set
            {
                updateValueKeyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateValueKeyList"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdLookupValue"));
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
        public string GeosSettingsKey
        {
            get { return geosSettingsKey; }
            set
            {
                geosSettingsKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosSettingsKey"));
            }
        }
        public List<ValueKey> GetLookupkeylist
        {
            get { return getLookupkeylist; }
            set
            {
                getLookupkeylist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GetLookupkeylist"));
            }
        }
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
        List<string> LookUpKeyNameList = new List<string>();
        #endregion

        #region Public ICommand
        public ICommand AddNewValueKeyViewCancelButtonCommand { get; set; }
        public ICommand AddNewValueKeyAcceptButton { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public AddNewValueKeyViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddCustomPropertyViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddNewValueKeyViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddNewValueKeyAcceptButton = new RelayCommand(new Action<object>(AddNewValueKeyAcceptButtonAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor AddCustomPropertyViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomPropertyViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private List<GeosSettings> geosSettings = new List<GeosSettings>();
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init(string GeosSettingsKeyString)
        {
            try
            {
                
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                GeosSettingsKey = GeosSettingsKeyString;
                GetAllRecordForLookpKey();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void EditInit(ValueKey valueKey)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Name = valueKey.LookupKeyName;
                IdLookupValue = valueKey.IdLookupKey;
                GetAllRecordForLookpKey();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void AddNewValueKeyAcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewValueKeyAcceptButtonAction()...", category: Category.Info, priority: Priority.Low);
                
                allowValidation = true;

                LookUpKeyNameList = GetLookupkeylist.Select(a => a.LookupKeyName).ToList();
                
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));


                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = " ";

                if (error != null)
                    {
                        return;
                    }
                
                   


                    //int KeyCount = 0;
                    //KeyCount = GetLookupkeylist.Where(a => a.LookupKeyName == Name).Count();
                    //if (Name!=null && KeyCount>0)
                    //{
                    //    InformationError = " Duplicate name.";
                    //    return;
                    //}
                    //else
                    //{
                        
                    //}
                    
               
                if (IsNew)
                {
                    NewValueKeyList = new ValueKey();
                    NewValueKeyList.LookupKeyName = Name;
                    IsSave = SCMService.AddNewValueKey(NewValueKeyList, GeosSettingsKey);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("KeyAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    UpdateValueKeyList = new ValueKey();
                    UpdateValueKeyList.IdLookupKey = IdLookupValue;
                    UpdateValueKeyList.LookupKeyName = Name;
                    IsSave = SCMService.UpdateValueKey(UpdateValueKeyList);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("KeyUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddNewValueKeyAcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewValueKeyAcceptButtonAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewValueKeyAcceptButtonAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewValueKeyAcceptButtonAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void GetAllRecordForLookpKey()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAllRecordForLookpKey()...", category: Category.Info, priority: Priority.Low);
                GetLookupkeylist = new List<ValueKey>();
                GetLookupkeylist = SCMService.GetAllLookupKey();
                GeosApplication.Instance.Logger.Log("Method GetAllRecordForLookpKey()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllRecordForLookpKey() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllRecordForLookpKey() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAllRecordForLookpKey()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
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

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                                me[BindableBase.GetPropertyName(() => Name)];
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
                string name = BindableBase.GetPropertyName(() => Name);
                if (columnName == name)
                {
                    return AddEditNewValueKeyValidation.GetErrorMessage(name, LookUpKeyNameList, Name);
                }
                return null;
            }
        }


        #endregion
    }
}
