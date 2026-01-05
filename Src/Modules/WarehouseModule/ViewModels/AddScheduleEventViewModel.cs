using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Epc;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.XtraRichEdit.Model;
using System.Windows.Shell;
using Emdep.Geos.Data.Common.WMS;
using System.Drawing;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.XtraPrinting;
using DevExpress.XtraSpreadsheet.Import.OpenXml;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[nsatpute][22.09.2025][GEOS2-8795]
    internal class AddScheduleEventViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("10.13.3.33:99");
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Commands        
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }

        public ICommand CheckChangedCommand { get; set; }

        #endregion


        #region Declaration
        private bool isBusy;
        private bool isSave;
        private string windowHeader;
        private ObservableCollection<LookupValue> eventTypeList;
        private ObservableCollection<LookupValue> logisticList;
        private DateTime startDate;
        private DateTime startTime;
        private DateTime endDate;
        private DateTime endTime;
        private short selectedIndexForEventType;
        private short selectedIndexForLogistic;
        private string observations;
        private bool isLogisticEnable;
        private bool allDayEventChecked;
        private bool isEndDateTimeEnable;
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isSave"));
            }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public ObservableCollection<LookupValue> EventTypeList
        {
            get { return eventTypeList; }
            set
            {
                eventTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EventTypeList"));
            }
        }
        public ObservableCollection<LookupValue> LogisticList
        {
            get { return logisticList; }
            set
            {
                logisticList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LogisticList"));
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                if (!checkChanged)
                {
                    if (AllDayEventChecked)
                    {
                        startDate = StartTime = StartDate.Date;
                        EndDate = EndTime = StartDate.Date.AddDays(1).AddSeconds(-1);
                        IsEndDateTimeEnable = false;
                    }
                    else
                    {
                        IsEndDateTimeEnable = true;
                    }
                }
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public short SelectedIndexForEventType
        {
            get { return selectedIndexForEventType; }
            set
            {
                selectedIndexForEventType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEventType"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                if (SelectedIndexForEventType != -1 && EventTypeList[SelectedIndexForEventType].IdLookupValue == 2289)
                {
                    IsLogisticEnable = false;
                    SelectedIndexForLogistic = -1;
                }
                else
                {
                    IsLogisticEnable = true;
                }
            }
        }

        public short SelectedIndexForLogistic
        {
            get { return selectedIndexForLogistic; }
            set
            {
                selectedIndexForLogistic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForLogistic"));
            }
        }

        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Observations"));
            }
        }
        //[nsatpute][16.10.2025][GEOS2-8799]
        public bool IsLogisticEnable
        {
            get { return isLogisticEnable; }
            set
            {
                isLogisticEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLogisticEnable"));
            }
        }
        public bool AllDayEventChecked
        {
            get { return allDayEventChecked; }
            set
            {
                allDayEventChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllDayEventChecked"));
            }
        }
        public bool IsEndDateTimeEnable
        {
            get { return isEndDateTimeEnable; }
            set
            {
                isEndDateTimeEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEndDateTimeEnable"));
            }
        }

        public ScheduleEvent ScheduleEvent;
        public ObservableCollection<MonthlyData> MonthList;
        private bool firstErrorCallObservation = false;
        private bool firstErrorCallType = false;
        private bool firstErrorCallLogistic = false;
        private bool checkChanged = false; 
        #endregion
        //[nsatpute][14.10.2025][GEOS2-8799]
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Observations" && !firstErrorCallObservation)
                {
                    firstErrorCallObservation = true;
                    return string.Empty;
                }
                if (columnName == "SelectedIndexForEventType" && !firstErrorCallType)
                {
                    firstErrorCallType = true;
                    return string.Empty;
                }
                if (columnName == "SelectedIndexForLogistic" && !firstErrorCallLogistic)
                {
                    firstErrorCallLogistic = true;
                    return string.Empty;
                }
                // [nsatpute][07.10.2025][GEOS2-8797]
                DateTime sDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                DateTime eDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
               
                if (columnName == "EndDate" && eDate < sDate)
                {
                    return "Start date should not be grater than end Date";
                }
                if (columnName == "EndDate" && eDate.Date != sDate.Date && SelectedIndexForLogistic != -1)
                {
                    return "Except for the Inventory event, the Start date and End date should be the same.";
                }
                else if (columnName == "StartDate" && eDate < sDate)
                {

                    return "Start date should not be grater than end Date";
                }
                //else if (columnName == "Observations" && (Observations == null || Observations.Trim() == string.Empty))
                //{
                //    return "Observation should not be empty.";
                //}
                else if (columnName == "SelectedIndexForEventType" && SelectedIndexForEventType == -1)
                {
                    return "Event Type should not be empty.";
                }
                else if (columnName == "SelectedIndexForLogistic" && SelectedIndexForLogistic == -1 && IsLogisticEnable)
                {
                    return "Logostic should not be empty.";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string Error
        {
            get { return string.Empty; }
        }

        #region Constructor
        public AddScheduleEventViewModel()
        {
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CheckChangedCommand = new RelayCommand(new Action<object>(CheckChangedCommandAction));
        }
        #endregion

        #region Methods
        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEventType"));
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForLogistic"));
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
                if (Validation())
                    return;

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction(object obj)\r\n()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;

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

                Save();

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void CheckChangedCommandAction(object e)
        {
            try
            {                
                GeosApplication.Instance.Logger.Log("Method CheckChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                checkChanged = true;
                if (AllDayEventChecked)
                {
                    StartDate = StartTime = StartDate.Date;
                    EndDate = EndTime = StartDate.Date.AddDays(1).AddSeconds(-1);
                    IsEndDateTimeEnable = false;
                }
                else
                {
                    IsEndDateTimeEnable = true;
                }
                checkChanged = false;

            }
            catch (Exception ex)
            {
                checkChanged = false;
                IsEndDateTimeEnable = true;
                GeosApplication.Instance.Logger.Log("Error in CheckChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method CheckChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //[nsatpute][16.10.2025][GEOS2-8799]
        private void Save()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Save()...", category: Category.Info, priority: Priority.Low);
                if (Observations == null)
                    Observations = " ";
                ScheduleEvent = new ScheduleEvent();
                ScheduleEvent.StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                ScheduleEvent.EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
                ScheduleEvent.IdTypeEvent = EventTypeList[SelectedIndexForEventType].IdLookupValue;
                if (IsLogisticEnable)
                    ScheduleEvent.IdLogistic = LogisticList[SelectedIndexForLogistic].IdLookupValue;
                ScheduleEvent.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                ScheduleEvent.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                ScheduleEvent.Observations = Observations;
                IsSave = WarehouseService.SaveWarehouseScheduleEvent(ScheduleEvent);
                ScheduleEvent.ButtonColor = (SolidColorBrush)new BrushConverter().ConvertFrom(EventTypeList[SelectedIndexForEventType].HtmlColor);
                if (IsSave)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method Save()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Save " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Save " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Save()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][22.09.2025][GEOS2-8795]
        public void Init(ObservableCollection<MonthlyData> MonthList, ObservableCollection<LookupValue> EventTypeList)
        {
            this.MonthList = MonthList;
            this.EventTypeList = EventTypeList;
            FillLogisticList();
            SelectedIndexForEventType = -1;
            SelectedIndexForLogistic = -1;
        }

        //[nsatpute][22.09.2025][GEOS2-8795]
        //[nsatpute][14.10.2025][GEOS2-8799]
        private void FillLogisticList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()...", category: Category.Info, priority: Priority.Low);
                LogisticList = new ObservableCollection<LookupValue>();
                IList<LookupValue> temptypeList = CrmService.GetLookupValues(101);
                LogisticList = new ObservableCollection<LookupValue>(temptypeList);
                GeosApplication.Instance.Logger.Log("Method FillLogisticList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLogisticList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][07.10.2025][GEOS2-8797]
        private bool Validation()
        {
            DateTime sDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
            DateTime eDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
            if (eDate < sDate)
            {
                return true;
            }
            //else if (Observations == null || Observations.Trim() == string.Empty)
            //{
            //    return true;
            //}
            else if (SelectedIndexForEventType == -1)
            {
                return true;
            }
            else if (SelectedIndexForLogistic == -1 && IsLogisticEnable)
            {
                return true;
            }
            if ( EndDate.Date != startDate.Date && SelectedIndexForLogistic != -1)
            {
                return true;
            }
            else if (MultipleEventValidation())
            {
                return true;
            }
            else
            {

                if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Youareunabletoaddeventsd").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        //[nsatpute][27.10.2025][GEOS2-8799]
        private bool MultipleEventValidation()
        {
            bool sameDayEvent = false;
            DateTime eventDate = StartDate.Date;
            ScheduleEvent se = null;
            foreach (MonthlyData md in MonthList)
            {
                if (startDate.Month == 01)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.JanuaryScheduleEvents != null && md.JanuaryScheduleEvents.Count > 0)
                        {
                            if (md.JanuaryScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.JanuaryScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JanuaryScheduleEvents[0].IdLogistic == 0)
                                {
                                    ShowValidationMessage(); return true;
                                }
                                if (md.JanuaryScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 02)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.FebruaryScheduleEvents != null && md.FebruaryScheduleEvents.Count > 0)
                        {
                            if (md.FebruaryScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }
                            if (md.FebruaryScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.FebruaryScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.FebruaryScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 03)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.MarchScheduleEvents != null && md.MarchScheduleEvents.Count > 0)
                        {
                            if (md.MarchScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }
                            if (md.MarchScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.MarchScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.MarchScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 04)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.AprilScheduleEvents != null && md.AprilScheduleEvents.Count > 0)
                        {
                            if (md.AprilScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.AprilScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.AprilScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.AprilScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 05)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.MayScheduleEvents != null && md.MayScheduleEvents.Count > 0)
                        {
                            if (md.MayScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.MayScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.MayScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.MayScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 06)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.JuneScheduleEvents != null && md.JuneScheduleEvents.Count > 0)
                        {
                            if (md.JuneScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.JuneScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.JuneScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JuneScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 07)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.JulyScheduleEvents != null && md.JulyScheduleEvents.Count > 0)
                        {
                            if (md.JulyScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.JulyScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.JulyScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JulyScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 08)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.AugustScheduleEvents != null && md.AugustScheduleEvents.Count > 0)
                        {
                            if (md.AugustScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.AugustScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.AugustScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.AugustScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 09)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.SeptemberScheduleEvents != null && md.SeptemberScheduleEvents.Count > 0)
                        {
                            if (md.SeptemberScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.SeptemberScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.SeptemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.SeptemberScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 10)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.OctoberScheduleEvents != null && md.OctoberScheduleEvents.Count > 0)
                        {
                            if (md.OctoberScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.OctoberScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.OctoberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.OctoberScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 11)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.NovemberScheduleEvents != null && md.NovemberScheduleEvents.Count > 0)
                        {
                            if (md.NovemberScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }

                            if (md.NovemberScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.NovemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.NovemberScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }

                if (startDate.Month == 12)
                {
                    if (md.Day == eventDate.Day)
                    {
                        if (md.DecemberScheduleEvents != null && md.DecemberScheduleEvents.Count > 0)
                        {
                            if (md.DecemberScheduleEvents[0].IdLogistic == 0)
                            {
                                ShowValidationMessage(); return true;
                            }
                            if (md.DecemberScheduleEvents.Count == 2)
                            {
                                ShowValidationMessage(); return true;
                            }
                            else if (md.DecemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.DecemberScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    ShowValidationMessage(); return true;
                                }
                            }
                            else
                            {
                                ShowValidationMessage(); return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private void ShowValidationMessage()
        {            
            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Transportfrequencyyearview_Pleasechooseanotherday").ToString(), Environment.NewLine, EventTypeList[SelectedIndexForEventType].Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        }
        #endregion

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
    }
}

