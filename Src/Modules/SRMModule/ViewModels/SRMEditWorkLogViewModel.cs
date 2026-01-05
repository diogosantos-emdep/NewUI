using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class SRMEditWorkLogViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion //End Of Services

        #region Public Icommands
        public ICommand EditWorkLogViewCancelButtonCommand { get; set; }
        public ICommand EditWorkLogViewAcceptButtonCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        #endregion

        #region Declarations
        private string windowHeader;
        private bool isSave;
        private List<OTWorkingTime> existWorkLogItemList;
        private DateTime? startTime;
        private string startTimeInHoursAndMinutes;
        private DateTime? endTime;
        private int selectedUserIndex;
        private List<OTAssignedUser> otUserList;
        private string endTimeInHoursAndMinutes;
        private List<Company> company;
        private OTWorkingTime workLogItem;


        private string error = string.Empty;

        private DateTime? startTimeOfDay;
        private DateTime? endTimeOfDay;

        #endregion

        #region Properties
        public OTWorkingTime EditWorkLogItem { get; set; }
        public OTAssignedUser OtAssignUser { get; set; }
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
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
                StartTimeOfDay = StartTime;
            }
        }
        public string StartTimeInHoursAndMinutes
        {
            get
            {
                return startTimeInHoursAndMinutes;
            }

            set
            {
                startTimeInHoursAndMinutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeInHoursAndMinutes"));
            }
        }
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
                EndTimeOfDay = EndTime;
            }
        }

        public string EndTimeInHoursAndMinutes
        {
            get
            {
                return endTimeInHoursAndMinutes;
            }

            set
            {
                endTimeInHoursAndMinutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeInHoursAndMinutes"));
            }
        }

        public OTWorkingTime WorkLogItem
        {
            get
            {
                return workLogItem;
            }
            set
            {
                workLogItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItem"));
            }
        }
        public List<OTWorkingTime> ExistWorkLogItemList
        {
            get
            {
                return existWorkLogItemList;
            }
            set
            {
                existWorkLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistWorkLogItem"));
            }
        }
        public List<OTAssignedUser> OtUserList
        {
            get
            {
                return otUserList;
            }
            set
            {
                otUserList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtUserList"));
            }
        }
        public int SelectedUserIndex
        {
            get
            {
                return selectedUserIndex;
            }

            set
            {
                selectedUserIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUserIndex"));
            }
        }

        public DateTime? StartTimeOfDay
        {
            get { return startTimeOfDay; }
            set
            {
                startTimeOfDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeOfDay"));
            }
        }

        public DateTime? EndTimeOfDay
        {
            get { return endTimeOfDay; }
            set
            {
                endTimeOfDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeOfDay"));
            }
        }
        #endregion

        #region public Events
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

        #region Constructor
        public SRMEditWorkLogViewModel()
        {
            EditWorkLogViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            EditWorkLogViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditWorkLogSave));
            OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
            OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
        }
        #endregion

        #region Methods
        public void EditInit(List<OTWorkingTime> WorkLogItemList, OTWorkingTime OTWorkLog, ObservableCollection<OTAssignedUser> otAssignedUserList, List<Company> OtSite)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("WMSEditWorkLogHeader").ToString();
                company = OtSite;
                WorkLogItem = (OTWorkingTime)OTWorkLog;
                ExistWorkLogItemList = new List<OTWorkingTime>(WorkLogItemList.ToList());

                FillOtUserList(otAssignedUserList);
                if (!OtUserList.Any(x => x.IdUser == OTWorkLog.IdOperator))
                {
                    OTAssignedUser otuser = new OTAssignedUser();
                    otuser.IdUser = OTWorkLog.IdOperator;
                    otuser.UserShortDetail = new UserShortDetail();
                    otuser.UserShortDetail.IdUser = OTWorkLog.IdOperator;
                    otuser.UserShortDetail.UserName = OTWorkLog.UserShortDetail.UserName;
                    otuser.IsEnabled = false;
                    OtUserList.Add(otuser);
                }

                SelectedUserIndex = OtUserList.FindIndex(x => x.IdUser == OTWorkLog.IdOperator);
                StartTime = OTWorkLog.StartTime;
                EndTime = OTWorkLog.EndTime;
                StartTimeOfDay = OTWorkLog.StartTime;
                EndTimeOfDay = OTWorkLog.EndTime;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

        }

        private void FillOtUserList(ObservableCollection<OTAssignedUser> otAssignedUserList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOtUserList()...", category: Category.Info, priority: Priority.Low);

                IList<OTAssignedUser> tempOtUserList = otAssignedUserList;
                OtUserList = new List<OTAssignedUser>();
                OtUserList = new List<OTAssignedUser>(tempOtUserList);
                OtUserList.OrderBy(i => i.UserShortDetail.UserName);

                GeosApplication.Instance.Logger.Log("Method FillOtUserList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            //catch (FaultException<ServiceException> ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in Method FillOtUserList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            //}
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOtUserList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOtUserList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void EditWorkLogSave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWorkLogSave()...", category: Category.Info, priority: Priority.Low);

                // error = EnableValidationAndGetError();
                allowValidation = true;
                error = EnableValidationAndGetError();
                // PropertyChanged(this, new PropertyChangedEventArgs("SelectedUserIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTimeOfDay"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTimeOfDay"));

                if (error != null)
                {
                    return;
                }

                var startTime = new DateTime(StartTime.Value.Year, StartTime.Value.Month, StartTime.Value.Day, StartTimeOfDay.Value.Hour, StartTimeOfDay.Value.Minute, StartTimeOfDay.Value.Second);
                var endTime = new DateTime(EndTime.Value.Year, EndTime.Value.Month, EndTime.Value.Day, EndTimeOfDay.Value.Hour, EndTimeOfDay.Value.Minute, EndTimeOfDay.Value.Second);

                EditWorkLogItem = new OTWorkingTime() { UserShortDetail = OtUserList[SelectedUserIndex].UserShortDetail, StartTime = startTime, EndTime = endTime, TransactionOperation = ModelBase.TransactionOperations.Update };



                IsSave = true;

                foreach (OTWorkingTime worklogitem in ExistWorkLogItemList)
                {
                    if (EditWorkLogItem.UserShortDetail.IdUser == worklogitem.IdOperator)
                    {
                        if (WorkLogItem.IdOTWorkingTime != worklogitem.IdOTWorkingTime)
                        {
                            if (worklogitem.StartTime < EditWorkLogItem.EndTime && worklogitem.EndTime > EditWorkLogItem.StartTime)
                            {
                                IsSave = false;
                                break;
                            }
                            else
                            {
                                IsSave = true;
                            }
                        }
                    }
                }

                if (IsSave)
                {
                    EditWorkLogItem.UserShortDetail.UserImage = null;
                    WorkLogItem.UserShortDetail = EditWorkLogItem.UserShortDetail;
                    WorkLogItem.IdOperator = EditWorkLogItem.UserShortDetail.IdUser;
                    WorkLogItem.StartTime = EditWorkLogItem.StartTime;
                    WorkLogItem.EndTime = EditWorkLogItem.EndTime;
                    EditWorkLogItem.TotalTime = (EditWorkLogItem.EndTime).Value - (EditWorkLogItem.StartTime).Value;
                    foreach (var item in company)
                    {
                        bool result = SRMService.UpdateWorkLog_V2540(item, WorkLogItem);
                    }
                   
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method EditWorkLogSave()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogOverlaped").ToString(), EditWorkLogItem.UserShortDetail.UserName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method EditWorkLogSave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startDateErrorMessage = string.Empty;

                if (StartTime != null && EndTime != null)
                {
                    if (StartTime.Value.Date > EndTime.Value.Date)
                    {
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("WMSAddHolidayStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("WMSAddHolidayEndDateError").ToString();
                    }
                    else
                    {
                        startDateErrorMessage = string.Empty;
                        endDateErrorMessage = string.Empty;
                    }

                }
                else
                {
                    startDateErrorMessage = string.Empty;
                    endDateErrorMessage = string.Empty;
                }

                CheckDateTimeValidation();

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnTimeEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging ...", category: Category.Info, priority: Priority.Low);


                startTimeErrorMessage = string.Empty;

                if (StartTimeOfDay != null && EndTimeOfDay != null)
                {

                    TimeSpan _StartTime = StartTimeOfDay.Value.TimeOfDay;
                    TimeSpan _EndTime = EndTimeOfDay.Value.TimeOfDay;
                    if (_StartTime > _EndTime)
                    {
                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("WorkLogStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("WorkLogEndTimeError").ToString();
                    }
                    else
                    {
                        startTimeErrorMessage = string.Empty;
                        endTimeErrorMessage = string.Empty;
                    }
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }


                CheckDateTimeValidation();
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CheckDateTimeValidation()
        {
            try
            {
                if (StartTimeOfDay != null && EndTimeOfDay != null)
                {
                    var _StartTimeHours = StartTimeOfDay.Value.TimeOfDay.Hours;
                    var _StartTimeMinutes = StartTimeOfDay.Value.TimeOfDay.Minutes;
                    var _EndTimeHours = EndTimeOfDay.Value.TimeOfDay.Hours;
                    var _EndTimeMinutes = EndTimeOfDay.Value.TimeOfDay.Minutes;
                    DateTime _TempStartTime = StartTimeOfDay.Value.Date.AddHours(_StartTimeHours).AddMinutes(_StartTimeMinutes);
                    DateTime _TempEndTime = EndTimeOfDay.Value.Date.AddHours(_EndTimeHours).AddMinutes(_EndTimeMinutes);

                    if (_TempStartTime > _TempEndTime)
                    {
                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("WorkLogStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("WorkLogEndTimeError").ToString();
                    }
                    else
                    {
                        startTimeErrorMessage = string.Empty;
                        endTimeErrorMessage = string.Empty;
                    }
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }

                EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTimeOfDay"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTimeOfDay"));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validations
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;
        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => StartTime)] +
                    me[BindableBase.GetPropertyName(() => EndTime)] +
                     me[BindableBase.GetPropertyName(() => StartTimeOfDay)] +
                    me[BindableBase.GetPropertyName(() => EndTimeOfDay)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string startDate = BindableBase.GetPropertyName(() => StartTime);
                string endDate = BindableBase.GetPropertyName(() => EndTime);
                string startTimeOfDay = BindableBase.GetPropertyName(() => StartTimeOfDay);
                string endTimeOfDay = BindableBase.GetPropertyName(() => EndTimeOfDay);

                if (columnName == startDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    //else if (!string.IsNullOrEmpty(startTimeErrorMessage))
                    //{
                    //    return startTimeErrorMessage;
                    //}
                    else
                    {
                        return WorkLogValidation.GetErrorMessage(startDate, StartTime);
                    }
                }

                if (columnName == endDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    //else if (!string.IsNullOrEmpty(endTimeErrorMessage))
                    //{
                    //    return endTimeErrorMessage;
                    //}
                    else
                    {
                        return WorkLogValidation.GetErrorMessage(endDate, EndTime);
                    }


                }

                if (columnName == startTimeOfDay)
                {
                    if (!string.IsNullOrEmpty(endTimeErrorMessage))
                    {
                        return endTimeErrorMessage;
                    }
                    else
                    {
                        return WorkLogValidation.GetErrorMessage(startTimeOfDay, StartTimeOfDay);
                    }


                }
                if (columnName == endTimeOfDay)
                {
                    if (!string.IsNullOrEmpty(endTimeErrorMessage))
                    {
                        return endTimeErrorMessage;
                    }
                    else
                    {
                        return WorkLogValidation.GetErrorMessage(endTimeOfDay, EndTimeOfDay);
                    }


                }

                return null;
            }
        }
        #endregion
    }
}
