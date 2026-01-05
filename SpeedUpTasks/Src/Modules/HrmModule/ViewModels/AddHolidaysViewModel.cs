using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
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
using System.Windows.Input;
using Emdep.Geos.Data.Common.Hrm;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Editors;
using System.Globalization;
using DevExpress.Xpf.Core;

/// <summary>
/// Sprint 41--HRM	M041-07-New configuration section Holidays--To add new holiday ---sdesai
/// </summary>
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddHolidaysViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private bool isNew;
        private string windowHeader;
        private string holidayName;
        private ObservableCollection<CompanyHoliday> existCompanyHolidaysList;
        private bool isSave;
        private int selectedIndexHolidayType;
        private DateTime? startDate;
        private DateTime? endDate;
        private int Recursive;
        private bool inUse;
        CompanyHoliday Holiday;
        private List<DateTime> _FromDates;
        private bool invertIsAllDayEvent;
        private List<DateTime> _ToDates;
        private string timeEditMask;
        private bool isAllDayEvent;
        private DateTime? startTime;
        private DateTime? endTime;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;
        private TimeSpan sTime;
        private TimeSpan eTime;
        private Company company;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties

        public List<LookupValue> HolidaysTypeList { get; set; }
        public Int32 WorkingPlantId { get; set; }
        public CompanyHoliday NewHoliday { get; set; }
        public CompanyHoliday UpdateHoliday { get; set; }

        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public int SelectedIndexHolidayType
        {
            get
            {
                return selectedIndexHolidayType;
            }

            set
            {
                selectedIndexHolidayType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexHolidayType"));
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

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

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

        public string HolidayName
        {
            get
            {
                return holidayName;
            }

            set
            {
                holidayName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HolidayName"));
            }
        }

        public ObservableCollection<CompanyHoliday> ExistCompanyHolidaysList
        {
            get
            {
                return existCompanyHolidaysList;
            }

            set
            {
                existCompanyHolidaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistCompanyHolidaysList"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("_FromDates"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("_ToDates"));
                }
            }
        }

        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        public bool InvertIsAllDayEvent
        {
            get { return invertIsAllDayEvent; }
            set
            {
                invertIsAllDayEvent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InvertIsAllDayEvent"));
            }
        }

        public bool IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllDayEvent"));

                if (value)
                {
                    InvertIsAllDayEvent = false;
                    StartTime = FromDates[0];
                    EndTime = ToDates[0];
                }
                else
                {
                    InvertIsAllDayEvent = true;
                }
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

        public string StartTimeErrorMessage
        {
            get
            {
                return startTimeErrorMessage;
            }

            set
            {
                startTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeErrorMessage"));
            }
        }

        public string EndTimeErrorMessage
        {
            get
            {
                return endTimeErrorMessage;
            }

            set
            {
                endTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeErrorMessage"));
            }
        }

        public TimeSpan STime
        {
            get
            {
                return sTime;
            }

            set
            {
                sTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("STime"));
            }
        }

        public TimeSpan ETime
        {
            get
            {
                return eTime;
            }

            set
            {
                eTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETime"));
            }
        }

        public Company Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Company"));
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

        #region Public ICommand
        public ICommand AddHolidaysViewCancelButtonCommand { get; set; }
        public ICommand AddHolidayViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnTimeEditValueChangingCommand { get; set; }
        public ICommand CheckedCommand { get; set; }

        public ICommand CommandTextInput { get; set; }

        #endregion

        #region Constructor
        public AddHolidaysViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddHolidaysViewModel()...", category: Category.Info, priority: Priority.Low);

                AddHolidaysViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddHolidayViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddHoliday));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                OnTimeEditValueChangingCommand = new DelegateCommand<RoutedEventArgs>(OnTimeEditValueChanging);
                CheckedCommand = new RelayCommand(new Action<object>(CheckedCommandAction));
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillFromDates();
                FillToDates();
                InvertIsAllDayEvent = true;

                GeosApplication.Instance.Logger.Log("Constructor AddHolidaysViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddHolidaysViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                FillHolidaysTypeList();
                IsAllDayEvent = true;
                //Company = Company;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Fill Holidays Type List
        /// </summary>
        private void FillHolidaysTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillHolidaysTypeList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(28);
                HolidaysTypeList = new List<LookupValue>();
                HolidaysTypeList = new List<LookupValue>(temptypeList);
                HolidaysTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                GeosApplication.Instance.Logger.Log("Method FillHolidaysTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillHolidaysTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ExistHolidays(ObservableCollection<CompanyHoliday> CompanyHolidaysDetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExistHolidays()...", category: Category.Info, priority: Priority.Low);

                ExistCompanyHolidaysList = new ObservableCollection<CompanyHoliday>(CompanyHolidaysDetails);

                GeosApplication.Instance.Logger.Log("Method ExistHolidays()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExistHolidays()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method EditHoliday to Edit Holiday
        /// </summary>
        public void EditHoliday(ObservableCollection<CompanyHoliday> CompanyHolidaysDetails, CompanyHoliday holiday)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditHoliday()...", category: Category.Info, priority: Priority.Low);
                ExistCompanyHolidaysList = new ObservableCollection<CompanyHoliday>(CompanyHolidaysDetails);
                FillHolidaysTypeList();
                Holiday = holiday;
                HolidayName = holiday.Name;
                SelectedIndexHolidayType = HolidaysTypeList.FindIndex(x => x.IdLookupValue == holiday.IdHoliday);
                StartDate = holiday.StartDate;
                EndDate = holiday.EndDate;

                if (!Convert.ToBoolean(holiday.IsAllDayEvent))
                {
                    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
                }

                IsAllDayEvent = Convert.ToBoolean(holiday.IsAllDayEvent);
                Recursive = holiday.IsRecursive;

                if (Recursive == 1)
                {
                    InUse = true;
                }
                else
                {
                    InUse = false;
                }

                GeosApplication.Instance.Logger.Log("Method EditHoliday()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditHoliday()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>

        public void InitReadOnly(ObservableCollection<CompanyHoliday> CompanyHolidaysDetails, CompanyHoliday holiday)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                ExistCompanyHolidaysList = new ObservableCollection<CompanyHoliday>(CompanyHolidaysDetails);
                Holiday = holiday;
                HolidayName = holiday.Name;
                HolidaysTypeList = new List<LookupValue>();
                HolidaysTypeList.Add(holiday.Holiday);
                SelectedIndexHolidayType = 0;
                StartDate = holiday.StartDate;
                EndDate = holiday.EndDate;
                StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
                IsAllDayEvent = Convert.ToBoolean(holiday.IsAllDayEvent);
                Recursive = holiday.IsRecursive;

                if (Recursive == 1)
                {
                    InUse = true;
                }
                else
                {
                    InUse = false;
                }

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Add Holiday
        /// [001][7-19-2020][smazhar][GESO2-2425][Not allowed to edit when many plants selected]
        /// </summary>
        /// <param name="obj"></param>
        private void AddHoliday(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddHoliday()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("HolidayName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexHolidayType"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));

                if (error != null)
                {
                    return;
                }

                if (InUse)
                    Recursive = 1;
                else
                    Recursive = 0;

                if (!string.IsNullOrEmpty(HolidayName))
                    HolidayName = HolidayName.Trim();

                StartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                EndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                STime = StartTime.Value.TimeOfDay;
                ETime = EndDate.Value.TimeOfDay;
                
                CompanyHoliday TempHoliday = new CompanyHoliday();
                var ExistHoliday = ExistCompanyHolidaysList.Where(x => x.Name == HolidayName && x.IdCompany == Holiday.IdCompany);
               //var ExistHoliday = ExistCompanyHolidaysList.Where(x => x.Name == HolidayName);
               TempHoliday = ExistHoliday.FirstOrDefault(x => x.Name == HolidayName && x.IdCompany == Holiday.IdCompany);
                //TempHoliday = ExistHoliday.FirstOrDefault(x => x.Name == HolidayName);

                if (isNew)
                {
                    if (TempHoliday == null)
                    {
                        WorkingPlantId = ((Company)HrmCommon.Instance.SelectedAuthorizedPlantsList.First()).IdCompany;

                        NewHoliday = new CompanyHoliday()
                        {
                            Name = HolidayName,
                            IdHoliday = HolidaysTypeList[selectedIndexHolidayType].IdLookupValue,
                            Holiday = HolidaysTypeList[selectedIndexHolidayType],
                            StartDate = StartDate,
                            EndDate = EndDate,
                            IsRecursive = (byte)Recursive,
                            IdCompany = WorkingPlantId,
                            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                        };

                        NewHoliday = HrmService.AddCompanyHoliday(NewHoliday);
                        NewHoliday.Company = (Company)HrmCommon.Instance.SelectedAuthorizedPlantsList.First(); // Company;

                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HolidaysInformationAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HolidayNameExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    if (TempHoliday == null || TempHoliday.IdCompanyHoliday == Holiday.IdCompanyHoliday )
                    {
                        UpdateHoliday = new CompanyHoliday()
                        {
                            IdCompanyHoliday = Holiday.IdCompanyHoliday,
                            Name = HolidayName,
                            IdHoliday = HolidaysTypeList[selectedIndexHolidayType].IdLookupValue,
                            Holiday = HolidaysTypeList[selectedIndexHolidayType],
                            StartDate = StartDate,
                            EndDate = EndDate,
                            IsRecursive = (byte)Recursive,
                            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                            IdCompany = WorkingPlantId
                        };

                        //UpdateHoliday.IdCompanyHoliday = Holiday.IdCompanyHoliday;

                        UpdateHoliday = HrmService.UpdateCompanyHoliday(UpdateHoliday);

                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HolidaysInformationUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }

                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HolidayNameExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddHoliday()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Add Holidays()-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }


        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startDateErrorMessage = string.Empty;

                if (StartDate != null && EndDate != null)
                {


                    if (StartDate.Value.Date > EndDate.Value.Date)
                    {
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddHolidayStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddHolidayEndDateError").ToString();
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


        private void FillFromDates()
        {
            FromDates = new List<DateTime>();

            double addMinutes = 0;
            for (int i = 0; i < 48; i++)
            {
                DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
                FromDates.Add(otherDate);
                addMinutes += 30;
            }
        }

        private void FillToDates()
        {
            ToDates = new List<DateTime>();

            double addMinutes = 0;
            for (int i = 0; i < 48; i++)
            {
                DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
                ToDates.Add(otherDate);
                addMinutes += 30;
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
                    if (StartTime > EndTime)
                    {
                        StartTimeErrorMessage = System.Windows.Application.Current.FindResource("HolidayStartTimeError").ToString();
                        EndTimeErrorMessage = System.Windows.Application.Current.FindResource("HolidayEndTimeError").ToString();
                    }
                    else
                    {
                        StartTimeErrorMessage = string.Empty;
                        EndTimeErrorMessage = string.Empty;
                    }
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
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                {
                    if (!IsAllDayEvent)
                    {
                        if (StartDate != null && EndDate != null && StartTime != null && EndTime != null)
                        {
                            DateTime _TempStartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                            DateTime _TimeEndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                            if (_TempStartDate >= _TimeEndDate)
                            {
                                StartTimeErrorMessage = System.Windows.Application.Current.FindResource("HolidayStartTimeError").ToString();
                                EndTimeErrorMessage = System.Windows.Application.Current.FindResource("HolidayEndTimeError").ToString();
                            }
                            else
                            {
                                StartTimeErrorMessage = string.Empty;
                                EndTimeErrorMessage = string.Empty;
                            }
                        }
                        else
                        {
                            StartTimeErrorMessage = string.Empty;
                            EndTimeErrorMessage = string.Empty;
                        }
                    }
                    else
                    {
                        StartTimeErrorMessage = string.Empty;
                        EndTimeErrorMessage = string.Empty;
                    }

                    if (StartDate != null && EndDate != null)
                    {
                        string error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
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

        private void CheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckedCommandAction()...", category: Category.Info, priority: Priority.Low);
                CheckDateTimeValidation();
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
               
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation

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

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => HolidayName)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexHolidayType)] +
                    me[BindableBase.GetPropertyName(() => StartDate)] +
                    me[BindableBase.GetPropertyName(() => EndDate)] +
                    me[BindableBase.GetPropertyName(() => StartTime)] +
                    me[BindableBase.GetPropertyName(() => EndTime)];

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
                string holidayName = BindableBase.GetPropertyName(() => HolidayName);
                string selectedType = BindableBase.GetPropertyName(() => SelectedIndexHolidayType);
                string holidayStartDate = BindableBase.GetPropertyName(() => StartDate);
                string holidayEndDate = BindableBase.GetPropertyName(() => EndDate);
                string holidayStartTime = BindableBase.GetPropertyName(() => StartTime);
                string holidayEndTime = BindableBase.GetPropertyName(() => EndTime);

                if (columnName == holidayName)
                {
                    return HolidaysValidation.GetErrorMessage(holidayName, HolidayName);
                }

                if (columnName == selectedType)
                {
                    return HolidaysValidation.GetErrorMessage(selectedType, selectedIndexHolidayType);
                }

                if (columnName == holidayStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return HolidaysValidation.GetErrorMessage(holidayStartDate, StartDate);
                    }
                }
                if (columnName == holidayEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                }
                if (columnName == holidayStartDate)
                {
                    return HolidaysValidation.GetErrorMessage(holidayStartDate, StartDate);
                }
                if (columnName == holidayEndDate)
                {
                    return HolidaysValidation.GetErrorMessage(holidayEndDate, EndDate);
                }

                if (columnName == holidayStartTime)
                {
                    if (!string.IsNullOrEmpty(StartTimeErrorMessage))
                    {
                        return StartTimeErrorMessage;
                    }
                    else
                    {
                        return HolidaysValidation.GetErrorMessage(holidayStartTime, StartTime);
                    }
                }

                if (columnName == holidayEndTime)
                {
                    if (!string.IsNullOrEmpty(EndTimeErrorMessage))
                    {
                        return EndTimeErrorMessage;
                    }
                    else
                    {
                        return HolidaysValidation.GetErrorMessage(holidayEndTime, EndTime);
                    }
                }


                return null;
            }
        }
        #endregion


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
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
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
