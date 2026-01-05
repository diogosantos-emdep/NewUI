using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PCM.Views;
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

namespace Emdep.Geos.Modules.PCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4874]
    public class ArticlesCategoriesMappingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
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
        double tabHeight;
        private double gridHeight;
        private int windowHeight;
        bool isWMSExpand = true;
        bool isPCMExpand = true;    
        private bool isExpand;
        private ObservableCollection<PCMArticleCategory> articleCategoryMenuList;
        private ObservableCollection<ArticleCategory> warehouseCategoryMenuList;    
        private ObservableCollection<ArticleCategorieMapping> articleCategoryMappingList;
        List<ArticleCategory> articleRecords = new List<ArticleCategory>();
        PCMArticleCategory pCMArt = new PCMArticleCategory();
        public string WMSParent = string.Empty;
        public string PCMParent = string.Empty;
        List<ArticleCategorieMapping> updatedList;
        ObservableCollection<PCMArticleCategory> clonedPCMArticleCategory;
        #endregion

        #region Properties
        public double TabHeight
        {
            get
            {
                return tabHeight;
            }

            set
            {
                tabHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TabHeight"));
            }
        }
        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }

            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }
        public double GridHeight
        {
            get
            {
                return gridHeight;
            }

            set
            {
                gridHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridHeight"));
            }
        }
        public List<ArticleCategory> ArticleRecords
        {
            get
            {
                return articleRecords;
            }

            set
            {
                articleRecords = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleRecords"));
            }
        }

        public PCMArticleCategory PCMArt
        {
            get
            {
                return pCMArt;
            }

            set
            {
                pCMArt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArt"));
            }
        }      

        public bool IsWMSExpand
        {
            get
            {
                return isWMSExpand;
            }

            set
            {
                isWMSExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWMSExpand"));
            }
        }
        public bool IsPCMExpand
        {
            get
            {
                return isPCMExpand;
            }

            set
            {
                isPCMExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPCMExpand"));
            }
        }
        public ObservableCollection<ArticleCategorieMapping> ArticleCategoryMappingList
        {
            get { return articleCategoryMappingList; }
            set
            {
                articleCategoryMappingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleCategoryMappingList"));
            }
        }   
        public List<ArticleCategorieMapping> UpdatedList
        {
            get { return updatedList; }
            set
            {
                updatedList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedList"));
            }
        }
        public bool IsExpand
        {
            get { return isExpand; }
            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpand"));
            }
        }
        public ObservableCollection<PCMArticleCategory> CategoryMenulist
        {
            get { return articleCategoryMenuList; }
            set
            {
                articleCategoryMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryMenulist"));
            }
        }              
        public ObservableCollection<PCMArticleCategory> ClonedPCMArticleCategory
        {
            get { return clonedPCMArticleCategory; }
            set
            {
                clonedPCMArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedPCMArticleCategory"));
            }
        }        
        public ObservableCollection<ArticleCategory> WarehouseCategoryMenuList
        {
            get { return warehouseCategoryMenuList; }
            set
            {
                warehouseCategoryMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseCategoryMenuList"));
            }
        }
        #endregion

        #region Public ICommands
        public ICommand CommandCompleteRecordDragDropPCMArticle { get; set; }
        public ICommand CommandOnDragRecordOverPCMArticle { get; set; }        
        public ICommand ExpandAndCollapsPCMCategoriesCommand { get; set; }
        public ICommand ExpandAndCollapsWMSCategoriesCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }        
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand DeleteDetectionCommand { get; set; }
        public ICommand CommandDropRecordPCMArticle { get; set; }
        #endregion

        #region Constructor
        public ArticlesCategoriesMappingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticlesCategoriesMappingViewModel()...", category: Category.Info, priority: Priority.Low);
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
                IsExpand = true;
                ExpandAndCollapsPCMCategoriesCommand = new DelegateCommand<object>(ExpandAndCollapsPCMCategoriesCommandAction);
                ExpandAndCollapsWMSCategoriesCommand = new DelegateCommand<object>(ExpandAndCollapsWMSCategoriesCommandAction);
                CommandCompleteRecordDragDropPCMArticle = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropPCMArticle);
                CommandOnDragRecordOverPCMArticle = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverPCMArticle);
                CommandDropRecordPCMArticle = new DelegateCommand<DropRecordEventArgs>(PCMArticleViewDropRecordOption);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                DeleteDetectionCommand = new DelegateCommand<object>(DeleteDetectionCommandAction);
                IsWMSExpand = true;
                IsPCMExpand = true;
                UpdatedList = new List<ArticleCategorieMapping>();               
                var screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                WindowHeight = screenHeight - 500;
                TabHeight = WindowHeight - 50;                    
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor ArticlesCategoriesMappingViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void PCMArticleViewDropRecordOption(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PCMArticleViewDropRecordOption()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in ArticleRecords)
                {
                    if (!ArticleCategoryMappingList.Any(i => i.IdWMSArticleCategory == item.IdArticleCategory))
                    {
                        ArticleCategorieMapping art = new ArticleCategorieMapping();
                        art.IdWMSArticleCategory = item.IdArticleCategory;
                        art.WMSName = item.Name;
                        WMSParent = string.Empty;
                        PCMParent = string.Empty;
                        if (string.IsNullOrEmpty(item.ParentName))
                            art.WMSArticleMapping = item.Name;
                        else
                        {
                            BuildWMSParentAndMapping(WarehouseCategoryMenuList.ToList(), item.ParentName);

                            string[] parts = WMSParent.Split('>'); Array.Reverse(parts);
                            art.WMSArticleMapping = string.Join(" > ", parts);
                            art.WMSArticleMapping = (art.WMSArticleMapping + " > " + item.Name).Trim();
                        }
                        art.IdPCMArticleCategory = PCMArt.IdPCMArticleCategory;
                        art.PCMName = PCMArt.Name;
                        BuildPCMParentAndMapping(CategoryMenulist.ToList(), PCMArt.Parent);

                        string[] parts1 = PCMParent.Split('>'); Array.Reverse(parts1);
                        art.PCMArticleMapping = string.Join(" > ", parts1);
                        art.PCMArticleMapping = (art.PCMArticleMapping + " > " + PCMArt.Name).Trim();

                        ArticleCategoryMappingList.Add(art);
                    }
                }
                e.Handled = true;
                e.Effects = DragDropEffects.None;
                GeosApplication.Instance.Logger.Log("Method PCMArticleViewDropRecordOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in Method PCMArticleViewDropRecordOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteDetectionCommandAction(object obj)
        {
            try
            {
                if (obj != null)
                {
                    DevExpress.Xpf.Grid.TableView table = (DevExpress.Xpf.Grid.TableView)obj;
                    ArticleCategorieMapping selectedMapping = (ArticleCategorieMapping)table.FocusedRow;
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteCategoryMappingMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        var PcmArticle = CategoryMenulist.FirstOrDefault(i => i.IdPCMArticleCategory == selectedMapping.IdPCMArticleCategory);
                        if (PcmArticle.Article_count == 0)
                        {
                            selectedMapping.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            if (!UpdatedList.Any(i => i.IdPCMArticleCategory == selectedMapping.IdPCMArticleCategory && i.IdWMSArticleCategory == selectedMapping.IdWMSArticleCategory))
                                UpdatedList.Add(selectedMapping);

                            ArticleCategoryMappingList.Remove(selectedMapping);
                            ArticleCategoryMappingList = new ObservableCollection<ArticleCategorieMapping>(ArticleCategoryMappingList);
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CategoryMappingDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteDetectionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                bool isUpdated = false;
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ArticleCategoryMappingList != null)
                {
                    UpdatedList.AddRange(ArticleCategoryMappingList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList());
                }
                if (UpdatedList.Count > 0)
                {
                    isUpdated = PCMService.AddDeleteArticleCategoryMapping(UpdatedList);
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CategoryMappingAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                if (isUpdated)
                {
                    RequestClose(null, null);
                }             
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CompleteRecordDragDropPCMArticle(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticle()...", category: Category.Info, priority: Priority.Low);                
                e.Handled = false;             
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticle()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropArticle() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOverPCMArticle(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticle()...", category: Category.Info, priority: Priority.Low);
                if (typeof(ArticleCategory).IsAssignableFrom(e.GetRecordType()))
                {
                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                    ArticleRecords = new List<ArticleCategory>(data.Records.OfType<ArticleCategory>());
                    PCMArt = (PCMArticleCategory)e.TargetRecord;                    
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;                 
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticle()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecmaprdOverArticle() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void BuildWMSParentAndMapping(List<ArticleCategory> items, string targetKeyName)
        {

            string parentMapping = string.Empty;
            var parentItem = items.FirstOrDefault(i => i.KeyName == targetKeyName);
            if (string.IsNullOrEmpty(WMSParent))
                WMSParent = parentItem.Name;
            else
                WMSParent = WMSParent + " > " + parentItem.Name;
            if (!string.IsNullOrEmpty(parentItem.ParentName))
            {
                BuildWMSParentAndMapping(items, parentItem.ParentName);
            }
        }
        private void BuildPCMParentAndMapping(List<PCMArticleCategory> items, ulong? targetKey)
        {

            string parentMapping = string.Empty;
            var parentItem = CategoryMenulist.FirstOrDefault(i => i.IdPCMArticleCategory == targetKey);
            if (string.IsNullOrEmpty(PCMParent))
                PCMParent = parentItem.Name;
            else
                PCMParent = PCMParent + " > " + parentItem.Name;
            if (parentItem.Parent!=null&& parentItem.Parent != 0)
            {
                BuildPCMParentAndMapping(items, parentItem.Parent);
            }
        }
        private void ExpandAndCollapsPCMCategoriesCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsPCMExpand)
            {
                t.ExpandAllNodes();
                IsPCMExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsPCMExpand = true;
            }
        }
        private void ExpandAndCollapsWMSCategoriesCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsWMSExpand)
            {
                t.ExpandAllNodes();
                IsWMSExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsWMSExpand = true;
            }
        }
        public void Init()
        {
            try
            {
                FillArticlesCategoryList();
                FillWarehouseCategoryList();
                ArticleCategoryMappingList = new ObservableCollection<ArticleCategorieMapping>(PCMService.GetWMS_PCMCategoryMapping());
                foreach (var item in ArticleCategoryMappingList)
                {
                    var tempwms = WarehouseCategoryMenuList.FirstOrDefault(i => i.IdArticleCategory == item.IdWMSArticleCategory);
                    WMSParent = string.Empty;
                    PCMParent = string.Empty;
                    if (string.IsNullOrEmpty(tempwms.ParentName))
                        item.WMSArticleMapping = item.WMSName;
                    else
                    {
                        BuildWMSParentAndMapping(WarehouseCategoryMenuList.ToList(), tempwms.ParentName);
                        string[] parts = WMSParent.Split('>'); Array.Reverse(parts);
                        item.WMSArticleMapping = string.Join(" > ", parts);
                        item.WMSArticleMapping = (item.WMSArticleMapping + " > " + item.WMSName).Trim();
                    }
                    item.IdPCMArticleCategory = item.IdPCMArticleCategory;
                    item.PCMName = item.PCMName;
                    var temppcm = CategoryMenulist.FirstOrDefault(i => i.IdPCMArticleCategory == item.IdPCMArticleCategory);
                    BuildPCMParentAndMapping(CategoryMenulist.ToList(), temppcm.Parent);
                    string[] parts1 = PCMParent.Split('>'); Array.Reverse(parts1);
                    item.PCMArticleMapping = string.Join(" > ", parts1);

                    item.PCMArticleMapping = (item.PCMArticleMapping + " > " + temppcm.Name).Trim();

                }
                ArticleCategoryMappingList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Modify);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillWarehouseCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseCategoryList()...", category: Category.Info, priority: Priority.Low);
              
                WarehouseCategoryMenuList = new ObservableCollection<ArticleCategory>(PCMService.GetWMSArticlesWithCategoryForReference());
                WarehouseCategoryMenuList = new ObservableCollection<ArticleCategory>(WarehouseCategoryMenuList.OrderBy(x => x.Position));
                GeosApplication.Instance.Logger.Log("Method FillWarehouseCategoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseCategoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseCategoryList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillWarehouseCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillArticlesCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);    
                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2290());
                foreach (PCMArticleCategory category in CategoryMenulist)//[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
                {
                    if (category.InUse == "NO")
                        category.IspcmCategoryNOTInUse = true;
                }
                UpdatePCMCategoryCount();
                PCMArticleCategory articleCategories = new PCMArticleCategory();
                articleCategories.Name = "All";
                articleCategories.KeyName = "Group_All";
                articleCategories.Article_count = CategoryMenulist.Sum(i => i.Article_count);
                articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                CategoryMenulist.Insert(0, articleCategories);
                CategoryMenulist = new ObservableCollection<PCMArticleCategory>(CategoryMenulist.OrderBy(x => x.Position));
                ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)CategoryMenulist;
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticlesCategoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticlesCategoryList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillArticlesCategoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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
        //warehouse

        #endregion
    }
}
