using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.XtraScheduler;

namespace Emdep.Geos.Modules.Hrm.CommonClass
{
    /// <summary>
    /// [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
    /// </summary>
    public static class EmployeeMonthlyData
    {
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
        public static void GetEmployeeAttendanceFromService_2640(int idEmployee, ref IHrmService HrmService, 
            ref DateTime? fromDate, ref DateTime? toDate, ref CustomObservableCollection<EmployeeAttendance> employeeAttendanceList)
        {

            try
            {
       
                GeosApplication.Instance.Logger.Log($"Method GetHRMDataOnceFromServiceForAttendance_2640()...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {                    
                    employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();
                   
                    var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                    LoadFullYearEmployeeAttendanceAsync_V2640(idEmployee, HrmService, fromDate, toDate, employeeAttendanceList);
                }
                else
                {                 
                    if (employeeAttendanceList != null) employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                }

                GeosApplication.Instance.Logger.Log($"Method GetHRMDataOnceFromServiceForAttendance_2640()...executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method GetHRMDataOnceFromServiceForAttendance_2640() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method GetHRMDataOnceFromServiceForAttendance_2640() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method GetHRMDataOnceFromServiceForAttendance_2640()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void GetHRMEmployeesDataOnceFromServiceForAttendance_2640(
               ref IHrmService HrmService, ref ICrmService CrmStartUp,
               ref CustomObservableCollection<CompanyHoliday> companyHolidays,
               ref CustomObservableCollection<LookupValue> holidayList,
               ref CustomObservableCollection<EmployeeLeave> employeeLeavesList,
               ref CustomObservableCollection<LabelHelper> labelItems,
               ref CustomObservableCollection<StatusHelper> statusItems,
               ref CustomObservableCollection<UI.Helper.Appointment> appointmentItems,
               ref DateTime? fromDate, ref DateTime? toDate, GridControl GridControl1,
               ref CustomObservableCollection<CompanyShift> CompanyShiftsList, 
               ref CustomObservableCollection<Employee> employeesList, ref CustomObservableCollection<Department> department,
               ref CustomObservableCollection<Employee> employeeListFinalForLeaves)
        {

            try
            {
                GeosApplication.Instance.Logger.Log($"Method GetHRMDataOnceFromServiceForAttendance_2640()...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    employeeListFinalForLeaves = new CustomObservableCollection<Employee>(); //chitra.girigosavi GEOS2-7107 23/06/2025
                    employeesList = new CustomObservableCollection<Employee>();
                    employeeLeavesList = new CustomObservableCollection<EmployeeLeave>();
                    companyHolidays = new CustomObservableCollection<CompanyHoliday>();
                    holidayList = new CustomObservableCollection<LookupValue>();
                    labelItems = new CustomObservableCollection<LabelHelper>();
                    statusItems = new CustomObservableCollection<StatusHelper>();
                    CompanyShiftsList = new CustomObservableCollection<CompanyShift>();
                    appointmentItems = new CustomObservableCollection<UI.Helper.Appointment>();

                    var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                    department = new CustomObservableCollection<Department>();

                    department.AddRangeWithTemporarySuppressedNotification(
                       HrmService.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2630(
                       plantOwnersIds, GeosApplication.Instance.SelectedHRMAttendanceDate.Year,
                       HrmCommon.Instance.ActiveEmployee.Organization,
                       HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                       HrmCommon.Instance.IdUserPermission));

                    //CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out fromDate, out toDate, null, null);
                    employeesList = new CustomObservableCollection<Employee>();
              
                    employeesList.AddRangeWithTemporarySuppressedNotification(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2520(
                        plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.IdUserPermission));

                    companyHolidays.AddRangeWithTemporarySuppressedNotification(HrmService.GetCompanyHolidaysBySelectedIdCompany_V2530(plantOwnersIds, GeosApplication.Instance.SelectedHRMAttendanceDate.Year));
                    holidayList.AddRangeWithTemporarySuppressedNotification(CrmStartUp.GetLookupValues(28).AsEnumerable());
                    employeeLeavesList.AddRangeWithTemporarySuppressedNotification(HrmService.GetEmployeeLeavesBySelectedIdCompany_V2520(
                        plantOwnersIds, GeosApplication.Instance.SelectedHRMAttendanceDate.Year, 
                        HrmCommon.Instance.ActiveEmployee.Organization, 
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, 
                        HrmCommon.Instance.IdUserPermission));

                    #region chitra.girigosavi GEOS2-7107 23/06/2025
                    employeeListFinalForLeaves = new CustomObservableCollection<Employee>();
                    employeeListFinalForLeaves.AddRangeWithTemporarySuppressedNotification(
                        HrmService.GetAllEmployeesForLeaveByIdCompany_V2420(
                        plantOwnersIds, HrmCommon.Instance.SelectedPeriod,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.IdUserPermission));
                    #endregion

                    ViewModels.EmployeeAttendanceViewModel.FillEmployeeWorkType();
                    ViewModels.EmployeeAttendanceViewModel.FillEmployeeLeaveType();

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
                }
                else
                {
                    if (employeeListFinalForLeaves != null) employeeListFinalForLeaves.ClearWithTemporarySuppressedNotification();//chitra.girigosavi GEOS2-7107 23/06/2025
                    if (companyHolidays != null) companyHolidays.ClearWithTemporarySuppressedNotification();
                    if (holidayList != null) holidayList.ClearWithTemporarySuppressedNotification();
                    if (employeeLeavesList != null) employeeLeavesList.ClearWithTemporarySuppressedNotification();
                    if (labelItems != null) labelItems.ClearWithTemporarySuppressedNotification();
                    if (statusItems != null) statusItems.ClearWithTemporarySuppressedNotification();
                    if (appointmentItems != null) appointmentItems.ClearWithTemporarySuppressedNotification();
                }

                GeosApplication.Instance.Logger.Log($"Method GetHRMDataOnceFromServiceForAttendance_2640()...executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method GetHRMDataOnceFromServiceForAttendance_2640() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method GetHRMDataOnceFromServiceForAttendance_2640() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method GetHRMDataOnceFromServiceForAttendance_2640()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }


        public static async void LoadFullYearEmployeeAttendanceAsync_V2640(int idEmployee, IHrmService HrmService, DateTime? StartDate, DateTime? EndDate,
            CustomObservableCollection<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log($"Method LoadFullYearEmployeeAttendanceAsync_V2640...", category: Category.Info, priority: Priority.Low);
                List<EmployeeAttendance> newEmployeeAttendance = new List<EmployeeAttendance>();
             
                var fetchedList = await ReturnFullYearEmployeeAttendanceList_V2640(idEmployee,HrmService, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));

                if (employeeAttendanceList == null)
                    employeeAttendanceList = new CustomObservableCollection<EmployeeAttendance>();

                employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(fetchedList);
                try
                {
                    if (employeeAttendanceList != null && employeeAttendanceList.Count > 0)
                    {
                        var distinctList = employeeAttendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(g => g.First()).ToList();
                        employeeAttendanceList.ClearWithTemporarySuppressedNotification();
                        employeeAttendanceList.AddRangeWithTemporarySuppressedNotification(distinctList);
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log($"Get an error in Method LoadFullYearEmployeeAttendanceAsync_V2640...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                }
         
                GeosApplication.Instance.Logger.Log($"Method LoadFullYearEmployeeAttendanceAsync_V2640....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {              
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method LoadFullYearEmployeeAttendanceAsync_V2640 Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {             
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method LoadFullYearEmployeeAttendanceAsync_V2640 Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {             
                GeosApplication.Instance.Logger.Log($"Get an error in Method LoadFullYearEmployeeAttendanceAsync_V2640...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private static async Task<List<EmployeeAttendance>> ReturnFullYearEmployeeAttendanceList_V2640(int idemployee, IHrmService HrmService, DateTime fromDate, DateTime toDate)
        {
            var plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
            var attendanceList = HrmService.GetSelectedEmployeeAttendance_V2640(idemployee, plantOwnersIds,
                GeosApplication.Instance.SelectedHRMAttendanceDate.Year,
                HrmCommon.Instance.ActiveEmployee.Organization,
                HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                HrmCommon.Instance.IdUserPermission,
                fromDate,
                toDate);

            var distinctList = attendanceList.GroupBy(e => e.IdEmployeeAttendance).Select(g => g.First()).ToList();
            return await Task.FromResult(distinctList);
        }


        public static void CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly(out DateTime? fromDate, out DateTime? toDate,     
            int? selectedPeriodYear, int? selectedPeriodMonth)
        {
            const string methodNameWithBrackets = nameof(CalculateFromDateToDateAtSelectedPeriodForOneMonthOnly) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
             
                selectedPeriodYear = (selectedPeriodYear == null) ? (int)GeosApplication.Instance.SelectedHRMAttendanceDate.Year : selectedPeriodYear;
                selectedPeriodMonth = (selectedPeriodMonth == null) ? DateTime.Now.Month : selectedPeriodMonth;

                var firstDateIntheCurrentMonth = new DateTime((int)selectedPeriodYear, (int)selectedPeriodMonth, 1);
                var daysInMonth = DateTime.DaysInMonth(firstDateIntheCurrentMonth.Year, firstDateIntheCurrentMonth.Month);
                var lastDateIntheCurrentMonth = new DateTime(firstDateIntheCurrentMonth.Year, firstDateIntheCurrentMonth.Month, daysInMonth, 23, 59, 59);

                fromDate = firstDateIntheCurrentMonth;
                toDate = lastDateIntheCurrentMonth;

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
    }
}
