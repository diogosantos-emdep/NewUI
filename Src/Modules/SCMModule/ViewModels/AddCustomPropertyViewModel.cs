using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4502][06/07/2023]
    public class AddCustomPropertyViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISCMService SCMService = new SCMServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
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
        ObservableCollection<Language> languages;//[Sudhir.Jangra][GEOS2-4502][18/07/2023]
        Language selectedLanguage;//[Sudhir.Jangra][GEOS2-4502][18/07/2023]
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
        CustomProperty newCustomPropertyDetails;
        CustomProperty updateCustomPropertyDetails;
        private double dialogHeight;
        private double dialogWidth;

        string windowHeader;

        ObservableCollection<Data.Common.SCM.ValueType> valueType;//[Sudhir.Jangra][GEOS2-4502]
        Data.Common.SCM.ValueType selectedValueType;//[Sudhir.Jangra][GEOS2-4502]

        ObservableCollection<ValueKey> valueKey;//[Sudhir.Jangra][GEOS2-4504]
        ValueKey selectedValueKey;//[Sudhir.Jangra][GEOS2-4504]
        bool isSave;
        bool isValueKeyEnabled;//[Sudhir.Jangra]
        bool isNew;
        private Int32 idCustomConnectorProperty;
        private string informationError;
        private ConnectorProperties selectedConnectorProperty;
        /// <summary>
        /// 
        /// </summary>
        private List<GeosSettings> geosSettings = new List<GeosSettings>();
        private string valueListFilterString;
        private ObservableCollection<ConnectorProperties> listConnectorProperties;
        List<string> ConnectorNameList = new List<string>();
        private bool isDeleted;
        public string TempCustomName=string.Empty;
        #endregion

        #region Properties
        public List<GeosSettings> GeosSettings
        {
            get { return geosSettings; }
            set
            {
                geosSettings = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosSettings"));
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
                UncheckedCopyDescription(null);
            }
        }

        public CustomProperty NewCustomPropertyDetails
        {
            get { return newCustomPropertyDetails; }
            set
            {
                newCustomPropertyDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewCustomPropertyDetails"));
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

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public ObservableCollection<Data.Common.SCM.ValueType> ValueType
        {
            get { return valueType; }
            set
            {
                valueType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueType"));
            }
        }
        public Data.Common.SCM.ValueType SelectedValueType
        {
            get { return selectedValueType; }
            set
            {
                selectedValueType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedValueType"));
                LoadValueKeyAndList();
            }
        }
        public ObservableCollection<ValueKey> ValueKey
        {
            get { return valueKey; }
            set
            {
                valueKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueKey"));
            }
        }
        public ValueKey SelectedValueKey
        {
            get { return selectedValueKey; }
            set
            {
                selectedValueKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedValueKey"));
                ValueListFilterString = string.Empty;
                GetLookupValuelistByIDLookupkey(SelectedValueKey);
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
        public bool IsValueKeyEnabled
        {
            get { return isValueKeyEnabled; }
            set
            {
                isValueKeyEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsValueKeyEnabled"));
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
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public ConnectorProperties SelectedConnectorProperty
        {
            get { return selectedConnectorProperty; }
            set
            {
                selectedConnectorProperty = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorProperty"));
            }
        }
        private ObservableCollection<LookUpValues> lookupValueList;
        public ObservableCollection<LookUpValues> LookupValueList
        {
            get { return lookupValueList; }
            set
            {
                lookupValueList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LookupValueList"));
            }
        }
        private LookUpValues selectedLookupValue;
        public LookUpValues SelectedLookupValue
        {
            get { return selectedLookupValue; }
            set
            {
                selectedLookupValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLookupValue"));
            }
        }
        public string ValueListFilterString
        {
            get { return valueListFilterString; }
            set
            {
                valueListFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueListFilterString"));
            }
        }
        public ObservableCollection<ConnectorProperties> ListConnectorProperties
        {
            get { return listConnectorProperties; }
            set { listConnectorProperties = value; OnPropertyChanged(new PropertyChangedEventArgs("ListConnectorProperties")); }
        }
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand AddCustomPropertyViewCancelButtonCommand { get; set; }

        public ICommand AddNewValueKeyButtonCommand { get; set; }

        public ICommand AddListValueButtonCommand { get; set; }

        public ICommand EditValueKeyButtonCommand { get; set; }
        public ICommand UncheckedCopyDescriptionCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeProductTypeDescriptionCommand { get; set; }
        public ICommand ChangeProductTypeNameCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand LookupValueHyperlinkClickCommand { get; set; }
        public ICommand DeleteValueListCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public AddCustomPropertyViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddCustomPropertyViewModel ...", category: Category.Info, priority: Priority.Low);

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
                AddCustomPropertyViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddNewValueKeyButtonCommand = new RelayCommand(new Action<object>(AddNewValueKeyButtonCommandAction));
                AddListValueButtonCommand = new RelayCommand(new Action<object>(AddListValueButtonCommandAction));
                EditValueKeyButtonCommand = new RelayCommand(new Action<object>(EditValueKeyButtonCommandAction));
                UncheckedCopyDescriptionCommand = new DelegateCommand<object>(UncheckedCopyDescription);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                ChangeProductTypeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeProductTypeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                LookupValueHyperlinkClickCommand = new RelayCommand(new Action<object>(LookupValueHyperlinkClickCommandAction));
                DeleteValueListCommand = new RelayCommand(new Action<object>(DeleteValueListCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
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
        private void UncheckedCopyDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()...", category: Category.Info, priority: Priority.Low);

                if (SelectedLanguage != null)
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

                GeosApplication.Instance.Logger.Log("Method UncheckedCopyDescription()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UncheckedCopyDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(ConnectorProperties ConnectorProperty, ObservableCollection<ConnectorProperties> TempListConnectorProperties)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ListConnectorProperties = new ObservableCollection<ConnectorProperties>();
                ListConnectorProperties= TempListConnectorProperties;
                SelectedConnectorProperty = new ConnectorProperties();
                SelectedConnectorProperty = ConnectorProperty;
                GetGeosSettings();
                AddLanguage();
                GetValueType();
                GetValueKey();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        public void EditInit(ConnectorProperties ConnectorProperty, ObservableCollection<ConnectorProperties> TempListConnectorProperties)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GetGeosSettings();
                ListConnectorProperties = new ObservableCollection<ConnectorProperties>();
                ListConnectorProperties = TempListConnectorProperties;
                SelectedConnectorProperty = new ConnectorProperties();
                SelectedConnectorProperty = ConnectorProperty;
                IdCustomConnectorProperty = SelectedConnectorProperty.IdConnectorProperty;
                var temp = SCMService.GetEditCustomProperty(IdCustomConnectorProperty);
                if (temp != null)
                {
                    TempCustomName = Convert.ToString(temp.Name);
                    Name = Convert.ToString(temp.Name);
                    Name_en = Convert.ToString(temp.Name);
                    Name_es = Convert.ToString(temp.Name_es);
                    Name_fr = Convert.ToString(temp.Name_fr);
                    Name_pt = Convert.ToString(temp.Name_pt);
                    Name_ro = Convert.ToString(temp.Name_ro);
                    Name_ru = Convert.ToString(temp.Name_ru);
                    Name_zh = Convert.ToString(temp.Name_zh);

                    Description = Convert.ToString(temp.Description);
                    Description_en = Convert.ToString(temp.Description);
                    Description_es = Convert.ToString(temp.Description_es);
                    Description_fr = Convert.ToString(temp.Description_fr);
                    Description_pt = Convert.ToString(temp.Description_pt);
                    Description_ro = Convert.ToString(temp.Description_ro);
                    Description_ru = Convert.ToString(temp.Description_ru);
                    Description_zh = Convert.ToString(temp.Description_zh);


                    ValueType = new ObservableCollection<Data.Common.SCM.ValueType>(SCMService.GetValueType());
                    //ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKey());
                    if (GeosSettings.Count > 0)
                    {
                        string GeosSettingValue = GeosSettings.FirstOrDefault().SettingValue;
                        ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKey(GeosSettingValue));
                    }

                    SelectedValueType = ValueType.FirstOrDefault(x => x.IdLookupValue == temp.IdConnectorType);

                    SelectedValueKey = ValueKey.FirstOrDefault(x => x.IdLookupKey == temp.IdLookupKey);
                }
                AddLanguage();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        private void GetValueType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetValueType()...", category: Category.Info, priority: Priority.Low);
                if (GeosSettings.Count > 0)
                {

                    ValueType = new ObservableCollection<Data.Common.SCM.ValueType>(SCMService.GetValueType());
                    SelectedValueType = ValueType.FirstOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method GetValueType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetValueType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetValueType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetValueType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetValueKey()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetValueKey()...", category: Category.Info, priority: Priority.Low);
                string GeosSettingValue = GeosSettings.FirstOrDefault().SettingValue;
                ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKey(GeosSettingValue));

                SelectedValueKey = ValueKey.FirstOrDefault();
                //GetLookupValuelistByIDLookupkey(SelectedValueKey);
                GeosApplication.Instance.Logger.Log("Method GetValueKey()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetValueKey() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetValueKey() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetValueKey() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
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
                addNewValueKeyViewModel.IsNew = true;
                String GeosSettingsKeyString = GeosSettings.FirstOrDefault().SettingValue;
                addNewValueKeyViewModel.Init(GeosSettingsKeyString);
                addNewValueKeyView.DataContext = addNewValueKeyViewModel;

                addNewValueKeyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMAddNewValueKeyTitle").ToString();
                addNewValueKeyView.ShowDialogWindow();

                if (addNewValueKeyViewModel.IsSave)
                {
                    Data.Common.SCM.ValueKey NewItem = new Data.Common.SCM.ValueKey();
                    NewItem.IdLookupKey = addNewValueKeyViewModel.NewValueKeyList.IdLookupKey;
                    NewItem.LookupKeyName = addNewValueKeyViewModel.NewValueKeyList.LookupKeyName;
                    ValueKey.Add(NewItem);
                    GetGeosSettings();
                    GetValueKey();
                }
                GeosApplication.Instance.Logger.Log("Method AddNewValueKeyButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddNewValueKeyButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddListValueButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddListValueButtonCommandAction().", category: Category.Info, priority: Priority.Low);
                AddListValueView addListValueView = new AddListValueView();
                AddListValueViewModel addListValueViewModel = new AddListValueViewModel();
                EventHandler handle = delegate { addListValueView.Close(); };
                addListValueViewModel.RequestClose += handle;
                addListValueViewModel.IsNew = true;
                addListValueViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMAddListValueTitle").ToString();
                addListValueView.DataContext = addListValueViewModel;
                addListValueViewModel.Init(SelectedValueKey);

                addListValueView.ShowDialogWindow();
                GetLookupValuelistByIDLookupkey(SelectedValueKey);
                GeosApplication.Instance.Logger.Log("Method AddListValueButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddListValueButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditValueKeyButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueKeyButtonCommandAction().", category: Category.Info, priority: Priority.Low);
                ValueKey valueKey = SelectedValueKey;
                AddNewValueKeyView editValueKeyView = new AddNewValueKeyView();
                AddNewValueKeyViewModel editValueKeyViewModel = new AddNewValueKeyViewModel();
                //  EditValueKeyView editValueKeyView = new EditValueKeyView();
                //   EditValueKeyViewModel editValueKeyViewModel = new EditValueKeyViewModel();
                EventHandler handle = delegate { editValueKeyView.Close(); };
                editValueKeyViewModel.RequestClose += handle;
                editValueKeyViewModel.IsNew = false;
                editValueKeyViewModel.EditInit(valueKey);
                editValueKeyView.DataContext = editValueKeyViewModel;
                editValueKeyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMUpdateValueKeyTitle").ToString();

                editValueKeyView.ShowDialogWindow();


                if (editValueKeyViewModel.IsSave)
                {
                    int indexToUpdate = ValueKey.FindIndex(item => item.IdLookupKey == editValueKeyViewModel.UpdateValueKeyList.IdLookupKey);
                    if (indexToUpdate >= 0)
                    {
                        Data.Common.SCM.ValueKey existingItem = ValueKey[indexToUpdate];
                        existingItem.LookupKeyName = editValueKeyViewModel.UpdateValueKeyList.LookupKeyName;
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

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if(ListConnectorProperties.Count>0)
                {
                   
                    ConnectorNameList = ListConnectorProperties.Where(x => x.IdConnectorCategory == 1815 && x.PropertyName!= TempCustomName).Select(a => a.PropertyName).ToList();
                }
              
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
                    NewCustomPropertyDetails = new CustomProperty();

                    if (IsCheckedCopyDescription == true)
                    {
                        TempCustomName = Name;
                        NewCustomPropertyDetails.Name = Name;
                        NewCustomPropertyDetails.Name_es = Name;
                        NewCustomPropertyDetails.Name_fr = Name;
                        NewCustomPropertyDetails.Name_pt = Name;
                        NewCustomPropertyDetails.Name_ro = Name;
                        NewCustomPropertyDetails.Name_ru = Name;
                        NewCustomPropertyDetails.Name_zh = Name;

                        NewCustomPropertyDetails.Description = Description;
                        NewCustomPropertyDetails.Description_es = Description;
                        NewCustomPropertyDetails.Description_fr = Description;
                        NewCustomPropertyDetails.Description_pt = Description;
                        NewCustomPropertyDetails.Description_ro = Description;
                        NewCustomPropertyDetails.Description_ru = Description;
                        NewCustomPropertyDetails.Description_zh = Description;
                    }
                    else
                    {
                        TempCustomName = Name_en;
                        NewCustomPropertyDetails.Name = Name_en;
                        NewCustomPropertyDetails.Name_es = Name_es;
                        NewCustomPropertyDetails.Name_fr = Name_fr;
                        NewCustomPropertyDetails.Name_pt = Name_pt;
                        NewCustomPropertyDetails.Name_ro = Name_ro;
                        NewCustomPropertyDetails.Name_ru = Name_ru;
                        NewCustomPropertyDetails.Name_zh = Name_zh;

                        NewCustomPropertyDetails.Description = Description_en;
                        NewCustomPropertyDetails.Description_es = Description_es;
                        NewCustomPropertyDetails.Description_fr = Description_fr;
                        NewCustomPropertyDetails.Description_pt = Description_pt;
                        NewCustomPropertyDetails.Description_ro = Description_ro;
                        NewCustomPropertyDetails.Description_ru = Description_ru;
                        NewCustomPropertyDetails.Description_zh = Description_zh;
                    }
                    //NewCustomPropertyDetails.ValueType = new Data.Common.SCM.ValueType();
                    //NewCustomPropertyDetails.ValueType.IdLookupValue = SelectedValueType.IdLookupValue;

                    NewCustomPropertyDetails.IdConnectorType = SelectedValueType.IdLookupValue;
                    //if (SelectedValueType.Name == "CUSTOM")
                    //{
                    // NewCustomPropertyDetails.ValueKey = new Data.Common.SCM.ValueKey();
                    if (SelectedValueKey == null || SelectedValueType.Name.ToUpper() != "LIST") // [GEOS2-4962][Rupali Sarode][14-12-2023]
                    {
                        NewCustomPropertyDetails.IdLookupKey = 0;
                    }
                    else
                    { 
                        NewCustomPropertyDetails.IdLookupKey = SelectedValueKey.IdLookupKey;
                    }
                    //}
                    if (SelectedConnectorProperty.IdConnectorCategory == 0)
                    {
                        NewCustomPropertyDetails.ValueKey = new Data.Common.SCM.ValueKey();
                        NewCustomPropertyDetails.IdConnectorCategory = SelectedConnectorProperty.IdConnectorProperty;

                        IsSave = SCMService.AddCustomProperty(NewCustomPropertyDetails);
                        
                        if (IsSave == true)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }

                }
                else
                {
                    UpdateCustomPropertyDetails = new CustomProperty();

                    if (IsCheckedCopyDescription == true)
                    {
                        TempCustomName = Name;
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
                        TempCustomName = Name_en;
                        UpdateCustomPropertyDetails.Name = Name_en;
                        UpdateCustomPropertyDetails.Name_es = Name_es;
                        UpdateCustomPropertyDetails.Name_fr = Name_fr;
                        UpdateCustomPropertyDetails.Name_pt = Name_pt;
                        UpdateCustomPropertyDetails.Name_ro = Name_ro;
                        UpdateCustomPropertyDetails.Name_ru = Name_ru;
                        UpdateCustomPropertyDetails.Name_zh = Name_zh;

                        UpdateCustomPropertyDetails.Description = Description_en;
                        UpdateCustomPropertyDetails.Description_es = Description_es;
                        UpdateCustomPropertyDetails.Description_fr = Description_fr;
                        UpdateCustomPropertyDetails.Description_pt = Description_pt;
                        UpdateCustomPropertyDetails.Description_ro = Description_ro;
                        UpdateCustomPropertyDetails.Description_ru = Description_ru;
                        UpdateCustomPropertyDetails.Description_zh = Description_zh;
                    }
                    UpdateCustomPropertyDetails.IdCustomConnectorProperty = IdCustomConnectorProperty;
                    //UpdateCustomPropertyDetails.ValueType = new Data.Common.SCM.ValueType();
                    UpdateCustomPropertyDetails.IdConnectorType = SelectedValueType.IdLookupValue;

                    //if (SelectedValueType.Name == "CUSTOM")
                    //{
                    // UpdateCustomPropertyDetails.ValueKey = new Data.Common.SCM.ValueKey();
                    if (SelectedValueKey == null || SelectedValueType.Name.ToUpper() != "LIST") // [GEOS2-4962][Rupali Sarode][14-12-2023]
                    {
                        UpdateCustomPropertyDetails.IdLookupKey = 0;
                    }
                    else
                    { 
                        UpdateCustomPropertyDetails.IdLookupKey = SelectedValueKey.IdLookupKey;
                    }
                    //}
                    if (SelectedConnectorProperty.CategoryName == "CUSTOM")
                    {
                        UpdateCustomPropertyDetails.IdConnectorCategory = SelectedConnectorProperty.IdConnectorCategory;
                        UpdateCustomPropertyDetails.IdCustomConnectorProperty = SelectedConnectorProperty.IdConnectorProperty;
                        IsSave = SCMService.UpdateEditCustomProperty(UpdateCustomPropertyDetails);
                        if (IsSave == true)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
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

        private void LoadValueKeyAndList()
        {
            if (SelectedValueType.Name != "List")
            {
                IsValueKeyEnabled = false;
                ValueKey = new ObservableCollection<Data.Common.SCM.ValueKey>();
            }
            else
            {
                if (GeosSettings.Count > 0)
                {


                    string GeosSettingValue = GeosSettings.FirstOrDefault().SettingValue;
                    IsValueKeyEnabled = true;
                    ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKey(GeosSettingValue));
                    SelectedValueKey = ValueKey.FirstOrDefault();
                }
            }
        }

        private void GetLookupValuelistByIDLookupkey(ValueKey SelectedValueKey)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetLookupValuelistByIDLookupkey()...", category: Category.Info, priority: Priority.Low);
                LookupValueList = new ObservableCollection<LookUpValues>();
                if (SelectedValueKey != null)
                {
                    LookupValueList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(SelectedValueKey.IdLookupKey));

                }

                // SelectedValueKey = ValueKey.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method GetLookupValuelistByIDLookupkey()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetLookupValuelistByIDLookupkey() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetLookupValuelistByIDLookupkey() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetLookupValuelistByIDLookupkey() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LookupValueHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
                //  Name = ListConnectorFamilies.Select(i => i.Name).ToList();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                LookUpValues SelectedRow = (LookUpValues)detailView.DataControl.CurrentItem;
                DateTime? tempEndDate;

                SelectedLookupValue = new LookUpValues();

                SelectedLookupValue = SelectedRow;


                //SelectedModuleEquivalentWeight = (ModulesEquivalencyWeight)obj;

                AddListValueView addListValueView = new AddListValueView();
                AddListValueViewModel addListValueViewModel = new AddListValueViewModel();
                
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addListValueView.DataContext = addListValueViewModel;
                EventHandler handle = delegate { addListValueView.Close(); };
                addListValueViewModel.RequestClose += handle;
                addListValueViewModel.IsNew = false;
                addListValueViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SCMUpdateListValueTitle").ToString();
                // addListValueViewModel.LblEquivalentWeight = "Module";
                addListValueViewModel.EditInit(SelectedLookupValue);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                addListValueView.Owner = Window.GetWindow(ownerInfo);

                addListValueView.ShowDialog();



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }



                if (addListValueViewModel.IsSave)
                {
                    GetLookupValuelistByIDLookupkey(SelectedValueKey);
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
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
                    return AddEditCustompropertyValidation.GetErrorMessage(name, ConnectorNameList,Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditCustompropertyValidation.GetErrorMessage(headerInformtionError,null,InformationError);
                }

                return null;
            }
        }


        private void GetGeosSettings()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetGeosSettings()...", category: Category.Info, priority: Priority.Low);

                GeosSettings = new List<Data.Common.GeosSettings>();
                GeosSettings = WorkbenchStartUp.GetSelectedGeosSettings_V2420("SCM_ConnectorList_LookupKeys");
                GeosApplication.Instance.Logger.Log("Method GetGeosSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetGeosSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetGeosSettings() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetGeosSettings() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteValueListCommandAction(object obj)
        {
            //DataRow dr1 = (DataRow)((System.Data.DataRowView)obj).Row;
            LookUpValues tempLookUpValues = new LookUpValues();
            tempLookUpValues = (LookUpValues)obj;
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteValueListCommandAction()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteValuelistRecord"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    //IsDeleted = SCMService.UpdateEditCustomProperty(UpdateCustomPropertyDetails);
                    IsDeleted = SCMService.IsDeletedValueList_V2420(Convert.ToInt32(tempLookUpValues.IdLookupValue));
                    if (IsDeleted)
                    {
                        GetLookupValuelistByIDLookupkey(SelectedValueKey);
                    }
                }
               
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteValueListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteValueListCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteValueListCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteValueListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
