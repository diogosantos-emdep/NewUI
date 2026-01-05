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
using Emdep.Geos.Data.Common.PCM;
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
using DevExpress.Utils.CommonDialogs.Internal;

namespace Emdep.Geos.Modules.ERM.ViewModels
{

    public class WorkStagesViewModel :ViewModelBase , INotifyPropertyChanged
    {
        #region Services

       // IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        #endregion

        #region Declaration
        TreeListControl treeListControlInstance;
        TableView tableViewInstance;
        public string ERM_WorkStageGrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_WorkStageGrid_Setting.Xml";

        private WorkOperationByStages selectedWorkOperationMenulist;
        private ObservableCollection<WorkStages> workStagesList;
        private WorkStages selectedWorkStages;
        private WorkStages selectedWorkStagesMenulist;
        private WorkStages selectedWorkStagesList;
        private ObservableCollection<Site> plantList;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private bool isColumnChooserVisibleForGrid;
        private TableView view;
        #endregion
        #region Properties
        public WorkOperationByStages SelectedWorkOperationMenulist
        {
            get { return selectedWorkOperationMenulist; }
            set
            {
                selectedWorkOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperationMenulist"));
            }

        }
        public ObservableCollection<WorkStages> WorkStagesList
        {
            get { return workStagesList; }
            set
            {
                workStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStagesList"));
            }
        }
        public WorkStages SelectedWorkStages
        {
            get { return selectedWorkStages; }
            set
            {
                selectedWorkStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkStages"));
            }

        }
        public WorkStages SelectedWorkStagesMenulist
        {
            get { return selectedWorkStagesMenulist; }
            set
            {
                selectedWorkStagesMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkStagesMenulist"));
            }

        }
        public WorkStages SelectedWorkStagesList
        {
            get { return selectedWorkStagesList; }
            set
            {
                selectedWorkStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkStagesList"));
            }

        }
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
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
        #endregion
        #region ICommands
        public ICommand AddStandardOperationsDictionaryListCommand { get; set; }
        public ICommand DeleteWorkStagesCommand { get; set; }
        public ICommand AddWorkStagesCommand { get; set; }
        public ICommand RefreshWorkStagesCommand { get; set; }
        public ICommand PrintWorkStagesCommand { get; set; }
        public ICommand ExportWorkStagesCommand { get; set; }
        public ICommand EditWorkStagesCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        #endregion

