using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Views;
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

namespace Emdep.Geos.Modules.SCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4503][13/07/2023]
    public class EditCustomPropertyViewModel
    {
        #region Service
         ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      // ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        string windowHeader;
        ObservableCollection<Language> languages;
        Language selectedLanguage;
        private string name;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;

        private string description;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;

        private bool isCheckedCopyDescription;
        CustomProperty updateCustomPropertyDetails;

        private double dialogHeight;
        private double dialogWidth;

        private ObservableCollection<Data.Common.SCM.ValueType> valueTypeList;//[Sudhir.Jangra][GEOS2-4505]
        private Data.Common.SCM.ValueType selectedValueType;//[Sudhir.Jangra][GEOS2-4505]

        private ObservableCollection<ValueKey> valueKeyList;//[Sudhir.Jangra][GEOS2-4505]
        private ValueKey selectedValueKey;//[Sudhir.Jangra][GEOS2-4505]
        private Int32 idCustomConnectorProperty;//[Sudhir.Jangra][GEOS2-4503]

        bool isSave;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
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
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        public string Description_en
        {
            get { return description_en; }
            set
            {
                description_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
            }
        }
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
            }
        }
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            }
        }
        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
            }
        }
        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
            }
        }
        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
            }
        }
        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
            }
        }
        public bool IsCheckedCopyDescription
        {
            get { return isCheckedCopyDescription; }
            set
            {
                isCheckedCopyDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyDescription"));
                //  UncheckedCopyDescription(null);
            }
        }

        public CustomProperty UpdateCustomPropertyDetails
        {
            get { return updateCustomPropertyDetails; }
            set
            {
                updateCustomPropertyDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateCustomPropertyDetails"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public ObservableCollection<Data.Common.SCM.ValueType> ValueTypeList
        {
            get { return valueTypeList; }
            set
            {
                valueTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueTypeList"));
            }
        }
        public Data.Common.SCM.ValueType SelectedValueType
        {
            get { return selectedValueType; }
            set
            {
                selectedValueType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedValueType"));
            }
        }
        public ObservableCollection<ValueKey> ValueKeyList
        {
            get { return valueKeyList; }
            set
            {
                valueKeyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueKeyList"));
            }
        }
        public ValueKey SelectedValueKey
        {
            get { return selectedValueKey; }
            set
            {
                selectedValueKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedValueKey"));
            }
        }
        public Int32 IdCustomConnectorProperty
        {
            get { return idCustomConnectorProperty; }
            set
            {
                idCustomConnectorProperty = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCustomConnectorProperty"));
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
        public ICommand EditCustomPropertyViewCancelButton { get; set; }
        public ICommand UncheckedCopyDescriptionCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeProductTypeDescriptionCommand { get; set; }
        public ICommand ChangeProductTypeNameCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        public ICommand AddNewValueKeyButtonCommand { get; set; }

        public ICommand EditValueKeyButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }        //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public EditCustomPropertyViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditCustomPropertyViewModel ...", category: Category.Info, priority: Priority.Low);
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                IsCheckedCopyDescription = true;
                EditCustomPropertyViewCancelButton = new RelayCommand(new Action<object>(CloseWindow));
                //  UncheckedCopyDescriptionCommand = new DelegateCommand<object>(UncheckedCopyDescription);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                ChangeProductTypeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeProductTypeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                AddNewValueKeyButtonCommand = new RelayCommand(new Action<object>(AddNewValueKeyButtonCommandAction));
                EditValueKeyButtonCommand = new RelayCommand(new Action<object>(EditValueKeyButtonCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);        //[shweta.thube][GEOS2-6630][04.04.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor EditCustomPropertyViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditCustomPropertyViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init(Int32 id)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                var temp = SCMService.GetEditCustomProperty(id);
                IdCustomConnectorProperty = id;
                Name = temp.Name;
                Description = temp.Description;

                ValueTypeList = new ObservableCollection<Data.Common.SCM.ValueType>(SCMService.GetValueType());
                ValueKeyList = new ObservableCollection<ValueKey>(SCMService.GetValueKey());

                SelectedValueType = ValueTypeList.FirstOrDefault(x => x.IdLookupValue == temp.ValueType.IdLookupValue);

                SelectedValueKey = ValueKeyList.FirstOrDefault(x => x.IdLookupValue == temp.ValueKey.IdLookupValue);
                AddLanguage();
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
        private void AddLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);
                Languages = new ObservableCollection<Language>(SCMService.GetAllLanguages());
                SelectedLanguage = Languages.FirstOrDefault();
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                 Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyDescription = true;
                }
                else
                {
                    IsCheckedCopyDescription = false;
                }
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
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetDescriptionToLanguage()...", category: Category.Info, priority: Priority.Low);
                if (IsCheckedCopyDescription == false && SelectedLanguage != null)
                {
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
                    {
                        Description_zh = Description;
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetDescriptionToLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetDescriptionToLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()...", category: Category.Info, priority: Priority.Low);


                if (IsCheckedCopyDescription == false && SelectedLanguage != null)
                {
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


                if (IsCheckedCopyDescription == false)
                {
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                        Description = Description_en;
                        Name = Name_en;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        Description = Description_es;
                        Name = Name_es;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                        Description = Description_fr;
                        Name = Name_fr;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                        Description = Description_pt;
                        Name = Name_pt;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                        Description = Description_ro;
                        Name = Name_ro;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                        Description = Description_ru;
                        Name = Name_ru;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
                    {
                        Description = Description_zh;
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

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                UpdateCustomPropertyDetails = new CustomProperty();

                if (IsCheckedCopyDescription == true)
                {
                    UpdateCustomPropertyDetails.Name = Name;
                    UpdateCustomPropertyDetails.Name_es = Name;
                    UpdateCustomPropertyDetails.Name_fr = Name;
                    UpdateCustomPropertyDetails.Name_pt = Name;
                    UpdateCustomPropertyDetails.Name_ro = Name;
                    UpdateCustomPropertyDetails.Name_ru = Name;
                    UpdateCustomPropertyDetails.Name_zh = Name;

                    UpdateCustomPropertyDetails.Description = Description;
                    UpdateCustomPropertyDetails.Description_es = Description;
                    UpdateCustomPropertyDetails.Description_fr = Description;
                    UpdateCustomPropertyDetails.Description_pt = Description;
                    UpdateCustomPropertyDetails.Description_ro = Description;
                    UpdateCustomPropertyDetails.Description_ru = Description;
                    UpdateCustomPropertyDetails.Description_zh = Description;
                }
                else
                {
                    UpdateCustomPropertyDetails.Name = Name;
                    UpdateCustomPropertyDetails.Name_es = Name;
                    UpdateCustomPropertyDetails.Name_fr = Name;
                    UpdateCustomPropertyDetails.Name_pt = Name;
                    UpdateCustomPropertyDetails.Name_ro = Name;
                    UpdateCustomPropertyDetails.Name_ru = Name;
                    UpdateCustomPropertyDetails.Name_zh = Name;

                    UpdateCustomPropertyDetails.Description = Description;
                    UpdateCustomPropertyDetails.Description_es = Description;
                    UpdateCustomPropertyDetails.Description_fr = Description;
                    UpdateCustomPropertyDetails.Description_pt = Description;
                    UpdateCustomPropertyDetails.Description_ro = Description;
                    UpdateCustomPropertyDetails.Description_ru = Description;
                    UpdateCustomPropertyDetails.Description_zh = Description;
                }
                UpdateCustomPropertyDetails.IdCustomConnectorProperty = IdCustomConnectorProperty;
                UpdateCustomPropertyDetails.ValueType = SelectedValueType;
                UpdateCustomPropertyDetails.ValueKey = SelectedValueKey;



                IsSave = SCMService.UpdateEditCustomProperty(UpdateCustomPropertyDetails);

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

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

        private void AddNewValueKeyButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewValueKeyButtonCommandAction().", category: Category.Info, priority: Priority.Low);
                AddNewValueKeyView addNewValueKeyView = new AddNewValueKeyView();
                AddNewValueKeyViewModel addNewValueKeyViewModel = new AddNewValueKeyViewModel();
                EventHandler handle = delegate { addNewValueKeyView.Close(); };
                addNewValueKeyViewModel.RequestClose += handle;
                addNewValueKeyView.DataContext = addNewValueKeyViewModel;
                addNewValueKeyView.ShowDialogWindow();

                if (addNewValueKeyViewModel.IsSave)
                {
                    Data.Common.SCM.ValueKey NewItem = new Data.Common.SCM.ValueKey();
                    NewItem.IdLookupValue = addNewValueKeyViewModel.NewValueKeyList.IdLookupValue;
                    NewItem.Name = addNewValueKeyViewModel.NewValueKeyList.Name;
                    ValueKeyList.Add(NewItem);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewValueKeyButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddNewValueKeyButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditValueKeyButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueKeyButtonCommandAction().", category: Category.Info, priority: Priority.Low);
                ValueKey valueKey = SelectedValueKey;
                EditValueKeyView editValueKeyView = new EditValueKeyView();
                EditValueKeyViewModel editValueKeyViewModel = new EditValueKeyViewModel();
                EventHandler handle = delegate { editValueKeyView.Close(); };
                editValueKeyViewModel.RequestClose += handle;
                editValueKeyViewModel.Init(valueKey);
                editValueKeyView.DataContext = editValueKeyViewModel;
                editValueKeyView.ShowDialogWindow();


                if (editValueKeyViewModel.IsSave)
                {
                    int indexToUpdate = ValueKeyList.FindIndex(item => item.IdLookupValue == editValueKeyViewModel.NewValueKeyList.IdLookupValue);
                    if (indexToUpdate >= 0)
                    {
                        Data.Common.SCM.ValueKey existingItem = ValueKeyList[indexToUpdate];
                        existingItem.Name = editValueKeyViewModel.NewValueKeyList.Name;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditValueKeyButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditValueKeyButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
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
                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);

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
