using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.UI.Commands;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Hrm.Views;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using Emdep.Geos.UI.CustomControls;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common;
using DevExpress.Mvvm;
using DevExpress.Export.Xl;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class ExportAttendanceViewModel: IDataErrorInfo
    {
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private DateTime startDate;
        private DateTime endDate;
        private bool isBusy;
        private List<EmployeeAttendance> employeeAttendanceList;
        private ObservableCollection<long> selectedPlantList;

        private List<object> selectedAuthorizedPlantsList;
        private ObservableCollection<Company> isCompanyList;
        #endregion  //Declaration

        #region public Properties

        public List<EmployeeAttendance> EmployeeAttendanceList
        {
            get
            {
                return employeeAttendanceList;
            }
            set
            {
                employeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceList"));

            }
        }

        public ObservableCollection<long> SelectedPlantList
        {
            get { return selectedPlantList; }
            set
            {
                selectedPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantList"));
            }
        }

        public List<object> SelectedAuthorizedPlantsList
        {
            get
            {
                return selectedAuthorizedPlantsList;
            }

            set
            {
                selectedAuthorizedPlantsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAuthorizedPlantsList"));
            }
        }
        public ObservableCollection<Company> IsCompanyList
        {
            get
            {
                return isCompanyList;
            }

            set
            {
                isCompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompanyList"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public static bool IsPreferenceChanged { get; set; }
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
        #endregion // Properties

        #region public ICommand
        public ICommand ExportAttendanceViewCancelButtonCommand { get; set; }
        public ICommand ExportAttendanceViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion // ICommand

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // Events

        #region validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }
        #endregion

        #region Constructor
        public ExportAttendanceViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExportAttendanceViewModel ...", category: Category.Info, priority: Priority.Low);

                ExportAttendanceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                ExportAttendanceViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptCommand));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                StartDate = new DateTime(DateTime.Now.Year, 1,1);
                EndDate = DateTime.Today;

                if (HrmCommon.Instance.IsCompanyList != null)
                    IsCompanyList=new ObservableCollection<Company>(HrmCommon.Instance.IsCompanyList);

                GeosApplication.Instance.Logger.Log("Constructor ExportAttendanceViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExportAttendanceViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Constructor

        #region Methods
        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsPreferenceChanged = false;
            RequestClose(null, null);
        }

        private void AcceptCommand(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptCommand ...", category: Category.Info, priority: Priority.Low);
                //string error = EnableValidationAndGetError();
                ExportAttendancetList(obj);
                GeosApplication.Instance.Logger.Log("Method AcceptCommand() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptCommand() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][avpawar][GEOS2-2667][Manual column not appear when we export attendance from shortcut keys]
        /// </summary>
        /// <param name="obj"></param>
        private void ExportAttendancetList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportAttendancetList()...", category: Category.Info, priority: Priority.Low);

               
                
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Attendance List";
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

                    

                    List<Company> plantOwners = SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                   
                    //[001] ServiceStack method changed form GetEmployeeAttendanceShortDetail to GetEmployeeAttendanceShortDetail_V2070
                    //EmployeeAttendanceList = new List<EmployeeAttendance>(HrmService.GetEmployeeAttendanceShortDetail(plantOwnersIds, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate, EndDate));
                    EmployeeAttendanceList = new List<EmployeeAttendance>(HrmService.GetEmployeeAttendanceShortDetail_V2070(plantOwnersIds, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate, EndDate));

                    TableView departmentTableView = ((TableView)obj);
                    departmentTableView.Grid.ItemsSource = EmployeeAttendanceList;
                    departmentTableView.ShowTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    departmentTableView.ShowFixedTotalSummary = false;
                    
                    departmentTableView.ExportToXlsx(ResultFileName, options);
                    departmentTableView.Grid.ItemsSource = null;
                    IsBusy = false;
                    departmentTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("Method ExportAttendancetList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportAttendancetList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            if (e.ColumnFieldName == "AccountingDate")
            {
                if (e.Value != null && e.Value.ToString() != "Accounting Date" && e.ColumnFieldName == "AccountingDate")
                {
                    e.Value = string.Format("{0:dd/MM/yyyy}", (DateTime)e.Value);
                }
            }

            if (e.ColumnFieldName == "Employee.LstEmployeeDepartments")
            {
                var DeptList = e.Value as List<Department>;
                if (DeptList != null)
                {
                    e.Value = string.Join("\n", DeptList.Select(x => x.DepartmentName));
                    e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
                }
            }
            e.Handled = true;
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
              
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
        #endregion //Methods 
    }