using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.Views;
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
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using System.Globalization;
using DevExpress.Data;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class WorkOperationsViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IERMService ERMService = new ERMServiceController("localhost:6699");
        #endregion

        #region Declaration
        TreeListControl treeListControlInstance;
        TableView tableViewInstance;
        public string ERM_WorkOperationsGrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_WorkOperationsGrid_Setting.Xml";

        private bool isBusy;
        private bool isDeleted;
        private ObservableCollection<WorkOperation> workOperationsList;
        private WorkOperation selectedWorkOperationsList;
        private WorkOperation selectedWorkOperations;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private ObservableCollection<WorkOperationByStages> workOperationMenulist;
        private WorkOperationByStages selectedWorkOperationMenulist;
        private ObservableCollection<WorkOperationByStages> clonedWorkOperationByStages;
        private ObservableCollection<WorkOperation> workOperationsList_All;
        private ObservableCollection<Stages> getStages;
        private bool isColumnChooserVisibleForGrid;
        //<!--//[001][rdixit][GEOS2-3710][09/06/2022]-->
        private List<LookupValue> statusList;
        private LookupValue selectedStatus;
        private List<LookupValue> typeList;
        private UInt32? idType;
        private LookupValue selectedType;
        private TableView view;
        private ObservableCollection<WorkOperation> parentList;
        private WorkOperation selectedParent;
        private bool? isAllSave;
        List<WorkOperation> validateParentList;
        bool IsWOExistInSOD;
        #region GEOS2-3880 Work Operation log
        private ObservableCollection<WorkOperationChangeLog> workOperationChangeLogList;
        private ObservableCollection<WorkOperationChangeLog> workOperationAllChangeLogList;
        private ObservableCollection<WorkOperation> clonedWorkOperation;
        #endregion
        #region GEOS2-3880 change log gulab lakade
        private string tempworkstagename;
        //private WorkOperation selectedParentoldTemp;
        #endregion
        //GEOS2-3954 - time changes HH:mm:ss
        string decimalSeperator;
        #endregion

        #region Properties
        #region GEOS2-3954 - time changes HH:mm:ss
        public string DecimalSeperator
        {
            get
            {
                return decimalSeperator;
            }
            set
            {
                decimalSeperator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DecimalSeperator"));
            }
        }
        #endregion
        public bool IsColumnChooserVisibleForGrid
        {
            get
            {
                return isColumnChooserVisibleForGrid;
            }

            set
            {
                isColumnChooserVisibleForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColumnChooserVisibleForGrid"));
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
        public ObservableCollection<WorkOperation> WorkOperationsList
        {
            get { return workOperationsList; }
            set
            {
                workOperationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationsList"));
            }
        }
        public ObservableCollection<WorkOperation> WorkOperationsList_All
        {
            get { return workOperationsList_All; }
            set
            {
                workOperationsList_All = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationsList_All"));
            }
        }
        public WorkOperation SelectedWorkOperationsList
        {
            get { return selectedWorkOperationsList; }
            set
            {
                selectedWorkOperationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperationsList"));
            }

        }
        public WorkOperation SelectedWorkOperation
        {
            get { return selectedWorkOperations; }
            set
            {
                selectedWorkOperations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperation"));
            }

        }
        public WorkOperationByStages SelectedWorkOperationMenulist
        {
            get { return selectedWorkOperationMenulist; }
            set
            {
                selectedWorkOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperationMenulist"));
            }

        }
        public ObservableCollection<WorkOperationByStages> WorkOperationMenulist
        {
            get
            {
                return workOperationMenulist;
            }

            set
            {
                workOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationMenulist"));
            }
        }
        public ObservableCollection<WorkOperationByStages> ClonedWorkOperationByStages
        {
            get
            {
                return clonedWorkOperationByStages;
            }
            set
            {
                clonedWorkOperationByStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedWorkOperationByStages"));
            }
        }
        public ObservableCollection<Stages> GetStages
        {
            get
            {
                return getStages;
            }
            set
            {
                getStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GetStages"));
            }
        }

        //[001][rdixit][GEOS2-3710][09/06/2022]
        public List<LookupValue> StatusList
        {
            get
            {
                return statusList;
            }
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
        public ObservableCollection<WorkOperation> ParentList
        {
            get { return parentList; }
            set
            {
                parentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentList"));
            }
        }
        public List<WorkOperation> ValidateParentList
        {
            get { return validateParentList; }
            set
            {
                validateParentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValidateParentList"));
            }
        }
        public WorkOperation SelectedParent
        {
            get
            {
                return selectedParent;
            }

            set
            {
                selectedParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParent"));
            }
        }
        public List<LookupValue> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeList"));
            }
        }
        public UInt32? IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdType"));
            }
        }
        public LookupValue SelectedType
        {
            get
            {
                return selectedType;
            }

            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
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
        #region GEOS2-3880 change log gulab lakade
        //string Tempworkstagename = string.Empty;
        public string Tempworkstagename
        {
            get { return tempworkstagename; }
            set
            {
                tempworkstagename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tempworkstagename"));

            }
        }
        //public WorkOperation SelectedParentoldTemp
        //{
        //    get
        //    {
        //        return selectedParentoldTemp;
        //    }

        //    set
        //    {
        //        selectedParentoldTemp = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedParentoldTemp"));
        //    }
        //}
        #endregion


        #region GEOS2-3880 work operation gulab lakade

        public ObservableCollection<WorkOperationChangeLog> WorkOperationChangeLogList
        {
            get { return workOperationChangeLogList; }
            set
            {
                workOperationChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationChangeLogList"));
            }
        }
        public ObservableCollection<WorkOperationChangeLog> WorkOperationAllChangeLogList
        {
            get { return workOperationAllChangeLogList; }
            set
            {
                workOperationAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationAllChangeLogList"));
            }
        }
        public ObservableCollection<WorkOperation> ClonedWorkOperation
        {
            get { return clonedWorkOperation; }
            set
            {
                clonedWorkOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedWorkOperation"));
            }
        }
        #endregion
        #endregion

        #region Commands
        public ICommand RefreshWorksOperationCommand { get; set; }
        public ICommand PrintWorksOperationCommand { get; set; }
        public ICommand ExportWorksOperationCommand { get; set; }
        public ICommand DeleteWorkOperationsListCommand { get; set; }
        public ICommand AddWorksOperationCommand { get; set; }
        public ICommand EditWorkOperationCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand SelectStageCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand TreeListViewLoadedCommand { get; set; }
        public ICommand TableViewUnloadedCommand { get; set; }
        //[001][rdixit][GEOS2-3710][09/06/2022]
        public ICommand UpdateMultipleRowsWorkOperationGridCommand { get; set; }
        public ICommand ActivityValueChangedCommand { get; set; }

        #endregion

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
        public WorkOperationsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOperationsViewModel()...", category: Category.Info, priority: Priority.Low);
                RefreshWorksOperationCommand = new RelayCommand(new Action<object>(RefreshWorksOperationCommandAction));
                PrintWorksOperationCommand = new RelayCommand(new Action<object>(PrintWorksOperationCommandAction));
                ExportWorksOperationCommand = new RelayCommand(new Action<object>(ExportWorksOperationCommandAction));
                DeleteWorkOperationsListCommand = new RelayCommand(new Action<object>(DeleteWorksOperationCommandAction));
                AddWorksOperationCommand = new DelegateCommand<object>(AddWorksOperationCommandAction);
                UpdateMultipleRowsWorkOperationGridCommand = new DelegateCommand<object>(UpdateMultipleRowsWorkOperationGridCommandAction);
                EditWorkOperationCommand = new RelayCommand(new Action<object>(EditWorkOperationCommandAction));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
                SelectStageCommand = new DelegateCommand<object>(RetrieveWorkOperationsByStages);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                TreeListViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TreeListViewLoadedCommandAction);
                TableViewUnloadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewUnloadedCommandAction);
                ActivityValueChangedCommand = new DelegateCommand<object>(ActivityValueChangedCommandAction);
                #region GEOS2-3880
                WorkOperationChangeLogList = new ObservableCollection<WorkOperationChangeLog>();
                #endregion
                #region GEOS2-3954
                var currentculter = CultureInfo.CurrentCulture;
                DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                #endregion
                GeosApplication.Instance.Logger.Log("Constructor Constructor WorkOperationsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor WorkOperationsViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                FillStatusList();
                FillWorkOperations();
                FillWorkOperationsStages();
                FillparentList(WorkOperationsList);
                FillTypeList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception EX)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", EX.Message), category: Category.Exception, priority: Priority.Low);

            }

            // SetMinMaxDatesAndFillCurrencyConversions();
        }
        private void ActivityValueChangedCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ActivityValueChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void TreeListViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TreeListViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                treeListControlInstance = (TreeListControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;
                //    TreeListView view = (TreeListView)treeListControlInstance.View;

                GeosApplication.Instance.Logger.Log("Method TreeListViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TreeListViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_WorkOperationsGrid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_WorkOperationsGrid_SettingFilePath);
                    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlInstance.View;
                    this.tableViewInstance = tableView;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);

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
                        visibleFalseColumnsCount++;
                    }
                }

                if (visibleFalseColumnsCount > 0)
                {
                    IsColumnChooserVisibleForGrid = true;
                }
                else
                {
                    IsColumnChooserVisibleForGrid = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem() {
                    SummaryType = SummaryItemType.Sum,
                     DisplayFormat=@" {0:hh\:mm\:ss}",
                    FieldName = "UITempobservedTime"
                },
                new GridSummaryItem() {
                    SummaryType = SummaryItemType.Sum,
                    DisplayFormat=@" {0:hh\:mm\:ss}",
                    FieldName = "UITempNormalTime",

                },
                #region GEOS2-4138
                    new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                #endregion
                });

                //TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                // datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsColumnChooserVisibleForGrid = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {

            }
        }
        private void TableViewUnloadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                GridControl gridControlInstance = ((WorkOperationsView)obj.OriginalSource).wogrid; // ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                TableView tableView = (TableView)gridControlInstance.View;

                tableView.SearchString = string.Empty;
                if (gridControlInstance.GroupCount > 0)
                    gridControlInstance.ClearGrouping();
                gridControlInstance.ClearSorting();
                gridControlInstance.FilterString = null;
                gridControlInstance.SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);

                //int visibleFalseColumnsCount = 0;

                //if (File.Exists(ERM_WorkOperationsGrid_SettingFilePath))
                //{
                //    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_WorkOperationsGrid_SettingFilePath);
                //    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //    TableView tableView = (TableView)gridControlInstance.View;

                //    if (tableView.SearchString != null)
                //    {
                //        tableView.SearchString = null;
                //    }
                //}

                //// ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                ////This code for save grid layout.
                //((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_WorkOperationsGrid_SettingFilePath);

                //GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                //foreach (GridColumn column in gridControl.Columns)
                //{
                //    //DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                //    //if (descriptor != null)
                //    //{
                //    //    descriptor.AddValueChanged(column, VisibleChanged);
                //    //}

                //    //DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                //    //if (descriptorColumnPosition != null)
                //    //{
                //    //    descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                //    //}

                //    if (column.Visible == false)
                //    {
                //        visibleFalseColumnsCount++;
                //    }
                //}

                //if (visibleFalseColumnsCount > 0)
                //{
                //    IsColumnChooserVisibleForGrid = true;
                //}
                //else
                //{
                //    IsColumnChooserVisibleForGrid = false;
                //}

                ////TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                //// datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method TableViewUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //[001][kshinde][08/06/2022][GEOS2-3711] Added new service method
        public void FillWorkOperations()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOperations()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //[001][kshinde][08/06/2022][GEOS2-3711]
                //WorkOperationsList = new ObservableCollection<WorkOperation>(ERMService.GetAllWorkOperations_V2240());
                //[GEOS2-3933][Rupali Sarode]
                //WorkOperationsList = new ObservableCollection<WorkOperation>(ERMService.GetAllWorkOperations_V2280());
                //WorkOperationsList = new ObservableCollection<WorkOperation>(ERMService.GetAllWorkOperations_V2320());
                WorkOperationsList = new ObservableCollection<WorkOperation>(ERMService.GetAllWorkOperations_V2330());
                // start GEOS2 - 3880 add log
                ClonedWorkOperation = new ObservableCollection<WorkOperation>(WorkOperationsList.Select(i => (WorkOperation)i.Clone()).ToList());
                //end GEOS2-3880 add log
                #region GEOS2-3954 Time format HH:MM:SS(Increase performance)
                try
                {
                    Dictionary<string, byte[]> tempdict = new Dictionary<string, byte[]>();
                    WorkOperationsList.ToList().ForEach(i => i.WOStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == i.IdStatus).Value);
                    WorkOperationsList.ToList().ForEach(i => i.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == i.IdStatus).HtmlColor);
                    WorkOperationsList.ToList().ForEach(i => i.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == i.IdStatus));

                    //WorkOperationsList.ToList().Where(j => j.Type != null && !string.IsNullOrEmpty(j.Type.ImageName)).ToList().ForEach(i => i.Type.ImageData = geosService.GetLookupImages(i.Type.ImageName));
                    WorkOperationsList.ToList().ForEach(i => i.StatusList = StatusList.ToList());
                    foreach (var item in WorkOperationsList)
                    {
                        if (item.Type != null && !string.IsNullOrEmpty(item.Type.ImageName))
                        {
                            if (tempdict.Keys.Contains(item.Type.ImageName))
                            {
                                item.Type.ImageData = tempdict.FirstOrDefault(i => i.Key == item.Type.ImageName).Value;
                            }
                            else
                            {
                                item.Type.ImageData = geosService.GetLookupImages(item.Type.ImageName); //"1-1.png"); //item.LookupValueImages
                                tempdict.Add(item.Type.ImageName, item.Type.ImageData);
                            }

                        }

                        string onlyCharacters = item.Code.Substring(0, 3);
                    }
                }
                catch (Exception ex)
                { }
                #endregion
                //foreach (var item in WorkOperationsList)
                //{
                //    item.WOStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                //    item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                //    item.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus);
                //    if (item.Type != null && !string.IsNullOrEmpty(item.Type.ImageName))
                //    {
                //item.Type.ImageData = geosService.GetLookupImages(item.Type.ImageName); //"1-1.png"); //item.LookupValueImages
                //    }
                //    item.StatusList = StatusList.ToList();
                //    string onlyCharacters = item.Code.Substring(0, 3);
                //    //if (item.IdParent != null)
                //    //{
                //    //    ParentList = new ObservableCollection<WorkOperation>(ERMService.GetParentListByIdParentAndCode_V2240((int)item.IdParent, onlyCharacters));
                //    //    //UpdateCountForParent();
                //    //    ParentList = new ObservableCollection<WorkOperation>(parentList.OrderBy(x => x.Position));
                //    //    item.ParentList = ParentList.ToList();
                //    //} 

                //    //if (item.ObservedTime > 0)
                //    //{
                //    //    item.NormalTime = (float)Math.Round(item.ObservedTime.Value * ((float)item.Activity / 100), 2);
                //    //    #region GEOS2-3954 Time format HH:MM:SS
                //    //    string tempd = Convert.ToString(item.ObservedTime);
                //    //    string[] parts = new string[2];
                //    //    int i1 = 0;
                //    //    int i2 = 0;
                //    //    if (tempd.Contains(DecimalSeperator))
                //    //    {
                //    //        parts = tempd.Split(Convert.ToChar(DecimalSeperator));
                //    //        i1 = int.Parse(parts[0]);
                //    //        i2 = int.Parse(parts[1]);
                //    //        i1 = (i1 * 60) + i2;
                //    //    }
                //    //    else
                //    //    {
                //    //        parts = tempd.Split(Convert.ToChar(DecimalSeperator));
                //    //        i1 = int.Parse(parts[0]);
                //    //        i1 = (i1 * 60);
                //    //    }
                //    //    item.UITempobservedTime = TimeSpan.FromSeconds(i1);

                //    //    string temnormaltime = Convert.ToString(item.NormalTime);
                //    //    string[] NormaltimeArr = new string[2];
                //    //    int nt1 = 0;
                //    //    int nt2 = 0;
                //    //    if (temnormaltime.Contains(DecimalSeperator))
                //    //    {
                //    //        NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    //        nt1 = int.Parse(NormaltimeArr[0]);
                //    //        nt2 = int.Parse(NormaltimeArr[1]);
                //    //        nt1 = (nt1 * 60) + nt2;
                //    //    }
                //    //    else
                //    //    {
                //    //        NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    //        nt1 = int.Parse(NormaltimeArr[0]);
                //    //        nt1 = (nt1 * 60);
                //    //    }

                //    //    item.UITempNormalTime = TimeSpan.FromSeconds(nt1);
                //    //    #endregion

                //    //}
                //}

                SelectedWorkOperation = new WorkOperation();
                SelectedWorkOperation = WorkOperationsList.FirstOrDefault();

                WorkOperationsList_All = new ObservableCollection<WorkOperation>(WorkOperationsList);
                //[rdixit][22/06/2022][GEOS2-3795]
                //foreach (WorkOperation WO in WorkOperationsList)
                //{
                //    if (WO.ObservedTime > 0)
                //    {
                //        WO.NormalTime = (float)Math.Round(WO.ObservedTime.Value * ((float)WO.Activity / 100), 2);
                //    }
                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillWorkOperations()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperations() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperations() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWorkOperations()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        public void FillWorkOperationsStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOperationsStages()...", category: Category.Info, priority: Priority.Low);
                ObservableCollection<WorkOperationByStages> OperationList = new ObservableCollection<WorkOperationByStages>();
                ObservableCollection<WorkOperationByStages> tempWorkOperationMenulist = new ObservableCollection<WorkOperationByStages>();
                if (GetStages == null)
                {
                    GetStages = new ObservableCollection<Stages>(ERMService.GetStages());
                }
                else
                {
                    GetStages.Clear();
                    GetStages.AddRange(ERMService.GetStages());
                }

                OperationList.Clear();
                //foreach (Stages item in GetStages)
                //{
                //    OperationList.AddRange(ERMService.GetAllWorkOperationStages_V2240(item.IdStage));
                //}

                //UpdateWorkOperationCount(OperationList);
                if (WorkOperationMenulist == null)
                {
                    WorkOperationMenulist = new ObservableCollection<WorkOperationByStages>();
                }
                else
                {
                    WorkOperationMenulist.Clear();
                }

                WorkOperationByStages workOperationByStages = new WorkOperationByStages();
                //workOperationByStages.Name = "All";
                //workOperationByStages.KeyName = "Group_All";
                //workOperationByStages.WorkOperation_count = OperationList.GroupBy(i=>i.IdworkOperationByStage).Count();
                //workOperationByStages.NameWithWorkOperationCount = "All [" + workOperationByStages.WorkOperation_count + "]";
                //tempWorkOperationMenulist.Insert(0, workOperationByStages);
                foreach (Stages item in GetStages)
                {
                    OperationList.AddRange(ERMService.GetAllWorkOperationStages_V2240(item.IdStage));
                    WorkOperationByStages workOperation = new WorkOperationByStages();
                    workOperation.IdworkOperationByStage = item.IdStage;
                    workOperation.Name = item.Code;
                    workOperation.KeyName = "Stage_" + item.IdStage;
                    workOperation.Parent = null;
                    workOperation.IdParent = null;
                    workOperation.Position = Convert.ToUInt32(item.IdSequence);
                    workOperation.WorkOperation_count = OperationList.Where(x => x.IdStage == item.IdStage).GroupBy(i => i.IdworkOperationByStage).Count();
                    workOperation.NameWithWorkOperationCount = item.Code + " [" + workOperation.WorkOperation_count + "]";
                    tempWorkOperationMenulist.Add(workOperation);
                    tempWorkOperationMenulist.AddRange(OperationList.Where(x => x.IdStage == item.IdStage));
                }
                UpdateWorkOperationCount(OperationList);

                workOperationByStages.Name = "All";
                workOperationByStages.KeyName = "Group_All";
                workOperationByStages.WorkOperation_count = OperationList.GroupBy(i => i.IdworkOperationByStage).Count();
                workOperationByStages.NameWithWorkOperationCount = "All [" + workOperationByStages.WorkOperation_count + "]";
                tempWorkOperationMenulist.Insert(0, workOperationByStages);
                var tempWorkOperationMenulist2 = tempWorkOperationMenulist.OrderBy(x => x.Position).ToList();
                //WorkOperationMenulist.Clear();
                if (treeListControlInstance != null) treeListControlInstance.BeginDataUpdate();
                WorkOperationMenulist.AddRange(tempWorkOperationMenulist2);
                if (treeListControlInstance != null) treeListControlInstance.EndDataUpdate();

                //WorkOperationMenulist = new ObservableCollection<WorkOperationByStages>(WorkOperationMenulist.OrderBy(x => x.Position));
                SelectedWorkOperationMenulist = WorkOperationMenulist.FirstOrDefault();
                if (treeListControlInstance != null) treeListControlInstance.View.ExpandAllNodes();

                //WorkOperationsList = new ObservableCollection<WorkOperation>(WorkOperationsList_All);

                //SelectedWorkOperation = WorkOperationsList.FirstOrDefault();
                // ClonedWorkOperationByStages = (ObservableCollection<WorkOperationByStages>)WorkOperationMenulist;

                GeosApplication.Instance.Logger.Log("Method FillWorkOperationsStages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperationsStages() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperationsStages() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWorkOperationsStages()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][01-04-2022][GEOS2-3658]
        private void UpdateWorkOperationCount(ObservableCollection<WorkOperationByStages> OperationList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateWorkOperationCount()...", category: Category.Info, priority: Priority.Low);

                foreach (WorkOperationByStages item in OperationList)
            {
                int count = 0;
                //[001]
                //if (item.WorkOperation_count_original != null)
                //{
                //    count = item.WorkOperation_count_original;
                //}
                if (OperationList.Any(a => a.ParentName == item.KeyName))
                {
                    List<WorkOperationByStages> getFirstList = OperationList.Where(a => a.ParentName == item.KeyName).ToList();
                    foreach (WorkOperationByStages item1 in getFirstList)
                    {
                        if (item1.WorkOperation_count_original != null)
                        {
                            count = count + item1.WorkOperation_count_original;
                        }
                        if (OperationList.Any(a => a.ParentName == item1.KeyName))
                        {
                            List<WorkOperationByStages> getSecondList = OperationList.Where(a => a.ParentName == item1.KeyName).ToList();
                            foreach (WorkOperationByStages item2 in getSecondList)
                            {
                                if (item2.WorkOperation_count_original != null)
                                {
                                    count = count + item2.WorkOperation_count_original;
                                }
                                if (OperationList.Any(a => a.ParentName == item2.KeyName))
                                {
                                    List<WorkOperationByStages> getThirdList = OperationList.Where(a => a.ParentName == item2.KeyName).ToList();
                                    foreach (WorkOperationByStages item3 in getThirdList)
                                    {
                                        if (item3.WorkOperation_count_original != null)
                                        {
                                            count = count + item3.WorkOperation_count_original;
                                        }
                                        if (OperationList.Any(a => a.ParentName == item3.KeyName))
                                        {
                                            List<WorkOperationByStages> getForthList = OperationList.Where(a => a.ParentName == item3.KeyName).ToList();
                                            foreach (WorkOperationByStages item4 in getForthList)
                                            {
                                                if (item4.WorkOperation_count_original != null)
                                                {
                                                    count = count + item4.WorkOperation_count_original;
                                                }
                                                if (OperationList.Any(a => a.ParentName == item4.KeyName))
                                                {
                                                    List<WorkOperationByStages> getFifthList = OperationList.Where(a => a.ParentName == item4.KeyName).ToList();
                                                    foreach (WorkOperationByStages item5 in getFifthList)
                                                    {
                                                        if (item5.WorkOperation_count_original != null)
                                                        {
                                                            count = count + item5.WorkOperation_count_original;
                                                        }
                                                        if (OperationList.Any(a => a.ParentName == item5.KeyName))
                                                        {
                                                            List<WorkOperationByStages> getSixthList = OperationList.Where(a => a.ParentName == item5.KeyName).ToList();
                                                            foreach (WorkOperationByStages item6 in getSixthList)
                                                            {
                                                                if (item6.WorkOperation_count_original != null)
                                                                {
                                                                    count = count + item6.WorkOperation_count_original;
                                                                }
                                                                if (OperationList.Any(a => a.ParentName == item6.KeyName))
                                                                {
                                                                    List<WorkOperationByStages> getSeventhList = OperationList.Where(a => a.ParentName == item6.KeyName).ToList();
                                                                    foreach (WorkOperationByStages item7 in getSeventhList)
                                                                    {
                                                                        if (item7.WorkOperation_count_original != null)
                                                                        {
                                                                            count = count + item7.WorkOperation_count_original;
                                                                        }
                                                                        if (OperationList.Any(a => a.ParentName == item7.KeyName))
                                                                        {
                                                                            List<WorkOperationByStages> getEightthList = OperationList.Where(a => a.ParentName == item7.KeyName).ToList();
                                                                            foreach (WorkOperationByStages item8 in getEightthList)
                                                                            {
                                                                                if (item8.WorkOperation_count_original != null)
                                                                                {
                                                                                    count = count + item8.WorkOperation_count_original;
                                                                                }
                                                                                if (OperationList.Any(a => a.ParentName == item8.KeyName))
                                                                                {
                                                                                    List<WorkOperationByStages> getNinethList = OperationList.Where(a => a.ParentName == item8.KeyName).ToList();
                                                                                    foreach (WorkOperationByStages item9 in getNinethList)
                                                                                    {
                                                                                        if (item9.WorkOperation_count_original != null)
                                                                                        {
                                                                                            count = count + item9.WorkOperation_count_original;
                                                                                        }
                                                                                        if (OperationList.Any(a => a.ParentName == item9.KeyName))
                                                                                        {
                                                                                            List<WorkOperationByStages> gettenthList = OperationList.Where(a => a.ParentName == item9.KeyName).ToList();
                                                                                            foreach (WorkOperationByStages item10 in gettenthList)
                                                                                            {
                                                                                                if (item10.WorkOperation_count_original != null)
                                                                                                {
                                                                                                    count = count + item10.WorkOperation_count_original;
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
                item.WorkOperation_count = count;
                item.NameWithWorkOperationCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.WorkOperation_count) + "]");
            }
                GeosApplication.Instance.Logger.Log("Method UpdateWorkOperationCount()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method UpdateWorkOperationCount() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }

        }
        private void RefreshWorksOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorksOperationCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView;


                view = ERMWorkOperationViewMultipleCellEditHelper.Viewtableview;
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["WorkOperationGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsWorkOperationGridCommandAction(ERMWorkOperationViewMultipleCellEditHelper.Viewtableview);
                    }
                    ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged = false;
                }

                ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ERMWorkOperationViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillWorkOperations();

                if (obj != null && obj is TableView)
                {
                    detailView = (TableView)obj;
                }
                else
                {
                    detailView = tableViewInstance;
                }

                if (detailView != null)
                {
                    //  TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    detailView.SearchString = null;
                }
                FillparentList(WorkOperationsList);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorksOperationCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorksOperationCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintWorksOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorksOperationCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkOperationsListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkOperationsListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportWorksOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorksOperationCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Operations";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

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

                    GeosApplication.Instance.Logger.Log("Method ExportWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() != "Parent" && e.ColumnFieldName == "IdParent")
            {
                var tempValue = e.Value;
                e.Value = WorkOperationsList.Where(w => w != null && w.IdWorkOperation == Convert.ToInt32(e.Value)).Select(s => s.Name).DefaultIfEmpty("").FirstOrDefault();
                if (e.Value == string.Empty)
                {
                    e.Value = WorkOperationsList.Where(w => w != null && w.IdParent == Convert.ToUInt64(tempValue)).Select(s => s.Parent).DefaultIfEmpty("").FirstOrDefault();
                }
                //e.Value = AllParentList.FirstOrDefault(a =>a!=null && a.IdWorkOperation == Convert.ToInt32(e.Value)).Name;
            }

            if (e.Value != null && e.Value.ToString() != "Status" && e.ColumnFieldName == "IdStatus")
            {
                e.Value = StatusList.FirstOrDefault(a => a.IdLookupValue == Convert.ToInt32(e.Value)).Value;
            }
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        private void DeleteWorksOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteWorksOperationCommandAction()...", category: Category.Info, priority: Priority.Low);
                //Service Method changed from ERM_IsWOExistInSOD to ERM_IsWOExistInSOD_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                IsWOExistInSOD = ERMService.ERM_IsWOExistInSOD_V2350(SelectedWorkOperationsList.IdWorkOperation);//[rdixit][04.08.2022][GEOS2-3764]
                ValidateParentList = new List<WorkOperation>();
                ValidateParentList.AddRange(WorkOperationsList.Select(x => x.ParentList).FirstOrDefault());
                if (SelectedWorkOperationsList.IdStatus == 1535)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationForActive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                else if (SelectedWorkOperationsList.IdStatus == 1536)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationForInactive").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                else if (IsWOExistInSOD)
                {
                    if (ValidateParentList.Any(i => i.Name == SelectedWorkOperationsList.Name))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationPresentinMultipleSOD").ToString() + "\n" + System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationForHavingSons").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationPresentinMultipleSOD").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                else if (ValidateParentList.Any(i => i.Name == SelectedWorkOperationsList.Name))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RemoveWorkOperationGridValidationForHavingSons").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["WorkOperationsDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = ERMService.IsDeletedWorkOperationList(SelectedWorkOperationsList.IdWorkOperation); //(SelectedBasePrice.IdBasePriceList, (uint)GeosApplication.Instance.ActiveUser.IdUser);

                        if (IsDeleted)
                        {
                            WorkOperationsList.Remove(SelectedWorkOperationsList);
                            WorkOperationsList = new ObservableCollection<WorkOperation>(WorkOperationsList);
                            SelectedWorkOperationsList = WorkOperationsList.FirstOrDefault();

                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            TableView detailView = (TableView)obj;
                            RefreshWorksOperationCommandAction(detailView);
                            FillWorkOperationsStages();

                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteWorksOperationCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteWorksOperationCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddWorksOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddWorksOperationCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                AddEditWorkOperationView addEditWorkOperationView = new AddEditWorkOperationView();
                AddEditWorkOperationViewModel addEditWorkOperationViewModel = new AddEditWorkOperationViewModel();
                EventHandler handle = delegate { addEditWorkOperationView.Close(); };
                addEditWorkOperationViewModel.RequestClose += handle;
                addEditWorkOperationViewModel.WindowHeader = Application.Current.FindResource("AddWorkOperationHeader").ToString();
                addEditWorkOperationViewModel.IsNew = true;
                addEditWorkOperationViewModel.Init(SelectedWorkOperationMenulist);
                addEditWorkOperationView.DataContext = addEditWorkOperationViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditWorkOperationView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditWorkOperationView.ShowDialog();

                if (addEditWorkOperationViewModel.IsSave == true)
                {
                    //for maid grid changes
                    addEditWorkOperationViewModel.NewWorkOperation.LastUpdated = GeosApplication.Instance.ServerDateTime;

                    //addEditWorkOperationViewModel.NewWorkOperation.IdWorkOperation = 1;
                    if (addEditWorkOperationViewModel.NewWorkOperation.Parent == "---")
                    {
                        addEditWorkOperationViewModel.NewWorkOperation.Parent = string.Empty;
                    }


                    List<Stages> st = new List<Stages>();
                    string str = string.Empty;
                    if (addEditWorkOperationViewModel.SelectedStages != null)
                    {
                        st = addEditWorkOperationViewModel.SelectedStages.Cast<Stages>().ToList();
                        st.RemoveAll(d => d.IdStage == 0);
                        str = String.Join("\n", st.Select(p => p.CodeWithName));
                    }

                    addEditWorkOperationViewModel.NewWorkOperation.WorkStage = str;

                    if (addEditWorkOperationViewModel.NewWorkOperation.Type.Value == "---") addEditWorkOperationViewModel.NewWorkOperation.Type.Value = "";
                    #region GEOS2-4046 Some issue in WO & SOD
                    // shubham[skadam]GEOS2-4046 Some issue in WO & SOD 24 11 2022
                    try
                    {
                        SelectedWorkOperation.ObservedTime = addEditWorkOperationViewModel.ObservedTime;
                        addEditWorkOperationViewModel.UITempobservedTime = addEditWorkOperationViewModel.UITempobservedTime;
                        addEditWorkOperationViewModel.NewWorkOperation.UITempobservedTime = addEditWorkOperationViewModel.UITempobservedTime;
                        addEditWorkOperationViewModel.NormalTime = addEditWorkOperationViewModel.NormalTime;
                        #region GEOS2-3954 time change HH:mm:ss
                        addEditWorkOperationViewModel.UITempNormalTime = addEditWorkOperationViewModel.UITempNormalTime;
                        addEditWorkOperationViewModel.NewWorkOperation.UITempNormalTime = addEditWorkOperationViewModel.UITempNormalTime;
                        #endregion
                    }
                    catch (Exception ex) { }
                    #endregion
                    #region GEOS2-3752 gulab lakade
                    addEditWorkOperationViewModel.NewWorkOperation.WOStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == addEditWorkOperationViewModel.NewWorkOperation.IdStatus).Value;
                    addEditWorkOperationViewModel.NewWorkOperation.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == addEditWorkOperationViewModel.NewWorkOperation.IdStatus).HtmlColor;
                    addEditWorkOperationViewModel.NewWorkOperation.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == addEditWorkOperationViewModel.NewWorkOperation.IdStatus);
                    #endregion

                    #region [GEOS2-5484][Rupali Sarode][20-03-2024]
                    ObservableCollection<WorkOperation> NewRowWorkOperationList = new ObservableCollection<WorkOperation>();

                    NewRowWorkOperationList.Add(addEditWorkOperationViewModel.NewWorkOperation);
                    FillparentList(NewRowWorkOperationList);

                    #endregion [GEOS2-5484][Rupali Sarode][20-03-2024]

                    WorkOperationsList_All.Add(addEditWorkOperationViewModel.NewWorkOperation);
                    WorkOperationsList.Add(addEditWorkOperationViewModel.NewWorkOperation);

                    // RefreshWorksOperationCommandAction(null);  //[gulablakade][GEOS2-3752]
                    FillWorkOperationsStages();
                    
                    

                    SelectedWorkOperationsList = WorkOperationsList.FirstOrDefault(x => x.IdWorkOperation == addEditWorkOperationViewModel.NewWorkOperation.IdWorkOperation);
                    // start GEOS2-3880 add log
                    ClonedWorkOperation = new ObservableCollection<WorkOperation>(WorkOperationsList.Select(i => (WorkOperation)i.Clone()).ToList());
                    //end GEOS2-3880 add log
                }


                GeosApplication.Instance.Logger.Log("Method AddWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditWorkOperationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWorkOperationCommandAction()...", category: Category.Info, priority: Priority.Low);

                //var beforeUpdateSelectedWorkOperationMenulist = SelectedWorkOperationMenulist;
                //var beforeUpdateSelectedWorkOperationsList = SelectedWorkOperationsList;
                var beforeUpdateSelectedWorkOperationMenulist = SelectedWorkOperationMenulist;
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                WorkOperation SelectedRow = (WorkOperation)detailView.DataControl.CurrentItem;
                SelectedWorkOperation = SelectedRow;


                AddEditWorkOperationView addEditWorkOperationView = new AddEditWorkOperationView();
                AddEditWorkOperationViewModel addEditWorkOperationViewModel = new AddEditWorkOperationViewModel();

                EventHandler handle = delegate { addEditWorkOperationView.Close(); };
                addEditWorkOperationViewModel.RequestClose += handle;
                addEditWorkOperationViewModel.WindowHeader = Application.Current.FindResource("EditWorkOperationHeader").ToString();
                addEditWorkOperationViewModel.IsNew = false;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                addEditWorkOperationViewModel.EditInit(SelectedRow);
                //addEditWorkOperationViewModel.EditCommand(SelectedArticle);
                addEditWorkOperationView.DataContext = addEditWorkOperationViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditWorkOperationView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditWorkOperationView.ShowDialog();
                WorkOperationByStages selected_Wo = SelectedWorkOperationMenulist;
                if (addEditWorkOperationViewModel.IsSave == true)
                {
                    // TreeView
                    //var selectedWOFromTreeView = WorkOperationMenulist.FirstOrDefault(x => x.IdworkOperationByStage == SelectedWorkOperation.IdWorkOperation);
                    //if(selectedWOFromTreeView!=null)
                    //{
                    //    selectedWOFromTreeView.Name = addEditWorkOperationViewModel.Name_en;

                    //    if (!addEditWorkOperationViewModel.SelectedParent.Name.Equals("---"))
                    //    {
                    //        selectedWOFromTreeView.Parent = addEditWorkOperationViewModel.SelectedParent.Name;
                    //        selectedWOFromTreeView.ParentName = addEditWorkOperationViewModel.SelectedParent.KeyName;
                    //    }
                    //    else
                    //    {
                    //        selectedWOFromTreeView.Parent = string.Empty;
                    //        selectedWOFromTreeView.ParentName = string.Empty;
                    //    }
                    //}

                    //for maid grid changes
                    SelectedWorkOperation.Description = addEditWorkOperationViewModel.Description_en;
                    SelectedWorkOperation.Name = addEditWorkOperationViewModel.Name_en;
                    if (!addEditWorkOperationViewModel.SelectedParent.Name.Equals("---"))
                    {
                        SelectedWorkOperation.Parent = addEditWorkOperationViewModel.SelectedParent.Name;
                    }
                    else
                    {
                        SelectedWorkOperation.Parent = string.Empty;
                    }
                    List<Stages> st = new List<Stages>();
                    string str = string.Empty;
                    if (addEditWorkOperationViewModel.SelectedStages != null)
                    {
                        st = addEditWorkOperationViewModel.SelectedStages.Cast<Stages>().ToList();
                        st.RemoveAll(d => d.IdStage == 0);
                        str = String.Join("\n", st.Select(p => p.CodeWithName));
                    }
                    SelectedWorkOperation.WorkStage = str;
                    SelectedWorkOperation.Type = addEditWorkOperationViewModel.SelectedType;
                    SelectedWorkOperation.Status = addEditWorkOperationViewModel.SelectedStatus;
                    SelectedWorkOperation.IdType = (uint)SelectedWorkOperation.Type.IdLookupValue;
                    SelectedWorkOperation.IdStatus = (uint)SelectedWorkOperation.Status.IdLookupValue;
                    #region GEOS2-3752 gulab lakade
                    SelectedWorkOperation.WOStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == SelectedWorkOperation.IdStatus).Value;
                    SelectedWorkOperation.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == SelectedWorkOperation.IdStatus).HtmlColor;
                    SelectedWorkOperation.Status = StatusList.FirstOrDefault(a => a.IdLookupValue == SelectedWorkOperation.IdStatus);
                    #endregion
                    if (SelectedWorkOperation.Type != null && SelectedWorkOperation.Type.Value == "---") SelectedWorkOperation.Type.Value = "";
                    SelectedWorkOperation.Code = addEditWorkOperationViewModel.Code;
                    SelectedWorkOperation.Distance = addEditWorkOperationViewModel.Distance;//[001][kshinde][11/06/2022][GEOS2-3709]
                    SelectedWorkOperation.ObservedTime = addEditWorkOperationViewModel.ObservedTime;
                    #region GEOS2-3954 time change HH:mm:ss
                    SelectedWorkOperation.UITempobservedTime = addEditWorkOperationViewModel.UITempobservedTime;
                    #endregion
                    SelectedWorkOperation.Activity = addEditWorkOperationViewModel.Activity;
                    SelectedWorkOperation.Remarks = addEditWorkOperationViewModel.Remarks.Trim(); //[GEOS2-3933][Rupali Sarode]

                    SelectedWorkOperation.NormalTime = addEditWorkOperationViewModel.NormalTime;
                    #region GEOS2-3954 time change HH:mm:ss
                    SelectedWorkOperation.UITempNormalTime = addEditWorkOperationViewModel.UITempNormalTime;
                    #endregion
                    SelectedWorkOperation.DetectedProblems = addEditWorkOperationViewModel.DetectedProblems;
                    SelectedWorkOperation.ImprovementsProposals = addEditWorkOperationViewModel.ImprovementsProposals;
                    detailView.DataControl.CurrentItem = SelectedWorkOperation;
                    detailView.ImmediateUpdateRowPosition = true;
                    detailView.EnableImmediatePosting = true;

                    //RefreshWorksOperationCommandAction(detailView);   //[gulablakade][GEOS2-3752]
                    FillWorkOperationsStages();
                    SelectedWorkOperationsList = WorkOperationsList.FirstOrDefault(x => x.IdWorkOperation == SelectedRow.IdWorkOperation);
                    //SelectedWorkOperation.IdStage = addEditWorkOperationViewModel.IdStage;           
                    //var beforeUpdateSelectedWorkOperationMenulist = SelectedWorkOperationMenulist; WorkOperationMenulist
                    //var beforeUpdateSelectedWorkOperationsList = SelectedWorkOperationsList; WorkOperationsList
                    // start GEOS2-3880 add log
                    ClonedWorkOperation = new ObservableCollection<WorkOperation>(WorkOperationsList.Select(i => (WorkOperation)i.Clone()).ToList());
                    //end GEOS2-3880 add log

                }
                GeosApplication.Instance.Logger.Log("Method EditWorkOperationCommandAction()...", category: Category.Info, priority: Priority.Low);
                #region GEOS2-4046 Some issue in WO & SOD
                // shubham[skadam]GEOS2-4046 Some issue in WO & SOD 24 11 2022
                try
                {
                    SelectedWorkOperationMenulist = WorkOperationMenulist.FirstOrDefault(x => x.KeyName == beforeUpdateSelectedWorkOperationMenulist.KeyName);
                    RetrieveWorkOperationsByStages(null);
                }
                catch (Exception ex) { }
                #endregion
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditWorkOperationCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);
            //[rdixit][GEOS2-3710][22/06/2022]
            #region Status
            if (e.Column.FieldName == "IdStatus")
            {
                List<object> filterItem = new List<object>();

                foreach (WorkOperation item in WorkOperationsList)
                {
                    //item.WOStatus = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).Value;
                    //item.StatusHTMLColor = StatusList.FirstOrDefault(a => a.IdLookupValue == item.IdStatus).HtmlColor;
                    //item.StatusList = StatusList.ToList();

                    string StatusValue = item.WOStatus;

                    if (StatusValue == null)
                    {
                        continue;
                    }

                    if (!filterItem.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == StatusValue))
                    {
                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                        customComboBoxItem.DisplayValue = StatusValue;
                        customComboBoxItem.EditValue = item.IdStatus;
                        filterItem.Add(customComboBoxItem);
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItem.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
            }
            #endregion
            //[rdixit][GEOS2-3710][22/06/2022]
            #region ParentList
            else if (e.Column.FieldName == "IdParent")
            {
                List<object> ParentfilterItem = new List<object>();

                foreach (WorkOperation item in WorkOperationsList)
                {
                    //string onlyCharacters = item.Code.Substring(0, 3);
                    //ParentList = new ObservableCollection<WorkOperation>(ERMService.GetParentListByIdParentAndCode_V2240((int)item.IdParent, onlyCharacters));
                    //item.Parent = ParentList.FirstOrDefault(a => a.IdParent == item.IdParent).Name;                    
                    //item.ParentList = ParentList.ToList();

                    string ParentValue = item.Parent;

                    if (ParentValue == null)
                    {
                        continue;
                    }

                    if (!ParentfilterItem.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == ParentValue))
                    {
                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                        customComboBoxItem.DisplayValue = ParentValue;
                        customComboBoxItem.EditValue = item.IdParent;
                        ParentfilterItem.Add(customComboBoxItem);
                    }
                }

                e.ComboBoxEdit.ItemsSource = ParentfilterItem.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
            }
            #endregion

            else
            {
                #region Workstage
                if (e.Column.FieldName != "WorkStage")
                {
                    return;
                }

                try
                {
                    List<object> filterItems = new List<object>();
                    if (e.Column.FieldName == "WorkStage")
                    {
                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Blanks)",
                            EditValue = CriteriaOperator.Parse("IsNull([WorkStage])")//[002] added
                        });

                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Non blanks)",
                            EditValue = CriteriaOperator.Parse("!IsNull([WorkStage])")
                        });

                        foreach (var dataObject in WorkOperationsList)
                        {
                            if (dataObject.WorkStage == null)
                            {
                                continue;
                            }
                            else if (dataObject.WorkStage != null)
                            {
                                if (dataObject.WorkStage.Contains("\n"))
                                {
                                    string tempPlants = dataObject.WorkStage;
                                    for (int index = 0; index < tempPlants.Length; index++)
                                    {
                                        string empPlants = tempPlants.Split('\n').First();

                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = empPlants;
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("WorkStage Like '%{0}%'", empPlants));
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
                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == WorkOperationsList.Where(y => y.WorkStage == dataObject.WorkStage).Select(slt => slt.WorkStage).FirstOrDefault().Trim()))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = WorkOperationsList.Where(y => y.WorkStage == dataObject.WorkStage).Select(slt => slt.WorkStage).FirstOrDefault().Trim();
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("WorkStage Like '%{0}%'", WorkOperationsList.Where(y => y.WorkStage == dataObject.WorkStage).Select(slt => slt.WorkStage).FirstOrDefault().Trim()));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }
                    }
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                    #endregion


                    GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }
        }
        private List<string> getIdsForRetriveArticlesByParentClick()
        {

            List<string> ids = new List<string>();
            if (WorkOperationMenulist.Any(a => a.ParentName == SelectedWorkOperationMenulist.KeyName))
            {
                List<WorkOperationByStages> getFirstList = WorkOperationMenulist.Where(a => a.ParentName == SelectedWorkOperationMenulist.KeyName).ToList();
                foreach (WorkOperationByStages item1 in getFirstList)
                {
                    if (item1.WorkOperation_count_original != null)
                    {
                        ids.Add(item1.IdworkOperationByStage.ToString());
                    }
                    if (WorkOperationMenulist.Any(a => a.ParentName == item1.KeyName))
                    {
                        List<WorkOperationByStages> getSecondList = WorkOperationMenulist.Where(a => a.ParentName == item1.KeyName).ToList();
                        foreach (WorkOperationByStages item2 in getSecondList)
                        {
                            if (item2.WorkOperation_count_original != null)
                            {
                                ids.Add(item2.IdworkOperationByStage.ToString());
                            }
                            if (WorkOperationMenulist.Any(a => a.ParentName == item2.KeyName))
                            {
                                List<WorkOperationByStages> getThirdList = WorkOperationMenulist.Where(a => a.ParentName == item2.KeyName).ToList();
                                foreach (WorkOperationByStages item3 in getThirdList)
                                {
                                    if (item3.WorkOperation_count_original != null)
                                    {
                                        ids.Add(item3.IdworkOperationByStage.ToString());
                                    }
                                    if (WorkOperationMenulist.Any(a => a.ParentName == item3.KeyName))
                                    {
                                        List<WorkOperationByStages> getForthList = WorkOperationMenulist.Where(a => a.ParentName == item3.KeyName).ToList();
                                        foreach (WorkOperationByStages item4 in getForthList)
                                        {
                                            if (item4.WorkOperation_count_original != null)
                                            {
                                                ids.Add(item4.IdworkOperationByStage.ToString());
                                            }
                                            if (WorkOperationMenulist.Any(a => a.ParentName == item4.KeyName))
                                            {
                                                List<WorkOperationByStages> getFifthList = WorkOperationMenulist.Where(a => a.ParentName == item4.KeyName).ToList();
                                                foreach (WorkOperationByStages item5 in getFifthList)
                                                {
                                                    if (item5.WorkOperation_count_original != null)
                                                    {
                                                        ids.Add(item5.IdworkOperationByStage.ToString());
                                                    }
                                                    if (WorkOperationMenulist.Any(a => a.ParentName == item5.KeyName))
                                                    {
                                                        List<WorkOperationByStages> getSixthList = WorkOperationMenulist.Where(a => a.ParentName == item5.KeyName).ToList();
                                                        foreach (WorkOperationByStages item6 in getSixthList)
                                                        {
                                                            if (item6.WorkOperation_count_original != null)
                                                            {
                                                                ids.Add(item6.IdworkOperationByStage.ToString());
                                                            }
                                                            if (WorkOperationMenulist.Any(a => a.ParentName == item6.KeyName))
                                                            {
                                                                List<WorkOperationByStages> getSeventhList = WorkOperationMenulist.Where(a => a.ParentName == item6.KeyName).ToList();
                                                                foreach (WorkOperationByStages item7 in getSeventhList)
                                                                {
                                                                    if (item7.WorkOperation_count_original != null)
                                                                    {
                                                                        ids.Add(item7.IdworkOperationByStage.ToString());
                                                                    }
                                                                    if (WorkOperationMenulist.Any(a => a.ParentName == item7.KeyName))
                                                                    {
                                                                        List<WorkOperationByStages> getEightthList = WorkOperationMenulist.Where(a => a.ParentName == item7.KeyName).ToList();
                                                                        foreach (WorkOperationByStages item8 in getEightthList)
                                                                        {
                                                                            if (item8.WorkOperation_count_original != null)
                                                                            {
                                                                                ids.Add(item8.IdworkOperationByStage.ToString());
                                                                            }
                                                                            if (WorkOperationMenulist.Any(a => a.ParentName == item8.KeyName))
                                                                            {
                                                                                List<WorkOperationByStages> getNinethList = WorkOperationMenulist.Where(a => a.ParentName == item8.KeyName).ToList();
                                                                                foreach (WorkOperationByStages item9 in getNinethList)
                                                                                {
                                                                                    if (item9.WorkOperation_count_original != null)
                                                                                    {
                                                                                        ids.Add(item9.IdworkOperationByStage.ToString());
                                                                                    }
                                                                                    if (WorkOperationMenulist.Any(a => a.ParentName == item9.KeyName))
                                                                                    {
                                                                                        List<WorkOperationByStages> gettenthList = WorkOperationMenulist.Where(a => a.ParentName == item9.KeyName).ToList();
                                                                                        foreach (WorkOperationByStages item10 in gettenthList)
                                                                                        {
                                                                                            if (item10.WorkOperation_count_original != null)
                                                                                            {
                                                                                                ids.Add(item10.IdworkOperationByStage.ToString());
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
        private void RetrieveWorkOperationsByStages(object obj)
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
                if (SelectedWorkOperationMenulist == null || SelectedWorkOperationMenulist.Name == "All")
                {
                    WorkOperationsList = new ObservableCollection<WorkOperation>(WorkOperationsList_All);
                }
                else
                {
                    string Concat_ChildArticles = string.Join(",", getIdsForRetriveArticlesByParentClick().Select(x => x.ToString()).ToArray());
                    string[] ids;

                    if (SelectedWorkOperationMenulist.KeyName.Contains("Stage_"))
                        ids = (Concat_ChildArticles).Split(',');
                    else
                        ids = (Concat_ChildArticles + "," + SelectedWorkOperationMenulist.IdworkOperationByStage).Split(',');

                    WorkOperationsList = new ObservableCollection<WorkOperation>(WorkOperationsList_All.Where(x => ids.Contains(x.IdWorkOperation.ToString())));
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
        //[001][rdixit][GEOS2-3710][09/06/2022]
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);
                //if (StatusList == null)
                //    StatusList = new ObservableCollection<string>();
                //StatusList.Add("Active");
                //StatusList.Add("Draft");
                //StatusList.Add("Inactive");

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(83);
                StatusList = new List<LookupValue>(tempStatusList);
                // SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1537);
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method StatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method StatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method StatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-3710][09/06/2022]
        List<string> codeCharacters = new List<string>();
        ObservableCollection<WorkOperation> AllParentList = new ObservableCollection<WorkOperation>();
        private void FillparentList(ObservableCollection<WorkOperation> workOperationsList_All)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillparentList()...", category: Category.Info, priority: Priority.Low);

                foreach (var item in workOperationsList_All)
                {
                    string onlyCharacters = item.Code.Substring(0, 3);

                    if (!codeCharacters.Any(a => a.Equals(onlyCharacters)))
                    {
                        if (item.IdParent != null)
                        {
                            
                            ParentList = new ObservableCollection<WorkOperation>(ERMService.GetParentListByIdParentAndCode_V2240((int)item.IdParent, onlyCharacters));
                            //UpdateCountForParent();
                            ParentList.ToList().ForEach(f => f.Code = onlyCharacters);
                            ParentList = new ObservableCollection<WorkOperation>(parentList.OrderBy(x => x.Position));
                            item.ParentList = ParentList.ToList();
                            AllParentList.AddRange(ParentList);
                            codeCharacters.Add(onlyCharacters);
                        }

                    }
                    else
                    {
                        if (item.IdParent != null)
                        {

                            ObservableCollection<WorkOperation> tempAllParentList = new ObservableCollection<WorkOperation>();
                            ParentList = new ObservableCollection<WorkOperation>();
                            tempAllParentList.AddRange(AllParentList.Where(w => w.Code.Equals(onlyCharacters)));
                            ParentList = tempAllParentList;
                            // ParentList = new ObservableCollection<WorkOperation>(ERMService.GetParentListByIdParentAndCode_V2240((int)item.IdParent, onlyCharacters));
                            //UpdateCountForParent();
                            ParentList = new ObservableCollection<WorkOperation>(parentList.OrderBy(x => x.Position));
                            item.ParentList = ParentList.ToList();
                        }
                    }

                }

                GeosApplication.Instance.Logger.Log("Method FillparentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillparentList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillparentList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillparentList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-3710][09/06/2022]
        public void UpdateMultipleRowsWorkOperationGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsWorkOperationGridCommandAction ...", category: Category.Info, priority: Priority.Low);

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
                string Name = string.Empty;
                string Description = string.Empty;
                string Parent = string.Empty;
                float? Distance = null;
                float ObservedTime = 0;
                Int32 Activity = 0;
                string DetectedProblems = string.Empty;
                string ImprovementsProposals = string.Empty;
                UInt64 ParentId = 0;
                string Type = string.Empty;
                int CellStatus = 0;
                Int32 IdType;
                CellStatus = StatusList.Where(sl => sl.IdLookupValue == ERMWorkOperationViewMultipleCellEditHelper.IdStatus).Select(u => u.IdLookupValue).FirstOrDefault();
                Name = ERMWorkOperationViewMultipleCellEditHelper.Name;
                Description = ERMWorkOperationViewMultipleCellEditHelper.Description;
                ParentId = ERMWorkOperationViewMultipleCellEditHelper.IdParent;
                Distance = ERMWorkOperationViewMultipleCellEditHelper.Distance;
                ObservedTime = ERMWorkOperationViewMultipleCellEditHelper.ObservedTime;
                Activity = ERMWorkOperationViewMultipleCellEditHelper.Activity;
                ImprovementsProposals = ERMWorkOperationViewMultipleCellEditHelper.ImprovementProposal;
                DetectedProblems = ERMWorkOperationViewMultipleCellEditHelper.DetectedProblem;
                // Parent = parentList.Where(i => i.IdParent == (ulong)ERMWorkOperationViewMultipleCellEditHelper.IdParent).Select(u => u.Parent).FirstOrDefault();
                Parent = WorkOperationsList.Where(j => j.IsUpdatedRow == true).ToList().Where(i => i.Parent == ERMWorkOperationViewMultipleCellEditHelper.Parent).Select(u => u.Parent).FirstOrDefault();
                Type = TypeList.Where(i => i.Value == ERMWorkOperationViewMultipleCellEditHelper.Type).Select(u => u.Value).FirstOrDefault();
                IdType = TypeList.Where(i => i.Value == ERMWorkOperationViewMultipleCellEditHelper.Type).Select(u => u.IdLookupValue).FirstOrDefault();
                WorkOperation[] foundRow = WorkOperationsList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                #region Temparary commented
                //int count = 0;
                //if (foundRow.Any(i => i.Description == null || i.Description.Trim() == string.Empty))
                //    foreach (WorkOperation item in foundRow)
                //    {
                //        //if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                //        //{
                //        if (Description != null)
                //        {
                //            if (item.Description == null)
                //                item.Description = string.Empty;

                //            item.Description = item.Description.Trim(' ', '\r');

                //            if (string.IsNullOrEmpty(item.Description) || item.Description == "")
                //            {
                //                item.Description = string.Empty;
                //                count++;

                //            }
                //        }
                //        if (Name != null)
                //        {
                //            if (item.Name == null)
                //                item.Name = string.Empty;

                //            item.Name = item.Name.Trim(' ', '\r');

                //            if (string.IsNullOrEmpty(item.Name) || item.Name == "")
                //            {
                //                item.Name = string.Empty;
                //                count++;

                //            }
                //        }
                //        //}
                //    }
                //if (count > 0)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmptySpacesNotAllowed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    return;
                //}
                #endregion
                uint ModifierID = (uint)GeosApplication.Instance.ActiveUser.IdUser; ;
                bool isUpdated = false;
                foreach (WorkOperation item in foundRow)
                {
                    WorkOperation WO = item;
                    WorkOperation _WorkOperation = new WorkOperation();
                    _WorkOperation.IdWorkOperation = item.IdWorkOperation;
                    //_WorkOperation.IdParent = item.IdParent;
                    if (item.ParentList != null)
                        _WorkOperation.IdParent = item.ParentList.Where(w => w.Name.Equals(item.Parent)).Select(s => s.IdWorkOperation).FirstOrDefault();
                    _WorkOperation.IdStatus = item.IdStatus;
                    if (item.Type != null)
                        _WorkOperation.IdType = Convert.ToUInt32(TypeList.Where(i => i.Value.Equals(item.Type.Value)).Select(u => u.IdLookupValue).FirstOrDefault());
                    //_WorkOperation.IdType = item.IdType;
                    if (item.Name != null)
                        item.Name = item.Name.Trim(' ', '\r');
                    _WorkOperation.Name = item.Name;
                    if (item.Description != null)
                        item.Description = item.Description.Trim(' ', '\r');
                    _WorkOperation.Description = item.Description;
                    _WorkOperation.Distance = item.Distance;

                    #region GEOS2-3954 Time format HH:MM:SS
                    //int TempOTDay = item.UITempobservedTime.Days;
                    //int TempOTHours = item.UITempobservedTime.Hours;
                    //int TempOTminute = item.UITempobservedTime.Minutes;
                    //int TempOTSecond = item.UITempobservedTime.Seconds;
                    //string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + DecimalSeperator + TempOTSecond;
                    //float tempfloat = float.Parse(tempstring);
                    //ObservedTime = tempfloat;
                    #endregion

                    //_WorkOperation.ObservedTime = item.ObservedTime;
                    _WorkOperation.ObservedTime = (float?)Math.Round(Convert.ToDouble(item.UITempobservedTime.TotalMinutes), 2); // item.ObservedTime;
                    _WorkOperation.Activity = item.Activity;
                    _WorkOperation.DetectedProblems = item.DetectedProblems;
                    _WorkOperation.ImprovementsProposals = item.ImprovementsProposals;
                    //[GEOS2-3933][Rupali Sarode]
                    _WorkOperation.Remarks = item.Remarks == null ? "" : item.Remarks.Trim();
                    #region GEOS2-3880 Add Log gulab lakade
                    AddChangedWorkOperationLogDetails("Update", WO);
                    _WorkOperation.LstWorkOperationChangeLogList = WorkOperationChangeLogList.ToList();
                    #endregion
                    //isUpdated = ERMService.UpdateWorkOperationMultipleRecords_V2280(_WorkOperation,ModifierID);
                    isUpdated = ERMService.UpdateWorkOperationMultipleRecords_V2320(_WorkOperation, ModifierID);
                    WorkOperationChangeLogList.Clear();  //GEOS2-3880 Add Log gulab lakade
                }
                if (isUpdated)
                {
                    ObservableCollection<WorkOperation> foundRowFillparentList = new ObservableCollection<WorkOperation>();
                    foundRowFillparentList.AddRange(foundRow);
                    IsAllSave = true;

                    //FillWorkOperations(); //[gulablakade][GEOS2-3752]
                    FillparentList(foundRowFillparentList);
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();
                    AllParentList = new ObservableCollection<WorkOperation>();
                    codeCharacters = new List<string>();
                    // start GEOS2-3880 add log
                    ClonedWorkOperation = new ObservableCollection<WorkOperation>(WorkOperationsList.Select(i => (WorkOperation)i.Clone()).ToList());
                    //end GEOS2-3880 add log
                }
                else
                    IsAllSave = false;

                if (IsAllSave == null)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                else if (IsAllSave.Value == true)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else if (IsAllSave.Value == false)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                ERMWorkOperationViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged = false;
            }
            catch (Exception ex)
            {
                // GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsWorkOperationGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        //[001][rdixit][GEOS2-3710][09/06/2022]
        private void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList()...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> tempTypeList = new List<LookupValue>(PCMService.GetLookupValues(93));
                tempTypeList.Insert(0, new LookupValue() { IdLookupValue = 0, Value = "---", InUse = true });
                TypeList = new List<LookupValue>(tempTypeList);
                SelectedType = TypeList.FirstOrDefault();
                foreach (var item in tempTypeList)
                {
                    item.ImageData = geosService.GetLookupImages(item.ImageName); //"1-1.png"); //item.LookupValueImages
                }
                GeosApplication.Instance.Logger.Log("Method FillTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UpdateCountForParent()
        {
            foreach (WorkOperation item in parentList)
            {
                int count = 0;
                if (item.WorkOperation_count_original != null)
                {
                    count = item.WorkOperation_count_original;
                }
                if (parentList.Any(a => a.IdParent == item.IdWorkOperation))
                {
                    List<WorkOperation> getFirstParentList = parentList.Where(a => a.IdParent == item.IdWorkOperation).ToList();
                    foreach (WorkOperation item1 in getFirstParentList)
                    {
                        if (item1.WorkOperation_count_original != null)
                        {
                            count = count + item1.WorkOperation_count_original;
                        }
                        if (parentList.Any(a => a.IdParent == item1.IdWorkOperation))
                        {
                            List<WorkOperation> getSecondParentList = parentList.Where(a => a.IdParent == item1.IdWorkOperation).ToList();
                            foreach (WorkOperation item2 in getSecondParentList)
                            {
                                if (item2.WorkOperation_count_original != null)
                                {
                                    count = count + item2.WorkOperation_count_original;
                                }
                                if (parentList.Any(a => a.IdParent == item2.IdWorkOperation))
                                {
                                    List<WorkOperation> getThirdParentList = parentList.Where(a => a.IdParent == item2.IdWorkOperation).ToList();
                                    foreach (WorkOperation item3 in getThirdParentList)
                                    {
                                        if (item3.WorkOperation_count_original != null)
                                        {
                                            count = count + item3.WorkOperation_count_original;
                                        }
                                        if (parentList.Any(a => a.IdParent == item3.IdWorkOperation))
                                        {
                                            List<WorkOperation> getForthParentList = parentList.Where(a => a.IdParent == item3.IdWorkOperation).ToList();
                                            foreach (WorkOperation item4 in getForthParentList)
                                            {
                                                if (item4.WorkOperation_count_original != null)
                                                {
                                                    count = count + item4.WorkOperation_count_original;
                                                }
                                                if (parentList.Any(a => a.IdParent == item4.IdWorkOperation))
                                                {
                                                    List<WorkOperation> getFifthParentList = parentList.Where(a => a.IdParent == item4.IdWorkOperation).ToList();
                                                    foreach (WorkOperation item5 in getFifthParentList)
                                                    {
                                                        if (item5.WorkOperation_count_original != null)
                                                        {
                                                            count = count + item5.WorkOperation_count_original;
                                                        }
                                                        if (parentList.Any(a => a.IdParent == item5.IdWorkOperation))
                                                        {
                                                            List<WorkOperation> getSixthParentList = parentList.Where(a => a.IdParent == item5.IdWorkOperation).ToList();
                                                            foreach (WorkOperation item6 in getSixthParentList)
                                                            {
                                                                if (item6.WorkOperation_count_original != null)
                                                                {
                                                                    count = count + item6.WorkOperation_count_original;
                                                                }
                                                                if (parentList.Any(a => a.IdParent == item6.IdWorkOperation))
                                                                {
                                                                    List<WorkOperation> getSeventhParentList = parentList.Where(a => a.IdParent == item6.IdWorkOperation).ToList();
                                                                    foreach (WorkOperation item7 in getSeventhParentList)
                                                                    {
                                                                        if (item7.WorkOperation_count_original != null)
                                                                        {
                                                                            count = count + item7.WorkOperation_count_original;
                                                                        }
                                                                        if (parentList.Any(a => a.IdParent == item7.IdWorkOperation))
                                                                        {
                                                                            List<WorkOperation> getEightthParentList = parentList.Where(a => a.IdParent == item7.IdWorkOperation).ToList();
                                                                            foreach (WorkOperation item8 in getEightthParentList)
                                                                            {
                                                                                if (item8.WorkOperation_count_original != null)
                                                                                {
                                                                                    count = count + item8.WorkOperation_count_original;
                                                                                }
                                                                                if (parentList.Any(a => a.IdParent == item8.IdWorkOperation))
                                                                                {
                                                                                    List<WorkOperation> getNinethParentList = parentList.Where(a => a.IdParent == item8.IdWorkOperation).ToList();
                                                                                    foreach (WorkOperation item9 in getNinethParentList)
                                                                                    {
                                                                                        if (item9.WorkOperation_count_original != null)
                                                                                        {
                                                                                            count = count + item9.WorkOperation_count_original;
                                                                                        }
                                                                                        if (parentList.Any(a => a.IdParent == item9.IdWorkOperation))
                                                                                        {
                                                                                            List<WorkOperation> gettenthParentList = parentList.Where(a => a.IdParent == item9.IdWorkOperation).ToList();
                                                                                            foreach (WorkOperation item10 in gettenthParentList)
                                                                                            {
                                                                                                if (item10.WorkOperation_count_original != null)
                                                                                                {
                                                                                                    count = count + item10.WorkOperation_count_original;
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
                item.WorkOperation_count = count;
                // item.NameWithCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.WorkOperation_count) + "]");
            }
        }
        #region GEOS2-3954 Gulab lakade Time format HH:mm:ss
        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                #region GEOS2-3954 Time format HH:MM:SS
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                string tempd = Convert.ToString(observedtime);
                string[] parts = new string[2];
                int i1 = 0;
                int i2 = 0;
                if (tempd.Contains(culterseparator))
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i2 = int.Parse(parts[1]);

                    if (Convert.ToString(parts[1]).Length == 1)
                    {
                        i1 = (i1 * 60) + i2 * 10;
                    }
                    else
                    {
                        i1 = (i1 * 60) + i2;
                    }

                }
                else
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i1 = (i1 * 60);
                }



                UITempobservedTime = TimeSpan.FromSeconds(i1);
                int ts1 = UITempobservedTime.Hours;
                int ts2 = UITempobservedTime.Minutes;
                int ts3 = UITempobservedTime.Seconds;


                #endregion

                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromSeconds(0);
                return UITempobservedTime;
            }

        }

        public TimeSpan ConvertfloattoTimespanForNormalTime(string Normaltime)
        {
            TimeSpan UITempNormalTime;
            try
            {
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                #region GEOS2-3954 Time format HH:MM:SS

                string temnormaltime = Convert.ToString(Normaltime);
                string[] NormaltimeArr = new string[2];
                int nt1 = 0;
                int nt2 = 0;
                if (temnormaltime.Contains(culterseparator))
                {
                    //char[] culterseparatorarr =Convert.ToChar(culterseparator);
                    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                    nt1 = int.Parse(NormaltimeArr[0]);
                    nt2 = int.Parse(NormaltimeArr[1]);
                    if (Convert.ToString(NormaltimeArr[1]).Length == 1)
                    {
                        nt1 = (nt1 * 60) + nt2 * 10;
                    }
                    else
                    {
                        nt1 = (nt1 * 60) + nt2;
                    }

                }
                else
                {
                    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                    nt1 = int.Parse(NormaltimeArr[0]);
                    nt1 = (nt1 * 60);
                }

                UITempNormalTime = TimeSpan.FromSeconds(nt1);

                #endregion
                return UITempNormalTime;
            }
            catch (Exception ex)
            {
                UITempNormalTime = TimeSpan.FromSeconds(0);
                return UITempNormalTime;
            }

        }
        public String ConvertTimespantoFloat(String observedtime)
        {

            var currentculter = CultureInfo.CurrentCulture;
            string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
            string[] NormaltimeArr = new string[2];
            int nt1 = 0;
            int nt2 = 0;
            int nt3 = 0;
            if (observedtime.Contains(":"))
            {
                NormaltimeArr = observedtime.Split(':');
                nt1 = int.Parse(NormaltimeArr[0]);
                nt2 = int.Parse(NormaltimeArr[1]);
                nt3 = int.Parse(NormaltimeArr[2]);
                nt1 = (nt1 * 60) + nt2;
            }
            string tempstring = string.Empty;
            if (Convert.ToString(nt3).Length == 1)
            {
                tempstring = Convert.ToString(nt1) + culterseparator + "0" + Convert.ToString(nt3);
            }
            else
            {
                tempstring = Convert.ToString(nt1) + culterseparator + Convert.ToString(nt3);
            }

            return tempstring;
        }
        public string CheckHour(TimeSpan hours)
        {
            string Temptime = string.Empty;
            try
            {
                if (hours.Hours > 0)
                {
                    Temptime = Convert.ToString(hours.ToString(@"hh\:mm\:ss"));
                }
                else
                {
                    Temptime = Convert.ToString(hours.ToString(@"mm\:ss"));
                }
                return Temptime;
            }
            catch (Exception ex)
            {
                return Temptime;
            }
        }
        #endregion
        #region GEOS2-3880 work operation gulab lakade
        public void AddChangedWorkOperationLogDetails(string LogValue, WorkOperation _WorkOperation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedWorkOperationLogDetails()...", category: Category.Info, priority: Priority.Low);
                if (LogValue == "Update")
                {
                    var ClonedWOValue = ClonedWorkOperation.Where(x => x.IdWorkOperation == _WorkOperation.IdWorkOperation).FirstOrDefault();
                    if (ClonedWOValue != null)
                    {
                        //string oldWorkStage = Convert.ToString(ClonedWOValue.WorkStage);

                        //string newWorkStage = Convert.ToString(_WorkOperation.WorkStage);
                        //#region Workstage
                        //List<string> stringList = oldvalue.WorkStage.Split(',').ToList();

                        //foreach (var item in stringList)
                        //{
                        //    Stages stage = StagesList.Where(s => s.IdStage == Convert.ToInt32(item)).FirstOrDefault();
                        //    if (stage != null)
                        //    {
                        //        foreach (Stages selecteditem in SelectedStages)
                        //        {
                        //            if (Convert.ToString(selecteditem.IdStage) != Convert.ToString(stage.IdStage))
                        //            {
                        //                string log = "Work Operation Work stage " + Convert.ToString(stage.CodeWithName) + " has been removed.";
                        //                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        //            }
                        //        }

                        //    }
                        //}

                        //if (SelectedStages.Count > 0)
                        //{
                        //    foreach (Stages item in SelectedStages)
                        //    {
                        //        if (!stringList.Any(a => a.Equals(Convert.ToString(item.IdStage))))
                        //        {
                        //            //tempNewstagelist.Add(item);
                        //            //string Newstagename = Convert.ToString(item.CodeWithName);
                        //            string log = "Work Operation Work stage " + Convert.ToString(item.CodeWithName) + " has been added.";
                        //            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        //        }
                        //    }
                        //}
                        ////string newstage = Convert.ToString(SelectedStages);
                        //#endregion
                        #region Name
                        if (ClonedWOValue.Name != _WorkOperation.Name)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(_WorkOperation.Name) && !string.IsNullOrEmpty(ClonedWOValue.Name))
                            {
                                log = "The Name has been changed from " + Convert.ToString(ClonedWOValue.Name) + " to " + Convert.ToString(_WorkOperation.Name) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(_WorkOperation.Name) && !string.IsNullOrEmpty(ClonedWOValue.Name))
                            {
                                log = "The Name has been changed from " + Convert.ToString(ClonedWOValue.Name) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWOValue.Name) && !string.IsNullOrEmpty(_WorkOperation.Name))
                            {
                                log = "The Name has been changed from None to " + Convert.ToString(_WorkOperation.Name) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion

                        #region Description

                        if (_WorkOperation.Description != ClonedWOValue.Description)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(_WorkOperation.Description) && !string.IsNullOrWhiteSpace(_WorkOperation.Description) && !string.IsNullOrEmpty(ClonedWOValue.Description) && !string.IsNullOrWhiteSpace(ClonedWOValue.Description))
                            {
                                log = "The Description has been changed from " + Convert.ToString(ClonedWOValue.Description) + " to " + Convert.ToString((_WorkOperation.Description).Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(_WorkOperation.Description) && !string.IsNullOrEmpty(ClonedWOValue.Description) && !string.IsNullOrWhiteSpace(ClonedWOValue.Description))
                            {
                                log = "The Description has been changed from " + Convert.ToString((ClonedWOValue.Description).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWOValue.Description) && !string.IsNullOrEmpty(_WorkOperation.Description) && !string.IsNullOrWhiteSpace(_WorkOperation.Description))
                            {
                                log = "The Description has been changed from None to " + Convert.ToString((_WorkOperation.Description).Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region Parent

                        string SelectedParentold = string.Empty;
                        SelectedParentold = Convert.ToString(ClonedWOValue.Parent);

                        if (Convert.ToString(SelectedParentold) != Convert.ToString(_WorkOperation.Parent))
                        {

                            if (!string.IsNullOrEmpty(Convert.ToString(SelectedParentold)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Parent)) && (Convert.ToString(_WorkOperation.Parent) != "---"))
                            {
                                string log = "Work Operation Parent has been changed from " + Convert.ToString(SelectedParentold) + " to " + Convert.ToString(_WorkOperation.Parent) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                            if ((string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Parent)) || (Convert.ToString(_WorkOperation.Parent) == "---")) && (!string.IsNullOrEmpty(Convert.ToString(SelectedParentold))))
                            {
                                string log = "Work Operation Parent has been changed from " + Convert.ToString(SelectedParentold) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(SelectedParentold)) && ((Convert.ToString(_WorkOperation.Parent) != "---") || !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Parent))))
                            {
                                string log = "Work Operation Parent has been changed from None to " + Convert.ToString(_WorkOperation.Parent) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }

                        }

                        #endregion


                        #region Type
                        if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Type)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Type)))
                        {
                            if (Convert.ToString(_WorkOperation.Type.Value) != "---")
                            {
                                string log = "Work Operation Type has been changed from None to " + Convert.ToString(_WorkOperation.Type.Value) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }

                        }
                        else
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Type)) && string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Type)))
                        {
                            if ((!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Type.Value)) && Convert.ToString(ClonedWOValue.Type.Value) != "---") && (Convert.ToString(_WorkOperation.Type.Value) == "---" || String.IsNullOrEmpty(Convert.ToString(_WorkOperation.Type.Value))))
                            {
                                string log = "Work Operation Type has been changed from " + Convert.ToString(ClonedWOValue.Type.Value) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        else
                         if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Type)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Type)))
                        {
                            if (Convert.ToString(ClonedWOValue.Type.Value) != Convert.ToString(_WorkOperation.Type.Value) && (Convert.ToString(ClonedWOValue.Type.Value) != "---" && Convert.ToString(_WorkOperation.Type.Value) != "---"))
                            {
                                string log = "Work Operation Type has been changed from " + Convert.ToString(ClonedWOValue.Type.Value) + " to " + Convert.ToString(_WorkOperation.Type.Value) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region Status
                        string OldStatus = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.WOStatus)))
                        {
                            OldStatus = Convert.ToString(ClonedWOValue.WOStatus);
                        }
                        else
                        {
                            OldStatus = Convert.ToString(ClonedWOValue.Status.Value);
                        }
                        if (Convert.ToString(OldStatus) != Convert.ToString(_WorkOperation.WOStatus))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(OldStatus)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.WOStatus)))
                            {
                                string log = "Work Operation Status has been changed from " + Convert.ToString(OldStatus) + " to " + Convert.ToString(_WorkOperation.WOStatus) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(OldStatus)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.WOStatus)))
                            {
                                string log = "Work Operation Status has been changed from None to " + Convert.ToString(_WorkOperation.WOStatus) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.WOStatus)) && !string.IsNullOrEmpty(Convert.ToString(OldStatus)))
                            {
                                string log = "Work Operation Status has been changed from " + Convert.ToString(OldStatus) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }


                        #endregion
                        #region Distance
                        if (Convert.ToString(ClonedWOValue.Distance) != Convert.ToString(_WorkOperation.Distance))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Distance)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Distance)))
                            {
                                string log = "Work Operation Distance has been changed from " + Convert.ToString(ClonedWOValue.Distance) + " to " + Convert.ToString(_WorkOperation.Distance) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Distance)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Distance)))
                            {
                                string log = "Work Operation Distance has been changed from None to " + Convert.ToString(_WorkOperation.Distance) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Distance)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Distance)))
                            {
                                string log = "Work Operation Distance has been changed from " + Convert.ToString(ClonedWOValue.Distance) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region ObserveTime
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.ObservedTime)))
                        {
                            ClonedWOValue.UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(ClonedWOValue.ObservedTime)); // ConvertfloattoTimespanForNormalTime(Convert.ToString(ClonedWOValue.ObservedTime));
                        }

                        if (Convert.ToString(ClonedWOValue.UITempobservedTime) != Convert.ToString(_WorkOperation.UITempobservedTime))
                        {
                            string OldObservedTime = Convert.ToString(CheckHour(ClonedWOValue.UITempobservedTime));
                            string NewObservedTime = Convert.ToString(CheckHour(_WorkOperation.UITempobservedTime));
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.UITempobservedTime)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.UITempobservedTime)))
                            {

                                string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(OldObservedTime) + " to " + Convert.ToString(NewObservedTime) + ".";
                                //string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(ClonedWOValue.ObservedTime) + " to " + Convert.ToString(_WorkOperation.ObservedTime) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.UITempobservedTime)) && !string.IsNullOrEmpty(Convert.ToString(NewObservedTime)))
                            {
                                string log = "Work Operation ObservedTime has been changed from None to " + Convert.ToString(_WorkOperation.UITempobservedTime) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if ((string.IsNullOrEmpty(Convert.ToString(_WorkOperation.UITempobservedTime))) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.UITempobservedTime)))
                            {

                                string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(OldObservedTime) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region Activity
                        if (Convert.ToString(ClonedWOValue.Activity) != Convert.ToString(_WorkOperation.Activity))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Activity)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Activity)))
                            {
                                string log = "Work Operation Activity has been changed from " + Convert.ToString(ClonedWOValue.Activity) + " to " + Convert.ToString(_WorkOperation.Activity) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Activity)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Activity)))
                            {
                                string log = "Work Operation Activity has been changed from None to " + Convert.ToString(_WorkOperation.Activity) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Activity)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Activity)))
                            {
                                string log = "Work Operation Activity has been changed from " + Convert.ToString(ClonedWOValue.Activity) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        //#region NormalTime
                        //float oldNormalTime = 0;
                        //if (ClonedWOValue.ObservedTime.Value > 0)
                        //{
                        //    oldNormalTime = (float)Math.Round(ClonedWOValue.ObservedTime.Value * ((float)ClonedWOValue.Activity / 100), 2);
                        //}
                        //else
                        //{
                        //    oldNormalTime = ClonedWOValue.NormalTime;
                        //}
                        //if ((Convert.ToString(oldNormalTime) != Convert.ToString(_WorkOperation.NormalTime)) && oldNormalTime != 0)
                        //{
                        //    if (!string.IsNullOrEmpty(Convert.ToString(oldNormalTime)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.NormalTime)))
                        //    {
                        //        string log = "Work Operation NormalTime has been changed from " + Convert.ToString(oldNormalTime) + " to " + Convert.ToString(_WorkOperation.NormalTime) + ".";
                        //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        //    }
                        //    else
                        //        if (string.IsNullOrEmpty(Convert.ToString(oldNormalTime)) || oldNormalTime == 0)
                        //    {
                        //        string log = "Work Operation NormalTime has been changed from None to " + Convert.ToString(_WorkOperation.NormalTime) + ".";
                        //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        //    }
                        //    else
                        //        if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.NormalTime)))
                        //    {
                        //        string log = "Work Operation NormalTime has been changed from " + Convert.ToString(oldNormalTime) + " to None.";
                        //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        //    }
                        //}
                        //#endregion
                        #region Detected problem
                        if (Convert.ToString(ClonedWOValue.DetectedProblems) != Convert.ToString(_WorkOperation.DetectedProblems))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.DetectedProblems)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.DetectedProblems)))
                            {
                                string log = "Work Operation Detected Problems has been changed from " + Convert.ToString(ClonedWOValue.DetectedProblems) + " to " + Convert.ToString((_WorkOperation.DetectedProblems).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.DetectedProblems)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.DetectedProblems)))
                            {
                                string log = "Work Operation Detected Problems has been changed from None to " + Convert.ToString((_WorkOperation.DetectedProblems).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.DetectedProblems)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.DetectedProblems)))
                            {
                                string log = "Work Operation Detected Problems has been changed from " + Convert.ToString(ClonedWOValue.DetectedProblems) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region Improvementproposal
                        if (Convert.ToString(ClonedWOValue.ImprovementsProposals) != Convert.ToString(_WorkOperation.ImprovementsProposals))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.ImprovementsProposals)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.ImprovementsProposals)))
                            {
                                string log = "Work Operation Improvements Proposals has been changed from " + Convert.ToString(ClonedWOValue.ImprovementsProposals) + " to " + Convert.ToString((_WorkOperation.ImprovementsProposals).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.ImprovementsProposals)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.ImprovementsProposals)))
                            {
                                string log = "Work Operation Improvements Proposals has been changed from None to " + Convert.ToString((_WorkOperation.ImprovementsProposals).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.ImprovementsProposals)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.ImprovementsProposals)))
                            {
                                string log = "Work Operation Improvements Proposals has been changed from " + Convert.ToString((ClonedWOValue.ImprovementsProposals).Trim()) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion
                        #region Remarks
                        if (Convert.ToString(ClonedWOValue.Remarks) != Convert.ToString(_WorkOperation.Remarks))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.Remarks)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.Remarks)))
                            {
                                string log = "Work Operation Remarks has been changed from " + Convert.ToString((ClonedWOValue.Remarks).Trim()) + " to " + Convert.ToString((_WorkOperation.Remarks).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Remarks)) && !string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(_WorkOperation.Remarks)))
                            {
                                string log = "Work Operation Remarks has been changed from None to " + Convert.ToString((_WorkOperation.Remarks).Trim()) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(_WorkOperation.Remarks)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWOValue.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWOValue.Remarks)))
                            {
                                string log = "Work Operation Remarks has been changed from " + Convert.ToString((ClonedWOValue.Remarks).Trim()) + " to None.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = _WorkOperation.IdWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        #endregion

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddChangedWorkOperationLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedWorkOperationLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //private Visibility isAccordionControlVisibleLogs;
        //public Visibility IsAccordionControlVisibleLogs
        //{
        //    get { return isAccordionControlVisibleLogs; }
        //    set
        //    {
        //        isAccordionControlVisibleLogs = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisibleLogs"));
        //    }
        //}
        //private void HideLogPanel(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method HideLogPanel ...", category: Category.Info, priority: Priority.Low);

        //        if (IsAccordionControlVisibleLogs == Visibility.Collapsed)
        //            IsAccordionControlVisibleLogs = Visibility.Visible;
        //        else
        //            IsAccordionControlVisibleLogs = Visibility.Collapsed;
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method HideLogPanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion
        #endregion

    }
}
