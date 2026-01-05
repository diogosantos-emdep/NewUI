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
using System.IO;
using Microsoft.Win32;
using System.Windows.Data;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.PCM.Common_Classes;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class ProductTypeArticleViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }



        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }



        #endregion // End Of Events 

        #region Declarations

        private ObservableCollection<Articles> articleList;
        private ObservableCollection<Articles> articleList_All;
        private Articles selectedArticle;

        private ObservableCollection<PCMArticleCategory> categoryMenulist;
        private PCMArticleCategory selectedCategory;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        private List<PCMArticleCategory> pcmArticleCategory;
        private ObservableCollection<PCMArticleCategory> clonedPCMArticleCategory;
        private PCMArticleCategory updatedItem;
        private bool isSave;
        private List<LookupValue> tempStatusList;
        private LookupValue selectedStatus;
        private List<LookupValue> statusList;
        private TableView view;
        private bool isArticleSave;
        private bool isAllSave;
        private List<PCMArticleLogEntry> changeLogsEntry;

        private bool confirmationYesNo { get; set; }

        #endregion

        #region Properties

        public Articles ArticleData { get; set; }

        public ObservableCollection<Articles> ArticleList
        {
            get
            {
                return articleList;
            }

            set
            {
                articleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList"));
            }
        }

        public ObservableCollection<Articles> ArticleList_All
        {
            get
            {
                return articleList_All;
            }

            set
            {
                articleList_All = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList_All"));
            }
        }

        public Articles SelectedArticle
        {
            get
            {
                return selectedArticle;
            }

            set
            {
                selectedArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticle"));
            }
        }

        public ObservableCollection<PCMArticleCategory> CategoryMenulist
        {
            get
            {
                return categoryMenulist;
            }

            set
            {
                categoryMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryMenulist"));
            }
        }

        public PCMArticleCategory SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                selectedCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCategory"));
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
        public List<PCMArticleCategory> PCMArticleCategory
        {
            get { return pcmArticleCategory; }
            set
            {
                pcmArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleCategory"));
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
        public ObservableCollection<PCMArticleCategory> ClonedPCMArticleCategory
        {
            get
            {
                return clonedPCMArticleCategory;
            }
            set
            {
                clonedPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedProductType"));
            }
        }

        public PCMArticleCategory UpdatedItem
        {
            get
            {
                return updatedItem;
            }

            set
            {
                updatedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedItem"));
            }
        }

        public bool ConfirmationYesNo
        {
            get
            {
                return confirmationYesNo;
            }
            set
            {
                confirmationYesNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfirmationYesNo"));
            }
        }

        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }

        public bool IsArticleSave
        {
            get { return isArticleSave; }
            set
            {
                isArticleSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleSave"));
            }
        }

        public bool IsAllSave
        {
            get { return isAllSave; }
            set
            {
                isAllSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllSave"));
            }
        }

        public List<PCMArticleLogEntry> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }

        #endregion

        #region ICommands
        public ICommand SelectCategoryCommand { get; set; }
        public ICommand ImportWarehouseItemsToPCMCommand { get; set; }
        public ICommand AddNewCategoryCommand { get; set; }
        public ICommand EditCategoryCommand { get; set; }
        public ICommand RefreshArticleViewCommand { get; set; }
        public ICommand EditArticleCommand { get; set; }
        public ICommand PrintArticleCommand { get; set; }
        public ICommand ExportArticleCommand { get; set; }
        public ICommand CommandOnDragRecordOverArticleCatagoriesGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropArticleCatagories { get; set; }
        public ICommand CommandTreeListViewDropRecordArticleCatagories { get; set; }
        public ICommand UpdateMultipleRowsArticleGridCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }

        public ICommand DeleteCategoryCommand { get; set; }


        #endregion

        #region Constructor

        public ProductTypeArticleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ProductTypeArticleViewModel ...", category: Category.Info, priority: Priority.Low);

                AddNewCategoryCommand = new DelegateCommand<object>(AddNewCategory);
                EditCategoryCommand = new DelegateCommand<object>(EditCategory);
                RefreshArticleViewCommand = new RelayCommand(new Action<object>(RefreshArticleView));

                EditArticleCommand = new RelayCommand(new Action<object>(EditArticleAction));
                PrintArticleCommand = new RelayCommand(new Action<object>(PrintArticle));
                ExportArticleCommand = new RelayCommand(new Action<object>(ExportArticle));
                CommandOnDragRecordOverArticleCatagoriesGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleCatagoriesGrid);
                CommandCompleteRecordDragDropArticleCatagories = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticleCatagories);
                CommandTreeListViewDropRecordArticleCatagories = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordArticleCategories);
                UpdateMultipleRowsArticleGridCommand = new DelegateCommand<object>(UpdateMultipleRowsArticleGridCommandAction);
                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);

                DeleteCategoryCommand = new DelegateCommand<object>(DeleteCategoryAction);

                GeosApplication.Instance.Logger.Log("Constructor ProductTypeArticleViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor ProductTypeArticleViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Method

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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

                SelectCategoryCommand = new DelegateCommand<object>(RetrieveArticlesByCategory);
                ImportWarehouseItemsToPCMCommand = new DelegateCommand<object>(ImportWarehouseItemsToPCM);

                FillArticleList();
                FillArticleCatagoryList();
                FillStatusList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteCategoryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCategoryAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteArticleCategoryMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SelectedCategory.Article_count == 0)
                    {
                        List<PCMArticleCategory> tempList = new List<Data.Common.PCM.PCMArticleCategory>();
                        List<string> temp = getIdsForRetriveArticlesByParentClick();
                        tempList.Add(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                        CategoryMenulist.Remove(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));

                        foreach (string id in temp)
                        {
                            tempList.Add(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
                            CategoryMenulist.Remove(CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
                        }
                        bool isDelete = PCMService.IsDeletePCMArticleCategory(tempList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CategoryDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Can not delete selected category."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }


                GeosApplication.Instance.Logger.Log("Method DeleteCategoryAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCategoryAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCategoryAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DeleteCategoryAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()..."), category: Category.Info, priority: Priority.Low);

                AddEditCategoryView addEditCategoryView = new AddEditCategoryView();
                AddEditCategoryViewModel addEditCategoryViewModel = new AddEditCategoryViewModel();
                EventHandler handle = delegate { addEditCategoryView.Close(); };
                addEditCategoryViewModel.RequestClose += handle;
                addEditCategoryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCategoryHeader").ToString();
                addEditCategoryViewModel.IsNew = true;
                addEditCategoryViewModel.Init();
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();

                if (addEditCategoryViewModel.IsSave)
                {

                    addEditCategoryViewModel.OrderCategoryList.Where(a => a.IdPCMArticleCategory == 0).ToList().ForEach(a => { a.IdPCMArticleCategory = addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory; });

                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(addEditCategoryViewModel.OrderCategoryList.Where(a => a.Name != "---"));

                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    CategoryMenulist.Insert(0, articleCategories);

                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));

                    SelectedCategory = CategoryMenulist.Where(a => a.IdPCMArticleCategory == addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory).FirstOrDefault();

                    if (SelectedCategory != null)
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(a => a.PcmArticleCategory.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                    }
                    else
                    {
                        SelectedCategory = CategoryMenulist.FirstOrDefault();
                    }

                    FillArticleCatagoryList();
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()..."), category: Category.Info, priority: Priority.Low);
                if (SelectedCategory.Name == "All" && SelectedCategory.KeyName == "Group_All")
                {
                    return;
                }
                AddEditCategoryView addEditCategoryView = new AddEditCategoryView();
                AddEditCategoryViewModel addEditCategoryViewModel = new AddEditCategoryViewModel();
                EventHandler handle = delegate { addEditCategoryView.Close(); };
                addEditCategoryViewModel.RequestClose += handle;
                addEditCategoryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCategoryHeader").ToString();
                addEditCategoryViewModel.IsNew = false;
                addEditCategoryViewModel.EditInitCategory(SelectedCategory);
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();
                PCMArticleCategory selected_Cat=SelectedCategory;
                if (addEditCategoryViewModel.IsSave)
                {
                    //if (SelectedCategory.Parent != addEditCategoryViewModel.UpdatedItem.Parent)
                    //{
                    //    addEditCategoryViewModel.OrderCategoryList.Where(a => a.IdPCMArticleCategory == addEditCategoryViewModel.UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a =>
                    //    {
                    //        a.Name = addEditCategoryViewModel.UpdatedItem.Name;
                    //        a.NameWithArticleCount = addEditCategoryViewModel.UpdatedItem.Name + " [" + a.Article_count.ToString() + "]";
                    //        a.Parent = addEditCategoryViewModel.UpdatedItem.Parent;
                    //        a.ParentName = addEditCategoryViewModel.UpdatedItem.ParentName;
                    //        a.Position = addEditCategoryViewModel.UpdatedItem.Position;
                    //    });
                    //}
                    //else
                    //{
                    //    addEditCategoryViewModel.OrderCategoryList.Where(a => a.IdPCMArticleCategory == addEditCategoryViewModel.UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a =>
                    //    {
                    //        a.Name = addEditCategoryViewModel.UpdatedItem.Name;
                    //        a.NameWithArticleCount = addEditCategoryViewModel.UpdatedItem.Name + " [" + a.Article_count.ToString() + "]";
                    //        a.Position = addEditCategoryViewModel.UpdatedItem.Position;
                    //    });
                    //}
                    //CategoryMenulist = new ObservableCollection<PCMArticleCategory>(addEditCategoryViewModel.OrderCategoryList.Where(a => a.Name != "---"));

                    //PCMArticleCategory articleCategories = new PCMArticleCategory();
                    //articleCategories.Name = "All";
                    //articleCategories.KeyName = "Group_All";
                    //articleCategories.Article_count = ArticleList_All.Count();
                    //articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    //CategoryMenulist.Insert(0, articleCategories);

                    //CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));

                    //SelectedCategory = CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == addEditCategoryViewModel.UpdatedItem.IdPCMArticleCategory);



                    //if (SelectedCategory.Parent != addEditCategoryViewModel.Parent)
                    //{
                    //    CategoryMenulist.Where(a => a.KeyName == SelectedCategory.ParentName).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(SelectedCategory.Article_count + a.Article_count) + "]"); a.Article_count = Convert.ToInt32(SelectedCategory.Article_count + a.Article_count); });
                    //    CategoryMenulist.Where(a => a.KeyName == addEditCategoryViewModel.ParentName).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(a.Article_count - SelectedCategory.Article_count) + "]"); a.Article_count = Convert.ToInt32(a.Article_count - SelectedCategory.Article_count); });
                    //}

                    //if (SelectedCategory != null)
                    //{
                    //    ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(a => a.PcmArticleCategory.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                    //}
                    //else
                    //{
                    //    SelectedCategory = CategoryMenulist.FirstOrDefault();
                    //}

                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                    UpdatePCMCategoryCount();
                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    CategoryMenulist.Insert(0, articleCategories);
                    CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                    SelectedCategory = selected_Cat;

                    ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;


                    if (SelectedCategory == null || SelectedCategory.Name == "All")
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                    }
                    else
                    {
                        string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                        string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method AddNewCategory()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshArticleView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshArticleView()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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

                ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2060());
                SelectedArticle = ArticleList.FirstOrDefault();

                detailView.SearchString = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshArticleView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshArticleView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCatelogueView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCatelogueView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditArticleAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()..."), category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                TableView detailView = (TableView)obj;
                EditPCMArticleView editPCMArticleView = new EditPCMArticleView();
                EditPCMArticleViewModel editPCMArticleViewModel = new EditPCMArticleViewModel();
                EventHandler handle = delegate { editPCMArticleView.Close(); };
                editPCMArticleViewModel.RequestClose += handle;
                editPCMArticleViewModel.IsNew = false;
                editPCMArticleViewModel.EditInit(SelectedArticle);
                editPCMArticleView.DataContext = editPCMArticleViewModel;
                //var ownerInfo = (detailView as FrameworkElement);
                //editPCMArticleView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                editPCMArticleView.ShowDialog();

                PCMArticleCategory selected_Cat = SelectedCategory;

                if (editPCMArticleViewModel.IsSave)
                {
                    SelectedArticle.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                    SelectedArticle.IdPCMStatus = editPCMArticleViewModel.UpdatedArticle.IdPCMStatus;
                    SelectedArticle.PcmArticleCategory.Name = editPCMArticleViewModel.SelectedCategory.Name;
                    SelectedArticle.PCMStatus = editPCMArticleViewModel.SelectedStatus.Value;
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();

                    ArticleList_All.Where(a => a.IdArticle == SelectedArticle.IdArticle).ToList().ForEach(a =>
                    {
                        a.PcmArticleCategory.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                        a.IdPCMArticleCategory = editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory;
                    });

                    if (editPCMArticleViewModel.ClonedArticle.IdPCMArticleCategory != editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory)
                    {
                        //PCMArticleCategory pcmArticleCategory_old = CategoryMenulist.Where(a => a.IdPCMArticleCategory == editPCMArticleViewModel.ClonedArticle.IdPCMArticleCategory).FirstOrDefault();
                        //PCMArticleCategory pcmArticleCategory = CategoryMenulist.Where(a => a.IdPCMArticleCategory == editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory).FirstOrDefault();

                        //if (pcmArticleCategory.Parent == null)
                        //{
                        //    CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_old.IdPCMArticleCategory).ToList().ForEach(a =>
                        //    {
                        //        a.Article_count = a.Article_count - 1;
                        //        a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //    });

                        //    if (pcmArticleCategory_old.KeyName != pcmArticleCategory.KeyName)
                        //    {
                        //        CategoryMenulist.Where(a => a.KeyName == pcmArticleCategory_old.ParentName).ToList().ForEach(a =>
                        //        {
                        //            a.Article_count = a.Article_count - 1;
                        //            a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //        });

                        //        CategoryMenulist.Where(a => a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a =>
                        //        {
                        //            a.Article_count = a.Article_count + 1;
                        //            a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //        });
                        //    }
                        //}
                        //else
                        //{
                        //    CategoryMenulist.Where(a => a.IdPCMArticleCategory == editPCMArticleViewModel.UpdatedArticle.IdPCMArticleCategory).ToList().ForEach(a =>
                        //    {
                        //        a.Article_count = a.Article_count + 1;
                        //        a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //    });

                        //    CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_old.IdPCMArticleCategory).ToList().ForEach(a =>
                        //    {
                        //        a.Article_count = a.Article_count - 1;
                        //        a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //    });

                        //    if (pcmArticleCategory_old.Parent != pcmArticleCategory.Parent)
                        //    {
                        //        CategoryMenulist.Where(a => a.KeyName == pcmArticleCategory_old.ParentName).ToList().ForEach(a =>
                        //        {
                        //            a.Article_count = a.Article_count - 1;
                        //            a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //        });

                        //        CategoryMenulist.Where(a => a.KeyName == pcmArticleCategory.ParentName).ToList().ForEach(a =>
                        //        {
                        //            a.Article_count = a.Article_count + 1;
                        //            a.NameWithArticleCount = a.Name + " [" + a.Article_count.ToString() + "]";
                        //        });
                        //    }
                        //}
                        CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                        UpdatePCMCategoryCount();
                        PCMArticleCategory articleCategories = new PCMArticleCategory();
                        articleCategories.Name = "All";
                        articleCategories.KeyName = "Group_All";
                        articleCategories.Article_count = ArticleList_All.Count();
                        articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                        CategoryMenulist.Insert(0, articleCategories);
                        CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                        SelectedCategory = selected_Cat;

                        ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;


                        if (SelectedCategory == null || SelectedCategory.Name == "All")
                        {
                            ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                        }
                        else
                        {
                            string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                            string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                            ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                        }
                        //if (SelectedCategory.Parent != null)
                        //{
                        //    ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(a => a.PcmArticleCategory.IdPCMArticleCategory == SelectedCategory.IdPCMArticleCategory));
                        //}
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log(string.Format("Method EditArticleAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditArticleAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveArticlesByCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveArticlesByCategory()...", category: Category.Info, priority: Priority.Low);
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
                if (SelectedCategory == null || SelectedCategory.Name == "All")
                {
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                }
                else
                {
                    string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                    string[] ids = (Concat_ChildArticles + "," + SelectedCategory.IdPCMArticleCategory).Split(',');
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveArticlesByCategory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveArticlesByCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList()...", category: Category.Info, priority: Priority.Low);

                ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2060());
                SelectedArticle = new Articles();
                SelectedArticle = ArticleList.FirstOrDefault();

                ArticleList_All = new ObservableCollection<Articles>(ArticleList);
                GeosApplication.Instance.Logger.Log("Method FillArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillArticleCatagoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);

                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                UpdatePCMCategoryCount();
                PCMArticleCategory articleCategories = new PCMArticleCategory();
                articleCategories.Name = "All";
                articleCategories.KeyName = "Group_All";
                articleCategories.Article_count = ArticleList_All.Count();
                articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                CategoryMenulist.Insert(0, articleCategories);
                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                SelectedCategory = CategoryMenulist.FirstOrDefault();
                ArticleList = new ObservableCollection<Articles>(ArticleList_All);

                SelectedArticle = ArticleList.FirstOrDefault();
                ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleCatagoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleCatagoryList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticleCatagoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ImportWarehouseItemsToPCM(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()...", category: Category.Info, priority: Priority.Low);

                view = ProductTypeArticleViewMultipleCellEditHelper.Viewtableview;
                if (ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsArticleGridCommandAction(ProductTypeArticleViewMultipleCellEditHelper.Viewtableview);
                    }
                    ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                TableView detailView = (TableView)obj;
                ImportWarehouseItemToPCMView importWarehouseItemToPCMView = new ImportWarehouseItemToPCMView();
                ImportWarehouseItemToPCMModel importWarehouseItemToPCMModel = new ImportWarehouseItemToPCMModel();
                EventHandler handle = delegate { importWarehouseItemToPCMView.Close(); };
                importWarehouseItemToPCMModel.RequestClose += handle;
                importWarehouseItemToPCMModel.Init();
                importWarehouseItemToPCMView.DataContext = importWarehouseItemToPCMModel;
                var ownerInfo = (detailView as FrameworkElement);
                importWarehouseItemToPCMView.Owner = Window.GetWindow(ownerInfo);
                importWarehouseItemToPCMView.ShowDialog();

                if (importWarehouseItemToPCMModel.IsSaveChanges)
                {
                    FillArticleList();
                }
                FillArticleCatagoryList();

                GeosApplication.Instance.Logger.Log("Method ImportWarehouseItemsToPCM()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ImportWarehouseItemsToPCM()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintArticle(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ArticlesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ArticlesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportArticle(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportModules()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Articles List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportModules()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportModules()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void OnDragRecordOverArticleCatagoriesGrid(DragRecordOverEventArgs e)
        {
            try
            {
                //[001]
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()...", category: Category.Info, priority: Priority.Low);

                if (e.DropPosition == DropPosition.Inside)//&& typeof(Detections).IsAssignableFrom(e.GetRecordType())
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }

                //

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverDetectionsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void CompleteRecordDragDropArticleCatagories(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                //[001]
                if (ConfirmationYesNo == true)
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;


                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropDetections() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        /// 
        private void TreeListViewDropRecordArticleCategories(DropRecordEventArgs e)
        {
            try
            {
                //[001]
                ProductTypeArticleView ptav = new ProductTypeArticleView();
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["SaveArticleCatagory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    ConfirmationYesNo = true;

                    uint tmpIdPCMArticleCategoryIndexOf;
                    bool isParentChange = false;
                    List<PCMArticleCategory> PCMArticleCategory_ParentChange = new List<PCMArticleCategory>();
                    PCMArticleCategory = new List<PCMArticleCategory>();


                    var data1 = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    List<PCMArticleCategory> newRecords1 = data1.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName, Position = x.Position }).ToList();

                    PCMArticleCategory SelectedOrderCategory = (PCMArticleCategory)e.TargetRecord;

                    List<PCMArticleCategory> lstUpdateItem = new List<PCMArticleCategory>();
                    lstUpdateItem = newRecords1.ToList();

                    UpdatedItem = newRecords1.FirstOrDefault();

                    uint pos = 1;
                    uint status = 0;

                    #region OneParentoAnaterParent
                    if (UpdatedItem.Parent != null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

                        if (ClonedPCMArticleCategory.Count > 0)
                        {
                            if (e.DropPosition == DropPosition.Inside)
                            {
                                ulong? UpdatedItemParent = UpdatedItem.Parent;
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_Selectorder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).ToList();
                                List<uint> indexCollection = new List<uint>();
                                pos = 1;
                                foreach (var updateArt in lstUpdateItem)
                                {
                                    indexCollection.Add(updateArt.IdPCMArticleCategory);

                                }
                                pos = 1;
                                foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
                                {
                                    if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
                                    {
                                        CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }

                                pos = 1;
                                foreach (var updateArt in lstUpdateItem)
                                {
                                    CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = SelectedOrderCategory.KeyName; });
                                    PCMArticleCategory.Add(CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                }

                                foreach (var updateArt in pcmArticleCategory_ForSetOrder_Selectorder)
                                {

                                    PCMArticleCategory.Add(CategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                }
                                if (PCMArticleCategory.Count > 0)
                                {
                                    pos = 1;
                                    foreach (PCMArticleCategory updateArt in PCMArticleCategory)
                                    {
                                        updateArt.Position = pos++;
                                    }
                                }


                                if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
                                {
                                    PCMArticleCategory_ParentChange = CategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                }



                            }
                            else
                            {


                                if (e.DropPosition != DropPosition.Inside && (SelectedOrderCategory.Parent == null || UpdatedItem.Parent != SelectedOrderCategory.Parent))
                                {

                                    ulong? UpdatedItemParent = UpdatedItem.Parent;
                                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = CategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                                    List<uint> indexCollection = new List<uint>();
                                    pos = 1;
                                    foreach (var updateArt in lstUpdateItem)
                                    {
                                        indexCollection.Add(updateArt.IdPCMArticleCategory);

                                    }
                                    pos = 1;
                                    foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
                                    {
                                        if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
                                        {
                                            CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                        }
                                    }

                                    if (SelectedOrderCategory.Parent == null || (e.DropPosition == DropPosition.Inside && SelectedOrderCategory.Parent != null))
                                    {

                                    }
                                    else
                                    {
                                        pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();

                                        switch (e.DropPosition)
                                        {
                                            case DropPosition.Append:
                                                break;
                                            case DropPosition.Before:
                                                {
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {

                                                        updateArt.Parent = SelectedOrderCategory.Parent;
                                                        updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                        updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                        pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                    }
                                                }
                                                break;
                                            case DropPosition.After:
                                                {
                                                    tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                    foreach (var updateArt in lstUpdateItem)
                                                    {

                                                        updateArt.Parent = SelectedOrderCategory.Parent;
                                                        updateArt.ParentName = SelectedOrderCategory.ParentName;
                                                        updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                        pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                        tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                    }
                                                }
                                                break;
                                            case DropPosition.Inside:
                                                {

                                                }
                                                break;
                                            default:
                                                break;
                                        }


                                    }


                                    pos = 1;
                                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                                    {
                                        CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
                                    }

                                    if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
                                    {
                                        PCMArticleCategory_ParentChange = CategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                    }
                                }
                                else
                                {
                                    pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent

                                            ).OrderBy(a => a.Position).ToList();
                                    if (UpdatedItem.Parent == SelectedOrderCategory.Parent)
                                    {
                                        if (SelectedOrderCategory.Position > UpdatedItem.Position)
                                        {
                                            switch (e.DropPosition)
                                            {
                                                case DropPosition.Append:
                                                    break;
                                                case DropPosition.Before:
                                                    {

                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                                        }


                                                    }
                                                    break;
                                                case DropPosition.After:
                                                    {
                                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                        }

                                                    }
                                                    break;
                                                case DropPosition.Inside:
                                                    {


                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }


                                        }
                                        else
                                        {
                                            switch (e.DropPosition)
                                            {
                                                case DropPosition.Append:
                                                    break;
                                                case DropPosition.Before:
                                                    {

                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                                        }


                                                    }
                                                    break;
                                                case DropPosition.After:
                                                    {
                                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                                        foreach (var updateArt in lstUpdateItem)
                                                        {
                                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                                        }


                                                    }
                                                    break;
                                                case DropPosition.Inside:
                                                    {

                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        if (e.DropPosition != DropPosition.Inside)
                                        {
                                            pos = 1;
                                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                                            {
                                                CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    #endregion 
                    else if (SelectedOrderCategory.Parent == null && UpdatedItem.Parent == null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = CategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();

                        if (SelectedOrderCategory.Position > UpdatedItem.Position)
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;
                                case DropPosition.Before:
                                    {

                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                        }


                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                        }

                                    }
                                    break;
                                case DropPosition.Inside:
                                    {

                                    }
                                    break;
                                default:
                                    break;
                            }


                        }
                        else
                        {
                            switch (e.DropPosition)
                            {
                                case DropPosition.Append:
                                    break;
                                case DropPosition.Before:
                                    {

                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

                                        }

                                    }
                                    break;
                                case DropPosition.After:
                                    {
                                        tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
                                        foreach (var updateArt in lstUpdateItem)
                                        {
                                            pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
                                            updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
                                            pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
                                            tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
                                        }


                                    }
                                    break;
                                case DropPosition.Inside:
                                    {


                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (e.DropPosition != DropPosition.Inside)
                        {
                            pos = 1;
                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
                            {
                                CategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                            }
                        }
                    }
                    if (UpdatedItem.Parent != null)
                    {
                        if (e.DropPosition == DropPosition.Inside)
                        {
                            PCMArticleCategory.OrderBy(a => a.Position);
                            //PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
                        }
                        else
                        {
                            PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
                        }


                        if (PCMArticleCategory_ParentChange.Count > 0)
                        {
                            PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
                        }
                    }
                    else if (UpdatedItem.Parent == null && SelectedOrderCategory.Parent == null)
                    {
                        PCMArticleCategory = CategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
                        if (PCMArticleCategory_ParentChange.Count > 0)
                        {
                            PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
                        }
                    }

                    if (e.IsFromOutside == false && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
                    {
                        if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
                        {
                            var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                            List<PCMArticleCategory> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName }).ToList();

                            PCMArticleCategory temp = newRecords.FirstOrDefault();

                            PCMArticleCategory target_record = (PCMArticleCategory)e.TargetRecord;
                            if ((temp.Parent == null && target_record.Parent == null) || (temp.Parent != null && target_record.Parent != null) || target_record.Parent == null) // && temp.Parent == target_record.Parent
                            {
                                e.Effects = DragDropEffects.Move;
                                e.Handled = true;

                            }
                            else
                            {
                                e.Effects = DragDropEffects.None;
                                e.Handled = true;
                            }
                        }
                    }

                    ////save data from darag and dropped data.
                    if (PCMArticleCategory != null)
                    {
                        if (PCMArticleCategory.Count > 0)
                        {

                            IsSave = PCMService.IsUpdatedPCMArticleCategoryOrder(PCMArticleCategory, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            FillArticleCatagoryList();


                            //  RequestClose(null, null);
                            GeosApplication.Instance.Logger.Log(string.Format("Method AcceptArticleCategoryAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                        }
                    }

                }
                else
                {
                    ConfirmationYesNo = false;

                }
                //
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private List<string> getIdsForRetriveArticlesByParentClick()
        {
            List<string> ids = new List<string>();
            if (CategoryMenulist.Any(a => a.Parent == SelectedCategory.IdPCMArticleCategory))
            {
                List<PCMArticleCategory> getFirstList = CategoryMenulist.Where(a => a.Parent == SelectedCategory.IdPCMArticleCategory).ToList();
                foreach (PCMArticleCategory item1 in getFirstList)
                {
                    if (item1.Article_count_original != null)
                    {
                        ids.Add(item1.IdPCMArticleCategory.ToString());
                    }
                    if (CategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                    {
                        List<PCMArticleCategory> getSecondList = CategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                        foreach (PCMArticleCategory item2 in getSecondList)
                        {
                            if (item2.Article_count_original != null)
                            {
                                ids.Add(item2.IdPCMArticleCategory.ToString());
                            }
                            if (CategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                            {
                                List<PCMArticleCategory> getThirdList = CategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                foreach (PCMArticleCategory item3 in getThirdList)
                                {
                                    if (item3.Article_count_original != null)
                                    {
                                        ids.Add(item3.IdPCMArticleCategory.ToString());
                                    }
                                    if (CategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                    {
                                        List<PCMArticleCategory> getForthList = CategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                        foreach (PCMArticleCategory item4 in getForthList)
                                        {
                                            if (item4.Article_count_original != null)
                                            {
                                                ids.Add(item4.IdPCMArticleCategory.ToString());
                                            }
                                            if (CategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                            {
                                                List<PCMArticleCategory> getFifthList = CategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                foreach (PCMArticleCategory item5 in getFifthList)
                                                {
                                                    if (item5.Article_count_original != null)
                                                    {
                                                        ids.Add(item5.IdPCMArticleCategory.ToString());
                                                    }
                                                    if (CategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                    {
                                                        List<PCMArticleCategory> getSixthList = CategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                        foreach (PCMArticleCategory item6 in getSixthList)
                                                        {
                                                            if (item6.Article_count_original != null)
                                                            {
                                                                ids.Add(item6.IdPCMArticleCategory.ToString());
                                                            }
                                                            if (CategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                            {
                                                                List<PCMArticleCategory> getSeventhList = CategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                foreach (PCMArticleCategory item7 in getSeventhList)
                                                                {
                                                                    if (item7.Article_count_original != null)
                                                                    {
                                                                        ids.Add(item7.IdPCMArticleCategory.ToString());
                                                                    }
                                                                    if (CategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                    {
                                                                        List<PCMArticleCategory> getEightthList = CategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                        foreach (PCMArticleCategory item8 in getEightthList)
                                                                        {
                                                                            if (item8.Article_count_original != null)
                                                                            {
                                                                                ids.Add(item8.IdPCMArticleCategory.ToString());
                                                                            }
                                                                            if (CategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                            {
                                                                                List<PCMArticleCategory> getNinethList = CategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                foreach (PCMArticleCategory item9 in getNinethList)
                                                                                {
                                                                                    if (item9.Article_count_original != null)
                                                                                    {
                                                                                        ids.Add(item9.IdPCMArticleCategory.ToString());
                                                                                    }
                                                                                    if (CategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                    {
                                                                                        List<PCMArticleCategory> gettenthList = CategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
                                                                                        foreach (PCMArticleCategory item10 in gettenthList)
                                                                                        {
                                                                                            if (item10.Article_count_original != null)
                                                                                            {
                                                                                                ids.Add(item10.IdPCMArticleCategory.ToString());
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
            return ids;
        }
        private void UpdatePCMCategoryCount()
        {
            foreach (PCMArticleCategory item in CategoryMenulist)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (CategoryMenulist.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstList = CategoryMenulist.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (CategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondList = CategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (CategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdList = CategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (CategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthList = CategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (CategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthList = CategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (CategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthList = CategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (CategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhList = CategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (CategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthList = CategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (CategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethList = CategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (CategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthList = CategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
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


        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);

                tempStatusList = PCMService.GetLookupValues(45).ToList();
                StatusList = new List<LookupValue>(tempStatusList);
                //StatusList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //SelectedStatus = StatusList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to save data of multiple rows on main Article Grid
        /// [GEOS2-2574][avpawar][Improvement in the categories manager [item 1 - Manage SubCategories] [#PCM49]]
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateMultipleRowsArticleGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                view = obj as TableView;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                IsArticleSave = false;
                IsAllSave = false;

                int? cellIdStatus = null;

                cellIdStatus = StatusList.Where(sl => sl.IdLookupValue == ProductTypeArticleViewMultipleCellEditHelper.IdStatus).Select(u => u.IdLookupValue).FirstOrDefault();

                Articles[] foundRow = ArticleList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();

                foreach (Articles item in foundRow)
                {
                    Articles AI = item;
                    ChangeLogsEntry = new List<PCMArticleLogEntry>();
                    Articles _Articles = new Articles();
                    _Articles.IdArticle = item.IdArticle;
                    _Articles.IdPCMArticleCategory = item.IdPCMArticleCategory;
                    _Articles.IdPCMStatus = item.IdPCMStatus;
                    _Articles.PCMArticleLogEntiryList = new List<PCMArticleLogEntry>();

                    Articles TempArticleList = new Articles();
                    TempArticleList = PCMService.GetArticleByIdArticle(_Articles.IdArticle);
                   
                    if (cellIdStatus.HasValue && cellIdStatus != 0)
                    {
                        int StatusListIdCurrent = StatusList.Where(sl => sl.IdLookupValue == AI.IdPCMStatus).Select(u => u.IdLookupValue).FirstOrDefault();

                        if (TempArticleList.IdPCMStatus != StatusListIdCurrent)
                        {
                            string StatusNameOld = string.Empty;
                            string StatusNameNew = string.Empty;

                            if (_Articles.IdPCMStatus != null)
                            {
                                int IdStatusNameOld = StatusList.Where(sl => sl.IdLookupValue == TempArticleList.IdPCMStatus).Select(u => u.IdLookupValue).SingleOrDefault();
                                StatusNameOld = item.PCMStatus;     //StatusList.Where(sl => sl.IdLookupValue == TempArticleList[0].IdPCMStatus).Select(u => u.Value).SingleOrDefault(); //item.PCMStatus;
                            }

                            _Articles.IdPCMStatus = StatusListIdCurrent;

                            if (_Articles.IdPCMStatus != null)
                            {
                                int IdStatusNameNew = StatusList.Where(sl => sl.IdLookupValue == _Articles.IdPCMStatus).Select(u => u.IdLookupValue).SingleOrDefault();
                                StatusNameNew = StatusList.Where(sl => sl.IdLookupValue == _Articles.IdPCMStatus).Select(u => u.Value).SingleOrDefault();
                            }

                            ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle = (uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), StatusNameOld, StatusNameNew) });
                        }

                        else
                        {
                            // _Articles.IdPCMStatus = StatusListIdCurrent;
                            //string StatusNameOld = item.PCMStatus;
                            //string StatusNameNew = StatusList.Where(sl => sl.IdLookupValue == _Articles.IdPCMStatus).Select(u => u.Value).SingleOrDefault();
                            //ChangeLogsEntry.Add(new PCMArticleLogEntry() { IdArticle =(uint)_Articles.IdArticle, IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ProductTypeChangeLogStatus").ToString(), StatusNameOld, StatusNameNew) });
                            //_Articles.IdPCMStatus = TempArticleList[0].IdPCMStatus;
                            _Articles.IdPCMStatus = TempArticleList.IdPCMStatus;
                        }
                    }
                    else
                    {
                        _Articles.IdPCMStatus = TempArticleList.IdPCMStatus;
                    }

                    if (ChangeLogsEntry != null)
                        _Articles.PCMArticleLogEntiryList.AddRange(ChangeLogsEntry);
                    _Articles.TransactionOperation = ModelBase.TransactionOperations.Update;
                    _Articles.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;

                    IsArticleSave = PCMService.IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(_Articles.IdPCMArticleCategory, _Articles);
                    
                    item.IsUpdatedRow = false;
                }

                if (IsArticleSave)
                    IsAllSave = true;
                else
                    IsAllSave = false;

                if (IsAllSave)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsArticleGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to add checked list for column StatusList
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "IdPCMStatus")
                {
                    List<object> filterItems = new List<object>();

                    foreach (Articles item in ArticleList)
                    {
                        string StatusValue = item.PCMStatus;

                        if (StatusValue == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == StatusValue))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = StatusValue;
                            customComboBoxItem.EditValue = item.IdPCMStatus;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopup()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }


    #endregion

}
