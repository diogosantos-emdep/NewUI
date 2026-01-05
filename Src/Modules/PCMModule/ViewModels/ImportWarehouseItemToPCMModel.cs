using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
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
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class ImportWarehouseItemToPCMModel : ViewModelBase, INotifyPropertyChanged
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
        bool isArticleCategoryExpand;
        private double dialogHeight;
        private double dialogWidth;
        private bool isSaveChanges;

        private ObservableCollection<Articles> warehouseArticleList;
        private Articles selectedWarehouseArticle;

        private ObservableCollection<Articles> articleList;
        private ObservableCollection<Articles> articleList_Cloned;
        private ObservableCollection<Articles> articleList_All;
        private ObservableCollection<Articles> warehouseArticleList_All;
        private Articles selectedArticle;

        private ObservableCollection<PCMArticleCategory> articleCategoryMenulist;
        private PCMArticleCategory selectedArticleCategory;

        private ObservableCollection<ArticleCategories> warehouseArticleCategoryMenulist;
        private ArticleCategories selectedWarehouseArticleCategory;

        private string articleFilterString;
        private string warehouseFilterString;

        private ProductTypeImage maximizedElement;
        private ObservableCollection<Articles> articleListForAddDeleteRetrive;
        private int selectionState;
        private bool confirmationYesNo { get; set; }
        private List<PCMArticleCategory> pcmArticleCategory;
        private ObservableCollection<PCMArticleCategory> clonedPCMArticleCategory;
        private PCMArticleCategory updatedItem;
        private bool isSave;
        private string isArticleDependentInAnotherModules;
        private bool? isCheckedArticle;
        private bool isCellChecked;



        #endregion

        #region Properties
        //[30.11.2022][sshegaonkar][GEOS2-2718]

        public bool IsArticleCategoryExpand
        {
            get { return isArticleCategoryExpand; }
            set
            {
                isArticleCategoryExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleCategoryExpand"));
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

        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }

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

        public ObservableCollection<Articles> ArticleList_Cloned
        {
            get
            {
                return articleList_Cloned;
            }

            set
            {
                articleList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList_Cloned"));
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

        public ObservableCollection<Articles> WarehouseArticleList_All
        {
            get
            {
                return warehouseArticleList_All;
            }

            set
            {
                warehouseArticleList_All = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseArticleList_All"));
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

        public ObservableCollection<PCMArticleCategory> ArticleCategoryMenulist
        {
            get
            {
                return articleCategoryMenulist;
            }

            set
            {
                articleCategoryMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleCategoryMenulist"));
            }
        }

        public PCMArticleCategory SelectedArticleCategory
        {
            get
            {
                return selectedArticleCategory;
            }

            set
            {
                selectedArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleCategory"));
            }
        }
        public string WarehouseFilterString
        {
            get { return warehouseFilterString; }
            set
            {
                warehouseFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseFilterString"));
            }
        }

        public ObservableCollection<Articles> WarehouseArticleList
        {
            get
            {
                return warehouseArticleList;
            }

            set
            {
                warehouseArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseArticleList"));
            }
        }

        public Articles SelectedWarehouseArticle
        {
            get
            {
                return selectedWarehouseArticle;
            }

            set
            {
                selectedWarehouseArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseArticle"));
            }
        }

        public ObservableCollection<ArticleCategories> WarehouseArticleCategoryMenulist
        {
            get
            {
                return warehouseArticleCategoryMenulist;
            }

            set
            {
                warehouseArticleCategoryMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseArticleCategoryMenulist"));
            }
        }

        public ArticleCategories SelectedWarehouseArticleCategory
        {
            get
            {
                return selectedWarehouseArticleCategory;
            }

            set
            {
                selectedWarehouseArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseArticleCategory"));
            }
        }
        public string ArticleFilterString
        {
            get
            {
                return articleFilterString;
            }

            set
            {
                articleFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleFilterString"));
            }
        }
        public ProductTypeImage MaximizedElement
        {
            get
            {
                return maximizedElement;
            }
            set
            {
                maximizedElement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElement"));
            }
        }
        public ObservableCollection<Articles> ArticleListForAddDeleteRetrive
        {
            get
            {
                return articleListForAddDeleteRetrive;
            }

            set
            {
                articleListForAddDeleteRetrive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleListForAddDeleteRetrive"));
            }
        }
        public int SelectionState
        {
            get
            {
                return selectionState;
            }
            set
            {
                selectionState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectionState"));
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
        public List<PCMArticleCategory> PCMArticleCategory
        {
            get { return pcmArticleCategory; }
            set
            {
                pcmArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleCategory"));
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

        public string IsArticleDependentInAnotherModules
        {
            get
            {
                return isArticleDependentInAnotherModules;
            }

            set
            {
                isArticleDependentInAnotherModules = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsArticleDependentInAnotherModules"));
            }
        }

        public bool? IsCheckedArticle
        {
            get
            {
                return isCheckedArticle;
            }

            set
            {
                isCheckedArticle = value;
                if (IsCellChecked == false)
                {
                    if (isCheckedArticle == true)
                    {
                        ArticleList.ToList().ForEach(a => a.IsChecked = true);
                    }
                    else
                    {
                        ArticleList.ToList().ForEach(a => a.IsChecked = false);
                    }
                }
                else
                {
                    IsCellChecked = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCustomer"));
            }
        }

        public bool IsCellChecked
        {
            get
            {
                return isCellChecked;
            }

            set
            {
                isCellChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCellChecked"));
            }
        }

        List<string> warehouseArticleReference;
        public List<string> WarehouseArticleReference
        {
            get
            {
                return warehouseArticleReference;
            }
            set
            {
                warehouseArticleReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseArticleReference"));
            }
        }

        string reference=string.Empty;
        public string References
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("References"));
            }
        }

        string myFilterString;
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        #endregion

        #region ICommands
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand SelectWarehouseArticleCategoryCommand { get; set; }
        public ICommand SelectArticleCategoryCommand { get; set; }
        public ICommand CommandOnDragRecordOverArticle { get; set; }
        public ICommand CommandOnDragRecordOverArticleGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropArticle { get; set; }
        public ICommand PasteInSearchControlCommand { get; set; }
        public ICommand AcceptPCMArticleButtonCommand { get; set; }
        public ICommand CancelPCMArticleButtonCommand { get; set; }
        public ICommand OpenWarehouseArticleSelectedImageCommand { get; set; }
        public ICommand OpenArticleSelectedImageCommand { get; set; }
        public ICommand DeleteArticleCommand { get; set; }
        public ICommand AddNewCategoryCommand { get; set; }
        public ICommand EditCategoryCommand { get; set; }
        public ICommand CommandOnDragRecordOverArticleCatagoriesGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropArticleCatagories { get; set; }
        public ICommand CommandTreeListViewDropRecordArticleCatagories { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }

        public ICommand ExpandAndCollapseArticleCategoryCommand { get; set; }      //[30.11.2022][sshegaonkar][GEOS2-2718]

        public ICommand CustomShowFilterPopupCommand { get; set; }
        #endregion

        #region Constructor

        public ImportWarehouseItemToPCMModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor ImportWarehouseItemToPCMModel ...", category: Category.Info, priority: Priority.Low);
                SelectionState = 0;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;

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

                IsArticleCategoryExpand = true;

                ExpandAndCollapseArticleCategoryCommand = new DelegateCommand<object>(ExpandAndCollapseArticleCategoryCommandAction); //[30.11.2022][sshegaonkar][GEOS2-2718]
                EscapeButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SelectWarehouseArticleCategoryCommand = new RelayCommand(new Action<object>(RetrieveWarehouseArticlesByCategory));
                SelectArticleCategoryCommand = new RelayCommand(new Action<object>(RetrieveArticlesByCategory));

                CommandOnDragRecordOverArticle = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticle);
                CommandOnDragRecordOverArticleGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleGrid);
                CommandCompleteRecordDragDropArticle = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticle);
                PasteInSearchControlCommand = new DelegateCommand<object>(PasteInSearchControlCommandAction);
                OpenWarehouseArticleSelectedImageCommand = new DelegateCommand<object>(OpenWarehouseArticleSelectedImageAction);
                OpenArticleSelectedImageCommand = new DelegateCommand<object>(OpenArticleSelectedImageAction);
                DeleteArticleCommand = new DelegateCommand<object>(DeleteArticle);

                AddNewCategoryCommand = new DelegateCommand<object>(AddNewCategory);
                EditCategoryCommand = new DelegateCommand<object>(EditCategory);

                AcceptPCMArticleButtonCommand = new DelegateCommand<CompleteRecordDragDropEventArgs>(AddWarehouseItemsToPCM);
                CancelPCMArticleButtonCommand = new DelegateCommand<CompleteRecordDragDropEventArgs>(CloseWindow);

                CommandOnDragRecordOverArticleCatagoriesGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverArticleCatagoriesGrid);
                CommandCompleteRecordDragDropArticleCatagories = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropArticleCatagories);
                CommandTreeListViewDropRecordArticleCatagories = new DelegateCommand<DropRecordEventArgs>(TreeListViewDropRecordArticleCategories);
                DeleteCategoryCommand = new DelegateCommand<object>(DeleteCategoryAction);

                SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChangedCommandAction);

                //CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor ImportWarehouseItemToPCMModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor ImportWarehouseItemToPCMModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Command Action
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveWarehouseArticlesByCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveWarehouseArticlesByCategory()...", category: Category.Info, priority: Priority.Low);
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
                if (SelectedWarehouseArticleCategory == null || SelectedWarehouseArticleCategory.Name == "All")
                {
                    WarehouseArticleList = new ObservableCollection<Articles>(WarehouseArticleList_All);
                }
                else
                {
                    string Concat_ChildArticles = string.Join(",", getIdsForRetriveWarehouseArticlesByParentClick().Select(x => x.ToString()).ToArray());
                    string[] ids = (Concat_ChildArticles + "," + SelectedWarehouseArticleCategory.IdArticleCategory).Split(',');
                    WarehouseArticleList = new ObservableCollection<Articles>(WarehouseArticleList_All.Where(x => ids.Contains(x.IdArticleCategory.ToString())));
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveWarehouseArticlesByCategory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveWarehouseArticlesByCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                if (SelectedArticleCategory == null || SelectedArticleCategory.Name == "All")
                {
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                    ArticleList_Cloned = new ObservableCollection<Articles>(ArticleList_All);
                }
                else
                {
                    string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                    string[] ids = (Concat_ChildArticles + "," + SelectedArticleCategory.IdPCMArticleCategory).Split(',');
                    ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                    ArticleList_Cloned = new ObservableCollection<Articles>(ArticleList);
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
        private void OnDragRecordOverArticle(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticle()...", category: Category.Info, priority: Priority.Low);


                if (typeof(Articles).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticle()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverArticle() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverArticleGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticleGrid()...", category: Category.Info, priority: Priority.Low);
                SelectionState = 1;
                if (SelectedArticleCategory.Name != "All")
                {
                    if ((e.IsFromOutside) && typeof(Articles).IsAssignableFrom(e.GetRecordType()))
                    {
                        e.Effects = DragDropEffects.Copy;
                        e.Handled = true;
                        //IsEnabled = true;
                    }
                }
                else
                {
                    e.Handled = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverArticleGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverArticleGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][GOS2-2183][17-07-2020][The count PCM_articles  (grid and category menu) don’t refresh the value after drag and drop items.]
        /// </summary>
        /// <param name="e"></param>
        private void CompleteRecordDragDropArticle(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticle()...", category: Category.Info, priority: Priority.Low);
                e.Handled = true;
                if (ArticleList != null)
                {
                    ArticleList = new ObservableCollection<Articles>(ArticleList.GroupBy(opt => opt.IdArticle).Select(g => g.First()));
                    if (ArticleListForAddDeleteRetrive == null)
                    {
                        ArticleListForAddDeleteRetrive = new ObservableCollection<Articles>();
                    }
                    foreach (Articles article in ArticleList)
                    {
                        //if(ArticleList_Cloned.Any(a => a.IdArticle == article.IdArticle) || ArticleListForAddDeleteRetrive.Any(a => a.IdArticle == article.IdArticle))
                        //{
                        //    deleteArticleAndManagedCategoryCount(article);
                        //}
                        if (!ArticleList_Cloned.Any(a => a.IdArticle == article.IdArticle))
                        {
                            if (!ArticleListForAddDeleteRetrive.Any(a => a.IdArticle == article.IdArticle))
                            {
                                article.IdPCMArticleCategory = SelectedArticleCategory.IdPCMArticleCategory;
                                article.PcmArticleCategory.IdPCMArticleCategory = SelectedArticleCategory.IdPCMArticleCategory;
                                article.ArticleCategory.Name = SelectedArticleCategory.Name;
                                article.TransactionOperation = ModelBase.TransactionOperations.Add;
                                ArticleListForAddDeleteRetrive.Add(article);
                                SelectedArticleCategory.Article_count++; //[001] Added
                                ArticleList_All.Add(article);
                                //[001] Start
                                SelectedArticleCategory.NameWithArticleCount = SelectedArticleCategory.Name + "[" + SelectedArticleCategory.Article_count + "]";
                                if (SelectedArticleCategory.ParentName != null)
                                {
                                    ArticleCategoryMenulist.Where(a => a.KeyName == SelectedArticleCategory.ParentName).ToList().ForEach(a =>
                                    {
                                        a.Article_count = a.Article_count + 1;
                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                    });
                                    PCMArticleCategory parentrecord = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedArticleCategory.Parent);
                                    if (parentrecord != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord.Parent))
                                    {
                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord.Parent).ToList().ForEach(a =>
                                        {
                                            a.Article_count = a.Article_count + 1;
                                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                        });
                                        PCMArticleCategory parentrecord1 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord.Parent);
                                        if (parentrecord1 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord1.Parent))
                                        {
                                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord1.Parent).ToList().ForEach(a =>
                                            {
                                                a.Article_count = a.Article_count + 1;
                                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                            });
                                            PCMArticleCategory parentrecord2 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord1.Parent);
                                            if (parentrecord2 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord2.Parent))
                                            {
                                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord2.Parent).ToList().ForEach(a =>
                                                {
                                                    a.Article_count = a.Article_count + 1;
                                                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                                });
                                                PCMArticleCategory parentrecord3 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord2.Parent);
                                                if (parentrecord3 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord3.Parent))
                                                {
                                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord3.Parent).ToList().ForEach(a =>
                                                    {
                                                        a.Article_count = a.Article_count + 1;
                                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                                    });
                                                    PCMArticleCategory parentrecord4 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord3.Parent);
                                                    if (parentrecord4 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord4.Parent))
                                                    {
                                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord4.Parent).ToList().ForEach(a =>
                                                        {
                                                            a.Article_count = a.Article_count + 1;
                                                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                                        });
                                                        PCMArticleCategory parentrecord5 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord4.Parent);
                                                        if (parentrecord5 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord5.Parent))
                                                        {
                                                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord5.Parent).ToList().ForEach(a =>
                                                            {
                                                                a.Article_count = a.Article_count + 1;
                                                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                                    {
                                        a.Article_count = a.Article_count + 1;
                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                    });

                                }
                                else
                                {
                                    ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                                    {
                                        a.Article_count = a.Article_count + 1;
                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                    });
                                }
                                //[001] End
                            }
                            else
                            {
                            }
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropArticle()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropArticle() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenWarehouseArticleSelectedImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWarehouseArticleSelectedImageAction()...", category: Category.Info, priority: Priority.Low);

                if (SelectedWarehouseArticle.ArticleImageInBytes != null)
                {
                    AppBarControlView appBarControlView = new AppBarControlView();
                    AppBarControlViewModel appBarControlViewModel = new AppBarControlViewModel();

                    appBarControlViewModel.Init(SelectedWarehouseArticle);
                    EventHandler handle = delegate { appBarControlView.Close(); };
                    appBarControlViewModel.RequestClose += handle;
                    appBarControlView.DataContext = appBarControlViewModel;
                    appBarControlView.ShowDialog();
                }

                GeosApplication.Instance.Logger.Log("Method OpenWarehouseArticleSelectedImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenWarehouseArticleSelectedImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenArticleSelectedImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenArticleSelectedImageAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedArticle.ArticleImageInBytes != null)
                {
                    AppBarControlView appBarControlView = new AppBarControlView();
                    AppBarControlViewModel appBarControlViewModel = new AppBarControlViewModel();

                    appBarControlViewModel.Init(SelectedArticle);
                    EventHandler handle = delegate { appBarControlView.Close(); };
                    appBarControlViewModel.RequestClose += handle;
                    appBarControlView.DataContext = appBarControlViewModel;
                    appBarControlView.ShowDialog();
                }
                GeosApplication.Instance.Logger.Log("Method OpenArticleSelectedImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenArticleSelectedImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][GOS2-2183][17-07-2020][The count PCM_articles  (grid and category menu) don’t refresh the value after drag and drop items.]
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteArticle(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteArticle()...", category: Category.Info, priority: Priority.Low);

                List<Articles> List = ArticleList.Where(x => x.IsChecked == true).ToList();
                if(List.Count == 0)
                {
                    List.Add(SelectedArticle);
                }

                if (List.Count > 0)
                {
                    List<Articles> UsedArticlesList = new List<Articles>();
                    List<string> ArticleReferences = new List<string>();
                    string result = string.Empty;
                    string result1 = string.Empty;

                    string ReferenceCannotBeDeleted = string.Empty;

                    foreach (Articles item in List)
                    {
                        IsArticleDependentInAnotherModules = PCMService.GetPCMArticleExistNames_V2120(item.IdArticle);

                        if (!(string.IsNullOrEmpty(IsArticleDependentInAnotherModules)))
                        {
                            UsedArticlesList.Add(item);

                            ReferenceCannotBeDeleted = String.Join("", item.Reference + " " + string.Format(System.Windows.Application.Current.FindResource("ArticlesExistsIn").ToString()) + " " + IsArticleDependentInAnotherModules + ".");
                            result1 += String.Join("", ReferenceCannotBeDeleted + "\r");
                            IsArticleDependentInAnotherModules = string.Empty;
                        }
                    }

                    result = string.Format(result1);
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(result + System.Environment.NewLine + Application.Current.Resources["DeleteSelectedArticles"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo, MessageBoxResult.No);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        foreach (Articles item in List)
                        {
                            deleteArticleAndManagedCategoryCount(item);
                            SelectedArticle = ArticleList.FirstOrDefault();
                        }
                    }
                }

                //else
                //{
                //    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Environment.NewLine + Application.Current.Resources["DeleteArticle"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo, MessageBoxResult.No);
                //    if (MessageBoxResult == MessageBoxResult.Yes)
                //    {
                //        IsArticleDependentInAnotherModules = PCMService.GetPCMArticleExistNames_V2120(SelectedArticle.IdArticle);
                //        deleteArticleAndManagedCategoryCount(SelectedArticle);
                //        SelectedArticle = ArticleList.FirstOrDefault();
                //    }
                //}
                GeosApplication.Instance.Logger.Log("Method DeleteArticle()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteArticle() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteArticle() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DeleteArticle() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                addEditCategoryViewModel.ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/ImageEditLogo.png"));
                addEditCategoryViewModel.IsReferenceImageExist = false;
                addEditCategoryViewModel.Init();
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();

                if (addEditCategoryViewModel.IsSave)
                {

                    addEditCategoryViewModel.OrderCategoryList.Where(a => a.IdPCMArticleCategory == 0).ToList().ForEach(a => { a.IdPCMArticleCategory = addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory; });

                    ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(addEditCategoryViewModel.OrderCategoryList.Where(a => a.Name != "---"));

                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    ArticleCategoryMenulist.Insert(0, articleCategories);

                    ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(ArticleCategoryMenulist.OrderBy(x => x.Position));

                    SelectedArticleCategory = ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == addEditCategoryViewModel.NewArticleCategory.IdPCMArticleCategory).FirstOrDefault();

                    if (SelectedArticleCategory != null)
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(a => a.PcmArticleCategory.IdPCMArticleCategory == SelectedArticleCategory.IdPCMArticleCategory));
                    }
                    else
                    {
                        SelectedArticleCategory = ArticleCategoryMenulist.FirstOrDefault();
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
                if (SelectedArticleCategory.Name == "All" && SelectedArticleCategory.KeyName == "Group_All")
                {
                    return;
                }
                AddEditCategoryView addEditCategoryView = new AddEditCategoryView();
                AddEditCategoryViewModel addEditCategoryViewModel = new AddEditCategoryViewModel();
                EventHandler handle = delegate { addEditCategoryView.Close(); };
                addEditCategoryViewModel.RequestClose += handle;
                addEditCategoryViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCategoryHeader").ToString();
                addEditCategoryViewModel.IsNew = false;
                addEditCategoryViewModel.EditInitCategory(SelectedArticleCategory);
                addEditCategoryView.DataContext = addEditCategoryViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditCategoryView.Owner = Window.GetWindow(ownerInfo);
                addEditCategoryView.ShowDialog();
                PCMArticleCategory selected_Cat = SelectedArticleCategory;
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

                    ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                    UpdatePCMCategoryCount();
                    PCMArticleCategory articleCategories = new PCMArticleCategory();
                    articleCategories.Name = "All";
                    articleCategories.KeyName = "Group_All";
                    articleCategories.Article_count = ArticleList_All.Count();
                    articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                    ArticleCategoryMenulist.Insert(0, articleCategories);
                    ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(ArticleCategoryMenulist.OrderBy(x => x.Position));
                    SelectedArticleCategory = selected_Cat;

                    ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)ArticleCategoryMenulist;


                    if (SelectedArticleCategory == null || SelectedArticleCategory.Name == "All")
                    {
                        ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                    }
                    else
                    {
                        string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                        string[] ids = (Concat_ChildArticles + "," + SelectedArticleCategory.IdPCMArticleCategory).Split(',');
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
        #endregion

        #region Methods
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
                FillWarehouseArticleList();
                FillArticleList();
                FillArticleCatagoryList();
                FillWarehouseArticleCatagoryList();
                //Shubham[skadam] GEOS2-5024 Improvements in PCM module 25 12 2023
                MyFilterString = string.Empty;
                //MyFilterString = "[Reference] In (" + String.Join(",", WarehouseArticleReference) + ")";
                //MyFilterString = "[Reference] In ('02PM4','04A4Z','3HMONTMAT','01PSC2DT','01PSC4','01RKP','01TDRKGDT','01TTRKP','04APN25')";
                //MyFilterString = "[Reference] In (" + String.Join(",", WarehouseArticleReference) + ")";
                MyFilterString = "[Reference] In ('" + String.Join("','", WarehouseArticleReference) + "')";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillWarehouseArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseArticleList()...", category: Category.Info, priority: Priority.Low);

                WarehouseArticleList = new ObservableCollection<Articles>(PCMService.GetAllActiveArticles());

                SelectedWarehouseArticle = new Articles();
                SelectedWarehouseArticle = WarehouseArticleList.FirstOrDefault();

                WarehouseArticleList_All = new ObservableCollection<Articles>(WarehouseArticleList);
                GeosApplication.Instance.Logger.Log("Method FillWarehouseArticleList()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseArticleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseArticleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillWarehouseArticleList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList()...", category: Category.Info, priority: Priority.Low);

                ArticleList = new ObservableCollection<Articles>(PCMService.GetAllPCMArticles_V2110());
                SelectedArticle = new Articles();
                SelectedArticle = ArticleList.FirstOrDefault();

                ArticleList_All = new ObservableCollection<Articles>(ArticleList);
                ArticleList_Cloned = new ObservableCollection<Articles>(ArticleList);
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


        private void FillWarehouseArticleCatagoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);
                WarehouseArticleCategoryMenulist = new ObservableCollection<ArticleCategories>(PCMService.GetActiveArticleCategories());
                UpdateWarehouseCategoryCount();
                ArticleCategories articleCategories = new ArticleCategories();
                articleCategories.Name = "All";
                articleCategories.KeyName = "Group_All";
                articleCategories.Article_count = WarehouseArticleList.Count();
                articleCategories.NameWithArticleCount = "All [" + articleCategories.Article_count + "]";
                WarehouseArticleCategoryMenulist.Insert(0, articleCategories);
                SelectedWarehouseArticleCategory = new ArticleCategories();
                SelectedWarehouseArticleCategory = WarehouseArticleCategoryMenulist.Where(a => a.ParentName == null && a.Name == "All").FirstOrDefault();
               
                string Concat_ChildArticles = string.Join(",", getIdsForRetriveWarehouseArticlesByParentClick().Select(x => x.ToString()).ToArray());
                string[] ids = (Concat_ChildArticles + "," + SelectedWarehouseArticleCategory.IdArticleCategory).Split(',');
                //WarehouseArticleList = new ObservableCollection<Articles>(WarehouseArticleList_All.Where(x => ids.Contains(x.IdArticleCategory.ToString())));

                WarehouseArticleList = new ObservableCollection<Articles>(WarehouseArticleList_All);
                SelectedWarehouseArticle = WarehouseArticleList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillWarehouseArticleCatagoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseArticleCatagoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseArticleCatagoryList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillWarehouseArticleCatagoryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillArticleCatagoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);

                ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(PCMService.GetPCMArticleCategories_V2060());
                UpdatePCMCategoryCount();
                PCMArticleCategory pCMArticleCategory = new PCMArticleCategory();
                pCMArticleCategory.Name = "All";
                pCMArticleCategory.KeyName = "Group_All";
                pCMArticleCategory.Article_count = ArticleList.Count();
                pCMArticleCategory.NameWithArticleCount = "All [" + pCMArticleCategory.Article_count + "]";

                ArticleCategoryMenulist.Insert(0, pCMArticleCategory);
                ArticleCategoryMenulist = new ObservableCollection<PCMArticleCategory>(ArticleCategoryMenulist.OrderBy(x => x.Position));
                SelectedArticleCategory = new PCMArticleCategory();
                SelectedArticleCategory = ArticleCategoryMenulist.Where(a => a.ParentName == null && a.Name == "All").FirstOrDefault();


                string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                string[] ids = (Concat_ChildArticles + "," + SelectedArticleCategory.IdPCMArticleCategory).Split(',');
               // ArticleList = new ObservableCollection<Articles>(ArticleList_All.Where(x => ids.Contains(x.IdPCMArticleCategory.ToString())));
                ArticleList = new ObservableCollection<Articles>(ArticleList_All);
                ArticleList_Cloned = new ObservableCollection<Articles>(ArticleList);

                SelectedArticle = ArticleList.FirstOrDefault();
                ClonedPCMArticleCategory = (ObservableCollection<PCMArticleCategory>)ArticleCategoryMenulist;
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

        private void AddWarehouseItemsToPCM(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddWarehouseItemsToPCM()...", category: Category.Info, priority: Priority.Low);
                if (ArticleListForAddDeleteRetrive != null)
                {
                    var article_list = ArticleListForAddDeleteRetrive.GroupBy(x => x.IdPCMArticleCategory).ToList().Select(x => x.ToList());
                    foreach (var item in article_list)
                    {
                      // IsSaveChanges = PCMService.IsUpdatePCMArticleCategoryInArticle(item[0].IdPCMArticleCategory, item.ToList());
                       IsSaveChanges = PCMService.AddDeletePCMArticle_V2140(item[0].IdPCMArticleCategory, item.ToList(), GeosApplication.Instance.ActiveUser.IdUser);
                    }
                }
             
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMArticleUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                if (IsSaveChanges && ArticleListForAddDeleteRetrive!=null && ArticleListForAddDeleteRetrive.Where(i=>i.TransactionOperation!= ModelBase.TransactionOperations.Delete).Count()>0 )
                {
                   var ownerInfo = (obj as FrameworkElement);
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["PCMArticleCostPriceWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        try
                        {
                            GeosApplication.Instance.Logger.Log("AddArticleCostPrice()...", category: Category.Info, priority: Priority.Low);

                            TableView detailView = (TableView)obj;

                            AddArticleCostPriceView addArticleCostPriceView = new AddArticleCostPriceView();
                            AddArticleCostPriceViewModel addArticleCostPriceViewModel = new AddArticleCostPriceViewModel();

                            EventHandler handle = delegate { addArticleCostPriceView.Close(); };
                            addArticleCostPriceViewModel.RequestClose += handle;
                           addArticleCostPriceViewModel.WindowHeader ="Articles";
                            addArticleCostPriceView.DataContext = addArticleCostPriceViewModel;
                            addArticleCostPriceViewModel.IsNew = true;
                            addArticleCostPriceViewModel.Init(ArticleListForAddDeleteRetrive.ToList());

                           // var ownerInfo = (obj as FrameworkElement);
                           // addArticleCostPriceView.Owner = Window.GetWindow(ownerInfo);
                            addArticleCostPriceView.ShowDialog();

                           
                            GeosApplication.Instance.Logger.Log(" AddArticleCostPrice()....executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method AddArticleCostPrice() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AddWarehouseItemsToPCM()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWarehouseItemsToPCM() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWarehouseItemsToPCM() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddWarehouseItemsToPCM() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][14/09/2020][GEOS2-2126]-PCM -CW4 - Improvements in the PCM Article [Item: 12_14] [#PCM44]
        /// </summary>
        private void OnDragRecordOverArticleCatagoriesGrid(DragRecordOverEventArgs e)
        {
            try
            {
                //[001]
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverDetectionsGrid()...", category: Category.Info, priority: Priority.Low);

                if (e.DropPosition == DropPosition.Inside && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))//&& typeof(Detections).IsAssignableFrom(e.GetRecordType())
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
                    if (newRecords1.Count == 0)
                    {
                        e.Handled = false;
                        return;
                    }
                    PCMArticleCategory SelectedOrderCategory = (PCMArticleCategory)e.TargetRecord;

                    List<PCMArticleCategory> lstUpdateItem = new List<PCMArticleCategory>();
                    lstUpdateItem = newRecords1.ToList();

                    UpdatedItem = newRecords1.FirstOrDefault();

                    uint pos = 1;
                    uint status = 0;

                    #region OneParentoAnaterParent
                    if (UpdatedItem.Parent != null)
                    {
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

                        if (ClonedPCMArticleCategory.Count > 0)
                        {
                            if (e.DropPosition == DropPosition.Inside)
                            {
                                ulong? UpdatedItemParent = UpdatedItem.Parent;
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
                                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_Selectorder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).ToList();
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
                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }

                                pos = 1;
                                foreach (var updateArt in lstUpdateItem)
                                {
                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = SelectedOrderCategory.KeyName; });
                                    PCMArticleCategory.Add(ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
                                }

                                foreach (var updateArt in pcmArticleCategory_ForSetOrder_Selectorder)
                                {

                                    PCMArticleCategory.Add(ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
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
                                    PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                }



                            }
                            else
                            {


                                if (e.DropPosition != DropPosition.Inside && (SelectedOrderCategory.Parent == null || UpdatedItem.Parent != SelectedOrderCategory.Parent))
                                {

                                    ulong? UpdatedItemParent = UpdatedItem.Parent;
                                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
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
                                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
                                        }
                                    }

                                    if (SelectedOrderCategory.Parent == null || (e.DropPosition == DropPosition.Inside && SelectedOrderCategory.Parent != null))
                                    {

                                    }
                                    else
                                    {
                                        pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();

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
                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
                                    }

                                    if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
                                    {
                                        PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
                                    }
                                }
                                else
                                {
                                    pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent

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
                                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

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
                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();

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
                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

                            }
                        }
                    }
                    if (UpdatedItem.Parent != null)
                    {
                        if (e.DropPosition == DropPosition.Inside)
                        {
                            PCMArticleCategory.OrderBy(a => a.Position);
                            //PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
                        }
                        else
                        {
                            PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
                        }


                        if (PCMArticleCategory_ParentChange.Count > 0)
                        {
                            PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
                        }
                    }
                    else if (UpdatedItem.Parent == null && SelectedOrderCategory.Parent == null)
                    {
                        PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
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
        //private void TreeListViewDropRecordArticleCategories(DropRecordEventArgs e)
        //{
        //    try
        //    {
        //        //[001]
        //        ProductTypeArticleView ptav = new ProductTypeArticleView();
        //        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["SaveArticleCatagory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

        //        if (MessageBoxResult == MessageBoxResult.Yes)
        //        {

        //            ConfirmationYesNo = true;

        //            uint tmpIdPCMArticleCategoryIndexOf;
        //            bool isParentChange = false;
        //            List<PCMArticleCategory> PCMArticleCategory_ParentChange = new List<PCMArticleCategory>();
        //            PCMArticleCategory = new List<PCMArticleCategory>();


        //            var data1 = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
        //            List<PCMArticleCategory> newRecords1 = data1.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName, Position = x.Position }).ToList();

        //            PCMArticleCategory SelectedOrderCategory = (PCMArticleCategory)e.TargetRecord;

        //            List<PCMArticleCategory> lstUpdateItem = new List<PCMArticleCategory>();
        //            lstUpdateItem = newRecords1.ToList();

        //            UpdatedItem = newRecords1.FirstOrDefault();

        //            uint pos = 1;
        //            uint status = 0;

        //            #region OneParentoAnaterParent
        //            if (UpdatedItem.Parent != null)
        //            {
        //                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

        //                if (ClonedPCMArticleCategory.Count > 0)
        //                {
        //                    if (e.DropPosition == DropPosition.Inside)
        //                    {
        //                        ulong? UpdatedItemParent = UpdatedItem.Parent;
        //                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
        //                        List<uint> indexCollection = new List<uint>();
        //                        pos = 1;
        //                        foreach (var updateArt in lstUpdateItem)
        //                        {
        //                            indexCollection.Add(updateArt.IdPCMArticleCategory);

        //                        }
        //                        pos = 1;
        //                        foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
        //                        {
        //                            if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
        //                            {
        //                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
        //                            }
        //                        }

        //                        pos = 0;
        //                        foreach (var updateArt in lstUpdateItem)
        //                        {
        //                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = SelectedOrderCategory.KeyName; });
        //                        }

        //                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder1 = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
        //                        pos = 1;
        //                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder1)
        //                        {
        //                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                        }

        //                        if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
        //                        {
        //                            PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
        //                        }



        //                    }
        //                    else
        //                    {


        //                        if (e.DropPosition != DropPosition.Inside && (SelectedOrderCategory.Parent == null || UpdatedItem.Parent != SelectedOrderCategory.Parent))
        //                        {

        //                            ulong? UpdatedItemParent = UpdatedItem.Parent;
        //                            List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
        //                            List<uint> indexCollection = new List<uint>();
        //                            pos = 1;
        //                            foreach (var updateArt in lstUpdateItem)
        //                            {
        //                                indexCollection.Add(updateArt.IdPCMArticleCategory);

        //                            }
        //                            pos = 1;
        //                            foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
        //                            {
        //                                if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
        //                                {
        //                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
        //                                }
        //                            }

        //                            if (SelectedOrderCategory.Parent == null || (e.DropPosition == DropPosition.Inside && SelectedOrderCategory.Parent != null))
        //                            {

        //                            }
        //                            else
        //                            {
        //                                pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();

        //                                switch (e.DropPosition)
        //                                {
        //                                    case DropPosition.Append:
        //                                        break;
        //                                    case DropPosition.Before:
        //                                        {
        //                                            foreach (var updateArt in lstUpdateItem)
        //                                            {

        //                                                updateArt.Parent = SelectedOrderCategory.Parent;
        //                                                updateArt.ParentName = SelectedOrderCategory.ParentName;
        //                                                updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                            }
        //                                        }
        //                                        break;
        //                                    case DropPosition.After:
        //                                        {
        //                                            tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                            foreach (var updateArt in lstUpdateItem)
        //                                            {

        //                                                updateArt.Parent = SelectedOrderCategory.Parent;
        //                                                updateArt.ParentName = SelectedOrderCategory.ParentName;
        //                                                updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                            }
        //                                        }
        //                                        break;
        //                                    case DropPosition.Inside:
        //                                        {

        //                                        }
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }


        //                            }


        //                            pos = 1;
        //                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                            {
        //                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
        //                            }

        //                            if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
        //                            {
        //                                PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent

        //                                    ).OrderBy(a => a.Position).ToList();
        //                            if (UpdatedItem.Parent == SelectedOrderCategory.Parent)
        //                            {
        //                                if (SelectedOrderCategory.Position > UpdatedItem.Position)
        //                                {
        //                                    switch (e.DropPosition)
        //                                    {
        //                                        case DropPosition.Append:
        //                                            break;
        //                                        case DropPosition.Before:
        //                                            {

        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.After:
        //                                            {
        //                                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                                }

        //                                            }
        //                                            break;
        //                                        case DropPosition.Inside:
        //                                            {


        //                                            }
        //                                            break;
        //                                        default:
        //                                            break;
        //                                    }


        //                                }
        //                                else
        //                                {
        //                                    switch (e.DropPosition)
        //                                    {
        //                                        case DropPosition.Append:
        //                                            break;
        //                                        case DropPosition.Before:
        //                                            {

        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.After:
        //                                            {
        //                                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.Inside:
        //                                            {

        //                                            }
        //                                            break;
        //                                        default:
        //                                            break;
        //                                    }
        //                                }
        //                                if (e.DropPosition != DropPosition.Inside)
        //                                {
        //                                    pos = 1;
        //                                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                                    {
        //                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                                    }
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //            #endregion 
        //            else if (SelectedOrderCategory.Parent == null && UpdatedItem.Parent == null)
        //            {
        //                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();

        //                if (SelectedOrderCategory.Position > UpdatedItem.Position)
        //                {
        //                    switch (e.DropPosition)
        //                    {
        //                        case DropPosition.Append:
        //                            break;
        //                        case DropPosition.Before:
        //                            {

        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                }


        //                            }
        //                            break;
        //                        case DropPosition.After:
        //                            {
        //                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                }

        //                            }
        //                            break;
        //                        case DropPosition.Inside:
        //                            {

        //                            }
        //                            break;
        //                        default:
        //                            break;
        //                    }


        //                }
        //                else
        //                {
        //                    switch (e.DropPosition)
        //                    {
        //                        case DropPosition.Append:
        //                            break;
        //                        case DropPosition.Before:
        //                            {

        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                }

        //                            }
        //                            break;
        //                        case DropPosition.After:
        //                            {
        //                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                }


        //                            }
        //                            break;
        //                        case DropPosition.Inside:
        //                            {


        //                            }
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                }
        //                if (e.DropPosition != DropPosition.Inside)
        //                {
        //                    pos = 1;
        //                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                    {
        //                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                    }
        //                }
        //            }
        //            if (UpdatedItem.Parent != null)
        //            {
        //                if (e.DropPosition == DropPosition.Inside)
        //                {

        //                    PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
        //                }
        //                else
        //                {
        //                    PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
        //                }


        //                if (PCMArticleCategory_ParentChange.Count > 0)
        //                {
        //                    PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
        //                }
        //            }
        //            else if (UpdatedItem.Parent == null && SelectedOrderCategory.Parent == null)
        //            {
        //                PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
        //                if (PCMArticleCategory_ParentChange.Count > 0)
        //                {
        //                    PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
        //                }
        //            }

        //            if (e.IsFromOutside == false && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
        //            {
        //                if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
        //                {
        //                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
        //                    List<PCMArticleCategory> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName }).ToList();

        //                    PCMArticleCategory temp = newRecords.FirstOrDefault();
        //                    //if(e.DropPosition != DropPosition.Inside)
        //                    //{
        //                    //    
        //                    //}
        //                    PCMArticleCategory target_record = (PCMArticleCategory)e.TargetRecord;
        //                    if ((temp.Parent == null && target_record.Parent == null) || (temp.Parent != null && target_record.Parent != null) || target_record.Parent == null) // && temp.Parent == target_record.Parent
        //                    {
        //                        e.Effects = DragDropEffects.Move;
        //                        e.Handled = true;

        //                    }
        //                    else
        //                    {
        //                        e.Effects = DragDropEffects.None;
        //                        e.Handled = false;
        //                    }
        //                }
        //            }

        //            ////save data from darag and dropped data.
        //            if (PCMArticleCategory != null)
        //            {
        //                if (PCMArticleCategory.Count > 0)
        //                {

        //                    IsSave = PCMService.IsUpdatedPCMArticleCategoryOrder(PCMArticleCategory, (uint)GeosApplication.Instance.ActiveUser.IdUser);

        //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

        //                    FillArticleCatagoryList();

        //                    //  RequestClose(null, null);
        //                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptArticleCategoryAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            ConfirmationYesNo = false;

        //        }
        //        //
        //    }

        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void TreeListViewDropRecordArticleCategories(DropRecordEventArgs e)
        //{
        //    try
        //    {
        //        //[001]
        //        ProductTypeArticleView ptav = new ProductTypeArticleView();
        //        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["SaveArticleCatagory"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

        //        if (MessageBoxResult == MessageBoxResult.Yes)
        //        {

        //            ConfirmationYesNo = true;
        //            uint tmpIdPCMArticleCategoryIndexOf;
        //            bool isParentChange = false;
        //            List<PCMArticleCategory> PCMArticleCategory_ParentChange = new List<PCMArticleCategory>();
        //            PCMArticleCategory = new List<PCMArticleCategory>();

        //            var data1 = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
        //            List<PCMArticleCategory> newRecords1 = data1.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName, Position = x.Position }).ToList();

        //            PCMArticleCategory SelectedOrderCategory = (PCMArticleCategory)e.TargetRecord;
        //            List<PCMArticleCategory> lstUpdateItem = new List<PCMArticleCategory>();
        //            lstUpdateItem = newRecords1.ToList();

        //            UpdatedItem = newRecords1.FirstOrDefault();

        //            uint pos = 1;
        //            uint status = 0;

        //            #region OneParentoAnaterParent
        //            if (UpdatedItem.Parent != null)
        //            {
        //                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();

        //                if (ClonedPCMArticleCategory.Count > 0)
        //                {
        //                    if (e.DropPosition == DropPosition.Inside)
        //                    {
        //                        ulong? UpdatedItemParent = UpdatedItem.Parent;
        //                        List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
        //                        List<uint> indexCollection = new List<uint>();
        //                        pos = 1;
        //                        foreach (var updateArt in lstUpdateItem)
        //                        {
        //                            indexCollection.Add(updateArt.IdPCMArticleCategory);

        //                        }
        //                        pos = 1;
        //                        foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
        //                        {
        //                            if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
        //                            {
        //                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
        //                            }
        //                        }

        //                        pos = 1;
        //                        foreach (var updateArt in lstUpdateItem)
        //                        {
        //                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = SelectedOrderCategory.IdPCMArticleCategory; a.ParentName = SelectedOrderCategory.KeyName; });
        //                        }

        //                        if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
        //                        {
        //                            PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
        //                        }
        //                    }
        //                    else
        //                    {


        //                        if (e.DropPosition != DropPosition.Inside && (SelectedOrderCategory.Parent == null || UpdatedItem.Parent != SelectedOrderCategory.Parent))
        //                        {

        //                            ulong? UpdatedItemParent = UpdatedItem.Parent;
        //                            List<PCMArticleCategory> pcmArticleCategory_ForSetOrder_UpdatedItem = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItem.Parent).OrderBy(a => a.Position).ToList();
        //                            List<uint> indexCollection = new List<uint>();
        //                            pos = 1;
        //                            foreach (var updateArt in lstUpdateItem)
        //                            {
        //                                indexCollection.Add(updateArt.IdPCMArticleCategory);
        //                                 }
        //                            pos = 1;
        //                            foreach (PCMArticleCategory pcmArticleCategory_UpdatedItem in pcmArticleCategory_ForSetOrder_UpdatedItem)
        //                            {
        //                                if (!indexCollection.Contains(pcmArticleCategory_UpdatedItem.IdPCMArticleCategory))
        //                                {
        //                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory_UpdatedItem.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; });
        //                                }
        //                            }

        //                            if (SelectedOrderCategory.Parent == null || (e.DropPosition == DropPosition.Inside && SelectedOrderCategory.Parent != null))
        //                            {

        //                            }
        //                            else
        //                            {
        //                                pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();

        //                                switch (e.DropPosition)
        //                                {
        //                                    case DropPosition.Append:
        //                                        break;
        //                                    case DropPosition.Before:
        //                                        {
        //                                            foreach (var updateArt in lstUpdateItem)
        //                                            {

        //                                                updateArt.Parent = SelectedOrderCategory.Parent;
        //                                                updateArt.ParentName = SelectedOrderCategory.ParentName;
        //                                                updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                            }
        //                                        }
        //                                        break;
        //                                    case DropPosition.After:
        //                                        {
        //                                            tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                            foreach (var updateArt in lstUpdateItem)
        //                                            {

        //                                                updateArt.Parent = SelectedOrderCategory.Parent;
        //                                                updateArt.ParentName = SelectedOrderCategory.ParentName;
        //                                                updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                            }
        //                                        }
        //                                        break;
        //                                    case DropPosition.Inside:
        //                                        {

        //                                        }
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }


        //                            }


        //                            pos = 1;
        //                            foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                            {
        //                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });
        //                            }

        //                            if (pcmArticleCategory_ForSetOrder_UpdatedItem.Count > 0)
        //                            {
        //                                PCMArticleCategory_ParentChange = ArticleCategoryMenulist.Where(a => a.Parent == UpdatedItemParent).OrderBy(a => a.Position).ToList();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent

        //                                    ).OrderBy(a => a.Position).ToList();
        //                            if (UpdatedItem.Parent == SelectedOrderCategory.Parent)
        //                            {
        //                                if (SelectedOrderCategory.Position > UpdatedItem.Position)
        //                                {
        //                                    switch (e.DropPosition)
        //                                    {
        //                                        case DropPosition.Append:
        //                                            break;
        //                                        case DropPosition.Before:
        //                                            {

        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.After:
        //                                            {
        //                                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                                }

        //                                            }
        //                                            break;
        //                                        case DropPosition.Inside:
        //                                            {

        //                                            }
        //                                            break;
        //                                        default:
        //                                            break;
        //                                    }


        //                                }
        //                                else
        //                                {
        //                                    switch (e.DropPosition)
        //                                    {
        //                                        case DropPosition.Append:
        //                                            break;
        //                                        case DropPosition.Before:
        //                                            {

        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.After:
        //                                            {
        //                                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                                foreach (var updateArt in lstUpdateItem)
        //                                                {
        //                                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                                }


        //                                            }
        //                                            break;
        //                                        case DropPosition.Inside:
        //                                            {

        //                                            }
        //                                            break;
        //                                        default:
        //                                            break;
        //                                    }
        //                                }
        //                                if (e.DropPosition != DropPosition.Inside)
        //                                {
        //                                    pos = 1;
        //                                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                                    {
        //                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                                    }
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //            #endregion 
        //            else if (SelectedOrderCategory.Parent == null && UpdatedItem.Parent == null)
        //            {
        //                List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();

        //                if (SelectedOrderCategory.Position > UpdatedItem.Position)
        //                {
        //                    switch (e.DropPosition)
        //                    {
        //                        case DropPosition.Append:
        //                            break;
        //                        case DropPosition.Before:
        //                            {

        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                }


        //                            }
        //                            break;
        //                        case DropPosition.After:
        //                            {
        //                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    var aa = pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault());
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                }

        //                            }
        //                            break;
        //                        case DropPosition.Inside:
        //                            {

        //                            }
        //                            break;
        //                        default:
        //                            break;
        //                    }


        //                }
        //                else
        //                {
        //                    switch (e.DropPosition)
        //                    {
        //                        case DropPosition.Append:
        //                            break;
        //                        case DropPosition.Before:
        //                            {

        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == SelectedOrderCategory.IdPCMArticleCategory).FirstOrDefault()));// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);

        //                                }

        //                            }
        //                            break;
        //                        case DropPosition.After:
        //                            {
        //                                tmpIdPCMArticleCategoryIndexOf = SelectedOrderCategory.IdPCMArticleCategory;
        //                                foreach (var updateArt in lstUpdateItem)
        //                                {
        //                                    pcmArticleCategory_ForSetOrder.RemoveAt(Convert.ToInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == updateArt.IdPCMArticleCategory).FirstOrDefault())));
        //                                    updateArt.Position = Convert.ToUInt32(pcmArticleCategory_ForSetOrder.IndexOf(pcmArticleCategory_ForSetOrder.Where(x => x.IdPCMArticleCategory == tmpIdPCMArticleCategoryIndexOf).FirstOrDefault())) + 1;// SelectedOrderCategory.Position;                                                    
        //                                    pcmArticleCategory_ForSetOrder.Insert(Convert.ToInt32(updateArt.Position), updateArt);
        //                                    tmpIdPCMArticleCategoryIndexOf = updateArt.IdPCMArticleCategory;
        //                                }


        //                            }
        //                            break;
        //                        case DropPosition.Inside:
        //                            {


        //                            }
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                }
        //                if (e.DropPosition != DropPosition.Inside)
        //                {
        //                    pos = 1;
        //                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                    {
        //                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                    }
        //                }
        //            }
        //            if (UpdatedItem.Parent != null)
        //            {
        //                if (e.DropPosition == DropPosition.Inside)
        //                {
        //                    List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
        //                    pos = 1;
        //                    foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategory_ForSetOrder)
        //                    {
        //                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == pcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a => { a.Position = pos++; a.Parent = pcmArticleCategory.Parent; });

        //                    }
        //                    PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.IdPCMArticleCategory).OrderBy(a => a.Position).ToList();
        //                }
        //                else
        //                {
        //                    PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == SelectedOrderCategory.Parent).OrderBy(a => a.Position).ToList();
        //                }


        //                if (PCMArticleCategory_ParentChange.Count > 0)
        //                {
        //                    PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
        //                }
        //            }
        //            else if (UpdatedItem.Parent == null && SelectedOrderCategory.Parent == null)
        //            {
        //                PCMArticleCategory = ArticleCategoryMenulist.Where(a => a.Parent == null && a.Position != 0).OrderBy(a => a.Position).ToList();
        //                if (PCMArticleCategory_ParentChange.Count > 0)
        //                {
        //                    PCMArticleCategory.AddRange(PCMArticleCategory_ParentChange);
        //                }
        //            }

        //            if (e.IsFromOutside == false && typeof(PCMArticleCategory).IsAssignableFrom(e.GetRecordType()))
        //            {
        //                if (e.Data.GetDataPresent(typeof(RecordDragDropData)))
        //                {
        //                    var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
        //                    List<PCMArticleCategory> newRecords = data.Records.OfType<PCMArticleCategory>().Select(x => new PCMArticleCategory { Name = x.Name, KeyName = x.KeyName, Parent = x.Parent, IdPCMArticleCategory = x.IdPCMArticleCategory, ParentName = x.ParentName }).ToList();

        //                    PCMArticleCategory temp = newRecords.FirstOrDefault();
        //                    //if(e.DropPosition != DropPosition.Inside)
        //                    //{
        //                    //   
        //                    //}
        //                    PCMArticleCategory target_record = (PCMArticleCategory)e.TargetRecord;
        //                    if ((temp.Parent == null && target_record.Parent == null) || (temp.Parent != null && target_record.Parent != null) || target_record.Parent == null) // && temp.Parent == target_record.Parent
        //                    {
        //                        e.Effects = DragDropEffects.Move;
        //                        e.Handled = false;

        //                    }
        //                    else
        //                    {
        //                        e.Effects = DragDropEffects.None;
        //                        e.Handled = true;
        //                    }
        //                }
        //            }

        //            ////save data from darag and dropped data.
        //            if (PCMArticleCategory != null)
        //            {
        //                if (PCMArticleCategory.Count > 0)
        //                {

        //                    IsSave = PCMService.IsUpdatedPCMArticleCategoryOrder(PCMArticleCategory, (uint)GeosApplication.Instance.ActiveUser.IdUser);

        //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCategorySuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

        //                   // RequestClose(null, null);
        //                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptArticleCategoryAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            ConfirmationYesNo = false;

        //        }
        //        //
        //    }

        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method TreeListViewDropRecordDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private List<string> getIdsForRetriveWarehouseArticlesByParentClick()
        {
            List<string> ids = new List<string>();
            if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == SelectedWarehouseArticleCategory.IdArticleCategory))
            {
                List<ArticleCategories> getFirstList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == SelectedWarehouseArticleCategory.IdArticleCategory).ToList();
                foreach (ArticleCategories item1 in getFirstList)
                {
                    if (item1.Article_count_original != null)
                    {
                        ids.Add(item1.IdArticleCategory.ToString());
                    }
                    if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item1.IdArticleCategory))
                    {
                        List<ArticleCategories> getSecondList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item1.IdArticleCategory).ToList();
                        foreach (ArticleCategories item2 in getSecondList)
                        {
                            if (item2.Article_count_original != null)
                            {
                                ids.Add(item2.IdArticleCategory.ToString());
                            }
                            if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item2.IdArticleCategory))
                            {
                                List<ArticleCategories> getThirdList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item2.IdArticleCategory).ToList();
                                foreach (ArticleCategories item3 in getThirdList)
                                {
                                    if (item3.Article_count_original != null)
                                    {
                                        ids.Add(item3.IdArticleCategory.ToString());
                                    }
                                    if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item3.IdArticleCategory))
                                    {
                                        List<ArticleCategories> getForthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item3.IdArticleCategory).ToList();
                                        foreach (ArticleCategories item4 in getForthList)
                                        {
                                            if (item4.Article_count_original != null)
                                            {
                                                ids.Add(item4.IdArticleCategory.ToString());
                                            }
                                            if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item4.IdArticleCategory))
                                            {
                                                List<ArticleCategories> getFifthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item4.IdArticleCategory).ToList();
                                                foreach (ArticleCategories item5 in getFifthList)
                                                {
                                                    if (item5.Article_count_original != null)
                                                    {
                                                        ids.Add(item5.IdArticleCategory.ToString());
                                                    }
                                                    if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item5.IdArticleCategory))
                                                    {
                                                        List<ArticleCategories> getSixthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item5.IdArticleCategory).ToList();
                                                        foreach (ArticleCategories item6 in getSixthList)
                                                        {
                                                            if (item6.Article_count_original != null)
                                                            {
                                                                ids.Add(item6.IdArticleCategory.ToString());
                                                            }
                                                            if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item6.IdArticleCategory))
                                                            {
                                                                List<ArticleCategories> getSeventhList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item6.IdArticleCategory).ToList();
                                                                foreach (ArticleCategories item7 in getSeventhList)
                                                                {
                                                                    if (item7.Article_count_original != null)
                                                                    {
                                                                        ids.Add(item7.IdArticleCategory.ToString());
                                                                    }
                                                                    if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item7.IdArticleCategory))
                                                                    {
                                                                        List<ArticleCategories> getEightthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item7.IdArticleCategory).ToList();
                                                                        foreach (ArticleCategories item8 in getEightthList)
                                                                        {
                                                                            if (item8.Article_count_original != null)
                                                                            {
                                                                                ids.Add(item8.IdArticleCategory.ToString());
                                                                            }
                                                                            if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item8.IdArticleCategory))
                                                                            {
                                                                                List<ArticleCategories> getNinethList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item8.IdArticleCategory).ToList();
                                                                                foreach (ArticleCategories item9 in getNinethList)
                                                                                {
                                                                                    if (item9.Article_count_original != null)
                                                                                    {
                                                                                        ids.Add(item9.IdArticleCategory.ToString());
                                                                                    }
                                                                                    if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item9.IdArticleCategory))
                                                                                    {
                                                                                        List<ArticleCategories> gettenthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item9.IdArticleCategory).ToList();
                                                                                        foreach (ArticleCategories item10 in gettenthList)
                                                                                        {
                                                                                            if (item10.Article_count_original != null)
                                                                                            {
                                                                                                ids.Add(item10.IdArticleCategory.ToString());
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
        private List<string> getIdsForRetriveArticlesByParentClick()
        {
            List<string> ids = new List<string>();
            if (ArticleCategoryMenulist.Any(a => a.Parent == SelectedArticleCategory.IdPCMArticleCategory))
            {
                List<PCMArticleCategory> getFirstList = ArticleCategoryMenulist.Where(a => a.Parent == SelectedArticleCategory.IdPCMArticleCategory).ToList();
                foreach (PCMArticleCategory item1 in getFirstList)
                {
                    if (item1.Article_count_original != null)
                    {
                        ids.Add(item1.IdPCMArticleCategory.ToString());
                    }
                    if (ArticleCategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                    {
                        List<PCMArticleCategory> getSecondList = ArticleCategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                        foreach (PCMArticleCategory item2 in getSecondList)
                        {
                            if (item2.Article_count_original != null)
                            {
                                ids.Add(item2.IdPCMArticleCategory.ToString());
                            }
                            if (ArticleCategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                            {
                                List<PCMArticleCategory> getThirdList = ArticleCategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                foreach (PCMArticleCategory item3 in getThirdList)
                                {
                                    if (item3.Article_count_original != null)
                                    {
                                        ids.Add(item3.IdPCMArticleCategory.ToString());
                                    }
                                    if (ArticleCategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                    {
                                        List<PCMArticleCategory> getForthList = ArticleCategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                        foreach (PCMArticleCategory item4 in getForthList)
                                        {
                                            if (item4.Article_count_original != null)
                                            {
                                                ids.Add(item4.IdPCMArticleCategory.ToString());
                                            }
                                            if (ArticleCategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                            {
                                                List<PCMArticleCategory> getFifthList = ArticleCategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                foreach (PCMArticleCategory item5 in getFifthList)
                                                {
                                                    if (item5.Article_count_original != null)
                                                    {
                                                        ids.Add(item5.IdPCMArticleCategory.ToString());
                                                    }
                                                    if (ArticleCategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                    {
                                                        List<PCMArticleCategory> getSixthList = ArticleCategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                        foreach (PCMArticleCategory item6 in getSixthList)
                                                        {
                                                            if (item6.Article_count_original != null)
                                                            {
                                                                ids.Add(item6.IdPCMArticleCategory.ToString());
                                                            }
                                                            if (ArticleCategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                            {
                                                                List<PCMArticleCategory> getSeventhList = ArticleCategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                foreach (PCMArticleCategory item7 in getSeventhList)
                                                                {
                                                                    if (item7.Article_count_original != null)
                                                                    {
                                                                        ids.Add(item7.IdPCMArticleCategory.ToString());
                                                                    }
                                                                    if (ArticleCategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                    {
                                                                        List<PCMArticleCategory> getEightthList = ArticleCategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                        foreach (PCMArticleCategory item8 in getEightthList)
                                                                        {
                                                                            if (item8.Article_count_original != null)
                                                                            {
                                                                                ids.Add(item8.IdPCMArticleCategory.ToString());
                                                                            }
                                                                            if (ArticleCategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                            {
                                                                                List<PCMArticleCategory> getNinethList = ArticleCategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                foreach (PCMArticleCategory item9 in getNinethList)
                                                                                {
                                                                                    if (item9.Article_count_original != null)
                                                                                    {
                                                                                        ids.Add(item9.IdPCMArticleCategory.ToString());
                                                                                    }
                                                                                    if (ArticleCategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                    {
                                                                                        List<PCMArticleCategory> gettenthList = ArticleCategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
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
        private void UpdateWarehouseCategoryCount()
        {
            foreach (ArticleCategories item in WarehouseArticleCategoryMenulist)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item.IdArticleCategory))
                {
                    List<ArticleCategories> getFirstList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item.IdArticleCategory).ToList();
                    foreach (ArticleCategories item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item1.IdArticleCategory))
                        {
                            List<ArticleCategories> getSecondList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item1.IdArticleCategory).ToList();
                            foreach (ArticleCategories item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item2.IdArticleCategory))
                                {
                                    List<ArticleCategories> getThirdList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item2.IdArticleCategory).ToList();
                                    foreach (ArticleCategories item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item3.IdArticleCategory))
                                        {
                                            List<ArticleCategories> getForthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item3.IdArticleCategory).ToList();
                                            foreach (ArticleCategories item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item4.IdArticleCategory))
                                                {
                                                    List<ArticleCategories> getFifthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item4.IdArticleCategory).ToList();
                                                    foreach (ArticleCategories item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item5.IdArticleCategory))
                                                        {
                                                            List<ArticleCategories> getSixthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item5.IdArticleCategory).ToList();
                                                            foreach (ArticleCategories item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item6.IdArticleCategory))
                                                                {
                                                                    List<ArticleCategories> getSeventhList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item6.IdArticleCategory).ToList();
                                                                    foreach (ArticleCategories item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item7.IdArticleCategory))
                                                                        {
                                                                            List<ArticleCategories> getEightthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item7.IdArticleCategory).ToList();
                                                                            foreach (ArticleCategories item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item8.IdArticleCategory))
                                                                                {
                                                                                    List<ArticleCategories> getNinethList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item8.IdArticleCategory).ToList();
                                                                                    foreach (ArticleCategories item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (WarehouseArticleCategoryMenulist.Any(a => a.Parent == item9.IdArticleCategory))
                                                                                        {
                                                                                            List<ArticleCategories> gettenthList = WarehouseArticleCategoryMenulist.Where(a => a.Parent == item9.IdArticleCategory).ToList();
                                                                                            foreach (ArticleCategories item10 in gettenthList)
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
        private void UpdatePCMCategoryCount()
        {
            foreach (PCMArticleCategory item in ArticleCategoryMenulist)
            {
                int count = 0;
                if (item.Article_count_original != null)
                {
                    count = item.Article_count_original;
                }
                if (ArticleCategoryMenulist.Any(a => a.Parent == item.IdPCMArticleCategory))
                {
                    List<PCMArticleCategory> getFirstList = ArticleCategoryMenulist.Where(a => a.Parent == item.IdPCMArticleCategory).ToList();
                    foreach (PCMArticleCategory item1 in getFirstList)
                    {
                        if (item1.Article_count_original != null)
                        {
                            count = count + item1.Article_count_original;
                        }
                        if (ArticleCategoryMenulist.Any(a => a.Parent == item1.IdPCMArticleCategory))
                        {
                            List<PCMArticleCategory> getSecondList = ArticleCategoryMenulist.Where(a => a.Parent == item1.IdPCMArticleCategory).ToList();
                            foreach (PCMArticleCategory item2 in getSecondList)
                            {
                                if (item2.Article_count_original != null)
                                {
                                    count = count + item2.Article_count_original;
                                }
                                if (ArticleCategoryMenulist.Any(a => a.Parent == item2.IdPCMArticleCategory))
                                {
                                    List<PCMArticleCategory> getThirdList = ArticleCategoryMenulist.Where(a => a.Parent == item2.IdPCMArticleCategory).ToList();
                                    foreach (PCMArticleCategory item3 in getThirdList)
                                    {
                                        if (item3.Article_count_original != null)
                                        {
                                            count = count + item3.Article_count_original;
                                        }
                                        if (ArticleCategoryMenulist.Any(a => a.Parent == item3.IdPCMArticleCategory))
                                        {
                                            List<PCMArticleCategory> getForthList = ArticleCategoryMenulist.Where(a => a.Parent == item3.IdPCMArticleCategory).ToList();
                                            foreach (PCMArticleCategory item4 in getForthList)
                                            {
                                                if (item4.Article_count_original != null)
                                                {
                                                    count = count + item4.Article_count_original;
                                                }
                                                if (ArticleCategoryMenulist.Any(a => a.Parent == item4.IdPCMArticleCategory))
                                                {
                                                    List<PCMArticleCategory> getFifthList = ArticleCategoryMenulist.Where(a => a.Parent == item4.IdPCMArticleCategory).ToList();
                                                    foreach (PCMArticleCategory item5 in getFifthList)
                                                    {
                                                        if (item5.Article_count_original != null)
                                                        {
                                                            count = count + item5.Article_count_original;
                                                        }
                                                        if (ArticleCategoryMenulist.Any(a => a.Parent == item5.IdPCMArticleCategory))
                                                        {
                                                            List<PCMArticleCategory> getSixthList = ArticleCategoryMenulist.Where(a => a.Parent == item5.IdPCMArticleCategory).ToList();
                                                            foreach (PCMArticleCategory item6 in getSixthList)
                                                            {
                                                                if (item6.Article_count_original != null)
                                                                {
                                                                    count = count + item6.Article_count_original;
                                                                }
                                                                if (ArticleCategoryMenulist.Any(a => a.Parent == item6.IdPCMArticleCategory))
                                                                {
                                                                    List<PCMArticleCategory> getSeventhList = ArticleCategoryMenulist.Where(a => a.Parent == item6.IdPCMArticleCategory).ToList();
                                                                    foreach (PCMArticleCategory item7 in getSeventhList)
                                                                    {
                                                                        if (item7.Article_count_original != null)
                                                                        {
                                                                            count = count + item7.Article_count_original;
                                                                        }
                                                                        if (ArticleCategoryMenulist.Any(a => a.Parent == item7.IdPCMArticleCategory))
                                                                        {
                                                                            List<PCMArticleCategory> getEightthList = ArticleCategoryMenulist.Where(a => a.Parent == item7.IdPCMArticleCategory).ToList();
                                                                            foreach (PCMArticleCategory item8 in getEightthList)
                                                                            {
                                                                                if (item8.Article_count_original != null)
                                                                                {
                                                                                    count = count + item8.Article_count_original;
                                                                                }
                                                                                if (ArticleCategoryMenulist.Any(a => a.Parent == item8.IdPCMArticleCategory))
                                                                                {
                                                                                    List<PCMArticleCategory> getNinethList = ArticleCategoryMenulist.Where(a => a.Parent == item8.IdPCMArticleCategory).ToList();
                                                                                    foreach (PCMArticleCategory item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.Article_count_original != null)
                                                                                        {
                                                                                            count = count + item9.Article_count_original;
                                                                                        }
                                                                                        if (ArticleCategoryMenulist.Any(a => a.Parent == item9.IdPCMArticleCategory))
                                                                                        {
                                                                                            List<PCMArticleCategory> gettenthList = ArticleCategoryMenulist.Where(a => a.Parent == item9.IdPCMArticleCategory).ToList();
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
        private void deleteArticleAndManagedCategoryCount(Articles Article)
        {
            if (ArticleListForAddDeleteRetrive == null)
            {
                ArticleListForAddDeleteRetrive = new ObservableCollection<Articles>();
            }
            if (ArticleList_Cloned.Any(a => a.IdArticle == Article.IdArticle))
            {
                Article.TransactionOperation = ModelBase.TransactionOperations.Delete;
                ArticleListForAddDeleteRetrive.Add(Article);
                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == Article.IdPCMArticleCategory).ToList().ForEach(a =>
                {
                    a.Article_count = a.Article_count - 1;
                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                });
            }
            else
            {
                ArticleListForAddDeleteRetrive.Remove(ArticleList.FirstOrDefault(a => a.IdArticle == Article.IdArticle));
                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == Article.IdPCMArticleCategory).ToList().ForEach(a =>
                {
                    a.Article_count = a.Article_count - 1;
                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                });
            }
            //[001] Start
            if (SelectedArticleCategory.IdPCMArticleCategory == Article.IdPCMArticleCategory)
            {
                SelectedArticleCategory.NameWithArticleCount = SelectedArticleCategory.Name + "[" + SelectedArticleCategory.Article_count + "]";
                if (SelectedArticleCategory.ParentName != null)
                {
                    ArticleCategoryMenulist.Where(a => a.KeyName == SelectedArticleCategory.ParentName).ToList().ForEach(a =>
                    {
                        a.Article_count = a.Article_count - 1;
                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                    });
                    PCMArticleCategory parentrecord = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedArticleCategory.Parent);
                    if (parentrecord != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord.Parent))
                    {
                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord.Parent).ToList().ForEach(a =>
                        {
                            a.Article_count = a.Article_count - 1;
                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                        });
                        PCMArticleCategory parentrecord1 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord.Parent);
                        if (parentrecord1 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord1.Parent))
                        {
                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord1.Parent).ToList().ForEach(a =>
                            {
                                a.Article_count = a.Article_count - 1;
                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                            });
                            PCMArticleCategory parentrecord2 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord1.Parent);
                            if (parentrecord2 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord2.Parent))
                            {
                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord2.Parent).ToList().ForEach(a =>
                                {
                                    a.Article_count = a.Article_count - 1;
                                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                });
                                PCMArticleCategory parentrecord3 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord2.Parent);
                                if (parentrecord3 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord3.Parent))
                                {
                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord3.Parent).ToList().ForEach(a =>
                                    {
                                        a.Article_count = a.Article_count - 1;
                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                    });
                                    PCMArticleCategory parentrecord4 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord3.Parent);
                                    if (parentrecord4 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord4.Parent))
                                    {
                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord4.Parent).ToList().ForEach(a =>
                                        {
                                            a.Article_count = a.Article_count - 1;
                                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                        });
                                        PCMArticleCategory parentrecord5 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord4.Parent);
                                        if (parentrecord5 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord5.Parent))
                                        {
                                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord5.Parent).ToList().ForEach(a =>
                                            {
                                                a.Article_count = a.Article_count - 1;
                                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                    {
                        a.Article_count = a.Article_count - 1;
                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                    });
                }
                else
                {
                    ArticleCategoryMenulist.Where(a => a.ParentName == SelectedArticleCategory.KeyName && a.IdPCMArticleCategory == Article.PcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a =>
                    {
                        a.Article_count = a.Article_count - 1;
                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                    });
                    ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                    {
                        a.Article_count = a.Article_count - 1;
                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                    });
                }
            }
            else
            {
                foreach (PCMArticleCategory item in ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == Article.IdPCMArticleCategory).ToList())
                {
                    item.NameWithArticleCount = item.Name + "[" + item.Article_count + "]";
                    if (item.ParentName != null)
                    {
                        ArticleCategoryMenulist.Where(a => a.KeyName == item.ParentName).ToList().ForEach(a =>
                        {
                            a.Article_count = a.Article_count - 1;
                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                        });
                        PCMArticleCategory parentrecord = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == item.Parent);
                        if (parentrecord != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord.Parent))
                        {
                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord.Parent).ToList().ForEach(a =>
                            {
                                a.Article_count = a.Article_count - 1;
                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                            });
                            PCMArticleCategory parentrecord1 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord.Parent);
                            if (parentrecord1 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord1.Parent))
                            {
                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord1.Parent).ToList().ForEach(a =>
                                {
                                    a.Article_count = a.Article_count - 1;
                                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                });
                                PCMArticleCategory parentrecord2 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord1.Parent);
                                if (parentrecord2 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord2.Parent))
                                {
                                    ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord2.Parent).ToList().ForEach(a =>
                                    {
                                        a.Article_count = a.Article_count - 1;
                                        a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                    });
                                    PCMArticleCategory parentrecord3 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord2.Parent);
                                    if (parentrecord3 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord3.Parent))
                                    {
                                        ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord3.Parent).ToList().ForEach(a =>
                                        {
                                            a.Article_count = a.Article_count - 1;
                                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                        });
                                        PCMArticleCategory parentrecord4 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord3.Parent);
                                        if (parentrecord4 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord4.Parent))
                                        {
                                            ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord4.Parent).ToList().ForEach(a =>
                                            {
                                                a.Article_count = a.Article_count - 1;
                                                a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                            });
                                            PCMArticleCategory parentrecord5 = ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == parentrecord4.Parent);
                                            if (parentrecord5 != null && ArticleCategoryMenulist.Any(a => a.IdPCMArticleCategory == parentrecord5.Parent))
                                            {
                                                ArticleCategoryMenulist.Where(a => a.IdPCMArticleCategory == parentrecord5.Parent).ToList().ForEach(a =>
                                                {
                                                    a.Article_count = a.Article_count - 1;
                                                    a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                        {
                            a.Article_count = a.Article_count - 1;
                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                        });
                    }
                    else
                    {
                        ArticleCategoryMenulist.Where(a => a.ParentName == item.KeyName && a.IdPCMArticleCategory == Article.PcmArticleCategory.IdPCMArticleCategory).ToList().ForEach(a =>
                        {
                            a.Article_count = a.Article_count - 1;
                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                        });
                        ArticleCategoryMenulist.Where(a => a.KeyName == "Group_All").ToList().ForEach(a =>
                        {
                            a.Article_count = a.Article_count - 1;
                            a.NameWithArticleCount = a.Name + "[" + a.Article_count.ToString() + "]";
                        });
                    }
                }
            }
            //[001] End
            ArticleList_All.Remove(ArticleList.FirstOrDefault(a => a.IdArticle == Article.IdArticle));
            ArticleList.Remove(ArticleList.FirstOrDefault(a => a.IdArticle == Article.IdArticle));
            ArticleList = new ObservableCollection<Articles>(ArticleList);
        }

        private void DeleteCategoryAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCategoryAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteArticleCategoryMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SelectedArticleCategory.Article_count == 0)
                    {
                        List<PCMArticleCategory> tempList = new List<Data.Common.PCM.PCMArticleCategory>();
                        List<string> temp = getIdsForRetriveArticlesByParentClick();
                        tempList.Add(ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedArticleCategory.IdPCMArticleCategory));
                        ArticleCategoryMenulist.Remove(ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == SelectedArticleCategory.IdPCMArticleCategory));

                        foreach (string id in temp)
                        {
                            tempList.Add(ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
                            ArticleCategoryMenulist.Remove(ArticleCategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == Convert.ToUInt32(id)));
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

        private void SelectedItemChangedCommandAction(object obj)
        {
            var List = ArticleList.Where(x => x.IsChecked == true).ToList();
            var List1 = ArticleList.Where(x => x.IsChecked == true).ToList();
        }

        private void PasteInSearchControlCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PasteInSearchControlCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                var clipboardData = Clipboard.GetText();
                if (clipboardData != null)
                {
                    string[] rows = clipboardData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length > 0)
                    {
                        string search = string.Join(" ", rows.Select(a => a.ToString()));
                        tableView.SearchString = search;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method PasteInSearchControlCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on PasteInSearchControlCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

    //[30.11.2022][sshegaonkar][GEOS2-2718]
        private void ExpandAndCollapseArticleCategoryCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (isArticleCategoryExpand)
            {
                t.ExpandAllNodes();
                IsArticleCategoryExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsArticleCategoryExpand = true;
            }
        }

		//Shubham[skadam] GEOS2-5024 Improvements in PCM module 25 12 2023
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "Reference")
            {
                return;
            }
            try
            {
                List<object> filterItems = new List<object>();
                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //customComboBoxItem.DisplayValue = References;
                //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Reference IN '{0}'", References));
                CriteriaOperator criteria = new InOperator("Reference", WarehouseArticleReference);
                customComboBoxItem.EditValue = CriteriaOperator.And(criteria);
                filterItems.Add(customComboBoxItem);
                //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 21 12 2023
                //if (e.Column.FieldName == "Reference")
                //{
                //    if(!string.IsNullOrEmpty(References))
                //    {
                //        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == References))
                //        {
                //            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //            customComboBoxItem.DisplayValue = References;
                //            //customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Reference IN '{0}'", References));
                //            CriteriaOperator criteria = new InOperator("Reference", WarehouseArticleReference);
                //            customComboBoxItem.EditValue = CriteriaOperator.And(criteria);
                //            filterItems.Add(customComboBoxItem);
                //        }
                //    }

                //}
                #region OldCode
                //if (e.Column.FieldName == "Country")
                //{
                //    foreach (var dataObject in WarehouseArticleList)
                //    {
                //        if (string.IsNullOrEmpty(dataObject.Reference))
                //        {
                //            continue;
                //        }
                //        else if (!string.IsNullOrEmpty(dataObject.Reference))
                //        {

                //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == dataObject.Reference))
                //            {
                //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                customComboBoxItem.DisplayValue = dataObject.Reference;
                //                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Reference Like '%{0}%'", dataObject.Reference));
                //                filterItems.Add(customComboBoxItem);
                //            }
                //            else
                //                continue;
                //        }
                //        else
                //        {
                //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == WarehouseArticleList.Where(y => y.Reference == dataObject.Reference).Select(slt => slt.Reference).FirstOrDefault().Trim()))
                //            {
                //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                customComboBoxItem.DisplayValue = WarehouseArticleList.Where(y => y.Reference == dataObject.Reference).Select(slt => slt.Reference).FirstOrDefault().Trim();
                //                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Reference Like '%{0}%'", WarehouseArticleList.Where(y => y.Reference == dataObject.Reference).Select(slt => slt.Reference).FirstOrDefault().Trim()));
                //                filterItems.Add(customComboBoxItem);
                //            }
                //        }
                //    }
                //}
                #endregion
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

    }
}
