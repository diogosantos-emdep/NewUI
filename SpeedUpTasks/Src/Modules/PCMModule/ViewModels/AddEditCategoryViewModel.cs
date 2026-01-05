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
using DevExpress.Xpf.Editors;
using System.Text.RegularExpressions;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddEditCategoryViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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
        public ObservableCollection<PCMArticleCategory> tempOrderCategoryList { get; set; }
        public PCMArticleCategory NewArticleCategory { get; set; }
        public PCMArticleCategory UpdatedItem { get; set; }
        public PCMArticleCategory NewCategory { get; set; }

        private string windowHeader;
        private bool isNew;
        private bool isSave;

        private string name;
        private string description;

        private bool isCheckedCopyNameAndDescription;

        private ObservableCollection<Language> languages;
        private Language languageSelected;

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

        private ObservableCollection<PCMArticleCategory> orderCategoryList;
        private PCMArticleCategory selectedOrderCategory;
        private PCMArticleCategory clonedPCMArticleCategory;

        private int article_count;
        private uint? idArticleCategory;
        private uint idPCMArticleCategory;
        private long isLeaf;

        private string keyName;
        private string parentName;
        private ulong? parent;
        private uint position;
        private string nameWithArticleCount;

        private string nameErrorMsg = string.Empty;
        private string lastNameErrorMsg = string.Empty;
        private string error = string.Empty;
        private ObservableCollection<PCMArticleCategory> parentCategoryList;
        private PCMArticleCategory selectedParentCategory;
        public ObservableCollection<PCMArticleCategory> tempParentCategoryList { get; set; }

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

        public ObservableCollection<PCMArticleCategory> OrderCategoryList
        {
            get
            {
                return orderCategoryList;
            }

            set
            {
                orderCategoryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderCategoryList"));
            }
        }

        public PCMArticleCategory SelectedOrderCategory
        {
            get
            {
                return selectedOrderCategory;
            }

            set
            {
                selectedOrderCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrderCategory"));
            }
        }

        public ObservableCollection<PCMArticleCategory> ParentCategoryList
        {
            get
            {
                return parentCategoryList;
            }

            set
            {
                parentCategoryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentCategoryList"));
            }
        }

        public PCMArticleCategory SelectedParentCategory
        {
            get
            {
                return selectedParentCategory;
            }

            set
            {
                selectedParentCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParentCategory"));
            }
        }

        public PCMArticleCategory ClonedPCMArticleCategory
        {
            get
            {
                return clonedPCMArticleCategory;
            }

            set
            {
                clonedPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedPCMArticleCategory"));
            }
        }

        public int Article_count
        {
            get
            {
                return article_count;
            }

            set
            {
                article_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Article_count"));
            }
        }

        public uint? IdArticleCategory
        {
            get
            {
                return idArticleCategory;
            }

            set
            {
                idArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticleCategory"));
            }
        }

        public uint IdPCMArticleCategory
        {
            get
            {
                return idPCMArticleCategory;
            }

            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPCMArticleCategory"));
            }
        }

        public long IsLeaf
        {
            get
            {
                return isLeaf;
            }

            set
            {
                isLeaf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLeaf"));
            }
        }

        public string KeyName
        {
            get
            {
                return keyName;
            }

            set
            {
                keyName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KeyName"));
            }
        }

        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentName"));
            }
        }

        public ulong? Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Parent"));
            }
        }

        public uint Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Position"));
            }
        }

        public string NameWithArticleCount
        {
            get
            {
                return nameWithArticleCount;
            }

            set
            {
                nameWithArticleCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NameWithArticleCount"));
            }
        }


        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptCategoryCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CheckedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand CommandEditValueChanged { get; set; }


        #endregion

        #region Constructor

        public AddEditCategoryViewModel()
        {
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
            CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
            AcceptCategoryCommand = new DelegateCommand<object>(AcceptCategoryAction);

            CheckedCopyNameDescriptionCommand = new DelegateCommand<object>(CheckedCopyNameDescription);
            ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
            ChangeNameCommand = new DelegateCommand<EditValueChangingEventArgs>(SetNameToLanguage);
            ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
            CommandEditValueChanged = new DelegateCommand<object>(CommandEditValueChangedAction);

            IsCheckedCopyNameAndDescription = true;
        }

       


        #endregion

        #region Method

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

                AddLanguages();
                FillParentCategoryList();
                FillOrderCategoryList();

                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        public void EditInitCategory(PCMArticleCategory selectedCategory)
        {
            try
            {
                Init();
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);

                PCMArticleCategory temp = (PCMService.GetPCMArticleCategoryById(selectedCategory.IdPCMArticleCategory));
                ClonedPCMArticleCategory = (PCMArticleCategory)temp.Clone();

                IdArticleCategory = temp.IdArticleCategory;
                IdPCMArticleCategory = temp.IdPCMArticleCategory;
                Article_count = selectedCategory.Article_count;
                IsLeaf = temp.IsLeaf;
                KeyName = selectedCategory.KeyName;
                if (temp.Parent != null)
                    Parent = (ulong)temp.Parent;

                ParentName = selectedCategory.ParentName;
                Position = temp.Position;


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

                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                    Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyNameAndDescription = true;
                }
                else
                {
                    IsCheckedCopyNameAndDescription = false;
                }

                if (temp.Parent != null)
                {
                    SelectedParentCategory = ParentCategoryList.FirstOrDefault(x => x.IdPCMArticleCategory == temp.Parent);
                }
                if (SelectedParentCategory.IdPCMArticleCategory != 0)
                {
                    OrderCategoryList = new ObservableCollection<PCMArticleCategory>(ParentCategoryList.Where(a => a.Parent == SelectedParentCategory.IdPCMArticleCategory).ToList());
                    OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                }
                else
                {
                    if (OrderCategoryList == null)
                    {
                        OrderCategoryList = new ObservableCollection<PCMArticleCategory>();
                        OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                    }
                }

                OrderCategoryList = new ObservableCollection<PCMArticleCategory>(OrderCategoryList);
               // SelectedOrderCategory = OrderCategoryList.FirstOrDefault();
                SelectedOrderCategory = OrderCategoryList.FirstOrDefault(x => x.IdPCMArticleCategory == SelectedParentCategory.IdPCMArticleCategory);

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOrderCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList..."), category: Category.Info, priority: Priority.Low);

                //if (SelectedParentCategory.IdPCMArticleCategory != 0)
                //{
                //    OrderCategoryList = new ObservableCollection<PCMArticleCategory>(ParentCategoryList.Where(a => a.Parent == SelectedParentCategory.IdPCMArticleCategory).ToList());
                //    OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                //}
                //else
                //{
                //    if(OrderCategoryList == null)
                //    {
                //        OrderCategoryList = new ObservableCollection<PCMArticleCategory>();
                //        OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                //    }
                //}

                //OrderCategoryList = new ObservableCollection<PCMArticleCategory>(OrderCategoryList);
                //SelectedOrderCategory = OrderCategoryList.FirstOrDefault();
                //OrderCategoryList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                //UpdatePCMCategoryCount();
                //OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                //OrderCategoryList = new ObservableCollection<PCMArticleCategory>(OrderCategoryList.OrderBy(x => x.Position));
                //tempOrderCategoryList = new ObservableCollection<PCMArticleCategory>(OrderCategoryList);
                //SelectedOrderCategory = tempOrderCategoryList.FirstOrDefault();



                GeosApplication.Instance.Logger.Log(string.Format("Method FillOrderCategoryList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillParentCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillParentCategoryList..."), category: Category.Info, priority: Priority.Low);

                ParentCategoryList = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                UpdatePCMCategoryCountForParent();
                ParentCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                ParentCategoryList = new ObservableCollection<PCMArticleCategory>(ParentCategoryList.OrderBy(x => x.Position));
                tempParentCategoryList = new ObservableCollection<PCMArticleCategory>(ParentCategoryList);
                SelectedParentCategory = tempParentCategoryList.FirstOrDefault();



                GeosApplication.Instance.Logger.Log(string.Format("Method FillParentCategoryList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillParentCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillParentCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillParentCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptCategoryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptCategoryAction()..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameAndDescription == false && Name_en == null)
                {
                    LanguageSelected = Languages.FirstOrDefault(a => a.IdLanguage == 2);
                    Description = Description_en;
                    Name = Name_en;
                }

                allowValidation = true;

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
               // PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrderCategory"));

                if (error != null)
                {
                    return;
                }

                if (IsNew)
                {
                    NewArticleCategory = new PCMArticleCategory();

                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        NewArticleCategory.Name = Name;
                        NewArticleCategory.Name_es = Name;
                        NewArticleCategory.Name_fr = Name;
                        NewArticleCategory.Name_pt = Name;
                        NewArticleCategory.Name_ro = Name;
                        NewArticleCategory.Name_ru = Name;
                        NewArticleCategory.Name_zh = Name;

                        NewArticleCategory.Description = Description;
                        NewArticleCategory.Description_es = Description;
                        NewArticleCategory.Description_fr = Description;
                        NewArticleCategory.Description_pt = Description;
                        NewArticleCategory.Description_ro = Description;
                        NewArticleCategory.Description_ru = Description;
                        NewArticleCategory.Description_zh = Description;
                        NewArticleCategory.NameWithArticleCount = Name + " [0]";
                    }
                    else
                    {
                        NewArticleCategory.Name = Name_en;
                        NewArticleCategory.Name_es = Name_es;
                        NewArticleCategory.Name_fr = Name_fr;
                        NewArticleCategory.Name_pt = Name_pt;
                        NewArticleCategory.Name_ro = Name_ro;
                        NewArticleCategory.Name_ru = Name_ru;
                        NewArticleCategory.Name_zh = Name_zh;

                        NewArticleCategory.Description = Description_en;
                        NewArticleCategory.Description_es = Description_es;
                        NewArticleCategory.Description_fr = Description_fr;
                        NewArticleCategory.Description_pt = Description_pt;
                        NewArticleCategory.Description_ro = Description_ro;
                        NewArticleCategory.Description_ru = Description_ru;
                        NewArticleCategory.Description_zh = Description_zh;
                        NewArticleCategory.NameWithArticleCount = Name_en + " [0]";
                    }

                    if (SelectedOrderCategory.IdPCMArticleCategory > 0)
                    {
                        NewArticleCategory.Position = SelectedOrderCategory.Position;
                        NewArticleCategory.Parent = SelectedOrderCategory.Parent;
                        NewArticleCategory.IsLeaf = SelectedOrderCategory.IsLeaf;
                        NewArticleCategory.ParentName = SelectedOrderCategory.ParentName;
                    }

                    else
                    {


                        if (OrderCategoryList.Count > 1)
                        {

                            NewArticleCategory.Position = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().Position - 1;
                            NewArticleCategory.Parent = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().Parent == 0 ? null : OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().Parent;
                            NewArticleCategory.IsLeaf = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().IsLeaf;
                            NewArticleCategory.ParentName = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().ParentName;
                        }
                        else
                        {
                            if (OrderCategoryList.Count == 1)
                            {
                                NewArticleCategory.Position = 1;
                                NewArticleCategory.Parent = SelectedParentCategory.IdPCMArticleCategory;
                                NewArticleCategory.IsLeaf = IsLeaf;
                                NewArticleCategory.ParentName = SelectedParentCategory.ParentName;
                            }

                        }
                    }
                  

                 
                    NewArticleCategory.KeyName = "Group_0";
                   

                    NewArticleCategory.Article_count = 0;


                    NewArticleCategory.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    OrderCategoryList.Add(NewArticleCategory);
                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = OrderCategoryList.Where(a => a.Parent == NewArticleCategory.Parent && a.Name != "---").OrderBy(a => a.Position).ToList();
                    uint pos = 1;
                    uint Old_Position_set = 0;
                    if (NewArticleCategory.Parent == null)
                    {
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                        {
                            if (pcmArticleCategory.Position == SelectedOrderCategory.Position && pcmArticleCategory.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory && pcmArticleCategory.KeyName != NewArticleCategory.KeyName)
                            {
                                pos++;
                                Old_Position_set = pos;
                                OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos; });
                            }
                            else
                            {
                                if (pcmArticleCategory.KeyName == NewArticleCategory.KeyName)
                                {
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = Old_Position_set - 1; });
                                    pos++;
                                }
                                else
                                {
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                        {
                            if (pcmArticleCategory.Position == SelectedOrderCategory.Position && pcmArticleCategory.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory && pcmArticleCategory.KeyName != NewArticleCategory.KeyName)
                            {
                                pos++;
                                OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.Parent == pcmArticleCategory.Parent).ToList().ForEach(a => { a.Position = pos; });
                            }
                            else
                            {
                                if (pcmArticleCategory.KeyName == NewArticleCategory.KeyName)
                                {
                                    pos--;
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.Parent == pcmArticleCategory.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                    pos++;
                                }
                                else
                                {
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.Parent == pcmArticleCategory.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                }
                            }
                        }
                    }
                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_new = OrderCategoryList.Where(a => a.Parent == NewArticleCategory.Parent && a.IdPCMArticleCategory > 0).OrderBy(a => a.Position).ToList();

                    NewArticleCategory = PCMService.AddPCMArticleCategory(NewArticleCategory, pcmArticleCategory_ForSetOrder_new);

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    IsSave = true;
                    RequestClose(null, null);
                }
                else
                {
                    UpdatedItem = new PCMArticleCategory();
                    UpdatedItem.IdPCMArticleCategory = IdPCMArticleCategory;
                    UpdatedItem.IdArticleCategory = IdArticleCategory;

                    if (IsCheckedCopyNameAndDescription == true)
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

                    if (SelectedOrderCategory.IdPCMArticleCategory > 0)
                    {


                        if ((ClonedPCMArticleCategory.Parent == null && SelectedOrderCategory.Parent == null) || (ClonedPCMArticleCategory.Parent != null && SelectedOrderCategory.Parent != null))
                        {
                            UpdatedItem.Position = SelectedOrderCategory.Position;
                            UpdatedItem.Parent = SelectedOrderCategory.Parent == 0 ? null : SelectedOrderCategory.Parent;
                            UpdatedItem.IsLeaf = SelectedOrderCategory.IsLeaf;
                            UpdatedItem.ParentName = SelectedOrderCategory.ParentName;
                        }
                        else
                        {
                            UpdatedItem.Position = Position;
                            UpdatedItem.Parent = Parent == 0 ? null : Parent;
                            UpdatedItem.IsLeaf = IsLeaf;
                            UpdatedItem.ParentName = ParentName;
                        }
                    }
                    else
                    {
                        if (OrderCategoryList.Count > 1)
                        {
                            UpdatedItem.Position = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().Position - 1;
                            UpdatedItem.Parent = OrderCategoryList.Where(x =>x.IdPCMArticleCategory != 0).FirstOrDefault().Parent == 0 ? null : OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().Parent;
                            UpdatedItem.IsLeaf = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().IsLeaf;
                            UpdatedItem.ParentName = OrderCategoryList.Where(x => x.IdPCMArticleCategory != 0).FirstOrDefault().ParentName;
                        }
                        else
                        {
                            if (OrderCategoryList.Count == 1)
                            {
                                UpdatedItem.Position = 1;
                                UpdatedItem.Parent = SelectedParentCategory.IdPCMArticleCategory;
                                UpdatedItem.IsLeaf = IsLeaf;
                                UpdatedItem.ParentName = SelectedParentCategory.ParentName;
                            }
                        }
                    }
                    UpdatedItem.KeyName = KeyName;
                    UpdatedItem.NameWithArticleCount = Name_en + " [" + Article_count + "]";

                    UpdatedItem.Article_count = Article_count;

                    UpdatedItem.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UpdatedItem.ModificationDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_new = new List<PCMArticleCategory>();

                    if (SelectedOrderCategory.IdPCMArticleCategory != ClonedPCMArticleCategory.IdPCMArticleCategory)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = OrderCategoryList.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                        uint pos = 1;
                        uint status = 0;
                        if (ClonedPCMArticleCategory.Parent == null && SelectedOrderCategory.Parent == null)
                        {
                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                            {
                                if (status == 0 && pcmArticleCategory.Position == SelectedOrderCategory.Position && (pcmArticleCategory.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory || pcmArticleCategory.KeyName == UpdatedItem.KeyName))
                                {
                                    status = 1;
                                    UpdatedItem.Position = pos;
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                }
                                else
                                {
                                    if (pcmArticleCategory.KeyName != UpdatedItem.KeyName)
                                    {
                                        OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }
                            }
                            pcmArticleCategory_ForSetOrder_new = OrderCategoryList.Where(a => a.Parent == UpdatedItem.Parent && a.IdPCMArticleCategory != UpdatedItem.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
                        }
                        else if (ClonedPCMArticleCategory.Parent != null && SelectedOrderCategory.Parent != null)
                        {
                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                            {
                                if (status == 0 && pcmArticleCategory.Position == SelectedOrderCategory.Position && (pcmArticleCategory.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory || pcmArticleCategory.KeyName == UpdatedItem.KeyName))
                                {
                                    status = 1;
                                    UpdatedItem.Position = pos;
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                    OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.Parent == pcmArticleCategory.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                }
                                else
                                {
                                    if (pcmArticleCategory.KeyName != UpdatedItem.KeyName)
                                    {
                                        OrderCategoryList.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory && a.Parent == pcmArticleCategory.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }
                            }
                            pcmArticleCategory_ForSetOrder_new = OrderCategoryList.Where(a => a.Parent == UpdatedItem.Parent && a.IdPCMArticleCategory != UpdatedItem.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
                        }
                    }
                    IsSave = PCMService.IsUpdatePCMArticleCategory(UpdatedItem, pcmArticleCategory_ForSetOrder_new);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptCategoryAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptCategoryAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

                if (IsCheckedCopyNameAndDescription == false && LanguageSelected != null)
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

        private void CheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected != null)
                {
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void UpdatePCMCategoryCount()
        {
            foreach (PCMArticleCategory item in OrderCategoryList)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (OrderCategoryList.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstList = OrderCategoryList.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (OrderCategoryList.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondList = OrderCategoryList.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (OrderCategoryList.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdList = OrderCategoryList.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (OrderCategoryList.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthList = OrderCategoryList.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (OrderCategoryList.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthList = OrderCategoryList.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (OrderCategoryList.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthList = OrderCategoryList.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (OrderCategoryList.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhList = OrderCategoryList.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (OrderCategoryList.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthList = OrderCategoryList.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (OrderCategoryList.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethList = OrderCategoryList.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (OrderCategoryList.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthList = OrderCategoryList.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
                                                                                            foreach (PCMArticleCategory item10 in gettenthList)
                                                                                            {
                                                                                                if (item10.Article_count_original != null)
                                                                                                {
                                                                                                    count = count + item10.Article_count_original;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                item.Article_count = count;
                item.NameWithArticleCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.Article_count) + "]");
            }
        }

        private void UpdatePCMCategoryCountForParent()
        {
            foreach (PCMArticleCategory item in ParentCategoryList)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (ParentCategoryList.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstParentList = ParentCategoryList.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstParentList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (ParentCategoryList.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondParentList = ParentCategoryList.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondParentList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (ParentCategoryList.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdParentList = ParentCategoryList.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdParentList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (ParentCategoryList.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthParentList = ParentCategoryList.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthParentList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (ParentCategoryList.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthParentList = ParentCategoryList.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthParentList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (ParentCategoryList.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthParentList = ParentCategoryList.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthParentList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (ParentCategoryList.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhParentList = ParentCategoryList.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhParentList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (ParentCategoryList.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthParentList = ParentCategoryList.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthParentList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (ParentCategoryList.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethParentList = ParentCategoryList.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethParentList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (ParentCategoryList.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthParentList = ParentCategoryList.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
                                                                                            foreach (PCMArticleCategory item10 in gettenthParentList)
                                                                                            {
                                                                                                if (item10.Article_count_original != null)
                                                                                                {
                                                                                                    count = count + item10.Article_count_original;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                item.Article_count = count;
                item.NameWithArticleCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.Article_count) + "]");
            }
        }

        private void CommandEditValueChangedAction(object obj)
        {
            if (SelectedParentCategory.IdPCMArticleCategory != 0)
            {
                OrderCategoryList = new ObservableCollection<PCMArticleCategory>(ParentCategoryList.Where(a => a.Parent == SelectedParentCategory.IdPCMArticleCategory).ToList());
                OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
            }
            else
            {
                if (OrderCategoryList == null)
                {
                    OrderCategoryList = new ObservableCollection<PCMArticleCategory>();
                    OrderCategoryList.Insert(0, new PCMArticleCategory() { Name = "---", KeyName = "defaultCategory", IdPCMArticleCategory = 0 });
                }
            }

            if (IdPCMArticleCategory > 0 && ClonedPCMArticleCategory.Parent == SelectedParentCategory.IdPCMArticleCategory)
            {
                SelectedOrderCategory = OrderCategoryList.FirstOrDefault(a => a.IdPCMArticleCategory == IdPCMArticleCategory);
            }
            else
            {
                SelectedOrderCategory = OrderCategoryList.FirstOrDefault();
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
                   // me[BindableBase.GetPropertyName(() => SelectedOrderCategory)];

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
               // string order = BindableBase.GetPropertyName(() => SelectedOrderCategory);

                if (columnName == name)
                {
                    return AddEditCategoryValidation.GetErrorMessage(name, Name);
                }

                //if (columnName == order)
                //{
                //    return AddEditCategoryValidation.GetErrorMessage(order, SelectedOrderCategory);
                //}

                return null;
            }
        }



        #endregion

    }
}
