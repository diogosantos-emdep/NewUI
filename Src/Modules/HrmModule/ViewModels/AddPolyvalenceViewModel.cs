using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class AddPolyvalenceViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommands
        public ICommand AddPolyvalenceViewCancelButtonCommand { get; set; }
        public ICommand AddPolyvalenceViewAcceptButtonCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }

        public ICommand CommandTextInput { get; set; }

        #endregion


        #region Declaration

        private bool isSave;
        private string windowHeader;
        private bool isNew;
        private int maxLimitOfPercentage;
        private int percentage;
        private int selectedOrganizationIndex;
        IWorkbenchStartUp objWorkbenchStartUp;
        List<JobDescription> jobDescriptionList;
        private string PercentageErrorMessage = string.Empty;
        private string error = string.Empty;
        private string remarks;
        private int selectedJobDescription;
        private ObservableCollection<EmployeePolyvalence> employeePolyvalenceList;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionsList;
        private ObservableCollection<EmployeePolyvalence> existEmployeePolyvalenceList;
        bool allowValidation = false;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
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


        #region Properties
        public EmployeePolyvalence NewEmployeePolyvalence { get; set; }
        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }
        public EmployeePolyvalence EditEmployeePolyvalence { get; set; }

        public List<JobDescription> JobDescriptionList
        {
            get { return jobDescriptionList; }
            set
            {
                jobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionList"));
            }
        }
        public List<Company> OrganizationList { get; set; }
        public ObservableCollection<EmployeePolyvalence> ExistEmployeePolyvalenceList
        {
            get
            {
                return existEmployeePolyvalenceList;
            }

            set
            {
                existEmployeePolyvalenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeePolyvalenceList"));
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
      
        public int Percentage
        {
            get
            {
                return percentage;
            }

            set
            {
                percentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }
        public int MaxLimitOfPercentage
        {
            get
            {
                return maxLimitOfPercentage;
            }

            set
            {
                maxLimitOfPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxLimitOfPercentage"));
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
        public int SelectedOrganizationIndex
        {
            get
            {
                return selectedOrganizationIndex;
            }

            set
            {
                selectedOrganizationIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrganizationIndex"));
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
        public int SelectedJobDescription
        {
            get
            {
                return selectedJobDescription;
            }

            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
            }
        }
      
        public ObservableCollection<EmployeePolyvalence> EmployeePolyvalenceList
        {
            get { return employeePolyvalenceList; }
            set
            {
                employeePolyvalenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeePolyvalenceList"));
            }
        }
        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionsList
        {
            get { return employeeJobDescriptionsList; }
            set
            {
                employeeJobDescriptionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionsList"));
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


        #region Constructor
        public AddPolyvalenceViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddPolyvalenceViewModel()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("Polyvalence").ToString();
                AddPolyvalenceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddPolyvalenceViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddEmployeePolyvalence));
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                MaxLimitOfPercentage = 100;

                GeosApplication.Instance.Logger.Log("Constructor AddPolyvalenceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddPolyvalenceViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region Methods


        public void Init(ObservableCollection<EmployeePolyvalence> EmployeePolyvalenceList )
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddPolyvalence").ToString();
                ExistEmployeePolyvalenceList = EmployeePolyvalenceList;
                EmployeePolyvalence emp = new EmployeePolyvalence();
                Percentage = MaxLimitOfPercentage;
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();
                FillOrganizationList();
                FillJobDescriptionList();
                SelectedOrganizationIndex = OrganizationList.FindIndex(x => x.IdCompany == CurrentGeosProvider.IdCompany);
                if (SelectedOrganizationIndex == -1)
                    SelectedOrganizationIndex = 0;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeePolyvalence> EmployeePolyvalence, EmployeePolyvalence empPolyvalence)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditPolyvalence").ToString();
                ExistEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(EmployeePolyvalence.ToList());
                ExistEmployeePolyvalenceList.Remove(empPolyvalence);
                FillJobDescriptionList();
                FillOrganizationList();

                SelectedOrganizationIndex = OrganizationList.FindIndex(x => x.IdCompany == empPolyvalence.IdCompany);
                if (SelectedOrganizationIndex == -1)
                    SelectedOrganizationIndex = 0;
                SelectedJobDescription = JobDescriptionList.FindIndex(x => x.IdJobDescription == empPolyvalence.IdJobDescription);
                Percentage = empPolyvalence.PolyvalenceUsage;
                Remarks = empPolyvalence.PolyvalenceRemarks;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// </summary>
        public void InitReadOnly(ObservableCollection<EmployeePolyvalence> EmployeePolyvalence, EmployeePolyvalence empPolyvalence)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditPolyvalence").ToString();
                ExistEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(EmployeePolyvalence.ToList());
                ExistEmployeePolyvalenceList.Remove(empPolyvalence);
                OrganizationList = new List<Company>();
                OrganizationList.Add(empPolyvalence.Company);
                JobDescriptionList = new List<JobDescription>();
                empPolyvalence.JobDescription.JobDescriptionTitleAndCode = empPolyvalence.JobDescription.JobDescriptionCode + "-" + empPolyvalence.JobDescription.JobDescriptionTitle;
                JobDescriptionList.Add(empPolyvalence.JobDescription);
                SelectedOrganizationIndex = 0;
                SelectedJobDescription = 0;
                Percentage = empPolyvalence.PolyvalenceUsage;
                Remarks = empPolyvalence.PolyvalenceRemarks;

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (Convert.ToInt16(e.NewValue) == 0)
                {
                    PercentageErrorMessage = System.Windows.Application.Current.FindResource("EmployeePercentagee").ToString();
                }
                else
                {
                    PercentageErrorMessage = string.Empty;
                }

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTextEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                    me[BindableBase.GetPropertyName(() => SelectedJobDescription)] +
                    me[BindableBase.GetPropertyName(() => SelectedOrganizationIndex)] +
                       me[BindableBase.GetPropertyName(() => Percentage)];

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
                string empselectedJobDescription = BindableBase.GetPropertyName(() => SelectedJobDescription);
                string empSelectedOrganizationIndex = BindableBase.GetPropertyName(() => SelectedOrganizationIndex);
                string empPercentage = BindableBase.GetPropertyName(() => Percentage);


                if (columnName == empselectedJobDescription)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedJobDescription, SelectedJobDescription);
                }

                if (columnName == empSelectedOrganizationIndex)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedOrganizationIndex, SelectedOrganizationIndex);
                }
                
                if (columnName == empPercentage)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empPercentage, Percentage);
                }

                return null;
            }
        }
        /// <summary>
        /// this method use for add new employee polyvalence
        /// </summary>
        /// <param name="obj"></param>
        private void AddEmployeePolyvalence(object obj)
        {
            error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganizationIndex"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));
            PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
            if (error != null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(Remarks))
                Remarks = Remarks.Trim();
            EmployeePolyvalence TempNewPolyvalence = new EmployeePolyvalence();
            TempNewPolyvalence = ExistEmployeePolyvalenceList.FirstOrDefault(x => x.IdCompany == OrganizationList[SelectedOrganizationIndex].IdCompany && x.IdJobDescription == JobDescriptionList[SelectedJobDescription].IdJobDescription);
            if (TempNewPolyvalence == null)
                {
                    if (IsNew == true)
                    {
                        NewEmployeePolyvalence = new EmployeePolyvalence() { Company = OrganizationList[SelectedOrganizationIndex], IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany, JobDescription = JobDescriptionList[SelectedJobDescription], IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription, PolyvalenceUsage = (ushort)Percentage, PolyvalenceRemarks = Remarks, TransactionOperation = ModelBase.TransactionOperations.Add };
                        int sumOfPolyvalenceUsage = 0;
                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditEmployeePolyvalence = new EmployeePolyvalence() { Company = OrganizationList[SelectedOrganizationIndex], IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany, JobDescription = JobDescriptionList[SelectedJobDescription], IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription, PolyvalenceUsage = (ushort)Percentage, PolyvalenceRemarks = Remarks, TransactionOperation = ModelBase.TransactionOperations.Update };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeePolyvalenceExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

            GeosApplication.Instance.Logger.Log("Method AddEmployeePolyvalence()...", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// this method use for fill company list
        /// </summary>
        private void FillOrganizationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrganizationList()...", category: Category.Info, priority: Priority.Low);

                IList<Company> tempList = HrmCommon.Instance.IsOrganizationList;
                OrganizationList = new List<Company>();
                OrganizationList.Insert(0, new Company() { Alias = "---" });
                OrganizationList.AddRange(tempList);

                GeosApplication.Instance.Logger.Log("Method FillOrganizationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOrganizationList()..." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// this method use for Fill Job Description list
        /// </summary>
        private void FillJobDescriptionList()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()...", category: Category.Info, priority: Priority.Low);
                IList<JobDescription> tempCountryList = HrmService.GetAllJobDescriptions(); ;
                JobDescriptionList = new List<JobDescription>();
                JobDescriptionList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---" });
                JobDescriptionList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillJobDescriptionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        //--------------mazhar--------------------//
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
