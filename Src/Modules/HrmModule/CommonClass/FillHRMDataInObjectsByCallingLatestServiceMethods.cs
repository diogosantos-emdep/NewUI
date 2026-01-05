using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.XtraScheduler;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.ViewModels;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using Emdep.Geos.UI.ServiceProcess;

namespace Emdep.Geos.Modules.Hrm
{
    
    public static class FillHrmDataInObjectsByCallingLatestServiceMethods
    {
        public static CancellationTokenSource tokenToLoadFullYearAttendanceInAsync;
        public static System.Threading.Tasks.Task<List<EmployeeAttendance>> taskToLoadFullYearAttendanceInAsync;
        //IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // static IHrmService HrmService = new HrmServiceController("localhost:6699");

        #region #region GEOS2-3887 shubham[skadam]
        public static async void LoadFullYearEmployeeAttendanceAsync_V3887(IHrmService HrmService, CustomObservableCollection<EmployeeAttendance> employeeAttendanceList, GridControl GridControl1 )
        {
            const string methodNameWithBrackets = nameof(LoadFullYearEmployeeAttendanceAsync) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
                DateTime? fromDate;
                DateTime? toDate;
                List<EmployeeAttendance> newEmployeeAttendance = new List<EmployeeAttendance>();
                CalculateFromDateToDateAtSelectedPeriodForFullYear(out fromDate, out toDate, null);
                List<DateTime> GetAllCurrentYearMonths = GetMonthsBetween(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
                if (GetAllCurrentYearMonths.Count == 11|| GetAllCurrentYearMonths.Count == 10)
                {
                    GetAllCurrentYearMonths.Add(Convert.ToDateTime(toDate));
                }
                GetAllCurrentYearMonths= GetAllCurrentYearMonths.OrderByDescending(d=>d).ToList();
                List<DateTime> tempAllCurrentYearMonths = new List<DateTime>();
                foreach (DateTime DateTimeitem in GetAllCurrentYearMonths)
                {
                    //if (DateTimeitem.Month <= DateTime.Now.Month +1)
                    if (HrmCommon.Instance.SelectedPeriod != DateTime.Now.Year)
                    {
                        if (DateTimeitem.Month != DateTime.Now.Month)
                            tempAllCurrentYearMonths.Add(DateTimeitem);
                    }
                    else
                    {
                        if (DateTimeitem.Month <= DateTime.Now.Month)
                        {
                            if (DateTimeitem.Month != DateTime.Now.Month)
                                tempAllCurrentYearMonths.Add(DateTimeitem);
                        }
                    }
                }
                GetAllCurrentYearMonths.Clear();
                GetAllCurrentYearMonths.AddRange(tempAllCurrentYearMonths);
                foreach (DateTime DateTimeItem in GetAllCurrentYearMonths)
                {
                    HrmCommon.Instance.AttendanceLoadingMessage = "Loading full year ( " + DateTimeItem.ToString("MMMM") + "-" + HrmCommon.Instance.SelectedPeriod + " ) attendance in background...";
                    var startDate = new DateTime(DateTimeItem.Year, DateTimeItem.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    if (tokenToLoadFullYearAttendanceInAsync != null)
                    {
                        try
                        {
                            tokenToLoadFullYearAttendanceInAsync.Cancel();
                            taskToLoadFullYearAttendanceInAsync.Wait(tokenToLoadFullYearAttendanceInAsync.Token);
                            GeosApplication.Instance.Logger.Log($"In method {methodNameWithBrackets}, token to load full year attendance is canceled", category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log($"Get an error in {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    tokenToLoadFullYearAttendanceInAsync = new CancellationTokenSource();
                    HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                    newEmployeeAttendance = await LoadEmployeesAttendanceAsyncTask_V3887(HrmService, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));

                    if (tokenToLoadFullYearAttendanceInAsync != null && tokenToLoadFullYearAttendanceInAsync.IsCancellationRequested)
                    {
                        return;
                    }
                   
                    if (employeeAttendanceList == null)
                        employeeAttendanceList = new CustomObservableCollection<Data.Common.Hrm.EmployeeAttendance>();
                    //else
                    //    employeeAttendanceList.ClearWithTemporarySuppressedNotification();

                    employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(newEmployeeAttendance);
                    try
                    {
                        //Shubham[skadam] GEOS2-6526 Night Shift Application Issue (3/3) 16 11 2024
                        if (employeeAttendanceList != null && employeeAttendanceList.Count > 0)
                        {
                            var distinctList = employeeAttendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(g => g.First()).ToList();
                            //employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                            //employeeAttendanceList.Clear();
                            employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                            //employeeAttendanceList.AddRange(distinctList);
                            employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(distinctList);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }
                    HrmCommon.Instance.AttendanceLoadingMessage = "";
                    if (GridControl1 != null)
                    {
                        GridControl1.RefreshData();
                    }
                }
               
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                HrmCommon.Instance.AttendanceLoadingMessage = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                HrmCommon.Instance.AttendanceLoadingMessage = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                HrmCommon.Instance.AttendanceLoadingMessage = "";
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static async Task<List<EmployeeAttendance>> LoadEmployeesAttendanceAsyncTask_V3887(IHrmService HrmService, DateTime fromDate, DateTime toDate)
        {
            const string methodNameWithBrackets = nameof(LoadEmployeesAttendanceAsyncTask) + "()";
            var EmployeeAttendanceList = new List<EmployeeAttendance>();
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                taskToLoadFullYearAttendanceInAsync =
                    new Task<List<EmployeeAttendance>>(() =>
                    {
                        return ReturnFullYearEmployeeAttendanceList_V3887(HrmService, ref EmployeeAttendanceList,fromDate,toDate);

                    }, tokenToLoadFullYearAttendanceInAsync.Token);
                taskToLoadFullYearAttendanceInAsync.Start();
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                return await taskToLoadFullYearAttendanceInAsync;
            }
            catch (FaultException<ServiceException> ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }
		//[nsatpute][19-12-2024][GEOS2-6636]
        private static List<EmployeeAttendance> ReturnFullYearEmployeeAttendanceList_V3887(IHrmService HrmService, ref List<EmployeeAttendance> EmployeeAttendanceList, DateTime  fromDate, DateTime toDate)
        {
            var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
            EmployeeAttendanceList = new List<EmployeeAttendance>();
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2300 to GetSelectedIdCompanyEmployeeAttendance_V2420 by [rdixit][GEOS2-2466][10.08.2023]
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2420 to GetSelectedIdCompanyEmployeeAttendance_V2220 by [rajashri][GEOS2-5508][22.05.2024]
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2420 to GetSelectedIdCompanyEmployeeAttendance_V2520 by [cpatil][GEOS2-5640][24.05.2024]
            //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
            //HrmService = new HrmServiceController("localhost:6699");
            #region Rupali Sarode - GEOS2-3751
            EmployeeAttendanceList.AddRange(
            HrmService.GetSelectedIdCompanyEmployeeAttendance_V2590(plantOwnersIds,
                HrmCommon.Instance.SelectedPeriod,
                HrmCommon.Instance.ActiveEmployee.Organization,
                HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                HrmCommon.Instance.IdUserPermission,
                (DateTime)fromDate,
                (DateTime)toDate));
            #endregion
            //Shubham[skadam] GEOS2-6526 Night Shift Application Issue (3/3) 16 11 2024
            return EmployeeAttendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(s => s.First()).ToList();
        }
        #endregion
        #region // shubham[skadam]GEOS2-3887 Attendance loading message not working properly 23 11 2022
        public static List<DateTime> GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                monthDiff -= 1;
            }

            List<DateTime> results = new List<DateTime>();
            for (int i = monthDiff; i >= 1; i--)
            {
                if(to.AddMonths(-i).Month!=DateTime.Now.Month)
                results.Add(to.AddMonths(-i));
            }

            return results;
        }
        #endregion
        public static async void LoadFullYearEmployeeAttendanceAsync(
            IHrmService HrmService,
            CustomObservableCollection<EmployeeAttendance> employeeAttendanceList,
            GridControl GridControl1
            )
        {
            const string methodNameWithBrackets = nameof(LoadFullYearEmployeeAttendanceAsync) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (tokenToLoadFullYearAttendanceInAsync != null
                    //&& taskToLoadFullYearAttendanceInAsync.Status==TaskStatus.
                    )
                {
                    try
                    {
                        tokenToLoadFullYearAttendanceInAsync.Cancel();
                        taskToLoadFullYearAttendanceInAsync.Wait(tokenToLoadFullYearAttendanceInAsync.Token);
                        GeosApplication.Instance.Logger.Log($"In method {methodNameWithBrackets}, token to load full year attendance is canceled", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Get an error in {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }
                }
                tokenToLoadFullYearAttendanceInAsync = new CancellationTokenSource();
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                var newEmployeeAttendance = await LoadEmployeesAttendanceAsyncTask(
                HrmService);

                if (tokenToLoadFullYearAttendanceInAsync != null &&
                    tokenToLoadFullYearAttendanceInAsync.IsCancellationRequested)
                {
                    return;
                }

                if (employeeAttendanceList == null)
                    employeeAttendanceList = new CustomObservableCollection<Data.Common.Hrm.EmployeeAttendance>();
                else
                    employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                
                employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(newEmployeeAttendance);

                if (GridControl1 != null)
                {
                    GridControl1.RefreshData();
                }
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static async Task<List<EmployeeAttendance>>
            LoadEmployeesAttendanceAsyncTask(
                IHrmService HrmService)
        {
            const string methodNameWithBrackets = nameof(LoadEmployeesAttendanceAsyncTask) + "()";
            var EmployeeAttendanceList = new List<EmployeeAttendance>();
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                taskToLoadFullYearAttendanceInAsync =
                    new Task<List<EmployeeAttendance>>(() =>
                    {
                        return ReturnFullYearEmployeeAttendanceList(HrmService, ref EmployeeAttendanceList);

                    }, tokenToLoadFullYearAttendanceInAsync.Token);
                taskToLoadFullYearAttendanceInAsync.Start();
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                return await taskToLoadFullYearAttendanceInAsync;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }
		//[nsatpute][19-12-2024][GEOS2-6636]
        private static List<EmployeeAttendance> ReturnFullYearEmployeeAttendanceList(IHrmService HrmService, 
            ref List<EmployeeAttendance> EmployeeAttendanceList)
        {
            var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
            DateTime? fromDate;
            DateTime? toDate;
            CalculateFromDateToDateAtSelectedPeriodForFullYear(out fromDate, out toDate, null);
            EmployeeAttendanceList = new List<EmployeeAttendance>();
            //EmployeeAttendanceList.AddRange(
            //HrmService.GetSelectedIdCompanyEmployeeAttendance_V2110(plantOwnersIds,
            //    HrmCommon.Instance.SelectedPeriod,
            //    HrmCommon.Instance.ActiveEmployee.Organization,
            //    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
            //    HrmCommon.Instance.IdUserPermission,
            //    (DateTime)fromDate,
            //    (DateTime)toDate));
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2300 to GetSelectedIdCompanyEmployeeAttendance_V2420 by [rdixit][GEOS2-2466][10.08.2023]
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2420 to GetSelectedIdCompanyEmployeeAttendance_V2220 by [rajashri][GEOS2-5508][22.05.2024]
            //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2420 to GetSelectedIdCompanyEmployeeAttendance_V2520 by [cpatil][GEOS2-5640][24.05.2024]
            //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
            //HrmService = new HrmServiceController("localhost:6699");
            #region Rupali Sarode - GEOS2-3751
            EmployeeAttendanceList.AddRange(
            HrmService.GetSelectedIdCompanyEmployeeAttendance_V2590(plantOwnersIds,
                HrmCommon.Instance.SelectedPeriod,
                HrmCommon.Instance.ActiveEmployee.Organization,
                HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                HrmCommon.Instance.IdUserPermission,
                (DateTime)fromDate,
                (DateTime)toDate));
            #endregion

            // EmployeeAttendanceViewModel.SetIsManual(EmployeeAttendanceList);
            //Shubham[skadam] GEOS2-6526 Night Shift Application Issue (3/3) 16 11 2024
            return EmployeeAttendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(s => s.First()).ToList();
        }

        public static void GetHRMDataOnceFromServiceForAttendance(
            ref IHrmService HrmService,
            ref ICrmService CrmStartUp,
            ref CustomObservableCollection<Employee> employeesList,
            ref CustomObservableCollection<EmployeeAttendance> employeeAttendanceList,
            ref CustomObservableCollection<CompanyHoliday> companyHolidays,
            ref CustomObservableCollection<LookupValue> holidayList,
            ref CustomObservableCollection<EmployeeLeave> employeeLeavesList,
            ref CustomObservableCollection<LabelHelper> labelItems,
            ref CustomObservableCollection<StatusHelper> statusItems,
            ref CustomObservableCollection<UI.Helper.Appointment> appointmentItems,
            ref DateTime? fromDate,
            ref DateTime? toDate,
            GridControl GridControl1,
            ref CustomObservableCollection<CompanyShift> CompanyShiftsList,
            ref CustomObservableCollection<Department>department
            )
        {
            
            

            const string methodNameWithBrackets = nameof(GetHRMDataOnceFromServiceForAttendance) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);


                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;

                    employeesList = new CustomObservableCollection<Employee>();
                    employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                    employeeLeavesList = new CustomObservableCollection<EmployeeLeave>();
                    companyHolidays = new CustomObservableCollection<CompanyHoliday>();
                    holidayList = new CustomObservableCollection<LookupValue>();
                    labelItems = new CustomObservableCollection<LabelHelper>();
                    statusItems = new CustomObservableCollection<StatusHelper>();
                    CompanyShiftsList = new CustomObservableCollection<CompanyShift>();
                    appointmentItems = new CustomObservableCollection<UI.Helper.Appointment>();

                    var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    // Added as future provision to speed up employee selection. Not used right now.
                    // CompanyShiftsList.AddRangeWithTemporarySuppressedNotification(HrmService.GetAllCompanyShiftsByIdCompany_V2035(plantOwnersIds));
                    #region Sudhir Jangra GEOS2-4054

                    //[rdixit][GEOS2-4055][05.01.2023] Service Updated
                    //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
                    //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
                    //Service Method Changed from GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2410 to GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    #endregion
                    department = new CustomObservableCollection<Department>();
                    //department.AddRangeWithTemporarySuppressedNotification(
                    //    HrmService.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420(
                    //    plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                    //    HrmCommon.Instance.ActiveEmployee.Organization,
                    //    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                    //    HrmCommon.Instance.IdUserPermission));

                    //[Rahul.Gadhave][GEOS2-6085] [Date:03-10-2024]  //[rdixit][GEOS2-7799][10.04.2025]
                    department.AddRangeWithTemporarySuppressedNotification(
                       HrmService.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2630(
                       plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                       HrmCommon.Instance.ActiveEmployee.Organization,
                       HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                       HrmCommon.Instance.IdUserPermission));

                    CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out fromDate, out toDate, null, null);
                    employeesList = new CustomObservableCollection<Employee>();
                    #region Service Comments
                    //  HrmService = new HrmServiceController("localhost:6699");
                    //Service Method Changed from GetAllEmployeesForAttendanceByIdCompany_V2410 to GetAllEmployeesForAttendanceByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service Method Changed from GetAllEmployeesForAttendanceByIdCompany_V2420 to GetAllEmployeesForAttendanceByIdCompany_V2480 by [rdixit][GEOS2-5157][17.01.2024]
                    //Service Method Changed from GetAllEmployeesForAttendanceByIdCompany_V2480 to GetAllEmployeesForAttendanceByIdCompany_V2520 by [cpatil][GEOS2-5640][24.05.2024]
                    #endregion
                    employeesList.AddRangeWithTemporarySuppressedNotification(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2520(
                        plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.IdUserPermission));
                    
                    employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();


                    #region Rupali Sarode - GEOS2-3751
                    //employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(
                    //    HrmService.GetSelectedIdCompanyEmployeeAttendance_V2110(
                    //    plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                    //    HrmCommon.Instance.ActiveEmployee.Organization,
                    //    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                    //    HrmCommon.Instance.IdUserPermission,
                    //    (System.DateTime)fromDate,
                    //    (System.DateTime)toDate));

                    //   HrmService = new HrmServiceController("localhost:6699");
                    //employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(
                    //  HrmService.GetSelectedIdCompanyEmployeeAttendance_V2300(
                    //  plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                    //  HrmCommon.Instance.ActiveEmployee.Organization,
                    //  HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                    //  HrmCommon.Instance.IdUserPermission,
                    //  (System.DateTime)fromDate,
                    //  (System.DateTime)toDate));



                    #endregion
					//[nsatpute][19-12-2024][GEOS2-6636]
                    //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                    //GetSelectedIdCompanyEmployeeAttendance_V2420
                    //Service Method Changed from GetSelectedIdCompanyEmployeeAttendance_V2420 to GetSelectedIdCompanyEmployeeAttendance_V2520 by [cpatil][GEOS2-5640][24.05.2024]
                    //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
                    //HrmService = new HrmServiceController("localhost:6699");
                    employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(
                     HrmService.GetSelectedIdCompanyEmployeeAttendance_V2590(
                     plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                     HrmCommon.Instance.ActiveEmployee.Organization,
                     HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                     HrmCommon.Instance.IdUserPermission,
                     (System.DateTime)fromDate,
                     (System.DateTime)toDate));

                    try
                    {
						//Shubham[skadam] GEOS2-6526 Night Shift Application Issue (3/3) 16 11 2024
                        if (employeeAttendanceList!=null && employeeAttendanceList.Count > 0)
                        {
                            var distinctList = employeeAttendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(g => g.First()).ToList();
                            //employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                            //employeeAttendanceList.Clear();
                            employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                            //employeeAttendanceList.AddRange(distinctList);
                            employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(distinctList);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }

                    // EmployeeAttendanceViewModel.SetIsManual(employeeAttendanceList);
                    //companyHolidays.AddRangeWithTemporarySuppressedNotification(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                    //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    //Shubham[skadam] GEOS2-5958 The company holidays not display in Leaves/Attendance schedule. 17 07 2024
                    companyHolidays.AddRangeWithTemporarySuppressedNotification(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    holidayList.AddRangeWithTemporarySuppressedNotification(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    //GetEmployeeLeavesBySelectedIdCompany_V2110 Service Method Updated with GetEmployeeLeavesBySelectedIdCompany_V2330 [rdixit][21.10.2022][GEOS2-3263]
                    //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
                    //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
                    //Service Method Changed from GetEmployeeLeavesBySelectedIdCompany_V2410 to GetEmployeeLeavesBySelectedIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service Method Changed from GetEmployeeLeavesBySelectedIdCompany_V2420 to GetEmployeeLeavesBySelectedIdCompany_V2520 by [cpatil][GEOS2-5640][24.05.2024]
                    employeeLeavesList.AddRangeWithTemporarySuppressedNotification(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2520(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                   
                    EmployeeAttendanceViewModel.FillEmployeeWorkType();
                    EmployeeAttendanceViewModel.FillEmployeeLeaveType();

                    foreach (CompanyHoliday CompanyHoliday in companyHolidays)
                    {
                        var modelAppointment = new UI.Helper.Appointment
                        {
                            Subject = CompanyHoliday.Name,
                            StartDate = CompanyHoliday.StartDate,
                            EndDate = CompanyHoliday.EndDate,
                            Label = CompanyHoliday.IdHoliday
                        };
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                            using (var recurrenceInfo = new RecurrenceInfo
                            {
                                Type = RecurrenceType.Yearly,
                                Periodicity = 1,
                                Month = CompanyHoliday.StartDate.Value.Month,
                                WeekOfMonth = WeekOfMonth.None,
                                DayNumber = CompanyHoliday.StartDate.Value.Day,
                                Range = RecurrenceRange.NoEndDate
                            })
                            {
                                modelAppointment.RecurrenceInfo = recurrenceInfo.ToXml();
                            }
                        }

                        appointmentItems.AddRangeWithTemporarySuppressedNotification(modelAppointment);
                    }

                    foreach (var AttendenceType in GeosApplication.Instance.AttendanceTypeList)
                    {
                        if (AttendenceType.IdLookupValue > 0 && AttendenceType.HtmlColor != null && AttendenceType.HtmlColor != string.Empty)
                        {
                            var labelForCompanyWork = new LabelHelper
                            {
                                Id = AttendenceType.IdLookupValue,
                                Color = new BrushConverter().ConvertFromString(AttendenceType.HtmlColor.ToString()) as SolidColorBrush,
                                Caption = AttendenceType.Value
                            };
                            labelItems.AddRangeWithTemporarySuppressedNotification(labelForCompanyWork);
                        }
                    }

                    foreach (var Holiday in holidayList)
                    {
                        if (Holiday.HtmlColor != null && Holiday.HtmlColor != string.Empty)
                        {
                            var labelForCompanyHoliday = new LabelHelper
                            {
                                Id = Holiday.IdLookupValue,
                                Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush,
                                Caption = Holiday.Value
                            };
                            labelItems.AddRangeWithTemporarySuppressedNotification(labelForCompanyHoliday);
                        }
                    }

                    statusItems.AddRangeWithTemporarySuppressedNotification(new StatusHelper
                    {
                        Id = 0,
                        Brush = new SolidColorBrush(Colors.Transparent),
                        Caption = "Night Shift"
                    });
                    statusItems.AddRangeWithTemporarySuppressedNotification(new StatusHelper
                    {
                        Id = 1,
                        Brush = new SolidColorBrush(Colors.SlateBlue),
                        Caption = "Night Shift"
                    });
                    statusItems.AddRangeWithTemporarySuppressedNotification(new StatusHelper
                    {
                        Id = 3,
                        Brush = new SolidColorBrush(Colors.SlateBlue),
                        Caption = "Mobile"
                    });

                    // shubham[skadam]GEOS2-3887 Attendance loading message not working properly 23 11 2022
                    LoadFullYearEmployeeAttendanceAsync_V3887(
                        HrmService, employeeAttendanceList,
                        GridControl1);
                }
                else
                {
                    if (employeesList != null) employeesList.ClearWithTemporarySuppressedNotification();
                    if (employeeAttendanceList != null) employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                    if (companyHolidays != null) companyHolidays.ClearWithTemporarySuppressedNotification();
                    if (holidayList != null) holidayList.ClearWithTemporarySuppressedNotification();
                    if (employeeLeavesList != null) employeeLeavesList.ClearWithTemporarySuppressedNotification();
                    if (labelItems != null) labelItems.ClearWithTemporarySuppressedNotification();
                    if (statusItems != null) statusItems.ClearWithTemporarySuppressedNotification();
                    if (appointmentItems != null) appointmentItems.ClearWithTemporarySuppressedNotification();
                }

                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        public static void GetHRMDataOnceFromServiceForEmployeeLeavesScreen(
            ref IHrmService HrmService,
            ref ICrmService CrmStartUp,
            ref CustomObservableCollection<Employee> employeesList,
            // ref CustomObservableCollection<EmployeeAttendance> employeeAttendanceList,
            ref CustomObservableCollection<CompanyHoliday> companyHolidays,
            ref CustomObservableCollection<LookupValue> holidayList,
            ref CustomObservableCollection<EmployeeLeave> employeeLeavesList,
            ref CustomObservableCollection<LabelHelper> labelItems,
            ref CustomObservableCollection<UI.Helper.Appointment> appointmentItems,
            ref DateTime? fromDate,
            ref DateTime? toDate,
            GridControl GridControl1,
            ref CustomObservableCollection<Department> department
            )
        {
            const string methodNameWithBrackets = nameof(GetHRMDataOnceFromServiceForEmployeeLeavesScreen) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;

                    employeesList = new CustomObservableCollection<Employee>();
                   // employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                    employeeLeavesList = new CustomObservableCollection<EmployeeLeave>();
                    companyHolidays = new CustomObservableCollection<CompanyHoliday>();
                    holidayList = new CustomObservableCollection<LookupValue>();
                    labelItems = new CustomObservableCollection<LabelHelper>();
                    appointmentItems = new CustomObservableCollection<UI.Helper.Appointment>();

                    var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    // Added as future provision to speed up employee selection. Not used right now.
                    // CompanyShiftsList.AddRangeWithTemporarySuppressedNotification(HrmService.GetAllCompanyShiftsByIdCompany_V2035(plantOwnersIds));
                    department = new CustomObservableCollection<Department>();
                    //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
                    //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
                    //Service Method Changed from GetAllEmployeesByDepartmentByIdCompany_V2410 to GetAllEmployeesByDepartmentByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    department.AddRangeWithTemporarySuppressedNotification(
                        HrmService.GetAllEmployeesByDepartmentByIdCompany_V2420(
                        plantOwnersIds, HrmCommon.Instance.SelectedPeriod, 
                        HrmCommon.Instance.ActiveEmployee.Organization, 
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, 
                        HrmCommon.Instance.IdUserPermission));

                    CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out fromDate, out toDate, null, null);
                    employeesList = new CustomObservableCollection<Employee>();
                    //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
                    //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
                    //Service Method Changed from GetAllEmployeesForLeaveByIdCompany_V2410 to GetAllEmployeesForLeaveByIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    employeesList.AddRangeWithTemporarySuppressedNotification(
                        HrmService.GetAllEmployeesForLeaveByIdCompany_V2420(
                        plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.IdUserPermission));
                    //employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();

                    //employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(
                    //    HrmService.GetSelectedIdCompanyEmployeeAttendance_V2110(
                    //    plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                    //    HrmCommon.Instance.ActiveEmployee.Organization,
                    //    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                    //    HrmCommon.Instance.IdUserPermission,
                    //    (System.DateTime)fromDate,
                    //    (System.DateTime)toDate));

                    // EmployeeAttendanceViewModel.SetIsManual(employeeAttendanceList);
                    //companyHolidays.AddRangeWithTemporarySuppressedNotification(HrmService.GetCompanyHolidaysBySelectedIdCompany(plantOwnersIds));
                    //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
                    //Shubham[skadam] GEOS2-5958 The company holidays not display in Leaves/Attendance schedule. 17 07 2024
                    companyHolidays.AddRangeWithTemporarySuppressedNotification(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, HrmCommon.Instance.SelectedPeriod));
                    holidayList.AddRangeWithTemporarySuppressedNotification(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    //GetEmployeeLeavesBySelectedIdCompany_V2110 Service Method Updated with GetEmployeeLeavesBySelectedIdCompany_V2330 [rdixit][21.10.2022][GEOS2-3263]
                    //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
                    //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
                    //Service Method Changed from GetEmployeeLeavesBySelectedIdCompany_V2410 to GetEmployeeLeavesBySelectedIdCompany_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service Method Changed from GetEmployeeLeavesBySelectedIdCompany_V2420 to GetEmployeeLeavesBySelectedIdCompany_V2520 by [cpatil][GEOS2-5640][24.05.2024]
                    employeeLeavesList.AddRangeWithTemporarySuppressedNotification(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2520(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));

                    EmployeeAttendanceViewModel.FillEmployeeWorkType();
                    EmployeeAttendanceViewModel.FillEmployeeLeaveType();

                    foreach (CompanyHoliday CompanyHoliday in companyHolidays)
                    {
                        var modelAppointment = new UI.Helper.Appointment
                        {
                            Subject = CompanyHoliday.Name,
                            StartDate = CompanyHoliday.StartDate,
                            EndDate = CompanyHoliday.EndDate,
                            Label = CompanyHoliday.IdHoliday
                        };
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Type = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
                            using (var recurrenceInfo = new RecurrenceInfo
                            {
                                Type = RecurrenceType.Yearly,
                                Periodicity = 1,
                                Month = CompanyHoliday.StartDate.Value.Month,
                                WeekOfMonth = WeekOfMonth.None,
                                DayNumber = CompanyHoliday.StartDate.Value.Day,
                                Range = RecurrenceRange.NoEndDate
                            })
                            {
                                modelAppointment.RecurrenceInfo = recurrenceInfo.ToXml();
                            }
                        }

                        appointmentItems.AddRangeWithTemporarySuppressedNotification(modelAppointment);
                    }
                    foreach (var EmployeeLeave in employeeLeavesList)
                    {
                        var modelAppointment = new UI.Helper.Appointment
                        {
                            //Added Employee Main JDAbbreviation in Subject [rdixit][21.10.2022][GEOS2-3263]
                            Subject = string.Format("{0} {1}_{2}_{3}", EmployeeLeave.Employee.FirstName, EmployeeLeave.Employee.LastName, EmployeeLeave.Employee.EmployeeMainJDAbbreviation, EmployeeLeave.CompanyLeave.Name),
                            StartDate = Convert.ToDateTime(EmployeeLeave.StartDate.ToString())
                        };
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            var NewTime = new TimeSpan(0, 0, 0);
                            if (EmployeeLeave.EndDate.Value.TimeOfDay == NewTime)
                                modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                            else
                                modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }
                        else
                        {

                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }
                        if (Convert.ToBoolean(EmployeeLeave.IsAllDayEvent))
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate.Value.AddDays(1);
                        }
                        else
                        {
                            modelAppointment.EndDate = EmployeeLeave.EndDate;
                        }

                        modelAppointment.Label = EmployeeLeave.IdLeave;
                        modelAppointment.EmployeeCode = EmployeeLeave.Employee.EmployeeCode;
                        modelAppointment.IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                        modelAppointment.IdEmployee = EmployeeLeave.IdEmployee;
                        modelAppointment.IsAllDayEvent = EmployeeLeave.IsAllDayEvent;
                        appointmentItems.AddRangeWithTemporarySuppressedNotification(modelAppointment);
                    }

                    //foreach (var AttendenceType in GeosApplication.Instance.AttendanceTypeList)
                    //{
                    //    if (AttendenceType.IdLookupValue > 0 && AttendenceType.HtmlColor != null && AttendenceType.HtmlColor != string.Empty)
                    //    {
                    //        var labelForCompanyWork = new LabelHelper
                    //        {
                    //            Id = AttendenceType.IdLookupValue,
                    //            Color = new BrushConverter().ConvertFromString(AttendenceType.HtmlColor.ToString()) as SolidColorBrush,
                    //            Caption = AttendenceType.Value
                    //        };
                    //        labelItems.AddRangeWithTemporarySuppressedNotification(labelForCompanyWork);
                    //    }
                    //}

                    foreach (var EmployeeLeaveType in GeosApplication.Instance.EmployeeLeaveList)
                    {
                        if (EmployeeLeaveType.IdLookupValue > 0 && EmployeeLeaveType.HtmlColor != null && EmployeeLeaveType.HtmlColor != string.Empty)
                        {
                            var label = new LabelHelper
                            {
                                Id = Convert.ToInt32(EmployeeLeaveType.IdLookupValue),
                                Color = new BrushConverter().ConvertFromString(EmployeeLeaveType.HtmlColor.ToString()) as SolidColorBrush,
                                Caption = EmployeeLeaveType.Value
                            };
                            labelItems.Add(label);
                        }
                    }

                    foreach (var Holiday in holidayList)
                    {
                        if (Holiday.HtmlColor != null && Holiday.HtmlColor != string.Empty)
                        {
                            var labelForCompanyHoliday = new LabelHelper
                            {
                                Id = Holiday.IdLookupValue,
                                Color = new BrushConverter().ConvertFromString(Holiday.HtmlColor.ToString()) as SolidColorBrush,
                                Caption = Holiday.Value
                            };
                            labelItems.AddRangeWithTemporarySuppressedNotification(labelForCompanyHoliday);
                        }
                    }
                    
                    //LoadFullYearEmployeeAttendanceAsync(
                    //    HrmService, employeeAttendanceList,
                    //    GridControl1);
                }
                else
                {
                    if (employeesList != null) employeesList.ClearWithTemporarySuppressedNotification();
                    //if (employeeAttendanceList != null) employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                    if (companyHolidays != null) companyHolidays.ClearWithTemporarySuppressedNotification();
                    if (holidayList != null) holidayList.ClearWithTemporarySuppressedNotification();
                    if (employeeLeavesList != null) employeeLeavesList.ClearWithTemporarySuppressedNotification();
                    if (labelItems != null) labelItems.ClearWithTemporarySuppressedNotification();
                    if (appointmentItems != null) appointmentItems.ClearWithTemporarySuppressedNotification();
                    if (department != null) department.ClearWithTemporarySuppressedNotification();
                }

                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Calculate From Date To Date At Selected Period For one month only
        /// </summary> 
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="selectedPeriodYear">default value is HrmCommon.Instance.SelectedPeriod</param>
        /// <param name="selectedPeriodMonth">default value is current month</param>
        public static void CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out DateTime? fromDate, out DateTime? toDate,
             int? selectedPeriodYear, int? selectedPeriodMonth)
        {
            const string methodNameWithBrackets = nameof(CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                //if (fromDate == null || toDate == null)
                //{
                    selectedPeriodYear = (selectedPeriodYear == null) ? (int)HrmCommon.Instance.SelectedPeriod : selectedPeriodYear;
                    selectedPeriodMonth = (selectedPeriodMonth == null) ? DateTime.Now.Month : selectedPeriodMonth;

                    var firstDateIntheCurrentMonth = new DateTime((int)selectedPeriodYear, (int)selectedPeriodMonth, 1);
                    var daysInMonth = DateTime.DaysInMonth(firstDateIntheCurrentMonth.Year, firstDateIntheCurrentMonth.Month);
                    var lastDateIntheCurrentMonth = new DateTime(firstDateIntheCurrentMonth.Year, firstDateIntheCurrentMonth.Month, daysInMonth, 23, 59, 59);

                fromDate = firstDateIntheCurrentMonth;
                    toDate = lastDateIntheCurrentMonth;

                    GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                    return;
                //}
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            fromDate = null;
            toDate = null;
        }

        /// <summary>
        /// Calculate From Date To Date At Selected Period For Full Year
        /// </summary> 
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="selectedPeriodYear">default value is HrmCommon.Instance.SelectedPeriod</param>
        public static void CalculateFromDateToDateAtSelectedPeriodForFullYear(
            out DateTime? fromDate, out DateTime? toDate,
             int? selectedPeriodYear)
        {
            const string methodNameWithBrackets = nameof(CalculateFromDateToDateAtSelectedPeriodForFullYear) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                //if (fromDate == null || toDate == null)
                //{
                    selectedPeriodYear = (selectedPeriodYear == null) ? (int)HrmCommon.Instance.SelectedPeriod : selectedPeriodYear;
                    var firstDateIntheFirstMonthOfYear = new DateTime((int)selectedPeriodYear, 1, 1);
                var lastDateIntheLastMonthOfYear = new DateTime(firstDateIntheFirstMonthOfYear.Year, 12, 31, 23, 59, 59);
                    fromDate = firstDateIntheFirstMonthOfYear;
                    toDate = lastDateIntheLastMonthOfYear;
                //}
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                return;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            fromDate = null;
            toDate = null;
        }

        public static void ShowPleaseWaitScreen()
        {
            if (!DXSplashScreen.IsActive)
            {
                Mouse.OverrideCursor = Cursors.Wait;
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
        }

        public static void ClosePleaseWaitScreen()
        {
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

    }
     
    public class CustomObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;
        
        public void AddRangeWithTemporarySuppressedNotification(T newItem)
        {
            this._suppressNotification = true;
            this.Add(newItem);
            this._suppressNotification = false;
        }

        public void AddRangeWithTemporarySuppressedNotification(IEnumerable<T> newItems)
        {
            this._suppressNotification = true;
            this.AddRange(newItems);
            this._suppressNotification = false;
        }

        public void ClearWithTemporarySuppressedNotification()
        {
            this._suppressNotification = true;
            this.Clear();
            this._suppressNotification = false;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }
    }

}
