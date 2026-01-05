using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Async;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{


    internal class EmployeeBacklogHolidaysViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        // [nsatpute][14-11-2024][GEOS2-5747]
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Fields
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => throw new NotImplementedException();
        private bool showBacklogDays;
        private Visibility showNextButton;
        private Visibility showSaveButton;
        private ObservableCollection<EmdepSiteDetails> regionList;
        private List<object> selectedRegion;
        private ObservableCollection<EmdepSiteDetails> countryList;
        private List<object> selectedCountry;
        private ObservableCollection<EmdepSiteDetails> plantList;
        private List<object> selectedPlant;
        private List<EmdepSiteDetails> allSiteDetails;
        private ObservableCollection<Department> departmentList;
        private List<object> selectedDepartment;
        private List<Employee> employeeList;
        private ObservableCollection<Employee> employeeListFiltered;
        private List<Employee> selectedEmployeeList;
        private bool isBusy;
        private bool enableFilters;
        private bool regionChanged;
        private bool initComplete;
        #endregion

        #region Constructors

        public EmployeeBacklogHolidaysViewModel()
        {
            try
            {
                initComplete = false;
                GeosApplication.Instance.Logger.Log("Constructor EmployeeBacklogHolidaysViewModel ...", category: Category.Info, priority: Priority.Low);
                NextButtonCommand = new DelegateCommand<object>(NextButtonCommandAction);
                ResetButtonCommand = new DelegateCommand<object>(ResetButtonCommandAction);
                SaveButtonCommand = new DelegateCommand<object>(SaveButtonCommandAction);
                Init();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                initComplete = true;
                GeosApplication.Instance.Logger.Log("Constructor EmployeeBacklogHolidaysViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                isBusy = false;
                initComplete = true;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor EmployeeBacklogHolidaysViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Properties

        public bool ShowBacklogDays
        {
            get { return showBacklogDays; }
            set
            {
                showBacklogDays = value;
                OnPropertyChanged("ShowBacklogDays");
            }
        }

        public Visibility ShowNextButton
        {
            get { return showNextButton; }
            set
            {
                showNextButton = value;
                OnPropertyChanged("ShowNextButton");
            }
        }
        public Visibility ShowSaveButton
        {
            get { return showSaveButton; }
            set
            {
                showSaveButton = value;
                OnPropertyChanged("ShowSaveButton");
            }
        }

        public ObservableCollection<EmdepSiteDetails> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged("RegionList");
            }
        }

        public List<object> SelectedRegion
        {
            get
            {
                return selectedRegion;
            }

            set
            {
                if (initComplete)
                {
                    regionChanged = true;
                    selectedRegion = value;
                    OnPropertyChanged("SelectedRegion");
                    // Load Country List
                    CountryList = new ObservableCollection<EmdepSiteDetails>();
                    PlantList = new ObservableCollection<EmdepSiteDetails>();
                    SelectedCountry = null;
                    SelectedPlant = null;
                    if (selectedRegion != null)
                    {
                        foreach (int i in selectedRegion.OfType<EmdepSiteDetails>().Select(region => region.IdRegion).ToList())
                        {
                            foreach (EmdepSiteDetails site in AllSiteDetails.Where(x => x.IdRegion == i))
                            {
                                if (!CountryList.Any(x => x.IdCountry == site.IdCountry))
                                    CountryList.Add(site);
                            }
                        }
                    }
                    ReloadGridByRegion();
                    regionChanged = false;
                }
            }
        }

        public ObservableCollection<EmdepSiteDetails> CountryList
        {
            get
            {
                return countryList;
            }

            set
            {
                countryList = value;
                OnPropertyChanged("CountryList");
            }
        }

        public List<object> SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                if (initComplete)
                {

                    if (value == null)
                        selectedCountry = value;
                    if (!regionChanged)
                    {
                        OnPropertyChanged("SelectedCountry");
                        selectedCountry = value;
                        SelectedPlant = null;
                        PlantList = new ObservableCollection<EmdepSiteDetails>();
                        if (SelectedCountry != null)
                        {
                            foreach (int i in SelectedCountry.OfType<EmdepSiteDetails>().Select(region => region.IdCountry).ToList())
                            {
                                foreach (EmdepSiteDetails site in AllSiteDetails.Where(x => x.IdCountry == i))
                                {
                                    if (!PlantList.Any(x => x.IdSite == site.IdSite))
                                        PlantList.Add(site);
                                }
                            }
                        }
                        ReloadGridByCountry();
                    }
                }
            }
        }

        public ObservableCollection<EmdepSiteDetails> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }

        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                if (initComplete)
                {
                    selectedPlant = value;
                    OnPropertyChanged("SelectedPlant");
                    ReloadGridBySites();
                }
            }
        }

        public List<EmdepSiteDetails> AllSiteDetails
        {
            get
            {
                return allSiteDetails;
            }

            set
            {
                allSiteDetails = value;
                OnPropertyChanged("AllSiteDetails");
            }
        }

        public ObservableCollection<Department> DepartmentList
        {
            get
            {
                return departmentList;
            }

            set
            {
                departmentList = value;
                OnPropertyChanged("DepartmentList");
            }
        }
        public List<object> SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }

            set
            {
                if (initComplete)
                {
                    selectedDepartment = value;
                    OnPropertyChanged("SelectedDepartment");
                    ReloadGridByDepartment();
                }
            }
        }

        public List<Employee> EmployeeList
        {
            get
            {
                return employeeList;
            }

            set
            {
                employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }
        public ObservableCollection<Employee> EmployeeListFiltered
        {
            get
            {
                return employeeListFiltered;
            }

            set
            {
                employeeListFiltered = value;
                OnPropertyChanged("EmployeeListFiltered");
            }
        }
        public List<Employee> SelectedEmployeeList
        {
            get
            {
                return selectedEmployeeList;
            }

            set
            {
                selectedEmployeeList = value;
                OnPropertyChanged("SelectedEmployeeList");
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        public bool EnableFilters
        {
            get { return enableFilters; }
            set
            {
                enableFilters = value;
                OnPropertyChanged("EnableFilters");
            }
        }
        #endregion

        #region Commands
        public ICommand NextButtonCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public ICommand SaveButtonCommand { get; set; }


        #endregion

        #region Methods      

        private void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
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
                ShowBacklogDays = false;
                ShowSaveButton = Visibility.Collapsed;
                ShowNextButton = Visibility.Visible;
                EnableFilters = true;
                FillRegions();
                FillDepartment();
                FillEmployeesByDepartmentAndCompany();
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                isBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                if (SelectedPlant == null)
                    SelectedPlant = new List<object>();

                AllSiteDetails = new List<EmdepSiteDetails>(HrmService.GetEmdepsitesCountryRegion_V2590());// [nsatpute][24-12-2024][GEOS2-6774]
                RegionList = new ObservableCollection<EmdepSiteDetails>();
                foreach (EmdepSiteDetails site in AllSiteDetails)
                {
                    if (!RegionList.Any(x => x.IdRegion == site.IdRegion))
                        RegionList.Add(site);
                }

                SelectedRegion = new List<object>(SelectedRegion);
                GeosApplication.Instance.Logger.Log("Method FillRegions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillDepartment()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                DepartmentList = new ObservableCollection<Department>(HrmService.GetAllDepartments());
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillEmployeesByDepartmentAndCompany()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                EmployeeList = HrmService.GetEmployeesByDepartmentAndCompany_V2590();
                EmployeeListFiltered = new ObservableCollection<Employee>();
                //EmployeeList.ForEach(x => { EmployeeListFiltered.Add((Employee)x.Clone()); });
                EmployeeListFiltered.AddRange(EmployeeList.Select(x => (Employee)x.Clone()));
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SaveEmployeeBacklogHours()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                //EmployeeList = HrmService.GetEmployeesByDepartmentAndCompany();
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ReloadGridByRegion()
        {
            if (SelectedRegion != null)
            {
                EmployeeListFiltered = new ObservableCollection<Employee>();
                List<int> lstRegion = SelectedRegion.Cast<EmdepSiteDetails>().Select(x => x.IdRegion).ToList();
                List<int?> lstSites = new List<int?>();
                List<string> lstDepartments = new List<string>();
                if (AllSiteDetails != null)
                {
                    foreach (EmdepSiteDetails es in AllSiteDetails)
                    {
                        if (lstRegion.Contains(es.IdRegion))
                            lstSites.Add(es.IdSite);
                    }
                    if (EmployeeList != null)
                    {
                        foreach (Employee emp in EmployeeList)
                        {
                            if (lstSites.Contains(emp.IdCompanyLocation))
                                employeeListFiltered.Add((Employee)emp.Clone());
                        }
                        if (SelectedDepartment != null)
                        {
                            List<string> lstDepartment = SelectedDepartment.Cast<Department>().Select(x => x.DepartmentName).ToList();
                            foreach (Employee emp in EmployeeListFiltered.ToList())
                            {
                                if (!lstDepartment.Contains(emp.EmployeeDepartments))
                                    EmployeeListFiltered.Remove(emp);
                            }
                        }
                    }
                }

                EmployeeListFiltered = EmployeeListFiltered.OrderBy(x => x.IdCompanyLocation).OrderBy(y => y.EmployeeDepartments).ToObservableCollection();
            }
            else
            {
                EmployeeListFiltered = new ObservableCollection<Employee>();
                EmployeeListFiltered.AddRange(EmployeeList.Select(x => (Employee)x.Clone()));
            }
        }

        private void ReloadGridByCountry()
        {
            if (SelectedCountry != null)
            {
                EmployeeListFiltered = new ObservableCollection<Employee>();
                List<int> lstcountry = SelectedCountry.Cast<EmdepSiteDetails>().Select(x => x.IdCountry).ToList();
                List<int?> lstSites = new List<int?>();
                if (AllSiteDetails != null)
                {
                    foreach (EmdepSiteDetails es in AllSiteDetails)
                    {
                        if (lstcountry.Contains(es.IdCountry))
                            lstSites.Add(es.IdSite);
                    }
                    if (EmployeeList != null)
                    {
                        foreach (Employee emp in EmployeeList)
                        {
                            if (lstSites.Contains(emp.IdCompanyLocation))
                                employeeListFiltered.Add((Employee)emp.Clone());
                        }
                        if (SelectedDepartment != null)
                        {
                            List<string> lstDepartment = SelectedDepartment.Cast<Department>().Select(x => x.DepartmentName).ToList();
                            foreach (Employee emp in EmployeeListFiltered.ToList())
                            {
                                if (!lstDepartment.Contains(emp.EmployeeDepartments))
                                    EmployeeListFiltered.Remove(emp);
                            }
                        }
                    }
                }
                EmployeeListFiltered = EmployeeListFiltered.OrderBy(x => x.IdCompanyLocation).OrderBy(y => y.EmployeeDepartments).ToObservableCollection();
            }
            else
            {
                ReloadGridByRegion();
            }
        }

        private void ReloadGridBySites()
        {
            if (SelectedPlant != null)
            {
                EmployeeListFiltered = new ObservableCollection<Employee>();
                List<int> lstSites = SelectedPlant.Cast<EmdepSiteDetails>().Select(x => x.IdSite).ToList();
                if (EmployeeList != null)
                {
                    foreach (Employee emp in EmployeeList)
                    {
                        foreach (int i in lstSites)
                        {
                            if (emp.IdCompanyLocation == i)
                                employeeListFiltered.Add((Employee)emp.Clone());
                        }
                    }
                    if (SelectedDepartment != null)
                    {
                        List<string> lstDepartment = SelectedDepartment.Cast<Department>().Select(x => x.DepartmentName).ToList();
                        foreach (Employee emp in EmployeeListFiltered.ToList())
                        {
                            if (!lstDepartment.Contains(emp.EmployeeDepartments))
                                EmployeeListFiltered.Remove(emp);
                        }
                    }
                }
                EmployeeListFiltered = EmployeeListFiltered.OrderBy(x => x.IdCompanyLocation).OrderBy(y => y.EmployeeDepartments).ToObservableCollection();
            }
            else
            {
                ReloadGridByCountry();
            }
        }
        private void ReloadGridByDepartment()
        {
            ReloadGridByRegion(); ReloadGridByCountry(); ReloadGridBySites();
            if (SelectedDepartment != null)
            {
                List<string> lstDepartment = SelectedDepartment.Cast<Department>().Select(x => x.DepartmentName).ToList();
                if (EmployeeList != null)
                {
                    foreach (Employee emp in EmployeeListFiltered.ToList())
                    {
                        if (!lstDepartment.Contains(emp.EmployeeDepartments))
                            EmployeeListFiltered.Remove(emp);
                    }
                }
                EmployeeListFiltered = EmployeeListFiltered.OrderBy(x => x.IdCompanyLocation).OrderBy(y => y.EmployeeDepartments).ToObservableCollection();
            }
        }

        #endregion

        #region Event Handlers
		// [nsatpute][17-12-2024][GEOS2-5747]
        private void NextButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (EmployeeListFiltered.Where(x => x.IsSelected).Count() == 0)
                {
                    CustomMessageBox.Show(Application.Current.Resources["Automaticbacklog_Pleaseselectanyrecord"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
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

                SelectedEmployeeList = new List<Employee>();
                EmployeeListFiltered.ForEach(x => { SelectedEmployeeList.Add((Employee)x.Clone()); });
                EmployeeListFiltered = EmployeeListFiltered.Where(x => x.IsSelected).ToObservableCollection();
                EmployeeListFiltered = HrmService.GetEmployeeBacklogHours_V2600(EmployeeListFiltered.ToList()).ToObservableCollection(); // [nsatpute][16-01-2025][GEOS2-6862]
                ShowSaveButton = Visibility.Visible;
                ShowNextButton = Visibility.Collapsed;
                EnableFilters = false;
                ShowBacklogDays = true;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method NextButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ResetButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ResetButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                EmployeeListFiltered = new ObservableCollection<Employee>(SelectedEmployeeList);
                ShowSaveButton = Visibility.Collapsed; ;
                ShowNextButton = Visibility.Visible;
                ShowBacklogDays = false;
                EnableFilters = true;
                GeosApplication.Instance.Logger.Log("Method ResetButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ResetButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SaveButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (EmployeeListFiltered.Where(x => x.IsSelected).Count() == 0)
                {
                    CustomMessageBox.Show(Application.Current.Resources["Automaticbacklog_Pleaseselectanyrecord"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                HrmService.SaveEmployeeBacklogHours(EmployeeListFiltered.Where(x => x.IsSelected).ToList());
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Automaticbacklog_Backloghourshavebeensuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);                
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
                EmployeeList = EmployeeList.Where(x => x.IsSelected != true).ToList();
                foreach(Employee e in EmployeeList.ToList())
                {
                    if (EmployeeListFiltered.Where(x => x.IsSelected).ToList().Any(x => x.IdEmployee == e.IdEmployee))
                        EmployeeList.Remove(e);
                }

                SelectedCountry = null;
                SelectedDepartment = null;
                SelectedEmployeeList = null;
                SelectedRegion = null;
                SelectedPlant = null;
                ShowBacklogDays = false;
                ShowSaveButton = Visibility.Collapsed;
                ShowNextButton = Visibility.Visible;
                EnableFilters = true;
                EmployeeListFiltered = new ObservableCollection<Employee>();
                EmployeeListFiltered.AddRange(EmployeeList.Select(x => (Employee)x.Clone()));
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
