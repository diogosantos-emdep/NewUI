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
using System.IO;
using DevExpress.Mvvm;
using System.ComponentModel;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddOrderGroupInOptionsAndDetectionsViewModel : ViewModelBase, INotifyPropertyChanged
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

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private bool isCheckedCopyNameDescription;

        private string name;
        private string description;
        private string header;
        private string type;

        private ObservableCollection<Language> languages;
        private Language languageSelected;

        private ObservableCollection<DetectionGroup> groupList_Order;
        private DetectionGroup selectedGroupItem_Order;
        private DetectionGroup selectedGroupItem;

        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;

        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;

        private uint idGroup;
        private DateTime updateDate;

        #endregion

        #region Properties

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
        public ObservableCollection<DetectionGroup> GroupList_Order
        {
            get
            {
                return groupList_Order;
            }

            set
            {
                groupList_Order = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList_Order"));
            }
        }

        public DetectionGroup SelectedGroupItem_Order
        {
            get
            {
                return selectedGroupItem_Order;
            }

            set
            {
                selectedGroupItem_Order = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroupItem_Order"));
            }
        }

        public DetectionGroup SelectedGroupItem
        {
            get
            {
                return selectedGroupItem;
            }

            set
            {
                selectedGroupItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroupItem"));
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

        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdGroup"));
            }
        }
        public DateTime UpdateDate
        {
            get
            {
                return updateDate;
            }

            set
            {
                updateDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDate"));

            }
        }
        #endregion

        #region ICommand

        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddOrderGroupCommand { get; set; }
        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand AcceptGroupCommand { get; set; }



        #endregion

        #region Constructor

        public AddOrderGroupInOptionsAndDetectionsViewModel()
        {
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
            CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
            UncheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UncheckedCopyNameDescription);
            ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
            ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
            ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
            AcceptGroupCommand = new DelegateCommand<object>(AddGroup);
           
            IsCheckedCopyNameDescription = true;
        }




        #endregion

        #region Methods

        public void Init()
        {
            AddLanguages();
        }

        private void AddGroup(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddGroup()...", category: Category.Info, priority: Priority.Low);

                SelectedGroupItem = new DetectionGroup();
                SelectedGroupItem.IdGroup = IdGroup;
                if (IsCheckedCopyNameDescription == true)
                {
                    SelectedGroupItem.Name = Name;
                    SelectedGroupItem.Name_es = Name;
                    SelectedGroupItem.Name_fr = Name;
                    SelectedGroupItem.Name_pt = Name;
                    SelectedGroupItem.Name_ro = Name;
                    SelectedGroupItem.Name_ru = Name;
                    SelectedGroupItem.Name_zh = Name;

                    SelectedGroupItem.Description = Description;
                    SelectedGroupItem.Description_es = Description;
                    SelectedGroupItem.Description_fr = Description;
                    SelectedGroupItem.Description_pt = Description;
                    SelectedGroupItem.Description_ro = Description;
                    SelectedGroupItem.Description_ru = Description;
                    SelectedGroupItem.Description_zh = Description;
                }
                else
                {
                    SelectedGroupItem.Name = Name_en;
                    SelectedGroupItem.Name_es = Name_es;
                    SelectedGroupItem.Name_fr = Name_fr;
                    SelectedGroupItem.Name_pt = Name_pt;
                    SelectedGroupItem.Name_ro = Name_ro;
                    SelectedGroupItem.Name_ru = Name_ru;
                    SelectedGroupItem.Name_zh = Name_zh;

                    SelectedGroupItem.Description = Description_en;
                    SelectedGroupItem.Description_es = Description_es;
                    SelectedGroupItem.Description_fr = Description_fr;
                    SelectedGroupItem.Description_pt = Description_pt;
                    SelectedGroupItem.Description_ro = Description_ro;
                    SelectedGroupItem.Description_ru = Description_ru;
                    SelectedGroupItem.Description_zh = Description_zh;
                }
                SelectedGroupItem.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                SelectedGroupItem.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                SelectedGroupItem.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                if (Header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    SelectedGroupItem.IdDetectionType = 3;
                }
                else if (Header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    SelectedGroupItem.IdDetectionType = 2;
                }
                else if (Header == System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString())
                {
                    SelectedGroupItem.IdDetectionType = 4;
                }
             
                    //var a = SelectedGroupItem_Order;

                    IsSave = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddGroup()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddGroup() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        public void Init(string selectedHeader, ObservableCollection<DetectionGroup> GroupList_Order_List)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Header = selectedHeader;
                Type = Header;
                GroupList_Order = new ObservableCollection<DetectionGroup>(GroupList_Order_List);
                SelectedGroupItem_Order = GroupList_Order.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddGroupListForOrder() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddGroupListForOrder() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddGroupListForOrder() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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


        private void AddGroupListForOrder(string header)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddGroupListForOrder()...", category: Category.Info, priority: Priority.Low);

                if (header == System.Windows.Application.Current.FindResource("CaptionOptions").ToString())
                {
                    GroupList_Order = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(3));
                    SelectedGroupItem_Order = GroupList_Order.FirstOrDefault();
                }
                if (header == System.Windows.Application.Current.FindResource("CaptionDetections").ToString())
                {
                    GroupList_Order = new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(2));
                    SelectedGroupItem_Order = GroupList_Order.FirstOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method AddGroupListForOrder()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddGroupListForOrder() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddGroupListForOrder() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddGroupListForOrder() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(DetectionGroup selectedDetectionGroup, string header, ObservableCollection<DetectionGroup> GroupList_Order_List)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()..."), category: Category.Info, priority: Priority.Low);
                Header = header;
                Type = Header;
                GroupList_Order = new ObservableCollection<DetectionGroup>(GroupList_Order_List);
                IdGroup = selectedDetectionGroup.IdGroup;
                Name = selectedDetectionGroup.Name;
                Name_en = selectedDetectionGroup.Name;
                Name_es = selectedDetectionGroup.Name_es;
                Name_fr = selectedDetectionGroup.Name_fr;
                Name_pt = selectedDetectionGroup.Name_pt;
                Name_ro = selectedDetectionGroup.Name_ro;
                Name_ru = selectedDetectionGroup.Name_ru;
                Name_zh = selectedDetectionGroup.Name_zh;
                Description = selectedDetectionGroup.Description;
                Description_en = selectedDetectionGroup.Description;
                Description_es = selectedDetectionGroup.Description_es;
                Description_fr = selectedDetectionGroup.Description_fr;
                Description_pt = selectedDetectionGroup.Description_pt;
                Description_ro = selectedDetectionGroup.Description_ro;
                Description_ru = selectedDetectionGroup.Description_ru;
                Description_zh = selectedDetectionGroup.Description_zh;
                SelectedGroupItem_Order = GroupList_Order.FirstOrDefault(x => x.OrderNumber == selectedDetectionGroup.OrderNumber);
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                    Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyNameDescription = true;
                }
                else
                {
                    IsCheckedCopyNameDescription = false;
                }
                UpdateDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected != null)
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
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method UncheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false && LanguageSelected != null)
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
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameDescription == false && LanguageSelected != null)
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

                if (IsCheckedCopyNameDescription == false)
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

        
        #endregion
    }
}
