using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewShiftViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// [M049-07][Add option to add  shifts][adadibathina]
        /// [issue - asked remove validation for start time and endtime]
        /// [HRM-M052-01] Allow all possible timings in shifts [adadibathina]
        /// </summary>
        /// 


        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declaration
        private string windowHeader;
        private bool isReadOnly;
        private bool isSave;
        private CompanyShift newShift;
        private CompanyShift selectedCompanyShift;
        //[HRM-M052-01]
        //private List<DateTime> fromTime;
        //private List<DateTime> toTime;
        private string timeEditMask;
        private DateTime breakTime;
        private string shiftName = string.Empty;
        private List<object> days;
        //private List<object> selectedDays;
        private bool isNew;
        private DateTime selectedToTime;
        private DateTime selectedFromTime;
        private List<CompanySchedule> workSchedules;
        string Error = string.Empty;
        // [M049-07][27/10/2018][Add option to add  shifts][adadibathina]
        // [issue - asked remove validation for start time and endtime]
        //private string selectedToTimeValidationMessage;
        //private string selectedFromTimeValidationMessage;
        private string selectedDaysValidationMessage;
        private string workscheduleValidationMessage;
        private CompanySchedule selectedWorkSchedule;
        private List<CompanyShift> CompanyShiftsList;
        private bool isTimeValidationCanOccur = false;
        private DateTime sundayStartTime;
        private DateTime sundayEndTime;
        private DateTime sundayBreakTime;
        private DateTime mondayStartTime;
        private DateTime mondayEndTime;
        private DateTime mondayBreakTime;
        private DateTime tuesdayStartTime;
        private DateTime tuesdayEndTime;
        private DateTime tuesdayBreakTime;
        private DateTime wednesdayStartTime;
        private DateTime wednesdayEndTime;
        private DateTime wednesdayBreakTime;
        private DateTime thursdayStartTime;
        private DateTime thursdayEndTime;
        private DateTime thursdayBreakTime;
        private DateTime fridayStartTime;
        private DateTime fridayEndTime;
        private DateTime fridayBreakTime;
        private DateTime saturdayStartTime;
        private DateTime saturdayEndTime;
        private DateTime saturdayBreakTime;
        private bool isNightShift;
        private int nightShift;
        #endregion

        #region Properties

        public CompanyShift UpdateShift { get; set; }
        public CompanyShift TempShift { get; set; }

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


        public DateTime SelectedToTime
        {
            get { return selectedToTime; }
            set
            {
                selectedToTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedToTime"));
            }

        }

        public DateTime SelectedFromTime
        {
            get { return selectedFromTime; }
            set { selectedFromTime = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedFromTime")); }

        }

        //[HRM-M052-01]
        //public List<DateTime> FromTime
        //{
        //    get { return fromTime; }
        //    set { fromTime = value; OnPropertyChanged(new PropertyChangedEventArgs("FromTime")); }
        //}
        //public List<DateTime> ToTime
        //{
        //    get { return toTime; }
        //    set { toTime = value; OnPropertyChanged(new PropertyChangedEventArgs("ToTime")); }
        //}




        public CompanyShift NewShift
        {

            get { return newShift; }


            set
            {
                newShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewShift"));
            }

        }



        public string ShiftName
        {
            get { return shiftName; }
            set { shiftName = value; OnPropertyChanged(new PropertyChangedEventArgs("ShiftName")); }
        }

        // [M049-07][27/10/2018][Add option to add  shifts][adadibathina]
        // [issue - asked remove validation for start time and endtime]
        //public string SelectedToTimeValidationMessage
        //{
        //    get
        //    {
        //        return selectedToTimeValidationMessage;
        //    }

        //    set
        //    {
        //        selectedToTimeValidationMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedToTimeValidationMessage"));
        //    }
        //}
        //public string SelectedFromTimeValidationMessage
        //{
        //    get
        //    {
        //        return selectedFromTimeValidationMessage;
        //    }

        //    set
        //    {
        //        selectedFromTimeValidationMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedFromTimeValidationMessage"));
        //    }
        //}



        public string SelectedDaysValidationMessage
        {
            get { return selectedDaysValidationMessage; }
            set { selectedDaysValidationMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedDaysValidationMessage")); }
        }

        public string WorkscheduleValidationMessage
        {
            get { return workscheduleValidationMessage; }
            set { workscheduleValidationMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("WorkscheduleValidationMessage")); }
        }



        public DateTime BreakTime
        {
            get { return breakTime; }
            set { breakTime = value; OnPropertyChanged(new PropertyChangedEventArgs("BreakTime")); }
        }


        public List<object> Days
        {
            get { return days; }
            set { days = value; OnPropertyChanged(new PropertyChangedEventArgs("Days")); }

        }

        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }



        //public List<object> SelectedDays
        //{
        //    get { return selectedDays; }
        //    set { selectedDays = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedDays")); }
        //}

        public List<CompanySchedule> WorkSchedules
        {
            get { return workSchedules; }
            set { workSchedules = value; OnPropertyChanged(new PropertyChangedEventArgs("WorkSchedules")); }
        }

        public CompanySchedule SelectedWorkSchedule
        {
            get { return selectedWorkSchedule; }
            set { selectedWorkSchedule = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkSchedule")); }
        }
        public DateTime SundayStartTime
        {
            get
            {
                return sundayStartTime;
            }

            set
            {
                sundayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SundayStartTime"));
            }
        }

        public DateTime SundayEndTime
        {
            get
            {
                return sundayEndTime;
            }

            set
            {
                sundayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SundayEndTime"));
            }
        }

        public DateTime SundayBreakTime
        {
            get
            {
                return sundayBreakTime;
            }

            set
            {
                sundayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SundayBreakTime"));
            }
        }

        public DateTime MondayStartTime
        {
            get
            {
                return mondayStartTime;
            }

            set
            {
                mondayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MondayStartTime"));
            }
        }

        public DateTime MondayEndTime
        {
            get
            {
                return mondayEndTime;
            }

            set
            {
                mondayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MondayEndTime"));
            }
        }

        public DateTime MondayBreakTime
        {
            get
            {
                return mondayBreakTime;
            }

            set
            {
                mondayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MondayBreakTime"));
            }
        }

        public DateTime TuesdayStartTime
        {
            get
            {
                return tuesdayStartTime;
            }

            set
            {
                tuesdayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TuesdayStartTime"));
            }
        }

        public DateTime TuesdayEndTime
        {
            get
            {
                return tuesdayEndTime;
            }

            set
            {
                tuesdayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TuesdayEndTime"));
            }
        }

        public DateTime TuesdayBreakTime
        {
            get
            {
                return tuesdayBreakTime;
            }

            set
            {
                tuesdayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TuesdayBreakTime"));
            }
        }

        public DateTime WednesdayStartTime
        {
            get
            {
                return wednesdayStartTime;
            }

            set
            {
                wednesdayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WednesdayStartTime"));
            }
        }

        public DateTime WednesdayEndTime
        {
            get
            {
                return wednesdayEndTime;
            }

            set
            {
                wednesdayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WednesdayEndTime"));
            }
        }

        public DateTime WednesdayBreakTime
        {
            get
            {
                return wednesdayBreakTime;
            }

            set
            {
                wednesdayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WednesdayBreakTime"));
            }
        }

        public DateTime ThursdayStartTime
        {
            get
            {
                return thursdayStartTime;
            }

            set
            {
                thursdayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ThursdayStartTime"));
            }
        }

        public DateTime ThursdayEndTime
        {
            get
            {
                return thursdayEndTime;
            }

            set
            {
                thursdayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ThursdayEndTime"));
            }
        }

        public DateTime ThursdayBreakTime
        {
            get
            {
                return thursdayBreakTime;
            }

            set
            {
                thursdayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ThursdayBreakTime"));
            }
        }

        public DateTime FridayStartTime
        {
            get
            {
                return fridayStartTime;
            }

            set
            {
                fridayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FridayStartTime"));
            }
        }

        public DateTime FridayEndTime
        {
            get
            {
                return fridayEndTime;
            }

            set
            {
                fridayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FridayEndTime"));
            }
        }

        public DateTime FridayBreakTime
        {
            get
            {
                return fridayBreakTime;
            }

            set
            {
                fridayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FridayBreakTime"));
            }
        }

        public DateTime SaturdayStartTime
        {
            get
            {
                return saturdayStartTime;
            }

            set
            {
                saturdayStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaturdayStartTime"));
            }
        }

        public DateTime SaturdayEndTime
        {
            get
            {
                return saturdayEndTime;
            }

            set
            {
                saturdayEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaturdayEndTime"));
            }
        }

        public DateTime SaturdayBreakTime
        {
            get
            {
                return saturdayBreakTime;
            }

            set
            {
                saturdayBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaturdayBreakTime"));
            }
        }
        public bool IsNightShift
        {
            get
            {
                return isNightShift;
            }

            set
            {
                isNightShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNightShift"));
            }
        }
        public int NightShift
        {
            get
            {
                return nightShift;
            }

            set
            {
                nightShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NightShift"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand AddNewShiftViewCancelButtonCommand { get; set; }
        public ICommand AddNewShiftAcceptButtonCommand { get; set; }
        public ICommand AddNewShiftCancelButtonCommand { get; set; }
        // [M049-07][27/10/2018][Add option to add  shifts][adadibathina]
        // [issue - asked remove validation for start time and endtime]
        //public ICommand AddNewShiftDateSelectionChangedCommand { get; set; }
        public ICommand AddShiftWorkScheduleSelectionChangedCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Ctor
        public AddNewShiftViewModel()
        {
            string idsSelectedplants = string.Empty;
            GeosApplication.Instance.Logger.Log("Constructor AddNewShiftViewModel()...", category: Category.Info, priority: Priority.Low);
            //SelectedDays = new List<object>();

            TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            AddNewShiftAcceptButtonCommand = new RelayCommand(new Action<object>(AddNewShiftInformation));
            AddNewShiftViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            AddNewShiftCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            LoadedCommand = new RelayCommand(new Action<object>(Loaded));
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            #region Time ValidationCommend

            /// [issue - asked remove validation for start time and endtime]
            //AddNewShiftDateSelectionChangedCommand = new RelayCommand(new Action<object>(TimeValidation));
            #endregion

            AddShiftWorkScheduleSelectionChangedCommand = new RelayCommand(new Action<object>(WorkscheduleAndNameValidation));
            //[HRM-M052-01]
            //  FillFromTime();
            // FillToTime();
            FillWorkSchedules();
            Days = GeosApplication.Instance.GetWeekNames();

        }
        #endregion

        #region Methods
        /// <summary>
        /// Sprint 49-[M049-07][20180810][adadibathina]
        /// Method to initialize AddNewShiftViewModel
        /// </summary>
        public void Init(ObservableCollection<CompanyShift> CompanyShiftsList)
        {
            try
            {
                TempShift = new CompanyShift();

                SelectedWorkSchedule = WorkSchedules.FirstOrDefault(x => x.Name.Equals("---"));
                this.CompanyShiftsList = CompanyShiftsList.ToList();
                IsNightShift = false;
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewShiftViewModel Method Init(CompanyShiftsList)  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewShiftViewModel Method Init(CompanyShiftsList) Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddNewShiftViewModel Method Init(CompanyShiftsList)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void Init(CompanyShift SelectedCompanyShift, ObservableCollection<CompanyShift> CompanyShiftsList)
        {
            try
            {
                TempShift = new CompanyShift();
                this.CompanyShiftsList = CompanyShiftsList.ToList();
                SelectedWorkSchedule = WorkSchedules.FirstOrDefault(x => x.Name == SelectedCompanyShift.CompanySchedule.Name);
                ShiftName = SelectedCompanyShift.Name;
                //[HRM-M052-01]
                //SelectedFromTime = FromTime.FirstOrDefault(x => x.TimeOfDay == SelectedCompanyShift.StartTime1);
                //SelectedToTime = ToTime.FirstOrDefault(x => x.TimeOfDay == SelectedCompanyShift.EndTime1);
                SelectedFromTime = Convert.ToDateTime(SelectedCompanyShift.StartTime1.ToString());
                SelectedToTime = Convert.ToDateTime(SelectedCompanyShift.EndTime1.ToString());
                BreakTime = Convert.ToDateTime(SelectedCompanyShift.BreakTime.ToString());
                selectedCompanyShift = SelectedCompanyShift;

                #region SelectedDays
                //if (SelectedCompanyShift.Mon == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Monday")));
                //}
                //if (SelectedCompanyShift.Tue == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Tuesday")));
                //}
                //if (SelectedCompanyShift.Wed == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Wednesday")));
                //}
                //if (SelectedCompanyShift.Thu == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Thursday")));
                //}
                //if (SelectedCompanyShift.Fri == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Friday")));
                //}
                //if (SelectedCompanyShift.Sat == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Saturday")));
                //}
                //if (SelectedCompanyShift.Sun == 1)
                //{
                //    SelectedDays.Add(Days.Cast<DayOfWeek>().FirstOrDefault(x => x.ToString().Equals("Sunday")));
                //}
                #endregion

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddNewShiftViewModel Method Init( CompanyShift)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// Method to Loaded
        /// </summary>
        /// <param name="obj"></param>
        private void Loaded(object obj)
        {
            isTimeValidationCanOccur = true;
        }

        /// <summary>
        /// Method to Fill From Time
        /// [HRM-M052-01]
        /// </summary>
        //private void FillFromTime()
        //{
        //    FromTime = new List<DateTime>();
        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        FromTime.Add(otherDate);
        //        addMinutes += 30;
        //    }
        //}

        /// <summary>
        /// Method to Fill To Time
        ///[HRM-M052-01]
        /// </summary>

        //private void FillToTime()
        //{
        //    ToTime = new List<DateTime>();
        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        ToTime.Add(otherDate);
        //        addMinutes += 30;
        //    }
        //}


        /// <summary>
        /// Method to Fill To Time
        /// </summary>

        private void FillWorkSchedules()
        {
            string idsSelectedplants = string.Empty;
            idsSelectedplants = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(x => x.IdCompany));
            WorkSchedules = HrmService.GetCompanyScheduleByIdCompany(idsSelectedplants);
        }
        /// <summary>
        /// Method to Add Newshift
        /// </summary>

        private void AddNewShiftInformation(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewShiftViewModel Method AddNewShiftInformation ()...", category: Category.Info, priority: Priority.Low);
            if (!string.IsNullOrEmpty(ShiftName))
                ShiftName = ShiftName.Trim();

            //SelectedDaysValidation();
            if (isNew)
                WorkscheduleAndNameValidation(new object());

            Error = EnableValidationAndGetError();
           // PropertyChanged(this, new PropertyChangedEventArgs("SelectedDays"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedWorkSchedule"));
            PropertyChanged(this, new PropertyChangedEventArgs("ShiftName"));
            if (!string.IsNullOrEmpty(Error))
            {
                return;
            }

            if (IsNightShift)
                NightShift = 1;
            else
                NightShift = 0;
            
            TempShift.Name = ShiftName;
            TempShift.IdCompanySchedule = SelectedWorkSchedule.IdCompanySchedule;
            TempShift.CompanySchedule = SelectedWorkSchedule;

            TempShift.MonStartTime = MondayStartTime.TimeOfDay;
            TempShift.MonEndTime = MondayEndTime.TimeOfDay;
            TempShift.MonBreakTime = MondayBreakTime.TimeOfDay;

            TempShift.TueStartTime = TuesdayStartTime.TimeOfDay;
            TempShift.TueEndTime = TuesdayEndTime.TimeOfDay;
            TempShift.TueBreakTime = TuesdayBreakTime.TimeOfDay;

            TempShift.WedStartTime = WednesdayStartTime.TimeOfDay;
            TempShift.WedEndTime = WednesdayEndTime.TimeOfDay;
            TempShift.WedBreakTime = WednesdayBreakTime.TimeOfDay;

            TempShift.ThuStartTime = ThursdayStartTime.TimeOfDay;
            TempShift.ThuEndTime = ThursdayEndTime.TimeOfDay;
            TempShift.ThuBreakTime = ThursdayBreakTime.TimeOfDay;

            TempShift.FriStartTime = FridayStartTime.TimeOfDay;
            TempShift.FriEndTime = FridayEndTime.TimeOfDay;
            TempShift.FriBreakTime = FridayBreakTime.TimeOfDay;

            TempShift.SatStartTime = SaturdayStartTime.TimeOfDay;
            TempShift.SatEndTime = SaturdayEndTime.TimeOfDay;
            TempShift.SatBreakTime = SaturdayBreakTime.TimeOfDay;

            TempShift.SunStartTime = SundayStartTime.TimeOfDay;
            TempShift.SunEndTime = SundayEndTime.TimeOfDay;
            TempShift.SunBreakTime = SundayBreakTime.TimeOfDay;

            TempShift.CompanySchedule.Company = new Company();
            TempShift.CompanySchedule.Company.Alias = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().FirstOrDefault(x => x.IdCompany == SelectedWorkSchedule.IdCompany).Alias;

            TempShift.IsNightShift = (sbyte)NightShift;

            #region SelectedDays
            //NewShift.Mon = 0; NewShift.Tue = 0; NewShift.Wed = 0; NewShift.Thu = 0; NewShift.Fri = 0; NewShift.Sat = 0; NewShift.Sun = 0;
            //foreach (var day in SelectedDays)
            //{
            //    switch (day.ToString())
            //    {
            //        case "Monday":
            //            TempShift.Mon = 1;
            //            break;
            //        case "Tuesday":
            //            TempShift.Tue = 1;
            //            break;
            //        case "Wednesday":
            //            TempShift.Wed = 1;
            //            break;
            //        case "Thursday":
            //            TempShift.Thu = 1;
            //            break;
            //        case "Friday":
            //            TempShift.Fri = 1;
            //            break;
            //        case "Saturday":
            //            TempShift.Sat = 1;
            //            break;
            //        case "Sunday":
            //            TempShift.Sun = 1;
            //            break;

            //    }
            //}
            #endregion

            try
            {
                if (isNew)
                {
                    NewShift = HrmService.AddCompanyShift_V2035(TempShift);

                    CompanyShiftsList.Add(NewShift);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddShiftSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                //else
                //{
                //    TempShift.IdCompanyShift = selectedCompanyShift.IdCompanyShift;
                //   // TempShift.IdCompanySchedule = SelectedWorkSchedule.IdCompanySchedule;
                //    UpdateShift = HrmService.UpdateCompanyShift(TempShift);
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdatedShiftSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //}

                RequestClose(null, null);
                IsSave = true;
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewShiftViewModel Method AddNewShiftInformation() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewShiftViewModel Method AddNewShiftInformation() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewShiftViewModel Method  AddNewShiftInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        #region TimeValidation

        /// <summary>
        ///Validation for time
        /// [M049-07][[27/102018]][Add option to add  shifts][adadibathina]
        /// [issue - asked to remove validation for start time and endtime]

        /// </summary>
        //public void TimeValidation(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);

        //    #region FirstTimeValidation
        //    if (isTimeValidationCanOccur == false)
        //    {
        //        return;
        //    }
        //    #endregion

        //    if (!HrmCommon.Instance.IsPermissionReadOnly)
        //    {
        //        if (SelectedToTime != null && SelectedFromTime != null)
        //        {
        //            if (SelectedFromTime >= SelectedToTime)
        //            {
        //                SelectedToTimeValidationMessage = System.Windows.Application.Current.FindResource("AddShiftToTimeError").ToString();
        //                SelectedFromTimeValidationMessage = System.Windows.Application.Current.FindResource("AddShiftFromTimeError").ToString();
        //            }
        //            else
        //            {
        //                SelectedToTimeValidationMessage = string.Empty;
        //                SelectedFromTimeValidationMessage = string.Empty;
        //            }
        //        }
        //        else
        //        {
        //            SelectedToTimeValidationMessage = string.Empty;
        //            SelectedFromTimeValidationMessage = string.Empty;
        //        }
        //    }
        //    Error = EnableValidationAndGetError();
        //    PropertyChanged(this, new PropertyChangedEventArgs("SelectedToTime"));
        //    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFromTime"));

        //}
        #endregion



        /// <summary>
        ///Validation for SelectedDays
        /// </summary>
        //public void SelectedDaysValidation()
        //{
        //    GeosApplication.Instance.Logger.Log("Method AddNewShiftViewModel Method CheckSelectedDaysValidation ()...", category: Category.Info, priority: Priority.Low);
        //    if (SelectedDays != null)
        //    {
        //        if (SelectedDays.Count == 0)
        //        {
        //            SelectedDaysValidationMessage = System.Windows.Application.Current.FindResource("AddShiftSelectedDaysError").ToString();
        //        }
        //        else
        //        {
        //            SelectedDaysValidationMessage = string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        SelectedDaysValidationMessage = System.Windows.Application.Current.FindResource("AddShiftSelectedDaysError").ToString();
        //    }
        //}


        /// <summary>
        ///Validation for Name and Workschedule 
        ////[001][7-16-2020][GESO2-2418][HRM - Repeated Shift]
        /// </summary>
        public void WorkscheduleAndNameValidation(object obj)
        {


            workscheduleValidationMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("Method AddNewShiftViewModel Method WorkscheduleAndNameValidation ()...", category: Category.Info, priority: Priority.Low);
            if (CompanyShiftsList.Any(x => x.Name.ToUpper() == ShiftName.ToUpper() && x.CompanySchedule.IdCompanySchedule == SelectedWorkSchedule.IdCompanySchedule))
            {
                WorkscheduleValidationMessage = System.Windows.Application.Current.FindResource("AddShiftWorkscheduleAndNameError").ToString();
            }
            Error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedWorkSchedule"));
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
                IDataErrorInfo DataError = (IDataErrorInfo)this;
                string error = DataError[BindableBase.GetPropertyName(() => ShiftName)] +
                //DataError[BindableBase.GetPropertyName(() => SelectedFromTime)] +
                // DataError[BindableBase.GetPropertyName(() => SelectedToTime)] +
                //DataError[BindableBase.GetPropertyName(() => SelectedDays)] +
                DataError[BindableBase.GetPropertyName(() => SelectedWorkSchedule)];
                //DataError[BindableBase.GetPropertyName(() => SelectedToTime)];
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
                string shiftName = BindableBase.GetPropertyName(() => ShiftName);
                if (columnName == shiftName)
                {
                    return ShiftsValidations.GetErrorMessage(shiftName, ShiftName);
                }

                #region Time validation
                // [M049-07][27/10/2018][Add option to add  shifts][adadibathina]
                // [issue - asked remove validation for start time and endtime]
                //string selectedFromTime = BindableBase.GetPropertyName(() => SelectedFromTime);
                //if (columnName == selectedFromTime)
                //{
                //    if (!string.IsNullOrEmpty(SelectedFromTimeValidationMessage))
                //    {
                //        return SelectedFromTimeValidationMessage;
                //    }
                //}
                //string selectedToTime = BindableBase.GetPropertyName(() => SelectedToTime);
                //if (columnName == selectedToTime)
                //{
                //    if (!string.IsNullOrEmpty(SelectedToTimeValidationMessage))
                //    {
                //        return SelectedToTimeValidationMessage;
                //    }
                //} 
                #endregion

                // string selectedDays = BindableBase.GetPropertyName(() => SelectedDays);
                //if (columnName == selectedDays)
                //{
                //    if (!string.IsNullOrEmpty(SelectedDaysValidationMessage))
                //    {
                //        return SelectedDaysValidationMessage;
                //    }
                //}
                string selectedWorkSchedule = BindableBase.GetPropertyName(() => SelectedWorkSchedule);
                if (columnName == selectedWorkSchedule)
                {
                    if (!string.IsNullOrEmpty(workscheduleValidationMessage))
                    {
                        return workscheduleValidationMessage;
                    }
                    else
                    {
                        return ShiftsValidations.GetErrorMessage(selectedWorkSchedule, SelectedWorkSchedule.Name);
                    }
                }
                return null;
            }
        }
        #endregion
    }

}
