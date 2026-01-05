using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlantOperationalRealTimeViewModel : INotifyPropertyChanged
    {
        #region Declaration
        private double dialogHeight;
        private double dialogWidth;
        private List<ERMNonOTItemType> timeTypesList;
        private ERMNonOTItemType selectedReal;
        private string windowHeader;
        private List<ERMEmployeeLeave> leaveTypesList;  //[GEOS2-4707][rupali sarode][25-07-2023]
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
        private const bool TempTrue=true;
        private const bool TempFalse = false;
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
        private Int32 real;
        #endregion

        #region Property
        private ObservableCollection<DocumentPanel> listDocumentPanel;
        public ObservableCollection<DocumentPanel> ListDocumentPanel
        {
            get { return listDocumentPanel; }
            set
            {
                listDocumentPanel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDocumentPanel"));
            }
        }

        ObservableCollection<DocumentPanel> listDocumentPanelNew;
        public ObservableCollection<DocumentPanel> ListDocumentPanelNew
        {
            get { return listDocumentPanelNew; }
            set
            {
                listDocumentPanelNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDocumentPanelNew"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public List<ERMNonOTItemType> TimeTypesList
        {
            get { return timeTypesList; }
            set
            {
                timeTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeTypesList"));
            }
        }
        public Int32 Real
        {
            get { return real; }
            set
            {
                real = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Real"));
            }
        }
        public ERMNonOTItemType SelectedReal
        {
            get { return selectedReal; }
            set
            {
                selectedReal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReal"));
            }
        }


        #region [GEOS2-4707][rupali sarode][25-07-2023]
        public List<ERMEmployeeLeave> LeaveTypesList
        {
            get { return leaveTypesList; }
            set
            {
                leaveTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveTypesList"));
            }
        }

        private Visibility isGridControlVisible;
        public Visibility IsGridControlVisible
        {
            get { return isGridControlVisible; }
            set
            {
                isGridControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridControlVisible"));
            }
        }
        #endregion

        private ObservableCollection<BandItem> bands = new ObservableCollection<BandItem>();
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }
        #endregion

        #region Commands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        #endregion


        #region Constructor
        public PlantOperationalRealTimeViewModel()
        {
            try
            {
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 600;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 600;
                //GeosApplication.Instance.Logger.Log("Constructor AddEditEquivalentWeightViewModel ...", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                //AcceptEquivalentWeightActionCommand = new DelegateCommand<object>(AcceptEquivalentWeightAction);
            }
            catch (Exception ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                //GeosApplication.Instance.Logger.Log("Constructor AddEditEquivalentWeightViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
        }

        #endregion
        #region Method

        //public void Init(List<ERMNonOTItemType> NonOTTimeTypesList, List<ERMPlantOperationalPlanning> PlantOperationalPlanning, uint IdEmployee, string CalenderWeek, int HRPlan,List<int> IDEmployeeJobDescriptionList, List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription, ObservableCollection<PlanningDateReviewStages> PlantOperationStagesList)
        #region [GEOS2-4839][gulabrao lakade][27 09 2023]
        //public void Init(List<ERMNonOTItemType> NonOTTimeTypesList, List<ERMPlantOperationalPlanning> PlantOperationalPlanning, string IdStage, uint IdEmployee, string CalenderWeek, int HRPlan, List<Int32> IDEmployeesJobDescriptionList)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        List<PlantOperationalPlanningRealInfo> FinalTempPlantOperationalPlanningRealInfo = new List<PlantOperationalPlanningRealInfo>();
        //        var TempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && a.PlantOperationalPlanningRealInfo.Any(p => p.Idstage == Convert.ToInt32(IdStage) && p.CalenderWeek == CalenderWeek)).ToList();
        //        foreach (var item in TempPlantOperationalPlanning)
        //        {
        //            FinalTempPlantOperationalPlanningRealInfo.AddRange(item.PlantOperationalPlanningRealInfo.Where(a => a.Idstage == Convert.ToInt32(IdStage) && a.IdEmployee == Convert.ToInt32(IdEmployee) && a.CalenderWeek == CalenderWeek));
        //        }

        //        PlantOperationalPlanningRealInfo TempPlantOperationalPlanningRealInfo = new PlantOperationalPlanningRealInfo
        //        {

        //            IdReason = 2,
        //            IdEmployee = Convert.ToInt32(IdEmployee),
        //            ReasonValue = "HR Plan",
        //            CalenderWeek = CalenderWeek,
        //        };
        //        FinalTempPlantOperationalPlanningRealInfo.Add(TempPlantOperationalPlanningRealInfo);

        //        TimeTypesList = NonOTTimeTypesList;
        //        List<ERMNonOTItemType> TempTimeTypesList = new List<ERMNonOTItemType>();
        //        foreach (var item in TimeTypesList)
        //        {
        //            var tempRealvalue = FinalTempPlantOperationalPlanningRealInfo.Where(a => a.ReasonValue == item.ReasonValue).ToList();
        //            if (tempRealvalue.Count() > 0)
        //            {
        //                foreach (var itemreal in tempRealvalue)
        //                {
        //                    DateTime timeStart = Convert.ToDateTime(itemreal.StartTime);
        //                    DateTime timeEnd = Convert.ToDateTime(itemreal.EndTime);
        //                    if (timeStart <= timeEnd)
        //                    {
        //                        SelectedReal = new ERMNonOTItemType();
        //                        SelectedReal.IdReason = Convert.ToInt32(itemreal.IdReason);
        //                        SelectedReal.ReasonValue = itemreal.ReasonValue;

        //                        TimeSpan TSReal = timeEnd - timeStart;
        //                        SelectedReal.Real = Convert.ToInt32(TSReal.TotalMinutes);
        //                        TempTimeTypesList.Add(SelectedReal);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //TimeTypesList = new List<ERMNonOTItemType>();
        //                SelectedReal = new ERMNonOTItemType();
        //                SelectedReal.IdReason = Convert.ToInt32(item.IdReason);
        //                SelectedReal.ReasonValue = item.ReasonValue;
        //                // SelectedReal.Real = timeDifference;
        //                SelectedReal.Real = 0;// Convert.ToInt32(TSReal.TotalMinutes);


        //                TempTimeTypesList.Add(SelectedReal);
        //            }
        //        }
        //        var sumByReasonValue = TempTimeTypesList.GroupBy(a => a.ReasonValue).Select(group => new
        //        {
        //            ReasonValue = group.Key,
        //            SumReal = group.Sum(b => b.Real)
        //        }).ToList();
        //        TimeTypesList = new List<ERMNonOTItemType>();
        //        foreach (var Temptest in sumByReasonValue)
        //        {
        //            SelectedReal = new ERMNonOTItemType();
        //            SelectedReal.ReasonValue = Temptest.ReasonValue;
        //            SelectedReal.Real = Temptest.SumReal;
        //            if (Temptest.ReasonValue == "HR Plan")
        //            {
        //                //   TimeTypesList
        //                // SelectedReal.IdReason = 2;
        //                int sumreal = TimeTypesList.Sum(i => i.Real);
        //                float tempHRPlan = (float)TimeSpan.FromHours(Convert.ToDouble(HRPlan)).TotalMinutes;
        //                SelectedReal.Real = Convert.ToInt32(Math.Round(tempHRPlan, 0)) - sumreal;

        //                TimeTypesList.Where(a => a.IdReason == 2).ToList();
        //            }
        //            TimeTypesList.Add(SelectedReal);
        //        }

        //        #region [GEOS2-4707][rupali sarode][25-07-2023]

        //        List<ERMEmployeeLeave> FinalTempPlantOperationalPlanningLeave = new List<ERMEmployeeLeave>();

        //        decimal TotalJobDescriptionUsage = 0;
        //        LeaveTypesList = new List<ERMEmployeeLeave>();

        //        List<ERMPlantOperationalPlanning> tempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && IDEmployeesJobDescriptionList.Contains(a.IdJobDescription) && a.EmployeeLeave.Any(x => x.CalenderWeek == CalenderWeek)).ToList();
        //        //List<ERMPlantOperationalPlanning> tempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && IDEmployeesJobDescriptionList.Contains(a.IdJobDescription)).ToList();
        //        if (tempPlantOperationalPlanning.Count > 0)
        //        {
        //            TotalJobDescriptionUsage = Convert.ToDecimal(tempPlantOperationalPlanning.Sum(a => a.JobDescriptionUsage));
        //            var Templeave = tempPlantOperationalPlanning.FirstOrDefault().EmployeeLeave.Where(a => a.CalenderWeek == CalenderWeek).ToList();

        //            decimal LeaveMinutes = 0;
        //            //var Templeave=  item.EmployeeLeave.Where(a => a.CalenderWeek == CalenderWeek).ToList();
        //            var LeaveDayOfWeek = 0;
        //            if (Templeave != null)
        //            {

        //                foreach (var leaveitem in Templeave)
        //                {
        //                    ERMEmployeeLeave LeaveType = new ERMEmployeeLeave();
        //                    double TotalLeaveCount = 0;

        //                    if (leaveitem.IsAllDayEvent == 0)
        //                    {
        //                        TimeSpan StartDateTime = Convert.ToDateTime(leaveitem.StartDate).TimeOfDay;
        //                        TimeSpan EndDateTime = Convert.ToDateTime(leaveitem.EndDate).TimeOfDay;

        //                        if (StartDateTime <= EndDateTime)
        //                        {
        //                            LeaveDayOfWeek = (int)Convert.ToDateTime(leaveitem.StartDate).DayOfWeek;

        //                            // LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime).TotalMinutes);
        //                            switch (LeaveDayOfWeek)
        //                            {

        //                                case 1:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.MonBreakTime).TotalMinutes);
        //                                    break;

        //                                case 2:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.TueBreakTime).TotalMinutes);
        //                                    break;

        //                                case 3:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.WedBreakTime).TotalMinutes);
        //                                    break;

        //                                case 4:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.ThuBreakTime).TotalMinutes);
        //                                    break;

        //                                case 5:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.FriBreakTime).TotalMinutes);
        //                                    break;

        //                                case 6:

        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.SatBreakTime).TotalMinutes);
        //                                    break;

        //                                case 7:
        //                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.SunBreakTime).TotalMinutes);
        //                                    break;

        //                            }

        //                            LeaveType.LeaveMinutes = Math.Round((LeaveMinutes * TotalJobDescriptionUsage) / 100);
        //                        }
        //                    }
        //                    else if (leaveitem.IsAllDayEvent == 1)
        //                    {
        //                        //Substract holiday if exists in leave

        //                        var Holiday = PlantOperationalPlanning.FirstOrDefault().CompanyHoliday.Where(x => x.StartDate >= leaveitem.StartDate && x.EndDate <= leaveitem.EndDate).FirstOrDefault();
        //                        int TotalHolidayCount = 0;
        //                        double TotalHolidayHours = 0;

        //                        if (leaveitem.StartDate <= leaveitem.EndDate)
        //                        {
        //                            if (Holiday != null)
        //                            {
        //                                if (Holiday.IsAllDayEvent == 1)
        //                                {
        //                                    TotalHolidayCount = 1 + Convert.ToInt32((Convert.ToDateTime(Holiday.EndDate).Date - Convert.ToDateTime(Holiday.StartDate).Date).TotalDays);
        //                                }
        //                                else if (Holiday.IsAllDayEvent == 0)
        //                                {

        //                                    TimeSpan StartDateTime = Convert.ToDateTime(Holiday.StartDate).TimeOfDay;
        //                                    TimeSpan EndDateTime = Convert.ToDateTime(Holiday.EndDate).TimeOfDay;

        //                                    TotalHolidayHours = Convert.ToDouble((EndDateTime - StartDateTime).TotalHours);
        //                                }
        //                            }

        //                            int DailyHoursCount = 0;
        //                            //Find daily hours count
        //                            DailyHoursCount = tempPlantOperationalPlanning.FirstOrDefault().EmployeeInformation.FirstOrDefault().DailyHoursCount;

        //                            //calculate breaktime for multiple days

        //                            DateTime? LeaveStartDate = leaveitem.StartDate;
        //                            DateTime? LeaveEndDate = leaveitem.EndDate;

        //                            #region [GEOS2-4813][Rupali Sarode][12-09-2023]
        //                            // TimeSpan BreakTime = new TimeSpan(0, 0, 0);
        //                            double BreakTimeMinutes = 0;
        //                            double BreakTimeMinutesMain = 0;
        //                            // TimeSpan BreakTimeMain = new TimeSpan(0, 0, 0);

        //                            while (LeaveEndDate >= LeaveStartDate)
        //                            {
        //                                switch ((int)Convert.ToDateTime(LeaveStartDate).DayOfWeek)
        //                                {

        //                                    case 1:
        //                                        BreakTimeMinutes = leaveitem.MonBreakTime.Minutes;

        //                                        break;

        //                                    case 2:

        //                                        BreakTimeMinutes = leaveitem.TueBreakTime.Minutes;
        //                                        break;

        //                                    case 3:
        //                                        BreakTimeMinutes = leaveitem.WedBreakTime.Minutes;
        //                                        break;

        //                                    case 4:
        //                                        BreakTimeMinutes = leaveitem.ThuBreakTime.Minutes;
        //                                        break;

        //                                    case 5:
        //                                        BreakTimeMinutes = leaveitem.FriBreakTime.Minutes;
        //                                        break;

        //                                    case 6:
        //                                        BreakTimeMinutes = leaveitem.SatBreakTime.Minutes;
        //                                        break;

        //                                    case 7:
        //                                        BreakTimeMinutes = leaveitem.SunBreakTime.Minutes;
        //                                        break;

        //                                }

        //                                BreakTimeMinutesMain = BreakTimeMinutesMain + BreakTimeMinutes;

        //                                #endregion [GEOS2-4813][Rupali Sarode][12-09-2023]

        //                                LeaveStartDate = Convert.ToDateTime(LeaveStartDate).AddDays(1);
        //                            }
        //                            TimeSpan TimeDifference = new TimeSpan();
        //                            if (leaveitem.IsAllDayEvent == 1)
        //                                TimeDifference = (Convert.ToDateTime(leaveitem.EndDate.Value.AddDays(1)) - Convert.ToDateTime(leaveitem.StartDate));
        //                            else
        //                                TimeDifference = (Convert.ToDateTime(leaveitem.EndDate) - Convert.ToDateTime(leaveitem.StartDate));

        //                            double TotalDays = 0;
        //                            TotalDays = TimeDifference.Days - TotalHolidayCount; //Substract Holidays

        //                            double TotalHours = (TotalDays * DailyHoursCount) - TotalHolidayHours;  //Substract Holiday hours

        //                            //  decimal MinutesForDay = Convert.ToDecimal(TimeSpan.FromHours(TotalHours).TotalMinutes + TimeSpan.FromHours(TimeDifference.Hours).TotalMinutes +
        //                            //  TimeSpan.FromMinutes(TimeDifference.Minutes).TotalMinutes);
        //                            decimal MinutesForDay = 0;
        //                            // [GEOS2-4813][Rupali Sarode][12-09-2023]
        //                            if (leaveitem.IsAllDayEvent == 1)
        //                                MinutesForDay = Convert.ToDecimal(TimeSpan.FromHours(TotalHours).TotalMinutes - BreakTimeMinutesMain);
        //                            else
        //                                MinutesForDay = Convert.ToDecimal((TimeSpan.FromHours(TotalHours).TotalMinutes + TimeSpan.FromHours(TimeDifference.Hours).TotalMinutes +
        //                         TimeSpan.FromMinutes(TimeDifference.Minutes).TotalMinutes) - BreakTimeMinutesMain);



        //                            LeaveType.LeaveMinutes = Math.Round((MinutesForDay * TotalJobDescriptionUsage) / 100);
        //                        }


        //                    }

        //                    LeaveType.LeaveType = leaveitem.LeaveType;

        //                    LeaveTypesList.Add(LeaveType);

        //                }
        //            }

        //        }

        //        if (LeaveTypesList.Count > 0)
        //        {
        //            IsGridControlVisible = Visibility.Visible;
        //        }
        //        else
        //        {
        //            IsGridControlVisible = Visibility.Hidden;
        //        }

        //        #endregion [GEOS2-4707][rupali sarode][25-07-2023]

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
        //    }



        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        public void Init(List<ERMNonOTItemType> NonOTTimeTypesList, List<ERMPlantOperationalPlanning> PlantOperationalPlanning, uint IdEmployee, string CalenderWeek, Dictionary<int, Int32> HRPLANDictionary, ObservableCollection<PlanningDateReviewStages> PlantOperationStagesList, List<int> IDEmployeeJobDescriptionList,List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription, List<PlantOperationWeek> PlantWeekList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                List<PlantOperationalPlanningRealInfo> FinalTempPlantOperationalPlanningRealInfo = new List<PlantOperationalPlanningRealInfo>();
                PlantOperationWeek SelectedWeek = new PlantOperationWeek();
                SelectedWeek = PlantWeekList.Where(i => i.CalenderWeek == CalenderWeek).FirstOrDefault();
                
                if (HRPLANDictionary != null)
                {

                    ListDocumentPanel = new ObservableCollection<DocumentPanel>();
                    
                    //string tempIdJobDesc = string.Empty;
                    //foreach (var IdJobDesc in IDEmployeeJobDescriptionList)
                    //{
                    //    tempIdJobDesc
                    //}

                    //var tempiddesc = (from IdJD in IDEmployeeJobDescriptionList
                    //                  from wsjd in WorkStageWiseJobDescription
                    //                  where wsjd.IdJobDescription.Contains(Convert.ToString(IdJD))
                    //                  select new
                    //                  {
                    //                      wsjd.IdWorkStage
                    //                  }
                    //                  ).GroupBy(a=>a.IdWorkStage).ToList();

                    //WorkStageWiseJobDescription.Where(a => a.IdJobDescription.Contains(Convert.ToString(IDEmployeeJobDescriptionList.))).GroupBy(b => b.IdWorkStage).ToList();

                    foreach (var IdStageitem in HRPLANDictionary)
                    {



                        int IdStage = IdStageitem.Key;//Convert.ToInt32(IdStageitem.Key);

                        Int32 HRPlan = Convert.ToInt32(IdStageitem.Value);

                        List<int> IDEmployeesJobDescriptionList = new List<int>();

                        var tempIdWorkStage = (from IdJD in IDEmployeeJobDescriptionList
                                               from wsjd in WorkStageWiseJobDescription
                                               where wsjd.IdJobDescription.Contains(Convert.ToString(IdJD)) && wsjd.IdWorkStage== IdStage
                                               select new
                                               {
                                                   IdJD
                                               }
                                      ).ToList();
                        if(tempIdWorkStage.Count>0)
                        {
                            foreach(var item in tempIdWorkStage)
                            {
                                IDEmployeesJobDescriptionList.Add(item.IdJD);
                            }
                        }

                        // IDEmployeesJobDescriptionList.Add()

                        //IDEmployeesJobDescriptionList = IDEmployeeJobDescriptionList;// IdstageItem.IdJobDescription.Select(Int32.Parse).ToList();
                        var TempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && a.PlantOperationalPlanningRealInfo.Any(p => p.Idstage == Convert.ToInt32(IdStage) && p.CalenderWeek == CalenderWeek)).ToList();
                        foreach (var item in TempPlantOperationalPlanning)
                        {
                            FinalTempPlantOperationalPlanningRealInfo.AddRange(item.PlantOperationalPlanningRealInfo.Where(a => a.Idstage == Convert.ToInt32(IdStage) && a.IdEmployee == Convert.ToInt32(IdEmployee) && a.CalenderWeek == CalenderWeek));
                        }

                        PlantOperationalPlanningRealInfo TempPlantOperationalPlanningRealInfo = new PlantOperationalPlanningRealInfo
                        {

                            IdReason = 2,
                            IdEmployee = Convert.ToInt32(IdEmployee),
                            ReasonValue = "HR Plan",
                            CalenderWeek = CalenderWeek,
                        };
                        FinalTempPlantOperationalPlanningRealInfo.Add(TempPlantOperationalPlanningRealInfo);

                        TimeTypesList = NonOTTimeTypesList;
                        List<ERMNonOTItemType> TempTimeTypesList = new List<ERMNonOTItemType>();
                        foreach (var item in TimeTypesList)
                        {
                            var tempRealvalue = FinalTempPlantOperationalPlanningRealInfo.Where(a => a.ReasonValue == item.ReasonValue).ToList();
                            if (tempRealvalue.Count() > 0)
                            {
                                foreach (var itemreal in tempRealvalue)
                                {
                                    DateTime timeStart = Convert.ToDateTime(itemreal.StartTime);
                                    DateTime timeEnd = Convert.ToDateTime(itemreal.EndTime);
                                    if (timeStart <= timeEnd)
                                    {
                                        SelectedReal = new ERMNonOTItemType();
                                        SelectedReal.IdReason = Convert.ToInt32(itemreal.IdReason);
                                        SelectedReal.ReasonValue = itemreal.ReasonValue;

                                        TimeSpan TSReal = timeEnd - timeStart;
                                        SelectedReal.Real = Convert.ToInt32(TSReal.TotalMinutes);
                                        TempTimeTypesList.Add(SelectedReal);
                                    }
                                }
                            }
                            else
                            {
                                //TimeTypesList = new List<ERMNonOTItemType>();
                                SelectedReal = new ERMNonOTItemType();
                                SelectedReal.IdReason = Convert.ToInt32(item.IdReason);
                                SelectedReal.ReasonValue = item.ReasonValue;
                                // SelectedReal.Real = timeDifference;
                                SelectedReal.Real = 0;// Convert.ToInt32(TSReal.TotalMinutes);


                                TempTimeTypesList.Add(SelectedReal);
                            }
                        }
                        var sumByReasonValue = TempTimeTypesList.GroupBy(a => a.ReasonValue).Select(group => new
                        {
                            ReasonValue = group.Key,
                            SumReal = group.Sum(b => b.Real)
                        }).ToList();
                        TimeTypesList = new List<ERMNonOTItemType>();
                        foreach (var Temptest in sumByReasonValue)
                        {
                            SelectedReal = new ERMNonOTItemType();
                            SelectedReal.ReasonValue = Temptest.ReasonValue;
                            SelectedReal.Real = Temptest.SumReal;
                            if (Temptest.ReasonValue == "HR Plan")
                            {
                                //   TimeTypesList
                                // SelectedReal.IdReason = 2;
                                int sumreal = TimeTypesList.Sum(i => i.Real);
                                float tempHRPlan = (float)TimeSpan.FromHours(Convert.ToDouble(HRPlan)).TotalMinutes;
                                SelectedReal.Real = Convert.ToInt32(Math.Round(tempHRPlan, 0)) - sumreal;

                                TimeTypesList.Where(a => a.IdReason == 2).ToList();
                            }
                            TimeTypesList.Add(SelectedReal);
                        }


                      

                        #region [GEOS2-4707][rupali sarode][25-07-2023]

                        List<ERMEmployeeLeave> FinalTempPlantOperationalPlanningLeave = new List<ERMEmployeeLeave>();

                        decimal TotalJobDescriptionUsage = 0;
                        LeaveTypesList = new List<ERMEmployeeLeave>();
                        
                        //List<ERMPlantOperationalPlanning> tempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && IDEmployeesJobDescriptionList.Contains(a.IdJobDescription) && a.EmployeeLeave.Any(x => x.CalenderWeek == CalenderWeek) ).ToList();
                        List<ERMPlantOperationalPlanning> tempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && SelectedWeek.FirstDateofweek >= a.JobDescriptionStartDate && SelectedWeek.LastDateofWeek <= a.EndDate && IDEmployeesJobDescriptionList.Contains(a.IdJobDescription) && a.EmployeeLeave.Any(x => x.CalenderWeek == CalenderWeek)).ToList();
                        //List<ERMPlantOperationalPlanning> tempPlantOperationalPlanning = PlantOperationalPlanning.Where(a => a.IdEmployee == IdEmployee && IDEmployeesJobDescriptionList.Contains(a.IdJobDescription)).ToList();
                        if (tempPlantOperationalPlanning.Count > 0)
                        {
                            TotalJobDescriptionUsage = Convert.ToDecimal(tempPlantOperationalPlanning.Sum(a => a.JobDescriptionUsage));
                            var Templeave = tempPlantOperationalPlanning.FirstOrDefault().EmployeeLeave.Where(a => a.CalenderWeek == CalenderWeek).ToList();

                            decimal LeaveMinutes = 0;
                            //var Templeave=  item.EmployeeLeave.Where(a => a.CalenderWeek == CalenderWeek).ToList();
                            var LeaveDayOfWeek = 0;
                            if (Templeave != null)
                            {

                                foreach (var leaveitem in Templeave)
                                {
                                    ERMEmployeeLeave LeaveType = new ERMEmployeeLeave();
                                    double TotalLeaveCount = 0;

                                    if (leaveitem.IsAllDayEvent == 0)
                                    {
                                        TimeSpan StartDateTime = Convert.ToDateTime(leaveitem.StartDate).TimeOfDay;
                                        TimeSpan EndDateTime = Convert.ToDateTime(leaveitem.EndDate).TimeOfDay;

                                        if (StartDateTime <= EndDateTime)
                                        {
                                            LeaveDayOfWeek = (int)Convert.ToDateTime(leaveitem.StartDate).DayOfWeek;

                                            // LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime).TotalMinutes);
                                            switch (LeaveDayOfWeek)
                                            {

                                                case 1:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.MonBreakTime).TotalMinutes);
                                                    break;

                                                case 2:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.TueBreakTime).TotalMinutes);
                                                    break;

                                                case 3:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.WedBreakTime).TotalMinutes);
                                                    break;

                                                case 4:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.ThuBreakTime).TotalMinutes);
                                                    break;

                                                case 5:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.FriBreakTime).TotalMinutes);
                                                    break;

                                                case 6:

                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.SatBreakTime).TotalMinutes);
                                                    break;

                                                case 7:
                                                    LeaveMinutes = Convert.ToInt64((EndDateTime - StartDateTime - leaveitem.SunBreakTime).TotalMinutes);
                                                    break;

                                            }

                                            LeaveType.LeaveMinutes = Math.Round((LeaveMinutes * TotalJobDescriptionUsage) / 100);
                                        }
                                    }
                                    else if (leaveitem.IsAllDayEvent == 1)
                                    {
                                        //Substract holiday if exists in leave

                                        var Holiday = PlantOperationalPlanning.FirstOrDefault().CompanyHoliday.Where(x => x.StartDate >= leaveitem.StartDate && x.EndDate <= leaveitem.EndDate).FirstOrDefault();
                                        int TotalHolidayCount = 0;
                                        double TotalHolidayHours = 0;

                                        if (leaveitem.StartDate <= leaveitem.EndDate)
                                        {
                                            if (Holiday != null)
                                            {
                                                if (Holiday.IsAllDayEvent == 1)
                                                {
                                                    TotalHolidayCount = 1 + Convert.ToInt32((Convert.ToDateTime(Holiday.EndDate).Date - Convert.ToDateTime(Holiday.StartDate).Date).TotalDays);
                                                }
                                                else if (Holiday.IsAllDayEvent == 0)
                                                {

                                                    TimeSpan StartDateTime = Convert.ToDateTime(Holiday.StartDate).TimeOfDay;
                                                    TimeSpan EndDateTime = Convert.ToDateTime(Holiday.EndDate).TimeOfDay;

                                                    TotalHolidayHours = Convert.ToDouble((EndDateTime - StartDateTime).TotalHours);
                                                }
                                            }

                                            int DailyHoursCount = 0;
                                            //Find daily hours count
                                            DailyHoursCount = tempPlantOperationalPlanning.FirstOrDefault().EmployeeInformation.FirstOrDefault().DailyHoursCount;

                                            //calculate breaktime for multiple days

                                            DateTime? LeaveStartDate = leaveitem.StartDate;
                                            DateTime? LeaveEndDate = leaveitem.EndDate;

                                            #region [GEOS2-4813][Rupali Sarode][12-09-2023]
                                            // TimeSpan BreakTime = new TimeSpan(0, 0, 0);
                                            double BreakTimeMinutes = 0;
                                            double BreakTimeMinutesMain = 0;
                                            // TimeSpan BreakTimeMain = new TimeSpan(0, 0, 0);

                                            while (LeaveEndDate >= LeaveStartDate)
                                            {
                                                switch ((int)Convert.ToDateTime(LeaveStartDate).DayOfWeek)
                                                {

                                                    case 1:
                                                        BreakTimeMinutes = leaveitem.MonBreakTime.Minutes;

                                                        break;

                                                    case 2:

                                                        BreakTimeMinutes = leaveitem.TueBreakTime.Minutes;
                                                        break;

                                                    case 3:
                                                        BreakTimeMinutes = leaveitem.WedBreakTime.Minutes;
                                                        break;

                                                    case 4:
                                                        BreakTimeMinutes = leaveitem.ThuBreakTime.Minutes;
                                                        break;

                                                    case 5:
                                                        BreakTimeMinutes = leaveitem.FriBreakTime.Minutes;
                                                        break;

                                                    case 6:
                                                        BreakTimeMinutes = leaveitem.SatBreakTime.Minutes;
                                                        break;

                                                    case 7:
                                                        BreakTimeMinutes = leaveitem.SunBreakTime.Minutes;
                                                        break;

                                                }

                                                BreakTimeMinutesMain = BreakTimeMinutesMain + BreakTimeMinutes;

                                                #endregion [GEOS2-4813][Rupali Sarode][12-09-2023]

                                                LeaveStartDate = Convert.ToDateTime(LeaveStartDate).AddDays(1);
                                            }
                                            TimeSpan TimeDifference = new TimeSpan();
                                            if (leaveitem.IsAllDayEvent == 1)
                                                TimeDifference = (Convert.ToDateTime(leaveitem.EndDate.Value.AddDays(1)) - Convert.ToDateTime(leaveitem.StartDate));
                                            else
                                                TimeDifference = (Convert.ToDateTime(leaveitem.EndDate) - Convert.ToDateTime(leaveitem.StartDate));

                                            double TotalDays = 0;
                                            TotalDays = TimeDifference.Days - TotalHolidayCount; //Substract Holidays

                                            double TotalHours = (TotalDays * DailyHoursCount) - TotalHolidayHours;  //Substract Holiday hours

                                            //  decimal MinutesForDay = Convert.ToDecimal(TimeSpan.FromHours(TotalHours).TotalMinutes + TimeSpan.FromHours(TimeDifference.Hours).TotalMinutes +
                                            //  TimeSpan.FromMinutes(TimeDifference.Minutes).TotalMinutes);
                                            decimal MinutesForDay = 0;
                                            // [GEOS2-4813][Rupali Sarode][12-09-2023]
                                            if (leaveitem.IsAllDayEvent == 1)
                                                MinutesForDay = Convert.ToDecimal(TimeSpan.FromHours(TotalHours).TotalMinutes - BreakTimeMinutesMain);
                                            else
                                                MinutesForDay = Convert.ToDecimal((TimeSpan.FromHours(TotalHours).TotalMinutes + TimeSpan.FromHours(TimeDifference.Hours).TotalMinutes +
                                         TimeSpan.FromMinutes(TimeDifference.Minutes).TotalMinutes) - BreakTimeMinutesMain);



                                            LeaveType.LeaveMinutes = Math.Round((MinutesForDay * TotalJobDescriptionUsage) / 100);
                                        }


                                    }

                                    LeaveType.LeaveType = leaveitem.LeaveType;

                                    LeaveTypesList.Add(LeaveType);

                                }
                            }

                        }

                        if (LeaveTypesList.Count > 0)
                        {
                            List<ERMEmployeeLeave>  TempLeaveTypesList = new List<ERMEmployeeLeave>();
                            var tempLeaveTypeList = LeaveTypesList.GroupBy(x => x.LeaveType).ToList();
                            foreach(var item in tempLeaveTypeList)
                            {
                                ERMEmployeeLeave TempLeaveTypes = new ERMEmployeeLeave();
                                TempLeaveTypes.LeaveType = item.Key;
                                TempLeaveTypes.LeaveMinutes = LeaveTypesList.Where(a => a.LeaveType == item.Key).Sum(b => b.LeaveMinutes);
                                TempLeaveTypesList.Add(TempLeaveTypes);
                            }
                            LeaveTypesList = new List<ERMEmployeeLeave>();
                            LeaveTypesList = TempLeaveTypesList;
                            IsGridControlVisible = Visibility.Visible;
                        }
                        else
                        {
                            IsGridControlVisible = Visibility.Hidden;
                        }

                        #endregion [GEOS2-4707][rupali sarode][25-07-2023]

                       
                        string StageCode = PlantOperationStagesList.Where(x => x.IdStage == IdStage).Select(a => a.StageCode).FirstOrDefault();
                        DocumentPanel searchedResult = new DocumentPanel();
                        searchedResult.ShowCloseButton = false;
                        searchedResult.Caption = StageCode;//string.Format(System.Windows.Application.Current.FindResource("SCM_SearchedHeaderTab").ToString(), ConnectorsList.Count);
                                                           //searchedResult.Visibility = Visibility.Visible;
                        LayoutPanel layout = new LayoutPanel();

                        Grid grid = new Grid();

                        GridControl gridControl = new GridControl();
                        gridControl.VerticalAlignment = VerticalAlignment.Stretch;
                        gridControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                        gridControl.AllowLiveDataShaping = true;
                        gridControl.Width = 840;

                        TableView tableView = new TableView();
                        tableView.NavigationStyle = GridViewNavigationStyle.Row;
                        tableView.AllowHorizontalScrollingVirtualization = true;
                        tableView.RowDetailsVisibilityMode = RowDetailsVisibilityMode.Visible;

                        tableView.ShowHorizontalLines = true;
                        //tableView.ShowSearchPanelMode = ShowSearchPanelMode.Always;
                        tableView.ShowColumnHeaders = true;
                        tableView.VerticalScrollbarVisibility = ScrollBarVisibility.Auto;
                        tableView.HorizontalScrollbarVisibility = ScrollBarVisibility.Auto;
                        tableView.AllowFixedColumnMenu = false;
                        tableView.AutoWidth = true;
                        tableView.Width = 840;
                        tableView.ShowSearchPanelCloseButton = false;
                        tableView.SearchPanelHorizontalAlignment = HorizontalAlignment.Stretch;
                        tableView.ColumnChooserColumnDisplayMode = ColumnChooserColumnDisplayMode.ShowHiddenColumnsOnly;
                        tableView.ShowGroupPanel = false;
                        tableView.AllowConditionalFormattingMenu = false;
                        tableView.AllowSorting = false;
                        tableView.AllowColumnFiltering=false;
                       
                        DataTable dt = new DataTable();
                        if (LeaveTypesList.Count > 0)
                        {
                            DataTable dt1 = new DataTable();
                            dt1.Columns.Add("ReasonValue");
                            dt1.Columns.Add("Real");
                            dt1.Columns.Add("");
                            foreach (var items in TimeTypesList)
                            {
                                DataRow row = dt1.NewRow();
                                row["ReasonValue"] = items.ReasonValue;
                                row["Real"] = items.Real;
                                dt1.Rows.Add(row);
                            }
                            DataTable dt2 = new DataTable();
                            dt2.Columns.Add("LeaveType");
                            dt2.Columns.Add("LeaveMinutes");
                            dt.Merge(dt1);
                            dt.Merge(dt2);
                            foreach (var Leaveitems in LeaveTypesList)
                            {
                                DataRow row = dt2.NewRow();
                                row["LeaveType"] = Leaveitems.LeaveType;
                                row["LeaveMinutes"] = Leaveitems.LeaveMinutes;
                                dt2.Rows.Add(row);
                            }
                           int dt2count= dt2.Rows.Count;
                            int dtcount = dt.Rows.Count;

                            //Bands = new ObservableCollection<BandItem>(); Bands.Clear();
                            //BandItem band0 = new BandItem() { BandName = "TimeType", BandHeader = "Time Type", Visible = true };
                            //band0.Columns = new ObservableCollection<ColumnItem>();
                            //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "ReasonValue", HeaderText = "ReasonValue", Width = 120, IsVertical = false, Visible = true });
                            //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "ReasonValue", HeaderText = "Real", Width = 120, IsVertical = false, Visible = true });

                            //Bands.Add(band0);
                            //BandItem band1 = new BandItem() { BandName = "Space", BandHeader = "", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                            //band1.Columns = new ObservableCollection<ColumnItem>();
                            //band1.Columns.Add(new ColumnItem() { ColumnFieldName = "", HeaderText = "", Width = 120, IsVertical = false, Visible = true });
                            //Bands.Add(band1);
                            //BandItem band2 = new BandItem() { BandName = "Leaves", BandHeader = "Leaves", Visible = true };
                            //band2.Columns = new ObservableCollection<ColumnItem>();
                            //band2.Columns.Add(new ColumnItem() { ColumnFieldName = "LeaveType", HeaderText = "LeaveType", Width = 90, IsVertical = false, Visible = true });
                            //band2.Columns.Add(new ColumnItem() { ColumnFieldName = "LeaveMinutes", HeaderText = "LeaveMinutes", Width = 90, IsVertical = false, Visible = true });

                            //Bands.Add(band2);


                            for (int i = 0; i < dt2count; i++)
                            {
                                if(i< dtcount)
                                {
                                    dt.Rows[i]["LeaveType"] = dt2.Rows[i]["LeaveType"];
                                    dt.Rows[i]["LeaveMinutes"] = dt2.Rows[i]["LeaveMinutes"];
                                }
                                else
                                {
                                    DataRow row = dt.NewRow();
                                    row["LeaveType"] = dt2.Rows[i]["LeaveType"];
                                    row["LeaveMinutes"] = dt2.Rows[i]["LeaveMinutes"];
                                    dt.Rows.Add(row);
                                }
                               
                            }


                           

                           
                            
                            string ReasonValue = string.Format(System.Windows.Application.Current.FindResource("TimeType").ToString());
                            string Real = string.Format(System.Windows.Application.Current.FindResource("Real").ToString());
                            string LeaveType = string.Format(System.Windows.Application.Current.FindResource("Leaves").ToString());
                            string LeaveMinutes = string.Format(System.Windows.Application.Current.FindResource("Minutes").ToString());
                            gridControl.Columns.Add(new GridColumn() { FieldName = "ReasonValue", ReadOnly = true, Header = ReasonValue, Width = 203, FixedWidth = true, MaxWidth = 203, MinWidth = 203 });
                            gridControl.Columns.Add(new GridColumn() { FieldName = "Real", ReadOnly = true, Header = Real, Width = 202, FixedWidth = true, MaxWidth = 202, MinWidth = 202 });
                            gridControl.Columns.Add(new GridColumn() { FieldName = "", ReadOnly = true, Header = "", Width = 2, FixedWidth=true, MaxWidth=2, MinWidth=2 });
                            gridControl.Columns.Add(new GridColumn() { FieldName = "LeaveType", ReadOnly = true, Header = LeaveType, Width = 202, FixedWidth = true, MaxWidth = 202, MinWidth = 202 });
                            gridControl.Columns.Add(new GridColumn() { FieldName = "LeaveMinutes", ReadOnly = true, Header = LeaveMinutes, Width = 210, FixedWidth = true, MaxWidth = 210, MinWidth = 210 });
                        }
                        else
                        {
                            dt.Columns.Add("ReasonValue");
                            dt.Columns.Add("Real");
                            string ReasonValue = string.Format(System.Windows.Application.Current.FindResource("TimeType").ToString());
                            string Real = string.Format(System.Windows.Application.Current.FindResource("Real").ToString());
                            gridControl.Columns.Add(new GridColumn() { FieldName = "ReasonValue", ReadOnly = true, Header = ReasonValue, Width = 410, FixedWidth = true, MaxWidth = 410, MinWidth = 410 });
                            gridControl.Columns.Add(new GridColumn() { FieldName = "Real", ReadOnly = true, Header = Real, Width = 412, FixedWidth = true, MaxWidth = 412, MinWidth = 412 });
                           

                            foreach (var items in TimeTypesList)
                            {
                                DataRow row = dt.NewRow();
                                row["ReasonValue"] = items.ReasonValue;
                                row["Real"] = items.Real;
                                dt.Rows.Add(row);
                            }
                        }
                        
                        gridControl.View = tableView;
                        gridControl.ItemsSource = dt.DefaultView;
                        
                        grid.Children.Add(gridControl);
                        grid.UpdateLayout();
                        layout.Content = grid;
                        searchedResult.Content = layout;
                        ListDocumentPanel.Add(searchedResult);
                        
                    }
                    //}
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }



            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
       

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                //IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
