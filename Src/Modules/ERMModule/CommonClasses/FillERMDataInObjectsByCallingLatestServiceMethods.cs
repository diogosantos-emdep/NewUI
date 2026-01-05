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

using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Modules.ERM.ViewModels;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Modules.ERM
{
    
    public static class FillERMDataInObjectsByCallingLatestServiceMethods
    {
                  

        public static void GetERMDataOnceFromServiceForAttendance(
            ref IERMService ERMService,
            ref ICrmService CrmStartUp,
            ref CustomObservableCollection<Holidays> companyHolidays,
            //ref CustomObservableCollection<ProductionPlanningReview> productionPlanningReviewList,
            ref CustomObservableCollection<LookupValue> holidayList,
            ref CustomObservableCollection<LabelHelper> labelItems,
            ref List<GeosAppSetting> pendingPOColorList,
            ref CustomObservableCollection<UI.Helper.PlanningAppointment> appointmentItems,
            

            ref string FromDate,
            ref string ToDate,
              ref CustomObservableCollection<UI.Helper.PlanningAppointment> tempAppointmentItems
            )
        {
            
            

            const string methodNameWithBrackets = nameof(GetERMDataOnceFromServiceForAttendance) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                

                    companyHolidays = new CustomObservableCollection<Holidays>();
                   // productionPlanningReviewList= new CustomObservableCollection<ProductionPlanningReview>();
                  //  holidayList = new CustomObservableCollection<LookupValue>();
                    labelItems = new CustomObservableCollection<LabelHelper>();
              
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                  //  List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            //DateTime tempToyear = DateTime.Parse(ToDate.ToString());
                            //string TOyear = Convert.ToString(tempToyear.Year);
                            DateTime tempFromyear = DateTime.Parse(FromDate.ToString());
                            string year = Convert.ToString(tempFromyear.Year);
                            string PlantName = Convert.ToString(itemPlantOwnerUsers.IdSite);
                        //    string PlantName = Convert.ToString(itemPlantOwnerUsers.Alias);
                          //  ERMService = new ERMServiceController("localhost:6699");
                            companyHolidays.AddRangeWithTemporarySuppressedNotification(ERMService.GetCompanyHolidaysBySelectedIdCompany(PlantName, year
                           ));

                        }
                        catch (Exception ex) { }
                    }
                }

                if (appointmentItems == null)
                {
                    appointmentItems = new CustomObservableCollection<UI.Helper.PlanningAppointment>();
                }
                    foreach (Holidays CompanyHoliday in companyHolidays)
                    {
                    var modelAppointment = new UI.Helper.PlanningAppointment
                    {

                        Subject = CompanyHoliday.Name,
                        StartDate = CompanyHoliday.StartDate,
                        EndDate = CompanyHoliday.EndDate,
                        Label = CompanyHoliday.IdHoliday,
                        ContentSubject = CompanyHoliday.Name,
                        IsHolidayData = Visibility.Visible,
                        IsHolidayDate = Visibility.Collapsed,
                        IsPlannedDeliveryDate = true


                    };
                        if (CompanyHoliday.IsRecursive == 1)
                        {
                            modelAppointment.Types = (int)DevExpress.XtraScheduler.AppointmentType.Pattern;
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
                                //modelAppointment.RecurrenceInfo = recurrenceInfo.ToXml();
                            }
                        }
                if (CompanyHoliday != null)
                {
                      
                        
                }
                //modelAppointment.IsHolidayData = Visibility.Collapsed;
                appointmentItems.AddRangeWithTemporarySuppressedNotification(modelAppointment);
                    //tempAppointmentItems.AddRangeWithTemporarySuppressedNotification(modelAppointment);
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

               
                foreach (var item in pendingPOColorList)
                {
                    var labelForCompanyHoliday = new LabelHelper
                    {
                        Id = item.IdAppSetting,
                        Color = new BrushConverter().ConvertFromString(item.DefaultValue != null ? item.DefaultValue : "#FFFFFF") as SolidColorBrush
                    };

                    labelItems.AddRangeWithTemporarySuppressedNotification(labelForCompanyHoliday);

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
