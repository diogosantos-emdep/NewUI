using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Crm.Views;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class PlantQuotaViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region TaskLog
        // [M050-05]	[Edit plant quota] [adadibathina]

        #endregion


        #region Services
       // ICrmService crmControl = new CrmServiceController("localhost:6699");
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        #endregion // Services

        #region Declaration

        DataTable dtPlantQuota;
        DataTable dtPlantQuotaCopy;
        public string PlantQuotaGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PlantQuota.Xml";
        bool isPlantQuotaColumnChooserVisible;
        bool isBusy;
        private bool isInit;
        private string myFilterString;

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private ObservableCollection<BandItem> bands= new ObservableCollection<BandItem>();
        private ObservableCollection<ParentBandItem> bands_FirstLevel_Year = 
            new ObservableCollection<ParentBandItem>();
        private PlantQuotaViewModel objPlantQuotaViewModel;

        #endregion // Declaration

        #region Public Properties
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }


        public IList<LookupValue> BusinessUnitList { get; set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
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

        public ObservableCollection<ParentBandItem> Bands_FirstLevel_Year
        {
            get { return bands_FirstLevel_Year; }
            set
            {
                bands_FirstLevel_Year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands_FirstLevel_Year"));
            }
        }

        public PlantQuotaViewModel ObjPlantQuotaViewModel
        {
            get { return objPlantQuotaViewModel; }
            set
            {
                objPlantQuotaViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPlantQuotaViewModel"));
            }
        }

        public List<PlantBusinessUnitSalesQuota> CompanyPlantQuota { get; set; }

        private DataTable dtPlantQuotaedit;
        public DataTable DtPlantQuotaEdit
        {
            get { return dtPlantQuotaedit; }
            set
            {
                dtPlantQuotaedit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantQuota"));
            }
        }

        public DataTable DtPlantQuota
        {
            get { return dtPlantQuota; }
            set
            {
                dtPlantQuota = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantQuota"));
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
        public DataTable DtPlantQuotaCopy
        {
            get { return dtPlantQuotaCopy; }
            set
            {
                dtPlantQuotaCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantQuotaCopy"));
            }
        }

        public bool IsPlantQuotaColumnChooserVisible
        {
            get { return isPlantQuotaColumnChooserVisible; }
            set
            {
                isPlantQuotaColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlantQuotaColumnChooserVisible"));
            }
        }
        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        #endregion  // Properties

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

        #region Commands
        public ICommand PrintButtonCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand RefreshPlantQuotaViewCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand ExportButtonCommand { get; set; }


        #endregion // Commands

        #region Constructor
        public PlantQuotaViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PlantQuotaViewModel ...", category: Category.Info, priority: Priority.Low);
                IsInit = true;
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintPlantQuotaGrid));
                //CustomCellAppearanceCommand = new DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                CommandGridDoubleClick = new DelegateCommand<object>(EditPlantQuotaViewWindowShow);
                RefreshPlantQuotaViewCommand = new RelayCommand(new Action<object>(RefreshPlantQuota));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportPlantQuotaButtonCommandAction));
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                PlantQuotaDetails();
                IsInit = false;
                GeosApplication.Instance.Logger.Log("Constructor PlantQuotaViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsInit = false;
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for Export to Excel
        /// </summary>
        /// <param name="obj">PlantQuotaGridView</param>
        public void ExportPlantQuotaButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPlantQuotaButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Plant Quota";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (.)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                    TableView plantQuotaView = ((TableView)obj);

                    plantQuotaView.ShowTotalSummary = false;
                    plantQuotaView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Rotate Columns...
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;
                    int colCount = 0;
                    //DataRow dr = DtPlantQuotaCopy.Rows.IndexOf(0);
                    foreach (GridColumn gridColumn in plantQuotaView.VisibleColumns)
                    {
                        if (BusinessUnitList.Any(op => (op.Value == gridColumn.Header.ToString()
                                    || (gridColumn.Header.ToString().Contains(op.Value)
                                     ))))
                        {
                            string colText = worksheetColumns[colCount].Heading + 3;
                            Cell cell = worksheet.Cells[colText];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            //cell.Alignment.RotationAngle = 90;
                        }
                        else if (gridColumn.Header.ToString().Contains("Total") ||
                            (gridColumn.Header.ToString().Contains("Rate")) || 
                            (gridColumn.Header.ToString().Contains("Date")) || 
                            (gridColumn.Header.ToString().Contains("Quota")) ||
                            (gridColumn.Header.ToString().Contains("PlantName")) ||
                            (gridColumn.Header.ToString().Contains("Region")))
                        {
                            string colText = worksheetColumns[colCount].Heading + 3;
                            Cell cell = worksheet.Cells[colText];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                           // cell.Alignment.RotationAngle = 90;

                        }
                        
                        colCount++;
                    }

                    DevExpress.Spreadsheet.CellRange colRangeRow1 = worksheet.Rows[0];
                    colRangeRow1.Font.Bold = true;
                    DevExpress.Spreadsheet.CellRange colRangeRow2 = worksheet.Rows[1];
                    colRangeRow2.Font.Bold = true;
                    DevExpress.Spreadsheet.CellRange colRangeRow3 = worksheet.Rows[2];
                    colRangeRow3.Font.Bold = true;
                    control.SaveDocument();
                    IsBusy = false;
                    plantQuotaView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportPlantQuotaButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportPlantQuotaButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }

        /// <summary>
        /// Method for  fill Target And Forecast Details
        /// </summary>
        private void PlantQuotaDetails()
        {
            GeosApplication.Instance.Logger.Log("Method PlantQuotaDetails ...", category: Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                PlantQuotaDetailsByUser();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                PlantQuotaDetailsByPlant();
            }
            else
            {
                PlantQuotaDetailsByActiveUser();
            }
            GeosApplication.Instance.Logger.Log("Method PlantQuotaDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void PlantQuotaDetailsByUser()
        {
            GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByUser ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //AddColumnsToDataTable();
                // CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionwise(GeosApplication.Instance.ActiveUser.IdUser,
                //  GeosApplication.Instance.IdCurrencyByRegion,
                // Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.IdUserPermission);
                //CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(GeosApplication.Instance.ActiveUser.IdUser,
                    CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(GeosApplication.Instance.ActiveUser.IdUser,
                 GeosApplication.Instance.IdCurrencyByRegion,
                 GeosApplication.Instance.SelectedyearStarDate,
                 GeosApplication.Instance.SelectedyearEndDate,
                 GeosApplication.Instance.IdUserPermission);
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PlantQuotaDetailsByPlant()
        {
            GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByPlant ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //AddColumnsToDataTable();
                //CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionwise(GeosApplication.Instance.ActiveUser.IdUser,
                // GeosApplication.Instance.IdCurrencyByRegion,
                // Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.IdUserPermission).OrderBy(x => x.ShortName).ToList();
               // CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(GeosApplication.Instance.ActiveUser.IdUser,
                CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(GeosApplication.Instance.ActiveUser.IdUser,
                                GeosApplication.Instance.IdCurrencyByRegion,
                                GeosApplication.Instance.SelectedyearStarDate,
                                GeosApplication.Instance.SelectedyearEndDate,
                                GeosApplication.Instance.IdUserPermission).OrderBy(x => x.ShortName).ToList();
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PlantQuotaDetailsByActiveUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByActiveUser ...", category: Category.Info, priority: Priority.Low);
                //CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionwise(GeosApplication.Instance.ActiveUser.IdUser,
                //                                                                    GeosApplication.Instance.IdCurrencyByRegion,
                //                                                                    Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.IdUserPermission).OrderBy(x => x.ShortName).ToList();
               // CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(GeosApplication.Instance.ActiveUser.IdUser,
                CompanyPlantQuota = crmControl.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(GeosApplication.Instance.ActiveUser.IdUser,
                GeosApplication.Instance.IdCurrencyByRegion,
                GeosApplication.Instance.SelectedyearStarDate,
                GeosApplication.Instance.SelectedyearEndDate,
                GeosApplication.Instance.IdUserPermission).OrderBy(x => x.ShortName).ToList();
                AddColumnsToDataTable();
                FillDashboard();
                GeosApplication.Instance.Logger.Log("Method PlantQuotaDetailsByActiveUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PlantQuotaDetailsByActiveUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }
            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                PlantQuotaDetailsByUser();
            }
            else
            {
                CompanyPlantQuota = new List<PlantBusinessUnitSalesQuota>();
                DtPlantQuota.Rows.Clear();
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                PlantQuotaDetailsByPlant();
            }
            else
            {
                CompanyPlantQuota = new List<PlantBusinessUnitSalesQuota>();
                DtPlantQuota.Rows.Clear();
            }
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// Method for fill Target And Forecast Details from database.
        /// </summary>
        private void RefreshPlantQuota(object obj)
        {

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
            IsInit = false;
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
            PlantQuotaDetails();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }



        }

        /// <summary>
        /// Method for BestFit the grid and save and load Grid as per user.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;
                if (File.Exists(PlantQuotaGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PlantQuotaGridSettingFilePath);
                }
                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }
                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }
                if (visibleFalseCoulumn > 0)
                {
                    IsPlantQuotaColumnChooserVisible = true;
                }
                else
                {
                    IsPlantQuotaColumnChooserVisible = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                //datailView.BestFitColumns();
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PlantQuotaGridSettingFilePath);
                }
                if (column.Visible == false)
                {
                    IsPlantQuotaColumnChooserVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);
                //Cmtd
                //Bands = new ObservableCollection<BandItem>();
                if (Bands_FirstLevel_Year == null)
                {
                    Bands_FirstLevel_Year = new ObservableCollection<ParentBandItem>();
                }
                else
                {
                    Bands_FirstLevel_Year.Clear();
                }
                ParentBandItem ParentBandItem1 = new ParentBandItem()
                {
                    Header = "",
                    Name = "ParentBandItem1",
                    HeaderToolTip = ""
                };
                Bands_FirstLevel_Year.Add(ParentBandItem1);
                //   ParentBandItem1 = new ParentBandItem()
                //  {
                //      Header = "ParentBandItem2",
                //      Name = "ParentBandItem2",
                //      HeaderToolTip = "ParentBandItem2"
                //  };
                //  Bands_FirstLevel_Year.Add(ParentBandItem1);
                //   ParentBandItem1 = new ParentBandItem()
                //  {
                //      Header = "ParentBandItem3",
                //      Name = "ParentBandItem3",
                //      HeaderToolTip = "ParentBandItem3"
                //  };
                //  Bands_FirstLevel_Year.Add(ParentBandItem1);

                //  //gridControlBand1.Bands.Add(Bands);
                //  Bands.Clear();
                BandItem band1 = new BandItem() { BandHeader = "" };
                band1.Columns = new ObservableCollection<ColumnItem>();
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "PlantName", HeaderText = "PlantName", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Region", HeaderText = "Region", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                //  Bands.Add(band1);
                Bands_FirstLevel_Year[0].Bands.Add(band1);


                //  band1 = new BandItem() { BandHeader = "" };
                //  band1.Columns = new ObservableCollection<ColumnItem>();
                //  band1.Columns.Add(new ColumnItem() { ColumnFieldName = "PlantName", HeaderText = "PlantName", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                //  band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Region", HeaderText = "Region", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                // // Bands.Add(band1);
                //  Bands_FirstLevel_Year[1].Bands.Add(band1);

                //  band1 = new BandItem() { BandHeader = "" };
                //  band1.Columns = new ObservableCollection<ColumnItem>();
                //  band1.Columns.Add(new ColumnItem() { ColumnFieldName = "PlantName", HeaderText = "PlantName", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                //  band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Region", HeaderText = "Region", Width = 120, IsVertical = false, Settings = SettingsType.Default, Visible = true });
                //  //Bands.Add(band1);
                //  Bands_FirstLevel_Year[2].Bands.Add(band1);

                DtPlantQuotaCopy = new DataTable();
                DtPlantQuotaCopy.Columns.Add("PlantName", typeof(string));
                DtPlantQuotaCopy.Columns.Add("Region", typeof(string));
                DtPlantQuotaCopy.Columns.Add("IdPlant", typeof(long));
                DtPlantQuotaCopy.Columns.Add("IdCurrency", typeof(Int16));

                BusinessUnitList = crmControl.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission);
                List<LookupValue> TempBusinessUnitList = new List<LookupValue>(BusinessUnitList.ToList());
                List<int> TempLookupListId = new List<int>() { 5, 4, 3 };
                TempBusinessUnitList = TempBusinessUnitList.OrderBy(Or => TempLookupListId.IndexOf(Or.IdLookupValue)).ToList();

                TotalSummary = new ObservableCollection<Summary>();
                CompanyPlantQuota.Sort((x, y) => x.Year.CompareTo(y.Year));
                CompanyPlantQuota.Reverse();

                //Add year wise bands Bands_FirstLevel_Year
                List<int> YearsList = new List<int>();

                foreach (var item in CompanyPlantQuota)
                {
                    if (!YearsList.Contains(item.Year) && item.Year != 0)
                    {
                        YearsList.Add(item.Year);
                    }
                }

                foreach (var item in YearsList)
                {
                    ParentBandItem ParentBandItem = new ParentBandItem()
                    {
                        Header = $"{item.ToString()}",
                        Name = $"Name{item.ToString()}",
                        HeaderToolTip = $"{item.ToString()}"
                    };
                    Bands_FirstLevel_Year.Add(ParentBandItem);
                }

                bool isBandExist = false;
                foreach (var item in CompanyPlantQuota)
                {
                    foreach (var itemBand in Bands)
                    {
                        if (itemBand.BandHeader == item.Year.ToString())
                        {
                            isBandExist = true;
                        }
                        else if (item.Year == 0)
                        {
                            isBandExist = true;
                        }
                        else if (itemBand.BandHeader != item.Year.ToString())
                        {
                            isBandExist = false;
                        }
                    }
                    if (isBandExist == false)
                    {
                        string WMSCurrencyName = GeosApplication.Instance.Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion).Name;
                        for (int i = 0; i < TempBusinessUnitList.Count; i++)
                        {
                            if (!DtPlantQuotaCopy.Columns.Contains((TempBusinessUnitList[i].Value + "-" + item.Year).ToString()))
                            {
                                BandItem bandBU = new BandItem() {
                                    BandName = TempBusinessUnitList[i].Value,
                                    BandHeader = TempBusinessUnitList[i].Value };
                                bandBU.Columns = new ObservableCollection<ColumnItem>();

                                string fieldName = TempBusinessUnitList[i].Value + "-" + item.Year.ToString();
                                DtPlantQuotaCopy.Columns.Add((TempBusinessUnitList[i].Value + "-" + item.Year).ToString(), typeof(double));
                                
                                bandBU.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = "Quota " + "("+ WMSCurrencyName + ")", Width = 120, Visible = true, IsVertical = false, Settings = SettingsType.Amount });
                                DtPlantQuotaCopy.Columns.Add((TempBusinessUnitList[i].Value + "-" + item.Year + "ConvertedAmount").ToString(), typeof(double));
                                DtPlantQuotaCopy.Columns.Add((TempBusinessUnitList[i].Value + "-" + item.Year + "ConvertedAmountWithSymbol").ToString(), typeof(string));

                                bandBU.Columns.Add(new ColumnItem() {Tag = fieldName + "ConvertedAmount",  ColumnFieldName = fieldName + "ConvertedAmountWithSymbol", HeaderText = "Quota", Width = 120, Visible = true, IsVertical = false, Settings = SettingsType.Amount });

                                //band2.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, HeaderText = TempBusinessUnitList[i].Value.ToString(), Width = 120, Visible = true, IsVertical = false, Settings = SettingsType.Amount });
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName, DisplayFormat = " {0:C}" });
                                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = fieldName + "ConvertedAmount", DisplayFormat = " {0:C}" });

                                if (item.Year != 0)
                                {
                                    //  Bands.Add(bandBU);
                                    var parentBandItem1 = Bands_FirstLevel_Year.FirstOrDefault(x => (x.Header.ToString() == item.Year.ToString()));

                                    if (parentBandItem1 != null)
                                    {
                                        parentBandItem1.Bands.Add(bandBU);
                                    }
                                }
                            }
                        }

                        BandItem bandExchangeRate = new BandItem()
                        {
                            BandName = "Band_ExchangeRate",
                            BandHeader = "Exchange Rate"
                        };

                        BandItem bandTotal = new BandItem()
                        {
                            BandName = "Band_Total",
                            BandHeader = "Total"
                        };

                        bandTotal.Columns = new ObservableCollection<ColumnItem>();
                        bandExchangeRate.Columns = new ObservableCollection<ColumnItem>();

                        if (!DtPlantQuotaCopy.Columns.Contains(("Total" + "-" + item.Year).ToString()))
                        {
                            string totalSummaryField = "Total" + "-" + item.Year.ToString();
                            DtPlantQuotaCopy.Columns.Add(totalSummaryField, typeof(double)).AllowDBNull = true;
                            bandTotal.Columns.Add(new ColumnItem() { ColumnFieldName = totalSummaryField, HeaderText = "Quota " + "(" + WMSCurrencyName + ")", Width = 120, IsVertical = false, Settings = SettingsType.Amount, Visible = true });

                            string totalSummaryFieldConvertedAmount = "Total" + "-" + item.Year.ToString() + "ConvertedAmount";
                            DtPlantQuotaCopy.Columns.Add(totalSummaryFieldConvertedAmount, typeof(double)).AllowDBNull = true;

                            DtPlantQuotaCopy.Columns.Add(totalSummaryFieldConvertedAmount + "WithSymbol", typeof(string)).AllowDBNull = true;

                            bandTotal.Columns.Add(new ColumnItem() { ColumnFieldName = totalSummaryFieldConvertedAmount+ "WithSymbol",
                                HeaderText = "Quota", Width = 120, IsVertical = false, Settings = SettingsType.Amount,
                                Visible = true, Tag = totalSummaryFieldConvertedAmount});

                            TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = totalSummaryField, DisplayFormat = " {0:C}" });
                        }

                        if (!DtPlantQuotaCopy.Columns.Contains(("ExchangeRateDate" + "-" + item.Year).ToString()))
                        {
                            string ExchangeRateDateField = "ExchangeRateDate" + "-" + item.Year.ToString();
                            DtPlantQuotaCopy.Columns.Add(ExchangeRateDateField, typeof(DateTime)).AllowDBNull = true;
                            bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateDateField, HeaderText = "Date", Width = 100, IsVertical = false, Settings = SettingsType.CloseDate, Visible = true });
                            //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = ExchangeRateField, DisplayFormat = " {0:C}" });
                        }

                        if (!DtPlantQuotaCopy.Columns.Contains(("ExchangeRate" + "-" + item.Year).ToString()))
                        {
                            string ExchangeRateField = "ExchangeRate" + "-" + item.Year.ToString();
                            DtPlantQuotaCopy.Columns.Add(ExchangeRateField, typeof(double)).AllowDBNull = true;
                            bandExchangeRate.Columns.Add(new ColumnItem() { ColumnFieldName = ExchangeRateField, HeaderText = "Rate", Width = 80, IsVertical = false, Settings = SettingsType.Amount, Visible = true });
                            //TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = ExchangeRateField, DisplayFormat = " {0:C}" });
                        }

                        // Bands.Add(band2);
                        if (bandTotal.Columns.Count > 0)
                        {
                            if (item.Year != 0)
                            {
                                var parentBandItem1 = Bands_FirstLevel_Year.FirstOrDefault(
                                x => (x.Header.ToString() == item.Year.ToString()));

                                if (parentBandItem1 != null)
                                {
                                    parentBandItem1.Bands.Add(bandTotal);
                                }
                            }
                        }

                        if (bandExchangeRate.Columns.Count > 0)
                        {
                            if (item.Year != 0)
                            {
                                var parentBandItem1 = Bands_FirstLevel_Year.First(
                                x => (x.Header.ToString() == item.Year.ToString()));

                                if (parentBandItem1 != null)
                                {
                                    parentBandItem1.Bands.Add(bandExchangeRate);
                                }
                            }
                        }
                        
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDashboard()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                bool found = false;
                int rowCounter = 0;
                double? totalsum = null;
                double totalsumConvertedAmount = 0;

                CompanyPlantQuota.Sort((x, y) => x.IdPlant.CompareTo(y.IdPlant));
                foreach (PlantBusinessUnitSalesQuota item in CompanyPlantQuota)
                {
                    if (DtPlantQuotaCopy.Rows.Count == 0)
                    {
                        DataRow dr = DtPlantQuotaCopy.NewRow();
                        dr["PlantName"] = item.ShortName.ToString();
                        dr["Region"] = item.Region.ToString();
                        dr["IdPlant"] = item.IdPlant;
                        dr["IdCurrency"] = item.IdSalesQuotaCurrency;

                        if (item.ExchangeRateDate.Equals(DBNull.Value) || item.ExchangeRateDate == null)
                            dr["ExchangeRateDate" + "-" + item.Year.ToString()] = DBNull.Value;
                        else
                            dr["ExchangeRateDate" + "-" + item.Year.ToString()] = item.ExchangeRateDate;

                        if (item.CurrencyConversionRate.Equals(DBNull.Value))
                            dr["ExchangeRate" + "-" + item.Year.ToString()] = DBNull.Value;
                        else
                            dr["ExchangeRate" + "-" + item.Year.ToString()] = item.CurrencyConversionRate;

                        if (item.LookupValue != null)
                        {
                            foreach (var item1 in item.LookupValue)
                            {
                                string colName = item1.Value + "-" + item.Year.ToString();
                                dr[colName] = item1.SalesQuotaAmount; // ConvertedAmount;//SalesQuotaAmount;
                                totalsum = (totalsum != null ? totalsum : 0) + Convert.ToDouble(item1.SalesQuotaAmount);

                                totalsumConvertedAmount += Convert.ToDouble(item1.ConvertedAmount);

                                string colNameConvertedAmount = item1.Value + "-" + item.Year.ToString() + "ConvertedAmount";
                                dr[colNameConvertedAmount] = item1.ConvertedAmount;

                                dr[colNameConvertedAmount + "WithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                            i => i.IdCurrency == item1.IdSalesQuotaCurrency).Symbol 
                                                                            + " "+ item1.ConvertedAmount.ToString("N2");
                            }
                            string totalColumn = "Total" + "-" + item.Year.ToString();
                            dr[totalColumn] = totalsum != null ? totalsum : (object)DBNull.Value;

                            dr[totalColumn + "ConvertedAmount"] = totalsumConvertedAmount;
                           
                            dr[totalColumn + "ConvertedAmountWithSymbol"] =
                                                                         GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                            i => i.IdCurrency == item.IdSalesQuotaCurrency).Symbol + " "
                                                                + totalsumConvertedAmount.ToString("N2");

                        }

                        DtPlantQuotaCopy.Rows.Add(dr);
                        rowCounter += 1;
                    }
                    else
                    {
                        if (DtPlantQuotaCopy.Rows[rowCounter - 1]["IdPlant"].ToString() == item.IdPlant.ToString())
                        {
                           // DataRow dr = DtPlantQuotaCopy.NewRow();
                            if (item.ExchangeRateDate.Equals(DBNull.Value) || item.ExchangeRateDate == null)
                                DtPlantQuotaCopy.Rows[rowCounter - 1]["ExchangeRateDate" + "-" + item.Year.ToString()] = DBNull.Value;
                            else
                                DtPlantQuotaCopy.Rows[rowCounter - 1]["ExchangeRateDate" + "-" + item.Year.ToString()] = item.ExchangeRateDate;

                            if (item.CurrencyConversionRate.Equals(DBNull.Value))
                                DtPlantQuotaCopy.Rows[rowCounter - 1]["ExchangeRate" + "-" + item.Year.ToString()] = DBNull.Value;
                            else
                                DtPlantQuotaCopy.Rows[rowCounter - 1]["ExchangeRate" + "-" + item.Year.ToString()] = item.CurrencyConversionRate;

                            double? totalSum = null;
                            double totalSumConvertedAmount = 0;
                            if (item.LookupValue != null)
                            {
                                foreach (var item1 in item.LookupValue)
                                {
                                    string colName = item1.Value + "-" + item.Year.ToString();
                                    DtPlantQuotaCopy.Rows[rowCounter - 1][colName] = item1.SalesQuotaAmount; //ConvertedAmount;//SalesQuotaAmount;

                                    totalSum = (totalSum != null ? totalSum : 0) + Convert.ToDouble(item1.SalesQuotaAmount);
                                    totalSumConvertedAmount += Convert.ToDouble(item1.ConvertedAmount);

                                    string colNameConvertedAmount = item1.Value + "-" + item.Year.ToString() + "ConvertedAmount";
                                    DtPlantQuotaCopy.Rows[rowCounter - 1][colNameConvertedAmount] = item1.ConvertedAmount;//ConvertedAmount;

                                    DtPlantQuotaCopy.Rows[rowCounter - 1][colNameConvertedAmount + "WithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                                                                    i => i.IdCurrency == item1.IdSalesQuotaCurrency).Symbol
                                                                                                                    + " " + item1.ConvertedAmount.ToString("N2");
                                }

                                string totalColumn = "Total" + "-" + item.Year.ToString();
                                DtPlantQuotaCopy.Rows[rowCounter - 1][totalColumn] = totalSum != null ? totalSum : (object)DBNull.Value;

                                DtPlantQuotaCopy.Rows[rowCounter - 1][totalColumn + "ConvertedAmount"] = totalSumConvertedAmount;
                                DtPlantQuotaCopy.Rows[rowCounter - 1][totalColumn + "ConvertedAmountWithSymbol"] =
                                                                                 GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                                    i => i.IdCurrency == item.IdSalesQuotaCurrency).Symbol + " "
                                                                        + totalSumConvertedAmount.ToString("N2");
                            }
                        }
                        else
                        {
                            DataRow dr = DtPlantQuotaCopy.NewRow();
                            dr["PlantName"] = item.ShortName.ToString();
                            dr["Region"] = item.Region.ToString();
                            dr["IdPlant"] = item.IdPlant;
                            dr["IdCurrency"] = item.IdSalesQuotaCurrency;

                            if (item.ExchangeRateDate.Equals(DBNull.Value) || item.ExchangeRateDate ==null)
                                dr["ExchangeRateDate" + "-" + item.Year.ToString()] = DBNull.Value;
                            else
                                dr["ExchangeRateDate" + "-" + item.Year.ToString()] = item.ExchangeRateDate;

                            if (item.CurrencyConversionRate.Equals(DBNull.Value))
                                dr["ExchangeRate" + "-" + item.Year.ToString()] = DBNull.Value;
                            else
                                dr["ExchangeRate" + "-" + item.Year.ToString()] = item.CurrencyConversionRate;

                            double? total = null;
                            double totalConvertedAmount = 0;
                            if (item.LookupValue != null)
                            {
                                foreach (var item1 in item.LookupValue)
                                {
                                    string colName = item1.Value + "-" + item.Year.ToString();
                                    dr[colName] = item1.SalesQuotaAmount; //ConvertedAmount;//SalesQuotaAmount;

                                    total = (total != null ? total : 0) + Convert.ToDouble(item1.SalesQuotaAmount);
                                    totalConvertedAmount += Convert.ToDouble(item1.ConvertedAmount);

                                    string colNameConvertedAmount = item1.Value + "-" + item.Year.ToString() + "ConvertedAmount";
                                    dr[colNameConvertedAmount] = item1.ConvertedAmount;//ConvertedAmount;

                                    dr[colNameConvertedAmount + "WithSymbol"] = GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                                    i => i.IdCurrency == item1.IdSalesQuotaCurrency).Symbol
                                                                                    + " " + item1.ConvertedAmount.ToString("N2");
                                }

                                string totalColumn = "Total" + "-" + item.Year.ToString();
                                dr[totalColumn] = total != null ? total : (object)DBNull.Value;

                                dr[totalColumn + "ConvertedAmount"] = totalConvertedAmount;
                                dr[totalColumn + "ConvertedAmountWithSymbol"] =
                                                                             GeosApplication.Instance.Currencies.FirstOrDefault(
                                                                            i => i.IdCurrency == item.IdSalesQuotaCurrency).Symbol + " "
                                                                            + totalConvertedAmount.ToString("N2");
                            }
                            DtPlantQuotaCopy.Rows.Add(dr);
                            rowCounter += 1;
                        }
                    }
                }

                DtPlantQuota = DtPlantQuotaCopy;

                GeosApplication.Instance.Logger.Log("Method FillDashboard() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPlantQuotaGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPlantQuotaGrid ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintPlantQuotaGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintPlantQuotaGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// To edit Plant Quota
        /// </summary>
        /// <param name="obj"></param>
        //[M050-05]
        private void EditPlantQuotaViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditPlantQuotaViewWindowShow ...", category: Category.Info, priority: Priority.Low);
            string shortName = string.Empty;

            try
            {
                if (GeosApplication.Instance.IsPermissionAdminOnly)
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }
                    EditPlantQuotaViewModel editPlantQuotaViewModel = new EditPlantQuotaViewModel();

                    string plant = ((System.Data.DataRowView)obj).Row.ItemArray[0].ToString();
                    int plantId = Convert.ToInt32(((System.Data.DataRowView)obj).Row.ItemArray[2].ToString());//1

                    List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = new List<PlantBusinessUnitSalesQuota>(crmControl.GetPlantQuotaDetailsById(GeosApplication.Instance.ActiveUser.IdUser,
                           GeosApplication.Instance.IdCurrencyByRegion,
                           GeosApplication.Instance.SelectedyearStarDate,
                           GeosApplication.Instance.SelectedyearEndDate,
                           GeosApplication.Instance.IdUserPermission, plantId).ToList());
                    editPlantQuotaViewModel.PlantId = plantId;
                    editPlantQuotaViewModel.PlantBusinessUnitSalesQuota = new ObservableCollection<PlantBusinessUnitSalesQuota>(plantBusinessUnitSalesQuota as List<PlantBusinessUnitSalesQuota>);

                    editPlantQuotaViewModel.InIt();
                    EditPlantQuotaView editPlantQuotaView = new EditPlantQuotaView();
                    EventHandler handle = delegate { editPlantQuotaView.Close(); };
                    editPlantQuotaViewModel.RequestClose += handle;
                    editPlantQuotaViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditPlantQuotsViewTitle").ToString() + " " + plant;
                    editPlantQuotaView.DataContext = editPlantQuotaViewModel;
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    editPlantQuotaView.ShowDialogWindow();
                    if(editPlantQuotaViewModel.IsUpdated)
                    RefreshPlantQuota(new object());
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }





            //try
            //{
            //    if (GeosApplication.Instance.IsPermissionAdminOnly)
            //    {
            //        if ((System.Data.DataRowView)obj != null)
            //        {
            //            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }

            //            string plant = ((System.Data.DataRowView)obj).Row.ItemArray[0].ToString();
            //            int plantId = Convert.ToInt32(((System.Data.DataRowView)obj).Row.ItemArray[1].ToString());

            //            DtPlantQuotaCopy = DtPlantQuota.Copy();
            //            DtPlantQuotaCopy.Rows.Clear();
            //            DtPlantQuotaCopy.ImportRow(((System.Data.DataRowView)obj).Row);
            //            DtPlantQuotaEdit = DtPlantQuotaCopy;

            //            EditPlantQuotaViewModel editPlantQuotaViewModel = new EditPlantQuotaViewModel();
            //            EditPlantQuotaView editPlantQuotaView = new EditPlantQuotaView();
            //            EventHandler handle = delegate { editPlantQuotaView.Close(); };
            //            editPlantQuotaViewModel.RequestClose += handle;
            //            editPlantQuotaViewModel.InIt(DtPlantQuotaEdit);
            //            editPlantQuotaView.DataContext = editPlantQuotaViewModel;

            //            IsBusy = false;
            //            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            //            editPlantQuotaView.ShowDialogWindow();

            //            if (editPlantQuotaViewModel.isUpdated)
            //            {
            //                DataRow dataRow = DtPlantQuota.AsEnumerable().FirstOrDefault(row => Convert.ToInt32(row["IdPlant"]) == Convert.ToInt32(editPlantQuotaViewModel.DtPlant.Rows[0]["IdPlant"]));
            //                if (dataRow != null)
            //                {
            //                    long totalBusinessUnitsum = 0;
            //                    foreach (LookupValue businessUnit in BusinessUnitList)
            //                    {
            //                        dataRow[businessUnit.Value.ToString()] = editPlantQuotaViewModel.DtPlant.Rows[0][businessUnit.Value.ToString()];
            //                        totalBusinessUnitsum = totalBusinessUnitsum + Convert.ToInt64(editPlantQuotaViewModel.DtPlant.Rows[0][businessUnit.Value.ToString()]);
            //                    }
            //                    dataRow["Total"] = totalBusinessUnitsum;
            //                }
            //            }
            //        }
            //    }
            //    GeosApplication.Instance.Logger.Log("Method EditPlantQuotaViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            //}
            //catch (Exception ex)
            //{
            //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
        }
        #endregion // Methods
    }
}