        # region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, e);
            }
        }
        #endregion
        #region Constructor
        public WorkStagesViewModel() {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOperationsViewModel()...", category: Category.Info, priority: Priority.Low);

                AddWorkStagesCommand = new DelegateCommand<object>(AddWorkStagesCommandAction);
                EditWorkStagesCommand = new RelayCommand(new Action<object>(EditWorkStagesCommandAction));
                RefreshWorkStagesCommand = new RelayCommand(new Action<object>(RefreshWorkStagesCommandAction));
                PrintWorkStagesCommand = new RelayCommand(new Action<object>(PrintWorkStagesCommandAction));
                ExportWorkStagesCommand = new RelayCommand(new Action<object>(ExportWorkStagesCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
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

                FillWorkStages();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception EX)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", EX.Message), category: Category.Exception, priority: Priority.Low);

            }

            // SetMinMaxDatesAndFillCurrencyConversions();
        }

        public void FillWorkStages()
        {
            try
            {

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                WorkStagesList = new ObservableCollection<WorkStages>();
                WorkStagesList = new ObservableCollection<WorkStages>(ERMService.GetAllWorkStages_V2350());

                //PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                foreach (var item in WorkStagesList)
                {
                   
                    
                    if (string.IsNullOrEmpty(item.PlantName))
                    {
                        item.PlantName = "ALL";
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

        private void AddWorkStagesCommandAction(object obj) {
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
                AddEditWorkStagesView addEditWorkStagesView = new AddEditWorkStagesView();
                AddEditWorkStagesViewModel addEditWorkStagesViewModel = new AddEditWorkStagesViewModel();
                EventHandler handle = delegate { addEditWorkStagesView.Close(); };
                addEditWorkStagesViewModel.RequestClose += handle;
                addEditWorkStagesViewModel.WindowHeader = Application.Current.FindResource("AddWorkStagesHeader").ToString();
                addEditWorkStagesViewModel.IsNew = true;
                //addEditWorkStagesViewModel.Init(SelectedWorkOperationMenulist);
                addEditWorkStagesView.DataContext = addEditWorkStagesViewModel;
                addEditWorkStagesViewModel.Init();

                var ownerInfo = (obj as FrameworkElement);
                addEditWorkStagesView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditWorkStagesView.ShowDialog();

                if (addEditWorkStagesViewModel.IsSave == true)
                {
                    FillWorkStages();
                }
                    GeosApplication.Instance.Logger.Log("Method AddWorksOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddWorksOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void EditWorkStagesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWorkOperationCommandAction()...", category: Category.Info, priority: Priority.Low);

                //var beforeUpdateSelectedWorkOperationMenulist = SelectedWorkOperationMenulist;
                //var beforeUpdateSelectedWorkOperationsList = SelectedWorkOperationsList;
                var beforeUpdateSelectedWorkStagesMenulist = SelectedWorkStagesMenulist;
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                WorkStages SelectedRow = (WorkStages)detailView.DataControl.CurrentItem;
                SelectedWorkStages = SelectedRow;


                AddEditWorkStagesView addEditWorkStagesView = new AddEditWorkStagesView();
                AddEditWorkStagesViewModel addEditWorkStagesViewModel = new AddEditWorkStagesViewModel();

                EventHandler handle = delegate { addEditWorkStagesView.Close(); };
                addEditWorkStagesViewModel.RequestClose += handle;
                addEditWorkStagesViewModel.WindowHeader = Application.Current.FindResource("EditWorkStagesHeader").ToString();
                addEditWorkStagesViewModel.IsNew = false;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                addEditWorkStagesViewModel.EditInit(SelectedRow);
                addEditWorkStagesView.DataContext = addEditWorkStagesViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditWorkStagesView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditWorkStagesView.ShowDialog();
                WorkOperationByStages selected_Wo = SelectedWorkOperationMenulist;
                if (addEditWorkStagesViewModel.IsSave == true)
                { 
                    FillWorkStages(); 
                }
                GeosApplication.Instance.Logger.Log("Method EditWorkOperationCommandAction()...", category: Category.Info, priority: Priority.Low);
                #region GEOS2-4046 Some issue in WO & SOD
                // shubham[skadam]GEOS2-4046 Some issue in WO & SOD 24 11 2022
                //try
                //{
                //    SelectedWorkOperationMenulist = WorkStagesMenulist.FirstOrDefault(x => x.KeyName == beforeUpdateSelectedWorkOperationMenulist.KeyName);
                //    RetrieveWorkOperationsByStages(null);
                //}
                //catch (Exception ex) { }
                #endregion
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditWorkOperationCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportWorkStagesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkStagesCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Stages";
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

                    GeosApplication.Instance.Logger.Log("Method ExportWorkStagesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkStagesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        private void PrintWorkStagesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkStagesCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkStagesListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkStagesListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintWorkStagesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkStagesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        private void RefreshWorkStagesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkStagesCommandAction()...", category: Category.Info, priority: Priority.Low);
               

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView;
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
                FillWorkStages();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshWorkStagesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkStagesCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkStagesCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkStagesCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_WorkStageGrid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_WorkStageGrid_SettingFilePath);
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
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_WorkStageGrid_SettingFilePath);

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
                gridControl.FilterString = "[IsProductionStage] In ('Yes')";
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                 new GridSummaryItem() {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}"
                }});  
                //new GridSummaryItem() {
                //    SummaryType = SummaryItemType.Sum,
                //    DisplayFormat=@" {0:hh\:mm\:ss}",
                //    FieldName = "UITempNormalTime",

                //}});
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_WorkStageGrid_SettingFilePath);
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_WorkStageGrid_SettingFilePath);

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

        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "PlantName")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

               
                if (e.Column.FieldName == "PlantName")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([PlantName])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([PlantName])")
                    });

                    foreach (var dataObject in WorkStagesList)
                    {
                        if (dataObject.PlantName == null)
                        {
                            continue;
                        }
                        else if (dataObject.PlantName != null)
                        {
                            if (dataObject.PlantName.Contains("\n"))
                            {
                                string tempPlants = dataObject.PlantName;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PlantName Like '%{0}%'", empPlants));
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
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == WorkStagesList.Where(y => y.PlantName == dataObject.PlantName).Select(slt => slt.PlantName).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = WorkStagesList.Where(y => y.PlantName == dataObject.PlantName).Select(slt => slt.PlantName).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("PlantName Like '%{0}%'", WorkStagesList.Where(y => y.PlantName == dataObject.PlantName).Select(slt => slt.PlantName).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }



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

