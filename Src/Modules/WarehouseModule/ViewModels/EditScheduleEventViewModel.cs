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
using DevExpress.Mvvm.Native;
using DevExpress.Data.Extensions;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    //[nsatpute][14.10.2025][GEOS2-8799]
    internal class EditScheduleEventViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("10.13.3.33:99");
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Commands        
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand EditButtonCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
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
        private DateTime creationDate;
        private DateTime? modificationDate;
        private int selectedIndexForEventType;
        private int selectedIndexForLogistic;
        private string observations;
        private string logistic;
        private string createdByPerson;
        private string modifiedByPerson;
        private Visibility editDeleteButtonVisibility;
        private bool allDayEventChecked;
        private bool checkChanged = false;
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
        //[nsatpute][16.10.2025][GEOS2-8799]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDate"));
            }
        }
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModificationDate"));
            }
        }
        public int SelectedIndexForEventType
        {
            get { return selectedIndexForEventType; }
            set
            {
                selectedIndexForEventType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEventType"));
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public int SelectedIndexForLogistic
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
        public string Logistic
        {
            get { return logistic; }
            set
            {
                logistic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Logistic"));
            }
        }
        //[nsatpute][16.10.2025][GEOS2-8799]
        public string CreatedByPerson
        {
            get { return createdByPerson; }
            set
            {
                createdByPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedByPerson"));
            }
        }
        public string ModifiedByPerson
        {
            get { return modifiedByPerson; }
            set
            {
                modifiedByPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedByPerson"));
            }
        }
        public Visibility EditDeleteButtonVisibility
        {
            get { return editDeleteButtonVisibility; }
            set
            {
                editDeleteButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditDeleteButtonVisibility"));
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
        private bool firstErrorCall = false;
        #endregion

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Observations" && !firstErrorCall)
                {
                    firstErrorCall = true;
                    return string.Empty;
                }
                // [nsatpute][07.10.2025][GEOS2-8797]
                DateTime sDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                DateTime eDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);

                if (columnName == "EndDate" && eDate < sDate)
                {
                    return "Start date should not be grater than end Date";
                }
                if (columnName == "EndDate" && StartDate.Date != EndDate.Date && Logistic.Trim() != string.Empty)
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
                else if (columnName == "SelectedIndexForLogistic" && SelectedIndexForLogistic == -1)
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
        public EditScheduleEventViewModel()
        {
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            EditButtonCommand = new RelayCommand(new Action<object>(EditButtonCommandAction));
            DeleteButtonCommand = new RelayCommand(new Action<object>(DeleteButtonCommandAction));
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
                OnPropertyChanged(new PropertyChangedEventArgs("Observations"));

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
                //[nsatpute][16.10.2025][GEOS2-8799]
                UpdateScheduleEvent();

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
		//[nsatpute][05-11-2025][GEOS2-8799]
        public void EditButtonCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWarehouseScheduleEvent ...", category: Category.Info, priority: Priority.Low);
                EditSingleScheduleEventViewModel editSingleScheduleEventViewModel = new EditSingleScheduleEventViewModel();


                editSingleScheduleEventViewModel.Init(MonthList, EventTypeList, ScheduleEvent);
                EditSingleScheduleEventView editSingleScheduleEventView = new EditSingleScheduleEventView();
                EventHandler handle = delegate { editSingleScheduleEventView.Close(); };
                editSingleScheduleEventViewModel.RequestClose += handle;
                editSingleScheduleEventView.DataContext = editSingleScheduleEventViewModel;
                editSingleScheduleEventView.ShowDialogWindow();
                //[nsatpute][16.10.2025][GEOS2-8799]
                if (editSingleScheduleEventViewModel.IsSave)
                {
                    this.IsSave = true;
                    StartDate = editSingleScheduleEventViewModel.ScheduleEvent.StartDate;
                    StartTime = editSingleScheduleEventViewModel.ScheduleEvent.StartDate;
                    EndDate = editSingleScheduleEventViewModel.ScheduleEvent.EndDate;
                    EndTime = editSingleScheduleEventViewModel.ScheduleEvent.EndDate;

                    if (editSingleScheduleEventViewModel.ScheduleEvent.IdLogistic == 0)
                        Logistic = string.Empty;
                    else
                        Logistic = LogisticList.FirstOrDefault(x => x.IdLookupValue == editSingleScheduleEventViewModel.ScheduleEvent.IdLogistic).Value;

                    Observations = editSingleScheduleEventViewModel.ScheduleEvent.Observations;

                    if (editSingleScheduleEventViewModel.ScheduleEvent.ModificationDate != DateTime.MinValue)
                        ModificationDate = editSingleScheduleEventViewModel.ScheduleEvent.ModificationDate;
                    CreatedByPerson = editSingleScheduleEventViewModel.ScheduleEvent.CreatedByPerson;
                    ModifiedByPerson = editSingleScheduleEventViewModel.ScheduleEvent.ModifiedByPerson;
                }
                GeosApplication.Instance.Logger.Log("Method EditWarehouseScheduleEvent() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditWarehouseScheduleEvent() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[nsatpute][16.10.2025][GEOS2-8799]
        private void UpdateScheduleEvent()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateScheduleEvent()...", category: Category.Info, priority: Priority.Low);
                DateTime modifiedDate = DateTime.Now;
                ScheduleEvent sEvent = new ScheduleEvent();
                sEvent.IdWarehouseSchedule = ScheduleEvent.IdWarehouseSchedule;
                ScheduleEvent.StartDate = sEvent.StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                ScheduleEvent.EndDate = sEvent.EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
                ScheduleEvent.ModifiedBy = sEvent.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                ScheduleEvent.ModifiedByPerson = GeosApplication.Instance.ActiveUser.FullName;
                ScheduleEvent.Observations = sEvent.Observations = Observations;
                ScheduleEvent.ModificationDate = sEvent.ModificationDate = modifiedDate;
                IsSave = WarehouseService.UpdateWarehouseScheduleEvent(sEvent);
                ScheduleEvent.ButtonColor = (SolidColorBrush)new BrushConverter().ConvertFrom(EventTypeList[SelectedIndexForEventType].HtmlColor);
                if (IsSave)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Editscheduleeventview_Scheduleeventupdatedsuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method UpdateScheduleEvent....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateScheduleEvent " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateScheduleEvent " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateScheduleEvent()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][16.10.2025][GEOS2- 8799]
        public void DeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction(object obj)\r\n()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["Editscheduleeventview_Areyousureyouwanttodelet"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
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

                    DeleteScheduleEvent();
                }
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][16.10.2025][GEOS2-8799]
        private void DeleteScheduleEvent()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteScheduleEvent()...", category: Category.Info, priority: Priority.Low);
                ScheduleEvent schEvent = new ScheduleEvent();
                schEvent.IdWarehouseSchedule = ScheduleEvent.IdWarehouseSchedule;
                IsSave = WarehouseService.DeleteWarehouseScheduleEvent(schEvent);
                ScheduleEvent.ButtonColor = (SolidColorBrush)new BrushConverter().ConvertFrom(EventTypeList[SelectedIndexForEventType].HtmlColor);
                if (IsSave)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteScheduleEvent()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteScheduleEvent " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteScheduleEvent " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteScheduleEvent()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        //[nsatpute][16.10.2025][GEOS2-8799]
        public void Init(ObservableCollection<MonthlyData> MonthList, ObservableCollection<LookupValue> EventTypeList, ScheduleEvent sEvent)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                this.MonthList = MonthList;
                this.EventTypeList = EventTypeList;
                this.ScheduleEvent = sEvent;
                StartDate = sEvent.StartDate;
                StartTime = sEvent.StartDate;
                EndDate = sEvent.EndDate;
                EndTime = sEvent.EndDate;
                FillLogisticList();
                SelectedIndexForEventType = EventTypeList.FindIndex(x => x.IdLookupValue == sEvent.IdTypeEvent);
                if (sEvent.IdLogistic == 0)
                    Logistic = string.Empty;
                else
                    Logistic = LogisticList.FirstOrDefault(x => x.IdLookupValue == sEvent.IdLogistic).Value;
                Observations = sEvent.Observations;
                CreationDate = sEvent.CreationDate;
                if (sEvent.ModificationDate != DateTime.MinValue)
                    ModificationDate = sEvent.ModificationDate;
                CreatedByPerson = sEvent.CreatedByPerson;
                ModifiedByPerson = sEvent.ModifiedByPerson;
                if (sEvent.IsDone || !GeosApplication.Instance.IsWMS_SchedulePermission)
                    EditDeleteButtonVisibility = Visibility.Collapsed;
                else
                    EditDeleteButtonVisibility = Visibility.Visible;
                WindowHeader = System.Windows.Application.Current.FindResource("Editscheduleeventview_Viewevent").ToString() + " " + EventTypeList.FirstOrDefault(x => x.IdLookupValue == sEvent.IdTypeEvent);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[nsatpute][22.09.2025][GEOS2-8795]
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
            if (EndDate.Date != startDate.Date && Logistic != string.Empty)
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
            else if (SelectedIndexForLogistic == -1)
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
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.JanuaryScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JanuaryScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.JanuaryScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                                if (md.JanuaryScheduleEvents[0].IdLogistic == LogisticList[SelectedIndexForLogistic].IdLookupValue)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.JanuaryScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.FebruaryScheduleEvents[0].IdLogistic == 0 && md.FebruaryScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            if (md.FebruaryScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.FebruaryScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.FebruaryScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.FebruaryScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.FebruaryScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.MarchScheduleEvents[0].IdLogistic == 0 && md.MarchScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            if (md.MarchScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.MarchScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.MarchScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.MarchScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.MarchScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.AprilScheduleEvents[0].IdLogistic == 0 && md.AprilScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.AprilScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.AprilScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.AprilScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.AprilScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.AprilScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.MayScheduleEvents[0].IdLogistic == 0 && md.MayScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.MayScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.MayScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.MayScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.MayScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.MayScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.JuneScheduleEvents[0].IdLogistic == 0 && md.JuneScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.JuneScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.JuneScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JuneScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.JuneScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.JuneScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.JulyScheduleEvents[0].IdLogistic == 0 && md.JulyScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.JulyScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.JulyScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.JulyScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.JulyScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.JulyScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.AugustScheduleEvents[0].IdLogistic == 0 && md.AugustScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.AugustScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.AugustScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.AugustScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.AugustScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.AugustScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.SeptemberScheduleEvents[0].IdLogistic == 0 && md.SeptemberScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.SeptemberScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.SeptemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.SeptemberScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.SeptemberScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.SeptemberScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.OctoberScheduleEvents[0].IdLogistic == 0 && md.OctoberScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.OctoberScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.OctoberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.OctoberScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.OctoberScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.OctoberScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.NovemberScheduleEvents[0].IdLogistic == 0 && md.NovemberScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }

                            if (md.NovemberScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.NovemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.NovemberScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.NovemberScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.NovemberScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
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
                            if (md.DecemberScheduleEvents[0].IdLogistic == 0 && md.DecemberScheduleEvents[0].StartDate.Date != StartDate.Date)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Editscheduleeventview_Itisnotallowedtohavemore").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            if (md.DecemberScheduleEvents.Count == 2)
                            {
                                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Itisnotallowedtohavetwo").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                            else if (md.DecemberScheduleEvents[0].IdTypeEvent == EventTypeList[SelectedIndexForEventType].IdLookupValue)
                            {
                                if (md.DecemberScheduleEvents[0].IdLogistic == ScheduleEvent.IdLogistic && md.DecemberScheduleEvents[0].IdWarehouseSchedule != ScheduleEvent.IdWarehouseSchedule)
                                {
                                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thelogistictypeoftheevent").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return true;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Addscheduleeventview_Thetypeofeventforthespec").ToString(), EventTypeList.FirstOrDefault(x => x.IdLookupValue == md.DecemberScheduleEvents[0].IdTypeEvent).Value), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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
