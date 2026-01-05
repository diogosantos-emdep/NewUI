using DevExpress.Diagram.Core;
using DevExpress.Diagram.Core.Localization;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Reports;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Modules.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing.Imaging;
using DevExpress.Xpf.Printing;
using System.Windows.Media.Imaging;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Charts;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGauges.Core.Customization;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraGauges.Core.Base;
using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.XtraGauges.Core.Primitive;
using System.Globalization;
using System.Threading;
using System.Runtime.InteropServices;
using DevExpress.Spreadsheet;
using Microsoft.Win32;
using System.Net;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeOrganizationViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //[M051-08][Year selection is not saved after change section][adadibathina]
        // [000][skale][23/01/2020][GEOS2-1919]- GHRM - Org chart in excel
        #endregion

        #region Services  

        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public INavigationService _Service;
        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IGeosRepositoryService geosService = new GeosRepositoryServiceController("localhost:6699");
        //IHrmService HrmService = new HrmServiceController("localhost:6699");

        #endregion // End Services

        #region Declaration

        private object selectedItem;
        private ObservableCollection<Department> employeeDepartments;
        private bool isBusy;

        private Employee employeeUpdatedDetail;
        private Department Dept;
        private List<Department> childDepartmentList;
        private List<JobDescription> jobDescriptions;
        private List<Employee> employees;
        private Department departments;
        private Department childDepartments;
        private string employeeCode;
        private string companyName;
        private Company selectedComapany;
        private List<EmployeeHeirarchy> employeeHierarchys;
        private ObservableCollection<EmployeeHeirarchy> employeeHierarchysforIsolatedDept;
        private ObservableCollection<EmployeeHeirarchy> finalEmployeeHierarchys;
        private Visibility isEmployeeDetailViewVisible;
        private Visibility isEmployeeHierarchicalViewVisible;
        private Visibility isolatedDepartmentHierarchyIsVisible;
        private List<Department> isolatedDepartmentList;
        private List<Department> normalDepartmentList;
        private List<JobDescription> jobDescriptionList;
        private ObservableCollection<Company> totalEmployeeList;
        private ObservableCollection<JobDescription> allJobDescriptionList;
        private ObservableCollection<LookupValue> departmentAreaAverageList;
        private Department SelectedDepartment;
        private int ParentDeptKey = 0, DeptKey = 0, JobKey = 0, EmpKey = 0, EmployeeGroup = 0, JDGroup = 0;
        private int normalColumnsSpan;
        private int isolatedColumnsSpan;
        private int isolatedColumn;
        private Visibility isButtonVisible;
        private bool isEmployeeImageShow;
        private int PrintExportDiagram;
        private List<Employee> organizationGridList;
        private static double TotalPercentage = 0;
        private static double TotalProductionPercentage = 0;
        private Visibility isAccordionControlVisible;
        //[000] added
        private string excelFilePath = string.Empty;
        bool addShiftEnabled;

        #endregion

        #region Public ICommands

        public ICommand DocumentViewCommand { get; set; }
        public ICommand ShowEmployeeDetailViewCommand { get; private set; }
        public ICommand ShowEmployeeHierarchicalViewCommand { get; private set; }
        public ICommand PlantOwnerEditValueChangedCommand { get; private set; }
        //public ICommand CustomLayoutItemsCommand { get; set; }
        public ICommand CommandDepartmentSelection { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand SingleEmployeeDocumentViewCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand PieChartDrawSeriesPointCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand SelectedDepartmentChangedCommand { get; set; }
        public ICommand ExportToExcelButtonCommand { get; set; }
        #endregion

        #region public Events

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

        public Bitmap ReportHeaderImage { get; set; }

        public ObservableCollection<Company> TotalEmployeeList
        {
            get { return totalEmployeeList; }
            set
            {
                totalEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalEmployeeList"));
            }
        }

        public Visibility IsolatedDepartmentHierarchyIsVisible
        {
            get { return isolatedDepartmentHierarchyIsVisible; }
            set
            {
                isolatedDepartmentHierarchyIsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsolatedDepartmentHierarchyIsVisible"));
            }
        }

        public List<Department> IsolatedDepartmentList
        {
            get { return isolatedDepartmentList; }
            set
            {
                isolatedDepartmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsolatedDepartmentList"));
            }
        }

        public Employee EmployeeUpdatedDetail
        {
            get { return employeeUpdatedDetail; }
            set
            {
                employeeUpdatedDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeUpdatedDetail"));
            }
        }

        public ObservableCollection<EmployeeHeirarchy> EmployeeHierarchysforIsolatedDept
        {
            get { return employeeHierarchysforIsolatedDept; }
            set
            {
                employeeHierarchysforIsolatedDept = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeHierarchysforIsolatedDept"));
            }
        }

        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCode"));
            }
        }

        public List<EmployeeHeirarchy> EmployeeHierarchys
        {
            get { return employeeHierarchys; }
            set
            {
                employeeHierarchys = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeHierarchys"));
            }
        }

        public ObservableCollection<EmployeeHeirarchy> FinalEmployeeHierarchys
        {
            get { return finalEmployeeHierarchys; }
            set
            {
                finalEmployeeHierarchys = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalEmployeeHierarchys"));
            }
        }

        public Department Departments
        {
            get { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Departments"));
            }
        }
        public Department ChildDepartments
        {
            get { return childDepartments; }
            set
            {
                childDepartments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChildDepartments"));
            }
        }
        public List<Department> ChildDepartmentList
        {
            get { return childDepartmentList; }
            set
            {
                childDepartmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChildDepartmentList"));
            }
        }

        public List<JobDescription> JobDescriptions
        {
            get { return jobDescriptions; }
            set
            {
                jobDescriptions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptions"));
            }
        }

        public List<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Employees"));
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ObservableCollection<Department> EmployeeDepartments
        {
            get { return employeeDepartments; }
            set
            {
                employeeDepartments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDepartments"));
            }
        }

        public Visibility IsEmployeeDetailViewVisible
        {
            get { return isEmployeeDetailViewVisible; }
            set
            {
                isEmployeeDetailViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeDetailViewVisible"));
            }
        }

        public Visibility IsEmployeeHierarchicalViewVisible
        {
            get { return isEmployeeHierarchicalViewVisible; }
            set
            {
                isEmployeeHierarchicalViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeHierarchicalViewVisible"));
            }
        }

        public virtual object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (IsEmployeeDetailViewVisible == Visibility.Visible)
                {
                    SelectItemForCompany();
                }
                else
                {
                    DisplayEmployeeHeirarchicalView();
                }

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public List<Department> NormalDepartmentList
        {
            get { return normalDepartmentList; }
            set
            {
                normalDepartmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NormalDepartmentList"));
            }
        }

        public int NormalColumnsSpan
        {
            get { return normalColumnsSpan; }
            set
            {
                normalColumnsSpan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NormalColumnsSpan"));
            }
        }

        public int IsolatedColumnsSpan
        {
            get { return isolatedColumnsSpan; }
            set
            {
                isolatedColumnsSpan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NormalColumnsSpan"));
            }
        }

        public int IsolatedColumn
        {
            get { return isolatedColumn; }
            set
            {
                isolatedColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsolatedColumn"));
            }
        }

        public Visibility IsButtonVisible
        {
            get { return isButtonVisible; }
            set
            {
                isButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonVisible"));
            }
        }

        public List<JobDescription> JobDescriptionList
        {
            get { return jobDescriptionList; }
            set
            {
                jobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionList"));
            }
        }

        public bool IsEmployeeImageShow
        {
            get { return isEmployeeImageShow; }
            set
            {
                isEmployeeImageShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeImageShow"));
            }
        }

        public ObservableCollection<JobDescription> AllJobDescriptionList
        {
            get { return allJobDescriptionList; }
            set
            {
                allJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllJobDescriptionList"));
            }
        }

        public ObservableCollection<LookupValue> DepartmentAreaAverageList
        {
            get { return departmentAreaAverageList; }
            set
            {
                departmentAreaAverageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentAreaAverageList"));
            }
        }

        public List<Employee> OrganizationGridList
        {
            get { return organizationGridList; }
            set
            {
                organizationGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrganizationGridList"));
            }
        }

        public Visibility IsAccordionControlVisible
        {
            get { return isAccordionControlVisible; }
            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }
        //[000] added  
        public string ExcelFilePath
        {
            get { return excelFilePath; }
            set { excelFilePath = value; }
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

        #endregion

        #region Constructor
        /// <summary>
        ///[000][skale][23/01/2020][GEOS2-1919]- GHRM - Org chart in excel
        /// </summary>
        public EmployeeOrganizationViewModel()
        {
            try
            {
                SetUserPermission();

                GeosApplication.Instance.Logger.Log("Constructor EmployeeOrganizationViewModel()...", category: Category.Info, priority: Priority.Low);
                DocumentViewCommand = new RelayCommand(new Action<object>(OpenJobDescriptionDocument));
                SingleEmployeeDocumentViewCommand = new RelayCommand(new Action<object>(OpenEducationDocumentForSingleEmployee));
                ShowEmployeeDetailViewCommand = new RelayCommand(new Action<object>(ShowEmployeeDetailView));
                ShowEmployeeHierarchicalViewCommand = new RelayCommand(new Action<object>(ShowEmployeeHierarchicalView));
                PlantOwnerEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                CommandDepartmentSelection = new DelegateCommand(SelectItemForCompany);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshOrganizationView));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintHierarchicalEmployeeDiagram));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportHierarchicalEmployeeDiagram));
                CommandGridDoubleClick = new DelegateCommand<object>(OpenEmployeeProfileDetailView);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                PieChartDrawSeriesPointCommand = new DelegateCommand<object>(PieChartDrawSeriesPointCommandAction);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                SelectedDepartmentChangedCommand = new RelayCommand(new Action<object>(SelectedDepartmentChangedCommandAction));
                GeosApplication.Instance.FillFinancialYear();
                //[000] added
                ExportToExcelButtonCommand = new RelayCommand(new Action<object>(OrganizationChartExportToExcel));

                // SelectedPeriod = GeosApplication.Instance.ServerDateTime.Date.Year;
                IsEmployeeDetailViewVisible = Visibility.Visible;
                IsEmployeeHierarchicalViewVisible = Visibility.Hidden;
                IsEmployeeImageShow = false;

                ReportHeaderImage = new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo_mini);
                GeosApplication.Instance.Logger.Log("Constructor EmployeeOrganizationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeOrganizationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = System.Windows.ResizeMode.NoResize,
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

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    FillDepartmentListByPlant();
                    IsEmployeeDetailViewVisible = Visibility.Visible;
                    IsEmployeeHierarchicalViewVisible = Visibility.Hidden;
                    IsolatedDepartmentHierarchyIsVisible = Visibility.Hidden;
                    IsButtonVisible = Visibility.Collapsed;
                    foreach (var item in DepartmentAreaAverageList)
                    {
                        TotalPercentage = TotalPercentage + (double)item.Average;
                        if (item.IdLookupValue == 127)
                        {
                            TotalProductionPercentage = TotalProductionPercentage + (double)item.Average;
                        }
                    }

                    SelectedItem = EmployeeDepartments.First();
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Display Employee Heirarchical View
        /// </summary>
        private void DisplayEmployeeHeirarchicalView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeHeirarchicalView ...", category: Category.Info, priority: Priority.Low);

                ParentDeptKey = 0; DeptKey = 0; JobKey = 0; EmpKey = 0; EmployeeGroup = 0; JDGroup = 0;

                IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = System.Windows.ResizeMode.NoResize,
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

                if (SelectedItem is Department)
                {
                    SelectedDepartment = new Department();
                    SelectedDepartment = (Department)SelectedItem;

                    //This If section display the all department hierarchy.
                    if (SelectedDepartment.DepartmentName == "ALL")
                    {
                        EmployeeHierarchys = new List<EmployeeHeirarchy>();
                        NormalDepartmentList = new List<Department>(EmployeeDepartments.Where(x => x.IdDepartment != 0));
                        if (NormalDepartmentList.Count > 0)
                        {
                            for (int i = 0; i < NormalDepartmentList.Count; i++)
                            {
                                SelectedDepartment = NormalDepartmentList[i];
                                //  List<JobDescription> JobDescriptionWithInUseYesAndIsRemoteNo= AllJobDescriptionList.ToList((jd=>jd.JobDescriptionIsRemote)
                                JobDescriptionList = AllJobDescriptionList.Where(x => ((x.IdDepartment == SelectedDepartment.IdDepartment && x.EmployeeJobDescriptions != null) ||
                                                                                       (x.IdDepartment == SelectedDepartment.IdDepartment && x.ChildJobDescriptions.Count > 0 ? x.IsEmployeeExistInChildJD : false) ||
                                                                                       (x.IdDepartment == SelectedDepartment.IdDepartment && x.JobDescriptionIsMandatory == 1)) && (x.JobDescriptionInUse == 1 && x.JobDescriptionIsRemote == 0)).ToList();

                                if (JobDescriptionList.Count > 0)
                                {
                                    for (int j = 0; j < JobDescriptionList.Count; j++)
                                    {
                                        // Code for Avoide Duplicate hierarchy 

                                        if (JobDescriptionList[j].IdParent > 0)
                                        {
                                            JobDescription parentJobDesc = JobDescriptionList.FirstOrDefault(x => x.IdJobDescription == JobDescriptionList[j].IdParent);
                                            if (parentJobDesc != null)
                                            {
                                                EmployeeHeirarchy existParentJDHeirarchy = EmployeeHierarchys.FirstOrDefault(x => x.IdJobDescription == parentJobDesc.IdJobDescription);
                                                if (existParentJDHeirarchy == null)
                                                    continue;
                                            }
                                        }
                                        EmployeeHeirarchy existJDHeirarchy = EmployeeHierarchys.FirstOrDefault(x => x.IdJobDescription == JobDescriptionList[j].IdJobDescription);
                                        if (existJDHeirarchy == null)
                                        {
                                            int EmpCount = JobDescriptionList[j].EmployeeJobDescriptions != null ? JobDescriptionList[j].EmployeeJobDescriptions.Count : 0;
                                            string JobTitle = !string.IsNullOrEmpty(JobDescriptionList[j].Abbreviation) ? JobDescriptionList[j].Abbreviation : JobDescriptionList[j].JobDescriptionTitle;

                                            JobKey++;
                                            EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "JD-" + JobKey, ParentID = "JD-" + JobKey, Name = JobTitle + " " + "(" + EmpCount + ")", ContentType = "Job Description", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = SelectedDepartment.DepartmentHtmlColor, IdJobDescription = JobDescriptionList[j].IdJobDescription, ChildOrientation = "V" });

                                            GetJobDescriptionDataForALL(JobDescriptionList[j]);

                                        }
                                    }
                                }
                            }
                        }
                        // Assign all normal department hierarchy to FinalEmployeeHierarchys list which is binded to diagram control.

                        NormalColumnsSpan = 1;
                        HrmCommon.Instance.ColsSpan = 1;
                        IsolatedColumn = 4;
                        FinalEmployeeHierarchys = new ObservableCollection<EmployeeHeirarchy>(EmployeeHierarchys);
                        FillIsolatedAllDepartmentData();
                        IsolatedDepartmentHierarchyIsVisible = Visibility.Visible;
                        IsEmployeeHierarchicalViewVisible = Visibility.Visible;
                        PrintExportDiagram = 1;
                    }
                    else
                    {
                        EmployeeHierarchys = new List<EmployeeHeirarchy>();
                        if (SelectedDepartment.DepartmentIsIsolated == 0)
                        {
                            //JobDescriptionList = AllJobDescriptionList.Where(x => x.IdDepartment == SelectedDepartment.IdDepartment && x.EmployeeJobDescriptions != null ).ToList();
                            JobDescriptionList = AllJobDescriptionList.Where(x => ((x.IdDepartment == SelectedDepartment.IdDepartment && x.EmployeeJobDescriptions != null) ||
                                                                                   (x.IdDepartment == SelectedDepartment.IdDepartment && x.ChildJobDescriptions.Count > 0 ? x.IsEmployeeExistInChildJD : false) ||
                                                                                   (x.IdDepartment == SelectedDepartment.IdDepartment && x.JobDescriptionIsMandatory == 1))).ToList();
                            if (JobDescriptionList.Count > 0)
                            {
                                for (int i = 0; i < JobDescriptionList.Count; i++)
                                {
                                    if (JobDescriptionList[i].IdParent > 0)
                                    {
                                        JobDescription parentJobDesc = JobDescriptionList.FirstOrDefault(x => x.IdJobDescription == JobDescriptionList[i].IdParent);
                                        if (parentJobDesc != null)
                                        {
                                            EmployeeHeirarchy existParentJDHeirarchy = EmployeeHierarchys.FirstOrDefault(x => x.IdJobDescription == parentJobDesc.IdJobDescription);
                                            if (existParentJDHeirarchy == null)
                                                continue;
                                        }
                                    }
                                    EmployeeHeirarchy existJDHeirarchy = EmployeeHierarchys.FirstOrDefault(x => x.IdJobDescription == JobDescriptionList[i].IdJobDescription);

                                    if (existJDHeirarchy == null)
                                    {
                                        int EmpCount = JobDescriptionList[i].EmployeeJobDescriptions != null ? JobDescriptionList[i].EmployeeJobDescriptions.Count : 0;
                                        string JobTitle = !string.IsNullOrEmpty(JobDescriptionList[i].Abbreviation) ? JobDescriptionList[i].Abbreviation : JobDescriptionList[i].JobDescriptionTitle;

                                        JobKey++;
                                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "JD-" + JobKey, ParentID = "JD-" + JobKey, Name = JobTitle + " " + "(" + EmpCount + ")", ContentType = "Job Description", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = SelectedDepartment.DepartmentHtmlColor, IdJobDescription = JobDescriptionList[i].IdJobDescription, ChildOrientation = "V" });

                                        GetJobDescriptionData(JobDescriptionList[i]);
                                    }
                                }
                            }

                            NormalColumnsSpan = 3;
                            HrmCommon.Instance.ColsSpan = 1;
                            IsolatedColumn = 4;
                            FinalEmployeeHierarchys = new ObservableCollection<EmployeeHeirarchy>(EmployeeHierarchys);
                            EmployeeHierarchysforIsolatedDept = new ObservableCollection<EmployeeHeirarchy>();
                            IsolatedDepartmentHierarchyIsVisible = Visibility.Hidden;
                            IsEmployeeHierarchicalViewVisible = Visibility.Visible;

                            PrintExportDiagram = 2;
                        }
                        else
                        {
                            FillIsolatedSingleDepartmentData();
                            FinalEmployeeHierarchys = new ObservableCollection<EmployeeHeirarchy>();
                            IsEmployeeHierarchicalViewVisible = Visibility.Visible;
                            IsolatedDepartmentHierarchyIsVisible = Visibility.Visible;
                            NormalColumnsSpan = 1;
                            HrmCommon.Instance.ColsSpan = 4;
                            IsolatedColumn = 1;
                            PrintExportDiagram = 3;
                        }
                    }
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeHeirarchicalView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DisplayEmployeeHeirarchicalView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);

                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        //public void SetCustomLayoutItem(DiagramCustomLayoutItemsEventArgs e)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method SetCustomLayoutItem ...", category: Category.Info, priority: Priority.Low);

        //        GeosApplication.Instance.Logger.Log("Method SetCustomLayoutItem() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method SetCustomLayoutItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void ShowEmployeeDetailView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowEmployeeDetailView ...", category: Category.Info, priority: Priority.Low);

                IsEmployeeDetailViewVisible = Visibility.Visible;
                IsEmployeeHierarchicalViewVisible = Visibility.Hidden;
                IsolatedDepartmentHierarchyIsVisible = Visibility.Collapsed;
                SelectedItem = EmployeeDepartments[0];
                IsButtonVisible = Visibility.Collapsed;
                IsEmployeeImageShow = true;
                GeosApplication.Instance.Logger.Log("Method ShowEmployeeDetailView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowEmployeeDetailView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowEmployeeHierarchicalView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowEmployeeHierarchicalView ...", category: Category.Info, priority: Priority.Low);

                IsEmployeeDetailViewVisible = Visibility.Hidden;
                IsEmployeeHierarchicalViewVisible = Visibility.Visible;
                IsolatedDepartmentHierarchyIsVisible = Visibility.Visible;

                SelectedItem = null;
                SelectedItem = EmployeeDepartments.First();
                IsButtonVisible = Visibility.Visible;
                IsEmployeeImageShow = false;
                GeosApplication.Instance.Logger.Log("Method ShowEmployeeHierarchicalView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowEmployeeHierarchicalView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenEducationDocumentForSingleEmployee(object obj)
        {
            try
            {
                AccordionControl a = (AccordionControl)obj;
                Employee empDocument = (Employee)a.SelectedItem;

                //CardView detailView = (CardView)obj;
                //Employee empDocument = (Employee)detailView.FocusedRow;
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()...", category: Category.Info, priority: Priority.Low);
                // Open PDF in another window

                EmployeeDocumentView employeeEducationDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeEducationDocumentViewModel = new EmployeeDocumentViewModel();
                employeeEducationDocumentViewModel.OpenPdfByEmployeeCode(empDocument.EmployeeCode, empDocument.EmployeeJobDescription);
                employeeEducationDocumentView.DataContext = employeeEducationDocumentViewModel;
                employeeEducationDocumentView.Show();

                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to open Job Description Document
        /// </summary>
        /// <param name="obj"></param>
        private void OpenJobDescriptionDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()...", category: Category.Info, priority: Priority.Low);
                // Open PDF in another window

                EmployeeDocumentView employeeEducationDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeEducationDocumentViewModel = new EmployeeDocumentViewModel();
                employeeEducationDocumentViewModel.OpenPdfByEmployeeCode(null, obj, true);
                employeeEducationDocumentView.DataContext = employeeEducationDocumentViewModel;
                employeeEducationDocumentView.Show();

                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

            var values = (object[])obj;
            SelectedItem = null;

            //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            //{
            //    return;
            //}

            AccordionControl accordionControl = (AccordionControl)values[0];
            var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
            if (searchControl != null)
                searchControl.SearchText = null;

            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = System.Windows.ResizeMode.NoResize,
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
            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                //SelectedItem = null;
                FillDepartmentListByPlant();
                SelectedItem = EmployeeDepartments.First();

            }
            else
            {
                EmployeeDepartments = new ObservableCollection<Department>();
                FinalEmployeeHierarchys = new ObservableCollection<EmployeeHeirarchy>();
                EmployeeHierarchysforIsolatedDept = new ObservableCollection<EmployeeHeirarchy>();
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SelectedYearChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

            //if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            //{
            //    return;
            //}

            var values = (object[])obj;
            AccordionControl accordionControl = (AccordionControl)values[0];
            var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
            if (searchControl != null)
                searchControl.SearchText = null;

            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = System.Windows.ResizeMode.NoResize,
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

            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
            {
                FillDepartmentListByPlant();
            }
            else
            {
                EmployeeDepartments = new ObservableCollection<Department>();
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method For Fill Department List By Plant
        /// [001][cpatil][GEOS2-3635][22-03-2022]
        /// </summary>
        public void FillDepartmentListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList.Count > 0 && HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    string plantOwnersIds = string.Empty;

                    if (HrmCommon.Instance.IsOrganizationList.Any(HrmCommon.Instance.SelectedAuthorizedPlantsList.Contains))
                    {
                        List<Company> plantOwners = HrmCommon.Instance.IsOrganizationList.Intersect(HrmCommon.Instance.SelectedAuthorizedPlantsList).Cast<Company>().ToList();
                        // List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    }
                    else if (HrmCommon.Instance.IsOrganizationList.Count > 0)
                    {
                        HrmCommon.Instance.SelectedAuthorizedPlantsList.Clear();
                        HrmCommon.Instance.SelectedAuthorizedPlantsList.Add(HrmCommon.Instance.IsOrganizationList.FirstOrDefault());
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    }
                    #region Service method changed
                    //[GEOS2-2795][rdixit][04.11.2022] Service methods changed from GetOrganizationalChartDepartmentArea_V2250 to GetOrganizationalChartDepartmentArea_V2330
                    //[GEOS2-2795][rdixit][01.11.2022] Service methods changed from GetAllEmployeesForOrganizationByIdCompany_V2250 to GetAllEmployeesForOrganizationByIdCompany_V2330
                    //[GEOS2-2795][rdixit][04.11.2022] Service methods changed from GetEmployeesCountByIdCompany_V2250 to GetEmployeesCountByIdCompany_V2330
                    //[GEOS2-2795][rdixit][04.11.2022] Service methods changed from GetOrganizationHierarchy_V2250 to GetOrganizationHierarchy_V2330
                    //Service updated from GetEmployeesCountByIdCompany_V2330 to GetEmployeesCountByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetEmployeesCountByIdCompany_V2330 to GetOrganizationalChartDepartmentArea_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service updated from GetAllEmployeesForOrganizationByIdCompany_V2420 to GetAllEmployeesForOrganizationByIdCompany_V2440 by [rdixit][GEOS2-4621][10.10.2023]
                    //EmployeeDepartments = new ObservableCollection<Department>((HrmService.GetAllEmployeesForOrganizationByIdCompany_V2440(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission)).Where(i => i.DepartmentInUse == 1).ToList());//[rdixit][11.08.2022][GEOS2-2396]         
                    //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
                    //[rdixit][GEOS2-5659][27.03.2025]
                    //[rdixit][GEOS2-7799][10.04.2025]
                    #endregion

                    EmployeeDepartments = new ObservableCollection<Department>((HrmService.GetAllEmployeesForOrganizationByIdCompany_V2630(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission)).Where(i => i.DepartmentInUse == 1).ToList());//[rdixit][11.08.2022][GEOS2-2396]         
                    #region GEOS2-5140
                    foreach (Department itemDepartment in EmployeeDepartments)
                    {
                        if (itemDepartment.Employees != null)
                        {
                            foreach (Employee item in itemDepartment.Employees)
                            {
                                try
                                {
                                    byte[] ImageBytes = null;
                                    if (GeosApplication.ImageUrlBytePair == null)
                                        GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                                    if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == item.ProfileImagePath.ToLower()))
                                    {
                                        ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == item.ProfileImagePath.ToLower()).Value;
                                        item.ProfileImageInBytes = ImageBytes;
                                    }
                                    else
                                    {
                                        if (GeosApplication.IsImageURLException == false)
                                        {
                                            //using (WebClient webClient = new WebClient())
                                            //{
                                            //    ImageBytes = webClient.DownloadData(item.ProfileImagePath);
                                            //    item.ProfileImageInBytes = ImageBytes;
                                            //}
                                            ImageBytes = Utility.ImageUtil.GetImageByWebClient(item.ProfileImagePath);
                                            item.ProfileImageInBytes = ImageBytes;
                                        }
                                        else
                                        {
                                            ImageBytes = geosService.GetImagesByUrl(item.ProfileImagePath);
                                            item.ProfileImageInBytes = ImageBytes;
                                        }
                                        if (ImageBytes.Length > 0)
                                        {
                                            GeosApplication.ImageUrlBytePair.Add(item.ProfileImagePath, ImageBytes);
                                            item.ProfileImageInBytes = ImageBytes;
                                        }
                                        else
                                        {
                                            item.ProfileImageInBytes = null;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (GeosApplication.IsImageURLException == false)
                                        GeosApplication.IsImageURLException = true;

                                    if (!string.IsNullOrEmpty(item.ProfileImagePath))
                                    {
                                        byte[] ImageBytes = null;
                                        ImageBytes = geosService.GetImagesByUrl(item.ProfileImagePath);
                                        GeosApplication.ImageUrlBytePair.Add(item.ProfileImagePath, ImageBytes);
                                    }

                                }

                            }
                        }
                    }

                    #endregion
                    AllJobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetOrganizationHierarchy_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).Where(i => i.Department.DepartmentInUse == 1).ToList());//[rdixit][11.08.2022][GEOS2-2396]                 
                    TotalEmployeeList = new ObservableCollection<Company>(HrmService.GetEmployeesCountByIdCompany_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    DepartmentAreaAverageList = new ObservableCollection<LookupValue>(HrmService.GetOrganizationalChartDepartmentArea_V2420(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                    #region [rdixit][06.10.2023][GEOS2-4621]
                    if (EmployeeDepartments != null)
                    {
                        foreach (var item1 in EmployeeDepartments)
                        {
                            if (item1.Employees != null)
                            {
                                foreach (var item in item1.Employees)
                                {
                                    if (item.EmployeeContractSituations.Any(i => i.IdEmployeeExitEvent == null))
                                    {
                                        item.EmployeeJobDescription.JobDescriptionStartDate = item.EmployeeContractSituations.Where(i => i.IdEmployeeExitEvent == null).ToList().Select(d => d.ContractSituationStartDate).Min();
                                    }
                                    else
                                    {
                                        EmployeeContractSituation maxContractSituation = item.EmployeeContractSituations.OrderByDescending(s => s.ContractSituationStartDate).FirstOrDefault();
                                        if (maxContractSituation != null)
                                            item.EmployeeJobDescription.JobDescriptionStartDate = item.EmployeeContractSituations.Where(i => i.IdEmployeeExitEvent == maxContractSituation.IdEmployeeExitEvent).ToList().Select(d => d.ContractSituationStartDate).Min();
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

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

        // Recursive function for get Job Description Data.
        private void GetJobDescriptionData(JobDescription JobDesc)
        {
            try
            {
                // add employee in hierarchy class of the job description
                if (JobDesc.EmployeeJobDescriptions != null && JobDesc.EmployeeJobDescriptions.Count > 0)
                {

                    EmployeeGroup++;
                    EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EG-" + EmployeeGroup, ParentID = "JD-" + JobKey, Name = "EmployeeGroup", DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null, ChildOrientation = "V" });

                    for (int j = 0; j < JobDesc.EmployeeJobDescriptions.Count; j++)
                    {
                        // EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "EG-" + EmployeeGroup, Name = JobDesc.EmployeeJobDescriptions[j].Employee.FullName, ContentType = "Employee", IdGender = JobDesc.EmployeeJobDescriptions[j].Employee.IdGender, EmployeeProfileImageInBytes = JobDesc.EmployeeJobDescriptions[j].Employee.ProfileImageInBytes, DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null });

                        List<Usage> Usage = new List<Usage>();
                        Usage ActualUsage = new Usage() { Id = 1, Name = JobDesc.JobDescriptionTitle, Percentage = JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage };
                        Usage OtherUsage = new Usage() { Id = 2, Name = "Other", Percentage = 100 - JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage };
                        Usage.Add(ActualUsage);
                        Usage.Add(OtherUsage);
                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "EG-" + EmployeeGroup, Name = JobDesc.EmployeeJobDescriptions[j].Employee.FullName, ContentType = "Employee", IdGender = JobDesc.EmployeeJobDescriptions[j].Employee.IdGender, EmployeeProfileImageInBytes = JobDesc.EmployeeJobDescriptions[j].Employee.ProfileImageInBytes, DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null, JobDescriptionUsage = JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage, JobDescriptionUsages = Usage });
                        EmpKey++;
                    }
                }

                List<JobDescription> ChildJobDescriptions = AllJobDescriptionList.Where(x => ((x.IdParent == JobDesc.IdJobDescription && x.EmployeeJobDescriptions != null) ||
                                                                                              (x.IdParent == JobDesc.IdJobDescription && x.ChildJobDescriptions.Count > 0 ? x.IsEmployeeExistInChildJD : false) ||
                                                                                              (x.IdParent == JobDesc.IdJobDescription && x.JobDescriptionIsMandatory == 1))).ToList();

                if (ChildJobDescriptions != null && ChildJobDescriptions.Count > 0)
                {
                    JDGroup++;
                    string jdgroupStr = "JG-" + JDGroup;
                    EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = jdgroupStr, ParentID = "JD-" + JobKey, Name = " JDGroup", ContentType = "Job Description", EmployeeProfileImageInBytes = null, ChildOrientation = JobDesc.ChildOrientation });

                    //Add child Job description
                    foreach (JobDescription subJD in ChildJobDescriptions)
                    {
                        int EmpCount = subJD.EmployeeJobDescriptions != null ? subJD.EmployeeJobDescriptions.Count : 0;
                        string JobTitle = !string.IsNullOrEmpty(subJD.Abbreviation) ? subJD.Abbreviation : subJD.JobDescriptionTitle;

                        string ChildOrientation = JobDesc.ChildOrientation == "H" || JobDesc.ChildOrientation == null ? "V" : JobDesc.ChildOrientation;

                        JobKey++;
                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "JD-" + JobKey, ParentID = jdgroupStr, Name = JobTitle + " " + "(" + EmpCount + ")", ContentType = "Job Description", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = subJD.Department.DepartmentHtmlColor, IdJobDescription = subJD.IdJobDescription, ChildOrientation = ChildOrientation });

                        GetJobDescriptionData(subJD);
                    }
                }
                else
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method GetJobDescriptionData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetJobDescriptionData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        // Recursive function for get Job Description Data.
        private void GetJobDescriptionDataForALL(JobDescription JobDesc)
        {
            try
            {
                // add employee in hierarchy class of the job description
                if (JobDesc.EmployeeJobDescriptions != null && JobDesc.EmployeeJobDescriptions.Count > 0)
                {

                    EmployeeGroup++;
                    EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EG-" + EmployeeGroup, ParentID = "JD-" + JobKey, Name = "EmployeeGroup", DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null, ChildOrientation = "V" });

                    for (int j = 0; j < JobDesc.EmployeeJobDescriptions.Count; j++)
                    {
                        // EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "EG-" + EmployeeGroup, Name = JobDesc.EmployeeJobDescriptions[j].Employee.FullName, ContentType = "Employee", IdGender = JobDesc.EmployeeJobDescriptions[j].Employee.IdGender, EmployeeProfileImageInBytes = JobDesc.EmployeeJobDescriptions[j].Employee.ProfileImageInBytes, DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null });

                        List<Usage> Usage = new List<Usage>();
                        Usage ActualUsage = new Usage() { Id = 1, Name = JobDesc.JobDescriptionTitle, Percentage = JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage };
                        Usage OtherUsage = new Usage() { Id = 2, Name = "Other", Percentage = 100 - JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage };
                        Usage.Add(ActualUsage);
                        Usage.Add(OtherUsage);
                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "EG-" + EmployeeGroup, Name = JobDesc.EmployeeJobDescriptions[j].Employee.FullName, ContentType = "Employee", IdGender = JobDesc.EmployeeJobDescriptions[j].Employee.IdGender, EmployeeProfileImageInBytes = JobDesc.EmployeeJobDescriptions[j].Employee.ProfileImageInBytes, DepartmentHtmlColor = "Transparent", JobDescriptionTitle = null, JobDescriptionUsage = JobDesc.EmployeeJobDescriptions[j].JobDescriptionUsage, JobDescriptionUsages = Usage });
                        EmpKey++;
                    }
                }

                List<JobDescription> ChildJobDescriptions = AllJobDescriptionList.Where(x => ((x.IdParent == JobDesc.IdJobDescription && x.EmployeeJobDescriptions != null) ||
                                                                                              (x.IdParent == JobDesc.IdJobDescription && x.ChildJobDescriptions.Count > 0 ? x.IsEmployeeExistInChildJD : false) ||
                                                                                              (x.IdParent == JobDesc.IdJobDescription && x.JobDescriptionIsMandatory == 1)) && (x.JobDescriptionInUse == 1 && x.JobDescriptionIsRemote == 0)).ToList();

                if (ChildJobDescriptions != null && ChildJobDescriptions.Count > 0)
                {
                    JDGroup++;
                    string jdgroupStr = "JG-" + JDGroup;
                    EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = jdgroupStr, ParentID = "JD-" + JobKey, Name = " JDGroup", ContentType = "Job Description", EmployeeProfileImageInBytes = null, ChildOrientation = JobDesc.ChildOrientation });

                    //Add child Job description
                    foreach (JobDescription subJD in ChildJobDescriptions)
                    {
                        int EmpCount = subJD.EmployeeJobDescriptions != null ? subJD.EmployeeJobDescriptions.Count : 0;
                        string JobTitle = !string.IsNullOrEmpty(subJD.Abbreviation) ? subJD.Abbreviation : subJD.JobDescriptionTitle;

                        string ChildOrientation = JobDesc.ChildOrientation == "H" || JobDesc.ChildOrientation == null ? "V" : JobDesc.ChildOrientation;

                        JobKey++;
                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "JD-" + JobKey, ParentID = jdgroupStr, Name = JobTitle + " " + "(" + EmpCount + ")", ContentType = "Job Description", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = subJD.Department.DepartmentHtmlColor, IdJobDescription = subJD.IdJobDescription, ChildOrientation = ChildOrientation });

                        GetJobDescriptionDataForALL(subJD);
                    }
                }
                else
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method GetJobDescriptionData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetJobDescriptionData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // Display All Isolated Department Hierarchy 
        private void FillIsolatedAllDepartmentData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillIsolatedAllDepartmentData ...", category: Category.Info, priority: Priority.Low);
                IsolatedDepartmentList = new List<Department>(EmployeeDepartments.Where(x => x.IdDepartment != 0));
                EmployeeHierarchys = new List<EmployeeHeirarchy>();
                DeptKey = 0; EmpKey = 0;
                for (int i = 0; i < IsolatedDepartmentList.Count; i++)
                {
                    if (IsolatedDepartmentList[i].Employees != null)
                    {
                        // EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "ID-" + DeptKey, ParentID = "ID-" + DeptKey, Name = IsolatedDepartmentList[i].DepartmentName + " " + "(" + IsolatedDepartmentList[i].Employees.Count + ")", ContentType = "Department", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = IsolatedDepartmentList[i].DepartmentHtmlColor, JobDescriptionTitle = null });
                        string JobTitle;
                        for (int j = 0; j < IsolatedDepartmentList[i].Employees.Count; j++)
                        {
                            if (IsolatedDepartmentList[i].Employees[j].EmployeeJobDescription.JobDescription.JobDescriptionIsRemote == 1 && IsolatedDepartmentList[i].Employees[j].EmployeeJobDescription.JobDescription.JobDescriptionInUse == 1)
                            {
                                if (!string.IsNullOrEmpty(IsolatedDepartmentList[i].Employees[j].EmployeeJobDescription.JobDescription.Abbreviation))
                                {
                                    JobTitle = IsolatedDepartmentList[i].Employees[j].EmployeeJobDescription.JobDescription.Abbreviation;
                                }
                                else
                                {
                                    JobTitle = IsolatedDepartmentList[i].Employees[j].EmployeeJobDescription.JobDescription.JobDescriptionTitle;
                                }
                                EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "ID-" + DeptKey, Name = IsolatedDepartmentList[i].Employees[j].FirstName + " " + IsolatedDepartmentList[i].Employees[j].LastName, ContentType = "Employee", IdGender = IsolatedDepartmentList[i].Employees[j].IdGender, EmployeeProfileImageInBytes = IsolatedDepartmentList[i].Employees[j].ProfileImageInBytes, DepartmentHtmlColor = IsolatedDepartmentList[i].DepartmentHtmlColor, JobDescriptionTitle = JobTitle });
                                EmpKey++;
                            }
                        }
                    }
                    //  DeptKey++;
                }
                EmployeeHierarchysforIsolatedDept = new ObservableCollection<EmployeeHeirarchy>(EmployeeHierarchys);
                GeosApplication.Instance.Logger.Log("Method FillIsolatedAllDepartmentData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillIsolatedAllDepartmentData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // Display selected single Isolated Department hierarchy. 
        private void FillIsolatedSingleDepartmentData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillIsolatedDepartmentData ...", category: Category.Info, priority: Priority.Low);

                EmployeeHierarchys = new List<EmployeeHeirarchy>();
                DeptKey = 0; EmpKey = 0;
                EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "ID-" + DeptKey, ParentID = "ID-" + DeptKey, Name = SelectedDepartment.DepartmentName + " " + "(" + SelectedDepartment.Employees.Count + ")", ContentType = "Department", EmployeeProfileImageInBytes = null, DepartmentHtmlColor = SelectedDepartment.DepartmentHtmlColor, JobDescriptionTitle = null });
                if (SelectedDepartment.Employees != null)
                {
                    string JobTitle;
                    for (int i = 0; i < SelectedDepartment.Employees.Count; i++)
                    {

                        if (!string.IsNullOrEmpty(SelectedDepartment.Employees[i].EmployeeJobDescription.JobDescription.Abbreviation))
                        {
                            JobTitle = SelectedDepartment.Employees[i].EmployeeJobDescription.JobDescription.Abbreviation;
                        }
                        else
                        {
                            JobTitle = SelectedDepartment.Employees[i].EmployeeJobDescription.JobDescription.JobDescriptionTitle;
                        }
                        EmployeeHierarchys.Add(new EmployeeHeirarchy() { ID = "EMP-" + EmpKey, ParentID = "ID-" + DeptKey, Name = SelectedDepartment.Employees[i].FirstName + " " + SelectedDepartment.Employees[i].LastName, ContentType = "Employee", IdGender = SelectedDepartment.Employees[i].IdGender, EmployeeProfileImageInBytes = SelectedDepartment.Employees[i].ProfileImageInBytes, DepartmentHtmlColor = SelectedDepartment.DepartmentHtmlColor, JobDescriptionTitle = JobTitle });
                        EmpKey++;

                    }
                }
                EmployeeHierarchysforIsolatedDept = new ObservableCollection<EmployeeHeirarchy>(EmployeeHierarchys);
                GeosApplication.Instance.Logger.Log("Method FillIsolatedDepartmentData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillIsolatedDepartmentData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Refresh Organization View
        /// </summary>

        public void RefreshOrganizationView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshOrganizationView()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            // ResizeMode = ResizeMode.NoResize,
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

                AccordionControl accordionControl = (AccordionControl)obj;
                var searchControl = LayoutTreeHelper.GetVisualChildren(accordionControl).OfType<SearchControl>().FirstOrDefault();
                if (searchControl != null)
                    searchControl.SearchText = null;
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    FillDepartmentListByPlant();
                    SelectedItem = EmployeeDepartments[0];
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshOrganizationView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshOrganizationView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Print Hierarchical Employee Diagram
        /// [HRM-M043-09] Hierarchy by JD not by department	--- By amit
        /// </summary>

        private void PrintHierarchicalEmployeeDiagram(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintHierarchicalEmployeeDiagram()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = System.Windows.ResizeMode.NoResize,
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

                //Font font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, 9.75F);
                var values = (object[])obj;
                string plantName = string.Empty;
                double plantTotal = 0;
                DiagramControl diaga = (DiagramControl)values[0];
                DiagramControl diaga1 = (DiagramControl)values[1];

                EmployeeHierarchyReport empHieReport = new EmployeeHierarchyReport();

                empHieReport.xrDepartmentAverageChart.BeginInit();
                empHieReport.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.Font.Size); // new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, 9.75F);
                empHieReport.xrLabel1.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.xrLabel1.Font.Size);
                // empHieReport.xrlblPlantName.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.xrlblPlantName.Font.Size);
                empHieReport.xrLabel2.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.xrLabel2.Font.Size);
                empHieReport.xrlblPlantTotal.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.xrlblPlantTotal.Font.Size);
                empHieReport.xrDepartmentAverageChart.Legend.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.xrDepartmentAverageChart.Legend.Font.Size);
                empHieReport.xrLabel3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);

                // For All Department Diagram Isolated and Normal department
                if (PrintExportDiagram == 1)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        diaga.ExportDiagram(ms, DevExpress.Diagram.Core.DiagramExportFormat.PNG);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = BitmapImage2Bitmap(image);
                        Bitmap employeeHierarchy = ImageTrim(img);
                        empHieReport.empHierarchyImage.Image = employeeHierarchy;
                        empHieReport.empHierarchyImage.Sizing = ImageSizeMode.StretchImage;
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        diaga1.ExportDiagram(ms, DevExpress.Diagram.Core.DiagramExportFormat.PNG);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = BitmapImage2Bitmap(image);
                        if (EmployeeHierarchysforIsolatedDept.Count > 0)
                        {
                            Bitmap empHerarchyIsolatedDept = ImageTrim(img);
                            empHieReport.empHerarchyIsolatedDept.Image = empHerarchyIsolatedDept;
                        }
                        else
                            empHieReport.empHerarchyIsolatedDept.Image = img;
                        empHieReport.empHerarchyIsolatedDept.Sizing = ImageSizeMode.Squeeze;
                    }
                    //empHieReport.xrBandBoxNormal.Visible = true;
                    //empHieReport.xrBandBoxIsolated.Visible = true;

                }
                // For Single Selected Normal Department Diagram 
                if (PrintExportDiagram == 2)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        diaga.ExportDiagram(ms, DevExpress.Diagram.Core.DiagramExportFormat.PNG);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = BitmapImage2Bitmap(image);
                        empHieReport.empHierarchyImage.Image = img;
                        empHieReport.empHierarchyImage.WidthF = 820;
                        empHieReport.empHierarchyImage.ImageAlignment = ImageAlignment.MiddleCenter;
                        empHieReport.empHerarchyIsolatedDept.WidthF = 1;
                        empHieReport.empHierarchyImage.Sizing = ImageSizeMode.Squeeze;
                        empHieReport.xrBandBoxNormal.Visible = false;
                        empHieReport.xrBandBoxIsolated.Visible = false;
                    }
                }
                // For Single Selected Isolated Department Diagram 
                if (PrintExportDiagram == 3)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        diaga1.ExportDiagram(ms, DevExpress.Diagram.Core.DiagramExportFormat.PNG);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = BitmapImage2Bitmap(image);
                        empHieReport.empHierarchyImage.Image = img;
                        empHieReport.empHierarchyImage.WidthF = 820;
                        empHieReport.empHerarchyIsolatedDept.WidthF = 1;
                        empHieReport.empHierarchyImage.Sizing = ImageSizeMode.StretchImage;
                        empHieReport.xrBandBoxNormal.Visible = false;
                        empHieReport.xrBandBoxIsolated.Visible = false;
                    }
                }
                empHieReport.imgLogo.Image = ReportHeaderImage;
                // This code display the selected plant name and total employee of plant
                if (TotalEmployeeList.Count > 0)
                {
                    int k = 1081;
                    for (int i = 0; i < TotalEmployeeList.Count; i++)
                    {
                        XRLabel label = new XRLabel() { Text = TotalEmployeeList[i].Alias };
                        label.WidthF = 60;
                        label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, empHieReport.Font.Size);
                        empHieReport.Bands[BandKind.TopMargin].Controls.Add(label);
                        label.LocationF = new System.Drawing.PointF(k, 22);
                        label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        label.ForeColor = System.Drawing.Color.White;
                        label.BackColor = System.Drawing.Color.Black;
                        k = k - 70;
                        // plantName = plantName + " " + TotalEmployeeList[i].Alias;
                        plantTotal = plantTotal + TotalEmployeeList[i].EmployeesCount;
                    }
                    // empHieReport.xrlblPlantName.Text = plantName;
                    empHieReport.xrlblPlantTotal.Text = plantTotal.ToString();

                    plantName = string.Join(",", TotalEmployeeList.Select(x => x.Alias));
                }


                XRTable table1 = new XRTable();
                table1.Borders = DevExpress.XtraPrinting.BorderSide.All;
                table1.BorderWidth = 1;
                table1.LocationF = new System.Drawing.PointF(16F, 5F);
                table1.WidthF = 575;

                XRTableRow row3 = new XRTableRow();
                XRTableCell cell3 = new XRTableCell();
                cell3.Text = "Plant Data";
                cell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                cell3.BackColor = System.Drawing.Color.WhiteSmoke;
                row3.Cells.Add(cell3);
                table1.Rows.Add(row3);
                //empHieReport.Bands[BandKind.Detail].Controls.Add(table1);
                table1.EndInit();


                XRLabel label2 = new XRLabel();
                label2.LocationF = new System.Drawing.PointF(16F, 5F);
                label2.Borders = BorderSide.All;
                label2.BorderWidth = 1;
                label2.SizeF = new System.Drawing.SizeF(575F, 65F);
                //empHieReport.Bands[BandKind.Detail].Controls.Add(label2);

                XRTable table = new XRTable();
                table.Borders = DevExpress.XtraPrinting.BorderSide.All;
                table.BorderWidth = 1;
                table.LocationF = new System.Drawing.PointF(595F, 5F);
                table.WidthF = 550;
                table.BeginInit();
                for (int i = 0; i < 1; i++)
                {
                    XRTableRow row2 = new XRTableRow();
                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = "HR Data";
                    cell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    cell2.WidthF = DepartmentAreaAverageList.Count * 250;
                    cell2.BackColor = System.Drawing.Color.WhiteSmoke;
                    row2.Cells.Add(cell2);
                    table.Rows.Add(row2);
                    XRTableRow row = new XRTableRow();
                    for (int j = 0; j < DepartmentAreaAverageList.Count; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        cell.Text = DepartmentAreaAverageList[j].Value;
                        cell.WidthF = 580 / DepartmentAreaAverageList.Count;
                        cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        row.Cells.Add(cell);
                    }
                    table.Rows.Add(row);

                    XRTableRow row1 = new XRTableRow();
                    for (int j = 0; j < DepartmentAreaAverageList.Count; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        cell.Text = DepartmentAreaAverageList[j].Count.ToString();
                        cell.WidthF = 580 / DepartmentAreaAverageList.Count;
                        cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(DepartmentAreaAverageList[j].HtmlColor);
                        cell.BackColor = color;
                        row1.Cells.Add(cell);
                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = DepartmentAreaAverageList[j].Percentage.ToString() + "%";
                        cell1.WidthF = 580 / DepartmentAreaAverageList.Count;
                        cell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        row1.Cells.Add(cell1);
                    }
                    table.Rows.Add(row1);

                }
                // empHieReport.Bands[BandKind.Detail].Controls.Add(table);
                table.EndInit();

                ReportFooterBand band = new ReportFooterBand();
                band.Controls.Add(table1);
                band.Controls.Add(label2);
                band.Controls.Add(table);
                band.PrintAtBottom = true;
                empHieReport.Bands.Add(band);

                empHieReport.xrDepartmentAverageChart.DataSource = DepartmentAreaAverageList;
                //empHieReport.xrDepartmentAverageChart.Series[1].Name = plantName;
                empHieReport.xrDepartmentAverageChart.Series[1].LegendTextPattern = plantName;
                empHieReport.xrDepartmentAverageChart.EndInit();
                empHieReport.xrLabel4.Text = TotalProductionPercentage.ToString();

                DashboardGauge gaugeControl = (DashboardGauge)empHieReport.xrGauge1.Gauge;
                gaugeControl.BeginUpdate();
                gaugeControl.Model.Composite.Remove(gaugeControl.Elements[1] as IRenderableElement);
                gaugeControl.Elements.Remove(gaugeControl.Elements[1]);
                gaugeControl.EndUpdate();
                SetupArcScaleForRetention(gaugeControl.Elements[0] as ArcScale);
                SetupArcScaleBackground(gaugeControl.Elements[1] as ArcScaleBackgroundLayer);
                CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
                empHieReport.xrLabel3.Text = GeosApplication.Instance.ServerDateTime.Date.ToString("d", CultureInfo);

                //To hide Isolated Dept. if empty
                //(#70708) Improve the visualization of the org.chart [16-04-2019][sdesai]
                if (EmployeeHierarchysforIsolatedDept.Count <= 0)
                {
                    empHieReport.empHerarchyIsolatedDept.Visible = false;
                    var pageWidth = empHieReport.PageWidth;
                    empHieReport.empHierarchyImage.WidthF = pageWidth - 15;
                    empHieReport.empHierarchyImage.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopCenter;
                    empHieReport.empHierarchyImage.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                    empHieReport.xrBandBoxNormal.WidthF = 1130F;
                    empHieReport.empHierarchyImage.SizeF = new System.Drawing.SizeF(1110F, 1330F);
                }

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = empHieReport;
                empHieReport.CreateDocument();
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintHierarchicalEmployeeDiagram()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintHierarchicalEmployeeDiagram()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private Bitmap ImageTrim(Bitmap img)
        {
            //get image data
            BitmapData bd = img.LockBits(new Rectangle(System.Drawing.Point.Empty, img.Size),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int[] rgbValues = new int[img.Height * img.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            img.UnlockBits(bd);

            #region determine bounds
            int left = bd.Width;
            int top = bd.Height;
            int right = 0;
            int bottom = 0;


            //determine top
            for (int i = 0; i < rgbValues.Length; i++)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    top = r;
                    break;
                }
            }

            //determine bottom
            for (int i = rgbValues.Length - 1; i >= 0; i--)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (int c = 0; c < left; c++)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (left > c)
                            {
                                left = c;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int c = bd.Width - 1; c > right; c--)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (right < c)
                            {
                                right = c;
                                break;
                            }
                        }
                    }
                }
            }

            int width = right - left + 1;
            int height = bottom - top + 1;
            #endregion

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);
            }

            //create new image
            Bitmap newImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData nbd
                = newImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);

            return newImage;
        }
        /// <summary>
        /// Method to Set up ArcScale Background
        /// </summary>
        /// <param name="arcScaleBackgroundLayer"></param>
        static void SetupArcScaleBackground(ArcScaleBackgroundLayer arcScaleBackgroundLayer)
        {
            arcScaleBackgroundLayer.ShapeType = BackgroundLayerShapeType.CircularHalf_Style27;
            arcScaleBackgroundLayer.ZOrder = 1000;

        }
        /// <summary>
        /// Method to Set u pArcScale For Retention
        /// </summary>
        /// <param name="arcScale"></param>
        static void SetupArcScaleForRetention(ArcScale arcScale)
        {
            arcScale.BeginUpdate();
            AddScaleRangesForRetention(arcScale);

            arcScale.MaxValue = (Int32)TotalPercentage;
            arcScale.MinValue = 0;
            arcScale.Center = new PointF2D(125, 160);

            arcScale.MinorTickmark.ShowTick = false;

            arcScale.MajorTickmark.ShowTick = false;
            arcScale.MajorTickmark.ShowText = false;

            arcScale.StartAngle = -180;
            arcScale.EndAngle = 0;

            arcScale.RadiusX = 102;
            arcScale.RadiusY = 102;

            arcScale.EndUpdate();
        }
        /// <summary>
        /// Method to Add Scale Ranges For Retention
        /// </summary>
        /// <param name="scale"></param>
        static void AddScaleRangesForRetention(ArcScale scale)
        {
            ArcScaleRange range1 = new ArcScaleRange();
            range1.AppearanceRange.ContentBrush = new SolidBrushObject(System.Drawing.Color.Blue);
            range1.StartValue = 0;
            range1.EndValue = (Int32)TotalProductionPercentage;
            range1.StartThickness = 15;
            range1.EndThickness = 15;
            range1.ShapeOffset = 0;

            ArcScaleRange range2 = new ArcScaleRange();
            range2.AppearanceRange.ContentBrush = new SolidBrushObject(System.Drawing.Color.White);
            range2.StartValue = (Int32)TotalProductionPercentage;
            range2.EndValue = (Int32)TotalPercentage; ;
            range2.StartThickness = 15;
            range2.EndThickness = 15;
            range2.ShapeOffset = 0;

            scale.Ranges.AddRange(new IRange[] { range1, range2 });
        }

        /// <summary>
        /// Method For Convert Bitmap Image to Image
        /// [HRM-M043-09] Hierarchy by JD not by department	--- By amit
        /// </summary>
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            return new Bitmap(bitmapImage.StreamSource);
        }


        /// <summary>
        /// Method For Export Hierarchical Employee Diagram
        /// [HRM-M043-09] Hierarchy by JD not by department	--- By amit
        /// </summary>

        private void ExportHierarchicalEmployeeDiagram(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintHierarchicalEmployeeDiagram()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = System.Windows.ResizeMode.NoResize,
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

                var values = (object[])obj;
                DiagramControl normalDeptDiagram = (DiagramControl)values[0];
                DiagramControl isolatedDeptDiagram = (DiagramControl)values[1];
                normalDeptDiagram.FitToDrawing();
                string exportedPath = "";

                if (PrintExportDiagram == 1)
                {
                    exportedPath = normalDeptDiagram.ExportNormalDiagram(DiagramExportFormat.PNG);
                    //exportedPath = isolatedDeptDiagram.ExportIsolatedDiagram(DiagramExportFormat.PNG);     
                }

                if (PrintExportDiagram == 2)
                {
                    exportedPath = normalDeptDiagram.ExportNormalDiagram(DiagramExportFormat.PNG);
                }

                if (PrintExportDiagram == 3)
                {
                    exportedPath = isolatedDeptDiagram.ExportIsolatedDiagram(DiagramExportFormat.PNG);
                }

                System.Diagnostics.Process photoViewer = new System.Diagnostics.Process();
                photoViewer.StartInfo.FileName = @"" + exportedPath;
                photoViewer.Start();
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintHierarchicalEmployeeDiagram()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintHierarchicalEmployeeDiagram()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // This function display the selected department data in Employee detail view.
        private void SelectItemForCompany()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForCompany()...", category: Category.Info, priority: Priority.Low);

                if (IsEmployeeDetailViewVisible == Visibility.Visible)
                {
                    Employees = new List<Employee>();
                    List<Employee> EmployeeList = new List<Employee>();
                    OrganizationGridList = new List<Employee>();

                    if (SelectedItem is Employee)
                    {
                        EmployeeList.Add((Employee)SelectedItem);
                        Employees = new List<Employee>(EmployeeList);
                        OrganizationGridList = Employees;
                    }
                    else if (SelectedItem is Department)
                    {
                        SelectedDepartment = new Department();
                        SelectedDepartment = (Department)SelectedItem;

                        if (SelectedDepartment.DepartmentName == "ALL")
                        {
                            for (int i = 1; i < EmployeeDepartments.Count; i++)
                            {
                                for (int j = 0; j < EmployeeDepartments[i].Employees.Count; j++)
                                {
                                    EmployeeList.Add(EmployeeDepartments[i].Employees[j]);
                                }
                            }

                            Employees = new List<Employee>(EmployeeList);
                            OrganizationGridList = Employees.GroupBy(x => x.IdEmployee, (key, g) => g.OrderBy(e => e.IdEmployee).First()).ToList();
                        }
                        else
                        {
                            var DeptId = (EmployeeDepartments.Where(x => x.IdDepartment == ((Department)SelectedItem).IdDepartment)).ToList().DefaultIfEmpty();
                            Employees = ((Department)SelectedItem).Employees.ToList();
                            OrganizationGridList = Employees.GroupBy(x => x.IdEmployee, (key, g) => g.OrderBy(e => e.IdEmployee).First()).ToList();
                        }
                    }
                    else
                    {
                        Employees = new List<Employee>();
                        OrganizationGridList = new List<Employee>();
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SelectItemForCompany()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForCompany()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For display selected employee profile.
        /// </summary>
        private void OpenEmployeeProfileDetailView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetailView()...", category: Category.Info, priority: Priority.Low);
                CardView detailView = (CardView)((object[])obj)[0];
                Employee employee = (Employee)detailView.DataControl.CurrentItem;

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
                                ResizeMode = System.Windows.ResizeMode.NoResize,
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

                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                        if (HrmCommon.Instance.IsPermissionReadOnly)
                            employeeProfileDetailViewModel.InitReadOnly(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                        else
                            employeeProfileDetailViewModel.Init(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                    }

                    employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);
                    employeeProfileDetailView.ShowDialog();

                    if (employeeProfileDetailViewModel.IsSaveChanges)
                    {
                        if (employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployeeStatus == 138)
                        {
                            foreach (Department item in EmployeeDepartments)
                            {
                                if (item.Employees != null &&
                                    item.Employees.Count > 0 &&
                                    item.Employees.Any(x => x.IdEmployee == employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployee))
                                {
                                    item.Employees.Remove(item.Employees.FirstOrDefault(x => x.IdEmployee == employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployee));
                                }
                            }

                            OrganizationGridList.Remove(OrganizationGridList.FirstOrDefault(x => x.IdEmployee == employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdEmployee));
                            employee = null;

                            Int16 idDepartment = -1;
                            if (SelectedItem is Employee)
                            {
                                SelectedItem = null;
                            }
                            else if (SelectedItem is Department)
                            {
                                idDepartment = Convert.ToInt16(((Department)SelectedItem).IdDepartment);
                            }

                            AccordionControl accordionControl = (AccordionControl)((object[])obj)[1];
                            ObservableCollection<Department> temp = new ObservableCollection<Department>(EmployeeDepartments.ToList());
                            accordionControl.ItemsSource = null;
                            accordionControl.ItemsSource = temp;
                            accordionControl.UpdateLayout();

                            if (idDepartment != -1)
                                SelectedItem = temp.FirstOrDefault(x => x.IdDepartment == idDepartment);

                            //This code is done to refresh accordioan and gridcontrol card view.
                            detailView.DataControl.RefreshData();
                        }
                        else
                        {
                            employee.EmployeeCode = employeeProfileDetailViewModel.EmployeeUpdatedDetail.EmployeeCode;
                            employee.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                            employee.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;
                            employee.NativeName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.NativeName;
                            employee.Gender = employeeProfileDetailViewModel.EmployeeUpdatedDetail.Gender;
                            employee.IdGender = employeeProfileDetailViewModel.EmployeeUpdatedDetail.IdGender;
                            employee.AddressStreet = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressStreet;
                            employee.AddressZipCode = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressZipCode;
                            employee.AddressRegion = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressRegion;
                            employee.AddressCountry = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressCountry;
                            employee.AddressCity = employeeProfileDetailViewModel.EmployeeUpdatedDetail.AddressCity;
                            employee.ProfileImageInBytes = employeeProfileDetailViewModel.EmployeeUpdatedDetail.ProfileImageInBytes;
                            employee.LengthOfServiceString = employeeProfileDetailViewModel.LengthOfService;


                            employee.FullAddress = employee.AddressStreet;
                            if (!string.IsNullOrEmpty(employee.AddressZipCode))
                                employee.FullAddress = employee.FullAddress + " - " + employee.AddressZipCode;
                            if (!string.IsNullOrEmpty(employee.AddressCity))
                                employee.FullAddress = employee.FullAddress + " - " + employee.AddressCity;
                            if (!string.IsNullOrEmpty(employee.AddressRegion))
                                employee.FullAddress = employee.FullAddress + " - " + employee.AddressRegion;
                            if (!string.IsNullOrEmpty(employee.AddressCountry.Name))
                                employee.FullAddress = employee.FullAddress + " - " + employee.AddressCountry.Name;

                            employee.EmployeeProfessionalContacts = new List<EmployeeContact>(employeeProfileDetailViewModel.EmployeeProfessionalContactList);
                            employee.EmployeeContractSituations = new List<EmployeeContractSituation>(employeeProfileDetailViewModel.EmployeeContractSituationList);
                            employee.EmployeeJobDescriptions = new List<EmployeeJobDescription>(employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)));

                            if (employee.EmployeeJobDescriptions.Count > 0)
                            {
                                employee.EmployeeJobDescription.JobDescription.JobDescriptionCode = employee.EmployeeJobDescriptions[0].JobDescription.JobDescriptionCode;
                                employee.EmployeeJobDescription.JobDescription.JobDescriptionTitle = employee.EmployeeJobDescriptions[0].JobDescription.JobDescriptionTitle;
                                employee.EmployeeJobDescription.JobDescriptionUsage = employee.EmployeeJobDescriptions[0].JobDescriptionUsage;
                            }

                            if (employee.EmployeeProfessionalContacts.Count > 0)
                            {
                                EmployeeContact ProfContact = employee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88);
                                if (ProfContact != null)
                                {
                                    employee.EmployeeContactEmail = ProfContact.EmployeeContactValue;
                                }
                                else
                                {
                                    employee.EmployeeContactEmail = string.Empty;
                                }
                                ProfContact = employee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90);
                                if (ProfContact != null)
                                {
                                    employee.EmployeeContactMobile = ProfContact.EmployeeContactValue;
                                }
                                else
                                {
                                    employee.EmployeeContactMobile = string.Empty;
                                }
                            }

                            if (employee.EmployeeContractSituations.Count > 0)
                            {
                                if (employee.EmployeeContractSituations != null)
                                {
                                    EmployeeContractSituation contract = employee.EmployeeContractSituations.FirstOrDefault(x => x.ContractSituationEndDate >= GeosApplication.Instance.ServerDateTime.Date || x.ContractSituationEndDate == null);
                                    if (contract != null)
                                        employee.ContractSituation.Name = contract.ContractSituation.Name + " [" + contract.Company.Alias + "]";
                                    else
                                    {
                                        EmployeeContractSituation _contract = employee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault();
                                        if (_contract != null)
                                            employee.ContractSituation.Name = _contract.ContractSituation.Name + " [" + _contract.Company.Alias + "]";
                                    }
                                }
                                else
                                {
                                    employee.ContractSituation.Name = string.Empty;
                                }
                            }

                            employee.EmployeeJobCodes = employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)).Select(x => x.JobDescription.JobDescriptionCode).ToList();

                            if (employee.EmployeeJobDescription.JobDescriptionUsage == 100)
                            {
                                employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)).Select(x => x.JobDescription.JobDescriptionTitle + " [" + x.Company.Alias + "]").ToArray());
                            }
                            else
                            {
                                employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)).Select(x => x.JobDescription.JobDescriptionTitle + "(" + x.JobDescriptionUsage + "%)" + " [" + x.Company.Alias + "]").ToArray());
                            }

                            employee.EmployeeJobTitles = AddPlantAliasToJobTitle(employee.EmployeeJobTitles, employeeProfileDetailViewModel.EmployeeJobDescriptionList.Where(ejd => ejd.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date && (ejd.JobDescriptionEndDate == null || ejd.JobDescriptionEndDate >= DateTime.Now)).ToList());
                        }
                    }

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetailView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfileDetailView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Add plant alias when JD belongs to different plant
        /// [cpatil][2018-12-25][HRM-M053-04] Add plant when JD belongs to different plants
        /// [001][skale][08-08-2019][GEOS2-1650]100% should not display when JD is only one (In Organization)
        /// </summary>
        private string AddPlantAliasToJobTitle(string employeeJobTitles, List<EmployeeJobDescription> empJobDescription)
        {
            if (!string.IsNullOrEmpty(employeeJobTitles))
            {
                List<String> result = new List<String>(employeeJobTitles.Split(new string[] { "\n" }, StringSplitOptions.None));
                if (result.Count() > 1)
                {
                    string matchString = null;
                    bool isDiffPlant = false;
                    matchString = result[0].Substring(result[0].IndexOf("[") + 1, ((result[0].IndexOf("]")) - (result[0].IndexOf("[") + 1)));
                    if (string.IsNullOrEmpty(matchString))
                    {
                        isDiffPlant = false;
                    }
                    else
                    {
                        matchString = " [" + matchString + "]";
                        foreach (string item in result)
                        {
                            if (!item.Contains(matchString))
                            {
                                isDiffPlant = true;
                                break;
                            }
                        }
                    }

                    if (isDiffPlant == false)
                    {
                        employeeJobTitles = String.Join("\n", empJobDescription.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)").ToArray());
                    }

                }
                else if (result.Count() == 1)
                {
                    // employeeJobTitles = String.Join("\n", empJobDescription.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)").ToArray());
                    //[001]Added
                    employeeJobTitles = String.Join("\n", empJobDescription.Select(x => x.JobDescription.JobDescriptionTitle));
                }

            }
            return employeeJobTitles;
        }

        private void PieChartDrawSeriesPointCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PieChartDrawSeriesPointCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CustomDrawSeriesPointEventArgs e = (CustomDrawSeriesPointEventArgs)obj;

                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                {
                    if (e.SeriesPoint.Argument == "Other")
                        e.DrawOptions.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#333333 ");
                    if (e.SeriesPoint.Argument != "Other")
                        e.DrawOptions.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF");//Green
                }
                else if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    if (e.SeriesPoint.Argument == "Other")
                        e.DrawOptions.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF ");
                    if (e.SeriesPoint.Argument != "Other")
                        e.DrawOptions.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#333333");//Green
                }

                GeosApplication.Instance.Logger.Log("Method PieChartDrawSeriesPointCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PieChartDrawSeriesPointCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fit diagram to page when department changed.
        /// [GEOS2-1627][Sprint-66][sdesai] Improve the visualization of the org. chart
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedDepartmentChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedDepartmentChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (obj != null)
                {
                    var values = (object[])obj;
                    DiagramControl IsolatedDepartmentHierarchy = (DiagramControl)values[0];
                    DiagramControl EmployeeHierarchy = (DiagramControl)values[1];

                    if (IsolatedDepartmentHierarchy != null)
                        IsolatedDepartmentHierarchy.FitToDrawing();

                    if (EmployeeHierarchy != null)
                    {
                        //EmployeeHierarchy.CanvasSizeMode = CanvasSizeMode.Fill;
                        //SelectedDepartment
                        if (SelectedItem is Department && (((Department)SelectedItem).IdDepartment == 0 || ((Department)SelectedItem).IdDepartment == 9))
                        {
                            EmployeeHierarchy.UpdateConnectorsRoute(EmployeeHierarchy.Items.OfType<DiagramConnector>());
                            EmployeeHierarchy.ScrollToPoint(new System.Windows.Point(100, 1100), HorizontalAlignment.Center, VerticalAlignment.Center);
                            EmployeeHierarchy.FitToWidth();
                            //EmployeeHierarchy.UpdateLayout();
                        }
                        else
                        {
                            EmployeeHierarchy.FitToDrawing();
                        }

                        //EmployeeHierarchy.CanvasSizeMode = CanvasSizeMode.AutoSize;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method SelectedDepartmentChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedDepartmentChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000][skale][23/01/2020][GEOS2-1919]- GHRM - Org chart in excel
        /// [001][cpatil][24/08/2020][GEOS2-2539]- In HRM system, organization chart does not display all employees.
        /// [002][avpawar][GEOS2-2539] - Identify the Job descriptions working remotely 
        /// [003][cpatil][GEOS2-2819] - Org Chart wrong
        /// [004][cpatil][GEOS2-3635][22-03-2022]
        /// </summary>
        /// <param name="obj"></param>
        public void OrganizationChartExportToExcel(object obj)
        {

            Workbook workbook = new Workbook();
            FileStream stream = null;

            GeosApplication.Instance.Logger.Log("Method OrganizationChartExportToExcel ...", category: Category.Info, priority: Priority.Low);

            try
            {
                ObservableCollection<EmployeeOrganizationChart> OrganizationChartEmployeeList = new ObservableCollection<EmployeeOrganizationChart>();
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {

                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList.Count <= 1)
                    {

                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.DefaultExt = "xlsx";
                        saveFile.FileName = plantOwners[0].Alias + "_OrgChart";
                        saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                        saveFile.FilterIndex = 1;
                        saveFile.Title = "Save Organization Chart Excel Report";

                        if (!(Boolean)saveFile.ShowDialog())
                        {
                            ExcelFilePath = string.Empty;
                        }
                        else
                        {
                            StartPleaseWaitIndicator();

                            byte[] excelTemplateInByte = geosService.GetOrganizationChart(plantOwners[0].Alias);
                            //[001] Changed service method
                            // OrganizationChartEmployeeList = new ObservableCollection<EmployeeOrganizationChart>(HrmService.GetEmployeesForOrganizationChart_V2046(plantOwners[0].IdCompany, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                            //[002] Changed service method
                            //[003] Changed service method
                            //[004] Changed service method
                            //[005] Changed service method from GetEmployeesForOrganizationChart_V2250 to GetEmployeesForOrganizationChart_V2330
                            //OrganizationChartEmployeeList = new ObservableCollection<EmployeeOrganizationChart>(HrmService.GetEmployeesForOrganizationChart_V2330(plantOwners[0].IdCompany, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                            //[Sudhir.Jangra][GEOS2-4038][04/07/2023]
                            //Service Method Changed from GetEmployeesForOrganizationChart_V2410 to GetEmployeesForOrganizationChart_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                            OrganizationChartEmployeeList = new ObservableCollection<EmployeeOrganizationChart>(HrmService.GetEmployeesForOrganizationChart_V2420(plantOwners[0].IdCompany, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                            Dictionary<string, decimal> PlantWorkHourslist = HrmService.GetPlantWorkHours(plantOwners[0].IdCompany, HrmCommon.Instance.SelectedPeriod);
                            List<CompanyDepartment> CompanyDepartments = HrmService.GetNumberOfWorkStationByIdDepartment("58,59,61", plantOwners[0].IdCompany);
                            List<CompanyDepartment> CompanyDepartmentAreas = HrmService.GetSizeByIdDepartmentArea("129,127", plantOwners[0].IdCompany);
                            double CompanySize = HrmService.GetCompanySize(plantOwners[0].IdCompany);

                            if (OrganizationChartEmployeeList != null && OrganizationChartEmployeeList.Count > 0)
                            {
                                ExcelFilePath = (saveFile.FileName);
                                workbook.LoadDocument(excelTemplateInByte, DocumentFormat.Xlsm);
                                Worksheet wsDl = workbook.Worksheets[1];

                                string cellPercentageFormat = @"0.00%";

                                using (stream = new FileStream(ExcelFilePath, FileMode.Create, FileAccess.ReadWrite))
                                {
                                    for (int i = 0; i < OrganizationChartEmployeeList.Count; i++)
                                    {
                                        wsDl.Cells[i + 2, 0].Value = OrganizationChartEmployeeList[i].JobCode;
                                        wsDl.Cells[i + 2, 1].Value = OrganizationChartEmployeeList[i].JDCode;
                                        wsDl.Cells[i + 2, 2].Value = (double)OrganizationChartEmployeeList[i].JobDescriptionUsage / 100;
                                        wsDl.Cells[i + 2, 3].Value = OrganizationChartEmployeeList[i].DepartmentName;
                                        wsDl.Cells[i + 2, 4].Value = OrganizationChartEmployeeList[i].Organization;
                                        wsDl.Cells[i + 2, 5].Value = OrganizationChartEmployeeList[i].EmployeeCode;
                                        wsDl.Cells[i + 2, 6].Value = OrganizationChartEmployeeList[i].FirstName;
                                        wsDl.Cells[i + 2, 7].Value = OrganizationChartEmployeeList[i].LastName;
                                        wsDl.Cells[i + 2, 8].Value = OrganizationChartEmployeeList[i].JobTitle;
                                        wsDl.Cells[i + 2, 9].Value = OrganizationChartEmployeeList[i].BirthDate;
                                        wsDl.Cells[i + 2, 10].Value = OrganizationChartEmployeeList[i].HireDate;
                                        wsDl.Cells[i + 2, 11].Value = OrganizationChartEmployeeList[i].EmployeeStatus;
                                        wsDl.Cells[i + 2, 12].Value = OrganizationChartEmployeeList[i].CompanyLocation;
                                        wsDl.Cells[i + 2, 13].Value = OrganizationChartEmployeeList[i].Company;
                                        wsDl.Cells[i + 2, 14].Value = OrganizationChartEmployeeList[i].LengthOfServiceString;
                                        wsDl.Cells[i + 2, 15].Value = OrganizationChartEmployeeList[i].JDRemote; //[002] Added
                                        //[Sudhir.Jangra][GEOS2-4038][04/07/2023]
                                        wsDl.Cells[0, 16].ColumnWidth = 500;
                                        wsDl.Cells[1, 16].ColumnWidth = 500;
                                        wsDl.Cells[i + 2, 16].Value = OrganizationChartEmployeeList[i].ShiftName;
                                        wsDl.Cells[0, 17].ColumnWidth = 250;
                                        wsDl.Cells[1, 17].ColumnWidth = 250;
                                        wsDl.Cells[i + 2, 17].Value = OrganizationChartEmployeeList[i].ShiftType;
                                        wsDl.Cells[0, 18].ColumnWidth = 400;
                                        wsDl.Cells[1, 18].ColumnWidth = 400;
                                        wsDl.Cells[i + 2, 18].Value = OrganizationChartEmployeeList[i].MonStartTime;
                                        wsDl.Cells[0, 19].ColumnWidth = 400;
                                        wsDl.Cells[1, 19].ColumnWidth = 400;
                                        wsDl.Cells[i + 2, 19].Value = OrganizationChartEmployeeList[i].MonEndTime;
                                        wsDl.Cells[0, 20].ColumnWidth = 400;
                                        wsDl.Cells[1, 20].ColumnWidth = 400;
                                        wsDl.Cells[i + 2, 20].Value = OrganizationChartEmployeeList[i].MonBreakTime;
                                        wsDl.Cells[i + 2, 21].Value = OrganizationChartEmployeeList[i].TotalWorkingHours;
                                        wsDl.Cells[i + 2, 22].Value = OrganizationChartEmployeeList[i].BackupEmployeeCode;
                                    }

                                    Worksheet wsGeneralInfo = workbook.Worksheets[3];
                                    wsGeneralInfo.Cells[1, 1].Value = 0;
                                    wsGeneralInfo.Cells[2, 1].Value = 0;
                                    wsGeneralInfo.Cells[3, 1].Value = 0;
                                    wsGeneralInfo.Cells[4, 1].Value = 0;

                                    for (int i = 0; i < DepartmentAreaAverageList.Count; i++)
                                    {
                                        if (DepartmentAreaAverageList[i].IdLookupValue == 126)
                                        {
                                            wsGeneralInfo.Cells[1, 1].NumberFormat = cellPercentageFormat;
                                            wsGeneralInfo.Cells[1, 1].Value = (double)DepartmentAreaAverageList[i].Average / 100;
                                        }
                                        else if (DepartmentAreaAverageList[i].IdLookupValue == 127)
                                        {
                                            wsGeneralInfo.Cells[2, 1].NumberFormat = cellPercentageFormat;
                                            wsGeneralInfo.Cells[2, 1].Value = (double)DepartmentAreaAverageList[i].Average / 100;
                                        }
                                        else if (DepartmentAreaAverageList[i].IdLookupValue == 128)
                                        {
                                            wsGeneralInfo.Cells[3, 1].NumberFormat = cellPercentageFormat;
                                            wsGeneralInfo.Cells[3, 1].Value = (double)DepartmentAreaAverageList[i].Average / 100;
                                        }
                                        else if (DepartmentAreaAverageList[i].IdLookupValue == 129)
                                        {
                                            wsGeneralInfo.Cells[4, 1].NumberFormat = cellPercentageFormat;
                                            wsGeneralInfo.Cells[4, 1].Value = (double)DepartmentAreaAverageList[i].Average / 100;
                                        }
                                    }

                                    if (PlantWorkHourslist != null && PlantWorkHourslist.Count > 0)
                                    {
                                        if (PlantWorkHourslist.Any(x => x.Key == "Week"))
                                        {
                                            wsGeneralInfo.Cells[5, 1].Value = PlantWorkHourslist.Where(x => x.Key == "Week").FirstOrDefault().Value;
                                        }
                                        if (PlantWorkHourslist.Any(x => x.Key == "Day"))
                                        {
                                            wsGeneralInfo.Cells[6, 1].Value = PlantWorkHourslist.Where(x => x.Key == "Day").FirstOrDefault().Value;
                                        }
                                        if (PlantWorkHourslist.Any(x => x.Key == "Plant_CNC_N_Shifts"))
                                        {
                                            wsGeneralInfo.Cells[10, 1].Value = PlantWorkHourslist.Where(x => x.Key == "Plant_CNC_N_Shifts").FirstOrDefault().Value;
                                        }
                                    }

                                    wsGeneralInfo.Cells[7, 1].Value = 0;
                                    wsGeneralInfo.Cells[8, 1].Value = 0;
                                    wsGeneralInfo.Cells[9, 1].Value = 0;
                                    wsGeneralInfo.Cells[11, 1].Value = 0;
                                    wsGeneralInfo.Cells[12, 1].Value = 0;
                                    wsGeneralInfo.Cells[13, 1].Value = 0;

                                    if (CompanyDepartments != null && CompanyDepartments.Count > 0)
                                    {
                                        foreach (var item in CompanyDepartments)
                                        {
                                            if (item.IdDepartment == 58) // CAD
                                            {
                                                wsGeneralInfo.Cells[7, 1].Value = item.NumberOfWorkstations;
                                                continue;
                                            }
                                            else if (item.IdDepartment == 61) // CAM
                                            {
                                                wsGeneralInfo.Cells[8, 1].Value = item.NumberOfWorkstations;
                                                continue;
                                            }
                                            else if (item.IdDepartment == 59) // CNC
                                            {
                                                wsGeneralInfo.Cells[9, 1].Value = item.NumberOfWorkstations;
                                                continue;
                                            }
                                        }
                                    }

                                    //Plant Land
                                    wsGeneralInfo.Cells[11, 1].Value = CompanySize;

                                    if (CompanyDepartmentAreas != null && CompanyDepartmentAreas.Count > 0)
                                    {
                                        foreach (var item in CompanyDepartmentAreas)
                                        {
                                            if (item.IdDepartmentArea == 129) //Plant Office
                                            {
                                                wsGeneralInfo.Cells[12, 1].Value = item.Size;
                                                continue;
                                            }
                                            else if (item.IdDepartmentArea == 127) //Plant Production
                                            {
                                                wsGeneralInfo.Cells[13, 1].Value = item.Size;
                                                continue;
                                            }
                                        }
                                    }

                                    workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                                }

                                System.Diagnostics.Process.Start(excelFilePath);

                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            }
                        }
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["OrganizationChartFailedExportMessage"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method OrganizationChartExportToExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (System.IO.IOException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in OrganizationChartExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrganizationChartExportToExcel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OrganizationChartExportToExcel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in OrganizationChartExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                workbook.Dispose();
                if (stream != null)
                    stream.Dispose();
            }

        }
        public void StartPleaseWaitIndicator()
        {

            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = System.Windows.ResizeMode.NoResize,
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
        }
        private void SetUserPermission()
        {
            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    AddShiftEnabled = true;
                    break;

                case PermissionManagement.Admin:
                    AddShiftEnabled = true;
                    break;

                case PermissionManagement.PlantViewer:
                    AddShiftEnabled = false;
                    break;

                case PermissionManagement.GlobalViewer:
                    AddShiftEnabled = false;
                    break;

                default:
                    AddShiftEnabled = false;
                    break;
            }
        }
        #endregion

    }
}


