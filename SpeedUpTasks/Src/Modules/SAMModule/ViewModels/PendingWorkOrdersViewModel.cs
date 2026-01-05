using DevExpress.Data;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
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
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class PendingWorkOrdersViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services Region

        #region Declaration

        private ObservableCollection<TileBarFilters> quickFilterList = new ObservableCollection<TileBarFilters>();
        private ObservableCollection<Template> listOfTemplate = new ObservableCollection<Template>();
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        private List<Ots> mainOtsList = new List<Ots>();
        private List<Ots> filterWiseListOfWorkOrder = new List<Ots>();
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM> columns;
        private IList<Template> templates;
        List<OfferOption> offerOptions;
        private DataTable dttable;
        private DataTable dttableCopy;
        TableView view;
        private string filterString;
        private bool isBusy;
        private TileBarFilters selectedFilter;
        private int visibleRowCount;
        private string userSettingsKey = "SAM_PendingWorkOrder_";
        private bool isEdit;
        private bool isSave;
        private bool isWorkOrderColumnChooserVisible;
        public string PendingWorkOrdersGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SAM_PendingWorkOrdersGridSetting.Xml";
        private object geosAppSettingList;
        private List<TileBarFilters> filterList;
        #endregion // End Of Declaration

        #region Properties

        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
        }
        public IList<Template> Templates
        {
            get { return templates; }
            set { templates = value; }
        }
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public virtual string ResultFileName { get; set; }

        public virtual bool DialogResult { get; set; }
        public List<int> ProgressList { get; private set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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

        
        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }

        public List<Ots> FilterWiseListOfWorkOrder
        {
            get { return filterWiseListOfWorkOrder; }
            set
            {
                filterWiseListOfWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterWiseListOfWorkOrder"));
            }
        }

        public List<Ots> MainOtsList
        {
            get { return mainOtsList; }
            set
            {
                mainOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainOtsList"));
            }
        }

        public ObservableCollection<Template> ListOfTemplate
        {
            get { return listOfTemplate; }
            set
            {
                listOfTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("listOfTemplate"));
            }
        }

        public ObservableCollection<TileBarFilters> QuickFilterList
        {
            get { return quickFilterList; }
            set
            {
                quickFilterList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuickFilterList"));
            }
        }

        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }

        public string CustomFilterStringName { get; set; }

        public bool IsWorkOrderColumnChooserVisible
        {
            get { return isWorkOrderColumnChooserVisible; }
            set
            {
                isWorkOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrderColumnChooserVisible"));
            }
        }

        public object GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        #endregion //End Of Properties

        #region Icommands
        public ICommand CommandFilterTileClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand PlantOwnerPopupClosed { get; private set; }
        public ICommand RefreshWorkOrderViewCommand { get; set; }
        public ICommand PrintWorkOrderViewCommand { get; set; }
        public ICommand ExportWorkOrderViewCommand { get; set; }
        public ICommand ScanWorkOderCommand { get; set; }
        public ICommand RefundWorkOrderCommand { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand UpdateMultipleRowsWorkOrderCommand { get; set; }
        public ICommand CommandTileBarDoubleClick { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand { get; set; }
        public ICommand CustomUnboundColumnDataCommand1 { get; set; }
        //public ICommand CustomCellAppearanceCommand { get; set; }

        public ICommand ScanValidationCommand { get; set; }
        

        #endregion //End Of Icommand

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Constructor

        public PendingWorkOrdersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                               
                    UpdateMultipleRowsWorkOrderCommand = new DelegateCommand<object>(UpdateMultipleRowsCommandAction);
                    CommandFilterTileClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                CommandGridDoubleClick = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);
                PlantOwnerPopupClosed = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedAction);
                RefreshWorkOrderViewCommand = new RelayCommand(new Action<object>(RefreshPendingWorkOrderList));
                PrintWorkOrderViewCommand = new RelayCommand(new Action<object>(PrintPendingWorkOrderList));
                ExportWorkOrderViewCommand = new RelayCommand(new Action<object>(ExportPendingWorkOrderList));
                ScanWorkOderCommand = new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrder);
                RefundWorkOrderCommand = new DelegateCommand<object>(RefundWorkOrderCommandAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new DelegateCommand<object>(CommandTileBarDoubleClickAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CustomUnboundColumnDataCommand = new DelegateCommand<DevExpress.Xpf.Grid.GridColumnDataEventArgs>(CustomUnboundColumnDataAction);
                CustomUnboundColumnDataCommand1 = new DelegateCommand<object>(CustomUnboundColumnDataAction);
                //CustomCellAppearanceCommand = new DelegateCommand<CustomRowAppearanceEventArgs>(CustomCellAppearanceAction);

                ScanValidationCommand =new DevExpress.Mvvm.DelegateCommand<object>(ScanWorkOrderValidation);
                FillProgressList();
                FillListOfColor();  // Called only once for colors
                LoadData();
                FillLeadGridDetails();
                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion //End Of Constructor

        #region Methods

        /// <summary>
        /// This method is called from constructor & refresh & plant combobox
        /// </summary>
        private void LoadData()
        {
            FillMainOtList();
            FillPendingWorkOrderFilterList();
            AddCustomSetting();
        }

        /// <summary>
        /// [001]-sjadhav
        /// This method is called from constructor & fills Progress List
        /// </summary>
        private void FillProgressList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProgressList ...", category: Category.Info, priority: Priority.Low);

                ProgressList = new List<int>();
                ProgressList.Add(0);
                ProgressList.Add(10);
                ProgressList.Add(20);
                ProgressList.Add(30);
                ProgressList.Add(40);
                ProgressList.Add(50);
                ProgressList.Add(60);
                ProgressList.Add(70);
                ProgressList.Add(80);
                ProgressList.Add(90);
                ProgressList.Add(100);

                GeosApplication.Instance.Logger.Log("Method FillProgressList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProgressList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
        /// This method is called from constructor & save data in grid
        /// </summary>
        public void UpdateMultipleRowsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction in SAM Work Order...", category: Category.Info, priority: Priority.Low);

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
                bool isOtSave = false;
                bool isAllSave = false;
                
                Ots[] foundRow = MainOtsList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                if (foundRow.Length==0)
                {
                    foundRow = GeosApplication.Instance.MainOtsList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                }
                
                foreach (Ots Ot  in foundRow)
                {
                       isOtSave = SAMService.UpdateOTFromGrid(Ot.Site, Ot);
                    if (isOtSave)
                        isAllSave = true;
                    else
                        isAllSave = false;
                }

                if (isAllSave)
                {                   
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderMsgUpdateFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                }
                MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsCommandAction() in SAM Work Order executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsCommandAction() Method in SAM Work Order" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction...", category: Category.Info, priority: Priority.Low);

            view = MultipleCellEditHelperSAMWorkOrder.Viewtableview;
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                }
                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
            }

            MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;

            if (view != null)
            {
                MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
            }



            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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

            LoadData();
            FillLeadGridDetails();
            FilterString = string.Empty;

            if (QuickFilterList.Count > 0)
                SelectedFilter = QuickFilterList.FirstOrDefault();

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        public void RefreshPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                view = MultipleCellEditHelperSAMWorkOrder.Viewtableview;
                if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                    }

                    MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
                }

                MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;

                if (view != null)
                {
                    MultipleCellEditHelperSAMWorkOrder.SetIsValueChanged(view, false);
                }


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

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                IsBusy = true;

                // code for hide column chooser if empty
                TableView tableView = (TableView)gridControl.View;
                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }

                LoadData();
                FillLeadGridDetails();
                FilterString = string.Empty;
                detailView.SearchString = null;
                IsBusy = false;

                if (QuickFilterList.Count > 0)
                    SelectedFilter = QuickFilterList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[001][20-05-2020][cpatil][GEOS2-1936] Ignore STRUCTURE template items in the #Modules count
        //[002][17-06-2020][smazhar][GEOS2-2376]  Filter the users by the selected plant
        private void FillMainOtList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainOtList...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<Ots> TempMainOtsList = new List<Ots>();
                    MainOtsList = new List<Ots>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            // [001]
                            // [002] changed service function
                            TempMainOtsList = new List<Ots>(SAMService.GetPendingWorkorders_V2044(plant));
                          
                            MainOtsList.AddRange(TempMainOtsList);                            
                        }

                        ///----------[Sprint-83] [GEOS2-2372]  [25-06-2020] [sjadhav]---------
                        if (GeosApplication.Instance.MainOtsList.Count == 0)
                        {
                            GeosApplication.Instance.MainOtsList = MainOtsList;
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillMainOtList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillMainOtList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    MainOtsList = new List<Ots>(MainOtsList);
                }
                else
                {
                    MainOtsList = new List<Ots>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for refresh Lead Grid details.
        /// </summary>
        private void FillLeadGridDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                
                AddDataTableColumns();
                FillDataTable();
                GeosApplication.Instance.Logger.Log(" FillLeadGridDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadGridDetails() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
                Dttable.Rows.Clear();
                DttableCopy = Dttable.Copy();

                for (int i = 0; i < MainOtsList.Count; i++)
                {
                    DataRow dr = DttableCopy.NewRow();
                    dr["IdOt"] = MainOtsList[i].IdOT.ToString();
                    dr["OfferType"] = MainOtsList[i].Quotation.Offer.OfferType.IdOfferType;
                    dr["Code"] = MainOtsList[i].MergeCode.ToString();
                    dr["Type"] = MainOtsList[i].Quotation.Template.Name.ToString();
                    dr["Description"] = MainOtsList[i].Quotation.Offer.Description.ToString();
                    dr["Modules"] = MainOtsList[i].Modules.ToString();
                    dr["Group"] = MainOtsList[i].Quotation.Site.Customer.CustomerName.ToString();
                    dr["Plant"] = MainOtsList[i].Quotation.Site.Name.ToString();
                    dr["Country"] = MainOtsList[i].Quotation.Site.Country.Name.ToString();

                    if (MainOtsList[i].Quotation.Offer.CarOEM != null)
                        dr["OEM"] = MainOtsList[i].Quotation.Offer.CarOEM.Name;
                    if (MainOtsList[i].Quotation.Offer.CarProject != null)
                        dr["Project"] = MainOtsList[i].Quotation.Offer.CarProject.Name.ToString();
                    if (MainOtsList[i].Quotation.Offer.BusinessUnit != null)
                        dr["BusinessUnit"] = MainOtsList[i].Quotation.Offer.BusinessUnit.Value.ToString();


                    if (MainOtsList[i].PoDate != null)
                        dr["PODate"] = MainOtsList[i].PoDate;
                    else
                        dr["PODate"] = DBNull.Value;

                    dr["Status"] = MainOtsList[i].Quotation.Offer.GeosStatus.Name.ToString();
                    dr["HtmlColor"] = MainOtsList[i].Quotation.Offer.GeosStatus.HtmlColor.ToString();

                    if (MainOtsList[i].DeliveryDate != null)
                        dr["DeliveryDate"] = MainOtsList[i].DeliveryDate;
                    else
                        dr["DeliveryDate"] = DBNull.Value;

                    if (MainOtsList[i].ExpectedStartDate != null)
                        dr["PlannedStartDate"] = MainOtsList[i].ExpectedStartDate;
                    else
                        dr["PlannedStartDate"] = DBNull.Value;

                    if (MainOtsList[i].ExpectedEndDate != null)
                        dr["PlannedEndDate"] = MainOtsList[i].ExpectedEndDate;
                    else
                        dr["PlannedEndDate"] = DBNull.Value;

                    if (MainOtsList[i].ExpectedStartDate != null && MainOtsList[i].ExpectedEndDate != null)
                    {
                        DateTime startdate = (DateTime)MainOtsList[i].ExpectedStartDate;
                        DateTime enddate = (DateTime)MainOtsList[i].ExpectedEndDate;
                        dr["PlannedDuration"] = (enddate - startdate).TotalDays;
                    }

                    if (MainOtsList[i].RealStartDate != null)
                        dr["RealStartDate"] = MainOtsList[i].RealStartDate;
                    else
                        dr["RealStartDate"] = DBNull.Value;

                    if (MainOtsList[i].RealEndDate != null)
                        dr["RealEndDate"] = MainOtsList[i].RealEndDate;
                    else
                        dr["RealEndDate"] = DBNull.Value;

                    //dr["DeliveryWeek"] = MainOtsList[i].Ex.ToString();

                    if (MainOtsList[i].DeliveryDate != null)
                    {
                        CultureInfo cul = CultureInfo.CurrentCulture;
                        dr["ExpectedDeliveryWeek"] = MainOtsList[i].DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)MainOtsList[i].DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
                    }
                    dr["Delay"] = MainOtsList[i].Delay;
                    dr["Progress"] = MainOtsList[i].Progress;
                    //dr["PlannedDuration"] = MainOtsList[i].RealDuration;
                    dr["RealDuration"] = MainOtsList[i].RealDuration;

                    try
                    {
                        //foreach (OptionsByOfferGrid item in MainOtsList[i].Quotation.Offer.OptionsByOffersGrid)
                        foreach (OptionsByOfferGrid item in MainOtsList[i].Quotation.Offer.OptionsByOffersGrid)
                        {
                            if (item.OfferOption != null)
                            {
                                if (item.IdOption.ToString() == "6" ||
                                     item.IdOption.ToString() == "19" ||
                                     item.IdOption.ToString() == "21" ||
                                     item.IdOption.ToString() == "23" ||
                                     item.IdOption.ToString() == "25" ||
                                     item.IdOption.ToString() == "27")
                                {
                                    var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                    int indexc = Columns.IndexOf(column);
                                    Columns[indexc].Visible = false;
                                }
                                else if (DttableCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                                {
                                    var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                    int indexc = Columns.IndexOf(column);
                                    Columns[indexc].Visible = true;
                                    dr[item.OfferOption] = item.Quantity;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    try
                    {

                        if (MainOtsList[i].Quotation.Offer.ProductCategoryGrid.IdProductCategory > 0)
                        {
                            if (MainOtsList[i].Quotation.Offer.ProductCategoryGrid.Category != null)
                                dr["Category1"] = MainOtsList[i].Quotation.Offer.ProductCategoryGrid.Category.Name;

                            if (MainOtsList[i].Quotation.Offer.ProductCategoryGrid.IdParent == 0)
                            {
                                dr["Category1"] = MainOtsList[i].Quotation.Offer.ProductCategoryGrid.Name;
                            }
                            else
                            {
                                dr["Category2"] = MainOtsList[i].Quotation.Offer.ProductCategoryGrid.Name;
                            }
                        }
                        dr["OfferStartDateMinValue"] = GeosApplication.Instance.ServerDateTime.Date;
                        dr["ProducedModules"] = MainOtsList[i].ProducedModules;
                        dr["OperatorNames"] = MainOtsList[i].OperatorNames;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    DttableCopy.Rows.Add(dr);
                }
                Dttable = DttableCopy;
                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            }

        /// <summary>
        /// [001][sjadhav][26/06/2020][GEOS2-2381]- Add new columns in Work Orders grid
        /// </summary>
        private void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM>() {
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="",HeaderText=" ", Settings = SettingsTypeSAM.Empty, AllowCellMerge=false, Width=40,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Type",HeaderText="Type", Settings = SettingsTypeSAM.Type, AllowCellMerge=false, Width=85,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OfferType",HeaderText="OfferType", Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false, Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="IdOt",HeaderText="IdOt", Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false, Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Code",HeaderText="Code", Settings = SettingsTypeSAM.OfferCode, AllowCellMerge=false, Width=125,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Description",HeaderText="Description", Settings = SettingsTypeSAM.Description, AllowCellMerge=false, Width=250,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Modules", HeaderText="#Modules", Settings = SettingsTypeSAM.Modules, AllowCellMerge=false, Width=70,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Group", HeaderText="Group", Settings = SettingsTypeSAM.Group, AllowCellMerge=false, Width=160,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Plant", HeaderText="Plant", Settings = SettingsTypeSAM.Plant, AllowCellMerge=false, Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Country", HeaderText="Country", Settings = SettingsTypeSAM.Default, AllowCellMerge=false, Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Project", HeaderText="Project", Settings = SettingsTypeSAM.Project, AllowCellMerge=false, Width=60,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OEM", HeaderText="OEM", Settings = SettingsTypeSAM.OEM, AllowCellMerge=false, Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="BusinessUnit", HeaderText="Business Unit", Settings = SettingsTypeSAM.BusinessUnit, AllowCellMerge=false, Width=140,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PODate", HeaderText="PO Date",Settings = SettingsTypeSAM.PODate, AllowCellMerge=false,Width=60,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Status", HeaderText="Status",Settings = SettingsTypeSAM.Status, AllowCellMerge=false,Width=130,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="HtmlColor", HeaderText="HtmlColor",Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="DeliveryDate", HeaderText="Expected Delivery Date",Settings = SettingsTypeSAM.DeliveryDate, AllowCellMerge=false,Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="ExpectedDeliveryWeek", HeaderText="Expected Delivery Week", Settings = SettingsTypeSAM.DeliveryWeek, AllowCellMerge=false, Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Delay", HeaderText="Delay", Settings = SettingsTypeSAM.Delay, AllowCellMerge=false, Width=40,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Progress", HeaderText="Progress",Settings = SettingsTypeSAM.PercentText, AllowCellMerge=false,Width=60,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedStartDate", HeaderText="Planned Start Date",Settings = SettingsTypeSAM.PlannedStartDate, AllowCellMerge=false,Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedEndDate", HeaderText="Planned End Date",Settings = SettingsTypeSAM.PlannedEndDate, AllowCellMerge=false,Width=120,AllowEditing=true,Visible=true,IsVertical= false,FixedWidth=false },                        
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="PlannedDuration", HeaderText="Planned Duration(d)",Settings = SettingsTypeSAM.PlannedDuration, UnboundType="Integer",AllowCellMerge=false,Width=120,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealStartDate", HeaderText="Real Start Date",Settings = SettingsTypeSAM.RealStartDate, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealEndDate", HeaderText="Real End Date",Settings = SettingsTypeSAM.RealEndDate, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="RealDuration", HeaderText="Real Duration(h)",Settings = SettingsTypeSAM.RealDuration, AllowCellMerge=false,Width=100,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Category1", HeaderText="Category1",Settings = SettingsTypeSAM.Category1, AllowCellMerge=false,Width=200,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Category2", HeaderText="Category2",Settings = SettingsTypeSAM.Category2, AllowCellMerge=false,Width=200,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false },                       
                        new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="OperatorNames", HeaderText="OperatorNames",Settings = SettingsTypeSAM.Hidden, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=false },                       
                 }; 

                 Dttable = new DataTable();
                Dttable.Columns.Add("IdOt", typeof(long));
                Dttable.Columns.Add("OfferType", typeof(byte));
                Dttable.Columns.Add("Type", typeof(string));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Modules", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Project", typeof(string));
                Dttable.Columns.Add("OEM", typeof(string));
                Dttable.Columns.Add("BusinessUnit", typeof(string));
                Dttable.Columns.Add("PODate", typeof(DateTime));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("DeliveryDate", typeof(DateTime));
                Dttable.Columns.Add("ExpectedDeliveryWeek", typeof(string));
                Dttable.Columns.Add("Delay", typeof(Int32));
                Dttable.Columns.Add("Progress", typeof(string));
                Dttable.Columns.Add("PlannedStartDate", typeof(DateTime));
                Dttable.Columns.Add("PlannedEndDate", typeof(DateTime));
                Dttable.Columns.Add("OfferStartDateMinValue", typeof(DateTime));
                Dttable.Columns.Add("PlannedDuration", typeof(string));
                Dttable.Columns.Add("RealStartDate", typeof(DateTime));
                Dttable.Columns.Add("RealEndDate", typeof(DateTime));                
                Dttable.Columns.Add("RealDuration", typeof(string));
                Dttable.Columns.Add("Category1", typeof(string));
                Dttable.Columns.Add("Category2", typeof(string));
                Dttable.Columns.Add("OperatorNames", typeof(string));
                
                // Total Summary
                TotalSummary = new ObservableCollection<Summary>();
                Templates = new List<Template>();
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total: {0}" });
                //OfferOptions = SAMService.GetAllOt();
                //for (int i = 0; i < OfferOptions.Count; i++)
                //{
                //    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                //    {
                //        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                //        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                //        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                //    }
                //}
                //added site column in last on the grid.
                OfferOptions = CrmStartUp.GetAllOfferOptions();
                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsTypeSAM.Array, AllowCellMerge = false, Width = 40, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = false });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                    }
                }
                Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = "ProducedModules", HeaderText = "Produced Modules", Settings = SettingsTypeSAM.ProducedModules, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = false });
                Dttable.Columns.Add("ProducedModules", typeof(Int32));
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter List
        /// </summary>
        public void FillPendingWorkOrderFilterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList....", category: Category.Info, priority: Priority.Low);

                ListOfTemplate = new ObservableCollection<Template>();
                FillPendingWorkOrderFilterTiles(ListOfTemplate, MainOtsList);

                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method To Fill Pending Work Order Filter Tiles 
        /// [001][skale][07/11/2019][GEOS2-1758]- Create Orders section with the grid of working orders
        /// </summary>
        public void FillPendingWorkOrderFilterTiles(ObservableCollection<Template> FilterList, List<Ots> MainListWorkOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles....", category: Category.Info, priority: Priority.Low);

                QuickFilterList = new ObservableCollection<TileBarFilters>();

                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllWO").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(),
                    //ImageUri = "Template.png",
                    //BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                foreach (Ots ot in MainListWorkOrder)
                {
                    if (ot.UserShortDetails != null && ot.UserShortDetails.Count > 0)
                    {
                        foreach (UserShortDetail user in ot.UserShortDetails)
                        {

                            ImageSource userImage = null;
                            //[001] added
                            if (user.UserImageInBytes == null)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                   
                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png"));

                                    else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png"));
                                }
                                else
                                {

                                    if (user != null && user.IdUserGender == 1)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png"));
                                        else if (user != null && user.IdUserGender == 2)
                                        userImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png"));
                                }

                            }
                            else
                            {
                                userImage = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes);
                            }

                            if (!QuickFilterList.Any(x => x.Id == user.IdUser))
                            {
                                QuickFilterList.Add(new TileBarFilters()
                                {
                                    Caption = user.UserName,
                                    Id = user.IdUser,
                                    Type = user.UserName,
                                    EntitiesCount = MainListWorkOrder.Count(x => !string.IsNullOrEmpty(x.OperatorNames) && x.OperatorNames.Split(',').ToList().Contains(user.UserName)),
                                    // Image = SAMCommon.Instance.ByteArrayToBitmapImage(user.UserImageInBytes),   
                                    Image = userImage,
                                    //BackColor = item.HtmlColor,
                                    EntitiesCountVisibility = Visibility.Visible,
                                    Height = 80,
                                    width = 200
                                });
                            }
                        }
                    }
                }

                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("UnassignedWO").ToString()),
                    Id = 0,
                    EntitiesCount = MainListWorkOrder.Count(x => string.IsNullOrEmpty(x.OperatorNames)),
                    //ImageUri = "Template.png",
                    //BackColor = "Wheat",
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                QuickFilterList.Add(new TileBarFilters()
                {
                    Caption = System.Windows.Application.Current.FindResource("CustomFilters").ToString(),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });
                filterList = new List<TileBarFilters>();
                filterList = QuickFilterList.ToList();
                GeosApplication.Instance.Logger.Log("Method FillPendingWorkOrderFilterTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPendingWorkOrderFilterTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for showing Grid as per Filter Tile Selection
        /// </summary>
        private void ShowSelectedFilterGridAction(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (QuickFilterList.Count > 0)
                {
                    System.Windows.Controls.SelectionChangedEventArgs obj = (System.Windows.Controls.SelectionChangedEventArgs)e;

                    string Template = null;
                    string _FilterString = null;

                    if (obj.AddedItems.Count > 0)
                    {
                        //int IdTemplate = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Id;
                        Template = ((TileBarFilters)(obj.AddedItems)[0]).Type;
                        _FilterString = ((TileBarFilters)(obj.AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)(obj.AddedItems)[0]).Caption;
                    }

                    if (CustomFilterStringName != null && CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    if (CustomFilterStringName == "Unassigned")
                    {
                        FilterString = "IsNullOrEmpty([OperatorNames])";
                        return;
                    }

                    if (Template == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            FilterString = _FilterString;
                        else
                            FilterString = string.Empty;
                    }
                    else
                    {
                        //FilterString = "[OperatorNames] In ('" + Template + "')";
                        FilterString = "Contains([OperatorNames], '" + Template + "')";
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for showing Grid's selected row Item detailed Window
        /// </summary>
        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                
                TableView detailView = (TableView)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                long OtId= Convert.ToInt64(((System.Data.DataRowView)detailView.DataControl.CurrentItem).Row.ItemArray[0].ToString());
                Ots FoundRow = MainOtsList.Where(mol => mol.IdOT == OtId).FirstOrDefault();                
                workOrderItemDetailsViewModel.Init(FoundRow);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workOrderItemDetailsView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PendingWorkOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;

                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPendingWorkOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Pending Work Order List";
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    options.CustomizeDocumentColumn += Options_CustomizeDocumentColumn;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportPendingWorkOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPendingWorkOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Open Scan ot view
        /// [000][skale][11-12-2019][GEOS2-1881] Add new option to Log the worked time in an OT
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("LogWork").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                EventHandler handler = delegate { workOrderScanView.Close(); };

                workOrderScanViewModel.RequestClose += handler;

                workOrderScanView.DataContext = workOrderScanViewModel;
                workOrderScanView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private List<GeosAppSetting> copyGeosAppSettingList = null;
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            string htmlColor = string.Empty;
            DateTime todaysDate = DateTime.Now.Date;
            if (copyGeosAppSettingList == null)
            {
                copyGeosAppSettingList = ((List<GeosAppSetting>)GeosAppSettingList);
            }
           
            if (e.ColumnFieldName == "RowData.Row")
            {
                if(e.Value!=" ")
                {
                    var Type = ((System.Data.DataRowView)e.Value);
                    DataRowView dr = (DataRowView)Type;

                    byte idoffertype = (byte)dr.Row["OfferType"];
                    DateTime DeliveryDate = (DateTime)dr.Row["DeliveryDate"];
                    if (idoffertype == 2 || idoffertype == 3)
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 17)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //red
                    }
                    else if (DeliveryDate.Date <= todaysDate)
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 14)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //yellow
                    }
                    else if (DeliveryDate.Date > todaysDate && DeliveryDate.Date <= todaysDate.AddDays(6))
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 15)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //Green
                    }
                    else if (DeliveryDate.Date >= todaysDate.AddDays(7))
                    {
                        htmlColor = copyGeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 16)?.DefaultValue;
                        e.Formatting.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor);    //Blue
                    }
                    e.Value = null;
                }
            }

            e.Formatting.Alignment = new XlCellAlignment()
            {
                WrapText = true,
            };
           
            if (e.Value != null && OfferOptions.Any(a=>a.Name==e.Value.ToString()) && OfferOptions.Any(a=>a.Name== e.ColumnFieldName))
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    TextRotation = 90,
                };
            }
            if (e.Value!=null && e.Value.ToString() != "Planned Duration(d)" && e.ColumnFieldName == "PlannedDuration")
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    HorizontalAlignment = XlHorizontalAlignment.Right
                };
            }

            if (e.Value != null && e.Value.ToString() != "#Modules" && e.ColumnFieldName == "Modules")
            {
                e.Formatting.Alignment = new XlCellAlignment()
                {
                    WrapText = true,
                    HorizontalAlignment = XlHorizontalAlignment.Right
                };
            }
            e.Handled = true;
        }

        private void Options_CustomizeDocumentColumn(CustomizeDocumentColumnEventArgs e)
        {
            if (e.ColumnFieldName == "Quotation.Offer.IsCritical")
                e.DocumentColumn.WidthInPixels = 90;
        }

        

        public void Dispose()
        {           
        }

        private void RefundWorkOrderCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()...", category: Category.Info, priority: Priority.Low);
                //WorkOrderScanView workOrderScanView = new WorkOrderScanView();
                //WorkOrderScanViewModel workOrderScanViewModel = new WorkOrderScanViewModel();
                //workOrderScanViewModel.WindowHeader = Application.Current.FindResource("RefundWorkOrderHeader").ToString();
                //workOrderScanViewModel.Init(MainOtsList);
                //workOrderScanViewModel.IsRefund = true;
                //EventHandler handler = delegate { workOrderScanView.Close(); };
                //workOrderScanViewModel.RequestClose += handler;
                //workOrderScanView.DataContext = workOrderScanViewModel;
                //workOrderScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method RefundWorkOrderCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            ShowFilterEditor(obj);
        }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;

                customFilterEditorViewModel.Init(e.FilterControl, QuickFilterList);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        QuickFilterList.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = QuickFilterList.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    QuickFilterList.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = QuickFilterList.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    foreach (var item in tempUserSettings)
                    {
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Ots)), item.Value, false);
                        List<Ots> tempList = new List<Ots>();

                        foreach (var ot in MainOtsList)
                        {
                            if (evaluator.Fit(ot))
                                tempList.Add(ot);
                        }

                        FilterString = item.Value;
                        QuickFilterList.Add(new TileBarFilters()
                        {
                            Caption = item.Key.Replace(userSettingsKey, ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCount = tempList.Count,
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

        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                //if (ListOfTemplate.Any(x => x.Name == CustomFilterStringName) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()) || CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("AllWorkOrder").ToString()))
                //    return;
                foreach(var item in filterList)
                {
                    if(CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Caption))
                            return;
                    }
                }
                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                List<GridColumn> GridColumnList = gridControl.Columns.Where(x => x.FieldName != null).ToList();
                string columnName = FilterString.Substring(FilterString.IndexOf("[") + 1, FilterString.IndexOf("]") - 2 - FilterString.IndexOf("[") + 1);
                GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals(columnName));
                IsEdit = true;
                table.ShowFilterEditor(column);

                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        ///// <summary>
        ///// [001][skhade][2019-10-17][GEOS2-1548] Add a new column "#Out of Stock" in picking -> Work orders
        ///// </summary>
        ///// <param name="e"></param>
        //private void CustomCellAppearanceAction(CustomRowAppearanceEventArgs e)
        //{
        //    if (((CustomCellAppearanceEventArgs)e).Column != null && ((CustomCellAppearanceEventArgs)e).Column.Name == "OutOfStock")
        //    {
        //        e.Result = e.ConditionalValue;
        //        e.Handled = true;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumn = 0;

                if (File.Exists(PendingWorkOrdersGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PendingWorkOrdersGridSettingFilePath);
                    GridControl gridControlView = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlView.View;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);

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
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsWorkOrderColumnChooserVisible = true;
                }
                else
                {
                    IsWorkOrderColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);

                if (datailView.FormatConditions == null || datailView.FormatConditions.Count == 0)
                {
                    var profitFormatCondition = new FormatCondition()
                    {
                        Expression = "[DeliveryDate] <= LocalDateTimeToday()",
                        FieldName = "ExpectedDeliveryWeek",
                        Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                        {
                            Foreground = Brushes.Red
                        }
                    };
                    datailView.FormatConditions.Add(profitFormatCondition);
                }

                //datailView.BestFitColumns();
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

            if (e.Property.Name == "FormatConditions")
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PendingWorkOrdersGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CustomUnboundColumnDataAction(object e)
        {
            CellValueChangedEventArgs obj = (CellValueChangedEventArgs)e;
            //e = Dttable;
            if (obj.Column.FieldName == "PlannedStartDate" || obj.Column.FieldName== "PlannedEndDate")
            {
                DateTime? PlannedStartDate = null;
                DateTime? PlannedEndDate = null;
                DataRowView dr = (DataRowView)obj.Row;
                if (dr.Row["PlannedStartDate"].ToString() != "")
                {
                    PlannedStartDate = Convert.ToDateTime(dr.Row["PlannedStartDate"].ToString());
                }
                if (dr.Row["PlannedEndDate"].ToString() != "")
                {
                    PlannedEndDate = Convert.ToDateTime(dr.Row["PlannedEndDate"].ToString());
                }
                if(PlannedStartDate!=null && PlannedEndDate!=null)
                {
                    dr.Row["PlannedDuration"] = ((DateTime)PlannedEndDate - (DateTime)PlannedStartDate).TotalDays;
                }
                //Ots item = MainOtsList[obj.ListSourceRowIndex];
                //if (item.ExpectedStartDate != null && item.ExpectedEndDate != null)
                //{
                //    DateTime startdate = (DateTime)item.ExpectedStartDate;
                //    DateTime enddate = (DateTime)item.ExpectedEndDate;
                //    obj.Value = (enddate - startdate).TotalDays;
                //}
            }

            //if (e.Column.FieldName == "ExpectedDeliveryWeek")
            //{
            //    Ots item = MainOtsList[e.ListSourceRowIndex];
            //    if(item.DeliveryDate != null)
            //    {
            //        CultureInfo cul = CultureInfo.CurrentCulture;
            //        e.Value = item.DeliveryDate.Value.Year + "CW" + cul.Calendar.GetWeekOfYear((DateTime)item.DeliveryDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
            //    }
            //}
        }

        /// <summary>
        /// Open Scan ot view
        /// [000][psutar][04-05-2020][GEOS2-1881] scan OT validation
        /// </summary>
        /// <param name="obj"></param>
        private void ScanWorkOrderValidation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanWorkOrderValidation...", category: Category.Info, priority: Priority.Low);

                WorkOrderScanValidationView workOrderScanView = new WorkOrderScanValidationView();
                WorkOrderScanValidationViewModel workOrderScanViewModel = new WorkOrderScanValidationViewModel();
                workOrderScanViewModel.WindowHeader = Application.Current.FindResource("Validation").ToString();
                workOrderScanViewModel.Init(MainOtsList);
                EventHandler handler = delegate { workOrderScanView.Close(); };

                workOrderScanViewModel.RequestClose += handler;

                workOrderScanView.DataContext = workOrderScanViewModel;
                workOrderScanView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method ScanWorkOrderValidation executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanWorkOrderValidation() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion //End Of Methods
    }
}
