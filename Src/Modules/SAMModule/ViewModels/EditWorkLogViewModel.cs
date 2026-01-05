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

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    /// <summary>
    /// [000][20200727]][srowlo][GEOS2-2374] Add a new permission to Edit work log entries
    /// new Page added
    /// </summary>
    class EditWorkLogViewModel : INotifyPropertyChanged
    {

        #region Services

        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        private Company company;
        private OTWorkingTime workLogItem;
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
        public List<OTWorkingTime> ExistWorkLogItemList {
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
        public EditWorkLogViewModel()
        {
            EditWorkLogViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            EditWorkLogViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditWorkLogSave));
        }
        #endregion

        #region Methods
        public void EditInit(List<OTWorkingTime> WorkLogItemList, OTWorkingTime OTWorkLog, ObservableCollection<OTAssignedUser> otAssignedUserList,Company OtSite)
        {            
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditWorkLogHeader").ToString();
                company = OtSite;
                WorkLogItem =  (OTWorkingTime)OTWorkLog;
                ExistWorkLogItemList = new List<OTWorkingTime>(WorkLogItemList.ToList());
                                
                FillOtUserList(otAssignedUserList);           
                if(!OtUserList.Any(x=>x.IdUser==OTWorkLog.IdOperator))
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
                
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUserIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                 
                EditWorkLogItem = new OTWorkingTime() { UserShortDetail= OtUserList[SelectedUserIndex].UserShortDetail, StartTime = StartTime,EndTime=EndTime, TransactionOperation = ModelBase.TransactionOperations.Update };

                

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

                    bool result = SAMService.UpdateWorkLog(company, WorkLogItem);
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

  
        #endregion
    }
}
