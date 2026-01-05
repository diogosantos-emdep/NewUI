using System;
using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using System.Windows.Input;
using System.Windows;
using DevExpress.Mvvm.UI;
using System.Windows.Media;
using Prism.Logging;
using Emdep.Geos.UI.Commands;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Hrm;
using System.Linq;
using Emdep.Geos.Data.Common;
using System.Collections.Generic;
using Emdep.Geos.Data.Common.PCM;
using DevExpress.Xpf.Accordion;
using System.Text;
using Emdep.Geos.Utility;
using System.Globalization;
using Emdep.Geos.Modules.PCM.Views;
using System.IO;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class AnnouncementViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

        #region Declaration
        #endregion

        #region Properties
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        private Boolean newChangeType;
        public Boolean NewChangeType
        {
            get { return newChangeType; }
            set
            {
                newChangeType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewChangeType"));
            }
        }

        private Boolean updateChangeType;
        public Boolean UpdateChangeType
        {
            get { return updateChangeType; }
            set
            {
                updateChangeType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateChangeType"));
            }
        }
        private ObservableCollection<Department> employeeDepartments;
        public ObservableCollection<Department> EmployeeDepartments
        {
            get { return employeeDepartments; }
            set
            {
                employeeDepartments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDepartments"));
            }
        }

        ObservableCollection<Site> plantList;
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }

        object plantSelectedItem;
        public virtual object PlantSelectedItem
        {
            get
            {
                return plantSelectedItem;
            }

            set
            {
                plantSelectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantSelectedItem"));
            }
        }

        object departmentSelectedItem;
        public virtual object DepartmentSelectedItem
        {
            get
            {
                return departmentSelectedItem;
            }

            set
            {
                departmentSelectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentSelectedItem"));
            }
        }

        ObservableCollection<Employee> employees;
        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Employees"));
            }
        }

        object selectedEmployee;
        public virtual object SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployee"));
            }
        }

        ObservableCollection<Company> companyList;
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }


       
        List<PCMArticlesEmailDetails> pcmArticleDetails;
        public List<PCMArticlesEmailDetails> PCMArticleDetails
        {
            get
            {
                return pcmArticleDetails;
            }

            set
            {
                pcmArticleDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMArticleDetails"));
            }
        }

        List<PCMCPTypesEmailDetails> pcmCPTypeStructuresDetails;
        public List<PCMCPTypesEmailDetails> PCMStructuresDetails
        {
            get
            {
                return pcmCPTypeStructuresDetails;
            }

            set
            {
                pcmCPTypeStructuresDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMStructuresDetails"));
            }
        }

        List<PCMCPTypesEmailDetails> pcmCPTypeModulesDetails;
        public List<PCMCPTypesEmailDetails> PCMModulesDetails
        {
            get
            {
                return pcmCPTypeModulesDetails;
            }

            set
            {
                pcmCPTypeModulesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMModulesDetails"));
            }
        }

        List<PCMDetectionEmailDetails> detectionEmailDetails;
        public List<PCMDetectionEmailDetails> PCMDetectionsDetails
        {
            get
            {
                return detectionEmailDetails;
            }

            set
            {
                detectionEmailDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMDetectionsDetails"));
            }
        }
        List<PCMDetectionEmailDetails> optionsEmailDetails;
        public List<PCMDetectionEmailDetails> PCMOptionsDetails
        {
            get
            {
                return optionsEmailDetails;
            }

            set
            {
                optionsEmailDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMOptionsDetails"));
            }
        }

        List<PCMDetectionEmailDetails> waysEmailDetails;
        public List<PCMDetectionEmailDetails> PCMWaysDetails
        {
            get
            {
                return waysEmailDetails;
            }

            set
            {
                waysEmailDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMWaysDetails"));
            }
        }
        List<PCMDetectionEmailDetails> sparePartEmailDetails;
        public List<PCMDetectionEmailDetails> PCMSparePartDetails
        {
            get
            {
                return sparePartEmailDetails;
            }

            set
            {
                sparePartEmailDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PCMSparePartDetails"));
            }
        }


        ProductTypes productTypesDetails;
        public ProductTypes ProductTypesDetails
        {
            get
            {
                return productTypesDetails;
            }

            set
            {
                productTypesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesDetails"));
            }
        }

        public string AnnouncementFolderName { get; set; }
        public string AnnouncementFileName { get; set; }
        public Dictionary<string,string> AnnouncementFileNameANDFolder { get; set; }
        public Dictionary<string,byte[]> AnnouncementFilebyte { get; set; }
        DetectionDetails clonedDetections;
        public DetectionDetails ClonedDetections
        {
            get
            {
                return clonedDetections;
            }

            set
            {
                clonedDetections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDetections"));
            }
        }
        #endregion

        #region ICommands

        public ICommand RefreshItemCommand { get; set; }
        public ICommand PrintItemCommand { get; set; }
        public ICommand ExportItemCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand NewChangeTypeValueChangingCommand { get; set; }
        public ICommand UpdateChangeTypeValueChangingCommand { get; set; }
        public ICommand SelectedPlantCommand { get; set; }

        public ICommand CommandOnDragRecordOverEmployees { get; set; }
        public ICommand CommandOnDragRecordOverFamilyGrid { get; set; }
        public ICommand CommandCompleteRecordDragDropEmployees { get; set; }
        public ICommand CommandAccordionControl_MouseDoubleClick { get; set; }
        public ICommand DeleteCommentCommand { get; set; }

        public ICommand SendButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-3891][27/02/2023]
        #endregion

        #region Constructor

        public AnnouncementViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AnnouncementViewModel....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                NewChangeTypeValueChangingCommand = new DelegateCommand<object>(NewChangeTypeCommandAction);
                UpdateChangeTypeValueChangingCommand = new DelegateCommand<object>(UpdateChangeTypeCommandAction);
                SelectedPlantCommand = new DelegateCommand<object>(SelectedPlantCommandAction);
                SendButtonCommand = new RelayCommand(new Action<object>(SendButtonCommandAction));//[Sudhir.Jangra][GEOS2-3891][27/02/2023]
                StartDate = DateTime.Now.AddMonths(-1);

                NewChangeType = true;
                UpdateChangeType = false;
                FillPlantByIdUser();
                //if (plantList!=null)
                //{
                //    PlantSelectedItem = plantList.FirstOrDefault();
                //    FillDepartmentListByPlantId(plantList.FirstOrDefault().IdSite.ToString());
                //}
                if (CompanyList != null)
                {
                    dynamic d = (dynamic)GeosApplication.Instance.SelectedPlantOwnerUsersList;
                    if (d != null)
                    {
                        Emdep.Geos.Data.Common.Company company = (Company)d[0];
                        PlantSelectedItem = (Company)CompanyList.Where(w => w.IdCompany == company.IdCompany).FirstOrDefault();
                        //FillDepartmentListByPlantId(CompanyList.FirstOrDefault().IdCompany.ToString());
                    }
                }
                CommandOnDragRecordOverEmployees = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverEmployees);
                CommandOnDragRecordOverFamilyGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverEmployeesGrid);
                CommandCompleteRecordDragDropEmployees = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropEmployees);
                CommandAccordionControl_MouseDoubleClick = new DelegateCommand<object>(AccordionControl_MouseDoubleClick);
                DeleteCommentCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                //FillDepartmentListByPlant();
                //AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor AnnouncementViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AnnouncementViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }




        #endregion //End Of Constructor

        #region Methods
        public void Init()
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
        //shubham[skadam] GEOS2-3787 Improvement related to Modules and Detections by customer (#PCM80) 22 JUN 2022
        private void NewChangeTypeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NewChangeTypeCommandAction()...", category: Category.Info, priority: Priority.Low);
                //UpdateChangeType = false;
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method NewChangeTypeCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void UpdateChangeTypeCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateChangeTypeCommandAction()...", category: Category.Info, priority: Priority.Low);
                //NewChangeType = false;
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateChangeTypeCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void FillDepartmentListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentListByPlant ...", category: Category.Info, priority: Priority.Low);
                //List<Company> plantOwners = GeosApplication.Instance.PlantOwnerUsersList.Cast<Company>().ToList();
                //var plantOwnersIds1 = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                string plantOwnersIds = string.Empty;
                plantOwnersIds = "18";
                EmployeeDepartments = new ObservableCollection<Department>((HrmService.GetAllEmployeesForOrganizationByIdCompany_V2330(plantOwnersIds, DateTime.Now.Year,
                    "11559", "1", 39)).Where(i => i.DepartmentInUse == 1).ToList());
                GeosApplication.Instance.Logger.Log("Method FillDepartmentListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        public void FillPlantByIdUser()
        {
            try
            {
                //GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser);
                //GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                //CompanyList = new ObservableCollection<Company>(CrmStartUp.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser));

                //CompanyList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2031(GeosApplication.Instance.ActiveUser.IdUser));
                //CompanyList = new ObservableCollection<Company>(CompanyList.Where(x => x.IsCompany == 1).OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));


                CompanyList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2031(GeosApplication.Instance.ActiveUser.IdUser));
                Company company = new Company();
                company.Alias = "ALL";
                company.Country = new Country();
                company.Country.IdCountry = 0;
                CompanyList.Add(company);
                CompanyList = new ObservableCollection<Company>(CompanyList.OrderBy(o => o.Country.IdCountry).ToList());

                //PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                GeosApplication.Instance.Logger.Log("Method FillPlantByIdUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantByIdUser() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantByIdUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        private void SelectedPlantCommandAction(object obj)
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GeosApplication.Instance.Logger.Log("Method SelectedPlantCommandAction()...", category: Category.Info, priority: Priority.Low);
                AccordionSelectedItemChangedEventArgs AccordionSelectedItem = (AccordionSelectedItemChangedEventArgs)obj;
                //Site Site=(Site)AccordionSelectedItem.NewItem;
                //FillDepartmentListByPlantId(Site.IdSite.ToString());
                try
                {
                    if (AccordionSelectedItem.NewItem != null)
                    {
                        Company company = (Company)AccordionSelectedItem.NewItem;
                        if (company.Country.IdCountry == 0)
                        {
                            string IdCompany = string.Join(",", CompanyList.Select(s => s.IdCompany).Where(w => w != 0));
                            FillDepartmentListByPlantId(IdCompany);
                        }
                        else
                        {
                            FillDepartmentListByPlantId(company.IdCompany.ToString());
                        }
                    }

                }
                catch (Exception ex)
                {
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedPlantCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void FillDepartmentListByPlantId(string plantOwnersId)
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GeosApplication.Instance.Logger.Log("Method FillDepartmentListByPlant ...", category: Category.Info, priority: Priority.Low);
                //List<Company> plantOwners = GeosApplication.Instance.PlantOwnerUsersList.Cast<Company>().ToList();
                //var plantOwnersIds1 = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                //EmployeeDepartments = new ObservableCollection<Department>((HrmService.GetAllEmployeesForOrganizationByIdCompany_V2330(plantOwnersId, DateTime.Now.Year,
                //    null, null,39)).Where(i => i.DepartmentInUse == 1).ToList());
                EmployeeDepartments = new ObservableCollection<Department>((PCMService.GetAllEmployeesForOrganizationByIdCompany_V2360(plantOwnersId)).Where(i => i.DepartmentInUse == 1).ToList());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDepartmentListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void OnDragRecordOverEmployees(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily()...", category: Category.Info, priority: Priority.Low);

                if (typeof(Employee).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CompleteRecordDragDropEmployees(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()...", category: Category.Info, priority: Priority.Low);


                e.Handled = true;
                // IsEnabled = true;

                //if (Families == null)
                {
                    // Families = new ObservableCollection<ConnectorFamilies>();
                }
                List<Employee> records = e.Records.OfType<Employee>().ToList();
                foreach (var item in records)
                {
                    //  FamilyMenulist.Where(a => a.IdFamily == item.IdFamily).ToList().ForEach(b => b.IsCurrentFamily = true);
                }
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverEmployeesGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamilyGrid()...", category: Category.Info, priority: Priority.Low);


                if ((e.IsFromOutside) && typeof(Employee).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                    // IsEnabled = true;
                }

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverFamilyGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSparePartsGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AccordionControl_MouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            //AccordionControl accordion = e as AccordionControl;
            //var treeItemEmployee= accordion.SelectedItem as Data.Common.Hrm.Employee;
            //if (treeItemEmployee == null)
            //{
            //    Employees.Add(treeItemEmployee);
            //}
            //else
            //{
            //    return;
            //}
        }

        private void AccordionControl_MouseDoubleClick(object obj)
        {

            System.Windows.Input.MouseButtonEventArgs MouseButtonEventArgs = (System.Windows.Input.MouseButtonEventArgs)obj;
            AccordionControl accordion = MouseButtonEventArgs.Source as AccordionControl;
            var treeItemEmployee = accordion.SelectedItem as Data.Common.Hrm.Employee;
            if (treeItemEmployee != null)
            {
                if (Employees == null)
                {
                    Employees = new ObservableCollection<Employee>();
                }
                if (!Employees.Any(a => a.IdEmployee == treeItemEmployee.IdEmployee))
                {
                    Employees.Add(treeItemEmployee);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Method to delete Location
        /// </summary>
        /// <param name="obj"></param>

        private void DeleteCommentCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction()...", category: Category.Info, priority: Priority.Low);
                Employee employee = (Employee)obj;
                if (employee != null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["PCMAnnouncementViewRecipientsDelete"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        Employees.Remove(employee);
                        GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in DeleteCommentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SendButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendButtonCommandAction...", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                if (Employees==null)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMEmailSendSelectRecipient").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (Employees.Count == 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMEmailSendSelectRecipient").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                AnnouncementFileNameANDFolder = new Dictionary<string, string>();
                AnnouncementFilebyte =  new Dictionary<string, byte[]> ();
                AnnouncementFolderName = DateTime.Now.ToString("yyyyMMddHHmmss");
                string tempFolderPath = Path.GetTempPath();
                string createFolderName = tempFolderPath + "PCMAnnouncement\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + AnnouncementFolderName + "\\";
                AnnouncementFolderName = createFolderName;
                if (!System.IO.Directory.Exists(AnnouncementFolderName))
                {
                    System.IO.Directory.CreateDirectory(AnnouncementFolderName);
                }
                string Type = string.Empty;
                if (NewChangeType == true)
                {
                    Type = "New";
                }
                else
                {
                    Type = "Updated";
                }
                #region OldCode
                //PCMArticleDetails = new List<PCMArticlesEmailDetails>();
                //PCMWaysDetails = new List<PCMDetectionEmailDetails>();
                //PCMDetectionsDetails = new List<PCMDetectionEmailDetails>();
                //PCMSparePartDetails = new List<PCMDetectionEmailDetails>();
                //PCMOptionsDetails = new List<PCMDetectionEmailDetails>();

                //PCMStructuresDetails = new List<PCMCPTypesEmailDetails>();
                //PCMModulesDetails = new List<PCMCPTypesEmailDetails>();
                #endregion
                string EmployeeContactEmail =  string.Join(";", Employees.Where(e => e.EmployeeContactEmail != null).Select(s => s.EmployeeContactEmail));
                string text = PCMService.ReadMailTemplate("PCMAnnouncementEmailTemplate.html");
                #region OldCode
                // text = text.Replace("[DynamicDataDate] ", (StartDate.ToString("dd MMMM yyyy") + " - " + DateTime.Now.ToString("dd MMMM yyyy")));
                //if (!string.IsNullOrEmpty(EmployeeContactEmail))
                //{
                //    PCMAnnouncementEmailDetails EmailSendingErrorMsgList = PCMService.GetEmailForDows_v2360(StartDate, DateTime.Now, NewChangeType, UpdateChangeType, EmployeeContactEmail);
                //}
                #endregion
                System.Globalization.Calendar cal = new System.Globalization.CultureInfo("en-US").Calendar;
                int weekStart = cal.GetWeekOfYear(StartDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int weekEnd = cal.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                //PCMAnnouncementEmailDetails EmailSendingErrorMsgList = PCMService.GetEmailForDows_V2360(StartDate, DateTime.Now, NewChangeType, UpdateChangeType, EmployeeContactEmail);
                //PCMAnnouncementEmailDetails EmailSendingErrorMsgList = PCMService.GetEmailForDows_V2360New(StartDate, DateTime.Now, NewChangeType, UpdateChangeType, EmployeeContactEmail);
                //Shubham[skadam]  GEOS2-4394 Modify some names in email about changes in Commercial Catalogue 28 07 2023 
                PCMAnnouncementEmailDetails EmailSendingErrorMsgList = PCMService.GetEmailForDows_V2420(StartDate, DateTime.Now, NewChangeType, UpdateChangeType, EmployeeContactEmail);
                #region OldCode
                //if (EmailSendingErrorMsgList.PCMArticleDetails != null)
                //{
                //    PCMArticleDetails.AddRange(EmailSendingErrorMsgList.PCMArticleDetails);
                //}
                //if (EmailSendingErrorMsgList.DetectionEmailDetails != null)
                //{
                //    foreach (PCMDetectionEmailDetails itemDetection in EmailSendingErrorMsgList.DetectionEmailDetails)
                //    {
                //        try
                //        {
                //            if (itemDetection.DetectionType == 1)
                //            {
                //                PCMWaysDetails.Add(itemDetection);
                //            }
                //            else if (itemDetection.DetectionType == 2)
                //            {
                //                PCMDetectionsDetails.Add(itemDetection);
                //            }
                //            else if (itemDetection.DetectionType == 3)
                //            {
                //                PCMOptionsDetails.Add(itemDetection);
                //            }
                //            else if (itemDetection.DetectionType == 4)
                //            {
                //                PCMSparePartDetails.Add(itemDetection);
                //            }
                //        }
                //        catch (Exception)
                //        {

                //        }
                //    }
                //}
                //if (EmailSendingErrorMsgList.PCMCPTypesDetails != null)
                //{
                //    foreach (PCMCPTypesEmailDetails itemDetection in EmailSendingErrorMsgList.PCMCPTypesDetails)
                //    {
                //        try
                //        {
                //            if (itemDetection.CPType == 24)
                //            {
                //                PCMStructuresDetails.Add(itemDetection);
                //            }
                //            else
                //            {
                //                PCMModulesDetails.Add(itemDetection);
                //            }
                //        }
                //        catch (Exception)
                //        {

                //        }
                //    }
                //}
                #endregion
                StringBuilder DynamicDatafinal = new StringBuilder();
                if (weekStart> weekEnd)
                {
                    int temp = weekEnd;
                    weekEnd = weekStart;
                    weekStart = 1;
                }
                for (int i = weekStart; i <= weekEnd; i++)
                {
                    bool flage = false;
                    List<PCMArticlesEmailDetails> temparticlesDetails = new List<PCMArticlesEmailDetails>();
                    List<PCMDetectionEmailDetails> tempDetectionDetails = new List<PCMDetectionEmailDetails>();
                    List<PCMCPTypesEmailDetails> tempCPTypesDetails = new List<PCMCPTypesEmailDetails>();
                    flage = false;
                    if (EmailSendingErrorMsgList.PCMArticleDetails != null)
                    {
                      List<PCMArticlesEmailDetails> articlesDetails= EmailSendingErrorMsgList.PCMArticleDetails.Where(w=>w.Week==i).ToList();
                        if (articlesDetails!=null && articlesDetails.Count!=0)
                        {
                            flage = true;
                            temparticlesDetails.AddRange(articlesDetails);
                        }
                    }
                    if (EmailSendingErrorMsgList.DetectionEmailDetails != null)
                    {
                        List<PCMDetectionEmailDetails> detectionDetails = EmailSendingErrorMsgList.DetectionEmailDetails.Where(w => w.Week == i).ToList();
                        if (detectionDetails != null && detectionDetails.Count!=0)
                        {
                            flage = true;
                            tempDetectionDetails.AddRange(detectionDetails);
                        }
                    }
                    if (EmailSendingErrorMsgList.PCMCPTypesDetails != null)
                    {
                        List<PCMCPTypesEmailDetails> CPTypesDetails = EmailSendingErrorMsgList.PCMCPTypesDetails.Where(w => w.Week == i).ToList();
                        if (CPTypesDetails != null && CPTypesDetails.Count!=0)
                        {
                            flage = true;
                            tempCPTypesDetails.AddRange(CPTypesDetails);
                        }
                    }
                    if (flage)
                    {
                        PCMArticleDetails = new List<PCMArticlesEmailDetails>();
                        PCMWaysDetails = new List<PCMDetectionEmailDetails>();
                        PCMDetectionsDetails = new List<PCMDetectionEmailDetails>();
                        PCMSparePartDetails = new List<PCMDetectionEmailDetails>();
                        PCMOptionsDetails = new List<PCMDetectionEmailDetails>();
                        PCMStructuresDetails = new List<PCMCPTypesEmailDetails>();
                        PCMModulesDetails = new List<PCMCPTypesEmailDetails>();
                        DateTime dateTimeWeekNumber = StartDate;
                        if (temparticlesDetails != null && temparticlesDetails.Count !=0)
                        {
                            dateTimeWeekNumber = temparticlesDetails.Where(s=>s.ReportDate!=null).Select(s=>s.ReportDate).FirstOrDefault();
                            PCMArticleDetails.AddRange(temparticlesDetails);
                        }
                        if (tempDetectionDetails != null && tempDetectionDetails.Count!=0)
                        {
                            dateTimeWeekNumber = tempDetectionDetails.Where(s => s.ReportDate != null).Select(s => s.ReportDate).FirstOrDefault();
                            if (ClonedDetections==null)
                            {
                                ClonedDetections = new DetectionDetails();
                            }
                            foreach (PCMDetectionEmailDetails itemDetection in tempDetectionDetails)
                            {
                                try
                                {
                                    if (itemDetection.DetectionType == 1)//Way
                                    {
                                        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                                        ClonedDetections = (PCMService.GetDetectionByIdDetection_V2470(Convert.ToUInt32(itemDetection.Id), GeosApplication.Instance.ActiveUser.IdUser));
                                        SavePDFFileForDetection();
                                        PCMWaysDetails.Add(itemDetection);
                                    }
                                    else if (itemDetection.DetectionType == 2)// Detection
                                    {
                                        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                                        ClonedDetections = (PCMService.GetDetectionByIdDetection_V2470(Convert.ToUInt32(itemDetection.Id), GeosApplication.Instance.ActiveUser.IdUser));
                                        SavePDFFileForDetection();
                                        PCMDetectionsDetails.Add(itemDetection);
                                    }
                                    else if (itemDetection.DetectionType == 3)//Option
                                    {
                                        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                                        ClonedDetections = (PCMService.GetDetectionByIdDetection_V2470(Convert.ToUInt32(itemDetection.Id), GeosApplication.Instance.ActiveUser.IdUser));
                                        SavePDFFileForDetection();
                                        PCMOptionsDetails.Add(itemDetection);
                                    }
                                    else if (itemDetection.DetectionType == 4)//Spare Part
                                    {
                                        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                                        ClonedDetections = (PCMService.GetDetectionByIdDetection_V2360(Convert.ToUInt32(itemDetection.Id), GeosApplication.Instance.ActiveUser.IdUser));
                                        SavePDFFileForDetection();
                                        PCMSparePartDetails.Add(itemDetection);
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        if (tempCPTypesDetails != null && tempCPTypesDetails.Count != 0)
                        {
                            dateTimeWeekNumber = tempCPTypesDetails.Where(s => s.ReportDate != null).Select(s => s.ReportDate).FirstOrDefault();
                            foreach (PCMCPTypesEmailDetails itemDetection in tempCPTypesDetails)
                            {
                                try
                                {
                                    if (itemDetection.CPType == 24)
                                    {
                                        // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 10 01 2023
                                        //ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2350(itemDetection.IdCPType, itemDetection.IdTemplate);
                                        //PCMStructuresDetails.Add(itemDetection);
                                    }
                                    else
                                    {
                                        // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 10 01 2023
                                        ProductTypesDetails = PCMService.GetProductTypeByIdCpType_V2590(itemDetection.IdCPType, itemDetection.IdTemplate,GeosApplication.Instance.ActiveUser.IdUser);
                                        SavePDFFileForModule();
                                        PCMModulesDetails.Add(itemDetection);
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }

                        System.Globalization.CultureInfo cultureinfo =  new System.Globalization.CultureInfo("en-GB");
                        // 46
                        int thisWeekNumber = GetIso8601WeekOfYear(dateTimeWeekNumber);
                        DateTime firstDayOfWeek = FirstDateOfWeek(dateTimeWeekNumber.Year, thisWeekNumber, cultureinfo); 
                        DateTime firstDayOfLastYearWeek = FirstDateOfWeek(dateTimeWeekNumber.Year, thisWeekNumber, cultureinfo);
                        DateTime lastDayOfWeek =LastDayOfWeek(dateTimeWeekNumber);
                        StringBuilder DynamicData = new StringBuilder();
                        DynamicData.Append("<div id='table3' style='background-color:#004694;'>	<p style='text-align:center; color:white;'>" +
                        (firstDayOfWeek.ToString("dd MMM yyyy") + " - " + lastDayOfWeek.ToString("dd MMM yyyy")) 
                        + " </p> </div>");

                        //Modules
                        if (PCMModulesDetails != null && PCMModulesDetails.Count > 0)
                        {
                            DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Modules </header></h2 > <ul type ='circle'> ");
                            foreach (PCMCPTypesEmailDetails item in PCMModulesDetails)
                            {
                                if (item.Type.ToLower().Equals("new".ToLower()))
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:green'>" + item.Type + ":</font> " + item.CPName + "</li>");
                                }
                                else
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:blue'>" + item.Type + ":</font> " + item.CPName + "</li>");
                                }
                            }
                            DynamicData.Append("</ul></div >");
                        }


                        //Detections
                        if (PCMDetectionsDetails != null && PCMDetectionsDetails.Count > 0)
                        {
                            DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Detection </header></h2 > <ul type ='circle'> ");
                            foreach (PCMDetectionEmailDetails item in PCMDetectionsDetails)
                            {
                                if (item.Type.ToLower().Equals("new".ToLower()))
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:green'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                                else
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:blue'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                            }
                            DynamicData.Append("</ul></div>");
                        }

                        //Options
                        if (PCMOptionsDetails != null && PCMOptionsDetails.Count > 0)
                        {
                            DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Options </header></h2 > <ul type ='circle'> ");
                            foreach (PCMDetectionEmailDetails item in PCMOptionsDetails)
                            {
                                if (item.Type.ToLower().Equals("new".ToLower()))
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:green'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                                else
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:blue'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                            }
                            DynamicData.Append("</ul></div>");
                        }

                        //Ways
                        if (PCMWaysDetails != null && PCMWaysDetails.Count > 0)
                        {
                            DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Ways </header></h2 > <ul type ='circle'> ");
                            foreach (PCMDetectionEmailDetails item in PCMWaysDetails)
                            {
                                if (item.Type.ToLower().Equals("new".ToLower()))
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:green'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                                else
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:blue'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                            }
                            DynamicData.Append("</ul></div>");
                        }

                        //SparePart
                        if (PCMSparePartDetails != null && PCMSparePartDetails.Count > 0)
                        {
                            DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> SparePart </header></h2 > <ul type ='circle'> ");
                            foreach (PCMDetectionEmailDetails item in PCMSparePartDetails)
                            {
                                if (item.Type.ToLower().Equals("new".ToLower()))
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:green'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                                else
                                {
                                    DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <font style='color:blue'>" + item.Type + ":</font> " + item.DetectionName + "</li>");
                                }
                            }
                            DynamicData.Append("</ul></div>");
                        }

                        ////Article
                        //if (PCMArticleDetails != null && PCMArticleDetails.Count > 0)
                        //{
                        //    DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Article </header></h2 > <ul type ='circle'> ");
                        //    foreach (PCMArticlesEmailDetails item in PCMArticleDetails)
                        //    {
                        //        if (item.Type.ToLower().Equals("new".ToLower()))
                        //        {
                        //            DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <p style='color:green'>" + item.Type + "</p>: " + item.ArticleReference + "</li>");
                        //        }
                        //        else
                        //        {
                        //            DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <p style='color:blue'>" + item.Type + "</p>: " + item.ArticleReference + "</li>");
                        //        }
                        //    }
                        //    DynamicData.Append("</ul></div>");
                        //}

                        try
                        {
                            ////Structures
                            //if (PCMStructuresDetails != null && PCMStructuresDetails.Count > 0)
                            //{
                            //    DynamicData.Append("<div 'id =table4'> <h2><header style ='font-weight:bold;'> Structures </header></h2 > <ul type ='circle'> ");
                            //    foreach (PCMCPTypesEmailDetails item in PCMStructuresDetails)
                            //    {
                            //        if (item.Type.ToLower().Equals("new".ToLower()))
                            //        {
                            //            DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <p style='color:green'>" + item.Type + "</p>: " + item.CPName + "</li>");
                            //        }
                            //        else
                            //        {
                            //            DynamicData.Append("<li> [" + item.ReportDate.ToShortDateString() + "] <p style='color:blue'>" + item.Type + "</p>: " + item.CPName + "</li>");
                            //        }
                            //    }
                            //    DynamicData.Append("</ul></div>");
                            //}
                            DynamicDatafinal.Append(DynamicData);

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
                text = text.Replace("[PCMDynamicData]", DynamicDatafinal.ToString());

                try
                {
                    foreach (var item in AnnouncementFileNameANDFolder)
                    {
                      byte[] temp=  File.ReadAllBytes(item.Value+ item.Key);
                        AnnouncementFilebyte.Add(item.Key,temp);
                    }
                }
                catch (Exception ex)
                {
                }
                //bool isSend = PCMService.IsPCMAnnouncementEmailSend_V2360(EmployeeContactEmail,text, AnnouncementFilebyte);
                //Shubham[skadam]  GEOS2-4394 Modify some names in email about changes in Commercial Catalogue 28 07 2023 
                bool isSend = PCMService.IsPCMAnnouncementEmailSend_V2420(EmployeeContactEmail,text, AnnouncementFilebyte);
                if (isSend)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMEmailSendSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                #region DeleteAnnouncementFolder
                try
                {
                    string[] dirs = Directory.GetDirectories(tempFolderPath + "PCMAnnouncement\\");
                    foreach (var item in dirs)
                    {
                        //if (item != createFolderName)
                        //{
                        //    string searchpattern = "QAdocuments" + '*';
                        //    //string[] files = Directory.GetFiles(item, "*.*", SearchOption.AllDirectories);
                        //    string[] files = Directory.GetFiles(item, searchpattern, SearchOption.AllDirectories);
                        //    foreach (var fileitem in files)
                        //    {
                        //        //File.Delete(fileitem);
                        //    }
                        //}
                        if (item != createFolderName)
                        {
                            Directory.Delete(item, true);
                        }
                    }

                }
                catch (Exception ex)
                {
                }
                #endregion
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method SendButtonCommandAction().....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        public  int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public  DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            try
            {
                DateTime jan1 = new DateTime(year, 1, 1);
                int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
                DateTime firstWeekDay = jan1.AddDays(daysOffset);
                int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
                if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
                {
                    weekOfYear -= 1;
                }
                return firstWeekDay.AddDays(weekOfYear * 7);
            }
            catch (Exception ex)
            {
                DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);
                int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
                DateTime firstWeekDay = jan1.AddDays(daysOffset);
                return firstWeekDay.AddDays(weekOfYear * 7);
            }
           
        }

        public  DateTime FirstDayOfWeek(DateTime date)
        {
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset);
            return fdowDate;
        }

        public  DateTime LastDayOfWeek(DateTime date)
        {
            DateTime ldowDate = FirstDayOfWeek(date).AddDays(6);
            return ldowDate;
        }

        public void SavePDFFileForModule()
        {

            try
            {
                PCMPrintModuleView pCMPrintModuleView = new PCMPrintModuleView();
                PCMPrintModuleViewModel pCMPrintModuleViewModel = new PCMPrintModuleViewModel();
                EventHandler handle = delegate { pCMPrintModuleView.Close(); };
                pCMPrintModuleViewModel.RequestClose += handle;
                if (pCMPrintModuleViewModel.ProductTypesDetails == null)
                {
                    pCMPrintModuleViewModel.ProductTypesDetails = new ProductTypes();
                }
                pCMPrintModuleViewModel.ProductTypesDetails = ProductTypesDetails;
                pCMPrintModuleView.DataContext = pCMPrintModuleViewModel;
                pCMPrintModuleViewModel.AnnouncementFolderName = AnnouncementFolderName;
                //AnnouncementFolderNameAnnouncementFolderName
                pCMPrintModuleViewModel.AcceptButtonCommandWindow();
                AnnouncementFileName = pCMPrintModuleViewModel.AnnouncementFileName;
                if (AnnouncementFileNameANDFolder==null)
                {
                    AnnouncementFileNameANDFolder = new Dictionary<string, string>();
                }
                AnnouncementFileNameANDFolder.Add(AnnouncementFileName, AnnouncementFolderName);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method SavePDFFile().....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SavePDFFileForDetection()
        {

            try
            {
                PCMPrintModuleDetectionView pCMPrintModuleDetectionView = new PCMPrintModuleDetectionView();
                PCMPrintModuleDetectionViewModel pCMPrintModuleDetectionViewModel = new PCMPrintModuleDetectionViewModel();
                EventHandler handle = delegate { pCMPrintModuleDetectionView.Close(); };
                pCMPrintModuleDetectionViewModel.RequestClose += handle;
                if (pCMPrintModuleDetectionViewModel.DetectionDetails == null)
                {
                    pCMPrintModuleDetectionViewModel.DetectionDetails = new DetectionDetails();
                }
                pCMPrintModuleDetectionViewModel.DetectionDetails = ClonedDetections;
                pCMPrintModuleDetectionView.DataContext = pCMPrintModuleDetectionViewModel;

                pCMPrintModuleDetectionViewModel.AnnouncementFolderName = AnnouncementFolderName;
                pCMPrintModuleDetectionViewModel.AcceptButtonCommandWindow();
                AnnouncementFileName = pCMPrintModuleDetectionViewModel.AnnouncementFileName;
                if (AnnouncementFileNameANDFolder == null)
                {
                    AnnouncementFileNameANDFolder = new Dictionary<string, string>();
                }
                AnnouncementFileNameANDFolder.Add(AnnouncementFileName, AnnouncementFolderName);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method SavePDFFile().....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Public Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        #endregion // End Of Events 
    }
}