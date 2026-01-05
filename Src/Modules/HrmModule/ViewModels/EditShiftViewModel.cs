using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EditShiftViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
         IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IHrmService HrmService = new HrmServiceController("localhost:6699");
       // ICrmService CRMService = new CrmServiceController("localhost:6699");
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
        private List<CompanySchedule> workSchedules;
        private List<CompanyShift> CompanyShiftsList;
        private CompanySchedule selectedWorkSchedule;
        private string shiftName = string.Empty;
        private bool isSave;
        private string workscheduleValidationMessage;
        private string Error = string.Empty;
        private bool isNightShift;
        private bool isInUse;//[Added InUse][sudhir.jangra][GEOS2-GEOS2-2716]
        private int nightShift;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private List<LookupValue> typeList;//[Sudhir.Jangra][GEOS2-4023][19/04/2023]
        private int selectedIndexType;//[Sudhir.Jangra][GEOS2-4023][20/04/2023]
        #endregion

        #region Properties
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
        //[InUSE][sudhir.jangra][GEOS2-2716][18/10/2022]
        public bool IsInUse
        {
            get
            {
                return isInUse;

            }
            set
            {
                isInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isInUse"));
            }
        }
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
        public string ShiftName
        {
            get { return shiftName; }
            set { shiftName = value; OnPropertyChanged(new PropertyChangedEventArgs("ShiftName")); }
        }
        public CompanyShift UpdateShift { get; set; }
        public CompanyShift ExistShift { get; set; }
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
        public string WorkscheduleValidationMessage
        {
            get { return workscheduleValidationMessage; }
            set { workscheduleValidationMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("WorkscheduleValidationMessage")); }
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
        //[Sudhir.Jangra][GEOS2-4023][19/04/2023]
        public List<LookupValue> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeList"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4023][20/04/2023]
        public int SelectedIndexType
        {
            get { return selectedIndexType; }
            set
            {
                selectedIndexType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexType"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand EditShiftCancelButtonCommand { get; set; }
        public ICommand EditShiftAcceptButtonCommand { get; set; }
        public ICommand EditShiftWorkScheduleSelectionChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public EditShiftViewModel()
        {
            try
            {
                SetUserPermission();
                IsAcceptEnabled = GeosApplication.Instance.IsHRMManageShiftPermission;
                IsReadOnlyField = !GeosApplication.Instance.IsHRMManageShiftPermission;
                GeosApplication.Instance.Logger.Log("Constructor EditShiftViewModel()...", category: Category.Info, priority: Priority.Low);
                EditShiftCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                EditShiftAcceptButtonCommand = new RelayCommand(new Action<object>(EditShiftInformation));
                EditShiftWorkScheduleSelectionChangedCommand = new RelayCommand(new Action<object>(WorkscheduleAndNameValidation));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillWorkSchedules();
                FillTypeList();//[Sudhir.Jangra][GEOS2-4023][19/04/2023]
                GeosApplication.Instance.Logger.Log("Constructor EditShiftViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EditShiftViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void EditShiftInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditShiftInformation()...", category: Category.Info, priority: Priority.Low);
                if (!string.IsNullOrEmpty(ShiftName))
                    ShiftName = ShiftName.Trim();

                //WorkscheduleAndNameValidation(new object());

                Error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedWorkSchedule"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexType"));//[Sudhir.Jangra][GEOS2-4023][20/04/2023]
                PropertyChanged(this, new PropertyChangedEventArgs("ShiftName"));
                if (!string.IsNullOrEmpty(Error))
                {
                    return;
                }
                if (SelectedIndexType == 1)
                {
                    IsNightShift = false;
                }
                else if (SelectedIndexType == 2)
                {
                    IsNightShift = false;
                }
                else if (SelectedIndexType == 3)
                {
                    IsNightShift = true;
                }
                if (IsNightShift)
                    NightShift = 1;
                else
                    NightShift = 0;
                sbyte IsInUsesbyte = 0;
                if (IsInUse)
                {
                    IsInUsesbyte = 1;
                }
                else
                {
                    IsInUsesbyte = 0;
                }
                if (ExistShift.IsInUse == 1 && IsInUsesbyte == 0)
                {
                    if (HrmService.GetIsCompanyShiftAssignedToEmployee(ExistShift.IdCompanyShift))
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["EditShiftInUseValidation"].ToString(), ShiftName), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    }

                }

                UpdateShift = new CompanyShift()
                {
                    Name = ShiftName,
                    IdCompanyShift = ExistShift.IdCompanyShift,
                    CompanySchedule = SelectedWorkSchedule,
                    IdCompanySchedule = SelectedWorkSchedule.IdCompanySchedule,
                    IdType = TypeList[SelectedIndexType].IdLookupValue,//[Sudhir.Jangra][GEOS2-4023][20/04/2023]
                    MonStartTime = MondayStartTime.TimeOfDay,
                    MonEndTime = MondayEndTime.TimeOfDay,
                    MonBreakTime = MondayBreakTime.TimeOfDay,

                    TueStartTime = TuesdayStartTime.TimeOfDay,
                    TueEndTime = TuesdayEndTime.TimeOfDay,
                    TueBreakTime = TuesdayBreakTime.TimeOfDay,

                    WedStartTime = WednesdayStartTime.TimeOfDay,
                    WedEndTime = WednesdayEndTime.TimeOfDay,
                    WedBreakTime = WednesdayBreakTime.TimeOfDay,

                    ThuStartTime = ThursdayStartTime.TimeOfDay,
                    ThuEndTime = ThursdayEndTime.TimeOfDay,
                    ThuBreakTime = ThursdayBreakTime.TimeOfDay,

                    FriStartTime = FridayStartTime.TimeOfDay,
                    FriEndTime = FridayEndTime.TimeOfDay,
                    FriBreakTime = FridayBreakTime.TimeOfDay,

                    SatStartTime = SaturdayStartTime.TimeOfDay,
                    SatEndTime = SaturdayEndTime.TimeOfDay,
                    SatBreakTime = SaturdayBreakTime.TimeOfDay,

                    SunStartTime = SundayStartTime.TimeOfDay,
                    SunEndTime = SundayEndTime.TimeOfDay,
                    SunBreakTime = SundayBreakTime.TimeOfDay,
                    
                    IsNightShift = (sbyte)NightShift,
                    IsInUse = IsInUsesbyte


                };

                //UpdateShift = HrmService.UpdateCompanyShift_V2035(UpdateShift);
                // UpdateShift = HrmService.UpdateCompanyShift_V2330(UpdateShift);
                //[Sudhir.Jangra][GEOS2-4023][20/04/2023]
                UpdateShift = HrmService.UpdateCompanyShift_V2380(UpdateShift);
                UpdateShift.CompanySchedule.Company = ExistShift.CompanySchedule.Company;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdatedShiftSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                IsSave = true;
                GeosApplication.Instance.Logger.Log("Method EditShiftInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditShiftInformation() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditShiftInformation() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditShiftInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill work schedules
        ///  [001][cpatil][10-11-2022][GEOS2-3114]
        /// </summary>
        private void FillWorkSchedules()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillWorkSchedules()...", category: Category.Info, priority: Priority.Low);
                string idsSelectedplants = string.Empty;
                idsSelectedplants = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(x => x.IdCompany));
                WorkSchedules = HrmService.GetCompanyScheduleByIdCompany_V2330(idsSelectedplants);
                GeosApplication.Instance.Logger.Log("Method FillWorkSchedules()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWorkSchedules() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkSchedules() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkSchedules()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(CompanyShift companyShift, ObservableCollection<CompanyShift> companyShiftsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                CompanyShiftsList = companyShiftsList.ToList();
                ExistShift = companyShift;

                SelectedWorkSchedule = WorkSchedules.FirstOrDefault(x => x.IdCompanySchedule == ExistShift.CompanySchedule.IdCompanySchedule);
                SelectedIndexType = TypeList.FindIndex(x => x.IdLookupValue == ExistShift.IdType);//[Sudhir.Jangra][GEOS2-4023][20/04/2023]
                ShiftName = ExistShift.Name;
                MondayStartTime = Convert.ToDateTime(ExistShift.MonStartTime.ToString());
                MondayEndTime = Convert.ToDateTime(ExistShift.MonEndTime.ToString());
                MondayBreakTime = Convert.ToDateTime(ExistShift.MonBreakTime.ToString());

                TuesdayStartTime = Convert.ToDateTime(ExistShift.TueStartTime.ToString());
                TuesdayEndTime = Convert.ToDateTime(ExistShift.TueEndTime.ToString());
                TuesdayBreakTime = Convert.ToDateTime(ExistShift.TueBreakTime.ToString());

                WednesdayStartTime = Convert.ToDateTime(ExistShift.WedStartTime.ToString());
                WednesdayEndTime = Convert.ToDateTime(ExistShift.WedEndTime.ToString());
                WednesdayBreakTime = Convert.ToDateTime(ExistShift.WedBreakTime.ToString());

                ThursdayStartTime = Convert.ToDateTime(ExistShift.ThuStartTime.ToString());
                ThursdayEndTime = Convert.ToDateTime(ExistShift.ThuEndTime.ToString());
                ThursdayBreakTime = Convert.ToDateTime(ExistShift.ThuBreakTime.ToString());

                FridayStartTime = Convert.ToDateTime(ExistShift.FriStartTime.ToString());
                FridayEndTime = Convert.ToDateTime(ExistShift.FriEndTime.ToString());
                FridayBreakTime = Convert.ToDateTime(ExistShift.FriBreakTime.ToString());

                SaturdayStartTime = Convert.ToDateTime(ExistShift.SatStartTime.ToString());
                SaturdayEndTime = Convert.ToDateTime(ExistShift.SatEndTime.ToString());
                SaturdayBreakTime = Convert.ToDateTime(ExistShift.SatBreakTime.ToString());

                SundayStartTime = Convert.ToDateTime(ExistShift.SunStartTime.ToString());
                SundayEndTime = Convert.ToDateTime(ExistShift.SunEndTime.ToString());
                SundayBreakTime = Convert.ToDateTime(ExistShift.SunBreakTime.ToString());

                if (ExistShift.IsNightShift == 1)
                    IsNightShift = true;
                else
                    IsNightShift = false;
                //[InUSE][sudhir.jangra][GEOS2-2716][18/10/2022]
                IsInUse = (ExistShift.IsInUse == 1) ? true : false;





                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// summary>
        ///Validation for Name and Workschedule 
        ///[001][7-16-2020][GESO2-2418][HRM - Repeated Shift]
        /// </summary>
        public void WorkscheduleAndNameValidation(object obj)
        {
            workscheduleValidationMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("Method AddNewShiftViewModel Method WorkscheduleAndNameValidation ()...", category: Category.Info, priority: Priority.Low);
            //List<CompanyShift> tempCompanyShiftsList = CompanyShiftsList.Where(x => x.IdCompanyShift != ExistShift.IdCompanyShift).ToList();
            //CompanyShift Com_Shift = CompanyShiftsList.Where(x => x.Name.ToUpper() == ShiftName.ToUpper() && x.CompanySchedule.IdCompanySchedule == SelectedWorkSchedule.IdCompanySchedule && x.IdCompanyShift != ExistShift.IdCompanyShift).FirstOrDefault();
            //SelectedWorkSchedule = new CompanySchedule();
            CompanyShiftsList = CompanyShiftsList.Where(i => i.Name != null).ToList();
            if (CompanyShiftsList.Any(x => x.Name.ToUpper() == ShiftName.ToUpper() && x.CompanySchedule.IdCompanySchedule == SelectedWorkSchedule.IdCompanySchedule && x.IdCompanyShift != ExistShift.IdCompanyShift))
            {
                WorkscheduleValidationMessage = System.Windows.Application.Current.FindResource("AddShiftWorkscheduleAndNameError").ToString();
            }
            //  Error = EnableValidationAndGetError();
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
        public void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList()...", category: Category.Info, priority: Priority.Low);
                
                    var temp = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(105));
                TypeList =new List<LookupValue>(temp);
                TypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                DataError[BindableBase.GetPropertyName(() => SelectedWorkSchedule)] +
                DataError[BindableBase.GetPropertyName(() => SelectedIndexType)];//[Sudhir.Jangra][GEOS2-4023][20/04/2023]
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
                string type = BindableBase.GetPropertyName(() => SelectedIndexType);
                if (columnName == type)
                {
                    return ShiftsValidations.GetErrorMessage(type, SelectedIndexType);
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
