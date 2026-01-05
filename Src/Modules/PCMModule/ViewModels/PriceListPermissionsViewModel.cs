using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PCM.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class PriceListPermissionsViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        // new public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declaration

        string fromDate;
        string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        DateTime startDate;
        DateTime endDate;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";
        private bool isBusy;
        //string allPermission = "R" + System.Environment.NewLine + "W";
        List<UserPermissionByBPLPriceList> userPermissionByBPLPriceList;
        List<UserPermissionByCPLPriceList> userPermissionByCPLPriceList;
        List<BasePrice> basePriceList;
        List<CustomerPrice> customerPriceList;
        List<Users> usersList;
        Permissions selectAllPriceList;
        public List<string> PermissionsList { get; set; }
        public List<Permissions> ALLPriceListPermissionsList { get; set; }
        public IList<LookupValue> LookUpValueList { get; set; }

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private DataTable dttable;
        private DataTable dttableCopy;
        TableView view;
        bool isUserPermissionsave = false;
        bool isAllSave = false;

        #endregion

        #region Properties

        public string FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }
        public string ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }
        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
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
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
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

        public Permissions SelectAllPriceList
        {
            get { return selectAllPriceList; }
            set { selectAllPriceList = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectAllPriceList")); }
        }
        public List<Users> UsersList
        {
            get { return usersList; }
            set { usersList = value; OnPropertyChanged(new PropertyChangedEventArgs("UsersList")); }
        }
        public List<UserPermissionByBPLPriceList> UserPermissionByBPLPriceList
        {
            get { return userPermissionByBPLPriceList; }
            set { userPermissionByBPLPriceList = value; OnPropertyChanged(new PropertyChangedEventArgs("UserPermissionByBPLPriceList")); }
        }
        public List<UserPermissionByCPLPriceList> UserPermissionByCPLPriceList
        {
            get { return userPermissionByCPLPriceList; }
            set { userPermissionByCPLPriceList = value; OnPropertyChanged(new PropertyChangedEventArgs("UserPermissionByCPLPriceList")); }
        }
        public List<BasePrice> BasePricesList
        {
            get { return basePriceList; }
            set { basePriceList = value; OnPropertyChanged(new PropertyChangedEventArgs("BasePricesList")); }
        }
        public List<CustomerPrice> CustomerPriceList
        {
            get { return customerPriceList; }
            set { customerPriceList = value; OnPropertyChanged(new PropertyChangedEventArgs("CustomerPriceList")); }
        }

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        #endregion

        #region Constructor

        public PriceListPermissionsViewModel()
        {
            PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
            PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
            ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
            CancelCommand = new DelegateCommand<object>(CancelCommandAction);

            PrintButtonCommand = new RelayCommand(new Action<object>(PrintPriceListPermissionsButtonCommandAction));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportPriceListPermissionsButtonCommandAction));
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshPriceListPermissionsList));
            UpdateMultipleRowsPriceListPermissionsCommand = new RelayCommand(new Action<object>(InsertUpdateMultipleRowsPriceListPermissions));
            RuleValueChangedCommand = new DelegateCommand<object>(RuleValueChangedCommandAction);
            DeleteAppointmentCommand = new DelegateCommand<object>(DeleteAppointment);
        }

        #endregion

        #region Public ICommands

        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand UpdateMultipleRowsPriceListPermissionsCommand { get; set; }
        public ICommand RuleValueChangedCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }

        #endregion

        #region Methods

        public async void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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

                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsCalendarVisible = Visibility.Collapsed;

                FillPermissionsList();
                SetDefaultPeriod();
                AddDataTableColumns();
                await Task.Run(() => FillDataTable());

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Collapsed;
        }

        void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }

        void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction()...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        void PrintPriceListPermissionsButtonCommandAction(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method PrintPriceListPermissionsButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.Logger.Log("Method PrintPriceListPermissionsButtonCommandAction()... executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintPriceListPermissionsButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void ExportPriceListPermissionsButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPriceListPermissionsButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Price List Permissions";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (DialogResult == true)
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
                    TableView tableView = ((TableView)obj);
                    tableView.ShowTotalSummary = false;
                    tableView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Split all merged cells in the worksheet. 
                    foreach (var item in worksheet.Cells.GetMergedRanges())
                    {
                        item.UnMerge();
                    }

                    // Rotate.
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;
                    int countTechCol = 0;

                    foreach (GridColumn gridColumn in tableView.VisibleColumns)
                    {
                        if (gridColumn.FieldName != "Name")
                        {
                            string text = worksheetColumns[countTechCol].Heading + 1;

                            Cell cell = worksheet.Cells[text];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            cell.Alignment.RotationAngle = 90;
                            cell.RowHeight = 500;
                            cell.Alignment.WrapText = true;
                        }
                        countTechCol++;
                    }
                    control.SaveDocument();

                    IsBusy = false;
                    tableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportPriceListPermissionsButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                    ResultFileName = string.Empty;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPriceListPermissionsButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void RefreshPriceListPermissionsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPriceListPermissionsList()...", category: Category.Info, priority: Priority.Low);

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

                view = PriceListPermissionMultipleCellEditHelper.Viewtableview;
                if (PriceListPermissionMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        InsertUpdateMultipleRowsPriceListPermissions(PriceListPermissionMultipleCellEditHelper.Viewtableview);
                    }
                    PriceListPermissionMultipleCellEditHelper.IsValueChanged = false;
                }

                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsCalendarVisible = Visibility.Collapsed;

                FillPermissionsList();
                SetDefaultPeriod();
                AddDataTableColumns();
                FillDataTable();

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshPriceListPermissionsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPriceListPermissionsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InsertUpdateMultipleRowsPriceListPermissions(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InsertUpdateMultipleRowsPriceListPermissions()...", category: Category.Info, priority: Priority.Low);

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
                UInt64 idBasePrice;
                UInt64 idCustomerPrice;
                uint idActiveUser;
                string permission = string.Empty;

                DataRow[] foundRow = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();
                foreach (DataRow item in foundRow)
                {
                    int idUser = (int)item.ItemArray[0];

                    if (item.ItemArray[3].ToString() == string.Empty)
                    {
                        PLMService.DeleteUserPermissionByBPLForParticularColumn(idUser, 0);
                        PLMService.DeleteUserPermissionByCPLForParticularColumn(idUser, 0);

                        for (int i = 4; i < item.Table.Columns.Count; i++)
                        {
                            idBasePrice = Convert.ToUInt64(item.Table.Columns[i + 1].ToString().Remove(0, 4));
                            idCustomerPrice = Convert.ToUInt64(item.Table.Columns[i + 1].ToString().Remove(0, 4));
                            idActiveUser = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            permission = item.ItemArray[i].ToString();

                            if (item.Table.Columns[i].ToString().Substring(0, 3) == "BPL")
                            {
                                if (string.IsNullOrEmpty(permission) == true)
                                {
                                    PLMService.DeleteUserPermissionByBPLForParticularColumn(idUser, idBasePrice);
                                    isUserPermissionsave = true;
                                }
                                else
                                    SaveBPL(idBasePrice, idUser, idActiveUser, permission);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(permission) == true)
                                {
                                    PLMService.DeleteUserPermissionByCPLForParticularColumn(idUser, idBasePrice);
                                    isUserPermissionsave = true;
                                }
                                else
                                    SaveCPL(idCustomerPrice, idUser, idActiveUser, permission);
                            }
                            if (isUserPermissionsave)
                                isAllSave = true;
                            else
                                isAllSave = false;

                            i = i + 1;
                        }
                    }
                    else
                    {
                        idBasePrice = idCustomerPrice = 0;
                        permission = item.ItemArray[3].ToString();
                        idActiveUser = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        PLMService.DeleteUserPermissionByForParticularUser((int)idUser);

                        SaveBPL(idBasePrice, idUser, idActiveUser, permission);
                        SaveCPL(idCustomerPrice, idUser, idActiveUser, permission);

                        if (isUserPermissionsave)
                            isAllSave = true;
                        else
                            isAllSave = false;
                    }
                }

                if (isAllSave)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PriceListPermissionsMsgInsertUpdateSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    FillDataTable();
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PriceListPermissionsMsgInsertUpdateFail").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                PriceListPermissionMultipleCellEditHelper.SetIsValueChanged(view, false);
                PriceListPermissionMultipleCellEditHelper.IsValueChanged = isUserPermissionsave = false;

                GeosApplication.Instance.Logger.Log("Constructor InsertUpdateMultipleRowsPriceListPermissions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method InsertUpdateMultipleRowsPriceListPermissions()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void RuleValueChangedCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;

                GeosApplication.Instance.Logger.Log("Method RuleValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Column.FieldName == "ALLPriceLists")
                {
                    DataRowView RowValue = (DataRowView)((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Row;
                    if (RowValue == null)
                        return;

                    DataRow[] foundRow = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();
                    foreach (DataRow item in foundRow)
                    {
                        if (RowValue.Row.ItemArray[0].ToString() == item.ItemArray[0].ToString())
                        {
                            DataRow dr = item;
                            int index = Dttable.Rows.IndexOf(item);

                            for (int j = 4; j < item.Table.Columns.Count; j = j + 2)
                            {
                                dr[j] = item.ItemArray[3];
                            }

                            Dttable.Rows[index].ItemArray = dr.ItemArray;
                        }
                    }
                }
                else if (((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Column.FieldName.Substring(0, 3) == "BPL"
                     || ((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Column.FieldName.Substring(0, 3) == "CPL")
                {
                    DataRow[] foundRow = Dttable.AsEnumerable().Where(row => Convert.ToBoolean(row["IsChecked"]) == true).ToArray();

                    foreach (DataRow item in foundRow)
                    {
                        DataRowView RowValue = (DataRowView)((DevExpress.Xpf.Grid.CellValueEventArgs)obj).Row;
                        if (RowValue == null)
                            return;

                        if (RowValue.Row.ItemArray[0].ToString() == item.ItemArray[0].ToString())
                        {
                            DataRow dr = item;
                            int index = Dttable.Rows.IndexOf(item);
                            bool allPricListSameflag = true;
                            for (int j = 6; j < item.Table.Columns.Count; j = j + 2)
                            {
                                if (item.ItemArray[4].ToString() != item.ItemArray[j].ToString())
                                {
                                    allPricListSameflag = false;
                                    break;
                                }
                            }

                            if (allPricListSameflag == false)
                                dr[3] = string.Empty;
                            else
                                dr[3] = dr[4];

                            Dttable.Rows[index].ItemArray = dr.ItemArray;
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method RuleValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RuleValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void DeleteAppointment(object e)
        {
            //if (e is KeyEventArgs)
            //{
            //    KeyEventArgs obj = e as KeyEventArgs;

            //    if (obj.Source is SchedulerControlEx)
            //    {
            //        if (obj.Key == Key.Delete)
            //        {
            //            SchedulerControlEx schedule = (SchedulerControlEx)obj.Source;
            //            if (schedule.SelectedAppointments != null)
            //            {
            //                if (schedule.SelectedAppointments.Count > 0)
            //                {
            //                    if (schedule.SelectedAppointments[0].CustomFields["IdEmployeeAttendance"] != null)
            //                    {
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        void FillPermissionsList()
        {
            ALLPriceListPermissionsList = new List<Permissions>();
            PermissionsList = new List<string>();

            PermissionsList.Add("");
            Permissions permissions = new Permissions();
            permissions.PermissionName = "";
            ALLPriceListPermissionsList.Add(permissions);

            LookUpValueList = CrmStartUp.GetLookupValues(92);

            foreach (var LookupValue in LookUpValueList)
            {
                permissions = new Permissions();
                permissions.PermissionName = LookupValue.Abbreviation;
                ALLPriceListPermissionsList.Add(permissions);
                PermissionsList.Add(LookupValue.Abbreviation);
            }
        }

        void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                        new Emdep.Geos.UI.Helper.Column() { FieldName="IdUser",HeaderText="IdUser", Settings = SettingsType.Hidden, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="Name",HeaderText="Name", Settings = SettingsType.Name, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true  },
                        new Emdep.Geos.UI.Helper.Column() { FieldName="ALLPriceLists",HeaderText="ALL Price Lists", Settings = SettingsType.ALLPriceLists, AllowCellMerge=false,Width=50,AllowEditing=true,Visible=true,IsVertical= true,FixedWidth=true },
                        new Emdep.Geos.UI.Helper.Column() {FieldName="IsChecked",HeaderText="IsChecked", Settings = SettingsType.IsChecked, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true  },
                };

                Dttable = new DataTable();

                Dttable.Columns.Add("IdUser", typeof(Int32));
                Dttable.Columns.Add("IsChecked", typeof(bool));
                Dttable.Columns.Add("Name", typeof(string));
                Dttable.Columns.Add("ALLPriceLists", typeof(string));

                BasePricesList = PLMService.GetBasePriceListByDates(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)).ToList();
                for (int i = 0; i < BasePricesList.Count; i++)
                {
                    if (!Dttable.Columns.Contains("BPL-" + BasePricesList[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "BPL-" + BasePricesList[i].Name.ToString(), HeaderText = "BPL-" + BasePricesList[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 55, AllowEditing = true, Visible = true, IsVertical = true, FixedWidth = true });
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "BPL-" + BasePricesList[i].IdBasePriceList.ToString(), HeaderText = "BPL-" + BasePricesList[i].IdBasePriceList.ToString(), Settings = SettingsType.IdPriceList, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });

                        Dttable.Columns.Add("BPL-" + BasePricesList[i].Name.ToString(), typeof(string));
                        Dttable.Columns.Add("BPL-" + BasePricesList[i].IdBasePriceList.ToString(), typeof(string));
                    }
                }

                CustomerPriceList = PLMService.GetCustomerPriceListByDates(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null)).ToList();
                for (int i = 0; i < CustomerPriceList.Count; i++)
                {
                    if (!Dttable.Columns.Contains("CPL-" + CustomerPriceList[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "CPL-" + CustomerPriceList[i].Name.ToString(), HeaderText = "CPL-" + CustomerPriceList[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 55, AllowEditing = true, Visible = true, IsVertical = true, FixedWidth = true });
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "CPL-" + CustomerPriceList[i].IdCustomerPriceList.ToString(), HeaderText = "CPL-" + CustomerPriceList[i].IdCustomerPriceList.ToString(), Settings = SettingsType.IdPriceList, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });

                        Dttable.Columns.Add("CPL-" + CustomerPriceList[i].Name.ToString(), typeof(string));
                        Dttable.Columns.Add("CPL-" + CustomerPriceList[i].IdCustomerPriceList.ToString(), typeof(string));
                    }
                }

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

        void FillDataTable()
        {
            GeosApplication.Instance.Logger.Log("Method FillDataTable ...", category: Category.Info, priority: Priority.Low);

            Dttable.Rows.Clear();
            DttableCopy = Dttable.Copy();

            UserPermissionByBPLPriceList = PLMService.GetAllUserPermissionsByBPLPriceList(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null));
            UserPermissionByCPLPriceList = PLMService.GetAllUserPermissionsByCPLPriceList(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null));
            UsersList = PLMService.GetAllUsersList();

            for (int i = 0; i < UsersList.Count; i++)
            {
                try
                {
                    int count = 0, count1 = 0;
                    bool isCommonPermission = true;
                    DataRow dr = DttableCopy.NewRow();
                    dr["IdUser"] = UsersList[i].IdUser;
                    dr["IsChecked"] = false;
                    dr["Name"] = UsersList[i].Name;
                    
                    if ((UserPermissionByBPLPriceList.Any(x => x.User.IdUser == UsersList[i].IdUser) && UserPermissionByBPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).FirstOrDefault().BasePrice.IdBasePriceList == 0) || (UserPermissionByCPLPriceList.Any(x => x.User.IdUser == UsersList[i].IdUser) && UserPermissionByCPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).FirstOrDefault().CustomerPrice.IdCustomerPriceList == 0))
                    {
                        string permissions = ConvertPermissions(UserPermissionByBPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).FirstOrDefault().Permission.IdPermission);
                        dr["ALLPriceLists"] = permissions;
                        BasePricesList = PLMService.GetBasePriceListByDates(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null));
                        CustomerPriceList = PLMService.GetCustomerPriceListByDates(DateTime.ParseExact(FromDate, shortDateFormat, null), DateTime.ParseExact(ToDate, shortDateFormat, null));

                        for (int j = 0; j < BasePricesList.Count; j++)
                        {
                            count = (Dttable.Columns.IndexOf("BPL-" + BasePricesList[j].IdBasePriceList.ToString()));
                            count1 = (Dttable.Columns.IndexOf("BPL-" + BasePricesList[j].Name));
                            if (DttableCopy.Columns.Contains("BPL-" + BasePricesList[j].Name) && count != -1)
                            {
                                dr[count] = BasePricesList[j].IdBasePriceList.ToString();
                                dr[count1] = permissions;
                            }
                        }

                        for (int j = 0; j < CustomerPriceList.Count; j++)
                        {
                            count = (Dttable.Columns.IndexOf("CPL-" + CustomerPriceList[j].IdCustomerPriceList.ToString()));
                            count1 = (Dttable.Columns.IndexOf("CPL-" + CustomerPriceList[j].Name));
                            if (DttableCopy.Columns.Contains("CPL-" + CustomerPriceList[j].Name) && count != -1)
                            {
                                dr[count] = CustomerPriceList[j].IdCustomerPriceList.ToString();
                                dr[count1] = ConvertPermissions(UserPermissionByCPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).FirstOrDefault().Permission.IdPermission);
                            }
                        }
                    }
                    else
                    {
                        List<UserPermissionByBPLPriceList> userBPLList = new List<UserPermissionByBPLPriceList>(UserPermissionByBPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).ToList());
                        for (int j = 0; j < userBPLList.Count; j++)
                        {
                            count = (Dttable.Columns.IndexOf("BPL-" + userBPLList[j].BasePrice.IdBasePriceList.ToString()));
                            count1 = (Dttable.Columns.IndexOf("BPL-" + userBPLList[j].BasePrice.Name));
                            if (DttableCopy.Columns.Contains("BPL-" + userBPLList[j].BasePrice.Name) && count != -1)
                            {
                                dr[count] = userBPLList[j].BasePrice.IdBasePriceList.ToString();
                                dr[count1] = ConvertPermissions(userBPLList[j].Permission.IdPermission);
                            }
                        }

                        List<UserPermissionByCPLPriceList> userCPLList = new List<UserPermissionByCPLPriceList>(UserPermissionByCPLPriceList.Where(x => x.User.IdUser == UsersList[i].IdUser).ToList());
                        for (int j = 0; j < userCPLList.Count; j++)
                        {
                            count = (Dttable.Columns.IndexOf("CPL-" + userCPLList[j].CustomerPrice.IdCustomerPriceList.ToString()));
                            count1 = (Dttable.Columns.IndexOf("CPL-" + userCPLList[j].CustomerPrice.Name));
                            if (DttableCopy.Columns.Contains("CPL-" + userCPLList[j].CustomerPrice.Name) && count != -1)
                            {
                                dr[count] = userCPLList[j].CustomerPrice.IdCustomerPriceList.ToString();
                                dr[count1] = ConvertPermissions(userCPLList[j].Permission.IdPermission);
                            }
                        }

                        for (int j = 4; j < dr.Table.Columns.Count; j++)
                        {
                            if (dr[j].ToString() != dr[4].ToString() && isCommonPermission == true)
                            {
                                isCommonPermission = false;
                                break;
                            }

                            j++;
                        }
                    }

                    DttableCopy.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            Dttable = DttableCopy;

            GeosApplication.Instance.Logger.Log("Method FillDataTable() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        void SaveBPL(UInt64 idBasePrice, int idUser, uint idActiveUser, string permission)
        {
            UserPermissionByBPLPriceList userBPL = new UserPermissionByBPLPriceList();
            userBPL.User.IdUser = idUser;
            userBPL.BasePrice.IdBasePriceList = idBasePrice;
            userBPL.Permission.IdPermission = ConvertBackPermissions(permission);
            userBPL.IdCreator = idActiveUser;
            userBPL.CreationDate = GeosApplication.Instance.ServerDateTime;
            userBPL.IdModifier = idActiveUser;
            userBPL.ModificationDate = GeosApplication.Instance.ServerDateTime;

            isUserPermissionsave = PLMService.InsertUpdateUserPermissionByBPLForParticularColumn(userBPL);
        }

        void SaveCPL(UInt64 idCustomerPrice, int idUser, uint idActiveUser, string permission)
        {
            UserPermissionByCPLPriceList userCPL = new UserPermissionByCPLPriceList();
            userCPL.User.IdUser = idUser;
            userCPL.CustomerPrice.IdCustomerPriceList = idCustomerPrice;
            userCPL.Permission.IdPermission = ConvertBackPermissions(permission);
            userCPL.IdCreator = idActiveUser;
            userCPL.CreationDate = GeosApplication.Instance.ServerDateTime;
            userCPL.IdModifier = idActiveUser;
            userCPL.ModificationDate = GeosApplication.Instance.ServerDateTime;

            isUserPermissionsave = PLMService.InsertUpdateUserPermissionByCPLForParticularColumn(userCPL);
        }

        string ConvertPermissions(Int32 idPermission)
        {
            string permission = string.Empty;
            if (idPermission == 50)
                permission = LookUpValueList[0].Abbreviation;
            else if (idPermission == 62)
                permission = LookUpValueList[1].Abbreviation;

            return permission;
        }

        int ConvertBackPermissions(string Permission)
        {
            int idPermission = 0;
            if (Permission == LookUpValueList[0].Abbreviation)
                idPermission = 50;
            if (Permission == LookUpValueList[1].Abbreviation)
                idPermission = 62;

            return idPermission;
        }

        void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;

                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    SetDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString(shortDateFormat);
                    ToDate = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString(shortDateFormat);
                    ToDate = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString(shortDateFormat);
                    ToDate = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString(shortDateFormat);
                    ToDate = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString(shortDateFormat);
                    ToDate = EndDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 8)//this year
                {
                    SetDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString(shortDateFormat);
                    ToDate = EndToDate.ToString(shortDateFormat);
                }

                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }

                AddDataTableColumns();
                FillDataTable();

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        Action Processing()
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
            return null;
        }

        void SetDefaultPeriod()
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }

    public class Permissions
    {
        public string PermissionName { get; set; }
    }
}
