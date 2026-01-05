using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Views;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.PCM;
using DevExpress.XtraGauges.Core.Model;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using DevExpress.Mvvm.Native;


namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class InspectionByCategoryViewModel : ModelBase,INotifyPropertyChanged, IDisposable
    {      
        //[rdixit][GEOS2-5906][13.11.2024]
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Declaration
        bool isExpand;
        bool allowDragDrop;
        int flag;
        private double gridHeight;
        private string windowHeader;
        private string name;
        private string error = string.Empty;
        private string searchString;
        private int windowHeight;
        private int windowWidth;
        private ObservableCollection<ArticleCategories> warehouseArticleCategoryMenulist;
        private ArticleCategories selectedWarehouseArticleCategory;
        ObservableCollection<ProductInspectionReworkCauses> reworkCausesByArticleCategorylist;
        ProductInspectionReworkCauses selectedReworkCausesByArticleCategory;
        ObservableCollection<ProductInspectionReworkCauses> reworkCausesList;
        ProductInspectionReworkCauses selectedReworkCauses;
        bool isDeleted;
        #endregion

        #region Properties 
        int Index { get; set; }
        ObservableCollection<ProductInspectionReworkCauses> AllReworkCausesList { get; set; }
        public bool IsExpand
        {
            get
            {
                return isExpand;
            }

            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpand"));
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
        public int WindowWidth
        {
            get
            {
                return windowWidth;
            }

            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
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
        public ObservableCollection<ProductInspectionReworkCauses> ReworkCausesByArticleCategorylist
        {
            get
            {
                return reworkCausesByArticleCategorylist;
            }

            set
            {
                reworkCausesByArticleCategorylist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReworkCausesByArticleCategorylist"));
            }
        }
        public ProductInspectionReworkCauses SelectedReworkCausesByArticleCategory
        {
            get
            {
                return selectedReworkCausesByArticleCategory;
            }

            set
            {
                selectedReworkCausesByArticleCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReworkCausesByArticleCategory"));
            }
        }
        public ProductInspectionReworkCauses SelectedReworkCauses
        {
            get
            {
                return selectedReworkCauses;
            }

            set
            {
                selectedReworkCauses = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReworkCauses"));
            }
        }
        public ObservableCollection<ProductInspectionReworkCauses> ReworkCausesList
        {
            get
            {
                return reworkCausesList;
            }

            set
            {
                reworkCausesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReworkCausesList"));
            }
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
        public bool AllowDragDrop
        {
            get
            {
                return allowDragDrop;
            }

            set
            {
                allowDragDrop = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowDragDrop"));
            }
        }
        

        #endregion

        #region Command
        public ICommand SelectWarehouseArticleCategoryCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteReworkCauseCommand { get; set; }
        public ICommand ExpandAndCollapseCommand { get; set; }
        public ICommand CommandOnDragRecordOver1 { get; set; }
        public ICommand CommandCompleteRecordDragDrop1 { get; set; }
        public ICommand CommandTreeListViewDropRecord1 { get; set; }
        public ICommand CommandCompleteRecordDragDropFromReworkCauses { get; set; }
        public ICommand CommandNewreworkClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }

        
        #endregion

        #region  Constructor
        public InspectionByCategoryViewModel()
        {

            try
            {
                LoadingScreen();
                GeosApplication.Instance.Logger.Log("Constructor InspectionByCategoryViewModel()...", category: Category.Info, priority: Priority.Low);
                CancelCommand = new RelayCommand(new Action<object>(CloseWindow));
                DeleteReworkCauseCommand = new RelayCommand(new Action<object>(DeleteReworkCauseForArticleCategoryItem));
                SelectWarehouseArticleCategoryCommand = new RelayCommand(new Action<object>(RetrieveWarehouseArticlesByCategory));                              
                ExpandAndCollapseCommand = new DelegateCommand<object>(ExpandAndCollapseCommandAction);
                CommandOnDragRecordOver1 = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOver);
                CommandCompleteRecordDragDrop1 = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropAction);
                CommandTreeListViewDropRecord1= new DelegateCommand<DropRecordEventArgs>(DropRecordRework);
                CommandCompleteRecordDragDropFromReworkCauses = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropFromReworkCauses);
                CommandNewreworkClick = new DelegateCommand<object>(AddNewReworkCauseCommandAction);
                CommandGridDoubleClick = new DelegateCommand<object>(EditNewReworkCauseCommandAction);
                var screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                var screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                WindowHeight = screenHeight - 350;           
                WindowWidth = screenWidth - 150;
                GridHeight = WindowHeight - 90;
                IsExpand = false;
                AllowDragDrop = GeosApplication.Instance.IsWMSManageInspectionPoints;
                ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>();
                FillWarehouseArticleCatagoryList();                
                FillAllReworkCauses();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor InspectionByCategoryViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor InspectionByCategoryViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void LoadingScreen()
        {
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
        }
        private void EditNewReworkCauseCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction ...", category: Category.Info, priority: Priority.Low);
                AddReworkCauseViewModel addReworkCauseViewModel = new AddReworkCauseViewModel();
                AddReworkCauseView addReworkCauseView = new AddReworkCauseView();
                EventHandler handle = delegate { addReworkCauseView.Close(); };
                addReworkCauseViewModel.RequestClose += handle;
                addReworkCauseView.DataContext = addReworkCauseViewModel;
                addReworkCauseViewModel.NewReworkCause = SelectedReworkCauses;
                addReworkCauseViewModel.Name = SelectedReworkCauses.ReworkCause;
                addReworkCauseViewModel.NewReworkCause.TransactionOperation = TransactionOperations.Modify;
                addReworkCauseView.ShowDialogWindow();

                if (addReworkCauseViewModel.IsSave)
                {
                    var reworkCauseItem = ReworkCausesList.FirstOrDefault(i => i.IdReworkCause == SelectedReworkCauses.IdReworkCause);
                    if (reworkCauseItem != null)
                    {
                        reworkCauseItem.ReworkCause = addReworkCauseViewModel.Name;
                    }

                    var allReworkCauseItem = AllReworkCausesList.FirstOrDefault(i => i.IdReworkCause == SelectedReworkCauses.IdReworkCause);
                    if (allReworkCauseItem != null)
                    {
                        allReworkCauseItem.ReworkCause = addReworkCauseViewModel.Name;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void AddNewReworkCauseCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction ...", category: Category.Info, priority: Priority.Low);

                AddReworkCauseViewModel addReworkCauseViewModel = new AddReworkCauseViewModel();
                AddReworkCauseView addReworkCauseView = new AddReworkCauseView();
                EventHandler handle = delegate { addReworkCauseView.Close(); };
                addReworkCauseViewModel.RequestClose += handle;             
                addReworkCauseView.DataContext = addReworkCauseViewModel;
                addReworkCauseView.ShowDialogWindow();

                if (addReworkCauseViewModel.IsSave)
                {                
                    ReworkCausesList.Add(addReworkCauseViewModel.NewReworkCause.Clone() as ProductInspectionReworkCauses);
                    AllReworkCausesList.Add(addReworkCauseViewModel.NewReworkCause.Clone() as ProductInspectionReworkCauses);                  
                }
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void CompleteRecordDragDropFromReworkCauses(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor CompleteRecordDragDropFromReworkCauses()...", category: Category.Info, priority: Priority.Low);
             
                if (e.Records != null && e.Records.Any() && SelectedWarehouseArticleCategory != null)
                {                  
                    if (ReworkCausesByArticleCategorylist != null)
                    {
                        for (int i = 0; i < ReworkCausesByArticleCategorylist.Count; i++)
                        {
                            ReworkCausesByArticleCategorylist[i].Position = i + 1;
                            ReworkCausesByArticleCategorylist[i].IdArticleCategory = SelectedWarehouseArticleCategory.IdArticleCategory;
                        }
                        long index = ReworkCausesByArticleCategorylist.FirstOrDefault(i => i.IdReworkCause == ((ProductInspectionReworkCauses)e.Records[0]).IdReworkCause).Position;                   
                        var ListToUpdate = ReworkCausesByArticleCategorylist.Where(i => i.Position >= index).ToList();
                        if (ListToUpdate?.Count > 0)
                            WarehouseService.AddUpdateReworkListByArticleCategory_V2580(ListToUpdate);
                    }
                    if (ReworkCausesList != null && e.Records != null)
                    {
                        ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(
                            ReworkCausesList.Where(item => !e.Records.Any(record => (record as ProductInspectionReworkCauses)?.IdReworkCause == item?.IdReworkCause)).ToList()
                        );
                    }                    
                }
           
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFromReworkCauses()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CompleteRecordDragDropFromReworkCauses() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CompleteRecordDragDropFromReworkCauses() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method CompleteRecordDragDropFromReworkCauses() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }

        }
        private void CompleteRecordDragDropAction(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor CompleteRecordDragDropAction()...", category: Category.Info, priority: Priority.Low);
                if (e.Records != null && e.Records.Any())
                {
                    if (ReworkCausesByArticleCategorylist != null)
                    {
                        ReworkCausesByArticleCategorylist.ToList().ForEach(i => i.IdArticleCategory = SelectedWarehouseArticleCategory.IdArticleCategory);
                        if (ReworkCausesByArticleCategorylist?.Count > 0)
                            WarehouseService.AddUpdateReworkListByArticleCategory_V2580(ReworkCausesByArticleCategorylist.ToList());
                    }
                    if (ReworkCausesList != null && e.Records != null)
                    {
                        ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(
                            ReworkCausesList.Where(item => !e.Records.Any(record => (record as ProductInspectionReworkCauses)?.IdReworkCause == item?.IdReworkCause)).ToList()
                        );
                    }
                }
                e.Handled = true;
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExpandAndCollapseCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExpandAndCollapseCommandAction()...", category: Category.Info, priority: Priority.Low);
                TreeListView t = (TreeListView)obj;
                if (IsExpand)
                {
                    t.ExpandAllNodes();
                    IsExpand = false;
                }
                else
                {
                    t.CollapseAllNodes();
                    IsExpand = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ExpandAndCollapseCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnDragRecordOver(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOver()...", category: Category.Info, priority: Priority.Low);
                if (SelectedWarehouseArticleCategory != null)
                {
                    if ((e.IsFromOutside) && e.TargetRecord != null && typeof(ProductInspectionReworkCauses).IsAssignableFrom(e.GetRecordType()))
                    {
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                    }
                    else if ((e.IsFromOutside) && typeof(ProductInspectionReworkCauses).IsAssignableFrom(e.GetRecordType()) && e.TargetRecord == null)
                    {
                        e.Effects = DragDropEffects.Move;
                        e.Handled = false;
                    }
                }
             
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOver()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOver() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }  
        private void DropRecordRework(DropRecordEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DropRecordRework()...", category: Category.Info, priority: Priority.Low);
                Index = 0;
                if (e.IsFromOutside == false)
                {
                    LoadingScreen();
                    var tab = e.OriginalSource as TableView;
                    var selecteditem = tab.FocusedRow as ProductInspectionReworkCauses;
                    int selectedItemIndex = ReworkCausesByArticleCategorylist.IndexOf(selecteditem);
                    int listCount = ReworkCausesByArticleCategorylist.Count;

                    if (selectedItemIndex > e.TargetRowHandle)  // Moving up
                    {
                        if (listCount > 1)
                        {
                            if (e.DropPosition == DropPosition.Before)
                            {
                                // Adjust positions for items between the target and selected index
                                for (int i = e.TargetRowHandle; i < selectedItemIndex; i++)
                                {
                                    ReworkCausesByArticleCategorylist[i].Position = i + 2;
                                }
                                // Update the selected item's new position
                                ReworkCausesByArticleCategorylist
                                    .FirstOrDefault(i => i.IdReworkCause == selecteditem.IdReworkCause)
                                    .Position = e.TargetRowHandle + 1;
                            }
                            else if (e.DropPosition == DropPosition.After)
                            {
                                for (int i = e.TargetRowHandle + 1; i < selectedItemIndex; i++)
                                {
                                    ReworkCausesByArticleCategorylist[i].Position = i + 2;
                                }
                                // Update the selected item's new position
                                ReworkCausesByArticleCategorylist
                                    .FirstOrDefault(i => i.IdReworkCause == selecteditem.IdReworkCause)
                                    .Position = Math.Min(listCount, e.TargetRowHandle + 2);
                            }
                        }
                    }
                    else  // Moving down
                    {
                        if (listCount > 1)
                        {
                            if (e.DropPosition == DropPosition.Before)
                            {
                                // Adjust positions for items between the selected and target index
                                for (int i = selectedItemIndex + 1; i <= e.TargetRowHandle; i++)
                                {
                                    ReworkCausesByArticleCategorylist[i].Position = i;
                                }
                                // Update the selected item's new position
                                ReworkCausesByArticleCategorylist
                                    .FirstOrDefault(i => i.IdReworkCause == selecteditem.IdReworkCause)
                                    .Position = e.TargetRowHandle + 1;
                            }
                            else if (e.DropPosition == DropPosition.After)
                            {
                                for (int i = selectedItemIndex + 1; i <= e.TargetRowHandle; i++)
                                {
                                    ReworkCausesByArticleCategorylist[i].Position = i;
                                }
                                // Update the selected item's new position to the end if moving to the last position
                                ReworkCausesByArticleCategorylist
                                    .FirstOrDefault(i => i.IdReworkCause == selecteditem.IdReworkCause)
                                    .Position = Math.Min(listCount, e.TargetRowHandle + 1);
                            }
                        }
                    }

                    ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>(ReworkCausesByArticleCategorylist.OrderBy(i => i.Position).ToList());
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DropRecordRework() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveWarehouseArticlesByCategory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveWarehouseArticlesByCategory()...", category: Category.Info, priority: Priority.Low);
                LoadingScreen();
                ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(AllReworkCausesList.Select(i => i.Clone() as ProductInspectionReworkCauses));
                if (SelectedWarehouseArticleCategory != null)
                {
                    ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>(WarehouseService.GetReworkCausesbyArticleCategory(SelectedWarehouseArticleCategory.IdArticleCategory));
                    if (ReworkCausesByArticleCategorylist != null)
                    {
                        ReworkCausesByArticleCategorylist.ForEach(i=>i.TransactionOperation = TransactionOperations.Modify);
                        SelectedReworkCausesByArticleCategory = ReworkCausesByArticleCategorylist.FirstOrDefault();
                        ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(ReworkCausesList.Where(i => !ReworkCausesByArticleCategorylist.Any(j => j.IdReworkCause == i.IdReworkCause)).ToList());
                    }
                    else
                    {
                        ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>();
                        SelectedReworkCausesByArticleCategory = new ProductInspectionReworkCauses();
                    }
                }
                else
                {
                    ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>();
                    SelectedReworkCausesByArticleCategory = new ProductInspectionReworkCauses();
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
        private void DeleteReworkCauseForArticleCategoryItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteReworkCauseForArticleCategoryItem()...", category: Category.Info, priority: Priority.Low);             
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteAticleCategoryRework"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = WarehouseService.IsDeletedReworkCauseForArticleCategory_V2580(SelectedReworkCausesByArticleCategory);
                    if (IsDeleted)
                    {
                        ReworkCausesList.Add((ProductInspectionReworkCauses)SelectedReworkCausesByArticleCategory.Clone());
                        ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>(ReworkCausesByArticleCategorylist
                            .Where(i=>i.IdReworkCause != SelectedReworkCausesByArticleCategory.IdReworkCause).ToList());

                        if (ReworkCausesByArticleCategorylist?.Count > 0)
                        {
                            for (int i = 0; i < ReworkCausesByArticleCategorylist.Count; i++)
                            {
                                ReworkCausesByArticleCategorylist[i].Position = i + 1;
                            }
                            var ListToUpdate = ReworkCausesByArticleCategorylist.Where(i => i.Position >= SelectedReworkCausesByArticleCategory.Position).ToList();
                            if (ListToUpdate?.Count > 0)
                                WarehouseService.AddUpdateReworkListByArticleCategory_V2580(ListToUpdate);
                        }
                        ReworkCausesByArticleCategorylist = new ObservableCollection<ProductInspectionReworkCauses>(ReworkCausesByArticleCategorylist);
                        SelectedReworkCausesByArticleCategory = ReworkCausesByArticleCategorylist.FirstOrDefault();
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("DeleteAticleCategoryRework").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteReworkCauseForArticleCategoryItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteReworkCauseForArticleCategoryItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteReworkCauseForArticleCategoryItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteReworkCauseForArticleCategoryItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillWarehouseArticleCatagoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseArticleCatagoryList()...", category: Category.Info, priority: Priority.Low);
                WarehouseArticleCategoryMenulist = new ObservableCollection<ArticleCategories>(PCMService.GetActiveArticleCategories());             
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
        private void FillAllReworkCauses()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllReworkCauses()...", category: Category.Info, priority: Priority.Low);
                AllReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(WarehouseService.GetAllReworkCauses());
                ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>();
                //ReworkCausesList = new ObservableCollection<ProductInspectionReworkCauses>(AllReworkCausesList.Select(i => i.Clone() as ProductInspectionReworkCauses));
                GeosApplication.Instance.Logger.Log("Method FillAllReworkCauses()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAllReworkCauses() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAllReworkCauses() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAllReworkCauses() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {        
            RequestClose(null, null);
        }   
        #endregion
    }
}
