using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Helper;
using System.IO;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SynchronizationClass;
using DevExpress.Xpf.Grid;
using Newtonsoft.Json;
using DevExpress.Data.Filtering;
using System.Data;
using Emdep.Geos.Utility;
using DevExpress.Data;
using DevExpress.Xpf.LayoutControl;
using System.Globalization;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class DetectionsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
       IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
     //  IPCMService PCMService = new PCMServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event SearchStringToFilterCriteriaEventHandler SearchStringToFilterCriteria;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declarations
        private string header;
        private string selectedHeader;
        private ObservableCollection<DetectionDetails> detectionsMenulist;
        private ObservableCollection<DetectionDetails> tempDetectionsMenulist;
        private List<DetectionDetails> clonedDetectionsList;
        private DetectionDetails selectedDetection;
        private bool isDeleted;
        private bool isBusy;
        private List<GridColumn> GridColumnList;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private List<LookupValue> ecosVisibilityList;
        private List<DetectionGroup> detectionAllGroupList;
        private LookupValue selectedECOSVisibility;
        private ObservableCollection<TestTypes> testTypesList;
        private TestTypes selectedTestTypes;

        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;
        public List<DetectionGroup> DetectionAllGroupList
        {
            get { return detectionAllGroupList; }
            set
            {
                detectionAllGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionAllGroupList"));
            }
        }
        public List<LookupValue> ECOSVisibilityList
        {
            get { return ecosVisibilityList; }
            set
            {
                ecosVisibilityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ECOSVisibilityList"));
            }
        }

        public LookupValue SelectedECOSVisibility
        {
            get
            {
                return selectedECOSVisibility;
            }

            set
            {
                selectedECOSVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedECOSVisibility"));
            }
        }
        private TableView view;
        private bool isDetectionSave;
        private bool? isAllSave;
        public bool isSaveFromReference = false;

        private ObservableCollection<DetectionLogEntry> detectionChangeLogList;
        private ObservableCollection<DetectionLogEntry> changeLogList;
        private DetectionLogEntry selectedDetectionChangeLog;

        private bool isDetectionColumnChooserVisible;

        private ITokenService tokenService;
        List<GeosAppSetting> geosAppSettingList;
        private string visible;
        private bool isReadOnlyField;
        private bool allowDragDrop;
        private bool isEnabled;
        private ObservableCollection<DetectionTypes> testList;
        private ObservableCollection<DetectionTypes> productTypesMenulistForGridLayout;
        private DetectionTypes selectedTest;
        private ObservableCollection<DetectionGroup> groupList_Order;
        private ObservableCollection<DetectionOrderGroup> orderGroupList;
        private List<DetectionGroup> groupList;
        private DetectionGroup selectedGroup;
        private string type;
        private bool isEdit;
        private int selectedTileIndexDetection;
        private List<ProductTypes> templatesMenuList;
        private ObservableCollection<TileBarFilters> listofitem;
        private bool isGridOpen;
        private bool isBandOpen;
        private string myFilterString;
        private string userSettingsKey = "PCM_Modules_Detection";
        private int visibleRowCount;
        private DataTable dataTableForGridLayout;
        private TileBarFilters selectedFilter;
        private DetectionImage selectedDetectionImage;
        private DetectionImage detectionmaximizedElement;
        #endregion

        #region Properties
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public string DetectionGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "DetectionGridSetting.Xml";

        public ObservableCollection<DetectionDetails> DetectionsMenulist
        {
            get { return detectionsMenulist; }
            set
            {
                detectionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionsMenulist"));
            }
        }

        public ObservableCollection<DetectionDetails> TempDetectionsMenulist
        {
            get { return tempDetectionsMenulist; }
            set
            {
                tempDetectionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempDetectionsMenulist"));
            }
        }

        public List<DetectionDetails> ClonedDetectionsList
        {
            get
            {
                return clonedDetectionsList;
            }

            set
            {
                clonedDetectionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDetectionsList"));
            }
        }
        public DetectionDetails SelectedDetection
        {
            get
            {
                return selectedDetection;
            }
            set
            {
                selectedDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetection"));
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ObservableCollection<TestTypes> TestTypesList
        {
            get
            {
                return testTypesList;
            }

            set
            {
                testTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestTypesList"));
            }
        }

        public TestTypes SelectedTestTypes
        {
            get
            {
                return selectedTestTypes;
            }

            set
            {
                selectedTestTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTestTypes"));
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

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
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
        public bool IsDetectionSave
        {
            get { return isDetectionSave; }
            set
            {
                isDetectionSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionSave"));
            }
        }

        public bool? IsAllSave
        {
            get { return isAllSave; }
            set
            {
                isAllSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllSave"));
            }
        }

        public ObservableCollection<DetectionLogEntry> DetectionChangeLogList
        {
            get
            {
                return detectionChangeLogList;
            }

            set
            {
                detectionChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionChangeLogList"));
            }
        }

        public ObservableCollection<DetectionLogEntry> ChangeLogList
        {
            get
            {
                return changeLogList;
            }

            set
            {
                changeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogList"));
            }
        }

        public DetectionLogEntry SelectedDetectionChangeLog
        {
            get
            {
                return selectedDetectionChangeLog;
            }

            set
            {
                selectedDetectionChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetectionChangeLog"));
            }
        }

        public bool IsDetectionColumnChooserVisible
        {
            get
            {
                return isDetectionColumnChooserVisible;
            }

            set
            {
                isDetectionColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDetectionColumnChooserVisible"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
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

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled"));
            }
        }

        public ObservableCollection<DetectionTypes> TestList
        {
            get
            {
                return testList;
            }

            set
            {
                testList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestList"));
            }
        }
        public ObservableCollection<DetectionTypes> ProductTypesMenulistForGridLayout
        {
            get
            {
                return productTypesMenulistForGridLayout;
            }
            set
            {
                productTypesMenulistForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesMenulistForGridLayout"));
            }
        }
        public int SelectedTileIndexDetection
        {
            get
            {
                return selectedTileIndexDetection;
            }

            set
            {
                selectedTileIndexDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndexDetection"));
            }
        }
        public DetectionTypes SelectedTest
        {
            get
            {
                return selectedTest;
            }

            set
            {
                selectedTest = value;
                //SelectedOrderGroupListAction(selectedTest.IdDetectionType);
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTest"));
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
        public ObservableCollection<DetectionOrderGroup> OrderGroupList
        {
            get
            {
                return orderGroupList;
            }

            set
            {
                orderGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderGroupList"));
            }
        }

        public List<DetectionGroup> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList"));
            }
        }
        public DetectionGroup SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
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
        public string CustomFilterStringName { get; set; }

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public List<ProductTypes> TemplatesMenuList
        {
            get { return templatesMenuList; }
            set
            {
                templatesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplatesMenuList"));
            }
        }

        public ObservableCollection<TileBarFilters> Listofitem
        {
            get
            {
                return listofitem;
            }

            set
            {
                listofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listofitem"));
            }
        }
        public bool IsGridOpen
        {
            get
            {
                return isGridOpen;
            }
            set
            {
                isGridOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridOpen"));
            }
        }

        public bool IsBandOpen
        {
            get
            {
                return isBandOpen;
            }
            set
            {
                isBandOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBandOpen"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                if (myFilterString == "")
                {
                    SelectedFilter = Listofitem.FirstOrDefault();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        //public ObservableCollection<DetectionTypes> ProductTypesMenulist
        //{
        //    get { return productTypesMenulist; }
        //    set
        //    {
        //        productTypesMenulist = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesMenulist"));
        //    }
        //}
        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
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
        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }

        //[sshegaonkar[GEOS2-2922][16-02-23]
        public DetectionImage SelectedDetectionImage
        {
            get
            {
                return selectedDetectionImage;
            }

            set
            {
                selectedDetectionImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetectionImage"));
            }
        }

        //[sshegaonkar[GEOS2-2922][16-02-23]
        public DetectionImage DetectionMaximizedElement
        {
            get
            {
                return detectionmaximizedElement;
            }
            set
            {
                detectionmaximizedElement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionMaximizedElement"));
            }
        }
        #endregion

        #region ICommands

        public ICommand DetectionsDoubleClickCommand { get; set; }
        public ICommand AddDetectionButtonCommand { get; set; }
        public ICommand RefreshDetectionsCommand { get; set; }
        public ICommand DeleteDetectionCommand { get; set; }
        public ICommand PrintDetectionsCommand { get; set; }
        public ICommand ExportDetectionsCommand { get; set; }

        public ICommand CellValueUpdatedCommnad { get; set; }
        public ICommand PageLoadedCommand { get; set; }

        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand UpdateMultipleRowsDetectionGridCommand { get; set; }

        public ICommand DetectionGridControlLoadedCommand { get; set; }
        public ICommand DetectionItemListTableViewLoadedCommand { get; set; }
        public ICommand DetectionGridControlUnloadedCommand { get; set; }

        public ICommand SelectedOrderGroupListCommand { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand CommandDetectionShowTileBarFilterPopupClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand SearchStringToFilterCriterias { get; set; }
        public ICommand OpenDetectionImageCommand { get; set; }
        #endregion

        #region Constructor

        public DetectionsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DetectionsViewModel ...", category: Category.Info, priority: Priority.Low);

                DetectionsDoubleClickCommand = new RelayCommand(new Action<object>(EditDetectionItem));
                AddDetectionButtonCommand = new RelayCommand(new Action<object>(AddDetectionItem));
                RefreshDetectionsCommand = new RelayCommand(new Action<object>(RefreshDetectionView));
                DeleteDetectionCommand = new RelayCommand(new Action<object>(DeleteDetectionItem));
                PrintDetectionsCommand = new RelayCommand(new Action<object>(PrintDetections));
                ExportDetectionsCommand = new RelayCommand(new Action<object>(ExportDetections));
                CellValueUpdatedCommnad = new DelegateCommand<CellValueChangedEventArgs>(CellValueUpdatedCommnadAction);
                PageLoadedCommand = new RelayCommand(new Action<object>(OnViewLoaded));
                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                UpdateMultipleRowsDetectionGridCommand = new DelegateCommand<object>(UpdateMultipleRowsDetectionGridCommandAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                DetectionGridControlLoadedCommand = new DelegateCommand<object>(DetectionGridControlLoadedAction);
                DetectionItemListTableViewLoadedCommand = new DelegateCommand<object>(DetectionItemListTableViewLoadedAction);
                DetectionGridControlUnloadedCommand = new DelegateCommand<object>(DetectionGridControlUnloadedCommandAction);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                CommandDetectionShowTileBarFilterPopupClick = new DelegateCommand<object>(ShowSelectedFilterDetectionGridAction);
                //  SearchStringToFilterCriterias = new DelegateCommand<SearchStringToFilterCriteriaEventHandler>(TableView_SearchStringToFilterCriteria);
                OpenDetectionImageCommand = new DelegateCommand<object>(OpenDetectionImageAction); //[sshegaonkar[GEOS2-2922][16-02-23]
                GeosApplication.Instance.Logger.Log("Constructor DetectionsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor DetectionsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueUpdatedCommnadAction(CellValueChangedEventArgs obj)
        {
            // if (obj.Column.FieldName == "DetectionTypes.Name")
            obj.Source.PostEditor();
        }

        private void DeleteDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()...", category: Category.Info, priority: Priority.Low);
                //[rdixit][GEOS2-4119][23.01.2023]
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if (SelectedDetection.Name == null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteDetectionDetailsMessageWithoutName"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //IsDeleted = PCMService.IsDeletedDetection(SelectedDetection.IdDetections);
                        IsDeleted = PCMService.IsDeletedDetection_V2500(SelectedDetection.IdDetections, SelectedDetection.Name); //rajashri GEOS2-5464
                        if (IsDeleted)
                        {
                            DetectionsMenulist.Remove(SelectedDetection);
                            DetectionsMenulist = new ObservableCollection<DetectionDetails>(DetectionsMenulist);
                            SelectedDetection = DetectionsMenulist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteDetectionDetailsMessage"].ToString(), "[" + SelectedDetection.Name.ToString() + "]"), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //IsDeleted = PCMService.IsDeletedDetection(SelectedDetection.IdDetections);
                        IsDeleted = PCMService.IsDeletedDetection_V2500(SelectedDetection.IdDetections, SelectedDetection.Name); //rajashri GEOS2-5464
                        if (IsDeleted)
                        {
                            DetectionsMenulist.Remove(SelectedDetection);
                            DetectionsMenulist = new ObservableCollection<DetectionDetails>(DetectionsMenulist);
                            SelectedDetection = DetectionsMenulist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                TileBarArrange(TestList);
                AddCustomSetting();
                MyFilterString = string.Empty;
                AddCustomSettingCount(gridControl);
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetectionItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods

        public void Init()
        {
            try
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
                #region
                //Header = selectedHeader;
                //Type = Header;
                //GetAllModuleDetectionsWaysOptionsSpareParts_V2330 Service Method recplaced with GetAllModuleDetectionsWaysOptionsSpareParts_V2340 by [rdixit][GEOS2-3970][01.12.2022]
                //   DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllModuleDetectionsWaysOptionsSpareParts_V2340());
                //Service GetAllModuleDetectionsWaysOptionsSpareParts_V2370 updated with GetAllModuleDetectionsWaysOptionsSpareParts_V2380  //[rdixit][GEOS2-2922][24.04.2023]
                //Service GetAllModuleDetectionsWaysOptionsSpareParts_V2380 updated with GetAllModuleDetectionsWaysOptionsSpareParts_V2590 [rdixit][GEOS2-6575][31.12.2024] 
                #endregion
                DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllModuleDetectionsWaysOptionsSpareParts_V2590());                
                TempDetectionsMenulist = DetectionsMenulist;               
                if (DetectionsMenulist != null && DetectionsMenulist.Count() > 0)
                {
                    DetectionAllGroupList = DetectionsMenulist.FirstOrDefault().DetectionAllGroupList;
                }
                FillTypes();
                //FillOrderGroupList();
                FillTestTypes();
                FillStatusList();
                FillECOSVisibilityList();
                //FillTemplateMenulist();
                TileBarArrange(TestList);
                AddCustomSetting();
                //TileBarArrange(TemplatesMenuList);

                foreach (DetectionDetails item in DetectionsMenulist)
                {
                    item.TestList = new List<DetectionTypes>(TestList); //[rdixit][13.01.2022][GEOS2-4116]
                    item.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).Value;
                    item.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).HtmlColor;
                    item.ECOSVisibilityList = ECOSVisibilityList.ToList();
                    item.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus);
                    //item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;                   
                    //item.PCMStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                    item.IdPCMStatus = Convert.ToInt32(item.IdStatus);
                    item.PCMStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                    item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                    // item.IdStatus = item.IdStatus;
                    if (item.StatusHTMLColor == "")
                    {
                        //item.StatusHTMLColor = "#FFFFFFFF";

                        //if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "WhiteAndBlue")
                        //{
                        //    item.StatusHTMLColor = "#000000";
                        //}
                        //if (GeosApplication.Instance.UserSettings.FirstOrDefault(x => x.Key == "ThemeName").Value == "BlackAndBlue")
                        //{
                        //    item.StatusHTMLColor = "#FFFFFFFF";
                        //}
                    }
                    item.StatusList = StatusList.ToList();
                    MyFilterString = string.Empty;
                }
                //DetectionsMenulist.OrderBy(x=>x.Name);
                if (DetectionsMenulist.Count > 0)
                    SelectedDetection = DetectionsMenulist.FirstOrDefault();
                ClonedDetectionsList = DetectionsMenulist.Select(x => (DetectionDetails)x.Clone()).ToList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                #region To Delete Old version file [rdixit][GEOS2-6574][30.12.2023] 
                try
                {
                    List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();
                    if (GeosApplication.Instance.UserSettings.ContainsKey("PCMModuleDetectionTemplate_V2590"))
                    {
                        if (GeosApplication.Instance.UserSettings["PCMModuleDetectionTemplate_V2590"].ToString() == "0")
                        {
                            if (File.Exists(DetectionGridSettingFilePath))
                            {
                                File.Delete(DetectionGridSettingFilePath);
                                GeosApplication.Instance.UserSettings["PCMModuleDetectionTemplate_V2590"] = "1";
                                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                                {
                                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                                }
                                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                            }
                        }
                    }
                }
                catch (Exception ex) { GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low); }
                #endregion
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTestTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes..."), category: Category.Info, priority: Priority.Low);

                IList<TestTypes> tempTestTypesList = PCMService.GetAllTestTypes();
                TestTypesList = new ObservableCollection<TestTypes>();
                TestTypesList = new ObservableCollection<TestTypes>(tempTestTypesList);
                SelectedTestTypes = TestTypesList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTypes..."), category: Category.Info, priority: Priority.Low);

                IList<DetectionTypes> tempTestList = PCMService.GetAllDetectionTypes();
                TestList = new ObservableCollection<DetectionTypes>();
                TestList = new ObservableCollection<DetectionTypes>(tempTestList);

                SelectedTest = TestList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTypes()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pjadhav][24.11.2022][GEOS2-3970]
        //private void FillOrderGroupList()
        //{
        //    try
        //    {
        //        if (DetectionAllGroupList == null)
        //        {
        //            DetectionAllGroupList = new List<DetectionGroup>(PCMService.GetDetectionGroupsList(Convert.ToUInt32(2)));
        //            DetectionAllGroupList.AddRange(PCMService.GetDetectionGroupsList(Convert.ToUInt32(3)));
        //            DetectionAllGroupList.AddRange(PCMService.GetDetectionGroupsList(Convert.ToUInt32(4)));
        //        }
        //        foreach (var item in DetectionsMenulist)
        //        {
        //            item.DetectionGroupList = (new ObservableCollection<DetectionGroup>(PCMService.GetDetectionGroupsList(Convert.ToUInt32(item.IdDetectionType)))).ToList();

        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillOrderGroupList()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOrderGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillOrderGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillOrderGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void FillECOSVisibilityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillECOSVisibilityList..."), category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempECOSVisibilityList = PCMService.GetLookupValues(67).ToList();
                ECOSVisibilityList = new List<LookupValue>(tempECOSVisibilityList);
                SelectedECOSVisibility = ECOSVisibilityList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillECOSVisibilityList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillECOSVisibilityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(45);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatusIndex = StatusList.FindIndex(x => x.IdLookupValue == 225);

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDetectionItem()...", category: Category.Info, priority: Priority.Low);
                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        isSaveFromReference = true;
                        UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                    }
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                }

                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null) return;

                DetectionDetails detections = null;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    detections = (DetectionDetails)detailView.DataControl.CurrentItem;


                    if (detections != null)
                    {
                        bool status = IsDetectionColumnChooserVisible;
                        IsDetectionColumnChooserVisible = false;

                        if (detections.DetectionTypes.IdDetectionType == 2 || detections.DetectionTypes.IdDetectionType == 3)
                        {
                            EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                            EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                            EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                            addEditOptionsDetectionsViewModel.RequestClose += handle;

                            addEditOptionsDetectionsViewModel.IsNew = false;
                            addEditOptionsDetectionsViewModel.IsSelectedTestReadOnly = true;
                            addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;

                            if (detections.DetectionTypes.IdDetectionType == 2)
                            {
                                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString();
                                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());
                                addEditOptionsDetectionsViewModel.EditInitDetections(detections);
                            }
                            else if (detections.DetectionTypes.IdDetectionType == 3)
                            {
                                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
                                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());
                                addEditOptionsDetectionsViewModel.EditInitDetections(detections);
                            }
                            addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                            addEditOptionsDetectionsViewModel.IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                            addEditOptionsDetectionsView.ShowDialog();

                            if (addEditOptionsDetectionsViewModel.IsOptionSave || addEditOptionsDetectionsViewModel.IsWaySave || addEditOptionsDetectionsViewModel.IsSparepartSave || addEditOptionsDetectionsViewModel.IsDetectionSave)
                            {
                                if (addEditOptionsDetectionsViewModel.FourOptionWayDetectionSparePartImagesList != null)//[GEOS2-4532][rdixit][29.06.2023]
                                    DetectionGridImageViewModel.StaticDetectionImageList = addEditOptionsDetectionsViewModel.FourOptionWayDetectionSparePartImagesList.ToList();
                                detections.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                                detections.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                                detections.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                                detections.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                                detections.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                                detections.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                                detections.TestTypes.Name = addEditOptionsDetectionsViewModel.SelectedTestType.Name;
                                detections.DetectionGroupList = new List<DetectionGroup>();
                                detections.DetectionGroup = new DetectionGroup();
                                detections.DetectionGroupList = detections.DetectionAllGroupList.Where(j => j.IdDetectionType == addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType).ToList();
                                detections.WeldOrder = addEditOptionsDetectionsViewModel.UpdatedItem.WeldOrder;
                                detections.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                                detections.DetectionTypes.Name = addEditOptionsDetectionsViewModel.SelectedTest.Name;
                                detections.LastUpdate = addEditOptionsDetectionsViewModel.UpdatedItem.LastUpdate;
                                detections.IdPCMStatus = Convert.ToInt32(addEditOptionsDetectionsViewModel.UpdatedItem.IdStatus);
                                detections.IdStatus = addEditOptionsDetectionsViewModel.UpdatedItem.IdStatus;
                                detections.Status = addEditOptionsDetectionsViewModel.SelectedStatus;
                                detections.PCMStatus = addEditOptionsDetectionsViewModel.SelectedStatus.Value;
                                detections.StatusHTMLColor = addEditOptionsDetectionsViewModel.SelectedStatus.HtmlColor;
                                detections.ECOSVisibilityValue = addEditOptionsDetectionsViewModel.SelectedECOSVisibility.Value;
                                detections.IdECOSVisibility = addEditOptionsDetectionsViewModel.SelectedECOSVisibility.IdLookupValue;
                                detections.ECOSVisibilityHTMLColor = addEditOptionsDetectionsViewModel.SelectedECOSVisibility.HtmlColor;
                                if (addEditOptionsDetectionsViewModel.SelectedOrder != null)
                                {
                                    detections.IdGroup = addEditOptionsDetectionsViewModel.SelectedOrder.IdGroup;
                                    detections.DetectionGroup.Name = addEditOptionsDetectionsViewModel.SelectedOrder.Name;
                                    detections.DetectionGroup.IdGroup = Convert.ToUInt32(addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup);
                                }
                                detailView.DataControl.CurrentItem = detections;
                                detailView.ImmediateUpdateRowPosition = true;
                                detailView.EnableImmediatePosting = true;
                                RefreshDetectionView(detailView);
                            }
                            //[rdixit][29.03.2023][GEOS2-4262]
                            else if (addEditOptionsDetectionsViewModel.IsDuplicateDetectionAdded)
                            {
                                Init();
                            }

                        }

                        else if (detections.DetectionTypes.IdDetectionType == 1 || detections.DetectionTypes.IdDetectionType == 4)
                        {
                            EditDetectionView addOptionWayDetectionSparePartView = new EditDetectionView();
                            EditDetectionViewModel addOptionWayDetectionSparePartViewModel = new EditDetectionViewModel();
                            EventHandler handleWaysSparePart = delegate { addOptionWayDetectionSparePartView.Close(); };
                            addOptionWayDetectionSparePartViewModel.RequestClose += handleWaysSparePart;

                            addOptionWayDetectionSparePartViewModel.IsNew = false;
                            //addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Hidden;
                            addOptionWayDetectionSparePartViewModel.IsSelectedTestReadOnly = true;

                            if (detections.DetectionTypes.IdDetectionType == 1)
                            {
                                addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Hidden;
                                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWayHeader").ToString();
                                addOptionWayDetectionSparePartViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());
                                addOptionWayDetectionSparePartViewModel.EditInitWaysAndSparepart(detections);
                            }

                            else if (detections.DetectionTypes.IdDetectionType == 4)
                            {
                                addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Visible;
                                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                                addOptionWayDetectionSparePartViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());
                                addOptionWayDetectionSparePartViewModel.EditInitWaysAndSparepart(detections);
                            }
                            addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                            addOptionWayDetectionSparePartView.ShowDialog();

                            if (addOptionWayDetectionSparePartViewModel.IsWaySave || addOptionWayDetectionSparePartViewModel.IsOptionSave || addOptionWayDetectionSparePartViewModel.IsDetectionSave || addOptionWayDetectionSparePartViewModel.IsSparepartSave)
                            {
                                if (addOptionWayDetectionSparePartViewModel.FourOptionWayDetectionSparePartImagesList != null)//[GEOS2-4532][rdixit][29.06.2023]
                                    DetectionGridImageViewModel.StaticDetectionImageList = addOptionWayDetectionSparePartViewModel.FourOptionWayDetectionSparePartImagesList.ToList();
                                detections.IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                                detections.Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                                detections.Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
                                detections.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetectionType;
                                detections.Code = addOptionWayDetectionSparePartViewModel.UpdatedItem.Code;
                                detections.IdTestType = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdTestType;
                                detections.TestTypes.Name = addOptionWayDetectionSparePartViewModel.SelectedTestType.Name;
                                detections.IdGroup = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdGroup;
                                detections.DetectionGroup = new DetectionGroup();
                                detections.DetectionGroupList = new List<DetectionGroup>();
                                if (addOptionWayDetectionSparePartViewModel.SelectedOrder != null)
                                {
                                    detections.IdGroup = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdGroup;
                                    detections.DetectionGroup.IdGroup = Convert.ToUInt32(addOptionWayDetectionSparePartViewModel.UpdatedItem.IdGroup);
                                    detections.DetectionGroup.Name = addOptionWayDetectionSparePartViewModel.SelectedOrder.Name;
                                }
                                detections.DetectionGroupList = detections.DetectionAllGroupList.Where(j => j.IdDetectionType == addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetectionType).ToList();
                                detections.WeldOrder = addOptionWayDetectionSparePartViewModel.UpdatedItem.WeldOrder;
                                detections.Code = addOptionWayDetectionSparePartViewModel.UpdatedItem.Code;
                                detections.LastUpdate = addOptionWayDetectionSparePartViewModel.UpdatedItem.LastUpdate;
                                detections.DetectionTypes.Name = addOptionWayDetectionSparePartViewModel.SelectedTest.Name;
                                detections.LastUpdate = addOptionWayDetectionSparePartViewModel.UpdatedItem.LastUpdate;
                                detections.IdPCMStatus = Convert.ToInt32(addOptionWayDetectionSparePartViewModel.UpdatedItem.IdStatus);
                                detections.IdStatus = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdStatus;
                                detections.Status = addOptionWayDetectionSparePartViewModel.SelectedStatus;
                                detections.PCMStatus = addOptionWayDetectionSparePartViewModel.SelectedStatus.Value;
                                detections.StatusHTMLColor = addOptionWayDetectionSparePartViewModel.SelectedStatus.HtmlColor;
                                detections.ECOSVisibilityValue = addOptionWayDetectionSparePartViewModel.SelectedECOSVisibility.Value;
                                detections.IdECOSVisibility = addOptionWayDetectionSparePartViewModel.SelectedECOSVisibility.IdLookupValue;
                                detections.ECOSVisibilityHTMLColor = addOptionWayDetectionSparePartViewModel.SelectedECOSVisibility.HtmlColor;
                                detailView.DataControl.CurrentItem = detections;
                                detailView.ImmediateUpdateRowPosition = true;
                                detailView.EnableImmediatePosting = true;
                                RefreshDetectionView(detailView);
                            }
                            //[rdixit][29.03.2023][GEOS2-4262]
                            else if (addOptionWayDetectionSparePartViewModel.IsDuplicateDetectionAdded)
                            {
                                Init();
                            }
                        }

                        IsDetectionColumnChooserVisible = status;
                    }
                }
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDetectionItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDetectionItem()...", category: Category.Info, priority: Priority.Low);
                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        isSaveFromReference = true;
                        UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                    }
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                }

                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (obj == null) return;

                //DetectionDetails detections = null;

                if (obj is TableView)
                {
                    bool status = IsDetectionColumnChooserVisible;
                    IsDetectionColumnChooserVisible = false;

                    TableView detailView = (TableView)obj;
                    //detections = (DetectionDetails)detailView.DataControl.CurrentItem;

                    AddDetectionView addDetectionView = new AddDetectionView();
                    AddDetectionViewModel addDetectionViewModel = new AddDetectionViewModel();

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


                    EventHandler handleWaysSparePart = delegate { addDetectionView.Close(); };
                    addDetectionViewModel.RequestClose += handleWaysSparePart;

                    addDetectionViewModel.IsStackPanelVisible = Visibility.Collapsed;
                    addDetectionViewModel.IsSelectedTestReadOnly = false;
                    addDetectionViewModel.IsNew = true;
                    addDetectionViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWayHeader").ToString();
                    addDetectionViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());

                    addDetectionView.DataContext = addDetectionViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    var ownerInfo = (detailView as FrameworkElement);
                    addDetectionView.Owner = Window.GetWindow(ownerInfo);
                    addDetectionView.ShowDialog();
                    //[rdixit][GEOS2-][25.01.2023] Updated previous Method of add new Detection item in grid
                    detailView.SearchString = null;
                    MyFilterString = string.Empty;

                    #region OldCode

                    //if (addDetectionViewModel.IsWaySave)
                    //{
                    //    DetectionDetails tempWay = new DetectionDetails();

                    //    TableView tableView = (TableView)obj;
                    //    tempWay = (DetectionDetails)tableView.DataControl.CurrentItem;

                    //    if (tempWay.IdGroup != null)
                    //        tempWay.IdGroup = null;
                    //    tempWay.IdDetections = addDetectionViewModel.NewWay.IdDetections;
                    //    tempWay.Name = addDetectionViewModel.NewWay.Name;
                    //    tempWay.Description = addDetectionViewModel.NewWay.Description;
                    //    tempWay.IdDetectionType = (byte)addDetectionViewModel.NewWay.IdDetectionType;
                    //    if (tempWay.DetectionTypes == null)
                    //    {
                    //        tempWay.DetectionTypes = new DetectionTypes();
                    //    }
                    //    tempWay.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                    //    tempWay.IdTestType = addDetectionViewModel.SelectedTestType.IdTestType;
                    //    if (tempWay.TestTypes == null)
                    //    {
                    //        tempWay.TestTypes = new TestTypes();
                    //    }
                    //    tempWay.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                    //    tempWay.WeldOrder = addDetectionViewModel.NewWay.WeldOrder;
                    //    tempWay.Code = addDetectionViewModel.NewWay.Code;
                    //    tempWay.LastUpdate = addDetectionViewModel.NewWay.LastUpdate;
                    //    tempWay.Status.Value = addDetectionViewModel.SelectedStatus.Value;
                    //    tempWay.Status.HtmlColor = addDetectionViewModel.SelectedStatus.HtmlColor;
                    //    tempWay.IdStatus = Convert.ToUInt32(addDetectionViewModel.SelectedStatus.IdLookupValue);

                    //    //tempWay.IdECOSVisibility = addDetectionViewModel.IdECOSVisibility;
                    //    //tempWay.ECOSVisibilityValue = addDetectionViewModel.ECOSVisibilityValue;
                    //    //tempWay.ECOSVisibilityHTMLColor = addDetectionViewModel.ECOSVisibilityHTMLColor;
                    //    DetectionsMenulist.Add(tempWay);
                    //    SelectedDetection = tempWay;
                    //    DetectionsMenulist.OrderBy(x => x.Name);
                    //}

                    //if (addDetectionViewModel.IsSparepartSave)
                    //{
                    //    DetectionDetails tempSparePart = new DetectionDetails();

                    //    TableView tableView = (TableView)obj;
                    //    tempSparePart = (DetectionDetails)tableView.DataControl.CurrentItem;

                    //    //if (tempSparePart.IdGroup != null)
                    //    //    tempSparePart.IdGroup = null;
                    //    tempSparePart.IdDetections = addDetectionViewModel.NewSparePart.IdDetections;
                    //    tempSparePart.Name = addDetectionViewModel.NewSparePart.Name;
                    //    tempSparePart.Description = addDetectionViewModel.NewSparePart.Description;
                    //    tempSparePart.IdDetectionType = (byte)addDetectionViewModel.NewSparePart.IdDetectionType;
                    //    if (tempSparePart.DetectionTypes == null)
                    //    {
                    //        tempSparePart.DetectionTypes = new DetectionTypes();
                    //    }
                    //    tempSparePart.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                    //    tempSparePart.IdTestType = addDetectionViewModel.SelectedTestType.IdTestType;
                    //    if (tempSparePart.TestTypes == null)
                    //    {
                    //        tempSparePart.TestTypes = new TestTypes();
                    //    }
                    //    tempSparePart.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;

                    //    if (tempSparePart.DetectionGroup == null)
                    //    {
                    //        tempSparePart.DetectionGroup = new DetectionGroup();
                    //    }
                    //    if (addDetectionViewModel.NewDetection != null)
                    //    {
                    //        tempSparePart.IdGroup = addDetectionViewModel.NewDetection.IdGroup;
                    //        tempSparePart.DetectionGroup.Name = addDetectionViewModel.NewDetection.Name;
                    //    }
                    //    tempSparePart.WeldOrder = addDetectionViewModel.NewSparePart.WeldOrder;
                    //    tempSparePart.Code = addDetectionViewModel.NewSparePart.Code;
                    //    tempSparePart.LastUpdate = addDetectionViewModel.NewSparePart.LastUpdate;
                    //    tempSparePart.Status.Value = addDetectionViewModel.SelectedStatus.Value;
                    //    tempSparePart.Status.HtmlColor = addDetectionViewModel.SelectedStatus.HtmlColor;
                    //    tempSparePart.IdStatus = Convert.ToUInt32(addDetectionViewModel.SelectedStatus.IdLookupValue);

                    //    DetectionsMenulist.Add(tempSparePart);
                    //    DetectionsMenulist.OrderBy(x => x.Name);
                    //}

                    //if (addDetectionViewModel.IsDetectionSave)
                    //{
                    //    DetectionDetails tempDetection = new DetectionDetails();
                    //    TableView tableView = (TableView)obj;
                    //    tempDetection = (DetectionDetails)tableView.DataControl.CurrentItem;

                    //    tempDetection.IdDetections = addDetectionViewModel.NewDetection.IdDetections;
                    //    tempDetection.Name = addDetectionViewModel.NewDetection.Name;
                    //    tempDetection.Description = addDetectionViewModel.NewDetection.Description;
                    //    tempDetection.IdDetectionType = (byte)addDetectionViewModel.NewDetection.IdDetectionType;
                    //    tempDetection.Code = addDetectionViewModel.NewDetection.Code;
                    //    if (tempDetection.DetectionTypes == null)
                    //    {
                    //        tempDetection.DetectionTypes = new DetectionTypes();
                    //    }
                    //    tempDetection.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                    //    if (tempDetection.TestTypes == null)
                    //    {
                    //        tempDetection.TestTypes = new TestTypes();
                    //    }
                    //    tempDetection.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;

                    //    if (addDetectionViewModel.NewDetection != null)
                    //    {
                    //        tempDetection.IdGroup = addDetectionViewModel.NewDetection.IdGroup;
                    //    }
                    //    if (tempDetection.DetectionGroup == null)
                    //    {
                    //        tempDetection.DetectionGroup = new DetectionGroup();
                    //    }
                    //    tempDetection.DetectionGroup.Name = addDetectionViewModel.NewDetection.Name;
                    //    tempDetection.WeldOrder = addDetectionViewModel.NewDetection.WeldOrder;
                    //    tempDetection.LastUpdate = addDetectionViewModel.NewDetection.LastUpdate;
                    //    tempDetection.Status.Value = addDetectionViewModel.SelectedStatus.Value;
                    //    tempDetection.Status.HtmlColor = addDetectionViewModel.SelectedStatus.HtmlColor;
                    //    tempDetection.IdStatus = Convert.ToUInt32(addDetectionViewModel.SelectedStatus.IdLookupValue);

                    //    DetectionsMenulist.Add(tempDetection);
                    //    DetectionsMenulist.OrderBy(x => x.Name);
                    //}

                    //if (addDetectionViewModel.IsOptionSave)
                    //{
                    //    DetectionDetails tempOption = new DetectionDetails();
                    //    TableView tableView = (TableView)obj;
                    //    tempOption = (DetectionDetails)tableView.DataControl.CurrentItem;

                    //    tempOption.IdDetections = addDetectionViewModel.NewOption.IdDetections;
                    //    tempOption.Name = addDetectionViewModel.NewOption.Name;
                    //    tempOption.Description = addDetectionViewModel.NewOption.Description;
                    //    tempOption.IdDetectionType = (byte)addDetectionViewModel.NewOption.IdDetectionType;
                    //    tempOption.Code = addDetectionViewModel.NewOption.Code;
                    //    if (tempOption.DetectionTypes == null)
                    //    {
                    //        tempOption.DetectionTypes = new DetectionTypes();
                    //    }
                    //    tempOption.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                    //    if (tempOption.TestTypes == null)
                    //    {
                    //        tempOption.TestTypes = new TestTypes();
                    //    }
                    //    tempOption.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                    //    tempOption.IdGroup = addDetectionViewModel.NewOption.IdGroup;
                    //    if (tempOption.DetectionGroup == null)
                    //    {
                    //        tempOption.DetectionGroup = new DetectionGroup();
                    //    }
                    //    tempOption.DetectionGroup.Name = addDetectionViewModel.NewOption.Name;
                    //    tempOption.WeldOrder = addDetectionViewModel.NewOption.WeldOrder;
                    //    tempOption.LastUpdate = addDetectionViewModel.NewOption.LastUpdate;
                    //    tempOption.Status.Value = addDetectionViewModel.SelectedStatus.Value;
                    //    tempOption.Status.HtmlColor = addDetectionViewModel.SelectedStatus.HtmlColor;
                    //    tempOption.IdStatus = Convert.ToUInt32(addDetectionViewModel.SelectedStatus.IdLookupValue);

                    //    DetectionsMenulist.Add(tempOption);
                    //    DetectionsMenulist.OrderBy(x => x.Name);
                    //}

                    #endregion

                    if (addDetectionViewModel.IsWaySave || addDetectionViewModel.IsOptionSave || addDetectionViewModel.IsSparepartSave || addDetectionViewModel.IsDetectionSave)
                    {
                        if (addDetectionViewModel.FourOptionWayDetectionSparePartImagesList != null)//[GEOS2-4532][rdixit][29.06.2023]
                            DetectionGridImageViewModel.StaticDetectionImageList = addDetectionViewModel.FourOptionWayDetectionSparePartImagesList.ToList();
                        DetectionDetails NewItem = new DetectionDetails();
                        NewItem.DetectionTypes = new DetectionTypes();
                        NewItem.TestTypes = new TestTypes();
                        NewItem.DetectionGroupList = new List<DetectionGroup>();
                        NewItem.DetectionGroup = new DetectionGroup();
                        TableView tableView = (TableView)obj;
                        if (addDetectionViewModel.IsOptionSave)
                        {
                            NewItem.IdGroup = addDetectionViewModel.NewOption.IdGroup;
                            NewItem.DetectionGroup.IdGroup = (uint)addDetectionViewModel.NewOption.IdGroup;
                            NewItem.DetectionGroup.Name = NewItem.DetectionGroup.OriginalName = addDetectionViewModel.NewOption.DetectionOrderGroup.Name;
                            NewItem.IdDetections = addDetectionViewModel.NewOption.IdDetections;
                            NewItem.Name = addDetectionViewModel.NewOption.Name;
                            NewItem.Description = addDetectionViewModel.NewOption.Description;
                            NewItem.IdDetectionType = (byte)addDetectionViewModel.NewOption.IdDetectionType;
                            NewItem.Code = addDetectionViewModel.NewOption.Code;
                            NewItem.WeldOrder = addDetectionViewModel.NewOption.WeldOrder;
                            NewItem.LastUpdate = addDetectionViewModel.NewOption.LastUpdate;
                        }
                        if (addDetectionViewModel.IsWaySave)
                        {
                            NewItem.IdDetections = addDetectionViewModel.NewWay.IdDetections;
                            NewItem.Name = addDetectionViewModel.NewWay.Name;
                            NewItem.Description = addDetectionViewModel.NewWay.Description;
                            NewItem.IdDetectionType = (byte)addDetectionViewModel.NewWay.IdDetectionType;
                            NewItem.Code = addDetectionViewModel.NewWay.Code;
                            NewItem.WeldOrder = addDetectionViewModel.NewWay.WeldOrder;
                            NewItem.LastUpdate = addDetectionViewModel.NewWay.LastUpdate;
                        }
                        if (addDetectionViewModel.IsSparepartSave)
                        {
                            NewItem.IdGroup = addDetectionViewModel.NewSparePart.IdGroup;
                            NewItem.DetectionGroup.IdGroup = (uint)addDetectionViewModel.NewSparePart.IdGroup;
                            NewItem.DetectionGroup.Name = NewItem.DetectionGroup.OriginalName = addDetectionViewModel.NewSparePart.DetectionOrderGroup.Name;
                            NewItem.IdDetections = addDetectionViewModel.NewSparePart.IdDetections;
                            NewItem.Name = addDetectionViewModel.NewSparePart.Name;
                            NewItem.Description = addDetectionViewModel.NewSparePart.Description;
                            NewItem.IdDetectionType = (byte)addDetectionViewModel.NewSparePart.IdDetectionType;
                            NewItem.Code = addDetectionViewModel.NewSparePart.Code;
                            NewItem.WeldOrder = addDetectionViewModel.NewSparePart.WeldOrder;
                            NewItem.LastUpdate = addDetectionViewModel.NewSparePart.LastUpdate;
                        }
                        if (addDetectionViewModel.IsDetectionSave)
                        {

                            NewItem.IdGroup = addDetectionViewModel.NewDetection.IdGroup;
                            NewItem.DetectionGroup.IdGroup = (uint)addDetectionViewModel.NewDetection.IdGroup;
                            NewItem.DetectionGroup.Name = NewItem.DetectionGroup.OriginalName = addDetectionViewModel.NewDetection.DetectionOrderGroup.Name;
                            NewItem.IdDetections = addDetectionViewModel.NewDetection.IdDetections;
                            NewItem.Name = addDetectionViewModel.NewDetection.Name;
                            NewItem.Description = addDetectionViewModel.NewDetection.Description;
                            NewItem.IdDetectionType = (byte)addDetectionViewModel.NewDetection.IdDetectionType;
                            NewItem.Code = addDetectionViewModel.NewDetection.Code;
                            NewItem.WeldOrder = addDetectionViewModel.NewDetection.WeldOrder;
                            NewItem.LastUpdate = addDetectionViewModel.NewDetection.LastUpdate;
                        }
                        if (NewItem.DetectionGroupList != null)
                            NewItem.DetectionGroupList = NewItem.DetectionGroupList;
                        NewItem.DetectionAllGroupList = new List<DetectionGroup>(DetectionAllGroupList);
                        NewItem.TestList = new List<DetectionTypes>(TestList);
                        NewItem.DetectionTypes = addDetectionViewModel.SelectedTest;
                        NewItem.TestTypes = addDetectionViewModel.SelectedTestType;
                        NewItem.Status = addDetectionViewModel.SelectedStatus;
                        NewItem.IdPCMStatus = (int?)addDetectionViewModel.SelectedStatus.IdLookupValue;
                        NewItem.PCMStatus = addDetectionViewModel.SelectedStatus.Value;
                        NewItem.ECOSVisibilityValue = addDetectionViewModel.SelectedECOSVisibility.Value;
                        NewItem.IdECOSVisibility = addDetectionViewModel.SelectedECOSVisibility.IdLookupValue;
                        NewItem.ECOSVisibilityHTMLColor = addDetectionViewModel.SelectedECOSVisibility.HtmlColor;
                        NewItem.ECOSVisibilityList = ECOSVisibilityList.ToList();
                        NewItem.StatusHTMLColor = addDetectionViewModel.SelectedStatus.HtmlColor;

                        DetectionsMenulist.Add(NewItem); ClonedDetectionsList.Add(NewItem);
                        DetectionsMenulist.OrderBy(x => x.Name);
                        detailView.DataControl.CurrentItem = NewItem;
                        detailView.ImmediateUpdateRowPosition = true;
                        detailView.EnableImmediatePosting = true;
                        RefreshDetectionView(detailView);

                        IsDetectionColumnChooserVisible = status;
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddDetectionItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshDetectionView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()...", category: Category.Info, priority: Priority.Low);

                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                    }
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                }

                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;

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
                FillTestTypes();
                FillTypes();
                FillStatusList();
                #region
                // FillECOSVisibilityList();              
                //GetAllModuleDetectionsWaysOptionsSpareParts_V2330 Service Method recplaced with GetAllModuleDetectionsWaysOptionsSpareParts_V2340 by [rdixit][GEOS2-3970][01.12.2022]
                //  DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllModuleDetectionsWaysOptionsSpareParts_V2340());
                //[Sudhir.Jangra][GEOS2-2922][31/03/2023] GetAllModuleDetectionsWaysOptionsSpareParts_V2340 Service Method replaced with GetAllModuleDetectionsWaysOptionsSpareParts_V2370
                //Service GetAllModuleDetectionsWaysOptionsSpareParts_V2370 updated with GetAllModuleDetectionsWaysOptionsSpareParts_V2380 //[rdixit][GEOS2-2922][24.04.2023]
                //Service GetAllModuleDetectionsWaysOptionsSpareParts_V2380 updated with GetAllModuleDetectionsWaysOptionsSpareParts_V2590 [rdixit][GEOS2-6575][31.12.2024] 
                #endregion
                DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllModuleDetectionsWaysOptionsSpareParts_V2590());

                foreach (DetectionDetails item in DetectionsMenulist)
                {
                    item.TestList = new List<DetectionTypes>(TestList);
                    item.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).Value;
                    item.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).HtmlColor;
                    item.IdPCMStatus = Convert.ToInt32(item.IdStatus);
                    item.PCMStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                    item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                    item.ECOSVisibilityList = ECOSVisibilityList.ToList();
                    item.StatusList = StatusList.ToList();

                }
                ClonedDetectionsList = new List<DetectionDetails>(DetectionsMenulist.Select(x => (DetectionDetails)x.Clone()).ToList());
                SelectedDetection = DetectionsMenulist.FirstOrDefault();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                detailView.SearchString = null;
                MyFilterString = string.Empty;
                //[rdixit][GEOS2-4119][23.01.2023]
                TileBarArrange(TestList);
                AddCustomSetting();
                AddCustomSettingCount(gridControl);
                DetectionsMenulist = new ObservableCollection<DetectionDetails>(ClonedDetectionsList.Select(x => (DetectionDetails)x.Clone()).ToList());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDetectionView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintDetections(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);
                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        Init();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    }
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                }

                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    Init();
                }
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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4Plus;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["DetectionsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["DetectionsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportDetections(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDetections()...", category: Category.Info, priority: Priority.Low);
                view = DetectionsViewMultipleCellEditHelper.Viewtableview;
                if (DetectionsViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        UpdateMultipleRowsDetectionGridCommandAction(DetectionsViewMultipleCellEditHelper.Viewtableview);
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        //FillArticleList();
                        Init();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    }
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                }

                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                    //FillArticleList();
                    Init();
                }
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Detections List";
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

                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();

                    GeosApplication.Instance.Logger.Log("Method ExportDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportDetections()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {

            if (e.Value != null && e.Value.ToString() != "ECOS Visibility" && e.ColumnFieldName == "IdECOSVisibility")
            {
                e.Value = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == (int)e.Value).Value;
            }

            if (e.Value != null && e.Value.ToString() != "Test Type" && e.ColumnFieldName == "IdTestType")
            {
                if (TestTypesList.Any(a => a.IdTestType == (ulong)e.Value))
                {
                    e.Value = TestTypesList.FirstOrDefault(a => a.IdTestType == (ulong)e.Value).Name;
                }
            }

            if (e.Value != null && e.Value.ToString() != "Type" && e.ColumnFieldName == "IdDetectionType")
            {
                if (TestList.Any(a => a.IdDetectionType == (uint)e.Value))
                {
                    e.Value = TestList.FirstOrDefault(a => a.IdDetectionType == (uint)e.Value).Name;
                }
            }

            if (e.Value != null && e.Value.ToString() != "Status" && e.ColumnFieldName == "IdPCMStatus")
            {

                e.Value = StatusList.FirstOrDefault(a => a.IdLookupValue == (int)e.Value).Value;
            }

            if (e.Value != null && e.Value.ToString() != "Group" && e.ColumnFieldName == "IdGroup")
            {
                if (DetectionAllGroupList.Any(a => a.IdGroup == (UInt32)e.Value))
                {
                    e.Value = DetectionAllGroupList.FirstOrDefault(a => a.IdGroup == (UInt32)e.Value).OriginalName;
                }
            }

            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        /// <param name="obj"></param>
        /// [001] [cpatil][27-09-2021][GEOS2-3340] [Sr N  4 - Synchronization between PCM and ECOS. [#PLM69]]
        public async void UpdateMultipleRowsDetectionGridCommandAction(object obj)
        {
            try
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
                view = obj as TableView;
                GridControl gridControl = (view).Grid;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                DetectionsView detectionsView = new Views.DetectionsView();
                IsDetectionSave = false;
                IsAllSave = false;

                int MinValue = 0;
                int MaxValue = 0;
                int IdECOSValue = 0;
                string Name = string.Empty;
                MinValue = DetectionsViewMultipleCellEditHelper.Min;
                MaxValue = DetectionsViewMultipleCellEditHelper.Max;
                IdECOSValue = ECOSVisibilityList.Where(a => a.IdLookupValue == DetectionsViewMultipleCellEditHelper.IdECOS).Select(u => u.IdLookupValue).FirstOrDefault();

                DetectionDetails[] foundRow = DetectionsMenulist.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();

                List<DetectionDetails> temp = (PCMService.PCM_GetshortDetectionDetails_V2160());
                foreach (DetectionDetails item in foundRow)
                {
                    DetectionDetails AI = item;
                    //[rdixit][13.01.2022][GEOS2-4116]
                    DetectionDetails oldDetectionDetails = temp.Where(t => t.IdDetections == item.IdDetections).FirstOrDefault();
                    DetectionDetails oldData = ClonedDetectionsList.FirstOrDefault(a => a.IdDetections == item.IdDetections);
                    DetectionDetails _Detection = new DetectionDetails();
                    _Detection.Description = item.Description != null ? item.Description.Trim() : item.Description;
                    try
                    {
                        if (_Detection.Description == null)
                        {
                            _Detection.Description = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    _Detection.IdECOSVisibility = ECOSVisibilityList.FirstOrDefault(a => a.Value == item.ECOSVisibilityValue).IdLookupValue;
                    AI.IdECOSVisibility = ECOSVisibilityList.FirstOrDefault(a => a.Value == item.ECOSVisibilityValue).IdLookupValue;
                    _Detection.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.Value == item.ECOSVisibilityValue).Value;
                    _Detection.IdDetections = item.IdDetections;
                    _Detection.IdStatus = Convert.ToUInt32(StatusList.Where(i => i.Value == item.PCMStatus).FirstOrDefault().IdLookupValue);
                    _Detection.IdPCMStatus = Convert.ToInt32(StatusList.Where(i => i.Value == item.PCMStatus).FirstOrDefault().IdLookupValue);
                    _Detection.DetectionTypes = TestList.Where(i => i.Name == item.DetectionTypes.Name).FirstOrDefault();
                    _Detection.IdDetectionType = TestList.Where(i => i.Name == item.DetectionTypes.Name).FirstOrDefault().IdDetectionType;
                    _Detection.IdStatus = Convert.ToUInt32(StatusList.Where(i => i.Value == item.PCMStatus).FirstOrDefault().IdLookupValue);
                    _Detection.IdPCMStatus = Convert.ToInt32(StatusList.Where(i => i.Value == item.PCMStatus).FirstOrDefault().IdLookupValue);
                    _Detection.Code = item.Code;
                    if (item.DetectionGroup != null)
                    {
                        if (item.DetectionGroup.Name != null && item.DetectionGroup.Name != "")
                            _Detection.IdGroup = item.DetectionGroupList.Where(j => j.OriginalName == item.DetectionGroup.Name).FirstOrDefault().IdGroup;
                    }
                    _Detection.WeldOrder = item.WeldOrder;
                    _Detection.IdTestType = TestTypesList.Where(i => i.Name == item.TestTypes.Name).FirstOrDefault().IdTestType;
                    _Detection.TestTypes = TestTypesList.Where(i => i.Name == item.TestTypes.Name).FirstOrDefault();
                    if (AI.IdECOSVisibility == 323)
                    {
                        _Detection.IsShareWithCustomer = 1;
                        _Detection.IsSparePartOnly = 0;

                        if (_Detection.PurchaseQtyMin == 0)
                        {
                            _Detection.PurchaseQtyMin = 1;
                            AI.PurchaseQtyMin = 1;
                        }

                        if (_Detection.PurchaseQtyMax == 0)
                        {
                            _Detection.PurchaseQtyMax = 1;
                            AI.PurchaseQtyMax = 1;
                        }
                    }
                    else if (AI.IdECOSVisibility == 324)
                    {
                        _Detection.IsShareWithCustomer = 0;
                        _Detection.IsSparePartOnly = 1;

                        if (_Detection.PurchaseQtyMin == 0)
                        {
                            _Detection.PurchaseQtyMin = 1;
                            AI.PurchaseQtyMin = 1;
                        }

                        if (_Detection.PurchaseQtyMax == 0)
                        {
                            _Detection.PurchaseQtyMax = 1;
                            AI.PurchaseQtyMax = 1;
                        }
                    }
                    else if (AI.IdECOSVisibility == 325)
                    {
                        _Detection.IsShareWithCustomer = 0;
                        _Detection.IsSparePartOnly = 0;

                        if (_Detection.PurchaseQtyMin == 0)
                        {
                            _Detection.PurchaseQtyMin = 1;
                            AI.PurchaseQtyMin = 1;
                        }

                        if (_Detection.PurchaseQtyMax == 0)
                        {
                            _Detection.PurchaseQtyMax = 1;
                            AI.PurchaseQtyMax = 1;
                        }
                    }
                    else if (AI.IdECOSVisibility == 326)
                    {
                        _Detection.IsShareWithCustomer = 0;
                        _Detection.IsSparePartOnly = 0;
                        _Detection.PurchaseQtyMin = 0;
                        _Detection.PurchaseQtyMax = 0;
                    }
                    _Detection.DetectionLogEntryList = new List<DetectionLogEntry>();
                    //ECOS visibility
                    if (_Detection.IdECOSVisibility != oldDetectionDetails.IdECOSVisibility)
                    {
                        LookupValue tempECOS = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == _Detection.IdECOSVisibility);
                        LookupValue tempvalue = ECOSVisibilityList.FirstOrDefault(x => x.IdLookupValue == oldDetectionDetails.IdECOSVisibility);
                        if (tempECOS != null)
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogECOSVisibility").ToString(), tempvalue.Value, tempECOS.Value, oldData.Name) });
                    }
                    if (_Detection.IdStatus != oldDetectionDetails.IdStatus)
                    {
                        LookupValue tempStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == _Detection.IdStatus);
                        LookupValue tempStatusvalue = StatusList.FirstOrDefault(x => x.IdLookupValue == oldDetectionDetails.IdStatus);
                        if (tempStatus != null)
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogStatus").ToString(), tempStatusvalue.Value, tempStatus.Value) });
                    }
                    //[rdixit][GEOS2-3970][01.12.2022]         
                    #region Code
                    if (_Detection.Code != oldData.Code)
                    {
                        if (oldData.Code == null)
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), "None", _Detection.Code)
                            });
                        }
                        else if (_Detection.Code == null)
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), oldData.Code, "None")
                            });
                        }
                        else
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogCode").ToString(), oldData.Code, _Detection.Code)
                            });
                        }
                    }








                    #endregion

                    #region WeldOrder
                    if (_Detection.WeldOrder != oldData.WeldOrder)
                    {

                        _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                        {
                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogWeldOrder").ToString(),
                            oldData.WeldOrder, _Detection.WeldOrder)
                        });
                    }




                    #endregion

                    #region DetectionType
                    if (_Detection.IdDetectionType != oldData.IdDetectionType)
                    {
                        DetectionTypes tempTestType = TestList.FirstOrDefault(x => x.IdDetectionType == _Detection.IdDetectionType);
                        DetectionTypes tempTestTypeName = TestList.FirstOrDefault(x => x.IdDetectionType == oldData.IdDetectionType);
                        _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                        {
                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDetectionType").ToString(),
                            tempTestTypeName.Name, tempTestType.Name)
                        });
                    }




                    #endregion

                    #region Description
                    if (_Detection.Description != oldData.Description)
                    {
                        if (oldData.Description == null)
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), "None", _Detection.Description)
                            });
                        }
                        else if (_Detection.Description == null)
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), oldData.Description, "None")
                            });
                        }
                        else
                        {
                            _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogDescription").ToString(), oldData.Description, _Detection.Description)
                            });
                        }
                    }






                    #endregion

                    #region Group
                    if ((_Detection.IdGroup == null ? _Detection.IdGroup = 0 : _Detection.IdGroup) != (oldData.IdGroup == null ? oldData.IdGroup = 0 : oldData.IdGroup))
                    {
                        if (oldData.DetectionAllGroupList != null)
                        {
                            DetectionGroup oldGroupName = oldData.DetectionAllGroupList.Where(j => j.IdGroup == oldData.IdGroup).FirstOrDefault();
                            DetectionGroup NewGroupName = oldData.DetectionAllGroupList.Where(j => j.IdGroup == _Detection.IdGroup).FirstOrDefault();
                            if (oldData.IdGroup == 0 || oldData.IdGroup == null)
                            {
                                _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), "None", NewGroupName.OriginalName)
                                });
                            }
                            else if (_Detection.IdGroup == 0 || _Detection.IdGroup == null)
                            {
                                _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), oldGroupName.OriginalName, "None")
                                });
                            }
                            else
                            {
                                _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                                {
                                    IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogOrderGroup").ToString(), oldGroupName.OriginalName, NewGroupName.OriginalName)
                                });
                            }
                        }
                    }














                    #endregion

                    #region TestType
                    if (_Detection.IdTestType != oldData.IdTestType)
                    {

                        TestTypes OldTestType = TestTypesList.FirstOrDefault(x => x.IdTestType == oldData.IdTestType);
                        TestTypes NewTestType = TestTypesList.FirstOrDefault(x => x.IdTestType == _Detection.IdTestType);

                        _Detection.DetectionLogEntryList.Add(new DetectionLogEntry()
                        {
                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogTestType").ToString(),
                            OldTestType.Name, NewTestType.Name)
                        });

                    }
                    #endregion
                    //_Detection.DetectionLogEntryList.Add(ChangeLogList.ToList());
                    //IsDetectionSave = PCMService.IsUpdatePCM_DetectionECOSVisibility_Update_V2160(_Detection);
                    //IsDetectionSave = PCMService.IsUpdatePCM_DetectionECOSVisibility_Update_V2340(_Detection);
                    //[RGadhave][GEOS2-5896][25.09.2024]
                    _Detection.LastUpdate = DateTime.Now;
                    _Detection.LastUpdate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    IsDetectionSave = PCMService.IsUpdatePCM_DetectionECOSVisibility_Update_V2560(_Detection);


                }
                if (IsDetectionSave)
                {
                    IsAllSave = true;
                    if (isSaveFromReference == false)
                    {
                        Init();
                    }
                    //[rdixit][GEOS2-4119][21.01.2023]
                    DetectionsViewMultipleCellEditHelper.IsValueChanged = false;
                    RefreshDetectionView(view);
                    isSaveFromReference = false;

                }
                else
                    IsAllSave = false;

                if (IsAllSave == null)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
                else if (IsAllSave.Value == true)
                {

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                else if (IsAllSave.Value == false)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                DetectionsViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                DetectionsViewMultipleCellEditHelper.IsValueChanged = false;


                //[001] Added code for synchronization
                if (GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization == true)
                {
                    GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("58,59");
                    if (GeosAppSettingList != null && GeosAppSettingList.Count != 0)
                    {
                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 59) && GeosAppSettingList.Any(i => i.IdAppSetting == 58))
                        {
                            if ((!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) && (!string.IsNullOrEmpty((GeosAppSettingList[1].DefaultValue))))    //(!string.IsNullOrEmpty((GeosAppSettingList[0].DefaultValue))) // && (GeosAppSettingList[1].DefaultValue)))  //.Where(i => i.IdAppSetting == 57).Select(x => x.DefaultValue)))) // && (GeosAppSettingList[1].DefaultValue))) // Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue.to)
                            {
                                #region Synchronization Code
                                var ownerInfo = (obj as FrameworkElement);
                                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                                if (MessageBoxResult == MessageBoxResult.Yes)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "The Synchronization is running";

                                    if (foundRow.Count() > 0)
                                        GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("58,59");
                                    if (!DXSplashScreen.IsActive)
                                    {
                                        DXSplashScreen.Show(x =>
                                        {
                                            Window win = new Window()
                                            {
                                                Focusable = true,
                                                ShowActivated = false,
                                                WindowStyle = WindowStyle.None,
                                                ResizeMode = ResizeMode.NoResize,
                                                AllowsTransparency = false,
                                                Background = new SolidColorBrush(Colors.Transparent),
                                                ShowInTaskbar = false,
                                                Topmost = false,
                                                SizeToContent = SizeToContent.WidthAndHeight,
                                                WindowStartupLocation = WindowStartupLocation.CenterScreen,

                                            };
                                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);


                                            return win;
                                        }, x =>
                                        {
                                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                                        }, new object[] { new SplashScreenOwner(ownerInfo), WindowStartupLocation.CenterOwner }, null);
                                    }

                                    try
                                    {
                                        APIErrorDetail values = new APIErrorDetail();
                                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                                        {
                                            string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                                            if (tokeninformations.Count() >= 2)
                                            {

                                                if (GeosAppSettingList.Any(i => i.IdAppSetting == 58))
                                                {

                                                    APIErrorDetailForErrorFalse valuesErrorFalse = await PCMService.IsPCMDetectionSynchronization(GeosAppSettingList);
                                                    if (valuesErrorFalse.Code.Equals("ECOSSynchronizationFailed") && valuesErrorFalse.Error.Equals("ECOSSynchronizationFailed"))
                                                    {
                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), valuesErrorFalse.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                    }
                                                    else
                                                    {
                                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                                        if (valuesErrorFalse != null && valuesErrorFalse.Error == "false")
                                                        {

                                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                        }
                                                        else
                                                        {
                                                            if (values != null && values.Message != null)
                                                            {
                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), values.Message._Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                            }
                                                            else if (valuesErrorFalse != null && valuesErrorFalse.Message != null)
                                                            {
                                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), valuesErrorFalse.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));

                                    }
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                }
                                #endregion
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsDetectionGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsDetectionGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsDetectionGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OnViewLoaded(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if (DetectionsViewMultipleCellEditHelper.IsLoad == true)
                {
                    DetectionDetails[] foundRow = DetectionsMenulist.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                    foreach (DetectionDetails item in foundRow)
                    {
                        DetectionDetails oldData = ClonedDetectionsList.FirstOrDefault(a => a.IdDetections == item.IdDetections);

                        item.Description = oldData.Description;
                        item.IdECOSVisibility = oldData.IdECOSVisibility;
                        item.ECOSVisibilityValue = oldData.ECOSVisibilityValue;
                        item.ECOSVisibilityHTMLColor = oldData.ECOSVisibilityHTMLColor;

                        item.PurchaseQtyMin = oldData.PurchaseQtyMin;
                        item.PurchaseQtyMax = oldData.PurchaseQtyMax;

                        item.IdStatus = oldData.IdStatus;
                        // item.PcmArticleCategory = oldData.CategoryMenulist.FirstOrDefault(a => a.IdPCMArticleCategory == oldData.IdPCMArticleCategory);
                        item.StatusHTMLColor = oldData.StatusHTMLColor;
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                        ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                    }
                }
                AddCustomSettingCount(gridControl);
                GeosApplication.Instance.Logger.Log("Method OnViewLoaded() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method OnViewLoaded()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);
                if (e.Column.FieldName == "IdECOSVisibility")
                {
                    List<object> filterItems = new List<object>();

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {

                        item.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).Value;
                        item.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).HtmlColor;
                        item.ECOSVisibilityList = ECOSVisibilityList.ToList();
                        item.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus);
                        item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                        item.StatusList = StatusList.ToList();

                        if (item.ECOSVisibilityValue == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == item.ECOSVisibilityValue))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = item.ECOSVisibilityValue;
                            customComboBoxItem.EditValue = item.IdECOSVisibility;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                if (e.Column.FieldName == "IdGroup")
                {
                    List<object> filterItems = new List<object>();

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {
                        if (item.DetectionGroup != null)
                        {
                            item.DetectionGroup.OriginalName = item.DetectionGroup.Name;


                            if (item.DetectionGroup.Name == null)
                            {
                                continue;
                            }

                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == item.DetectionGroup.OriginalName))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = item.DetectionGroup.OriginalName;
                                customComboBoxItem.EditValue = item.IdGroup;
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                if (e.Column.FieldName == "IdDetectionType")
                {
                    List<object> filterItems = new List<object>();

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {


                        if (item.DetectionTypes.Name == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == item.DetectionTypes.Name))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = item.DetectionTypes.Name;
                            customComboBoxItem.EditValue = item.IdDetectionType;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                if (e.Column.FieldName == "IdTestType")
                {
                    List<object> filterItems = new List<object>();

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {


                        if (item.TestTypes.Name == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == item.TestTypes.Name))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = item.TestTypes.Name;
                            customComboBoxItem.EditValue = item.IdTestType;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                if (e.Column.FieldName == "IdPCMStatus")
                {
                    List<object> filterItems = new List<object>();

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {
                        item.ECOSVisibilityValue = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).Value;
                        item.ECOSVisibilityHTMLColor = ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == item.IdECOSVisibility).HtmlColor;
                        item.ECOSVisibilityList = ECOSVisibilityList.ToList();
                        item.IdPCMStatus = Convert.ToInt32(item.IdStatus);
                        item.PCMStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                        item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                        item.StatusList = StatusList.ToList();

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

                if (e.Column.FieldName == "ImageCount")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("ImageCount is null")
                    });

                    foreach (DetectionDetails item in DetectionsMenulist)
                    {
                        if (item.ImageCount == null)
                        {
                            continue;
                        }
                        else
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "Image(s)",
                                EditValue = CriteriaOperator.Parse("ImageCount is not null")
                            });
                            break;
                        }

                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                #region [rdixit][GEOS2-6575][31.12.2024]
                else if (e.Column.FieldName == "PCMCustomerInc.RegionName")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerInc.RegionName])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerInc.RegionName])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerInc.RegionName == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerInc.RegionName != null)
                        {
                            if (dataObject.PCMCustomerInc.RegionName.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerInc.RegionName;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerInc.RegionName Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerInc.RegionName == dataObject.PCMCustomerInc.RegionName)
                                    .Select(slt => slt.PCMCustomerInc.RegionName) .FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerInc.RegionName Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerInc.GroupName")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerInc.GroupName])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerInc.GroupName])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerInc.GroupName == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerInc.GroupName != null)
                        {
                            if (dataObject.PCMCustomerInc.GroupName.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerInc.GroupName;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerInc.GroupName Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerInc.GroupName == dataObject.PCMCustomerInc.GroupName)
                                    .Select(slt => slt.PCMCustomerInc.GroupName).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerInc.GroupName Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerInc.Plant.Name")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerInc.Plant.Name])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerInc.Plant.Name])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerInc.Plant.Name == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerInc.Plant.Name != null)
                        {
                            if (dataObject.PCMCustomerInc.Plant.Name.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerInc.Plant.Name;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerInc.Plant.Name Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string plantName = DetectionsMenulist.Where(y => y.PCMCustomerInc.Plant.Name == dataObject.PCMCustomerInc.Plant.Name)
                                    .Select(slt => slt.PCMCustomerInc.Plant.Name).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(plantName) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == plantName))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = plantName,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerInc.Plant.Name Like '%{plantName}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerInc.Country.Name")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerInc.Country.Name])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerInc.Country.Name])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerInc.Country.Name == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerInc.Country.Name != null)
                        {
                            if (dataObject.PCMCustomerInc.Country.Name.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerInc.Country.Name;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerInc.Country.Name Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerInc.Country.Name == dataObject.PCMCustomerInc.Country.Name)
                                    .Select(slt => slt.PCMCustomerInc.Country.Name).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerInc.Country.Name Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }

                else if (e.Column.FieldName == "PCMCustomerExc.RegionName")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerExc.RegionName])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerExc.RegionName])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerExc.RegionName == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerExc.RegionName != null)
                        {
                            if (dataObject.PCMCustomerExc.RegionName.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerExc.RegionName;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerExc.RegionName Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerExc.RegionName == dataObject.PCMCustomerExc.RegionName)
                                    .Select(slt => slt.PCMCustomerExc.RegionName).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerExc.RegionName Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerExc.GroupName")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerExc.GroupName])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerExc.GroupName])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerExc.GroupName == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerExc.GroupName != null)
                        {
                            if (dataObject.PCMCustomerExc.GroupName.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerExc.GroupName;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerExc.GroupName Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerExc.GroupName == dataObject.PCMCustomerExc.GroupName)
                                    .Select(slt => slt.PCMCustomerExc.GroupName).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerExc.GroupName Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerExc.Plant.Name")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerExc.Plant.Name])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerExc.Plant.Name])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerExc.Plant.Name == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerExc.Plant.Name != null)
                        {
                            if (dataObject.PCMCustomerExc.Plant.Name.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerExc.Plant.Name;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerExc.Plant.Name Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string plantName = DetectionsMenulist.Where(y => y.PCMCustomerExc.Plant.Name == dataObject.PCMCustomerExc.Plant.Name)
                                    .Select(slt => slt.PCMCustomerExc.Plant.Name).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(plantName) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == plantName))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = plantName,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerExc.Plant.Name Like '%{plantName}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                else if (e.Column.FieldName == "PCMCustomerExc.Country.Name")
                {
                    List<object> filterItems = new List<object>();
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PCMCustomerExc.Country.Name])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PCMCustomerExc.Country.Name])")
                    });

                    foreach (DetectionDetails dataObject in DetectionsMenulist)
                    {
                        if (dataObject.PCMCustomerExc.Country.Name == null)
                        {
                            continue;
                        }
                        else if (dataObject.PCMCustomerExc.Country.Name != null)
                        {
                            if (dataObject.PCMCustomerExc.Country.Name.Contains("\n"))
                            {
                                string tempPlants = dataObject.PCMCustomerExc.Country.Name;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PCMCustomerExc.Country.Name Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                // Extract the plant name once for reuse
                                string name = DetectionsMenulist.Where(y => y.PCMCustomerExc.Country.Name == dataObject.PCMCustomerExc.Country.Name)
                                    .Select(slt => slt.PCMCustomerExc.Country.Name).FirstOrDefault()?.Trim();

                                if (!string.IsNullOrEmpty(name) && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == name))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem
                                    {
                                        DisplayValue = name,
                                        EditValue = CriteriaOperator.Parse($"PCMCustomerExc.Country.Name Like '%{name}%'")
                                    };
                                    filterItems.Add(customComboBoxItem);
                                }

                            }
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopup()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in TestList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }

                if (CustomFilterStringName == "CUSTOM FILTERS" || CustomFilterStringName == "All")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Template"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TileBarArrange(ObservableCollection<DetectionTypes> templateMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = "All",
                    DisplayText = "All",
                    EntitiesCount = DetectionsMenulist.Count(),
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                if (templateMenulist == null)
                {
                    templateMenulist = new ObservableCollection<DetectionTypes>();
                }
                List<DetectionTypes> TempTemplateList = templateMenulist.Where(a => a.IdTemplate == 0 || a.IdTemplate != 24).ToList();
                if (TempTemplateList != null)
                {
                    foreach (DetectionTypes template in TempTemplateList)
                    {
                        if (!Listofitem.Any(a => a.DisplayText == template.Name))
                        {
                            Listofitem.Add(new TileBarFilters()
                            {
                                Caption = template.Name,
                                DisplayText = template.Name,
                                EntitiesCount = DetectionsMenulist.Count(x => x.DetectionTypes.Name == template.Name && x.Name != null),
                                EntitiesCountVisibility = Visibility.Visible,
                                BackColor = template.Color,
                                FilterCriteria = "[DetectionTypes.Name] In ('" + template.Name + "')",//[rdixit][GEOS2-4116][12.01.2023]
                                Height = 80,
                                width = 200
                            });
                        }
                    }

                }

                Listofitem.Add(new TileBarFilters()
                {


                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    FilterCriteria = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Height = 30,
                    width = 200
                });


                // After change index it will automatically redirect to method ShowSelectedFilterGridAction(object obj) and arrange the tile section count.
                if (Listofitem.Count > 0)
                    SelectedTileIndexDetection = 0;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowSelectedFilterDetectionGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;


                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                            return;


                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {

                                if (!string.IsNullOrEmpty(_FilterString))
                                    MyFilterString = _FilterString;
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;
                        }
                        else
                        {
                            if (str.Equals("All"))
                            {
                                MyFilterString = string.Empty;
                                DetectionsMenulist = new ObservableCollection<DetectionDetails>(DetectionsMenulist.Select(x => (DetectionDetails)x.Clone()).ToList());                           
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                {

                                    if (!string.IsNullOrEmpty(_FilterString))
                                        MyFilterString = _FilterString;
                                    else
                                        MyFilterString = string.Empty;
                                }
                                else
                                    MyFilterString = string.Empty;
                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedFilterGridAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                            Listofitem.Add(
                                     new TileBarFilters()
                                     {
                                         Caption = item.Key.Replace(userSettingsKey, ""),
                                         Id = 0,
                                         BackColor = null,
                                         ForeColor = null,
                                         FilterCriteria = item.Value,
                                         EntitiesCountVisibility = Visibility.Visible,
                                         Height = 80,
                                         width = 200
                                     });
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

                customFilterEditorViewModel.Init(e.FilterControl, Listofitem);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        Listofitem.Remove(tileBarItem);
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
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                        VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
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
                    //[rdixit][GEOS2-4116][12.01.2023]
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                    VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                    Listofitem.Add(new TileBarFilters()
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

                    string filterName = "";

                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = Listofitem.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            ShowFilterEditor(obj);
        }

        private void TableView_SearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        {
            e.Filter = CriteriaOperator.Parse(string.Format("Contains([Name], '{0}')", e.SearchString));
        }

        //[sshegaonkar[GEOS2-2922][16-02-23]
        private void OpenDetectionImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenDetectionImageAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                DetectionDetails SelectedRow = (DetectionDetails)detailView.DataControl.CurrentItem;
                uint IdDetection = Convert.ToUInt32(SelectedRow.IdDetections);
                DetectionGridImageView detectionGridImageView = new DetectionGridImageView();
                DetectionGridImageViewModel detectionGridImageViewModel =  new DetectionGridImageViewModel();
                EventHandler handle = delegate { detectionGridImageView.Close(); };
                detectionGridImageViewModel.RequestClose += handle;
                detectionGridImageViewModel.DetectionImage = new DetectionImage();
                detectionGridImageViewModel.DetectionImage.IdDetection = IdDetection;
                detectionGridImageViewModel.Init(IdDetection);
                detectionGridImageView.DataContext = detectionGridImageViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                detectionGridImageView.ShowDialogWindow();



                GeosApplication.Instance.Logger.Log("Method OpenDetectionImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenDetectionImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Column Chooser
        private void DetectionGridControlLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(DetectionGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(DetectionGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(DetectionGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, DetectionVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, DetectionVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsDetectionColumnChooserVisible = true;
                }
                else
                {
                    IsDetectionColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on DetectionGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void DetectionVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(DetectionGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsDetectionColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method DetectionVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in DetectionVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void DetectionVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(DetectionGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method DetectionVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in DetectionVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DetectionItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }

        private void DetectionGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(DetectionGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method DetectionGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on DetectionGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rdixit][GEOS2-4119][21.01.2023]
        private void AddCustomSettingCount(GridControl gridControl)
        {
            try
            {
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                foreach (var item in tempUserSettings)
                {
                    try
                    {
                        MyFilterString = Listofitem.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        Listofitem.FirstOrDefault(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSettingCount() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method AddCustomSettingCount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSettingCount() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
