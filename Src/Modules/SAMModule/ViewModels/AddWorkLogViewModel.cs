using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
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

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    class AddWorkLogViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISAMService SAMService = new SAMServiceController("localhost:6699");
       // IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");
        #endregion // End Services Region

        #region Events
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

        #region Declaration
        private string windowHeader;
        private string timeEditMask;
        private List<DateTime> _FromDates;
        private List<DateTime> _ToDates;
        private string oldItemWorkLog;
        private string newItemWorkLog;
        private OTWorkingTime selectedWorkLog;
        List<OTWorkingTime> workLogList;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;
        private string error = string.Empty;
        private DateTime startTime;
        private DateTime endTime;
        private DateTime startDate;
        private DateTime endDate;
        private List<OTWorkingTime> workLogItemList;
        private string worklogTotalTime;
        private string name;
        private List<UserShortDetail> userImageList;
        private string startDateErrorMessage = string.Empty;
        private int selectedIndexForEmployee;
        private string endDateErrorMessage = string.Empty;
        // private bool isPermissionEnabled;
        bool isReadOnly;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public List<DateTime> FromDates
        {
            get { return _FromDates; }
            set
            {
                if (value != _FromDates)
                {
                    _FromDates = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("FromDates"));
                }
            }
        }
        public List<DateTime> ToDates
        {
            get { return _ToDates; }
            set
            {
                if (value != _ToDates)
                {
                    _ToDates = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ToDates"));
                }
            }
        }
        public string TimeEditMask
        {
            get { return timeEditMask; }
            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }
        public string OldItemWorkLog
        {
            get { return oldItemWorkLog; }
            set
            {
                oldItemWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldItemWorkLog"));
            }
        }
        public string NewItemWorkLog
        {
            get { return newItemWorkLog; }
            set
            {
                newItemWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewItemWorkLog"));
            }
        }
        public OTWorkingTime SelectedWorkLog
        {
            get { return selectedWorkLog; }
            set
            {
                selectedWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkLog"));
            }
        }
        public List<OTWorkingTime> WorkLogList
        {
            get { return workLogList; }
            set
            {
                workLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogList"));
            }
        }

        public string StartTimeErrorMessage
        {
            get { return startTimeErrorMessage; }
            set
            {
                startTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeErrorMessage"));
            }
        }
        public string EndTimeErrorMessage
        {
            get { return endTimeErrorMessage; }
            set
            {
                endTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeErrorMessage"));
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnly"));
            }
        }
        public List<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set
            {
                workLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItemList"));
            }
        }
        public string WorklogTotalTime
        {
            get { return worklogTotalTime; }
            set
            {
                worklogTotalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorklogTotalTime"));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public List<UserShortDetail> UserImageList
        {
            get { return userImageList; }
            set { userImageList = value; }
        }
        public int SelectedIndexForEmployee
        {
            get
            {
                return selectedIndexForEmployee;
            }

            set
            {
                selectedIndexForEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEmployee"));
            }
        }
        #endregion

        #region Command  
        //  public ICommand CloseWindowCommand { get; set; }
        public ICommand AddItemsSaveWorkLogCommand { get; set; }
        public ICommand OnTimeEditValueChangingCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }

        #endregion

        #region Constructor 

        public AddWorkLogViewModel()
        {
            IsReadOnly = true;
            
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
          //OnTimeEditValueChangingCommand = new DelegateCommand<RoutedEventArgs>(OnTimeEditValueChanging);
            AddItemsSaveWorkLogCommand = new DelegateCommand<object>(SaveWorkLogAction);
            CancelButtonCommand = new DelegateCommand<object>(AddworklogCancelAction);
         //   SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedCommandAction);
            OnTimeEditValueChangingCommand = new DelegateCommand<RoutedEventArgs>(OnTimeEditValueChanging);
            OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
            FillFromDates();
            FillToDates();

        }
        #endregion

        #region Methods
        private void FillFromDates()
        {
            FromDates = new List<DateTime>();
            double addMinutes = 0;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            for (DateTime i = today; i <= tomorrow; i = i.AddMinutes(1))
            {
                DateTime dtotherdate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, i.Hour, i.Minute, i.Second);
                FromDates.Add(dtotherdate);
            }
        }

        private void FillToDates()
        {
            ToDates = new List<DateTime>();
            double addMinutes = 0;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            for (DateTime i = today; i <= tomorrow; i = i.AddMinutes(1))
            {
                DateTime dtotherdate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, i.Hour, i.Minute, i.Second);
                ToDates.Add(dtotherdate);
            }
        }

        private void SaveWorkLogAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert SaveCommentAction ...", category: Category.Info, priority: Priority.Low);
            // SAMLogEntries comment = new SAMLogEntries()
            error = EnableValidationAndGetError();
         
            PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
            PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
            

            //List<OTWorkingTime> temp = new List<OTWorkingTime>();
            if (error != null)
            {
                return;
            }

            OTWorkingTime worklog = new OTWorkingTime();
           
            DateTime WLSDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
            DateTime WLEDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
            worklog.UserShortDetail = new UserShortDetail();
            worklog.UserShortDetail.UserName = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName;
            worklog.UserShortDetail.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
           worklog.StartTime = WLSDate;
            worklog.EndTime = WLEDate;
            worklog.TotalTime = (worklog.EndTime.Value.TimeOfDay + worklog.StartTime.Value.TimeOfDay).Duration();
             //worklog.TotalTime =(worklog.EndTime.Value.TimeOfDay.Subtract( worklog.StartTime.Value.TimeOfDay));
          //  worklog.TotalTime = worklog.TotalTime.Subtract(worklog.TotalTime);
            //Math.Abs(Convert.To(worklog.TotalTime));
            int Hours = worklog.TotalTime.Days * 24 + worklog.TotalTime.Hours;
            worklog.Hours = string.Format("{0}H {1}M", Hours, worklog.TotalTime.Minutes);
          
          //  SelectedWorkLog = worklog;
            if (WorkLogList == null)
                WorkLogList = new List<OTWorkingTime>();
            WorkLogList.Add(worklog);
            WorkLogList = new List<OTWorkingTime>(WorkLogList.OrderByDescending(c => c.StartTime).ToList());
            worklog.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add;
            SelectedWorkLog = worklog;
           
            //OldItemComment = null;
            //NewItemComment = null;
            // }
            // IsNormal = true;

            RequestClose(null, null);

            GeosApplication.Instance.Logger.Log("Method SaveWorkLogAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void AddworklogCancelAction(object obj)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEditWorkOrderItemViewCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //public void CheckDateTimeValidation()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);

        //        //if(!SAMCommon.Instance.IsPermissionReadOnly)
        //       // if (!HrmCommon.Instance.IsPermissionReadOnly)
        //        {
        //            DateTime _TempStartDate = DateTime.Today;
        //            DateTime _TimeEndDate = DateTime.Today;

        //            if ( StartTime != null && EndTime != null)
        //            {
        //             //   _TempStartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
        //               // _TimeEndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
        //            }

        //            //if (allowValidation)
        //            //{
        //            //    error = EnableValidationAndGetError();
        //            //    PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
        //            //    PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
        //            //}
        //        }
        //        GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion

        #region Validation

        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)//[rdixit][GEOS2-4025][24.11.2022]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                //PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                if (error != null)
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnTimeEditValueChanging(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                StartTimeErrorMessage = string.Empty;

                if (StartTime != null && EndTime != null)
                {
                    // if (SelectedEmployeeShift.CompanyShift.IsNightShift != 1)
                    // {

                    if (EndTime < StartTime)
                    {
                        StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddWorkLogStartTimeError").ToString();
                        EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddWorkLogEndTimeError").ToString();
                    }
                    else
                    {
                        StartTimeErrorMessage = string.Empty;
                        EndTimeErrorMessage = string.Empty;
                    }
                    // }
                    //else
                    //{
                    //    StartTimeErrorMessage = string.Empty;
                    //    EndTimeErrorMessage = string.Empty;
                    //}
                }
                else
                {
                    StartTimeErrorMessage = string.Empty;
                    EndTimeErrorMessage = string.Empty;
                }

                CheckDateTimeValidation();

                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void CheckDateTimeValidation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);


                //DateTime _TempStartDate = DateTime.Today;
                // DateTime _TimeEndDate = DateTime.Today;

                if (StartTime != null && EndTime != null)
                {
                    DateTime _TempStartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                    DateTime _TimeEndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);


                    if (allowValidation)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                // string empName = BindableBase.GetPropertyName(() => SelectedIndexForEmployee);
                string WlStartTime = BindableBase.GetPropertyName(() => StartTime);
                string WlEndTime = BindableBase.GetPropertyName(() => EndTime);
                string WlStartDate = BindableBase.GetPropertyName(() => StartDate);
                string WlEndDate = BindableBase.GetPropertyName(() => EndDate);

                if (columnName == WlStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return ItemWorkLogValidation.GetErrorMessage(WlStartDate, StartDate,EndDate);
                    }
                }

                if (columnName == WlEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return ItemWorkLogValidation.GetErrorMessage(WlEndDate, StartDate, EndDate);
                    }
                }
                if (columnName == WlStartTime)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return ItemWorkLogValidation.GetErrorMessage(WlStartTime, StartTime, EndTime);
                    }
                }

                if (columnName == WlEndTime)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return ItemWorkLogValidation.GetErrorMessage(WlEndTime, StartTime, EndTime);
                    }
                }



                return null;
            }
        }
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                // me[BindableBase.GetPropertyName(() => SelectedIndexForEmployee)] +
                me[BindableBase.GetPropertyName(() => StartDate)] +
                me[BindableBase.GetPropertyName(() => EndDate)] +
                me[BindableBase.GetPropertyName(() => StartTime)] +
                me[BindableBase.GetPropertyName(() => EndTime)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
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
        #endregion
    }
}
