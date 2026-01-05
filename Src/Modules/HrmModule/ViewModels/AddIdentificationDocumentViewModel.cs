using DevExpress.Mvvm;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddIdentificationDocumentViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands
        public ICommand AddIdentificationDocumentCancelButtonCommand { get; set; }
        public ICommand AddIdentificationDocumentViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration

        private string windowHeader;
        private string documentNumber;
        private string documentName;
        private DateTime? issueDate;
        private DateTime? expiryDate;
        private bool isSave;
        private bool isNew;
        private string issueDateErrorMessage = string.Empty;
        private string expiryDateErrorMessage = string.Empty;
        private string error = string.Empty;
        private int selectedIndexDocumentType;
        private ObservableCollection<EmployeeDocument> existEmployeeDocumentList;
        private List<object> attachmentList;
        private byte[] identificationDocumentFileInBytes;
        private string identificationdocumentFileName;
        private string remark;
        private int idEmployee;
        private EmployeeDocument existEmployeeDocument;
        private ObservableCollection<EmployeeDocument> documentList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties

        public EmployeeDocument NewDocument { get; set; }
        public EmployeeDocument EditDocument { get; set; }
        public List<LookupValue> DocumentTypeList { get; set; }
        public bool IsBusy { get; set; }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public ObservableCollection<EmployeeDocument> ExistEmployeeDocumentList
        {
            get { return existEmployeeDocumentList; }
            set
            {
                existEmployeeDocumentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeDocumentList"));
            }
        }

        public string DocumentNumber
        {
            get { return documentNumber; }
            set
            {
                documentNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentNumber"));
            }
        }

        public DateTime? IssueDate
        {
            get { return issueDate; }
            set
            {
                issueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IssueDate"));
            }
        }

        public DateTime? ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                expiryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpiryDate"));
            }
        }

        public string DocumentName
        {
            get { return documentName; }
            set
            {
                documentName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentName"));
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

        public List<object> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public int SelectedIndexDocumentType
        {
            get { return selectedIndexDocumentType; }
            set
            {
                DocumentNumber = "";
                selectedIndexDocumentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexDocumentType"));
            }
        }

        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }

        public byte[] IdentificationDocumentFileInBytes
        {
            get { return identificationDocumentFileInBytes; }
            set
            {
                identificationDocumentFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdentificationDocumentFileInBytes"));
            }
        }

        public string IdentificationdocumentFileName
        {
            get { return identificationdocumentFileName; }
            set
            {
                identificationdocumentFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdentificationdocumentFileName"));
            }
        }

        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployee"));
            }
        }

        public EmployeeDocument ExistEmployeeDocument
        {
            get { return existEmployeeDocument; }
            set
            {
                existEmployeeDocument = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeDocument"));
            }
        }

        public ObservableCollection<EmployeeDocument> DocumentList
        {
            get { return documentList; }
            set
            {
                documentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentList"));
            }
        }

        private EmployeeContractSituation EmployeeActiveContract { get; set; }

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

        #region Public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Constructor

        public AddIdentificationDocumentViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddIdentificationDocumentViewModel()...", category: Category.Info, priority: Priority.Low);
                ExistEmployeeDocumentList = new ObservableCollection<EmployeeDocument>();
                AddIdentificationDocumentCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddIdentificationDocumentViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddIdentificationDocumentInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                NewDocument = new EmployeeDocument();
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.Logger.Log("Constructor AddIdentificationDocumentViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddIdentificationDocumentViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region Methods

        public void Init(ObservableCollection<EmployeeDocument> EmployeeDocumentList, int EmployeeId, EmployeeContractSituation empActiveContract)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddIdentificationDocument").ToString();
                EmployeeActiveContract = empActiveContract;

                ExistEmployeeDocumentList = EmployeeDocumentList;
                FillDocumentTypeList();
                IssueDate = null;
                ExpiryDate = null;
                IdEmployee = EmployeeId;

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeeDocument> EmployeeDocumentList, EmployeeDocument empDocument, int EmployeeId, EmployeeContractSituation empActiveContract)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditIdentificationDocument").ToString();
                EmployeeActiveContract = empActiveContract;

                ExistEmployeeDocumentList = new ObservableCollection<EmployeeDocument>(EmployeeDocumentList.ToList());
                ExistEmployeeDocumentList.Remove(empDocument);

                FillDocumentTypeList();
                SelectedIndexDocumentType = DocumentTypeList.FindIndex(x => x.IdLookupValue == empDocument.EmployeeDocumentType.IdLookupValue);
                DocumentName = empDocument.EmployeeDocumentName;
                DocumentNumber = empDocument.EmployeeDocumentNumber;
                IssueDate = empDocument.EmployeeDocumentIssueDate;
                ExpiryDate = empDocument.EmployeeDocumentExpiryDate;
                Remark = empDocument.EmployeeDocumentRemarks;
                IdentificationdocumentFileName = empDocument.EmployeeDocumentFileName;
                IdentificationDocumentFileInBytes = empDocument.EmployeeDocumentFileInBytes;

                AttachmentList = new List<object>();
                if (empDocument.Attachment != null)
                {
                    AttachmentList.Add(empDocument.Attachment);
                }
                else if (!string.IsNullOrEmpty(IdentificationdocumentFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empDocument.EmployeeDocumentFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }

                DocumentList = new ObservableCollection<EmployeeDocument>(EmployeeDocumentList.ToList());
                ExistEmployeeDocument = empDocument;
                IdEmployee = EmployeeId;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>
        public void InitReadOnly(EmployeeDocument empDocument)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditIdentificationDocument").ToString();

                DocumentTypeList = new List<LookupValue>();
                DocumentTypeList.Add(empDocument.EmployeeDocumentType);
                SelectedIndexDocumentType = 0;
                DocumentName = empDocument.EmployeeDocumentName;
                DocumentNumber = empDocument.EmployeeDocumentNumber;
                IssueDate = empDocument.EmployeeDocumentIssueDate;
                ExpiryDate = empDocument.EmployeeDocumentExpiryDate;
                Remark = empDocument.EmployeeDocumentRemarks;
                IdentificationdocumentFileName = empDocument.EmployeeDocumentFileName;
                IdentificationDocumentFileInBytes = empDocument.EmployeeDocumentFileInBytes;

                AttachmentList = new List<object>();
                if (empDocument.Attachment != null)
                {
                    AttachmentList.Add(empDocument.Attachment);
                }
                else if (!string.IsNullOrEmpty(IdentificationdocumentFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empDocument.EmployeeDocumentFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

            if (IssueDate != null && ExpiryDate != null)
            {
                if (IssueDate > ExpiryDate)
                {
                    issueDateErrorMessage = System.Windows.Application.Current.FindResource("IdentificationDocumentIssueDateError").ToString();
                    expiryDateErrorMessage = System.Windows.Application.Current.FindResource("IdentificationDocumentExpiryDateError").ToString();
                }
                else
                {
                    issueDateErrorMessage = string.Empty;
                    expiryDateErrorMessage = string.Empty;
                }

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("IssueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ExpiryDate"));
            }


            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillDocumentTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDocumentTypeList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(18);
                DocumentTypeList = new List<LookupValue>();
                //[GEOS2-6499][04.11.2024][rdixit]
                if (GeosApplication.Instance.IsHRMManageEmployeeContactsPermission)
                    DocumentTypeList = new List<LookupValue>(tempCountryList?.Where(i => i.IdLookupValue == 80 || i.IdLookupValue == 81));
                else
                    DocumentTypeList = new List<LookupValue>(tempCountryList);
                DocumentTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillDocumentTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDocumentTypeList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillContactTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => SelectedIndexDocumentType)] +
                    me[BindableBase.GetPropertyName(() => DocumentName)] +
                    me[BindableBase.GetPropertyName(() => DocumentNumber)] +
                    me[BindableBase.GetPropertyName(() => IssueDate)] +
                    me[BindableBase.GetPropertyName(() => ExpiryDate)];

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

                string empselectedIndexDocumentType = BindableBase.GetPropertyName(() => SelectedIndexDocumentType);
                string empDocumentName = BindableBase.GetPropertyName(() => DocumentName);
                string empDocumentNumber = BindableBase.GetPropertyName(() => DocumentNumber);
                string empDocumentIssueDate = BindableBase.GetPropertyName(() => IssueDate);
                string empDocumentExpiryDate = BindableBase.GetPropertyName(() => ExpiryDate);

                if (columnName == empselectedIndexDocumentType)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexDocumentType, SelectedIndexDocumentType);
                }

                if (columnName == empDocumentName)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empDocumentName, DocumentName);
                }

                if (columnName == empDocumentNumber&& DocumentTypeList[SelectedIndexDocumentType].IdLookupValue != 153)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empDocumentNumber, DocumentNumber);
                }

                if (columnName == empDocumentIssueDate)
                {
                    if (!string.IsNullOrEmpty(issueDateErrorMessage))
                    {
                        return issueDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empDocumentIssueDate, IssueDate);
                    }
                }

                if (columnName == empDocumentExpiryDate)
                {
                    if (!string.IsNullOrEmpty(expiryDateErrorMessage))
                    {
                        return expiryDateErrorMessage;
                    }
                }
                return null;
            }
        }


        private void AddIdentificationDocumentInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddIdentificationDocumentInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();                
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexDocumentType"));
                PropertyChanged(this, new PropertyChangedEventArgs("DocumentName"));
                if (DocumentTypeList[SelectedIndexDocumentType].IdLookupValue != 153)
                    PropertyChanged(this, new PropertyChangedEventArgs("DocumentNumber"));
              
                PropertyChanged(this, new PropertyChangedEventArgs("IssueDate"));

                if (error != null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(DocumentName))
                    DocumentName = DocumentName.Trim();

                if (!string.IsNullOrEmpty(DocumentNumber))
                    DocumentNumber = DocumentNumber.Trim();

                if (!string.IsNullOrEmpty(Remark))
                    Remark = Remark.Trim();

                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    IdentificationdocumentFileName = null;
                    IdentificationDocumentFileInBytes = null;
                }
                if (DocumentTypeList[SelectedIndexDocumentType].IdLookupValue!=153)
                {
                    EmployeeDocument TempNewDocument = ExistEmployeeDocumentList.FirstOrDefault(x => x.EmployeeDocumentType.IdLookupValue == DocumentTypeList[SelectedIndexDocumentType].IdLookupValue && x.EmployeeDocumentNumber == DocumentNumber.Trim());

                    if (TempNewDocument == null)
                    {
                        EmployeeDocument TempNewDocumentForFile = new EmployeeDocument();
                        var ExistFilesRecord = ExistEmployeeDocumentList.Where(x => x.EmployeeDocumentFileName != null);
                        TempNewDocumentForFile = ExistFilesRecord.FirstOrDefault(x => x.EmployeeDocumentFileName == IdentificationdocumentFileName);

                        if (EmployeeActiveContract == null && DocumentTypeList[SelectedIndexDocumentType].IdLookupValue == 193) //Added
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NoActiveContractError").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        Employee anotherEmployee = null;        //Added
                        if (DocumentTypeList[SelectedIndexDocumentType].IdLookupValue == 193)      //Added
                        {
                            //[rdixit][29.01.2025][GEOS2-6826]
                            anotherEmployee = HrmService.IsExistDocumentNumberInAnotherEmployee_V2610(DocumentNumber, IdEmployee, DocumentTypeList[SelectedIndexDocumentType].IdLookupValue, Convert.ToInt32(EmployeeActiveContract.IdCompany));
                        }
                        else
                        {
                            //[rdixit][29.01.2025][GEOS2-6826]
                            anotherEmployee = HrmService.IsExistDocumentNumberInAnotherEmployee_V2610(DocumentNumber, IdEmployee, DocumentTypeList[SelectedIndexDocumentType].IdLookupValue, 0);
                        }

                        if (TempNewDocumentForFile == null)
                        {

                            if (IsNew == true)
                            {
                                if (anotherEmployee == null)
                                {
                                    NewDocument = new EmployeeDocument()
                                    {
                                        EmployeeDocumentType = DocumentTypeList[SelectedIndexDocumentType],
                                        EmployeeDocumentIdType = DocumentTypeList[SelectedIndexDocumentType].IdLookupValue,
                                        EmployeeDocumentName = DocumentName,
                                        EmployeeDocumentNumber = DocumentNumber,
                                        EmployeeDocumentIssueDate = IssueDate,
                                        EmployeeDocumentExpiryDate = ExpiryDate,
                                        EmployeeDocumentFileName = IdentificationdocumentFileName,
                                        EmployeeDocumentFileInBytes = IdentificationDocumentFileInBytes,
                                        EmployeeDocumentRemarks = Remark,
                                        TransactionOperation = ModelBase.TransactionOperations.Add
                                    };

                                    IsSave = true;
                                    RequestClose(null, null);
                                }
                                else
                                {
                                    IsSave = false;
                                    //[rdixit][29.01.2025][GEOS2-6826]
                                    if (anotherEmployee.ExitDate != null)
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("IdentificationDocumentNumberExistWithDate").ToString(),
                                        anotherEmployee.EmployeeCode, anotherEmployee.FullName, anotherEmployee.EmployeeStatus?.Value, anotherEmployee.ExitDate?.ToShortDateString()),
                                        Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    else
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("IdentificationDocumentNumberExist").ToString(),
                                        anotherEmployee.EmployeeCode, anotherEmployee.FullName, anotherEmployee.EmployeeStatus?.Value),
                                        Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }

                            }
                            else
                            {
                                EmployeeDocument tempExistEmployeeDocument = DocumentList.First(x => x.IdEmployeeDocument == ExistEmployeeDocument.IdEmployeeDocument);
                                if (tempExistEmployeeDocument != null)
                                {
                                    if (anotherEmployee == null && tempExistEmployeeDocument.IdEmployeeDocument == ExistEmployeeDocument.IdEmployeeDocument)
                                    {
                                        EditDocument = new EmployeeDocument()
                                        {
                                            EmployeeDocumentType = DocumentTypeList[SelectedIndexDocumentType],
                                            EmployeeDocumentIdType = DocumentTypeList[SelectedIndexDocumentType].IdLookupValue,
                                            EmployeeDocumentName = DocumentName,
                                            EmployeeDocumentNumber = DocumentNumber,
                                            EmployeeDocumentIssueDate = IssueDate,
                                            EmployeeDocumentExpiryDate = ExpiryDate,
                                            EmployeeDocumentFileName = IdentificationdocumentFileName,
                                            EmployeeDocumentFileInBytes = IdentificationDocumentFileInBytes,
                                            EmployeeDocumentRemarks = Remark,

                                            TransactionOperation = ModelBase.TransactionOperations.Update
                                        };
                                        IsSave = true;
                                        RequestClose(null, null);
                                    }
                                    else
                                    {
                                        IsSave = false;
                                        //[rdixit][29.01.2025][GEOS2-6826]
                                        if (anotherEmployee.ExitDate != null)
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("IdentificationDocumentNumberExistWithDate").ToString(),
                                            anotherEmployee.EmployeeCode, anotherEmployee.FullName, anotherEmployee.EmployeeStatus?.Value, anotherEmployee.ExitDate?.ToShortDateString()),
                                            Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        else
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("IdentificationDocumentNumberExist").ToString(),
                                            anotherEmployee.EmployeeCode, anotherEmployee.FullName, anotherEmployee.EmployeeStatus?.Value),
                                            Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                }
                            }
                        }
                        else
                        {
                            IsSave = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddIdentificationDocumentFileExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }

                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddIdentificationDocumentExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    if (IsNew == true)
                    {
                        NewDocument = new EmployeeDocument()
                        {
                            EmployeeDocumentType = DocumentTypeList[SelectedIndexDocumentType],
                            EmployeeDocumentIdType = DocumentTypeList[SelectedIndexDocumentType].IdLookupValue,
                            EmployeeDocumentName = DocumentName,
                            EmployeeDocumentNumber = DocumentNumber,
                            EmployeeDocumentIssueDate = IssueDate,
                            EmployeeDocumentExpiryDate = ExpiryDate,
                            EmployeeDocumentFileName = IdentificationdocumentFileName,
                            EmployeeDocumentFileInBytes = IdentificationDocumentFileInBytes,
                            EmployeeDocumentRemarks = Remark,
                            TransactionOperation = ModelBase.TransactionOperations.Add
                        };

                        IsSave = true;
                        RequestClose(null, null);
                    }
                    else
                    {
                        EditDocument = new EmployeeDocument()
                        {
                            EmployeeDocumentType = DocumentTypeList[SelectedIndexDocumentType],
                            EmployeeDocumentIdType = DocumentTypeList[SelectedIndexDocumentType].IdLookupValue,
                            EmployeeDocumentName = DocumentName,
                            EmployeeDocumentNumber = DocumentNumber,
                            EmployeeDocumentIssueDate = IssueDate,
                            EmployeeDocumentExpiryDate = ExpiryDate,
                            EmployeeDocumentFileName = IdentificationdocumentFileName,
                            EmployeeDocumentFileInBytes = IdentificationDocumentFileInBytes,
                            EmployeeDocumentRemarks = Remark,

                            TransactionOperation = ModelBase.TransactionOperations.Update
                        };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }

              

                GeosApplication.Instance.Logger.Log("Method AddIdentificationDocumentInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddIdentificationDocumentInformation() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddIdentificationDocumentInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddIdentificationDocumentInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }


        private void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {

                    AttachmentList = new List<object>();
                    IdentificationDocumentFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    FileInfo file = new FileInfo(dlg.FileName);
                    IdentificationdocumentFileName = file.Name;

                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = IdentificationDocumentFileInBytes;

                    newAttachmentList.Add(attachment);


                    AttachmentList = newAttachmentList;
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseFileCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

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
            //[GEOS2-6499][rdixit][07.11.2024]
            if (GeosApplication.Instance.IsHRMManageEmployeeContactsPermission)
            {
                IsReadOnlyField = false;
                IsAcceptEnabled = true;
            }
        }
    }
}
