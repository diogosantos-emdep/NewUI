using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.CommonClass;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ServiceStack;
using DevExpress.Xpf.Editors;
using DevExpress.CodeParser;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
   public class ReadImportedLeaveViewModel : INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// [rdixit][13.07.2022][GEOS2-3697][[ZK_TIME] Add a new option “Import Leave” in the LEAVES section]
        /// </summary>

        #endregion

        #region Services      
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion 

        #region Events

        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declaration
        private bool isSave;
        private bool isBusy;
        private DateTime filterStartDate;
        private DateTime filterEndDate;
        private ObservableCollection<ImportLeaves> employeeLeaveData;
        private ObservableCollection<EmployeeLeaves> leaveSheetData;
        private ObservableCollection<LookupValue> leaveTypeList;
        private EmployeeLeave selectedLeave = null;
        private List<EmployeeShift> employeeShiftList;
        private CompanyShift companyShiftNone;
        private ObservableCollection<EmployeeLeave> employeeLeavesList;
        private ObservableCollection<EmployeeLeave> empAddedLeavesList;

        #endregion

        #region Properties

        private List<int> CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport { get; set; }
        private Dictionary<string, string> LstImportLeavesCodes { get; set; }
        private DateTime LeaveInTolerance { get; set; }
        private DateTime LeaveOutTolerance { get; set; }
        private List<IDictionary<string, object>> SheetData { get; set; }        
        public ObservableCollection<EmployeeLeave> EmployeeLeavesList
        {
            get { return employeeLeavesList; }
            set
            {
                employeeLeavesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeavesList"));
            }
        }        
        public ObservableCollection<EmployeeLeave> EmpAddedLeavesList
        {
            get { return empAddedLeavesList; }
            set
            {
                empAddedLeavesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmpAddedLeavesList"));
            }
        }
        public EmployeeLeave SelectedLeave
        {
            get { return selectedLeave; }
            set
            {
                selectedLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeave"));
            }
        }
        public DateTime FilterStartDate
        {
            get { return filterStartDate; }
            set
            {
                filterStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStartDate"));
            }
        }
        public DateTime FilterEndDate
        {
            get { return filterEndDate; }
            set
            {
                filterEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterEndDate"));
            }
        }
        //[rdixit][10.02.2025][GEOS2-6850]
        List<EmployeeAnnualLeave> employeeProfileLeaveList;
        List<EmployeeAnnualLeave> EmployeeProfileLeaveList
        {
            get { return employeeProfileLeaveList; }
            set
            {
                employeeProfileLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfileLeaveList"));
            }
        }
        public ObservableCollection<LookupValue> LeaveTypeList
        {
            get { return leaveTypeList; }
            set
            {
                leaveTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveTypeList"));
            }
        }
        public string MappingFailMessage { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public ObservableCollection<EmployeeLeaves> LeavesSheetData
        {
            get { return leaveSheetData; }
            set
            {
                leaveSheetData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeavesSheetData"));
            }
        }
        public ObservableCollection<ImportLeaves> EmployeeLeavesData
        {
            get { return employeeLeaveData; }
            set
            {
                employeeLeaveData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeavesData"));
            }
        }
        public bool IsMatchTypeWithDataSourceField { get; set; }
        public string TimeEditMask { get; set; }
        public List<EmployeeLeave> NewEmployeeLeavesList { get; set; }
        public List<EmployeeShift> EmployeeShiftList
        {
            get { return employeeShiftList; }
            set { employeeShiftList = value; OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList")); }
        }
        public GeosAppSetting Setting { get; set; }
        private string gridColHeader;
        public string GridColHeader
        {
            get
            {
                return gridColHeader;
            }
            set
            {
                gridColHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridColHeader"));
            }
        }
        private bool UseFirstAndLastDayRecordInLeaveImport { get; set; }
        #endregion

        #region Public Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand DeleteLeaveRowCommand { get; set; }
        public ICommand StartEndChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        //[rdixit][10.02.2025][GEOS2-6850]
        public ICommand TypeValidateCommand { get; set; }
        #endregion

        #region Constructor
        public ReadImportedLeaveViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReadImportedLeaveViewModel()...", category: Category.Info, priority: Priority.Low);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                DeleteLeaveRowCommand = new DelegateCommand<object>(DeleteLeaveRowCommandAction);
                StartEndChangedCommand = new DelegateCommand<CellValueChangedEventArgs>(StartEndChangedCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                TypeValidateCommand = new DelegateCommand<object>(TypeValidateCommandAction);
                //FillLeavesTypeList(); [GEOS2-6850][rdixit][30.01.2025]
                TimeEditMask = "HH:mm"; // 001 CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                GeosApplication.Instance.Logger.Log("Method ReadImportedLeaveViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ReadImportedLeaveViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        //[rdixit][10.02.2025][GEOS2-6850]
        public void TypeValidateCommandAction(object obj)
        {
            try
            {
                DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
                ComboBoxEdit combo = (ComboBoxEdit)e.Source;
                LookupValue leavetype = (LookupValue)combo.SelectedItem;
                long leaveId;
                if (e.Value == null)
                {

                }
                else
                {
                    if (!long.TryParse(e.Value.ToString(), out leaveId))
                    {
                        return;
                    }
                    var temp = EmployeeProfileLeaveList.FirstOrDefault(i => i.IdEmployee == leavetype.IdEmployee && i.IdLeave == leaveId);

                    var lookupValues = combo.ItemsSource as List<LookupValue>;
                    combo.SelectedItem = lookupValues.FirstOrDefault(i => i.IdLookupValue == leaveId);
                    if (temp == null)
                    {
                        e.IsValid = false;
                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        e.ErrorContent = string.Format("Not a Authorized leave type");
                    }
                    else
                    {
                        e.IsValid = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Init Method
        /// </summary>
        /// <param name="colData"></param>
        /// <param name="sheetData">Imported data</param>
        /// <param name="employeeLeavesList">total Leaves to compare</param>
        public void Init(ObservableCollection<ImportLeaves> colData, List<IDictionary<string, object>> sheetData, List<EmployeeLeave> employeeLeavesList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  Init()...", category: Category.Info, priority: Priority.Low);
                try
                {
                    //[GEOS2-6850][rdixit][30.01.2025]
                    if(sheetData != null)
                    {
                        List<int> empid = new List<int>();
                        foreach (var item in sheetData)
                        {
                            empid.Add(((Employee)item.FirstOrDefault(X => X.Key == "Employee").Value).IdEmployee);
                        }

                        if (empid != null)
                        {
                            LeaveTypeList = new ObservableCollection<LookupValue>(HrmService.GetEmployeeLeavesByLocations_V2590(empid));
                            if (LeaveTypeList == null)
                                LeaveTypeList = new ObservableCollection<LookupValue>();
                            //[rdixit][10.02.2025][GEOS2-6850]
                            EmployeeProfileLeaveList = new List<EmployeeAnnualLeave>(HrmService.GetAnnualLeavesByIdEmployees(string.Join(",", empid), DateTime.Now.Year));
                        }
                    }
                    string ImportLeavesCodes =CrmStartUp.GetGeosAppSettings(90).DefaultValue;
                    string[] ArrayImportLeavesCodes = ImportLeavesCodes.Split(',').ToArray();
                    if (LstImportLeavesCodes == null)
                    {
                        LstImportLeavesCodes = new Dictionary<string, string>();
                    }
                    foreach (string item in ArrayImportLeavesCodes)
                    {
                        string[] tempCodes = null;
                        if (item.Contains(';'))
                            tempCodes = item.Split(';').ToArray();
                        if (tempCodes.Count() == 2 && tempCodes[0] != null && tempCodes[1] != null)
                            LstImportLeavesCodes.Add(tempCodes[0].Replace('(', ' ').Trim(), tempCodes[1].Replace(')', ' ').Trim());
                    }

                    CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport = new List<int>();
                    var Setting62 = WorkbenchService.GetGeosAppSettings(62);
                    var CompanyIdsStringArray = Setting62.DefaultValue.Split(',');

                    for (int i = 0; i < CompanyIdsStringArray.Length; i++)
                    {
                        CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport.Add(int.Parse(CompanyIdsStringArray[i]));
                    }

                    SetUseFirstAndLastDayRecordInLeaveImport();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init(). " +
                        "Check IdAppSetting 62 'SetUseFirstAndLastDayRecordInLeavesImport' added correctly in table geos_app_settings. Now, Application will use default value 559,11558,11633. Error=" +
                        ex.Message, category: Category.Exception, priority: Priority.Low);
                    CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport = new List<int> { 559, 11558, 11633 }; // set default values. 559 is EPIN, 11558 is EEIN, 11633 is ETCN
                }

                EmployeeLeavesData = new ObservableCollection<ImportLeaves>(colData);
                LeavesSheetData = new ObservableCollection<EmployeeLeaves>();
                SheetData = sheetData;

                EmployeeLeavesList = new ObservableCollection<EmployeeLeave>();
                EmployeeLeavesList.AddRange(employeeLeavesList);
                ObservableCollection<EmployeeLeaves> listObj = new ObservableCollection<EmployeeLeaves>();
                MappingFailMessage = string.Empty;

                EmployeeShiftList = new List<EmployeeShift>();
                List<Employee> EmployeeShiftsList = new List<Employee>();

                companyShiftNone = new CompanyShift { IdCompanyShift = 0 };
                GetTolerance();
                #region sorting and removing unwanted data

                List<IDictionary<string, object>> sortedSheetData = new List<IDictionary<string, object>>();
                if (sheetData.Any(a => a != null && a.Any(aa => aa.Key.Equals("EmployeeCompanyEmail"))))
                {
                    GridColHeader = "EmployeeCompanyEmail";
                    sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeCompanyEmail"]))).ThenBy(row => Convert.ToDateTime(row["Converted_Date_Time"])).ToList();

                }
                else
                {
                    GridColHeader = "EmployeeClockTimeID";
                    sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeClockTimeID"]))).ThenBy(row => Convert.ToDateTime(row["Converted_Date_Time"])).ToList();
                }


                #endregion
                Setting = WorkbenchService.GetGeosAppSettings(32);
                foreach (IDictionary<string, object> item2 in sortedSheetData)
                {
                    CommonClass.EmployeeLeaves obj = new CommonClass.EmployeeLeaves();

                    foreach (var item3 in item2)
                    {
                        if (item3.Key == "EmployeeClockTimeID" || item3.Key == "EmployeeCompanyEmail")
                        {
                            obj.EmployeeClockTimeID = item3.Value.ToString();
                            obj.Employee = item2.FirstOrDefault(X => X.Key == "EmployeeName").Value.ToString();
                        }
                        else if (item3.Key == "Date")
                        {
                            try
                            {
                                obj.Date = Convert.ToDateTime(item2["Converted_Date_Time"]).Date;

                            }
                            catch (Exception)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("LeavesFieldMappingFailed").ToString());
                                break;
                            }
                        }
                        else if (item3.Key == "Time")
                        {
                            try
                            {
                                obj.Time = Convert.ToDateTime(item2["Converted_Date_Time"]);
                            }
                            catch (Exception)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("LeavesFieldMappingFailed").ToString());
                                break;
                            }
                        }
                        //[001]added
                        else if (item3.Key == "Employee")
                        {
                            try
                            {
                                Employee employee = (Employee)item3.Value;
                                obj.idEmployee = employee.IdEmployee;

                                obj.CompanyShiftList = new ObservableCollection<CompanyShift>();
                                foreach (EmployeeShift itemEmployeeShift in employee.EmployeeShifts)
                                {
                                    obj.CompanyShiftList.Add(itemEmployeeShift.CompanyShift);
                                }

                                if (!EmployeeShiftsList.Exists(x => x.IdEmployee == employee.IdEmployee))
                                {
                                    EmployeeShiftsList.Add(employee);
                                }

                            }
                            catch (Exception ex)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("LeavesFieldMappingFailed").ToString());
                                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                break;
                            }
                        }
                        else if (IsMatchTypeWithDataSourceField && item3.Key == "Type") //[002]
                        {
                            try
                            {
                                if (LstImportLeavesCodes.Any(i => i.Value.ToString() == item3.Value.ToString().PadLeft(4, '0')))
                                {
                                    //[GEOS2-6850][rdixit][30.01.2025]
                                    Employee employee = (Employee)item2.FirstOrDefault(X => X.Key == "Employee").Value;
                                    obj.LeaveTypeList = LeaveTypeList.Where(i => i.IdEmployee == employee.IdEmployee)?.ToList();
                                    if (obj.LeaveTypeList == null)
                                        obj.LeaveTypeList = new List<LookupValue>();
                                    obj.LeaveTypeList.Insert(0, new LookupValue() { IdLookupValue = 0, Value = "---", IdEmployee = employee.IdEmployee });
                                    string idLookvalue = LstImportLeavesCodes.Where(i => i.Value == item3.Value.ToString().PadLeft(4, '0').ToString()).FirstOrDefault().Key;
                                    obj.Type = obj.LeaveTypeList.Where(i => i.IdLookupValue == Convert.ToInt32(idLookvalue))?.FirstOrDefault();
                                    if (obj.Type == null)
                                        obj.Type = obj.LeaveTypeList[0];
                                }
                            }
                            catch (Exception ex)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("LeavesFieldMappingFailed").ToString());
                                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                break;
                            }
                        }

                    }

                    if (!IsMatchTypeWithDataSourceField)
                        obj.Type = LeaveTypeList[0];

                    listObj.Add(obj);
                }

                LeavesSheetData = listObj;
                Setting = WorkbenchService.GetGeosAppSettings(32);
                if (Convert.ToInt32(Setting.DefaultValue) == 1)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    var EmpDocumentNumbers = string.Join(",", LeavesSheetData.Select(i => i.EmployeeClockTimeID).Distinct());
                    List<Employee> Employees = new List<Employee>();
                    List<EmployeeShift> EmployeeNightShift = new List<EmployeeShift>();
                    if (EmployeeShiftsList.Count > 0)
                    {
                        EmployeeShiftList = EmployeeShiftsList.SelectMany(es => es.EmployeeShifts).ToList();
                        SetNightShiftStartEnd(EmployeeShiftList);
                    }

                }
                else
                {
                    SetStartEnd();
                }
                var temp = (LeavesSheetData.OrderBy(x => x.Employee).ThenBy(x => x.Date).ThenBy(x => x.Time)).ToList();
                LeavesSheetData.Clear();
                LeavesSheetData.AddRange(temp);
                SetShiftDataToShowInGridcolumnShift();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()-" + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()- ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        /// <summary>
        /// Method to Fill Leave Type from lookup 
        /// </summary>
        public void FillLeavesTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  FillLeavesTypeList()...", category: Category.Info, priority: Priority.Low);
                LeaveTypeList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32).AsEnumerable());
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method FillLeavesTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method FillLeavesTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

        /// <summary>
        /// Method to set In/Out Checkbox as Radio 
        /// </summary>
        /// <param name="args"></param>
        public void StartEndChangedCommandAction(CellValueChangedEventArgs args)
        {
            GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method InOutChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

            TableView dxTableView = args.OriginalSource as TableView;
            GridControl dxGrid = dxTableView.DataControl as GridControl;

            if (args.Column.FieldName == "Start")
            {
                dxGrid.SetFocusedRowCellValue(dxGrid.Columns["End"], "False");
            }
            else if (args.Column.FieldName == "End")
            {
                dxGrid.SetFocusedRowCellValue(dxGrid.Columns["Start"], "False");
            }

            if (args.Column.FieldName == "Start" ||
                args.Column.FieldName == "End" ||
                args.Column.FieldName == "CompanyShift")
            {
                bool invalue;
                CompanyShift companyShift;
                if (args.Column.FieldName == "Start")
                {
                    invalue = (bool)args.Value;
                }
                else
                {
                    invalue = (bool)dxGrid.GetFocusedRowCellValue(dxGrid.Columns["Start"]);
                }
                if (args.Column.FieldName == "CompanyShift")
                {
                    companyShift = (CompanyShift)args.Value;
                }
                else
                {
                    companyShift = (CompanyShift)dxGrid.GetFocusedRowCellValue(dxGrid.Columns["CompanyShift"]);
                }


                EmployeeLeaves currentInRow = (EmployeeLeaves)dxGrid.GetRow(args.RowHandle);
                if (invalue)
                {
                    for (int i = args.RowHandle; i < dxGrid.VisibleRowCount; i++)
                    {
                        EmployeeLeaves row = (EmployeeLeaves)dxGrid.GetRow(i);
                        if (row.End)
                        {
                            EmployeeLeaves nextOutRow = row;
                            nextOutRow.CompanyShift = companyShift;
                            EmployeeLeaves currentInRowAfterUpdate = currentInRow;
                            currentInRowAfterUpdate.CompanyShift = companyShift;
                            nextOutRow.EnableValidationAndGetError(currentInRowAfterUpdate);
                            dxGrid.RefreshData();
                            break;
                        }
                    }

                }
            }
            GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method InOutChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        }



        /// <param name="obj"></param>
        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                List<EmployeeLeaves> FilterdRows = new List<EmployeeLeaves>();
                var FilterdView = (TableView)obj;
                IList Rows = FilterdView.Grid.DataController.GetAllFilteredAndSortedRows();
                //[GEOS2-6850][rdixit][30.01.2025]
                if (LeavesSheetData != null)
                {                   
                    if (LeavesSheetData.Any(p => (p.Start || p.End) && (p.Type == null || p.Type?.IdLookupValue==0)))
                    {
                        {
                            CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveTypeErrorMessage"].ToString()),
                                "Red",CustomMessageBox.MessageImagePath.NotOk,MessageBoxButton.OK);
                       
                            return; 
                        }
                    }
                }
                foreach (var row in Rows)
                {
                    var employeeLeave = (EmployeeLeaves)row;
                    //[rdixit][10.02.2025][GEOS2-6850]
                    var temp = EmployeeProfileLeaveList.FirstOrDefault(i => i.IdEmployee == employeeLeave.Type.IdEmployee && i.IdLeave == employeeLeave.Type.IdLookupValue);
                    if (temp != null)
                        FilterdRows.Add(employeeLeave);
                }

                if (FilterdRows.Count != LeavesSheetData.Count)
                {
                    //[001]added
                    MessageBoxResult Result = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveFilterMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (Result != MessageBoxResult.Yes)
                        return;
                }
                //[rdixit][10.02.2025][GEOS2-6850]
                FilterdRows = new List<EmployeeLeaves>();
                foreach (var row in Rows)
                {
                    var employeeLeave = (EmployeeLeaves)row;
                    var temp = EmployeeProfileLeaveList.FirstOrDefault(i => i.IdEmployee == employeeLeave.Type.IdEmployee && i.IdLeave == employeeLeave.Type.IdLookupValue);

                    if (employeeLeave.Time == (null) || employeeLeave.Date == (null))
                        continue;
                    if ((employeeLeave.Start == true || employeeLeave.End == true))
                        if (temp != null)
                            FilterdRows.Add(employeeLeave);
                }

                bool errorFound = false;
                LeavesSheetData.Clear();
                LeavesSheetData.AddRange(FilterdRows);
                EmployeeLeaves inRecord = new EmployeeLeaves();
                // bool errorFound = false;
                for (int i = 0; i < LeavesSheetData.Count; i++)
                {
                    if (LeavesSheetData[i].Start)
                        inRecord = LeavesSheetData[i];

                    if (LeavesSheetData[i].End && inRecord != null)
                    {
                        string error = LeavesSheetData[i].EnableValidationAndGetError(inRecord);
                        if (!string.IsNullOrEmpty(error))
                            errorFound = true;
                    }
                }

                if (errorFound)
                {
                    CustomMessageBox.Show(
                        System.Windows.Application.Current.FindResource("ReadImportedLeavesValidationErrorCheckTheDataEntered").ToString() + Environment.NewLine +
                        System.Windows.Application.Current.FindResource("ReadImportedLeavesValidationErrorShiftValueIsRequired").ToString() + Environment.NewLine +
                        System.Windows.Application.Current.FindResource("ReadImportedLeavesValidationErrorShiftValueMustBeSameForInOut").ToString()
                        , Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

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

                List<EmployeeLeavesImportField> tmpList = new List<EmployeeLeavesImportField>();
                IsBusy = true;
                for (int i = 0; i < FilterdRows.Count; i++)
                {
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    EmployeeLeavesImportField objEmp = new EmployeeLeavesImportField() { Name = FilterdRows[i].EmployeeClockTimeID, IdCompanyLeave = FilterdRows[i].Type.IdLookupValue };

                    if (FilterdRows[i].Start == true)
                    {
                        string sdt = FilterdRows[i].Date.ToShortDateString() + " " + FilterdRows[i].Time.ToShortTimeString();
                        startDate = Convert.ToDateTime(sdt);
                        objEmp.StartDate = startDate;
                        EmployeeLeaves EmployeeLeavesEndRecord;
                        //[004] added
                        if (FilterdRows[i].CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        {
                            if (FilterdRows[i].CompanyShift.IsNightShift != 1)
                            {
                                //Night Employee (Check out time present or not with same day attendance)
                                EmployeeLeavesEndRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay > FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                                if (EmployeeLeavesEndRecord == null)//No out time is found
                                    continue;
                            }
                            else
                            {
                                EmployeeLeavesEndRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true).FirstOrDefault();
                                if (EmployeeLeavesEndRecord == null)
                                    continue;
                            }
                        }
                        else
                        {
                            EmployeeLeavesEndRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay > FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                            if (EmployeeLeavesEndRecord == null)//No out time is found
                                continue;
                        }
                        objEmp.CompanyShift = FilterdRows[i].CompanyShift;
                        objEmp.AccountingDate = FilterdRows[i].AccountingDate;
                        //end
                        objEmp.EndDate = DateTime.Parse(EmployeeLeavesEndRecord.Date.ToShortDateString() + " " + EmployeeLeavesEndRecord.Time.ToShortTimeString());
                        FilterdRows.Remove(EmployeeLeavesEndRecord);
                    }
                    else if (FilterdRows[i].End == true)
                    {
                        string edt = FilterdRows[i].Date.ToShortDateString() + " " + FilterdRows[i].Time.ToShortTimeString();
                        endDate = Convert.ToDateTime(edt);
                        objEmp.EndDate = endDate;
                        EmployeeLeaves EmployeeLeaveStartRecord;
                        if (FilterdRows[i].CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        {
                            if (FilterdRows[i].CompanyShift.IsNightShift != 1)
                            {
                                EmployeeLeaveStartRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay < FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                                if (EmployeeLeaveStartRecord == null)
                                    continue;
                            }
                            else
                            {
                                EmployeeLeaveStartRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true).FirstOrDefault();
                                if (EmployeeLeaveStartRecord == null)
                                    continue;
                            }
                        }
                        else
                        {
                            EmployeeLeaveStartRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.End == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay < FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                            if (EmployeeLeaveStartRecord == null)
                                continue;
                        }

                        objEmp.CompanyShift = FilterdRows[i].CompanyShift;
                        objEmp.AccountingDate = FilterdRows[i].AccountingDate;

                        objEmp.StartDate = DateTime.Parse(EmployeeLeaveStartRecord.Date + " " + EmployeeLeaveStartRecord.Time);
                        FilterdRows.Remove(EmployeeLeaveStartRecord);
                    }

                    tmpList.Add(objEmp);

                }

                EmpAddedLeavesList = new ObservableCollection<EmployeeLeave>();
                string errorMessage = string.Empty;
                bool isLeaveOverlapped = false;

                foreach (EmployeeLeavesImportField item in tmpList)
                {
                    Employee employee = new Employee();
                    if (item.StartDate != DateTime.MinValue && item.EndDate != DateTime.MinValue && item.StartDate < item.EndDate)
                    {
                        foreach (var item2 in SheetData)
                        {
                            foreach (var item3 in item2)
                            {
                                if (item3.Key == "EmployeeClockTimeID" && item3.Value.ToString().Equals(item.Name.ToString()))
                                {
                                    employee = (Employee)item2["Employee"];
                                    break;
                                }
                                if (item3.Key == "EmployeeCompanyEmail" && item3.Value.ToString().Equals(item.Name.ToString()))
                                {
                                    employee = (Employee)item2["Employee"];
                                    break;
                                }
                            }
                        }
                        // DateTime? AccountingDate;

                        //if (item.CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        //{
                        //    AccountingDate = item.AccountingDate;
                        //}
                        //else
                        //{
                        //    AccountingDate = item.StartDate;

                        //}
                        EmployeeLeave employeeLeave = new EmployeeLeave
                        {
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Employee = employee,
                            CompanyShift = item.CompanyShift,
                            IdCompanyShift = item.CompanyShift.IdCompanyShift,
                            IdEmployee = employee.IdEmployee,
                            IdLeave = item.IdCompanyLeave,
                            Creator = GeosApplication.Instance.ActiveUser.IdUser,
                            CreationDate = GeosApplication.Instance.ServerDateTime,
                        };

                        employeeLeave.Employee.TotalWorkedHours = (item.EndDate - item.StartDate).ToString();
                        if (!(EmployeeLeavesList.Where(x => x.IdEmployee == employeeLeave.IdEmployee).Any(x => (x.StartDate < employeeLeave.EndDate && employeeLeave.StartDate < x.EndDate))
                            || EmpAddedLeavesList.Where(x => x.IdEmployee == employeeLeave.IdEmployee).Any(x => (x.StartDate < employeeLeave.EndDate && employeeLeave.StartDate < x.EndDate))))
                            EmpAddedLeavesList.Add(employeeLeave);
                        else
                            isLeaveOverlapped = true;
                    }
                    else
                    {
                        if (item.StartDate == DateTime.MinValue)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("LeaveCheckInError").ToString() + "\n", item.Name, item.EndDate.Date.ToShortDateString());
                        else if (item.EndDate == DateTime.MinValue)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("LeaveCheckOutError").ToString() + "\n", item.Name, item.StartDate.Date.ToShortDateString());
                        else if (item.StartDate > item.EndDate)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("LeaveCheckInOutError").ToString() + "\n", item.Name, item.StartDate.Date.ToShortDateString());
                    }
                }

                if (EmpAddedLeavesList.Count > 0)
                {
                    NewEmployeeLeavesList = HrmService.AddEmployeeImportLeave(EmpAddedLeavesList.ToList());
                    IsSave = true;
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeaveSaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    if (errorMessage != "")
                        CustomMessageBox.Show(string.Format(errorMessage.ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    else if (isLeaveOverlapped)
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeLeaveOverlapped"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    else
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveSaveFailed"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetUseFirstAndLastDayRecordInLeaveImport()
        {
            var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

            bool useFirstAndLastDayRecordInLeaveImport = false;
            for (int j = 0; j < CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport.Count; j++)
            {
                if (companies.Any(a => a.IdCompany == CompanyIdsForSettingUseFirstAndLastDayRecordInLeavesImport[j]))
                {
                    useFirstAndLastDayRecordInLeaveImport = true;
                    break;
                }
            }

            UseFirstAndLastDayRecordInLeaveImport = useFirstAndLastDayRecordInLeaveImport;
        }
        private void GetTolerance()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  GetTolerance()...", category: Category.Info, priority: Priority.Low);
                DateTime result;
                var val = CrmStartUp.GetGeosAppSettings(19).DefaultValue;
                DateTime.TryParse(CrmStartUp.GetGeosAppSettings(19).DefaultValue, out result);
                LeaveInTolerance = result;
                DateTime.TryParse(CrmStartUp.GetGeosAppSettings(20).DefaultValue, out result);
                LeaveOutTolerance = result;
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method  GetTolerance()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetTolerance Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Smart import Leaves
        /// </summary>
        private void SetStartEnd()
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

                for (int i = 0; i < LeavesSheetData.Count; i++)
                {
                    var LeavePerDay = LeavesSheetData.Where(x => x.Date.Date == LeavesSheetData[i].Date.Date && x.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID).ToList();

                    if (LeavePerDay.Count == 1)
                    {
                        LeavePerDay[0].IsRowError = true;
                        i = LeavePerDay.IndexOf(LeavePerDay[LeavePerDay.Count - 1]);
                        continue;
                    }

                    for (int record = 0; record < LeavePerDay.Count; record++)
                    {
                        // Added new code [003]
                        if (record > 0 && LeavePerDay[record - 1].Start && LeavePerDay[record - 1].Date.Date != LeavePerDay[record].Date.Date)
                        {
                            LeavePerDay[record - 1].IsRowError = true;
                            LeavePerDay[record - 1].Start = false;
                        }
                        //Added new code[003]
                        if (record == 0 || LeavePerDay[record - 1].IsRowError)
                        {
                            var inRecords = LeavePerDay.Where(s => s.Time >= LeavePerDay[record].Time && s.Time <= LeavePerDay[record].Time.AddMinutes(LeaveInTolerance.Minute));
                            inRecords.First().Start = true;
                            record = LeavePerDay.IndexOf(inRecords.Last());
                            continue;
                        }

                        if (LeavePerDay[record - 1].End)
                        {
                            var inRecords = LeavePerDay.Where(s => s.Time >= LeavePerDay[record].Time && s.Time <= LeavePerDay[record].Time.AddMinutes(LeaveInTolerance.Minute));
                            inRecords.First().Start = true;
                            if (record == LeavePerDay.Count() - 1)
                            {
                                LeavePerDay[record].Start = false;
                            }
                            record = LeavePerDay.IndexOf(inRecords.Last());

                        }
                        else
                        {
                            // if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                            if (this.UseFirstAndLastDayRecordInLeaveImport)
                            {
                                var outRecords = LeavePerDay.Where(s => s.Time >= LeavePerDay[record].Time && s.Time <= LeavePerDay[record].Time.AddMinutes(LeaveOutTolerance.Minute));
                                var currentDateRecords = LeavePerDay.Where(s => s.Time.Date == LeavePerDay[record].Time.Date).ToList();
                                if (currentDateRecords.Exists(a => a.Start == true))
                                {
                                    currentDateRecords.Last().End = true;
                                    record = LeavePerDay.IndexOf(currentDateRecords.Last());
                                }
                            }
                            else
                            {
                                var outRecords = LeavePerDay.Where(s => s.Time >= LeavePerDay[record].Time && s.Time <= LeavePerDay[record].Time.AddMinutes(LeaveOutTolerance.Minute));
                                outRecords.Last().End = true;
                                record = LeavePerDay.IndexOf(outRecords.Last());
                            }

                        }
                    }

                    i = LeavesSheetData.IndexOf(LeavePerDay[LeavePerDay.Count - 1]);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///Import Leaves (all data sources) with night shifts values. [IES15]
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteLeaveRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method DeleteLeaveRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                EmployeeLeaves ObjLeave = (EmployeeLeaves)parameter;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["LeaveDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ObjLeave != null)
                    {
                        LeavesSheetData.Remove(ObjLeave);
                        //[001]added
                        if (ObjLeave.CompanyShift != null)
                        {
                            SetNightShiftStartEnd(EmployeeShiftList);
                        }
                        else
                        {
                            SetStartEnd();
                        }
                        SetShiftDataToShowInGridcolumnShift();
                    }
                }
                else
                {
                    IsBusy = false;
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method DeleteLeaveRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method DeleteLeaveRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method DeleteLeaveRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method DeleteLeaveRowCommandAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for set in/out for Night shift
        /// </summary>
        /// <param name="EmployeeShiftList"></param>

        private void SetNightShiftStartEnd(List<EmployeeShift> EmployeeShiftList)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method SetNightShiftStartEnd()...", category: Category.Info, priority: Priority.Low);
                for (int i = 0; i < LeavesSheetData.Count; i++)
                {
                    var LeavesPerEmployee = LeavesSheetData.Where(x => x.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID).ToList();

                    if (LeavesPerEmployee[0].CompanyShiftList.Count > 1 || EmployeeShiftList.Any(x => x.CompanyShift.IsNightShift == 1 && x.IdEmployee == LeavesSheetData[i].idEmployee))
                    {
                        for (int record = 0; record < LeavesPerEmployee.Count; record++)
                        {

                            Dictionary<long, TimeSpan> tempDictionary = new Dictionary<long, TimeSpan>();
                            TimeSpan empLeaveTime;
                            TimeSpan differenceShiftTime_LeaveTime;

                            #region Find nearest Shift 
                            // this code use for get Employee nearest Shif
                            foreach (EmployeeShift EmpShift in EmployeeShiftList.Where(x => x.IdEmployee == LeavesPerEmployee[record].idEmployee).ToList())
                            {
                                try
                                {
                                    if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Monday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.MonStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.MonStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.MonStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Tuesday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.TueStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.TueStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.TueStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Wednesday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.WedStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.WedStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.WedStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Thursday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.ThuStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.ThuStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.ThuStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);

                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Friday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.FriStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.FriStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.FriStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Saturday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;

                                        if (empLeaveTime >= EmpShift.CompanyShift.SatStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.SatStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.SatStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                    else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                                    {
                                        empLeaveTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                        if (empLeaveTime >= EmpShift.CompanyShift.SunStartTime)
                                            differenceShiftTime_LeaveTime = empLeaveTime - EmpShift.CompanyShift.SunStartTime;
                                        else
                                            differenceShiftTime_LeaveTime = EmpShift.CompanyShift.SunStartTime - empLeaveTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_LeaveTime);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }

                            long keyOfMinValue = 0;
                            if (tempDictionary.Count > 0)
                                keyOfMinValue = tempDictionary.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;

                            #endregion

                            #region Get Employee Shift Start Time And End Time
                            EmployeeShift employeeShift = EmployeeShiftList.Where(x => x.IdCompanyShift == keyOfMinValue).FirstOrDefault();

                            TimeSpan shiftStartTime = new TimeSpan();
                            TimeSpan shiftEndTime = new TimeSpan();
                            DateTime shiftStartDateTime = new DateTime();
                            DateTime shiftEndDateTime = new DateTime();

                            if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Monday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.MonEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Tuesday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.ThuEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Wednesday.ToString())
                            {

                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.WedEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Thursday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.ThuEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Friday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.FriEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Saturday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.SatEndTime;
                            }
                            else if (LeavesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                            {
                                shiftStartTime = LeavesPerEmployee[record].Time.TimeOfDay;
                                shiftEndTime = employeeShift.CompanyShift.SunEndTime;
                            }

                            if (employeeShift.CompanyShift.IsNightShift == 1)
                            {
                                shiftStartDateTime = LeavesPerEmployee[record].Time.Date.Add(shiftStartTime);
                                shiftEndDateTime = LeavesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime);
                            }
                            else
                            {
                                shiftStartDateTime = LeavesPerEmployee[record].Time.Date.Add(shiftStartTime);
                                var getEndDateTime = DateTime.Now.Date;
                                TimeSpan empty = new TimeSpan(0, 0, 0);
                                if (shiftEndTime == empty)
                                {
                                    getEndDateTime = LeavesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime).AddHours(3);
                                }
                                else
                                {
                                    getEndDateTime = LeavesPerEmployee[record].Time.Date.Add(shiftEndTime).AddHours(3);
                                }
                                shiftEndDateTime = getEndDateTime;
                                TimeSpan time = new TimeSpan(23, 59, 59);
                                if (shiftEndTime != empty)
                                {
                                    if (getEndDateTime.TimeOfDay > time)
                                    {
                                        shiftEndDateTime = LeavesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime);
                                    }
                                    else
                                    {
                                        if (shiftEndTime.Hours >= 23)
                                        {
                                            shiftEndDateTime = LeavesPerEmployee[record].Time.Date.Add(shiftEndTime).AddHours(3);
                                        }
                                        else
                                        {
                                            shiftEndDateTime = LeavesPerEmployee[record].Time.Add(shiftEndTime).AddHours(-3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Set In/Out For Night Shift Employee 


                            var GetShiftEntryList = LeavesPerEmployee.Where(x => x.Time >= shiftStartDateTime && x.Time <= shiftEndDateTime.AddHours(3)).ToList();
                            if (GetShiftEntryList.Count == 1)
                            {
                                GetShiftEntryList[0].IsRowError = true;
                                i = LeavesSheetData.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                                continue;
                            }

                            for (int count = 0; count < GetShiftEntryList.Count; count++)
                            {
                                if (count == 0)
                                {
                                    var inRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(LeaveInTolerance.Minute));
                                    inRecords.First().Start = true;
                                    inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                    inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                    count = GetShiftEntryList.IndexOf(inRecords.Last());
                                    inRecords.First().End = false;
                                    continue;
                                }
                                if (GetShiftEntryList[count - 1].End)
                                {
                                    var inRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(LeaveInTolerance.Minute));

                                    if (GetShiftEntryList.Count != inRecords.Count() + count)
                                    {
                                        inRecords.First().Start = true;
                                        inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                        inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(inRecords.Last());
                                        inRecords.First().End = false;
                                    }
                                    else
                                    {
                                        inRecords.First().Start = false;
                                        inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                        inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(inRecords.Last());
                                        inRecords.First().End = false;
                                    }
                                }
                                else
                                {
                                    if (this.UseFirstAndLastDayRecordInLeaveImport)
                                    {
                                        var outRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(LeaveOutTolerance.Minute));
                                        if (GetShiftEntryList.Exists(a => a.Start == true))
                                        {
                                            GetShiftEntryList.Last().End = true;
                                            GetShiftEntryList.Last().CompanyShift = employeeShift.CompanyShift;
                                            GetShiftEntryList.Last().AccountingDate = GetShiftEntryList[0].Time.Date;
                                            count = GetShiftEntryList.IndexOf(GetShiftEntryList.Last());
                                            GetShiftEntryList.Last().Start = false;
                                        }
                                    }
                                    else
                                    {
                                        var outRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(LeaveOutTolerance.Minute));
                                        outRecords.Last().End = true;
                                        outRecords.Last().CompanyShift = employeeShift.CompanyShift;
                                        outRecords.Last().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(outRecords.Last());
                                        outRecords.Last().Start = false;
                                    }
                                }
                            }
                            record = LeavesPerEmployee.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                        }

                        i = LeavesSheetData.IndexOf(LeavesPerEmployee[LeavesPerEmployee.Count - 1]);
                        #endregion

                    }
                    else
                    {

                        #region Set  Start/End Leave For Non Morining shift and afternoon shift Employee 
                        int LastRecordOfEmployee = 0; 
                        if (LeavesPerEmployee.Count == 1)
                        {
                            LeavesPerEmployee[0].IsRowError = true;
                            i = LeavesSheetData.IndexOf(LeavesPerEmployee[LeavesPerEmployee.Count - 1]);
                            continue;
                        }
                        else
                        {
                            for (int nonNightShiftRecordCount = 0; nonNightShiftRecordCount < LeavesPerEmployee.Count; nonNightShiftRecordCount++)
                            {
                                if (nonNightShiftRecordCount > 0 && LeavesPerEmployee[nonNightShiftRecordCount - 1].Start && LeavesPerEmployee[nonNightShiftRecordCount - 1].Date.Date != LeavesPerEmployee[nonNightShiftRecordCount].Date.Date)
                                {
                                    LeavesPerEmployee[nonNightShiftRecordCount - 1].IsRowError = true;
                                    LeavesPerEmployee[nonNightShiftRecordCount - 1].Start = false;
                                }
                                if (nonNightShiftRecordCount == 0 || LeavesPerEmployee[nonNightShiftRecordCount - 1].IsRowError)
                                {
                                    var inRecords = LeavesPerEmployee.Where(s => s.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID && s.Time >= LeavesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= LeavesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(LeaveInTolerance.Minute));
                                    inRecords.First().Start = true;
                                    nonNightShiftRecordCount = LeavesPerEmployee.IndexOf(inRecords.Last());
                                    inRecords.First().AccountingDate = inRecords.First().Time.Date;

                                    continue;
                                }


                                if (LeavesPerEmployee[nonNightShiftRecordCount - 1].End)
                                {
                                    var inRecords = LeavesPerEmployee.Where(s => s.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID && s.Time >= LeavesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= LeavesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(LeaveInTolerance.Minute));
                                    inRecords.First().Start = true;
                                    if (nonNightShiftRecordCount == LeavesPerEmployee.Count() - 1)
                                    {
                                        LeavesPerEmployee[nonNightShiftRecordCount].Start = false;
                                    }
                                    inRecords.First().AccountingDate = inRecords.First().Time.Date;
                                    nonNightShiftRecordCount = LeavesPerEmployee.IndexOf(inRecords.Last());

                                    LastRecordOfEmployee = LeavesPerEmployee.IndexOf(inRecords.First()) - 1;
                                }
                                else
                                {
                                    if (this.UseFirstAndLastDayRecordInLeaveImport)
                                    {
                                        var outRecords = LeavesPerEmployee.Where(s => s.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID && s.Time >= LeavesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= LeavesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(LeaveOutTolerance.Minute));
                                        var currentDateRecords = LeavesPerEmployee.Where(s => s.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID && s.Time.Date == LeavesPerEmployee[nonNightShiftRecordCount].Time.Date).ToList();
                                        if (currentDateRecords.Exists(a => a.Start == true))
                                        {
                                            currentDateRecords.Last().End = true;
                                            currentDateRecords.Last().AccountingDate = currentDateRecords.Last().Time.Date;
                                            nonNightShiftRecordCount = LeavesPerEmployee.IndexOf(currentDateRecords.Last());
                                            LastRecordOfEmployee = nonNightShiftRecordCount;
                                        }
                                        else
                                        {
                                            LeavesPerEmployee[nonNightShiftRecordCount - 1].IsRowError = true;

                                            currentDateRecords.First().Start = true;
                                            if (nonNightShiftRecordCount == LeavesPerEmployee.Count() - 1)
                                            {
                                                LeavesPerEmployee[nonNightShiftRecordCount].Start = false;
                                            }
                                            currentDateRecords.First().AccountingDate = currentDateRecords.First().Time.Date;

                                            currentDateRecords.Last().End = true;
                                            currentDateRecords.Last().AccountingDate = currentDateRecords.Last().Time.Date;
                                            nonNightShiftRecordCount = LeavesPerEmployee.IndexOf(currentDateRecords.Last());
                                            LastRecordOfEmployee = nonNightShiftRecordCount;
                                        }
                                    }
                                    else
                                    {
                                        var outRecords = LeavesPerEmployee.Where(s => s.Time >= LeavesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= LeavesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(LeaveOutTolerance.Minute));
                                        outRecords.Last().End = true;
                                        outRecords.Last().AccountingDate = outRecords.Last().Time.Date;
                                        nonNightShiftRecordCount = LeavesPerEmployee.IndexOf(outRecords.Last());
                                        LastRecordOfEmployee = nonNightShiftRecordCount;
                                    }
                                }
                            }
                        }
                        var inRecords_Last = LeavesPerEmployee.Where(s => s.EmployeeClockTimeID == LeavesSheetData[i].EmployeeClockTimeID && s.Time >= LeavesPerEmployee[LastRecordOfEmployee].Time && s.Time <= LeavesPerEmployee[LastRecordOfEmployee].Time.AddMinutes(LeaveInTolerance.Minute)).ToList();

                        if (inRecords_Last.Exists(a => a.Start == true) && !inRecords_Last.Exists(a => a.End == true))
                        {
                            if (inRecords_Last.Count > 1)
                            {
                                LastRecordOfEmployee = LeavesPerEmployee.IndexOf(inRecords_Last.Last());
                                LeavesPerEmployee[LastRecordOfEmployee].IsRowError = true;
                            }
                        }

                        i = LeavesSheetData.IndexOf(LeavesPerEmployee[LeavesPerEmployee.Count - 1]);
                        #endregion
                    }
                }

                GeosApplication.Instance.Logger.Log("ReadImportedLeaveViewModel Method SetNightShiftStartEnd()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedLeaveViewModel Method SetNightShiftStartEnd()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }
        
        private void SetShiftDataToShowInGridcolumnShift()
        {
            foreach (EmployeeLeaves item in LeavesSheetData)
            {
                if (item.CompanyShiftList == null)
                    item.CompanyShiftList = new ObservableCollection<CompanyShift>();
                    if (!item.CompanyShiftList.Contains(companyShiftNone))
                        item.CompanyShiftList.Insert(0, companyShiftNone);

                if (item.CompanyShiftList != null && item.CompanyShiftList.Count >= 1 && item.CompanyShift != null)
                {
                    item.CompanyShift = item.CompanyShiftList.First(x => x.IdCompanyShift == item.CompanyShift.IdCompanyShift);
                }
                else if (item.CompanyShiftList != null && item.CompanyShiftList.Count == 2)
                {
                    if (item.Start == true || item.End == true)
                    {
                        item.CompanyShift = item.CompanyShiftList[1];
                    }
                }

                if (item.CompanyShift == null)
                {
                    item.CompanyShift = item.CompanyShiftList[0];
                }
            }

            EmployeeLeaves inRecord = new EmployeeLeaves();
            // bool errorFound = false;
            for (int i = 0; i < LeavesSheetData.Count; i++)
            {
                if (LeavesSheetData[i].Start)
                    inRecord = LeavesSheetData[i];

                if (LeavesSheetData[i].End && inRecord != null)
                {
                    string error = LeavesSheetData[i].EnableValidationAndGetError(inRecord);
                    //if (!string.IsNullOrEmpty(error))
                    //    errorFound = true;
                }
            }
        }
        #endregion
    }
}
