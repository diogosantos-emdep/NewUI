using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddAuthorizedLeaveViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Declaration
        private bool isNew;
        private string windowHeader;
        private int regularDays;
        private decimal regularHours;
        private int additionalDays;
        private decimal additionalHours;
        private int selectedLeaveType;
        private ObservableCollection<EmployeeAnnualLeave> existEmployeeAnnualLeaveList;
        private int selectedYear;
        private int idEmployee;
        private decimal dailyWorkHoursCount;
        public EmployeeAnnualLeave ExistEmployeeAnnualLeave;
        private int idCompany;
        private int idCompanyShift;
        private bool isSave;
        private string error = string.Empty;
        //  private decimal enjoyedHours;
        private decimal enjoyedTillYesterday;
        private decimal enjoyingFromToday;
        private ObservableCollection<LookupValue> annualLeaveList;
        private bool isAdd;
        private bool isEdit;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private ObservableCollection<LookupValue> annualAdditionalLeaveList;
        TableView view;
        private int backlogDays;
        private decimal backlogHours;


        private decimal oldRemaining;//[Sudhir.jangra][GEOS-4220][18/05/2023]



        #endregion

        #region Properties

        public EmployeeAnnualLeave NewEmployeeAnnualLeave { get; set; }
        public EmployeeAnnualLeave UpdateEmployeeAnnualLeave { get; set; }

        public EmployeeAnnualAdditionalLeave NewEmployeeAnnualAdditionalLeave { get; set; }
        public EmployeeAnnualAdditionalLeave UpdateEmployeeAnnualAdditionalLeave { get; set; }
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

        public int RegularDays
        {
            get
            {
                return regularDays;
            }

            set
            {
                regularDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegularDays"));
            }
        }

        public decimal RegularHours
        {
            get
            {
                return regularHours;
            }

            set
            {
                regularHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegularHours"));
            }
        }

        public int AdditionalDays
        {
            get
            {
                return additionalDays;
            }

            set
            {
                additionalDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalDays"));
            }
        }

        public decimal AdditionalHours
        {
            get
            {
                return additionalHours;
            }

            set
            {
                additionalHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalHours"));
            }
        }

        public int SelectedLeaveType
        {
            get
            {
                return selectedLeaveType;
            }

            set
            {
                selectedLeaveType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveType"));
            }
        }
        public ObservableCollection<EmployeeAnnualLeave> ExistEmployeeAnnualLeaveList
        {
            get
            {
                return existEmployeeAnnualLeaveList;
            }

            set
            {
                existEmployeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeAnnualLeaveList"));
            }
        }

        public int SelectedYear
        {
            get
            {
                return selectedYear;
            }

            set
            {
                selectedYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedYear"));
            }
        }

        public int IdEmployee
        {
            get
            {
                return idEmployee;
            }

            set
            {
                idEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployee"));
            }
        }

        public decimal DailyWorkHoursCount
        {
            get
            {
                return dailyWorkHoursCount;
            }

            set
            {
                dailyWorkHoursCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DailyWorkHoursCount"));
            }
        }

        public int IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompany"));
            }
        }
        public int IdCompanyShift
        {
            get
            {
                return idCompanyShift;
            }

            set
            {
                idCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompanyShift"));
            }
        }

        //public decimal EnjoyedHours
        //{
        //    get
        //    {
        //        return enjoyedHours;
        //    }

        //    set
        //    {
        //        enjoyedHours = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("EnjoyedHours"));
        //    }
        //}

        public decimal EnjoyedTillYesterday
        {
            get
            {
                return enjoyedTillYesterday;
            }

            set
            {
                enjoyedTillYesterday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnjoyedTillYesterday"));
            }
        }

        public decimal EnjoyingFromToday
        {
            get
            {
                return enjoyingFromToday;
            }

            set
            {
                enjoyingFromToday = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnjoyingFromToday"));
            }
        }
        public CompanyShift TempCompanyShift { get; set; }

        public ObservableCollection<LookupValue> AnnualLeaveList
        {
            get
            {
                return annualLeaveList;
            }

            set
            {
                annualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AnnualLeaveList"));
            }
        }

        public ObservableCollection<LookupValue> AnnualAdditionalLeaveList
        {
            get
            {
                return annualAdditionalLeaveList;
            }

            set
            {
                annualAdditionalLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AnnualAdditionalLeaveList"));
            }
        }

        public bool IsAdd
        {
            get
            {
                return isAdd;
            }

            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }

        public bool IsEdit
        {
            get
            {
                return isEdit;
            }

            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
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

        public ObservableCollection<LookupValue> AdditionalReasonList
        {
            get
            {
                return additionalReasonList;
            }

            set
            {
                additionalReasonList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalReasonList"));
            }
        }


        public ObservableCollection<EmployeeAnnualAdditionalLeave> EmployeeAnnualAdditionalLeaveList
        {
            get { return employeeAnnualAdditionalLeaveList; }
            set
            {
                employeeAnnualAdditionalLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAnnualAdditionalLeaveList"));
            }
        }

        public ObservableCollection<EmployeeAnnualAdditionalLeave> AdditionalLeaveListForGrid
        {
            get { return additionalLeaveListForGrid; }
            set
            {
                additionalLeaveListForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalLeaveListForGrid"));
            }
        }

        public ObservableCollection<LookupValue> TempAdditionalLookupListForGrid
        {
            get { return tempadditionalLookupListForGrid; }
            set
            {
                tempadditionalLookupListForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempAdditionalLookupListForGrid"));
            }
        }

        public int AdditionalLookupListForGridDays
        {
            get { return additionalLookupListForGridDays; }
            set
            {
                additionalLookupListForGridDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("additionalLookupListForGridDays"));
            }
        }

        public Employee ObjectEmployee
        {
            get { return objectEmployee; }
            set
            {
                objectEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjectEmployee"));
            }
        }

        public int ConvertedAdditionalDay
        {
            get
            {
                return convertedAdditionalDay;
            }

            set
            {
                convertedAdditionalDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConvertedAdditionalDay"));
            }
        }

        public decimal ConvertedAdditionalHour
        {
            get
            {
                return convertedAdditionalHour;
            }

            set
            {
                convertedAdditionalHour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConvertedAdditionalHour"));
            }
        }

        public int BacklogDays
        {
            get
            {
                return backlogDays;
            }

            set
            {
                backlogDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BacklogDays"));

            }
        }

        public decimal BacklogHours
        {
            get
            {
                return backlogHours;
            }

            set
            {
                backlogHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BacklogHours"));
            }
        }

        public decimal OldRemaining//[Sudhir.Jangra][GEOS2-4220]
        {
            get { return oldRemaining; }
            set
            {
                oldRemaining = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldRemaining"));
            }
        }

        #region shubham[skadam] GEOS2-3655 HRM - Add holiday type  08 Sep 2022
        string comments = string.Empty;
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
            }
        }
        private List<EmployeeAnnualAdditionalLeave> oldadditionalLeaveListForGrid;
        public List<EmployeeAnnualAdditionalLeave> OldAdditionalLeaveListForGrid
        {
            get { return oldadditionalLeaveListForGrid; }
            set
            {
                oldadditionalLeaveListForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldAdditionalLeaveListForGrid"));
            }
        }

        private ObservableCollection<EmployeeAnnualLeave> oldEmployeeAnnualLeave;
        public ObservableCollection<EmployeeAnnualLeave> OldEmployeeAnnualLeave
        {
            get { return oldEmployeeAnnualLeave; }
            set
            {
                oldEmployeeAnnualLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldEmployeeAnnualLeave"));
            }
        }
        #endregion

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

        #region Public Commands
        public ICommand AddAuthorizedLeaveViewCancelButtonCommand { get; set; }
        public ICommand AddAuthorizedLeaveViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        public ICommand AddAuthorizedLeaveViewCommentsAcceptButtonCommand { get; set; }
        public ICommand ValidateCellCommand { get; set; }

        public ICommand LeaveValueChangedCommand { get; set; }

        #endregion

        #region Constructor
        public AddAuthorizedLeaveViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddAuthorizedLeaveViewModel()...", category: Category.Info, priority: Priority.Low);
                AddAuthorizedLeaveViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddAuthorizedLeaveViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddAuthorizedLeave));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                ValidateCellCommand = new RelayCommand(new Action<object>(ValidateCell));
                LeaveValueChangedCommand = new RelayCommand(new Action<object>(LeaveValueChangedCommandAction));
                //FillEmployeeLeaveType(); [HRM][GEOS2-6571][14.12.2024]
                GeosApplication.Instance.Logger.Log("Constructor AddAuthorizedLeaveViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddAuthorizedLeaveViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        private void LeaveValueChangedCommandAction(object obj)
        {
            if (AnnualLeaveList[SelectedLeaveType].IdLookupValue == 241)
            {
           

                IsAcceptEnabled = false;
                if (GeosApplication.Instance.CompensationLeave == null)
                {
                    GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                }
               
               
                if (!string.IsNullOrEmpty(GeosApplication.Instance.CompensationLeave.Comments))
                {
                    AdditionalLeaveListForGrid[0].Comments = GeosApplication.Instance.CompensationLeave.Comments;
                }
                //   AdditionalLeaveListForGrid.Add(GeosApplication.Instance.CompensationLeave);
               
            }
            else
            {
                IsAcceptEnabled = true;
            }

            // }
            // }

        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {

                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32));
                    GeosApplication.Instance.EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillEmployeeLeaveList()
        {
            try
            {
                //[HRM][GEOS2-6571][14.12.2024]
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveList()...", category: Category.Info, priority: Priority.Low);
                if (ObjectEmployee != null)
                {
                    AnnualLeaveList = new ObservableCollection<LookupValue>(HrmService.GetLeavesByLocations_V2590(new List<int>() { ObjectEmployee.IdEmployee }));
                    AnnualLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });

                    if (IsAdd)
                    {
                        for (int i = 0; i < ExistEmployeeAnnualLeaveList.Count; i++)
                        {
                            LookupValue AnnualLeave = new LookupValue();
                            AnnualLeave = AnnualLeaveList.Where(x => x.IdLookupValue == Convert.ToInt32(ExistEmployeeAnnualLeaveList[i].CompanyLeave.IdCompanyLeave))?.FirstOrDefault();
                            AnnualLeaveList.Remove(AnnualLeave);

                        }
                    }
                    else if (IsEdit)
                    {
                        var TempExistEmployeeAnnualLeaveList = ExistEmployeeAnnualLeaveList.Where(x => x.CompanyLeave.IdCompanyLeave != ExistEmployeeAnnualLeave.CompanyLeave.IdCompanyLeave).ToList();
                        for (int i = 0; i < TempExistEmployeeAnnualLeaveList.Count; i++)
                        {
                            LookupValue AnnualLeave = new LookupValue();
                            AnnualLeave = AnnualLeaveList.Where(x => x.IdLookupValue == Convert.ToInt32(TempExistEmployeeAnnualLeaveList[i].CompanyLeave.IdCompanyLeave))?.FirstOrDefault();
                            AnnualLeaveList.Remove(AnnualLeave);

                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        /// Method to Add Authorized Leave
        /// [001][cpatil][25-12-2019][GEOS2-1941] GRHM - Calculation of holidays
        /// </summary>
        /// <param name="obj"></param>
        private void AddAuthorizedLeave(object obj)
        {
            try
            {
                if (OldAdditionalLeaveListForGrid != null)
                {
                    Comments = string.Empty;
                    foreach (var item in OldAdditionalLeaveListForGrid)
                    {
                        EmployeeAnnualAdditionalLeave TempEmployeeAnnualAdditionalLeave = AdditionalLeaveListForGrid.Where(w => w.IdAdditionalLeaveReason == item.IdAdditionalLeaveReason).FirstOrDefault();
                        if (TempEmployeeAnnualAdditionalLeave != null)
                        {
                            if (item.ConvertedDays == TempEmployeeAnnualAdditionalLeave.ConvertedDays)
                            {

                            }
                            else
                            {
                                if (string.IsNullOrEmpty(TempEmployeeAnnualAdditionalLeave.Comments))
                                {
                                    Comments = null;
                                    break;
                                }
                            }
                            if (item.ConvertedHours == TempEmployeeAnnualAdditionalLeave.ConvertedHours)
                            {

                            }
                            else
                            {
                                if (string.IsNullOrEmpty(TempEmployeeAnnualAdditionalLeave.Comments))
                                {
                                    Comments = null;
                                    break;
                                }
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLeaveType"));
                PropertyChanged(this, new PropertyChangedEventArgs("Comments"));
                if (error != null)
                {
                    return;
                }

                //AdditionalLeaveListForGrid
                EmployeeAnnualLeave TempEmployeeAnnualLeave = new EmployeeAnnualLeave();
                CompanyLeave companyLeave = new CompanyLeave();
                EmployeeAnnualAdditionalLeave NewEmployeeAnnualAdditionalLeave = new EmployeeAnnualAdditionalLeave();

                if (IsNew)
                {
                    //  TempCompanyShift = HrmService.GetEmployeeShiftandDailyHours(IdEmployee, IdCompanyShift, IdCompany, SelectedYear);
                    //[001] service method changed GetEmployeeEnjoyedLeaveHours to GetEmployeeEnjoyedLeaveHours_V2038
                    TempEmployeeAnnualLeave = HrmService.GetEmployeeEnjoyedLeaveHours_V2140(IdEmployee, AnnualLeaveList[SelectedLeaveType].IdLookupValue, SelectedYear, IdCompany);
                    NewEmployeeAnnualLeave = new EmployeeAnnualLeave();
                    NewEmployeeAnnualLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                    NewEmployeeAnnualLeave.IdEmployee = IdEmployee;
                    NewEmployeeAnnualLeave.IdLeave = AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    NewEmployeeAnnualLeave.LeaveInUse = AnnualLeaveList[SelectedLeaveType].InUse == true ? 1 : 0;
                    //if (TempCompanyShift.CompanyAnnualSchedule != null)
                    //{
                    NewEmployeeAnnualLeave.RegularHoursCount = (RegularDays * DailyWorkHoursCount) + RegularHours;
                    // NewEmployeeAnnualLeave.AdditionalHoursCount = (AdditionalDays * TempCompanyShift.CompanyAnnualSchedule.DailyHoursCount) + AdditionalHours;
                    NewEmployeeAnnualLeave.BacklogHoursCount = (BacklogDays * DailyWorkHoursCount) + BacklogHours;
                    //}
                    //else
                    //{
                    //    NewEmployeeAnnualLeave.RegularHoursCount = 0;
                    //    // NewEmployeeAnnualLeave.AdditionalHoursCount = 0;
                    //    NewEmployeeAnnualLeave.BacklogHoursCount = 0;

                    //}

                    companyLeave.IdCompanyLeave = (ulong)AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    companyLeave.Name = AnnualLeaveList[SelectedLeaveType].Value;
                    companyLeave.HtmlColor = AnnualLeaveList[SelectedLeaveType].HtmlColor;
                    NewEmployeeAnnualLeave.CompanyLeave = companyLeave;
                    NewEmployeeAnnualLeave.Year = SelectedYear;

                    if (NewEmployeeAnnualLeave.Employee == null)
                    {
                        NewEmployeeAnnualLeave.Employee = new Employee();
                    }

                    //if (NewEmployeeAnnualLeave.Employee.CompanyShift == null)
                    //{
                    //    NewEmployeeAnnualLeave.Employee.CompanyShift = new CompanyShift();
                    //}

                    //if (NewEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule == null)
                    //{
                    //    NewEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule = new CompanyAnnualSchedule();
                    //}

                    NewEmployeeAnnualLeave.Employee.CompanyShift = TempCompanyShift;


                    foreach (var temp in AdditionalLeaveListForGrid)
                    {
                        temp.AdditionalLeaveTotalHours = (temp.ConvertedDays * DailyWorkHoursCount) + temp.ConvertedHours;
                        //temp.IdAdditionalLeaveReason = 
                        temp.IdEmployeeAnnualLeave = NewEmployeeAnnualLeave.IdEmployeeAnnualLeave;
                        temp.IdLeave = NewEmployeeAnnualLeave.IdLeave;
                        temp.TransactionOperation = NewEmployeeAnnualLeave.TransactionOperation;
                    }

                    NewEmployeeAnnualLeave.SelectedAdditionalLeaves = new List<EmployeeAnnualAdditionalLeave>(AdditionalLeaveListForGrid);

                    decimal totalAdditionalHours = 0;

                    if (AdditionalLeaveListForGrid != null)
                    {
                        for (int i = 0; i < AdditionalLeaveListForGrid.Count; i++)
                        {
                            totalAdditionalHours += AdditionalLeaveListForGrid[i].AdditionalLeaveTotalHours;
                        }
                    }

                    NewEmployeeAnnualLeave.EnjoyedTillYesterday = TempEmployeeAnnualLeave.EnjoyedTillYesterday;
                    NewEmployeeAnnualLeave.EnjoyingFromToday = TempEmployeeAnnualLeave.EnjoyingFromToday;
                    // NewEmployeeAnnualLeave.Remaining = (NewEmployeeAnnualLeave.RegularHoursCount + NewEmployeeAnnualLeave.AdditionalHoursCount) - NewEmployeeAnnualLeave.Enjoyed;
                    NewEmployeeAnnualLeave.Remaining =
                            (NewEmployeeAnnualLeave.RegularHoursCount +
                            NewEmployeeAnnualLeave.BacklogHoursCount +
                            totalAdditionalHours) -
                            NewEmployeeAnnualLeave.EnjoyedTillYesterday -
                            NewEmployeeAnnualLeave.EnjoyingFromToday;

                    //for (int i = 0; i < ObjectEmployee.EmployeeAnnualLeaves.Count; i++)
                    //{
                    //    if (ObjectEmployee.EmployeeAnnualLeaves[i].IdLeave == AnnualLeaveList[SelectedLeaveType].IdLookupValue)
                    //    {
                    //        //AdditionalLeaveListForGrid = new ObservableCollection<EmployeeAnnualAdditionalLeave>();
                    //        //AdditionalLeaveListForGrid.AddRange(ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                    //        ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves = AdditionalLeaveListForGrid.ToList();
                    //    }

                    //}



                    //ObjectEmployee.EmployeeAnnualLeavesAdditional = new List<EmployeeAnnualAdditionalLeave>();

                    //for (int i = 0; i < ObjectEmployee.EmployeeAnnualLeaves.Count; i++)
                    //{
                    //    ObjectEmployee.EmployeeAnnualLeavesAdditional.AddRange(ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                    //}

                    //for (int i = 0; i < ObjectEmployee.EmployeeAnnualLeavesAdditional.Count; i++)
                    //{
                    //    if (ObjectEmployee.EmployeeAnnualLeavesAdditional[i].IdLeave == SelectedLeaveType &&
                    //        ObjectEmployee.EmployeeAnnualLeavesAdditional[i].IdAdditionalLeaveReason ==  )
                    //    {
                    //        //AdditionalLeaveListForGrid = new ObservableCollection<EmployeeAnnualAdditionalLeave>();
                    //        //AdditionalLeaveListForGrid.AddRange(ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                    //        ObjectEmployee.EmployeeAnnualLeavesAdditional[i] = AdditionalLeaveListForGrid.ToList();
                    //    }
                    //}

                    //Code for cell value save in additional leave grid
                    //view = obj as TableView;
                    //int? cellDays = null;
                    //decimal? cellHours = null;

                    //cellDays = EmployeeAdditionalLeaveMultipleCellEditHelper.Days;
                    //cellHours = EmployeeAdditionalLeaveMultipleCellEditHelper.Hours;

                    //IList<LookupValue> TempLookupValueList = new List<LookupValue>();
                    //LookupValue[] foundRow = AdditionalLookupListForGrid.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();

                    //foreach(LookupValue item in foundRow)
                    //{
                    //    LookupValue LV = item;
                    //    LookupValue _LookupValueItem = new LookupValue();
                    //    _LookupValueItem.IdLookupValue = item.IdLookupValue;
                    //    TempAdditionalLookupListForGrid = new ObservableCollection<LookupValue>();
                    //    TempAdditionalLookupListForGrid.AddRange(AdditionalLookupListForGrid);  // = AdditionalLookupListForGrid;

                    //    if(LV.Days != TempAdditionalLookupListForGrid[0].Days)
                    //    {
                    //        int DaysOld = TempAdditionalLookupListForGrid[0].Days;
                    //        _LookupValueItem.Days = LV.Days; //cellSubject;
                    //        int DaysNew =_LookupValueItem.Days;
                    //    }

                    //    if (LV.Hours != TempAdditionalLookupListForGrid[0].Hours)
                    //    {
                    //        decimal HoursOld = TempAdditionalLookupListForGrid[0].Days;
                    //        _LookupValueItem.Hours = LV.Hours; //cellSubject;
                    //        decimal HoursNew = _LookupValueItem.Hours;
                    //    }
                    //}

                    //NewEmployeeAnnualAdditionalLeave = new EmployeeAnnualAdditionalLeave();

                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddAuthorizedLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);


                }
                else
                {
                    UpdateEmployeeAnnualLeave = new EmployeeAnnualLeave();
                    if (ExistEmployeeAnnualLeave.IdEmployeeAnnualLeave == 0)
                    {
                        UpdateEmployeeAnnualLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                    }
                    else
                    {
                        UpdateEmployeeAnnualLeave.TransactionOperation = ModelBase.TransactionOperations.Update;
                    }
                    //UpdateEmployeeAnnualLeave.TransactionOperation = ExistEmployeeAnnualLeave.TransactionOperation;

                    UpdateEmployeeAnnualLeave.IdEmployeeAnnualLeave = ExistEmployeeAnnualLeave.IdEmployeeAnnualLeave;
                    UpdateEmployeeAnnualLeave.IdEmployee = ExistEmployeeAnnualLeave.IdEmployee;
                    UpdateEmployeeAnnualLeave.IdLeave = AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    UpdateEmployeeAnnualLeave.LeaveInUse = AnnualLeaveList[SelectedLeaveType].InUse == true ? 1 : 0;
                    UpdateEmployeeAnnualLeave.RegularHoursCount = (RegularDays * DailyWorkHoursCount) + RegularHours;
                    //UpdateEmployeeAnnualLeave.AdditionalHoursCount = (AdditionalDays * ExistEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount) + AdditionalHours;
                    UpdateEmployeeAnnualLeave.BacklogHoursCount = (BacklogDays * DailyWorkHoursCount) + BacklogHours;
                    UpdateEmployeeAnnualLeave.Year = SelectedYear;
                    //UpdateEmployeeAnnualLeave.Enjoyed = EnjoyedHours;
                    //UpdateEmployeeAnnualLeave.Remaining = (UpdateEmployeeAnnualLeave.RegularHoursCount + UpdateEmployeeAnnualLeave.AdditionalHoursCount) - UpdateEmployeeAnnualLeave.Enjoyed;


                    //NewEmployeeAnnualLeave.EnjoyedTillYesterday = TempEmployeeAnnualLeave.EnjoyedTillYesterday;
                    //NewEmployeeAnnualLeave.EnjoyingFromToday = TempEmployeeAnnualLeave.EnjoyingFromToday;
                    //// NewEmployeeAnnualLeave.Remaining = (NewEmployeeAnnualLeave.RegularHoursCount + NewEmployeeAnnualLeave.AdditionalHoursCount) - NewEmployeeAnnualLeave.Enjoyed;
                    //NewEmployeeAnnualLeave.Remaining =
                    //        (NewEmployeeAnnualLeave.RegularHoursCount +
                    //        NewEmployeeAnnualLeave.BacklogHoursCount +
                    //        totalAdditionalHours) -
                    //        NewEmployeeAnnualLeave.EnjoyedTillYesterday -
                    //        NewEmployeeAnnualLeave.EnjoyingFromToday;

                    UpdateEmployeeAnnualLeave.Employee = ExistEmployeeAnnualLeave.Employee;
                    companyLeave.IdCompanyLeave = (ulong)AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    companyLeave.Name = AnnualLeaveList[SelectedLeaveType].Value;
                    companyLeave.HtmlColor = AnnualLeaveList[SelectedLeaveType].HtmlColor;
                    UpdateEmployeeAnnualLeave.CompanyLeave = companyLeave;

                    foreach (var temp in AdditionalLeaveListForGrid)
                    {
                        temp.AdditionalLeaveTotalHours = (temp.ConvertedDays * DailyWorkHoursCount) + temp.ConvertedHours;

                        //temp.IdAdditionalLeaveReason = 
                        temp.IdEmployeeAnnualLeave = UpdateEmployeeAnnualLeave.IdEmployeeAnnualLeave;
                        temp.IdLeave = UpdateEmployeeAnnualLeave.IdLeave;
                        if (temp.IdEmployeeAnnualLeavesAdditional == 0)
                        {
                            temp.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }
                        else
                        {
                            temp.TransactionOperation = UpdateEmployeeAnnualLeave.TransactionOperation;
                        }
                    }
                    UpdateEmployeeAnnualLeave.SelectedAdditionalLeaves = new List<EmployeeAnnualAdditionalLeave>(AdditionalLeaveListForGrid);

                    if (ObjectEmployee != null)
                    {
                        for (int i = 0; i < ObjectEmployee.EmployeeAnnualLeaves.Count; i++)
                        {
                            if (ObjectEmployee.EmployeeAnnualLeaves[i].IdLeave == AnnualLeaveList[SelectedLeaveType].IdLookupValue)
                            {
                                //AdditionalLeaveListForGrid = new ObservableCollection<EmployeeAnnualAdditionalLeave>();
                                //AdditionalLeaveListForGrid.AddRange(ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                                ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves =
                                    new List<EmployeeAnnualAdditionalLeave>(AdditionalLeaveListForGrid.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
                                //for (int j = 0; j < AdditionalLeaveListForGrid.Count; j++)
                                //{
                                //    var ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave
                                //    {
                                //        AdditionalLeaveReasonName = AdditionalLeaveListForGrid[j].AdditionalLeaveReasonName,
                                //        IdAdditionalLeaveReason = AdditionalLeaveListForGrid[j].IdAdditionalLeaveReason,
                                //        AdditionalLeaveTotalHours = AdditionalLeaveListForGrid[j].AdditionalLeaveTotalHours,
                                //        ConvertedDays = AdditionalLeaveListForGrid[j].ConvertedDays,
                                //        ConvertedHours = AdditionalLeaveListForGrid[j].ConvertedHours,
                                //        IdEmployee = AdditionalLeaveListForGrid[j].IdEmployee,
                                //        Year = AdditionalLeaveListForGrid[j].Year,
                                //        TransactionOperation = AdditionalLeaveListForGrid[j].TransactionOperation,
                                //        AdditionalLeaveInUse = AdditionalLeaveListForGrid[j].AdditionalLeaveInUse,
                                //        CompanyLeave =(CompanyLeave) AdditionalLeaveListForGrid[j].CompanyLeave.Clone(),
                                //        AdditionalLeaveLookupKey = AdditionalLeaveListForGrid[j].AdditionalLeaveLookupKey,
                                //        IdLeave = AdditionalLeaveListForGrid[j].IdLeave,
                                //        IdEmployeeAnnualLeave = AdditionalLeaveListForGrid[j].IdEmployeeAnnualLeave,
                                //        IdEmployeeAnnualLeavesAdditional = AdditionalLeaveListForGrid[j].IdEmployeeAnnualLeavesAdditional
                                //    };
                                //    ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves.Add(ObjAdditionalLeave);
                                //}
                            }
                        }

                        if (ObjectEmployee.EmployeeAnnualLeavesAdditional == null)
                        {
                            ObjectEmployee.EmployeeAnnualLeavesAdditional = new List<EmployeeAnnualAdditionalLeave>();
                        }
                        else
                        {
                            ObjectEmployee.EmployeeAnnualLeavesAdditional.Clear();
                        }

                        for (int i = 0; i < ObjectEmployee.EmployeeAnnualLeaves.Count; i++)
                        {
                            ObjectEmployee.EmployeeAnnualLeavesAdditional.AddRange(ObjectEmployee.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
                        }
                    }
                    decimal totalAdditionalHours = 0;

                    if (AdditionalLeaveListForGrid != null)
                    {
                        for (int i = 0; i < AdditionalLeaveListForGrid.Count; i++)
                        {
                            totalAdditionalHours += AdditionalLeaveListForGrid[i].AdditionalLeaveTotalHours;
                        }
                    }

                    UpdateEmployeeAnnualLeave.EnjoyedTillYesterday = EnjoyedTillYesterday;
                    UpdateEmployeeAnnualLeave.EnjoyingFromToday = EnjoyingFromToday;

                    UpdateEmployeeAnnualLeave.Remaining =
                        (UpdateEmployeeAnnualLeave.RegularHoursCount +
                        UpdateEmployeeAnnualLeave.BacklogHoursCount +
                        totalAdditionalHours) -
                        UpdateEmployeeAnnualLeave.EnjoyedTillYesterday -
                        UpdateEmployeeAnnualLeave.EnjoyingFromToday;
                    if (UpdateEmployeeAnnualLeave.Remaining != OldRemaining)
                    {
                        IsSave = true;
                    }
                    else
                    {
                        IsSave = false;
                    }


                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateAuthorizedLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(EmployeeAnnualLeave selectedEmployeeAnnualLeave, ObservableCollection<EmployeeAnnualLeave> LeaveList, Employee employeeDetail)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                if (selectedEmployeeAnnualLeave.IdLeave == 241)
                {
                    IsAcceptEnabled = false;
                }
                ObjectEmployee = employeeDetail;
                WindowHeader = Application.Current.FindResource("EditAuthorizedLeave").ToString();
                ExistEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(LeaveList);
                ExistEmployeeAnnualLeave = selectedEmployeeAnnualLeave;
                SetDailyWorkHoursCountAndGetCompanyShift();
                FillEmployeeLeaveList();
                FillEmployeeAnnualLeavesAdditionalList();
                //FillEmployeeAnnualLeavesAdditionalList();
                //if (ExistEmployeeAnnualLeave.Employee.CompanyShift != null)
                //{
                //    if (ExistEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                //    {
                //        DailyWorkHoursCount = ExistEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //    }
                //    else
                //        DailyWorkHoursCount = 0;
                //}
                //else
                //    dailyWorkHoursCount = 0;

                //FillEmployeeAnnualLeavesAdditionalList();

                RegularDays = (int)(ExistEmployeeAnnualLeave.RegularHoursCount / DailyWorkHoursCount);
                RegularHours = (ExistEmployeeAnnualLeave.RegularHoursCount % DailyWorkHoursCount);
                //AdditionalDays = (int)(ExistEmployeeAnnualLeave.AdditionalHoursCount / DailyWorkHoursCount);
                //AdditionalHours = (ExistEmployeeAnnualLeave.AdditionalHoursCount % DailyWorkHoursCount);
                BacklogDays = (int)(ExistEmployeeAnnualLeave.BacklogHoursCount / DailyWorkHoursCount);
                BacklogHours = (ExistEmployeeAnnualLeave.BacklogHoursCount % DailyWorkHoursCount);
                SelectedLeaveType = AnnualLeaveList.IndexOf(AnnualLeaveList.FirstOrDefault(x => x.IdLookupValue == Convert.ToUInt16(ExistEmployeeAnnualLeave.IdLeave)));
                EnjoyedTillYesterday = ExistEmployeeAnnualLeave.EnjoyedTillYesterday;
                EnjoyingFromToday = ExistEmployeeAnnualLeave.EnjoyingFromToday;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
                // shubham[skadam] GEOS2-3655 HRM - Add holiday type  08 Sep 2022
                OldAdditionalLeaveListForGrid = new List<EmployeeAnnualAdditionalLeave>();
                if (AdditionalLeaveListForGrid != null)
                    OldAdditionalLeaveListForGrid.AddRange(AdditionalLeaveListForGrid.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());

                //foreach (EmployeeAnnualAdditionalLeave item in OldAdditionalLeaveListForGrid)
                //{
                //    item.OldAdditionalLeaveListForGrid = new List<EmployeeAnnualAdditionalLeave>();
                //    item.OldAdditionalLeaveListForGrid.AddRange(AdditionalLeaveListForGrid);
                //}
                // OldEmployeeAnnualLeave = new ObservableCollection<EmployeeAnnualLeave>();
                //OldEmployeeAnnualLeave.Add(selectedEmployeeAnnualLeave);
                decimal totalAdditionalHours = 0;

                if (AdditionalLeaveListForGrid != null)
                {
                    for (int i = 0; i < AdditionalLeaveListForGrid.Count; i++)
                    {
                        totalAdditionalHours += AdditionalLeaveListForGrid[i].AdditionalLeaveTotalHours;
                    }
                }
                var hours = (RegularDays * DailyWorkHoursCount) + RegularHours;
                var backlogHours = (BacklogDays * DailyWorkHoursCount) + BacklogHours;
                OldRemaining = (hours + backlogHours + totalAdditionalHours) - EnjoyedTillYesterday - EnjoyingFromToday;




            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InitReadOnly(EmployeeAnnualLeave selectedEmployeeAnnualLeave, ObservableCollection<EmployeeAnnualLeave> LeaveList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                ObjectEmployee = ExistEmployeeAnnualLeave.Employee;
                WindowHeader = Application.Current.FindResource("EditAuthorizedLeave").ToString();
                ExistEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(LeaveList);
                ExistEmployeeAnnualLeave = selectedEmployeeAnnualLeave;
                SetDailyWorkHoursCountAndGetCompanyShift();
                FillEmployeeLeaveList();
                FillEmployeeAnnualLeavesAdditionalList();
                //if (ExistEmployeeAnnualLeave.Employee.CompanyShift != null)
                //{
                //    if (ExistEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule != null)
                //    {
                //        DailyWorkHoursCount = ExistEmployeeAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //    }
                //    else
                //        DailyWorkHoursCount = 0;
                //}
                //else
                //    dailyWorkHoursCount = 0;

                RegularDays = (int)(ExistEmployeeAnnualLeave.RegularHoursCount / DailyWorkHoursCount);
                RegularHours = (ExistEmployeeAnnualLeave.RegularHoursCount % DailyWorkHoursCount);
                //AdditionalDays = (int)(ExistEmployeeAnnualLeave.AdditionalHoursCount / DailyWorkHoursCount);
                //AdditionalHours = (ExistEmployeeAnnualLeave.AdditionalHoursCount % DailyWorkHoursCount);
                BacklogDays = (int)(ExistEmployeeAnnualLeave.BacklogHoursCount / DailyWorkHoursCount);
                BacklogHours = (ExistEmployeeAnnualLeave.BacklogHoursCount % DailyWorkHoursCount);
                SelectedLeaveType = AnnualLeaveList.IndexOf(AnnualLeaveList.FirstOrDefault(x => x.IdLookupValue == Convert.ToUInt16(ExistEmployeeAnnualLeave.IdLeave)));
                EnjoyedTillYesterday = ExistEmployeeAnnualLeave.EnjoyedTillYesterday;
                EnjoyingFromToday = ExistEmployeeAnnualLeave.EnjoyingFromToday;


                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        public void Init(ObservableCollection<EmployeeAnnualLeave> AnnualLeaveList, Employee employeeDetail)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                ObjectEmployee = employeeDetail;
                WindowHeader = Application.Current.FindResource("AddAuthorizedLeave").ToString();
                AdditionalDays = 0;
                AdditionalHours = 0;
                RegularDays = 0;
                RegularHours = 0;
                ExistEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(AnnualLeaveList);
                SetDailyWorkHoursCountAndGetCompanyShift();
                FillEmployeeLeaveList();
                FillEmployeeAnnualLeavesAdditionalList();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
                // shubham[skadam] GEOS2-3655 HRM - Add holiday type  08 Sep 2022
                // shubham[skadam] GEOS2-3655 HRM - Add holiday type  08 Sep 2022
                OldAdditionalLeaveListForGrid = new List<EmployeeAnnualAdditionalLeave>();
                if (AdditionalLeaveListForGrid != null)
                    OldAdditionalLeaveListForGrid.AddRange(AdditionalLeaveListForGrid.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private CompanyShift SetDailyWorkHoursCountAndGetCompanyShift()
        {
            TempCompanyShift = HrmService.GetEmployeeShiftandDailyHours(IdEmployee, IdCompanyShift, IdCompany, SelectedYear);

            if (ObjectEmployee != null && ObjectEmployee.IdEmployee != 0)
            {
                var list = HrmService.GetEmployeeCompanySchedule(ObjectEmployee.IdEmployee, SelectedYear);
                if (list != null && list.Count > 0 && list[0].CompanyAnnualSchedule != null)
                {
                    DailyWorkHoursCount = list[0].CompanyAnnualSchedule.DailyHoursCount;
                    if (TempCompanyShift != null)
                    {
                        TempCompanyShift.CompanyAnnualSchedule = list[0].CompanyAnnualSchedule;
                    }
                }
                else
                {
                    DailyWorkHoursCount = 0;
                }
            }
            //if (NewEmployeeAnnualLeave.Employee == null)
            //{
            //    NewEmployeeAnnualLeave.Employee = new Employee();
            //}
            if (TempCompanyShift == null)
            {
                TempCompanyShift = new CompanyShift();
            }

            if (TempCompanyShift.CompanyAnnualSchedule == null)
            {
                TempCompanyShift.CompanyAnnualSchedule = new CompanyAnnualSchedule();
            }

            TempCompanyShift.CompanyAnnualSchedule.DailyHoursCount = DailyWorkHoursCount;


            return TempCompanyShift;
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
        private ObservableCollection<LookupValue> additionalReasonList;
        private ObservableCollection<EmployeeAnnualAdditionalLeave> employeeAnnualLeavesAdditionalList;
        private ObservableCollection<EmployeeAnnualAdditionalLeave> employeeAnnualAdditionalLeaveList;
        private ObservableCollection<LookupValue> annualAdditionalLeavesLookupList;
        private ObservableCollection<EmployeeAnnualAdditionalLeave> additionalLeaveListForGrid;
        private int additionalLookupListForGridDays;
        private Employee objectEmployee;
        private ObservableCollection<LookupValue> tempadditionalLookupListForGrid;
        private int convertedAdditionalDay;
        private decimal convertedAdditionalHour;

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
                    me[BindableBase.GetPropertyName(() => SelectedLeaveType)]
                        + me[BindableBase.GetPropertyName(() => Comments)]
                      ;

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
                string _selectedType = BindableBase.GetPropertyName(() => SelectedLeaveType);
                string _Comments = BindableBase.GetPropertyName(() => Comments);

                if (columnName == _selectedType)
                {
                    return AuthorizedLeaveValidation.GetErrorMessage(_selectedType, SelectedLeaveType);
                }
                if (columnName == _Comments)
                {
                    return AuthorizedLeaveValidation.GetErrorMessage(_Comments, Comments);
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


        public void FillEmployeeAnnualLeavesAdditionalList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeAnnualLeavesAdditionalList()...", category: Category.Info, priority: Priority.Low);

                AdditionalLeaveListForGrid = new ObservableCollection<EmployeeAnnualAdditionalLeave>();

                if (ExistEmployeeAnnualLeave != null)
                {
                    AdditionalLeaveListForGrid.AddRange(
                        ExistEmployeeAnnualLeave.SelectedAdditionalLeaves.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
                }

                if (AdditionalLeaveListForGrid.Count == 0)
                {
                    for (int i = 0; i < ExistEmployeeAnnualLeaveList.Count; i++)
                    {
                        if (ExistEmployeeAnnualLeaveList[i].IdLeave == AnnualLeaveList[SelectedLeaveType].IdLookupValue)
                        {

                            AdditionalLeaveListForGrid.AddRange(ExistEmployeeAnnualLeaveList[i].SelectedAdditionalLeaves.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
                        }
                    }
                }

                List<LookupValue> AnnualAdditionalLeavesLookupList = new List<LookupValue>(CrmStartUp.GetLookupValues(76));

                //if (AdditionalLeaveListForGrid.Count < AnnualAdditionalLeavesLookupList.Count)
                //{

                // AdditionalLeaveListForGrid = new ObservableCollection<EmployeeAnnualAdditionalLeave>(ObjectEmployee.EmployeeAnnualLeaves.Select(x => x.SelectedAdditionalLeaves).FirstOrDefault());

                for (int i = 0; i < AnnualAdditionalLeavesLookupList.Count; i++)
                {
                    var leaveObj = AdditionalLeaveListForGrid.FirstOrDefault(x => x.IdAdditionalLeaveReason ==
                    AnnualAdditionalLeavesLookupList[i].IdLookupValue);
                    if (leaveObj == null && AnnualAdditionalLeavesLookupList[i].InUse)
                    {
                        EmployeeAnnualAdditionalLeave ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave();
                        ObjAdditionalLeave.AdditionalLeaveReasonName = AnnualAdditionalLeavesLookupList[i].Value;
                        ObjAdditionalLeave.IdAdditionalLeaveReason = AnnualAdditionalLeavesLookupList[i].IdLookupValue;
                        ObjAdditionalLeave.AdditionalLeaveTotalHours = 0;
                        ObjAdditionalLeave.ConvertedDays = 0;
                        ObjAdditionalLeave.ConvertedHours = 0;
                        if (ObjectEmployee != null)
                        {
                            ObjAdditionalLeave.IdEmployee = ObjectEmployee.IdEmployee;
                        }
                        ObjAdditionalLeave.Year = SelectedYear;
                        ObjAdditionalLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                        AdditionalLeaveListForGrid.Add(ObjAdditionalLeave);
                    }
                }
                var list = AdditionalLeaveListForGrid.OrderBy(x => x.AdditionalLeavePosition).ToList();
                AdditionalLeaveListForGrid.Clear();
                AdditionalLeaveListForGrid.AddRange(list);
                //}

                if (IsAdd)
                {
                    //foreach (var temp in AdditionalLeaveListForGrid)
                    //{
                    //    // EmployeeAnnualAdditionalLeave ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave();
                    //    temp.ConvertedDays = 0;
                    //    temp.ConvertedHours = 0;
                    //    ConvertedAdditionalDay = 0;
                    //    ConvertedAdditionalHour = 0;
                    //    //lv.IdLookupValue = temp.IdLookupValue;
                    //    //lv.Value = temp.Value;
                    //    //lv.Hours = 0;
                    //    //lv.Days = 0;
                    //    // AdditionalLookupListForGrid.Add(lv);
                    //}
                }

                //if (IsEdit)
                //{
                //List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                //ObjectEmployee = new Employee();
                //ObjectEmployee = HrmService.GetEmployeeByIdEmployee_V2150(IdEmployee, SelectedYear, plantOwnersIds);
                //EmployeeAnnualAdditionalLeaveList = new ObservableCollection<EmployeeAnnualAdditionalLeave>();
                //EmployeeAnnualAdditionalLeaveList.AddRange(ObjectEmployee.EmployeeAnnualLeavesAdditional);

                //List<int> IntList = new List<int>();

                //IntList = AnnualAdditionalLeavesLookupList.Where(x => EmployeeAnnualAdditionalLeaveList.Select(y => y.IdAdditionalLeaveReason).ToList().Contains(x.IdLookupValue)).Select(p => p.IdLookupValue).ToList();


                //if (IntList.Count > 0)

                foreach (var temp in AdditionalLeaveListForGrid)
                {
                    //temp.AdditionalLeaveTotalHours = 30;
                    temp.ConvertedDays = (int)(temp.AdditionalLeaveTotalHours / DailyWorkHoursCount);
                    temp.ConvertedHours = temp.AdditionalLeaveTotalHours % DailyWorkHoursCount;

                    //temp.ConvertedDays = 
                    // EmployeeAnnualAdditionalLeave ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave();
                    //lv.IdLookupValue = temp.IdAdditionalLeaveReason;
                    //    lv.Value = AnnualAdditionalLeavesLookupList.Where(x => x.IdLookupValue == temp.IdAdditionalLeaveReason).Select(p => p.Value).FirstOrDefault();
                    //lv.Hours = temp.Hours;
                    //if (temp.AdditionalLeaveTotalHours > 24)
                    //{
                    // temp.ConvertedDays = Convert.ToInt32()
                    //lv.Days = Convert.ToInt32( temp.AdditionalLeaveTotalHours / dailyWorkHoursCount);
                    //lv.Hours = temp.AdditionalLeaveTotalHours % dailyWorkHoursCount;
                    //lv.IsUpdatedRow = false;   
                    //}
                    //AdditionalLookupListForGrid.Add(lv); //= AnnualAdditionalLeavesLookupList.Where(x=> x.IdLookupValue == )
                }
                //}

                //else
                //{
                //    foreach (var temp in AnnualAdditionalLeavesLookupList)
                //    {
                //        LookupValue lv = new LookupValue();
                //        lv.IdLookupValue = temp.IdLookupValue;
                //        lv.Value = temp.Value;
                //        //lv.Hours = 0;
                //        //lv.Days = 0;
                //        //lv.IsUpdatedRow = false;
                //       // AdditionalLookupListForGrid.Add(lv);
                //    }
                //}
                //}

                //List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                //Employee employee = HrmService.GetEmployeeByIdEmployee_V2150(IdEmployee, SelectedYear, plantOwnersIds);
                //EmployeeAnnualAdditionalLeaveList = new ObservableCollection<EmployeeAnnualAdditionalLeave>();
                //EmployeeAnnualAdditionalLeaveList.AddRange(employee.EmployeeAnnualLeavesAdditional);

                //List<int> IntList = new List<int>();

                //IntList = AnnualAdditionalLeavesLookupList.Where(x => EmployeeAnnualAdditionalLeaveList.Select(y => y.IdAdditionalLeaveReason).ToList().Contains(x.IdLookupValue)).Select(p => p.IdLookupValue).ToList();

                //AdditionalLookupListForGrid = new ObservableCollection<LookupValue>();
                //if (IntList.Count > 0)
                //{
                //    foreach (var temp in EmployeeAnnualAdditionalLeaveList)
                //    {
                //        LookupValue lv = new LookupValue();
                //        lv.IdLookupValue = temp.IdAdditionalLeaveReason;
                //        lv.Value = AnnualAdditionalLeavesLookupList.Where(x => x.IdLookupValue == temp.IdAdditionalLeaveReason).Select(p => p.Value).FirstOrDefault();
                //        lv.Hours = temp.Hours;
                //        lv.Days = 20; //(temp.Hours - )
                //        //foreach (var temp2 in AnnualAdditionalLeavesLookupList)
                //        //AnnualAdditionalLeavesLookupList.)
                //        // LookupValue abc = AnnualAdditionalLeavesLookupList.Where(x => x.Da == temp).FirstOrDefault();
                //        AdditionalLookupListForGrid.Add(lv); //= AnnualAdditionalLeavesLookupList.Where(x=> x.IdLookupValue == )
                //    }
                //}
                //else
                //{
                //    foreach (var temp in AnnualAdditionalLeavesLookupList)
                //    {
                //        LookupValue lv = new LookupValue();
                //        lv.IdLookupValue = temp.IdLookupValue;
                //        lv.Value = temp.Value;
                //        lv.Hours = 0;
                //        lv.Days = 0;
                //        AdditionalLookupListForGrid.Add(lv); 
                //    }
                //}



                //new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(76));
                ////AdditionalReasonList = CrmStartUp.GetLookupValues(77)
                //if (AnnualAdditionalLeavesLookupList.Contains(EmployeeAnnualAdditionalLeaveList.Where(x => x.IdAdditionalLeaveReason)
                // IntList = AnnualAdditionalLeavesLookupList.Where(x => EmployeeAnnualAdditionalLeaveList.All(y => y.IdAdditionalLeaveReason == x.IdLookupValue)).Select(x => x.IdLookupValue).ToList();
                //foreach (var temp in IntList)
                //{
                //    EmployeeAnnualAdditionalLeave  pqr = EmployeeAnnualAdditionalLeaveList.Where(x => x.IdAdditionalLeaveReason == temp).FirstOrDefault();
                //}

                // if(AnnualAdditionalLeavesLookupList.Any(x=>x.))

                //var LMN = AnnualAdditionalLeavesLookupList.Any(p => EmployeeAnnualAdditionalLeaveList.Contains(p.IdLookupValue))

                //AnnualAdditionalLeavesLookupList.Any(x => EmployeeAnnualAdditionalLeaveList.Any(y => y.IdAdditionalLeaveReason == x.IdLookupKey))

                //if (AnnualAdditionalLeavesLookupList.Any(x => EmployeeAnnualAdditionalLeaveList.Any(y => y.IdAdditionalLeaveReason == x.IdLookupKey)))
                //{

                //}

                //if (IsAdd)
                //{

                //}
                //if (IsEdit)
                //{
                //    foreach (var temp in AnnualAdditionalLeavesLookupList)
                //    {
                //        if (EmployeeAnnualAdditionalLeaveList.Any(x => x.IdAdditionalLeaveReason == temp.IdLookupKey))
                //        {

                //        }
                //    }
                //}




                //AnnualAdditionalLeavesLookupList.Where(x => x.IdLookupKey == EmployeeAnnualAdditionalLeaveList.);

                GeosApplication.Instance.Logger.Log("Method FillEmployeeAnnualLeavesAdditionalList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeAnnualLeavesAdditionalList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region  shubham[skadam] GEOS2-3655 HRM - Add holiday type  13 Sep 2022
        Int64 OldcellValue = 0;
        public void ValidateCell(object args)
        {
            try
            {
                DevExpress.Xpf.Grid.GridCellValidationEventArgs e = (DevExpress.Xpf.Grid.GridCellValidationEventArgs)(args);
                if (e.Column.FieldName == "ConvertedDays")
                {
                    var cellValue = Convert.ToInt64(e.CellValue);
                    //if (OldcellValue== cellValue)
                    //{
                    //    return;
                    //}
                    OldcellValue = cellValue;
                    EmployeeAnnualAdditionalLeave employeeAnnualAdditionalLeave = (EmployeeAnnualAdditionalLeave)e.Row;
                    if (string.IsNullOrEmpty(employeeAnnualAdditionalLeave.Comments))
                    {
                        e.IsValid = false;
                        //e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        e.ErrorContent = "You cannot leave the comments field empty";
                    }

                }
                if (e.Column.FieldName == "ConvertedHours")
                {
                    var cellValue = Convert.ToInt64(e.CellValue);
                    EmployeeAnnualAdditionalLeave employeeAnnualAdditionalLeave = (EmployeeAnnualAdditionalLeave)e.Row;
                    if (string.IsNullOrEmpty(employeeAnnualAdditionalLeave.Comments))
                    {
                        e.IsValid = false;
                        //e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        e.ErrorContent = "You cannot leave the comments field empty";
                    }

                }

            }
            catch (Exception ex)
            {

            }

        }




        //private void TableView_ValidateCell(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        //{
        //    if (e.Column.FieldName == "Comments")
        //    {
        //        e.IsValid = !string.IsNullOrEmpty(e.Value as string);
        //    }
        //}

        //private void TableView_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        //{
        //    var item = e.Row as EmployeeAnnualAdditionalLeave;
        //    if (string.IsNullOrEmpty(item.Comments))
        //    {
        //        e.IsValid = false;
        //        e.ErrorContent = "You cannot leave the Comments empty";
        //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;

        //    }
        //}

        //private void TableView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        //{
        //    e.ErrorText = "You cannot leave the Comments empty.";
        //}
        #endregion
    }
    #region  shubham[skadam] GEOS2-3655 HRM - Add holiday type  13 Sep 2022
    //public class TableViewEx : TableView
    //{
    //    protected override MessageBoxResult DisplayInvalidRowError(IInvalidRowExceptionEventArgs e)
    //    {
    //        var result = base.DisplayInvalidRowError(e);
    //        if (result == MessageBoxResult.Yes)
    //        {
    //            Dispatcher.BeginInvoke(new Action(() => {
    //                this.Grid.CurrentColumn = this.Grid.Columns["Comments"];
    //            }), DispatcherPriority.Loaded);
    //        }
    //        else
    //        {
    //            Dispatcher.BeginInvoke(new Action(() => {
    //                this.Grid.CurrentColumn = this.Grid.Columns["Comments"];
    //            }), DispatcherPriority.Loaded);
    //        }
    //        return result;
    //    }
    //}
    #endregion
}
