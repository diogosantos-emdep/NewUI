using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Emdep.Geos.Data.Common.Hrm;
using System.ServiceModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.XtraSpreadsheet.API.Native.Implementation;
using System.Dynamic;
using DevExpress.Xpf.Editors;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Modules.Hrm.CommonClass;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;
using System.Linq;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using ExcelDataReader.Core;
using Emdep.Geos.Utility;
using ExcelDataReader;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Text;
using System.Reflection;
using System.Globalization;


namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class ImportLeaveFileViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// [rdixit][13.07.2022][GEOS2-3697][[ZK_TIME] Add a new option “Import Leave” in the LEAVES section]
        /// </summary>

        #endregion

        #region Service
        protected IOpenFileDialogService OpenFileDialogService { get { return this.GetService<IOpenFileDialogService>(); } }
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        private bool isSave;
        private ObservableCollection<ImportLeaves> employeeLeaves = new ObservableCollection<ImportLeaves>();
        private List<IDictionary<string, object>> reportData;
        ObservableCollection<string> exelSheetNames;
        private DateTime filterStartDate;
        private DateTime filterEndDate;
        bool IsBusy;
        ObservableCollection<string> textSeparators;
        private int sheetNameSelectedIndex;
        private int dsnSelectedIndex;
        ObservableCollection<string> dsnTableNames;
        private DataTable filteredTable { get; set; }
        int dataSourceSelectedIndex;
        string sheetNameSelectedItem;
        private Dictionary<string, int> matchedIndex { get; set; }
        ObservableCollection<string> dsns;
        private ObservableCollection<Attachment> attachmentFiles;
        private int attachedFileIndex;
        private byte[] fileInBytes;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private List<string> idsLeaveType;
        public enum SQL_INFO
        {
            DATA_SOURCE_NAME,
            DRIVER_NAME,
            DRIVER_VER,
            ODBC_VER,
            SERVER_NAME,
            SEARCH_PATTERN_ESCAPE,
            DBMS_NAME,
            DBMS_VER,
            IDENTIFIER_CASE,
            IDENTIFIER_QUOTE_CHAR,
            CATALOG_NAME_SEPARATOR,
            DRIVER_ODBC_VER,
            GROUP_BY,
            KEYWORDS,
            ORDER_BY_COLUMNS_IN_SELECT,
            QUOTED_IDENTIFIER_CASE,
            SQL_OJ_CAPABILITIES_30,
            SQL_SQL92_RELATIONAL_JOIN_OPERATORS,
            SQL_OJ_CAPABILITIES_20
        }

        #endregion

        #region Properties
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }
        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }
        public byte[] FileInBytes
        {
            get { return fileInBytes; }
            set
            {
                fileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileInBytes"));
            }
        }
        public string SheetNameSelectedItem
        {
            get { return sheetNameSelectedItem; }
            set
            {
                sheetNameSelectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedItem"));
            }
        }
        bool IsMatchTypeWithDataSourceField { get; set; }
        public DateTime FilterEndDate
        {
            get { return filterEndDate; }
            set { filterEndDate = value; OnPropertyChanged(new PropertyChangedEventArgs("FilterEndDate")); }
        }
        public List<EmployeeLeave> EmployeeLeavesList { get; set; }
        public ObservableCollection<string> Dsns
        {
            get { return dsns; }
            set
            {
                dsns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dsns"));
            }
        }
        public ObservableCollection<string> TextSeparators
        {
            get { return textSeparators; }
            set
            {
                textSeparators = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextSeparators"));
            }
        }
        public int DataSourceSelectedIndex
        {
            get { return dataSourceSelectedIndex; }
            set
            {
                dataSourceSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataSourceSelectedIndex"));
            }
        }
        public int SheetNameSelectedIndex
        {
            get { return sheetNameSelectedIndex; }
            set
            {
                sheetNameSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
            }
        }
        public int DsnSelectedIndex
        {
            get { return dsnSelectedIndex; }
            set
            {
                dsnSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DsnSelectedIndex"));
            }
        }
        public DateTime FilterStartDate
        {
            get { return filterStartDate; }
            set { filterStartDate = value; OnPropertyChanged(new PropertyChangedEventArgs("FilterStartDate")); }
        }
        public ObservableCollection<ImportLeaves> EmployeeLeaves
        {
            get { return employeeLeaves; }
            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
        }
        public ObservableCollection<string> ExelSheetNames
        {
            get { return exelSheetNames; }
            set
            {
                exelSheetNames = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExelSheetNames"));
            }
        }
        public List<IDictionary<string, object>> ReportData
        {
            get { return reportData; }
            set
            {
                reportData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReportData"));
            }
        }
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
        public ObservableCollection<string> DsnTableNames
        {
            get { return dsnTableNames; }
            set
            {
                dsnTableNames = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DsnTableNames"));
            }
        }
        int StringSplit { get; set; }
        public ObservableCollection<Attachment> AttachmentFiles
        {
            get { return attachmentFiles; }
            set
            {
                attachmentFiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentFiles"));
            }
        }
        public List<EmployeeLeave> EmpAddedLeavesList { get; set; }
        public int AttachedFileIndex
        {
            get { return attachedFileIndex; }
            set
            {
                attachedFileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFileIndex"));
            }
        }

        // 002 geos_app_setting
        string DateFormatGeosAppSetting { get; set; }
        String[] DateFormatGeosAppSettingArray { get; set; }

        string TimeFormatGeosAppSetting { get; set; }
        String[] TimeFormatGeosAppSettingArray { get; set; }

        private List<CompanySetting> DateFormatCompanySettings { get; set; }
        private List<CompanySetting> TimeFormatCompanySettings { get; set; }
        private List<PostgreSQLdb> PostgreSQLdbList { get; set; }
        #endregion

        #region Command
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }
        public ICommand ChooseFileCommandForLeaves { get; set; }
        public ICommand DataSourceSelectedIndexChanged { get; set; }
        public ICommand SheetNameSelectedIndexChanged { get; set; }
        public ICommand ShowSourceColumnCustomFilterPopupCommand { get; set; }
        public ICommand FilterStartDateValueChangedCommand { get; set; }
        public ICommand FilterEndDateValueChangedCommand { get; set; }
        public ICommand NextWindowCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
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

        #region Constructor
        public ImportLeaveFileViewModel()
        {
            SetUserPermission();
            SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ChooseFileCommandForLeaves = new RelayCommand(new Action<object>(BrowseFileAction));
            DataSourceSelectedIndexChanged = new DelegateCommand<EditValueChangedEventArgs>(DataSourceIndexChanged);
            SheetNameSelectedIndexChanged = new DelegateCommand<EditValueChangedEventArgs>(SheetNameIndexChanged);
            ShowSourceColumnCustomFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(ShowSourceColumnCustomFilterPopupCommandAction);
            FilterEndDateValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(DateChangeValidation);
            FilterStartDateValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(DateChangeValidation);
            NextWindowCommand = new RelayCommand(new Action<object>(Next));
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

            //[001]Added Code
            //string TypeCompaniesids = CrmStartUp.GetGeosAppSettings(86).DefaultValue;
            string TypeCompaniesids = CrmStartUp.GetGeosAppSettings(91).DefaultValue;
            string[] idsTypeCompany = TypeCompaniesids.Split(',').ToArray();
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && HrmCommon.Instance.SelectedAuthorizedPlantsList.Count > 0)
            {
                HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().FirstOrDefault();
                if (idsTypeCompany.Any(i => i == HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().FirstOrDefault().IdCompany.ToString()))
                {
                    IsMatchTypeWithDataSourceField = true;
                   
                }
            }

            if (IsMatchTypeWithDataSourceField)
            {
                string ImportLeavesCodes = CrmStartUp.GetGeosAppSettings(90).DefaultValue;
                string[] ArrayImportLeavesCodes = ImportLeavesCodes.Split(',').ToArray();
                Dictionary<string, string> LstImportLeaveCodes = new Dictionary<string, string>();
                if (LstImportLeaveCodes == null)
                {
                    LstImportLeaveCodes = new Dictionary<string, string>();
                }
                foreach (string item in ArrayImportLeavesCodes)
                {
                    string[] tempCodes = null;
                    if (item.Contains(';'))
                        tempCodes = item.Split(';').ToArray();
                    if (tempCodes.Count() == 2 && tempCodes[0] != null && tempCodes[1] != null)
                        LstImportLeaveCodes.Add(tempCodes[0].Replace('(', ' ').Trim(), tempCodes[1].Replace(')', ' ').Trim());
                }
                if (LstImportLeaveCodes != null && LstImportLeaveCodes.Count > 0)
                {
                    idsLeaveType = LstImportLeaveCodes.Select(i => (string.IsNullOrEmpty(i.Value.TrimStart(new Char[] { '0' })) ? "0" : i.Value.TrimStart(new Char[] { '0' }))).ToList();
                }
            }
        }
        #endregion

        #region Methods
        private void SetUserPermission()
        {
            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }

        /// <summary>
        /// Method for Custome Filter on Source COlumn
        /// </summary>
        /// <param name="obj"></param>

        public void ShowSourceColumnCustomFilterPopupCommandAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method ShowSourceColumnCustomFilterPopupCommandAction ...", category: Category.Info, priority: Priority.Low);
            if (e.Column.FieldName != "EmployeeLeavesImportField")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "EmployeeLeavesImportField")
                {
                    foreach (ImportLeaves item in EmployeeLeaves)
                    {
                        foreach (var sourceCol in item.EmployeeLeavesImportFieldList)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == sourceCol.Name.ToString()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = sourceCol.Name.ToString();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeLeavesImportField Like '%{0}%'", sourceCol.Name.ToString()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method ShowSourceColumnCustomFilterPopupCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowSourceColumnCustomFilterPopupCommandAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// data source changed
        /// </summary>
        /// <param name="obj"></param>
        public void DataSourceIndexChanged(EditValueChangedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method DataSourceIndexChanged() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj.NewValue == null)
                {
                    AttachmentFiles = new ObservableCollection<Attachment>();
                    EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                    ExelSheetNames = new ObservableCollection<string>();
                }
                else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
                {
                    AttachmentFiles = new ObservableCollection<Attachment>();
                    EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                    ExelSheetNames = new ObservableCollection<string>();
                    ReportData = new List<IDictionary<string, object>>();
                    DsnTableNames = new ObservableCollection<string>();
                    SetUserSetting(obj);
                }
                if (DataSourceSelectedIndex.Equals(2))
                {
                    FillTableNameFromDsn();
                    FillTableFromDsn(HowManyRecordsToBeFetched.Single);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DataSourceIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method DataSourceIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void Init(ObservableCollection<EmployeeLeave> employeeLeavesList)
        {
            GeosApplication.Instance.Logger.Log("Method Init() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                EmployeeLeavesList = employeeLeavesList.ToList();
                AttachmentFiles = new ObservableCollection<Attachment>();
                TextSeparators = new ObservableCollection<string>();
                ExelSheetNames = new ObservableCollection<string>();
                ReportData = new List<IDictionary<string, object>>();
                matchedIndex = new Dictionary<string, int>();
                Dsns = new ObservableCollection<string>();
                DsnTableNames = new ObservableCollection<string>();
                FilterStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                FilterEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                EmpAddedLeavesList = new List<Data.Common.Hrm.EmployeeLeave>();
                LoadSavedSettings();
                FillSeparators();
                FillDsns();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        void LoadSavedSettings()
        {
            GeosApplication.Instance.Logger.Log("Method LoadSavedSettings() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveDataSourceSelectedIndex"))
                {
                    int SelectedIndex;
                    int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveDataSourceSelectedIndex"], out SelectedIndex);
                    DataSourceSelectedIndex = SelectedIndex;
                }
                else
                    DataSourceSelectedIndex = 0;

                if (DataSourceSelectedIndex == 1)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveTextSeparator"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveTextSeparator"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }

                if (DataSourceSelectedIndex == 2)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcDns"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveOdbcDns"], out SelectedIndex);
                        DsnSelectedIndex = SelectedIndex;

                    }
                    else
                        DsnSelectedIndex = 0;

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcTableName"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveOdbcTableName"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                        string ImportLeavesSourceFieldSelectedIndex = string.Empty;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadSavedSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method LoadSavedSettings() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Separators For text files u can add new here if u want
        /// </summary>
        public void FillSeparators()
        {
            GeosApplication.Instance.Logger.Log("Method FillSeparators() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                TextSeparators.AddRange(new List<string> { "Tab", "Comma", "SemiColon", "Space" });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSeparators()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method FillSeparators() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void FillDsns()
        {
            Dsns.AddRange(EnumDsn(Registry.CurrentUser));
            Dsns.AddRange(EnumDsn(Registry.LocalMachine));
        }
        private IEnumerable<string> EnumDsn(RegistryKey rootKey)
    {
        RegistryKey regKey = rootKey.OpenSubKey(@"Software\ODBC\ODBC.INI\ODBC Data Sources");
        if (regKey != null)
        {
            foreach (string name in regKey.GetValueNames())
            {
                string value = regKey.GetValue(name, "").ToString();
                yield return name;
            }
        }
    }
        /// <summary>
        /// Selected File SelectedItemChanged
        /// </summary>
        /// <param name="obj"></param>
        public void SelectedItemChangedCommandAction(EditValueChangedEventArgs obj)
    {
        try
        {
            GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
            if (obj.NewValue == null)
            {
                EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                ExelSheetNames = new ObservableCollection<string>();
            }
            else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
            {
                ReportData = new List<IDictionary<string, object>>();
                EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                DsnTableNames = new ObservableCollection<string>();
                SetUserSetting(obj);
            }

            if (DataSourceSelectedIndex.Equals(2))
            {
                FillTableNameFromDsn();
                FillTableFromDsn(HowManyRecordsToBeFetched.Single);
            }

            GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            GeosApplication.Instance.Logger.Log("Get an error in Method SelectedItemChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
        public void SetUserSetting(EditValueChangedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserSetting() ...", category: Category.Info, priority: Priority.Low);

                if (obj.NewValue.ToString() == "Text")
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveTextSeparator"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveTextSeparator"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }
                else if (obj.NewValue.ToString() == "ODBC")
                {

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcDns"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveOdbcDns"], out SelectedIndex);
                        DsnSelectedIndex = SelectedIndex;
                    }
                    else
                        DsnSelectedIndex = 0;

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcTableName"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeaveOdbcTableName"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }

                GeosApplication.Instance.Logger.Log("Method SetUserSetting() executed successfully ...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillTableNameFromDsn()
        {
            try
            {
                string dbsmName = string.Empty;
                GeosApplication.Instance.Logger.Log("Method FillTableNameFromDsn() ...", category: Category.Info, priority: Priority.Low);
                string connectionString = "Dsn=" + Dsns[DsnSelectedIndex] + ";Trusted_Connection=Yes;";

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {

                    connection.Open();
                    dbsmName = GetInfoOfDBMS(connection, SQL_INFO.DBMS_NAME);
                    DataSet ds = new DataSet();
                    OdbcCommand oComm = new OdbcCommand();
                    DataTable tables = connection.GetSchema("Tables");
                    DataTable viewDatatable = connection.GetSchema("Views");
                    if (dbsmName == "Microsoft SQL Server")
                    {
                        try
                        {
                            if (!viewDatatable.AsEnumerable().Any(X => X["TABLE_NAME"].ToString().Trim().ToUpper() == "GHRMCHECKINOUT_VIEW"))
                            {

                                oComm.CommandText = "create view GHRMCHECKINOUT_VIEW as (select inf.[Badgenumber] ,ch.[CHECKTIME]  as PunchDate,ch.[CHECKTIME]  as PunchTime  From [CHECKINOUT] as ch  inner join [USERINFO] as inf on ch.USERID=inf.USERID)";
                                oComm.Connection = connection;
                                OdbcDataAdapter oAdapter = new OdbcDataAdapter(oComm);
                                oAdapter.Fill(ds);
                                viewDatatable = connection.GetSchema("Views");
                            }

                            DataRow row = viewDatatable.AsEnumerable().Where(X => X["TABLE_NAME"].ToString().ToUpper() == "GHRMCHECKINOUT_VIEW").FirstOrDefault();
                            if (row != null)
                            {
                                DsnTableNames.Add(row["TABLE_NAME"].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Sql Server while creating view GHRMCHECKINOUT_VIEW in Method FillTableNameFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    else if (dbsmName == "MySQL")
                    {
                        try
                        {
                            if (viewDatatable.AsEnumerable().Any(X => X["TABLE_NAME"].ToString().ToUpper() == "GHRMCHECKINOUT_VIEW"))
                            {
                                DataRow row = viewDatatable.AsEnumerable().Where(X => X["TABLE_NAME"].ToString().ToUpper() == "GHRMCHECKINOUT_VIEW").FirstOrDefault();
                                if (row != null)
                                {
                                    DsnTableNames.Add(row["TABLE_NAME"].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in MysQL server while Loading view in Method FillTableNameFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }

                }

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {

                    connection.Open();
                    dbsmName = GetInfoOfDBMS(connection, SQL_INFO.DBMS_NAME);
                    DataTable tables = connection.GetSchema("Tables");
                    DataTable viewDatatable = connection.GetSchema("Views");
                    if (dbsmName == "EXCEL")
                    {
                        foreach (DataRow row in tables.Rows)
                        {
                            DsnTableNames.Add(row["TABLE_NAME"].ToString().Replace("$", "").Replace("'", ""));
                        }
                    }
                    else if (dbsmName == "ACCESS")
                    {
                        foreach (DataRow row in tables.Rows)
                        {
                            if (row["TABLE_NAME"].ToString().Substring(0, 4) != "MSys")
                                DsnTableNames.Add(row["TABLE_NAME"].ToString());
                        }
                    }
                    else
                    {

                        foreach (DataRow row in tables.Rows)
                        {
                            DsnTableNames.Add(row["TABLE_NAME"].ToString());
                            try
                            {
                                if (PostgreSQLdbList == null)
                                {
                                    PostgreSQLdbList = new List<PostgreSQLdb>();
                                }
                                PostgreSQLdb postgreSQLdb = new PostgreSQLdb();
                                if (!string.IsNullOrEmpty(row["TABLE_NAME"].ToString()))
                                {
                                    postgreSQLdb.TABLE_NAME = row["TABLE_NAME"].ToString();
                                }
                                if (!string.IsNullOrEmpty(row["TABLE_SCHEM"].ToString()))
                                {
                                    postgreSQLdb.TABLE_SCHEM = row["TABLE_SCHEM"].ToString();
                                }
                                if (!string.IsNullOrEmpty(row["TABLE_CAT"].ToString()))
                                {
                                    postgreSQLdb.TABLE_CAT = row["TABLE_CAT"].ToString();
                                }
                                if (!string.IsNullOrEmpty(row["TABLE_TYPE"].ToString()))
                                {
                                    postgreSQLdb.TABLE_TYPE = row["TABLE_TYPE"].ToString();
                                }
                                if (!string.IsNullOrEmpty(row["REMARKS"].ToString()))
                                {
                                    postgreSQLdb.REMARKS = row["REMARKS"].ToString();
                                }
                                PostgreSQLdbList.Add(postgreSQLdb);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in Method FillTableNameFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTableNameFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillTableNameFromDsn() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public string GetInfoOfDBMS(OdbcConnection ocn, SQL_INFO info)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetInfoOfDBMS() ...", category: Category.Info, priority: Priority.Low);
                MethodInfo GetInfoStringUnhandled = ocn.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(c => c.Name == "GetInfoStringUnhandled");
                ParameterInfo SQL_INFO = GetInfoStringUnhandled.GetParameters().First(c => (c.ParameterType.ToString() == "System.Data.Odbc.ODBC32+SQL_INFO"));
                Array EnumValues = SQL_INFO.ParameterType.GetEnumValues();
                foreach (var enumval in EnumValues)
                {
                    if (enumval.ToString() == info.ToString())
                    {
                        return Convert.ToString(GetInfoStringUnhandled.Invoke(ocn, new object[] { enumval }));
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetInfoOfDBMS()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method GetInfoOfDBMS() executed successfully", category: Category.Info, priority: Priority.Low);
            return string.Empty;
        }
        private void FillTableFromDsn()
        {
            GeosApplication.Instance.Logger.Log("Method FillTableFromDsn() ...", category: Category.Info, priority: Priority.Low);
            string dbsmName = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                string cs = "Dsn=" + Dsns[DsnSelectedIndex] + ";Trusted_Connection=Yes;";
                using (OdbcConnection cn = new OdbcConnection(cs))
                {
                    cn.Open();
                    dbsmName = GetInfoOfDBMS(cn, SQL_INFO.DBMS_NAME);
                    OdbcCommand oComm = new OdbcCommand();
                    if (dbsmName == "EXCEL")
                    {
                        oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "$]";
                    }
                    else if (dbsmName == "MySQL")
                    {
                        oComm.CommandText = "Select * From " + DsnTableNames[SheetNameSelectedIndex] + "";
                    }
                    else if (dbsmName == "PostgreSQL")
                    {
                        try
                        {
                            // PostgreSQL: Use double quotes for table names to handle case sensitivity.
                            oComm.CommandText = "SELECT * FROM \"" + DsnTableNames[SheetNameSelectedIndex] + "\"";
                            if (PostgreSQLdbList != null)
                            {
                                if (PostgreSQLdbList.Any(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])))
                                {
                                    PostgreSQLdb postgreSQLdb = PostgreSQLdbList.Where(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])).FirstOrDefault();
                                    if (postgreSQLdb != null)
                                    {
                                        if (!string.IsNullOrEmpty(postgreSQLdb.TABLE_SCHEM))
                                        {
                                            oComm.CommandText = $"SELECT * FROM \"{postgreSQLdb.TABLE_SCHEM}\".\"{DsnTableNames[SheetNameSelectedIndex]}\"";
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                    }

                    oComm.Connection = cn;
                    OdbcDataAdapter oAdapter = new OdbcDataAdapter(oComm);
                    oAdapter.Fill(ds);

                }

                FillSourceIngrid(ds.Tables[0]);
                CalculateIndexForGridFromDsn();

                filteredTable = ds.Tables[0];

                SheetNameSelectedItem = DsnTableNames[SheetNameSelectedIndex];
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillTableFromDsn() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void FillSourceIngrid(DataTable dataTable)
        {
            GeosApplication.Instance.Logger.Log("Method FillSourceIngrid() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                List<string> TargetfieldNew = new List<string>();
                TargetfieldNew.Add("EmployeeClockTimeID");
                TargetfieldNew.Add("EmployeeCompanyEmail");

                List<string> Targetfield = new List<string>();
                employeeLeaves = new ObservableCollection<ImportLeaves>();
                Targetfield.Add("EmployeeClockTimeID"); Targetfield.Add("Date"); Targetfield.Add("Time");
                ////[001]Added Code
                ////string TypeCompaniesids = CrmStartUp.GetGeosAppSettings(86).DefaultValue;
                //string TypeCompaniesids = CrmStartUp.GetGeosAppSettings(91).DefaultValue;
                //string[] idsTypeCompany = TypeCompaniesids.Split(',').ToArray();
                //if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && HrmCommon.Instance.SelectedAuthorizedPlantsList.Count > 0)
                //{
                //    HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().FirstOrDefault();
                //    if (idsTypeCompany.Any(i => i == HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().FirstOrDefault().IdCompany.ToString()))
                //    {
                //        IsMatchTypeWithDataSourceField = true;
                //        Targetfield.Add("Type");
                //    }
                //}


                if(IsMatchTypeWithDataSourceField)
                {
                    Targetfield.Add("Type");
                }
            
                Targetfield.Add("EmployeeCompanyEmail");



                for (int k = 0; k < Targetfield.Count - 1; k++)
                {
                    ImportLeaves tmpEmpLeaves = new ImportLeaves();
                    tmpEmpLeaves.TargetFieldList = TargetfieldNew;
                    tmpEmpLeaves.Column = Targetfield[k].ToString();

                    tmpEmpLeaves.EmployeeLeavesImportFieldList = new List<EmployeeLeavesImportField>();
                    tmpEmpLeaves.EmployeeLeavesTargetFieldList = new List<EmployeeLeavesImportField>();
                    foreach (System.Data.DataRow item2 in dataTable.Rows)
                    {
                        for (int i = 0; i < Targetfield.Count; i++)
                        {
                            tmpEmpLeaves.EmployeeLeavesTargetFieldList.Add(new EmployeeLeavesImportField { Name = Targetfield[i].ToString() });
                        }
                        break;
                    }
                    foreach (System.Data.DataRow item2 in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            tmpEmpLeaves.EmployeeLeavesImportFieldList.Add(new EmployeeLeavesImportField { Name = item2[i].ToString() });
                        }

                        break;
                    }
                    EmployeeLeaves.Add(tmpEmpLeaves);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSourceIngrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillSourceIngrid() executed successfully...", category: Category.Info, priority: Priority.Low);
        }
        private void FillTableFromDsn(HowManyRecordsToBeFetched howManyRecordsToBeFetched) // enum single record, date range, all
        {
            GeosApplication.Instance.Logger.Log("Method FillTableFromDsn() ...", category: Category.Info, priority: Priority.Low);
            string dbsmName = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                string cs = "Dsn=" + Dsns[DsnSelectedIndex] + ";Trusted_Connection=Yes;";
                GeosApplication.Instance.Logger.Log($"Method FillTableFromDsn() OdbcConnection={cs}", category: Category.Info, priority: Priority.Low);
                using (OdbcConnection cn = new OdbcConnection(cs))
                {
                    cn.Open();
                    dbsmName = GetInfoOfDBMS(cn, SQL_INFO.DBMS_NAME); // Microsoft SQL Server
                    GeosApplication.Instance.Logger.Log($"Method FillTableFromDsn() Server Name={dbsmName}", category: Category.Info, priority: Priority.Low);
                    OdbcCommand oComm = new OdbcCommand();

                    switch (howManyRecordsToBeFetched)
                    {
                        case HowManyRecordsToBeFetched.Single:

                            switch (dbsmName)
                            {
                                case "EXCEL":
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "$]";
                                    break;
                                case "MySQL":
                                    oComm.CommandText = "Select * From " + DsnTableNames[SheetNameSelectedIndex] + "  LIMIT 1 ";
                                    break;
                                case "Microsoft SQL Server":
                                    oComm.CommandText = "Select TOP 1 * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                                    break;
                                case "PostgreSQL":
                                    try
                                    {
                                        // PostgreSQL: Use LIMIT 1 to get the first row
                                        oComm.CommandText = "SELECT * FROM \"" + DsnTableNames[SheetNameSelectedIndex] + "\" LIMIT 1";
                                        if (PostgreSQLdbList != null)
                                        {
                                            if (PostgreSQLdbList.Any(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])))
                                            {
                                                PostgreSQLdb postgreSQLdb = PostgreSQLdbList.Where(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])).FirstOrDefault();
                                                if (postgreSQLdb != null)
                                                {
                                                    if (!string.IsNullOrEmpty(postgreSQLdb.TABLE_SCHEM))
                                                    {
                                                        oComm.CommandText = $"SELECT * FROM \"{postgreSQLdb.TABLE_SCHEM}\".\"{DsnTableNames[SheetNameSelectedIndex]}\" LIMIT 1";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

                                    }
                                    break;
                                default:
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                                    break;
                            }

                            break;
                        case HowManyRecordsToBeFetched.WithinSelectedDateRange:

                            // Get date column index in datatable filteredTable
                            var EmployeeLeaveImportField = EmployeeLeaves[0].EmployeeLeavesImportFieldList.FirstOrDefault(x => x.Name == "Date");
                            int? indexOfDateColumnInDatatableFilteredTable = null;
                            indexOfDateColumnInDatatableFilteredTable = matchedIndex.FirstOrDefault(x => x.Key == "Date").Value;

                            string NameOfDateColumnInDatatableFilteredTable = string.Empty;
                            if (indexOfDateColumnInDatatableFilteredTable != null)
                            {
                                NameOfDateColumnInDatatableFilteredTable = filteredTable.Columns[indexOfDateColumnInDatatableFilteredTable.Value].ColumnName;
                            }

                            switch (dbsmName)
                            {
                                case "EXCEL":
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "$]";
                                    break;
                                case "MySQL":
                                    oComm.CommandText = "Select * From " + DsnTableNames[SheetNameSelectedIndex] + " WHERE " +
                                        NameOfDateColumnInDatatableFilteredTable + " BETWEEN CAST('" + FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss") +
                                        "' AS DATE) AND CAST('" + FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATE) ";

                                    // Example - oComm.CommandText = "Select * From fichajes3 WHERE Fecha BETWEEN CAST('2022-03-01' AS DATE) AND CAST('2022-03-31' AS DATE)";
                                    break;
                                case "Microsoft SQL Server":
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "] WHERE " +
                                        NameOfDateColumnInDatatableFilteredTable + " BETWEEN '" + FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss") +
                                        "' AND '" + FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                                    break;
                                case "PostgreSQL":
                                    try
                                    {
                                        // PostgreSQL: Use the BETWEEN clause for date filtering and double quotes for table names
                                        oComm.CommandText = "SELECT * FROM \"" + DsnTableNames[SheetNameSelectedIndex] + "\" WHERE " +
                                            NameOfDateColumnInDatatableFilteredTable + " BETWEEN '" + FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss") +
                                            "' AND '" + FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                        if (PostgreSQLdbList != null)
                                        {
                                            var postgreSQLdb = PostgreSQLdbList.FirstOrDefault(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex], StringComparison.OrdinalIgnoreCase));
                                            if (postgreSQLdb != null && !string.IsNullOrEmpty(postgreSQLdb.TABLE_SCHEM))
                                            {
                                                oComm.CommandText = $"SELECT * FROM \"{postgreSQLdb.TABLE_SCHEM}\".\"{DsnTableNames[SheetNameSelectedIndex]}\" " +
                                                $"WHERE \"{NameOfDateColumnInDatatableFilteredTable}\" " + $"BETWEEN '{FilterStartDate:yyyy-MM-dd HH:mm:ss}' " + $"AND '{FilterEndDate:yyyy-MM-dd HH:mm:ss}'";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    }
                                    break;
                                default:
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                                    break;
                            }

                            break;
                        case HowManyRecordsToBeFetched.All:

                            switch (dbsmName)
                            {
                                case "EXCEL":
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "$]";
                                    break;
                                case "MySQL":
                                    oComm.CommandText = "Select * From " + DsnTableNames[SheetNameSelectedIndex] + "  ";
                                    break;
                                case "Microsoft SQL Server":
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                                    break;
                                case "PostgreSQL":
                                    try
                                    {
                                        // PostgreSQL queries: Use double quotes for table names (case-sensitive).
                                        oComm.CommandText = "SELECT * FROM \"" + DsnTableNames[SheetNameSelectedIndex] + "\"";
                                        if (PostgreSQLdbList != null)
                                        {
                                            if (PostgreSQLdbList.Any(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])))
                                            {
                                                PostgreSQLdb postgreSQLdb = PostgreSQLdbList.Where(a => a.TABLE_NAME.Equals(DsnTableNames[SheetNameSelectedIndex])).FirstOrDefault();
                                                if (postgreSQLdb != null)
                                                {
                                                    if (!string.IsNullOrEmpty(postgreSQLdb.TABLE_SCHEM))
                                                    {
                                                        oComm.CommandText = $"SELECT * FROM \"{postgreSQLdb.TABLE_SCHEM}\".\"{DsnTableNames[SheetNameSelectedIndex]}\"";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    }
                                    break;
                                default:
                                    oComm.CommandText = "Select * From [" + DsnTableNames[SheetNameSelectedIndex] + "]";
                                    break;
                            }

                            break;
                        default:
                            break;
                    }

                    GeosApplication.Instance.Logger.Log($"Method FillTableFromDsn() CommandText={oComm.CommandText}", category: Category.Info, priority: Priority.Low);


                    oComm.Connection = cn;
                    OdbcDataAdapter oAdapter = new OdbcDataAdapter(oComm);
                    oAdapter.Fill(ds);
                    // Check ds.Tables[0].Rows.Count, while debugging
                    GeosApplication.Instance.Logger.Log($"Method FillTableFromDsn() Fetched Rows Count={ds.Tables[0].Rows.Count}", category: Category.Info, priority: Priority.Low);

                }

                FillSourceIngrid(ds.Tables[0]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CalculateIndexForGridFromDsn();
                }
                filteredTable = ds.Tables[0];

                SheetNameSelectedItem = DsnTableNames[SheetNameSelectedIndex];
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTableFromDsn()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillTableFromDsn() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //To calculate the index for grid Combobox items 
        private void CalculateIndexForGridFromDsn()
        {
            int seletedIndexCount = 0;
            GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridFromDsn() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                int[] importLeavesSourceField = new int[] { 0, 0, 0 ,0 };
                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveSourceFieldSelectedIndex") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ImportLeaveSourceFieldSelectedIndex"]))
                {
                    string importLeaveSourceFieldSelectedIndex = GeosApplication.Instance.UserSettings["ImportLeaveSourceFieldSelectedIndex"];
                    string[] importLeaveSourceFieldSelectedIndexArray = importLeaveSourceFieldSelectedIndex.Split(',');

                    for (int item = 0; item < importLeaveSourceFieldSelectedIndexArray.Length; item++)
                        int.TryParse(importLeaveSourceFieldSelectedIndexArray[item], out importLeavesSourceField[item]);

                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0 && EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > importLeavesSourceField[i])
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[importLeavesSourceField[i]];
                            else
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }
                        seletedIndexCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0)
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }

                        seletedIndexCount++;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridFromDsn() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CalculateIndexForGridFromDsn() Method  " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CalculateIndexForGridText()
        {
            int seletedIndexCount = 0;
            GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridText() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                int[] importLeavesSourceField = new int[] { 0, 0, 0,0 };

                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveTextSourceFieldSelectedIndex") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ImportLeaveTextSourceFieldSelectedIndex"]))
                {
                    string ImportLeavesTextSourceFieldSelectedIndex = GeosApplication.Instance.UserSettings["ImportLeaveTextSourceFieldSelectedIndex"];
                    string[] ImportLeavesTextSourceFieldSelectedIndexArray = ImportLeavesTextSourceFieldSelectedIndex.Split(',');

                    for (int item = 0; item < ImportLeavesTextSourceFieldSelectedIndexArray.Length; item++)
                        int.TryParse(ImportLeavesTextSourceFieldSelectedIndexArray[item], out importLeavesSourceField[item]);

                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0 && EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > importLeavesSourceField[i])
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[importLeavesSourceField[i]];
                            else
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }
                        seletedIndexCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0)
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }

                        seletedIndexCount++;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridText() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CalculateIndexForGridText() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CalculateIndexForGridExcel()
        {
            int seletedIndexCount = 0;
            GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridExcel() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (DataSourceSelectedIndex == 0)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeavesExcelSheetname"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportLeavesExcelSheetname"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }

                int[] importLeaveSourceField = new int[] { 0, 0, 0 ,0 };

                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveExcelSourceFieldSelectedIndex") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ImportLeaveExcelSourceFieldSelectedIndex"]))
                {
                    string ImportLeaveExcelSourceFieldSelectedIndex = GeosApplication.Instance.UserSettings["ImportLeaveExcelSourceFieldSelectedIndex"];
                    string[] ImportLeaveExcelSourceFieldSelectedIndexArray = ImportLeaveExcelSourceFieldSelectedIndex.Split(',');

                    for (int item = 0; item < ImportLeaveExcelSourceFieldSelectedIndexArray.Length; item++)
                        int.TryParse(ImportLeaveExcelSourceFieldSelectedIndexArray[item], out importLeaveSourceField[item]);

                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0 && EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > importLeaveSourceField[i])
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[importLeaveSourceField[i]];
                            else
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }
                        seletedIndexCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < EmployeeLeaves.Count; i++)
                    {
                        if (seletedIndexCount < EmployeeLeaves.Count)
                        {
                            if (EmployeeLeaves[i].EmployeeLeavesImportFieldList.Count > 0)
                                EmployeeLeaves[i].EmployeeLeavesImportField = EmployeeLeaves[i].EmployeeLeavesImportFieldList[seletedIndexCount];
                        }

                        seletedIndexCount++;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CalculateIndexForGridExcel() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// To close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// Sheet name SelectedItem chaged
        /// </summary>
        /// <param name="obj"></param>
        public void SheetNameIndexChanged(EditValueChangedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SheetNameIndexChanged() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ReportData = new List<IDictionary<string, object>>();

                if (obj.NewValue == null)
                {
                    EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                }
                else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
                {
                    EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                }

                if (DataSourceSelectedIndex.Equals(0))
                {
                    SheetNameSelectedIndex = ExelSheetNames.IndexOf(obj.NewValue.ToString());
                    FetchColumnsFromExcel(AttachmentFiles[0]);
                }

                if (DataSourceSelectedIndex.Equals(1))
                {
                    if (AttachmentFiles.Count > 0)
                        FetchColumnsFromText(AttachmentFiles[0]);
                }

                if (DataSourceSelectedIndex.Equals(2))
                {
                    FillTableFromDsn(HowManyRecordsToBeFetched.Single);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SheetNameIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method SheetNameIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// get colums from text file 
        /// </summary>
        /// <param name="attachedFile"></param>
        public void FetchColumnsFromText(Attachment attachedFile)
        {
            try
            {
                System.Data.DataTable SelectedSheet = new DataTable();
                EmployeeLeaves = new ObservableCollection<ImportLeaves>();

                GeosApplication.Instance.Logger.Log("Method FetchColumnsFromText() ...", category: Category.Info, priority: Priority.Low);
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

                DataTable data = ConvertTextFileToDataTable(attachedFile.FilePath);
                if (data == null)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
                    OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
                    return;
                }

                FillSourceIngrid(data);
                filteredTable = data.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable();
                AttachmentFiles.Clear();
                AttachmentFiles.Add(attachedFile);
                AttachedFileIndex = 0;
                CalculateIndexForGridText();

                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FetchColumnsFromExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FetchColumnsFromExcel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FetchColumnsFromExcel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// get the coloums from xls file from Attachment
        /// </summary>
        /// <param name="attachedFile"></param>
        public void FetchColumnsFromExcel(Attachment attachedFile)
        {
            try
            {
                System.Data.DataTable SelectedSheet = new DataTable();

                EmployeeLeaves = new ObservableCollection<ImportLeaves>();

                if (SheetNameSelectedIndex == -1)
                    SheetNameSelectedIndex = 0;

                GeosApplication.Instance.Logger.Log("Method FetchColumnsFromExcel() ...", category: Category.Info, priority: Priority.Low);

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

                int LoopCount = 0;
                using (var stream = File.Open(attachedFile.FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                        do
                        {
                            while (reader.Read())
                            {
                                //Getting Seleted Sheet
                                if (reader.Name.Equals(ExelSheetNames[SheetNameSelectedIndex]))
                                {
                                    var result = reader.AsDataSet();
                                    //Getting Seleted Sheet
                                    SelectedSheet = result.Tables[ExelSheetNames[SheetNameSelectedIndex]];

                                    if (SelectedSheet.Rows.Count > 0)
                                    {
                                        FillSourceIngrid(SelectedSheet);
                                        //Deleteing First coloum as the first coloum is xls coloum names
                                        SelectedSheet.Rows[0].Delete();
                                        SelectedSheet.AcceptChanges();
                                        //Removeing unwanted rows
                                        //[rdixit][GEOS2-5065][16.01.2024]
                                        filteredTable = SelectedSheet.Rows.Cast<DataRow>().Where(row => (row.ItemArray.Any(field => !(field is System.DBNull))) && (!string.IsNullOrWhiteSpace(row.ItemArray[2].ToString()))).CopyToDataTable();
                                    }
                                    else
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LeavesSheetBrowseFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        EmployeeLeaves = new ObservableCollection<ImportLeaves>();
                                    }

                                    break;
                                }
                                break;
                            }

                            reader.NextResult();
                            LoopCount++;
                        } while (ExelSheetNames.Count != LoopCount);
                }

                AttachmentFiles.Clear();
                AttachmentFiles.Add(attachedFile);
                AttachedFileIndex = 0;
                CalculateIndexForGridExcel();

                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FetchColumnsFromExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FetchColumnsFromExcel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FetchColumnsFromExcel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// Get the selected seperatior for datasource text file
        /// </summary>
        /// <returns></returns>
        private string SelectedSeparator()
        {
            try
            {
                if (TextSeparators[SheetNameSelectedIndex].Equals("Tab")) return "\t";
                if (TextSeparators[SheetNameSelectedIndex].Equals("Comma")) return ",";
                if (TextSeparators[SheetNameSelectedIndex].Equals("SemiColon")) return ";";
                if (TextSeparators[SheetNameSelectedIndex].Equals("Space")) return " ";
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedSeparator() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }

        /// <summary>
        /// Converting Text file in to Datatable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataTable ConvertTextFileToDataTable(string filePath)
        {
            GeosApplication.Instance.Logger.Log("Method ConvertTextFileToDataTable() ...", category: Category.Info, priority: Priority.Low);
            DataTable dataTable = new DataTable();
            string separator = SelectedSeparator();

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                var content = System.IO.File.ReadAllText(filePath).Replace(separator, ",");
                int numberOfColumns = lines[0].Split(char.Parse(separator)).Count();
                StringSplit = numberOfColumns;
                // File is in wrong for mate

                if (StringSplit.Equals(1))
                {
                    CustomMessageBox.Show("File is in wrong format", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    dataTable = null;
                    return dataTable;
                }

                for (int colum = 0; colum < numberOfColumns; colum++)
                {
                    dataTable.Columns.Add(new DataColumn("Column" + (colum + 1).ToString()));
                }

                foreach (string line in lines)
                {
                    var cols = line.Split(char.Parse(separator)).ToList();
                    if (StringSplit != cols.Count)
                        continue;
                    DataRow dr = dataTable.NewRow();
                    for (int columIndex = 0; columIndex < numberOfColumns; columIndex++)
                    {
                        dr[columIndex] = cols[columIndex].Trim();
                    }
                    dataTable.Rows.Add(dr);
                }

                GeosApplication.Instance.Logger.Log("Method ConvertTextFileToDataTable() executed successfully ...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                if (ex.Message.Contains("Index was outside the bounds"))
                {
                    StringSplit = 1;
                    CustomMessageBox.Show("File is in wrong format", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    dataTable = null;
                }
                GeosApplication.Instance.Logger.Log("Get an error in ConvertTextFileToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return dataTable;
        }

        /// <summary>
        /// To browse files
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            AttachmentFiles.Clear();
            ExelSheetNames = new ObservableCollection<string>();

            try
            {
                string ResultFileName;

                if (DataSourceSelectedIndex.Equals(0))
                    OpenFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|Excel Files (.xls)|*.xls";

                if (DataSourceSelectedIndex.Equals(1))
                    OpenFileDialogService.Filter = "Text Files|*.txt";

                OpenFileDialogService.FilterIndex = 1;
                bool DialogResult = OpenFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                    return;
                }
                else
                {
                    ResultFileName = (OpenFileDialogService.File).DirectoryName + "\\" + (OpenFileDialogService.File).Name;
                    FileInBytes = System.IO.File.ReadAllBytes(ResultFileName);
                    FileInfo file = new FileInfo(ResultFileName);
                    Attachment Attachment = new Attachment();
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = file.Name;
                    Attachment.IsDeleted = false;
                    Attachment.FileByte = FileInBytes;
                    AttachmentFiles.Insert(0, Attachment);
                    

                    if (DataSourceSelectedIndex.Equals(0))
                    {
                        FillSheetNames(Attachment);//[rdixit][GEOS2-3844][21.07.2022]
                        FetchColumnsFromExcel(Attachment);
                        SheetNameSelectedIndex = 0;
                    }

                    if (DataSourceSelectedIndex.Equals(1))
                    {
                        FetchColumnsFromText(Attachment);
                    }


                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                CustomMessageBox.Show(ex.Message, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// To get all the sheets available in xls sheet
        /// </summary>
        /// <param name="attachedFile"></param>
        public void FillSheetNames(Attachment attachedFile)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSheetNames() ...", category: Category.Info, priority: Priority.Low);
                ExelSheetNames.Clear();
                using (var stream = File.Open(attachedFile.FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            ExelSheetNames.Add(reader.Name);
                        } while (reader.NextResult());
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSheetNames()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillSheetNames() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Next button click
        /// [001][rdixit][12.07.2022][GEOS2-3697]
        /// </summary>
        /// <param name="obj"></param>
        private void Next(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NextWindow()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                matchedIndex = new Dictionary<string, int>();
                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFileIndex"));
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
                OnPropertyChanged(new PropertyChangedEventArgs("FilterEndDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStartDate"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
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

                    DevExpress.Xpf.Grid.GridControl GridImportLeaves = (DevExpress.Xpf.Grid.GridControl)obj;
                    if (IsMatchTypeWithDataSourceField)
                    {
                        for (int item = 0; item < 4; item++)
                        {
                            var DataGridRow = (ImportLeaves)GridImportLeaves.GetRow(item);
                            matchedIndex.Add(DataGridRow.Column, DataGridRow.EmployeeLeavesImportFieldList.IndexOf(DataGridRow.EmployeeLeavesImportField));
                        }
                    }
                    else
                    {
                        for (int item = 0; item < 3; item++)
                        {
                            var DataGridRow = (ImportLeaves)GridImportLeaves.GetRow(item);
                            matchedIndex.Add(DataGridRow.Column, DataGridRow.EmployeeLeavesImportFieldList.IndexOf(DataGridRow.EmployeeLeavesImportField));
                        }
                    }

                    GeosApplication.Instance.UserSettings["ImportLeaveDataSourceSelectedIndex"] = DataSourceSelectedIndex.ToString();

                    //[001] added
                    if (DataSourceSelectedIndex == 0)
                    {
                        if (SheetNameSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportLeavesExcelSheetname"] = SheetNameSelectedIndex.ToString();

                        string ImportLeavesExcelSourceFieldSelectedIndex = string.Empty;

                        for (int item = 0; item < EmployeeLeaves.Count; item++)
                        {
                            var DataGridRow = (ImportLeaves)GridImportLeaves.GetRow(item);
                            int index = DataGridRow.EmployeeLeavesImportFieldList.IndexOf(DataGridRow.EmployeeLeavesImportField);
                            ImportLeavesExcelSourceFieldSelectedIndex += index.ToString() + ",";
                        }
                        ImportLeavesExcelSourceFieldSelectedIndex = ImportLeavesExcelSourceFieldSelectedIndex.Remove(ImportLeavesExcelSourceFieldSelectedIndex.Length - 1, 1);
                        GeosApplication.Instance.UserSettings["ImportLeaveExcelSourceFieldSelectedIndex"] = ImportLeavesExcelSourceFieldSelectedIndex;
                    }
                    if (DataSourceSelectedIndex == 1)
                    {
                        if (SheetNameSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportLeaveTextSeparator"] = SheetNameSelectedIndex.ToString();

                        string ImportLeavesTextSourceFieldSelectedIndex = string.Empty;

                        for (int item = 0; item < EmployeeLeaves.Count; item++)
                        {
                            var DataGridRow = (ImportLeaves)GridImportLeaves.GetRow(item);
                            int index = DataGridRow.EmployeeLeavesImportFieldList.IndexOf(DataGridRow.EmployeeLeavesImportField);
                            ImportLeavesTextSourceFieldSelectedIndex += index.ToString() + ",";
                        }
                        ImportLeavesTextSourceFieldSelectedIndex = ImportLeavesTextSourceFieldSelectedIndex.Remove(ImportLeavesTextSourceFieldSelectedIndex.Length - 1, 1);
                        GeosApplication.Instance.UserSettings["ImportLeaveTextSourceFieldSelectedIndex"] = ImportLeavesTextSourceFieldSelectedIndex;
                    }

                    if (DataSourceSelectedIndex == 2)
                    {
                        if (DsnSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportLeaveOdbcDns"] = DsnSelectedIndex.ToString();

                        if (SheetNameSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportLeaveOdbcTableName"] = SheetNameSelectedIndex.ToString();

                        string ImportLeavesSourceFieldSelectedIndex = string.Empty;

                        for (int item = 0; item < EmployeeLeaves.Count; item++)
                        {
                            var DataGridRow = (ImportLeaves)GridImportLeaves.GetRow(item);
                            int index = DataGridRow.EmployeeLeavesImportFieldList.IndexOf(DataGridRow.EmployeeLeavesImportField);
                            ImportLeavesSourceFieldSelectedIndex += index.ToString() + ",";
                        }

                        ImportLeavesSourceFieldSelectedIndex = ImportLeavesSourceFieldSelectedIndex.Remove(ImportLeavesSourceFieldSelectedIndex.Length - 1, 1);
                        GeosApplication.Instance.UserSettings["ImportLeaveSourceFieldSelectedIndex"] = ImportLeavesSourceFieldSelectedIndex;
                    }
                    

                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                    FilterEndDate = FilterEndDate.AddHours(23).AddMinutes(59);
                    if (DataSourceSelectedIndex.Equals(2))
                    {
                        FillTableFromDsn(HowManyRecordsToBeFetched.WithinSelectedDateRange);
                    }
                    FillReports(filteredTable);

                    ReadImportedLeaveView readImportedLeaveView = new ReadImportedLeaveView();
                    ReadImportedLeaveViewModel readImportedLeaveViewModel = new ReadImportedLeaveViewModel();
                    readImportedLeaveViewModel.IsMatchTypeWithDataSourceField = IsMatchTypeWithDataSourceField;
                    readImportedLeaveViewModel.FilterStartDate = FilterStartDate;
                    readImportedLeaveViewModel.FilterEndDate = FilterEndDate;
                    readImportedLeaveViewModel.Init(EmployeeLeaves, ReportData, EmployeeLeavesList);
                    EventHandler handle = delegate { readImportedLeaveView.Close(); };
                    readImportedLeaveViewModel.RequestClose += handle;
                    readImportedLeaveView.DataContext = readImportedLeaveViewModel;

                    if (readImportedLeaveViewModel.MappingFailMessage != string.Empty)
                        CustomMessageBox.Show(readImportedLeaveViewModel.MappingFailMessage, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    else
                        readImportedLeaveView.ShowDialog();

                    IsSave = readImportedLeaveViewModel.IsSave;

                    if (readImportedLeaveViewModel.NewEmployeeLeavesList != null)
                        EmpAddedLeavesList.AddRange(readImportedLeaveViewModel.NewEmployeeLeavesList);
                    if (IsSave)
                    {
                        RequestClose(null, null);
                    }
                    else
                    {
                        if (DataSourceSelectedIndex.Equals(2))
                        {
                            FillTableFromDsn();
                            LoadSavedSettings();
                        }
                    }
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method NextWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method NextWindow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// To fill the the data into ReportData
        /// </summary>
        /// <param name="dataTable"></param>
        public void FillReports(DataTable dataTable)
        {

            GeosApplication.Instance.Logger.Log("Method FillReports() ...", category: Category.Info, priority: Priority.Low);
            List<string> empIds = new List<string>();
            List<IDictionary<string, object>> LocalReportData = new List<IDictionary<string, object>>();
            ReportData = new List<IDictionary<string, object>>();
            string EmpDocumentNumbers = string.Empty;

            try
            {
                foreach (System.Data.DataRow item2 in dataTable.Rows)
                {
                    try
                    {
                        IDictionary<string, object> row = new Dictionary<string, object>();

                        foreach (var matchIndex in matchedIndex)
                        {
                            if (item2.IsNull(matchIndex.Value))
                                continue;

                            row.Add(matchIndex.Key, item2.ItemArray[matchIndex.Value]);
                        }

                        if (row.Count > 0)
                        {
                            if (IsMatchTypeWithDataSourceField && row.Keys.Contains("Type") && row.FirstOrDefault(x => x.Key.Equals("Type")).Value != null)
                            {
                                if (idsLeaveType.Contains((string.IsNullOrEmpty(row.FirstOrDefault(x => x.Key.Equals("Type")).Value.ToString().TrimStart(new Char[] { '0' })) ? "0" : row.FirstOrDefault(x => x.Key.Equals("Type")).Value.ToString().TrimStart(new Char[] { '0' }))))
                                {
                                    if (row.FirstOrDefault(x => x.Key.Equals("EmployeeClockTimeID")).Value != null)
                                        empIds.Add(row.FirstOrDefault(x => x.Key.Equals("EmployeeClockTimeID")).Value.ToString());

                                    if (row.FirstOrDefault(x => x.Key.Equals("EmployeeCompanyEmail")).Value != null)
                                        empIds.Add(row.FirstOrDefault(x => x.Key.Equals("EmployeeCompanyEmail")).Value.ToString());
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (row.FirstOrDefault(x => x.Key.Equals("EmployeeClockTimeID")).Value != null)
                                    empIds.Add(row.FirstOrDefault(x => x.Key.Equals("EmployeeClockTimeID")).Value.ToString());

                                if (row.FirstOrDefault(x => x.Key.Equals("EmployeeCompanyEmail")).Value != null)
                                    empIds.Add(row.FirstOrDefault(x => x.Key.Equals("EmployeeCompanyEmail")).Value.ToString());
                            }
                            LocalReportData.Add(row);
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method FillReports()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

                empIds = empIds.Distinct().ToList();
                empIds.ForEach(x => { EmpDocumentNumbers = x + "," + EmpDocumentNumbers; });
                EmpDocumentNumbers = EmpDocumentNumbers.TrimEnd(',');

                List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                // [002] Changed service method GetEmpDtlByEmpDocNoAndPeriod_V2039 to GetEmpDtlByEmpDocNoAndPeriod_V2041
                // List<Employee> Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2036(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);

                List<Employee> Employees = new List<Employee>();
                if (matchedIndex.Any(a => a.Key.Equals("EmployeeClockTimeID")))
                {
                    Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2090(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                }
                else
                {
                    Employees = HrmService.GetEmpDtlByEmailNoAndPeriod_V2260(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                }


                Employee employee;

                //002
                //geos_app_settings
                DateFormatGeosAppSetting = CrmStartUp.GetGeosAppSettings(12).DefaultValue;
                DateFormatGeosAppSettingArray = DateFormatGeosAppSetting.Split(';').ToArray();

                TimeFormatGeosAppSetting = CrmStartUp.GetGeosAppSettings(13).DefaultValue;
                TimeFormatGeosAppSettingArray = TimeFormatGeosAppSetting.Split(';').ToArray();
                //company_settings
                DateFormatCompanySettings = HrmService.GetCompanySettingByIdCompany(12, string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().Select(Company => Company.IdCompany)), HrmCommon.Instance.SelectedPeriod);
                TimeFormatCompanySettings = HrmService.GetCompanySettingByIdCompany(13, string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().Select(Company => Company.IdCompany)), HrmCommon.Instance.SelectedPeriod);

                //[001] Added
                // get Employee Contract
                List<IDictionary<string, object>> tempReportData = new List<IDictionary<string, object>>();

                if (Employees != null && Employees.Count > 0)
                {
                    foreach (var item in Employees)
                    {
                        if (item.EmployeeContractSituations != null && item.EmployeeContractSituations.Count > 0)
                        {
                            foreach (var items in item.EmployeeContractSituations)
                            {
                                List<IDictionary<string, object>> ReportsData = new List<IDictionary<string, object>>();
                                if (matchedIndex.Any(a => a.Key.Equals("EmployeeClockTimeID")))
                                {
                                    ReportsData = LocalReportData.Where(x => x.FirstOrDefault(l => l.Key == "EmployeeClockTimeID").Value.ToString() == item.EmployeeDocument.EmployeeDocumentNumber &&
                                                                                 DateConversion(x.FirstOrDefault(l => l.Key == "Date").Value.ToString()) >= items.ContractSituationStartDate.Value.Date &&
                                                                                 DateConversion(x.FirstOrDefault(l => l.Key == "Date").Value.ToString()) <= (items.ContractSituationEndDate == null ?
                                                                                 GeosApplication.Instance.ServerDateTime.Date : ((items.ContractSituationEndDate) > GeosApplication.Instance.ServerDateTime.Date ?
                                                                                 GeosApplication.Instance.ServerDateTime.Date : items.ContractSituationEndDate))).ToList();
                                }
                                else
                                {
                                    ReportsData = LocalReportData.Where(x => x.FirstOrDefault(l => l.Key == "EmployeeCompanyEmail").Value.ToString() == item.EmployeeDocument.EmployeeDocumentNumber &&
                                                                                 DateConversion(x.FirstOrDefault(l => l.Key == "Date").Value.ToString()) >= items.ContractSituationStartDate.Value.Date &&
                                                                                 DateConversion(x.FirstOrDefault(l => l.Key == "Date").Value.ToString()) <= (items.ContractSituationEndDate == null ?
                                                                                 GeosApplication.Instance.ServerDateTime.Date : ((items.ContractSituationEndDate) > GeosApplication.Instance.ServerDateTime.Date ?
                                                                                 GeosApplication.Instance.ServerDateTime.Date : items.ContractSituationEndDate))).ToList();
                                }


                                tempReportData.AddRange(ReportsData);
                            }
                        }
                    }
                }
                //end

                foreach (var row in tempReportData)
                {
                    #region  removing unwanted data

                    object dateValue = row.FirstOrDefault(x => x.Key == "Date").Value;
                    object employeeClockTimeID = row.FirstOrDefault(x => x.Key == "EmployeeClockTimeID" || x.Key == "EmployeeCompanyEmail").Value;
                    object timeValue = row.FirstOrDefault(x => x.Key == "Time").Value;
                    object type = null;

                    if (IsMatchTypeWithDataSourceField)
                        type = row.FirstOrDefault(x => x.Key == "Type").Value;

                    if (dateValue == null || employeeClockTimeID == null || timeValue == null || (IsMatchTypeWithDataSourceField == true ? type == null : false))
                    {
                        continue;
                    }

                    string leaveDate = row.FirstOrDefault(x => x.Key == "Date").Value.ToString();
                    string leaveTime = row.FirstOrDefault(x => x.Key == "Time").Value.ToString();

                    employee = Employees.FirstOrDefault(x => x.EmployeeDocument.EmployeeDocumentNumber.Equals(row.ElementAt(0).Value.ToString()));

                    if (employee != null)
                    {
                        DateTime? LeaveDateTime = DateTimeConversion(leaveDate, leaveTime, employee);

                        if (!LeaveDateTime.HasValue)
                        {
                            continue;
                        }

                        //The “End Date” (date) are the final date (23:59:59) to find records in the Source.(we are considering all records on that date with = dont have to add hrs)
                        if (!(LeaveDateTime.Value >= FilterStartDate && LeaveDateTime.Value <= FilterEndDate))
                        {
                            continue;
                        }

                        row.Add("Converted_Date_Time", LeaveDateTime);
                    }
                    else
                    {
                        continue;
                    }

                    #endregion

                    if (Employees.Any(x => x.EmployeeDocument.EmployeeDocumentNumber.Equals(row.ElementAt(0).Value.ToString())))
                    {
                        var employeeName = Employees.FirstOrDefault(x => x.EmployeeDocument.EmployeeDocumentNumber.Equals(row.ElementAt(0).Value.ToString())).FullName;
                        row.Add("EmployeeName", employeeName);
                    }
                    else
                    {
                        row.Add("EmployeeName", string.Empty);
                    }

                    row.Add("Employee", Employees.FirstOrDefault(x => x.EmployeeDocument.EmployeeDocumentNumber.Equals(row.ElementAt(0).Value.ToString())));
                    ReportData.Add(row);
                }

                GeosApplication.Instance.Logger.Log("Method FillReports() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReports() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReports() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillReports()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is created for dateconversion when checking within contract dates.
        /// </summary>
        /// <param name="leaveDate">The date from odbc</param>
        /// <returns>If successfully converted then datetime else null.</returns>
        private DateTime? DateConversion(string leaveDate)
        {
            DateTime? LeaveDateTime = null;
            DateTime resultDate;

            try
            {
                // Default Conversion
                if (DateTime.TryParse(leaveDate, CultureInfo.CurrentCulture, DateTimeStyles.None, out resultDate))
                {
                    LeaveDateTime = resultDate;
                }

                // Custom geos_app_settings Conversion
                try
                {
                    if (resultDate == DateTime.MinValue)
                    {
                        if (DateTime.TryParseExact(leaveDate, DateFormatGeosAppSettingArray, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate))
                        {
                            LeaveDateTime = resultDate;
                        }
                        else
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Unable to parse date geos_app_settings {0} Method DateConversion().", leaveDate), category: Category.Warn, priority: Priority.Low);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in ImportLeaveFileViewModel Method DateTimeConversion() - geos_app_settings - {0}.", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ImportLeaveFileViewModel Method DateTimeConversion()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return LeaveDateTime;
        }

        private DateTime? DateTimeConversion(string leaveDate, string leaveTime, Employee employee)
        {
            DateTime? LeaveDateTime = null;
            DateTime resultDate;
            TimeSpan resultTime;

            try
            {
                #region Default Conversion

                if (DateTime.TryParse(leaveDate, CultureInfo.CurrentCulture, DateTimeStyles.None, out resultDate))
                {
                    LeaveDateTime = resultDate;

                    if (TimeSpan.TryParse(leaveTime, CultureInfo.CurrentCulture, out resultTime))
                    {
                        //sometimes through system it is converting wrong time like 061500 to 165 days so added this condition.
                        if (resultTime.Hours != 0 && resultTime.Minutes != 0 && resultTime.Seconds != 0)
                        {
                            LeaveDateTime = new DateTime(resultDate.Date.Year, resultDate.Date.Month, resultDate.Date.Day,
                                                              resultTime.Hours, resultTime.Minutes, resultTime.Seconds);
                            return LeaveDateTime;
                        }
                    }

                    //This is added only for excel because when i parse time formatted cell from excel is always like this e.g. "1899/12/31 12:30:00".
                    DateTime excelResultDateTime;
                    if (DateTime.TryParse(leaveTime, CultureInfo.CurrentCulture, DateTimeStyles.None, out excelResultDateTime))
                    {
                        DateTime excelMinDate = new DateTime(1899, 12, 31);
                        if (DateTime.Compare(excelResultDateTime.Date, excelMinDate) == 0)
                        {
                            LeaveDateTime = new DateTime(resultDate.Date.Year, resultDate.Date.Month, resultDate.Date.Day,
                                                              excelResultDateTime.Hour, excelResultDateTime.Minute, excelResultDateTime.Second);
                            return LeaveDateTime;
                        }
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Unable to parse excel ResultDateTime {0} Method DateTimeConversion().", LeaveDateTime), category: Category.Warn, priority: Priority.Low);
                    }
                }

                #endregion

                #region Custom geos_app_settings Conversion

                try
                {
                    if (resultDate == DateTime.MinValue)
                    {
                        if (DateTime.TryParseExact(leaveDate, DateFormatGeosAppSettingArray, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate))
                        {
                            LeaveDateTime = resultDate;
                        }
                        else
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Unable to parse date geos_app_settings {0} Method DateTimeConversion().", leaveDate), category: Category.Warn, priority: Priority.Low);
                        }
                    }

                    if (leaveTime.Contains(' ')) leaveTime = leaveTime.Split(' ')[1];

                    if (TimeSpan.TryParseExact(leaveTime, TimeFormatGeosAppSettingArray, CultureInfo.InvariantCulture, TimeSpanStyles.None, out resultTime))
                    {
                        LeaveDateTime = new DateTime(resultDate.Date.Year, resultDate.Date.Month, resultDate.Date.Day,
                                                          resultTime.Hours, resultTime.Minutes, resultTime.Seconds);
                        return LeaveDateTime;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Unable to parse time geos_app_settings {0} Method DateTimeConversion().", leaveTime), category: Category.Warn, priority: Priority.Low);
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in ReadImportedLeaveViewModel Method DateTimeConversion() - geos_app_settings - {0}.", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                #endregion

                #region Custom company_settings Conversion

                try
                {
                    // company_settings
                    int IdCompany = 0;
                    if (!string.IsNullOrEmpty(employee.EmployeeCompanyIds))
                    {
                        List<Int32> IdCompanies = employee.EmployeeCompanyIds.Split(',').Select(Int32.Parse).ToList();
                        IdCompany = IdCompanies.First();
                    }

                    //Formate.Split(';').ToArray();
                    string Formate = string.Empty;

                    if (resultDate == DateTime.MinValue)
                    {
                        if (DateFormatCompanySettings.Any(x => x.IdCompany == IdCompany))
                            Formate = DateFormatCompanySettings.FirstOrDefault(x => x.IdCompany == IdCompany).Value;

                        if (DateTime.TryParseExact(leaveDate, Formate.Split(';').ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate))
                        {
                            LeaveDateTime = resultDate;
                        }
                        else
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Unable to parse date company_settings {0} Method DateTimeConversion().", leaveDate), category: Category.Warn, priority: Priority.Low);
                        }
                    }

                    if (TimeFormatCompanySettings.Any(x => x.IdCompany == IdCompany))
                        Formate = TimeFormatCompanySettings.FirstOrDefault(x => x.IdCompany == IdCompany).Value;

                    if (leaveTime.Contains(' ')) leaveTime = leaveTime.Split(' ')[1];

                    if (TimeSpan.TryParseExact(leaveTime, Formate.Split(';').ToArray(), CultureInfo.InvariantCulture, TimeSpanStyles.None, out resultTime))
                    {
                        LeaveDateTime = new DateTime(resultDate.Date.Year, resultDate.Date.Month, resultDate.Date.Day,
                                                          resultTime.Hours, resultTime.Minutes, resultTime.Seconds);
                        return LeaveDateTime;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Unable to parse time company_settings {0} Method DateTimeConversion().", leaveTime), category: Category.Warn, priority: Priority.Low);
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in ReadImportedLeaveViewModel Method DateTimeConversion() - company_settings - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ReadImportedLeaveViewModel Method DateTimeConversion()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return LeaveDateTime;
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
        enum HowManyRecordsToBeFetched
        {
            Single,
            WithinSelectedDateRange,
            All
        }
        #endregion

        #region Validation
        public void DateChangeValidation(EditValueChangedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method DateChangeValidation() ...", category: Category.Info, priority: Priority.Low);

            if (obj.OldValue != null)
            {
                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("FilterEndDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStartDate"));
            }

            GeosApplication.Instance.Logger.Log("Method DateChangeValidation() executed successfully ...", category: Category.Info, priority: Priority.Low);
        }

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
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string AttachedFileIndexProp = BindableBase.GetPropertyName(() => AttachedFileIndex);
                string sheetNameSelectedIndex = BindableBase.GetPropertyName(() => SheetNameSelectedIndex);
                string employeeLeave = BindableBase.GetPropertyName(() => EmployeeLeaves);
                string filterEndDate = BindableBase.GetPropertyName(() => FilterEndDate);
                string filterStartDate = BindableBase.GetPropertyName(() => FilterStartDate);

                if (columnName == AttachedFileIndexProp && DataSourceSelectedIndex != 2)
                    return RequiredValidationRule.GetErrorMessage(AttachedFileIndexProp, AttachedFileIndex);

                if (DataSourceSelectedIndex.Equals(1))
                {
                    if (columnName == sheetNameSelectedIndex)
                    {
                        string error = RequiredValidationRule.GetErrorMessage(sheetNameSelectedIndex, StringSplit);

                        return error;
                    }
                }

                if (DataSourceSelectedIndex.Equals(2))
                {
                    if (columnName == sheetNameSelectedIndex)
                    {
                        if (DsnTableNames.Count == 0)
                            return "Error";

                    }
                }

                if (columnName == employeeLeave)
                {
                    string error = RequiredValidationRule.GetErrorMessage(employeeLeave, EmployeeLeaves.Count);

                    return error;
                }

                if (columnName == filterStartDate || columnName == filterEndDate)
                {
                    if (FilterEndDate < FilterStartDate)
                    {
                        return System.Windows.Application.Current.FindResource("ImportLeavesFilterDateError").ToString();
                    }
                }

                return null;
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => AttachedFileIndex)] +
                    me[BindableBase.GetPropertyName(() => EmployeeLeaves)] +
                    me[BindableBase.GetPropertyName(() => FilterEndDate)] +
                    me[BindableBase.GetPropertyName(() => FilterStartDate)] +
                    me[BindableBase.GetPropertyName(() => SheetNameSelectedIndex)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        #endregion
    }
}

