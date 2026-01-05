using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[Sudhir.Jangra][GEOS2-4565][27/07/2023]
    public class AddSubFamilyViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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
        string windowHeader;
        private int idFamily;
        public ObservableCollection<SubFamilyImage> modifiedFamilyImageList;
        private bool isUpdate;
        private uint idConnectorSubFamily;
        private ConnectorSubFamily clonedSubfamily;
        private ConnectorSubFamily updateSubFamilyDetails;
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
        private string isInUse;
        private ConnectorSubFamily newSubFamilyDetails;
        private ObservableCollection<SubFamilyImage> subFamilyImageList;
        private SubFamilyImage selectedImage;
        private int selectedImageIndex;
        private bool isSave;
        private bool isNew;
        private string informationError;
        private string error = string.Empty;
        public bool IsFromInformation = false;
        ObservableCollection<ConnectorSubFamily> subFamilyList;
        private ConnectorFamily clonedFmailyiamgeType;
        private bool isSCMEditFamiliesManager;//[pramod.misal][GEOS2-5482][24.05.2024]
        private bool isSCMEditFamiliesManagerBtn;//[pramod.misal][GEOS2-5482][24.05.2024]
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

        //[pramod.misal][GEOS2-5482][24.05.2024]
        public bool IsSCMEditFamiliesManagerBtn
        {
            get { return isSCMEditFamiliesManagerBtn; }
            set
            {

                isSCMEditFamiliesManagerBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFamiliesManagerBtn"));

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
        #region Name & Description
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
        #endregion
        public bool IsCheckedCopyDescription
        {
            get { return isCheckedCopyDescription; }
            set
            {
                isCheckedCopyDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyDescription"));

            }
        }
        public string IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInUse"));
            }
        }
        public ConnectorSubFamily NewSubFamilyDetails
        {
            get { return newSubFamilyDetails; }
            set
            {
                newSubFamilyDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewSubFamilyDetails"));
            }
        }        
        public ObservableCollection<SubFamilyImage> ModifiedFamilyImageList
        {
            get { return modifiedFamilyImageList; }
            set
            {
                modifiedFamilyImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedFamilyImageList"));
            }
        }
        public ObservableCollection<SubFamilyImage> SubFamilyImageList
        {
            get { return subFamilyImageList; }
            set
            {
                subFamilyImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubFamilyImageList"));
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
        public SubFamilyImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
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

        public bool IsNewFamily
        {
            get;set;
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
        public int IdFamily
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
        //[Aishwarya Ingale[18-08-2022]]
        public ObservableCollection<ConnectorSubFamily> SubFamilyList
        {
            get { return subFamilyList; }
            set
            {
                subFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubFamilyList"));
            }
        }
        public ConnectorSubFamily UpdateSubFamilyDetails
        {
            get { return updateSubFamilyDetails; }
            set
            {
                updateSubFamilyDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateSubFamilyDetails"));
            }
        }       
        public ConnectorSubFamily ClonedSubfamily
        {
            get
            {
                return clonedSubfamily;
            }
            set
            {
                clonedSubfamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedSubfamily"));
            }
        }
        public bool IsUpdate
        {
            get { return isUpdate; }
            set
            {
                isUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate"));
            }
        }     
        public uint IdConnectorSubFamily
        {
            get { return idConnectorSubFamily; }
            set { idConnectorSubFamily = value; OnPropertyChanged(new PropertyChangedEventArgs("IdConnectorSubFamily")); }
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
        #endregion

        #region Public ICommand
        public ICommand AddFamilyPropertyViewCancelButtonCommand { get; set; }
        public ICommand LoadedCopyNameDescriptionCommand { get; set; }
        public ICommand AddImageButtonCommand { get; set; }
        public ICommand AddAcceptButtonCommand { get; set; }

        public ICommand EditImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }

        public ICommand CheckedCopyNameDescriptionCommand { get; set; }
        public ICommand UnCheckedCopyNameDescriptionCommand { get; set; }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public AddSubFamilyViewModel()
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
                LoadedCopyNameDescriptionCommand = new DelegateCommand<object>(LoadedCopyNameDescription);
                AddImageButtonCommand = new DelegateCommand<object>(AddImageAction);
                EditImageCommand = new DelegateCommand<object>(EditImageAction);
                DeleteImageCommand = new DelegateCommand<object>(DeleteImageAction);
                AddAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeNameCommand = new DelegateCommand<EditValueChangingEventArgs>(SetNameToLanguage);
                CheckedCopyNameDescriptionCommand = new DelegateCommand<object>(CheckedCopyNameDescription);
                UnCheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UnCheckedCopyNameDescription);
                ModifiedFamilyImageList = new ObservableCollection<SubFamilyImage>();

                //[pramod.misal][GEOS2-5482][23.05.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFamiliesManager)
                {
                    IsSCMEditFamiliesManager = false;
                    IsSCMEditFamiliesManagerBtn = true;
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
                }
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
        public void Init(bool isNewFamily,int idFamily)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()", category: Category.Info, priority: Priority.Low);
                AddLanguage();
                IsInUse = "True";
                IsNewFamily = isNewFamily;
                IdFamily = idFamily;
                IsNew = true;
                SubFamilyImageList = new ObservableCollection<SubFamilyImage>();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        public void EditInit(int idSubFamily , string familyName)
        {
            try
            {
                AddLanguage();
                //Service Updated from GetSubFamilyDetails to GetSubFamilyDetails_V2450 [rdixit][19.10.2023][GEOS2-4958]
                //[rdixit][05.03.2025][GEOS2-7026] 
                ConnectorSubFamily subFamily = SCMService.GetSubFamilyDetails_V2620(idSubFamily, familyName);
                ClonedSubfamily = (ConnectorSubFamily)subFamily.Clone();
                ClonedSubfamily.FamilyName = familyName;
                IdConnectorSubFamily = ClonedSubfamily.Id;
                if (subFamily.IsInUse == "1")
                {
                    IsInUse = "True";
                }
                else
                {
                    IsInUse = "False";
                }
                #region Name Description
                Name = Convert.ToString(subFamily.Name);
                Name_en = Convert.ToString(subFamily.Name);
                Name_es = Convert.ToString(subFamily.Name_es);
                Name_fr = Convert.ToString(subFamily.Name_fr);
                Name_pt = Convert.ToString(subFamily.Name_pt);
                Name_ro = Convert.ToString(subFamily.Name_ro);
                Name_ru = Convert.ToString(subFamily.Name_ru);
                Name_zh = Convert.ToString(subFamily.Name_zh);

                Description = Convert.ToString(subFamily.Description);
                Description_en = Convert.ToString(subFamily.Description);
                Description_es = Convert.ToString(subFamily.Description_es);
                Description_fr = Convert.ToString(subFamily.Description_fr);
                Description_pt = Convert.ToString(subFamily.Description_pt);
                Description_ro = Convert.ToString(subFamily.Description_ro);
                Description_ru = Convert.ToString(subFamily.Description_ru);
                Description_zh = Convert.ToString(subFamily.Description_zh);

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                 Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyDescription = true;
                }
                else
                {
                    IsCheckedCopyDescription = false;
                }
                SubFamilyImageList = new ObservableCollection<SubFamilyImage>();
                //[rdixit][05.03.2025][GEOS2-7026] 
                if (subFamily.ImageList != null)
                {
                    foreach (var item in subFamily.ImageList)
                    {
                        item.TransactionOperation = ModelBase.TransactionOperations.Modify;
                        item.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(item.SubFamilyImageInBytes);
                        SubFamilyImageList.Add(item);
                    }
                }
                if (SubFamilyImageList?.Count > 0)
                {
                    if (!(SubFamilyImageList.Any(i => i.Position == 1)))
                    {
                        SubFamilyImageList.FirstOrDefault().Position = 1;
                        SubFamilyImageList.FirstOrDefault().TransactionOperation = ModelBase.TransactionOperations.Update;
                        //ModifiedFamilyImageList.Add(SubFamilyImageList.FirstOrDefault());
                    }
                }
                #endregion
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //public void EditInit(ConnectorSubFamily selectedSubfamilyConnectorFamilies)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method SubFamilyEditInit()", category: Category.Info, priority: Priority.Low);
        //        AddLanguage();

        //        if (selectedSubfamilyConnectorFamilies != null)
        //        {
        //            ClonedSubfamily = (ConnectorSubFamily)selectedSubfamilyConnectorFamilies.Clone();
        //            SubFamilyList = new ObservableCollection<ConnectorSubFamily>();
        //            IdConnectorSubFamily = ClonedSubfamily.Id;
        //            IdFamily = ClonedSubfamily.IdFamily;
        //            #region Name Description
        //            Name = Convert.ToString(selectedSubfamilyConnectorFamilies.Name);
        //            Name_en = Convert.ToString(selectedSubfamilyConnectorFamilies.Name);
        //            Name_es = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_es);
        //            Name_fr = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_fr);
        //            Name_pt = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_pt);
        //            Name_ro = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_ro);
        //            Name_ru = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_ru);
        //            Name_zh = Convert.ToString(selectedSubfamilyConnectorFamilies.Name_zh);

        //            Description = Convert.ToString(selectedSubfamilyConnectorFamilies.Description);
        //            Description_en = Convert.ToString(selectedSubfamilyConnectorFamilies.Description);
        //            Description_es = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_es);
        //            Description_fr = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_fr);
        //            Description_pt = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_pt);
        //            Description_ro = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_ro);
        //            Description_ru = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_ru);
        //            Description_zh = Convert.ToString(selectedSubfamilyConnectorFamilies.Description_zh);
        //            #endregion                  
        //            IdFamily = selectedSubfamilyConnectorFamilies.IdFamily;
        //            if (selectedSubfamilyConnectorFamilies.IsInUse == "1")
        //            {
        //                IsInUse = "True";
        //            }
        //            else
        //            {
        //                IsInUse = "False";
        //            }
        //            IsNew = false;
        //            SubFamilyImageList = new ObservableCollection<SubFamilyImage>();
        //            foreach (var item in selectedSubfamilyConnectorFamilies.ImageList)
        //            {
        //                item.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(item.SubFamilyImageInBytes);
        //                SubFamilyImageList.Add(item);
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log(string.Format("Method SubFamilyEditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method SubFamilyEditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void LoadedCopyNameDescription(object obj)
        {
            try
            {
                System.Windows.RoutedEventArgs obj1 = (System.Windows.RoutedEventArgs)obj;
                if (obj1.OriginalSource is CheckEdit)
                {
                    CheckEdit checkbox = (CheckEdit)obj1.OriginalSource;
                    if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                     Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                    {
                        IsCheckedCopyDescription = true;
                        checkbox.IsChecked = true;
                    }
                    else
                    {
                        IsCheckedCopyDescription = false;
                        checkbox.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckedCopyNameDescription() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);

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
        private void AddImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddSubFamilyImageView addFamilyImageView = new AddSubFamilyImageView();
                AddSubFamilyImageViewModel addFamilyImageViewModel = new AddSubFamilyImageViewModel();
                EventHandler handle = delegate { addFamilyImageView.Close(); };
                addFamilyImageViewModel.RequestClose += handle;
                addFamilyImageView.DataContext = addFamilyImageViewModel;
                addFamilyImageViewModel.IsNew = true;
                SubFamilyImage tempObj = new SubFamilyImage();
                tempObj.IdSubFamily = (uint)IdConnectorSubFamily;
                addFamilyImageViewModel.ImagesList = SubFamilyImageList;
                addFamilyImageViewModel.Init(tempObj);               
                addFamilyImageView.ShowDialog();

                if (addFamilyImageViewModel.IsSave)
                {
                    SelectedImage = new SubFamilyImage();
                    SubFamilyImage newImage = new SubFamilyImage();
                    newImage = addFamilyImageViewModel.SelectedImage;
                    newImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(newImage.SubFamilyImageInBytes);
                    newImage.TransactionOperation = ModelBase.TransactionOperations.Add;
                    SubFamilyImageList.Add(newImage);
                    ModifiedFamilyImageList.Add(newImage);
                }

                GeosApplication.Instance.Logger.Log("Method AddImageButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                NewSubFamilyDetails = new ConnectorSubFamily();
                if (IsNew)
                {
                    InformationError = null;
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

                    #region Name & Description
                    if (IsCheckedCopyDescription == true)
                    {
                        NewSubFamilyDetails.Name = Name;
                        NewSubFamilyDetails.Name_es = Name;
                        NewSubFamilyDetails.Name_fr = Name;
                        NewSubFamilyDetails.Name_pt = Name;
                        NewSubFamilyDetails.Name_ro = Name;
                        NewSubFamilyDetails.Name_ru = Name;
                        NewSubFamilyDetails.Name_zh = Name;
                        NewSubFamilyDetails.Description = Description;
                        NewSubFamilyDetails.Description_es = Description;
                        NewSubFamilyDetails.Description_fr = Description;
                        NewSubFamilyDetails.Description_pt = Description;
                        NewSubFamilyDetails.Description_ro = Description;
                        NewSubFamilyDetails.Description_ru = Description;
                        NewSubFamilyDetails.Description_zh = Description;
                    }
                    else
                    {
                        NewSubFamilyDetails.Name = Name_en;
                        NewSubFamilyDetails.Name_es = Name_es;
                        NewSubFamilyDetails.Name_fr = Name_fr;
                        NewSubFamilyDetails.Name_pt = Name_pt;
                        NewSubFamilyDetails.Name_ro = Name_ro;
                        NewSubFamilyDetails.Name_ru = Name_ru;
                        NewSubFamilyDetails.Name_zh = Name_zh;
                        NewSubFamilyDetails.Description = Description_en;
                        NewSubFamilyDetails.Description_es = Description_es;
                        NewSubFamilyDetails.Description_fr = Description_fr;
                        NewSubFamilyDetails.Description_pt = Description_pt;
                        NewSubFamilyDetails.Description_ro = Description_ro;
                        NewSubFamilyDetails.Description_ru = Description_ru;
                        NewSubFamilyDetails.Description_zh = Description_zh;
                    }
                    #endregion

                    if (IsInUse == "True")
                        NewSubFamilyDetails.IsInUse = "1";
                    else
                        NewSubFamilyDetails.IsInUse = "0";
                    //[rdixit][GEOS2-7026][12.03.2025]
                    if(SubFamilyImageList == null)
                    {
                        SubFamilyImageList = new ObservableCollection<SubFamilyImage>();
                    }
                    SubFamilyImageList.ToList().ForEach(x => x.TransactionOperation = ModelBase.TransactionOperations.Add);
                    NewSubFamilyDetails.ImageList = new ObservableCollection<SubFamilyImage>(SubFamilyImageList.Select(i=> (SubFamilyImage)i.Clone()).ToList());
                    NewSubFamilyDetails.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    if(!IsNewFamily)
                    {
                        if (NewSubFamilyDetails.ImageList != null)
                            NewSubFamilyDetails.ImageList.ToList().ForEach(i => i.AttachmentImage = null);
                        NewSubFamilyDetails.IdFamily = Convert.ToUInt32(IdFamily);
                        IsSave = SCMService.InsertSubFamily_V2620(NewSubFamilyDetails);
                    }
                    else
                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SubFamilyAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method AddAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    UpdateSubFamilyDetails = new ConnectorSubFamily();
                    UpdateSubFamilyDetails.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UpdateSubFamilyDetails.IdFamily = ClonedSubfamily.IdFamily;
                    UpdateSubFamilyDetails.FamilyName = ClonedSubfamily.FamilyName;
                    #region Name & Description
                    if (IsCheckedCopyDescription == true)
                    {
                        IsFromInformation = true;
                        UpdateSubFamilyDetails.Description = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_es = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_fr = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_pt = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_ro = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_ru = Description == null ? "" : Description.Trim();
                        UpdateSubFamilyDetails.Description_zh = Description == null ? "" : Description.Trim();

                        UpdateSubFamilyDetails.Name = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_es = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_fr = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_pt = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_ro = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_ru = Name == null ? "" : Name.Trim();
                        UpdateSubFamilyDetails.Name_zh = Name == null ? "" : Name.Trim();
                    }
                    else
                    {
                        IsFromInformation = true;
                        UpdateSubFamilyDetails.Description = Description_en == null ? "" : Description_en.Trim();
                        UpdateSubFamilyDetails.Description_es = Description_es == null ? "" : Description_es.Trim();
                        UpdateSubFamilyDetails.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                        UpdateSubFamilyDetails.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                        UpdateSubFamilyDetails.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                        UpdateSubFamilyDetails.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                        UpdateSubFamilyDetails.Description_zh = Description_zh == null ? "" : Description_zh.Trim();

                        UpdateSubFamilyDetails.Name = Name_en == null ? "" : Name_en.Trim();
                        UpdateSubFamilyDetails.Name_es = Name_es == null ? "" : Name_es.Trim();
                        UpdateSubFamilyDetails.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                        UpdateSubFamilyDetails.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                        UpdateSubFamilyDetails.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                        UpdateSubFamilyDetails.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                        UpdateSubFamilyDetails.Name_zh = Name_zh == null ? "" : Name_zh.Trim();
                    }
                    #endregion

                    if (IsInUse == "True")
                        UpdateSubFamilyDetails.IsInUse = "1";
                    else
                        UpdateSubFamilyDetails.IsInUse = "0";
                    //Images
                    if (SubFamilyImageList == null)
                    {
                        SubFamilyImageList = new ObservableCollection<SubFamilyImage>();
                    }
                    if (UpdateSubFamilyDetails.ImageList == null)
                        UpdateSubFamilyDetails.ImageList = new ObservableCollection<SubFamilyImage>();

                    UpdateSubFamilyDetails.Id = IdConnectorSubFamily;
                    #region Family Images
                    //Code added when the family name is changed then the image path shoud also update in DB                  
                    UpdateSubFamilyDetails.OldSubFamilyName = ClonedSubfamily.Name_es;
                    if (UpdateSubFamilyDetails.OldSubFamilyName.ToLower() != Name_es.ToLower())
                    {
                        if (SubFamilyImageList.Count > 0)
                        {
                            if (ModifiedFamilyImageList == null)
                                ModifiedFamilyImageList = new ObservableCollection<SubFamilyImage>();
                            foreach (var item in SubFamilyImageList.Where(i => (!ModifiedFamilyImageList.Any(k => k.IdSubFamilyImage == i.IdSubFamilyImage))).ToList())
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                ModifiedFamilyImageList.Add(item);
                            }
                        }
                    }
                    if (ModifiedFamilyImageList != null)
                    {
                        foreach (SubFamilyImage item in ModifiedFamilyImageList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete ||
                        i.TransactionOperation == ModelBase.TransactionOperations.Add || i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList())
                        {
                            item.AttachmentImage = null;
                            UpdateSubFamilyDetails.ImageList.Add(item);
                        }
                    }
                    #endregion
                    //Service Updated from UpdateSubFamily_V2430 to UpdateSubFamily_V2450 [rdixit][20.10.2023][GEOS2-4958]
                    //[rdixit][GEOS2-7026][12.03.2025]
                    IsUpdate = SCMService.UpdateSubFamily_V2620(UpdateSubFamilyDetails);

                    if (IsUpdate == true)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ConnectorFamilyUpdateMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    RequestClose(null, null);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);
                Emdep.Geos.UI.Helper.ImageContainer Obj1 = (Emdep.Geos.UI.Helper.ImageContainer)obj;
                AddSubFamilyImageView addSubFamilyImageView = new AddSubFamilyImageView();
                AddSubFamilyImageViewModel addSubFamilyImageViewModel = new AddSubFamilyImageViewModel();
                EventHandler handle = delegate { addSubFamilyImageView.Close(); };
                addSubFamilyImageViewModel.RequestClose += handle;             
                SelectedImage = (SubFamilyImage)Obj1.DataContext;
                SelectedImage.IdSubFamily = (uint)IdConnectorSubFamily;
                addSubFamilyImageViewModel.ImagesList = SubFamilyImageList;
                addSubFamilyImageViewModel.EditInit(SelectedImage);
                addSubFamilyImageView.DataContext = addSubFamilyImageViewModel;
                addSubFamilyImageView.ShowDialogWindow();

                if (addSubFamilyImageViewModel.IsSave)
                {
                    SubFamilyImage UpdatedImage = SubFamilyImageList.FirstOrDefault(i => i.IdSubFamilyImage == addSubFamilyImageViewModel.IdImage);              
                    UpdatedImage.OriginalFileName = addSubFamilyImageViewModel.ImageName;
                    UpdatedImage.Description = addSubFamilyImageViewModel.Description;
                    UpdatedImage.ConnectorFamilyImageInBytes = addSubFamilyImageViewModel.FileInBytes;
                    UpdatedImage.SavedFileName = addSubFamilyImageViewModel.SavedImageName;
                    UpdatedImage.Position = addSubFamilyImageViewModel.SelectedImage.Position;
                    UpdatedImage.CreatedBy = addSubFamilyImageViewModel.SelectedImage.CreatedBy;
                    UpdatedImage.ModifiedBy = addSubFamilyImageViewModel.SelectedImage.ModifiedBy;
                    UpdatedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    UpdatedImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                    SelectedImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(UpdatedImage.ConnectorFamilyImageInBytes);
                    ModifiedFamilyImageList.Add(UpdatedImage);
                    SubFamilyImageList = new ObservableCollection<SubFamilyImage>(SubFamilyImageList.OrderBy(a => a.Position));
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
                SelectedImage = (SubFamilyImage)Obj1.DataContext;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SubFamilyImage DeletedImage = new SubFamilyImage();
                    DeletedImage = (SubFamilyImage)Obj1.DataContext;

                    if (SubFamilyImageList?.Count > 0)
                        SubFamilyImageList.Remove(DeletedImage);

                    if (ModifiedFamilyImageList != null)
                    {
                        DeletedImage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        ModifiedFamilyImageList.Add(DeletedImage);
                    }
                    if (SubFamilyImageList?.Count > 0)
                    {
                        if (!(SubFamilyImageList.Any(i => i.Position == 1)))
                        {
                            SubFamilyImageList.FirstOrDefault().Position = 1;
                            if (SubFamilyImageList.FirstOrDefault().TransactionOperation != ModelBase.TransactionOperations.Add && SubFamilyImageList.FirstOrDefault().TransactionOperation != ModelBase.TransactionOperations.Update)
                            {
                                SubFamilyImageList.FirstOrDefault().TransactionOperation = ModelBase.TransactionOperations.Update;
                                ModifiedFamilyImageList.Add(SubFamilyImageList.FirstOrDefault());
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
        private void SetNameToLanguage(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()..."), category: Category.Info, priority: Priority.Low);
                //[001] Removed else part
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
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()..."), category: Category.Info, priority: Priority.Low);
                //[001] Removed else part
                if (IsCheckedCopyDescription == false && SelectedLanguage != null)
                {
                    if (SelectedLanguage.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description == null ? "" : Description;
                    }
                    else if (SelectedLanguage.TwoLetterISOLanguage == "ZH")
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
        private void UnCheckedCopyNameDescription(object obj)
        {
            System.Windows.RoutedEventArgs obj1 = (System.Windows.RoutedEventArgs)obj;
            if (obj1.OriginalSource is CheckEdit)
            {
                CheckEdit checkbox = (CheckEdit)obj1.OriginalSource;
                IsCheckedCopyDescription = false;
                checkbox.IsChecked = false;
            }
        }
        private void CheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()..."), category: Category.Info, priority: Priority.Low);

                if (SelectedLanguage != null)
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
                    return AddEditConnectorFamilyValidation.GetErrorMessage(name, null, Name);
                }
                return null;
            }
        }
        #endregion
        #endregion
    }
}
