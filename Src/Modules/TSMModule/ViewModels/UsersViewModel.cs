using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.ComponentModel;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.TSM.Views;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Emdep.Geos.UI.Helper;
using System.Data;
using Emdep.Geos.Data.Common.TSM;
using DevExpress.Data;
using System.Windows.Controls;
namespace Emdep.Geos.Modules.TSM.ViewModels
{
    public class UsersViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        // [GEOS2-5388][pallavi.kale][13.01.2025]
		//[nsatpute][GEOS2-5388][30-01-2025]
        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ITSMService TSMService = new TSMServiceController("localhost:6699");
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Declaration
        private DataTable localDataTable;
        private ObservableCollection<BandItem> bands;
        private BandItem bandLookupKey;
        private DataTable dataTable;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        bool isBusy;
        DataTable dataTableForGridLayoutCopy;
        public bool isUsersColumnChooserVisible;
        private List<TSMUsers> engineeringCustomerApplicationList;
        private List<LookupValue> usersLookUpValuesList;
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
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        public BandItem BandLookupKey
        {
            get { return bandLookupKey; }
            set
            {
                bandLookupKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandLookupKey"));
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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        public List<TSMUsers> EngineeringCustomerApplicationList
        {
            get { return engineeringCustomerApplicationList; }
            set
            {
                engineeringCustomerApplicationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngineeringCustomerApplicationList"));
            }
        }
        public List<LookupValue> UsersLookUpValuesList
        {
            get { return usersLookUpValuesList; }
            set
            {
                usersLookUpValuesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsersLookUpValuesList"));
            }
        }
        #endregion
        #region Public Commands
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditUserGridDoubleClickCommand { get; set; }
        #endregion
        #region Constructor        
        public UsersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UsersViewModel ...", category: Category.Info, priority: Priority.Low);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
                EditUserGridDoubleClickCommand = new DelegateCommand<object>(UserEditViewWindowShow);
                GeosApplication.Instance.Logger.Log("Constructor UsersViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UsersViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Methods 
        private void UserEditViewWindowShow(object obj)
        {
            try
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 141))
                {
                    GeosApplication.Instance.Logger.Log("Method UserEditViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                    TableView detailView = (TableView)obj;
                    if (detailView.FocusedRow is DataRowView drView)
                    {
                        long idTechnicianUser = Convert.ToUInt32(drView["IdTechnicianUser"]);
                        TSMUsers user = EngineeringCustomerApplicationList.FirstOrDefault(x => x.IdTechnicianUser == idTechnicianUser);
                        EditUserView editUserView = new EditUserView();
                        EditUserViewModel editUserViewModel = new EditUserViewModel();
                        editUserView.DataContext = editUserViewModel;
                        EventHandler handle = delegate { editUserView.Close(); };
                        editUserViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditUserHeader").ToString();
                        editUserViewModel.IsNew = false;
                        editUserViewModel.RequestClose += handle;
                        editUserViewModel.EmployeeCodeWithIdGender = user.EmployeeCodeWithIdGender;
                        editUserViewModel.EditInit((TSMUsers)user.Clone(), UsersLookUpValuesList);
                        editUserView.ShowDialog();
                        if (editUserViewModel.IsSave || editUserViewModel.IsUpdateUser)
                        {
                            EngineeringCustomerApplicationList.FirstOrDefault(x => x.IdTechnicianUser == idTechnicianUser).IdPermissions = editUserViewModel.EditUser.IdPermissions;
                            foreach (LookupValue permission in UsersLookUpValuesList)
                            {
                                if (user.IdPermissions != null && user.IdPermissions.Contains(permission.IdLookupValue))
                                    drView[permission.Value] = "X";
                                else
                                    drView[permission.Value] = string.Empty;
                            }
                            GridControl gridControl = detailView.Grid;
                            gridControl.ItemsSource = DataTable;
                            gridControl.RefreshData();
                            gridControl.UpdateLayout();
                        }
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Error: Focused row is not a valid DataRowView.", category: Category.Exception, priority: Priority.Low);
                    }
                    GeosApplication.Instance.Logger.Log("Method UserEditViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method UserEditViewWindowShow()... {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Users List";
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
                    activityTableView.ShowTotalSummary = true;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["UsersViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["UsersViewCustomPrintFooterTemplate"];
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
                Init();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                BandLookupKey = new BandItem() { BandName = "EngineeringCustomerApplication", BandHeader = "" };
                FillUsersLookUpValuesList();
                FillengineeringCustomerApplicationList_V2610();
                AddColumnsToDataTable();
                FillDataTable();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
        private static string GetResource(string resourceKey)
        {
            return Convert.ToString(Application.Current.FindResource(resourceKey));
        }
        private void AddColumnsToDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);
                List<BandItem> bandsLocal = new List<BandItem>();
                BandItem bandAll = new BandItem() { BandName = "All", BandHeader = "", FixedStyle = DevExpress.Xpf.Grid.FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                //bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Photo", HeaderText = GetResource("TSMUsersPhotoHeader"), Width = 70, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Image, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "FullName", HeaderText = GetResource("TSMUsersFullNameHeader"), Width = 280, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Tooltip, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Organization", HeaderText = GetResource("TSMUsersOrganizationHeader"), Width = 160, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Country", HeaderText = GetResource("TSMUsersCountryHeader"), Width = 160, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Country, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Region", HeaderText = GetResource("TSMUsersRegionHeader"), Width = 160, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "Username", HeaderText = GetResource("TSMUsersUsernameHeader"), Width = 200, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Default, Visible = true });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdTechnicianUser", HeaderText = "IdTechnicianUser", Width = 0, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Default, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "EmployeeCodeWithIdGender", HeaderText = "EmployeeCodeWithIdGender", Width = 0, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Default, Visible = false });
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "CountryURL", HeaderText = GetResource("TSMUsersCountryHeader"), Width = 0, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.Country, Visible = false });
                bandsLocal.Add(bandAll);
                localDataTable = new DataTable();
                localDataTable.Columns.Add("IdTechnicianUser", typeof(string));
                localDataTable.Columns.Add("FullName", typeof(string));
                localDataTable.Columns.Add("Organization", typeof(string));
                localDataTable.Columns.Add("Country", typeof(string));
                localDataTable.Columns.Add("Region", typeof(string));
                localDataTable.Columns.Add("Username", typeof(string));
                localDataTable.Columns.Add("Photo", typeof(byte[]));
                localDataTable.Columns.Add("CountryURL", typeof(string));
                localDataTable.Columns.Add("EmployeeCodeWithIdGender", typeof(string));
                bandsLocal.Add(BandLookupKey);
                BandLookupKey.Columns = new ObservableCollection<ColumnItem>();
                if (UsersLookUpValuesList != null && UsersLookUpValuesList.Count > 0)
                {
                    foreach (LookupValue item in UsersLookUpValuesList)
                    {
                        if (!string.IsNullOrEmpty(item.Abbreviation) && !BandLookupKey.Columns.Any(x => x.ColumnFieldName == item.Value.ToString()))
                        {
                            string fieldName = item.Value.ToString();
                            localDataTable.Columns.Add(fieldName, typeof(string));
                            BandLookupKey.Columns.Add(new ColumnItem() { ColumnFieldName = fieldName, ToolTipValue = item.Value , HeaderText = item.Abbreviation, Width = 110, Visible = true, IsVertical = false, TSMUsersSettings = TSMUsersSettingsType.DynamicColumns });
                        }
                    }
                }
                BandLookupKey.Columns = new ObservableCollection<ColumnItem>(BandLookupKey.Columns.OrderBy(x => x.HeaderText));
                Bands = new ObservableCollection<BandItem>(bandsLocal);
                TotalSummary = new ObservableCollection<Summary>() { new Summary() { Type = SummaryItemType.Count, FieldName = "FullName", DisplayFormat = "Total : {0}" } };
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
                DataTableForGridLayoutCopy = localDataTable.Copy();
                foreach (TSMUsers users in EngineeringCustomerApplicationList)
                {
                    DataRow dr = DataTableForGridLayoutCopy.NewRow();
                    dr["IdTechnicianUser"] = users.IdTechnicianUser;
                    dr["FullName"] = users.FullName;
                    dr["Organization"] = users.Organization;
                    dr["Country"] = users.CountryObj.Name;
                    dr["Region"] = users.Region;
                    dr["Username"] = users.UserName;
                    dr["CountryURL"] = users.CountryObj.CountryIconUrl;
                    //dr["Photo"] = users.EmployeeCodeWithIdGender;
                    dr["EmployeeCodeWithIdGender"] = users.EmployeeCodeWithIdGender;
                    if (UsersLookUpValuesList != null)
                    {
                        foreach (LookupValue permission in UsersLookUpValuesList)
                        {
                            if (users.IdPermissions != null && users.IdPermissions.Contains(permission.IdLookupValue))
                            {
                                dr[permission.Value.ToString()] = "X";
                            }
                        }
                    }
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
        private void FillengineeringCustomerApplicationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillengineeringCustomerApplicationList ...", category: Category.Info, priority: Priority.Low);
                EngineeringCustomerApplicationList = new List<TSMUsers>(TSMService.GetUserDetailsList_V2600());
                GeosApplication.Instance.Logger.Log("Method FillengineeringCustomerApplicationList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillUsersLookUpValuesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUsersLookUpValuesList ...", category: Category.Info, priority: Priority.Low);
                UsersLookUpValuesList = new List<LookupValue>(TSMService.GetLookupValues(171));
                GeosApplication.Instance.Logger.Log("Method FillUsersLookUpValuesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsersLookUpValuesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsersLookUpValuesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillUsersLookUpValuesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-6993][pallavi.kale][26.02.2025]
        private void FillengineeringCustomerApplicationList_V2610()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillengineeringCustomerApplicationList ...", category: Category.Info, priority: Priority.Low);
                EngineeringCustomerApplicationList = new List<TSMUsers>(TSMService.GetUserDetailsList_V2610());
                GeosApplication.Instance.Logger.Log("Method FillengineeringCustomerApplicationList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillengineeringCustomerApplicationList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}