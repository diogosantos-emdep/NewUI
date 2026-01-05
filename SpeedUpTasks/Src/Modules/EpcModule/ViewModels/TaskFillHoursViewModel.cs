using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class TaskFillHoursViewModel : ViewModelBase
    {
        protected override void OnParameterChanged(object parameter)
        {
            // DroppedTaskList = (ObservableCollection<ProjectTask>)parameter;
            TaskWorkingTimes = new ObservableCollection<TaskWorkingTime>(((List<TaskWorkingTime>)parameter).AsEnumerable());
            base.OnParameterChanged(parameter);

        }

        #region Services

        IEpcService epcControl;
        #endregion

        #region ICommands
        public ICommand FillTaskHoursAcceptButtonCommand { get; set; }
        public ICommand FillTaskHoursCancelButtonCommand { get; set; }

        public bool ISave { get; set; }



        private ObservableCollection<ProjectTask> droppedTaskList;
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

        public DateTime WorkDate
        {
            get
            {
                return workDate;
            }

            set
            {
                SetProperty(ref workDate, value, () => WorkDate);
            }
        }

        public float WorkTime
        {
            get
            {
                return workTime;
            }

            set
            {
                SetProperty(ref workTime, value, () => WorkTime);
            }
        }

        public string TaskDescription
        {
            get
            {
                return taskDescription;
            }

            set
            {
                SetProperty(ref taskDescription, value, () => TaskDescription);
            }
        }
        private string taskDescription;

        private DateTime workDate;

        private float workTime;

        ObservableCollection<TaskWorkingTime> workedProjectFilledTaskList = new ObservableCollection<TaskWorkingTime>();
        public ObservableCollection<TaskWorkingTime> WorkedProjectFilledTaskList
        {
            get
            {
                return workedProjectFilledTaskList;
            }

            set
            {
                SetProperty(ref workedProjectFilledTaskList, value, () => WorkedProjectFilledTaskList);
            }
        }

        //public List<TaskWorkingTime> TaskWorkingTimes
        //{
        //    get
        //    {
        //        return taskWorkingTimes;
        //    }

        //    set
        //    {
        //        SetProperty(ref taskWorkingTimes, value, () => TaskWorkingTimes);
        //    }
        //}

        //List<TaskWorkingTime> taskWorkingTimes = new List<TaskWorkingTime>();
        ObservableCollection<TaskWorkingTime> taskWorkingTimes = new ObservableCollection<TaskWorkingTime>();
        public ObservableCollection<TaskWorkingTime> TaskWorkingTimes
        {
            get
            {
                return taskWorkingTimes;
            }

            set
            {
                SetProperty(ref taskWorkingTimes, value, () => TaskWorkingTimes);
            }
        }


        #endregion

        #region Constructor
        public TaskFillHoursViewModel()
        {

            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            FillTaskHoursAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(FillTaskHours);
            FillTaskHoursCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
        }

        #endregion

        #region Methods
        private void FillTaskHours(object obj)
        {
            if (TaskWorkingTimes.Count > 0)
            {
                foreach (var item in TaskWorkingTimes)
                {
                    TimeSpan result = item.WorkingTimeInHoursInTimeSpan;
                    string fromTimeString = result.ToString("hh':'mm");
                    if (fromTimeString.Contains(":"))
                    {
                        string[] mins = fromTimeString.Split(':');
                        string minstring1 = mins[0];
                        string minstring2 = mins[1];
                        float minInFloat1 = float.Parse(minstring1);
                        float minInFloat2 = float.Parse(minstring2);
                        minInFloat2 = (minInFloat2 / 60);
                        float minfloat3 = minInFloat1 + minInFloat2;                       
                        item.WorkingTimeInHours = minfloat3;

                    }
                    // string fromTimeString1 = fromTimeString.Replace(':', '.');
                    // float d = float.Parse(fromTimeString1);
                    // string fromTimeString1 = fromTimeString.Replace(':', '.');          
                }
               
                ISave = epcControl.UpdateWorkingHoursInTaskList(TaskWorkingTimes.ToList());
            }

            RequestClose(null, null);
        }

        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            if (obj != null && ((DevExpress.Xpf.Grid.DataControlBase)obj).ItemsSource != null)
            {

                epcControl.DeleteTaskWorkingTimeByIdList(TaskWorkingTimes.ToList());

            }
            RequestClose(null, null);
        }


        #endregion
    }
}
