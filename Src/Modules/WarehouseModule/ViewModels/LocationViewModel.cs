using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class LocationViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //WMS M051-26	Locations not filtered by warehouse [adadibathina]
        //[WMS-M054-02]	New option Refill in warehouse locations [adadibathina]
        // [GEOS2-1477] [Allow to save current grid configuration in Locations section][avpawar]
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private List<WarehouseLocation> warehouseLocationsList;
        private ObservableCollection<Article> locationsList;
        private ObservableCollection<Article> filterWiseListOfLocations;
        private string myFilterString;
        private Article selectedLocation;
        private ObservableCollection<TileBarFilters> firstLevelLocationListOfTile = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<TileBarFilters> secondLevelLocationListOfTile = new ObservableCollection<TileBarFilters>();

        private int firstLevelLocationSelectedTileIndex;
        private int secondLevelLocationSelectedTileIndex;
        private string firstLevelLocationCaption;
        private string secondLevelLocationCaption;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private List<GridColumn> GridColumnList;
        private bool isEdit;
        private TileBarFilters selectedTileBarItem;
        string customFilterString;
        string filter;
      //  string filter1;
        public string LocationGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "LocationGridSetting.Xml";
        private bool isLocationColumnChooserVisible;
        #endregion

        #region Properties
        public int SecondLevelLocationSelectedTileIndex
        {
            get { return secondLevelLocationSelectedTileIndex; }
            set
            {
                secondLevelLocationSelectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SecondLevelLocationSelectedTileIndex"));
            }
        }
        public int FirstLevelLocationSelectedTileIndex
        {
            get { return firstLevelLocationSelectedTileIndex; }
            set
            {
                firstLevelLocationSelectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstLevelLocationSelectedTileIndex"));
            }
        }
        public ObservableCollection<TileBarFilters> FirstLevelLocationListOfTile
        {
            get { return firstLevelLocationListOfTile; }
            set
            {
                firstLevelLocationListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstLevelLocationListOfTile"));
            }
        }
        public ObservableCollection<TileBarFilters> SecondLevelLocationListOfTile
        {
            get { return secondLevelLocationListOfTile; }
            set
            {
                secondLevelLocationListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SecondLevelLocationListOfTile"));
            }
        }
        public List<WarehouseLocation> WarehouseLocationsList
        {
            get
            {
                return warehouseLocationsList;
            }

            set
            {
                warehouseLocationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationsList"));
            }
        }


        public ObservableCollection<Article> LocationsList
        {
            get
            {
                return locationsList;
            }

            set
            {
                locationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationsList"));
            }
        }

        public ObservableCollection<Article> FilterWiseListOfLocations
        {
            get
            {
                return filterWiseListOfLocations;
            }

            set
            {
                filterWiseListOfLocations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfLocations"));
            }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public Article SelectedLocation
        {
            get
            {
                return selectedLocation;
            }

            set
            {
                selectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation"));
            }
        }

        public string FirstLevelLocationCaption
        {
            get
            {
                return firstLevelLocationCaption;
            }

            set
            {
                firstLevelLocationCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstLevelLocationCaption"));
            }
        }

        public string SecondLevelLocationCaption
        {
            get
            {
                return secondLevelLocationCaption;
            }

            set
            {
                secondLevelLocationCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SecondLevelLocationCaption"));
            }
        }
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }
        //public string CustomFilterString
        //{
        //    get { return customFilterString; }
        //    set
        //    {
        //        customFilterString = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterString"));
        //    }
        //}
        public long WarehouseId { get; set; }
        public string FilterStringName { get; set; }
        public string FilterStringCriteria { get; set; }
        public bool IsLocationColumnChooserVisible
        {
            get
            {
                return isLocationColumnChooserVisible;
            }

            set
            {
                isLocationColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationColumnChooserVisible"));
            }
        }
        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Public Commands
        public ICommand RefreshLocationViewCommand { get; set; }
        public ICommand PrintLocationViewCommand { get; set; }
        public ICommand PrintLocationLabelCommand { get; set; }
        public ICommand ExportLocationViewCommand { get; set; }
        public ICommand CommandWarehouseEditValueChanged { get; private set; }
        public ICommand CommandFirstLevelLocationTileClick { get; set; }
        public ICommand CommandSecondLevelLocationTileClick { get; set; }
        public ICommand LocationViewReferenceHyperlinkClickCommand { get; set; }
        public ICommand CustomFilterEditorCreateCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }

        //M054-02
        public ICommand RefillCommand { get; set; }
        #endregion

        #region Constructor
        public LocationViewModel()
        {
            try
            {
                              
                GeosApplication.Instance.Logger.Log("Constructor LocationViewModel()...", category: Category.Info, priority: Priority.Low);
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
                RefreshLocationViewCommand = new RelayCommand(new Action<object>(RefreshLocationList));
                PrintLocationViewCommand = new RelayCommand(new Action<object>(PrintLocationList));
                PrintLocationLabelCommand = new RelayCommand(new Action<object>(PrintLocationLabelList));
                ExportLocationViewCommand = new RelayCommand(new Action<object>(ExportLocationList));
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);
                CommandFirstLevelLocationTileClick = new DelegateCommand<object>(CommandFirstLevelLocationTileClickAction);
                CommandSecondLevelLocationTileClick = new DelegateCommand<object>(CommandSecondLevelLocationTileClickAction);
                LocationViewReferenceHyperlinkClickCommand = new DevExpress.Mvvm.DelegateCommand<object>(LocationViewReferenceHyperlinkClickCommandAction);
                RefillCommand = new RelayCommand(new Action<object>(LocationRefill));
                CustomFilterEditorCreateCommand = new DelegateCommand<FilterEditorEventArgs>(CreateCustomFilterEditor);
                CommandTileBarDoubleClick = new DelegateCommand<object>(TileBarDoubleClick);
                CustomCellAppearanceCommand = new DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                FillLocationDetails();
                FillLocationsList();
                AddCustomFilter();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor LocationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor LocationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Method to get selected Second Level Location
        /// </summary>
        /// <param name="obj"></param>
        private void CommandSecondLevelLocationTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandSecondLevelLocationTileClickAction....", category: Category.Info, priority: Priority.Low);
                SecondLevelLocationCaption = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                string FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                FilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                FillFilterStingOfLocations();

                GeosApplication.Instance.Logger.Log("Method CommandSecondLevelLocationTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandSecondLevelLocationTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get selected First Level Location
        /// </summary>
        /// <param name="obj"></param>
        private void CommandFirstLevelLocationTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFirstLevelLocationTileClickAction....", category: Category.Info, priority: Priority.Low);
                FirstLevelLocationCaption = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;


                string FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                FilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                

                if (string.Equals(FilterStringName, System.Windows.Application.Current.FindResource("LocationViewCustomFilter").ToString()))
                {
                    FilterStringName = string.Empty;
                    return;
                }

                FillFilterStingOfLocations();

                if (!string.IsNullOrEmpty(FilterString) && FilterString.Equals(System.Windows.Application.Current.FindResource("LocationViewCustomFilter").ToString()))
                {
                    return;
                }

                else if (FilterString != null && !string.IsNullOrEmpty(FilterString))
                {
                    MyFilterString = FilterString;
                    FilterStringCriteria = FilterString;
                }

                else
                {
                    if(FirstLevelLocationCaption != null && SecondLevelLocationCaption !=null)
                    {
                        if (FirstLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                        {
                            MyFilterString = string.Empty;
                        }

                        else if (FirstLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                        {
                            MyFilterString = "StartsWith([ArticleWarehouseLocation.WarehouseLocation.FullName], '" + FirstLevelLocationCaption + "')";
                        }

                        else if (FirstLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                        {
                            MyFilterString = "StartsWith([ArticleWarehouseLocation.WarehouseLocation.FullName], '" + FirstLevelLocationCaption + "-" + SecondLevelLocationCaption + "')";
                        }

                        else
                        {
                            MyFilterString = "[ArticleWarehouseLocation.WarehouseLocation.FullName] like '%-" + SecondLevelLocationCaption + "-%'";
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CommandFirstLevelLocationTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandFirstLevelLocationTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillFilterStingOfLocations()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFilterStingOfLocations....", category: Category.Info, priority: Priority.Low);

                if (FirstLevelLocationCaption != null && SecondLevelLocationCaption != null)
                {

                    if (FirstLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                    {
                        MyFilterString = string.Empty; 
                    }

                    else if (FirstLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption.Equals((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                    {                    
                        MyFilterString = "StartsWith([ArticleWarehouseLocation.WarehouseLocation.FullName], '" + FirstLevelLocationCaption + "')";
                    }

                    else if (FirstLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())) && SecondLevelLocationCaption != ((System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString())))
                    {
                        MyFilterString = "StartsWith([ArticleWarehouseLocation.WarehouseLocation.FullName], '" + FirstLevelLocationCaption + "-" + SecondLevelLocationCaption + "')";
                    }

                    else
                    {
                        MyFilterString = "[ArticleWarehouseLocation.WarehouseLocation.FullName] like '%-" + SecondLevelLocationCaption + "-%'";

                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillFilterStingOfLocations....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillFilterStingOfLocations...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method LocationPopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);
            //When setting the warehouse from default the data should not be refreshed
            if (!WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;
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
            FillLocationDetails();
            FillLocationsList();
            AddCustomFilter();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method LocationPopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to get Location Details
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillLocationDetails()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationDetails...", category: Category.Info, priority: Priority.Low);


                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    ObservableCollection<Article> TempLocationList = new ObservableCollection<Article>();
                    LocationsList = new ObservableCollection<Article>();
                    Warehouses Warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                    try
                    {
                        // [001] Changed Service method
                        LocationsList.AddRange(new ObservableCollection<Article>(WarehouseService.GetAllArticlesWithWarehouseLocations_V2041(Warehouse)));
                        SelectedLocation = LocationsList.FirstOrDefault();
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillLocationDetails() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillLocationDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }

                }

                else
                {
                    LocationsList = new ObservableCollection<Article>();
                }

                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillLocationDetails executed successfully.", category: Category.Info, priority: Priority.Low);
            }


            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationDetails() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to Refresh Location List
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshLocationList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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


                FillLocationDetails();
                FillLocationsList();
                AddCustomFilter();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Print Location Label List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintLocationLabelList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLocationLabelList....", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                ObservableCollection<Article> ArticlesList = new ObservableCollection<Article>(gridControl.DataController.GetAllFilteredAndSortedRows().Cast<Article>());

                PrintArticleLocationLabelView printArticleLabelView = new PrintArticleLocationLabelView();
                PrintArticleLocationLabelViewModel printArticleLabelViewModel = new PrintArticleLocationLabelViewModel();
                EventHandler handle = delegate { printArticleLabelView.Close(); };
                printArticleLabelViewModel.RequestClose += handle;
                printArticleLabelViewModel.Init(ArticlesList);
                printArticleLabelView.DataContext = printArticleLabelViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                printArticleLabelView.Owner = Window.GetWindow(ownerInfo);
                printArticleLabelView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method PrintLocationLabelList....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLocationLabelList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            //try
            //{
            //    GeosApplication.Instance.Logger.Log("Method PrintLocationLabelList()...", category: Category.Info, priority: Priority.Low);

            //    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            //    {
            //        DXSplashScreen.Show(x =>
            //        {
            //            Window win = new Window()
            //            {
            //                ShowActivated = false,
            //                WindowStyle = WindowStyle.None,
            //                ResizeMode = ResizeMode.NoResize,
            //                AllowsTransparency = true,
            //                Background = new SolidColorBrush(Colors.Transparent),
            //                ShowInTaskbar = false,
            //                Topmost = true,
            //                SizeToContent = SizeToContent.WidthAndHeight,
            //                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //            };
            //            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
            //            win.Topmost = false;
            //            return win;
            //        }, x =>
            //        {
            //            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
            //        }, null, null);
            //    }

            //    PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
            //    pcl.Margins.Bottom = 5;
            //    pcl.Margins.Top = 5;
            //    pcl.Margins.Left = 5;
            //    pcl.Margins.Right = 5;
            //    pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LocationListReportPrintHeaderTemplate"];
            //    pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LocationListReportPrintFooterTemplate"];
            //    pcl.Landscape = true;
            //    pcl.CreateDocument(false);

            //    DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
            //    window.PreviewControl.DocumentSource = pcl;

            //    window.Show();

            //    DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
            //    printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

            //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            //    GeosApplication.Instance.Logger.Log("Method PrintLocationLabelList()....executed successfully", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method PrintLocationLabelList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }


        /// <summary>
        /// Method to Print Location List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLocationList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LocationListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LocationListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;

                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// Method to Export Location List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLocationList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Location List";
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
                    TableView locationTableView = ((TableView)obj);
                    locationTableView.ShowTotalSummary = false;
                    locationTableView.ShowFixedTotalSummary = false;
                    locationTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    locationTableView.ShowTotalSummary = true;

                    locationTableView.ShowFixedTotalSummary = true;

                    GeosApplication.Instance.Logger.Log("Method ExportLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to fill First Level Location List and Second Level Location List
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillLocationsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationsList...", category: Category.Info, priority: Priority.Low);
                // [001] Changed Service method
                WarehouseLocationsList = new List<WarehouseLocation>(WarehouseService.GetWarehouseLocationBySelectedWarehouse_V2034(WarehouseCommon.Instance.Selectedwarehouse));

                List<WarehouseLocation> FirstLocations = WarehouseLocationsList.Where(x => x.Parent == 0).OrderBy(x => x.Position).ThenBy(x => x.FullName).ToList();
                List<Int64> intFirstLocations = FirstLocations.Select(s => (Int64)s.IdWarehouseLocation).ToList();
                List<WarehouseLocation> SecondLocations = WarehouseLocationsList.Where(r => intFirstLocations.Contains(r.Parent)).OrderBy(x => x.Position).ThenBy(x => x.FullName).Distinct().ToList();
                List<WarehouseLocation> FirstLevelLocationList = FirstLocations.GroupBy(x => x.Name).Select(a => a.First()).ToList();
                List<WarehouseLocation> SecondLevelLocationList = SecondLocations.GroupBy(x => x.Name).Select(a => a.First()).ToList();

                FirstLevelLocationListOfTile = new ObservableCollection<TileBarFilters>();
                FirstLevelLocationListOfTile.Add(new TileBarFilters()

                {
                    Caption = (System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null

                });
                int _id = 1;
                foreach (var item in FirstLevelLocationList)
                {
                    FirstLevelLocationListOfTile.Add(new TileBarFilters()
                    {

                        Caption = item.Name,
                        Id = _id++,
                        BackColor = item.HtmlColor,
                        ForeColor = item.HtmlColor,
                      

                    });
                }

                FirstLevelLocationListOfTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("LocationViewCustomFilter").ToString()),
                    Id = -1,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                   
                });

                if (FirstLevelLocationListOfTile.Count > 0)
                    FirstLevelLocationSelectedTileIndex = 0;


                SecondLevelLocationListOfTile = new ObservableCollection<TileBarFilters>();
                SecondLevelLocationListOfTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null
                });
                foreach (var item in SecondLevelLocationList)
                {
                    SecondLevelLocationListOfTile.Add(new TileBarFilters()
                    {
                        Caption = item.Name,
                        Id = _id++,
                        BackColor = item.HtmlColor,
                        ForeColor = item.HtmlColor

                    });
                }

                if (SecondLevelLocationListOfTile.Count > 0)
                    SecondLevelLocationSelectedTileIndex = 0;


                GeosApplication.Instance.Logger.Log("Method FillLocationsList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationsList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationsList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void LocationViewReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LocationViewReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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

                TableView detailView = (TableView)obj;
                Article article = (Article)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                WarehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                if (articleDetailsViewModel.IsResult)
                {

                    ObservableCollection<Article> tempLocationsList = new ObservableCollection<Article>(FilterWiseListOfLocations.Where(x => x.IdArticle == article.IdArticle).ToList());
                    foreach (ArticleWarehouseLocations tempLocation in articleDetailsViewModel.UpdateArticle.LstArticleWarehouseLocations)
                    {
                        Article tempArticle = tempLocationsList.FirstOrDefault(x => x.ArticleWarehouseLocation.IdWarehouseLocation == tempLocation.IdWarehouseLocation);
                        if (tempArticle != null)
                        {
                            ArticleWarehouseLocations articleWarehouseLocation = (ArticleWarehouseLocations)tempArticle.ArticleWarehouseLocation.Clone();
                            articleWarehouseLocation.MinimumStock = tempLocation.MinimumStock;
                            articleWarehouseLocation.MaximumStock = tempLocation.MaximumStock;
                            tempArticle.ArticleWarehouseLocation = articleWarehouseLocation;
                        }

                    }
                    SelectedLocation = article;
                }
                detailView.Focus();
                GeosApplication.Instance.Logger.Log("Method LocationViewReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method LocationViewReferenceHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void LocationRefill(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LocationRefill....", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                RefillScanViewModel refillScanViewModel = new RefillScanViewModel();
                RefillScanView refillScanView = new RefillScanView();
                refillScanViewModel.Init();
                EventHandler handler = delegate { refillScanView.Close(); };
                refillScanViewModel.RequestClose += handler;
                refillScanView.DataContext = refillScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                refillScanView.Owner = Window.GetWindow(ownerInfo);
                refillScanView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method LocationRefill....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LocationRefill...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to create Custom Filter Editor
        /// </summary>
        private void CreateCustomFilterEditor(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
            ShowCustomFilterEditor(obj);
        }
        
        /// <summary>
        /// Mthod to show Custom Filter Editor
        /// </summary>
        /// <param name="e"></param>
        private void ShowCustomFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowCustomFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customfilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customfilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customfilterEditorViewModel.FilterName = FilterStringName;
                    customfilterEditorViewModel.IsSave = true;
                    customfilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customfilterEditorViewModel.IsNew = true;

                customfilterEditorViewModel.Init(e.FilterControl, FirstLevelLocationListOfTile);
                customfilterEditorView.DataContext = customfilterEditorViewModel;
                EventHandler handle = delegate { customfilterEditorView.Close(); };
                customfilterEditorViewModel.RequestClose += handle;
                customfilterEditorView.Title = titleText;
                customfilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customfilterEditorView.Grid.Children.Add(e.FilterControl);
                customfilterEditorView.ShowDialog();

                if (customfilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customfilterEditorViewModel.FilterName) && customfilterEditorViewModel.IsSave)
                {
                    TileBarFilters tileBarItem = FirstLevelLocationListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        FirstLevelLocationListOfTile.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key.Replace("WMS_Location_","");

                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }

                        MyFilterString = null;
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customfilterEditorViewModel.FilterName) && customfilterEditorViewModel.IsSave && !customfilterEditorViewModel.IsNew && customfilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = FirstLevelLocationListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        FilterStringName = customfilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customfilterEditorViewModel.FilterName;
                        tileBarItem.FilterCriteria = customfilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key.Replace("WMS_Location_", "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>(( "WMS_Location_" + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }

                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customfilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customfilterEditorViewModel.FilterName) && customfilterEditorViewModel.IsSave && customfilterEditorViewModel.IsNew && customfilterEditorViewModel.IsCancel)
                {
                    FirstLevelLocationListOfTile.Add(new TileBarFilters()
                    {
                        Caption = customfilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        FilterCriteria = customfilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200
                    });
                    SelectedTileBarItem = FirstLevelLocationListOfTile.LastOrDefault();
                    filter = "WMS_Location_";
                    customfilterEditorViewModel.FilterName = filter + customfilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[customfilterEditorViewModel.FilterName] = customfilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ShowCustomFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowCustomFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Double Click on CustomFilter
        /// </summary>
        /// <param name="obj"></param>
        private void TileBarDoubleClick(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarDoubleClick()...", category: Category.Info, priority: Priority.Low);

                if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("LocationViewCustomFilter").ToString()) || FilterStringName.Equals(System.Windows.Application.Current.FindResource("LocationTilebarCaption").ToString()))
                {
                    return;
                }

                else if (FilterStringName == "")
                {
                    return;
                }
               
                foreach (var item in WarehouseLocationsList)
                {

                    if (FilterStringName != null)
                    {

                        if (FilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                        
                    }

                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("[ArticleWarehouseLocation.WarehouseLocation.FullName]"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method TileBarDoubleClick() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method TileBarDoubleClick() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddCustomFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);

                foreach (var item in GeosApplication.Instance.UserSettings)
                {

                    if (item.Key.Contains("WMS_Location_"))
                    {

                        FirstLevelLocationListOfTile.Add(
                        new TileBarFilters()
                        {
                            Caption = item.Key.Replace("WMS_Location_", ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  [01][GEOS2-1477] [Allow to save grid configuration in work order section] [2019-07-10] [avpawar]
        /// Method for saving grid layoutInvokeDelegateCommand
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(LocationGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(LocationGridSettingFilePath);
                    GridControl GridControlLocationDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView LocationTableView = (TableView)GridControlLocationDetails.View;

                    if (LocationTableView.SearchString != null)
                    {
                        LocationTableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(LocationGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }


                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsLocationColumnChooserVisible = true;
                }
                else
                {
                    IsLocationColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                e.Allow = false;
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(LocationGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    isLocationColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(LocationGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
