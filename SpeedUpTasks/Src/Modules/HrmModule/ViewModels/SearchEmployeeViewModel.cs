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
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Hrm.Views;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.ServiceModel;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Editors;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class SearchEmployeeViewModel: ViewModelBase, INotifyPropertyChanged
    {

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services
        #region Declaration
        private Employee selectedGridRow;
        private ObservableCollection<Employee> finalEmployeeList;
      
        private DateTime? startDate;
        private DateTime? endDate;
        private string lastName;
        private string firstName;
       
        private Employee selectedEmployee;
        private int selectedPeriodIndex;
        private bool isBusy;
        private string visible;
        private List<object> selectedAuthorizedPlantsList;
        private ObservableCollection<Company> combineIslocationIsorganizationIscompanyList;
        #endregion  //Declaration

        #region public Properties

        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        public Employee SelectedEmployee
        {
            get
            {
                return selectedEmployee;
            }

            set
            {
                selectedEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployee"));
            }
        }
        //public List<string> PeriodList { get; set; }
        public ObservableCollection<long> PeriodList { get; set; }
      
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public Employee SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }
        public ObservableCollection<Employee> FinalEmployeeList
        {
            get { return finalEmployeeList; }
            set
            {
                finalEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalEmployeeList"));
            }
        }
        public static bool IsPreferenceChanged { get; set; }
        public int SelectedPeriodIndex
        {
            get { return selectedPeriodIndex; }
            set
            {
                selectedPeriodIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriodIndex"));
            }
        }
        public String FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        

        public String LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
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

        public ObservableCollection<Company> CombineIslocationIsorganizationIscompanyList
        {
            get
            {
                return combineIslocationIsorganizationIscompanyList;
            }

            set
            {
                combineIslocationIsorganizationIscompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CombineIslocationIsorganizationIscompanyList"));
            }
        }
        #endregion // Properties

        #region public ICommand
        public ICommand ExportAttendanceViewCancelButtonCommand { get; set; }
        public ICommand SearchButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand PeriodEditValueChangedCommand { get; private set; }
        public ICommand CommandGridDoubleClick { get; set; }

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

        //bool allowValidation = false;
        //string EnableValidationAndGetError()
        //{
        //    allowValidation = true;
        //    string error = ((IDataErrorInfo)this).Error;
        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        return error;
        //    }
        //    return null;
        //}
        #endregion

        #region Constructor
        public SearchEmployeeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SearchEmployeeViewModel ...", category: Category.Info, priority: Priority.Low);

                ExportAttendanceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SearchButtonCommand = new DelegateCommand<object>(SearchEmployee);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CommandGridDoubleClick = new DelegateCommand<object>(CommandGridDoubleClickMethod);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                
                FinalEmployeeList = new ObservableCollection<Employee>();
               
                StartDate = new DateTime(DateTime.Now.Year, 1,1);
                EndDate = DateTime.Today;               
                FillFinancialYear();

                if (HrmCommon.Instance.CombineIslocationIsorganizationIscompanyList != null)
                {
                    CombineIslocationIsorganizationIscompanyList = new ObservableCollection<Company>(HrmCommon.Instance.CombineIslocationIsorganizationIscompanyList);
                    SelectedAuthorizedPlantsList = HrmCommon.Instance.SelectedAuthorizedPlantsList;
                }
                GeosApplication.Instance.Logger.Log("Constructor SearchEmployeeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SearchEmployeeViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Constructor

        #region Methods

        private void ShortcutAction(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        public void FillFinancialYear()
        {
           DateTime maxDate = GeosApplication.Instance.ServerDateTime.Date;
            PeriodList = new ObservableCollection<long>();
          
            for (long i = maxDate.Year + 1; i >= 2000; i--)
            {
                PeriodList.Add(i);
            }
            
        }
        private void CommandGridDoubleClickMethod(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandGridDoubleClickMethod...", category: Category.Info, priority: Priority.Low);

                Employee employee = null;
                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    employee = (Employee)detailView.DataControl.CurrentItem;
                    SelectedGridRow = employee;
                }
                else if (obj is Employee)
                {
                    employee = (Employee)obj;
                    SelectedGridRow = employee;
                }

                if (employee != null)
                {
                    EmployeeProfileDetailView employeeProfileDetailView = new EmployeeProfileDetailView();
                    EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                    EventHandler handle = delegate { employeeProfileDetailView.Close(); };
                    employeeProfileDetailViewModel.RequestClose += handle;
                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;

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
                    if (SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            employeeProfileDetailViewModel.InitReadOnly(SelectedGridRow, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                        else
                            employeeProfileDetailViewModel.Init(SelectedGridRow, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());

                    }

                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
                    employeeProfileDetailViewModel.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    var ownerInfo = (obj as FrameworkElement);
                    employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);

                    employeeProfileDetailView.ShowDialog();
                }
                GeosApplication.Instance.Logger.Log("Method CommandGridDoubleClickMethod... executed", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CommandGridDoubleClickMethod() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CommandGridDoubleClickMethod() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CommandGridDoubleClickMethod() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchEmployee(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SearchEmployee()...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
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
                    List<Company> plantOwners = SelectedAuthorizedPlantsList.Cast<Company>().ToList();//[001] added
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    ObservableCollection<Employee> AllEmployeeGridList = new ObservableCollection<Employee>();

                    
                    if (String.IsNullOrEmpty(FirstName))
                        FirstName = "";
                    if (String.IsNullOrEmpty(LastName))
                        LastName = "";

                    ComboBoxEdit cmbPeriod = (ComboBoxEdit)obj;
                  

                    AllEmployeeGridList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesShortDetail(plantOwnersIds, string.Join(",", cmbPeriod.SelectedItems.ToList()), HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, FirstName, LastName));

                  
                    FinalEmployeeList.Clear();
                    FinalEmployeeList.AddRange(AllEmployeeGridList);
                    if (FinalEmployeeList.Count > 0)
                        SelectedEmployee = FinalEmployeeList.FirstOrDefault();


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method SearchEmployee()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SearchOpportunityOrOrder() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsPreferenceChanged = false;
            RequestClose(null, null);
        }     
    }
        #endregion //Methods 
    }