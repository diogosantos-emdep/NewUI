using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.ERM.Views;
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
using Emdep.Geos.Data.Common.ERM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using System.Data;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Data;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.Utility;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using Microsoft.Win32;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class StructuresEquivalencyWeightViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declarations

        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        private DataTable dataTable;
        private DataTable dataTableForGridLayout;
      //  ModulesEquivalencyWeight SelectedStructuresEquivalentWeight;
        private ObservableCollection<ModulesEquivalencyWeight> structuresEquivalencyWeightList;
        private ModulesEquivalencyWeight structuresEquivalencyWeight;
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> columns;

        private ObservableCollection<TileBarFilters> listofitem;
        private int selectedTileIndexEquivalencyWeight;
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        private string userSettingsKey = "ERM_Module_Structuers_Equivalency_Weight ";
        public string StructuresEquivalencyWeightGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "StructuresEquivalencyWeightGridSetting.Xml";
        private string myFilterString;
        private TileBarFilters selectedFilter;
        private List<GridColumn> GridColumnList;
        private ObservableCollection<ModulesEquivalencyWeight> tempStructuresEquivalencyWeightList;
        private bool isEdit;
        private int visibleRowCount;
        private bool isEquivalencyWeightColumnChooserVisible;
        #endregion Declarations

        #region Properties
        public ObservableCollection<ModulesEquivalencyWeight> StructuresEquivalencyWeightList
        {
            get
            {
                return structuresEquivalencyWeightList;
            }
            set
            {
                structuresEquivalencyWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StructuresEquivalencyWeightList"));
            }
        }
        public ModulesEquivalencyWeight StructuresEquivalencyWeight
        {
            get
            {
                return structuresEquivalencyWeight;
            }
            set
            {
                structuresEquivalencyWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StructuresEquivalencyWeight"));
            }
        }


        public DataTable DataTable
        {
            get { return dataTable; }
            set
            {
                dataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTable"));
            }
        }

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



        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
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
        public int SelectedTileIndexEquivalencyWeight
        {
            get
            {
                return selectedTileIndexEquivalencyWeight;
            }

            set
            {
                selectedTileIndexEquivalencyWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndexEquivalencyWeight"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                //if (myFilterString == "")
                //{
                //    SelectedFilter = Listofitem.FirstOrDefault();
                //}
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
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

        public string CustomFilterStringName { get; set; }

        public ObservableCollection<ModulesEquivalencyWeight> TempStructuresEquivalencyWeightList
        {
            get
            {
                return tempStructuresEquivalencyWeightList;
            }
            set
            {
                tempStructuresEquivalencyWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempStructuresEquivalencyWeightList"));
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

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public bool IsEquivalencyWeightColumnChooserVisible
        {
            get
            {
                return isEquivalencyWeightColumnChooserVisible;
            }

            set
            {
                isEquivalencyWeightColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEquivalencyWeightColumnChooserVisible"));
            }
        }
        #endregion

        #region ICommands
        public ICommand CommandEquivalencyWeightShowTileBarFilterPopupClick { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand PrintEquivalencyWeightCommand { get; set; }
        public ICommand ExportEquivalencyWeightCommand { get; set; }
        public ICommand EquivalencyWeightGridControlLoadedCommand { get; set; }
        public ICommand EquivalencyWeightItemListTableViewLoadedCommand { get; set; }
        public ICommand EquivalencyWeightGridControlUnloadedCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand EquivalencyWeightRefreshCommand { get; set; }
        public ICommand StructuresGridControlUnloadedCommand { get; set; }
        public ICommand StructuresEquivalencyWeightHyperlinkCommand { get; set; }
        #endregion

        #region Constructor
        public StructuresEquivalencyWeightViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ModuleEquivalencyWeightViewModel ...", category: Category.Info, priority: Priority.Low);

                //CommandEquivalencyWeightShowTileBarFilterPopupClick = new DelegateCommand<object>(ShowSelectedFilterEquivalencyWeightGridAction);
                //CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                //FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                PrintEquivalencyWeightCommand = new RelayCommand(new Action<object>(PrintEquivalencyWeightAction));
                ExportEquivalencyWeightCommand = new RelayCommand(new Action<object>(ExportEquivalencyWeightAction));
                StructuresGridControlUnloadedCommand = new DelegateCommand<object>(StructuresGridControlUnloadedCommandAction);
                //EquivalencyWeightGridControlLoadedCommand = new DelegateCommand<object>(EquivalencyWeightGridControlLoadedAction);
                //EquivalencyWeightItemListTableViewLoadedCommand = new DelegateCommand<object>(EquivalencyWeightItemListTableViewLoadedAction);
                //EquivalencyWeightGridControlUnloadedCommand = new DelegateCommand<object>(EquivalencyWeightGridControlUnloadedCommandAction);
                EquivalencyWeightRefreshCommand = new RelayCommand(new Action<object>(EquivalencyWeightRefreshCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                // CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                StructuresEquivalencyWeightHyperlinkCommand = new DelegateCommand<object>(StructuresEquivalencyWeightHyperlinkCommandAction);
                

                GeosApplication.Instance.Logger.Log("Constructor ModuleEquivalencyWeightViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor ProductTypeViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                StructuresEquivalencyWeightList = new ObservableCollection<ModulesEquivalencyWeight>();
              //  ERMService = new ERMServiceController("localhost:6699");
                StructuresEquivalencyWeightList.AddRange(ERMService.GetAllStructuresEquivalencyWeight_V2380());
                //  AddColumnsToDataTableWithoutBands();
                //TileBarArrange(StructuresEquivalencyWeightList);
                //AddCustomSetting();
                //MyFilterString = string.Empty;
                //AddCustomSettingCount(gridControl);
                TempStructuresEquivalencyWeightList = StructuresEquivalencyWeightList;
                MyFilterString = string.Empty; // [GEOS2-4547][Rupali Sarode][06-06-2023]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Pallavi Jadhav][Geos-4329][14 04 2023]
        private void PrintEquivalencyWeightAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEquivalencyWeightAction()...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintEquivalencyWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEquivalencyWeightAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Pallavi Jadhav][Geos-4329][14 04 2023]
        private void ExportEquivalencyWeightAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEquivalencyWeightAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "StructuresEquivalencyWeight";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Structures Equivalency Weight Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
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
                    ResultFileName = (saveFile.FileName);
                    TableView LeavesTableView = ((TableView)obj);
                    //LeavesTableView.ShowTotalSummary = false;
                    //LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    //LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ExportEquivalencyWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportEquivalencyWeightAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Pallavi Jadhav][Geos-4329][14 04 2023]
        private void StructuresGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method StructuresGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(StructuresEquivalencyWeightGridSettingFilePath);
              

                GeosApplication.Instance.Logger.Log("Method StructuresGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on StructuresGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void StructuresEquivalencyWeightHyperlinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method StructuresEquivalencyWeightHyperlinkCommandAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                ModulesEquivalencyWeight SelectedRow = (ModulesEquivalencyWeight)detailView.DataControl.CurrentItem;
                DateTime? tempEndDate;


                ModulesEquivalencyWeight SelectStructuresEquivalentWeight = new ModulesEquivalencyWeight();
                SelectStructuresEquivalentWeight = SelectedRow;

                AddEditModuleEquivalencyWeightView addEditStructuresEquivalencyWeightView = new AddEditModuleEquivalencyWeightView();
                AddEditModuleEquivalencyWeightViewModel addEditStructuresEquivalencyWeightViewModel = new AddEditModuleEquivalencyWeightViewModel();

                

                addEditStructuresEquivalencyWeightView.DataContext = addEditStructuresEquivalencyWeightViewModel;
                EventHandler handle = delegate { addEditStructuresEquivalencyWeightView.Close(); };
                addEditStructuresEquivalencyWeightViewModel.RequestClose += handle;

                addEditStructuresEquivalencyWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditStructuresEquivalencyWeight").ToString();

                addEditStructuresEquivalencyWeightViewModel.LblEquivalentWeight = "Structure";

                addEditStructuresEquivalencyWeightViewModel.EditInitStructures(SelectStructuresEquivalentWeight);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                addEditStructuresEquivalencyWeightView.Owner = Window.GetWindow(ownerInfo);
                addEditStructuresEquivalencyWeightView.ShowDialog();
                if (addEditStructuresEquivalencyWeightViewModel.IsSave == true)
                {

                    StructuresEquivalencyWeight = new ModulesEquivalencyWeight();

                    if (addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight != null)
                    {
                        //tempEndDate = addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight.EndDate;

                        //if (tempEndDate == null) tempEndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        //if (Convert.ToDateTime(DateTime.Now.ToShortDateString()) >= addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight.StartDate &&
                        //    Convert.ToDateTime(DateTime.Now.ToShortDateString()) <= tempEndDate)
                        //{

                            StructuresEquivalencyWeight = StructuresEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectStructuresEquivalentWeight.IdCPType);
                            StructuresEquivalencyWeight.EquivalentWeight = addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight.EquivalentWeight;
                            StructuresEquivalencyWeight.StartDate = addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight.StartDate;
                            StructuresEquivalencyWeight.EndDate = addEditStructuresEquivalencyWeightViewModel.LatestEquivalentWeight.EndDate;
                            StructuresEquivalencyWeight.LastUpdate = DateTime.Now;
                        //}
                        //else
                        //{
                        //    StructuresEquivalencyWeight = StructuresEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectStructuresEquivalentWeight.IdCPType);
                        //    StructuresEquivalencyWeight.EquivalentWeight = null;
                        //    StructuresEquivalencyWeight.StartDate = null;
                        //    StructuresEquivalencyWeight.EndDate = null;
                        //    StructuresEquivalencyWeight.LastUpdate = DateTime.Now;
                        //}
                    }
                    else
                    {
                        StructuresEquivalencyWeight = StructuresEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectStructuresEquivalentWeight.IdCPType);
                        StructuresEquivalencyWeight.EquivalentWeight = null;
                        StructuresEquivalencyWeight.StartDate = null;
                        StructuresEquivalencyWeight.EndDate = null;
                        StructuresEquivalencyWeight.LastUpdate = DateTime.Now;
                    }
                   
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method StructuresEquivalencyWeightHyperlinkCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method StructuresEquivalencyWeightHyperlinkCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        #region Column Chooser [Pallavi Jadhav][Geos-4329][14 04 2023]

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(StructuresEquivalencyWeightGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(StructuresEquivalencyWeightGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(StructuresEquivalencyWeightGridSettingFilePath);

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

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });



                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StructuresEquivalencyWeightGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StructuresEquivalencyWeightGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EquivalencyWeightRefreshCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightRefreshCommandAction()...", category: Category.Info, priority: Priority.Low);


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

                Init();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightRefreshCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EquivalencyWeightRefreshCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EquivalencyWeightRefreshCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EquivalencyWeightRefreshCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #endregion

    }
}
