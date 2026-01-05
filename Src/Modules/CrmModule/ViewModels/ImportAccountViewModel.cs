using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
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
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;
using System.Linq;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Utility;
using ExcelDataReader;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Text;
using System.Reflection;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.DataProcessing;
using Emdep.Geos.Data.Common.Crm;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    //[nsatpute][25-06-2024][GEOS2-5701] Add new import accounts/contacts option (1/2)  
    public class ImportAccountViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// [001][adadibathina] [GEOS2-48] Add startdate and enddate filter in import attendance 
        /// [002][skhade][2019-09-13][GEOS2-31] Change the time format in attendance import option.
        /// </summary>

        #endregion

        #region Service

        protected IOpenFileDialogService OpenFileDialogService { get { return this.GetService<IOpenFileDialogService>(); } }
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion

        #region Declaration

        private bool isSave;
        bool IsBusy;
        private ObservableCollection<Attachment> attachmentFiles;
        private byte[] fileInBytes;
        private int sheetNameSelectedIndex;
        private ObservableCollection<ImportAccounts> customerAccounts = new ObservableCollection<ImportAccounts>();
        private CustomerAccountImportField selectedField;
        private int attachedFileIndex;
        private List<IDictionary<string, object>> reportData;
        private DataTable filteredTable { get; set; }
        int dataSourceSelectedIndex;
        private Dictionary<string, int> matchedIndex { get; set; }
        ObservableCollection<string> textSeparators;
        ObservableCollection<string> exelSheetNames;
        private int dsnSelectedIndex;
        string sheetNameSelectedItem;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private ObservableCollection<LookupValue> attendanceTypeList;
        private List<string> idsAttendanceType;

        #endregion

        #region Properties
        public ObservableCollection<Company> AddedAcountList { get; set; }
        int StringSplit { get; set; }
        public int SheetNameSelectedIndex
        {
            get { return sheetNameSelectedIndex; }
            set
            {
                sheetNameSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SheetNameSelectedIndex"));
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
        public ObservableCollection<string> TextSeparators
        {
            get { return textSeparators; }
            set
            {
                textSeparators = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextSeparators"));
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
        public ObservableCollection<Attachment> AttachmentFiles
        {
            get { return attachmentFiles; }
            set
            {
                attachmentFiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentFiles"));
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

        public int AttachedFileIndex
        {
            get { return attachedFileIndex; }
            set
            {
                attachedFileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFileIndex"));
            }
        }

        List<Company> CustomerAccountList { get; set; }
        bool IsMatchTypeWithDataSourceField { get; set; }
        public ObservableCollection<ImportAccounts> CustomerAccounts
        {
            get { return customerAccounts; }
            set
            {
                customerAccounts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerAccounts"));
            }
        }

        public CustomerAccountImportField SelectedField
        {
            get { return selectedField; }
            set
            {
                selectedField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedField"));
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

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
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

        #endregion

        #region Public Icommands
        public ICommand CloseButtonCommand { get; set; }
        public ICommand ChooseFileCommandForImport { get; set; }
        public ICommand NextWindowCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand ShowSourceColumnCustomFilterPopupCommand { get; set; }
        public ICommand DataSourceSelectedIndexChanged { get; set; }
        public ICommand SheetNameSelectedIndexChanged { get; set; }
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

        #region Validation

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

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => AttachedFileIndex)] +
                    me[BindableBase.GetPropertyName(() => CustomerAccounts)] +
                    me[BindableBase.GetPropertyName(() => SheetNameSelectedIndex)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string AttachedFileIndexProp = BindableBase.GetPropertyName(() => AttachedFileIndex);
                string sheetNameSelectedIndex = BindableBase.GetPropertyName(() => SheetNameSelectedIndex);
                string employeeAttendance = BindableBase.GetPropertyName(() => CustomerAccounts);

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
                if (columnName == employeeAttendance)
                {
                    string error = RequiredValidationRule.GetErrorMessage(employeeAttendance, CustomerAccounts.Count);

                    return error;
                }

                return null;
            }
        }

        #endregion

        #region Constructor

        public ImportAccountViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ImportAttendanceFileViewModel()...", category: Category.Info, priority: Priority.Low);


                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                ChooseFileCommandForImport = new RelayCommand(new Action<object>(BrowseFileAction));
                NextWindowCommand = new RelayCommand(new Action<object>(Next));
                SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
                ShowSourceColumnCustomFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(ShowSourceColumnCustomFilterPopupCommandAction);
                DataSourceSelectedIndexChanged = new DelegateCommand<EditValueChangedEventArgs>(DataSourceIndexChanged);
                SheetNameSelectedIndexChanged = new DelegateCommand<EditValueChangedEventArgs>(SheetNameIndexChanged);
                string TypeCompaniesids = CrmStartUp.GetGeosAppSettings(86).DefaultValue;
                string[] idsTypeCompany = TypeCompaniesids.Split(',').ToArray();
                if (IsMatchTypeWithDataSourceField)
                {
                    string ImportAttendanceCodes = CrmStartUp.GetGeosAppSettings(87).DefaultValue;
                    string[] ArrayImportAttendanceCodes = ImportAttendanceCodes.Split(',').ToArray();
                    Dictionary<string, string> LstImportAttendanceCodes = new Dictionary<string, string>();
                    if (LstImportAttendanceCodes == null)
                    {
                        LstImportAttendanceCodes = new Dictionary<string, string>();
                    }
                    foreach (string item in ArrayImportAttendanceCodes)
                    {
                        string[] tempCodes = null;
                        if (item.Contains(';'))
                            tempCodes = item.Split(';').ToArray();
                        if (tempCodes.Count() == 2 && tempCodes[0] != null && tempCodes[1] != null)
                            LstImportAttendanceCodes.Add(tempCodes[0].Replace('(', ' ').Trim(), tempCodes[1].Replace(')', ' ').Trim());
                    }
                    if (LstImportAttendanceCodes != null && LstImportAttendanceCodes.Count > 0)
                    {
                        idsAttendanceType = LstImportAttendanceCodes.Select(i => (string.IsNullOrEmpty(i.Value.TrimStart(new Char[] { '0' })) ? "0" : i.Value.TrimStart(new Char[] { '0' }))).ToList();
                    }
                }
                GeosApplication.Instance.Logger.Log("Constructor ImportAttendanceFileViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ImportAttendanceFileViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init(ObservableCollection<Company> customerAccountList)
        {
            GeosApplication.Instance.Logger.Log("Method Init() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                CustomerAccountList = customerAccountList.ToList();
                AttachmentFiles = new ObservableCollection<Attachment>();
                TextSeparators = new ObservableCollection<string>();
                ExelSheetNames = new ObservableCollection<string>();
                ReportData = new List<IDictionary<string, object>>();
                matchedIndex = new Dictionary<string, int>();
                AddedAcountList = new ObservableCollection<Company>();
                LoadSavedSettings();
                FillSeparators();
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
                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceDataSourceSelectedIndex"))
                {
                    int SelectedIndex;
                    int.TryParse(GeosApplication.Instance.UserSettings["ImportAttendanceDataSourceSelectedIndex"], out SelectedIndex);
                    DataSourceSelectedIndex = SelectedIndex;
                }
                else
                    DataSourceSelectedIndex = 0;

                if (DataSourceSelectedIndex == 1)
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceTextSeparator"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportAttendanceTextSeparator"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
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

        public void ShowSourceColumnCustomFilterPopupCommandAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method ShowSourceColumnCustomFilterPopupCommandAction ...", category: Category.Info, priority: Priority.Low);
            if (e.Column.FieldName != "CustomerAccountImportField")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName == "CustomerAccountImportField")
                {
                    foreach (ImportAccounts item in CustomerAccounts)
                    {
                        foreach (var sourceCol in item.CustomerAccountImportFieldList)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == sourceCol.RegisteredName.ToString()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = sourceCol.RegisteredName.ToString();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CustomerAccountImportField Like '%{0}%'", sourceCol.RegisteredName.ToString()));
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

        public void DataSourceIndexChanged(EditValueChangedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method DataSourceIndexChanged() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj.NewValue == null)
                {
                    AttachmentFiles = new ObservableCollection<Attachment>();
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
                    ExelSheetNames = new ObservableCollection<string>();
                }
                else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
                {
                    AttachmentFiles = new ObservableCollection<Attachment>();
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
                    ExelSheetNames = new ObservableCollection<string>();
                    ReportData = new List<IDictionary<string, object>>();
                    SetUserSetting(obj);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DataSourceIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method DataSourceIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void SelectedItemChangedCommandAction(EditValueChangedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj.NewValue == null)
                {
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
                    ExelSheetNames = new ObservableCollection<string>();
                }
                else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
                {
                    ReportData = new List<IDictionary<string, object>>();
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
                    SetUserSetting(obj);
                }

                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedItemChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SheetNameIndexChanged(EditValueChangedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SheetNameIndexChanged() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ReportData = new List<IDictionary<string, object>>();

                if (obj.NewValue == null)
                {
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
                }
                else if (obj.NewValue != obj.OldValue && obj.OldValue != null)
                {
                    CustomerAccounts = new ObservableCollection<ImportAccounts>();
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
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SheetNameIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method SheetNameIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
        }

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

                    DevExpress.Xpf.Grid.GridControl GridImportAttendance = (DevExpress.Xpf.Grid.GridControl)obj;

                    for (int item = 0; item < 14; item++)
                    {
                        var DataGridRow = (ImportAccounts)GridImportAttendance.GetRow(item);
                        matchedIndex.Add(DataGridRow.Column, DataGridRow.CustomerAccountImportFieldList.IndexOf(DataGridRow.CustomerAccountImportField));
                    }

                    GeosApplication.Instance.UserSettings["ImportAttendanceDataSourceSelectedIndex"] = DataSourceSelectedIndex.ToString();

                    //[001] added
                    if (DataSourceSelectedIndex == 0)
                    {
                        if (SheetNameSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportAttendanceExcelSheetname"] = SheetNameSelectedIndex.ToString();

                        string ImportAttendanceExcelSourceFieldSelectedIndex = string.Empty;

                        for (int item = 0; item < CustomerAccounts.Count; item++)
                        {
                            var DataGridRow = (ImportAccounts)GridImportAttendance.GetRow(item);
                            int index = DataGridRow.CustomerAccountImportFieldList.IndexOf(DataGridRow.CustomerAccountImportField);
                            ImportAttendanceExcelSourceFieldSelectedIndex += index.ToString() + ",";
                        }
                        ImportAttendanceExcelSourceFieldSelectedIndex = ImportAttendanceExcelSourceFieldSelectedIndex.Remove(ImportAttendanceExcelSourceFieldSelectedIndex.Length - 1, 1);
                        GeosApplication.Instance.UserSettings["ImportAttendanceExcelSourceFieldSelectedIndex"] = ImportAttendanceExcelSourceFieldSelectedIndex;
                    }
                    if (DataSourceSelectedIndex == 1)
                    {
                        if (SheetNameSelectedIndex != -1)
                            GeosApplication.Instance.UserSettings["ImportAttendanceTextSeparator"] = SheetNameSelectedIndex.ToString();

                        string ImportAttendanceTextSourceFieldSelectedIndex = string.Empty;

                        for (int item = 0; item < CustomerAccounts.Count; item++)
                        {
                            var DataGridRow = (ImportAccounts)GridImportAttendance.GetRow(item);
                            int index = DataGridRow.CustomerAccountImportFieldList.IndexOf(DataGridRow.CustomerAccountImportField);
                            ImportAttendanceTextSourceFieldSelectedIndex += index.ToString() + ",";
                        }
                        ImportAttendanceTextSourceFieldSelectedIndex = ImportAttendanceTextSourceFieldSelectedIndex.Remove(ImportAttendanceTextSourceFieldSelectedIndex.Length - 1, 1);
                        GeosApplication.Instance.UserSettings["ImportAttendanceTextSourceFieldSelectedIndex"] = ImportAttendanceTextSourceFieldSelectedIndex;
                    }

                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                    FillReports(filteredTable);

                    ReadImportedCustomerAccountView readImportedCustomerAccountView = new ReadImportedCustomerAccountView();
                    ReadImportedCustomerAccountViewModel readImportedCustomerAccountViewModel = new ReadImportedCustomerAccountViewModel(ReportData);
                    IsSave = readImportedCustomerAccountViewModel.IsSave;
                    EventHandler handle = delegate { readImportedCustomerAccountView.Close(); };
                    readImportedCustomerAccountViewModel.RequestClose += handle;
                    readImportedCustomerAccountView.DataContext = readImportedCustomerAccountViewModel;
                    readImportedCustomerAccountView.ShowDialog();
                    IsSave = readImportedCustomerAccountViewModel.IsSave;
                    if (IsSave)
                    {
                        AddedAcountList = new ObservableCollection<Company>(readImportedCustomerAccountViewModel.ImportedAccounts.Where(i => i.IsSelected).ToList());
                        RequestClose(null, null);
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

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;

            if (AttachmentFiles == null)
                AttachmentFiles = new ObservableCollection<Attachment>();
            else
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
                    if (Path.GetExtension(Attachment.FilePath) == ".xls" || Path.GetExtension(Attachment.FilePath) == ".xlsx")
                        FillSheetNames(Attachment);

                    if (DataSourceSelectedIndex.Equals(0))
                    {
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

        public void FetchColumnsFromExcel(Attachment attachedFile)
        {
            try
            {
                System.Data.DataTable SelectedSheet = new DataTable();

                CustomerAccounts = new ObservableCollection<ImportAccounts>();

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
                                        //[rdixit][27.09.2023][GEOS2-4873]
                                        filteredTable = SelectedSheet.Rows.Cast<DataRow>().Where(row => (row.ItemArray.Any(field => !(field is System.DBNull))) &&
                                        (!string.IsNullOrWhiteSpace(row.ItemArray[2].ToString()))).CopyToDataTable();
                                    }
                                    else
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttendanceSheetBrowseFailed").ToString()),
                                            Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        CustomerAccounts = new ObservableCollection<ImportAccounts>();
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

                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendance"));

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

        private void CalculateIndexForGridText()
        {
            int seletedIndexCount = 0;
            GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridText() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                int[] importAttendanceSourceField = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceTextSourceFieldSelectedIndex") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ImportAttendanceTextSourceFieldSelectedIndex"]))
                {
                    string ImportAttendanceTextSourceFieldSelectedIndex = GeosApplication.Instance.UserSettings["ImportAttendanceTextSourceFieldSelectedIndex"];
                    string[] ImportAttendanceTextSourceFieldSelectedIndexArray = ImportAttendanceTextSourceFieldSelectedIndex.Split(',');

                    for (int item = 0; item < ImportAttendanceTextSourceFieldSelectedIndexArray.Length; item++)
                        int.TryParse(ImportAttendanceTextSourceFieldSelectedIndexArray[item], out importAttendanceSourceField[item]);

                    for (int i = 0; i < CustomerAccounts.Count; i++)
                    {
                        if (seletedIndexCount < CustomerAccounts.Count)
                        {
                            if (CustomerAccounts[i].CustomerAccountImportFieldList.Count > 0 && CustomerAccounts[i].CustomerAccountImportFieldList.Count > importAttendanceSourceField[i])
                                CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[importAttendanceSourceField[i]];
                            else
                                CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[seletedIndexCount];
                        }
                        seletedIndexCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < CustomerAccounts.Count; i++)
                    {
                        if (seletedIndexCount < CustomerAccounts.Count)
                        {
                            if (CustomerAccounts[i].CustomerAccountImportFieldList.Count > 0)
                                CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[seletedIndexCount];
                        }

                        seletedIndexCount++;
                    }
                }
                IsAcceptEnabled = true;
                GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridText() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsAcceptEnabled = true;
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
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceExcelSheetname"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportAttendanceExcelSheetname"], out SelectedIndex);
                        SheetNameSelectedIndex = SelectedIndex;
                    }
                    else
                        SheetNameSelectedIndex = 0;
                }

                int[] importAttendanceSourceField = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceExcelSourceFieldSelectedIndex") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ImportAttendanceExcelSourceFieldSelectedIndex"]))
                {
                    string ImportAttendanceExcelSourceFieldSelectedIndex = GeosApplication.Instance.UserSettings["ImportAttendanceExcelSourceFieldSelectedIndex"];
                    string[] ImportAttendanceExcelSourceFieldSelectedIndexArray = ImportAttendanceExcelSourceFieldSelectedIndex.Split(',');

                    for (int item = 0; item < ImportAttendanceExcelSourceFieldSelectedIndexArray.Length; item++)
                        int.TryParse(ImportAttendanceExcelSourceFieldSelectedIndexArray[item], out importAttendanceSourceField[item]);

                    for (int i = 0; i < CustomerAccounts.Count; i++)
                    {
                        if (seletedIndexCount < CustomerAccounts.Count)
                        {
                            if (CustomerAccounts[i].CustomerAccountImportFieldList.Count > 0 && CustomerAccounts[i].CustomerAccountImportFieldList.Count > importAttendanceSourceField[i])
                                CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[importAttendanceSourceField[i]];
                            else
                                CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[seletedIndexCount];
                        }
                        seletedIndexCount++;
                    }
                }
                else
                {
                    for (int i = 0; i < CustomerAccounts.Count; i++)
                    {
                        if (seletedIndexCount < CustomerAccounts.Count)
                        {
                            if (CustomerAccounts[i].CustomerAccountImportFieldList.Count > 0)
                            {
                                if (CustomerAccounts[i].CustomerAccountImportFieldList.Count < seletedIndexCount)
                                    CustomerAccounts[i].CustomerAccountImportField = CustomerAccounts[i].CustomerAccountImportFieldList[seletedIndexCount];
                                else
                                    CustomerAccounts[i].CustomerAccountImportField = new CustomerAccountImportField();
                            }
                        }

                        seletedIndexCount++;
                    }
                }
                IsAcceptEnabled = true;
                GeosApplication.Instance.Logger.Log("Method CalculateIndexForGridExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsAcceptEnabled = true;
                GeosApplication.Instance.Logger.Log("Get an error in CalculateIndexForGridExcel() Method  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][03.12.2025][GEOS2-10361]
        public void FillReports(DataTable dataTable)
        {
            GeosApplication.Instance.Logger.Log("Method FillReports() ...", category: Category.Info, priority: Priority.Low);
            ReportData = new List<IDictionary<string, object>>();

            try
            {
                foreach (System.Data.DataRow item2 in dataTable.Rows)
                {
                    try
                    {
                        IDictionary<string, object> row = new Dictionary<string, object>();

                        foreach (var matchIndex in matchedIndex)
                        {
							
                            if (matchIndex.Value < 0 || matchIndex.Value >= item2.ItemArray.Length)
                                continue;

                            if (item2.IsNull(matchIndex.Value))
                                continue;

                            row.Add(matchIndex.Key, item2.ItemArray[matchIndex.Value]);
                        }

                        if (row.Count > 0)
                            ReportData.Add(row);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method FillReports()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
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

        public void FillSourceIngrid(DataTable dataTable)
        {
            GeosApplication.Instance.Logger.Log("Method FillSourceIngrid() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                List<string> TargetfieldNew = new List<string>();
                TargetfieldNew.Add("Group");
                TargetfieldNew.Add("Plant");
                TargetfieldNew.Add("Country");
                TargetfieldNew.Add("City");
                TargetfieldNew.Add("ZipCode");
                TargetfieldNew.Add("Address");
                TargetfieldNew.Add("State");
                TargetfieldNew.Add("RegisteredName");
                TargetfieldNew.Add("RegistrationNumber");
                TargetfieldNew.Add("BusinessField");
                TargetfieldNew.Add("BusinessType");
                TargetfieldNew.Add("Website");
                TargetfieldNew.Add("SalesOwner");
                TargetfieldNew.Add("Source");

                List<string> Targetfield = new List<string>();
                Targetfield.Add("Group");
                Targetfield.Add("Plant");
                Targetfield.Add("Country");
                Targetfield.Add("City");
                Targetfield.Add("ZipCode");
                Targetfield.Add("Address");
                Targetfield.Add("State");
                Targetfield.Add("RegisteredName");
                Targetfield.Add("RegistrationNumber");
                Targetfield.Add("BusinessField");
                Targetfield.Add("BusinessType");
                Targetfield.Add("Website");
                Targetfield.Add("SalesOwner");
                Targetfield.Add("Source");
                CustomerAccounts = new ObservableCollection<ImportAccounts>();

                for (int k = 0; k < Targetfield.Count; k++)
                {
                    ImportAccounts tmpCustAccount = new ImportAccounts();
                    tmpCustAccount.TargetFieldList = TargetfieldNew;
                    tmpCustAccount.Column = Targetfield[k].ToString();

                    tmpCustAccount.CustomerAccountImportFieldList = new List<CustomerAccountImportField>();
                    tmpCustAccount.CustomerAccountTargetFieldList = new List<CustomerAccountImportField>();
                    foreach (System.Data.DataRow item2 in dataTable.Rows)
                    {
                        for (int i = 0; i < Targetfield.Count; i++)
                        {
                            tmpCustAccount.CustomerAccountTargetFieldList.Add(new CustomerAccountImportField { RegisteredName = Targetfield[i].ToString() });
                        }
                        break;
                    }
                    foreach (System.Data.DataRow item2 in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            tmpCustAccount.CustomerAccountImportFieldList.Add(new CustomerAccountImportField { RegisteredName = item2[i].ToString() });
                        }

                        break;
                    }
                    CustomerAccounts.Add(tmpCustAccount);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSourceIngrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillSourceIngrid() executed successfully...", category: Category.Info, priority: Priority.Low);
        }

        public void FetchColumnsFromText(Attachment attachedFile)
        {
            try
            {
                System.Data.DataTable SelectedSheet = new DataTable();
                CustomerAccounts = new ObservableCollection<ImportAccounts>();

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
                    OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendance"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendance"));

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

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void SetUserSetting(EditValueChangedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserSetting() ...", category: Category.Info, priority: Priority.Low);

                if (obj.NewValue.ToString() == "Text")
                {
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceTextSeparator"))
                    {
                        int SelectedIndex;
                        int.TryParse(GeosApplication.Instance.UserSettings["ImportAttendanceTextSeparator"], out SelectedIndex);
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

        #endregion
    }
}
