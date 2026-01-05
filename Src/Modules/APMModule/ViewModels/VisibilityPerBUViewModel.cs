
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.APM.ViewModels
{   //[Shweta.thube][GEOS2-6696]
    public partial class VisibilityPerBUViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Declaration
        private DataTable localDataTable;
        private ObservableCollection<BandItem> bands;
        private ObservableCollection<Summary> totalSummary;
        private BandItem bandPlugin;
        private List<VisibilityPerBU> employeeListAsPerOrganization;
        private DataTable dataTable;
        private Company selectedOrganization;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        bool isBusy;
        private bool isSaveBU;
        private bool isVisible;
        private TableView view;
        private List<VisibilityPerBU> clonedEmployeeList;
        private LookupValue selectedBusinessUnit;
        DataTable dataTableForGridLayoutCopy;
        public string VisibiltyPerBUGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "VisibiityPerBUGridSetting.Xml";
        private bool isVisibilityPerBUColumnChooserVisible;
        private List<LookupValue> businessUnitList;

        #endregion

        #region  public Properties

        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }
        public ObservableCollection<Summary> TotalSummary
        {
            get { return totalSummary; }
            set
            {
                totalSummary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalSummary"));
            }
        }
        public BandItem BandPlugin
        {
            get { return bandPlugin; }
            set
            {
                bandPlugin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandPlugin"));
            }
        }
        public List<VisibilityPerBU> EmployeeListAsPerOrganization
        {
            get { return employeeListAsPerOrganization; }
            set
            {
                employeeListAsPerOrganization = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeListAsPerOrganization"));
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
        public Company SelectedOrganization
        {
            get { return selectedOrganization; }
            set
            {
                selectedOrganization = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrganization"));
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

        public bool IsSaveBU
        {
            get { return isSaveBU; }
            set
            {
                isSaveBU = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveBU"));
            }
        }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }
        public List<VisibilityPerBU> ClonedEmployeeList
        {
            get
            {
                return clonedEmployeeList;
            }

            set
            {
                clonedEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedEmployeeList"));
            }
        }
        public LookupValue SelectedBusinessUnit
        {
            get
            {
                return selectedBusinessUnit;
            }

            set
            {
                selectedBusinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBusinessUnit"));
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

        public bool IsVisibilityPerBUColumnChooserVisible
        {
            get { return isVisibilityPerBUColumnChooserVisible; }
            set
            {
                isVisibilityPerBUColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibilityPerBUColumnChooserVisible"));
            }

        }

        public List<LookupValue> BusinessUnitList
        {
            get { return businessUnitList; }
            set
            {
                businessUnitList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BusinessUnitList"));
            }
        }
        #endregion

        #region Public Commands
        public ICommand LocationListClosedCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand UpdateRowsVisibilityPerBUGridCommand { get; set; }
        public ICommand JobDescriptionShowFilterPopup { get; set; }

        public ICommand VisibilityPerBUGridControlLoadedCommand { get; set; }

        public ICommand VisibilityPerBUItemListTableViewLoadedCommand { get; set; }

        public ICommand VisibilityPerBUGridControlUnloadedCommand { get; set; }

        #endregion

        #region Constructor        
        public VisibilityPerBUViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUViewModel ...", category: Category.Info, priority: Priority.Low);
                LocationListClosedCommand = new RelayCommand(new Action<object>(FilterEmployeeListCommandAction));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
                UpdateRowsVisibilityPerBUGridCommand = new DelegateCommand<object>(UpdateRowsVisibilityPerBUGridCommandAction);
                JobDescriptionShowFilterPopup = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);
                VisibilityPerBUGridControlLoadedCommand = new RelayCommand(new Action<object>(VisibilityPerBUGridControlLoadedCommandAction));
                VisibilityPerBUItemListTableViewLoadedCommand = new RelayCommand(new Action<object>(VisibilityPerBUItemListTableViewLoadedCommandAction));
                VisibilityPerBUGridControlUnloadedCommand = new RelayCommand(new Action<object>(VisibilityPerBUGridControlUnloadedCommandAction));


                GeosApplication.Instance.Logger.Log("Constructor VisibilityPerBUViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibilityPerBUViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods 
        private void VisibilityPerBUGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(VisibiltyPerBUGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on VisibilityPerBUGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void VisibilityPerBUItemListTableViewLoadedCommandAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new System.Windows.Point(20, 180),
                Size = new System.Windows.Size(250, 250)
            };
        }

        private void VisibilityPerBUGridControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUGridControlLoadedCommandAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(VisibiltyPerBUGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(VisibiltyPerBUGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(VisibiltyPerBUGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibilityPerBUVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibilityPerBUVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsVisibilityPerBUColumnChooserVisible = true;
                }
                else
                {
                    IsVisibilityPerBUColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUGridControlLoadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on VisibilityPerBUGridControlLoadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibilityPerBUVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(VisibiltyPerBUGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibilityPerBUVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibilityPerBUVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((GridControlBand)((System.Windows.FrameworkContentElement)sender).Parent).Parent).SaveLayoutToXml(VisibiltyPerBUGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsVisibilityPerBUColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibilityPerBUVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in VisibilityPerBUVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Visibility per BU List";
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

                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    // activityTableView.ShowFixedTotalSummary = true;
                    activityTableView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    // ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                    // ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();

                    GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            //FillEmployeeList();
        }
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ActionsViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ActionsViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            //  FillEmployeeList();
        }
        private void RefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                AddColumnsToDataTable();
                FillLocationList();
                FillEmployeeList();
                FillDataTable();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void UpdateRowsVisibilityPerBUGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SaveButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
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

                List<VisibilityPerBU> UpdateBusinessUnitList = new List<VisibilityPerBU>();
                view = obj as TableView;
                GridControl gridControl = (view).Grid;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                IsSaveBU = false;

                DataRow[] foundRow = DataTableForGridLayoutCopy.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();


                // Generate businessUnitsMapping dynamically based on BusinessUnitList
                var businessUnitsMapping = BusinessUnitList.ToDictionary(bu => bu.IdLookupValue, bu => bu.Value);

                foreach (DataRow item in foundRow)
                {
                    uint idEmployee = uint.Parse(item["IdEmployee"].ToString());
                    VisibilityPerBU added = EmployeeListAsPerOrganization.First(i => i.IdEmployee == idEmployee);

                    added.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                    foreach (var bu in businessUnitsMapping)
                    {
                        int businessUnitId = bu.Key;          // Business unit ID
                        string columnName = bu.Value;        // Column name in the DataTable

                        // Check if the column exists in the DataTable
                        if (item.Table.Columns.Contains(businessUnitId.ToString()))
                        {
                            string newValue = item[businessUnitId.ToString()]?.ToString();

                            // Handle cases where newValue is either "Yes" or "No", fallback to "No" if neither
                            newValue = string.Equals(newValue, "Yes", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No";

                            // Check and update visibility if the value differs
                            //if (added.IsVisible != newValue)
                           // {
                                added.IsVisible = newValue;
                                added.IdLookupBusinessUnit = businessUnitId;
                                UpdateBusinessUnitList.Add(added.Clone() as VisibilityPerBU);
                           // }

                        }
                    }

                    item.Delete(); // Delete the processed item
                }


                IsSaveBU = APMService.AddEmployeeWithBU_V2590(UpdateBusinessUnitList);

                if (IsSaveBU == true)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BusinessUnitUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                else if (IsSaveBU == false)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ArticleUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                VisibilityPerBUViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                VisibilityPerBUViewMultipleCellEditHelper.IsValueChanged = false;
                FillEmployeeList();
                FillDataTable();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method SaveButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                BandPlugin = new BandItem() { BandName = "BusinessUnit", BandHeader = "Business Unit" };
                FillBusinessUnitList();
                AddColumnsToDataTable();
                FillLocationList();
                int IdCompany = GeosApplication.Instance.ActiveUser.Company.IdCompany;
                SelectedOrganization = APMCommon.Instance.LocationList.FirstOrDefault(loc => loc.IdCompany == IdCompany);
                FillEmployeeList();
                FillDataTable();



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
        private void AddColumnsToDataTable()

        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);
                List<BandItem> bandsLocal = new List<BandItem>();
                BandItem bandAll = new BandItem() { BandName = "All", BandHeader = "All", FixedStyle = DevExpress.Xpf.Grid.FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "FullName", HeaderText = "Name", Width = 300, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Name, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Organizations", HeaderText = "Organization", Width = 120, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Organization, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "JobDescription", HeaderText = "Job Description", Width = 150, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.JobDescription, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Scope", HeaderText = "JD Scope", Width = 100, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.JDScope, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "YesNoList", HeaderText = " ", Width = 0, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Hidden, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "EmployeeProperty", HeaderText = " ", Width = 0, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Hidden, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IsChecked", HeaderText = "IsValueChanged", VisibilityPerBUSettings = VisibilityPerBUSettingsType.Hidden, Width = 0, Visible = false, IsVertical = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdEmployee", HeaderText = "IdEmployee", VisibilityPerBUSettings = VisibilityPerBUSettingsType.Hidden, Width = 0, Visible = false, IsVertical = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "BusinessUnits", HeaderText = "IdsBusinessUnits", VisibilityPerBUSettings = VisibilityPerBUSettingsType.Hidden, Width = 0, Visible = false, IsVertical = false });

                bandsLocal.Add(bandAll);

                localDataTable = new DataTable();
                localDataTable.Columns.Add("FullName", typeof(string));
                localDataTable.Columns.Add("Organizations", typeof(string));
                localDataTable.Columns.Add("JobDescription", typeof(string));
                localDataTable.Columns.Add("Scope", typeof(string));
                localDataTable.Columns.Add("YesNoList", typeof(List<string>));
                localDataTable.Columns.Add("EmployeeProperty", typeof(VisibilityPerBU));
                localDataTable.Columns.Add("IsChecked", typeof(bool));
                localDataTable.Columns.Add("IdEmployee", typeof(int));
                localDataTable.Columns.Add("BusinessUnits", typeof(string));

                bandsLocal.Add(BandPlugin);
                BandPlugin.Columns = new ObservableCollection<ColumnItem>();

                foreach (var item in BusinessUnitList)
                {
                    if (!string.IsNullOrEmpty(item.Value)&&!BandPlugin.Columns.Any(x=>x.ColumnFieldName==item.IdLookupValue.ToString()))
                    {
                        string fieldName = item.IdLookupValue.ToString();
                        localDataTable.Columns.Add(fieldName, typeof(string));
                        BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = item.Value, Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.DynamicColumns });
                    }
                }

                //BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = "TAU", HeaderText = "TAU", Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.TAU });
                //BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = "ElectricTestBoards", HeaderText = "Electric Test Boards", Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.ElectricTestBoards });
                //BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = "Engineering", HeaderText = "Engineering", Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Engineering });
                //BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = "Assembly", HeaderText = "Assembly", Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Assembly });
                //BandPlugin.Columns.Add(new ColumnItem() { ColumnFieldName = "Advanced", HeaderText = "Advanced", Width = 100, Visible = true, IsVertical = false, VisibilityPerBUSettings = VisibilityPerBUSettingsType.Advanced });

                //localDataTable.Columns.Add("TAU", typeof(string));
                //localDataTable.Columns.Add("ElectricTestBoards", typeof(string));
                //localDataTable.Columns.Add("Engineering", typeof(string));
                //localDataTable.Columns.Add("Assembly", typeof(string));
                //localDataTable.Columns.Add("Advanced", typeof(string));

                BandPlugin.Columns=new ObservableCollection<ColumnItem>(BandPlugin.Columns.OrderBy(x=>x.HeaderText));  

                Bands = new ObservableCollection<BandItem>(bandsLocal);

                TotalSummary = new ObservableCollection<Summary>() { new Summary() { Type = SummaryItemType.Count, FieldName = "FullName", DisplayFormat = "Total Count : {0}" } };
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
        private void FillEmployeeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeList ...", category: Category.Info, priority: Priority.Low);
                //SelectedOrganization = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == SelectedOrganization.IdCompany);
                if (SelectedOrganization != null)
                {
                   // EmployeeListAsPerOrganization = APMService.GetEmployeeListWithBU_V2590(SelectedOrganization.IdCompany);
                    EmployeeListAsPerOrganization = APMService.GetEmployeeListWithBU_V2650(SelectedOrganization.IdCompany); //[shweta.thube][GEOS2-7241][27/05/2025]
                }
                CalculateLengthOfService(); //[shweta.thube][GEOS2-7241][27/05/2025]
                ClonedEmployeeList = new List<VisibilityPerBU>(EmployeeListAsPerOrganization);
                AddColumnsToDataTable();
                FillDataTable();
                GeosApplication.Instance.Logger.Log("Method FillEmployeeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);
                localDataTable.Rows.Clear();
                DataTableForGridLayoutCopy = localDataTable.Copy();

                foreach (VisibilityPerBU item in EmployeeListAsPerOrganization)
                {
                    DataRow dr = DataTableForGridLayoutCopy.NewRow();
                    dr["FullName"] = item.FullName;
                    dr["JobDescription"] = item.JobDescription;
                    dr["Scope"] = item.JDScope;
                    dr["Organizations"] = item.Organization;
                    dr["YesNoList"] = new List<string>() { "Yes", "No" };

                    var businessUnits = item.BusinessUnits?
                    .Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(bu => bu.Trim())
                    ?? new List<string>();

                    foreach (var bu in BusinessUnitList)
                    {
                        string yesNo = "No";

                        if (businessUnits.Contains(bu.IdLookupValue.ToString()))
                        {
                            yesNo = "Yes";
                        }

                        dr[bu.IdLookupValue.ToString()] = yesNo;
                    }


                    // dr["TAU"] = item.TAU;
                    //dr["ElectricTestBoards"] = item.ElectricTestBoards;
                    //dr["Engineering"] = item.Engineering;
                    //dr["Assembly"] = item.Assembly;
                    //dr["Advanced"] = item.Advanced;
                    dr["EmployeeProperty"] = item;
                    dr["IsChecked"] = false;
                    dr["IdEmployee"] = item.IdEmployee;
                    dr["BusinessUnits"] = item.BusinessUnits;

                    DataTableForGridLayoutCopy.Rows.Add(dr);
                }
                DataTable = DataTableForGridLayoutCopy;
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
        private void FillLocationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationList ...", category: Category.Info, priority: Priority.Low);

                if (APMCommon.Instance.LocationList == null)
                {
                    APMCommon.Instance.LocationList = new List<Company>(APMService.GetAuthorizedLocationListByIdUser_V2570(GeosApplication.Instance.ActiveUser.IdUser));
                }

                GeosApplication.Instance.Logger.Log("Method FillLocationList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterEmployeeListCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEmployeeListCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
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
                int IdCompany = SelectedOrganization.IdCompany;
                FillEmployeeList();
                FillDataTable();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterEmployeeListCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterEmployeeListCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterEmployeeListCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FilterEmployeeListCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "JobDescription" && e.Column.FieldName != "Scope")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "JobDescription")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("JobDescription = ''")
                    });
                    DevExpress.Xpf.Grid.TableView t = (DevExpress.Xpf.Grid.TableView)e.OriginalSource;

                    //var temp = t.Grid.ActiveFilterInfo.FilterText;
                    // var gridlist = t.Grid.VisibleItems;
                    HashSet<string> uniqueValues = new HashSet<string>();
                    foreach (VisibilityPerBU item in EmployeeListAsPerOrganization)
                    {
                        if (!string.IsNullOrEmpty(item.JobDescription) && item.JobDescription != "")
                        {
                            string[] values = item.JobDescription.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string value in values)
                            {
                                string displayValue = value.Trim();
                                if (uniqueValues.Add(displayValue))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = displayValue;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("JobDescription Like '%{0}%'", displayValue));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }
                if (e.Column.FieldName == "Scope")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Scope = ''")
                    });
                    DevExpress.Xpf.Grid.TableView t = (DevExpress.Xpf.Grid.TableView)e.OriginalSource;

                    //var temp = t.Grid.ActiveFilterInfo.FilterText;
                    // var gridlist = t.Grid.VisibleItems;
                    HashSet<string> uniqueValues = new HashSet<string>();
                    foreach (VisibilityPerBU item in EmployeeListAsPerOrganization)
                    {
                        if (!string.IsNullOrEmpty(item.JDScope) && item.JDScope != "")
                        {
                            string[] values = item.JDScope.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string value in values)
                            {
                                string displayValue = value.Trim();
                                if (uniqueValues.Add(displayValue))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = displayValue;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Scope Like '%{0}%'", displayValue));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }

                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);
              
                BusinessUnitList = new List<LookupValue>(APMService.GetLookupValues_V2550(2));//TaskBusinessUnits                 
              
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-7241][27/05/2025]
        private void CalculateLengthOfService()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()...", category: Category.Info, priority: Priority.Low);
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.Today;

                if (EmployeeListAsPerOrganization != null)
                {
                    foreach (var item in EmployeeListAsPerOrganization)
                    {
                        if (item.EmployeeContractSituations != null)
                        {
                            List<EmployeeContractSituation> ContractList = item.EmployeeContractSituations.Select(i => (EmployeeContractSituation)i.Clone()).OrderBy(j => j.ContractSituationStartDate).ToList();

                            if (ContractList.Count > 0)
                            {
                                var lastExitEvent = item.EmployeeExitEvents?.OrderByDescending(i => i.ExitDate).FirstOrDefault();

                                if (lastExitEvent != null)
                                {
                                    var Newcontract = ContractList.Where(i => i.ContractSituationStartDate.Value.Date > lastExitEvent.ExitDate.Value.Date).FirstOrDefault();

                                    if (Newcontract == null)
                                    {
                                        startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                        var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                        endDate = contract?.ContractSituationEndDate ?? DateTime.MinValue; // or handle null properly
                                    }
                                    else
                                    {
                                        startDate = Convert.ToDateTime(Newcontract.ContractSituationStartDate);
                                        var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                        if (startDate > DateTime.Today)
                                            endDate = startDate;
                                        else
                                            endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                                    }
                                }
                                else
                                {
                                    startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                    var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                    if (startDate > DateTime.Today)
                                        endDate = startDate;
                                    else
                                        endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                                }

                                int year = endDate.Year - startDate.Year;
                                int month = endDate.Month - startDate.Month;
                                int day = endDate.Day - startDate.Day;
                                if (day < 0)
                                {
                                    month -= 1;
                                    DateTime previousMonth = endDate.AddMonths(-1);
                                    day += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                                }
                                if (month < 0)
                                {
                                    year -= 1;
                                    month += 12;
                                }
                                item.ContractSituationStartDate = startDate;
                                item.LengthOfService = Convert.ToString(year) + "Y  " + Convert.ToString(month) + "M";
                            }
                        }
                    }


                }
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateLengthOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}