using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Sprint 41--HRM	M041-08	New configuration section Job Descriptions---sdesai
/// </summary>
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeJobDescriptionsViewModel : INotifyPropertyChanged
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private JobDescription selectedJobDescription;
        private ObservableCollection<JobDescription> jobDescriptionList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public JobDescription SelectedJobDescription
        {
            get { return selectedJobDescription; }
            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
            }
        }

        public ObservableCollection<JobDescription> JobDescriptionList
        {
            get { return jobDescriptionList; }
            set
            {
                jobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionList"));
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

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events

        #region Public ICommand
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand EditJobDescriptionListDoubleClickCommand { get; set; }
        public ICommand AddNewJobDescriptionCommand { get; set; }
        public ICommand EditJobDescriptionDoubleClickCommand { get; set; }
        public ICommand DocumentViewCommand { get; set; }
        #endregion

        #region Constructor
        public EmployeeJobDescriptionsViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeJobDescriptionsViewModel()...", category: Category.Info, priority: Priority.Low);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintJobDescriptionsList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportJobDescriptionsList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshJobDescriptionsList));
                EditJobDescriptionListDoubleClickCommand = new DelegateCommand<object>(EditJobDescriptionInformation);
                AddNewJobDescriptionCommand = new RelayCommand(new Action<object>(AddNewJobDescription));
                EditJobDescriptionDoubleClickCommand = new RelayCommand(new Action<object>(EditJobDescriptionInformation));
                DocumentViewCommand = new RelayCommand(new Action<object>(BrowseJobDescriptionFile));
                GeosApplication.Instance.Logger.Log("Constructor EmployeeJobDescriptionsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor EmployeeJobDescriptionsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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

                //JobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetAllJobDescriptions_V2040());
                JobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetAllJobDescriptions_V2046());
                SelectedJobDescription = JobDescriptionList.First();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Open PDF in another window
        /// </summary>
        /// <param name="obj"></param>
        private void BrowseJobDescriptionFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BrowseJobDescriptionFile()...", category: Category.Info, priority: Priority.Low);

                EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                string EmployeeCode = string.Empty;
                employeeDocumentViewModel.OpenPdfByEmployeeCode(EmployeeCode, obj);
                employeeDocumentView.DataContext = employeeDocumentViewModel;
                employeeDocumentView.Show();

                GeosApplication.Instance.Logger.Log("Method BrowseJobDescriptionFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BrowseJobDescriptionFile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BrowseJobDescriptionFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseJobDescriptionFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method Refresh Job Descriptions List
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshJobDescriptionsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshJobDescriptionsList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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

                //JobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetAllJobDescriptions_V2040());
                JobDescriptionList = new ObservableCollection<JobDescription>(HrmService.GetAllJobDescriptions_V2046());
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshJobDescriptionsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshJobDescriptionsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Export Job Descriptions List
        /// </summary>
        /// <param name="obj"></param>
        private void ExportJobDescriptionsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportJobDescriptionsList ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "JobDescriptions";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
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
                    ResultFileName = (saveFile.FileName);
                    TableView JobDescriptionsTableView = ((TableView)obj);
                    JobDescriptionsTableView.ShowTotalSummary = false;
                    JobDescriptionsTableView.ShowFixedTotalSummary = false;
                    JobDescriptionsTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    JobDescriptionsTableView.ShowTotalSummary = false;
                    JobDescriptionsTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportJobDescriptionsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportJobDescriptionsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Print Job Descriptions List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintJobDescriptionsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintJobDescriptionsList()...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["JobDescriptionReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["JobDescriptionReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintJobDescriptionsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintJobDescriptionsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Edit Job Description Information
        /// [001][spawar][04-03-2020][GEOS2-1918] Job Description Configuration</para>
        /// [002][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        /// <param name="obj"></param>
        private void EditJobDescriptionInformation(object obj)
         {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditJobDescriptionInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                JobDescription jobDescription = (JobDescription)detailView.FocusedRow;
                SelectedJobDescription = jobDescription;
                if (jobDescription != null)
                {
                    AddNewJobDescriptionsView addNewJobDescriptionsView = new AddNewJobDescriptionsView();
                    AddNewJobDescriptionsViewModel addNewJobDescriptionsViewModel = new AddNewJobDescriptionsViewModel();
                    EventHandler handle = delegate { addNewJobDescriptionsView.Close(); };
                    addNewJobDescriptionsViewModel.RequestClose += handle;
                    addNewJobDescriptionsView.DataContext = addNewJobDescriptionsViewModel;
                    //addNewJobDescriptionsViewModel.EditInit(JobDescriptionList, jobDescription);
                    addNewJobDescriptionsViewModel.IsHealthAndSafetyVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4459]
                    addNewJobDescriptionsViewModel.IsEquipmentAndToolsVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-5549]

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addNewJobDescriptionsViewModel.InitReadOnly(jobDescription);
                    else
                        addNewJobDescriptionsViewModel.EditInit(JobDescriptionList, jobDescription);

                    addNewJobDescriptionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditJobDescriptionInformation").ToString();
                    addNewJobDescriptionsViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addNewJobDescriptionsView.Owner = Window.GetWindow(ownerInfo);
                    addNewJobDescriptionsView.ShowDialog();

                    if (addNewJobDescriptionsViewModel.IsSave)
                    {
                        jobDescription.JobDescriptionCode = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionCode;
                        jobDescription.OldJobDescriptionCode = addNewJobDescriptionsViewModel.UpdateJobDescription.OldJobDescriptionCode;
                        jobDescription.JobDescriptionTitle = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionTitle;
                        jobDescription.Abbreviation = addNewJobDescriptionsViewModel.UpdateJobDescription.Abbreviation;
                        jobDescription.ParentJobDescription = addNewJobDescriptionsViewModel.UpdateJobDescription.ParentJobDescription;
                        jobDescription.IdDepartment = addNewJobDescriptionsViewModel.UpdateJobDescription.IdDepartment;
                        jobDescription.Department = addNewJobDescriptionsViewModel.UpdateJobDescription.Department;
                        jobDescription.JobDescriptionFileName = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionFileName;
                        jobDescription.IsJobDescriptionFileUpdated = addNewJobDescriptionsViewModel.UpdateJobDescription.IsJobDescriptionFileUpdated;
                        jobDescription.JobDescriptionFileInBytes = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionFileInBytes;
                        jobDescription.JobDescriptionInUse = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionInUse;
                        jobDescription.IdParent = addNewJobDescriptionsViewModel.UpdateJobDescription.IdParent;
                        jobDescription.IdJDLevel = addNewJobDescriptionsViewModel.UpdateJobDescription.IdJDLevel; // [01] added
                        jobDescription.JDLevel = addNewJobDescriptionsViewModel.UpdateJobDescription.JDLevel;
                        //sjadhav
                        jobDescription.IdJdScope= addNewJobDescriptionsViewModel.UpdateJobDescription.IdJdScope; 
                        jobDescription.JDScope= addNewJobDescriptionsViewModel.UpdateJobDescription.JDScope;
                        jobDescription.JobDescriptionIsMandatory = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionIsMandatory;//[001]
                        SelectedJobDescription = jobDescription;
                        jobDescription.JobDescriptionIsRemote = addNewJobDescriptionsViewModel.UpdateJobDescription.JobDescriptionIsRemote; //[002]
                    }
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.RefreshData();
                    ((DevExpress.Xpf.Grid.TableView)obj).DataControl.UpdateLayout();
                }


                GeosApplication.Instance.Logger.Log("Method EditJobDescriptionInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditJobDescriptionInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Add New Job Description
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewJobDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewJobDescription()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                AddNewJobDescriptionsView addNewJobDescriptionsView = new AddNewJobDescriptionsView();
                AddNewJobDescriptionsViewModel addNewJobDescriptionsViewModel = new AddNewJobDescriptionsViewModel();
                EventHandler handle = delegate { addNewJobDescriptionsView.Close(); };
                addNewJobDescriptionsViewModel.RequestClose += handle;
                addNewJobDescriptionsView.DataContext = addNewJobDescriptionsViewModel;
                addNewJobDescriptionsViewModel.IsHealthAndSafetyVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4459]
                addNewJobDescriptionsViewModel.IsEquipmentAndToolsVisible = Visibility.Collapsed;//[Sudhir.Jangra][GEOS2-4459]

                addNewJobDescriptionsViewModel.Init(JobDescriptionList);
                addNewJobDescriptionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddJobDescriptionInformation").ToString();
                addNewJobDescriptionsViewModel.IsNew = true;
                var ownerInfo = (detailView as FrameworkElement);
                addNewJobDescriptionsView.Owner = Window.GetWindow(ownerInfo);
                addNewJobDescriptionsView.ShowDialog();

                if (addNewJobDescriptionsViewModel.IsSave)
                {
                    JobDescriptionList.Add(addNewJobDescriptionsViewModel.NewJobDescription);
                    SelectedJobDescription = addNewJobDescriptionsViewModel.NewJobDescription;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        //----------------------mazhar-------------------------------//
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
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
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
