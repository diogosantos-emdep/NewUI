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

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class ProductTypeViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        private bool isBusy;
        private ObservableCollection<ProductTypes> productTypesMenulist;
        private ObservableCollection<ProductTypes> productTypesMenulistForGridLayout;
        private bool isDeleted;

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private DataTable dataTable;
        private DataTable dataTableForGridLayout;
        private DataTable dataTableForGridLayoutCopy;
        private DataTable localDataTable;
        private BandItem bandWays;
        private BandItem bandOptions;
        private BandItem bandDetections;
        private BandItem bandSpareparts;
        private ObservableCollection<BandItem> bands;
        private bool isModuleColumnChooserVisible;
        private bool isGridColumnChooserVisible;
        
        private bool isGridOpen;
        private bool isBandOpen;
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> columns;
        private ulong idCPType;
        private bool isStructureGridColumnChooserVisible;

        public bool iSStructureVisible;
        public bool iSModuleVisible;

        private string title;
        private ObservableCollection<TileBarFilters> listofitem;
        private string myFilterString;
        private int selectedTileIndex;

        private List<Template> templatesMenuList;
        private bool isEdit;
        private string userSettingsKey = "PCM_Modules_";
        private string userSettingsBandKey = "PCM_BandModules_";
        private int visibleRowCount;
        private TileBarFilters selectedFilter;
        private Visibility isTileBarVisible;
        private Visibility isToggleButtonVisible;

        private List<GridColumn> GridColumnList;



        #endregion

        #region Properties

        public string ModuleGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ModuleGridSetting.Xml";

        public string ModuleOnlyGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ModuleOnlyGridSetting.Xml";

        public string StructureBandSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "StructureBandSetting.Xml";

        public string StructureGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "StructureGridSetting.Xml";

        public ObservableCollection<Summary> TotalSummary { get; private set; }


        public bool IsStructureGridColumnChooserVisible
        {
            get
            {
                return isStructureGridColumnChooserVisible;
            }

            set
            {
                isStructureGridColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStructureGridColumnChooserVisible"));
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


        public ObservableCollection<ProductTypes> ProductTypesMenulist
        {
            get { return productTypesMenulist; }
            set
            {
                productTypesMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesMenulist"));
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


        public DataTable DataTableForGridLayoutCopy
        {
            get
            {
                return dataTableForGridLayoutCopy;
            }
            set
            {
                dataTableForGridLayoutCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutCopy"));
            }
        }

        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        public bool IsModuleColumnChooserVisible
        {
            get { return isModuleColumnChooserVisible; }
            set
            {
                isModuleColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleColumnChooserVisible"));
            }
        }

        public bool IsGridColumnChooserVisible
        {
            get { return isGridColumnChooserVisible; }
            set
            {
                isGridColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridColumnChooserVisible"));
            }
        }


        public BandItem BandWays
        {
            get { return bandWays; }
            set
            {
                bandWays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandWays"));
            }
        }

        public BandItem BandOptions
        {
            get { return bandOptions; }
            set
            {
                bandOptions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandOptions"));
            }
        }

        public BandItem BandDetections
        {
            get { return bandDetections; }
            set
            {
                bandDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandDetections"));
            }
        }

        public BandItem BandSpareparts
        {
            get { return bandSpareparts; }
            set
            {
                bandSpareparts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandSpareparts"));
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

        public ObservableCollection<ProductTypes> ProductTypesMenulistForGridLayout
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

        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public ulong IdCPType
        {
            get
            {
                return idCPType;
            }
            set
            {
                idCPType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCPType"));
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
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

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public int SelectedTileIndex
        {
            get
            {
                return selectedTileIndex;
            }

            set
            {
                selectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndex"));
            }
        }

        public List<Template> TemplatesMenuList
        {
            get { return templatesMenuList; }
            set
            {
                templatesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplatesMenuList"));
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

        public Visibility IsTileBarVisible
        {
            get
            {
                return isTileBarVisible;
            }

            set
            {
                isTileBarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTileBarVisible"));
            }
        }

        public Visibility IsToggleButtonVisible
        {
            get
            {
                return isToggleButtonVisible;
            }

            set
            {
                isToggleButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToggleButtonVisible"));
            }
        }

        #endregion

        #region ICommands
        public ICommand GridControlLoadedCommand { get; set; }
        public ICommand OnlyGridControlLoadedCommand { get; set; }
        public ICommand ItemListTableViewLoadedCommand { get; set; }
        public ICommand ItemListTableView_GridLoadedCommand { get; set; }
        public ICommand ProductTypeDoubleClickCommand { get; set; }
        public ICommand StructureDoubleClickCommand { get; set; }
        public ICommand AddProductTypeButtonCommand { get; set; }
        public ICommand AddStructureButtonCommand { get; set; }
        public ICommand RefreshProductTypeCommand { get; set; }
        public ICommand DeleteProductTypeCommand { get; set; }
        public ICommand DeleteProductTypeBandCommand { get; set; }

        public ICommand PrintModulesCommand { get; set; }
        public ICommand ExportModulesCommand { get; set; }
        public ICommand ShowGridViewCommand { get; set; }
        public ICommand ShowBandViewCommand { get; set; }

        public ICommand ShowStructureGridViewCommand { get; set; }
        
        public ICommand ProductTypeForGridLayOutDoubleClickCommand { get; set; }
        public ICommand AddProductTypeForGridLayoutButtonCommand { get; set; }

        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }

        #endregion

        #region Constructor

        public ProductTypeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);
                GridControlLoadedCommand = new DelegateCommand<object>(GridControlLoadedAction);
                OnlyGridControlLoadedCommand = new DelegateCommand<object>(OnlyGridControlLoadedAction);
                ItemListTableViewLoadedCommand = new DelegateCommand<object>(ItemListTableViewLoadedAction);
                ItemListTableView_GridLoadedCommand = new DelegateCommand<object>(ItemListTableViewLoadedAction);

                ProductTypeDoubleClickCommand = new RelayCommand(new Action<object>(EditProductTypeItem));
                StructureDoubleClickCommand = new RelayCommand(new Action<object>(EditProductTypeItem));
                AddProductTypeButtonCommand = new RelayCommand(new Action<object>(AddProductTypeItem));
                AddStructureButtonCommand = new RelayCommand(new Action<object>(AddProductTypeItem));
                RefreshProductTypeCommand = new RelayCommand(new Action<object>(RefreshProductTypeView));
                DeleteProductTypeCommand = new RelayCommand(new Action<object>(DeleteProductTypeItem));
                DeleteProductTypeBandCommand = new RelayCommand(new Action<object>(DeleteProductTypeItemBand));
                PrintModulesCommand = new RelayCommand(new Action<object>(PrintModule));
                ExportModulesCommand = new RelayCommand(new Action<object>(ExportModules));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowOnlyGridView));
                ShowBandViewCommand = new RelayCommand(new Action<object>(ShowBandView));

                ShowStructureGridViewCommand = new RelayCommand(new Action<object>(ShowStructureGridView));
                CommandShowFilterPopupClick = new DelegateCommand<object>(ShowSelectedFilterGridAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);


                ProductTypeForGridLayOutDoubleClickCommand = new RelayCommand(new Action<object>(EditProductTypeItem));
                AddProductTypeForGridLayoutButtonCommand = new RelayCommand(new Action<object>(AddProductTypeItem));
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);

                BandWays = new BandItem() { BandName = "Ways", BandHeader = "Ways" };
                BandOptions = new BandItem() { BandName = "Options", BandHeader = "Options" };
                BandDetections = new BandItem() { BandName = "Detections", BandHeader = "Detections" };
                BandSpareparts = new BandItem() { BandName = "Spareparts", BandHeader = "Spareparts" };

                MyFilterString = string.Empty;

                GeosApplication.Instance.Logger.Log("Constructor ProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
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
                IsBusy = true;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (iSStructureVisible)
                {
                    IsTileBarVisible = Visibility.Collapsed;
                    IsToggleButtonVisible = Visibility.Collapsed;
                    ProductTypesMenulistForGridLayout = new ObservableCollection<ProductTypes>(PCMService.GetAllProductTypes().ToList().Where(a => a.Template != null && a.Template.IdTemplate == 24).ToList());
                }
                else
                {
                    IsTileBarVisible = Visibility.Visible;
                    IsToggleButtonVisible = Visibility.Visible;
                    ProductTypesMenulistForGridLayout = new ObservableCollection<ProductTypes>(PCMService.GetAllProductTypes().ToList().Where(a => a.Template == null || a.Template.IdTemplate != 24).ToList());
                }
                IsGridOpen = true;
                IsBandOpen = false;

                AddColumnsToDataTableWithoutBands();
                FillDataTableWithoutBands();

                if (!iSStructureVisible)
                {
                    FillTemplateMenulist();
                    TileBarArrange(TemplatesMenuList);
                }

                MyFilterString = string.Empty;
                AddCustomSetting();
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

        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in TemplatesMenuList)
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

        private void ItemListTableViewLoadedAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new Point(20, 180),
                Size = new Size(250, 250)
            };
        }

        //[001][smazhar][06-29-2020][GEOS2-2447]Last Update column name not changed in Band view
        private void AddColumnsToDataTableWithoutBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem>()
                {
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Code", Width = 200, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true, },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "Template", HeaderText = "Template", Width = 200, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "Name", HeaderText = "Name", Width = 200, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "Description", HeaderText = "Description", Width = 400, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "LastUpdate", HeaderText = "Last Update", Width = 150, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "Status", HeaderText = "Status", Width = 200, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true },

                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = " ", HeaderText = " ", Width = 50, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Delete, Visible = true},

                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "IdTemplate", HeaderText = "IdTemplate", Width = 0, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false },
                    new Emdep.Geos.UI.Helper.ColumnItem() { ColumnFieldName = "IdCPType", HeaderText = "IdCPType", Width = 0, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false },
                };

                DataTableForGridLayout = new DataTable();

                DataTableForGridLayout.Columns.Add("Reference", typeof(string));
                DataTableForGridLayout.Columns.Add("Template", typeof(string));
                DataTableForGridLayout.Columns.Add("Name", typeof(string));
                DataTableForGridLayout.Columns.Add("Description", typeof(string));
                DataColumn dc = DataTableForGridLayout.Columns.Add("LastUpdate", typeof(DateTime)); 
                dc.AllowDBNull = true;

                DataTableForGridLayout.Columns.Add("Status", typeof(string));

                DataTableForGridLayout.Columns.Add("IdCPType", typeof(ulong));
                DataTableForGridLayout.Columns.Add("IdTemplate", typeof(ulong));

                TotalSummary = new ObservableCollection<Summary>()
                { new Summary() { Type = SummaryItemType.Count, FieldName = "Reference", DisplayFormat = "Total : {0}" } };

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithoutBands executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTableWithoutBands()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTableWithoutBands ...", category: Category.Info, priority: Priority.Low);

                DataTableForGridLayout.Rows.Clear();
                DataTableForGridLayoutCopy = DataTableForGridLayout.Copy();

                foreach (ProductTypes item in ProductTypesMenulistForGridLayout)
                {
                    DataRow dr = DataTableForGridLayoutCopy.NewRow();

                    dr["Reference"] = item.Reference;
                    dr["Template"] = item.Template;
                    dr["Name"] = item.Name;
                    dr["Description"] = item.Description;

                    if (item.LastUpdate != null)
                    {
                        if (Convert.ToDateTime(item.LastUpdate) == DateTime.MinValue)
                        {
                            dr["LastUpdate"] = DBNull.Value;
                        }
                        else
                            dr["LastUpdate"] = item.LastUpdate;
                    }
                    else
                    {
                        dr["LastUpdate"] = DBNull.Value;
                    }

                    try
                    {
                        if (item.Status == null || item.Status.Value == null)
                        {
                            item.Status = new LookupValue();
                            item.Status.Value = "Draft";
                            dr["Status"] = item.Status.Value;
                        }
                        else
                            dr["Status"] = item.Status.Value;
                    }
                    catch (Exception ex)
                    {
                    }

                    dr["IdCPType"] = item.IdCPType;
                    dr["IdTemplate"] = item.IdTemplate;

                    IdCPType = item.IdCPType;

                    DataTableForGridLayoutCopy.Rows.Add(dr);
                }

                DataTableForGridLayout = DataTableForGridLayoutCopy;

                GeosApplication.Instance.Logger.Log("Method FillDataTableWithoutBands() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDataTableWithoutBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditProductTypeItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProductTypeItem()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                ProductTypes productTypes = null;

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if ((System.Data.DataRowView)detailView.DataControl.CurrentItem != null)
                {
                    DataRowView dr = (System.Data.DataRowView)detailView.DataControl.CurrentItem;
                    int IdCPType = Convert.ToInt32(dr.Row["IdCPType"].ToString());
                    int IdTemplate = Convert.ToInt32(dr.Row["IdTemplate"].ToString());

                    EditProductTypeView editProductTypeView = new EditProductTypeView();
                    EditProductTypeViewModel editProductTypeViewModel = new EditProductTypeViewModel();

                    EventHandler handle = delegate { editProductTypeView.Close(); };
                    editProductTypeViewModel.RequestClose += handle;
                    editProductTypeView.DataContext = editProductTypeViewModel;

                    editProductTypeViewModel.ProductTypeItem = new ProductTypes();

                    editProductTypeViewModel.ProductTypeItem.IdCPType = (ulong)IdCPType;
                    editProductTypeViewModel.ProductTypeItem.IdTemplate = (byte)IdTemplate;
                    if (iSModuleVisible)
                    {
                        editProductTypeViewModel.Init();
                    }
                    else if(iSStructureVisible)
                    {
                        editProductTypeViewModel.Init();
                        editProductTypeViewModel.SelectedTemplate = editProductTypeViewModel.TemplatesMenuList.FirstOrDefault(x => x.IdTemplate == IdTemplate);
                    }
                    var ownerInfo = (detailView as FrameworkElement);
                    editProductTypeView.Owner = Window.GetWindow(ownerInfo);
                    editProductTypeView.ShowDialog();

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

                    if (editProductTypeViewModel.IsSaveChanges)
                    {
                        productTypes = new ProductTypes();
                        dr["Template"] = editProductTypeViewModel.SelectedTemplate;
                        dr["IdTemplate"] = editProductTypeViewModel.UpdateProductTypes.IdTemplate;
                        dr["Name"] = editProductTypeViewModel.UpdateProductTypes.Name;
                        dr["Description"] = editProductTypeViewModel.UpdateProductTypes.Description;
                        dr["LastUpdate"] = editProductTypeViewModel.UpdateProductTypes.LastUpdate;
                        dr["Status"] = editProductTypeViewModel.SelectedStatus.Value;

                        productTypes.WayList = editProductTypeViewModel.UpdateProductTypes.WayList;
                        productTypes.OptionList = editProductTypeViewModel.UpdateProductTypes.OptionList;
                        productTypes.DetectionList = editProductTypeViewModel.UpdateProductTypes.DetectionList;
                        productTypes.SparePartList = editProductTypeViewModel.UpdateProductTypes.SparePartList;

                        if (productTypes.WayList != null)
                        {
                            foreach (Ways way in productTypes.WayList)
                            {
                                if (way.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    if (!string.IsNullOrEmpty(way.Name))
                                    {
                                        if (!BandWays.Columns.Any(a => a.ColumnFieldName == way.IdWays.ToString()))
                                        {
                                            string fieldName = way.IdWays.ToString();
                                            DataTable.Columns.Add(fieldName, typeof(string));
                                            BandWays.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = way.Name, Width = 28, Visible = true, IsVertical = true });
                                            dr[way.IdWays.ToString()] = "X";
                                        }
                                        else if (BandWays.Columns.Any(a => a.ColumnFieldName == way.IdWays.ToString()))
                                        {
                                            dr[way.IdWays.ToString()] = "X";
                                        }
                                    }
                                }
                                else if (way.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    dr[way.IdWays.ToString()] = string.Empty;
                                }
                            }
                        }

                        if (productTypes.OptionList != null)
                        {
                            foreach (Options option in productTypes.OptionList)
                            {
                                if (option.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    if (!string.IsNullOrEmpty(option.Name))
                                    {
                                        if (!BandOptions.Columns.Any(a => a.ColumnFieldName == option.IdOptions.ToString()))
                                        {
                                            string fieldName = option.IdOptions.ToString();
                                            DataTable.Columns.Add(fieldName, typeof(string));
                                            BandOptions.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = option.Name, Width = 28, Visible = true, IsVertical = true });
                                            dr[option.IdOptions.ToString()] = "X";
                                        }
                                        else if (BandOptions.Columns.Any(a => a.ColumnFieldName == option.IdOptions.ToString()))
                                        {
                                            dr[option.IdOptions.ToString()] = "X";
                                        }
                                    }
                                }
                                else if (option.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    dr[option.IdOptions.ToString()] = string.Empty;
                                }
                            }
                        }

                        if (productTypes.DetectionList != null)
                        {
                            foreach (Detections detection in productTypes.DetectionList)
                            {
                                if (detection.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    if (!string.IsNullOrEmpty(detection.Name))
                                    {
                                        if (!BandDetections.Columns.Any(a => a.ColumnFieldName == detection.IdDetections.ToString()))
                                        {
                                            string fieldName = detection.IdDetections.ToString();
                                            DataTable.Columns.Add(fieldName, typeof(string));
                                            BandDetections.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = detection.Name, Width = 28, Visible = true, IsVertical = true });
                                            dr[detection.IdDetections.ToString()] = "X";
                                        }
                                        else if (BandDetections.Columns.Any(a => a.ColumnFieldName == detection.IdDetections.ToString()))
                                        {
                                            dr[detection.IdDetections.ToString()] = "X";
                                        }
                                    }
                                }
                                else if (detection.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    dr[detection.IdDetections.ToString()] = string.Empty;
                                }
                            }
                        }

                        if (productTypes.SparePartList != null)
                        {
                            foreach (SpareParts sparePart in productTypes.SparePartList)
                            {
                                if (sparePart.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    if (!string.IsNullOrEmpty(sparePart.Name))
                                    {
                                        if (!BandSpareparts.Columns.Any(a => a.ColumnFieldName == sparePart.IdSpareParts.ToString()))
                                        {
                                            string fieldName = sparePart.IdSpareParts.ToString();
                                            DataTable.Columns.Add(fieldName, typeof(string));
                                            BandSpareparts.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = sparePart.Name, Width = 28, Visible = true, IsVertical = true });
                                            dr[sparePart.IdSpareParts.ToString()] = "X";
                                        }
                                        else if (BandSpareparts.Columns.Any(a => a.ColumnFieldName == sparePart.IdSpareParts.ToString()))
                                        {
                                            dr[sparePart.IdSpareParts.ToString()] = "X";
                                        }
                                    }
                                }
                                else if (sparePart.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    dr[sparePart.IdSpareParts.ToString()] = string.Empty;
                                }
                            }
                        }

                        if (editProductTypeViewModel.SelectedTemplate != null)
                        {
                            productTypes.IdTemplate = editProductTypeViewModel.SelectedTemplate.IdTemplate;
                        }

                        gridControl.RefreshData();
                        gridControl.UpdateLayout();
                        if(IsBandOpen)
                        {
                            ProductTypesMenulist.Where(a => a.IdCPType == editProductTypeViewModel.UpdateProductTypes.IdCPType).ToList().ForEach(a => { a.Template = editProductTypeViewModel.SelectedTemplate; a.IdTemplate = editProductTypeViewModel.UpdateProductTypes.IdTemplate; });
                        }
                        else if(IsGridOpen)
                        {
                            ProductTypesMenulistForGridLayout.Where(a => a.IdCPType == editProductTypeViewModel.UpdateProductTypes.IdCPType).ToList().ForEach(a => { a.Template = editProductTypeViewModel.SelectedTemplate;a.IdTemplate = editProductTypeViewModel.UpdateProductTypes.IdTemplate; });
                        }
                        TileBarArrange(TemplatesMenuList);
                        AddCustomSetting();
                    }

                    detailView.SearchString = null;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditProductTypeItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditProductTypeItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddProductTypeItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddProductTypeItem().", category: Category.Info, priority: Priority.Low);

                ProductTypes productTypes = null;

                AddProductTypeView addProductTypeView = new AddProductTypeView();
                AddProductTypeViewModel addProductTypeViewModel = new AddProductTypeViewModel();
                EventHandler handle = delegate { addProductTypeView.Close(); };
                addProductTypeViewModel.RequestClose += handle;

                if (iSModuleVisible)
                {
                    addProductTypeViewModel.Init();
                }
                else if(iSStructureVisible)
                {
                    addProductTypeViewModel.Init();
                    addProductTypeViewModel.FillTemplateList();
                    addProductTypeViewModel.SelectedTemplate = addProductTypeViewModel.TemplatesMenuList.FirstOrDefault(a => a.IdTemplate != null && a.IdTemplate == 24);
                }
                addProductTypeView.DataContext = addProductTypeViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addProductTypeView.Owner = Window.GetWindow(ownerInfo);
                addProductTypeView.ShowDialog();

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

                if (addProductTypeViewModel.IsSaveChanges)
                {
                    productTypes = new ProductTypes();
                    addProductTypeViewModel.NewProductType.Template = addProductTypeViewModel.SelectedTemplate;
                    addProductTypeViewModel.NewProductType.Status = addProductTypeViewModel.SelectedStatus;

                    if (addProductTypeViewModel.NewProductType != null)
                    {
                        if (IsGridOpen)
                        {
                            DataRow dataRow = DataTableForGridLayout.NewRow();
                            dataRow["Reference"] = addProductTypeViewModel.NewProductType.Reference;
                            dataRow["Template"] = addProductTypeViewModel.NewProductType.Template;
                            dataRow["Name"] = addProductTypeViewModel.NewProductType.Name;
                            dataRow["Description"] = addProductTypeViewModel.NewProductType.Description;
                            dataRow["LastUpdate"] = addProductTypeViewModel.NewProductType.LastUpdate;
                            dataRow["Status"] = addProductTypeViewModel.NewProductType.Status.Value;
                            dataRow["IdCPType"] = addProductTypeViewModel.NewProductType.IdCPType;
                            dataRow["IdTemplate"] = addProductTypeViewModel.NewProductType.IdTemplate;
                            DataTableForGridLayout.Rows.InsertAt(dataRow, 0);

                            ProductTypesMenulistForGridLayout.Add(addProductTypeViewModel.NewProductType);
                        }
                        else
                        {
                            DataRow dataRow = DataTable.NewRow();
                            dataRow["Reference"] = addProductTypeViewModel.NewProductType.Reference;
                            dataRow["Template"] = addProductTypeViewModel.NewProductType.Template;
                            dataRow["Name"] = addProductTypeViewModel.NewProductType.Name;
                            dataRow["Description"] = addProductTypeViewModel.NewProductType.Description;
                            dataRow["LastUpdate"] = addProductTypeViewModel.NewProductType.LastUpdate;
                            dataRow["Status"] = addProductTypeViewModel.NewProductType.Status.Value;
                            dataRow["IdCPType"] = addProductTypeViewModel.NewProductType.IdCPType;
                            if(iSModuleVisible)
                            dataRow["IdTemplate"] = addProductTypeViewModel.NewProductType.IdTemplate;
                            else if(iSStructureVisible)
                                dataRow["IdTemplate"] = addProductTypeViewModel.NewProductType.IdTemplate=24;
                            if (addProductTypeViewModel.NewProductType.WayList != null)
                            {
                                foreach (Ways way in addProductTypeViewModel.NewProductType.WayList)
                                {
                                    if (DataTable.Columns.Contains(way.IdWays.ToString()))
                                    {
                                        dataRow[way.IdWays.ToString()] = "X";
                                    }
                                    else
                                    {
                                        DataTable.Columns.Add(way.IdWays.ToString(), typeof(string));
                                        BandWays.Columns.Add(new ColumnItem() { ColumnFieldName = way.IdWays.ToString(), HeaderText = way.Name, Width = 28, Visible = true, IsVertical = true });
                                        dataRow[way.IdWays.ToString()] = "X";
                                    }
                                }
                            }

                            if (addProductTypeViewModel.NewProductType.OptionList != null)
                            {
                                foreach (Options option in addProductTypeViewModel.NewProductType.OptionList)
                                {
                                    if (DataTable.Columns.Contains(option.IdOptions.ToString()))
                                    {
                                        dataRow[option.IdOptions.ToString()] = "X";
                                    }
                                    else
                                    {
                                        DataTable.Columns.Add(option.IdOptions.ToString(), typeof(string));
                                        BandOptions.Columns.Add(new ColumnItem() { ColumnFieldName = option.IdOptions.ToString(), HeaderText = option.Name, Width = 28, Visible = true, IsVertical = true });
                                        dataRow[option.IdOptions.ToString()] = "X";
                                    }
                                }
                            }

                            if (addProductTypeViewModel.NewProductType.DetectionList != null)
                            {
                                foreach (Detections detection in addProductTypeViewModel.NewProductType.DetectionList)
                                {
                                    if (DataTable.Columns.Contains(detection.IdDetections.ToString()))
                                    {
                                        dataRow[detection.IdDetections.ToString()] = "X";
                                    }
                                    else
                                    {
                                        DataTable.Columns.Add(detection.IdDetections.ToString(), typeof(string));
                                        BandDetections.Columns.Add(new ColumnItem() { ColumnFieldName = detection.IdDetections.ToString(), HeaderText = detection.Name, Width = 28, Visible = true, IsVertical = true });
                                        dataRow[detection.IdDetections.ToString()] = "X";
                                    }
                                }
                            }

                            if (addProductTypeViewModel.NewProductType.SparePartList != null)
                            {
                                foreach (SpareParts sparePart in addProductTypeViewModel.NewProductType.SparePartList)
                                {
                                    if (DataTable.Columns.Contains(sparePart.IdSpareParts.ToString()))
                                    {
                                        dataRow[sparePart.IdSpareParts.ToString()] = "X";
                                    }
                                    else
                                    {
                                        DataTable.Columns.Add(sparePart.IdSpareParts.ToString(), typeof(string));
                                        BandSpareparts.Columns.Add(new ColumnItem() { ColumnFieldName = sparePart.IdSpareParts.ToString(), HeaderText = sparePart.Name, Width = 28, Visible = true, IsVertical = true });
                                        dataRow[sparePart.IdSpareParts.ToString()] = "X";
                                    }
                                }
                            }
                            DataTable.Rows.InsertAt(dataRow, 0);
                            ProductTypesMenulist.Add(addProductTypeViewModel.NewProductType);

                        }

                    }
                    TileBarArrange(TemplatesMenuList);
                    AddCustomSetting();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddProductTypeItem(). executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddProductTypeItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshProductTypeView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshProductTypeView()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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

                if (IsGridOpen)
                {
                    Init();
                    Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeGridView", this, null, this, true);

                }
                else if (IsBandOpen)
                {
                    IsBandOpen = true;
                    IsGridOpen = false;
                    
                    InitStructure();
                    Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeView", this, null, this, true);
                }

                detailView.SearchString = null;
               // detailView.ShowGroupPanel = false;
                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Method RefreshProductTypeView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in RefreshProductTypeView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in RefreshProductTypeView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method RefreshCatelogueView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteProductTypeItem(object obj)
        {
            DataRow dr1 = (DataRow)((System.Data.DataRowView)obj).Row;
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProductTypeItem()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteProductTypeMessageWithoutCode"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = PCMService.IsDeletedProductType((ulong)((System.Data.DataRowView)obj).Row.ItemArray[6]);
                    if (IsDeleted)
                    {
                        ProductTypesMenulistForGridLayout.Remove(ProductTypesMenulistForGridLayout.FirstOrDefault(a => a.IdCPType == (ulong)((System.Data.DataRowView)obj).Row.ItemArray[6]));
                        DataTableForGridLayout.Rows.Remove(dr1);

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        TileBarArrange(TemplatesMenuList);
                    }
                }
                AddCustomSetting();
                DataTableForGridLayout.AcceptChanges();
               
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteProductTypeItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteProductTypeItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteProductTypeItemBand(object obj)
        {
            DataRow dr1 = (DataRow)((System.Data.DataRowView)obj).Row;
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProductTypeItemBand()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteProductTypeMessageWithoutCode"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = PCMService.IsDeletedProductType((ulong)((System.Data.DataRowView)obj).Row.ItemArray[6]);
                    if (IsDeleted)
                    {
                        ProductTypesMenulist.Remove(ProductTypesMenulist.FirstOrDefault(a => a.IdCPType == (ulong)((System.Data.DataRowView)obj).Row.ItemArray[6]));
                        DataTable.Rows.Remove(dr1);
                        TileBarArrange(TemplatesMenuList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductTypeDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                AddCustomSetting();
                DataTableForGridLayout.AcceptChanges();

                GeosApplication.Instance.Logger.Log("Method DeleteProductTypeItemBand()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItemBand() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteProductTypeItemBand() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteProductTypeItemBand()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintModule(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                if (iSModuleVisible)
                {
                    pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ModulesReportPrintHeaderTemplate"];
                    pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ModulesReportPrintFooterTemplate"];
                }
                else
                {
                    pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["StructuresReportPrintHeaderTemplate"];
                    pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["StructuresReportPrintFooterTemplate"];
                }
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportModules(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportModules()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                if(iSModuleVisible)
                    saveFile.FileName = "Modules List";
                else
                    saveFile.FileName = "Structures List";
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
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportModules()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method ExportModules()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowOnlyGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowOnlyGridView()...", category: Category.Info, priority: Priority.Low);
                
                    Init();
                ProductTypeGridView productTypeView = new ProductTypeGridView();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeGridView", this, null, this, true);

                    GridControl gridControl = obj as GridControl;

                    int visibleFalseCoulumn = 0;
                    foreach (GridColumn column in gridControl.Columns)
                    {
                        if (!column.Visible)
                        {
                            if (column.FieldName != "IdTemplate")
                            {
                                if (column.FieldName != "IdCPType")
                                {
                                    visibleFalseCoulumn++;
                                }
                            }
                        }
                    }

                    if (visibleFalseCoulumn > 0)
                    {
                        IsGridColumnChooserVisible = true;
                    }
                    else
                    {
                        IsGridColumnChooserVisible = false;
                    } 
                GeosApplication.Instance.Logger.Log("Method ShowOnlyGridView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowOnlyGridView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void InitStructure()
        {
            try
            {
                IsBusy = true;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (iSStructureVisible)
                    ProductTypesMenulist = new ObservableCollection<ProductTypes>( PCMService.GetAllProductTypes().ToList().Where(a => a.Template != null && a.Template.IdTemplate == 24).ToList());
                else
                {
                    ProductTypesMenulist = new ObservableCollection<ProductTypes>(PCMService.GetAllProductTypes().ToList().Where(a => a.Template == null || a.Template.IdTemplate != 24).ToList());
                }

                AddColumnsToDataTable();
                FillDataTable();

                FillTemplateMenulist();
                TileBarArrange(TemplatesMenuList);
                MyFilterString = string.Empty;
                AddCustomSetting();

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in InitStructure() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in InitStructure() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method InitStructure()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowBandView(object obj)
        {
            try
            {
                IsBandOpen = true;
                IsGridOpen = false;
                InitStructure();
                
                ProductTypeGridView productTypeView = new ProductTypeGridView();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeView", this, null, this, true);

                GridControl gridControl = obj as GridControl;

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
                    IsModuleColumnChooserVisible = true;
                }
                else
                {
                    IsModuleColumnChooserVisible = false;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowBandView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        //[001][smazhar][06-29-2020][GEOS2-2447]Last Update column name not changed in Band view
        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

                List<BandItem> bandsLocal = new List<BandItem>();

                BandItem bandAll = new BandItem() { BandName = "All", BandHeader = "All", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Reference", HeaderText = "Code", Width = 80, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true, });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Template", HeaderText = "Template", Width = 120, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Name", HeaderText = "Name", Width = 150, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Description", HeaderText = "Description", Width = 250, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "LastUpdate", HeaderText = "Last Update", Width = 100, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Status", HeaderText = "Status", Width = 60, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdTemplate", HeaderText = "IdTemplate", Width = 170, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdCPType", HeaderText = "IdCPType", Width = 170, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Hidden, Visible = false });

                bandsLocal.Add(bandAll);

                localDataTable = new DataTable();

                localDataTable.Columns.Add("Reference", typeof(string));
                localDataTable.Columns.Add("Template", typeof(string));
                localDataTable.Columns.Add("Name", typeof(string));
                localDataTable.Columns.Add("Description", typeof(string));
                DataColumn dc = localDataTable.Columns.Add("LastUpdate", typeof(DateTime)); //later put this
                dc.AllowDBNull = true;

                localDataTable.Columns.Add("Status", typeof(string));
                localDataTable.Columns.Add("IdCPType", typeof(ulong));
                localDataTable.Columns.Add("IdTemplate", typeof(ulong));


                bandsLocal.Add(BandWays);
                BandWays.Columns = new ObservableCollection<ColumnItem>();

                bandsLocal.Add(BandOptions);
                BandOptions.Columns = new ObservableCollection<ColumnItem>();

                bandsLocal.Add(BandDetections);
                BandDetections.Columns = new ObservableCollection<ColumnItem>();

                bandsLocal.Add(BandSpareparts);
                BandSpareparts.Columns = new ObservableCollection<ColumnItem>();

                foreach (ProductTypes productType in ProductTypesMenulist)
                {
                    if (productType.WayList != null && productType.WayList.Count > 0)
                    {
                        foreach (Ways way in productType.WayList)
                        {
                            if (!string.IsNullOrEmpty(way.Name) && !BandWays.Columns.Any(a => a.ColumnFieldName == way.IdWays.ToString()))
                            {
                                string fieldName = way.IdWays.ToString();
                                localDataTable.Columns.Add(fieldName, typeof(string));
                                BandWays.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = way.Name, Width = 28, Visible = true, IsVertical = true, ProductTypeSettings = ProductTypeSettingsType.Default });
                            }
                        }
                    }

                    if (productType.OptionList != null && productType.OptionList.Count > 0)
                    {
                        foreach (Options option in productType.OptionList)
                        {
                            if (!string.IsNullOrEmpty(option.Name) && !BandOptions.Columns.Any(a => a.ColumnFieldName == option.IdOptions.ToString()))
                            {
                                string fieldName = option.IdOptions.ToString();
                                localDataTable.Columns.Add(fieldName, typeof(string));
                                BandOptions.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = option.Name, Width = 28, Visible = true, IsVertical = true, ProductTypeSettings = ProductTypeSettingsType.Default });
                            }
                        }
                    }

                    if (productType.DetectionList != null && productType.DetectionList.Count > 0)
                    {
                        foreach (Detections detection in productType.DetectionList)
                        {
                            if (!string.IsNullOrEmpty(detection.Name) && !BandDetections.Columns.Any(a => a.ColumnFieldName == detection.IdDetections.ToString()))
                            {
                                string fieldName = detection.IdDetections.ToString();
                                localDataTable.Columns.Add(fieldName, typeof(string));
                                BandDetections.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = detection.Name, Width = 28, Visible = true, IsVertical = true, ProductTypeSettings = ProductTypeSettingsType.Default });
                            }
                        }
                    }

                    if (productType.SparePartList != null && productType.SparePartList.Count > 0)
                    {
                        foreach (SpareParts sparePart in productType.SparePartList)
                        {
                            if (!string.IsNullOrEmpty(sparePart.Name) && !BandSpareparts.Columns.Any(a => a.ColumnFieldName == sparePart.IdSpareParts.ToString()))
                            {
                                string fieldName = sparePart.IdSpareParts.ToString();
                                localDataTable.Columns.Add(fieldName, typeof(string));
                                BandSpareparts.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = sparePart.Name, Width = 28, Visible = true, IsVertical = true, ProductTypeSettings = ProductTypeSettingsType.Default });
                            }
                        }
                    }
                }

                BandItem bandDelete = new BandItem() { BandName = " ", BandHeader = " ", FixedStyle = FixedStyle.Right, OverlayHeaderByChildren = true };
                bandDelete.Columns = new ObservableCollection<ColumnItem>();
                bandDelete.Columns.Add(new ColumnItem() { ColumnFieldName = " ", HeaderText = " ",Width = 50, IsVertical = false, ProductTypeSettings = ProductTypeSettingsType.Delete, Visible = true });

                bandsLocal.Add(bandDelete);
                localDataTable.Columns.Add(" ", typeof(string));


                BandWays.Columns = new ObservableCollection<ColumnItem>(BandWays.Columns.OrderBy(x => x.HeaderText));
                BandOptions.Columns = new ObservableCollection<ColumnItem>(BandOptions.Columns.OrderBy(x => x.HeaderText));
                BandDetections.Columns = new ObservableCollection<ColumnItem>(BandDetections.Columns.OrderBy(x => x.HeaderText));
                BandSpareparts.Columns = new ObservableCollection<ColumnItem>(BandSpareparts.Columns.OrderBy(x => x.HeaderText));

                Bands = new ObservableCollection<BandItem>(bandsLocal);

                TotalSummary = new ObservableCollection<Summary>()
                { new Summary() { Type = SummaryItemType.Count, FieldName = "Reference", DisplayFormat = "Total : {0}" } };

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);

                localDataTable.Rows.Clear();
              
                foreach (ProductTypes item in ProductTypesMenulist)
                {
                    
                    DataRow dr = localDataTable.NewRow();


                    dr["Reference"] = item.Reference;
                    dr["Template"] = item.Template;
                    dr["Name"] = item.Name;
                    dr["Description"] = item.Description;

                    if (item.LastUpdate != null)
                    {
                        if (Convert.ToDateTime(item.LastUpdate) == DateTime.MinValue)
                        {
                            dr["LastUpdate"] = DBNull.Value;
                        }
                        else
                            dr["LastUpdate"] = item.LastUpdate;
                    }
                    else
                    {
                        dr["LastUpdate"] = DBNull.Value;
                    }

                    try
                    {
                        if (item.Status == null || item.Status.Value == null)
                        {
                            item.Status = new LookupValue();
                            item.Status.Value = "Draft";
                            dr["Status"] = item.Status.Value;
                        }
                        else
                            dr["Status"] = item.Status.Value;
                    }
                    catch (Exception ex)
                    {
                    }

                    dr["IdCPType"] = item.IdCPType;
                    dr["IdTemplate"] = item.IdTemplate;

                    if (item.WayList != null)
                    {
                        foreach (Ways way in item.WayList)
                        {
                            if (!string.IsNullOrEmpty(way.Name))
                            dr[way.IdWays.ToString()] = "X";
                        }
                    }

                    if (item.OptionList != null)
                    {
                        foreach (Options option in item.OptionList)
                        {
                            if (!string.IsNullOrEmpty(option.Name))
                                dr[option.IdOptions.ToString()] = "X";
                        }
                    }

                    if (item.DetectionList != null)
                    {
                        foreach (Detections detection in item.DetectionList)
                        {
                            if (!string.IsNullOrEmpty(detection.Name))
                            dr[detection.IdDetections.ToString()] = "X";
                        }
                    }

                    if (item.SparePartList != null)
                    {
                        foreach (SpareParts sparepart in item.SparePartList)
                        {
                            if (!string.IsNullOrEmpty(sparepart.Name))
                            dr[sparepart.IdSpareParts.ToString()] = "X";
                        }
                    }

                    localDataTable.Rows.Add(dr);
                }

                DataTable = localDataTable;

                GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GridControlLoadedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction...", category: Category.Info, priority: Priority.Low);
                {
                    if (IsBandOpen)
                    {
                        int visibleFalseColumn = 0;
                        GridControl gridControl = obj as GridControl;
                        TableView tableView = (TableView)gridControl.View;

                        gridControl.BeginInit();

                        if (iSModuleVisible)
                        {
                            if (File.Exists(ModuleGridSettingFilePath))
                            {
                                gridControl.RestoreLayoutFromXml(ModuleGridSettingFilePath);
                            }
                        }
                        if(iSStructureVisible)
                        {
                            if (File.Exists(StructureBandSettingFilePath))
                            {
                                gridControl.RestoreLayoutFromXml(StructureBandSettingFilePath);
                            }
                        }

                        //This code for save grid layout.
                        if(iSModuleVisible)
                            gridControl.SaveLayoutToXml(ModuleGridSettingFilePath);
                        else if(iSStructureVisible)
                            gridControl.SaveLayoutToXml(StructureBandSettingFilePath);

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

                            if (!column.Visible)
                            {
                                if ((column.FieldName != "IdTemplate" && column.FieldName != "IdCPType"))
                                {
                                    visibleFalseColumn++;
                                    ((Emdep.Geos.UI.Helper.ColumnItem)column.DataContext).IsVertical = false;
                                }
                            }
                        }

                        if (visibleFalseColumn > 0)
                        {
                            IsModuleColumnChooserVisible = true;
                        }
                        else
                        {
                            IsModuleColumnChooserVisible = false;
                        }
                        gridControl.EndInit();
                        tableView.SearchString = null;
                        tableView.ShowGroupPanel = false;
                    }
                }

                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error on GridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnlyGridControlLoadedAction(object obj)
         {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnlyGridControlLoadedAction...", category: Category.Info, priority: Priority.Low);

                
                if (IsGridOpen)
                {
                    int visibleFalseColumn = 0;
                    GridControl gridControl = obj as GridControl;

                    gridControl.BeginInit();

                    if (iSModuleVisible)
                    {
                        if (File.Exists(ModuleOnlyGridSettingFilePath))
                        {
                            gridControl.RestoreLayoutFromXml(ModuleOnlyGridSettingFilePath);
                        }
                    }
                    if(iSStructureVisible)
                    {
                        if (File.Exists(StructureGridSettingFilePath))
                        {
                            gridControl.RestoreLayoutFromXml(StructureGridSettingFilePath);
                        }
                    }

                    foreach (GridColumn column in gridControl.Columns)
                    {
                        DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                        if (descriptor != null)
                        {
                            descriptor.AddValueChanged(column, VisibleChangedForGrid);
                        }

                        DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                        if (descriptorColumnPosition != null)
                        {
                            descriptorColumnPosition.AddValueChanged(column, VisibleIndexChangedForGrid);
                        }

                        if (!column.Visible)
                        {
                            if(column.FieldName !="IdTemplate")
                            {
                                if(column.FieldName!= "IdCPType")
                                {
                                    visibleFalseColumn++;
                                }
                            }
                        }
                    }

                    if (visibleFalseColumn > 0)
                    {
                        IsGridColumnChooserVisible = true;
                    }
                    else
                    {
                        IsGridColumnChooserVisible = false;
                    }

                    gridControl.EndInit();
                }
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OnlyGridControlLoadedAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error on OnlyGridControlLoadedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (IsBandOpen)
                {
                    if (column.ShowInColumnChooser)
                    {
                        if(iSModuleVisible)
                        ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ModuleGridSettingFilePath);
                        else if (iSStructureVisible)
                            ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(StructureBandSettingFilePath);
                    }

                    if (column.Visible == false)
                    {
                        IsModuleColumnChooserVisible = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    if (iSModuleVisible)
                        ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(ModuleGridSettingFilePath);
                    if (iSStructureVisible)
                        ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(StructureBandSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleChangedForGrid(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChangedForGrid ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    if (column.ShowInColumnChooser)
                    {
                        if(iSModuleVisible)
                        ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ModuleOnlyGridSettingFilePath);
                        else if (iSStructureVisible)
                            ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StructureGridSettingFilePath);
                    }

                    if (!column.Visible)
                    {
                        if (column.FieldName != "IdTemplate")
                        {
                            if (column.FieldName != "IdCPType")
                            {
                                IsGridColumnChooserVisible = true;
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChangedForGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibleChangedForGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChangedForGrid(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChangedGorGrid ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                {
                    if(iSModuleVisible)
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ModuleOnlyGridSettingFilePath);
                    else if (iSStructureVisible)
                        ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(StructureGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChangedGorGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibleIndexChangedGorGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TileBarArrange(List<Template> templateMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();

                if (IsBandOpen)
                {
                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = "All",
                        DisplayText = "All",
                        EntitiesCount = ProductTypesMenulist.Count(),
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });
                }
                else if(IsGridOpen)
                {
                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = "All",
                        DisplayText = "All",
                        EntitiesCount = ProductTypesMenulistForGridLayout.Count(),
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200
                    });

                }
                if (templateMenulist == null)
                {
                    templateMenulist = new List<Template>();
                }
                List<Template> TempTemplateList = templateMenulist.Where(a => a.IdTemplate == 0 || a.IdTemplate != 24).ToList();
                if (TempTemplateList != null)
                {
                    foreach (Template template in TempTemplateList)
                    {
                        if (IsBandOpen)
                        {
                            if (!Listofitem.Any(a => a.DisplayText == template.Name))
                            {
                                Listofitem.Add(new TileBarFilters()
                                {
                                    Caption = template.Name,
                                    DisplayText = template.Name,
                                    EntitiesCount = ProductTypesMenulist.Count(x => x.IdTemplate == template.IdTemplate && x.IdTemplate!=0),
                                    BackColor = template.HtmlColor,
                                    EntitiesCountVisibility = Visibility.Visible,
                                    Height = 80,
                                    width = 200
                                });
                            }
                        }
                        else if(IsGridOpen)
                        {
                            if (!Listofitem.Any(a => a.DisplayText == template.Name))
                            {
                                Listofitem.Add(new TileBarFilters()
                                {
                                    Caption = template.Name,
                                    DisplayText = template.Name,
                                    EntitiesCount = ProductTypesMenulistForGridLayout.Count(x => x.IdTemplate == template.IdTemplate && x.IdTemplate != 0),
                                    EntitiesCountVisibility = Visibility.Visible,
                                    BackColor = template.HtmlColor,
                                    Height = 80,
                                    width = 200
                                });
                            }
                        }
                    }
                }

                if (IsBandOpen)
                {
                    //Listofitem.Add(new TileBarFilters()
                    //{
                        //Caption = "Unassigned",
                        //Id = 0,
                        //EntitiesCount = ProductTypesMenulist.Count(x => x.Template == null),
                        //ImageUri = "Template.png",
                        //BackColor = "Wheat",
                        //EntitiesCountVisibility = Visibility.Visible,
                        //Height = 80,
                        //width = 200
                    //});
                }
                else if(IsGridOpen)
                {
                    //Listofitem.Add(new TileBarFilters()
                    //{
                        //Caption = "Unassigned",
                        //Id = 0,
                        //EntitiesCount = ProductTypesMenulistForGridLayout.Count(x => x.Template == null),
                        //ImageUri = "Template.png",
                        //BackColor = "Wheat",
                        //EntitiesCountVisibility = Visibility.Visible,
                       // Height = 80,
                        //width = 200
                    //});
                }

                Listofitem.Add(new TileBarFilters()
                {
                    

                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });


                // After change index it will automatically redirect to method ShowSelectedFilterGridAction(object obj) and arrange the tile section count.
                if (Listofitem.Count > 0)
                    SelectedTileIndex = 0;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTemplateMenulist()
        {
            try
            {
                TemplatesMenuList = new List<Template>(PCMService.GetAllTemplates().ToList().Where(a => a.IdTemplate == null || a.IdTemplate != 24).ToList());

                var List = TemplatesMenuList.GroupBy(info => info.IdTemplate)
                               .Select(group => new
                               {
                                   Name = TemplatesMenuList.FirstOrDefault(a => a.IdTemplate == group.Key).Name,
                                   Count = TemplatesMenuList.Where(b => b.IdTemplate == null && b.IdTemplate != 24).Count(),
                               }).ToList();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTemplateMenulist() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowSelectedFilterGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterGridAction....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                    string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                    string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                    CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    //if (CustomFilterStringName == "Unassigned")
                    //{
                    //    MyFilterString = "IsNullOrEmpty([Template])";
                    //    return;
                    //}

                    if (str == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            MyFilterString = _FilterString;
                        else
                            MyFilterString = string.Empty;
                    }
                    else if (str.Equals("All"))
                    {
                        MyFilterString = string.Empty;
                    }
                    else
                    {
                        MyFilterString = "[Template] In ('" + str + "')";
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
                if (IsBandOpen)
                {
                    tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsBandKey)).ToList();
                }
                else if (IsGridOpen)
                {
                    tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                }
                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        int count = 0;
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                            if (IsBandOpen)
                            {
                                count = DataTable.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).ToList().Count;
                            }
                            else if (IsGridOpen)
                            {
                                count = DataTableForGridLayout.Select(CriteriaToWhereClauseHelper.GetDataSetWhere(op)).ToList().Count;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        if (IsBandOpen)
                        {
                            Listofitem.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsBandKey, ""),
                                Id = 0,
                                BackColor = null,
                                ForeColor = null,
                                FilterCriteria = item.Value,
                                EntitiesCount = count,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });
                        }
                        else if (IsGridOpen)
                        {
                            Listofitem.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsKey, ""),
                                Id = 0,
                                BackColor = null,
                                ForeColor = null,
                                FilterCriteria = item.Value,
                                EntitiesCount = count,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });
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
                            if (IsBandOpen)
                            {
                                if (setting.Key.Contains(userSettingsBandKey))
                                    key = setting.Key.Replace(userSettingsBandKey, "");
                            }
                            else if (IsGridOpen)
                            {
                                if (setting.Key.Contains(userSettingsKey))
                                    key = setting.Key.Replace(userSettingsKey, "");
                            }
                            

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
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            if (IsBandOpen)
                            {
                                if (setting.Key.Contains(userSettingsBandKey))
                                    key = setting.Key.Replace(userSettingsBandKey, "");

                                if (!key.Equals(filterCaption))
                                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                                else
                                    lstUserConfiguration.Add(new Tuple<string, string>((userSettingsBandKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                            }
                            else if (IsGridOpen)
                            {
                                if (setting.Key.Contains(userSettingsKey))
                                    key = setting.Key.Replace(userSettingsKey, "");

                                if (!key.Equals(filterCaption))
                                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                                else
                                    lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));
                            }
                            
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
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
                    if (IsBandOpen)
                    {
                        filterName = userSettingsBandKey + customFilterEditorViewModel.FilterName;
                    }
                    else if (IsGridOpen)
                    {
                        filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    }
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

        private void ShowStructureGridView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowStructureGridView()...", category: Category.Info, priority: Priority.Low);

                InitStructure();
                Service.Navigate("Emdep.Geos.Modules.PCM.Views.ProductTypeStructureGridView", this, null, this, true);

                GridControl gridControl = obj as GridControl;

                int visibleFalseCoulumn = 0;
                foreach (GridColumn column in gridControl.Columns)
                {
                    if (!column.Visible)
                    {
                        if (column.FieldName != "IdTemplate")
                        {
                            if (column.FieldName != "IdCPType")
                            {
                                visibleFalseCoulumn++;
                            }
                        }
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsStructureGridColumnChooserVisible = true;
                }
                else
                {
                    IsStructureGridColumnChooserVisible = false;
                }
                GeosApplication.Instance.Logger.Log("Method ShowStructureGridView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ShowStructureGridView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
    }
}
