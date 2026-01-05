using DevExpress.Mvvm;
using DevExpress.Xpf.CodeView;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.Validations;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using System.ServiceModel;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddContactInformationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands

        public ICommand AddContactViewCancelButtonCommand { get; set; }
        public ICommand AddContactViewAcceptButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand SelectedIndexContactTypeCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        #endregion

        #region Declaration

        private int selectedIndexContactType;
        private string remark;
        private string contactValue = "";
        private bool isSave;
        private bool isNew;
        private bool isCompanyUse;
        public int CompanyUse;
        private string windowHeader;
        private ObservableCollection<EmployeeContact> existEmployeeContactList;
        private ObservableCollection<EmployeeContact> employeeContactList;
        private LookupValue selectedContactType;
        public bool IsBusy { get; private set; }
        private List<object> attachmentList;
        private byte[] _contractSituationFileInBytes;
        private List<string> suggestionsList;

        private bool isTextbox = true;
        private bool isCombobox = true;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties
        public List<LookupValue> ContactTypeList { get; set; }
        public EmployeeContact NewContact { get; set; }
        public EmployeeContact EditContact { get; set; }
        public ObservableCollection<EmployeeContact> ExistEmployeeContactList
        {
            get
            {
                return existEmployeeContactList;
            }

            set
            {
                existEmployeeContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContactList"));
            }
        }
        public ObservableCollection<EmployeeContact> EmployeeContactList
        {
            get
            {
                return employeeContactList;
            }

            set
            {
                employeeContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContactList"));
            }
        }
        

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public string ContactValue
        {
            get { return contactValue; }
            set
            {
                contactValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactValue"));
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

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
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

        public bool IsCompanyUse
        {
            get { return isCompanyUse; }
            set
            {
                isCompanyUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompanyUse"));
            }
        }

        public int SelectedIndexContactType
        {
            get { return selectedIndexContactType; }
            set
            {
                ContactValue = "";
                selectedIndexContactType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexContactType"));
            }
        }
        public LookupValue SelectedContactType
        {
            get { return selectedContactType; }
            set
            {
                selectedContactType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContactType"));
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

        public byte[] ContractSituationFileInBytes
        {
            get { return _contractSituationFileInBytes; }
            set { _contractSituationFileInBytes = value; }
        }

        public List<string> SuggestionsList
        {
            get { return suggestionsList; }
            set
            {
                suggestionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuggestionsList"));
            }
        }

        public bool IsTextbox
        {
            get { return isTextbox; }
            set
            {
                isTextbox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTextbox"));
            }
        }

        public bool IsCombobox
        {
            get { return isCombobox; }
            set
            {
                isCombobox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCombobox"));
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

        public AddContactInformationViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddContactInformationViewModel()...", category: Category.Info, priority: Priority.Low);
                EmployeeContactList = new ObservableCollection<EmployeeContact>();
                AddContactViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddContactViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddContactInformation));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                SelectedIndexContactTypeCommand = new RelayCommand(new Action<object>(SelectedIndexContactTypeCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                NewContact = new EmployeeContact();

                GeosApplication.Instance.Logger.Log("Constructor AddContactInformationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddContactInformationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init(ObservableCollection<EmployeeContact>EmployeeContactList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
       
                FillContactTypeList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(EmployeeContact empContact)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                FillContactTypeList();

                SelectedIndexContactType = ContactTypeList.FindIndex(x => x.IdLookupValue == empContact.EmployeeContactType.IdLookupValue);

                if (empContact.EmployeeContactIdType == 88)
                {
                    IsTextbox = false;
                    IsCombobox = true;
                }

                ContactValue = empContact.EmployeeContactValue;
                Remark = empContact.EmployeeContactRemarks;

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
       
        public void InitReadOnly(EmployeeContact empContact)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                ContactTypeList = new List<LookupValue>();
                ContactTypeList.Add(empContact.EmployeeContactType);

                SelectedIndexContactType = 0;

                if (empContact.EmployeeContactIdType == 88)
                {
                    IsTextbox = false;
                    IsCombobox = true;
                }

                ContactValue = empContact.EmployeeContactValue;
                Remark = empContact.EmployeeContactRemarks;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillContactTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactTypeList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(20);
                ContactTypeList = new List<LookupValue>();
                ContactTypeList = new List<LookupValue>(tempCountryList);
                ContactTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillContactTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method FillContactTypeList().{0}", ex.Message ), category: Category.Info, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => ContactValue)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexContactType)];

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
                string empcontactType = BindableBase.GetPropertyName(() => ContactValue);
                string empselectedIndexContactType = BindableBase.GetPropertyName(() => SelectedIndexContactType);

                if (columnName == empcontactType)
                {
                    if (!string.IsNullOrEmpty(ContactValue) && ContactTypeList != null && ContactTypeList[SelectedIndexContactType].IdLookupValue == 88)
                    {
                        var regex = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}";
                        var match = Regex.Match(ContactValue, regex, RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            return "The email address is not valid";
                        }
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empcontactType, ContactValue);
                    }
                }

                if (columnName == empselectedIndexContactType)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexContactType, SelectedIndexContactType);
                }

                return null;
            }
        }

        private void AddContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddContactInformation()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ContactValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexContactType"));

                if (error != null)
                {
                    return;
                }

                if (IsCompanyUse)
                    CompanyUse = 1;
                else
                    CompanyUse = 0;

                if (!string.IsNullOrEmpty(Remark))
                    Remark = Remark.Trim();

                EmployeeContact TempNewContact = new EmployeeContact();
                TempNewContact = EmployeeContactList.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == ContactTypeList[SelectedIndexContactType].IdLookupValue && x.EmployeeContactValue == ContactValue.Trim());
                if (TempNewContact == null)
                {
                    if (IsNew == true)
                    {
                        NewContact = new EmployeeContact() { EmployeeContactType = ContactTypeList[SelectedIndexContactType], EmployeeContactValue = ContactValue.Trim(), EmployeeContactRemarks = Remark, EmployeeContactIdType = ContactTypeList[SelectedIndexContactType].IdLookupValue, IsCompanyUse = Convert.ToByte(CompanyUse), TransactionOperation = ModelBase.TransactionOperations.Add };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                    else
                    {
                        EditContact = new EmployeeContact() { EmployeeContactType = ContactTypeList[SelectedIndexContactType], EmployeeContactValue = ContactValue.Trim(), EmployeeContactRemarks = Remark, EmployeeContactIdType = ContactTypeList[SelectedIndexContactType].IdLookupValue, IsCompanyUse = Convert.ToByte(CompanyUse), TransactionOperation = ModelBase.TransactionOperations.Update };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method AddContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        private void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            IsBusy = true;

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {

                    AttachmentList = new List<object>();
                    ContractSituationFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    FileInfo file = new FileInfo(dlg.FileName);
                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = ContractSituationFileInBytes;

                    newAttachmentList.Add(attachment);


                    AttachmentList = newAttachmentList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SelectedIndexContactTypeCommandAction(object obj)
        {
            try
            {
                if (SelectedIndexContactType == 0) return;

                if (ContactTypeList != null && ContactTypeList[SelectedIndexContactType].IdLookupValue == 88)
                {
                    IsTextbox = false;
                    IsCombobox = true;

                    if (GeosApplication.Instance.DomainUsers == null)
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

                        GeosApplication.Instance.DomainUsers = WorkbenchService.GetUserDataFromTheActiveDirectory(); // GeosApplication.Instance.GetUserDataFromTheActiveDirectory();

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    }

                    SuggestionsList = GeosApplication.Instance.DomainUsers.Select(x => x.Email).ToList();
                    SuggestionsList = SuggestionsList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                }
                else
                {
                    IsTextbox = true;
                    IsCombobox = false;
                    SuggestionsList = null;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexContactTypeCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexContactTypeCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SelectedIndexContactTypeCommandAction(). {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
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
        }
    }
}
