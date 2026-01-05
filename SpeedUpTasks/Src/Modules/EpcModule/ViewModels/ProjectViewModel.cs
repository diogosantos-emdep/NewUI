using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ProjectViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IEpcService epcControl;

        #endregion

        #region Collections

        private object btnScopeContent;
        public object BtnScopeContent
        {
            get { return btnScopeContent; }
            set
            {
                SetProperty(ref btnScopeContent, value, () => BtnScopeContent);
            }
        }

        private object btnWBSContent;
        public object BtnWBSContent
        {
            get { return btnWBSContent; }
            set
            {
                SetProperty(ref btnWBSContent, value, () => BtnWBSContent);
            }
        }

        private ObservableCollection<Product> productList;
        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set
            {
                SetProperty(ref productList, value, () => ProductList);
            }
        }

        private ObservableCollection<Customer> projectCustomerList;
        public ObservableCollection<Customer> ProjectCustomerList
        {
            get { return projectCustomerList; }
            set
            {
                SetProperty(ref projectCustomerList, value, () => ProjectCustomerList);
            }
        }

        private ObservableCollection<User> projectOwnerList;
        public ObservableCollection<User> ProjectOwnerList
        {
            get { return projectOwnerList; }
            set
            {
                SetProperty(ref projectOwnerList, value, () => ProjectOwnerList);
            }
        }

        private ObservableCollection<Data.Common.Epc.LookupValue> projectPriorityList;
        public ObservableCollection<Data.Common.Epc.LookupValue> ProjectPriorityList
        {
            get { return projectPriorityList; }
            set
            {
                SetProperty(ref projectPriorityList, value, () => ProjectPriorityList);
            }
        }

        private ObservableCollection<LookupValue> projectAnalysisPriorityList;
        public ObservableCollection<LookupValue> ProjectAnalysisPriorityList
        {
            get { return projectAnalysisPriorityList; }
            set
            {
                SetProperty(ref projectAnalysisPriorityList, value, () => ProjectAnalysisPriorityList);
            }
        }

        private ObservableCollection<LookupValue> projectAnalysisStatusList;
        public ObservableCollection<LookupValue> ProjectAnalysisStatusList
        {
            get { return projectAnalysisStatusList; }
            set
            {
                SetProperty(ref projectAnalysisStatusList, value, () => ProjectAnalysisStatusList);
            }
        }

        private ObservableCollection<Data.Common.Epc.LookupValue> projectStatusList;
        public ObservableCollection<Data.Common.Epc.LookupValue> ProjectStatusList
        {
            get { return projectStatusList; }
            set
            {
                SetProperty(ref projectStatusList, value, () => ProjectStatusList);
            }
        }

        private ObservableCollection<Data.Common.Epc.LookupValue> projectCategoryList;
        public ObservableCollection<Data.Common.Epc.LookupValue> ProjectCategoryList
        {
            get { return projectCategoryList; }
            set
            {
                SetProperty(ref projectCategoryList, value, () => ProjectCategoryList);
            }
        }

        private ObservableCollection<GeosStatus> projectGeosStatusList;
        public ObservableCollection<GeosStatus> ProjectGeosStatusList
        {
            get { return projectGeosStatusList; }
            set
            {
                SetProperty(ref projectGeosStatusList, value, () => ProjectGeosStatusList);
            }
        }

        private ObservableCollection<Data.Common.Epc.ProjectTask> projectTaskList = new ObservableCollection<Data.Common.Epc.ProjectTask>();
        private ObservableCollection<TaskWorkingTime> userWorkedHours = new ObservableCollection<TaskWorkingTime>();
        private ObservableCollection<DataHelper> departmentHoursList = new ObservableCollection<DataHelper>();

        public ObservableCollection<Data.Common.Epc.ProjectTask> ProjectTaskList
        {
            get { return projectTaskList; }
            set
            {
                SetProperty(ref projectTaskList, value, () => ProjectTaskList);
            }
        }

        public ObservableCollection<TaskWorkingTime> UserWorkedHours
        {
            get { return userWorkedHours; }
            set
            {
                SetProperty(ref userWorkedHours, value, () => UserWorkedHours);
            }
        }

        public ObservableCollection<DataHelper> DepartmentHoursList
        {
            get { return departmentHoursList; }
            set
            {
                SetProperty(ref departmentHoursList, value, () => DepartmentHoursList);
            }
        }

        #endregion

        #region Properties

        private Project projectData;
        public Project ProjectData
        {
            get { return projectData; }
            set
            {
                SetProperty(ref projectData, value, () => ProjectData);
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                SetProperty(ref selectedProduct, value, () => SelectedProduct);
            }
        }

        private ObservableCollection<Project> projectList = new ObservableCollection<Project>();
        public ObservableCollection<Project> ProjectList
        {
            get { return projectList; }
            set
            {
                SetProperty(ref projectList, value, () => ProjectList);
            }
        }

        private ICollectionView groupProduct;
        public ICollectionView GroupProduct
        {
            get { return groupProduct; }
            set
            {
                SetProperty(ref groupProduct, value, () => GroupProduct);
            }
        }

        private ObservableCollection<Team> projectTeamList = new ObservableCollection<Team>();
        public ObservableCollection<Team> ProjectTeamList
        {
            get { return projectTeamList; }
            set
            {
                SetProperty(ref projectTeamList, value, () => ProjectTeamList);
            }
        }

        private ICollectionView groupTeam;
        public ICollectionView GroupTeam
        {
            get { return groupTeam; }
            set
            {
                SetProperty(ref groupTeam, value, () => GroupTeam);
            }
        }

        private List<object> selectedTeamList;
        public List<object> SelectedTeamList
        {
            get { return selectedTeamList; }
            set
            {
                SetProperty(ref selectedTeamList, value, () => SelectedTeamList);
            }
        }

        #endregion

        #region Commands

        public ICommand OpenFolderCommand { get; set; }
        public ICommand IsValidatedCheckedCommand { get; set; }
        public ICommand EditKickoffSoftwareAreaButtonCommand { get; set; }
        public ICommand EditKickoffElectronicAreaButtonCommand { get; set; }
        public ICommand EditKickoffSupportAreaButtonCommand { get; set; }
        public ICommand EditCloseSoftwareAreaButtonCommand { get; set; }
        public ICommand EditCloseElectronicAreaButtonCommand { get; set; }
        public ICommand EditCloseSupportAreaButtonCommand { get; set; }
        public ICommand EditScopeButtonCommand { get; set; }
        public ICommand EditWbsButtonCommand { get; set; }
        public ICommand SetWatcherCommand { get; set; }
        public ICommand AddNewMilestoneCommand { get; set; }
        public ICommand DeleteMilestoneAttemptCommand { get; set; }
        public ICommand AddNewFollowupCommand { get; set; }
        public ICommand DeleteFollowupRowCommand { get; set; }
        public ICommand SelectedProductChangedCommand { get; set; }

        #endregion

        #region Constructor

        public ProjectViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));

            OpenFolderCommand = new DelegateCommand<string>(OpenFolderCommandAction);
            SetWatcherCommand = new Prism.Commands.DelegateCommand<MouseButtonEventArgs>(SetCommandAction);
            AddNewMilestoneCommand = new DelegateCommand<object>(AddNewMilestoneCommandAction);
            DeleteMilestoneAttemptCommand = new DelegateCommand<RoutedEventArgs>(DeleteMilestoneAttemptCommandAction);
            AddNewFollowupCommand = new DelegateCommand<object>(AddNewFollowupCommandAction);
            DeleteFollowupRowCommand = new DelegateCommand<object>(DeleteFollowupRowCommandAction);
            SelectedProductChangedCommand = new DelegateCommand<object>(SelectedProductChangedCommandAction);

            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 1, ParentID = 0, Items = "Management" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 2, ParentID = 1, Items = "Add New Test", Duration = "03:00", Worked = "01:30", DeliveryDate = new DateTime(2016, 04, 13), Progress = .5f, Team = "fpinas" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 3, ParentID = 0, Items = "Analysis" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 4, ParentID = 3, Items = "Wintestm test is not working", Duration = "33:00", Worked = "25:30", DeliveryDate = new DateTime(2016, 06, 22), Progress = .6f, Team = "jordi" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 5, ParentID = 0, Items = "Development" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 6, ParentID = 5, Items = "Wintestm test is not working", Duration = "04:00", Worked = "09:00", DeliveryDate = new DateTime(2016, 02, 13), Progress = .7f, Team = "fpinas" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 7, ParentID = 0, Items = "Validation" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 8, ParentID = 7, Items = "Wintestm test is not working", Duration = "13:00", Worked = "12:30", DeliveryDate = new DateTime(2016, 12, 13), Progress = .8f, Team = "lsharma" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 9, ParentID = 0, Items = "Documentation" });
            //ListProjectDevelopment.Add(new ProjectDevelopment() { ID = 10, ParentID = 9, Items = "Wintestm test is not working", Duration = "13:00", Worked = "12:30", DeliveryDate = new DateTime(2016, 12, 13), Progress = .8f, Team = "lsharma" });
            // ListFollowUp.Add(new ProjectFollowUp() { User = "fpinas", FollowupDate = new DateTime(2016, 04, 25), Description = "Ooops!" });
            //IsValidatedCheckedCommand = new RelayCommand(IsValidatedChecked);

            EditKickoffSoftwareAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.KickoffSoftwareAreaView", null, this); });
            EditKickoffElectronicAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.KickoffElectronicAreaView", null, this); });
            EditKickoffSupportAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.KickoffSupportAreaView", null, this); });
            EditCloseSoftwareAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.CloseSoftwareAreaView", null, this); });
            EditCloseElectronicAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.CloseElectronicAreaView", null, this); });
            EditCloseSupportAreaButtonCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.CloseSupportAreaView", null, this); });

            EditWbsButtonCommand = new DelegateCommand(() =>
            {
                Service.Navigate("Emdep.Geos.Modules.Epc.Views.WbsView", null, this);
            });

            EditScopeButtonCommand = new DelegateCommand(() =>
            {
                Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProjectScopeView", ProjectData, this);
            });

            //DevelopmentRowDoubleClickCommand = new DelegateCommand<object>(ExecuteRowDoubleClickCommand);
        }

        public void OpenFolderCommandAction(string projectPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(projectPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = projectPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("OpenFolder").ToString(), "Transparent", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        public void SetCommandAction(MouseButtonEventArgs e)
        {
            TreeListViewHitInfo hitInfo = ((TreeListView)e.Source).CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hitInfo.HitTest == TreeListViewHitTest.RowIndicator)
            {
                GridControl grid = ((GridControl)((TreeListView)e.Source).Parent);
                ProjectTask task = (ProjectTask)grid.GetRow(hitInfo.RowHandle);
                if (task.IdTask > 0)
                {
                    if (task.TaskWatchers.Count > 0)
                    {
                        epcControl.DeleteTaskWatcherById(task.TaskWatchers[0].IdTaskWatcher);
                        task.TaskWatchers.Remove(task.TaskWatchers[0]);
                    }
                    else
                    {
                        TaskWatcher watcher = new TaskWatcher() { IdTask = task.IdTask, IdUser = GeosApplication.Instance.ActiveUser.IdUser };
                        TaskWatcher taskwatcher = epcControl.AddTaskWatcher(watcher);
                        if (taskwatcher.IdTaskWatcher != 0)
                        {
                            task.TaskWatchers = new ObservableCollection<TaskWatcher>();
                            task.TaskWatchers.Add(taskwatcher);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods
        public void Init()
        {
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTSTATUS"))
            {
                ProjectStatusList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTSTATUS"];
            }
            else
            {
                ProjectStatusList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(2).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTSTATUS", ProjectStatusList);
            }

            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTCATEGORIES"))
            {
                ProjectCategoryList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTCATEGORIES"];
            }
            else
            {
                ProjectCategoryList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(3).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTCATEGORIES", ProjectCategoryList);
            }

            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTPRIORITY"))
            {
                ProjectPriorityList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTPRIORITY"];
            }
            else
            {
                ProjectPriorityList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(1).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTPRIORITY", ProjectPriorityList);
            }

            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECT_ANALYSIS_PRIORITY"))
            {
                ProjectAnalysisPriorityList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECT_ANALYSIS_PRIORITY"];
            }
            else
            {
                ProjectAnalysisPriorityList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(22).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_PROJECT_ANALYSIS_PRIORITY", ProjectAnalysisPriorityList);
            }

            ProjectAnalysisStatusList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(11).AsEnumerable());
            ProjectCustomerList = new ObservableCollection<Customer>(epcControl.GetCustomers().AsEnumerable());
            ProjectOwnerList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());
            ProjectGeosStatusList = new ObservableCollection<GeosStatus>(epcControl.GetGeosOfferStatus().AsEnumerable());

            ProjectData = epcControl.GetProjectByProjectId(this.ProjectData.IdProject);

            if (ProjectData.ProjectAnalysis != null && ProjectData.ProjectAnalysis.Count > 0)
            {
                if (ProjectData.ProjectAnalysis[0].IsScope)
                {
                    BtnScopeContent = "Edit";
                }
                else
                {
                    BtnScopeContent = "Add";
                }

                if (ProjectData.ProjectAnalysis[0].IsWBS)
                {
                    BtnWBSContent = "Edit";
                }
                else
                {
                    BtnWBSContent = "Add";
                }
            }

            List<Product> _productlist = epcControl.GetAllProducts().ToList();
            _productlist.RemoveAll(x => x.Childrens != null);

            ProductList = new ObservableCollection<Product>(_productlist);
            GroupProduct = CollectionViewSource.GetDefaultView(ProductList);
            GroupProduct.GroupDescriptions.Add(new PropertyGroupDescription("ParentProduct.ProductName"));
            ProjectOwnerList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());

            List<Team> list = epcControl.GetTeams().ToList();
            list.RemoveAll(x => x.IdParent == null);
            ProjectTeamList = new ObservableCollection<Team>(list);
            GroupTeam = CollectionViewSource.GetDefaultView(ProjectTeamList);
            GroupTeam.GroupDescriptions.Add(new PropertyGroupDescription("ParentTeam.Name"));

            List<ProjectTeam> SelectedProjectTeamList = new List<ProjectTeam>(epcControl.GetProjectTeams(this.ProjectData.IdProject));

            SelectedTeamList = new List<object>();
            foreach (var item in SelectedProjectTeamList)
            {
                object obj = (object)ProjectTeamList.FirstOrDefault(x => x.ParentChildName == item.Team.ParentChildName);
                if (obj != null)
                {
                    SelectedTeamList.Add(obj);
                }
            }

            SelectedProduct = ProductList.FirstOrDefault(x => x.IdProduct == ProjectData.IdProduct);

            LoadDevelopmentData();
        }

        private void LoadDevelopmentData()
        {
            int index = 0;
            int secondIndex = -25;
            
            // taskTypes like Management, Analysis, Development, Validation, Documentation.
            
            var taskTypes = epcControl.GetLookupValues(12).AsEnumerable();

            Dictionary<int, float> userAndPlannedHours = new Dictionary<int, float>();

            IList<TaskWorkingTime> projectWorkingTimeList = epcControl.GetProjectWorkingTimeByProjectId(ProjectData.IdProject);

            // Main Grid Development.
            foreach (var taskType in taskTypes)
            {
                ProjectTask rootProjectTask = new Data.Common.Epc.ProjectTask() { IdTask = --index, IdTaskType = taskType.IdLookupValue, TaskTitle = taskType.Value, ParentTask = null };
                ProjectTaskList.Add(rootProjectTask);

                // Planned Hours for Task types like Management, Analysis, Development, Validation, Documentation.
                float taskTypePlannedhours = 0.0f;
                float taskTypeWorkedhours = 0.0f;

                foreach (ProjectTask projectTask in ProjectData.ProjectTasks)
                {
                    if (projectTask.IdParentTask.HasValue && projectTask.IdParentTask > 0 && projectTask.IdTaskType == taskType.IdLookupValue)
                    {
                        projectTask.IdParentTask = index;
                        ProjectTaskList.Add(projectTask);

                        taskTypePlannedhours = taskTypePlannedhours + projectTask.PlannedHours;
                        taskTypeWorkedhours = taskTypeWorkedhours + projectTask.WorkedHours;

                        foreach (var taskUser in projectTask.TaskUsers)
                        {
                            ProjectTask user = new Data.Common.Epc.ProjectTask() { IdTask = --secondIndex, IdTaskType = projectTask.IdTaskType, Efficiency = 0.0f, TaskTitle = taskUser.User.FullName, IdParentTask = projectTask.IdTask };
                            ProjectTaskList.Add(user);

                            TaskWorkingTime taskWorkingTime = projectWorkingTimeList.FirstOrDefault(x => x.IdTask == projectTask.IdTask && x.IdUser == taskUser.IdUser);

                            if (taskWorkingTime != null)
                            {
                                user.WorkedHours = taskWorkingTime.WorkingTimeInHours;
                            }

                            if (projectTask.PlannedHours != 0)
                            {
                                user.Efficiency = (float)(Math.Round(((projectTask.PlannedHours / user.WorkedHours) * 100), 2));
                            }

                            if (projectTask.WorkedHours == 0)
                            {
                                user.Efficiency = 0;
                            }

                            if (user.Efficiency > 120)
                            {
                                user.Efficiency = 120;
                            }

                            // User Worked Hours.
                            if (!userAndPlannedHours.ContainsKey(taskUser.IdUser))
                            {
                                userAndPlannedHours.Add(taskUser.IdUser, projectTask.PlannedHours);
                            }
                            else
                            {
                                userAndPlannedHours[taskUser.IdUser] += projectTask.PlannedHours;
                            }
                        }

                        if (projectTask.PlannedHours != 0)
                        {
                            projectTask.Efficiency = (float)(Math.Round(((projectTask.PlannedHours / projectTask.WorkedHours) * 100), 2));
                        }

                        if (projectTask.WorkedHours == 0)
                        {
                            projectTask.Efficiency = 0;
                        }

                        if (projectTask.Efficiency > 120)
                        {
                            projectTask.Efficiency = 120;
                        }

                        //if (projectTask.TaskWatchers.Count > 0)
                        //{
                        //    ObservableCollection<TaskWatcher> listTaskWatcher = new ObservableCollection<TaskWatcher>(projectTask.TaskWatchers);
                        //    projectTask.TaskWatchers = listTaskWatcher;

                        //    //foreach (var item3 in item2.TaskWatchers.ToList())
                        //    //{
                        //    //    //if (item3.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
                        //    //    //{
                        //    //    //    item2.TaskWatchers.Remove(item3);
                        //    //    //}
                        //    //}
                        //}
                    }
                }

                rootProjectTask.PlannedHours = taskTypePlannedhours;
                rootProjectTask.WorkedHours = taskTypeWorkedhours;

                if (rootProjectTask.PlannedHours != 0)
                {
                    rootProjectTask.Efficiency = (float)(Math.Round(((rootProjectTask.PlannedHours / rootProjectTask.WorkedHours) * 100), 2));
                }

                if (rootProjectTask.WorkedHours == 0)
                {
                    rootProjectTask.Efficiency = 0;
                }

                if (rootProjectTask.Efficiency > 120)
                {
                    rootProjectTask.Efficiency = 120;
                }
            }

            // User Worked Hours.
            UserWorkedHours = new ObservableCollection<TaskWorkingTime>();
            foreach (var item in projectWorkingTimeList)
            {
                var user = UserWorkedHours.FirstOrDefault(x => x.IdUser == item.IdUser);
                if (user != null)
                {
                    user.WorkingTimeInHours += item.WorkingTimeInHours;
                }
                else
                {
                    UserWorkedHours.Add(item);
                }
            }

            // Department Worked Hours.
            // FloatValue1-Planned Hours.
            // FloatValue2-Worked Hours.
            // FloatValue3-Efficiency.

            var teams = epcControl.GetTeams();
            DepartmentHoursList.Clear();

            foreach (var team in teams)
            {
                DataHelper data = new DataHelper() { ByteValue = team.IdTeam, Name = team.ParentChildName };
                var v = team.UserTeams.FirstOrDefault(x => userAndPlannedHours.ContainsKey(x.IdUser));

                if (v != null)
                {
                    foreach (var userHour in userAndPlannedHours)
                    {
                        int r = team.UserTeams.Count(x => x.IdUser == userHour.Key);

                        if (r > 0)
                        {
                            data.FloatValue1 += userAndPlannedHours[userHour.Key];
                        }

                        var xx = UserWorkedHours.Where(x => x.IdUser == userHour.Key).Sum(x => x.WorkingTimeInHours);
                        data.FloatValue2 += xx;
                    }

                    if (data.FloatValue1 != 0)
                    {
                        data.FloatValue3 = (float)(Math.Round(((data.FloatValue1 / data.FloatValue2) * 100), 2));
                    }

                    if (data.FloatValue3 > 100)
                    {
                        data.ByteValue = 3;
                    }
                    else if (data.FloatValue3 < 100 && data.FloatValue3 > 0)
                    {
                        data.ByteValue = 1;
                    }

                    if (data.FloatValue3 > 120)
                    {
                        data.FloatValue3 = 120;
                    }

                    DepartmentHoursList.Add(data);
                }
            }
        }

        public void AddNewMilestoneCommandAction(object obj)
        {
            NewMilestoneView newMilestoneView = new NewMilestoneView();
            NewMilestoneViewModel newMilestoneViewModel = new ViewModels.NewMilestoneViewModel();

            ProjectMilestone newProjectMilestone = new ProjectMilestone()
            {
                IdProject = this.ProjectData.IdProject,
                CreationDate = DateTime.Now,
                TargetDate = DateTime.Now,
                IdProjectMilestoneStatus = 92
            };
            ((ISupportParameter)newMilestoneViewModel).Parameter = newProjectMilestone;

            EventHandler handle = delegate { newMilestoneView.Close(); };
            newMilestoneViewModel.RequestClose += handle;
            newMilestoneView.DataContext = newMilestoneViewModel;
            newMilestoneView.ShowDialogWindow();

            if (newMilestoneViewModel.ISave)
            {
                this.ProjectData.ProjectMilestones.Add(newMilestoneViewModel.ProjectMilestoneData);
            }
        }

        public void DeleteMilestoneAttemptCommandAction(RoutedEventArgs eventObject)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteMilestone").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                ProjectMilestoneDate projectMilestoneDate = (ProjectMilestoneDate)((ListBoxEdit)(((FrameworkElement)(eventObject.Source)).DataContext)).SelectedItem;

                bool isdeleted = epcControl.DeleteProjectMilestoneDateById(projectMilestoneDate.IdProjectMilestoneDate);

                ProjectMilestone projectMilestone = projectMilestoneDate.ProjectMilestone;
                if (projectMilestone.ProjectMilestoneDates != null)
                {
                    projectMilestone.ProjectMilestoneDates.Remove(projectMilestoneDate);
                }
            }
        }


        public void AddNewFollowupCommandAction(object obj)
        {
            NewFollowupView newFollowupView = new NewFollowupView();
            NewFollowupViewModel newFollowupViewModel = new NewFollowupViewModel();

            ProjectFollowup newProjectFollowup = new ProjectFollowup()
            {
                IdProject = this.ProjectData.IdProject,
                FollowupDate = DateTime.Now,
                IdUser = GeosApplication.Instance.ActiveUser.IdUser
            };
            ((ISupportParameter)newFollowupViewModel).Parameter = newProjectFollowup;

            EventHandler handle = delegate { newFollowupView.Close(); };
            newFollowupViewModel.RequestClose += handle;
            newFollowupView.DataContext = newFollowupViewModel;
            newFollowupView.ShowDialogWindow();

            if (newFollowupViewModel.ISave)
            {
                this.ProjectData.ProjectFollowups.Add(newFollowupViewModel.ProjectFollowupData);
            }
        }

        public void DeleteFollowupRowCommandAction(object obj)
        {
            
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.FindResource("DeleteFollowup").ToString()), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteProjectFollowupById(((ProjectFollowup)obj).IdProjectFollowup);
                ProjectData.ProjectFollowups.Remove((ProjectFollowup)obj);
            }
        }

        public void SelectedProductChangedCommandAction(object obj)
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.IdProduct != ProjectData.IdProduct)
                {
                    bool isupdated = epcControl.UpdateProjectProduct(ProjectData.IdProject, SelectedProduct.IdProduct);
                    if (isupdated)
                    {
                        CustomMessageBox.Show(String.Format(System.Windows.Application.Current.FindResource("ProjectMoveAfter").ToString(), ProjectData.ProjectCode, SelectedProduct.ProductName),
                                              "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        SelectedProduct = null;
                        ProjectList.Remove(ProjectList.FirstOrDefault(x => x.IdProject == ProjectData.IdProject));
                        ProjectData = null;
                    }
                }
            }
        }

        void ExecuteRowDoubleClickCommand(object row)
        {
        }

        private ProjectViewModel projectVM;
        public ProjectViewModel ProjectVM
        {
            get { return projectVM; }
            set
            {
                SetProperty(ref projectVM, value, () => ProjectVM);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            this.ProjectVM = ViewModelSource.Create(() => new ProjectViewModel());
            ProjectVM.ProjectData = (Project)this.Parameter;
            ProjectVM.Init();
        }

        //protected override void OnParameterChanged(object parameter)
        //{
        //    ProjectData = (Project)parameter;
        //    base.OnParameterChanged(parameter);
        //}

        #endregion
    }
}
