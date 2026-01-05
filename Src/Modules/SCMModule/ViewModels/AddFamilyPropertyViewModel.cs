using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Modules.SCM.Common_Classes;
using System.Collections.Generic;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.Modules.SCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4563][27/07/2023]
    public class AddFamilyPropertyViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        LookUpValues selectedConnectorType;
        ObservableCollection<LookUpValues> connectorTypeList;
        string windowHeader;
        private ConnectorSubFamily selectedSubFamily;
        ObservableCollection<FamilyImage> familyImagesListOriginal;
        ObservableCollection<ConnectorSubFamily> subFamilyList;
        ObservableCollection<ConnectorSubFamily> subFamiliesList;
        private bool isSave;
        private List<string> connectorName;
        private bool isNew;
        private ObservableCollection<Language> languages;
        private Language languageSelected;
        private string description;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private string name;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;
        private string informationError;
        private bool isCheckedCopyNameAndDescription;
        private string error = string.Empty;
        public bool IsFromInformation = false;
        private bool isInUse;
        private ObservableCollection<FamilyImage> familyImagesList;
        ObservableCollection<FamilyImage> deletedFamilyImagesList;
        private FamilyImage selectedImage;
        Int32 idFamily;
        private bool isUpdate;
        private ConnectorFamily clonedFmailyiamgeType;
        private ObservableCollection<FamilyImage> imagesList;
        private bool isSCMEditFamiliesManager;//[pramod.misal][GEOS2-5482][24.05.2024]
        private bool isSCMEditFamiliesManagerBtn;
        private bool isSCMEditFamiliesMgrhyperlink;

        #endregion

        #region Properties

        //[pramod.misal][GEOS2-5482][24.05.2024]
        public bool IsSCMEditFamiliesManager
        {
            get { return isSCMEditFamiliesManager; }
            set
            {

                isSCMEditFamiliesManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFamiliesManager"));

            }
        }

        



        public bool IsSCMEditFamiliesManagerBtn
        {
            get { return isSCMEditFamiliesManagerBtn; }
            set
            {

                isSCMEditFamiliesManagerBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFamiliesManagerBtn"));

            }
        }

        public bool IsSCMEditFamiliesMgrhyperlink
        {
            get { return isSCMEditFamiliesMgrhyperlink; }
            set
            {

                isSCMEditFamiliesMgrhyperlink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFamiliesMgrhyperlink"));

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
        public ObservableCollection<LookUpValues> ConnectorTypeList
        {
            get
            {
                return connectorTypeList;
            }
            set
            {
                connectorTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorTypeList"));
            }
        }
        public LookUpValues SelectedConnectorType
        {
            get
            {
                return selectedConnectorType;
            }
            set
            {
                selectedConnectorType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorType"));
            }
        }
        public ObservableCollection<ConnectorSubFamily> SubFamiliesList
        {
            get { return subFamiliesList; }
            set
            {
                subFamiliesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubFamiliesList"));
            }
        }
        public ObservableCollection<ConnectorSubFamily> SubFamilyList
        {
            get { return subFamilyList; }
            set
            {
                subFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubFamilyList"));
            }
        }
        public FamilyImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        public ConnectorFamily NewConnectorFamilies { get; set; }
        public ConnectorFamily UpdatedConnectorFamilies { get; set; }
        public bool IsSave
        {
            get
            {
                return isSave;
            }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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
        public Language LanguageSelected
        {
            get
            {
                return languageSelected;
            }

            set
            {
                languageSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelected"));
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

        #region Description
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                //if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                //{
                //    IsReadOnlyName = false;
                //    IsCheckedCopyNameReadOnly = false;
                //    IsEnabledCopyNameReadOnly = true;

                //    if (!(string.IsNullOrEmpty(description)))
                //        InformationError = " ";

                //    else
                //        InformationError = null;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                //  }
                ////else
                ////{
                //    description = value;
                //    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                //    IsReadOnlyName = true;
                //    IsCheckedCopyNameReadOnly = false;
                //    IsEnabledCopyNameReadOnly = false;
                ////}
            }
        }
        public string Description_en
        {
            get
            {
                return description_en;
            }
            set
            {
                description_en = value;
                if (!string.IsNullOrEmpty(description_en))
                {
                    description_en = description_en.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
            }
        }
        public string Description_es
        {
            get
            {
                return description_es;
            }
            set
            {
                description_es = value;
                if (!string.IsNullOrEmpty(description_es))
                {
                    description_es = description_es.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
            }
        }
        public string Description_fr
        {
            get
            {
                return description_fr;
            }
            set
            {
                description_fr = value;
                if (!string.IsNullOrEmpty(description_fr))
                {
                    description_fr = description_fr.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            }
        }
        public string Description_pt
        {
            get
            {
                return description_pt;
            }
            set
            {
                description_pt = value;
                if (!string.IsNullOrEmpty(description_pt))
                {
                    description_pt = description_pt.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
            }
        }
        public string Description_ro
        {
            get
            {
                return description_ro;
            }
            set
            {
                description_ro = value;
                if (!string.IsNullOrEmpty(description_ro))
                {
                    description_ro = description_ro.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
            }
        }
        public string Description_ru
        {
            get
            {
                return description_ru;
            }
            set
            {
                description_ru = value;
                if (!string.IsNullOrEmpty(description_ru))
                {
                    description_ru = description_ru.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
            }
        }
        public string Description_zh
        {
            get
            {
                return description_zh;
            }
            set
            {
                description_zh = value;
                if (!string.IsNullOrEmpty(description_zh))
                {
                    description_zh = description_zh.Trim(' ', '\r');
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
            }
        }
        #endregion
        #region Name
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }
        public string Name_en
        {
            get { return name_en; }
            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
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
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
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
        #endregion
        public bool IsCheckedCopyNameAndDescription
        {
            get
            {
                return isCheckedCopyNameAndDescription;
            }

            set
            {
                isCheckedCopyNameAndDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameAndDescription"));
            }
        }
        public bool IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInUse"));
            }
        }
       
        public ObservableCollection<FamilyImage> FamilyImagesList
        {
            get { return familyImagesList; }
            set
            {
                familyImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyImagesList"));
            }
        }
        public ObservableCollection<FamilyImage> DeletedFamilyImagesList
        {
            get { return deletedFamilyImagesList; }
            set
            {
                deletedFamilyImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedFamilyImagesList"));
            }
        }        
        public List<string> ConnectorName
        {
            get { return connectorName; }
            set
            {
                connectorName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorName"));
            }
        }
        public Int32 IdFamily
        {
            get
            {
                return idFamily;
            }

            set
            {
                idFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdFamily"));
            }
        }
        public bool IsUpdate
        {
            get
            {
                return isUpdate;
            }
            set
            {
                isUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate"));
            }
        }
        public ConnectorFamily ClonedFmailyiamgeType
        {
            get
            {
                return clonedFmailyiamgeType;
            }
            set
            {
                clonedFmailyiamgeType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedFmailyiamgeType"));
            }
        }        
        public ConnectorSubFamily SelectedSubFamily
        {
            get { return selectedSubFamily; }
            set
            {
                selectedSubFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSubFamily"));
            }
        }
        #endregion

        #region Public ICommand

        public ICommand AddSubFamilyButtonCommand { get; set; }
        public ICommand AddFamilyPropertyViewCancelButtonCommand { get; set; }
        //[GEOS2-4563][rupali sarode][31-07-2023]
        public ICommand AddImageButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand CheckedCopyNameDescriptionCommand { get; set; }
        public ICommand LoadedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand EditImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }       
        public ICommand EditSubFamilyButtonCommand { get; set; }
        public ICommand UnCheckedCopyNameDescriptionCommand { get; set; }

        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public AddFamilyPropertyViewModel()
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
                AddFamilyPropertyViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddSubFamilyButtonCommand = new RelayCommand(new Action<object>(AddSubFamilyButtonCommandAction));
                EditSubFamilyButtonCommand = new DelegateCommand<object>(EditSubFamilyButtonCommandAction);
                UnCheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UnCheckedCopyNameDescription);
                #region [GEOS2-4563][rupali sarode][31-07-2023]
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                AddImageButtonCommand = new DelegateCommand<object>(AddImageButtonCommandAction);
                CheckedCopyNameDescriptionCommand = new DelegateCommand<object>(CheckedCopyNameDescription);
                LoadedCopyNameDescriptionCommand = new DelegateCommand<object>(LoadedCopyNameDescription);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeNameCommand = new DelegateCommand<EditValueChangingEventArgs>(SetNameToLanguage);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
                EditImageCommand = new DelegateCommand<object>(EditImageAction);
                DeleteImageCommand = new DelegateCommand<object>(DeleteImageAction);
                DeletedFamilyImagesList = new ObservableCollection<FamilyImage>();
                #endregion [GEOS2-4563][rupali sarode][31-07-2023]
                GetConnectorType();

                //[pramod.misal][GEOS2-5482][23.05.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFamiliesManager)
                {
                    IsSCMEditFamiliesManager = false;
                    IsSCMEditFamiliesManagerBtn = true;
                    IsSCMEditFamiliesMgrhyperlink = true;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    IsSCMEditFamiliesManager = true;
                    IsSCMEditFamiliesManagerBtn = false;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission && GeosApplication.Instance.IsSCMEditFamiliesManager)
                {
                    IsSCMEditFamiliesManager = false;
                    IsSCMEditFamiliesManagerBtn = true;
                    IsSCMEditFamiliesMgrhyperlink = true;
                }
                if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    IsSCMEditFamiliesMgrhyperlink = true;
                }



                //if ((!GeosApplication.Instance.IsSCMPermissionAdmin || !GeosApplication.Instance.IsSCMEditFamiliesManager) && GeosApplication.Instance.IsSCMViewConfigurationPermission)
                //{
                //    IsSCMEditViewFamiliesManager = true;
                //}
                //else
                //{
                //    IsSCMEditViewFamiliesManager = false;
                //}
                //if (GeosApplication.Instance.IsSCMEditFamiliesManager || GeosApplication.Instance.IsSCMPermissionAdmin)
                //{
                //    IsSCMEditViewFamiliesManager = false;
                //}
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

        #region Method
        private void UnCheckedCopyNameDescription(object obj)
        {
            System.Windows.RoutedEventArgs obj1 = (System.Windows.RoutedEventArgs)obj;
            if (obj1.OriginalSource is CheckEdit)
            {
                CheckEdit checkbox = (CheckEdit)obj1.OriginalSource;
                IsCheckedCopyNameAndDescription = false;
                checkbox.IsChecked = false;
            }
        }
        private void AddSubFamilyButtonCommandAction(object obj)
        {
            try
            {
                TableView detailView = (TableView)obj;
                GeosApplication.Instance.Logger.Log("Method AddSubFamilyButtonCommandAction().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddSubFamilyView addSubFamilyView = new AddSubFamilyView();
                AddSubFamilyViewModel addSubFamilyViewModel = new AddSubFamilyViewModel();
                addSubFamilyViewModel.WindowHeader = Application.Current.FindResource("SCMAddSubFamilyPropertyTitle").ToString();
                EventHandler handle = delegate { addSubFamilyView.Close(); };
                addSubFamilyViewModel.RequestClose += handle;
                addSubFamilyView.DataContext = addSubFamilyViewModel;
                addSubFamilyViewModel.Init(IsNew, IdFamily);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                addSubFamilyView.Owner = Window.GetWindow(ownerInfo);
                addSubFamilyView.ShowDialogWindow();

                if (addSubFamilyViewModel.IsSave)
                {
                    if (SubFamiliesList == null)
                        SubFamiliesList = new ObservableCollection<ConnectorSubFamily>();

                    uint maxIDSubfamily = 1;
                    if (SubFamiliesList?.Count > 0)
                    {
                        maxIDSubfamily = SubFamiliesList.Select(a => a.Id).Max();
                        maxIDSubfamily = maxIDSubfamily + 1;
                    }
                    addSubFamilyViewModel.NewSubFamilyDetails.Id = maxIDSubfamily;
                    addSubFamilyViewModel.NewSubFamilyDetails.IdFamily =(uint)IdFamily;
                    SubFamiliesList.Add(addSubFamilyViewModel.NewSubFamilyDetails);     
                }

                GeosApplication.Instance.Logger.Log("Method AddSubFamilyButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddSubFamilyButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [GEOS2-4563][rupali sarode][31-07-2023]
        public void Init(List<string> Name)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()", category: Category.Info, priority: Priority.Low);
                ConnectorName = Name;
                AddLanguages();        
                IsNew = true;
                IsInUse = true;
                IsCheckedCopyNameAndDescription = true;
                FamilyImagesList = new ObservableCollection<FamilyImage>();
                SubFamilyList = new ObservableCollection<ConnectorSubFamily>();
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }

        }
        private void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages..."), category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadedCopyNameDescription(object obj)
        {
            try
            {
                System.Windows.RoutedEventArgs obj1 = (System.Windows.RoutedEventArgs)obj;
                CheckEdit checkbox = (CheckEdit)obj1.OriginalSource;

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyNameAndDescription = true;
                    checkbox.IsChecked = true;
                }
                else
                {
                    IsCheckedCopyNameAndDescription = false;
                    checkbox.IsChecked = false;
                }               
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckedCopyNameDescription() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected != null)
                {
                    if (string.IsNullOrEmpty(Description))
                        Description = string.Empty;
                    if (string.IsNullOrEmpty(Name))
                        Name = string.Empty;

                    Description_en = Description;
                    Description_es = Description;
                    Description_fr = Description;
                    Description_pt = Description;
                    Description_ro = Description;
                    Description_ru = Description;
                    Description_zh = Description;

                    Name_en = Name;
                    Name_es = Name;
                    Name_fr = Name;
                    Name_pt = Name;
                    Name_ro = Name;
                    Name_ru = Name;
                    Name_zh = Name;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckedCopyNameDescription() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()..."), category: Category.Info, priority: Priority.Low);            
                if (IsCheckedCopyNameAndDescription == false && LanguageSelected != null)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description_zh = Description == null ? "" : Description;
                    }
                }
                else
                {
                    Description_en = Description;
                    Description_es = Description;
                    Description_fr = Description;
                    Description_pt = Description;
                    Description_ro = Description;
                    Description_ru = Description;
                    Description_zh = Description;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetNameToLanguage(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()..."), category: Category.Info, priority: Priority.Low);              
                if (IsCheckedCopyNameAndDescription == false && LanguageSelected != null)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Name_en = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Name_es = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Name_fr = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Name_pt = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Name_ro = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Name_ru = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Name_zh = Name;
                    }
                }
                else
                {
                    Name_en = Name;
                    Name_es = Name;
                    Name_fr = Name;
                    Name_pt = Name;
                    Name_ro = Name;
                    Name_ru = Name;
                    Name_zh = Name;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveNameDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameDescriptionByLanguge()..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameAndDescription == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description = Description_en;
                        Name = Name_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description = Description_es;
                        Name = Name_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description = Description_fr;
                        Name = Name_fr;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description = Description_pt;
                        Name = Name_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description = Description_ro;
                        Name = Name_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description = Description_ru;
                        Name = Name_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description = Description_zh;
                        Name = Name_zh;
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameDescriptionByLanguge()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveNameDescriptionByLanguge() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        private void EditImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);
                Emdep.Geos.UI.Helper.ImageContainer Obj1 = (Emdep.Geos.UI.Helper.ImageContainer)obj;
                AddFamilyImageView addFamilyImageView = new AddFamilyImageView();
                AddFamilyImageViewModel addFamilyImageViewModel = new AddFamilyImageViewModel();
                EventHandler handle = delegate { addFamilyImageView.Close(); };
                addFamilyImageViewModel.RequestClose += handle;
                addFamilyImageViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditImageHeader").ToString();
                SelectedImage = (FamilyImage)Obj1.DataContext;
                addFamilyImageViewModel.EditInit(SelectedImage);

                addFamilyImageViewModel.FamilyImagesList = FamilyImagesList;               
                addFamilyImageView.DataContext = addFamilyImageViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addFamilyImageView.Owner = Window.GetWindow(ownerInfo);
                addFamilyImageView.ShowDialog();

                if (addFamilyImageViewModel.IsSave == true)
                {
                    FamilyImage UpdatedImage = FamilyImagesList.FirstOrDefault(i => i.IdSCMFamilyImage == addFamilyImageViewModel.IdImage);
                    UpdatedImage.IdSCMFamilyImage = addFamilyImageViewModel.IdImage;
                    UpdatedImage.OriginalFileName = addFamilyImageViewModel.ImageName;
                    UpdatedImage.Description = addFamilyImageViewModel.Description;
                    UpdatedImage.ConnectorFamilyImageInBytes = addFamilyImageViewModel.FileInBytes;
                    UpdatedImage.SavedFileName = addFamilyImageViewModel.ConnectorFamilySavedImageName;
                    UpdatedImage.Position = addFamilyImageViewModel.SelectedImage.Position;
                    UpdatedImage.CreatedBy = addFamilyImageViewModel.SelectedImage.CreatedBy;
                    UpdatedImage.ModifiedBy = addFamilyImageViewModel.SelectedImage.ModifiedBy;
                    UpdatedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    UpdatedImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                    SelectedImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(UpdatedImage.ConnectorFamilyImageInBytes);                                
                    FamilyImagesList = new ObservableCollection<FamilyImage>(FamilyImagesList.OrderBy(a => a.Position));
                }
                GeosApplication.Instance.Logger.Log("Method EditImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteImageAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteImageMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                Emdep.Geos.UI.Helper.ImageContainer Obj1 = (Emdep.Geos.UI.Helper.ImageContainer)obj;
                SelectedImage = (FamilyImage)Obj1.DataContext;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    FamilyImage DeletedImage = new FamilyImage();
                    DeletedImage = (FamilyImage)Obj1.DataContext;

                    if (FamilyImagesList?.Count > 0)
                        FamilyImagesList.Remove(DeletedImage);
                    //[rdixit][05.03.2025][GEOS2-7026] 
                    if (DeletedFamilyImagesList != null)
                    {
                        DeletedImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        DeletedFamilyImagesList.Add(DeletedImage);
                    }
                    if (FamilyImagesList.Count > 0)
                    {
                        if (!(FamilyImagesList.Any(i => i.Position == 1)))
                        {
                            FamilyImagesList.FirstOrDefault().Position = 1;
                            if (FamilyImagesList.FirstOrDefault().TransactionOperation != ModelBase.TransactionOperations.Add && FamilyImagesList.FirstOrDefault().TransactionOperation != ModelBase.TransactionOperations.Update)
                            {
                                FamilyImagesList.FirstOrDefault().TransactionOperation = ModelBase.TransactionOperations.Update;
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteImageAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        void getFamilyImages(ConnectorFamily currentFamily)
        {
            try
            {
                //[rdixit][01.09.2023][GEOS2-4565]
                if (FamilyImagesList == null)
                    FamilyImagesList = new ObservableCollection<FamilyImage>();
                 //[rdixit][05.03.2025][GEOS2-7026] 
                //Service GetFamilyImageImagesByIdFamily_V2430 updated with GetFamilyImageImagesByIdFamily_V2450 by [rdixit][19.10.2023][GEOS2-4958]
                currentFamily.FamilyImagesList = new List<FamilyImage>(SCMService.GetFamilyImagesByIdFamily_V2620(currentFamily.IdFamily));

                if (currentFamily.FamilyImagesList != null)
                {
                    foreach (var item in currentFamily.FamilyImagesList)
                    {
                        item.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(item.ConnectorFamilyImageInBytes);
                    }
                    FamilyImagesList = new ObservableCollection<FamilyImage>(currentFamily.FamilyImagesList);
                    if (FamilyImagesList.Count > 0)
                    {
                        if (!(FamilyImagesList.Any(i => i.Position == 1)))
                        {
                            FamilyImagesList.FirstOrDefault().Position = 1;
                            FamilyImagesList.FirstOrDefault().TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                    }
                }              
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in getFamilyImages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in getFamilyImages() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method getFamilyImages()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void getSubFamilies(ConnectorFamily currentFamily)
        {
            try
            {
                //Service GetSubFamily updated with GetSubFamily_V2430 [rdixit][01.09.2023][GEOS2 - 4565]
                //Service GetSubFamily_V2430 updated with GetSubFamily_V2450 [rdixit][19.10.2023][GEOS2-4958]
                //[rdixit][05.03.2025][GEOS2-7026] 
                SubFamiliesList = new ObservableCollection<ConnectorSubFamily>(SCMService.GetSubFamiliesListByIdFamily_V2620(IdFamily));
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in getSubFamilies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in getSubFamilies() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method getSubFamilies()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                if (IsNew)
                {
                    #region Add Family
                    InformationError = null;
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedConnectorType"));
                    if (string.IsNullOrEmpty(error))
                        InformationError = null;
                    else
                        InformationError = " ";
                    if (error != null)
                    {
                        return;
                    }
                    NewConnectorFamilies = new ConnectorFamily();
                    #region
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        NewConnectorFamilies.Name = Name;
                        NewConnectorFamilies.Name_es = Name;
                        NewConnectorFamilies.Name_fr = Name;
                        NewConnectorFamilies.Name_pt = Name;
                        NewConnectorFamilies.Name_ro = Name;
                        NewConnectorFamilies.Name_ru = Name;
                        NewConnectorFamilies.Name_zh = Name;
                        NewConnectorFamilies.Description = Description;
                        NewConnectorFamilies.Description_es = Description;
                        NewConnectorFamilies.Description_fr = Description;
                        NewConnectorFamilies.Description_pt = Description;
                        NewConnectorFamilies.Description_ro = Description;
                        NewConnectorFamilies.Description_ru = Description;
                        NewConnectorFamilies.Description_zh = Description;
                    }
                    else
                    {
                        NewConnectorFamilies.Name = Name_en;
                        NewConnectorFamilies.Name_es = Name_es;
                        NewConnectorFamilies.Name_fr = Name_fr;
                        NewConnectorFamilies.Name_pt = Name_pt;
                        NewConnectorFamilies.Name_ro = Name_ro;
                        NewConnectorFamilies.Name_ru = Name_ru;
                        NewConnectorFamilies.Name_zh = Name_zh;

                        NewConnectorFamilies.Description = Description_en;
                        NewConnectorFamilies.Description_es = Description_es;
                        NewConnectorFamilies.Description_fr = Description_fr;
                        NewConnectorFamilies.Description_pt = Description_pt;
                        NewConnectorFamilies.Description_ro = Description_ro;
                        NewConnectorFamilies.Description_ru = Description_ru;
                        NewConnectorFamilies.Description_zh = Description_zh;
                    }
                    #endregion
                    if (IsInUse == true)
                        NewConnectorFamilies.IsInUse = "1";
                    else
                        NewConnectorFamilies.IsInUse = "0";
                    NewConnectorFamilies.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    //[rdixit][25.01.2024][GEOS2-5148]
                    NewConnectorFamilies.ConnectorType = new LookUpValues();
                    NewConnectorFamilies.ConnectorType.IdLookupValue = SelectedConnectorType.IdLookupValue;
                    NewConnectorFamilies.ConnectorType.Value_en = SelectedConnectorType.Value_en;
                    NewConnectorFamilies.FamilyImagesList = FamilyImagesList.ToList();
                    NewConnectorFamilies.FamilyImagesList.ForEach(i => i.AttachmentImage = null);
                    NewConnectorFamilies.SubFamilyList = new List<ConnectorSubFamily>();
                    if (SubFamiliesList != null)
                    {
                        NewConnectorFamilies.SubFamilyList = SubFamiliesList.ToList();
                        NewConnectorFamilies.SubFamilyList.ForEach(a => { a.FamilyName = Name_es; a.ImageList.ToList().ForEach(x => x.AttachmentImage = null); });
                    }
                    //Service AddConnectorFamilies_V2420 updated with AddConnectorFamilies_V2430 [rdixit][01.09.2023][GEOS2-4565]
                    //Service AddConnectorFamilies_V2430 updated with AddConnectorFamilies_V2450 [rdixit][01.09.2023][GEOS2-4958]
                    //Service AddConnectorFamilies_V2450 updated with AddConnectorFamilies_V2480 [rdixit][25.01.2024][GEOS2-5148]
                    //Service AddConnectorFamilies_V2480 updated with AddConnectorFamilies_V2620 [rdixit][05.03.2025][GEOS2-7026] 
                    NewConnectorFamilies = SCMService.AddConnectorFamilies_V2620(NewConnectorFamilies);
                    NewConnectorFamilies.SubFamilyList.ToList().ForEach(i => i.IdFamily = Convert.ToUInt32(NewConnectorFamilies.IdFamily));
                    IsSave = true;
                    if (IsSave)
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("ConnectorFamilyAddSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    RequestClose(null, null);
                    #endregion
                }
                else
                {
                    InformationError = null;
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedConnectorType"));
                    if (string.IsNullOrEmpty(error))
                        InformationError = null;
                    else
                        InformationError = " ";
                    if (error != null)
                    {
                        return;
                    }
                    UpdatedConnectorFamilies = new ConnectorFamily();
                    UpdatedConnectorFamilies.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UpdatedConnectorFamilies.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UpdatedConnectorFamilies.IdFamily = IdFamily;
                    UpdatedConnectorFamilies.ConnectorType = new LookUpValues();       //[rdixit][25.01.2024][GEOS2-5148]
                    UpdatedConnectorFamilies.ConnectorType.IdLookupValue = SelectedConnectorType.IdLookupValue;
                    UpdatedConnectorFamilies.ConnectorType.Value_en = SelectedConnectorType.Value_en;
                    if (IsInUse == true)
                        UpdatedConnectorFamilies.IsInUse = "1";
                    else
                        UpdatedConnectorFamilies.IsInUse = "0";
                    #region Name & Description
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        IsFromInformation = true;
                        UpdatedConnectorFamilies.Description = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_es = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_fr = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_pt = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_ro = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_ru = Description == null ? "" : Description.Trim();
                        UpdatedConnectorFamilies.Description_zh = Description == null ? "" : Description.Trim();

                        UpdatedConnectorFamilies.Name = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_es = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_fr = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_pt = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_ro = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_ru = Name == null ? "" : Name.Trim();
                        UpdatedConnectorFamilies.Name_zh = Name == null ? "" : Name.Trim();
                    }
                    else
                    {
                        IsFromInformation = true;
                        UpdatedConnectorFamilies.Description = Description_en == null ? "" : Description_en.Trim();
                        UpdatedConnectorFamilies.Description_es = Description_es == null ? "" : Description_es.Trim();
                        UpdatedConnectorFamilies.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                        UpdatedConnectorFamilies.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                        UpdatedConnectorFamilies.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                        UpdatedConnectorFamilies.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                        UpdatedConnectorFamilies.Description_zh = Description_zh == null ? "" : Description_zh.Trim();

                        UpdatedConnectorFamilies.Name = Name_en == null ? "" : Name_en.Trim();
                        UpdatedConnectorFamilies.Name_es = Name_es == null ? "" : Name_es.Trim();
                        UpdatedConnectorFamilies.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                        UpdatedConnectorFamilies.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                        UpdatedConnectorFamilies.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                        UpdatedConnectorFamilies.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                        UpdatedConnectorFamilies.Name_zh = Name_zh == null ? "" : Name_zh.Trim();
                    }
                    #endregion
                    //[rdixit][05.03.2025][GEOS2-7026] 
                    #region Family Images
                    //Code added when the family name is changed then the image path shoud also update in DB
                    UpdatedConnectorFamilies.OldName = ClonedFmailyiamgeType.Name_es;
                    if (ClonedFmailyiamgeType.Name_es.ToLower() != Name_es.ToLower())
                    {
                        if (FamilyImagesList.Count > 0)
                        {
                            if (SubFamiliesList != null)
                            {
                                SubFamiliesList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Update);
                            }
                        }
                    }
                    UpdatedConnectorFamilies.FamilyImagesList = new List<FamilyImage>();

                    foreach (FamilyImage item in FamilyImagesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete ||
                    i.TransactionOperation == ModelBase.TransactionOperations.Add || i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList())
                    {
                        item.AttachmentImage = null;
                        UpdatedConnectorFamilies.FamilyImagesList.Add((FamilyImage)item.Clone());
                    }
                    if (DeletedFamilyImagesList != null)
                    {
                        UpdatedConnectorFamilies.FamilyImagesList.AddRange(DeletedFamilyImagesList.Select(i => (FamilyImage)i.Clone()).ToList());
                    }
                    UpdatedConnectorFamilies.FamilyImagesList.ForEach(i => i.AttachmentImage = null);
                    #endregion
                    SubFamiliesList.ToList().ForEach(a => { a.ImageList.ToList().ForEach(x => x.AttachmentImage = null); });
                    UpdatedConnectorFamilies.SubFamilyList = SubFamiliesList.ToList();

                    //Service UpdateConnectorFamilies_V2420 updated with UpdateConnectorFamilies_V2430 [rdixit][01.09.2023][GEOS2-4565] 
                    //Service UpdateConnectorFamilies_V2430 updated with UpdateConnectorFamilies_V2450 [rdixit][01.09.2023][GEOS2-4958]   
                    //Service UpdateConnectorFamilies_V2450 updated with UpdateConnectorFamilies_V2480 [rdixit][25.01.2024][GEOS2-5148]
                    //Service UpdateConnectorFamilies_V2480 updated with UpdateConnectorFamilies_V2620 [rdixit][05.03.2025][GEOS2-7026]  
                    UpdatedConnectorFamilies = SCMService.UpdateConnectorFamilies_V2620(UpdatedConnectorFamilies);
                    IsUpdate = true;

                    if (IsUpdate == true)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ConnectorFamilyUpdateMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddImageButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddFamilyImageView addFamilyImageView = new AddFamilyImageView();
                AddFamilyImageViewModel addFamilyImageViewModel = new AddFamilyImageViewModel();
                EventHandler handle = delegate { addFamilyImageView.Close(); };
                addFamilyImageViewModel.RequestClose += handle;
                addFamilyImageView.DataContext = addFamilyImageViewModel;
                addFamilyImageViewModel.FamilyImagesList = FamilyImagesList;
                addFamilyImageViewModel.Init(IdFamily);              
                addFamilyImageView.ShowDialog();

                if (addFamilyImageViewModel.IsSave == true)
                {
                    FamilyImage newImage = new FamilyImage();
                    newImage = addFamilyImageViewModel.SelectedImage;
                    newImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(newImage.ConnectorFamilyImageInBytes);
                    newImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                    FamilyImagesList.Add(newImage);
                }

                GeosApplication.Instance.Logger.Log("Method AddImageButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditInit(ConnectorFamily SelectConnectorFamilies, List<string> name)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()", category: Category.Info, priority: Priority.Low);                
                ClonedFmailyiamgeType = (ConnectorFamily)SelectConnectorFamilies.Clone();
                SelectedImage = new FamilyImage();
                ConnectorName = name;
                IdFamily = SelectConnectorFamilies.IdFamily;           
                if (SelectConnectorFamilies.IsInUse == "Yes")
                    IsInUse = true;
                else
                    IsInUse = false;
                if (!IsNew)
                {
                    AddLanguages();
                    IdFamily = SelectConnectorFamilies.IdFamily;
                    Name = SelectConnectorFamilies.Name;
                    Name_en = SelectConnectorFamilies.Name;
                    Name_es = SelectConnectorFamilies.Name_es;
                    Name_fr = SelectConnectorFamilies.Name_fr;
                    Name_pt = SelectConnectorFamilies.Name_pt;
                    Name_ro = SelectConnectorFamilies.Name_ro;
                    Name_ru = SelectConnectorFamilies.Name_ru;
                    Name_zh = SelectConnectorFamilies.Name_zh;
                    Description = SelectConnectorFamilies.Description;
                    Description_en = SelectConnectorFamilies.Description;
                    Description_es = SelectConnectorFamilies.Description_es;
                    Description_fr = SelectConnectorFamilies.Description_fr;
                    Description_pt = SelectConnectorFamilies.Description_pt;
                    Description_ro = SelectConnectorFamilies.Description_ro;
                    Description_ru = SelectConnectorFamilies.Description_ru;
                    Description_zh = SelectConnectorFamilies.Description_zh;
                    getFamilyImages(SelectConnectorFamilies);
                    getSubFamilies(SelectConnectorFamilies);
                    if (ConnectorTypeList?.Count > 0)
                        SelectedConnectorType = ConnectorTypeList.FirstOrDefault(i=>i.IdLookupValue == SelectConnectorFamilies?.ConnectorType.IdLookupValue);
                }          
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }

        }
        //[Aishwarya Ingale[Edit SubFamily Edit button]]
        private void EditSubFamilyButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                ConnectorSubFamily SelectedRow = (ConnectorSubFamily)detailView.DataControl.SelectedItem;                
                AddSubFamilyView addSubFamilyView = new AddSubFamilyView();
                AddSubFamilyViewModel addSubFamilyViewModel = new AddSubFamilyViewModel();
                addSubFamilyViewModel.WindowHeader = Application.Current.FindResource("SCMEditSubFamilyPropertyTitle").ToString();
                EventHandler handle = delegate { addSubFamilyView.Close(); };
                addSubFamilyViewModel.RequestClose += handle;
                addSubFamilyViewModel.IsNew = false;
                addSubFamilyView.DataContext = addSubFamilyViewModel;                
                addSubFamilyViewModel.EditInit(Convert.ToInt32(SelectedRow.Id), SelectedRow.FamilyName);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                addSubFamilyView.Owner = Window.GetWindow(ownerInfo);
                addSubFamilyView.ShowDialogWindow();
                if (addSubFamilyViewModel.IsUpdate == true)
                {
                    SelectedSubFamily = addSubFamilyViewModel.UpdateSubFamilyDetails;
                    SelectedSubFamily.TransactionOperation = ModelBase.TransactionOperations.Update;
                    ConnectorSubFamily modifiedSubFamily = SubFamiliesList.FirstOrDefault(r => r.Id == addSubFamilyViewModel.IdConnectorSubFamily);
                    int i = SubFamiliesList.IndexOf(modifiedSubFamily);
                    SelectedSubFamily.IdFamily = (uint)IdFamily;
                    SubFamiliesList.Insert(i, SelectedSubFamily); SubFamiliesList.Remove(modifiedSubFamily);
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();
                }

                GeosApplication.Instance.Logger.Log("Method AddSubFamilyButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddSubFamilyButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        void GetConnectorType()//[rdixit][25.01.2024][GEOS2-5148]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Started GetConnectorType()....", Category.Info, priority: Priority.Low);
                ConnectorTypeList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(145));
        
                GeosApplication.Instance.Logger.Log("Ended GetConnectorType()....", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method getSubFamilies()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                string error = me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => SelectedConnectorType)];

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
                string selectedConnectorType = BindableBase.GetPropertyName(() => SelectedConnectorType);
                if (columnName == name)
                {
                    return AddEditConnectorFamilyValidation.GetErrorMessage(name, ConnectorName?.Where(i => i != null).ToList(), Name);
                    
                }
                else if(columnName == selectedConnectorType)       //[rdixit][25.01.2024][GEOS2-5148]
                {
                    return AddEditConnectorFamilyValidation.GetErrorMessage(selectedConnectorType, null, SelectedConnectorType);
                }
                return null;
            }
        }
        #endregion       
    }
}