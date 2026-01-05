using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
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
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddJobDescriptionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Task Log

        //[000][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        //[001][skale][22-08-2019][GEOS2-1681]Error in JD percentage (%)

        #endregion

        #region Services       
         ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
          IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
          IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

       // ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
       // IHrmService HrmService = new HrmServiceController("localhost:6699");
      //  IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
        #endregion

        #region Public Icommands
        public ICommand AddJobDescriptionViewCancelButtonCommand { get; set; }
        public ICommand AddJobDescriptionViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration

        private int selectedIndexCountry = 0;
        private Visibility scopeVisble;
        private Visibility countryVisble;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string PercentageErrorMessage = string.Empty;
        private string windowHeader;

        private string error = string.Empty;
        private bool isSave;
        private string remarks;
        private int selectedJobDescription;
        private DateTime? startDate;
        private DateTime? endDate;
        private string description;
        private string entity;
        private bool isNew;
        private int percentage;
        private int maxLimitOfPercentage;
        private ObservableCollection<EmployeeJobDescription> existEmployeeJobDescriptionList;
        private ObservableCollection<LookupValue> scopeList;
        private ObservableCollection<Country> countryList;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionsList;
        IWorkbenchStartUp objWorkbenchStartUp;
        private int selectedEmpolyeeStatus;
        private int selectedOrganizationIndex;
        private int selectedScope;

        //[001] added 
        private Boolean isPermissionEnabled;
        private EmployeeJobDescription empJobDescription;
        bool addShiftEnabled;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private ObservableCollection<EmployeeJobDescription> employeeTotalJobDescriptionList;

        private bool isCheckedSetMain;
        private bool isEnabledSetMain;
        private GeosAppSetting appSettingList;//[Sudhir.Jangra][GEOS2-4722]
        private int appSettingDefaultValue;//[Sudhir.Jangra][GEOS2-4722]
        #endregion

        #region Properties

        public Visibility CountryVisble
        {
            get { return countryVisble; }
            set
            {
                countryVisble = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryVisble"));
            }
        }
        public Visibility ScopeVisble
        {
            get { return scopeVisble; }
            set
            {
                scopeVisble = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScopeVisble"));
            }
        }
        public int SelectedIndexCountry
        {
            get { return selectedIndexCountry; }
            set
            {
                selectedIndexCountry = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCountry"));
            }
        }

        public int SelectedScope
        {
            get { return selectedScope; }
            set
            {
                selectedScope = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScope"));
            }
        }
        public GeosProvider CurrentGeosProvider { get; set; }
        public Int32 JDScope { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }
        //public List<Country> CountryList { get; set; }
        public List<JobDescription> JobDescriptionList { get; set; }

        //public List<LookupValue> ScopeList { get; set; }
        public ObservableCollection<LookupValue> ScopeList
        {
            get { return scopeList; }
            set
            {
                scopeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScopeList"));
            }
        }

        public ObservableCollection<Country> CountryList
        {
            get { return countryList; }
            set
            {
                countryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryList"));
            }
        }

        public List<Company> OrganizationList { get; set; }

        public EmployeeJobDescription NewEmployeeJobDescription { get; set; }

        public EmployeeJobDescription EditEmployeeJobDescription { get; set; }

        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionsList
        {
            get { return employeeJobDescriptionsList; }
            set
            {
                employeeJobDescriptionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionsList"));
            }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public int MaxLimitOfPercentage
        {
            get { return maxLimitOfPercentage; }
            set
            {
                maxLimitOfPercentage = value;
                if (maxLimitOfPercentage < 0)
                {
                    maxLimitOfPercentage = 0;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("MaxLimitOfPercentage"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
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

        public ObservableCollection<EmployeeJobDescription> ExistEmployeeJobDescriptionList
        {
            get { return existEmployeeJobDescriptionList; }
            set
            {
                existEmployeeJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> EmployeeTotalJobDescriptionList
        {
            get { return employeeTotalJobDescriptionList; }
            set
            {
                employeeTotalJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTotalJobDescriptionList"));
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public string Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Entity"));
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

        //[000] added
        public int SelectedOrganizationIndex
        {
            get { return selectedOrganizationIndex; }
            set
            {
                selectedOrganizationIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrganizationIndex"));
            }
        }


        public int SelectedJobDescription
        {
            get { return selectedJobDescription; }
            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
            }
        }

        public int Percentage
        {
            get { return percentage; }
            set
            {
                if (StartDate > DateTime.Now && (empJobDescription != null && empJobDescription.JobDescriptionUsage != null && empJobDescription.JobDescriptionUsage == 100) && value == 0)
                {
                    percentage = 100;
                }
                else
                {
                    percentage = value;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }

        public int SelectedEmpolyeeStatus
        {
            get { return selectedEmpolyeeStatus; }
            set
            {
                selectedEmpolyeeStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmpolyeeStatus"));
            }
        }

        //[001] added
        public Boolean IsPermissionEnabled
        {
            get { return isPermissionEnabled; }
            set
            {
                isPermissionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionEnabled"));
            }
        }

        public EmployeeJobDescription EmpJobDescription
        {
            get { return empJobDescription; }
            set { empJobDescription = value; }
        }
        public bool AddShiftEnabled
        {
            get { return addShiftEnabled; }
            set
            {
                addShiftEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddShiftEnabled"));
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
        public bool IsCheckedSetMain
        {
            get
            {
                return isCheckedSetMain;
            }

            set
            {
                isCheckedSetMain = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedSetMain"));
            }
        }

        public bool IsEnabledSetMain
        {
            get
            {
                return isEnabledSetMain;
            }

            set
            {
                isEnabledSetMain = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledSetMain"));
            }
        }


        //End

        public GeosAppSetting AppSettingList//[Sudhir.Jangra][GEOS2-4722]
        {
            get { return appSettingList; }
            set
            {
                appSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppSettingList"));
            }
        }

        public int AppSettingDefaultValue//[Sudhir.Jangra][GEOS2-4722]
        {
            get { return appSettingDefaultValue; }
            set
            {
                appSettingDefaultValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppSettingDefaultValue"));
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

        public AddJobDescriptionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddJobDescriptionViewModel()...", category: Category.Info, priority: Priority.Low);
                SetUserPermission();
                FillAppSetting();//[Sudhir.Jangra][GEOS2-4722]
                AddJobDescriptionViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddJobDescriptionViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddJobDescription));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(OnJDSelectedIndexChanged);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

              


                GeosApplication.Instance.Logger.Log("Constructor AddJobDescriptionViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddJobDescriptionViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods 

        private void OnJDSelectedIndexChanged(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnJDSelectedIndexChanged()...", category: Category.Info, priority: Priority.Low);
                FillScopeList();
                GeosApplication.Instance.Logger.Log("Method OnJDSelectedIndexChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnJDSelectedIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        ///  [001][21-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        public bool SetVisibilityOfMainJDByPercentage(int percentage, DateTime? endDate)
        {
            bool isVisible = true;
            if (ExistEmployeeJobDescriptionList.Count <= 0 || percentage <= 0)
            {
                if (percentage == 25)
                {
                    isVisible = false;
                    return isVisible;
                }
                else
                {
                    IsCheckedSetMain = true;
                    IsEnabledSetMain = true;
                    return isVisible;
                }
            }
            else
            {
                if (endDate != null)
                {
                    if (endDate < DateTime.Now.Date)
                    {
                        isVisible = false;
                        return isVisible;
                    }
                }
                int PendingCount = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Count();
                if (PendingCount <= 0)
                {
                    if (percentage == 25)
                    {
                        isVisible = false;
                        return isVisible;
                    }
                    else
                    {
                        IsCheckedSetMain = true;
                        IsEnabledSetMain = true;
                        return isVisible;
                    }
                }

                if (PendingCount > 0)
                {
                    //[001]
                    if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                    {
                        List<ushort> Percentages = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).Select(a => a.JobDescriptionUsage).ToList();
                        if (Percentages.Count() == 1)
                        {
                            if (percentage < Percentages.FirstOrDefault())
                            {
                                isVisible = false;
                                return isVisible;
                            }
                        }
                        else
                        {
                            int MaxPercentage = Percentages.Max();
                            if (percentage < MaxPercentage)
                            {
                                isVisible = false;
                                return isVisible;
                            }

                        }
                    }
                    else
                    {
                        List<ushort> Percentages = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();
                        if (Percentages.Count() == 1)
                        {
                            if (percentage < Percentages.FirstOrDefault())
                            {
                                isVisible = false;
                                return isVisible;
                            }
                        }
                        else
                        {
                            int MaxPercentage = Percentages.Max();
                            if (percentage < MaxPercentage)
                            {
                                isVisible = false;
                                return isVisible;
                            }

                        }
                    }

                }
            }

            return isVisible;
        }

        /// <summary>
        /// [001][skale][22-08-2019][GEOS2-1681]Error in JD percentage (%)
        /// </summary>
        /// <param name="e"></param>
        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (Convert.ToInt16(e.NewValue) == 0)
                {
                    PercentageErrorMessage = System.Windows.Application.Current.FindResource("EmployeePercentagee").ToString();
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
                }
                else
                {
                    PercentageErrorMessage = string.Empty;
                }



                //[001] added
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                {
                    IsPermissionEnabled = true;
                    if (EndDate == null || EndDate >= DateTime.Now.Date)
                    {
                        int sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                        int totalJobDescriptionUsage = sumOfJobDescriptionUsage + Convert.ToInt32(e.NewValue);
                        if (totalJobDescriptionUsage <= 100)
                            IsPermissionEnabled = true;
                        else
                            IsPermissionEnabled = false;
                    }
                    else
                    {
                        IsPermissionEnabled = false;
                    }
                }
                else
                {
                    IsPermissionEnabled = false;
                }
                //end

                //set visibility for main JD checkbox
                if (e.OldValue != "" && SetVisibilityOfMainJDByPercentage(Convert.ToInt32(e.NewValue), EndDate) == false)
                {
                    IsCheckedSetMain = false;
                    IsEnabledSetMain = false;
                }
                else
                {
                    if (IsCheckedSetMain != true)
                        IsCheckedSetMain = false;

                    IsEnabledSetMain = true;
                }

                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTextEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][22-08-2019][GEOS2-1681]Error in JD percentage (%)
        /// [002][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1
        /// </summary>
        /// <param name="e"></param>
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                if (SelectedEmpolyeeStatus != 138)
                {
                    if (StartDate != null && StartDate > DateTime.Now)
                    {

                        List<EmployeeJobDescription> ejobList = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate <= DateTime.Now.Date && (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= StartDate) && a.IdEmployeeJobDescription != EmpJobDescription.IdEmployeeJobDescription).ToList();
                        if (StartDate > DateTime.Now && ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate <= DateTime.Now.Date && (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= StartDate) && a.IdEmployeeJobDescription != EmpJobDescription.IdEmployeeJobDescription))
                        {
                            startDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeJobFutureStartDate").ToString();
                        }
                        else
                        {
                            //[002]
                            if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                            {
                                MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                            }
                            else { MaxLimitOfPercentage = 100; }
                        }

                    }
                    else
                    {
                        startDateErrorMessage = string.Empty;

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
                      
                    }
                    if (IsNew)
                    {
                        if (!GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 100))
                        {
                            //  DateTime validationdays = (DateTime)StartDate;
                            DateTime val = DateTime.Now.AddDays(AppSettingDefaultValue);//[Sudhir.Jangra][GEOS2-4722]
                            if (StartDate <= val || StartDate <= DateTime.Now)
                            {
                                startDateErrorMessage = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionStartDateValidationError").ToString(), AppSettingDefaultValue);
                            }
                            else
                            {
                                startDateErrorMessage = string.Empty;
                            }
                        }
                    }
                    
                }
                else
                {
                    startDateErrorMessage = string.Empty;

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
                    if (IsNew)
                    {
                        if (!GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 100))
                        {
                            //  DateTime validationdays = (DateTime)StartDate;
                            DateTime val = DateTime.Now.AddDays(AppSettingDefaultValue);//[Sudhir.Jangra][GEOS2-4722]
                            if (StartDate <= val || StartDate <= DateTime.Now)
                            {
                                startDateErrorMessage = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionStartDateValidationError").ToString(), AppSettingDefaultValue);
                            }
                            else
                            {
                                startDateErrorMessage = string.Empty;
                            }
                        }
                    }
                   
                }

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));

                //[001] Added
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                {
                    IsPermissionEnabled = true;
                    int sumOfJobDescriptionUsage = 0;
                    int totalJobDescriptionUsage = 0;
                    if ((StartDate > DateTime.Now))
                    {
                        sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Where(ejdl => ejdl.JobDescriptionStartDate.Value > DateTime.Now).Sum(x => x.JobDescriptionUsage);
                        totalJobDescriptionUsage = sumOfJobDescriptionUsage + Percentage;
                    }
                    else
                    {
                        sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Where(ejdl => ejdl.JobDescriptionStartDate.Value <= DateTime.Now).Sum(x => x.JobDescriptionUsage);
                        totalJobDescriptionUsage = sumOfJobDescriptionUsage + Percentage;
                    }


                    if (totalJobDescriptionUsage <= 100 && (EndDate >= DateTime.Now.Date || EndDate == null))
                    {
                        IsPermissionEnabled = true;
                    }
                    else if (totalJobDescriptionUsage >= 100 && EndDate <= DateTime.Now.Date)
                    {
                        if (EmpJobDescription != null && EmpJobDescription.JobDescriptionEndDate != endDate)
                        {
                            IsPermissionEnabled = true;
                        }
                        else
                            IsPermissionEnabled = false;
                    }
                    else if (totalJobDescriptionUsage <= 100 && EndDate <= DateTime.Now.Date)
                    {
                        IsPermissionEnabled = true;
                    }
                    else
                        IsPermissionEnabled = false;
                }
                else
                {
                    IsPermissionEnabled = false;
                }

                //set visibility for main JD checkbox
                if (SetVisibilityOfMainJDByPercentage(Percentage, EndDate) == false)
                {
                    IsCheckedSetMain = false;
                    IsEnabledSetMain = false;
                }
                else
                {
                    if (IsCheckedSetMain != true)
                        IsCheckedSetMain = false;

                    IsEnabledSetMain = true;
                }
                //End
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        /// [002][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1

        /// </summary>
        /// <param name="EmployeeJobDescriptionList"></param>
        public void Init(ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddJobDescription").ToString();
                ExistEmployeeJobDescriptionList = EmployeeJobDescriptionList;
                EmployeeJobDescription emp = new EmployeeJobDescription();
                int sumOfJobDescriptionUsage = 0;

                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(ExistEmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date));
                if (EmployeeJobDescriptionsList.Count == 0)
                {
                    sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

                    if (sumOfJobDescriptionUsage < 100)
                    {
                        MaxLimitOfPercentage = 100 - sumOfJobDescriptionUsage;
                    }
                    else
                    {
                        //[002]
                        if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                        {
                            MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                        }
                        else
                        {
                            MaxLimitOfPercentage = 0;
                        }

                    }
                }
                else
                {
                    int TotalPercentage = 0;
                    foreach (var item in EmployeeJobDescriptionsList)
                    {
                        TotalPercentage = TotalPercentage + item.JobDescriptionUsage;
                    }
                    if (sumOfJobDescriptionUsage < 100)
                    {
                        MaxLimitOfPercentage = 100 - TotalPercentage;
                    }
                    else
                    {
                        //[002]
                        if (EmployeeJobDescriptionsList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                        {
                            MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                        }
                        else
                            MaxLimitOfPercentage = 0;
                    }
                }

                Percentage = MaxLimitOfPercentage;
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();

                FillJobDescriptionList();
                FillOrganizationList();
                //FillScopeList();
                SelectedOrganizationIndex = OrganizationList.FindIndex(x => x.IdCompany == CurrentGeosProvider.IdCompany); //[01] added

                //set visibility for main JD checkbox
                if (SetVisibilityOfMainJDByPercentage(Percentage, EndDate) == false)
                {
                    IsCheckedSetMain = false;
                    IsEnabledSetMain = false;
                }
                else
                {
                    if (IsCheckedSetMain != true)
                        IsCheckedSetMain = false;

                    IsEnabledSetMain = true;
                }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        /// [002][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1

        /// </summary>
        /// <param name="EmployeeJobDescriptionList"></param>
        /// <param name="empJobDescription"></param>
        //public void EditInit(ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList, EmployeeJobDescription empJobDescription, List<EmployeeJobDescription> ExistJobDescriptionList = null)
        public void EditInit(ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList, EmployeeJobDescription empJobDescription)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditJobDescription").ToString();

                ExistEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList());

                if (ExistEmployeeJobDescriptionList == null)
                {
                    ExistEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                }

                EmployeeTotalJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(ExistEmployeeJobDescriptionList.ToList());

                ExistEmployeeJobDescriptionList.Remove(empJobDescription);
                FillJobDescriptionList();

                EmpJobDescription = empJobDescription;

                FillOrganizationList();
                SelectedOrganizationIndex = OrganizationList.FindIndex(x => x.IdCompany == empJobDescription.IdCompany);
                SelectedJobDescription = JobDescriptionList.FindIndex(x => x.IdJobDescription == empJobDescription.IdJobDescription);


                SelectedJobDescription = JobDescriptionList.FindIndex(x => x.IdJobDescription == empJobDescription.IdJobDescription);

                int sumOfJobDescriptionUsage = 0;

                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(ExistEmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now));
                sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

                if (sumOfJobDescriptionUsage < 100)
                {
                    MaxLimitOfPercentage = 100 - sumOfJobDescriptionUsage;
                }
                else
                {
                    //[002]
                    if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                    {
                        MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                    }
                    else
                    {
                        MaxLimitOfPercentage = 0;
                    }

                }

                Percentage = empJobDescription.JobDescriptionUsage;
                Remarks = empJobDescription.JobDescriptionRemarks;

                StartDate = (DateTime)empJobDescription.JobDescriptionStartDate;
                EndDate = empJobDescription.JobDescriptionEndDate;

                if (empJobDescription.JobDescriptionEndDate != null || empJobDescription.JobDescriptionEndDate < DateTime.Now)
                {
                    MaxLimitOfPercentage = empJobDescription.JobDescriptionUsage;
                }
                //[002]
                if (empJobDescription.IsMainJobDescription == 1 && (empJobDescription.JobDescriptionEndDate >= DateTime.Now.Date || empJobDescription.JobDescriptionEndDate == null))
                {
                    IsCheckedSetMain = true;
                    IsEnabledSetMain = true;
                }
                else
                {
                    IsCheckedSetMain = false;
                    IsEnabledSetMain = false;
                }

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
        /// [001][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1
        /// [002][cpatil][24-03-2022][GEOS2-3496]HRM - Add new data after exit event [#ERF94]
        /// </summary>
        public void InitReadOnly(ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList, EmployeeJobDescription empJobDescription)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditJobDescription").ToString();
                ExistEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList());
                ExistEmployeeJobDescriptionList.Remove(empJobDescription);
                OrganizationList = new List<Company>();
                OrganizationList.Add(empJobDescription.Company);

                JobDescriptionList = new List<JobDescription>();
                empJobDescription.JobDescription.JobDescriptionTitleAndCode = empJobDescription.JobDescription.JobDescriptionCode + "-" + empJobDescription.JobDescription.JobDescriptionTitle;
                JobDescriptionList.Add(empJobDescription.JobDescription);

                SelectedOrganizationIndex = 0;
                SelectedJobDescription = 0;

                int sumOfJobDescriptionUsage = 0;
                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(ExistEmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date) || a.JobDescriptionEndDate >= DateTime.Now));
                sumOfJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                if (sumOfJobDescriptionUsage < 100)
                {
                    MaxLimitOfPercentage = 100 - sumOfJobDescriptionUsage;
                }
                else
                {
                    //[001]
                    if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                    {
                        MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                    }
                    else
                        MaxLimitOfPercentage = 0;
                }

                Percentage = empJobDescription.JobDescriptionUsage;
                Remarks = empJobDescription.JobDescriptionRemarks;
                StartDate = (DateTime)empJobDescription.JobDescriptionStartDate;
                EndDate = empJobDescription.JobDescriptionEndDate;
                //[001]
                if (empJobDescription.IsMainJobDescription == 1 && (empJobDescription.JobDescriptionEndDate >= DateTime.Now.Date || (empJobDescription.JobDescriptionEndDate == null && empJobDescription.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date)))
                {
                    IsCheckedSetMain = true;
                    IsEnabledSetMain = true;
                }
                else
                {
                    IsCheckedSetMain = false;
                    IsEnabledSetMain = false;
                }

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
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
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOrganizationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry ...", category: Category.Info, priority: Priority.Low);



                IList<Country> tempCountryList = CrmStartUp.GetCountries();
                CountryList = new ObservableCollection<Country>();
                CountryList.Insert(0, new Country() { Name = "---" });
                //ScopeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                CountryList.AddRange(tempCountryList);
                CountryList = new ObservableCollection<Country>(CountryList);
                if (empJobDescription != null && (CountryList.ToList().FindIndex(x => x.IdCountry == empJobDescription.IdCountry) != -1))
                    SelectedIndexCountry = CountryList.ToList().FindIndex(x => x.IdCountry == empJobDescription.IdCountry);
                else
                    SelectedIndexCountry = 0;
                GeosApplication.Instance.Logger.Log("Method FillCountry() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillScopeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScopeList()...", category: Category.Info, priority: Priority.Low);

                CountryList = null;
                ScopeList = null;
                JDScope = JobDescriptionList[SelectedJobDescription].IdJdScope;
                if (JDScope != null)
                {
                    if (JDScope == 278)
                    {
                        FillCountry();
                        CountryVisble = Visibility.Visible;
                        ScopeVisble = Visibility.Collapsed;
                    }
                    else if (JDScope == 279)
                    {
                        IList<LookupValue> tempList = CrmStartUp.GetLookupValues(8);
                        ScopeList = new ObservableCollection<LookupValue>(tempList);
                        //ScopeList.AddRange(0, new ObservableCollection<LookupValue> { Value = "---", IdLookupValue = 0 });
                        ScopeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                        ScopeList = new ObservableCollection<LookupValue>(ScopeList);

                        if (empJobDescription != null && (ScopeList.ToList().FindIndex(x => x.IdLookupValue == empJobDescription.IdRegion) != -1))
                            SelectedScope = ScopeList.ToList().FindIndex(x => x.IdLookupValue == empJobDescription.IdRegion);
                        else
                            SelectedScope = 0;
                        CountryVisble = Visibility.Collapsed;
                        ScopeVisble = Visibility.Visible;
                    }
                    else
                    {
                        CountryList = null;
                        ScopeList = null;
                        SelectedScope = SelectedIndexCountry = 0;
                        CountryVisble = ScopeVisble = Visibility.Collapsed;
                    }
                    //IList<LookupValue> tempScopeList = HrmService.GetAllJobDescriptions_V2046().OrderBy(a => a.JobDescriptionTitleAndCode).ToLookup();
                    //ScopeList = new List<LookupValue>();
                    //ScopeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                    //ScopeList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---" });
                    //ScopeList.AddRange(tempScopeList);
                    //SelectedScope = 0;
                }
                GeosApplication.Instance.Logger.Log("Method FillScopeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillScopeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillJobDescriptionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()...", category: Category.Info, priority: Priority.Low);
                IList<JobDescription> tempCountryList = HrmService.GetAllJobDescriptions_V2046().OrderBy(a => a.JobDescriptionTitleAndCode).ToList();
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

        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        /// </summary>
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error = me[BindableBase.GetPropertyName(() => SelectedJobDescription)] +
                               me[BindableBase.GetPropertyName(() => SelectedOrganizationIndex)] + //[01] added
                               me[BindableBase.GetPropertyName(() => StartDate)] +
                               me[BindableBase.GetPropertyName(() => EndDate)] +
                               me[BindableBase.GetPropertyName(() => SelectedScope)] +
                               me[BindableBase.GetPropertyName(() => SelectedIndexCountry)] +
                               me[BindableBase.GetPropertyName(() => Percentage)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }



        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string empselectedJobDescription = BindableBase.GetPropertyName(() => SelectedJobDescription);
                string empSelectedOrganizationIndex = BindableBase.GetPropertyName(() => SelectedOrganizationIndex); //[01] Added
                string empcontractStartDate = BindableBase.GetPropertyName(() => StartDate);
                string empcontractEndDate = BindableBase.GetPropertyName(() => EndDate);
                string empPercentage = BindableBase.GetPropertyName(() => Percentage);
                string empSelectedScope = BindableBase.GetPropertyName(() => SelectedScope);
                string empSelectedCountry = BindableBase.GetPropertyName(() => SelectedIndexCountry);

                if (columnName == empselectedJobDescription)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedJobDescription, SelectedJobDescription);
                }
                //[01] added
                if (columnName == empSelectedOrganizationIndex)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedOrganizationIndex, SelectedOrganizationIndex);
                }
                //end
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

                if (columnName == empPercentage)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empPercentage, Percentage);
                }

                if (columnName == empSelectedScope && ScopeVisble == Visibility.Visible)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedScope, SelectedScope);
                }

                if (columnName == empSelectedCountry && CountryVisble == Visibility.Visible)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedCountry, SelectedIndexCountry);
                }

                return null;
            }
        }

        /// <summary>
        /// [001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        /// [001][smazhar][06-29-2020][GEOS2-2441] Job Description Already Exist - Even if the previous is closed.
        ///  [002][21-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        /// </summary>
        /// <param name="obj"></param>
        private void AddJobDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddJobDescription()...", category: Category.Info, priority: Priority.Low);

               


                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganizationIndex")); //[01] added
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedScope"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));

                if (error != null)
                {
                    
                    return;
                }

                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();

                //[01] Added

                EmployeeJobDescription TempNewJobDescription = null;

                if (IsNew)
                    TempNewJobDescription = ExistEmployeeJobDescriptionList.FirstOrDefault(x => x.IdJobDescription == JobDescriptionList[SelectedJobDescription].IdJobDescription
                                                                                                 && x.JobDescriptionEndDate >= DateTime.Now.Date);

                if (TempNewJobDescription == null)
                {
                    Visibility visible = Visibility.Hidden;
                    if (IsNew == true)
                    {
                        List<ushort> Percentages = new List<ushort>();


                        if (IsCheckedSetMain == false)
                        {
                            //[002]
                            if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                            {
                                Percentages = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).Select(a => a.JobDescriptionUsage).ToList();
                                if (Percentages.Count > 0 && Percentages.Max() < Percentage && (EndDate >= DateTime.Now.Date || EndDate == null))
                                {
                                    IsCheckedSetMain = true;
                                    visible = Visibility.Visible;
                                }

                            }
                            else
                            {
                                // [001] Added
                                Percentages = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();

                                if (Percentages.Count > 0 && Percentages.Max() < Percentage && (EndDate >= DateTime.Now.Date || EndDate == null))
                                {
                                    IsCheckedSetMain = true;
                                    visible = Visibility.Visible;
                                }
                                else if (StartDate.Value.Date > DateTime.Now.Date)
                                {
                                    IsCheckedSetMain = true;
                                    visible = Visibility.Visible;
                                }
                            }
                        }
                        else
                        {
                            visible = Visibility.Visible;
                        }
                        if (Percentages.Count == 0 && Percentage != 25)
                        {
                            visible = Visibility.Hidden;
                        }


                        if (CountryVisble == Visibility.Visible)
                            NewEmployeeJobDescription = new EmployeeJobDescription()
                            {
                                IsMainVisible = visible,
                                IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0,
                                Company = OrganizationList[SelectedOrganizationIndex],
                                IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany,
                                JobDescription = JobDescriptionList[SelectedJobDescription],
                                IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription,
                                Country = CountryList[SelectedIndexCountry],
                                IdCountry = CountryList[SelectedIndexCountry].IdCountry,
                                StrJDScope = CountryList[SelectedIndexCountry].Name,
                                JobDescriptionUsage = (ushort)Percentage,
                                JobDescriptionStartDate = StartDate,
                                JobDescriptionEndDate = EndDate,
                                JobDescriptionRemarks = Remarks,
                                TransactionOperation = ModelBase.TransactionOperations.Add
                            };
                        else if (ScopeVisble == Visibility.Visible)
                            NewEmployeeJobDescription = new EmployeeJobDescription()
                            {
                                IsMainVisible = visible,
                                IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0,
                                Company = OrganizationList[SelectedOrganizationIndex],
                                IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany,
                                JobDescription = JobDescriptionList[SelectedJobDescription],
                                IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription,
                                Region = ScopeList[SelectedScope],
                                IdRegion = ScopeList[SelectedScope].IdLookupValue,
                                StrJDScope = ScopeList[SelectedScope].Value,
                                JobDescriptionUsage = (ushort)Percentage,
                                JobDescriptionStartDate = StartDate,
                                JobDescriptionEndDate = EndDate,
                                JobDescriptionRemarks = Remarks,
                                TransactionOperation = ModelBase.TransactionOperations.Add
                            };
                        else
                            NewEmployeeJobDescription = new EmployeeJobDescription() { IsMainVisible = visible, IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0, Company = OrganizationList[SelectedOrganizationIndex], IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany, JobDescription = JobDescriptionList[SelectedJobDescription], IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription, JobDescriptionUsage = (ushort)Percentage, JobDescriptionStartDate = StartDate, JobDescriptionEndDate = EndDate, JobDescriptionRemarks = Remarks, TransactionOperation = ModelBase.TransactionOperations.Add };
                        int sumOfJobDescriptionUsage = 0;
                        foreach (var list in ExistEmployeeJobDescriptionList)
                        {
                            sumOfJobDescriptionUsage = sumOfJobDescriptionUsage + list.JobDescriptionUsage;
                        }
                        if (sumOfJobDescriptionUsage < 100)
                        {
                            MaxLimitOfPercentage = 100 - sumOfJobDescriptionUsage;
                        }
                        else
                        {
                            if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                            {
                                MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                            }
                            else
                                MaxLimitOfPercentage = 0;
                        }

                        IsSave = true;
                        RequestClose(null, null);
                    }
                    else
                    {
                        //[01] added
                        List<ushort> Percentages = ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();
                        if (IsCheckedSetMain == false)
                        {
                            if (Percentages.Count > 0 && Percentages.Max() <= Percentage && (EndDate >= DateTime.Now.Date || EndDate == null))
                            {
                                if (ExistEmployeeJobDescriptionList.Any(a => (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)) && a.IsMainJobDescription == 1))
                                {
                                    int pervPercentage = ExistEmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)) && a.IsMainJobDescription == 1).FirstOrDefault().JobDescriptionUsage;
                                    if (pervPercentage != Percentage)
                                    {
                                        IsCheckedSetMain = true;
                                        visible = Visibility.Visible;
                                    }
                                }
                                else
                                {
                                    IsCheckedSetMain = true;
                                    visible = Visibility.Visible;
                                }

                            }
                        }
                        else
                        {
                            visible = Visibility.Visible;
                        }
                        if (Percentages.Count == 0 && Percentage != 25)
                        {
                            visible = Visibility.Hidden;
                        }

                        //sjadhav
                        if (CountryVisble == Visibility.Visible)
                            EditEmployeeJobDescription = new EmployeeJobDescription()
                            {
                                IsMainVisible = visible,
                                IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0,
                                Company = OrganizationList[SelectedOrganizationIndex],
                                IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany,
                                JobDescription = JobDescriptionList[SelectedJobDescription],
                                IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription,
                                Country = CountryList[SelectedIndexCountry],
                                IdCountry = CountryList[SelectedIndexCountry].IdCountry,
                                JobDescriptionUsage = (ushort)Percentage,
                                JobDescriptionStartDate = StartDate,
                                JobDescriptionEndDate = EndDate,
                                JobDescriptionRemarks = Remarks,
                                TransactionOperation = ModelBase.TransactionOperations.Update
                            };

                        else if (ScopeVisble == Visibility.Visible)
                            EditEmployeeJobDescription = new EmployeeJobDescription()
                            {
                                IsMainVisible = visible,
                                IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0,
                                Company = OrganizationList[SelectedOrganizationIndex],
                                IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany,
                                JobDescription = JobDescriptionList[SelectedJobDescription],
                                IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription,
                                Region = ScopeList[SelectedScope],
                                IdRegion = ScopeList[SelectedScope].IdLookupValue,

                                JobDescriptionUsage = (ushort)Percentage,
                                JobDescriptionStartDate = StartDate,
                                JobDescriptionEndDate = EndDate,
                                JobDescriptionRemarks = Remarks,
                                TransactionOperation = ModelBase.TransactionOperations.Update
                            };
                        else EditEmployeeJobDescription = new EmployeeJobDescription()
                        {
                            IsMainVisible = visible,
                            IsMainJobDescription = IsCheckedSetMain == true ? (byte)1 : (byte)0,
                            Company = OrganizationList[SelectedOrganizationIndex],
                            IdCompany = OrganizationList[SelectedOrganizationIndex].IdCompany,
                            JobDescription = JobDescriptionList[SelectedJobDescription],
                            IdJobDescription = JobDescriptionList[SelectedJobDescription].IdJobDescription,
                            JobDescriptionUsage = (ushort)Percentage,
                            JobDescriptionStartDate = StartDate,
                            JobDescriptionEndDate = EndDate,
                            JobDescriptionRemarks = Remarks,
                            TransactionOperation = ModelBase.TransactionOperations.Update
                        };


                        EmployeeJobDescription employeeJobDescription = EmployeeTotalJobDescriptionList.FirstOrDefault(b => b.IdJobDescription == JobDescriptionList[SelectedJobDescription].IdJobDescription);
                        int sumOfEmployeeJD = 0;
                        sumOfEmployeeJD = EmployeeJobDescriptionsList.Sum(a => a.JobDescriptionUsage);

                        if (employeeJobDescription != null && employeeJobDescription.JobDescriptionEndDate != null)
                        {


                            if (EndDate == null && sumOfEmployeeJD == 100)
                            {
                                if (StartDate > GeosApplication.Instance.ServerDateTime.Date)
                                {
                                    if (EmployeeJobDescriptionsList.Any(i => i.JobDescriptionStartDate.Value > GeosApplication.Instance.ServerDateTime.Date))
                                    {
                                        sumOfEmployeeJD = EmployeeJobDescriptionsList.Where(i => i.JobDescriptionStartDate.Value > GeosApplication.Instance.ServerDateTime.Date).Sum(a => a.JobDescriptionUsage);
                                        if (sumOfEmployeeJD == 100)
                                        {
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeEndDateNotNullMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                            IsSave = false;
                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeEndDateNotNullMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    IsSave = false;
                                    return;
                                }

                            }

                            if (sumOfEmployeeJD >= 100 && EndDate > GeosApplication.Instance.ServerDateTime.Date && employeeJobDescription.IdEmployeeJobDescription == 0)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeNotEnterFutureEndDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                IsSave = false;
                                return;
                            }
                        }

                        int sumOfJobDescriptionUsage = 0;
                        foreach (var list in ExistEmployeeJobDescriptionList)
                        {
                            sumOfJobDescriptionUsage = sumOfJobDescriptionUsage + list.JobDescriptionUsage;
                        }
                        if (sumOfJobDescriptionUsage < 100)
                        {
                            MaxLimitOfPercentage = 100 - sumOfJobDescriptionUsage;
                        }

                        else
                        {
                            if (ExistEmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                            {
                                MaxLimitOfPercentage = 100 - ExistEmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                            }
                            else
                                MaxLimitOfPercentage = 0;
                        }

                        //EmployeeJobDescription employeeJobDescription = EmployeeTotalJobDescriptionList.FirstOrDefault(b => b.IdJobDescription == JobDescriptionList[SelectedJobDescription].IdJobDescription);
                        //if (employeeJobDescription.JobDescriptionEndDate != null)
                        //{
                        //    if (EndDate == null)
                        //    {
                        //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeEndDateNotNullMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //        IsSave = false;
                        //        //RequestClose(null, null);
                        //        return;
                        //    }
                        //}


                        IsSave = true;
                        RequestClose(null, null);
                    }

                    //EmployeeJobDescription employeeJobDescription = EmployeeTotalJobDescriptionList.FirstOrDefault(b => b.IdJobDescription == JobDescriptionList[SelectedJobDescription].IdJobDescription);
                    //if(employeeJobDescription.JobDescriptionEndDate != null)
                    //{
                    //    if (EndDate == null)
                    //    {
                    //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeEndDateNotNullMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //        IsSave = false;
                    //        RequestClose(null, null);
                    //    }
                    //}                  
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddJobDescriptionExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
               
                GeosApplication.Instance.Logger.Log("Method AddJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
               
                GeosApplication.Instance.Logger.Log("Get an error in Method AddJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void FillAppSetting()
        {
            try
            {
                //[Sudhir.Jangra][GEOS2-4722]
                AppSettingList = WorkbenchStartUp.GetGeosAppSettings(116);

                AppSettingDefaultValue = Convert.ToInt32(AppSettingList.DefaultValue);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAppSetting...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
