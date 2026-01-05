using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Epc.ViewModels;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class TaskViewModel : NavigationViewModelBase, IDisposable
    {

        #region Services
        IEpcService epcControl;
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region Fields
        Boolean isExpandedNavBarControl = false;
        public TimeSpan MyTime { get; set; }
        ObservableCollection<ProjectTask> openTaskList = new ObservableCollection<ProjectTask>();
        ObservableCollection<TaskWorkingTime> workedProjectTaskList = new ObservableCollection<TaskWorkingTime>();
        ObservableCollection<User> teamUserList = new ObservableCollection<User>();
        DateTime currentDate;
        string weekTitle;
        private TaskWorkingTime activeTask;
        public ObservableCollection<ChartDataItem> WeeklyHoursList { get; set; }
        private TaskWorkingTime listSelectedTask;
        private TimeSpan _digitalTaskTracker;
        private ChartControl ccWeeklyHours;
        private float dayTotalTime;
        private int timecount;
        private Visibility playButtonVisibility;
        private Visibility stopButtonVisibility;
        #endregion

        #region ICommands
        public ICommand TodayEditValueChangedCommand { get; set; }
        public ICommand SelectWorkedHoursChartSeriesPointCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand TodaySpinNavigationCommand { get; set; }
        public ICommand TaskMoveCommand { get; set; }
        public ICommand DateEditLoadCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand PlayTaskWorkTimeCommand { get; set; }
        public ICommand PauseTaskWorkTimeCommand { get; set; }
        public ICommand HyperlinkToProjectCodeCommand { get; set; }
        public ICommand SelectedTaskDeatilsWorkedProjectTaskListCommand { get; set; }
        public ICommand SelectedUserIndexChangedCommand { get; set; }    
        public ICommand DropOnGauageCommand { get; set; }
        public ICommand TaskFillHoursCommands { get; set; }

        #endregion

        #region Properties

        List<object> selectedItems;
        public List<object> SelectedItems
        {
            get
            {
                return selectedItems;
            }
            set
            {
               
                 SetProperty(ref selectedItems, value, () => SelectedItems);
            }
        }


        private int workingTaskSelectedIndex;

        public ObservableCollection<ProjectTask> OpenTaskList
        {
            get { return openTaskList; }
            set
            {
                SetProperty(ref openTaskList, value, () => OpenTaskList);
            }
        }
        public TaskWorkingTime ActiveTask
        {
            get { return activeTask; }
            set
            {
                SetProperty(ref activeTask, value, () => ActiveTask);
            }
        }

        public TaskWorkingTime ListSelectedTask
        {
            get { return listSelectedTask; }
            set
            {               
                SetProperty(ref listSelectedTask, value, () => ListSelectedTask);
            }
        }

        public TimeSpan DigitalTaskTracker
        {
            get { return _digitalTaskTracker; }
            set
            {
                SetProperty(ref _digitalTaskTracker, value, () => DigitalTaskTracker);
            }
        }

        private DateTime _dateEditor;

        public DateTime DateEditor
        {
            get { return _dateEditor; }

            set { SetProperty(ref _dateEditor, value, () => DateEditor); }
        }

        private TaskWorkingTime selectedTaskWorkingTime;

        public  TaskWorkingTime SelectedTaskWorkingTime
        {
            get { return selectedTaskWorkingTime; }
            set
            {
                SetProperty(ref selectedTaskWorkingTime, value, () => SelectedTaskWorkingTime);
            }
        }

        private ObservableCollection<ProjectTask> droppedTaskList = new ObservableCollection<ProjectTask>();
        public ObservableCollection<ProjectTask> DroppedTaskList
        {
            get
            {
                return droppedTaskList;
            }

            set
            {
                SetProperty(ref droppedTaskList, value, () => DroppedTaskList);
            }
        }
        private string taskName;

        public string TaskName
        {
            get { return taskName; }
            set { SetProperty(ref taskName, value, () => TaskName); }
        }

        private string currentTime;

        public string CurrentTime
        {
            get { return currentTime; }
            set { SetProperty(ref currentTime, value, () => CurrentTime); }
        }

        private int selectedTaskIndex;

        public int SelectedTaskIndex
        {
            get { return selectedTaskIndex; }
            set { SetProperty(ref selectedTaskIndex, value, () => SelectedTaskIndex); }
        }

        public Boolean IsExpandedNavBarControl
        {
            get
            {
                return isExpandedNavBarControl;
            }
            set
            {
                SetProperty(ref isExpandedNavBarControl, value, () => IsExpandedNavBarControl);
            }
        }

        public string NavGroupHeader
        {
            get
            {
                if (!IsExpandedNavBarControl)
                    return "Day Log (" + TimeSpan.FromHours(DayTotalTime).Hours + "H " + TimeSpan.FromHours(DayTotalTime).Minutes + "m)";
                else
                {
                    return "Day Log";
                }
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }

            set
            {
                SetProperty(ref currentDate, value, () => CurrentDate);
            }
        }

        public string WeekTitle
        {
            get
            {
                return weekTitle;
            }

            set
            {
                SetProperty(ref weekTitle, value, () => WeekTitle);
            }
        }

        

        public ObservableCollection<TaskWorkingTime> WorkedProjectTaskList
        {
            get
            {
                return workedProjectTaskList;
            }

            set
            {
                SetProperty(ref workedProjectTaskList, value, () => WorkedProjectTaskList);
            }
        }

        public float DayTotalTime
        {
            get
            {
                return dayTotalTime;
            }

            set
            {
                SetProperty(ref dayTotalTime, value, () => DayTotalTime);
            }
        }

        public Visibility PlayButtonVisibility
        {
            get
            {
                return playButtonVisibility;
            }

            set
            {
                SetProperty(ref playButtonVisibility, value, () => PlayButtonVisibility);
            }
        }

        public Visibility StopButtonVisibility
        {
            get
            {
                return stopButtonVisibility;
            }

            set
            {
                SetProperty(ref stopButtonVisibility, value, () => StopButtonVisibility);
            }
        }

        public ObservableCollection<User> TeamUserList
        {
            get
            {
                return teamUserList;
            }

            set
            {
                SetProperty(ref teamUserList, value, () => TeamUserList);
            }
        }

        public int WorkingTaskSelectedIndex
        {
            get
            {
                return workingTaskSelectedIndex;
            }

            set
            {
                SetProperty(ref workingTaskSelectedIndex, value, () => WorkingTaskSelectedIndex);
            }
        }

       


        #endregion

        DispatcherTimer timer = new DispatcherTimer();

        #region Constructor
        public TaskViewModel()
        {
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;
            WeeklyHoursList = new ObservableCollection<ChartDataItem>();
            CurrentDate = DateTime.Now;
            OpenTaskList = new ObservableCollection<ProjectTask>();
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            Init();
            IsExpandedNavBarControl = false;
            ActiveTask = null;

            TodaySpinNavigationCommand = new DelegateCommand<SpinEventArgs>(TodaySpinNavigationAction);
            TaskMoveCommand = new DelegateCommand<DropEventArgs>(TaskMoveAction);
            CommandGridDoubleClick = new DelegateCommand<RowDoubleClickEventArgs>(TaskOpenAction);
            TodayEditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(TodayEditValueChangedAction);
            SelectWorkedHoursChartSeriesPointCommand = new DelegateCommand<MouseButtonEventArgs>(SelectWorkedHoursChartSeriesPointAction);
            DateEditLoadCommand = new DelegateCommand<object>(DateEditLoadCommandAction);
            PlayTaskWorkTimeCommand = new DelegateCommand(PlayTaskWorkTimedAction);
            // SelectedTaskWorkingTimeCommand = new DelegateCommand(PlayTaskWorkTimedAction);
           // SelectedTaskWorkingTimeCommand = new DelegateCommand<object>(GetTaskDetailAction);
            PauseTaskWorkTimeCommand = new DelegateCommand(PauseTaskWorkTimedAction);
            PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
            HyperlinkToProjectCodeCommand = new DelegateCommand<object>(new Action<object>((project) =>
            {
                ShowProjectView(project);
            }));

            DateEditor = DateTime.Today;
            timer.Tick += timer_Tick;
            DigitalTaskTracker = new TimeSpan(0, 0, 0);
            SetButtonVisibility(true);

           

            // DefaultSelectedUser = TeamUserList.FindIndex(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);
            SelectedTaskDeatilsWorkedProjectTaskListCommand = new DelegateCommand<object>(ShowTaskDeatilsViewAction);     
            SelectedUserIndexChangedCommand = new Prism.Commands.DelegateCommand<object>(SelectedUserIndex);       
            DropOnGauageCommand = new Prism.Commands.DelegateCommand<GaugeDropEventArgs>(OnGaugeDropAction);
            TaskFillHoursCommands = new Prism.Commands.DelegateCommand<object>(TaskFillHoursAction);
        }


        #endregion

        #region Mehtods
  
        public void OnGaugeDropAction(GaugeDropEventArgs e)
        {         
            TaskMoveAction(e);
            PlayTaskWorkTimedAction();
           
        }


        public void ShowProjectView(object ProjectTask)
        {
            Project data = (Project)ProjectTask;
            Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProductView", data, this);
        }


       public void ShowTaskDeatilsViewAction(object ProjectTask )
        {

            TaskWorkingTime data = (TaskWorkingTime)ProjectTask;
            ProjectTask dt = data.ProjectTask;
            Service.Navigate("Emdep.Geos.Modules.Epc.Views.TaskDetailsView", dt, Service.Current);
        }

        public void PlayTaskWorkTimedAction()
        {
            if (SelectedTaskWorkingTime != null)
            {

                if (ActiveTask == null)
                {
                    ActiveTask = SelectedTaskWorkingTime;

                    if (ActiveTask != null)
                    {
                        SetButtonVisibility(false);
                        PlayTaskWorkTimedPartial();
                    }
                }
                else
                {
                    SetButtonVisibility(false);
                    UpdateActiveTaskTime();

                    ActiveTask = SelectedTaskWorkingTime;
                    PlayTaskWorkTimedPartial();
                }

            }
            
        }

        public void PlayTaskWorkTimedPartial()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            DigitalTaskTracker = TimeSpan.FromHours(ActiveTask.WorkingTimeInHours);
            timer.Start();
            SetButtonVisibility(false);
        }

        public void PauseTaskWorkTimedAction()
        {
            if (ActiveTask != null)
            {
                timer.Stop();
                UpdateActiveTaskTime();
                ShowTotalHours();
                SetButtonVisibility(true);
            }
        }

        private void UpdateActiveTaskTime()
        {
            TaskWorkingTime tasktime = new TaskWorkingTime();
            tasktime.IdTask = ActiveTask.IdTask;
            tasktime.IdUser = ActiveTask.IdUser;
            tasktime.IdTaskWorkingTime = ActiveTask.IdTaskWorkingTime;
            tasktime.WorkingTimeInHours = ActiveTask.WorkingTimeInHours;
            tasktime.WorkingDate = ActiveTask.WorkingDate;
            bool result = epcControl.UpdateWorkingHoursInTask(tasktime);
            ShowTotalHours();
        }

        private void ShowTotalHours()
        {
            DayTotalTime = 0;
            foreach (var item in WorkedProjectTaskList)
            {
                if (ActiveTask != null)
                {
                    if (ActiveTask.IdTaskWorkingTime == item.IdTaskWorkingTime)
                    {
                        item.WorkingTimeInHours = ActiveTask.WorkingTimeInHours;
                    }
                }

                DayTotalTime += item.WorkingTimeInHours;
            }

        }

        public void SelectWorkedHoursChartSeriesPointAction(MouseButtonEventArgs e)
        {
            var pane = (DevExpress.Xpf.Charts.Pane)e.Source;
            var diagramm = LayoutTreeHelper.GetVisualParents(pane)
                   .OfType<XYDiagram2D>()
                   .FirstOrDefault();
            var chart = LayoutTreeHelper.GetVisualParents(diagramm)
                    .OfType<ChartControl>()
                    .FirstOrDefault();

            var hitInfo = chart.CalcHitInfo(e.GetPosition(chart));
            if (hitInfo.SeriesPoint != null)
            {
                ChartDataItem data = (ChartDataItem)hitInfo.SeriesPoint.Tag;
                CurrentDate = data.CurrentDate;
            }
        }

        public void DateEditLoadCommandAction(object e)
        {
            //var chartcontrol = (ChartControl)e;
            //ccWeeklyHours = chartcontrol;

            //chartcontrol.UpdateData();
        }

        public void TodayEditValueChangedAction(EditValueChangedEventArgs e)
        {
            setWeekTitle();

        }
        public void Init()
        {
            //Task.Run(() => { LoadOpenTask(); });
            LoadOpenTask();
            setWeekTitle();
        }

        public void LoadOpenTask()
        {
            TeamUserList = new ObservableCollection<User>(epcControl.GetUserTeams(GeosApplication.Instance.ActiveUser.IdUser).AsEnumerable());
            SelectedItems = new List<object>();
            foreach (var item in TeamUserList)
            {
                if (item.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
                    SelectedItems.Add(item);

            }

           // var list = epcControl.GetTeamOpenTaskByUserId(GeosApplication.Instance.ActiveUser.IdUser);
            List<User> selectedUsersList = SelectedItems.Select(s => (User)s).ToList();
            var list = epcControl.GetOpenTasks(selectedUsersList);
            OpenTaskList.AddRange(list.ToList());
        }
        public void TaskOpenAction(RowDoubleClickEventArgs e)
        {
            TableView detailView = e.HitInfo.Column.View as TableView;
            ProjectTask data = (ProjectTask)(detailView.DataControl as GridControl).GetRow(e.HitInfo.RowHandle);
            Service.Navigate("Emdep.Geos.Modules.Epc.Views.TaskDetailsView", data, Service.Current);
        }

        public void PrintAction(object obj)
        {
            PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
            pcl.Margins.Bottom = 5;
            pcl.Margins.Top = 5;
            pcl.Margins.Left = 5;
            pcl.Margins.Right = 5;
            pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TaskViewCustomPrintHeaderTemplate"];
            pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TaskViewCustomPrintFooterTemplate"];
            pcl.Landscape = true;
            pcl.CreateDocument(false);
            //pcl.ShowPrintPreview(Application.Current.MainWindow);
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            pcl.ShowPrintPreview(window);
        }

       
        public void TaskMoveAction(DropEventArgs e)
        {
           if(e is Emdep.Geos.UI.Helper.GaugeDropEventArgs)
            {
                if (e.SourceControl is DevExpress.Xpf.Grid.GridControl)
                {          
                    foreach (ProjectTask item in e.DraggedRows)
                    {
                        if (item is ProjectTask)
                        {
                            TaskWorkingTime work = new TaskWorkingTime();
                            work.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            work.WorkingDate = CurrentDate;
                            work.IdTask = item.IdTask;
                            work.WorkingTimeInHours = 0.00f;
                            work.TaskTitle = item.TaskTitle;
                            work.ProjectCode = item.Project.ProjectCode;
                            work = epcControl.AddWorkingHoursInTask(work);
                           // work.ProjectTask = item;
                            work.User = GeosApplication.Instance.ActiveUser;
                            WorkedProjectTaskList.Add(work);
                            SelectedTaskWorkingTime = work;
                        }
                    }
                }
            }

            if (e is DevExpress.Xpf.Grid.DragDrop.ListBoxDropEventArgs)
            {
                if (e.SourceControl is DevExpress.Xpf.Grid.GridControl)
                {
                    //List<string> lstTest = new List<string>();
                    List<TaskWorkingTime> taskWorkingTimes = new List<TaskWorkingTime>();
                    foreach (ProjectTask item in e.DraggedRows)
                    {
                        if (item is ProjectTask)
                        {                           
                            TaskWorkingTime work = new TaskWorkingTime();
                            work.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            work.IdTask = item.IdTask;
                            work.Description = item.Description;
                            work.WorkingDate = CurrentDate;
                            work.WorkingTimeInHours = 0.00F;
                            work.TaskTitle = item.TaskTitle;
                            work.ProjectCode = item.Project.ProjectCode;
                            taskWorkingTimes.Add(work);
                           // lstTest.Add(item.IdTask, work.ProjectTask.TaskTitle);            
                             // work.ProjectTask = item;
                        }
                    }
                
                    taskWorkingTimes = epcControl.AddWorkingHoursInTaskList(taskWorkingTimes);
                    //  taskWorkingTimes = taskWorkingTimes.Select(i => { i.ProjectTask.TaskTitle = lstTest.Select(x=>x.Value).; return i; }).ToList();
                    TaskFillHoursAction(taskWorkingTimes);
                }
            }
            e.Handled = true;
        }





        public void TaskFillHoursAction(object obj)
        {          
            TaskFillHoursView taskfillhoursview = new TaskFillHoursView();
            TaskFillHoursViewModel taskfillhoursviewmodel = new TaskFillHoursViewModel();
            ((ISupportParameter)taskfillhoursviewmodel).Parameter = obj;
            EventHandler handle = delegate { taskfillhoursview.Close(); };
            taskfillhoursviewmodel.RequestClose += handle;
            taskfillhoursview.DataContext = taskfillhoursviewmodel;
            taskfillhoursview.ShowDialogWindow();

            if (taskfillhoursviewmodel.ISave)
            {             
                foreach (TaskWorkingTime item in taskfillhoursviewmodel.TaskWorkingTimes)
                {
                  //  WorkedProjectTaskList.Clear();
                    WorkedProjectTaskList.Add(item);
                   
                }
                ShowTotalHours();

            }

        }


        public void SelectedUserIndex(object e)
        {
            OpenTaskList.Clear();         
            var obj = (EditValueChangingEventArgs)e;   
            List<object> selectedlist = (List<object>)obj.NewValue;
            List<object> oldselectedlist = (List<object>)obj.OldValue;
            SelectedItems = selectedlist;

            if (SelectedItems == null)
            {
                return;

            }
             if (oldselectedlist == null || (selectedlist.Count > oldselectedlist.Count))
                {

                    SelectedItems = selectedlist;
                }
                else if (selectedlist.Count < oldselectedlist.Count)
                {

                    SelectedItems = selectedlist;
                }
           
          
            List<User> selectedUsersList = SelectedItems.Select(s => (User)s).ToList();
            var list = epcControl.GetOpenTasks(selectedUsersList);
            OpenTaskList.AddRange(list.ToList());
        }
        private void setWeekTitle()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            CultureInfo cul = CultureInfo.CurrentCulture;

            WeekTitle = "CW " + cul.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0');
            FillWorkedTask();
            FillWeeklyHours();
            if (ccWeeklyHours != null)
            {
                ccWeeklyHours.UpdateData();
            }
            ShowTotalHours();
        }

        public void SetButtonVisibility(Boolean isplaybutton)
        {
            if (isplaybutton == true)
            {
                StopButtonVisibility = Visibility.Hidden;
                PlayButtonVisibility = Visibility.Visible;
            }
            else
            {
                PlayButtonVisibility = Visibility.Hidden;
                StopButtonVisibility = Visibility.Visible;
            }
        }

        private void FillWeeklyHours()
        {

            var taskWorkingTimes = epcControl.GetWeeklyTaskWorkingTime(GeosApplication.Instance.ActiveUser.IdUser, CurrentDate);

            WeeklyHoursList.Clear();
            DateTime input = CurrentDate;

            int delta = DayOfWeek.Monday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Monday", WeekDay = DayOfWeek.Monday, CurrentDate = input.AddDays(delta) });

            delta = DayOfWeek.Tuesday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Tuesday", WeekDay = DayOfWeek.Tuesday, CurrentDate = input.AddDays(delta) });

            delta = DayOfWeek.Wednesday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Wednesday ", WeekDay = DayOfWeek.Wednesday, CurrentDate = input.AddDays(delta) });

            delta = DayOfWeek.Thursday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Thursday ", WeekDay = DayOfWeek.Thursday, CurrentDate = input.AddDays(delta) });

            delta = DayOfWeek.Friday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Friday", WeekDay = DayOfWeek.Friday, CurrentDate = input.AddDays(delta) });

            delta = DayOfWeek.Saturday - input.DayOfWeek;
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Saturday", WeekDay = DayOfWeek.Saturday, CurrentDate = input.AddDays(delta) });

            delta = 7 + (DayOfWeek.Sunday - input.DayOfWeek);
            WeeklyHoursList.Add(new ChartDataItem() { Series = "Weekhours", Argument = "Sunday", WeekDay = DayOfWeek.Sunday, CurrentDate = input.AddDays(delta) });

            foreach (var item in WeeklyHoursList)
            {
                var d = (from r in taskWorkingTimes where r.WorkingDate.DayOfWeek == item.WeekDay select r.WorkingTimeInHours).Sum();
                item.Time = d;
            }
        }

        public void TodaySpinNavigationAction(SpinEventArgs e)
        {
            var editor = e.Source as DateEdit;
            DateTime value = (DateTime)editor.EditValue;
            if (e.IsSpinUp)
            {
                editor.EditValue = value.AddDays(1);
            }
            else
            {
                editor.EditValue = value.AddDays(-1);
            }

            e.Handled = true;
        }

        public void FillWorkedTask()
        {
            var list = epcControl.GetTaskWorkingTimeByDateAndUser(GeosApplication.Instance.ActiveUser.IdUser, CurrentDate);
            list=list.Select(i => { i.TaskTitle = i.ProjectTask.TaskTitle; return i; }).ToList();
            foreach (var x in WorkedProjectTaskList.ToList())
            {
                WorkedProjectTaskList.Remove(x);
            }

            WorkedProjectTaskList.AddRange(list.ToArray());
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (ActiveTask != null)
            {
                if (timecount >= 60)
                {

                    ActiveTask.WorkingTimeInHours = (float)Math.Round((ActiveTask.WorkingTimeInHours + (float)0.01666f), 4);
                    UpdateActiveTaskTime();
                    timecount = 0;
                }

                DigitalTaskTracker = DigitalTaskTracker.Add(new TimeSpan(0, 0, 1));
                timecount++;
            }
        }
        public void Dispose()
        {
        }
        #endregion
    }
}
