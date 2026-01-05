using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Mvvm;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.Scheduler.Menu;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class CalendarViewModel : NavigationViewModelBase, IDisposable
    {
        private DevExpress.Mvvm.INavigationService Service { get { return this.GetService<DevExpress.Mvvm.INavigationService>(); } }

        enum CalendarAppointmentType
        {
            TaskDelivery,
            ProjectDelivery,
            MilestoneDelivery,
            AnalysisDelivery
        }

        #region Services
        IEpcService epcControl;
        #endregion

        #region Properties

        public List<object> SelectedFilters { get; set; }
        public AppointmentLabelCollection Labels { get; private set; }
        public List<DataHelper> Filters { get; set; }

        DateTime fromDate;
        DateTime toDate;
        private List<object> selectedList;

        #endregion

        #region Collections

        public ObservableCollection<ProjectMilestone> projectmilestones = new ObservableCollection<ProjectMilestone>();
        public ObservableCollection<ProjectMilestoneDate> projectmilestonesdate = new ObservableCollection<ProjectMilestoneDate>();
        public ObservableCollection<ProjectAnalysis> projectanalysis = new ObservableCollection<ProjectAnalysis>();
        public ObservableCollection<Project> projects = new ObservableCollection<Project>();
        public ObservableCollection<ProjectTask> projecttasks = new ObservableCollection<ProjectTask>();

        private ObservableCollection<AppointmentModel> projectTasksDeliveryList = new ObservableCollection<AppointmentModel>();
        public ObservableCollection<DateTime> SelectedDates { get; set; }

        public ObservableCollection<AppointmentModel> ProjectTasksDeliveryList
        {
            get { return projectTasksDeliveryList; }
            set
            {
                SetProperty(ref projectTasksDeliveryList, value, () => ProjectTasksDeliveryList);
            }
        }

        #endregion

        #region ICommands

        public ICommand AddNewAppointmentCommand { get; private set; }
        public ICommand SelectFilterCommand { get; private set; }
        public ICommand SelectDateCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }

        public ICommand AllowEditAppointmentCommand { get; private set; }
        public ICommand PopupMenuShowingCommand { get; private set; }

        //public ICommand AllowAppointmentDeleteCommand { get; private set; }
        //public ICommand AllowAppointmentEditCommand { get; private set; }
        #endregion

        public CalendarViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            SelectedDates = new ObservableCollection<DateTime>();
            AddNewAppointmentCommand = new DevExpress.Mvvm.DelegateCommand<object>(AddNewAppointmentAction);
            SelectFilterCommand = new DevExpress.Mvvm.DelegateCommand<object>(SelectFilterAction);
            SelectDateCommand = new DevExpress.Mvvm.DelegateCommand<EventArgs>(SelectDatesAction);
            LoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(LoadAction);

            AllowEditAppointmentCommand = new DevExpress.Mvvm.DelegateCommand<EditAppointmentFormEventArgs>(AllowEditAppointmentAction);
            PopupMenuShowingCommand = new DevExpress.Mvvm.DelegateCommand<SchedulerMenuEventArgs>(PopupMenuShowingAction);    // PopupMenuShowingEventArgs

            //AllowAppointmentDeleteCommand = new DevExpress.Mvvm.DelegateCommand<object>(AllowAppointmentDeleteMethod);
            //AllowAppointmentEditCommand = new DevExpress.Mvvm.DelegateCommand<object>(AllowAppointmentEditMethod);

            SelectedFilters = new List<object>();

            Labels = new AppointmentLabelCollection();
            Labels.Clear();
            Labels.Add(Labels.CreateNewLabel(0, "Salmon", "Salmon", Colors.Salmon));
            Labels.Add(Labels.CreateNewLabel(1, "LightSeaGreen", "LightSeaGreen", Colors.LightSeaGreen));
            Labels.Add(Labels.CreateNewLabel(2, "Goldenrod", "Goldenrod", Colors.Goldenrod));
            Labels.Add(Labels.CreateNewLabel(3, "SkyBlue", "SkyBlue", Colors.SkyBlue));

            Filters = new List<DataHelper>();
            Filters.Add(new DataHelper() { Id = (int)CalendarAppointmentType.TaskDelivery, Name = "Task Delivery", StringValue1 = Labels[0].Color.ToString() });
            Filters.Add(new DataHelper() { Id = (int)CalendarAppointmentType.ProjectDelivery, Name = "Project Delivery", StringValue1 = Labels[1].Color.ToString() });
            Filters.Add(new DataHelper() { Id = (int)CalendarAppointmentType.MilestoneDelivery, Name = "Milestone", StringValue1 = Labels[2].Color.ToString() });
            Filters.Add(new DataHelper() { Id = (int)CalendarAppointmentType.AnalysisDelivery, Name = "Analysis Delivery", StringValue1 = Labels[3].Color.ToString() });

            SelectedFilters.Add(Filters[0]);
            SelectedFilters.Add(Filters[1]);
        }

        //private void AllowAppointmentDeleteMethod(object parameter)
        //{
        //    AppointmentOperationEventArgs eventArgs = parameter as AppointmentOperationEventArgs;
        //    eventArgs.Allow = eventArgs.Appointment.Id != "ProjectTask";
        //}

        //private void AllowAppointmentEditMethod(object parameter)
        //{
        //    AppointmentOperationEventArgs eventArgs = parameter as AppointmentOperationEventArgs;
        //    eventArgs.Allow = eventArgs.Appointment.Id != "ProjectTask" ;
        //}

        private void LoadAction(object parameter)
        {
            DevExpress.Xpf.Editors.DateNavigator.DateNavigator dateNavigator = (DevExpress.Xpf.Editors.DateNavigator.DateNavigator)parameter;
            SelectDatesAction(null);
        }

        private void SelectFilterAction(object parameter)
        {
            var obj = (EditValueChangedEventArgs)parameter;
            List<object> selectedlist = (List<object>)obj.NewValue;
            List<object> oldselectedlist = (List<object>)obj.OldValue;
            selectedList = selectedlist;

            if (selectedlist == null)
            {
                ProjectTasksDeliveryList.Clear();
            }
            else if (oldselectedlist == null || (selectedlist.Count > oldselectedlist.Count))
            {
                DataHelper data = selectedlist[selectedlist.Count - 1] as DataHelper;

                CalendarAppointmentType type = (CalendarAppointmentType)Enum.Parse(typeof(CalendarAppointmentType), data.Id.ToString());
                switch (type)
                {
                    case CalendarAppointmentType.TaskDelivery:
                        if (projecttasks == null)
                            projecttasks = new ObservableCollection<ProjectTask>(epcControl.GetProjectTasksDeliveryByDateAndUser(fromDate, toDate));
                        if (projecttasks != null)
                            foreach (var item in projecttasks)
                            {
                                ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.TaskTitle, StartTime = item.DueDate.Value, Label = 0, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.TaskDelivery });
                                // ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.TaskTitle, ResourceId = item.IdTask, StartTime = item.DueDate.Value, Label = 1, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.TaskDelivery });
                            }

                        break;

                    case CalendarAppointmentType.ProjectDelivery:
                        if (projects == null)
                            projects = new ObservableCollection<Project>(epcControl.GetProjectsDeliveryByDateAndUser(fromDate, toDate));
                        if (projects != null)
                            foreach (var item in projects)
                            {
                                ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.ProjectName, StartTime = item.DueDate.Value, Label = 1, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.ProjectDelivery });
                                // ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProject, Subject = item.ProjectName, StartTime = item.DueDate.Value, Label = 2, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.ProjectDelivery });
                            }

                        break;

                    case CalendarAppointmentType.MilestoneDelivery:
                        if (projectmilestones == null)
                            projectmilestones = new ObservableCollection<ProjectMilestone>(epcControl.GetProjectMilestonesByDateAndUser(fromDate, toDate));
                        if (projectmilestones != null)
                            foreach (var item in projectmilestones)
                            {
                                foreach (var item2 in item.ProjectMilestoneDates)
                                {
                                    ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.MilestoneTitle, StartTime = item2.TargetDate, Label = 2, EndTime = item2.TargetDate, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.MilestoneDelivery });
                                }

                                //  ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProjectMilestone, Subject = item.MilestoneTitle, StartTime = item.MilestoneDate.Value, Label = 3, EndTime = item.MilestoneDate.Value, Description = item.Description,  AppointmentType = (int)CalendarAppointmentType.MilestoneDelivery });

                            }
                        break;

                    case CalendarAppointmentType.AnalysisDelivery:
                        if (projectanalysis == null)
                            projectanalysis = new ObservableCollection<ProjectAnalysis>(epcControl.GetProjectAnalysisDeliveryByDateAndUser(fromDate, toDate));
                        if (projectanalysis != null)
                            foreach (var item in projectanalysis)
                            {
                                ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.Project.ProjectName, StartTime = item.EngDeliveryDate.Value, Label = 3, EndTime = item.EngDeliveryDate.Value, Description = item.Project.Description, AppointmentType = (int)CalendarAppointmentType.AnalysisDelivery });
                                // ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProject, Subject = item.Project.ProjectName, StartTime = item.EngDeliveryDate.Value, Label = 4, EndTime = item.EngDeliveryDate.Value, Description = item.Project.Description, AppointmentType = (int)CalendarAppointmentType.AnalysisDelivery });
                            }
                        break;

                    default:
                        break;
                }

            }
            else if (selectedlist.Count < oldselectedlist.Count)
            {
                IEnumerable<object> result = oldselectedlist.Except(selectedlist);

                if (result != null)
                {
                    foreach (var item2 in result)
                    {
                        DataHelper data = (DataHelper)item2;
                        CalendarAppointmentType type = (CalendarAppointmentType)Enum.Parse(typeof(CalendarAppointmentType), data.Id.ToString());
                        switch (type)
                        {
                            case CalendarAppointmentType.TaskDelivery:
                                var toRemove = ProjectTasksDeliveryList.Where(x => x.AppointmentType == (int)CalendarAppointmentType.TaskDelivery).ToList();
                                foreach (var item in toRemove)
                                    ProjectTasksDeliveryList.Remove(item);
                                break;

                            case CalendarAppointmentType.ProjectDelivery:
                                var toTasksDeliveryRemove = ProjectTasksDeliveryList.Where(x => x.AppointmentType == (int)CalendarAppointmentType.ProjectDelivery).ToList();
                                foreach (var item in toTasksDeliveryRemove)
                                    ProjectTasksDeliveryList.Remove(item);
                                break;

                            case CalendarAppointmentType.MilestoneDelivery:
                                var toMilestoneDeliveryRemove = ProjectTasksDeliveryList.Where(x => x.AppointmentType == (int)CalendarAppointmentType.MilestoneDelivery).ToList();
                                foreach (var item in toMilestoneDeliveryRemove)
                                    ProjectTasksDeliveryList.Remove(item);
                                break;

                            case CalendarAppointmentType.AnalysisDelivery:
                                var toAnalysisDeliveryRemove = ProjectTasksDeliveryList.Where(x => x.AppointmentType == (int)CalendarAppointmentType.AnalysisDelivery).ToList();
                                foreach (var item in toAnalysisDeliveryRemove)
                                    ProjectTasksDeliveryList.Remove(item);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void SelectDatesAction(EventArgs parameter)
        {
            if (SelectedDates.Count > 0)
            {
                fromDate = SelectedDates[0].Date;
                toDate = SelectedDates[0].Date;

                if (SelectedDates.Count > 1)
                {
                    toDate = SelectedDates[SelectedDates.Count - 1].Date;
                }

                ProjectTasksDeliveryList.Clear();
                projecttasks = null;
                projectmilestones = null;
                projects = null;
                projectanalysis = null;

                if (SelectedFilters != null)
                {
                    foreach (var titem in SelectedFilters)
                    {
                        DataHelper data = titem as DataHelper;
                        CalendarAppointmentType type = (CalendarAppointmentType)Enum.Parse(typeof(CalendarAppointmentType), data.Id.ToString());

                        switch (type)
                        {
                            case CalendarAppointmentType.TaskDelivery:
                                projecttasks = new ObservableCollection<ProjectTask>(epcControl.GetProjectTasksDeliveryByDateAndUser(fromDate, toDate));
                                foreach (var item in projecttasks)
                                {
                                    ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, Subject = item.TaskTitle, ResourceId = item.IdTask, StartTime = item.DueDate.Value, Label = 0, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.TaskDelivery });
                                }
                                break;

                            case CalendarAppointmentType.ProjectDelivery:

                                projects = new ObservableCollection<Project>(epcControl.GetProjectsDeliveryByDateAndUser(fromDate, toDate));
                                foreach (var item in projects)
                                {
                                    ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProject, Subject = item.ProjectName, StartTime = item.DueDate.Value, Label = 1, EndTime = item.DueDate.Value, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.ProjectDelivery });
                                }

                                break;
                            case CalendarAppointmentType.MilestoneDelivery:
                                projectmilestones = new ObservableCollection<ProjectMilestone>(epcControl.GetProjectMilestonesByDateAndUser(fromDate, toDate));
                                foreach (var item in projectmilestones)
                                {
                                    foreach (var item2 in item.ProjectMilestoneDates)
                                    {
                                        ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProjectMilestone, Subject = item.MilestoneTitle, StartTime = item2.TargetDate, Label = 2, EndTime = item2.TargetDate, Description = item.Description, AppointmentType = (int)CalendarAppointmentType.MilestoneDelivery });
                                    }

                                }
                                break;
                            case CalendarAppointmentType.AnalysisDelivery:

                                projectanalysis = new ObservableCollection<ProjectAnalysis>(epcControl.GetProjectAnalysisDeliveryByDateAndUser(fromDate, toDate));
                                foreach (var item in projectanalysis)
                                {
                                    ProjectTasksDeliveryList.Add(new AppointmentModel() { Data = item, ResourceId = item.IdProject, Subject = item.Project.ProjectName, StartTime = item.EngDeliveryDate.Value, Label = 3, EndTime = item.EngDeliveryDate.Value, Description = item.Project.Description, AppointmentType = (int)CalendarAppointmentType.AnalysisDelivery });
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void AddNewAppointmentAction(object parameter)
        {
        }

        private void AllowEditAppointmentAction(EditAppointmentFormEventArgs parameter)
        {
            parameter.Cancel = true;

            if (parameter.Appointment.Id is ProjectTask)
            {
                Service.Navigate("Emdep.Geos.Modules.Epc.Views.TaskDetailsView", parameter.Appointment.Id, Service.Current);
            }
            else if (parameter.Appointment.Id is Project)
            {
                Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProjectDetailsView", parameter.Appointment.Id, Service.Current);
            }
            else if (parameter.Appointment.Id is ProjectMilestone)
            {
                MilestoneDialogView milestoneDialogView = new Views.MilestoneDialogView();
                MilestoneDialogViewModel milestoneDialogViewModel = new MilestoneDialogViewModel();

                ((ISupportParameter)milestoneDialogViewModel).Parameter = parameter.Appointment.Id;

                EventHandler handle = delegate { milestoneDialogView.Close(); };
                milestoneDialogViewModel.RequestClose += handle;
                milestoneDialogView.DataContext = milestoneDialogViewModel;
                milestoneDialogView.ShowDialogWindow();

                if (milestoneDialogViewModel.IsMilestoneAchieved)
                {
                }

                if (milestoneDialogViewModel.IsMilestoneUpdated)
                {
                    ProjectMilestoneDate newProjectMilestonesDate = milestoneDialogViewModel.ProjectMilestoneData.ProjectMilestoneDates.LastOrDefault();

                    ProjectTasksDeliveryList.Add(new AppointmentModel()
                    {
                        Data = milestoneDialogViewModel.ProjectMilestoneData,
                        ResourceId = milestoneDialogViewModel.ProjectMilestoneData.IdProjectMilestone,
                        Subject = milestoneDialogViewModel.ProjectMilestoneData.MilestoneTitle,
                        StartTime = newProjectMilestonesDate.TargetDate,
                        Label = 2,
                        EndTime = newProjectMilestonesDate.TargetDate,
                        Description = newProjectMilestonesDate.Comments,
                        AppointmentType = (int)CalendarAppointmentType.MilestoneDelivery
                    });
                }

            }
            else if (parameter.Appointment.Id is Project)
            {
                Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProjectDetailsView", parameter.Appointment.Id, Service.Current);
            }

        }

        private void PopupMenuShowingAction(SchedulerMenuEventArgs parameter)
        { 
            if (parameter.Menu.Name == SchedulerMenuItemName.AppointmentMenu)
            {
                object delete = parameter.Menu.Items.FirstOrDefault(x => x is SchedulerBarItem && (string)((SchedulerBarItem)x).Content == "Delete");
                parameter.Menu.Items.Remove((SchedulerBarItem)delete);
            }
        }

        public void Dispose()
        {
        }

    }
}
