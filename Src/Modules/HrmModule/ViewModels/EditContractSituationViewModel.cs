using DevExpress.Mvvm;
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
    public class EditContractSituationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands

        public ICommand MergeButtonCommand { get; set; }
        public ICommand AddContractSituationViewCancelButtonCommand { get; set; }
        public ICommand AddContractSituationViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration
        private bool isSave;
        private string remarks;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string error = string.Empty;
        private Visibility splitVisble;
        private int selectedIndexContractSituation;
        private int selectedIndexProfessionalCategory;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? minStartDate;
        private DateTime? maxStartDate;
        private DateTime? maxEndDate;
        private DateTime? minEndDate;
        private string windowHeader;
        private List<object> attachmentList;
        private string contractSituationFileName;
        private byte[] contractSituationFileInBytes;
        private bool isNew;
        private ObservableCollection<EmployeeContractSituation> existEmployeeContractSituation;
        private int selectedIndexCompany;
        IWorkbenchStartUp objWorkbenchStartUp;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
      
        #endregion

        #region Properties
        public List<ContractSituation> ContractSituationList { get; set; }
        public List<ProfessionalCategory> ProfessionalCategoryList { get; set; }
        public EmployeeContractSituation NewEmployeeContractSituation { get; set; }
        public EmployeeContractSituation EditEmployeeContractSituation { get; set; }
        public bool IsBusy { get; set; }
        
        public Visibility SplitVisble
        {
            get
            {
                return splitVisble;
            }

            set
            {
                splitVisble = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SplitVisble"));
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
        public ObservableCollection<EmployeeContractSituation> ExistEmployeeContractSituation
        {
            get
            {
                return existEmployeeContractSituation;
            }

            set
            {
                existEmployeeContractSituation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContractSituation"));
            }
        }

        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public int SelectedIndexContractSituation
        {
            get
            {
                return selectedIndexContractSituation;
            }

            set
            {
                selectedIndexContractSituation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexContractSituation"));
            }
        }

        public int SelectedIndexProfessionalCategory
        {
            get
            {
                return selectedIndexProfessionalCategory;
            }

            set
            {
                selectedIndexProfessionalCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexProfessionalCategory"));
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                if (value == null)
                {
                    if (!ExistEmployeeContractSituation.Any(x => x.ContractSituationEndDate == null || x.ContractSituationEndDate.Value.Date >= DateTime.Now.Date))
                    {
                        endDate = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                    }
                }
                else
                {
                    endDate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                }

            }
        }

        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public List<object> AttachmentList
        {
            get
            {
                return attachmentList;
            }

            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public string ContractSituationFileName
        {
            get
            {
                return contractSituationFileName;
            }

            set
            {
                contractSituationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileName"));
            }
        }

        public byte[] ContractSituationFileInBytes
        {
            get
            {
                return contractSituationFileInBytes;
            }

            set
            {
                contractSituationFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileInBytes"));
            }
        }

        public DateTime? MinStartDate
        {
            get
            {
                return minStartDate;
            }

            set
            {
                minStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinStartDate"));
            }
        }
        public DateTime? MaxStartDate
        {
            get
            {
                return maxStartDate;
            }

            set
            {
                maxStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxStartDate"));
            }
        }

        public DateTime? MaxEndDate
        {
            get
            {
                return maxEndDate;
            }

            set
            {
                maxEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxEndDate"));
            }
        }
        public DateTime? MinEndDate
        {
            get
            {
                return minEndDate;
            }

            set
            {
                minEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinEndDate"));
            }
        }
        public int SelectedIndexCompany
        {
            get
            {
                return selectedIndexCompany;
            }

            set
            {
                selectedIndexCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompany"));
            }
        }
        public List<Company> CompanyList { get; set; }
        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }
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

        #region public Events
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
        public EditContractSituationViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddLanguagesViewModel()...", category: Category.Info, priority: Priority.Low);
                AddContractSituationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddContractSituationViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddContractSituationInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                ExistEmployeeContractSituation = new ObservableCollection<EmployeeContractSituation>();
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                MergeButtonCommand = new RelayCommand(new Action<object>(MergeFiles));
                GeosApplication.Instance.Logger.Log("Constructor AddLanguagesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddLanguagesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods


        private void MergeFiles(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method MergeFiles ...", category: Category.Info, priority: Priority.Low);
            EmployeeAttendance selectedEmpAttendance = new EmployeeAttendance();
                        
            
            //AddNewTaskbtnVisibility = false;

            //Tasks.Add(new Task()
            //{
            //    Name = "Header",
            //    Header = "Attendance1",
            //    IsComplete = false,
            //    IsNew = false,
            //    TaskEmployeeListFinal = EmployeeListFinal,
            //    IsEnabaledEmployee = false,
            //    SelectedIndexForEmployee = SelectedIndexForEmployee,
            //    //CompanyShifts = CompanyShifts,
            //    EmployeeShiftList = EmployeeShiftList,
            //    // TaskSelectedCompanyShift = SelectedCompanyShift,
            //    TaskSelectedEmployeeShift = SelectedEmployeeShift,
            //    SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
            //    IsEnabledShift = false,

            //    TaskStartDate = _StartTime,
            //    TaskStartTime = _StartTime,
            //    //STime = STime,

            //    TaskEndDate = _EndTime,
            //    TaskEndTime = _EndTime,
            //    //ETime = ETime,

            //    SelectedIndexForAttendanceType = SelectedIndexForAttendanceType,
            //    TaskAccountingDate = AccountingDate
            //});

            
            GeosApplication.Instance.Logger.Log("Method MergeFiles executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
            if (StartDate != null && EndDate != null)
            {
                if (StartDate > EndDate)
                {
                    startDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeContractSituationStartDateError").ToString();
                    endDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeContractSituationEndDateError").ToString();
                }
                else
                {
                    startDateErrorMessage = string.Empty;
                    endDateErrorMessage = string.Empty;
                }
            }
            else
            {
                startDateErrorMessage = string.Empty;
                endDateErrorMessage = string.Empty;
            }
            error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));

            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void FillContractSituationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContractSituationList()...", category: Category.Info, priority: Priority.Low);
                IList<ContractSituation> tempCountryList = HrmService.GetAllContractSituations();
                ContractSituationList = new List<ContractSituation>();
                ContractSituationList.Insert(0, new ContractSituation() { Name = "---" });
                ContractSituationList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillContractSituationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContractSituationList() Method " + ex.Detail.ErrorMessage+ GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContractSituationList() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillContractSituationList()....executed successfully" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        private void FillProfessionalCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProfessionalCategoryList()...", category: Category.Info, priority: Priority.Low);
                IList<ProfessionalCategory> tempCountryList = HrmService.GetAllProfessionalCategories();
                ProfessionalCategoryList = new List<ProfessionalCategory>();
                ProfessionalCategoryList.Insert(0, new ProfessionalCategory() { Name = "---" });
                ProfessionalCategoryList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillProfessionalCategoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalCategoryList() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalCategoryList() Method - ServiceUnexceptedException "+ ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillProfessionalCategoryList()....executed successfully" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        public void Init(ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddContractSituationInformation").ToString();
                ExistEmployeeContractSituation = EmployeeContractSituationList;
                FillContractSituationList();
                FillProfessionalCategoryList();
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                FillCompanyList();
                SelectedIndexCompany = CompanyList.FindIndex(x => x.IdCompany == CurrentGeosProvider.IdCompany);
                if (SelectedIndexCompany == -1)
                    SelectedIndexCompany = 0;

                if (EmployeeContractSituationList.Count > 0)
                {
                    MinStartDate = EmployeeContractSituationList[EmployeeContractSituationList.Count - 1].ContractSituationEndDate;
                }
                else
                {
                    MinStartDate = null;
                }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditInit(ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList, EmployeeContractSituation empContractSituation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);


                ExistEmployeeContractSituation = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList.ToList());
                ExistEmployeeContractSituation.Remove(empContractSituation);
                WindowHeader = Application.Current.FindResource("EditContractSituationInformation").ToString();
                FillContractSituationList();
                FillProfessionalCategoryList();
                FillCompanyList();
                SelectedIndexCompany = CompanyList.FindIndex(x => x.IdCompany == empContractSituation.IdCompany);
                if (SelectedIndexCompany == -1)
                    SelectedIndexCompany = 0;

                SelectedIndexContractSituation = ContractSituationList.FindIndex(x => x.IdContractSituation == empContractSituation.IdContractSituation);
                SelectedIndexProfessionalCategory = ProfessionalCategoryList.FindIndex(x => x.IdProfessionalCategory == empContractSituation.IdProfessionalCategory);
                StartDate = empContractSituation.ContractSituationStartDate;
                EndDate = empContractSituation.ContractSituationEndDate;
                Remarks = empContractSituation.ContractSituationRemarks;
                ContractSituationFileName = empContractSituation.ContractSituationFileName;
                ContractSituationFileInBytes = empContractSituation.ContractSituationFileInBytes;

                int index = EmployeeContractSituationList.IndexOf(EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationStartDate == empContractSituation.ContractSituationStartDate && x.ContractSituationEndDate == empContractSituation.ContractSituationEndDate));
                if (index == 0)
                {
                    if (EmployeeContractSituationList.Count == 1)
                    {
                        MaxStartDate = null;
                        MinStartDate = null;
                        MaxEndDate = null;
                        MinEndDate = null;
                    }
                    else
                    {
                        MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                        MinStartDate = null;
                        MaxEndDate = MaxStartDate;
                    }
                }
                if (EmployeeContractSituationList.Count > 1)
                {
                    if (index + 1 == EmployeeContractSituationList.Count)
                    {
                        MaxStartDate = null;
                        MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                    }
                }
                if (index > 0 && index < EmployeeContractSituationList.Count - 1)
                {
                    MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                    MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                    MinEndDate = MinStartDate;
                    MaxEndDate = MaxStartDate;
                }
                AttachmentList = new List<object>();
                if (empContractSituation.Attachment != null)
                {
                    AttachmentList.Add(empContractSituation.Attachment);
                }
                else if (!string.IsNullOrEmpty(ContractSituationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empContractSituation.ContractSituationFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }
                if(AttachmentList.Count==0)
                    SplitVisble=Visibility.Hidden;
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
        public void InitReadOnly(ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList, EmployeeContractSituation empContractSituation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);


                ExistEmployeeContractSituation = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList.ToList());
                ExistEmployeeContractSituation.Remove(empContractSituation);
                WindowHeader = Application.Current.FindResource("EditContractSituationInformation").ToString();
                ContractSituationList = new List<ContractSituation>();
                ContractSituationList.Add(empContractSituation.ContractSituation);
                ProfessionalCategoryList = new List<ProfessionalCategory>();
                ProfessionalCategoryList.Add(empContractSituation.ProfessionalCategory);
                SelectedIndexCompany = 0;
                SelectedIndexContractSituation = 0;
                SelectedIndexProfessionalCategory = 0;
                StartDate = empContractSituation.ContractSituationStartDate;
                EndDate = empContractSituation.ContractSituationEndDate;
                Remarks = empContractSituation.ContractSituationRemarks;
                ContractSituationFileName = empContractSituation.ContractSituationFileName;
                ContractSituationFileInBytes = empContractSituation.ContractSituationFileInBytes;

                int index = EmployeeContractSituationList.IndexOf(EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationStartDate == empContractSituation.ContractSituationStartDate && x.ContractSituationEndDate == empContractSituation.ContractSituationEndDate));
                if (index == 0)
                {
                    if (EmployeeContractSituationList.Count == 1)
                    {
                        MaxStartDate = null;
                        MinStartDate = null;
                        MaxEndDate = null;
                        MinEndDate = null;
                    }
                    else
                    {
                        MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                        MinStartDate = null;
                        MaxEndDate = MaxStartDate;
                    }
                }
                if (EmployeeContractSituationList.Count > 1)
                {
                    if (index + 1 == EmployeeContractSituationList.Count)
                    {
                        MaxStartDate = null;
                        MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                    }
                }
                if (index > 0 && index < EmployeeContractSituationList.Count - 1)
                {
                    MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                    MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                    MinEndDate = MinStartDate;
                    MaxEndDate = MaxStartDate;
                }
                AttachmentList = new List<object>();
                if (empContractSituation.Attachment != null)
                {
                    AttachmentList.Add(empContractSituation.Attachment);
                }
                else if (!string.IsNullOrEmpty(ContractSituationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empContractSituation.ContractSituationFileName;
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
                    me[BindableBase.GetPropertyName(() => SelectedIndexContractSituation)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexProfessionalCategory)] +
                     me[BindableBase.GetPropertyName(() => StartDate)] +
                      me[BindableBase.GetPropertyName(() => SelectedIndexCompany)] +
                      me[BindableBase.GetPropertyName(() => EndDate)];



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
                string empselectedIndexContractSituation = BindableBase.GetPropertyName(() => SelectedIndexContractSituation);
                string empselectedIndexProfessionalCategory = BindableBase.GetPropertyName(() => SelectedIndexProfessionalCategory);
                string empcontractStartDate = BindableBase.GetPropertyName(() => StartDate);
                string empcontractEndDate = BindableBase.GetPropertyName(() => EndDate);
                string empselectedIndexCompany = BindableBase.GetPropertyName(() => SelectedIndexCompany);

                if (columnName == empselectedIndexContractSituation)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexContractSituation, SelectedIndexContractSituation);
                }

                if (columnName == empselectedIndexProfessionalCategory)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexProfessionalCategory, SelectedIndexProfessionalCategory);
                }
                if (columnName == empcontractStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empcontractStartDate, StartDate);
                    }
                }
                if (columnName == empcontractEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                }
                if (columnName == empselectedIndexCompany)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexCompany, SelectedIndexCompany);
                }
                return null;
            }

        }
        private void AddContractSituationInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguageInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexContractSituation"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexProfessionalCategory"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompany"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));

                if (error != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();
                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    ContractSituationFileName = null;
                    ContractSituationFileInBytes = null;
                }

                //EmployeeContractSituation TempEmployeeContractSituation = new EmployeeContractSituation();
                //TempEmployeeContractSituation = ExistEmployeeContractSituation.FirstOrDefault(x => x.IdContractSituation == ContractSituationList[SelectedIndexContractSituation].IdContractSituation && x.IdProfessionalCategory == ProfessionalCategoryList[SelectedIndexProfessionalCategory].IdProfessionalCategory);
                //if (TempEmployeeContractSituation == null)
                //{
                EmployeeContractSituation TempNewContractSituationForFile = new EmployeeContractSituation();
                var ExistFilesRecord = ExistEmployeeContractSituation.Where(x => x.ContractSituationFileName != null);
                TempNewContractSituationForFile = ExistFilesRecord.FirstOrDefault(x => x.ContractSituationFileName == ContractSituationFileName);

                if (TempNewContractSituationForFile == null)
                {
                    if (IsNew == true)
                    {
                        NewEmployeeContractSituation = new EmployeeContractSituation()
                        {

                            ContractSituation = ContractSituationList[SelectedIndexContractSituation],
                            IdContractSituation = ContractSituationList[SelectedIndexContractSituation].IdContractSituation,
                            IdCompany = CompanyList[selectedIndexCompany].IdCompany,
                            Company = new Company() { IdCompany= CompanyList[selectedIndexCompany].IdCompany,  Alias= CompanyList[selectedIndexCompany] .Alias},
                            ProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory],
                            IdProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory].IdProfessionalCategory,
                            ContractSituationStartDate = StartDate,
                            ContractSituationEndDate = EndDate,
                            ContractSituationFileName = ContractSituationFileName,
                            ContractSituationFileInBytes = ContractSituationFileInBytes,
                            ContractSituationRemarks = Remarks,

                            TransactionOperation = ModelBase.TransactionOperations.Add
                        };

                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditEmployeeContractSituation = new EmployeeContractSituation()
                        {
                            ContractSituation = ContractSituationList[SelectedIndexContractSituation],
                            IdContractSituation = ContractSituationList[SelectedIndexContractSituation].IdContractSituation,
                            IdCompany = CompanyList[selectedIndexCompany].IdCompany,
                            Company = new Company() { IdCompany = CompanyList[selectedIndexCompany].IdCompany, Alias = CompanyList[selectedIndexCompany].Alias },
                            ProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory],
                            IdProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory].IdProfessionalCategory,
                            ContractSituationStartDate = StartDate,
                            ContractSituationEndDate = EndDate,
                            ContractSituationFileName = ContractSituationFileName,
                            ContractSituationFileInBytes = ContractSituationFileInBytes,
                            ContractSituationRemarks = Remarks,
                            TransactionOperation = ModelBase.TransactionOperations.Update
                        };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContractSituationInformationFileExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                //}
                //else
                //{
                //    IsSave = false;
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContractSituationInformationExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //}
                GeosApplication.Instance.Logger.Log("Method AddLanguageInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLanguageInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                    ContractSituationFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    FileInfo file = new FileInfo(dlg.FileName);
                    ContractSituationFileName = file.Name;

                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = ContractSituationFileInBytes;

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
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        /// <summary>
        /// this method use for fill company list
        /// </summary>
        private void FillCompanyList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyList()...", category: Category.Info, priority: Priority.Low);

                IList<Company> tempList = HrmCommon.Instance.IsCompanyList;
                CompanyList = new List<Company>();
                CompanyList.Insert(0, new Company() { Alias = "---" });
                CompanyList.AddRange(tempList);

                GeosApplication.Instance.Logger.Log("Method FillCompanyList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
