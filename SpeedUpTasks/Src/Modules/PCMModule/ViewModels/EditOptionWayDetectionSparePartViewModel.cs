using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.WindowsUI;
using WindowsUIDemo;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class EditOptionWayDetectionSparePartViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declarations

        private DetectionAttachedDoc clonedDetections;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private string code;
        private uint weldOrder;
        private string header;
        private ObservableCollection<Language> languages;


        private Template template;
        private uint idDetectionType;
        private string description;
        private string name;
        private uint idDetections;
        private ObservableCollection<TestTypes> testTypesMenuList;
        private TestTypes selectedTestType;
        private uint idDetectionTypeOption;
        private string type;

        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;

        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private bool isCheckedCopyNameDescription;

        private Language languageSelected;
        private bool IsCopyName;
        private bool IsCopyDescription;
        #endregion

        #region Properties

        public DetectionDetails UpdatedItem { get; set; }

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

        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        public uint WeldOrder
        {
            get
            {
                return weldOrder;
            }

            set
            {
                weldOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeldOrder"));
            }
        }

        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }

        public ObservableCollection<Language> Languages
        {
            get
            {
                return languages;
            }

            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }

        public Template Template
        {
            get
            {
                return template;
            }

            set
            {
                template = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Template"));
            }
        }

        public uint IdDetectionType
        {
            get
            {
                return idDetectionType;
            }

            set
            {
                idDetectionType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionType"));
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        
        public uint IdDetections
        {
            get
            {
                return idDetections;
            }

            set
            {
                idDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetections"));
            }
        }
        public ObservableCollection<TestTypes> TestTypesMenuList
        {
            get
            {
                return testTypesMenuList;
            }

            set
            {
                testTypesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestTypesMenuList"));
            }
        }

        public TestTypes SelectedTestType
        {
            get
            {
                return selectedTestType;
            }

            set
            {
                selectedTestType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTestType"));
            }
        }

        public uint IdDetectionTypeOption
        {
            get
            {
                return idDetectionTypeOption;
            }

            set
            {
                idDetectionTypeOption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionTypeOption"));
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
            }
        }
        public bool IsCheckedCopyNameDescription
        {
            get
            {
                return isCheckedCopyNameDescription;
            }

            set
            {
                isCheckedCopyNameDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameDescription"));
            }
        }

        public string Name_en
        {
            get
            {
                return name_en;
            }

            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
            }
        }

        public string Name_es
        {
            get
            {
                return name_es;
            }

            set
            {
                name_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_es"));
            }
        }

        public string Name_fr
        {
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_fr"));
            }
        }

        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_pt"));
            }
        }

        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ro"));
            }
        }

        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ru"));
            }
        }

        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
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

        public DetectionAttachedDoc ClonedDetections
        {
            get
            {
                return clonedDetections;
            }

            set
            {
                clonedDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDetections"));

            }
        }
        #endregion

        #region ICommands
        public ICommand AcceptButtonCommand { get; set; }

        public ICommand CancelButtonCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }

       

        #endregion

        #region Constructor

        public EditOptionWayDetectionSparePartViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditOptionWayDetectionSparePartViewModel ...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameAndDescriptionByLanguge);
                ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                UncheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UncheckedCopyNameDescription);

                AddLanguages();
                FillTestTypes();
                IsCheckedCopyNameDescription = true;

                GeosApplication.Instance.Logger.Log("Constructor EditOptionWayDetectionSparePartViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor EditOptionWayDetectionSparePartViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Method

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                UpdatedItem = new DetectionDetails();
                if (Header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    UpdatedItem.IdDetections = IdDetections;
                    UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsCheckedCopyNameDescription == true)
                    {
                        UpdatedItem.Name = Name;
                        UpdatedItem.Name_es = Name;
                        UpdatedItem.Name_fr = Name;
                        UpdatedItem.Name_pt = Name;
                        UpdatedItem.Name_ro = Name;
                        UpdatedItem.Name_ru = Name;
                        UpdatedItem.Name_zh = Name;

                        UpdatedItem.Description = Description;
                        UpdatedItem.Description_es = Description;
                        UpdatedItem.Description_fr = Description;
                        UpdatedItem.Description_pt = Description;
                        UpdatedItem.Description_ro = Description;
                        UpdatedItem.Description_ru = Description;
                        UpdatedItem.Description_zh = Description == null ? "" : Description;
                    }
                    else
                    {
                        UpdatedItem.Name = Name_en == null ? "" : Name_en;
                        UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                        UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                        UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                        UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                        UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                        UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                        UpdatedItem.Description = Description_en;
                        UpdatedItem.Description_es = Description_es;
                        UpdatedItem.Description_fr = Description_fr;
                        UpdatedItem.Description_pt = Description_pt;
                        UpdatedItem.Description_ro = Description_ro;
                        UpdatedItem.Description_ru = Description_ru;
                        UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                    }

                    UpdatedItem.Code = Code;
                    UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                    UpdatedItem.IdDetectionType = IdDetectionType;
                    UpdatedItem.Orientation = null;
                    UpdatedItem.NameToShow = "";
                    IsSave = PCMService.UpdateDetection(UpdatedItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OptionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }
                else if(Header == System.Windows.Application.Current.FindResource("CaptionWay").ToString())
                {
                    UpdatedItem.IdDetections = IdDetections;
                    UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsCheckedCopyNameDescription == true)
                    {
                        UpdatedItem.Name = Name;
                        UpdatedItem.Name_es = Name;
                        UpdatedItem.Name_fr = Name;
                        UpdatedItem.Name_pt = Name;
                        UpdatedItem.Name_ro = Name;
                        UpdatedItem.Name_ru = Name;
                        UpdatedItem.Name_zh = Name;

                        UpdatedItem.Description = Description;
                        UpdatedItem.Description_es = Description;
                        UpdatedItem.Description_fr = Description;
                        UpdatedItem.Description_pt = Description;
                        UpdatedItem.Description_ro = Description;
                        UpdatedItem.Description_ru = Description;
                        UpdatedItem.Description_zh = Description == null ? "" : Description;
                    }
                    else
                    {
                        UpdatedItem.Name = Name_en == null ? "" : Name_en;
                        UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                        UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                        UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                        UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                        UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                        UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                        UpdatedItem.Description = Description_en;
                        UpdatedItem.Description_es = Description_es;
                        UpdatedItem.Description_fr = Description_fr;
                        UpdatedItem.Description_pt = Description_pt;
                        UpdatedItem.Description_ro = Description_ro;
                        UpdatedItem.Description_ru = Description_ru;
                        UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                    }

                    UpdatedItem.Code = Code;
                    UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                    UpdatedItem.IdDetectionType = IdDetectionType;
                    UpdatedItem.Orientation = null;
                    UpdatedItem.NameToShow = "";
                    IsSave = PCMService.UpdateDetection(UpdatedItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WayItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }

                else if (Header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    UpdatedItem.IdDetections = IdDetections;
                    UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsCheckedCopyNameDescription == true)
                    {
                        UpdatedItem.Name = Name;
                        UpdatedItem.Name_es = Name;
                        UpdatedItem.Name_fr = Name;
                        UpdatedItem.Name_pt = Name;
                        UpdatedItem.Name_ro = Name;
                        UpdatedItem.Name_ru = Name;
                        UpdatedItem.Name_zh = Name;

                        UpdatedItem.Description = Description;
                        UpdatedItem.Description_es = Description;
                        UpdatedItem.Description_fr = Description;
                        UpdatedItem.Description_pt = Description;
                        UpdatedItem.Description_ro = Description;
                        UpdatedItem.Description_ru = Description;
                        UpdatedItem.Description_zh = Description == null ? "" : Description;
                    }
                    else
                    {
                        UpdatedItem.Name = Name_en == null ? "" : Name_en;
                        UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                        UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                        UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                        UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                        UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                        UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                        UpdatedItem.Description = Description_en;
                        UpdatedItem.Description_es = Description_es;
                        UpdatedItem.Description_fr = Description_fr;
                        UpdatedItem.Description_pt = Description_pt;
                        UpdatedItem.Description_ro = Description_ro;
                        UpdatedItem.Description_ru = Description_ru;
                        UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                    }

                    UpdatedItem.Code = Code;
                    UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                    UpdatedItem.IdDetectionType = IdDetectionType;
                    UpdatedItem.Orientation = null;
                    UpdatedItem.NameToShow = "";
                    IsSave = PCMService.UpdateDetection(UpdatedItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }

                else if (Header == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    UpdatedItem.IdDetections = IdDetections;
                    UpdatedItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    if (IsCheckedCopyNameDescription == true)
                    {
                        UpdatedItem.Name = Name;
                        UpdatedItem.Name_es = Name;
                        UpdatedItem.Name_fr = Name;
                        UpdatedItem.Name_pt = Name;
                        UpdatedItem.Name_ro = Name;
                        UpdatedItem.Name_ru = Name;
                        UpdatedItem.Name_zh = Name;

                        UpdatedItem.Description = Description;
                        UpdatedItem.Description_es = Description;
                        UpdatedItem.Description_fr = Description;
                        UpdatedItem.Description_pt = Description;
                        UpdatedItem.Description_ro = Description;
                        UpdatedItem.Description_ru = Description;
                        UpdatedItem.Description_zh = Description == null ? "" : Description;
                    }
                    else
                    {
                        UpdatedItem.Name = Name_en == null ? "" : Name_en;
                        UpdatedItem.Name_es = Name_es == null ? "" : Name_es;
                        UpdatedItem.Name_fr = Name_fr == null ? "" : Name_fr;
                        UpdatedItem.Name_pt = Name_pt == null ? "" : Name_pt;
                        UpdatedItem.Name_ro = Name_ro == null ? "" : Name_ro;
                        UpdatedItem.Name_ru = Name_ru == null ? "" : Name_ru;
                        UpdatedItem.Name_zh = Name_zh == null ? "" : Name_zh;

                        UpdatedItem.Description = Description_en;
                        UpdatedItem.Description_es = Description_es;
                        UpdatedItem.Description_fr = Description_fr;
                        UpdatedItem.Description_pt = Description_pt;
                        UpdatedItem.Description_ro = Description_ro;
                        UpdatedItem.Description_ru = Description_ru;
                        UpdatedItem.Description_zh = Description_zh == null ? "" : Description_zh;
                    }

                    UpdatedItem.Code = Code;
                    UpdatedItem.IdTestType = SelectedTestType.IdTestType;
                    UpdatedItem.IdDetectionType = IdDetectionType;
                    UpdatedItem.Orientation = null;
                    UpdatedItem.NameToShow = "";
                    IsSave = PCMService.UpdateDetection(UpdatedItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SparePartItemUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }

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

        private void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguages()...", category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTestTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTestTypes()...", category: Category.Info, priority: Priority.Low);

                TestTypesMenuList = new ObservableCollection<TestTypes>(PCMService.GetAllTestTypes());

                GeosApplication.Instance.Logger.Log("Method FillTestTypes()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTestTypes() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTestTypes() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitOptions(Options tempSelectedType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitOptions()...", category: Category.Info, priority: Priority.Low);
               
                var temp = (PCMService.GetDetectionByIdDetection(tempSelectedType.IdOptions));
                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x=>x.IdTestType == temp.IdTestType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }
                GeosApplication.Instance.Logger.Log("Method EditInitOptions()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitOptions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitOptions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitOptions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitWays(Ways tempSelectedWay)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWays()...", category: Category.Info, priority: Priority.Low);

                var temp = (PCMService.GetDetectionByIdDetection(tempSelectedWay.IdWays));
                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }
                GeosApplication.Instance.Logger.Log("Method EditWays()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWays() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWays() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditWays() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitDetections(Detections tempSelectedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()...", category: Category.Info, priority: Priority.Low);

                var temp = (PCMService.GetDetectionByIdDetection(tempSelectedDetection.IdDetections));
                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }
                GeosApplication.Instance.Logger.Log("Method EditInitDetections()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDetections() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitSparePart(SpareParts tempSelectedSpareParts)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitSparePart()...", category: Category.Info, priority: Priority.Low);

                var temp = (PCMService.GetDetectionByIdDetection(tempSelectedSpareParts.IdSpareParts));
                IdDetections = temp.IdDetections;
                IdDetectionType = temp.IdDetectionType;
                Description = temp.Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;
                Name = temp.Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                Code = temp.Code;
                WeldOrder = temp.WeldOrder;
                SelectedTestType = TestTypesMenuList.FirstOrDefault(x => x.IdTestType == temp.IdTestType);

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }
                GeosApplication.Instance.Logger.Log("Method EditInitSparePart()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitSparePart() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitSparePart() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitSparePart() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        public void InitOption(string header)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitOption()...", category: Category.Info, priority: Priority.Low);
                Header = header;
                Type = Header;

                GeosApplication.Instance.Logger.Log("Method InitOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitOption()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveNameAndDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveNameAndDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Name = Name_en;
                        Description = Description_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Name = Name_es;
                        Description = Description_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Name = Name_fr;
                        Description = Description_fr;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Name = Name_pt;
                        Description = Description_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Name = Name_ro;
                        Description = Description_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Name = Name_ru;
                        Description = Description_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Name = Name_zh;
                        Description = Description_zh;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method RetrieveNameAndDescriptionByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveNameAndDescriptionByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false)
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
                GeosApplication.Instance.Logger.Log("Method SetNameToLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetNameToLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetDescriptionToLanguage()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
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

        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyNameDescription()...", category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Name = Name_en;
                    Description = Description_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Name = Name_es;
                    Description = Description_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Name = Name_fr;
                    Description = Description_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Name = Name_pt;
                    Description = Description_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Name = Name_ro;
                    Description = Description_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Name = Name_ru;
                    Description = Description_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Name = Name_zh;
                    Description = Description_zh;
                }
                GeosApplication.Instance.Logger.Log("Method UncheckedCopyNameDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UncheckedCopyNameDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

    }
}
