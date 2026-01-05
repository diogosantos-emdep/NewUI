using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Data.Extensions;
using DevExpress.Xpf.Grid;
using System.Windows.Media;
using Emdep.Geos.Modules.SCM.Common_Classes;

namespace Emdep.Geos.Modules.SCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4502]
    public class AddListValueViewModel : INotifyPropertyChanged, IDataErrorInfo
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
        #endregion

        #region Declaration
        ObservableCollection<Language> languages;
        Language selectedLanguage;
        private string windowHeader;
        private string name;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;
        private Int32 idLookupValue;
        private Int32 idLookupKey;
        private string htmlColor;
        private Int32 position;
        private string abbreviation;
        private int inUse;
        private string informationError;
        private bool isCheckedCopyName;
        bool isSave;
        bool isNew;
        LookUpValues newLookUpValuesDetails;
        LookUpValues updateLookUpValuesDetails;
        #endregion

        #region Properties
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }
        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLanguage"));
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string Name_en
        {
            get { return name_en; }
            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
            }
        }
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_es"));
            }
        }
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_fr"));
            }
        }
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_pt"));
            }
        }
        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ro"));
            }
        }
        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ru"));
            }
        }
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
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
        public Int32 IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdLookupKey"));
            }

        }
        
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HtmlColor"));
            }
        }
       
        public Int32 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Position"));
            }
        }
       
        public string Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Abbreviation"));
            }
        }
       
        public int InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public bool IsCheckedCopyName
        {
            get { return isCheckedCopyName; }
            set
            {
                isCheckedCopyName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyName"));
                UncheckedCopyName(null);
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
        public LookUpValues NewLookUpValuesDetails
        {
            get { return newLookUpValuesDetails; }
            set
            {
                newLookUpValuesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewLookUpValuesDetails"));
            }
        }
        public LookUpValues UpdateLookUpValuesDetails
        {
            get { return updateLookUpValuesDetails; }
            set
            {
                updateLookUpValuesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateLookUpValuesDetails"));
            }
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
        #endregion

        #region Public ICommand
        public ICommand AddListValueViewCancelButtonCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand UncheckedCopyNameCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public AddListValueViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddCustomPropertyViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddListValueViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                IsCheckedCopyName = true;
                ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                UncheckedCopyNameCommand = new DelegateCommand<object>(UncheckedCopyName);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor AddCustomPropertyViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomPropertyViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init(ValueKey SelectedValueKey)
        {
            IdLookupKey = SelectedValueKey.IdLookupKey;
            Position = 1;
            AddLanguage();

        }
        private void AddLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);
                Languages = new ObservableCollection<Language>(SCMService.GetAllLanguages());
                SelectedLanguage = Languages.FirstOrDefault();
               
                GeosApplication.Instance.Logger.Log("Method AddLanguages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLanguages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLanguages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditInit(LookUpValues SelectedLookupValue)
        {

           
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                AddLanguage();
                Name = SelectedLookupValue.Value_en;
                Name_en= SelectedLookupValue.Value_en;
                Name_es = SelectedLookupValue.Value_es;
                Name_fr = SelectedLookupValue.Value_es;
                Name_pt = SelectedLookupValue.Value_pt;
                Name_ro = SelectedLookupValue.Value_ro;
                Name_ru = SelectedLookupValue.Value_ru;
                Name_zh = SelectedLookupValue.Value_zh;
                IdLookupValue = SelectedLookupValue.IdLookupValue;
                IdLookupKey = SelectedLookupValue.IdLookupKey;
                HtmlColor = SelectedLookupValue.HtmlColor;
                Position = SelectedLookupValue.Position;
                Abbreviation = SelectedLookupValue.Abbreviation;
                InUse = SelectedLookupValue.InUse;
                if(Name == Name_en && Name== Name_es && Name == Name_fr && Name == Name_pt && Name == Name_ro && Name == Name_ru && Name == Name_zh)
                {
                    IsCheckedCopyName = true;
                }
                else
                {
                    IsCheckedCopyName = false;
                }
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()...", category: Category.Info, priority: Priority.Low);


                if (IsCheckedCopyName == false && SelectedLanguage != null)
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        InformationError = string.Empty; ;
                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    }
                    else
                    {
                        InformationError = null;
                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    }
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                        Name_en = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        Name_es = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                        Name_fr = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                        Name_pt = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                        Name_ro = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                        Name_ru = Name;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
                    {
                        Name_zh = Name;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetNameToLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);


                if (IsCheckedCopyName == false)
                {
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                       
                        Name = Name_en;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        
                        Name = Name_es;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                       
                        Name = Name_fr;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                      
                        Name = Name_pt;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                       
                        Name = Name_ro;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                       
                        Name = Name_ru;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
                    {
                        
                        Name = Name_zh;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveDescriptionByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UncheckedCopyName(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()...", category: Category.Info, priority: Priority.Low);

                if (SelectedLanguage != null)
                {
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                        
                        Name = Name_en;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        
                        Name = Name_es;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                        
                        Name = Name_fr;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                       
                        Name = Name_pt;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                       
                        Name = Name_ro;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                       
                        Name = Name_ru;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
                    {
                      
                        Name = Name_zh;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UncheckedCopyDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                InformationError = null;
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                if (error != null)
                {
                    return;
                }

                if (IsNew)
                {
                    NewLookUpValuesDetails = new LookUpValues();

                    if (IsCheckedCopyName == true)
                    {
                        NewLookUpValuesDetails.Value_en = Name;
                        NewLookUpValuesDetails.Value_es = Name;
                        NewLookUpValuesDetails.Value_fr = Name;
                        NewLookUpValuesDetails.Value_pt = Name;
                        NewLookUpValuesDetails.Value_ro = Name;
                        NewLookUpValuesDetails.Value_ru = Name;
                        NewLookUpValuesDetails.Value_zh = Name;

                       
                    }
                    else
                    {

                        NewLookUpValuesDetails.Value_en = Name_en;
                        NewLookUpValuesDetails.Value_es = Name_es;
                        NewLookUpValuesDetails.Value_fr = Name_fr;
                        NewLookUpValuesDetails.Value_pt = Name_pt;
                        NewLookUpValuesDetails.Value_ro = Name_ro;
                        NewLookUpValuesDetails.Value_ru = Name_ru;
                        NewLookUpValuesDetails.Value_zh = Name_zh;
                        
                    }
                    NewLookUpValuesDetails.IdLookupKey = IdLookupKey;
                    NewLookUpValuesDetails.HtmlColor = HtmlColor;
                    NewLookUpValuesDetails.Position = Position;
                    NewLookUpValuesDetails.Abbreviation = Abbreviation;
                    NewLookUpValuesDetails.InUse = InUse;
                   

                        IsSave = SCMService.InsertLookupKey_V2420(NewLookUpValuesDetails);
                    if (IsSave == true)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValueListAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }

                }
                else
                {
                    UpdateLookUpValuesDetails = new LookUpValues();

                    if (IsCheckedCopyName == true)
                    {
                        UpdateLookUpValuesDetails.Value_en = Name;
                        UpdateLookUpValuesDetails.Value_es = Name;
                        UpdateLookUpValuesDetails.Value_fr = Name;
                        UpdateLookUpValuesDetails.Value_pt = Name;
                        UpdateLookUpValuesDetails.Value_ro = Name;
                        UpdateLookUpValuesDetails.Value_ru = Name;
                        UpdateLookUpValuesDetails.Value_zh = Name;


                    }
                    else
                    {

                        UpdateLookUpValuesDetails.Value_en = Name_en;
                        UpdateLookUpValuesDetails.Value_es = Name_es;
                        UpdateLookUpValuesDetails.Value_fr = Name_fr;
                        UpdateLookUpValuesDetails.Value_pt = Name_pt;
                        UpdateLookUpValuesDetails.Value_ro = Name_ro;
                        UpdateLookUpValuesDetails.Value_ru = Name_ru;
                        UpdateLookUpValuesDetails.Value_zh = Name_zh;

                    }
                    UpdateLookUpValuesDetails.IdLookupKey = IdLookupKey;
                    UpdateLookUpValuesDetails.HtmlColor = HtmlColor;
                    UpdateLookUpValuesDetails.Position = Position;
                    UpdateLookUpValuesDetails.Abbreviation = Abbreviation;
                    UpdateLookUpValuesDetails.InUse = InUse;
                    UpdateLookUpValuesDetails.IdLookupValue = IdLookupValue;
                    
                        
                        IsSave = SCMService.UpdateLookupKey_V2420(UpdateLookUpValuesDetails);
                   if(IsSave==true)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValueListUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }

                }


               

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                me[BindableBase.GetPropertyName(() => Name)] +
                me[BindableBase.GetPropertyName(() => InformationError)];


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
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);

                if (columnName == name)
                {
                    return AddEditCustompropertyValidation.GetErrorMessage(name,null, Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditCustompropertyValidation.GetErrorMessage(headerInformtionError,null, InformationError);
                }

                return null;
            }
        }


       
        #endregion
    }
}
